using System;
using System.Drawing;
using System.Windows.Forms;
using Client.Hooks;
using Client.Requests;

namespace Client.Forms
{
    public partial class MainForm : Form
    {
        private Client _client;
        private KeyboardHook _keyboardHook;

        private double _pictureBoxToScreenRelationX { get; set; }
        private double _pictureBoxToScreenRelationY { get; set; }
        public MainForm(Client client)
        {
            InitializeComponent();
            _client = client;
            _keyboardHook = new KeyboardHook();
            _keyboardHook.Unhook();
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            _client.StartAcceptResponses();
            this.KeyPreview = true;
            screenPictureBox.MouseEnter += screenPictureBox_MouseEnter;
            screenPictureBox.MouseLeave += screenPictureBox_MouseLeave;
            
            screenPictureBox.MouseMove += screenPictureBox_MouseMove;
            screenPictureBox.MouseDown += screenPictureBox_MouseDown;
            screenPictureBox.MouseUp += screenPictureBox_MouseUp;

            _keyboardHook.KeyDown += hook_KeyDown;
            _keyboardHook.KeyUp += hook_KeyUp;
            _pictureBoxToScreenRelationX = 1920.0 / screenPictureBox.Width;
            _pictureBoxToScreenRelationY = 1080.0 / screenPictureBox.Height;
            ScreenChanger.ScreenChanged += screen_Changed;
            
        }

        private void hook_KeyDown(object sender, KeyEventArgs e)
        {
            if (ActiveControl != screenPictureBox)
                return;
            
            e.Handled = true;
            KeyboardButtonDownRequest keyboardButtonDownRequest = new KeyboardButtonDownRequest(e.KeyValue);
            _client.SendRequest(keyboardButtonDownRequest);
        }
        
        
        private void hook_KeyUp(object sender, KeyEventArgs e)
        {
            if (ActiveControl != screenPictureBox)
                return;
            
            e.Handled = true;
            
            KeyboardButtonUpRequest keyboardButtonUpRequest = new KeyboardButtonUpRequest(e.KeyValue);
            _client.SendRequest(keyboardButtonUpRequest);
        }

        
        private void screenPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDownRequest mouseDownRequest = new MouseDownRequest((int)(e.X * _pictureBoxToScreenRelationX),
                (int)(e.Y * _pictureBoxToScreenRelationY), e.Button.ToString());
            _client.SendRequest(mouseDownRequest);
        }
        
        private void screenPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            MouseUpRequest mouseUpRequest = new MouseUpRequest((int)(e.X * _pictureBoxToScreenRelationX),
                (int)(e.Y * _pictureBoxToScreenRelationY), e.Button.ToString());
            _client.SendRequest(mouseUpRequest);
        }
        
        
        private void screenPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            MouseCoordinatesRequest mouseCoordinatesRequest = new MouseCoordinatesRequest((int)(e.X * _pictureBoxToScreenRelationX), (int)(e.Y * _pictureBoxToScreenRelationY));
            _client.SendRequest(mouseCoordinatesRequest);
        }

        private void screenPictureBox_MouseEnter(object sender, EventArgs e)
        {
            _keyboardHook.Hook();
            ActiveControl = screenPictureBox;
        }
        
        private void screenPictureBox_MouseLeave(object sender, EventArgs e)
        {
            _keyboardHook.Unhook();
            ActiveControl = null;
        }

        private void screen_Changed(Image screen)
        {
            screenPictureBox.Image = screen;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            return true;
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _client.Disconnect();
        }
    }
}