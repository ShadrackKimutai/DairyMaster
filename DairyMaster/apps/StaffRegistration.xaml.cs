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
using DairyMaster.apps;
using System.Data.SqlClient;
using MahApps.Metro.Controls.Dialogs;

namespace DairyMaster.apps
{
    /// <summary>
    /// Interaction logic for StaffRegistration.xaml
    /// </summary>
    public partial class StaffRegistration : MetroWindow
    {
        bool isActivestatus=true;
        BitmapImage passport;
        public StaffRegistration()
        {
            InitializeComponent();
           
        }

        private void btnLoadPicture_Click(object sender, RoutedEventArgs e)
        {
            passport = new BitmapImage();
            passport.BeginInit();
            passport.UriSource = new Uri(showimageDialog());
            passport.CacheOption = BitmapCacheOption.OnLoad;
            passport.EndInit();

            photoimgbox.Source = passport;
            HideOverlay();
        }
        private String IdGen()
        {
            Connection cn = new Connection();

            String pattID = "", Prodid = "";
            int Temp1 = 0;


            String k = DateTime.Now.Year.ToString();
        Start:
            Temp1 += 1;
            Prodid = "KDFC/STAFF/";

            try
            {

                cn.connect();
                pattID = Prodid + Temp1 + k;
                String CheckString;
                cn.cmd.Connection = cn.cnn;
                cn.cmd.Connection.Open();
                cn.cmd.CommandText = "Select top 1 staffNo from Staff Where staffNo='" + pattID + "' Order By staffNo DESC";
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

        private async void btnRegisterStaff_Click(object sender, RoutedEventArgs e)
        {
             Connection cn = new Connection();
            String StaffNo = txtregNo.Text;
            String Staff=firstName.Text+" "+surName.Text;//FarmerFName.Text+" "+FarmerOtherName.Text;
            String Phone = phoneNumber.Text;
            int IDNumber=Int32.Parse(idNumber.Text);
             String kra =krapin.Text.ToUpper();
             String nhif = nhifno.Text.ToUpper();
            String DOE=DateTime.Now.ToShortDateString();
            String Bankacc=accountnumber.Text;
            String Bank = banklist.Text.ToUpper();
            MessageBox.Show(isActivestatus.ToString());
            try
            {
                //MessageBox.Show(cn.connectionString);
                cn.connect();
                cn.cmd.Connection = cn.cnn;
                cn.cmd.Connection.Open();
                cn.cmd.CommandText = "insert into Staff(staffNo,staffName,PhoneNumber,staffID,Date_of_Employment,Active,AccountNo,Bank,kraPIN,nhifNo) VALUES('" + StaffNo.ToUpper() + "','" + Staff.ToUpper() + "','" + Phone.ToUpper() + "','" +IDNumber + "','" + DOE +"','"+isActivestatus.ToString()+"','"+Bankacc+"','"+Bank+"','"+kra.ToUpper()+"','"+nhif.ToUpper()+"')";
                cn.cmd.ExecuteNonQuery();
                await this.ShowMessageAsync("Registration is a Success", "You have Registered " + Staff.ToUpper() + " Successfuly", MessageDialogStyle.Affirmative, null);
                staffDatagrid.ItemsSource = LoadCollectionData();
                //farmerDatagrid.ItemsSource = LoadCollectionData();
                //ViewFarmersTab.BringIntoView();
              clearRegFields();
            }
            catch (Exception ex)
            {
                ShowOverlay();
                MessageBox.Show("Registration has Failed! The following Message was generated to explain why \n \n"+ex.Message+"\n \n"+ex.InnerException,"Registration Failure",MessageBoxButton.OK,MessageBoxImage.Error);
                HideOverlay();
            }
            
        }

        private void clearRegFields()
        {
            txtregNo.Text = IdGen();
            txtregNo.Clear();
            firstName.Clear();// +" " + 
           surName.Clear();//FarmerFName.Text+" "+FarmerOtherName.Text;
           phoneNumber.Clear();
           idNumber.Clear();
            krapin.Clear();
            nhifno.Clear();
            String DOE = DateTime.Now.ToShortDateString();
            accountnumber.Clear();
             
            
        }

        private void tglesw_Checked(object sender, RoutedEventArgs e)
        {
            isActivestatus = true;
        }

        private void tglesw_Unchecked(object sender, RoutedEventArgs e)
        {
            isActivestatus = false;
        }
        private List<Staff> LoadCollectionData()
        {

            List<Staff> farmer = new List<Staff>();
            Connection cn = new Connection();
            cn.connect();
            cn.cmd.Connection = cn.cnn;
            try { cn.cmd.Connection.Open(); }
            finally { }
            cn.cmd.CommandText = "Select * From Staff";
            try
            {

                cn.dr = cn.cmd.ExecuteReader();
                while (cn.dr.Read())
                {

                    farmer.Add(new Staff()
                    {
                        StaffNo = cn.dr.GetString(0),
                        staff  = cn.dr.GetString(1),
                        IDNumber = int.Parse(cn.dr.GetString(4)),
                        Phone = cn.dr.GetString(2),
                        DOE = cn.dr.GetDateTime(5),
                        Bank = cn.dr.GetString(8),
                        Bankacc = cn.dr.GetString(7),
                        isActive = cn.dr.GetBoolean(6),
                        nhif = cn.dr.GetString(9),
                        kra=cn.dr.GetString(10)

                        
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

        private void staffDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            txtregNo.Text = IdGen();
            staffDatagrid.ItemsSource = LoadCollectionData();
        }
       
    }
}
