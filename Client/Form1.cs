using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Client.Requests;
using Newtonsoft.Json.Linq;
using Task = System.Threading.Tasks.Task;

namespace Client
{
    public partial class Form1 : Form
    {

        private Client _client;
        public Form1(Client client)
        {
            InitializeComponent();
            _client = client;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Data.XCoefficient = 1920.0 / pictureBox1.Width;
            Data.YCoefficient = 1080.0 / pictureBox1.Height;
            ScreenManager.ScreenChanged += screen_Changed;
            
        }

        private bool update = false;
        

        private int x;
        private int y;
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            x = e.X;
            y = e.Y;
            MouseCoordinatesRequest mouseCoordinatesRequest = new MouseCoordinatesRequest(e.X, e.Y);
            _client.SendRequest(mouseCoordinatesRequest);
        }

        private void screen_Changed(Image screen)
        {
            pictureBox1.Image = screen;
        }
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            Cursor.Hide();
            update = true;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            Cursor.Show();
            update = false;
        }
    }
}