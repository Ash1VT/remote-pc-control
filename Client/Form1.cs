﻿using System;
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
        private KeyboardHook _keyboardHook;
        public Form1(Client client)
        {
            InitializeComponent();
            _client = client;
            _keyboardHook = new KeyboardHook();
            _keyboardHook.unhook();
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            screenPictureBox.MouseEnter += screenPictureBox_MouseEnter;
            screenPictureBox.MouseLeave += screenPictureBox_MouseLeave;
            
            screenPictureBox.MouseMove += screenPictureBox_MouseMove;
            screenPictureBox.MouseDown += screenPictureBox_MouseDown;
            screenPictureBox.MouseUp += screenPictureBox_MouseUp;

            _keyboardHook.KeyDown += hook_KeyDown;
            _keyboardHook.KeyUp += hook_KeyUp;
            Data.XCoefficient = 1920.0 / screenPictureBox.Width;
            Data.YCoefficient = 1080.0 / screenPictureBox.Height;
            ScreenManager.ScreenChanged += screen_Changed;
            
        }

        private void hook_KeyDown(object sender, KeyEventArgs e)
        {
            if (ActiveControl != screenPictureBox)
                return;
            
            // Console.WriteLine($"Down {e.KeyValue}");
            e.Handled = true;
            KeyboardButtonDownRequest keyboardButtonDownRequest = new KeyboardButtonDownRequest(e.KeyValue);
            _client.SendRequest(keyboardButtonDownRequest);
        }
        
        
        private void hook_KeyUp(object sender, KeyEventArgs e)
        {
            if (ActiveControl != screenPictureBox)
                return;
            
            // Console.WriteLine($"Up {e.KeyValue}");
            e.Handled = true;
            
            KeyboardButtonUpRequest keyboardButtonUpRequest = new KeyboardButtonUpRequest(e.KeyValue);
            _client.SendRequest(keyboardButtonUpRequest);
        }

        
        private void screenPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDownRequest mouseDownRequest = new MouseDownRequest((int)(e.X * Data.XCoefficient),
                (int)(e.Y * Data.YCoefficient), e.Button.ToString());
            _client.SendRequest(mouseDownRequest);
        }
        
        private void screenPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            MouseUpRequest mouseUpRequest = new MouseUpRequest((int)(e.X * Data.XCoefficient),
                (int)(e.Y * Data.YCoefficient), e.Button.ToString());
            _client.SendRequest(mouseUpRequest);
        }
        
        
        private void screenPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            MouseCoordinatesRequest mouseCoordinatesRequest = new MouseCoordinatesRequest((int)(e.X * Data.XCoefficient), (int)(e.Y * Data.YCoefficient));
            _client.SendRequest(mouseCoordinatesRequest);
        }

        private void screenPictureBox_MouseEnter(object sender, EventArgs e)
        {
            _keyboardHook.hook();
            ActiveControl = screenPictureBox;
        }
        
        private void screenPictureBox_MouseLeave(object sender, EventArgs e)
        {
            _keyboardHook.unhook();
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
    }
}