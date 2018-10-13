using System;
using System.IO;
using System.Collections.Generic;
using SoundFingerprinting;
using SoundFingerprinting.Audio;
using SoundFingerprinting.Builder;
using SoundFingerprinting.Configuration;
using SoundFingerprinting.DAO.Data;
using SoundFingerprinting.InMemory;
using SoundFingerprinting.Query;
using SoundFingerprinting.DAO;
using SoundFingerprinting.Data;

namespace SoundRecognition
{
     class SoundFingerprintingWrapper
     {
          private readonly string DATABASE_DIRECTORY_NAME = "Fingerprints";
          private readonly string FINGERPRINT_EXTENSION = ".fp";
          private readonly string TRACK_REFERENCE_EXTENSION = ".tr";

          private string mDatabaseDirectoryPath;
          private Logger mLogger;
          private readonly IModelService mModelService = new InMemoryModelService(); // Stores fingerprints in RAM.
          private readonly IAudioService mAudioService = new SoundFingerprintingAudioService(); // Default audio library.

          public string DatabaseCategory { get; set; } = "Unclassified";
          public string RecognizerType { get; set; } = ItemToRecognizeDataMap.RecognizerType[0];

          public SoundFingerprintingWrapper(string workingDirectory)
          {
               Initialize(workingDirectory);
          }

          public SoundFingerprintingWrapper(string workingDirectory, string databaseCategory)
          {
               DatabaseCategory = databaseCategory;
               Initialize(workingDirectory);
          }

          /// <summary>
          /// Search for all files with given extensios in given directory.
          /// </summary>
          /// <param name="waveFile"></param>
          public void LoadFingerPrintsDatabase()
          {
               try
               {
                    string[] files = Directory.GetFiles(mDatabaseDirectoryPath, $"*{FINGERPRINT_EXTENSION}");
                    foreach (string fingerPrintFile in files)
                    {
                         List<HashedFingerprint> hashedFingerprints =
                             SerializationMachine.ProtoDeserialize<List<HashedFingerprint>>(
                                 FilePath.CreateFilePath(fingerPrintFile));

                         string trackReferenceFile = fingerPrintFile.Replace(FINGERPRINT_EXTENSION, TRACK_REFERENCE_EXTENSION);
                         if (File.Exists(trackReferenceFile))
                         {
                              ModelReference<int> trackReference = SerializationMachine.ProtoDeserialize<ModelReference<int>>(
                                      FilePath.CreateFilePath(trackReferenceFile));

                              if (hashedFingerprints != null && trackReference != null)
                              {
                                   mModelService.InsertHashDataForTrack(hashedFingerprints, trackReference);
                                   mLogger.WriteLine($"Loaded fingerprint of track reference ID: {trackReference.Id}");
                              }
                              else
                              {
                                   mLogger.WriteLine($"Cannot load fingerprint {fingerPrintFile}");
                              }
                         }
                         else
                         {
                              mLogger.WriteError($"Error! the track reference file of {fingerPrintFile} is missing");
                         }
                    }
               }
               catch (DirectoryNotFoundException)
               {
                    mLogger.WriteError($"No such directory: {mDatabaseDirectoryPath}");
               }
               catch (Exception ex) when (ex is ArgumentException || ex is NullReferenceException)
               {
                    mLogger.WriteError($"Exception occures: {ex.Message}", ex);
               }
          }

          public void LoadWavFilesDatabase()
          {
               string databaseDirectoryPath = Path.Combine(mDatabaseDirectoryPath, RecognizerType, DatabaseCategory);

               try
               {
                    string[] files = Directory.GetFiles(databaseDirectoryPath);
                    foreach (string wavFile in files)
                    {
                         StoreNewAudioFileData(new WavFile(wavFile));
                    }
               }
               catch (DirectoryNotFoundException)
               {
                    mLogger.WriteError($"No such directory: {databaseDirectoryPath}");
               }
               catch (Exception ex) when (ex is ArgumentException || ex is NullReferenceException)
               {
                    mLogger.WriteError($"Exception occures: {ex.Message}", ex);
               }
          }

          public bool IsAudioFileDetected(IAudioFile audioFile, int amplification, double secondToAnalyze)
          {
               bool isAudioFileDetected = false;
               List<ResultEntry> matches = GetMatchesForAudioFile(audioFile, amplification, secondToAnalyze);
               if(matches.Count > 0 )
               {
                    isAudioFileDetected = true;
               }

               return isAudioFileDetected;
          }

          /// <summary>
          /// Stores the fingerprints of the given file into the ModelService.
          /// Uses HighPrecisionFingerprintConfiguration.
          /// </summary>
          /// <param name="waveFile"></param>
          private void StoreNewAudioFileData(IAudioFile waveFile)
          {
               TrackData track = new TrackData(
                   isrc: "TD100INPROG", // International Standart Recording Code.
                   artist: "The TD's",
                   title: waveFile.FilePath.NameWithoutExtension,
                   album: waveFile.FilePath.DirectoryPath,
                   releaseYear: DateTime.Today.Year,
                   length: waveFile.DurationInSeconds);

               // Stores track metadata in the datasource.
               //TRack trackReference = new TrackReference<int>(mModelService.InsertTrack(track).Id);
               IModelReference trackReference = mModelService.InsertTrack(track);
               ModelReference<int> trackReference2 = trackReference as ModelReference<int>;

               // Creates hashed fingerprints.
               List<HashedFingerprint> hashedFingerprints = FingerprintCommandBuilder.Instance
                                           .BuildFingerprintCommand()
                                           .From(waveFile.FilePath.FileFullPath)
                                           .WithFingerprintConfig(new DefaultFingerprintConfiguration())
                                           .UsingServices(mAudioService)
                                           .Hash()
                                           .Result;

               // Stores hashes in the database.
               //SaveFingerPrintsInMemory(hashedFingerprints, trackReference2, waveFile.FilePath.NameWithoutExtension); // Currenntly not using fp.
               mModelService.InsertHashDataForTrack(hashedFingerprints, trackReference);
               mLogger.WriteLine($"Stored {hashedFingerprints.Count} hashed fingerprints from {waveFile.FilePath.Name}");
          }

          private List<ResultEntry> GetMatchesForAudioFile(IAudioFile audioFile, int amplification, double secondToAnalyze)
          {
               List<ResultEntry> resultEntriesList = new List<ResultEntry>();
               List<double> matchTimesList = new List<double>();

               if (amplification <= 0) { amplification = 1; }
               mLogger.WriteLine($"Quering {audioFile.FilePath.Name} with amplification {amplification}, analyzing {secondToAnalyze} seconds");

               for (int i = 0; i < amplification; ++i)
               {
                    QueryResult queryResult = GetQueryResultForSong(audioFile.FilePath.FileFullPath, secondToAnalyze, 0);
                    if (queryResult != null)
                    {
                         foreach (ResultEntry resultEntry in queryResult.ResultEntries)
                         {
                              if (resultEntry.Confidence > 0.2 && !matchTimesList.Contains(resultEntry.TrackMatchStartsAt))
                              {
                                   resultEntriesList.Add(resultEntry);
                                   matchTimesList.Add(resultEntry.TrackMatchStartsAt);
                              }
                         }
                    }
               }

               return resultEntriesList;
          }

          private void SaveFingerPrintsInMemory(
              List<HashedFingerprint> newHashedFingerprints,
              ModelReference<int> trackReference,
              string name)
          {
               FilePath fingerPrintsDBFilePath =
                   FilePath.CreateFilePath(Path.Combine(mDatabaseDirectoryPath, RecognizerType, DatabaseCategory, $"{name}{FINGERPRINT_EXTENSION}"));
               FilePath trackRefernceFilePath =
                   FilePath.CreateFilePath(Path.Combine(mDatabaseDirectoryPath, RecognizerType, DatabaseCategory, $"{name}{TRACK_REFERENCE_EXTENSION}"));
               SerializationMachine.ProtoSerialize(newHashedFingerprints, fingerPrintsDBFilePath);
               SerializationMachine.ProtoSerialize(trackReference, trackRefernceFilePath);
          }

          private QueryResult GetQueryResultForSong(string queryAudioFile, double secondsToAnalyze, int startAtSecond)
          {
               QueryResult queryResult = null;
               DefaultQueryConfiguration queryConfiguration = new DefaultQueryConfiguration();
               System.Threading.Tasks.Task<QueryResult> queryTask = QueryCommandBuilder.Instance.BuildQueryCommand()
                                                            .From(queryAudioFile, secondsToAnalyze, startAtSecond)
                                                            .WithQueryConfig(queryConfiguration)
                                                            .UsingServices(mModelService, mAudioService)
                                                            .Query();
               queryResult = queryTask.Result;

               return queryResult;
          }

          private void Initialize(string workingDirectory)
          {
               mLogger = new Logger(nameof(SoundFingerprintingWrapper), ConsoleColor.Yellow);

               mDatabaseDirectoryPath = Path.Combine(workingDirectory, DATABASE_DIRECTORY_NAME);
               if (!Directory.Exists(DATABASE_DIRECTORY_NAME))
               {
                    Console.WriteLine($"Creating directory: {mDatabaseDirectoryPath}");
                    Directory.CreateDirectory(mDatabaseDirectoryPath);
               }
          }
     }
}