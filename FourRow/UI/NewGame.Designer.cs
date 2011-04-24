namespace FourRow.UI
{
    partial class NewGame
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.lblPlayer1 = new System.Windows.Forms.Label();
            this.lblPlayer2 = new System.Windows.Forms.Label();
            this.lblPlayer1Human = new System.Windows.Forms.Label();
            this.cmbPlayer2Option = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.cmbPlayer2AIDifficulty = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lblPlayer1
            // 
            this.lblPlayer1.AutoSize = true;
            this.lblPlayer1.Location = new System.Drawing.Point(30, 31);
            this.lblPlayer1.Name = "lblPlayer1";
            this.lblPlayer1.Size = new System.Drawing.Size(48, 13);
            this.lblPlayer1.TabIndex = 0;
            this.lblPlayer1.Text = "Player 1:";
            // 
            // lblPlayer2
            // 
            this.lblPlayer2.AutoSize = true;
            this.lblPlayer2.Location = new System.Drawing.Point(30, 62);
            this.lblPlayer2.Name = "lblPlayer2";
            this.lblPlayer2.Size = new System.Drawing.Size(48, 13);
            this.lblPlayer2.TabIndex = 1;
            this.lblPlayer2.Text = "Player 2:";
            // 
            // lblPlayer1Human
            // 
            this.lblPlayer1Human.AutoSize = true;
            this.lblPlayer1Human.Location = new System.Drawing.Point(95, 31);
            this.lblPlayer1Human.Name = "lblPlayer1Human";
            this.lblPlayer1Human.Size = new System.Drawing.Size(41, 13);
            this.lblPlayer1Human.TabIndex = 2;
            this.lblPlayer1Human.Text = "Human";
            // 
            // cmbPlayer2Option
            // 
            this.cmbPlayer2Option.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPlayer2Option.FormattingEnabled = true;
            this.cmbPlayer2Option.Location = new System.Drawing.Point(98, 58);
            this.cmbPlayer2Option.Name = "cmbPlayer2Option";
            this.cmbPlayer2Option.Size = new System.Drawing.Size(103, 21);
            this.cmbPlayer2Option.TabIndex = 3;
            this.cmbPlayer2Option.SelectedIndexChanged += new System.EventHandler(this.cmbPlayer2Option_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(265, 123);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(175, 123);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "&Continue";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // cmbPlayer2AIDifficulty
            // 
            this.cmbPlayer2AIDifficulty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPlayer2AIDifficulty.FormattingEnabled = true;
            this.cmbPlayer2AIDifficulty.Location = new System.Drawing.Point(207, 58);
            this.cmbPlayer2AIDifficulty.Name = "cmbPlayer2AIDifficulty";
            this.cmbPlayer2AIDifficulty.Size = new System.Drawing.Size(133, 21);
            this.cmbPlayer2AIDifficulty.TabIndex = 6;
            this.cmbPlayer2AIDifficulty.SelectedIndexChanged += new System.EventHandler(this.cmbPlayer2AIDifficulty_SelectedIndexChanged);
            // 
            // NewGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 158);
            this.Controls.Add(this.cmbPlayer2AIDifficulty);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.cmbPlayer2Option);
            this.Controls.Add(this.lblPlayer1Human);
            this.Controls.Add(this.lblPlayer2);
            this.Controls.Add(this.lblPlayer1);
            this.Name = "NewGame";
            this.Text = "New Game";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPlayer1;
        private System.Windows.Forms.Label lblPlayer2;
        private System.Windows.Forms.Label lblPlayer1Human;
        private System.Windows.Forms.ComboBox cmbPlayer2Option;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox cmbPlayer2AIDifficulty;
    }
}