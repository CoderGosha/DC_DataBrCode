namespace DataBarCode
{
    partial class LogUpload
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс  следует удалить; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogUpload));
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.buttonUpload = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelLog = new System.Windows.Forms.Label();
            this.buttonCleanScreen = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonUpload
            // 
            this.buttonUpload.BackColor = System.Drawing.Color.Azure;
            this.buttonUpload.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.buttonUpload.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.buttonUpload.Location = new System.Drawing.Point(26, 65);
            this.buttonUpload.Name = "buttonUpload";
            this.buttonUpload.Size = new System.Drawing.Size(186, 26);
            this.buttonUpload.TabIndex = 28;
            this.buttonUpload.Text = "Выгрузить ";
            this.buttonUpload.Click += new System.EventHandler(this.buttonUpload_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(214, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(23, 23);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(209, 23);
            this.label3.Text = "Логирование";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelVersion
            // 
            this.labelVersion.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.labelVersion.ForeColor = System.Drawing.Color.White;
            this.labelVersion.Location = new System.Drawing.Point(3, 33);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(232, 23);
            this.labelVersion.Text = "Версия ПО:";
            // 
            // labelLog
            // 
            this.labelLog.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.labelLog.ForeColor = System.Drawing.Color.White;
            this.labelLog.Location = new System.Drawing.Point(3, 139);
            this.labelLog.Name = "labelLog";
            this.labelLog.Size = new System.Drawing.Size(232, 23);
            this.labelLog.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // buttonCleanScreen
            // 
            this.buttonCleanScreen.BackColor = System.Drawing.Color.Azure;
            this.buttonCleanScreen.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.buttonCleanScreen.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.buttonCleanScreen.Location = new System.Drawing.Point(26, 99);
            this.buttonCleanScreen.Name = "buttonCleanScreen";
            this.buttonCleanScreen.Size = new System.Drawing.Size(186, 26);
            this.buttonCleanScreen.TabIndex = 29;
            this.buttonCleanScreen.Text = "Clean ScreenShot";
            this.buttonCleanScreen.Click += new System.EventHandler(this.buttonCleanScreen_Click);
            // 
            // LogUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.MediumAquamarine;
            this.ClientSize = new System.Drawing.Size(238, 295);
            this.ControlBox = false;
            this.Controls.Add(this.buttonCleanScreen);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.labelLog);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonUpload);
            this.KeyPreview = true;
            this.Name = "LogUpload";
            this.Text = "LogUpload";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LogUpload_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonUpload;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelLog;
        private System.Windows.Forms.Button buttonCleanScreen;
    }
}