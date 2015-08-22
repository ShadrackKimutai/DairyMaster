using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DairyMaster.apps
{
    class inDelivery
    {
        public TimeSpan TimeOfDelivery { get; set; }
        public string TimeOfDay { get; set; }
        public string FarmerNO { get; set; }
        public string FarmerName { get; set; }
        public double BuyingPrice{get; set;}
        public float Delivery { get; set; }
        public string ReceivedBy { get; set; }
    }
}
