using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DairyMaster.apps
{
    class Utilities
    {
        public string TimeOfDay()
        {
            string timeOfDay = "";
            if (int.Parse(DateTime.Now.Hour.ToString()) > 12)
            {
                timeOfDay = ("Evening");
            }
            else
            {
                timeOfDay = ("Morning");
            }
            return timeOfDay; 
        }
        public string inverseTimeOfDay()
        {
           string timeOfDay = "";
            if (int.Parse(DateTime.Now.Hour.ToString()) < 12)
            {
                timeOfDay = ("Evening");
            }
            else
            {
                timeOfDay = ("Morning");
            }
            return timeOfDay;
        }
        
    }
}
