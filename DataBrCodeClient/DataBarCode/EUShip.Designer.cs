namespace DataBarCode
{
    partial class EUShip
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EUShip));
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelBD = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer();
            this.dataGridEu = new System.Windows.Forms.DataGrid();
            this.dataGridTableStyleMain = new System.Windows.Forms.DataGridTableStyle();
            this.labelCaption = new System.Windows.Forms.Label();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonEUSearch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(215, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(23, 23);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
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
            // dataGridEu
            // 
            this.dataGridEu.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dataGridEu.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular);
            this.dataGridEu.Location = new System.Drawing.Point(3, 29);
            this.dataGridEu.Name = "dataGridEu";
            this.dataGridEu.RowHeadersVisible = false;
            this.dataGridEu.Size = new System.Drawing.Size(232, 217);
            this.dataGridEu.TabIndex = 4;
            this.dataGridEu.TableStyles.Add(this.dataGridTableStyleMain);
            // 
            // dataGridTableStyleMain
            // 
            this.dataGridTableStyleMain.MappingName = "EU";
            // 
            // labelCaption
            // 
            this.labelCaption.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.labelCaption.ForeColor = System.Drawing.Color.White;
            this.labelCaption.Location = new System.Drawing.Point(3, 0);
            this.labelCaption.Name = "labelCaption";
            this.labelCaption.Size = new System.Drawing.Size(192, 26);
            this.labelCaption.Text = "ТН: ";
            // 
            // buttonNext
            // 
            this.buttonNext.BackColor = System.Drawing.Color.Azure;
            this.buttonNext.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.buttonNext.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.buttonNext.Location = new System.Drawing.Point(35, 252);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(160, 26);
            this.buttonNext.TabIndex = 51;
            this.buttonNext.Text = "Подтвердить";
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonEUSearch
            // 
            this.buttonEUSearch.BackColor = System.Drawing.Color.Azure;
            this.buttonEUSearch.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.buttonEUSearch.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.buttonEUSearch.Location = new System.Drawing.Point(204, 252);
            this.buttonEUSearch.Name = "buttonEUSearch";
            this.buttonEUSearch.Size = new System.Drawing.Size(29, 26);
            this.buttonEUSearch.TabIndex = 55;
            this.buttonEUSearch.Text = "+";
            this.buttonEUSearch.Click += new System.EventHandler(this.buttonEUSearch_Click_1);
            // 
            // EUShip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.MediumAquamarine;
            this.ClientSize = new System.Drawing.Size(238, 295);
            this.ControlBox = false;
            this.Controls.Add(this.buttonEUSearch);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.labelCaption);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.labelBD);
            this.Controls.Add(this.dataGridEu);
            this.Name = "EUShip";
            this.Text = "Отгрузка";
            this.Closed += new System.EventHandler(this.EUShip_Closed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EUShip_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelBD;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGrid dataGridEu;
        private System.Windows.Forms.Label labelCaption;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.DataGridTableStyle dataGridTableStyleMain;
        private System.Windows.Forms.Button buttonEUSearch;
    }
}