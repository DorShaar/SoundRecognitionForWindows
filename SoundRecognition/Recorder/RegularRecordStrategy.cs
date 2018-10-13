using System;
using System.Threading;
using NAudio.Wave;

namespace SoundRecognition
{
     class RegularRecordStrategy : IRecordStrategy, IDisposable
     {
          private bool mIsDisposing = false;
          private WaveInEvent mWaveIn;
          private BufferedWaveProvider mBufferedWaveProvider;
          private Timer mTimer;

          public int BufferSize { get; private set; } = (int)Math.Pow(2, 11); // Must be a multiple of 2.
          public int SampleRate { get; private set; } = 44100;
          public string RecordsDirectory { get; set; }

          public event EventHandler<RecorderUpdateEventArgs> NotifyWithNewData;

          public void StartListeningToMicrophone(int audioDeviceNumber = 0)
          {
               mWaveIn = new WaveInEvent
               {
                    DeviceNumber = audioDeviceNumber,
                    WaveFormat = new WaveFormat(SampleRate, 1),
                    BufferMilliseconds = (int)((double)BufferSize / (double)SampleRate * 1000.0)
               };

               mWaveIn.DataAvailable += new EventHandler<WaveInEventArgs>(On_AudioDataAvailable);
               mBufferedWaveProvider = new BufferedWaveProvider(mWaveIn.WaveFormat)
               {
                    BufferLength = BufferSize * 2,
                    DiscardOnBufferOverflow = true
               };

               mWaveIn.StartRecording();
          }

          public void DoOperationWhileRecording()
          {
               mTimer = new Timer(SendLatestAudioBytes, null, 0, 2);
          }

          public void Dispose()
          {
               mIsDisposing = true;
               mTimer.Dispose();
               mWaveIn.StopRecording();
               mWaveIn.Dispose();
          }

          private void SendLatestAudioBytes(object state)
          {
               if(!mIsDisposing)
               {
                    byte[] audioBytes = new byte[BufferSize];
                    mBufferedWaveProvider.Read(audioBytes, 0, BufferSize);
                    NotifyWithNewData.Invoke(this, new RecorderUpdateEventArgs(string.Empty, audioBytes));
               }
          }

          private void On_AudioDataAvailable(object sender, WaveInEventArgs e)
          {
               mBufferedWaveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
          }

          // TODO Use IN CASE OF WIN FORMS IMPLEMENTATION
          // Turns off the timer, take as long as we need to plot, then turn the timer back on.
          //private void Timer_Tick(object sender, EventArgs e)
          //{
          //     timerReplot.Enabled = false;
          //     PlotLatestData();
          //     timerReplot.Enabled = true;
          //}
     }
}
