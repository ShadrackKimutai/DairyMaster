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
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;


namespace DairyMaster.apps
{
    /// <summary>
    /// Interaction logic for indelivery.xaml
    /// </summary>
    public partial class indelivery : MetroWindow
    {
        private string PhoneNumber;
        private string farmerName;
       float buyingPrice = 50;
      
       
      
        public indelivery()
        {
            InitializeComponent();

            deliveryNum.Value = 0;
            deliveryGrid.ItemsSource = LoadCollection();
           // getTotal();
        }

       
        private  async void fillCombo()
        {
            Utilities f=new Utilities();
            farmerNumber.Items.Clear();
             Connection cn = new Connection();
              cn.connect();
            cn.cmd.Connection = cn.cnn;
            cn.cmd.Connection.Open();
            cn.cmd.CommandText = "select FarmerNo from Farmer where FarmerNo NOT  IN (Select farmerNo from MilkDelivery where DateOfDelivery = '"+DateTime.Now.ToShortDateString()+"' And TimeofDay='"+f.TimeOfDay()+"')";
            try
            {
         
                cn.dr = cn.cmd.ExecuteReader();
                while (cn.dr.Read())
                {



                    farmerNumber.Items.Add(cn.dr.GetString(0));
                    
                }
            } catch(Exception ex){
                MessageBox.Show(ex.Message);
            }
            if (farmerNumber.Items.Count != 0)
            { 
                farmerNumber.SelectedIndex = 0; 
                fillName();
          
            }
            else
            {
                
                MessageDialogResult x;
                MetroDialogSettings yl=new MetroDialogSettings();
             
               
                yl.AffirmativeButtonText="Quit";
                yl.NegativeButtonText = "View Deliveries";
                x=await this.ShowMessageAsync("Entry Complete!", "All Farmers with Valid Farmer Numbers have been Processed. any other processing will have to be done Manualy",MessageDialogStyle.AffirmativeAndNegative,yl);
                if (x == MessageDialogResult.Affirmative)
                {
                    
                    this.Hide();
                }
                
                   
                    
                
                
            
            }
        
        }
        private void fillName(){
             Connection cn = new Connection();
            cn.connect();
            cn.cmd.Connection = cn.cnn;
            cn.cmd.Connection.Open();
            cn.cmd.CommandText = "Select Farmer_Name,Phone_Number From Farmer Where FarmerNo='"+farmerNumber.Text+"'";
            try
            {
                
                cn.dr = cn.cmd.ExecuteReader();
              while (cn.dr.Read())
                {
                  PhoneNumber=(cn.dr.GetString(1));
                  farmerName = (cn.dr.GetString(0));

                  selectedfarmerName.Content = "Farmer: " + farmerName; 
                 selectedFarmerContact.Content = "Contact Number: " + PhoneNumber; 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            getTotal();
        }
        private void farmerNumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fillName();
        }

        private void farmerNumber_GotFocus(object sender, RoutedEventArgs e)
        {
            fillName();
        }

        private void farmerNumber_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            fillName();
        }

        private void farmerNumber_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void farmerNumber_MouseEnter(object sender, MouseEventArgs e)
        {
            
        }

        private void farmerNumber_MouseLeave(object sender, MouseEventArgs e)
        {
            fillName();
        }

        private void farmerNumber_KeyUp(object sender, KeyEventArgs e)
        {
            fillName();
        }
        
        private async void registerFarmersDelivery_Click(object sender, RoutedEventArgs e)
        {
            if (farmerNumber.Items.Count != 0)
            {
                Connection cn = new Connection();
                double delivery = (double)deliveryNum.Value;
                delivery = System.Math.Round(delivery, 2);
                string deliveryhash = farmerNumber.Text.ToUpper() + DateTime.Now.ToShortDateString()+DateTime.Now.ToShortTimeString();
              try
                {
                    //MessageBox.Show(cn.connectionString);
                    Utilities f=new Utilities();
                    cn.connect();
                    cn.cmd.Connection = cn.cnn;
                    cn.cmd.Connection.Open();
                    cn.cmd.CommandText = "insert into MilkDelivery(FarmerHash,FarmerNo,FarmerName,FarmerMilkDelivery,DateOfDelivery,timeofdelivery,timeofday,BoughtAt,ReceivedBy) VALUES('" + deliveryhash + "','" + farmerNumber.Text + "','" + farmerName + "','" + delivery + "','" + DateTime.Now.ToShortDateString() + "','"+DateTime.Now.ToLongTimeString()+"','"+f.TimeOfDay()+"','" + buyingPrice + "','SELF')";
                  //MessageBox.Show( "insert into MilkDelivery(FarmerHash,FarmerNo,FarmerName,FarmerMilkDelivery,DateOfDelivery,timeofdelivery,timeofday,BoughtAt,ReceivedBy) VALUES('" + deliveryhash + "','" + farmerNumber.Text + "','" + farmerName + "','" + delivery + "','" + DateTime.Now.ToShortDateString() + "','"+DateTime.Now.ToLongTimeString()+"','"+f.TimeOfDay()+"','" + buyingPrice + "','SELF')");
                
                  cn.cmd.ExecuteNonQuery();
                    await this.ShowMessageAsync("Delivery Registered!", "You have entered a milk delivery of " + delivery + " kg's for " + farmerNumber.Text + " Bought at Sh" + buyingPrice + "", MessageDialogStyle.Affirmative, null);
                    deliveryNum.Value = 0;
                    deliveryGrid.ItemsSource = LoadCollection();
                    fillCombo();
                }
                 catch (Exception ex)
                {
                    ShowOverlay();
                    MessageBox.Show(ex.Message);//"Entry Failed! Most probable cause for this is that "+farmerName+ "'s has already delivered milk today", "Entry Failure", MessageBoxButton.OK, MessageBoxImage.Error);
                    HideOverlay();

                }
                 
            }
            else
            {
                MessageDialogResult x;
                MetroDialogSettings yl=new MetroDialogSettings();
             
               
                yl.AffirmativeButtonText="Quit";
                x=await this.ShowMessageAsync("Entry Complete!", "All Farmers with Valid Farmer Numbers have been Processed. any other processing will have to be done Manualy",MessageDialogStyle.Affirmative,yl);
                if (x == MessageDialogResult.Affirmative)
                {
                    MessageBox.Show("t");
                    this.Close();
                }
                
                   
                    
                
                
            }
        }
        private  void getTotal()
        {
           
            if (farmerNumber.Items.Count != 0) {
           String FirstMonthDate=DateTime.Now.Month.ToString()+"/1/"+DateTime.Now.Year.ToString();
            //farmerNumber.Items.Clear();
            Connection cn = new Connection();
            double x,y;
               
            cn.connect();
            cn.cmd.Connection = cn.cnn;
            cn.cmd.Connection.Open();
            cn.cmd.CommandText = "select sum(FarmerMilKDelivery) from MilkDelivery where FarmerNo like '%" + farmerNumber.SelectedValue.ToString() + "%' and DateOfDelivery Between '" + FirstMonthDate + "' and '" + DateTime.Now.Date.ToShortDateString() + "'";
            try
            {
              
                if (cn.cmd.ExecuteScalar().ToString() != "") { 
                x =double.Parse(cn.cmd.ExecuteScalar().ToString());
                y = Math.Round(x,1);
               
                }
                else
                {
                    y = 0.0;
                }
                    deliverythisMonth.Content = "Delivery This Month:" + y.ToString()+" Kilogrammes ";
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
            }else
            {
            }
        
        }
        private List<inDelivery> LoadCollection()
        {
            List<inDelivery> farmerCollection = new List<inDelivery>();
         
          
            Connection cn = new Connection();
            cn.connect();
            cn.cmd.Connection = cn.cnn;
            cn.cmd.Connection.Open();
            cn.cmd.CommandText = "Select FarmerNo,FarmerName,TimeOfDelivery,TimeOfDay,FarmerMilkDelivery,BoughtAt,ReceivedBy From MilkDelivery where DateOfDelivery='"+DateTime.Now.ToShortDateString()+"'";
            try
            {

                cn.dr = cn.cmd.ExecuteReader();
                while (cn.dr.Read())
                {

                    farmerCollection.Add(new inDelivery()
                    {
                        FarmerNO = cn.dr.GetString(0),
                        FarmerName = cn.dr.GetString(1),
                        TimeOfDelivery=cn.dr.GetTimeSpan(2),
                    TimeOfDay=cn.dr.GetString(3),
                        Delivery = cn.dr.GetFloat(4),
                        BuyingPrice = cn.dr.GetDouble(5),
                       ReceivedBy = cn.dr.GetString(6),
                        
                        
                    });









                }
            }
            catch (Exception ex)
            {
               
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
               
            }


            
            return farmerCollection;
        }

        private void deliveryGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void indelivery1_Loaded(object sender, RoutedEventArgs e)
        {
            fillCombo();
        }

        private void deliverythisMonth_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {

        }

        private void deliverythisMonth_ContextMenuClosing_1(object sender, ContextMenuEventArgs e)
        {

        }

      

        
    }
}
