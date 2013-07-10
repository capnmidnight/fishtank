//Copyright, Sean T. McBeth, 2007
//sean.mcbeth@gmail.com
//www.seanmcbeth.com
using System.Collections.Generic;
namespace ScriptedSystem
{
    public class SystemVariables
    {
        public static Dictionary<string, object> Vars;
        static SystemVariables()
        {
            Vars = new Dictionary<string, object>();
            Vars.Add("SPEED", 250);
        }
    }
}