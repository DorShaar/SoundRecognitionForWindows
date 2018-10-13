using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace SoundRecognition
{
     internal class Machine
     {
          private static readonly int MS_IN_ONE_SECOND = 1000;
          public static readonly int MaximalWorkingTimeInMS = 360 * MS_IN_ONE_SECOND; // 6 minutes.

          //private string mWorkingDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
          // TODO remove this is temp
          private string mWorkingDirectoryPath = @"C:\Users\Dor Shaar\source\repos\SoundRecognition\SoundRecognition\WorkingDirectory";
          private Logger mLogger;
          private IRecognizerMachine mRecognizer;
          private IScanner mScanner;

          private IItemInfo mItemInfo = null;
          public MachineStatus Status { get; private set; } = MachineStatus.TurnedOff;

          internal enum MachineStatus
          {
               TurnedOff = 0,
               OnAndNotWorking = 1,
               OnAndWorking = 2,
               OnAndShouldStop = 4
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

          public void TurnOn()
          {
               AskConfigureDatabase();
               mLogger = new Logger(mWorkingDirectoryPath, nameof(Machine), ConsoleColor.DarkYellow);
               mScanner = new ItemScanner(mWorkingDirectoryPath);

               Status = MachineStatus.OnAndNotWorking;
               mLogger.WriteLine("Recognizer machine turned on");
          }

          public void TurnOff()
          {
               if (Status != MachineStatus.TurnedOff)
               {
                    Status = MachineStatus.TurnedOff;
                    mLogger.WriteLine("Recognizer machine turned off");
               }
          }

          private void AskConfigureDatabase()
          {
               Console.WriteLine($"Current database directory: {mWorkingDirectoryPath}");

               if (!Directory.Exists(mWorkingDirectoryPath))
               {
                    SetDatabasePath();
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
                                   SetDatabasePath();
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

          private void SetDatabasePath()
          {
               Console.WriteLine($"Please enter valid path for database");
               string userInput = Console.ReadLine();

               while (!Directory.Exists(userInput))
               {
                    Console.WriteLine($"{userInput} does not exist. Please enter valid path for database");
                    userInput = Console.ReadLine();
               }

               mWorkingDirectoryPath = userInput;
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
               if(mItemInfo == null)
               {
                    itemName = "No item in the machine";
               }
               else
               {
                    itemName = mItemInfo.ItemName;
               }

               mLogger.WriteLine($"Current Item: {itemName}");
          }

          private void ScanItem()
          {
               mItemInfo = mScanner.Scan();
               if (mItemInfo != null)
               {
                    string recognizerType = (mScanner as ItemScanner).
                         ItemToRecognizeDataMap.GetRecognizerTypeByItem(mItemInfo);

                    mRecognizer = MachineRecognizerFactory.CreateRecognizer(
                        mWorkingDirectoryPath,
                        recognizerType);

                    if (mRecognizer != null)
                    {
                         mRecognizer.RecognizerFinished += SetShouldStopStatus;
                         string itemCategory = (mScanner as ItemScanner).
                              ItemToRecognizeDataMap.GetCategoryByItem(mItemInfo);
                         mRecognizer.LoadProcessedData(recognizerType, itemCategory);
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

          private void SetShouldStopStatus(object sender, RecognizerFinishedEventArgs eventArgs)
          {
               Status = MachineStatus.OnAndShouldStop;
          }

          private void StopMachine()
          {
               mItemInfo = null;
               Status = MachineStatus.OnAndNotWorking;
               mLogger.WriteLine("Machine stopped");
          }

          private void StartWorking()
          {
               if (mItemInfo == null)
               {
                    mLogger.WriteLine($"{nameof(mItemInfo)} is unknown, machine will not start");
                    return;
               }

               if (mRecognizer == null)
               {
                    mLogger.WriteLine($"{nameof(mRecognizer)} is unknown, machine will not start");
                    return;
               }

               Status = MachineStatus.OnAndWorking;
               mLogger.WriteLine("Recognizer machine started");

               int maxHeatingTimeAllowedInMS = Math.Min(
                    (mItemInfo as ItemInfo).MaxHittingTimeInSeconds * MS_IN_ONE_SECOND,
                    MaximalWorkingTimeInMS);

               Stopwatch stopwatch = new Stopwatch();
               stopwatch.Start();
               mLogger.WriteLine("Recognizer machine start working...");

               new Thread(() => mRecognizer.ProcessNewData(mItemInfo)).Start();

               while (Status != MachineStatus.OnAndShouldStop)
               {
                    if (stopwatch.ElapsedMilliseconds >= maxHeatingTimeAllowedInMS)
                    {
                         mRecognizer.Stop();
                         mLogger.WriteLine($"{nameof(Machine)} should stop since reached maximal working time allowed {maxHeatingTimeAllowedInMS}");
                    }
               }

               stopwatch.Stop();
               StopMachine();
          }
     }
}
