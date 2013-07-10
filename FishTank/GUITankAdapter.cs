//Copyright, Sean T. McBeth, 2007
//sean.mcbeth@gmail.com
//www.seanmcbeth.com
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Drawing;
using FishTank.Common;

namespace FishTank.UI.Graphical
{

    public class GUITankAdapter : TankAdapter, IDisposable
    {
        private delegate void PaintingDelegate(TankObject obj);
        static GUITankAdapter()
        {
        }
        private Bitmap buffer, bubble, fish, food, plant, poop, pump;
        private Graphics gdi;
        private Dictionary<Type, PaintingDelegate> painters;
        private Queue<string> messageQueue;
        public GUITankAdapter(Tank tank, int width, int height)
            : base(tank, width, height)
        {
            this.messageQueue = new Queue<string>();
            this.buffer = new Bitmap(this.displayWidth, this.displayHeight);
            this.gdi = Graphics.FromImage(this.buffer);
            this.bubble = global::FishTank.Properties.Resources.bubble;
            Bubble.AvatarWidth = bubble.Width * tank.Width / this.displayWidth;
            Bubble.AvatarHeight = bubble.Height * tank.Height / this.displayHeight;
            this.fish = global::FishTank.Properties.Resources.fish;
            Fish.AvatarWidth = fish.Width * tank.Width / this.displayWidth;
            Fish.AvatarHeight = fish.Height * tank.Height / this.displayHeight;
            this.poop = global::FishTank.Properties.Resources.poop;
            Poop.AvatarWidth = poop.Width * tank.Width / this.displayWidth;
            Poop.AvatarHeight = poop.Height * tank.Height / this.displayHeight;
            this.food = global::FishTank.Properties.Resources.food;
            Food.AvatarWidth = food.Width * tank.Width / this.displayWidth;
            Food.AvatarHeight = food.Height * tank.Height / this.displayHeight;
            this.plant = global::FishTank.Properties.Resources.plant;
            Plant.AvatarWidth = plant.Width * tank.Width / this.displayWidth;
            Plant.AvatarHeight = plant.Height * tank.Height / this.displayHeight;
            this.pump = global::FishTank.Properties.Resources.pump;
            Pump.AvatarWidth = pump.Width * tank.Width / this.displayWidth;
            Pump.AvatarHeight = pump.Height * tank.Height / this.displayHeight;

            painters = new Dictionary<Type, PaintingDelegate>();
            painters.Add(typeof(Fish), new PaintingDelegate(PaintFish));
            painters.Add(typeof(Poop), new PaintingDelegate(PaintPoop));
            painters.Add(typeof(Food), new PaintingDelegate(PaintFood));
            painters.Add(typeof(Plant), new PaintingDelegate(PaintPlant));
            painters.Add(typeof(Pump), new PaintingDelegate(PaintPump));
            painters.Add(typeof(Bubble), new PaintingDelegate(PaintBubble));
        }

        public void Dispose()
        {
            this.gdi.Dispose();
            this.buffer.Dispose();
        }
        private void PaintBubble(TankObject obj)
        {
            this.gdi.DrawImage(this.bubble, obj.X * this.displayWidth / tank.Width, obj.Y * this.displayHeight / tank.Height);
        }
        private void PaintPump(TankObject obj)
        {
            this.gdi.DrawImage(this.pump, obj.X * this.displayWidth / tank.Width, obj.Y * this.displayHeight / tank.Height);
        }
        private void PaintFish(TankObject obj)
        {
            Fish f = obj as Fish;
            this.gdi.DrawImage(this.fish, obj.X * this.displayWidth / tank.Width, obj.Y * this.displayHeight / tank.Height);
        }
        private void PaintPoop(TankObject obj)
        {
            this.gdi.DrawImage(this.poop, obj.X * this.displayWidth / tank.Width, obj.Y * this.displayHeight / tank.Height);
        }
        private void PaintFood(TankObject obj)
        {
            this.gdi.DrawImage(this.food, obj.X * this.displayWidth / tank.Width, obj.Y * this.displayHeight / tank.Height);
        }
        public static int OOO = 20, PPP = 10;
        private void PaintPlant(TankObject obj)
        {
            int y = obj.Y * this.displayHeight / tank.Height;
            using (Bitmap b = new Bitmap(plant.Width, this.displayHeight - y))
            {
                using (Graphics g = Graphics.FromImage(b))
                {
                    for (int dy = -(int)(obj.Age % plant.Height); dy < b.Height; dy += plant.Height)
                    {
                        g.DrawImage(this.plant, 0, dy);
                    }
                    Pen p = new Pen(Color.Green, 5);
                    int l = -1, r = b.Width;
                    for (int i = 0; i < b.Width; ++i)
                    {
                        if (l == -1 && b.GetPixel(i, 0).G > 10)
                        {
                            l = i;
                        }
                        if (r == b.Width && b.GetPixel(b.Width - i - 1, 0).G > 10)
                        {
                            r = b.Width - i - 1;
                        }
                    }
                    g.DrawLine(p, l + 2, 0, r - 5, 0);
                }
                this.gdi.DrawImage(b, obj.X * this.displayWidth / tank.Width, y);
            }
        }
        protected override void EmptyTank(string[] args)
        {
            base.EmptyTank(args);
        }
        protected override void Clean(string[] args)
        {
            base.Clean(args);
        }
        protected override void Error(string message)
        {
            System.Windows.Forms.MessageBox.Show(message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation, System.Windows.Forms.MessageBoxDefaultButton.Button1);
        }
        public void Display(Graphics output, int x, int y)
        {
            this.gdi.Clear(Color.White);

            foreach (TankObject obj in tank.Objects)
            {
                if (painters.ContainsKey(obj.GetType()))
                {
                    painters[obj.GetType()](obj);
                }
            }
            output.DrawImage(this.buffer, x, y);
        }

        public override void ProcessInput()
        {
            if (this.messageQueue.Count > 0)
            {
                string input = this.messageQueue.Dequeue();
                string script = input.Trim();
                this.ExecuteScript(script);
            }
        }

        public Queue<string> MessageQueue
        {
            get
            {
                return this.messageQueue;
            }
        }
    }
}
