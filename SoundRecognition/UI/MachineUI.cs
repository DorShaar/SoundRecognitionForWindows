using System;
using System.Windows.Forms;

namespace SoundRecognition
{
     public partial class MachineUI : Form
     {
          private bool mIsMachineTurnedOn = false;

          public event TurnOnMachine OnTurnOn;
          public event TurnOffMachine OnTurnOff;
          public event UpdateWorkingDirectory OnWorkingDirectoryUpdate;
          public event ScanItem OnScanItem;
          public event StartMachine OnStartMachine;

          public MachineUI()
          {
               InitializeComponent();

               LogRichTextBox.ReadOnly = true;

               WorkingDirectoryTextBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
               WorkingDirectoryTextBox.Text = @"C:\Users\Dor Shaar\source\repos\SoundRecognition\SoundRecognition\WorkingDirectory"; // TODO delete.
          }

          public void LogMsg(string msgToLog, ConsoleColor color)
          {
               ThreadHelper.AppendTextToRichTextBox(this, LogRichTextBox, msgToLog);
          }

          public void SetRevealMachineOnButtons(bool shouldBeRevealed)
          {
               ScanItemButton.Visible = shouldBeRevealed;
               StartMachineButton.Visible = shouldBeRevealed;
          }

          public void UpdateMachineItemName(string newMachineItemName)
          {
               ThreadHelper.SetText(this, CurrentItemNameLabel, newMachineItemName);
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
               OnStartMachine.Invoke();
          }
     }
}
