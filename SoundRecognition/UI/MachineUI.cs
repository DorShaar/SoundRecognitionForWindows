using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SoundRecognition
{
     public partial class MachineUI : Form
     {
          private bool mIsMachineTurnedOn = false;
          private bool mIsNeedsAutoScaling = true;

          public event TurnOnMachine OnTurnOn;
          public event TurnOffMachine OnTurnOff;
          public event UpdateWorkingDirectory OnWorkingDirectoryUpdate;
          public event ScanItem OnScanItem;
          public event StartMachine OnStartMachine;

          public MachineUI()
          {
               InitializeComponent();
               SetupGraphLabels();
          }

          public void SetRevealMachineOnButtons(bool shouldBeRevealed)
          {
               ScanItemButton.Visible = shouldBeRevealed;
               StartMachineButton.Visible = shouldBeRevealed;
          }

          public void UpdateMachineItemName(IItemInfo itemInfo)
          {
               if (itemInfo == null)
                    ThreadHelper.SetText(this, CurrentItemNameLabel, "None");
               else
                    ThreadHelper.SetText(this, CurrentItemNameLabel, itemInfo.ItemName);
          }

          public void SetWorkingDirectoryTextBox(string path)
          {
               WorkingDirectoryTextBox.Text = path;
          }

          private void TurnOnButton_Click(object sender, EventArgs e)
          {
               if (mIsMachineTurnedOn)
               {
                    OnTurnOff.Invoke();
                    TurnOnOffButton.Text = "Turn-On";
                    mIsMachineTurnedOn = false;
               }
               else
               {
                    OnTurnOn.Invoke();
                    TurnOnOffButton.Text = "Turn-Off";
                    mIsMachineTurnedOn = true;
               }
          }

          private void SetWorkingDirectoryButton_Click(object sender, EventArgs e)
          {
               FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
               if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
               {
                    WorkingDirectoryTextBox.Text = folderBrowserDialog.SelectedPath;
                    OnWorkingDirectoryUpdate.Invoke(WorkingDirectoryTextBox.Text);
               }
          }

          private void ScanItemButton_Click(object sender, EventArgs e)
          {
               OnScanItem.Invoke();
          }

          private void StartMachineButton_Click(object sender, EventArgs e)
          {
               Thread startMachineTask = new Thread(() => OnStartMachine.Invoke());
               startMachineTask.Start();
          }

          // FFT Visualization:

          private void SetupGraphLabels()
          {
               mFFTVisual.Figure.labelTitle = "Microphone FFT Data";
               mFFTVisual.Figure.labelY = "Power (raw)";
               mFFTVisual.Figure.labelX = "Frequency (Hz)";
               mFFTVisual.Redraw();
          }

          public void DrawData(SoundVisualizationDataPackage dataToDraw)
          {
               // Plots the Xs and Ys for graph.
               mFFTVisual.Clear();
               mFFTVisual.PlotSignal(
                    dataToDraw.FFTReal, dataToDraw.FFTPointSpacingHz, dataToDraw.FFTDrawColor);
               //mFFTVisual.PlotSignal(new double[5], 5); //TODO: what this is for? this is where the 5 red points are printed

               // Optionally adjust the scale to automatically fit the data.
               if (mIsNeedsAutoScaling)
               {
                    mFFTVisual.AxisAuto();
                    mIsNeedsAutoScaling = false;
               }

               // Reduces flicker and helps keep the program responsive.
               Application.DoEvents();
          }

          public void SetSoundVisulalization(bool isEnabled)
          {
               ClearFFTVisualGraph();
               ThreadHelper.SetEnabledProperty(this, mFFTVisual, isEnabled);
          }

          public void ClearFFTVisualGraph()
          {
               mFFTVisual.Clear(true);
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

          private void MachineUI_FormClosed(object sender, FormClosedEventArgs e)
          {
               OnTurnOff.Invoke();
               mIsMachineTurnedOn = false;
          }
     }
}
