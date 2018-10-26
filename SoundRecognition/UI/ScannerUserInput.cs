using System.Windows.Forms;

namespace SoundRecognition
{
     public partial class ScannerUserInput : Form
     {
          public string NewProductName { get; private set; }
          public int MaximalHittingTimeInSec { get; private set; }
          public string RecognitionType { get; private set; }
          public string Category { get; private set; }

          public ScannerUserInput()
          {
               InitializeComponent();
          }

          private void ProductNameTextBox_TextChanged(object sender, System.EventArgs e)
          {
               NewProductName = ProductNameTextBox.Text;
          }

          private void MaxHitTimeTextBoxUpDown_ValueChanged(object sender, System.EventArgs e)
          {
               MaximalHittingTimeInSec = (int)MaxHitTimeTextBoxUpDown.Value;
          }

          private void RecognitionTypeComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
          {
               RecognitionType = RecognitionTypeComboBox.SelectedItem as string;
          }

          private void CategoryTextBox_TextChanged(object sender, System.EventArgs e)
          {
               Category = CategoryTextBox.Text;
          }

          private void AddButton_Click(object sender, System.EventArgs e)
          {
               Close();
          }
     }
}
