using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gateway_Sample_Application.Properties;

namespace Gateway_Sample_Application
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            propertyGridSettings.SelectedObject = Settings.Default;
        }

        private void ButtonSend_Click(object sender, EventArgs e)
        {
            try
            {
                SMS.API.SendSingleMessage(textBoxNumber.Text, textBoxMessage.Text);
                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "!Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonSendMultiple_Click(object sender, EventArgs e)
        {
            List<Dictionary<string, string>> messages = new List<Dictionary<string, string>>();
            for (int i = 1; i <= numericUpDownMessagesCount.Value; i++)
            {
                var message = new Dictionary<string, string>
                {
                    { "number", textBoxNumber.Text },
                    { "message", textBoxMessage.Text }
                };
                messages.Add(message);
            }

            try
            {
                SMS.API.SendMessages(messages, false);
                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "!Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void propertyGridSettings_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            Settings.Default.Save();
        }
    }
}
