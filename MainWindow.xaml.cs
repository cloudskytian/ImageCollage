using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Image = System.Drawing.Image;
using MessageBox = System.Windows.MessageBox;

namespace ImageCollage
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Path_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path.Text = dialog.SelectedPath;
            }
        }

        private void Collage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int.Parse(row.Text);
                int.Parse(col.Text);
            }
            catch
            {
                MessageBox.Show("行列需为数字");
            }
            try
            {
                List<List<Image>> images = new List<List<Image>>();
                int height = 0;
                int width = 0;
                for (int y = 1; y <= int.Parse(row.Text); y++)
                {
                    width = 0;
                    Boolean setHeight = false;
                    List<Image> sonList = new List<Image>();
                    Image img = null;
                    for (int x = 1; x <= int.Parse(col.Text); x++)
                    {
                        img = Image.FromFile(path.Text + "\\" + prefix.Text + y + delimiter.Text + x + suffix.Text + "." + ext.Text);
                        sonList.Add(img);
                        width = width + img.Width;
                    }
                    if (setHeight == false)
                    {
                        //height = height + img.Height;
                        height = height + img.Height;
                        setHeight = true;
                    }
                    images.Add(sonList);
                }
                Image image = new Bitmap(width, height);
                Graphics graphics = Graphics.FromImage(image);
                int currentHeight = 0;
                foreach (var y in images)
                {
                    int currentWidth = 0;
                    Boolean setHeight = false;
                    int h = y.Last<Image>().Height;
                    foreach (var x in y)
                    {
                        graphics.DrawImage(x, currentWidth, currentHeight);
                        currentWidth = currentWidth + x.Width;
                        x.Dispose();
                    }
                    if (setHeight == false)
                    {
                        currentHeight = currentHeight + h;
                        setHeight = true;
                    }
                }
                image.Save(path.Text + "\\" + "image.png");
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    image.Save(memoryStream, ImageFormat.Png);
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = memoryStream;
                    bitmapImage.EndInit();
                    showImg.Source = bitmapImage;
                }
                GC.Collect();
                GC.GetTotalMemory(true);
            }
            catch
            {
                MessageBox.Show("输入有误");
            }
        }
    }
}
