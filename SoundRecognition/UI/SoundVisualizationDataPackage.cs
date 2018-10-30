using System.Drawing;

namespace SoundRecognition
{
    public class SoundVisualizationDataPackage
    {
        public double[] PCM { get; private set; }
        public double PCMPointSpacingMs { get; private set; }
        public Color PCMDrawColor { get; private set; } = Color.CadetBlue;
        public double[] FFTReal { get; private set; }
        public double FFTPointSpacingHz { get; private set; }
        public Color FFTDrawColor { get; private set; } = Color.CadetBlue;

        public SoundVisualizationDataPackage(double[] pcm, double pcmPointSpacingMs,
             double[] fftReal, double fftPointSpacingHZ, Color color)
        {
            PCM = pcm;
            PCMPointSpacingMs = pcmPointSpacingMs;
            FFTReal = fftReal;
            FFTPointSpacingHz = fftPointSpacingHZ;
            PCMDrawColor = color;
            FFTDrawColor = color;
        }

        public SoundVisualizationDataPackage(double[] pcm, double pcmPointSpacingMs,
             double[] fftReal, double fftPointSpacingHZ)
        {
            PCM = pcm;
            PCMPointSpacingMs = pcmPointSpacingMs;
            FFTReal = fftReal;
            FFTPointSpacingHz = fftPointSpacingHZ;
        }
    }
}
