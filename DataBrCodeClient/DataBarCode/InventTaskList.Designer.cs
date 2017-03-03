﻿namespace DataBarCode
{
    partial class InventTaskList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InventTaskList));
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.labelBD = new System.Windows.Forms.Label();
            this.buttonNext = new System.Windows.Forms.Button();
            this.dataGridTask = new System.Windows.Forms.DataGrid();
            this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
            this.labelMX = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer();
            this.dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn2 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn3 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.SuspendLayout();
            // 
            // labelBD
            // 
            this.labelBD.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.labelBD.ForeColor = System.Drawing.Color.White;
            this.labelBD.Location = new System.Drawing.Point(2, 278);
            this.labelBD.Name = "labelBD";
            this.labelBD.Size = new System.Drawing.Size(235, 16);
            this.labelBD.Text = "БД: Нет данных. Операции: 0";
            // 
            // buttonNext
            // 
            this.buttonNext.BackColor = System.Drawing.Color.Azure;
            this.buttonNext.Enabled = false;
            this.buttonNext.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.buttonNext.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.buttonNext.Location = new System.Drawing.Point(38, 249);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(160, 26);
            this.buttonNext.TabIndex = 60;
            this.buttonNext.Text = "Подтвердить";
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click_1);
            // 
            // dataGridTask
            // 
            this.dataGridTask.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dataGridTask.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular);
            this.dataGridTask.GridLineColor = System.Drawing.SystemColors.AppWorkspace;
            this.dataGridTask.Location = new System.Drawing.Point(4, 26);
            this.dataGridTask.Name = "dataGridTask";
            this.dataGridTask.RowHeadersVisible = false;
            this.dataGridTask.Size = new System.Drawing.Size(230, 217);
            this.dataGridTask.TabIndex = 59;
            this.dataGridTask.TableStyles.Add(this.dataGridTableStyle1);
            this.dataGridTask.CurrentCellChanged += new System.EventHandler(this.dataGridTask_CurrentCellChanged_1);
            // 
            // dataGridTableStyle1
            // 
            this.dataGridTableStyle1.GridColumnStyles.Add(this.dataGridTextBoxColumn1);
            this.dataGridTableStyle1.GridColumnStyles.Add(this.dataGridTextBoxColumn2);
            this.dataGridTableStyle1.GridColumnStyles.Add(this.dataGridTextBoxColumn3);
            this.dataGridTableStyle1.MappingName = "TASK";
            // 
            // labelMX
            // 
            this.labelMX.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.labelMX.ForeColor = System.Drawing.Color.White;
            this.labelMX.Location = new System.Drawing.Point(0, 2);
            this.labelMX.Name = "labelMX";
            this.labelMX.Size = new System.Drawing.Size(209, 21);
            this.labelMX.Text = "Очередь заданий";
            this.labelMX.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(215, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(23, 23);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // dataGridTextBoxColumn1
            // 
            this.dataGridTextBoxColumn1.Format = "";
            this.dataGridTextBoxColumn1.FormatInfo = null;
            this.dataGridTextBoxColumn1.HeaderText = "Дата";
            this.dataGridTextBoxColumn1.MappingName = "RZDN_DATE";
            this.dataGridTextBoxColumn1.NullText = "-";
            this.dataGridTextBoxColumn1.Width = 140;
            // 
            // dataGridTextBoxColumn2
            // 
            this.dataGridTextBoxColumn2.Format = "";
            this.dataGridTextBoxColumn2.FormatInfo = null;
            this.dataGridTextBoxColumn2.HeaderText = "Описание";
            this.dataGridTextBoxColumn2.MappingName = "TS_NUM";
            this.dataGridTextBoxColumn2.NullText = "-";
            this.dataGridTextBoxColumn2.Width = 130;
            // 
            // dataGridTextBoxColumn3
            // 
            this.dataGridTextBoxColumn3.Format = "";
            this.dataGridTextBoxColumn3.FormatInfo = null;
            this.dataGridTextBoxColumn3.HeaderText = "Номер";
            this.dataGridTextBoxColumn3.MappingName = "RZDN_PRM";
            this.dataGridTextBoxColumn3.NullText = "-";
            this.dataGridTextBoxColumn3.Width = 80;
            // 
            // InventTaskList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.MediumAquamarine;
            this.ClientSize = new System.Drawing.Size(238, 295);
            this.ControlBox = false;
            this.Controls.Add(this.labelBD);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.dataGridTask);
            this.Controls.Add(this.labelMX);
            this.Controls.Add(this.pictureBox1);
            this.Name = "InventTaskList";
            this.Text = "Инвентаризация";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InventTaskList_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelBD;
        private System.Windows.Forms.Button buttonNext;
        public System.Windows.Forms.DataGrid dataGridTask;
        private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
        private System.Windows.Forms.Label labelMX;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn1;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn2;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn3;
    }
}