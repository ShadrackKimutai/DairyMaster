using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
namespace DairyMaster.apps
{
    
    class Connection
    {
        public SqlCommand cmd=new SqlCommand();
        public SqlConnection cnn = new SqlConnection();
        public SqlDataAdapter da = new SqlDataAdapter();
        public DataSet ds = new DataSet();
        public SqlDataReader dr;
        public String connectionString = DairyMaster.Properties.Resources.tempDbPath;
        public void connect()
        {
            try {
            if ((cnn.State == ConnectionState.Connecting) || (cnn.State == ConnectionState.Open))
            {
                cnn.Close();
                cnn = new SqlConnection();
                cnn.ConnectionString = connectionString;

            }
            else
            {
                cnn = new SqlConnection();
                cnn.ConnectionString = connectionString;
            }
 }
            catch 
            {

            }
        }
    }
}
