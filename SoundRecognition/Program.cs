using System;
using System.IO;
using System.Windows.Forms;

namespace SoundRecognition
{
     // INFO: https://github.com/AddictedCS/soundfingerprinting

     // Colors:
     // Machine - DarkYellow.
     // Scanner - Magneta.
     // Recorder - DarkMagenta
     // PopRecognizer, specific sound recognizer - Green.
     // SoundsFingerprints - Yellow.
     // WavFileCreator - DarkGray.
     // WavFile - No logger.
     // ItemToRecognizeDataMap - white.
     // SerializationMachine - Cyan.
     // KNN - White.


     // Test: (On raspbery)
     // Test popRecognizer.
     // Test specificSoundsRecognizer.
     // Test directory listener.
     // Test maximal Microwave time (set it to 30 seconds.. see if everything is OK when record is aborted).

     // TODOs:


     // Less important.
     // TODO find out about higher HighPrecisionQueryConfiguration.
     // TODO investigate how fingerprint is built and try figure out if we can find out simmiilarities.

     // DONE:
     // Fix logging.
     // Notify when can delete records in Recorder.
     // Combine maps - one DB.
     // Fix saving and loading fingerprints.
     // Investigate recording to computer + saving while recording (or somthing like that) and analyizing the saved data.
     // More products <=> more recognize methods.
     // Save DBs.
     // Write barcodes into file.
     // binary file for microwave items data base.
     // Write algorithem which identifies first pops and then start to think about the intervals.
     // MicrowaveItem type - according to tag (bar code), parameters of working time and safty accordingly.
     // Start thinking about application run (safty and all that..).
     // injection point to MicrowaveMachine - IRecognizer should be general.
     // Load fingerprints data on application startup.
     // Cut from wav files the start - where there is no pops of popcorn.
     // Amplification.
     // More popcorn sounds - to add to database.

     class Program
     {
          [STAThread]
          static void Main(string[] args)
          {
               //UseWaveFile();
               //return;

               Application.EnableVisualStyles();
               Application.SetCompatibleTextRenderingDefault(false);
               RecognizerMachineManager recognizerMachineManager = new RecognizerMachineManager();
               recognizerMachineManager.Run();
          }

          public static void UseWaveFile()
          {
               try
               {
                    try
                    {
                         string directory = @"C:\Users\Dor Shaar\Desktop\Boiling Water";
                         WavFilesCreator wavFilesCreator = new WavFilesCreator(directory);
                         foreach (string wavFile in Directory.GetFiles(directory))
                         {
                              WavFile wav = new WavFile(wavFile);
                              wavFilesCreator.SplitWaveFileByInterval(wav, 7);
                         }
                    }

                    catch (FileNotFoundException e)
                    {
                         throw e;
                    }
                    catch (FormatException e)
                    {
                         Console.WriteLine(e.Message);
                    }
               }
               catch (FileNotFoundException e)
               {
                    Console.WriteLine(e.Message);
               }
               catch (FormatException e)
               {
                    Console.WriteLine(e.Message);
               }
          }
     }
}