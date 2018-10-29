using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SoundRecognition
{
     internal class ItemToRecognizeDataMap : IDatabaseHolder
     {
          private readonly string ITEMS_TO_RECOGNIZER_TYPE_MAP_NAME = "ItemToRecognizeDataMap.bin";
          private readonly string mDatabaseDirectoryName;
          private Dictionary<IItemInfo, ItemRecognizeData> mItemToRecognizerTypeDictionary = 
               new Dictionary<IItemInfo, ItemRecognizeData>();
          private Logger mLogger;

          public ItemToRecognizeDataMap(string databaseDirectoryName)
          {
               mDatabaseDirectoryName = databaseDirectoryName;
               mLogger = new Logger(nameof(ItemToRecognizeDataMap), ConsoleColor.White);
          }

          public static readonly string[] RecognizerType = new string[]
          {
               "UnkownRecognizer",         // Should be first as need to be default value
               "SpecificSoundRequired",
               "Popcorn"
          };

          public void LoadDatabases()
          {
               string itemsToRecognizerTypeMapDatabasePath =
                    Path.Combine(mDatabaseDirectoryName, ITEMS_TO_RECOGNIZER_TYPE_MAP_NAME);
               mItemToRecognizerTypeDictionary = SerializationMachine.
                    LoadDictionaryFromDB<IItemInfo, ItemRecognizeData>(itemsToRecognizerTypeMapDatabasePath);
          }

          public void SaveDatabases()
          {
               string itemsToRecognizerTypeDatabasePath = Path.Combine(mDatabaseDirectoryName, ITEMS_TO_RECOGNIZER_TYPE_MAP_NAME);
               SerializationMachine.SaveDictionaryIntoDB(itemsToRecognizerTypeDatabasePath, mItemToRecognizerTypeDictionary);
          }

          public void Add(IItemInfo itemInfo, string recognizerType, string category)
          {
               mItemToRecognizerTypeDictionary.Add(
                    itemInfo,
                    new ItemRecognizeData(recognizerType, category));
          }

          public string GetRecognizerTypeByItem(IItemInfo item)
          {
               string recognizerType = "UnkownRecognizer";
               ItemRecognizeData recognizerData = null;

               foreach(KeyValuePair<IItemInfo, ItemRecognizeData> keyValue in mItemToRecognizerTypeDictionary)
               {
                    if(keyValue.Key.Barcode == item.Barcode)
                    {
                         recognizerData = keyValue.Value;
                    }
               }

               if (recognizerData != null)
               {
                    recognizerType = recognizerData.RecognizerType;
               }

               return recognizerType;
          }

          public string GetCategoryByItem(IItemInfo item)
          {
               string category = "Unclassified";
               ItemRecognizeData recognizerData = null;

               foreach (KeyValuePair<IItemInfo, ItemRecognizeData> keyValue in mItemToRecognizerTypeDictionary)
               {
                    if (keyValue.Key.Barcode == item.Barcode)
                    {
                         recognizerData = keyValue.Value;
                    }
               }

               if (recognizerData != null)
               {
                    category = recognizerData.Category;
               }

               return category;
          }

          public void PrintCategories()
          {
               StringBuilder stringBuilder = new StringBuilder();
               stringBuilder.Append("Categories: ");

               if (mItemToRecognizerTypeDictionary.Count > 0)
               {
                    foreach (string category in GetDistinctCategories())
                    {
                         stringBuilder.Append($"{category}, ");
                    }
               }
               else
               {
                    stringBuilder.Append("{No cetagories}");
               }

               stringBuilder.Append(string.Empty);
               mLogger.WriteLine(stringBuilder.ToString());
          }

          public List<string> GetCategories()
          {
               return GetDistinctCategories().ToList();
          }

          private IEnumerable<string> GetDistinctCategories()
          {
               List<string> catagoriesList = new List<string>();
               foreach (ItemRecognizeData itemData in mItemToRecognizerTypeDictionary.Values)
               {
                    catagoriesList.Add(itemData.Category);
               }

               return catagoriesList.Distinct();
          }
     }
}
