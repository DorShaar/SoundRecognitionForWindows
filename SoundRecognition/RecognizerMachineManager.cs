using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SoundRecognition
{
     public delegate void TurnOnMachine();
     public delegate void TurnOffMachine();
     public delegate void ScanItem();
     public delegate void StartMachine();
     public delegate void UpdateWorkingDirectory(string newPath);

     public delegate string AddNewItem(string newItem, int maxHeatingTimeInSec, string recognitionType, string category);
     public delegate void ScanExistingItem(string imagePath);

     public delegate void MachineShouldFinish();
     public delegate void MachineSetItemInfo(IItemInfo itemInfo);

     public delegate void SendProcessLatestData(SoundVisualizationDataPackage package);

     public delegate void LogMsg(string msgToLog, ConsoleColor consoleColor);

     class RecognizerMachineManager
     {
          private Machine mMachine = new Machine();
          private MachineUI mMachineUI = new MachineUI();
          private ScannerUI mScannerUI = new ScannerUI();
          private Thread soundVisualizingThread = null;

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
               InitializeMachine();
               InitializeMachineUI();
               InitializeScannerUI();

               mMachine.OnMachineShouldFinish += DisableSoundsVisualizationForm;
          }

          // Machine Methods.

          private void InitializeMachine()
          {
               mMachine.OnMachineTurnOff += mMachineUI.UpdateMachineItemName;
          }

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
               soundVisualizingThread = new Thread(() => OpenScannerForm());
               soundVisualizingThread.Start();
          }

          private void StartMachine()
          {
               // In case recognizer is a "PopsRecognizer type, it registers the OnSendProcessLatestData event.
               if (mMachine.Recognizer is PopsRecognizer)
               {
                    (mMachine.Recognizer as PopsRecognizer).OnSendProcessLatestData += mMachineUI.DrawData;
                    mMachineUI.SetSoundVisulalization(true);
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
               mScannerUI.OnScanExistingBarcode += ScanBarcode;
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
          private string AddNewItem(string productName, int maxHeatingTimeInSeconds, string recognitionType, string category)
          {
               string barcodeImagePath = null;

               mMachine.ItemInfo = mMachine.Scanner.CreateNewBarcode(
                    productName,
                    maxHeatingTimeInSeconds,
                    recognitionType,
                    category);

               barcodeImagePath = Path.Combine(mMachine.Scanner.BarcodesDirectoryPath, mMachine.ItemInfo.ItemName + ".png");

               mMachine.ScanItem();
               return barcodeImagePath;
          }

          private void ScanBarcode(string imagePath)
          {
               mMachine.ItemInfo = mMachine.Scanner.ScanExistingBarcode(imagePath);
               mMachineUI.UpdateMachineItemName(mMachine.ItemInfo);
               mMachine.ScanItem();
          }

          // SoundVisualizationUI Methods.

          private void DisableSoundsVisualizationForm()
          {
               mMachineUI.SetSoundVisulalization(false);
          }
     }
}