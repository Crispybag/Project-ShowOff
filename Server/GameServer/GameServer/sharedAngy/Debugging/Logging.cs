using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class Logging
    {
        public enum debugState
        {
            USER = 0,
            SIMPLE = 1,
            DETAILED = 2,
            SPAM = 3
        }

        private static debugState debugger = debugState.DETAILED;

        private static bool debugLevel(debugState pState)
        {
            return ((int)debugger >= (int)pState);
        }
        public static void LogInfo(string pMessage, debugState pState = debugState.SIMPLE)
        {
            if (debugLevel(pState)) Console.WriteLine(pMessage);
        }
    }
}
