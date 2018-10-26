using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using ZXing;

namespace SoundRecognition
{
     internal class ItemScanner : IScanner
     {
          private readonly string DATABASE_DIRECTORY_NAME = "database";
          private readonly string ITEMS_DATA_BASE_NAME = "ItemsDB.bin";
          private readonly string BARCODE_DIRECTORY_NAME = "Barcodes";
          private readonly string PNG_EXTENSION = ".png";

          private readonly string mDatabaseDirectoryPath;
          private readonly string mItemsDBPath;
          public readonly string BarcodesDirectoryPath;
          private Dictionary<string, IItemInfo> mItemsDictionary =
               new Dictionary<string, IItemInfo> { };
          private Logger mLogger;

          public ItemToRecognizeDataMap ItemToRecognizeDataMap;

          public ItemScanner(string workingDirectoryPath)
          {
               mLogger = new Logger(workingDirectoryPath, nameof(ItemScanner), ConsoleColor.Magenta);
               mDatabaseDirectoryPath = Path.Combine(workingDirectoryPath, DATABASE_DIRECTORY_NAME);
               mItemsDBPath = Path.Combine(mDatabaseDirectoryPath, ITEMS_DATA_BASE_NAME);
               BarcodesDirectoryPath = Path.Combine(workingDirectoryPath, BARCODE_DIRECTORY_NAME);
               ItemToRecognizeDataMap = new ItemToRecognizeDataMap(mDatabaseDirectoryPath);
               LoadDatabases();
          }

          private void ShowScanManu()
          {
               mLogger.WriteLine(@"Choose an option:
1. Create New Barcode.
2. Scan Existing Barcode
");
          }

          public IItemInfo CreateNewBarcode(string productName, int maxHittingTimeInSeconds,
               string recognitionType, string category)
          {
               IItemInfo itemInfo = null;
               if (productName != null && maxHittingTimeInSeconds > 0 && recognitionType != null && category != null)
               {
                    // Created barcodes directory in case there is no existing one.
                    Directory.CreateDirectory(BarcodesDirectoryPath);

                    // Generate uniqe string and creates QR-Code from it.
                    Guid guid = Guid.NewGuid();
                    mLogger.WriteLine($"Generating string to encode: {guid}");
                    string stringToEncode = guid.ToString();

                    ZXing.Common.EncodingOptions encodingOptions = new ZXing.Common.EncodingOptions
                    {
                         Width = 750,
                         Height = 750
                    };

                    BarcodeWriter barcodeWriter = new BarcodeWriter
                    {
                         Format = BarcodeFormat.QR_CODE,
                         Options = encodingOptions
                    };

                    string qrCodePath = Path.Combine(BarcodesDirectoryPath, $"{productName}{PNG_EXTENSION}");
                    Bitmap qrCode = barcodeWriter.Write(stringToEncode);
                    qrCode.Save(qrCodePath);

                    // Adding the new item to DB.
                    itemInfo = new ItemInfo(
                         stringToEncode, maxHittingTimeInSeconds, productName);
                    mItemsDictionary.Add(stringToEncode, itemInfo);
                    mLogger.WriteLine($"{itemInfo.ItemName} added to database");

                    // Remember the recognition algorithm and item category.
                    SaveDatabases(itemInfo, recognitionType, category);
               }
               else
               {
                    mLogger.WriteError($@"One of more of the next variables is invalid:
{nameof(productName)}:{productName}
{nameof(maxHittingTimeInSeconds)}:{maxHittingTimeInSeconds}
{nameof(recognitionType)}:{recognitionType}
{nameof(category)}:{category}");
               }

               return itemInfo;
          }

          public IItemInfo ScanExistingBarcode(string barcodeImageName)
          {
               IItemInfo item = null;

               string barcodeImagePath = Path.Combine(BarcodesDirectoryPath, barcodeImageName);
               if (File.Exists(barcodeImagePath))
               {
                    // Loads the qrCode as a bitmap and decodes it into a text.
                    BarcodeReader barcodeReader = new BarcodeReader();
                    Bitmap qrCodeToRead = new Bitmap(barcodeImagePath);
                    Result readResult = barcodeReader.Decode(qrCodeToRead);

                    // Finds the relevant item by the encode text.
                    try
                    {
                         item = mItemsDictionary[readResult.Text];
                    }
                    catch(KeyNotFoundException e)
                    {
                         mLogger.WriteError($"{barcodeImageName} is not in the Datebase", e);
                    }
               }
               else
               {
                    mLogger.WriteError($"{barcodeImageName} does not exist in {BarcodesDirectoryPath}");
               }

               return item;
          }

          private void LoadDatabases()
          {
               // Saves items.
               mItemsDictionary = SerializationMachine.LoadDictionaryFromDB<string, IItemInfo>(mItemsDBPath);

               // Load ItemToRecognizerDataMap.
               ItemToRecognizeDataMap.LoadDatabases();
          }

          private void SaveDatabases(IItemInfo itemInfo, string recognitionType, string category)
          {
               string databaseCategoryDirectory = Path.Combine(mDatabaseDirectoryPath, recognitionType, category);
               Directory.CreateDirectory(databaseCategoryDirectory);

               ItemToRecognizeDataMap.Add(itemInfo, recognitionType, category);
               ItemToRecognizeDataMap.SaveDatabases();

               SerializationMachine.SaveDictionaryIntoDB(mItemsDBPath, mItemsDictionary);
          }

          // Obsolete methods.

          public IItemInfo Scan_obsolete()
          {
               IItemInfo item = null;

               ShowScanManu();
               ShowBarcodesAvailable();
               string userInput = Console.ReadLine();

               switch (userInput.ToLowerInvariant())
               {
                    //case "create":
                    //case "1":
                    //     item = CreateNewBarcode();
                    //     break;
                    //case "scan":
                    //case "2":
                    //     item = ScanExistingBarcode();
                    //     break;
                    //case "exit":
                    //     break;
                    //default:
                    //     mLogger.WriteLine("Invalid input");
                    //     break;
               }

               return item;
          }

          private IItemInfo CreateNewBarcode_obsolete()
          {
               // Created barcodes directory in case there is no existing one.
               Directory.CreateDirectory(BarcodesDirectoryPath);

               // Gets from user: product name and max hitting time for product.
               mLogger.WriteLine("Type the name of the product to register");
               string productName = Console.ReadLine();

               mLogger.WriteLine("Type number of maximal saftey hitting time in seconds ");
               string userInput = Console.ReadLine();
               int maxHittingTimeInSeconds;
               while (!int.TryParse(userInput, out maxHittingTimeInSeconds))
               {
                    mLogger.WriteError($"{userInput} is invalid input, type again");
                    userInput = Console.ReadLine();
               }

               // Generate uniqe string and creates QR-Code from it.
               Guid guid = Guid.NewGuid();
               mLogger.WriteLine($"Generating string to encode: {guid}");
               string stringToEncode = guid.ToString();

               BarcodeWriter barcodeWriter = new BarcodeWriter
               {
                    Format = BarcodeFormat.QR_CODE
               };

               string qrCodePath = Path.Combine(BarcodesDirectoryPath, $"{productName}{PNG_EXTENSION}");
               Bitmap qrCode = barcodeWriter.Write(stringToEncode);
               qrCode.Save(qrCodePath);

               // Adding the new item to DB.
               IItemInfo itemInfo = new ItemInfo(
                    stringToEncode, maxHittingTimeInSeconds, productName);
               mItemsDictionary.Add(stringToEncode, itemInfo);
               mLogger.WriteLine($"{itemInfo.ItemName} added to database");

               // User decides the recognition algorithm and the item category.
               string recognitionType = ClassifyItemToRecognitionType_obsolete(itemInfo);
               string category = ClassifyItemToCategory(itemInfo);

               // Remember the recognition algorithm and item category.
               SaveDatabases(itemInfo, recognitionType, category);

               return itemInfo;
          }

          /// <summary>
          /// The user decide if the recognition process is for "specific sound" or "popcorn".
          /// </summary>
          /// <param name="itemInfo"></param>
          /// <returns></returns>
          private string ClassifyItemToRecognitionType_obsolete(IItemInfo itemInfo)
          {
               mLogger.WriteLine($"Write which recognition type should handle {itemInfo.ItemName}");
               mLogger.WriteLine($"Type 1 for {ItemToRecognizeDataMap.RecognizerType[1]}");
               mLogger.WriteLine($"Type 2 for {ItemToRecognizeDataMap.RecognizerType[2]}");
               string recognitionTypeNum = Console.ReadLine();

               while ((recognitionTypeNum != "1") && (recognitionTypeNum != "2"))
               {
                    mLogger.WriteLine("Invalid input, type again");
                    recognitionTypeNum = Console.ReadLine();
               }

               return ItemToRecognizeDataMap.RecognizerType[int.Parse(recognitionTypeNum)];
          }

          private string ClassifyItemToCategory(IItemInfo itemInfo)
          {
               ItemToRecognizeDataMap.PrintCategories();
               bool isClassificationFinished = false;
               string category = "Unclassified";

               while (!isClassificationFinished)
               {
                    mLogger.WriteLine($"Write which Category {itemInfo.ItemName} suits or write new category");
                    category = Console.ReadLine().ToLower();

                    if (!ItemToRecognizeDataMap.GetCategories().Contains(category))
                    {
                         mLogger.WriteLine($"You are adding new category: {category}. Are you sure? Y/N");
                         string userInput = Console.ReadLine();
                         switch (userInput.ToLower())
                         {
                              case "y":
                              case "yes":
                                   // Finishes method.
                                   isClassificationFinished = true;
                                   break;
                              case "n":
                              case "no":
                                   // Returns to beggining of while loop.
                                   break;
                              default:
                                   // Returns to beggining of while loop.
                                   mLogger.WriteLine("Invalid input");
                                   break;
                         }
                    }
                    else
                    {
                         // Category already exist.
                         isClassificationFinished = true;
                    }
               }

               return category;
          }

          private void ShowBarcodesAvailable()
          {
               StringBuilder stringBuilder = new StringBuilder();
               if (Directory.Exists(BarcodesDirectoryPath))
               {
                    List<FilePath> files = FilePath.GetFiles(BarcodesDirectoryPath);
                    if (files.Count > 0)
                    {
                         stringBuilder.Append("Barcodes available:");
                         stringBuilder.Append(Environment.NewLine);
                         foreach (FilePath qrBarcodeImagePath in files)
                         {
                              stringBuilder.Append(qrBarcodeImagePath.Name);
                              stringBuilder.Append(Environment.NewLine);
                         }
                    }
                    else
                    {
                         stringBuilder.Append("no barcodes available");
                    }
               }
               else
               {
                    stringBuilder.Append("no barcodes available");
               }

               mLogger.WriteLine(stringBuilder.ToString());
          }

          private IItemInfo ScanExistingBarcode_obsolete()
          {
               IItemInfo item = null;

               mLogger.WriteLine("Type name of QR-Barcode image to scan");
               string barcodeImageName = Console.ReadLine();
               string barcodeImagePath = Path.Combine(BarcodesDirectoryPath, barcodeImageName);
               if (File.Exists(barcodeImagePath))
               {
                    // Loads the qrCode as a bitmap and decodes it into a text.
                    BarcodeReader barcodeReader = new BarcodeReader();
                    Bitmap qrCodeToRead = new Bitmap(barcodeImagePath);
                    Result readResult = barcodeReader.Decode(qrCodeToRead);

                    // Finds the relevant item by the encode text.
                    item = mItemsDictionary[readResult.Text];
               }
               else
               {
                    mLogger.WriteError($"{barcodeImageName} does not exist in {BarcodesDirectoryPath}");
               }

               return item;
          }
     }
}
