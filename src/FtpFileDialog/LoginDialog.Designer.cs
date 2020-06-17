namespace FtpFileDialog
{
  partial class LoginDialog
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
      this.ServerLabel = new System.Windows.Forms.Label();
      this.UsernameLabel = new System.Windows.Forms.Label();
      this.PasswordLabel = new System.Windows.Forms.Label();
      this.UsernameTextBox = new System.Windows.Forms.TextBox();
      this.PasswordTextBox = new System.Windows.Forms.TextBox();
      this.ServerTextBox = new System.Windows.Forms.TextBox();
      this.PassiveCheckbox = new System.Windows.Forms.CheckBox();
      this.PortLabel = new System.Windows.Forms.Label();
      this.ConnectButton = new System.Windows.Forms.Button();
      this.CancelButton = new System.Windows.Forms.Button();
      this.PortNumeric = new System.Windows.Forms.NumericUpDown();
      this.StartPathLabel = new System.Windows.Forms.Label();
      this.StartPathTextBox = new System.Windows.Forms.TextBox();
      ((System.ComponentModel.ISupportInitialize)(this.PortNumeric)).BeginInit();
      this.SuspendLayout();
      // 
      // ServerLabel
      // 
      this.ServerLabel.AutoSize = true;
      this.ServerLabel.Location = new System.Drawing.Point(22, 21);
      this.ServerLabel.Name = "ServerLabel";
      this.ServerLabel.Size = new System.Drawing.Size(38, 13);
      this.ServerLabel.TabIndex = 0;
      this.ServerLabel.Text = "Server";
      // 
      // UsernameLabel
      // 
      this.UsernameLabel.AutoSize = true;
      this.UsernameLabel.Location = new System.Drawing.Point(22, 111);
      this.UsernameLabel.Name = "UsernameLabel";
      this.UsernameLabel.Size = new System.Drawing.Size(55, 13);
      this.UsernameLabel.TabIndex = 1;
      this.UsernameLabel.Text = "Username";
      // 
      // PasswordLabel
      // 
      this.PasswordLabel.AutoSize = true;
      this.PasswordLabel.Location = new System.Drawing.Point(23, 141);
      this.PasswordLabel.Name = "PasswordLabel";
      this.PasswordLabel.Size = new System.Drawing.Size(53, 13);
      this.PasswordLabel.TabIndex = 2;
      this.PasswordLabel.Text = "Password";
      // 
      // UsernameTextBox
      // 
      this.UsernameTextBox.Location = new System.Drawing.Point(83, 108);
      this.UsernameTextBox.Name = "UsernameTextBox";
      this.UsernameTextBox.Size = new System.Drawing.Size(100, 20);
      this.UsernameTextBox.TabIndex = 2;
      this.UsernameTextBox.TextChanged += new System.EventHandler(this.input_TextChanged);
      // 
      // PasswordTextBox
      // 
      this.PasswordTextBox.Location = new System.Drawing.Point(83, 138);
      this.PasswordTextBox.Name = "PasswordTextBox";
      this.PasswordTextBox.PasswordChar = '*';
      this.PasswordTextBox.Size = new System.Drawing.Size(100, 20);
      this.PasswordTextBox.TabIndex = 3;
      this.PasswordTextBox.TextChanged += new System.EventHandler(this.input_TextChanged);
      // 
      // ServerTextBox
      // 
      this.ServerTextBox.Location = new System.Drawing.Point(83, 18);
      this.ServerTextBox.Name = "ServerTextBox";
      this.ServerTextBox.Size = new System.Drawing.Size(307, 20);
      this.ServerTextBox.TabIndex = 0;
      this.ServerTextBox.TextChanged += new System.EventHandler(this.input_TextChanged);
      // 
      // PassiveCheckbox
      // 
      this.PassiveCheckbox.AutoSize = true;
      this.PassiveCheckbox.Checked = true;
      this.PassiveCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.PassiveCheckbox.Location = new System.Drawing.Point(83, 168);
      this.PassiveCheckbox.Name = "PassiveCheckbox";
      this.PassiveCheckbox.Size = new System.Drawing.Size(120, 17);
      this.PassiveCheckbox.TabIndex = 4;
      this.PassiveCheckbox.Text = "Passive Connection";
      this.PassiveCheckbox.UseVisualStyleBackColor = true;
      // 
      // PortLabel
      // 
      this.PortLabel.AutoSize = true;
      this.PortLabel.Location = new System.Drawing.Point(22, 80);
      this.PortLabel.Name = "PortLabel";
      this.PortLabel.Size = new System.Drawing.Size(26, 13);
      this.PortLabel.TabIndex = 7;
      this.PortLabel.Text = "Port";
      // 
      // ConnectButton
      // 
      this.ConnectButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.ConnectButton.Enabled = false;
      this.ConnectButton.Location = new System.Drawing.Point(315, 164);
      this.ConnectButton.Name = "ConnectButton";
      this.ConnectButton.Size = new System.Drawing.Size(75, 23);
      this.ConnectButton.TabIndex = 5;
      this.ConnectButton.Text = "Connect";
      this.ConnectButton.UseVisualStyleBackColor = true;
      this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
      // 
      // CancelButton
      // 
      this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.CancelButton.Location = new System.Drawing.Point(234, 164);
      this.CancelButton.Name = "CancelButton";
      this.CancelButton.Size = new System.Drawing.Size(75, 23);
      this.CancelButton.TabIndex = 6;
      this.CancelButton.Text = "Cancel";
      this.CancelButton.UseVisualStyleBackColor = true;
      // 
      // PortNumeric
      // 
      this.PortNumeric.Location = new System.Drawing.Point(83, 78);
      this.PortNumeric.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
      this.PortNumeric.Name = "PortNumeric";
      this.PortNumeric.Size = new System.Drawing.Size(65, 20);
      this.PortNumeric.TabIndex = 8;
      this.PortNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.PortNumeric.Value = new decimal(new int[] {
            21,
            0,
            0,
            0});
      // 
      // StartPathLabel
      // 
      this.StartPathLabel.AutoSize = true;
      this.StartPathLabel.Location = new System.Drawing.Point(22, 51);
      this.StartPathLabel.Name = "StartPathLabel";
      this.StartPathLabel.Size = new System.Drawing.Size(53, 13);
      this.StartPathLabel.TabIndex = 9;
      this.StartPathLabel.Text = "Start path";
      // 
      // StartPathTextBox
      // 
      this.StartPathTextBox.Location = new System.Drawing.Point(83, 48);
      this.StartPathTextBox.Name = "StartPathTextBox";
      this.StartPathTextBox.Size = new System.Drawing.Size(226, 20);
      this.StartPathTextBox.TabIndex = 10;
      this.StartPathTextBox.TextChanged += new System.EventHandler(this.input_TextChanged);
      // 
      // LoginDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(417, 203);
      this.ControlBox = false;
      this.Controls.Add(this.StartPathTextBox);
      this.Controls.Add(this.StartPathLabel);
      this.Controls.Add(this.PortNumeric);
      this.Controls.Add(this.CancelButton);
      this.Controls.Add(this.ConnectButton);
      this.Controls.Add(this.PortLabel);
      this.Controls.Add(this.PassiveCheckbox);
      this.Controls.Add(this.ServerTextBox);
      this.Controls.Add(this.PasswordTextBox);
      this.Controls.Add(this.UsernameTextBox);
      this.Controls.Add(this.PasswordLabel);
      this.Controls.Add(this.UsernameLabel);
      this.Controls.Add(this.ServerLabel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Name = "LoginDialog";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Load += new System.EventHandler(this.FtpFileDialogLogin_Load);
      ((System.ComponentModel.ISupportInitialize)(this.PortNumeric)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label ServerLabel;
    private System.Windows.Forms.Label UsernameLabel;
    private System.Windows.Forms.Label PasswordLabel;
    private System.Windows.Forms.TextBox UsernameTextBox;
    private System.Windows.Forms.TextBox PasswordTextBox;
    private System.Windows.Forms.TextBox ServerTextBox;
    private System.Windows.Forms.CheckBox PassiveCheckbox;
    private System.Windows.Forms.Label PortLabel;
    private System.Windows.Forms.Button ConnectButton;
    private System.Windows.Forms.Button CancelButton;
    private System.Windows.Forms.NumericUpDown PortNumeric;
    private System.Windows.Forms.Label StartPathLabel;
    private System.Windows.Forms.TextBox StartPathTextBox;
  }
}