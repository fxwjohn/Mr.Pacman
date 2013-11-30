namespace Mr.Pacman
{
    partial class StartForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartForm));
            this.label_pacman = new System.Windows.Forms.Label();
            this.label_class = new System.Windows.Forms.Label();
            this.label_author = new System.Windows.Forms.Label();
            this.label_load = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_pacman
            // 
            this.label_pacman.AutoSize = true;
            this.label_pacman.BackColor = System.Drawing.Color.Transparent;
            this.label_pacman.Font = new System.Drawing.Font("Comic Sans MS", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_pacman.Location = new System.Drawing.Point(27, 41);
            this.label_pacman.Name = "label_pacman";
            this.label_pacman.Size = new System.Drawing.Size(268, 67);
            this.label_pacman.TabIndex = 0;
            this.label_pacman.Text = "Mr.Pacman";
            // 
            // label_class
            // 
            this.label_class.AutoSize = true;
            this.label_class.BackColor = System.Drawing.Color.Transparent;
            this.label_class.Font = new System.Drawing.Font("Comic Sans MS", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_class.Location = new System.Drawing.Point(329, 193);
            this.label_class.Name = "label_class";
            this.label_class.Size = new System.Drawing.Size(70, 29);
            this.label_class.TabIndex = 1;
            this.label_class.Text = "DA02";
            // 
            // label_author
            // 
            this.label_author.AutoSize = true;
            this.label_author.BackColor = System.Drawing.Color.Transparent;
            this.label_author.Font = new System.Drawing.Font("Comic Sans MS", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_author.Location = new System.Drawing.Point(329, 231);
            this.label_author.Name = "label_author";
            this.label_author.Size = new System.Drawing.Size(117, 29);
            this.label_author.TabIndex = 2;
            this.label_author.Text = "Xiwei Feng";
            // 
            // label_load
            // 
            this.label_load.AutoSize = true;
            this.label_load.BackColor = System.Drawing.Color.Transparent;
            this.label_load.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_load.Location = new System.Drawing.Point(39, 236);
            this.label_load.Name = "label_load";
            this.label_load.Size = new System.Drawing.Size(0, 23);
            this.label_load.TabIndex = 3;
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.YellowGreen;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(485, 302);
            this.Controls.Add(this.label_load);
            this.Controls.Add(this.label_author);
            this.Controls.Add(this.label_class);
            this.Controls.Add(this.label_pacman);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StartForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StartForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_pacman;
        private System.Windows.Forms.Label label_class;
        private System.Windows.Forms.Label label_author;
        public System.Windows.Forms.Label label_load;
    }
}