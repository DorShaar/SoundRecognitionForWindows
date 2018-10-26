using System;
using System.IO;
using NAudio.Wave;

namespace SoundRecognition
{
     class IntervalsRecorderStrategy : IRecordStrategy
     {
          private readonly string SUB_RECORD_NAME = "sub_record_";
          private readonly string WAV_EXTENSION = ".wav";
          private readonly int mIntervalPeriodInSeconds = 7;

          private int mRecordCounter = 0;
          private string mCurrentWavFilePath = string.Empty;
          private WaveInEvent mWaveIn;
          private WaveFileWriter mWaveFileWriter;

          public readonly int BufferSize = (int)Math.Pow(2, 11); // Must be a multiple of 2.
          public readonly int SampleRate = 44100;
          public string RecordsDirectory { get ; set ; }

          public void StartListeningToMicrophone(int audioDeviceNumber = 0)
          {
               mCurrentWavFilePath = Path.Combine(RecordsDirectory, $"{SUB_RECORD_NAME}{mRecordCounter}{WAV_EXTENSION}");

               //mWaveIn = new WaveIn(WaveCallbackInfo.FunctionCallback()); // maybe should use that.
               mWaveIn = new WaveInEvent
               {
                    DeviceNumber = audioDeviceNumber,
                    WaveFormat = new WaveFormat(SampleRate, 1),
                    BufferMilliseconds = (int)((double)BufferSize / (double)SampleRate * 1000.0)
               };

               mWaveIn.DataAvailable += new EventHandler<WaveInEventArgs>(On_AudioDataAvailable);
               mWaveIn.StartRecording();

               mWaveFileWriter = new WaveFileWriter(mCurrentWavFilePath, mWaveIn.WaveFormat);
          }

          public void Dispose()
          {
               mWaveIn.DataAvailable -= On_AudioDataAvailable;
               mWaveIn.StopRecording();
               mWaveIn.Dispose();
               mWaveFileWriter.Close();
               mWaveFileWriter.Dispose();
          }

          private void On_AudioDataAvailable(object sender, WaveInEventArgs e)
          {
               mWaveFileWriter.Write(e.Buffer, 0, e.BytesRecorded);
               if (mWaveFileWriter.TotalTime.Seconds > mIntervalPeriodInSeconds)
               {
                    mWaveFileWriter.Close();
                    mWaveFileWriter.Dispose();
                    mRecordCounter++;
                    mCurrentWavFilePath = Path.Combine(RecordsDirectory, $"{SUB_RECORD_NAME}{mRecordCounter}{WAV_EXTENSION}");
                    mWaveFileWriter = new WaveFileWriter(mCurrentWavFilePath, mWaveIn.WaveFormat);
               }
          }
     }
}
