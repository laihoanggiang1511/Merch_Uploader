namespace UploadTemplate
{
    partial class TrademarkView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grid_Trademark = new System.Windows.Forms.DataGridView();
            this.Column_SerialNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Trademark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_FiledOn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_RegisteredOn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkbox_LiveAndText = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Trademark)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grid_Trademark
            // 
            this.grid_Trademark.AllowUserToAddRows = false;
            this.grid_Trademark.AllowUserToResizeColumns = false;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.AliceBlue;
            this.grid_Trademark.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle9;
            this.grid_Trademark.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid_Trademark.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid_Trademark.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.grid_Trademark.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_Trademark.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_SerialNumber,
            this.Column_Trademark,
            this.Column_Status,
            this.Column_Type,
            this.Column_FiledOn,
            this.Column_RegisteredOn});
            this.grid_Trademark.Dock = System.Windows.Forms.DockStyle.Top;
            this.grid_Trademark.Location = new System.Drawing.Point(5, 5);
            this.grid_Trademark.Margin = new System.Windows.Forms.Padding(5);
            this.grid_Trademark.Name = "grid_Trademark";
            this.grid_Trademark.RowHeadersWidth = 50;
            this.grid_Trademark.RowTemplate.Height = 24;
            this.grid_Trademark.Size = new System.Drawing.Size(640, 234);
            this.grid_Trademark.TabIndex = 2;
            // 
            // Column_SerialNumber
            // 
            this.Column_SerialNumber.HeaderText = "Serial Number";
            this.Column_SerialNumber.MinimumWidth = 6;
            this.Column_SerialNumber.Name = "Column_SerialNumber";
            // 
            // Column_Trademark
            // 
            this.Column_Trademark.HeaderText = "Trademark";
            this.Column_Trademark.MinimumWidth = 6;
            this.Column_Trademark.Name = "Column_Trademark";
            // 
            // Column_Status
            // 
            this.Column_Status.HeaderText = "Status";
            this.Column_Status.MinimumWidth = 6;
            this.Column_Status.Name = "Column_Status";
            // 
            // Column_Type
            // 
            this.Column_Type.HeaderText = "Type";
            this.Column_Type.MinimumWidth = 6;
            this.Column_Type.Name = "Column_Type";
            // 
            // Column_FiledOn
            // 
            this.Column_FiledOn.HeaderText = "Filed On";
            this.Column_FiledOn.MinimumWidth = 6;
            this.Column_FiledOn.Name = "Column_FiledOn";
            // 
            // Column_RegisteredOn
            // 
            this.Column_RegisteredOn.HeaderText = "Registered On";
            this.Column_RegisteredOn.MinimumWidth = 6;
            this.Column_RegisteredOn.Name = "Column_RegisteredOn";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkbox_LiveAndText);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(5, 247);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(640, 61);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // checkbox_LiveAndText
            // 
            this.checkbox_LiveAndText.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkbox_LiveAndText.AutoSize = true;
            this.checkbox_LiveAndText.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkbox_LiveAndText.Location = new System.Drawing.Point(387, 18);
            this.checkbox_LiveAndText.MinimumSize = new System.Drawing.Size(250, 30);
            this.checkbox_LiveAndText.Name = "checkbox_LiveAndText";
            this.checkbox_LiveAndText.Size = new System.Drawing.Size(250, 40);
            this.checkbox_LiveAndText.TabIndex = 5;
            this.checkbox_LiveAndText.Text = "Show only \"Live\" and \"Text/Typeset\"";
            this.checkbox_LiveAndText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkbox_LiveAndText.UseVisualStyleBackColor = true;
            this.checkbox_LiveAndText.CheckedChanged += new System.EventHandler(this.checkbox_LiveAndText_CheckedChanged);
            // 
            // TrademarkView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 313);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grid_Trademark);
            this.Name = "TrademarkView";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TrademarkView";
            this.Load += new System.EventHandler(this.TrademarkView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grid_Trademark)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grid_Trademark;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_SerialNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Trademark;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_FiledOn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_RegisteredOn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkbox_LiveAndText;
    }
}