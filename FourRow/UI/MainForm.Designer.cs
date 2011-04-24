namespace FourRow.UI
{
    partial class MainForm
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
            this.btnNewGame = new System.Windows.Forms.Button();
            this.lblStateDescription = new System.Windows.Forms.Label();
            this.dropBoard = new FourRow.UI.DropBoard();
            this.SuspendLayout();
            // 
            // btnNewGame
            // 
            this.btnNewGame.Location = new System.Drawing.Point(73, 18);
            this.btnNewGame.Name = "btnNewGame";
            this.btnNewGame.Size = new System.Drawing.Size(75, 23);
            this.btnNewGame.TabIndex = 1;
            this.btnNewGame.Text = "NewGame";
            this.btnNewGame.UseVisualStyleBackColor = true;
            this.btnNewGame.Click += new System.EventHandler(this.btnNewGame_Click);
            // 
            // lblStateDescription
            // 
            this.lblStateDescription.AutoSize = true;
            this.lblStateDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStateDescription.ForeColor = System.Drawing.Color.DimGray;
            this.lblStateDescription.Location = new System.Drawing.Point(222, 433);
            this.lblStateDescription.Name = "lblStateDescription";
            this.lblStateDescription.Size = new System.Drawing.Size(132, 20);
            this.lblStateDescription.TabIndex = 3;
            this.lblStateDescription.Text = "State Description";
            // 
            // dropBoard
            // 
            this.dropBoard.Location = new System.Drawing.Point(73, 61);
            this.dropBoard.Name = "dropBoard";
            this.dropBoard.Size = new System.Drawing.Size(448, 347);
            this.dropBoard.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 524);
            this.Controls.Add(this.lblStateDescription);
            this.Controls.Add(this.dropBoard);
            this.Controls.Add(this.btnNewGame);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNewGame;
        private DropBoard dropBoard;
        private System.Windows.Forms.Label lblStateDescription;
    }
}

