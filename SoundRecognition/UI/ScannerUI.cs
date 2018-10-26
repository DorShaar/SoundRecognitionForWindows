using System.Drawing;
using System.Windows.Forms;

namespace SoundRecognition
{
     public partial class ScannerUI : Form
     {
          private int mDisplayedImageIndex = 0;
          public event AddNewItem OnAddNewBarcode;
          public event ScanExistingItem OnScanBarcode;

          public ScannerUI()
          {
               InitializeComponent();
               BarcodePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
          }

          public void SetBarcodesImageList(string imagesDirectory)
          {
               foreach (FilePath imageFilePath in FilePath.GetFiles(imagesDirectory))
                    ImageList.Images.Add(imageFilePath.Name, new Bitmap(imageFilePath.FileFullPath));

               ImageList.ImageSize = new Size(256, 256);

               ShowCurrentPicture();
          }

          private void AddNewItemButton_Click(object sender, System.EventArgs e)
          {
               using (ScannerUserInput scannerUserInputDialog = new ScannerUserInput())
               {
                    scannerUserInputDialog.ShowDialog();
                    string newBarcodeImagePath = OnAddNewBarcode.Invoke(
                         scannerUserInputDialog.NewProductName,
                         scannerUserInputDialog.MaximalHittingTimeInSec,
                         scannerUserInputDialog.RecognitionType,
                         scannerUserInputDialog.Category);

                    ImageList.Images.Add(
                         scannerUserInputDialog.NewProductName,
                         new Bitmap(newBarcodeImagePath));
               }
          }

          private void ScanBarcodeButton_Click(object sender, System.EventArgs e)
          {
               OnScanBarcode?.Invoke(BarcodeNameLable.Text);
               Close();
          }

          private void NextImageButton_Click(object sender, System.EventArgs e)
          {
               mDisplayedImageIndex++;
               if (mDisplayedImageIndex >= ImageList.Images.Count)
                    mDisplayedImageIndex = 0;

               ShowCurrentPicture();
          }

          private void PreviousImageButton_Click(object sender, System.EventArgs e)
          {
               mDisplayedImageIndex--;
               if (mDisplayedImageIndex < 0)
                    mDisplayedImageIndex = ImageList.Images.Count - 1;

               ShowCurrentPicture();
          }

          private void ShowCurrentPicture()
          {
               if (ImageList.Images.Count > 0)
               {
                    BarcodePictureBox.Image = ImageList.Images[mDisplayedImageIndex];
                    BarcodeNameLable.Text = ImageList.Images.Keys[mDisplayedImageIndex];
               }
          }

          private void CloseButton_Click(object sender, System.EventArgs e)
          {
               Close();
          }
     }
}
