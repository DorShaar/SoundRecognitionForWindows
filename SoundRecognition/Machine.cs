using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace SoundRecognition
{
     internal class Machine
     {
          private static readonly int MS_IN_ONE_SECOND = 1000;
          public static readonly int MaximalWorkingTimeInMS = 300 * MS_IN_ONE_SECOND; // 5 minutes.

          private Logger mLogger;

          public IRecognizerMachine Recognizer { get; private set; }
          public IItemInfo ItemInfo { get; set; } = null;
          public ItemScanner Scanner { get; private set; }

          public string WorkingDirectoryPath = @"C:\Users\Dor Shaar\Desktop\SoundRecognitionForWindows-master\SoundRecognition\WorkingDirectory"; // TODO change to documents
          public MachineStatus Status { get; private set; } = MachineStatus.TurnedOff;

          public event MachineShouldFinish OnMachineShouldFinish;
          public event MachineSetItemInfo OnMachineTurnOff;

          internal enum MachineStatus
          {
               TurnedOff = 0,
               OnAndNotWorking = 1,
               OnAndWorking = 2,
               OnAndShouldStop = 4
          }

          /// <summary>
          /// Logger should initialize first since it is half static and other components should
          /// be logged.
          /// </summary>
          public Machine()
          {
               mLogger = new Logger(WorkingDirectoryPath, nameof(Machine), ConsoleColor.DarkYellow);
          }

          public void TurnOn()
          {
               Scanner = new ItemScanner(WorkingDirectoryPath);

               Status = MachineStatus.OnAndNotWorking;
               mLogger.WriteLine("Recognizer machine turned on");
          }

          public void Run()
          {
               while (Status != MachineStatus.TurnedOff)
               {
                    ShowManu();
                    PrintCurrentItem();

                    string userInput = Console.ReadLine();
                    switch (userInput.ToLowerInvariant())
                    {
                         case "scan":
                         case "1":
                              ScanItem();
                              break;
                         case "start":
                         case "2":
                              StartWorking();
                              break;
                         case "turn off":
                         case "turn-off":
                         case "off":
                         case "exit":
                         case "3":
                              TurnOff();
                              break;
                         default:
                              mLogger.WriteLine("Invalid input" + Environment.NewLine);
                              break;
                    }
               }
          }

          public void ScanItem()
          {
               if (ItemInfo != null)
               {
                    string recognizerType = (Scanner as ItemScanner).
                         ItemToRecognizeDataMap.GetRecognizerTypeByItem(ItemInfo);

                    Recognizer = MachineRecognizerFactory.CreateRecognizer(
                        WorkingDirectoryPath,
                        recognizerType);

                    if (Recognizer != null)
                    {
                         Recognizer.RecognizerFinished += SetShouldStopStatus;
                         string itemCategory = (Scanner as ItemScanner).
                              ItemToRecognizeDataMap.GetCategoryByItem(ItemInfo);
                         Recognizer.LoadProcessedData(recognizerType, itemCategory);
                    }
                    else
                    {
                         mLogger.WriteLine("Scan result: Item has no recognizer");
                    }
               }
               else
               {
                    mLogger.WriteLine("Scan result: No recognized item available");
               }
          }

          public void StartWorking()
          {
               if (ItemInfo == null)
               {
                    mLogger.WriteLine($"{nameof(ItemInfo)} is unknown, machine will not start");
                    return;
               }

               if (Recognizer == null)
               {
                    mLogger.WriteLine($"{nameof(Recognizer)} is unknown, machine will not start");
                    return;
               }

               Status = MachineStatus.OnAndWorking;
               mLogger.WriteLine("Recognizer machine started");

               int maxHeatingTimeAllowedInMS = Math.Min(
                    (ItemInfo as ItemInfo).MaxHeatingTimeInSeconds * MS_IN_ONE_SECOND,
                    MaximalWorkingTimeInMS);

               Stopwatch stopwatch = new Stopwatch();
               stopwatch.Start();
               mLogger.WriteLine("Recognizer machine start working...");

               new Thread(() => Recognizer.ProcessNewData(ItemInfo)).Start();

               while (Status != MachineStatus.OnAndShouldStop)
               {
                    // Checks if the machine reaches timeout.
                    if (stopwatch.ElapsedMilliseconds >= maxHeatingTimeAllowedInMS)
                    {
                         string stopReason = $"{nameof(Machine)} should stop since reached maximal working time allowed {maxHeatingTimeAllowedInMS}";
                         Recognizer.Stop(stopReason);
                         OnMachineShouldFinish.Invoke();
                    }
               }

               stopwatch.Stop();
               StopMachine();
          }

          public void TurnOff()
          {
               if (Status != MachineStatus.TurnedOff)
               {
                    Recognizer?.Stop($"{nameof(Machine)} requested tp be turned off");
                    OnMachineShouldFinish.Invoke();
                    ClearItemInfo();
                    Status = MachineStatus.TurnedOff;
                    OnMachineTurnOff?.Invoke(null);
                    mLogger.WriteLine("Recognizer machine turned off");
               }
          }

          private void ShowManu()
          {
               mLogger.WriteLine(@"Choose an option:
1. Scan Item
2. Start
3. Turn-Off");
          }

          private void PrintCurrentItem()
          {
               string itemName;
               if (ItemInfo == null)
               {
                    itemName = "No item in the machine";
               }
               else
               {
                    itemName = ItemInfo.ItemName;
               }

               mLogger.WriteLine($"Current Item: {itemName}");
          }

          private void SetShouldStopStatus(object sender, RecognizerFinishedEventArgs eventArgs)
          {
               Status = MachineStatus.OnAndShouldStop;
          }

          private void StopMachine()
          {
               ClearItemInfo();
               Status = MachineStatus.OnAndNotWorking;
               mLogger.WriteLine("Machine stopped");
          }

          private void ClearItemInfo()
          {
               ItemInfo = null;
          }

          // Obsolete methods.

          private void AskConfigureDatabase_obsolete()
          {
               Console.WriteLine($"Current database directory: {WorkingDirectoryPath}");

               if (!Directory.Exists(WorkingDirectoryPath))
               {
                    SetDatabasePath_obsolete();
               }
               else
               {
                    Console.WriteLine("Would you like to change database path?");
                    string userInput = Console.ReadLine();
                    bool isUserInputValid = false;
                    while (!isUserInputValid)
                    {
                         switch (userInput.ToLower())
                         {
                              case "y":
                              case "yes":
                                   SetDatabasePath_obsolete();
                                   isUserInputValid = true;
                                   break;
                              case "n":
                              case "no":
                                   isUserInputValid = true;
                                   break;
                              default:
                                   Console.WriteLine("Invalid input. Type again");
                                   userInput = Console.ReadLine();
                                   break;
                         }
                    }
               }
          }

          private void SetDatabasePath_obsolete()
          {
               Console.WriteLine($"Please enter valid path for database");
               string userInput = Console.ReadLine();

               while (!Directory.Exists(userInput))
               {
                    Console.WriteLine($"{userInput} does not exist. Please enter valid path for database");
                    userInput = Console.ReadLine();
               }

               WorkingDirectoryPath = userInput;
          }
     }
}
