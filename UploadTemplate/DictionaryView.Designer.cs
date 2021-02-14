namespace UploadTemplate
{
    partial class DictionaryView
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
            this.grid_Dict = new System.Windows.Forms.DataGridView();
            this.Column_Key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_Remove = new System.Windows.Forms.Button();
            this.btn_Save = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Dict)).BeginInit();
            this.SuspendLayout();
            // 
            // grid_Dict
            // 
            this.grid_Dict.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_Dict.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_Key,
            this.Column_Value});
            this.grid_Dict.Location = new System.Drawing.Point(12, 12);
            this.grid_Dict.Name = "grid_Dict";
            this.grid_Dict.RowHeadersWidth = 51;
            this.grid_Dict.RowTemplate.Height = 24;
            this.grid_Dict.Size = new System.Drawing.Size(523, 171);
            this.grid_Dict.TabIndex = 0;
            // 
            // Column_Key
            // 
            this.Column_Key.HeaderText = "Key";
            this.Column_Key.MinimumWidth = 6;
            this.Column_Key.Name = "Column_Key";
            this.Column_Key.Width = 150;
            // 
            // Column_Value
            // 
            this.Column_Value.HeaderText = "Value";
            this.Column_Value.MinimumWidth = 6;
            this.Column_Value.Name = "Column_Value";
            this.Column_Value.Width = 300;
            // 
            // btn_Remove
            // 
            this.btn_Remove.Location = new System.Drawing.Point(242, 201);
            this.btn_Remove.Name = "btn_Remove";
            this.btn_Remove.Size = new System.Drawing.Size(92, 40);
            this.btn_Remove.TabIndex = 2;
            this.btn_Remove.Text = "Remove";
            this.btn_Remove.UseVisualStyleBackColor = true;
            this.btn_Remove.Click += new System.EventHandler(this.btn_Remove_Click);
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(340, 201);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(92, 40);
            this.btn_Save.TabIndex = 3;
            this.btn_Save.Text = "Save";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(438, 201);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(92, 40);
            this.btn_Cancel.TabIndex = 4;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // DictionaryView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(547, 253);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.btn_Remove);
            this.Controls.Add(this.grid_Dict);
            this.Name = "DictionaryView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dictionary Edit";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.DictionaryView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grid_Dict)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grid_Dict;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Key;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Value;
        private System.Windows.Forms.Button btn_Remove;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Button btn_Cancel;
    }
}