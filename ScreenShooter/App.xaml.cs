using ScreenShooter.Properties;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace ScreenShooter
{
    public partial class App : Application 
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                var bmp = ScreenShooter.CaptureScreen();

                ScreenShooter.SaveBitmap(bmp);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

            Shutdown();
        }
    }
}