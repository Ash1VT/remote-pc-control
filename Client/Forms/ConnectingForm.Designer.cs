using System.ComponentModel;

namespace Client;

partial class ConnectingForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

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
        this.connectButton = new System.Windows.Forms.Button();
        this.label1 = new System.Windows.Forms.Label();
        this.label2 = new System.Windows.Forms.Label();
        this.label3 = new System.Windows.Forms.Label();
        this.label4 = new System.Windows.Forms.Label();
        this.label5 = new System.Windows.Forms.Label();
        this.firstIpBlock = new System.Windows.Forms.NumericUpDown();
        this.secondIpBlock = new System.Windows.Forms.NumericUpDown();
        this.thirdIpBlock = new System.Windows.Forms.NumericUpDown();
        this.fourthIpBlock = new System.Windows.Forms.NumericUpDown();
        this.port = new System.Windows.Forms.NumericUpDown();
        ((System.ComponentModel.ISupportInitialize)(this.firstIpBlock)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.secondIpBlock)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.thirdIpBlock)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.fourthIpBlock)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.port)).BeginInit();
        this.SuspendLayout();
        // 
        // connectButton
        // 
        this.connectButton.Font = new System.Drawing.Font("Verdana", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.connectButton.Location = new System.Drawing.Point(12, 146);
        this.connectButton.Name = "connectButton";
        this.connectButton.Size = new System.Drawing.Size(453, 68);
        this.connectButton.TabIndex = 0;
        this.connectButton.Text = "Connect";
        this.connectButton.UseVisualStyleBackColor = true;
        this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
        // 
        // label1
        // 
        this.label1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.label1.Location = new System.Drawing.Point(12, 29);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(55, 32);
        this.label1.TabIndex = 4;
        this.label1.Text = "IP:";
        // 
        // label2
        // 
        this.label2.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.label2.Location = new System.Drawing.Point(159, 29);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(22, 32);
        this.label2.TabIndex = 5;
        this.label2.Text = ":";
        // 
        // label3
        // 
        this.label3.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.label3.Location = new System.Drawing.Point(263, 29);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(22, 32);
        this.label3.TabIndex = 6;
        this.label3.Text = ":";
        // 
        // label4
        // 
        this.label4.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.label4.Location = new System.Drawing.Point(367, 29);
        this.label4.Name = "label4";
        this.label4.Size = new System.Drawing.Size(22, 32);
        this.label4.TabIndex = 7;
        this.label4.Text = ":";
        // 
        // label5
        // 
        this.label5.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.label5.Location = new System.Drawing.Point(12, 88);
        this.label5.Name = "label5";
        this.label5.Size = new System.Drawing.Size(66, 32);
        this.label5.TabIndex = 8;
        this.label5.Text = "Port:";
        // 
        // firstIpBlock
        // 
        this.firstIpBlock.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.firstIpBlock.Location = new System.Drawing.Point(84, 27);
        this.firstIpBlock.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
        this.firstIpBlock.Name = "firstIpBlock";
        this.firstIpBlock.Size = new System.Drawing.Size(70, 32);
        this.firstIpBlock.TabIndex = 10;
        // 
        // secondIpBlock
        // 
        this.secondIpBlock.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.secondIpBlock.Location = new System.Drawing.Point(187, 27);
        this.secondIpBlock.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
        this.secondIpBlock.Name = "secondIpBlock";
        this.secondIpBlock.Size = new System.Drawing.Size(70, 32);
        this.secondIpBlock.TabIndex = 11;
        // 
        // thirdIpBlock
        // 
        this.thirdIpBlock.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.thirdIpBlock.Location = new System.Drawing.Point(291, 27);
        this.thirdIpBlock.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
        this.thirdIpBlock.Name = "thirdIpBlock";
        this.thirdIpBlock.Size = new System.Drawing.Size(70, 32);
        this.thirdIpBlock.TabIndex = 12;
        // 
        // fourthIpBlock
        // 
        this.fourthIpBlock.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.fourthIpBlock.Location = new System.Drawing.Point(395, 27);
        this.fourthIpBlock.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
        this.fourthIpBlock.Name = "fourthIpBlock";
        this.fourthIpBlock.Size = new System.Drawing.Size(70, 32);
        this.fourthIpBlock.TabIndex = 13;
        // 
        // port
        // 
        this.port.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.port.Location = new System.Drawing.Point(84, 86);
        this.port.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
        this.port.Name = "port";
        this.port.Size = new System.Drawing.Size(138, 32);
        this.port.TabIndex = 14;
        // 
        // ConnectingForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(479, 236);
        this.Controls.Add(this.port);
        this.Controls.Add(this.fourthIpBlock);
        this.Controls.Add(this.thirdIpBlock);
        this.Controls.Add(this.secondIpBlock);
        this.Controls.Add(this.firstIpBlock);
        this.Controls.Add(this.label5);
        this.Controls.Add(this.label4);
        this.Controls.Add(this.label3);
        this.Controls.Add(this.label2);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.connectButton);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "ConnectingForm";
        this.Text = "Connecting";
        this.Load += new System.EventHandler(this.ConnectingForm_Load);
        ((System.ComponentModel.ISupportInitialize)(this.firstIpBlock)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.secondIpBlock)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.thirdIpBlock)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.fourthIpBlock)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.port)).EndInit();
        this.ResumeLayout(false);
    }

    private System.Windows.Forms.NumericUpDown port;

    private System.Windows.Forms.NumericUpDown firstIpBlock;
    private System.Windows.Forms.NumericUpDown thirdIpBlock;
    private System.Windows.Forms.NumericUpDown fourthIpBlock;

    private System.Windows.Forms.NumericUpDown secondIpBlock;

    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;

    private System.Windows.Forms.Label label1;

    private System.Windows.Forms.Button connectButton;

    #endregion
}