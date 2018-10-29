using System;
using System.Windows.Forms;

namespace SoundRecognition
{
     public static class ThreadHelper
     {
          delegate void SetTextCallback(Form form, Control control, string text);
          delegate void SetTextToRichBoxCallback(Form form, RichTextBox richTextBox, string text);
          delegate void CloseFormCallback(Form form);
          delegate void SetEnabledPropertyCallback(Form form, Control control, bool isEnabled);

          /// <summary>
          /// Set text property of various controls
          /// </summary>
          /// <param name="form">The calling form</param>
          /// <param name="control"></param>
          /// <param name="text"></param>
          public static void SetText(Form form, Control control, string text)
          {
               // InvokeRequired required compares the thread ID of the 
               // calling thread to the thread ID of the creating thread. 
               // If these threads are different, it returns true. 
               if (control.InvokeRequired)
               {
                    SetTextCallback setTextcallbackFunction = new SetTextCallback(SetText);
                    form.Invoke(setTextcallbackFunction, new object[] { form, control, text });
               }
               else
               {
                    control.Text = text;
               }
          }

          public static void AppendTextToRichTextBox(Form form, RichTextBox richTextBox, string text)
          {
               // InvokeRequired required compares the thread ID of the 
               // calling thread to the thread ID of the creating thread. 
               // If these threads are different, it returns true. 
               if (richTextBox.InvokeRequired)
               {
                    // TODO Fix log here.
                    //SetTextToRichBoxCallback setTextcallbackFunction = new SetTextToRichBoxCallback(AppendTextToRichTextBox);
                    //form.Invoke(setTextcallbackFunction, new object[] { form, richTextBox, text });
               }
               else
               {
                    richTextBox.AppendText(text);
                    richTextBox.AppendText(Environment.NewLine + Environment.NewLine);
               }
          }

          public static void SetEnabledProperty(Form form, Control control, bool isEnabled)
          {
               if (control.InvokeRequired)
               {
                    SetEnabledPropertyCallback setTextcallbackFunction = new SetEnabledPropertyCallback(SetEnabledProperty);
                    form.Invoke(setTextcallbackFunction, new object[] { form, control, isEnabled });
               }
               else
               {
                    control.Enabled = isEnabled;
               }
          }

          public static void CloseForm(Form form)
          {
               if (form.InvokeRequired)
               {
                    CloseFormCallback closeFormCallbackFunction = new CloseFormCallback(CloseForm);
                    form.Invoke(closeFormCallbackFunction, new object[] { form });
               }
               else
               {
                    form.Close();
                    form.Dispose();
               }
          }
     }
}
