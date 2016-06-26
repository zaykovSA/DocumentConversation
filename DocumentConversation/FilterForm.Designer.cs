namespace DocumentConversation
{
    partial class FilterForm
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
            this.docNameTB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.docNumberTB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dateStart = new System.Windows.Forms.DateTimePicker();
            this.dateEnd = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.docUploaderCB = new System.Windows.Forms.ComboBox();
            this.docDepCB = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.docClientCB = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.docTypeCB = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.UseDateCheckBox = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // docNameTB
            // 
            this.docNameTB.Location = new System.Drawing.Point(15, 25);
            this.docNameTB.Name = "docNameTB";
            this.docNameTB.Size = new System.Drawing.Size(205, 20);
            this.docNameTB.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(178, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Укажите название или его часть:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(162, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Укажите номер или его часть:";
            // 
            // docNumberTB
            // 
            this.docNumberTB.Location = new System.Drawing.Point(15, 64);
            this.docNumberTB.Name = "docNumberTB";
            this.docNumberTB.Size = new System.Drawing.Size(205, 20);
            this.docNumberTB.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Выберите период:";
            // 
            // dateStart
            // 
            this.dateStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateStart.Location = new System.Drawing.Point(44, 103);
            this.dateStart.Name = "dateStart";
            this.dateStart.Size = new System.Drawing.Size(85, 20);
            this.dateStart.TabIndex = 5;
            // 
            // dateEnd
            // 
            this.dateEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateEnd.Location = new System.Drawing.Point(135, 103);
            this.dateEnd.Name = "dateEnd";
            this.dateEnd.Size = new System.Drawing.Size(85, 20);
            this.dateEnd.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(208, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Выберите загрузившего пользователя:";
            // 
            // docUploaderCB
            // 
            this.docUploaderCB.FormattingEnabled = true;
            this.docUploaderCB.Location = new System.Drawing.Point(15, 142);
            this.docUploaderCB.Name = "docUploaderCB";
            this.docUploaderCB.Size = new System.Drawing.Size(205, 21);
            this.docUploaderCB.TabIndex = 8;
            // 
            // docDepCB
            // 
            this.docDepCB.FormattingEnabled = true;
            this.docDepCB.Location = new System.Drawing.Point(251, 24);
            this.docDepCB.Name = "docDepCB";
            this.docDepCB.Size = new System.Drawing.Size(183, 21);
            this.docDepCB.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(248, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Выберите отдел:";
            // 
            // docClientCB
            // 
            this.docClientCB.FormattingEnabled = true;
            this.docClientCB.Location = new System.Drawing.Point(251, 63);
            this.docClientCB.Name = "docClientCB";
            this.docClientCB.Size = new System.Drawing.Size(183, 21);
            this.docClientCB.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(248, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Выберите клиента:";
            // 
            // docTypeCB
            // 
            this.docTypeCB.FormattingEnabled = true;
            this.docTypeCB.Location = new System.Drawing.Point(251, 103);
            this.docTypeCB.Name = "docTypeCB";
            this.docTypeCB.Size = new System.Drawing.Size(183, 21);
            this.docTypeCB.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(248, 87);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(137, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Выберите тип документа:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(251, 131);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(89, 32);
            this.button1.TabIndex = 15;
            this.button1.Text = "Применить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // UseDateCheckBox
            // 
            this.UseDateCheckBox.AutoSize = true;
            this.UseDateCheckBox.Location = new System.Drawing.Point(22, 106);
            this.UseDateCheckBox.Name = "UseDateCheckBox";
            this.UseDateCheckBox.Size = new System.Drawing.Size(15, 14);
            this.UseDateCheckBox.TabIndex = 16;
            this.UseDateCheckBox.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(346, 131);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(89, 32);
            this.button2.TabIndex = 17;
            this.button2.Text = "Отменить";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FilterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 169);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.UseDateCheckBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.docTypeCB);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.docClientCB);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.docDepCB);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.docUploaderCB);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dateEnd);
            this.Controls.Add(this.dateStart);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.docNumberTB);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.docNameTB);
            this.Name = "FilterForm";
            this.Text = "Параметры фильтрации";
            this.Load += new System.EventHandler(this.FilterForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox docNameTB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox docNumberTB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateStart;
        private System.Windows.Forms.DateTimePicker dateEnd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox docUploaderCB;
        private System.Windows.Forms.ComboBox docDepCB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox docClientCB;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox docTypeCB;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox UseDateCheckBox;
        private System.Windows.Forms.Button button2;
    }
}