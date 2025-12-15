namespace Pranav_UR2_Armbot_VS_Code_v2
{
    partial class Form1
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
            this.rawWebcamImg = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.binaryImg = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.warpedImg = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.shpeIDContorImg = new System.Windows.Forms.PictureBox();
            this.binaryThreshBar = new System.Windows.Forms.TrackBar();
            this.binThreshText = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.rawWebcamImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.binaryImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.warpedImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.shpeIDContorImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.binaryThreshBar)).BeginInit();
            this.SuspendLayout();
            // 
            // rawWebcamImg
            // 
            this.rawWebcamImg.Location = new System.Drawing.Point(12, 12);
            this.rawWebcamImg.Name = "rawWebcamImg";
            this.rawWebcamImg.Size = new System.Drawing.Size(945, 804);
            this.rawWebcamImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.rawWebcamImg.TabIndex = 0;
            this.rawWebcamImg.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(247, 876);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 32);
            this.label1.TabIndex = 1;
            this.label1.Text = "Raw Webcam Footage";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1588, 1510);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(180, 32);
            this.label2.TabIndex = 3;
            this.label2.Text = "Binary Image";
            // 
            // binaryImg
            // 
            this.binaryImg.Location = new System.Drawing.Point(1403, 953);
            this.binaryImg.Name = "binaryImg";
            this.binaryImg.Size = new System.Drawing.Size(664, 528);
            this.binaryImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.binaryImg.TabIndex = 2;
            this.binaryImg.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1549, 857);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(198, 32);
            this.label3.TabIndex = 5;
            this.label3.Text = "Warped Image";
            // 
            // warpedImg
            // 
            this.warpedImg.Location = new System.Drawing.Point(1028, 12);
            this.warpedImg.Name = "warpedImg";
            this.warpedImg.Size = new System.Drawing.Size(945, 804);
            this.warpedImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.warpedImg.TabIndex = 4;
            this.warpedImg.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2569, 857);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(247, 32);
            this.label4.TabIndex = 7;
            this.label4.Text = "Shapes & Centroids";
            // 
            // shpeIDContorImg
            // 
            this.shpeIDContorImg.Location = new System.Drawing.Point(2198, 12);
            this.shpeIDContorImg.Name = "shpeIDContorImg";
            this.shpeIDContorImg.Size = new System.Drawing.Size(945, 804);
            this.shpeIDContorImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.shpeIDContorImg.TabIndex = 6;
            this.shpeIDContorImg.TabStop = false;
            // 
            // binaryThreshBar
            // 
            this.binaryThreshBar.Location = new System.Drawing.Point(805, 1073);
            this.binaryThreshBar.Name = "binaryThreshBar";
            this.binaryThreshBar.Size = new System.Drawing.Size(592, 114);
            this.binaryThreshBar.TabIndex = 8;
            // 
            // binThreshText
            // 
            this.binThreshText.Location = new System.Drawing.Point(835, 1264);
            this.binThreshText.Name = "binThreshText";
            this.binThreshText.Size = new System.Drawing.Size(524, 38);
            this.binThreshText.TabIndex = 9;
            this.binThreshText.Text = "Value: ";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(209, 976);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(468, 178);
            this.startButton.TabIndex = 11;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(209, 1264);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(468, 178);
            this.stopButton.TabIndex = 12;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(3553, 1645);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.binThreshText);
            this.Controls.Add(this.binaryThreshBar);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.shpeIDContorImg);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.warpedImg);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.binaryImg);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rawWebcamImg);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.rawWebcamImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.binaryImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.warpedImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.shpeIDContorImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.binaryThreshBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox rawWebcamImg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox binaryImg;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox warpedImg;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox shpeIDContorImg;
        private System.Windows.Forms.TrackBar binaryThreshBar;
        private System.Windows.Forms.TextBox binThreshText;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
    }
}

