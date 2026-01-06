using System;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace FarsiLibrary.WPFDemo
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            try
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("fa-ir");
                Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;

                new App().Run(new MainWindow());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
