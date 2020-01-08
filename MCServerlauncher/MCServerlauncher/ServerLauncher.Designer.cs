﻿namespace MCServerLauncher
{
    partial class ServerLauncher
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
            this.btnLaunch = new System.Windows.Forms.Button();
            this.tbConsole = new System.Windows.Forms.TextBox();
            this.btnModifyProp = new System.Windows.Forms.Button();
            this.lbVersion = new System.Windows.Forms.ListBox();
            this.tbVersion = new System.Windows.Forms.TextBox();
            this.btnLaunchS = new System.Windows.Forms.Button();
            this.clbSType = new System.Windows.Forms.CheckedListBox();
            this.btnQLaunch = new System.Windows.Forms.Button();
            this.lblInstalled = new System.Windows.Forms.Label();
            this.btnReturn = new System.Windows.Forms.Button();
            this.chkCOSL = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnLaunch
            // 
            this.btnLaunch.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnLaunch.Location = new System.Drawing.Point(12, 12);
            this.btnLaunch.Name = "btnLaunch";
            this.btnLaunch.Size = new System.Drawing.Size(187, 36);
            this.btnLaunch.TabIndex = 0;
            this.btnLaunch.Text = "Download / Launch &Server";
            this.btnLaunch.UseVisualStyleBackColor = false;
            this.btnLaunch.Click += new System.EventHandler(this.Button1_Click);
            // 
            // tbConsole
            // 
            this.tbConsole.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tbConsole.Location = new System.Drawing.Point(526, 3);
            this.tbConsole.Multiline = true;
            this.tbConsole.Name = "tbConsole";
            this.tbConsole.ReadOnly = true;
            this.tbConsole.Size = new System.Drawing.Size(262, 151);
            this.tbConsole.TabIndex = 1;
            // 
            // btnModifyProp
            // 
            this.btnModifyProp.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnModifyProp.Location = new System.Drawing.Point(12, 54);
            this.btnModifyProp.Name = "btnModifyProp";
            this.btnModifyProp.Size = new System.Drawing.Size(187, 36);
            this.btnModifyProp.TabIndex = 2;
            this.btnModifyProp.Text = "Modi&fy Properties";
            this.btnModifyProp.UseVisualStyleBackColor = false;
            this.btnModifyProp.Click += new System.EventHandler(this.BtnModifyProp_Click);
            // 
            // lbVersion
            // 
            this.lbVersion.FormattingEnabled = true;
            this.lbVersion.Location = new System.Drawing.Point(12, 114);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(120, 95);
            this.lbVersion.TabIndex = 4;
            this.lbVersion.Visible = false;
            this.lbVersion.SelectedIndexChanged += new System.EventHandler(this.LbVersion_SelectedIndexChanged);
            // 
            // tbVersion
            // 
            this.tbVersion.Location = new System.Drawing.Point(12, 12);
            this.tbVersion.Name = "tbVersion";
            this.tbVersion.Size = new System.Drawing.Size(187, 20);
            this.tbVersion.TabIndex = 5;
            this.tbVersion.Visible = false;
            // 
            // btnLaunchS
            // 
            this.btnLaunchS.Location = new System.Drawing.Point(219, 12);
            this.btnLaunchS.Name = "btnLaunchS";
            this.btnLaunchS.Size = new System.Drawing.Size(133, 36);
            this.btnLaunchS.TabIndex = 6;
            this.btnLaunchS.Text = "&Go!";
            this.btnLaunchS.UseVisualStyleBackColor = true;
            this.btnLaunchS.Visible = false;
            this.btnLaunchS.Click += new System.EventHandler(this.Button3_Click);
            // 
            // clbSType
            // 
            this.clbSType.CheckOnClick = true;
            this.clbSType.FormattingEnabled = true;
            this.clbSType.Items.AddRange(new object[] {
            "Vanilla",
            "Spigot"});
            this.clbSType.Location = new System.Drawing.Point(176, 114);
            this.clbSType.Name = "clbSType";
            this.clbSType.Size = new System.Drawing.Size(120, 94);
            this.clbSType.TabIndex = 7;
            this.clbSType.Visible = false;
            this.clbSType.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ClbSType_ItemCheck);
            this.clbSType.SelectedIndexChanged += new System.EventHandler(this.ClbSType_SelectedIndexChanged);
            // 
            // btnQLaunch
            // 
            this.btnQLaunch.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnQLaunch.Location = new System.Drawing.Point(12, 96);
            this.btnQLaunch.Name = "btnQLaunch";
            this.btnQLaunch.Size = new System.Drawing.Size(187, 36);
            this.btnQLaunch.TabIndex = 9;
            this.btnQLaunch.Text = "&Quick Launch";
            this.btnQLaunch.UseVisualStyleBackColor = false;
            this.btnQLaunch.Click += new System.EventHandler(this.BtnQLaunch_Click);
            // 
            // lblInstalled
            // 
            this.lblInstalled.AutoSize = true;
            this.lblInstalled.Location = new System.Drawing.Point(9, 98);
            this.lblInstalled.Name = "lblInstalled";
            this.lblInstalled.Size = new System.Drawing.Size(89, 13);
            this.lblInstalled.TabIndex = 10;
            this.lblInstalled.Text = "Installed Versions";
            this.lblInstalled.Visible = false;
            // 
            // btnReturn
            // 
            this.btnReturn.Location = new System.Drawing.Point(12, 237);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(75, 23);
            this.btnReturn.TabIndex = 11;
            this.btnReturn.Text = "&Return";
            this.btnReturn.UseVisualStyleBackColor = true;
            this.btnReturn.Visible = false;
            this.btnReturn.Click += new System.EventHandler(this.BtnReturn_Click);
            // 
            // chkCOSL
            // 
            this.chkCOSL.AutoSize = true;
            this.chkCOSL.Location = new System.Drawing.Point(219, 54);
            this.chkCOSL.Name = "chkCOSL";
            this.chkCOSL.Size = new System.Drawing.Size(142, 17);
            this.chkCOSL.TabIndex = 12;
            this.chkCOSL.Text = "Close On Server Launch";
            this.chkCOSL.UseVisualStyleBackColor = true;
            this.chkCOSL.Visible = false;
            this.chkCOSL.CheckedChanged += new System.EventHandler(this.CheckBox1_CheckedChanged);
            // 
            // ServerLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.chkCOSL);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.lblInstalled);
            this.Controls.Add(this.btnQLaunch);
            this.Controls.Add(this.clbSType);
            this.Controls.Add(this.btnLaunchS);
            this.Controls.Add(this.tbVersion);
            this.Controls.Add(this.lbVersion);
            this.Controls.Add(this.btnModifyProp);
            this.Controls.Add(this.tbConsole);
            this.Controls.Add(this.btnLaunch);
            this.Name = "ServerLauncher";
            this.Text = "ServerLauncher";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ServerLauncher_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLaunch;
        public System.Windows.Forms.TextBox tbConsole;
        private System.Windows.Forms.Button btnModifyProp;
        private System.Windows.Forms.ListBox lbVersion;
        private System.Windows.Forms.TextBox tbVersion;
        private System.Windows.Forms.Button btnLaunchS;
        private System.Windows.Forms.CheckedListBox clbSType;
        private System.Windows.Forms.Button btnQLaunch;
        private System.Windows.Forms.Label lblInstalled;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.CheckBox chkCOSL;
    }
}