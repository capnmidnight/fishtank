using System;
using System.Collections.Generic;
using System.Text;
using ScriptedSystem;

namespace ConsoleHelper
{
    public class TextAdapter
    {
        public ConsoleBuffer buffer;
        ScriptHost host;
        public TextAdapter(ScriptHost host, int width, int height)
        {
            this.buffer = new ConsoleBuffer(width, height);
            this.host = host;
            this.host.AddCommand("clrscr", new CommandDelegate(this.Clear), false);
            this.host.AddCommand("get", new CommandDelegate(this.Get), false);
        }
        private void Get(string[] args)
        {
            if (args.Length != 1)
            {
                Error("ERROR: Argument for GET command: <variable name>");
            }
            else
            {
                string subCommandKey = null;
                subCommandKey = string.Format("get_{0}", args[0].ToLower());

                if (this.host.subCommands.ContainsKey(subCommandKey))
                {
                    this.host.subCommands[subCommandKey](null);
                }
                else
                {
                    Error(string.Format("Unknown variable name: {0}", args[0].ToUpper()));
                }
            }
        }
        private void Clear(string[] args)
        {
            this.buffer.Clear();
        }
        public void Error(string message)
        {
            this.buffer.Prompt(message, true);
        }
        public void ProcessInput()
        {
            if (Console.KeyAvailable)
            {
                this.buffer.Prompt("[PAUSED, enter command] :> ", false);
                string input = Console.ReadLine();
                this.buffer.CorrectInputBuffer(input);
                string script = input.Trim();
                host.ExecuteScript(script);
            }
        }
    }
}
