using ScreenShooter.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenShooter
{
    public static class ScreenShooter
    {
        /// <summary>
        /// file filters
        /// </summary>
        private static string[] filters = {
            "Portable Network Graphics (*.png)|*.png",
            "Joint Photographic Experts Group (*.jpeg)|*.jpeg",
            "Graphics Interchange Format (*.gif)|*.gif",
            "Tagged Image File Format (*.tiff)|*.tiff",
            "Windows Bitmap (*.bmp)|*.bmp"
        };

        /// <summary>
        /// image-formats
        /// </summary>
        private static ImageFormat[] formats = {
            ImageFormat.Png,
            ImageFormat.Jpeg,
            ImageFormat.Gif,
            ImageFormat.Tiff,
            ImageFormat.Bmp
        };

        /// <summary>
        /// capture the primary screen and returns it at bitmap
        /// </summary>
        /// <returns>the captures screen as bitmap</returns>
        public static Bitmap CaptureScreen()
        {
            var screen = Screen.PrimaryScreen;
            var bounds = screen.Bounds;
            var format = PixelFormat.Format32bppArgb;
            var bmp = new Bitmap(bounds.Width, bounds.Height, format);
            var gfx = Graphics.FromImage(bmp);
            gfx.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);
            return bmp;
        }

        /// <summary>
        /// ask for a file and save the bitmap to the file
        /// </summary>
        /// <param name="bmp">the bitmap to save</param>
        public static void SaveBitmap(Bitmap bmp)
        {
            var now = DateTime.Now;
            var dir
                = string.IsNullOrWhiteSpace(Settings.Default.Dir)
                ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                : Settings.Default.Dir;

            var dialog = new SaveFileDialog
            {
                FileName = $"{now.Year:0000}{now.Month:00}{now.Day:00} {now.Hour:00}{now.Minute:00}{now.Second:00}",
                InitialDirectory = dir,
                Filter = string.Join("|", filters),
                FilterIndex = Settings.Default.FilterIndex
            };

            var result = dialog.ShowDialog();
            if (result != DialogResult.OK) return;

            var filterindex = Math.Max(1, Math.Min(formats.Length, dialog.FilterIndex));
            var format = formats[filterindex - 1];
            dir = Path.GetDirectoryName(dialog.FileName);

            Settings.Default.Dir = dir;
            Settings.Default.FilterIndex = filterindex;
            Settings.Default.Save();

            bmp.Save(dialog.FileName, format);
        }
    }
}
