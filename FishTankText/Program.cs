//Copyright, Sean T. McBeth, 2007
//sean.mcbeth@gmail.com
//www.seanmcbeth.com
using System;
using FishTank.Common;
using System.Diagnostics;
using ScriptedSystem;

namespace FishTank.UI.Text
{
    class Program
    {
        static void Main(string[] args)
        {
            Resize();
            Fish.AvatarHeight = 1;
            Fish.AvatarWidth = 3;
            Tank tank = new Tank(1000, 500);
            TextTankAdapter adapter = new TextTankAdapter(tank, Console.WindowWidth, Console.WindowHeight - 1);
            Stopwatch clock = new Stopwatch();
            clock.Start();

            adapter.ExecuteScript("help");

            while (adapter.IsRunning)
            {
                adapter.Print();
                adapter.ProcessInput();
                if ((int)clock.ElapsedMilliseconds > (int)SystemVariables.Vars["SPEED"])
                {
                    tank.Update((int)clock.ElapsedMilliseconds);
                    clock.Reset();
                    clock.Start();
                }
            }
        }

        private static void Resize()
        {
            ConsoleKey input = ConsoleKey.Q;
            while (input != ConsoleKey.X)
            {
                string output = string.Format("Resize Console Window (WASDX): W({0}), H({1})", Console.WindowWidth, Console.WindowHeight);
                Console.Write(output);
                ConsoleKeyInfo inf = Console.ReadKey(true);
                input = inf.Key;
                if (input == ConsoleKey.W)
                {
                    Console.WindowHeight--;
                }
                else if (input == ConsoleKey.S)
                {
                    Console.WindowHeight++;
                }
                else if (input == ConsoleKey.A)
                {
                    Console.WindowWidth--;
                }
                else if (input == ConsoleKey.D)
                {
                    Console.WindowWidth++;
                }
                for (int i = 0; i < output.Length; ++i) Console.Write("\b");
            }
        }
    }
}
