using System;
using System.IO;

namespace SoundRecognition
{
     internal class Logger
     {
          private readonly string LOG_TEXT_EXTENSION = ".txt";
          private readonly string LOG_DIRECTORY_NAME = "logs";
          
          private string mWriterName;
          private ConsoleColor mConsoleColor;

          private static string mLogsDirectory;
          private static object mLocker = new Object();
          private static string mLogTextFilePath;
          private static bool mIsAlreadyCreated = false;

          public static event LogMsg OnLogMsg;

          /// <summary>
          /// Logger will log in one text file all the operation of the application.
          /// </summary>
          /// <param name="workingDirectory"></param>
          /// <param name="writerName"></param>
          /// <param name="consoleColor"></param>
          public Logger(string workingDirectory, string writerName, ConsoleColor consoleColor)
          {
               if(!mIsAlreadyCreated)
               {
                    mLogsDirectory = Path.Combine(workingDirectory, LOG_DIRECTORY_NAME);
                    Directory.CreateDirectory(mLogsDirectory);

                    string timePrefix = GetTimePrefix();
                    mLogTextFilePath = Path.Combine(mLogsDirectory, $"{timePrefix}{LOG_TEXT_EXTENSION}");
               }

               mWriterName = writerName;
               mConsoleColor = consoleColor;

               mIsAlreadyCreated = true;
          }

          public Logger(string writerName, ConsoleColor consoleColor)
          {
               if (!mIsAlreadyCreated)
               {
                    throw new InvalidOperationException($"Should call {nameof(Logger)} with {nameof(mLogsDirectory)}");
               }

               mWriterName = writerName;
               mConsoleColor = consoleColor;
          }

          public void WriteLine(string textToLog)
          {
               lock (mLocker)
               {
                    using (StreamWriter streamWriter = new StreamWriter(mLogTextFilePath, true))
                    {
                         streamWriter.WriteLine(textToLog);
                    }

                    Print(textToLog, mConsoleColor);
               }
          }

          public void WriteError(string textToLog)
          {
               lock (mLocker)
               {
                    using (StreamWriter streamWriter = new StreamWriter(mLogTextFilePath, true))
                    {
                         streamWriter.WriteLine("Error!");
                         streamWriter.WriteLine(textToLog);
                    }

                    Print(textToLog, ConsoleColor.Red);
               }
          }

          public void WriteError(string textToLog, Exception exception)
          {
               lock (mLocker)
               {
                    using (StreamWriter streamWriter = new StreamWriter(mLogTextFilePath, true))
                    {
                         streamWriter.WriteLine(textToLog);
                         streamWriter.WriteLine(exception.ToString());
                    }

                    Print(textToLog, ConsoleColor.Red);
                    Print(exception.Message, ConsoleColor.Red);
               }
          }

          private void Print(string textToLog, ConsoleColor consoleColor)
          {
               OnLogMsg?.Invoke($"{mWriterName}: {textToLog}", consoleColor);
               PrintToConsole_obsolete(textToLog, consoleColor);
          }

          private void PrintToConsole_obsolete(string textToLog, ConsoleColor consoleColor)
          {
               Console.ForegroundColor = consoleColor;
               Console.WriteLine($"{mWriterName}: {textToLog}");
               Console.ResetColor();
          }

          private string GetTimePrefix()
          {
               DateTime dateTime = DateTime.Now;
               return $"{dateTime.Day}.{dateTime.Month}.{dateTime.Year}_{dateTime.Hour}.{dateTime.Minute}.{dateTime.Second}";
          }
     }
}