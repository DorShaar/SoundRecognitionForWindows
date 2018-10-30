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
          private MachineStatus mStatus = MachineStatus.TurnedOff;
          public string WorkingDirectoryPath { get; private set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

          public IRecognizerMachine Recognizer { get; private set; }
          public IItemInfo ItemInfo { get; set; } = null;
          public ItemScanner Scanner { get; private set; }

          public event MachineShouldFinish OnMachineShouldFinish;
          public event MachineSetItemInfo OnMachineTurnOff;
          public event MachineSendInfoMsg OnMachineFailedToStart;
          public event MachineSendInfoMsg OnMachineFinishedWithResult;

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
               mStatus = MachineStatus.OnAndNotWorking;
               mLogger.WriteLine("Recognizer machine turned on");
               Scanner = new ItemScanner(WorkingDirectoryPath);
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
                    string errorMsg = $"{nameof(ItemInfo)} is unknown, machine will not start";
                    mLogger.WriteLine(errorMsg);
                    OnMachineFailedToStart?.Invoke(errorMsg);
                    return;
               }

               if (Recognizer == null)
               {
                    string errorMsg = $"{nameof(Recognizer)} is unknown, machine will not start";
                    mLogger.WriteLine(errorMsg);
                    OnMachineFailedToStart?.Invoke(errorMsg);
                    return;
               }

               mStatus = MachineStatus.OnAndWorking;
               mLogger.WriteLine("Recognizer machine started");

               int maxHeatingTimeAllowedInMS = Math.Min(
                    (ItemInfo as ItemInfo).MaxHeatingTimeInSeconds * MS_IN_ONE_SECOND,
                    MaximalWorkingTimeInMS);

               Stopwatch stopwatch = new Stopwatch();
               stopwatch.Start();
               mLogger.WriteLine("Recognizer machine start working...");

               new Thread(() => Recognizer.ProcessNewData(ItemInfo)).Start();

               while (mStatus != MachineStatus.OnAndShouldStop)
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
               OnMachineFinishedWithResult?.Invoke(Recognizer.RecognitionStatus.ToString());
               StopMachine();
          }

          public void TurnOff()
          {
               if (mStatus != MachineStatus.TurnedOff)
               {
                    Recognizer?.Stop($"{nameof(Machine)} requested to be turned off");
                    OnMachineShouldFinish.Invoke();
                    ClearItemInfo();
                    mStatus = MachineStatus.TurnedOff;
                    OnMachineTurnOff?.Invoke(null);
                    mLogger.WriteLine("Recognizer machine turned off");
               }
          }

          /// <summary>
          /// That property setting is in method since we want to make sure all the paths
          /// of the machine components are updated also.
          /// </summary>
          /// <param name="newPath"></param>
          public void SetWorkingDirectoryPath(string newPath)
          {
               WorkingDirectoryPath = newPath;
               Scanner?.SetWorkingDirectoryPath(newPath);
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
               mStatus = MachineStatus.OnAndShouldStop;
               OnMachineShouldFinish.Invoke();
          }

          private void StopMachine()
          {
               ClearItemInfo();
               mStatus = MachineStatus.OnAndNotWorking;
               mLogger.WriteLine("Machine stopped");
          }

          private void ClearItemInfo()
          {
               ItemInfo = null;
          }

          // Obsolete methods.

          public void Run_obsolete()
          {
               while (mStatus != MachineStatus.TurnedOff)
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
