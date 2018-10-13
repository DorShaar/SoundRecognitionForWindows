using Accord.Math;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;

namespace SoundRecognition
{
     internal class PopsRecognizer : IRecognizerMachine
     {
          private static readonly int MS_IN_ONE_SECOND = 1000;
          private readonly int MAXIMAL_POP_INTERVAL_ALLOWED_IN_MS = 4 * MS_IN_ONE_SECOND;
          private readonly string RECORDS_DIRECTORY_NAME = "RecordsData";

          private readonly string mRecordsDataDirectoryPath;
          private bool mShouldStop = false;
          private Recorder mRecorder;
          private Stopwatch mStopwatch = new Stopwatch();
          private readonly Logger mLogger;

          private int mTimeoutCount = 1; // TODO ask Tomer what is that for?
          private int mSampleCount = 0;
          private List<double> mEnergyForBlocksList = new List<double>();
          private RecordInfoDescriptor mRecordInfoDescriptor = new RecordInfoDescriptor();
          private KnnTester mKnnTester;

          private AutoResetEvent mRecognizerFinishedEvent = new AutoResetEvent(false);
          public event EventHandler<RecognizerFinishedEventArgs> RecognizerFinished;

          private enum ePopState
          {
               Unkown,
               BeforePeak,
               AfterPeak,
          }

          public PopsRecognizer(string workingDirectory)
          {
               mLogger = new Logger(nameof(PopsRecognizer), ConsoleColor.Green);
               mRecordsDataDirectoryPath = Path.Combine(workingDirectory, RECORDS_DIRECTORY_NAME);
          }

          public void LoadProcessedData(string recognizerType, string itemCategory)
          {
               // TODO TOMER help how to load the xmls?
               //mKnnTester = new KnnTester(<list>);
          }

          public void ProcessNewData(IItemInfo itemInfo)
          {
               mLogger.WriteLine("Start processing new data");
               using (RegularRecordStrategy recordStrategy = new RegularRecordStrategy())
               using (mRecorder = new Recorder(mRecordsDataDirectoryPath, recordStrategy))
               {
                    // Sets the recorder to notify ProcessLatestData when it has new data to process.
                    recordStrategy.NotifyWithNewData += ProcessLatestData;

                    mStopwatch.Start();

                    // Start listening and recording.
                    mRecorder.Record();

                    // Letting the recorder sending new data untill recognizer is signled to stop.
                    while (!mShouldStop) { };
                    mStopwatch.Stop();
                    mRecordInfoDescriptor.Save(mRecordsDataDirectoryPath);
                    mLogger.WriteLine($"Record data saved");
               }

               mLogger.WriteLine("Stopped processing new data");
               mRecognizerFinishedEvent.Set();
          }

          public void Stop()
          {
               mShouldStop = true;

               // Waiting for ProcessNewData to finish.
               mRecognizerFinishedEvent.WaitOne();
               mLogger.WriteLine($"{nameof(PopsRecognizer)} stopped");

               // Notifying that the recognizer finished working.
               RecognizerFinished.Invoke(
                    this,
                    new RecognizerFinishedEventArgs());
          }

          private void ProcessLatestData(Object sender, RecorderUpdateEventArgs e) // ORG: PlotLatestData()
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

               // Keeps the real half (the other half imaginary)
               Array.Copy(fft, fftReal, fftReal.Length);

               if (Recognize(fftReal) == eRecognitionStatus.Recognized)
               {
                    mLogger.WriteLine($"Pop detected after: {mStopwatch.ElapsedMilliseconds}");
               }
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

               if (mEnergyForBlocksList.Count > 30) // TODO remove back to 1000
               {
                    double avgEnergy = mEnergyForBlocksList.Sum() / mEnergyForBlocksList.Count;
                    double varianceEnergy = CalculateVariance(avgEnergy);
                    double factorC = -0.0000015 * varianceEnergy + 1.5142857;

                    // In that case, pop is recognized.
                    if (energySum > factorC * avgEnergy)
                    {
                         if (mSampleCount % 5 == 0) // TODO consider cause with it recognizer much less.
                         {
                              mRecordInfoDescriptor.AddRecognitionTime(mStopwatch.ElapsedMilliseconds / 1000.0);
                              recognitionStatus = eRecognitionStatus.Recognized;
                         }

                         if (mSampleCount % 10 == 0)
                         {
                              // TODO send to knn.
                         }
                    }

                    mSampleCount++;
               }

               // TODO ASK TOMER what is that block doing?
               mEnergyForBlocksList.Add(energySum);
               if (mStopwatch.IsRunning && (mStopwatch.ElapsedMilliseconds / 2000.0 - 2 * mTimeoutCount) > 0)
               {
                    mTimeoutCount++;
                    mRecordInfoDescriptor.Duration = mStopwatch.ElapsedMilliseconds / 1000.0;
                    //TODO: here we can try classify using knn
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

          private double CalculateVariance(double avgEnergy)
          {
               double varianceEnergy = 0;

               foreach (double energy in mEnergyForBlocksList)
               {
                    varianceEnergy += Math.Pow((avgEnergy - energy), 2);
               }

               varianceEnergy /= mEnergyForBlocksList.Count;
               return varianceEnergy;
          }
     }
}
