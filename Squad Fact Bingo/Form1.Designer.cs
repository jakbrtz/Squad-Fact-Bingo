
namespace Squad_Fact_Bingo
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.BTNimport = new System.Windows.Forms.Button();
            this.BTNclear = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.BTNguess = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.CausesValidation = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 41);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.Size = new System.Drawing.Size(776, 397);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseClick);
            // 
            // BTNimport
            // 
            this.BTNimport.Location = new System.Drawing.Point(12, 12);
            this.BTNimport.Name = "BTNimport";
            this.BTNimport.Size = new System.Drawing.Size(75, 23);
            this.BTNimport.TabIndex = 1;
            this.BTNimport.Text = "Import";
            this.BTNimport.UseVisualStyleBackColor = true;
            this.BTNimport.Click += new System.EventHandler(this.BTNimport_Click);
            // 
            // BTNclear
            // 
            this.BTNclear.Location = new System.Drawing.Point(93, 12);
            this.BTNclear.Name = "BTNclear";
            this.BTNclear.Size = new System.Drawing.Size(75, 23);
            this.BTNclear.TabIndex = 2;
            this.BTNclear.Text = "Clear";
            this.BTNclear.UseVisualStyleBackColor = true;
            this.BTNclear.Click += new System.EventHandler(this.BTNclear_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "OFD";
            this.openFileDialog1.Filter = "Tab-deliminated file|*.txt";
            // 
            // BTNguess
            // 
            this.BTNguess.Location = new System.Drawing.Point(174, 12);
            this.BTNguess.Name = "BTNguess";
            this.BTNguess.Size = new System.Drawing.Size(75, 23);
            this.BTNguess.TabIndex = 3;
            this.BTNguess.Text = "Guess";
            this.BTNguess.UseVisualStyleBackColor = true;
            this.BTNguess.Click += new System.EventHandler(this.BTNguess_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BTNguess);
            this.Controls.Add(this.BTNclear);
            this.Controls.Add(this.BTNimport);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form1";
            this.Text = "Squad Bingo Assistance";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button BTNimport;
        private System.Windows.Forms.Button BTNclear;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button BTNguess;
    }
}

