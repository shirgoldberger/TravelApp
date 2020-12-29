using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TravelApp.Objects
{
    public sealed class Utils
    {
        private static readonly Utils instance = new Utils();
        private static Object locker;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Utils()
        {
        }

        private Utils()
        {
            locker = new object();
        }

        public static Utils Instance
        {
            get
            {
                return instance;
            }
        }
        public void errorAndExit(string msg)
        {
            lock(locker)
            {
                MessageBox.Show(msg);
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!1");
            }
        }
    }
}
