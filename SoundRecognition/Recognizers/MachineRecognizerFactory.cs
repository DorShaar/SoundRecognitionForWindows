
namespace SoundRecognition
{
    internal class MachineRecognizerFactory
    {
        internal static IRecognizerMachine CreateRecognizer(
            string workingDirectoryPath,
            string recognizerType)
        {
            IRecognizerMachine recognizerMachine = null;
            if (recognizerType == ItemToRecognizeDataMap.RecognizerType[2])
            {
                recognizerMachine = new PopsRecognizer(workingDirectoryPath);
            }
            else if (recognizerType == ItemToRecognizeDataMap.RecognizerType[1])
            {
                int amplification = 10;
                int secondsToAnalyzeAudioFiles = 10;

                recognizerMachine = new SpecificSoundRecognizer(
                    workingDirectoryPath,
                    amplification,
                    secondsToAnalyzeAudioFiles);
            }

            return recognizerMachine;
        }
    }
}
