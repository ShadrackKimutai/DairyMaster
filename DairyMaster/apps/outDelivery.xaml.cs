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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;


namespace DairyMaster.apps
{
    /// <summary>
    /// Interaction logic for testPage.xaml
    /// </summary>
    public partial class outDelivery : MetroWindow
    {
        bool blovkLoop = false;
        bool lock_load = false;
        public outDelivery()
        {
            InitializeComponent();

            Connection cn = new Connection();
            Utilities ut = new Utilities();
            
            try
            {
                
                cn.connect();


                cn.cmd.Connection = cn.cnn;
                cn.cmd.Connection.Open();
                cn.cmd.CommandText = "SELECT Delivered FROM MilkCreameryDelivery WHere Date='" + DateTime.Now.ToShortDateString() + "' AND TimeOfDay='" + ut.TimeOfDay() + "'";
                object x=cn.cmd.ExecuteScalar();
                if (x==null)
                {
                    
                }else
                {
                   
                lock_load= bool.Parse(cn.cmd.ExecuteScalar().ToString());
              
                }
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            fillStaff();
                    fillTransporter();
        }
        private void Delivered()
        {
           
        }
        private void totalMilk(DateTime TotalDate)
        {
            if (blovkLoop == false)
            {
                float totalmilk = 0;
                String obj;
                Utilities ut = new Utilities();
                Connection cn = new Connection();
                try
                {

                    cn.connect();


                    cn.cmd.Connection = cn.cnn;
                    cn.cmd.Connection.Open();
                    cn.cmd.CommandText = "Select sum(FarmerMilkDelivery) from MilkDelivery Where DateOfDelivery='" + TotalDate + "' AND TimeofDay='"+ut.TimeOfDay()+"' AND DateOfDelivery NOT IN(SELECT Date FROM MilkCreameryDelivery WHere Date='" + TotalDate + "' AND TimeOfDay='"+ut.TimeOfDay()+"' )";
                    obj = cn.cmd.ExecuteScalar().ToString();
                    if (obj == "")
                    {
                        
                        obj = "0";
                        notifyDelivery();
                        loadEdatCoop.IsEnabled = false;
                        fillTodayDelivery();
                        tabC.SelectedIndex = 1;

                    }
                    else
                    {
                        totalmilk = float.Parse(obj);
                    }
                }
                catch (Exception ex)
                {
                    displayMetroError(ex);
                    System.Diagnostics.Debugger.Log(0, "db Error", ex.Message);

                    totalmilk = 0;
                }

                loadedmilk.Text = totalmilk.ToString();
                blovkLoop = true;
            }
        }
        private async void notifyDelivery()
        {
            await this.ShowMessageAsync("Delivery Pending", "You have already added the Delivery. Enter and Update Milk Unloaded at Creamery ", MessageDialogStyle.Affirmative, null);
                
        }
        private void fillTodayDelivery()
        {
            Connection cn = new Connection();
            cn.connect();
            cn.cmd.Connection = cn.cnn;
            cn.cmd.Connection.Open();
            cn.cmd.CommandText = "Select litres_loaded,Loader,Supplier,Driver,Conductor,Vehicle From MilkCreameryDelivery Where Date='" + DateTime.Now.ToShortDateString() + "' and Delivered='False'";
            try
            {

                cn.dr = cn.cmd.ExecuteReader();
                while (cn.dr.Read())
                {
                  loadedmilk1.Text=cn.dr.GetDouble(0).ToString();
                    staffcombo1.Text = (cn.dr.GetString(1));
                    suppliercombo1.Text=(cn.dr.GetString(2));
                    Driver1.Text=(cn.dr.GetString(3));
                    Conductor1.Text = (cn.dr.GetString(4));
                    Vehicle1.Text = (cn.dr.GetString(5));
                    //MessageBox.Show(cn.dr.GetString(0) + cn.dr.GetString(1) + cn.dr.GetString(2));
                }
            }
            catch (Exception ex)
            {
                displayMetroError(ex);
            }
            fillStaff();
            fillTransporter();
        }
       private String IdGen()
        {
            Connection cn = new Connection();

            String pattID = "", Prodid = "";
            int Temp1 = 0;


            String k = DateTime.Now.Year.ToString();
        Start:
            Temp1 += 1;
            Prodid = "KDFC/DLV/";

            try
            {

                cn.connect();
                pattID = Prodid + Temp1 + k;
                String CheckString;
                cn.cmd.Connection = cn.cnn;
                cn.cmd.Connection.Open();
                cn.cmd.CommandText = "Select top 1 DeliveryID from MilkCreameryDelivery Where DeliveryID='" + pattID + "' Order By DeliveryID DESC";
                CheckString = cn.cmd.ExecuteScalar().ToString();
                if (CheckString == null)
                {
                    return Temp1.ToString();
                }
                else goto Start;


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debugger.Log(0,"db Error",ex.Message);
              

            }

            return pattID;
        
        }
        private  void fillStaff()
        {
             staffcombo.Items.Clear();
             Connection cn = new Connection();
              cn.connect();
            cn.cmd.Connection = cn.cnn;
            cn.cmd.Connection.Open();
            cn.cmd.CommandText = "select StaffName from Staff Where Active='True'";
            try
            {
         
                cn.dr = cn.cmd.ExecuteReader();
                while (cn.dr.Read())
                {



                    staffcombo.Items.Add(cn.dr.GetString(0));
                    
                }
            } catch(Exception ex){
                displayMetroError(ex);
            }

            
        }
        private void fillTransporter()
        {
        
            
             Connection cn = new Connection();
              cn.connect();
            cn.cmd.Connection = cn.cnn;
            cn.cmd.Connection.Open();
            cn.cmd.CommandText = "select SupplierName from Supplier";
            try
            {
         
                cn.dr = cn.cmd.ExecuteReader();
                while (cn.dr.Read())
                {



                    suppliercombo.Items.Add(cn.dr.GetString(0));
                    
                }
            } catch(Exception ex){
                displayMetroError(ex);
            }
            fillTransporterDetails();
        }

        private void fillTransporterDetails()
        {
            
            Connection cn = new Connection();
            cn.connect();
            cn.cmd.Connection = cn.cnn;
            cn.cmd.Connection.Open();
            cn.cmd.CommandText = "Select Misc3,Misc2,Misc1 From Supplier Where SupplierActive='True' AND SupplierName='" + suppliercombo.Text + "'";
            try
            {

                cn.dr = cn.cmd.ExecuteReader();
                while (cn.dr.Read())
                {
                    Driver.Text = (cn.dr.GetString(1));
                    Conductor.Text= (cn.dr.GetString(0));
                    Vehicle.Text = (cn.dr.GetString(2));
                    //MessageBox.Show(cn.dr.GetString(0) + cn.dr.GetString(1) + cn.dr.GetString(2));
                }
            }
            catch (Exception ex)
            {
                displayMetroError(ex);
            }
        }

        private void suppliercombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Driver.Clear();
            Conductor.Clear();
            Vehicle.Clear();
            Connection cn = new Connection();
            cn.connect();
            cn.cmd.Connection = cn.cnn;
            cn.cmd.Connection.Open();
            cn.cmd.CommandText = "Select Misc3,Misc2,Misc1 From Supplier Where SupplierActive='True' and SupplierName='" + suppliercombo.SelectedItem.ToString() + "'";
            try
            {

                cn.dr = cn.cmd.ExecuteReader();
                while (cn.dr.Read())
                {
                    Driver.Text = (cn.dr.GetString(1));
                    Conductor.Text = (cn.dr.GetString(0));
                    Vehicle.Text = (cn.dr.GetString(2));
                   // MessageBox.Show(cn.dr.GetString(0) + cn.dr.GetString(1) + cn.dr.GetString(2));
                }
            }
            catch (Exception ex)
            {
                displayMetroError(ex);
            }
           
        }

     

        private async void AddDelivery_Click(object sender, RoutedEventArgs e)
        {
             Connection cn = new Connection();
            String DeliveryID = IdGen();
            float Loaded=float.Parse(loadedmilk.Text);
            float UnLoaded = 0;
            float Deficit=Loaded-UnLoaded;
             String Supplier =suppliercombo.SelectedItem.ToString();
             String DelVehicle = Vehicle.Text;
            String DOD=DateTime.Now.ToShortDateString();
            String driver=Driver.Text;
            String conductor = Conductor.Text;
            String StaffName=staffcombo.SelectedItem.ToString();
            Utilities f = new Utilities();
          try
            {
                //MessageBox.Show(cn.connectionString);
                cn.connect();
                cn.cmd.Connection = cn.cnn;
                cn.cmd.Connection.Open();
                cn.cmd.CommandText = "INSERT INTO MilkCreameryDelivery(DeliveryID,Date,timeofday,Litres_loaded,Litres_Unloaded,Deficit,Loader,Driver,Conductor,Vehicle,Supplier,Delivered) VALUES('" + DeliveryID + "','" + DOD + "','"+f.TimeOfDay()+"','" + Loaded + "','" + UnLoaded + "','" + Deficit + "','" + StaffName + "','" + driver + "','" + conductor + "','" + DelVehicle + "','" + Supplier + "','0')";
                
                cn.cmd.ExecuteNonQuery();
                MessageDialogResult x = new MessageDialogResult();
                MetroDialogSettings y = new MetroDialogSettings();
                y.AffirmativeButtonText = "Go to Delivery Tab";
                y.NegativeButtonText = "Close";

               x= await this.ShowMessageAsync("Delivery Added", "You have Added the Delivery Successfuly. Upon return, the Transporter("+Supplier+") Must enter milk unloaded at the creamery in the next Tab", MessageDialogStyle.AffirmativeAndNegative, y);
               if (x.ToString() == "Negative")
               {
                   this.Close();

               }
               else
               {
                   
                   loadEdatCoop.IsEnabled = false;
                   fillTodayDelivery();
                   tabC.SelectedIndex = 1;
               }
                //farmerDatagrid.ItemsSource = LoadCollectionData();
                //ViewFarmersTab.BringIntoView();
               // clearRegFields();
            }
          catch (Exception ex)
            {
                displayMetroError(ex);
            }
        }

        private void AddDelivery1_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void UpdateDb_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(Unloaded.Value.ToString());
            if (Unloaded.Value != 0)
            {
       
            Double loaded=double.Parse(loadedmilk1.Text);
            Double unloaded = double.Parse(Unloaded.Value.ToString());
            Double Deficit = loaded - unloaded;
            
            if (Deficit<3.0)
            {
                Deficit=0;
            }
             Connection cn = new Connection();
             try{
                 cn.connect();
                cn.cmd.Connection = cn.cnn;
                cn.cmd.Connection.Open();
                cn.cmd.CommandText = "update MilkCreameryDelivery Set Litres_Unloaded='"+unloaded+"', Deficit='"+Deficit+"',Delivered='True' WHERE DATE='"+DateTime.Now.ToShortDateString()+"'";

                cn.cmd.ExecuteScalar();
                MessageDialogResult x = new MessageDialogResult();
                MetroDialogSettings y = new MetroDialogSettings();
                y.AffirmativeButtonText = "Quit";
              
                 await this.ShowMessageAsync("Delivery Confirmed", "You have updated the Delivery Successfuly.", MessageDialogStyle.Affirmative, y);
               if (x.ToString() == "Negative")
                {
                    this.Close();

                }
                //farmerDatagrid.ItemsSource = LoadCollectionData();
                //ViewFarmersTab.BringIntoView();
               // clearRegFields();
            }
            catch (Exception ex)
            {
                displayMetroError(ex);
                //ShowOverlay();
                //MessageBox.Show("Registration has Failed! The following Message was generated to explain why \n \n"+ex.Message+"\n \n"+ex.InnerException,"Registration Failure",MessageBoxButton.OK,MessageBoxImage.Error);
                //HideOverlay();
            }
            }
            else
            {
                Unloaded.Focus();
            }
        }

        private async void displayMetroError(Exception ex)
        {
            await this.ShowMessageAsync("Error Detected", "An Error has been detected. The System has returned the following Message \n\n" + ex.Message, MessageDialogStyle.Affirmative, null);
            
        }

        
        private async void displayNoTask()
        {
            MessageDialogResult ex=new MessageDialogResult();
            MetroDialogSettings yx = new MetroDialogSettings();
            yx.AffirmativeButtonText = "Quit";


            await this.ShowMessageAsync("All Tasks Done","All Milk has Been Loaded and Delivered. No other Milk Delivery transactions are available at this moment.",MessageDialogStyle.Affirmative,yx);
            
            if (ex.ToString() == "Negative")
            { 
                this.Close();
            }
        }

        private void outDelivery_Loaded(object sender, RoutedEventArgs e)
        {
            if (lock_load == false)
                totalMilk(DateTime.Now.Date);
            else
            {
                displayNoTask();
            }
        }
    }
}