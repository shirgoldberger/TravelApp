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
                DbConnection.createConnection(e.Args[0], e.Args[1], e.Args[2], int.Parse(e.Args[3]));
            } catch(Exception e1)
            {
                MessageBox.Show(e1.Message);
                return;
            }
            wnd.Show();
        }
    }
}
