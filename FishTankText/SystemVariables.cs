using System;
using System.Collections.Generic;
using System.Text;

namespace FishTank.UI.Text
{
    class SystemVariables
    {
        public static Dictionary<string, object> Vars;
        static SystemVariables()
        {
            Vars = new Dictionary<string, object>();
            Vars.Add("SPEED", 250);
        }
    }
}