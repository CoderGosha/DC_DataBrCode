namespace DataBarCode
{
    partial class ServiceFunc
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServiceFunc));
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.labelVersion = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonAutostart = new System.Windows.Forms.Button();
            this.labelAutoStart = new System.Windows.Forms.Label();
            this.labellnk = new System.Windows.Forms.Label();
            this.buttonLnk = new System.Windows.Forms.Button();
            this.labelApp = new System.Windows.Forms.Label();
            this.buttonAppOFF = new System.Windows.Forms.Button();
            this.buttonAppOn = new System.Windows.Forms.Button();
            this.labelStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelVersion
            // 
            this.labelVersion.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.labelVersion.ForeColor = System.Drawing.Color.White;
            this.labelVersion.Location = new System.Drawing.Point(2, 33);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(232, 23);
            this.labelVersion.Text = "Версия ПО:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(213, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(23, 23);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(2, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(209, 23);
            this.label3.Text = "Сервис";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // buttonAutostart
            // 
            this.buttonAutostart.BackColor = System.Drawing.Color.Azure;
            this.buttonAutostart.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.buttonAutostart.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.buttonAutostart.Location = new System.Drawing.Point(25, 131);
            this.buttonAutostart.Name = "buttonAutostart";
            this.buttonAutostart.Size = new System.Drawing.Size(186, 26);
            this.buttonAutostart.TabIndex = 32;
            this.buttonAutostart.Text = "Автозапуск";
            this.buttonAutostart.Click += new System.EventHandler(this.buttonAutostart_Click);
            // 
            // labelAutoStart
            // 
            this.labelAutoStart.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.labelAutoStart.ForeColor = System.Drawing.Color.White;
            this.labelAutoStart.Location = new System.Drawing.Point(3, 56);
            this.labelAutoStart.Name = "labelAutoStart";
            this.labelAutoStart.Size = new System.Drawing.Size(232, 23);
            this.labelAutoStart.Text = "Автостарт:";
            // 
            // labellnk
            // 
            this.labellnk.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.labellnk.ForeColor = System.Drawing.Color.White;
            this.labellnk.Location = new System.Drawing.Point(3, 79);
            this.labellnk.Name = "labellnk";
            this.labellnk.Size = new System.Drawing.Size(232, 23);
            this.labellnk.Text = "Ярлыки:";
            // 
            // buttonLnk
            // 
            this.buttonLnk.BackColor = System.Drawing.Color.Azure;
            this.buttonLnk.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.buttonLnk.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.buttonLnk.Location = new System.Drawing.Point(25, 163);
            this.buttonLnk.Name = "buttonLnk";
            this.buttonLnk.Size = new System.Drawing.Size(186, 26);
            this.buttonLnk.TabIndex = 35;
            this.buttonLnk.Text = "Ярлыки";
            this.buttonLnk.Click += new System.EventHandler(this.buttonLnk_Click);
            // 
            // labelApp
            // 
            this.labelApp.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.labelApp.ForeColor = System.Drawing.Color.White;
            this.labelApp.Location = new System.Drawing.Point(3, 102);
            this.labelApp.Name = "labelApp";
            this.labelApp.Size = new System.Drawing.Size(232, 23);
            this.labelApp.Text = "Other APP:";
            // 
            // buttonAppOFF
            // 
            this.buttonAppOFF.BackColor = System.Drawing.Color.Azure;
            this.buttonAppOFF.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.buttonAppOFF.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.buttonAppOFF.Location = new System.Drawing.Point(25, 195);
            this.buttonAppOFF.Name = "buttonAppOFF";
            this.buttonAppOFF.Size = new System.Drawing.Size(186, 26);
            this.buttonAppOFF.TabIndex = 43;
            this.buttonAppOFF.Text = "App OFF";
            this.buttonAppOFF.Click += new System.EventHandler(this.buttonAppOFF_Click);
            // 
            // buttonAppOn
            // 
            this.buttonAppOn.BackColor = System.Drawing.Color.Azure;
            this.buttonAppOn.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.buttonAppOn.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.buttonAppOn.Location = new System.Drawing.Point(25, 227);
            this.buttonAppOn.Name = "buttonAppOn";
            this.buttonAppOn.Size = new System.Drawing.Size(186, 26);
            this.buttonAppOn.TabIndex = 44;
            this.buttonAppOn.Text = "App ON";
            this.buttonAppOn.Click += new System.EventHandler(this.buttonAppOn_Click);
            // 
            // labelStatus
            // 
            this.labelStatus.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.labelStatus.ForeColor = System.Drawing.Color.White;
            this.labelStatus.Location = new System.Drawing.Point(4, 260);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(232, 23);
            this.labelStatus.Text = "Status";
            // 
            // ServiceFunc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.MediumAquamarine;
            this.ClientSize = new System.Drawing.Size(238, 295);
            this.ControlBox = false;
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.buttonAppOn);
            this.Controls.Add(this.buttonAppOFF);
            this.Controls.Add(this.labelApp);
            this.Controls.Add(this.buttonLnk);
            this.Controls.Add(this.labellnk);
            this.Controls.Add(this.labelAutoStart);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonAutostart);
            this.KeyPreview = true;
            this.Name = "ServiceFunc";
            this.Text = "ServiceFunc";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ServiceFunc_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonAutostart;
        private System.Windows.Forms.Label labelAutoStart;
        private System.Windows.Forms.Label labellnk;
        private System.Windows.Forms.Button buttonLnk;
        private System.Windows.Forms.Label labelApp;
        private System.Windows.Forms.Button buttonAppOFF;
        private System.Windows.Forms.Button buttonAppOn;
        private System.Windows.Forms.Label labelStatus;
    }
}