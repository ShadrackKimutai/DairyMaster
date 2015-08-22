using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DairyMaster.apps
{
    class Farmer
    {

        //public BitmapImage passport;
        public string farmerNO { get; set; }
        public string farmerName { get; set; }
        public string phoneNumber { get; set; }
        //public BitmapImage farmerPhoto {get; set;}
        public string nextOfKin { get; set; }
        public int farmerID { get; set; }
        public DateTime dateOfRegistration { get; set; }
        public bool isActive { get; set; }
        public string Bank { get; set; }
        public string BankNum { get; set; }
    }
}
