﻿using Accord.Math;
using KNN;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;

namespace SoundRecognition
{
     /// <summary>
     /// Uses Knn to identify popcorn readiness.
     /// </summary>
     internal class PopsRecognizer : IRecognizerMachine
     {
          // Constants.
          private static readonly int MS_IN_ONE_SECOND = 1000;
          private readonly int MAXIMAL_POP_INTERVAL_ALLOWED_IN_MS = 4 * MS_IN_ONE_SECOND;
          private readonly string RECORDS_DIRECTORY_NAME = "RecordsData";
          private readonly int mSampleRate = 44100;

          // Knn parameters.
          private readonly int KNN_PARAMETER = 5; // The k parameter for the KNN test.
          private readonly string classificationA = "popcornDone";
          private readonly string classificationB = "underCooked";
          private KnnTester mKnnTester;

          private readonly string mRecordsDataDirectoryPath;
          private bool mIsStopped = false;
          private bool mShouldStop = false;
          private Recorder mRecorder;
          private Stopwatch mStopwatch = new Stopwatch();
          private readonly Logger mLogger;

          private int mIntervalsInSeconds = 4;
          private int mSampleCount = 0;
          private List<double> mEnergyForBlocksList = new List<double>();
          private RecordInfoDescriptor mRecordInfoDescriptor = new RecordInfoDescriptor();

          public string RecognitionStatus { get; private set; } = "Not recognized yet";

          private AutoResetEvent mRecognizerFinishedEvent = new AutoResetEvent(true);
          public event SendProcessLatestData OnSendProcessLatestData;
          public event EventHandler<RecognizerFinishedEventArgs> RecognizerFinished;

          public PopsRecognizer(string workingDirectory)
          {
               mLogger = new Logger(nameof(PopsRecognizer), ConsoleColor.Green);
               mRecordsDataDirectoryPath = Path.Combine(workingDirectory, RECORDS_DIRECTORY_NAME);
          }

          public void LoadProcessedData(string recognizerType, string itemCategory)
          {
               mKnnTester = new KnnTester(
                    mRecordsDataDirectoryPath,
                    recognizerType,
                    itemCategory,
                    classificationA,
                    classificationB);
          }

          public void ProcessNewData(IItemInfo itemInfo)
          {
               mRecognizerFinishedEvent.Reset();
               mLogger.WriteLine("Start processing new data");
               using (RegularRecordStrategy recordStrategy = new RegularRecordStrategy())
               using (mRecorder = new Recorder(mRecordsDataDirectoryPath, recordStrategy))
               {
                    // Sets the recorder to notify ProcessLatestData when it has new data to process.
                    recordStrategy.NotifyWithNewData += ProcessLatestData;

                    mStopwatch.Start();

                    // Start listening and recording.
                    mRecorder.Record();

                    // Letting the recorder sending new data until recognizer is signled to stop.
                    while (!mShouldStop) { };
                    mStopwatch.Stop();
               }

               mLogger.WriteLine("Stopped processing new data");
               mRecognizerFinishedEvent.Set();
          }

          public void Stop(string stopReason)
          {
               RecognitionStatus = stopReason;
               if (!mIsStopped)
               {
                    mShouldStop = true;

                    // Waiting for ProcessNewData to finish.
                    mRecognizerFinishedEvent.WaitOne();
                    mIsStopped = true;
                    mLogger.WriteLine($"{nameof(PopsRecognizer)} stopped. Stop Reason: {stopReason}");

                    // Notifying that the recognizer finished working.
                    RecognizerFinished.Invoke(
                         this,
                         new RecognizerFinishedEventArgs());
               }
          }

          private void ProcessLatestData(Object sender, RecorderUpdateEventArgs e)
          {
               if (e.AudioBytes.Length == 0)
                    return;
               if (e.AudioBytes[(mRecorder.RecordStrategy as RegularRecordStrategy).BufferSize - 2] == 0)
                    return;

               // Incoming data is 16-bit (2 bytes per audio point).
               int bytesPerPoint = 2;

               // Creates (32-bit) int array ready to fill with the 16-bit data.
               int graphPointCount = e.AudioBytes.Length / bytesPerPoint;

               // Create double arrays to hold the data we will graph.
               double[] pcm = new double[graphPointCount];
               double[] fft = new double[graphPointCount];
               double[] fftReal = new double[graphPointCount / 2];

               // Populate Xs and Ys with double data.
               for (int i = 0; i < graphPointCount; ++i)
               {
                    // Reads the int16 from the two bytes.
                    Int16 val = BitConverter.ToInt16(e.AudioBytes, i * 2);

                    // Stores the value in Ys as a percent (+/- 100% = 200%).
                    pcm[i] = (double)(val) / Math.Pow(2, 16) * 200.0;
               }

               // Calculate the full FFT.
               fft = FFT(pcm);

               // Determine horizontal axis units for graphs.
               double pcmPointSpacingMs = mSampleRate / 1000;
               double fftMaxFreq = mSampleRate / 2;
               double fftPointSpacingHz = fftMaxFreq / graphPointCount;

               // Keeps the real half (the other half imaginary)
               Array.Copy(fft, fftReal, fftReal.Length);

               System.Drawing.Color graphColor = System.Drawing.Color.CadetBlue;
               if (Recognize(fftReal) == eRecognitionStatus.Recognized)
               {
                    mLogger.WriteLine($"Pop detected after: {mStopwatch.ElapsedMilliseconds}");
                    graphColor = System.Drawing.Color.Green;
               }

               // Sends data to draw on UI.
               OnSendProcessLatestData.Invoke(new SoundVisualizationDataPackage(
                    pcm, pcmPointSpacingMs, fftReal, fftPointSpacingHz, graphColor));
          }

          private double[] FFT(double[] data)
          {
               double[] fft = new double[data.Length];
               Complex[] fftComplex = new Complex[data.Length];
               for (int i = 0; i < data.Length; i++)
                    fftComplex[i] = new Complex(data[i], 0.0);

               FourierTransform.FFT(fftComplex, FourierTransform.Direction.Forward);
               for (int i = 0; i < data.Length; i++)
                    fft[i] = fftComplex[i].Magnitude;

               return fft;
          }

          /// <summary>
          /// Based on article: http://mziccard.me/2015/05/28/beats-detection-algorithms-1/
          /// </summary>
          /// <param name="values"></param>
          private eRecognitionStatus Recognize(double[] values) // ORG: PlotSignal()
          {
               eRecognitionStatus recognitionStatus = eRecognitionStatus.UnRecognized;
               double energySum = CalculateEnergy(values);

               // We let the blocks list number grow to collect better statistical information.
               if (mEnergyForBlocksList.Count > 900)
               {
                    double avgEnergy = mEnergyForBlocksList.Sum() / mEnergyForBlocksList.Count;
                    double varianceEnergy = CalculateVariance(avgEnergy);
                    //double factorC = -0.0025714 * varianceEnergy + 1.5142857; // this is the factorC from the original artical (not the same one as in the article above)
                    double factorC = -0.0000015 * varianceEnergy + 1.5142857; // this is the factorC we were using with the earlier records

                    // In that case, pop is recognized.
                    if (energySum > factorC * avgEnergy)
                    {
                         if (mSampleCount % 5 == 0)
                         {
                              mRecordInfoDescriptor.AddRecognitionTime(mStopwatch.ElapsedMilliseconds / 1000.0);
                              recognitionStatus = eRecognitionStatus.Recognized;
                         }
                    }

                    mSampleCount++;
               }

               //new improvement:
               // Adds use only the first 2000 blocks, in order to reduce the influence of the many pops at the peak,
               // because when getting around 3000-3500 blocks, a small pop is to close to the avarage. 
               if (mEnergyForBlocksList.Count < 2000)
                    mEnergyForBlocksList.Add(energySum);

               /// Every <see cref="mIntervalsInSeconds"/> seconds sending test object to the KNN tester.
               if (mStopwatch.IsRunning && (mStopwatch.ElapsedMilliseconds / 1000.0) > 4 * mIntervalsInSeconds)
               {
                    mIntervalsInSeconds++;
                    mRecordInfoDescriptor.UpdateDurationAndLastSection(mStopwatch.ElapsedMilliseconds / 1000.0);

                    RecordNeighbor testObject = mKnnTester.GenerateNeighborFromRecordInfoDescriptor(mRecordInfoDescriptor);
                    if (mKnnTester.TestAndClassify(testObject, KNN_PARAMETER) == classificationA)
                         Stop("Recognizer detected that the popcorn is ready");
               }

               return recognitionStatus;
          }

          private double CalculateEnergy(double[] values)
          {
               double totalEnergy = 0;
               foreach (double val in values)
               {
                    totalEnergy += Math.Pow(val, 2);
               }

               return totalEnergy;
          }

          /// <summary>
          /// Working on copied list to avoid reading and calculating while other thread is adding values.
          /// </summary>
          /// <param name="avgEnergy"></param>
          /// <returns></returns>
          private double CalculateVariance(double avgEnergy)
          {
               double varianceEnergy = 0;

               List<double> energyCopyList = new List<double>(mEnergyForBlocksList);

               foreach (double energy in energyCopyList)
               {
                    varianceEnergy += Math.Pow((avgEnergy - energy), 2);
               }

               varianceEnergy /= energyCopyList.Count;

               return varianceEnergy;
          }
     }
}
