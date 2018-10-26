using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using ScottPlotMicrophoneFFT;

namespace SoundRecognition
{
     public delegate void TurnOnMachine();
     public delegate void TurnOffMachine();
     public delegate void ScanItem();
     public delegate void StartMachine();
     public delegate void UpdateWorkingDirectory(string newPath);

     public delegate string AddNewItem(string newItem, int maxHittingTimeInSec, string recognitionType, string category);
     public delegate void ScanExistingItem(string imagePath);

     public delegate void MachineShouldFinish();

     public delegate void SendProcessLatestData(SoundVisualizationDataPackage package);

     public delegate void LogMsg(string msgToLog, ConsoleColor consoleColor);

     class RecognizerMachineManager
     {
          private Machine mMachine = new Machine();
          private MachineUI mMachineUI = new MachineUI();
          private ScannerUI mScannerUI = new ScannerUI();
          private SoundVisualizationUI mSoundVisualization = new SoundVisualizationUI();

          public RecognizerMachineManager()
          {
               Initialize();
          }

          public void Run()
          {
               Application.Run(mMachineUI);
          }

          private void Initialize()
          {
               InitializeMachineUI();
               InitializeScannerUI();

               mMachine.OnMachineShouldFinish += CloseSoundsVisualizationForm;
               Logger.OnLogMsg += WriteLogToUI;
          }

          // Machine Methods.

          private void InitializeMachineUI()
          {
               mMachineUI.OnTurnOn += TurnOnMachine;
               mMachineUI.OnTurnOff += TurnOffMachine;
               mMachineUI.OnWorkingDirectoryUpdate += UpdateWorkingDirectory;
               mMachineUI.OnScanItem += ScanItem;
               mMachineUI.OnStartMachine += StartMachine;
          }

          private void TurnOnMachine()
          {
               mMachine.TurnOn();
               mMachineUI.SetRevealMachineOnButtons(true);
               mScannerUI.SetBarcodesImageList(mMachine.Scanner.BarcodesDirectoryPath);
          }

          private void TurnOffMachine()
          {
               mMachine.TurnOff();
               mMachineUI.SetRevealMachineOnButtons(false);
          }

          private void UpdateWorkingDirectory(string newPath)
          {
               mMachine.WorkingDirectoryPath = newPath;
          }

          private void ScanItem()
          {
               Thread thread = new Thread(() => OpenScannerForm());
               thread.Start();
          }

          private void StartMachine()
          {
               // In case recognizer is a "PopsRecognizer type, it registers the OnSendProcessLatestData event.
               if (mMachine.Recognizer is PopsRecognizer)
               {
                    (mMachine.Recognizer as PopsRecognizer).OnSendProcessLatestData += mSoundVisualization.DrawData;

                    Thread thread = new Thread(() => OpenForm(mSoundVisualization));
                    thread.Start();
               }

               mMachine.StartWorking();
          }

          private void OpenForm<T>(T form) where T : Form
          {
               if (form.IsDisposed)
               {
                    form = Activator.CreateInstance<T>();
               }

               Application.Run(form);
          }

          // Scanner Methods.

          private void InitializeScannerUI()
          {
               mScannerUI.OnAddNewBarcode += AddNewItem;
               mScannerUI.OnScanBarcode += ScanBarcode;
          }

          private void OpenScannerForm()
          {
               if (mScannerUI.IsDisposed)
               {
                    mScannerUI = new ScannerUI();
                    InitializeScannerUI();
                    mScannerUI.SetBarcodesImageList(mMachine.Scanner.BarcodesDirectoryPath);
               }

               Application.Run(mScannerUI);
          }

          /// <summary>
          /// Add new barcode to the database. Returns the full path of the barcode image.
          /// </summary>
          private string AddNewItem(string productName, int maxHittingTimeInSeconds, string recognitionType, string category)
          {
               string barcodeImagePath;

               mMachine.ItemInfo = mMachine.Scanner.CreateNewBarcode(
                    productName,
                    maxHittingTimeInSeconds,
                    recognitionType,
                    category);

               barcodeImagePath = Path.Combine(mMachine.Scanner.BarcodesDirectoryPath, mMachine.ItemInfo.ItemName + ".png");

               mMachine.ScanItem();
               return barcodeImagePath;
          }

          private void ScanBarcode(string imagePath)
          {
               mMachine.ItemInfo = mMachine.Scanner.ScanExistingBarcode(imagePath);
               if (mMachine.ItemInfo != null)
                    mMachineUI.UpdateMachineItemName(mMachine.ItemInfo.ItemName);

               mMachine.ScanItem();
          }

          // SoundVisualizationUI Methods.

          private void CloseSoundsVisualizationForm()
          {
               ThreadHelper.CloseForm(mSoundVisualization);
          }

          // Logger methods.

          private void WriteLogToUI(string msgToLog, ConsoleColor color)
          {
               mMachineUI.LogMsg(msgToLog, color);
          }
     }
}