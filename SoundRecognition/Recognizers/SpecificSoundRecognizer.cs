﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace SoundRecognition
{
     internal class SpecificSoundRecognizer : IRecognizerMachine
     {
          private readonly string RECORDS_DIRECTORY_NAME = "SubRecords";

          private readonly int mAmplification = 10;
          private readonly double mSecondsToAnalyzeAudioFiles = 10;
          private readonly string mRecordsDirectory;

          private bool mIsStopped = false;
          private bool mShouldStop = false;
          private Recorder mRecorder;
          private Queue<IAudioFile> mSubSoundsQueue = new Queue<IAudioFile>();
          private SoundFingerprintingWrapper mSoundFingerprintingUtility;
          private readonly Logger mLogger;

          private AutoResetEvent mRecognizerFinishedEvent = new AutoResetEvent(true);

          public string RecognitionStatus { get; private set; }

          public event EventHandler<RecognizerFinishedEventArgs> RecognizerFinished;

          public SpecificSoundRecognizer(string workingDirectory, int amplification, int SecondsToAnalyzeAudioFiles)
          {
               mRecordsDirectory = Path.Combine(workingDirectory, RECORDS_DIRECTORY_NAME);
               mLogger = new Logger(workingDirectory, nameof(SpecificSoundRecognizer), ConsoleColor.Green);
               mAmplification = amplification;
               mSecondsToAnalyzeAudioFiles = SecondsToAnalyzeAudioFiles;
               mSoundFingerprintingUtility = new SoundFingerprintingWrapper(mRecordsDirectory);
          }

          public void LoadProcessedData(string recognizerType, string itemCategory)
          {
               mSoundFingerprintingUtility.DatabaseCategory = itemCategory;
               mSoundFingerprintingUtility.RecognizerType = recognizerType;
               mSoundFingerprintingUtility.LoadWavFilesDatabase();
          }

          public void ProcessNewData(IItemInfo item)
          {
               mRecognizerFinishedEvent.Reset();
               using (IntervalsRecorderStrategy intervalsRecorder = new IntervalsRecorderStrategy())
               using (mRecorder = new Recorder(mRecordsDirectory, intervalsRecorder))
               using (FileSystemWatcher fileSystemWatcher = new FileSystemWatcher())
               {
                    fileSystemWatcher.Path = mRecordsDirectory;
                    fileSystemWatcher.EnableRaisingEvents = true;

                    /// Listening to records directory. In case new record created, insert
                    /// it into <see cref="mSubSoundsQueue"/>
                    fileSystemWatcher.Created += FileSystemWatcher_Created;
                    fileSystemWatcher.Renamed += FileSystemWatcher_Renamed;
                    fileSystemWatcher.Deleted += FileSystemWatcher_Deleted;
                    fileSystemWatcher.Changed += FileSystemWatcher_Changed;

                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    mRecorder.Record();

                    RunDetectionAlgorithm(stopwatch);
                    stopwatch.Stop();
               }

               mRecognizerFinishedEvent.Set();

               // Notifying that the recognizer finished working.
               RecognizerFinished.Invoke(
                    this,
                    new RecognizerFinishedEventArgs());
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
                    mLogger.WriteLine($"{nameof(SpecificSoundRecognizer)} stopped. Stop Reason: {stopReason}");
               }
          }

          private void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
          {
               mLogger.WriteLine($"A new file has been renamed from {e.OldName} to {e.Name}");
          }

          private void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
          {
               mLogger.WriteLine($"A new file has been deleted - {e.Name}");
          }

          private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
          {
               mLogger.WriteLine($"A new file has been changed - {e.Name}. Should have delete that file before!");
          }

          private void FileSystemWatcher_Created(object source, FileSystemEventArgs e)
          {
               mLogger.WriteLine($"New sub record detected: {e.Name}");
               FilePath audioFilePath = FilePath.CreateFilePath(mRecordsDirectory, e.Name);
               int timeToWaitMS = 1000;
               while (FilesOperations.IsFileLocked(new FileInfo(audioFilePath.FileFullPath)))
               {
                    mLogger.WriteLine($"{audioFilePath.FileFullPath} is locked. Waiting for {timeToWaitMS} ms");
                    Thread.Sleep(timeToWaitMS);
               }

               mSubSoundsQueue.Enqueue(new WavFile(e.FullPath));
          }

          // Should not call Stop(), otherwise we need to figure out how to manage AutoResEvent flags.
          private void RunDetectionAlgorithm(Stopwatch stopwatch)
          {
               while (!mShouldStop)
               {
                    if (Recognize() == eRecognitionStatus.Recognized)
                    {
                         RecognitionStatus = "Algorithm should stop since identified suitable sound"; 
                         mShouldStop = true;
                         mLogger.WriteLine(RecognitionStatus);
                    }
               }
          }

          private eRecognitionStatus Recognize()
          {
               eRecognitionStatus recognitionStatus = eRecognitionStatus.UnRecognized;
               if (mSubSoundsQueue.Count != 0)
               {
                    IAudioFile subSound = mSubSoundsQueue.Dequeue();
                    bool isMatchFound = mSoundFingerprintingUtility.IsAudioFileDetected(
                       subSound,
                       mAmplification,
                       mSecondsToAnalyzeAudioFiles);

                    if (isMatchFound)
                    {
                         recognitionStatus = eRecognitionStatus.Recognized;
                    }
               }

               return recognitionStatus;
          }
     }
}
