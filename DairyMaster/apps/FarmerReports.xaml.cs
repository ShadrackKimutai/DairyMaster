using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls.Dialogs;
namespace DairyMaster.apps
{
    /// <summary>
    /// Interaction logic for FarmerReports.xaml
    /// </summary>
    public partial class FarmerReports : MetroWindow
    {
        private String x;
        public FarmerReports()
        {
            InitializeComponent();
        }

        private List<FarmerReport> LoadCollectionData()
        {

            List<FarmerReport> farmerCollection = new List<FarmerReport>();


            Connection cn = new Connection();
            cn.connect();
            cn.cmd.Connection = cn.cnn;
            cn.cmd.Connection.Open();
            MessageBox.Show(x);
            cn.cmd.CommandText = "Select TimeOfDelivery,TimeOfDay,FarmerMilkDelivery,BoughtAt,ReceivedBy From MilkDelivery where FarmerNo='" + x.ToUpper() + "'";
            try
            {

                cn.dr = cn.cmd.ExecuteReader();
                while (cn.dr.Read())
                {

                    farmerCollection.Add(new FarmerReport()
                    {
                       
                        TimeOfDelivery = cn.dr.GetTimeSpan(0),
                        TimeOfDay = cn.dr.GetString(1),
                        Delivery = cn.dr.GetFloat(2),
                        BuyingPrice = cn.dr.GetDouble(3),
                        ReceivedBy = cn.dr.GetString(4),


                    });









                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }



            return farmerCollection;
        }

        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {

            MetroDialogSettings yx = new MetroDialogSettings();
            MessageDialogResult xy = new MessageDialogResult();
          x = "";
            yx.AffirmativeButtonText = "Show Records";
        begin:
            x = await this.ShowInputAsync("Enter Farmer Number", "Please enter the Farmer Number of the Farmer you wish to see reports for", yx);
            x = checkNumber(x);
            if (x == "")
            {
                xy = await this.ShowMessageAsync("Entry Error", "You have entered nothing or an Invalid entry, Press Ok to enter correct Farmer Number or Cancel to Quit ", MessageDialogStyle.AffirmativeAndNegative, null);
                if (xy.ToString() != "Negative")
                    goto begin;
                else
                    this.Close();
            }
          
            deliveryGrid.ItemsSource = LoadCollectionData();
        }

        private string checkNumber(String Check)
        {
            Connection cn = new Connection();
            String CheckString="";
            try
            {

                cn.connect();
                
                cn.cmd.Connection = cn.cnn;
                cn.cmd.Connection.Open();
                cn.cmd.CommandText = "Select FarmerNo from Farmer Where FarmerNo='" + Check + "' ";
                CheckString = cn.cmd.ExecuteScalar().ToString();
                if (CheckString == null)
                {
                    return "";
                }
               
                   
                


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debugger.Log(0, "db Error", ex.Message);
            }
           return CheckString;
        }
    }
}
