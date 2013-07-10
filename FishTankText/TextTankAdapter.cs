//Copyright, Sean T. McBeth, 2007
//sean.mcbeth@gmail.com
//www.seanmcbeth.com
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using FishTank.Common;
using ConsoleHelper;
using ScriptedSystem;

namespace FishTank.UI.Text
{

    public class TextTankAdapter : TankAdapter
    {
        private delegate void PrintingDelegate(TankObject obj);
        private static string[] _Prefix;
        private static Dictionary<Type, string> _Singular, _Plural;


        static TextTankAdapter()
        {
            _Prefix = new string[] { "The tank contains", "There are", "Upon looking in the fishtank, you find", "Where did this stuff come from? It's", "It's", "Using your psychic powers, you sense" };
            _Singular = new Dictionary<Type, string>();
            _Singular.Add(typeof(Fish), "fish");
            _Singular.Add(typeof(Poop), "poop");
            _Singular.Add(typeof(Food), "food");
            _Singular.Add(typeof(Plant), "plant");
            _Singular.Add(typeof(Snail), "snail");

            _Plural = new Dictionary<Type, string>();
            _Plural.Add(typeof(Fish), "fish");
            _Plural.Add(typeof(Poop), "turds");
            _Plural.Add(typeof(Food), "foods");
            _Plural.Add(typeof(Plant), "plants");
            _Plural.Add(typeof(Snail), "snails");
        }

        private bool isVerbose;
        private TextAdapter text;
        private Dictionary<Type, PrintingDelegate> printers;
        private string top;
        public TextTankAdapter(Tank tank, int width, int height)
            : base(tank, width, height)
        {
            this.isVerbose = true;
            this.isRunning = true;
            this.AddCommand("ascii", new CommandDelegate(this.Show), false);
            this.AddCommand("verbose", new CommandDelegate(this.Verbose), false);
            this.AddCommand("help", new CommandDelegate(this.Help), false);
            this.text = new TextAdapter(this, Console.WindowWidth, Console.WindowHeight - 1);
            top = "";
            for (int i = 0; i < width; ++i)
                top += "~";
            printers = new Dictionary<Type, PrintingDelegate>();
            printers.Add(typeof(Fish), new PrintingDelegate(PrintFish));
            printers.Add(typeof(Poop), new PrintingDelegate(PrintPoop));
            printers.Add(typeof(Food), new PrintingDelegate(PrintFood));
            printers.Add(typeof(Plant), new PrintingDelegate(PrintPlant));
            printers.Add(typeof(Pump), new PrintingDelegate(PrintPump));
            printers.Add(typeof(Bubble), new PrintingDelegate(PrintBubble));
            //printers.Add(typeof(Algae), new PrintingDelegate(PrintAlgae));
            printers.Add(typeof(Snail), new PrintingDelegate(PrintSnail));
        }
        private void PrintAlgae(TankObject obj)
        {
            this.text.buffer.Set(obj.X, obj.Y, ConsoleColor.DarkGreen);
        }
        private void PrintBubble(TankObject obj)
        {
            char c = '.';
            if (obj.Age > 10)
                c = 'o';
            if (obj.Age > 20)
                c = 'O';
            this.text.buffer.Set(obj.X, obj.Y, c, ConsoleColor.Cyan);
        }
        private void PrintPump(TankObject obj)
        {
            this.text.buffer.Set(obj.X, obj.Y, "ooo", ConsoleColor.Gray, ConsoleColor.DarkGray);
            this.text.buffer.Set(obj.X, obj.Y + 1, "ooo", ConsoleColor.Gray, ConsoleColor.DarkGray);
            for (int y = obj.Y + 2; y < tank.Height - 1; ++y)
            {
                this.text.buffer.Set(obj.X + 1, y, ':', ConsoleColor.Gray, ConsoleColor.DarkGray);
            }
        }
        private void PrintFish(TankObject obj)
        {
            this.text.buffer.Set(obj.X * this.displayWidth / tank.Width, obj.Y * this.displayHeight / tank.Height, ">=>", ConsoleColor.Cyan);
        }
        private void PrintPoop(TankObject obj)
        {
            this.text.buffer.Set(obj.X, obj.Y, '~', ConsoleColor.DarkYellow);
        }
        private void PrintFood(TankObject obj)
        {
            this.text.buffer.Set(obj.X, obj.Y, "#", ConsoleColor.Green);
        }
        private void PrintSnail(TankObject obj)
        {
            this.text.buffer.Set(obj.X, obj.Y, "@", ConsoleColor.Yellow);
        }
        private void PrintPlant(TankObject obj)
        {
            for (int y = obj.Y; y < Console.WindowHeight; ++y)
            {
                this.text.buffer.Set(obj.X, y, (((y + obj.Age) % 2 == 0) ? '/' : '\\'), ConsoleColor.Green, ConsoleColor.DarkGreen);
            }
        }
        private void Help(string[] args)
        {
            this.text.buffer.Set(0, 0, "C# Console FishTank", ConsoleColor.Gray, ConsoleColor.Black);
            this.text.buffer.Set(0, 1, "  Typing will initiate the command prompt", ConsoleColor.Gray, ConsoleColor.Black);
            this.text.buffer.Set(0, 2, "  Commands:", ConsoleColor.Gray, ConsoleColor.Black);
            this.text.buffer.Set(0, 3, "    empty -> reset the tank to an empty state", ConsoleColor.Gray, ConsoleColor.Black);
            this.text.buffer.Set(0, 4, "    exit -> end the simulation", ConsoleColor.Gray, ConsoleColor.Black);
            this.text.buffer.Set(0, 5, "    add [count] <item_name> -> add an item, with an optional count parameter", ConsoleColor.Gray, ConsoleColor.Black);
            this.text.buffer.Set(0, 6, "      available items: fish, food, plant, snail, pump", ConsoleColor.Gray, ConsoleColor.Black);
            this.text.buffer.Set(0, 7, "    clean -> remove dirt generating items: old food, poop, algae", ConsoleColor.Gray, ConsoleColor.Black);
            this.text.buffer.Set(0, 8, "    set <var_name> <var_value> -> set a system variable value", ConsoleColor.Gray, ConsoleColor.Black);
            this.text.buffer.Set(0, 9, "      available variables: speed (in milliseconds)", ConsoleColor.Gray, ConsoleColor.Black);
            this.text.buffer.Set(0, 10, "	clrscr -> clears the screen", ConsoleColor.Gray, ConsoleColor.Black);
            this.text.buffer.Set(0, 11, "    ascii -> switch to ASCII mode graphics", ConsoleColor.Gray, ConsoleColor.Black);
            this.text.buffer.Set(0, 12, "    verbose -> switch to verbose mode (default)", ConsoleColor.Gray, ConsoleColor.Black);
            this.text.buffer.Set(0, 13, "    help -> show this help message", ConsoleColor.Gray, ConsoleColor.Black);
            this.text.buffer.Prompt("Press any key to begin.", true);
            this.text.buffer.Clear();
            this.text.buffer.Flush();
        }
        private void Show(string[] args)
        {
            this.isVerbose = false;
        }
        private void Verbose(string[] args)
        {
            this.isVerbose = true;
        }
        public bool IsVerbose
        {
            get
            {
                return this.isVerbose;
            }
        }
        public void Print()
        {

            if (isVerbose)
            {
                this.text.buffer.Clear(ConsoleColor.Black);
                this.VerbosePrint();
            }
            else
            {
                this.text.buffer.Clear(ConsoleColor.Black);
                this.ASCIIPrint();
            }
            this.text.buffer.Set(0, tank.Height - 1, tank.Dirt.ToString(), ConsoleColor.Yellow, ConsoleColor.DarkYellow);
            this.text.buffer.Flush();
        }
        private void VerbosePrint()
        {
            System.IO.StringWriter output = new System.IO.StringWriter();

            Dictionary<string, int> counts = this.CountObjects();
            if (counts.Count > 0)
            {
                int msgId = this.prand.Next(TextTankAdapter._Prefix.Length);
                output.Write("{0} ", TextTankAdapter._Prefix[msgId]);
                string last = null;
                int commaCount = 0;
                foreach (string descrip in counts.Keys)
                {
                    if (last != null)
                    {
                        output.Write("{0}, ", last);
                        //Track the number of commas, because using only 1 is bad grammar
                        commaCount++;
                    }
                    last = string.Format("{0} {1}", (counts[descrip] == 1) ? "a" : counts[descrip].ToString(), descrip);
                }

                //Remove the single comma and replace it with just a space. This gives us the proper
                // handling of conjuctive sequences as defined by Standard English.
                if (commaCount == 1)
                {
                    output.Write("\b\b ");
                }
                if (counts.Count > 1)
                {
                    output.Write("and ");
                }
                output.Write("{0}. ", last);

                if (tank.Dirt > 0)
                {
                    if (tank.Dirt < 33)
                    {
                        output.Write("The tank is slightly dirty.");
                    }
                    else if (tank.Dirt < 67)
                    {
                        output.Write("The tank is dirty.");
                    }
                    else
                    {
                        output.Write("The tank is very dirty!");
                    }
                }
            }
            else
            {
                output.Write("The fish tank is empty");
            }
            this.text.buffer.SetWrap(0, 0, output.ToString(), ConsoleColor.Gray, ConsoleColor.Black);
        }
        private void ASCIIPrint()
        {
            this.text.buffer.Set(0, 0, top, ConsoleColor.DarkBlue);

            foreach (TankObject obj in tank.Objects)
            {
                if (printers.ContainsKey(obj.GetType()))
                {
                    printers[obj.GetType()](obj);
                }
            }
        }
        private struct Descrip
        {
            public Type type;
            public string action;
        }
        private Dictionary<string, int> CountObjects()
        {
            Dictionary<Descrip, int> counts = new Dictionary<Descrip, int>();
            foreach (TankObject obj in tank.Objects)
            {
                Descrip descrip = new Descrip();
                descrip.action = obj.ActionDescription;
                descrip.type = obj.GetType();
                if (!counts.ContainsKey(descrip))
                {
                    counts.Add(descrip, 0);
                }
                counts[descrip]++;
            }
            Dictionary<string, int> counts2 = new Dictionary<string, int>();
            foreach (Descrip key in counts.Keys)
            {
                if (_Singular.ContainsKey(key.type) || _Plural.ContainsKey(key.type))
                {
                    string name = null;
                    if (counts[key] == 1)
                    {
                        name = _Singular[key.type];
                    }
                    else
                    {
                        name = _Plural[key.type];
                    }
                    counts2.Add(string.Format("{0} {1}", name, key.action), counts[key]);
                }
            }
            return counts2;
        }

        protected override void Error(string message)
        {
            text.Error(message);
        }
        public override void ProcessInput()
        {
            text.ProcessInput();
        }
    }
}
