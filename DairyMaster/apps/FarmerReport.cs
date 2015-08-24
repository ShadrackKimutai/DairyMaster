using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DairyMaster.apps
{
    class FarmerReport
    {
        public TimeSpan TimeOfDelivery { get; set; }
        public string TimeOfDay { get; set; }
        public double BuyingPrice{get; set;}
        public float Delivery { get; set; }
        public string ReceivedBy { get; set; }
    
    }
}
