namespace FtpFileDialog
{
  partial class FtpBrowseProgressDialog
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
      this.LoadingLabel = new System.Windows.Forms.Label();
      this.progressBar1 = new System.Windows.Forms.ProgressBar();
      this.CancelButton = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // LoadingLabel
      // 
      this.LoadingLabel.AutoSize = true;
      this.LoadingLabel.Location = new System.Drawing.Point(12, 9);
      this.LoadingLabel.Name = "LoadingLabel";
      this.LoadingLabel.Size = new System.Drawing.Size(133, 13);
      this.LoadingLabel.TabIndex = 0;
      this.LoadingLabel.Text = "FTP File System Loading...";
      // 
      // progressBar1
      // 
      this.progressBar1.Location = new System.Drawing.Point(15, 30);
      this.progressBar1.Name = "progressBar1";
      this.progressBar1.Size = new System.Drawing.Size(401, 13);
      this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
      this.progressBar1.TabIndex = 1;
      this.progressBar1.Value = 1;
      // 
      // CancelButton
      // 
      this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.CancelButton.Location = new System.Drawing.Point(341, 49);
      this.CancelButton.Name = "CancelButton";
      this.CancelButton.Size = new System.Drawing.Size(75, 23);
      this.CancelButton.TabIndex = 2;
      this.CancelButton.Text = "Cancel";
      this.CancelButton.UseVisualStyleBackColor = true;
      this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
      // 
      // FtpBrowseProgressDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(428, 82);
      this.ControlBox = false;
      this.Controls.Add(this.CancelButton);
      this.Controls.Add(this.progressBar1);
      this.Controls.Add(this.LoadingLabel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Name = "FtpBrowseProgressDialog";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label LoadingLabel;
    private System.Windows.Forms.ProgressBar progressBar1;
    private new System.Windows.Forms.Button CancelButton;
  }
}