//Copyright, Sean T. McBeth, 2007
//sean.mcbeth@gmail.com
//www.seanmcbeth.com
using System;
using System.Collections.Generic;
using ScriptedSystem;

namespace FishTank.Common
{
    public abstract class TankAdapter : ScriptHost
    {
        protected Tank tank;
        protected Random prand;
        protected int displayWidth, displayHeight;

        public TankAdapter(Tank tank, int displayWidth, int displayHeight)
        {
            this.displayWidth = displayWidth;
            this.displayHeight = displayHeight;
            this.tank = tank;
            this.prand = new Random();

            this.AddCommand("add", new CommandDelegate(this.Add), false);
            this.AddCommand("clean", new CommandDelegate(this.Clean), false);
            this.AddCommand("empty", new CommandDelegate(this.EmptyTank), false);
            this.AddCommand("add_fish", new CommandDelegate(this.AddFish), true);
            this.AddCommand("add_food", new CommandDelegate(this.AddFood), true);
            this.AddCommand("add_plant", new CommandDelegate(this.AddPlant), true);
            this.AddCommand("add_plants", new CommandDelegate(this.AddPlant), true);
            this.AddCommand("add_pump", new CommandDelegate(this.AddPump), true);
        }

        protected virtual void EmptyTank(string[] args)
        {
            this.tank.Objects.Clear();
            this.tank.Dirt = 0;
            //this.tank.Algae.Clean();
        }
        private void AddFish(string[] args)
        {
            tank.Objects.Add(new Fish(this.prand.Next(tank.Width - Fish.AvatarWidth), this.prand.Next(tank.Height - Fish.AvatarHeight)));
        }
        private void AddFood(string[] args)
        {
            tank.Objects.Add(new Food(this.prand.Next(tank.Width)));
        }
        private void AddPlant(string[] args)
        {
            tank.Objects.Add(new Plant(this.prand.Next(tank.Width), this.prand.Next(tank.Height)));
        }
        private void AddPump(string[] args)
        {
            tank.Objects.Add(new Pump());
        }

        private void Add(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Error("Argument needed for ADD command: object to add");
            }
            else
            {
                string subCommandKey = null;
                int count = 1;
                if (args.Length == 1)
                {
                    subCommandKey = string.Format("add_{0}", args[0].ToLower());
                }
                else if (args.Length == 2)
                {
                    subCommandKey = string.Format("add_{0}", args[1].ToLower());
                    try
                    {
                        count = int.Parse(args[0]);
                    }
                    catch (System.Exception)
                    {
                        Error("Count argument invalid number");
                        count = 0;
                    }
                }
                if (this.subCommands.ContainsKey(subCommandKey))
                {
                    CommandDelegate subCommand = this.subCommands[subCommandKey];
                    for (; count > 0; --count)
                    {
                        subCommand(null);
                    }
                }
                else
                {
                    Error(string.Format("Unknown item type: {0}", args[0]));
                }
            }
        }

        public int DisplayWidth
        {
            get
            {
                return this.displayWidth;
            }
        }
        public int DisplayHeight
        {
            get
            {
                return this.displayHeight;
            }
        }

        public double ScaleX
        {
            get
            {
                return (double)this.displayWidth / (double)this.tank.Width;
            }
        }

        public double ScaleY
        {
            get
            {
                return (double)this.displayHeight / (double)this.tank.Height;
            }
        }
        protected virtual void Clean(string[] args)
        {
            if (tank.Dirt > 0)
            {
                List<TankObject> kill = new List<TankObject>();
                foreach (TankObject obj in tank.Objects)
                {
                    if (obj is Poop || obj is Food)
                    {
                        kill.Add(obj);
                    }
                }
                foreach (TankObject obj in kill)
                {
                    tank.Objects.Remove(obj);
                }
                kill.Clear();
                tank.Dirt = 0;
                //tank.Algae.Clean();
            }
        }
    }
}
