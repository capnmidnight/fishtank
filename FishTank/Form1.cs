//Copyright, Sean T. McBeth, 2007
//sean.mcbeth@gmail.com
//www.seanmcbeth.com
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using FishTank.Common;
using FishTank.UI.Graphical;

namespace FishTank
{
    public partial class Form1 : Form
    {
        private delegate void DrawOp();
        private DrawOp draw;
        private Thread thread;
        private Tank tank;
        private GUITankAdapter adapter;
        private Graphics front;
        public Form1()
        {
            InitializeComponent();
            this.tank = new Tank(1000, 500);
            this.adapter = new GUITankAdapter(tank, this.ClientSize.Width, this.ClientSize.Height);
            this.front = this.CreateGraphics();
            this.thread = new Thread(new ThreadStart(Run));
            this.thread.Start();
            this.draw = new DrawOp(this.Draw);
        }
        private void Run()
        {
            Stopwatch clock = new Stopwatch();
            clock.Start();

            while (adapter.IsRunning)
            {
                this.Invoke(this.draw);
                adapter.ProcessInput();
                tank.Update((int)clock.ElapsedMilliseconds);
                clock.Reset();
                clock.Start();
                Application.DoEvents();
            }
        }
        private void Draw()
        {
            adapter.Display(this.front, this.ClientRectangle.X, this.ClientRectangle.Y);
        }
        private void menuItem3_Click(object sender, EventArgs e)
        {
            adapter.MessageQueue.Enqueue("add 20 food");
        }
        private void Shutdown()
        {
            adapter.MessageQueue.Enqueue("exit");
            //wait for completion
            DateTime s = DateTime.Now;
            while ((DateTime.Now - s).TotalMilliseconds < 2000)
            {
                Application.DoEvents();
            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            this.Shutdown();
            Application.Exit();
        }

        private void menuItem9_Click(object sender, EventArgs e)
        {
            this.Shutdown();
            this.Close();
        }

        private void menuItem4_Click(object sender, EventArgs e)
        {
            adapter.MessageQueue.Enqueue("add fish");
        }

        private void menuItem5_Click(object sender, EventArgs e)
        {
            adapter.MessageQueue.Enqueue("add pump");
        }

        private void menuItem6_Click(object sender, EventArgs e)
        {
            adapter.MessageQueue.Enqueue("add plant");
        }

        private void menuItem7_Click(object sender, EventArgs e)
        {
            adapter.MessageQueue.Enqueue("clean");
        }

        private void menuItem8_Click(object sender, EventArgs e)
        {
            adapter.MessageQueue.Enqueue("empty");
        }
    }
}