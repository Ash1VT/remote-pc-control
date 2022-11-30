using System;
using System.Windows.Forms;

namespace Client;

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
        // if (String.IsNullOrWhiteSpace(firstIpBlock.Value) || String.IsNullOrWhiteSpace(secondIpBlockTextBox.Text)
        //     || String.IsNullOrWhiteSpace(thirdIpBlockTextBox.Text) || String.IsNullOrWhiteSpace(fourthIpBlockTextBox.Text) 
        //     || String.IsNullOrWhiteSpace(portTextBox.Text))
        // {
        //     MessageBox.Show("Some fields are not filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //     return;
        // }

        string serverAddress = $"{firstIpBlock.Value}.{secondIpBlock.Value}.{thirdIpBlock.Value}.{fourthIpBlock.Value}";
        int serverPort = (int)port.Value;

        try
        {
            _client.Connect(serverAddress, serverPort);
            
            MainForm mainForm = new MainForm(_client);
            mainForm.Show();
            
            
            
        }
        catch
        {
            MessageBox.Show(
                "Cannot connect to server, make sure server credentials are correct and server is available", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}