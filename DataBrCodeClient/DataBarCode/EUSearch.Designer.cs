namespace DataBarCode
{
    partial class EUSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EUSearch));
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.labelMX = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBoxScan = new System.Windows.Forms.TextBox();
            this.dataGridEu = new System.Windows.Forms.DataGrid();
            this.dataGridTableStyleMain = new System.Windows.Forms.DataGridTableStyle();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelMX
            // 
            this.labelMX.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.labelMX.ForeColor = System.Drawing.Color.White;
            this.labelMX.Location = new System.Drawing.Point(30, 2);
            this.labelMX.Name = "labelMX";
            this.labelMX.Size = new System.Drawing.Size(180, 21);
            this.labelMX.Text = "Введите ЕУ:";
            this.labelMX.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(215, -2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(23, 23);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // textBoxScan
            // 
            this.textBoxScan.Location = new System.Drawing.Point(5, 26);
            this.textBoxScan.Name = "textBoxScan";
            this.textBoxScan.Size = new System.Drawing.Size(157, 23);
            this.textBoxScan.TabIndex = 30;
            // 
            // dataGridEu
            // 
            this.dataGridEu.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dataGridEu.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular);
            this.dataGridEu.GridLineColor = System.Drawing.SystemColors.AppWorkspace;
            this.dataGridEu.Location = new System.Drawing.Point(5, 53);
            this.dataGridEu.Name = "dataGridEu";
            this.dataGridEu.RowHeadersVisible = false;
            this.dataGridEu.Size = new System.Drawing.Size(230, 207);
            this.dataGridEu.TabIndex = 31;
            this.dataGridEu.TableStyles.Add(this.dataGridTableStyleMain);
            this.dataGridEu.CurrentCellChanged += new System.EventHandler(this.dataGridEu_CurrentCellChanged);
            // 
            // dataGridTableStyleMain
            // 
            this.dataGridTableStyleMain.MappingName = "EU";
            // 
            // buttonNext
            // 
            this.buttonNext.BackColor = System.Drawing.Color.Azure;
            this.buttonNext.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonNext.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.buttonNext.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.buttonNext.Location = new System.Drawing.Point(39, 264);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(160, 26);
            this.buttonNext.TabIndex = 51;
            this.buttonNext.Text = "Подтвердить";
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonSearch
            // 
            this.buttonSearch.BackColor = System.Drawing.Color.Azure;
            this.buttonSearch.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular);
            this.buttonSearch.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.buttonSearch.Location = new System.Drawing.Point(168, 26);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(65, 23);
            this.buttonSearch.TabIndex = 52;
            this.buttonSearch.Text = "Поиск";
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // EUSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.MediumAquamarine;
            this.ClientSize = new System.Drawing.Size(238, 295);
            this.ControlBox = false;
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.dataGridEu);
            this.Controls.Add(this.textBoxScan);
            this.Controls.Add(this.labelMX);
            this.Controls.Add(this.pictureBox1);
            this.Name = "EUSearch";
            this.Text = "Поиск ЕУ";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EUSearch_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelMX;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBoxScan;
        public System.Windows.Forms.DataGrid dataGridEu;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.DataGridTableStyle dataGridTableStyleMain;
    }
}