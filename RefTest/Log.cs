using System;

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Reflection;


namespace RefTest
{
    
    public static class Log
    {
        public static void State(object caller, string uniqueToken, string extraData = "") 
        {
            var className = caller.GetType().Name;
//            var uniqueValue = caller.GetHashCode(); // Does not work all the time

            StackTrace stackTrace = new StackTrace();           // get call stack
            StackFrame[] stackFrames = stackTrace.GetFrames();  // get method calls (frames)

            StackFrame callingFrame = stackFrames[1];
            var info = callingFrame.GetMethod();
            var method = cleanUpMethodName(info.Name);

            Console.WriteLine("Class: [{0}] Ref: [{1}] Method: [{2}] Extra: [{3}]", className, uniqueToken, method, extraData);
        }

        public static string UniqueToken()
        {
            return Guid.NewGuid().ToString("N").GetHashCode().ToString();
        }

        static string cleanUpMethodName(string name)
        {
            var method = name;

            // NOTE: .ctor is constructor and Finalize is the destructor
            if (string.Equals(name, ".ctor"))
            {
                method = "Constructor";
            }
                    
            return method;
        }

    }
}

