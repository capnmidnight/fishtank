using System;
using System.Collections.Generic;
using System.Text;

namespace FishTank.Common
{
    public abstract class ScriptHost
    {
        protected Dictionary<string, CommandDelegate> commands;
        protected Dictionary<string, CommandDelegate> subCommands;
        protected Tank tank;
        protected Random prand;

        public ScriptHost(Tank tank)
        {
            this.tank = tank;
            this.prand = new Random();
            this.commands = new Dictionary<string, CommandDelegate>();
            this.subCommands = new Dictionary<string, CommandDelegate>();

            this.AddCommand("add", new CommandDelegate(this.Add), false);
            this.AddCommand("clean", new CommandDelegate(this.Clean), false);
            this.AddCommand("empty", new CommandDelegate(this.EmptyTank), false);
            this.AddCommand("add_fish", new CommandDelegate(this.AddFish), true);
            this.AddCommand("add_food", new CommandDelegate(this.AddFood), true);
            this.AddCommand("add_plant", new CommandDelegate(this.AddPlant), true);
            this.AddCommand("add_plants", new CommandDelegate(this.AddPlant), true);
            this.AddCommand("add_snail", new CommandDelegate(this.AddSnail), true);
            this.AddCommand("add_snails", new CommandDelegate(this.AddSnail), true);
            this.AddCommand("add_pump", new CommandDelegate(this.AddPump), true);
        }
        protected void AddCommand(string key, CommandDelegate command, bool sub)
        {
            if (sub)
            {
                this.subCommands.Add(key, command);
            }
            else
            {
                this.commands.Add(key, command);
            }
        }
        public void ExecuteScript(string script)
        {
            string[] parts = null;
            if (script.Length > 0)
            {
                parts = script.Split(' ');
                string commandKey = parts[0].ToLower();
                string[] args = null;
                if (parts.Length > 1)
                {
                    args = new string[parts.Length - 1];
                    for (int i = 1; i < parts.Length; ++i)
                    {
                        args[i - 1] = parts[i];
                    }
                }
                this.ExecuteCommand(commandKey, args, false);
            }
        }
        protected void ExecuteCommand(string commandKey, string[] args, bool includePrivate)
        {
            if (this.commands.ContainsKey(commandKey))
            {
                CommandDelegate command = this.commands[commandKey];
                command(args);
            }
            else if (includePrivate && this.subCommands.ContainsKey(commandKey))
            {
                CommandDelegate command = this.subCommands[commandKey];
                command(args);
            }
            else
            {
                this.Error(string.Format("Unknownk command: {0}", commandKey.ToUpper()));
            }
        }

        private void EmptyTank(string[] args)
        {
            this.tank.Objects.Clear();
            this.tank.Dirt = 0;
        }
        private void AddFish(string[] args)
        {
            tank.Objects.Add(new Fish(this.prand.Next(tank.Width), this.prand.Next(tank.Height)));
        }
        private void AddFood(string[] args)
        {
            tank.Objects.Add(new Food(this.prand.Next(tank.Width)));
        }
        private void AddPlant(string[] args)
        {
            tank.Objects.Add(new Plant(this.prand.Next(tank.Width), this.prand.Next(tank.Height)));
        }
        private void AddSnail(string[] args)
        {
            tank.Objects.Add(new Snail(this.prand.Next(tank.Width), this.prand.Next(tank.Height)));
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

        private void Clean(string[] args)
        {
            if (tank.Dirt > 0)
            {
                List<TankObject> kill = new List<TankObject>();
                foreach (TankObject obj in tank.Objects)
                {
                    if (obj is Poop || obj is Food || obj is Algae)
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
            }
        }
        protected abstract void Error(string message);
    }
}
