namespace MCServerLauncher.Diags
{
    partial class diagArgumentGenerator
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbMinMemory = new System.Windows.Forms.TextBox();
            this.tbMaxMemory = new System.Windows.Forms.TextBox();
            this.chkOnlineMode = new System.Windows.Forms.CheckBox();
            this.chkUseGUI = new System.Windows.Forms.CheckBox();
            this.cmbMinTYPE = new System.Windows.Forms.ComboBox();
            this.cmbMaxTYPE = new System.Windows.Forms.ComboBox();
            this.btnDone = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Noto Mono", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Min. Memory";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Noto Mono", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "Max. Memory";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Noto Mono", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "Online Mode";
            this.label3.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Noto Mono", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 18);
            this.label4.TabIndex = 3;
            this.label4.Text = "Use GUI";
            // 
            // tbMinMemory
            // 
            this.tbMinMemory.Location = new System.Drawing.Point(125, 23);
            this.tbMinMemory.Name = "tbMinMemory";
            this.tbMinMemory.Size = new System.Drawing.Size(149, 20);
            this.tbMinMemory.TabIndex = 4;
            this.tbMinMemory.Text = "1024";
            // 
            // tbMaxMemory
            // 
            this.tbMaxMemory.Location = new System.Drawing.Point(125, 51);
            this.tbMaxMemory.Name = "tbMaxMemory";
            this.tbMaxMemory.Size = new System.Drawing.Size(149, 20);
            this.tbMaxMemory.TabIndex = 5;
            this.tbMaxMemory.Text = "1024";
            // 
            // chkOnlineMode
            // 
            this.chkOnlineMode.AutoSize = true;
            this.chkOnlineMode.Checked = true;
            this.chkOnlineMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOnlineMode.Location = new System.Drawing.Point(125, 87);
            this.chkOnlineMode.Name = "chkOnlineMode";
            this.chkOnlineMode.Size = new System.Drawing.Size(15, 14);
            this.chkOnlineMode.TabIndex = 6;
            this.chkOnlineMode.UseVisualStyleBackColor = true;
            // 
            // chkUseGUI
            // 
            this.chkUseGUI.AutoSize = true;
            this.chkUseGUI.Checked = true;
            this.chkUseGUI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseGUI.Location = new System.Drawing.Point(125, 113);
            this.chkUseGUI.Name = "chkUseGUI";
            this.chkUseGUI.Size = new System.Drawing.Size(15, 14);
            this.chkUseGUI.TabIndex = 7;
            this.chkUseGUI.UseVisualStyleBackColor = true;
            this.chkUseGUI.Visible = false;
            // 
            // cmbMinTYPE
            // 
            this.cmbMinTYPE.FormattingEnabled = true;
            this.cmbMinTYPE.Items.AddRange(new object[] {
            "MB",
            "GB"});
            this.cmbMinTYPE.Location = new System.Drawing.Point(280, 23);
            this.cmbMinTYPE.Name = "cmbMinTYPE";
            this.cmbMinTYPE.Size = new System.Drawing.Size(53, 21);
            this.cmbMinTYPE.TabIndex = 8;
            this.cmbMinTYPE.Text = "MB";
            // 
            // cmbMaxTYPE
            // 
            this.cmbMaxTYPE.FormattingEnabled = true;
            this.cmbMaxTYPE.Items.AddRange(new object[] {
            "MB",
            "GB"});
            this.cmbMaxTYPE.Location = new System.Drawing.Point(280, 50);
            this.cmbMaxTYPE.Name = "cmbMaxTYPE";
            this.cmbMaxTYPE.Size = new System.Drawing.Size(53, 21);
            this.cmbMaxTYPE.TabIndex = 9;
            this.cmbMaxTYPE.Text = "MB";
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(280, 132);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 23);
            this.btnDone.TabIndex = 10;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(199, 132);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // diagArgumentGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 167);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.cmbMaxTYPE);
            this.Controls.Add(this.cmbMinTYPE);
            this.Controls.Add(this.chkUseGUI);
            this.Controls.Add(this.chkOnlineMode);
            this.Controls.Add(this.tbMaxMemory);
            this.Controls.Add(this.tbMinMemory);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "diagArgumentGenerator";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Argument Generator";
            this.Load += new System.EventHandler(this.diagArgumentGenerator_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbMinMemory;
        private System.Windows.Forms.TextBox tbMaxMemory;
        private System.Windows.Forms.CheckBox chkOnlineMode;
        private System.Windows.Forms.CheckBox chkUseGUI;
        private System.Windows.Forms.ComboBox cmbMinTYPE;
        private System.Windows.Forms.ComboBox cmbMaxTYPE;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.Button button1;
    }
}