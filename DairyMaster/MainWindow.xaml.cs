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
using System.Threading;
using System.Windows.Threading;

using MahApps.Metro.Controls.Dialogs;
using DairyMaster;
namespace DairyMaster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        public MainWindow()
        {
            ticks();
            initializeDB();
        }

        private  void initializeDB()
        {
            try
            {
                apps.Connection cn = new apps.Connection();
                cn.connect();
                cn.cnn.Open();
                cn.cnn.Close();
            }
            catch (Exception ex)
            {
                string x=ex.Message;
            }
        }

        private void ticks()
        {
            DispatcherTimer dt = new DispatcherTimer();
            dt.Tick += new EventHandler(dt_Tick);
            dt.Interval = new TimeSpan(0, 0, 1);
          dt.Start();
        }

        private void dt_Tick(object sender, EventArgs e)
        {
            timeLbl.Content = System.DateTime.Now.ToShortTimeString();
            datelbl.Content = System.DateTime.Now.DayOfWeek.ToString() + "\n" + System.DateTime.Now.ToShortDateString();
        }

        private void tile2_Click(object sender, RoutedEventArgs e)
        {
            apps.outDelivery tp = new apps.outDelivery();
            tp.ShowDialog();
        }

        private void Tile_Click_1(object sender, RoutedEventArgs e)
        {
            apps.indelivery delv = new apps.indelivery();
            delv.WindowState = WindowState.Maximized;
            delv.ShowDialog();
        }

        private void tile3_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tile4_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Tile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tile7_Click(object sender, RoutedEventArgs e)
        {
            apps.FarmerRegistration reg = new apps.FarmerRegistration();//'Launch registration'
            reg.WindowState = WindowState.Maximized;
            reg.ShowDialog();
        }

        private void Tile8_Click(object sender, RoutedEventArgs e)
        {
            apps.StaffRegistration sreg = new apps.StaffRegistration();
            sreg.WindowState = WindowState.Maximized;
            sreg.ShowDialog();
        }

        private void Tile9_Click(object sender, RoutedEventArgs e)
        {
            apps.Supplier sreg = new apps.Supplier();
            sreg.WindowState = WindowState.Maximized;
            sreg.ShowDialog();
        }

        private void Tile10_Click(object sender, RoutedEventArgs e)
        {
            apps.Utilities x=new apps.Utilities();
           
        }

        private void Tile10_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //Tile10.BorderThickness = ;
        }

        private void Tile10_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private async void metroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoginDialogData x=new LoginDialogData();
                x=await this.ShowLoginAsync("Login","Please Login to Use the Application",null);
                //MessageBox.Show("UserName: " + x.Username + " Password: " + x.Password);
        }
    }
}
