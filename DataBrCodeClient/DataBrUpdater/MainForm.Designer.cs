namespace DataBrUpdater
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.buttonRelease = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonDevelop = new System.Windows.Forms.Button();
            this.buttonClean = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.labelloading = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonRelease
            // 
            this.buttonRelease.BackColor = System.Drawing.Color.Azure;
            this.buttonRelease.Enabled = false;
            this.buttonRelease.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.buttonRelease.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.buttonRelease.Location = new System.Drawing.Point(36, 90);
            this.buttonRelease.Name = "buttonRelease";
            this.buttonRelease.Size = new System.Drawing.Size(160, 26);
            this.buttonRelease.TabIndex = 33;
            this.buttonRelease.Text = "Release";
            this.buttonRelease.Click += new System.EventHandler(this.buttonRelease_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(193, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(45, 42);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(16, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(171, 28);
            this.label1.Text = "Обновление";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // buttonDevelop
            // 
            this.buttonDevelop.BackColor = System.Drawing.Color.Azure;
            this.buttonDevelop.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.buttonDevelop.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.buttonDevelop.Location = new System.Drawing.Point(36, 178);
            this.buttonDevelop.Name = "buttonDevelop";
            this.buttonDevelop.Size = new System.Drawing.Size(160, 26);
            this.buttonDevelop.TabIndex = 38;
            this.buttonDevelop.Text = "Develop";
            this.buttonDevelop.Visible = false;
            this.buttonDevelop.Click += new System.EventHandler(this.buttonDevelop_Click);
            // 
            // buttonClean
            // 
            this.buttonClean.BackColor = System.Drawing.Color.Azure;
            this.buttonClean.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.buttonClean.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.buttonClean.Location = new System.Drawing.Point(36, 58);
            this.buttonClean.Name = "buttonClean";
            this.buttonClean.Size = new System.Drawing.Size(160, 26);
            this.buttonClean.TabIndex = 39;
            this.buttonClean.Text = "Clean";
            this.buttonClean.Click += new System.EventHandler(this.buttonLast_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.BackColor = System.Drawing.Color.Azure;
            this.buttonExit.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.buttonExit.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.buttonExit.Location = new System.Drawing.Point(36, 240);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(160, 26);
            this.buttonExit.TabIndex = 40;
            this.buttonExit.Text = "Exit";
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // labelloading
            // 
            this.labelloading.ForeColor = System.Drawing.Color.White;
            this.labelloading.Location = new System.Drawing.Point(0, 178);
            this.labelloading.Name = "labelloading";
            this.labelloading.Size = new System.Drawing.Size(235, 52);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.MediumAquamarine;
            this.ClientSize = new System.Drawing.Size(238, 295);
            this.ControlBox = false;
            this.Controls.Add(this.labelloading);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.buttonClean);
            this.Controls.Add(this.buttonDevelop);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonRelease);
            this.Name = "MainForm";
            this.Text = "DataBrUpdater";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonRelease;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonDevelop;
        private System.Windows.Forms.Button buttonClean;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Label labelloading;
    }
}

