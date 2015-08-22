using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data.SqlClient;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using DairyMaster.apps;


namespace DairyMaster.apps
{
    /// <summary>
    /// Interaction logic for Farmer_Registration.xaml
    /// </summary>
    public partial class FarmerRegistration : MetroWindow
    {
        BitmapImage passport;
        public FarmerRegistration()
            

        {
            InitializeComponent();

          
            
        }
        private void fillGrid()
        {
            farmerDatagrid.ItemsSource = LoadCollectionData();
        }

        private  void btnLoadPicture_Click(object sender, RoutedEventArgs e)
        {




            passport = new BitmapImage();
            passport.BeginInit();
            passport.UriSource = new Uri(showimageDialog());
            passport.CacheOption = BitmapCacheOption.OnLoad;
            passport.EndInit();

            photoimgbox.Source = passport;
            HideOverlay();
          
        }


        public string showimageDialog()
        {
            ShowOverlay();
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpg|PNG Files (*.png)|*.png|JPG Files (*.jepg)|*.jepg|GIF Files (*.gif)|*.gif";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                return filename;

            }
            return "";

        }

       
        private String IdGen()
        {
            Connection cn=new Connection();
           
         String pattID="", Prodid="";
        int Temp1=0; 
          
          
        String k   = DateTime.Now.Year.ToString();
       Start: 
            Temp1 +=1;
        Prodid = "KDFC";
        
        try{
        
            cn.connect();
            pattID = Prodid +Temp1 + k;
            String CheckString;
            cn.cmd.Connection = cn.cnn;
            cn.cmd.Connection.Open();
            cn.cmd.CommandText = "Select top 1 FarmerNo from Farmer Where FarmerNo='" + pattID + "' Order By FarmerNo DESC";
            CheckString = cn.cmd.ExecuteScalar().ToString();
            if (CheckString == null)
            {
                return Temp1.ToString();
            }
            else goto Start;
          
           
        }catch(Exception ex){
            System.Diagnostics.Debugger.Log(0, "db Error", ex.Message);
        }

        return pattID;
        }

        

        private void FarmerFName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void FarmerFName_GotFocus(object sender, RoutedEventArgs e)
        {
            FarmerFName.Text = "";
        }

        private void FarmerOtherName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void FarmerOtherName_GotFocus(object sender, RoutedEventArgs e)
        {
            FarmerOtherName.Text ="";
        }

        private void FarmerPhone_GotFocus(object sender, RoutedEventArgs e)
        {
            FarmerPhone.Text = "";
        }

        private void FarmerFamilyName_GotFocus(object sender, RoutedEventArgs e)
        {
            FarmerFamilyName.Text = "";
        }

        private void NextofKinFName_GotFocus(object sender, RoutedEventArgs e)
        {
            NextofKinFName.Text = "";
        }

        private void NextOfKinOtherNames_GotFocus(object sender, RoutedEventArgs e)
        {
            NextOfKinOtherNames.Text = "";
        }
        private void clearRegFields()
        {
            IdGen();
            FarmerFName.Text = "";
            FarmerOtherName.Text="";
            FarmerPhone.Text="";
            NextofKinFName.Text = "";
            NextOfKinOtherNames.Text="";
            FarmerFamilyName.Text="";
            FarmerBankNo.Text="";
          
        }
        private async void  btnRegister_Click(object sender, RoutedEventArgs e)
        {
             Connection cn = new Connection();
            String FarmerNo = txtregNo.Text;
            String Farmer=FarmerFName.Text+" "+FarmerOtherName.Text;
            String Phone = FarmerPhone.Text;
            String NextOfKin = NextofKinFName.Text +" "+ NextOfKinOtherNames.Text;
            String Family = FarmerFamilyName.Text;
            String DOR=DateTime.Now.ToShortDateString();
            String Bankacc = FarmerBankNo.Text;
            String Bank = BankList.Text;
           
            try
            {
                //MessageBox.Show(cn.connectionString);
                cn.connect();
                cn.cmd.Connection = cn.cnn;
                cn.cmd.Connection.Open();
                cn.cmd.CommandText = "insert into Farmer(FarmerNo,Farmer_Name,Phone_Number,NextOfKin_Name,FarmerID,Date_of_Registration,Active,AccountNo,Bank) VALUES('" + FarmerNo.ToUpper() + "','" + Farmer.ToUpper() + "','" + Phone.ToUpper() + "','" + NextOfKin.ToUpper() + "','" + Family.ToUpper() + "','" + DOR +"','1','"+Bankacc+"','"+Bank+"')";
                cn.cmd.ExecuteNonQuery();
                await this.ShowMessageAsync("Registration is a Success", "You have Registered " + Farmer.ToUpper() + " Successfuly", MessageDialogStyle.Affirmative, null);
                farmerDatagrid.ItemsSource = LoadCollectionData();
                ViewFarmersTab.BringIntoView();
                txtregNo.Text = IdGen();
                clearRegFields();
            }
            catch (Exception ex)
            {
                ShowOverlay();
                MessageBox.Show("Registration has Failed! The following Message was generated to explain why \n \n"+ex.Message+"\n \n"+ex.InnerException,"Registration Failure",MessageBoxButton.OK,MessageBoxImage.Error);
                HideOverlay();
            }


        }

        private  List<Farmer> LoadCollectionData()
        {
             
            List<Farmer> farmer = new List<Farmer>();
            Connection cn = new Connection();
              cn.connect();
            cn.cmd.Connection = cn.cnn;
            cn.cmd.Connection.Open();
           cn.cmd.CommandText = "Select * From Farmer";
            try
            {
         
                cn.dr = cn.cmd.ExecuteReader();
                while (cn.dr.Read())
                {
            
            farmer.Add(new Farmer()
            {
                        farmerNO = cn.dr.GetString(0),
                        farmerName = cn.dr.GetString(1),
                        farmerID =int.Parse(cn.dr.GetString(5)),
                        phoneNumber = cn.dr.GetString(2),
                        dateOfRegistration = cn.dr.GetDateTime(6),
                        Bank = cn.dr.GetString(9),
                        BankNum = cn.dr.GetString(8),
                        isActive = cn.dr.GetBoolean(7),
                        nextOfKin = cn.dr.GetString(4),
            });
                        

                       





                   
    }
            }
            catch (Exception ex)
            {
                 ShowOverlay();
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                HideOverlay();
            }
            

                    return farmer;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            txtregNo.Text = IdGen();

            fillGrid();
        }
      
       

      
    }
}
