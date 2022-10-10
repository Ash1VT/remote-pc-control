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

        private Client _client = new Client();
        private Client _client1 = new Client();
        private Client _client2 = new Client();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Data.XCoefficient = 1920.0 / pictureBox1.Width;
            Data.YCoefficient = 1080.0 / pictureBox1.Height;

            _client.Connect("192.168.0.102", 13000);
            _client1.Connect("192.168.0.102", 13000);
            _client2.Connect("192.168.0.102", 13000);
            
            
            Task task = new Task(
                () =>
                {
                    while (true)
                    {
                        Request request = new ScreenRequest();
                        
                        
                        if (!_client.Send(request))
                        {
                            break;
                        }
            
                    }
                }
            );
            task.Start();
            
            Task task1 = new Task(
                () =>
                {
                    while (true)
                    {
                        Request request = new ScreenRequest();
                        
                        
                        if (!_client1.Send(request))
                        {
                            break;
                        }
            
                    }
                }
            );
            task1.Start();

            Task task2 = new Task(
                () =>
                {
                    while (true)
                    {
                        Request request = new ScreenRequest();
                        
                        
                        if (!_client2.Send(request))
                        {
                            break;
                        }
            
                    }
                }
            );
            task2.Start();
               
        }
    

        private void timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("UPDATE");
            pictureBox1.Image = Data.Image;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
       
            int x = (int)(e.X * Data.XCoefficient);
            int y = (int)(e.Y * Data.YCoefficient);

            _client.Send(new MouseCoordinatesRequest(x, y));
        }
    }
}