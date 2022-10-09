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

namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Client client = new Client();
            client.Connect("127.0.0.1", 13000);
            
            
            Task task = new Task(
                () =>
                {
                    while (true)
                    {
                        Request request = new ScreenRequest();
                        JObject jObject = request.ToJson();
                        
                        if (!client.Send(jObject.ToString()))
                        {
                            break;
                        }
            
                    }
                }
            );
            task.Start();
            
        }
    

        private void timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            pictureBox1.Image = Data.Image;
        }
    }
}