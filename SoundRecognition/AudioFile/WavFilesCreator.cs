using System;
using System.IO;

namespace SoundRecognition
{
    internal class WavFilesCreator
    {
        private readonly string RECORDS_SECTIONS_DIRECTORY_NAME = "RecordsSections";
        private readonly string mOutputDirectoryPath;
        private Logger mLogger;

        public WavFilesCreator(string workingDirectoryPath)
        {
            mOutputDirectoryPath = Path.Combine(workingDirectoryPath, RECORDS_SECTIONS_DIRECTORY_NAME);
            mLogger = new Logger(mOutputDirectoryPath, nameof(WavFilesCreator), ConsoleColor.DarkGray);
            Initialize();
        }

        /// <summary>
        /// Creates a folder with the same BARCODE_DIRECTORY_NAME wave file name, moves the wave file to that folder
        /// and creates another folder with the splitted wave files.
        /// </summary>
        /// <param name="intervalInSeconds"></param>
        public void SplitWaveFileByInterval(WavFile wavFile, float intervalInSeconds)
        {
            // Creates directory "waveFilesDirectory".
            FilePath waveFilesDirectory = FilePath.CreateFilePath(
                wavFile.FilePath.DirectoryPath, wavFile.FilePath.NameWithoutExtension);
            Directory.CreateDirectory(waveFilesDirectory.FileFullPath);
            mLogger.WriteLine($"New directory created:{waveFilesDirectory.FileFullPath}");

            // Moves wave file into directory.
            FilePath newWaveFilePath = FilePath.CreateFilePath(
                 waveFilesDirectory.FileFullPath, wavFile.FilePath.Name);
            File.Move(wavFile.FilePath.FileFullPath, newWaveFilePath.FileFullPath);
            wavFile.FilePath = newWaveFilePath;
            mLogger.WriteLine($"Wave file renamed to {newWaveFilePath.FileFullPath}");

            // Creates directory inside "waveFilesDirectory" for the splitted wave files.
            FilePath splittedWaveFilesDirectory = FilePath.CreateFilePathWithPrefix(
                waveFilesDirectory.FileFullPath, "splitted", wavFile.FilePath.NameWithoutExtension);
            Directory.CreateDirectory(splittedWaveFilesDirectory.FileFullPath);
            mLogger.WriteLine($"New directory created:{splittedWaveFilesDirectory.FileFullPath}");

            int bytesNumberPerSplit = (int)(intervalInSeconds * wavFile.SampleRate * wavFile.BytesPerSample);
            int splitsNumber = (int)Math.Ceiling(
                Convert.ToDecimal(wavFile.DataSizeInBytes / bytesNumberPerSplit));
            int bytesReaderIndex = 0;

            mLogger.WriteLine(
                $"{wavFile.FilePath.Name} will be splited into interval of {intervalInSeconds} seconds");

            for (int splitNum = 0; splitNum < splitsNumber; ++splitNum)
            {
                byte[] samplesGroup = new byte[bytesNumberPerSplit];
                for (int bytesWriterIndex = 0;
                    (bytesWriterIndex < bytesNumberPerSplit) &&
                    (bytesReaderIndex < wavFile.DataSizeInBytes);
                     ++bytesWriterIndex, ++bytesReaderIndex)
                {
                    samplesGroup[bytesWriterIndex] = wavFile.SoundData[bytesReaderIndex];
                }

                FilePath splittedWaveFilePath = FilePath.CreateFilePathWithPrefix(
                    splittedWaveFilesDirectory.FileFullPath, splitNum.ToString(), $"split{WavFile.WavFileExtension}");
                using (Stream outputSteam = new FileStream(
                    splittedWaveFilePath.FileFullPath, FileMode.Create, FileAccess.Write))
                {
                    wavFile.WriteWaveFileHeaderToStream(outputSteam, bytesNumberPerSplit);
                    outputSteam.Write(samplesGroup, 0, bytesNumberPerSplit);
                }

                mLogger.WriteLine($"{splittedWaveFilePath.FileFullPath} was saved");
            }
        }

        /// <summary>
        /// Get queue (order is important) of .wav files and combine them to one wav files.
        /// Updates the relevant metadata.
        /// Takes the first wav file in the queue and using it's metadata properties. So
        /// in case two wav files with different byte rate combined, unexpected behaviour
        /// can occur.
        /// </summary>
        /// <param name="wavFilesQueue"></param>
        public void CombineWaveFiles(FixedSizedQueue<WavFile> wavFilesQueue, string combinedWaveFileName)
        {
            FilePath combinedWaveFile = FilePath.CreateFilePath(mOutputDirectoryPath, combinedWaveFileName);

            if (wavFilesQueue != null && wavFilesQueue.Count > 0)
            {
                WavFile firstWaveFile;
                wavFilesQueue.TryPeek(out firstWaveFile);
                int totalBytesNumber = 0;

                // Iterating over queue is by the order of the dequeuing, but without actual dequeuing
                foreach (WavFile wavFile in wavFilesQueue)
                {
                    totalBytesNumber += wavFile.SoundData.Length;
                }

                byte[] samplesGroup = new byte[totalBytesNumber];

                int bytesWriterIndex = 0;
                foreach (WavFile wavFile in wavFilesQueue)
                {
                    // Behaves like memcpy from c.
                    wavFile.SoundData.CopyTo(samplesGroup, bytesWriterIndex);
                    bytesWriterIndex += wavFile.SoundData.Length;
                }

                using (Stream outputSteam = new FileStream(
                   combinedWaveFile.FileFullPath, FileMode.Create, FileAccess.Write))
                {
                    firstWaveFile.WriteWaveFileHeaderToStream(outputSteam, totalBytesNumber);
                    outputSteam.Write(samplesGroup, 0, totalBytesNumber);
                }
            }
        }

        public void CutFromWaveFile(WavFile waveFile, FilePath outputFilePath, int cutBeginInSeconds, int cutEndInSeconds)
        {
            int secondsToCut = cutEndInSeconds - cutBeginInSeconds;
            if (secondsToCut < 0)
            {
                mLogger.WriteLine($"{nameof(cutBeginInSeconds)} is greater then {nameof(cutEndInSeconds)}");
                return;
            }
            else if ((cutBeginInSeconds < 0) || (cutEndInSeconds > waveFile.DurationInSeconds))
            {
                mLogger.WriteLine($"Illegel value of {nameof(cutBeginInSeconds)} or {nameof(cutEndInSeconds)}");
                return;
            }
            else
            {
                // The method: Copies from start. When reaches endCopyPosition stop to copy and return to
                // copy when reaches startCopyAgainPosition.
                int endCopyPosition = cutBeginInSeconds * waveFile.ByteRate;
                int startCopyAgainPosition = cutEndInSeconds * waveFile.ByteRate;
                int writer = 0;

                byte[] cutSoundData = new byte[waveFile.SoundData.Length - (startCopyAgainPosition - endCopyPosition)];

                for (int reader = 0; reader < endCopyPosition; ++reader, ++writer)
                {
                    cutSoundData[writer] = waveFile.SoundData[reader];
                }

                for (int reader = startCopyAgainPosition; reader < waveFile.SoundData.Length; ++reader, ++writer)
                {
                    cutSoundData[writer] = waveFile.SoundData[reader];
                }

                using (Stream outputSteam = new FileStream(
                    outputFilePath.FileFullPath, FileMode.Create, FileAccess.Write))
                {
                    waveFile.WriteWaveFileHeaderToStream(outputSteam, cutSoundData.Length);
                    outputSteam.Write(cutSoundData, 0, cutSoundData.Length);
                }

                mLogger.WriteLine($"{outputFilePath.FileFullPath} was saved");
            }
        }

        public void CutFromStart(WavFile wavFile, FilePath outputFilePath, int secondsToCut)
        {
            CutFromWaveFile(wavFile, outputFilePath, 0, secondsToCut);
        }

        public void CutFromEnd(WavFile wavFile, FilePath outputFilePath, int secondsToCut)
        {
            CutFromWaveFile(
                wavFile,
                outputFilePath,
                wavFile.DurationInSeconds - secondsToCut,
                wavFile.DurationInSeconds);
        }

        private void Initialize()
        {
            Directory.CreateDirectory(mOutputDirectoryPath);
        }
    }
}
