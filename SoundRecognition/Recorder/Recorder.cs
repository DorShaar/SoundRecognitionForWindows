using System;
using System.IO;

namespace SoundRecognition
{
    internal class Recorder : IRecorder
    {
        private Logger mLogger;
        public IRecordStrategy RecordStrategy;

        public Recorder(string recordsDirectory, IRecordStrategy recordStrategy)
        {
            mLogger = new Logger(nameof(Recorder), ConsoleColor.DarkMagenta);

            RecordStrategy = recordStrategy;
            RecordStrategy.RecordsDirectory = recordsDirectory;
            mLogger.WriteLine($"Creating directory: {recordsDirectory}");
            Directory.CreateDirectory(recordsDirectory);
        }

        public void Record()
        {
            try
            {
                RecordStrategy.StartListeningToMicrophone();
                mLogger.WriteLine("Recording...");
            }
            catch (Exception e)
            {
                mLogger.WriteError("Start recording failed", e);
            }
        }

        public void Dispose()
        {
            RecordStrategy.Dispose();
            mLogger.WriteLine($"Stopped recording");
            DeleteSubRecordings();
        }

        private void DeleteSubRecordings()
        {
            foreach (string subRecord in Directory.GetFiles(RecordStrategy.RecordsDirectory))
            {
                if (subRecord.EndsWith(WavFile.WavFileExtension))
                {
                    mLogger.WriteLine($"Going to delete {subRecord}");
                    File.Delete(subRecord);
                    mLogger.WriteLine($"{subRecord} deleted");
                }
            }
        }
    }
}
