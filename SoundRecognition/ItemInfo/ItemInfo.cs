using System;

namespace SoundRecognition
{
     [Serializable]
     class ItemInfo : IItemInfo
     {
          public string Barcode { get; private set; }
          public int MaxHittingTimeInSeconds { get; private set; }
          public string ItemName { get; private set; }
          public static ItemInfo DefaultItem =
               new ItemInfo("", Machine.MaximalWorkingTimeInMS, "Default Item");

          public ItemInfo(string barcode, int hittingTimeInSec, string itemName)
          {
               Barcode = barcode;
               MaxHittingTimeInSeconds = hittingTimeInSec;
               ItemName = itemName;
          }
     }
}
