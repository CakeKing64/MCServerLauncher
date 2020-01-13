namespace MCServerLauncher
{
    partial class WorldMod
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
            this.lstWorlds = new System.Windows.Forms.ListBox();
            this.lstBackups = new System.Windows.Forms.ListBox();
            this.btnMoveBack = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnSet = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblDnD = new System.Windows.Forms.Label();
            this.lblCurrent = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstWorlds
            // 
            this.lstWorlds.AllowDrop = true;
            this.lstWorlds.FormattingEnabled = true;
            this.lstWorlds.Location = new System.Drawing.Point(12, 64);
            this.lstWorlds.Name = "lstWorlds";
            this.lstWorlds.Size = new System.Drawing.Size(200, 290);
            this.lstWorlds.TabIndex = 0;
            this.lstWorlds.SelectedIndexChanged += new System.EventHandler(this.LstWorlds_SelectedIndexChanged);
            this.lstWorlds.DragDrop += new System.Windows.Forms.DragEventHandler(this.LstWorlds_DragDrop);
            // 
            // lstBackups
            // 
            this.lstBackups.AllowDrop = true;
            this.lstBackups.FormattingEnabled = true;
            this.lstBackups.Location = new System.Drawing.Point(343, 64);
            this.lstBackups.Name = "lstBackups";
            this.lstBackups.Size = new System.Drawing.Size(200, 290);
            this.lstBackups.TabIndex = 1;
            this.lstBackups.SelectedIndexChanged += new System.EventHandler(this.LstBackups_SelectedIndexChanged);
            // 
            // btnMoveBack
            // 
            this.btnMoveBack.AllowDrop = true;
            this.btnMoveBack.Enabled = false;
            this.btnMoveBack.Location = new System.Drawing.Point(218, 64);
            this.btnMoveBack.Name = "btnMoveBack";
            this.btnMoveBack.Size = new System.Drawing.Size(119, 23);
            this.btnMoveBack.TabIndex = 2;
            this.btnMoveBack.Text = "Move to backups";
            this.btnMoveBack.UseVisualStyleBackColor = true;
            // 
            // btnCopy
            // 
            this.btnCopy.AllowDrop = true;
            this.btnCopy.Enabled = false;
            this.btnCopy.Location = new System.Drawing.Point(218, 93);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(119, 23);
            this.btnCopy.TabIndex = 3;
            this.btnCopy.Text = "Copy to backups";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.BtnCopy_Click);
            // 
            // btnSet
            // 
            this.btnSet.AllowDrop = true;
            this.btnSet.Location = new System.Drawing.Point(218, 123);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(119, 23);
            this.btnSet.TabIndex = 4;
            this.btnSet.Text = "Set as current world";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.BtnSet_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.AllowDrop = true;
            this.btnDelete.Location = new System.Drawing.Point(218, 153);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(119, 23);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            this.btnRefresh.AllowDrop = true;
            this.btnRefresh.Location = new System.Drawing.Point(218, 331);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(119, 23);
            this.btnRefresh.TabIndex = 6;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // lblDnD
            // 
            this.lblDnD.AllowDrop = true;
            this.lblDnD.AutoSize = true;
            this.lblDnD.Location = new System.Drawing.Point(12, 371);
            this.lblDnD.Name = "lblDnD";
            this.lblDnD.Size = new System.Drawing.Size(286, 13);
            this.lblDnD.TabIndex = 7;
            this.lblDnD.Text = "Drag and drop folders / zip files to add them to the world list";
            // 
            // lblCurrent
            // 
            this.lblCurrent.AutoSize = true;
            this.lblCurrent.Location = new System.Drawing.Point(12, 48);
            this.lblCurrent.Name = "lblCurrent";
            this.lblCurrent.Size = new System.Drawing.Size(96, 13);
            this.lblCurrent.TabIndex = 8;
            this.lblCurrent.Text = "Current World: ???";
            // 
            // WorldMod
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 450);
            this.Controls.Add(this.lblCurrent);
            this.Controls.Add(this.lblDnD);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnSet);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnMoveBack);
            this.Controls.Add(this.lstBackups);
            this.Controls.Add(this.lstWorlds);
            this.Name = "WorldMod";
            this.Text = "World Manager";
            this.Load += new System.EventHandler(this.WorldMod_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.WorldMod_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.WorldMod_DragEnter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstWorlds;
        private System.Windows.Forms.ListBox lstBackups;
        private System.Windows.Forms.Button btnMoveBack;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblDnD;
        private System.Windows.Forms.Label lblCurrent;
    }
}