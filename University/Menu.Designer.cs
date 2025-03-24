namespace University
{
    partial class Menu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Menu));
            this.btnDept = new System.Windows.Forms.Button();
            this.btnCollege = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDept
            // 
            this.btnDept.BackColor = System.Drawing.Color.BlanchedAlmond;
            this.btnDept.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDept.Location = new System.Drawing.Point(369, 116);
            this.btnDept.Margin = new System.Windows.Forms.Padding(4);
            this.btnDept.Name = "btnDept";
            this.btnDept.Size = new System.Drawing.Size(312, 116);
            this.btnDept.TabIndex = 0;
            this.btnDept.Text = "Department";
            this.btnDept.UseVisualStyleBackColor = false;
            this.btnDept.Click += new System.EventHandler(this.btnDept_Click);
            // 
            // btnCollege
            // 
            this.btnCollege.BackColor = System.Drawing.Color.BlanchedAlmond;
            this.btnCollege.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCollege.Location = new System.Drawing.Point(369, 257);
            this.btnCollege.Margin = new System.Windows.Forms.Padding(4);
            this.btnCollege.Name = "btnCollege";
            this.btnCollege.Size = new System.Drawing.Size(312, 116);
            this.btnCollege.TabIndex = 1;
            this.btnCollege.Text = "College";
            this.btnCollege.UseVisualStyleBackColor = false;
            this.btnCollege.Click += new System.EventHandler(this.btnCollege_Click);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1024, 554);
            this.Controls.Add(this.btnCollege);
            this.Controls.Add(this.btnDept);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Menu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Menu";
            this.Load += new System.EventHandler(this.Menu_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDept;
        private System.Windows.Forms.Button btnCollege;
    }
}