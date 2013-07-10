using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptedSystem
{
    public delegate void CommandDelegate(string[] args);
    public abstract class ScriptHost
    {
        protected Dictionary<string, CommandDelegate> commands;
        public Dictionary<string, CommandDelegate> subCommands;
        protected bool isRunning;
        public ScriptHost()
        {
            this.isRunning = true;
            this.commands = new Dictionary<string, CommandDelegate>();
            this.subCommands = new Dictionary<string, CommandDelegate>();
            this.AddCommand("exit", new CommandDelegate(Exit), false);
            this.AddCommand("set", new CommandDelegate(this.Set), false);
            this.AddCommand("set_speed", new CommandDelegate(this.SetSpeed), true);
        }
        private void SetSpeed(string[] args)
        {
            int speed = int.Parse(args[0]);
            SystemVariables.Vars["SPEED"] = speed;
        }
        private void Set(string[] args)
        {
            if (args.Length != 2)
            {
                Error("ERROR: Argument for SET command: <variable name> <value>");
            }
            else
            {
                string subCommandKey = null;
                string value = null;
                subCommandKey = string.Format("set_{0}", args[0].ToLower());
                value = args[1];

                if (this.subCommands.ContainsKey(subCommandKey))
                {
                    this.subCommands[subCommandKey](new string[] { value });
                }
                else
                {
                    Error(string.Format("Unknown variable name: {0}", args[0].ToUpper()));
                }
            }
        }

        private void Exit(string[] args)
        {
            this.isRunning = false;
        }
        public void AddCommand(string key, CommandDelegate command, bool sub)
        {
            if (sub)
                this.subCommands.Add(key, command);
            else
                this.commands.Add(key, command);
        }
        protected abstract void Error(string message);
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
                        args[i - 1] = parts[i];
                }
                this.ExecuteCommand(commandKey, args, false);
            }
        }
        protected void ExecuteCommand(string commandKey, string[] args, bool includePrivate)
        {
            if (this.commands.ContainsKey(commandKey))
                this.commands[commandKey](args);
            else if (includePrivate && this.subCommands.ContainsKey(commandKey))
                this.subCommands[commandKey](args);
            else
                this.Error(string.Format("Unknownk command: {0}", commandKey.ToUpper()));
        }
        public bool IsRunning
        {
            get
            {
                return this.isRunning;
            }
        }
        public abstract void ProcessInput();
    }
}
