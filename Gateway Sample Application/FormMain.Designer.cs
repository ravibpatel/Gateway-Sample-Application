namespace Gateway_Sample_Application
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxNumber = new System.Windows.Forms.TextBox();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.numericUpDownMessagesCount = new System.Windows.Forms.NumericUpDown();
            this.buttonSendMultiple = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageSender = new System.Windows.Forms.TabPage();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.propertyGridSettings = new System.Windows.Forms.PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMessagesCount)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabPageSender.SuspendLayout();
            this.tabPageSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxNumber
            // 
            this.textBoxNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNumber.Location = new System.Drawing.Point(8, 6);
            this.textBoxNumber.Name = "textBoxNumber";
            this.textBoxNumber.Size = new System.Drawing.Size(537, 20);
            this.textBoxNumber.TabIndex = 0;
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMessage.Location = new System.Drawing.Point(8, 32);
            this.textBoxMessage.Multiline = true;
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new System.Drawing.Size(537, 198);
            this.textBoxMessage.TabIndex = 1;
            // 
            // buttonSend
            // 
            this.buttonSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSend.Location = new System.Drawing.Point(440, 236);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(105, 23);
            this.buttonSend.TabIndex = 3;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.ButtonSend_Click);
            // 
            // numericUpDownMessagesCount
            // 
            this.numericUpDownMessagesCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDownMessagesCount.Location = new System.Drawing.Point(8, 239);
            this.numericUpDownMessagesCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMessagesCount.Name = "numericUpDownMessagesCount";
            this.numericUpDownMessagesCount.Size = new System.Drawing.Size(60, 20);
            this.numericUpDownMessagesCount.TabIndex = 4;
            this.numericUpDownMessagesCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // buttonSendMultiple
            // 
            this.buttonSendMultiple.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSendMultiple.Location = new System.Drawing.Point(329, 236);
            this.buttonSendMultiple.Name = "buttonSendMultiple";
            this.buttonSendMultiple.Size = new System.Drawing.Size(105, 23);
            this.buttonSendMultiple.TabIndex = 5;
            this.buttonSendMultiple.Text = "Send Multiple";
            this.buttonSendMultiple.UseVisualStyleBackColor = true;
            this.buttonSendMultiple.Click += new System.EventHandler(this.ButtonSendMultiple_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageSender);
            this.tabControl.Controls.Add(this.tabPageSettings);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(561, 293);
            this.tabControl.TabIndex = 6;
            // 
            // tabPageSender
            // 
            this.tabPageSender.Controls.Add(this.textBoxNumber);
            this.tabPageSender.Controls.Add(this.numericUpDownMessagesCount);
            this.tabPageSender.Controls.Add(this.buttonSendMultiple);
            this.tabPageSender.Controls.Add(this.textBoxMessage);
            this.tabPageSender.Controls.Add(this.buttonSend);
            this.tabPageSender.Location = new System.Drawing.Point(4, 22);
            this.tabPageSender.Name = "tabPageSender";
            this.tabPageSender.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSender.Size = new System.Drawing.Size(553, 267);
            this.tabPageSender.TabIndex = 0;
            this.tabPageSender.Text = "Sender";
            this.tabPageSender.UseVisualStyleBackColor = true;
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.Controls.Add(this.propertyGridSettings);
            this.tabPageSettings.Location = new System.Drawing.Point(4, 22);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSettings.Size = new System.Drawing.Size(553, 267);
            this.tabPageSettings.TabIndex = 1;
            this.tabPageSettings.Text = "Settings";
            this.tabPageSettings.UseVisualStyleBackColor = true;
            // 
            // propertyGridSettings
            // 
            this.propertyGridSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridSettings.Location = new System.Drawing.Point(3, 3);
            this.propertyGridSettings.Name = "propertyGridSettings";
            this.propertyGridSettings.Size = new System.Drawing.Size(547, 261);
            this.propertyGridSettings.TabIndex = 0;
            this.propertyGridSettings.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.PropertyGridSettings_PropertyValueChanged);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 293);
            this.Controls.Add(this.tabControl);
            this.Name = "FormMain";
            this.ShowIcon = false;
            this.Text = "Gateway Sample App";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMessagesCount)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabPageSender.ResumeLayout(false);
            this.tabPageSender.PerformLayout();
            this.tabPageSettings.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxNumber;
        private System.Windows.Forms.TextBox textBoxMessage;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.NumericUpDown numericUpDownMessagesCount;
        private System.Windows.Forms.Button buttonSendMultiple;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageSender;
        private System.Windows.Forms.TabPage tabPageSettings;
        private System.Windows.Forms.PropertyGrid propertyGridSettings;
    }
}

