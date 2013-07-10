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

namespace FishTank.UI.Graphical
{
    public partial class Form1 : Form
    {
        private Thread thread;
        private Tank tank;
        private GUITankAdapter adapter;
        private Graphics front;
        public Form1()
        {
            InitializeComponent();

            this.tank = new Tank(1000, 500);
            this.adapter = new GUITankAdapter(tank, this.ClientSize.Width, this.ClientSize.Height - this.menuStrip1.Height);
            this.front = this.CreateGraphics();
            this.thread = new Thread(new ThreadStart(Run));
            this.thread.Start();
        }
        private void Run()
        {
            Stopwatch clock = new Stopwatch();
            clock.Start();

            while (adapter.IsRunning)
            {
                adapter.Display(this.front, this.ClientRectangle.X, this.ClientRectangle.Y+this.menuStrip1.Height);
                adapter.ProcessInput();
                tank.Update((int)clock.ElapsedMilliseconds);
                clock.Reset();
                clock.Start();
                Application.DoEvents();
            }
        }
        private void Shutdown()
        {
            adapter.MessageQueue.Enqueue("exit");
            //wait for completion
            while (thread.IsAlive)
            {
                Application.DoEvents();
            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            this.Shutdown();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Shutdown();
            this.Close();
        }

        private void aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            adapter.MessageQueue.Enqueue("add fish");
        }

        private void addPumpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            adapter.MessageQueue.Enqueue("add pump");
        }

        private void addSnailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            adapter.MessageQueue.Enqueue("add snail");
        }

        private void cleanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            adapter.MessageQueue.Enqueue("clean");
        }

        private void emptyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            adapter.MessageQueue.Enqueue("empty");
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            adapter.MessageQueue.Enqueue("add plant");
        }

        private void feedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            adapter.MessageQueue.Enqueue("add 20 food");
        }

    }
}