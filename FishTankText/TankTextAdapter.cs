using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using FishTank.Common;

namespace FishTank.UI.Text
{

    public class TextTankAdapter : ScriptHost, ITankAdapter
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
        private bool isRunning;
        private ConsoleBuffer buffer;
        private Dictionary<Type, PrintingDelegate> printers;

        public TextTankAdapter(Tank tank)
            : base(tank)
        {
            this.isVerbose = true;
            this.isRunning = true;
            this.AddCommand("ascii", new CommandDelegate(this.Show), false);
            this.AddCommand("verbose", new CommandDelegate(this.Verbose), false);
            this.AddCommand("clrscr", new CommandDelegate(this.Clear), false);
            this.AddCommand("exit", new CommandDelegate(this.Exit), false);
            this.AddCommand("help", new CommandDelegate(this.Help), false);
            this.AddCommand("set", new CommandDelegate(this.Set), false);
            this.AddCommand("set_speed", new CommandDelegate(this.SetSpeed), true);
            this.buffer = new ConsoleBuffer(tank.Width, tank.Height);
            printers = new Dictionary<Type, PrintingDelegate>();
            printers.Add(typeof(Fish), new PrintingDelegate(PrintFish));
            printers.Add(typeof(Poop), new PrintingDelegate(PrintPoop));
            printers.Add(typeof(Food), new PrintingDelegate(PrintFood));
            printers.Add(typeof(Plant), new PrintingDelegate(PrintPlant));
            printers.Add(typeof(Pump), new PrintingDelegate(PrintPump));
            printers.Add(typeof(Bubble), new PrintingDelegate(PrintBubble));
            printers.Add(typeof(Algae), new PrintingDelegate(PrintAlgae));
            printers.Add(typeof(Snail), new PrintingDelegate(PrintSnail));
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
        private void PrintAlgae(TankObject obj)
        {
            this.buffer.Set(obj.X, obj.Y, ConsoleColor.DarkGreen);
        }
        private void PrintBubble(TankObject obj)
        {
            char c = '.';
            if (obj.Age > 10)
                c = 'o';
            if (obj.Age > 20)
                c = 'O';
            this.buffer.Set(obj.X, obj.Y, c, ConsoleColor.Cyan);
        }
        private void PrintPump(TankObject obj)
        {
            this.buffer.Set(obj.X, obj.Y, "ooo", ConsoleColor.Gray, ConsoleColor.DarkGray);
            this.buffer.Set(obj.X, obj.Y + 1, "ooo", ConsoleColor.Gray, ConsoleColor.DarkGray);
            for (int y = obj.Y + 2; y < tank.Height - 1; ++y)
            {
                this.buffer.Set(obj.X + 1, y, ':', ConsoleColor.Gray, ConsoleColor.DarkGray);
            }
        }
        private void PrintFish(TankObject obj)
        {
            this.buffer.Set(obj.X, obj.Y, ">=>", ConsoleColor.Cyan);
        }
        private void PrintPoop(TankObject obj)
        {
            this.buffer.Set(obj.X, obj.Y, '~', ConsoleColor.DarkYellow);
        }
        private void PrintFood(TankObject obj)
        {
            this.buffer.Set(obj.X, obj.Y, "#", ConsoleColor.Green);
        }
        private void PrintSnail(TankObject obj)
        {
            this.buffer.Set(obj.X, obj.Y, "@", ConsoleColor.Yellow);
        }
        private void PrintPlant(TankObject obj)
        {
            for (int y = obj.Y; y < Console.WindowHeight; ++y)
            {
                this.buffer.Set(obj.X, y, (((y + obj.Age) % 2 == 0) ? '/' : '\\'), ConsoleColor.Green, ConsoleColor.DarkGreen);
            }
        }
        protected override void Error(string message)
        {
            this.buffer.Prompt(message, true);
        }
        private void Help(string[] args)
        {
            this.buffer.Set(0, 0, "C# Console FishTank", ConsoleColor.Gray, ConsoleColor.Black);
            this.buffer.Set(0, 1, "  Typing will initiate the command prompt", ConsoleColor.Gray, ConsoleColor.Black);
            this.buffer.Set(0, 2, "  Commands:", ConsoleColor.Gray, ConsoleColor.Black);
            this.buffer.Set(0, 3, "    empty -> reset the tank to an empty state", ConsoleColor.Gray, ConsoleColor.Black);
            this.buffer.Set(0, 4, "    exit -> end the simulation", ConsoleColor.Gray, ConsoleColor.Black);
            this.buffer.Set(0, 5, "    add [count] <item_name> -> add an item, with an optional count parameter", ConsoleColor.Gray, ConsoleColor.Black);
            this.buffer.Set(0, 6, "      available items: fish, food, plant", ConsoleColor.Gray, ConsoleColor.Black);
            this.buffer.Set(0, 7, "    clean -> remove dirt generating items: old food, poop", ConsoleColor.Gray, ConsoleColor.Black);
            this.buffer.Set(0, 8, "	clrscr -> clears the screen", ConsoleColor.Gray, ConsoleColor.Black);
            this.buffer.Set(0, 9, "    ascii -> switch to ASCII mode graphics", ConsoleColor.Gray, ConsoleColor.Black);
            this.buffer.Set(0, 10, "    verbose -> switch to verbose mode (default)", ConsoleColor.Gray, ConsoleColor.Black);
            this.buffer.Set(0, 11, "    help -> show this help message", ConsoleColor.Gray, ConsoleColor.Black);
            this.buffer.Prompt("Press any key to begin.", true);
            this.buffer.Clear();
            this.buffer.Flush();
        }
        private void Exit(string[] args)
        {
            this.isRunning = false;
        }
        private void Show(string[] args)
        {
            this.isVerbose = false;
        }
        private void Verbose(string[] args)
        {
            this.isVerbose = true;
        }
        private void Clear(string[] args)
        {
            this.buffer.Clear();
        }
        public bool IsRunning
        {
            get
            {
                return this.isRunning;
            }
        }
        public bool IsVerbose
        {
            get
            {
                return this.isVerbose;
            }
        }
        public void Display()
        {

            if (isVerbose)
            {
                this.buffer.Clear(ConsoleColor.Black);
                this.VerbosePrint();
            }
            else
            {
                this.buffer.Clear(ConsoleColor.Black);
                this.ASCIIPrint();
            }
            this.buffer.Set(0, tank.Height - 1, tank.Dirt.ToString(), ConsoleColor.Yellow, ConsoleColor.DarkYellow);
            this.buffer.Flush();
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
            this.buffer.SetWrap(0, 0, output.ToString(), ConsoleColor.Gray, ConsoleColor.Black);
        }
        private void ASCIIPrint()
        {
            this.buffer.Set(0, 0, "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~", ConsoleColor.DarkBlue);

            foreach (TankObject obj in tank.Objects)
            {
                if (0 <= obj.X && obj.X < Console.WindowWidth && 0 <= obj.Y && obj.Y < Console.WindowHeight)
                {
                    if (printers.ContainsKey(obj.GetType()))
                    {
                        printers[obj.GetType()](obj);
                    }
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

        public void ProcessInput()
        {
            if (Console.KeyAvailable)
            {
                this.buffer.Prompt("[PAUSED, enter command] :> ", false);
                string input = Console.ReadLine();
                this.buffer.CorrectInputBuffer(input);
                string script = input.Trim();
                this.ExecuteScript(script);
            }
        }
    }
}
