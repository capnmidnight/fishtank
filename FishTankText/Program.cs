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
            DateTime start = DateTime.Now;
            adapter.ExecuteScript("help");

            while (adapter.IsRunning)
            {
                adapter.ProcessInput();
                TimeSpan delta = DateTime.Now - start;
                if (delta.TotalMilliseconds > (int)SystemVariables.Vars["SPEED"])
                {
                    for (int i = 0; i < delta.TotalMilliseconds; i += (int)SystemVariables.Vars["TIMESTEP"])
                        tank.Update((int)SystemVariables.Vars["TIMESTEP"]);
                    adapter.Print();
                    start += delta;
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
