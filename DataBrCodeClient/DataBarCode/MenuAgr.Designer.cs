namespace DataBarCode
{
    partial class MenuAgr
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MenuAgr));
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelBD = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer();
            this.buttonapr = new System.Windows.Forms.Button();
            this.buttonpga = new System.Windows.Forms.Button();
            this.buttontgp = new System.Windows.Forms.Button();
            this.buttonT19 = new System.Windows.Forms.Button();
            this.buttonT50 = new System.Windows.Forms.Button();
            this.buttonT21 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(209, 23);
            this.label3.Text = "Агрегаты";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(215, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(23, 23);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // labelBD
            // 
            this.labelBD.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.labelBD.ForeColor = System.Drawing.Color.White;
            this.labelBD.Location = new System.Drawing.Point(0, 279);
            this.labelBD.Name = "labelBD";
            this.labelBD.Size = new System.Drawing.Size(235, 16);
            this.labelBD.Text = "БД: Нет данных. Операции: 0";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // buttonapr
            // 
            this.buttonapr.BackColor = System.Drawing.Color.Azure;
            this.buttonapr.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.buttonapr.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.buttonapr.Location = new System.Drawing.Point(26, 100);
            this.buttonapr.Name = "buttonapr";
            this.buttonapr.Size = new System.Drawing.Size(186, 26);
            this.buttonapr.TabIndex = 25;
            this.buttonapr.Text = "АПР-1550";
            this.buttonapr.Click += new System.EventHandler(this.buttonapr_Click);
            // 
            // buttonpga
            // 
            this.buttonpga.BackColor = System.Drawing.Color.Azure;
            this.buttonpga.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.buttonpga.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.buttonpga.Location = new System.Drawing.Point(26, 68);
            this.buttonpga.Name = "buttonpga";
            this.buttonpga.Size = new System.Drawing.Size(186, 26);
            this.buttonpga.TabIndex = 26;
            this.buttonpga.Text = "ПГА 2-8";
            this.buttonpga.Click += new System.EventHandler(this.buttonpga_Click);
            // 
            // buttontgp
            // 
            this.buttontgp.BackColor = System.Drawing.Color.Azure;
            this.buttontgp.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.buttontgp.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.buttontgp.Location = new System.Drawing.Point(26, 36);
            this.buttontgp.Name = "buttontgp";
            this.buttontgp.Size = new System.Drawing.Size(186, 26);
            this.buttontgp.TabIndex = 27;
            this.buttontgp.Text = "ТПГ 159";
            this.buttontgp.Click += new System.EventHandler(this.buttontgp_Click);
            // 
            // buttonT19
            // 
            this.buttonT19.BackColor = System.Drawing.Color.Azure;
            this.buttonT19.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.buttonT19.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.buttonT19.Location = new System.Drawing.Point(26, 134);
            this.buttonT19.Name = "buttonT19";
            this.buttonT19.Size = new System.Drawing.Size(186, 26);
            this.buttonT19.TabIndex = 28;
            this.buttonT19.Text = "ТЭСА 19-50";
            this.buttonT19.Click += new System.EventHandler(this.buttonT19_Click);
            // 
            // buttonT50
            // 
            this.buttonT50.BackColor = System.Drawing.Color.Azure;
            this.buttonT50.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.buttonT50.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.buttonT50.Location = new System.Drawing.Point(25, 166);
            this.buttonT50.Name = "buttonT50";
            this.buttonT50.Size = new System.Drawing.Size(186, 26);
            this.buttonT50.TabIndex = 29;
            this.buttonT50.Text = "ТЭСА 50-76";
            this.buttonT50.Click += new System.EventHandler(this.buttonT50_Click);
            // 
            // buttonT21
            // 
            this.buttonT21.BackColor = System.Drawing.Color.Azure;
            this.buttonT21.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.buttonT21.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.buttonT21.Location = new System.Drawing.Point(26, 198);
            this.buttonT21.Name = "buttonT21";
            this.buttonT21.Size = new System.Drawing.Size(186, 26);
            this.buttonT21.TabIndex = 30;
            this.buttonT21.Text = "ТЭСА 21-89";
            this.buttonT21.Click += new System.EventHandler(this.buttonT21_Click);
            // 
            // MenuAgr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.MediumAquamarine;
            this.ClientSize = new System.Drawing.Size(238, 295);
            this.ControlBox = false;
            this.Controls.Add(this.buttonT21);
            this.Controls.Add(this.buttonT50);
            this.Controls.Add(this.buttonT19);
            this.Controls.Add(this.buttontgp);
            this.Controls.Add(this.buttonpga);
            this.Controls.Add(this.buttonapr);
            this.Controls.Add(this.labelBD);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label3);
            this.Name = "MenuAgr";
            this.Text = "Агрегаты";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MenuAgr_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelBD;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button buttonapr;
        private System.Windows.Forms.Button buttonpga;
        private System.Windows.Forms.Button buttontgp;
        private System.Windows.Forms.Button buttonT19;
        private System.Windows.Forms.Button buttonT50;
        private System.Windows.Forms.Button buttonT21;
    }
}