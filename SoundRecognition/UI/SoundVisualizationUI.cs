using System;
using System.Text;
using System.Windows.Forms;
using ScottPlot;
using SoundRecognition;

namespace ScottPlotMicrophoneFFT
{
     public partial class SoundVisualizationUI : Form
     {
          private SoundVisualizationUserControl mPCMVisual;
          private SoundVisualizationUserControl mFFTVisual;

          private int mNumberOfDraws = 0;
          private bool mIsNeedsAutoScaling = true;

          public SoundVisualizationUI()
          {
               InitializeComponent();
               SetupGraphLabels();
          }

          private void SetupGraphLabels()
          {
               mPCMVisual.Figure.labelTitle = "Microphone PCM Data";
               mPCMVisual.Figure.labelY = "Amplitude (PCM)";
               mPCMVisual.Figure.labelX = "Time (ms)";
               mPCMVisual.Redraw();

               mFFTVisual.Figure.labelTitle = "Microphone FFT Data";
               mFFTVisual.Figure.labelY = "Power (raw)";
               mFFTVisual.Figure.labelX = "Frequency (Hz)";
               mFFTVisual.Redraw();
          }

          public void DrawData(SoundVisualizationDataPackage dataToDraw)
          {
               // Plots the Xs and Ys for both graphs.
               mPCMVisual.Clear();
               mPCMVisual.PlotSignal(
                    dataToDraw.PCM, dataToDraw.PCMPointSpacingMs, dataToDraw.PCMDrawColor);
               mPCMVisual.PlotSignal(new double[5], 5);
               mFFTVisual.Clear();
               mFFTVisual.PlotSignal(
                    dataToDraw.FFTReal, dataToDraw.FFTPointSpacingHz, dataToDraw.FFTDrawColor);
               mFFTVisual.PlotSignal(new double[5], 5);

               // Optionally adjust the scale to automatically fit the data.
               if (mIsNeedsAutoScaling)
               {
                    mPCMVisual.AxisAuto();
                    mFFTVisual.AxisAuto();
                    mIsNeedsAutoScaling = false;
               }

               mNumberOfDraws++;
               lblStatus.Text = $"Analyzed and graphed PCM and FFT data {mNumberOfDraws} times";

               // Reduces flicker and helps keep the program responsive.
               Application.DoEvents();
          }

          private void AutoScaleToolStripMenuItem_Click(object sender, EventArgs e)
          {
               mIsNeedsAutoScaling = true;
          }

          private void InfoMessageToolStripMenuItem_Click(object sender, EventArgs e)
          {
               StringBuilder msg = new StringBuilder();
               msg.AppendLine("left-click-drag to pan");
               msg.AppendLine("right-click-drag to zoom");
               msg.AppendLine("middle-click to auto-axis\n");
               msg.AppendLine("double-click for graphing stats\n");

               MessageBox.Show(msg.ToString());
          }

          private void WebsiteToolStripMenuItem_Click(object sender, EventArgs e)
          {
               System.Diagnostics.Process.Start("https://github.com/swharden/Csharp-Data-Visualization");
          }
     }
}
