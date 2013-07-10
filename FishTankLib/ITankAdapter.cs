using System;
using System.Collections.Generic;
using System.Text;

namespace FishTank.Common
{
    public interface ITankAdapter
    {
        void ExecuteScript(string script);
        void Display();
        void ProcessInput();
        bool IsRunning { get; }
    }
}
