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
            this.lbl64Bit = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbMinMemory = new System.Windows.Forms.TextBox();
            this.tbMaxMemory = new System.Windows.Forms.TextBox();
            this.chkUseGUI = new System.Windows.Forms.CheckBox();
            this.chkX64 = new System.Windows.Forms.CheckBox();
            this.cmbMinTYPE = new System.Windows.Forms.ComboBox();
            this.cmbMaxTYPE = new System.Windows.Forms.ComboBox();
            this.btnDone = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnAdv = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
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
            // lbl64Bit
            // 
            this.lbl64Bit.AutoSize = true;
            this.lbl64Bit.Font = new System.Drawing.Font("Noto Mono", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl64Bit.Location = new System.Drawing.Point(12, 110);
            this.lbl64Bit.Name = "lbl64Bit";
            this.lbl64Bit.Size = new System.Drawing.Size(80, 18);
            this.lbl64Bit.TabIndex = 2;
            this.lbl64Bit.Text = "x64 Mode";
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
            // chkUseGUI
            // 
            this.chkUseGUI.AutoSize = true;
            this.chkUseGUI.Checked = true;
            this.chkUseGUI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseGUI.Location = new System.Drawing.Point(125, 87);
            this.chkUseGUI.Name = "chkUseGUI";
            this.chkUseGUI.Size = new System.Drawing.Size(15, 14);
            this.chkUseGUI.TabIndex = 6;
            this.chkUseGUI.UseVisualStyleBackColor = true;
            // 
            // chkX64
            // 
            this.chkX64.AutoSize = true;
            this.chkX64.Checked = true;
            this.chkX64.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkX64.Location = new System.Drawing.Point(125, 113);
            this.chkX64.Name = "chkX64";
            this.chkX64.Size = new System.Drawing.Size(15, 14);
            this.chkX64.TabIndex = 7;
            this.chkX64.UseVisualStyleBackColor = true;
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
            this.btnDone.Location = new System.Drawing.Point(199, 132);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 23);
            this.btnDone.TabIndex = 10;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(280, 132);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnAdv
            // 
            this.btnAdv.Location = new System.Drawing.Point(12, 132);
            this.btnAdv.Name = "btnAdv";
            this.btnAdv.Size = new System.Drawing.Size(107, 23);
            this.btnAdv.TabIndex = 12;
            this.btnAdv.Text = "Advanced";
            this.btnAdv.UseVisualStyleBackColor = true;
            this.btnAdv.Click += new System.EventHandler(this.btnAdv_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(279, 104);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 13;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // diagArgumentGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 167);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnAdv);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.cmbMaxTYPE);
            this.Controls.Add(this.cmbMinTYPE);
            this.Controls.Add(this.chkX64);
            this.Controls.Add(this.chkUseGUI);
            this.Controls.Add(this.tbMaxMemory);
            this.Controls.Add(this.tbMinMemory);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbl64Bit);
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
        private System.Windows.Forms.Label lbl64Bit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbMinMemory;
        private System.Windows.Forms.TextBox tbMaxMemory;
        private System.Windows.Forms.CheckBox chkUseGUI;
        private System.Windows.Forms.CheckBox chkX64;
        private System.Windows.Forms.ComboBox cmbMinTYPE;
        private System.Windows.Forms.ComboBox cmbMaxTYPE;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnAdv;
        private System.Windows.Forms.Button btnReset;
    }
}