namespace Library_Management_System.Borrower_UI
{
    partial class EnterwhatyouRemember
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtMaterialTitle = new System.Windows.Forms.TextBox();
            this.rbtnArticleorjournal = new System.Windows.Forms.RadioButton();
            this.rbtnBook = new System.Windows.Forms.RadioButton();
            this.rbtnDissertation = new System.Windows.Forms.RadioButton();
            this.btnRequest = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtMaterialTitle
            // 
            this.txtMaterialTitle.Location = new System.Drawing.Point(188, 49);
            this.txtMaterialTitle.Multiline = true;
            this.txtMaterialTitle.Name = "txtMaterialTitle";
            this.txtMaterialTitle.Size = new System.Drawing.Size(330, 183);
            this.txtMaterialTitle.TabIndex = 0;
            // 
            // rbtnArticleorjournal
            // 
            this.rbtnArticleorjournal.AutoSize = true;
            this.rbtnArticleorjournal.Location = new System.Drawing.Point(188, 254);
            this.rbtnArticleorjournal.Name = "rbtnArticleorjournal";
            this.rbtnArticleorjournal.Size = new System.Drawing.Size(99, 17);
            this.rbtnArticleorjournal.TabIndex = 1;
            this.rbtnArticleorjournal.TabStop = true;
            this.rbtnArticleorjournal.Text = "Article / Journal";
            this.rbtnArticleorjournal.UseVisualStyleBackColor = true;
            this.rbtnArticleorjournal.CheckedChanged += new System.EventHandler(this.rbtnArticleorjournal_CheckedChanged);
            // 
            // rbtnBook
            // 
            this.rbtnBook.AutoSize = true;
            this.rbtnBook.Location = new System.Drawing.Point(335, 254);
            this.rbtnBook.Name = "rbtnBook";
            this.rbtnBook.Size = new System.Drawing.Size(50, 17);
            this.rbtnBook.TabIndex = 2;
            this.rbtnBook.TabStop = true;
            this.rbtnBook.Text = "Book";
            this.rbtnBook.UseVisualStyleBackColor = true;
            this.rbtnBook.CheckedChanged += new System.EventHandler(this.rbtnBook_CheckedChanged);
            // 
            // rbtnDissertation
            // 
            this.rbtnDissertation.AutoSize = true;
            this.rbtnDissertation.Location = new System.Drawing.Point(438, 254);
            this.rbtnDissertation.Name = "rbtnDissertation";
            this.rbtnDissertation.Size = new System.Drawing.Size(80, 17);
            this.rbtnDissertation.TabIndex = 3;
            this.rbtnDissertation.TabStop = true;
            this.rbtnDissertation.Text = "Dissertation";
            this.rbtnDissertation.UseVisualStyleBackColor = true;
            this.rbtnDissertation.CheckedChanged += new System.EventHandler(this.rbtnDissertation_CheckedChanged);
            // 
            // btnRequest
            // 
            this.btnRequest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(107)))), ((int)(((byte)(216)))));
            this.btnRequest.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(88)))), ((int)(((byte)(180)))));
            this.btnRequest.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnRequest.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRequest.FlatAppearance.BorderSize = 0;
            this.btnRequest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRequest.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRequest.ForeColor = System.Drawing.Color.White;
            this.btnRequest.Location = new System.Drawing.Point(309, 310);
            this.btnRequest.Name = "btnRequest";
            this.btnRequest.Size = new System.Drawing.Size(76, 33);
            this.btnRequest.TabIndex = 17;
            this.btnRequest.Text = "Request";
            this.btnRequest.UseVisualStyleBackColor = false;
            this.btnRequest.Click += new System.EventHandler(this.btnRequest_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(372, 45);
            this.label1.TabIndex = 18;
            this.label1.Text = "Request InterLibraryLoan";
            // 
            // EnterwhatyouRemember
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRequest);
            this.Controls.Add(this.rbtnDissertation);
            this.Controls.Add(this.rbtnBook);
            this.Controls.Add(this.rbtnArticleorjournal);
            this.Controls.Add(this.txtMaterialTitle);
            this.Name = "EnterwhatyouRemember";
            this.Size = new System.Drawing.Size(721, 379);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtMaterialTitle;
        private System.Windows.Forms.RadioButton rbtnArticleorjournal;
        private System.Windows.Forms.RadioButton rbtnBook;
        private System.Windows.Forms.RadioButton rbtnDissertation;
        private System.Windows.Forms.Button btnRequest;
        private System.Windows.Forms.Label label1;
    }
}
