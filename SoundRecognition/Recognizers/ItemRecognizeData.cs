using System;

namespace SoundRecognition
{
    [Serializable]
    internal class ItemRecognizeData
    {
        // Identifies the recognition type of the item (example: recognition by specific sound).
        public string RecognizerType { get; set; }

        // The catagory of the Item (example: Popcorn, ManaHama, Chips..). That is free user's input.
        public string Category { get; set; }

        public ItemRecognizeData(string recognizerType, string category)
        {
            RecognizerType = recognizerType;
            Category = category;
        }
    }
}
