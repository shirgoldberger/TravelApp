using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TravelApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow wnd = new MainWindow();
            try
            {
                string user_id = ConfigurationManager.AppSettings.Get("user_id");
                string password = ConfigurationManager.AppSettings.Get("password");
                string db_name = ConfigurationManager.AppSettings.Get("db_name");
                int port = int.Parse(ConfigurationManager.AppSettings.Get("port"));
                DbConnection.createConnection(user_id, password, db_name, port);
            } 
            catch(Exception e1)
            {
                MessageBox.Show(e1.Message);
                System.Environment.Exit(0);
                return;
            }
            wnd.Show();
        }
    }
}
