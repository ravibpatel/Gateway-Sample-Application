using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gateway_Sample_Application.Properties;
using static SMS.API;

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
                //Dictionary<string, object>[] messages = SMS.API.SendMessageToContactsList(1, textBoxMessage.Text, SMS.API.Option.USE_SPECIFIED, new[] {"1"});
                long timestamp = (long) DateTime.UtcNow.AddMinutes(2).Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                SendSingleMessage(textBoxNumber.Text, textBoxMessage.Text, null, timestamp);
                //var contact = AddContact(1, textBoxNumber.Text);
                //contact = UnsubscribeContact(1, textBoxNumber.Text);
                //StringBuilder stringBuilder = new StringBuilder();
                //foreach (var key in contact.Keys)
                //{
                //    stringBuilder.AppendLine($"{key}=>{contact[key]}");
                //}
                //MessageBox.Show(stringBuilder.ToString());
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
                long timestamp = (long)DateTime.UtcNow.AddMinutes(2).Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                SendMessages(messages, Option.USE_SPECIFIED, null, timestamp);
                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "!Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PropertyGridSettings_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            Settings.Default.Save();
        }
    }
}
