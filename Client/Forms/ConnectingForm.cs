using System;
using System.Windows.Forms;

namespace Client.Forms;

public partial class ConnectingForm : Form
{
    private Client _client;
    
    public ConnectingForm(Client client)
    {
        InitializeComponent();
        _client = client;
    }

    private void ConnectingForm_Load(object sender, EventArgs e)
    {
        
        
    }



    private void connectButton_Click(object sender, EventArgs e)
    {
        string serverAddress = $"{firstIpBlock.Value}.{secondIpBlock.Value}.{thirdIpBlock.Value}.{fourthIpBlock.Value}";
        int serverPort = (int)port.Value;

        try
        {
            _client.Connect(serverAddress, serverPort);
            
            MainForm mainForm = new MainForm(_client);
            this.Visible = false;
            mainForm.ShowDialog();
            this.Visible = true;

        }
        catch
        {
            MessageBox.Show(
                "Cannot connect to server, make sure server credentials are correct and server is available", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}