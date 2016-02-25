namespace TextureEditor
{
    partial class mainForm
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
            this.btnLoadRom = new System.Windows.Forms.Button();
            this.btnSaveRom = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gbImage = new System.Windows.Forms.GroupBox();
            this.cbTextures = new System.Windows.Forms.ComboBox();
            this.btnImportImage = new System.Windows.Forms.Button();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.btnExportImage = new System.Windows.Forms.Button();
            this.cbLevel = new System.Windows.Forms.ComboBox();
            this.btnSaveLevel = new System.Windows.Forms.Button();
            this.btnLoadLevel = new System.Windows.Forms.Button();
            this.saveImageDialog = new System.Windows.Forms.SaveFileDialog();
            this.openImageDialog = new System.Windows.Forms.OpenFileDialog();
            this.lblLevel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.gbImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLoadRom
            // 
            this.btnLoadRom.Location = new System.Drawing.Point(35, 15);
            this.btnLoadRom.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoadRom.Name = "btnLoadRom";
            this.btnLoadRom.Size = new System.Drawing.Size(153, 49);
            this.btnLoadRom.TabIndex = 0;
            this.btnLoadRom.Text = "Load ROM";
            this.btnLoadRom.UseVisualStyleBackColor = true;
            this.btnLoadRom.Click += new System.EventHandler(this.btnLoadRom_Click);
            // 
            // btnSaveRom
            // 
            this.btnSaveRom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveRom.Enabled = false;
            this.btnSaveRom.Location = new System.Drawing.Point(427, 13);
            this.btnSaveRom.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveRom.Name = "btnSaveRom";
            this.btnSaveRom.Size = new System.Drawing.Size(153, 49);
            this.btnSaveRom.TabIndex = 1;
            this.btnSaveRom.Text = "Save ROM";
            this.btnSaveRom.UseVisualStyleBackColor = true;
            this.btnSaveRom.Click += new System.EventHandler(this.btnSaveRom_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "smashbrosrom";
            this.openFileDialog.Filter = "Z64 Roms|*.z64";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Z64 Roms|*.z64";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lblLevel);
            this.groupBox1.Controls.Add(this.gbImage);
            this.groupBox1.Controls.Add(this.cbLevel);
            this.groupBox1.Controls.Add(this.btnSaveLevel);
            this.groupBox1.Controls.Add(this.btnLoadLevel);
            this.groupBox1.Location = new System.Drawing.Point(12, 71);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(590, 239);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Textures/Palettes";
            // 
            // gbImage
            // 
            this.gbImage.Controls.Add(this.cbTextures);
            this.gbImage.Controls.Add(this.btnImportImage);
            this.gbImage.Controls.Add(this.pictureBox);
            this.gbImage.Controls.Add(this.btnExportImage);
            this.gbImage.Location = new System.Drawing.Point(200, 21);
            this.gbImage.Name = "gbImage";
            this.gbImage.Size = new System.Drawing.Size(374, 191);
            this.gbImage.TabIndex = 7;
            this.gbImage.TabStop = false;
            this.gbImage.Text = "Texture";
            // 
            // cbTextures
            // 
            this.cbTextures.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTextures.Enabled = false;
            this.cbTextures.FormattingEnabled = true;
            this.cbTextures.Location = new System.Drawing.Point(17, 23);
            this.cbTextures.Name = "cbTextures";
            this.cbTextures.Size = new System.Drawing.Size(329, 24);
            this.cbTextures.TabIndex = 0;
            this.cbTextures.SelectedIndexChanged += new System.EventHandler(this.cbTextures_SelectedIndexChanged);
            // 
            // btnImportImage
            // 
            this.btnImportImage.Enabled = false;
            this.btnImportImage.Location = new System.Drawing.Point(206, 114);
            this.btnImportImage.Margin = new System.Windows.Forms.Padding(4);
            this.btnImportImage.Name = "btnImportImage";
            this.btnImportImage.Size = new System.Drawing.Size(153, 40);
            this.btnImportImage.TabIndex = 6;
            this.btnImportImage.Text = "Import Image";
            this.btnImportImage.UseVisualStyleBackColor = true;
            this.btnImportImage.Click += new System.EventHandler(this.btnImportImage_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(12, 53);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(174, 127);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // btnExportImage
            // 
            this.btnExportImage.Enabled = false;
            this.btnExportImage.Location = new System.Drawing.Point(206, 53);
            this.btnExportImage.Margin = new System.Windows.Forms.Padding(4);
            this.btnExportImage.Name = "btnExportImage";
            this.btnExportImage.Size = new System.Drawing.Size(153, 40);
            this.btnExportImage.TabIndex = 5;
            this.btnExportImage.Text = "Export Image";
            this.btnExportImage.UseVisualStyleBackColor = true;
            this.btnExportImage.Click += new System.EventHandler(this.btnExportImage_Click);
            // 
            // cbLevel
            // 
            this.cbLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLevel.Enabled = false;
            this.cbLevel.FormattingEnabled = true;
            this.cbLevel.Items.AddRange(new object[] {
            "Hyrule Temple"});
            this.cbLevel.Location = new System.Drawing.Point(23, 52);
            this.cbLevel.Name = "cbLevel";
            this.cbLevel.Size = new System.Drawing.Size(153, 24);
            this.cbLevel.TabIndex = 0;
            // 
            // btnSaveLevel
            // 
            this.btnSaveLevel.Enabled = false;
            this.btnSaveLevel.Location = new System.Drawing.Point(23, 161);
            this.btnSaveLevel.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveLevel.Name = "btnSaveLevel";
            this.btnSaveLevel.Size = new System.Drawing.Size(153, 40);
            this.btnSaveLevel.TabIndex = 4;
            this.btnSaveLevel.Text = "Save Level";
            this.btnSaveLevel.UseVisualStyleBackColor = true;
            this.btnSaveLevel.Click += new System.EventHandler(this.btnSaveLevel_Click);
            // 
            // btnLoadLevel
            // 
            this.btnLoadLevel.Enabled = false;
            this.btnLoadLevel.Location = new System.Drawing.Point(23, 100);
            this.btnLoadLevel.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoadLevel.Name = "btnLoadLevel";
            this.btnLoadLevel.Size = new System.Drawing.Size(153, 40);
            this.btnLoadLevel.TabIndex = 3;
            this.btnLoadLevel.Text = "Load Level";
            this.btnLoadLevel.UseVisualStyleBackColor = true;
            this.btnLoadLevel.Click += new System.EventHandler(this.btnLoadLevel_Click);
            // 
            // saveImageDialog
            // 
            this.saveImageDialog.Filter = "BMP files|*.bmp";
            // 
            // openImageDialog
            // 
            this.openImageDialog.Filter = "BMP files|*.bmp|PNG files|*.png";
            // 
            // lblLevel
            // 
            this.lblLevel.AutoSize = true;
            this.lblLevel.Location = new System.Drawing.Point(34, 33);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(41, 16);
            this.lblLevel.TabIndex = 8;
            this.lblLevel.Text = "Level";
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 322);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSaveRom);
            this.Controls.Add(this.btnLoadRom);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "mainForm";
            this.Text = "TBST Beta Tool: Level Texture Swapper V.0.1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLoadRom;
        private System.Windows.Forms.Button btnSaveRom;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnLoadLevel;
        private System.Windows.Forms.Button btnSaveLevel;
        private System.Windows.Forms.ComboBox cbLevel;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ComboBox cbTextures;
        private System.Windows.Forms.Button btnImportImage;
        private System.Windows.Forms.Button btnExportImage;
        private System.Windows.Forms.GroupBox gbImage;
        private System.Windows.Forms.SaveFileDialog saveImageDialog;
        private System.Windows.Forms.OpenFileDialog openImageDialog;
        private System.Windows.Forms.Label lblLevel;
    }
}

