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
    /// Interaction logic for Supplier.xaml
    /// </summary>
    public partial class Supplier : MetroWindow
    {
        public bool isActivestatus;
        public Supplier()
        {
            InitializeComponent();
            txtSupno.Text = IdGen();
            isActivestatus = false;
        }
        private String IdGen()
        {
            Connection cn = new Connection();

            String pattID = "", Prodid = "";
            int Temp1 = 0;


            String k = DateTime.Now.Year.ToString();
        Start:
            Temp1 += 1;
            Prodid = "KDFC/SUPPLIER/";

            try
            {

                cn.connect();
                pattID = Prodid + Temp1 + k;
                String CheckString;
                cn.cmd.Connection = cn.cnn;
                cn.cmd.Connection.Open();
                cn.cmd.CommandText = "Select top 1 supplierNo from Supplier Where supplierNo='" + pattID + "' Order By supplierNo DESC";
                CheckString = cn.cmd.ExecuteScalar().ToString();
                if (CheckString == null)
                {
                    return Temp1.ToString();
                }
                else goto Start;


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debugger.Log(0, "db Error", ex.Message);


            }

            return pattID;
        }

        private async void RegisterTransporter_Click(object sender, RoutedEventArgs e)
        {
              Connection cn = new Connection();
            String SupNo = txtSupno.Text;
            String SupName=txtsupName.Text.ToUpper();
            String Phone = txtSupPhone.Text;
            int IDNumber=Int32.Parse(txtsupId.Text);
             String vehicle=travehicle.Text.ToUpper()+"("+travehicleType.Text.ToUpper()+")";
             String Driver = tradriver.Text.ToUpper();
            String TurnBoy=traTurnBoy.Text.ToUpper();
            String DOR=DateTime.Now.ToShortDateString();
            String Bankacc=bankacc.Text;
            String Bank = banklist.Text.ToUpper();
            Boolean isActive= isActivestatus ;
           
            try
            {
                //MessageBox.Show(cn.connectionString);
                cn.connect();
                cn.cmd.Connection = cn.cnn;
                cn.cmd.Connection.Open();
                cn.cmd.CommandText = "insert into Supplier(SupplierNo,SupplierName,SupplierID,DOR,SupplierTelNo,SupplierBank,SupplierBankacc,SupplierActive,Misc1,Misc2,Misc3) Values('"+SupNo+"','"+SupName+"','"+IDNumber+"','"+DOR+"','"+Phone+"','"+Bank+"','"+Bankacc+"','"+isActive+"','"+vehicle+"','"+Driver+"','"+TurnBoy+"')";
                cn.cmd.ExecuteNonQuery();
                await this.ShowMessageAsync("Registration is a Success", " The Transporter has been entered into the system Successfuly", MessageDialogStyle.Affirmative, null);
               
                //farmerDatagrid.ItemsSource = LoadCollectionData();
                //ViewFarmersTab.BringIntoView();
               // clearRegFields();
            }
            catch (Exception ex)
            {
                ShowOverlay();
                MessageBox.Show("Registration has Failed! The following Message was generated to explain why \n \n"+ex.Message+"\n \n"+ex.InnerException,"Registration Failure",MessageBoxButton.OK,MessageBoxImage.Error);
                HideOverlay();
            }
            this.Close();
        }

        private void ActiveorNot_Checked(object sender, RoutedEventArgs e)
        {
            isActivestatus = true;
        }
private void ActiveorNot_Unchecked(object sender, RoutedEventArgs e)
{
    isActivestatus = false;
}








        
    }
}
