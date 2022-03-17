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
using System.IO;
using System.Drawing;
using QRCoder;

namespace QRCode_Generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void generate_btn_Click(object sender, RoutedEventArgs e)
        {
            string url = url_txt.Text;
            string foreground = foreground_txt.Text;
            string background = background_txt.Text;
            string fileName = fileName_txt.Text;

            string default_foreground = "#000000";
            string default_background = "#ffffff";
            string default_fileName = "qrcode.jpg";

            string root_path = Directory.GetCurrentDirectory();
            string temp_path = root_path + "/" + "temp";
            string backup_path = root_path + "/" + "backup";

            bool isComplete = false;

            if (foreground == "") foreground = default_foreground;

            if (background == "") background = default_background;

            if (fileName == "") fileName = default_fileName;

            if (url != "")
            {
                try
                {
                    QRCodeGenerator codeGenerator = new QRCodeGenerator();
                    QRCodeData codeData = codeGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
                    QRCode code = new QRCode(codeData);
                    Bitmap codeImage = code.GetGraphic(20, foreground, background);

                    Directory.CreateDirectory(temp_path);
                    Directory.CreateDirectory(backup_path);
                    codeImage.Save(temp_path + "/" + fileName);
                    codeImage.Save(backup_path + "/" + fileName);

                    ImageSource imageSource = new BitmapImage(new Uri(temp_path + "/" + fileName));
                    code_img.Source = imageSource;

                    isComplete = true;

                    if (isComplete)
                    {
                        MessageBox.Show("Generate QRCode completed", "QRCode Generator", MessageBoxButton.OK, MessageBoxImage.Information);
                        url_txt.Clear();
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show("Error, " + error, "QRCode Generator", MessageBoxButton.OK, MessageBoxImage.Error);
                    MessageBox.Show("Change file name", "QRCode Generator", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Error, URL is null", "QRCode Generator", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void save_btn_Click(object sender, RoutedEventArgs e)
        {
            string fileName = fileName_txt.Text;
            string location = location_txt.Text;

            string root_path = Directory.GetCurrentDirectory();
            string backup_path = root_path + "/" + "backup";

            string default_fileName = "qrcode.jpg";

            bool isComplete = false;

            if (fileName == "") fileName = default_fileName;

            if (location != "")
            {
                try
                {
                    string sourceFile = System.IO.Path.Combine(backup_path, fileName);
                    string destFile = System.IO.Path.Combine(location, fileName);

                    System.IO.File.Copy(sourceFile, destFile, true);

                    isComplete = true;

                    if (isComplete)
                    {
                        MessageBox.Show("Completed", "QRCode Generator", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show("Error, " + error, "QRCode Generator", MessageBoxButton.OK, MessageBoxImage.Error);
                    MessageBox.Show("Restart program", "QRCode Generator", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Error, Location is null", "QRCode Generator", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void clearTemp_btn_Click(object sender, RoutedEventArgs e)
        {
            string root_path = Directory.GetCurrentDirectory();
            string temp_path = root_path + "/" + "temp";
            string backup_path = root_path + "/" + "backup";

            if (Directory.Exists(temp_path))
            {
                try
                {
                    Directory.Delete(temp_path, true);
                }
                catch (Exception error)
                {
                    MessageBox.Show("Error, " + error, "QRCode Generator", MessageBoxButton.OK, MessageBoxImage.Error);
                    MessageBox.Show("Restart program", "QRCode Generator", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }

            if (Directory.Exists(backup_path))
            {
                try
                {
                    Directory.Delete(backup_path, true);
                }
                catch (Exception error)
                {
                    MessageBox.Show("Error, " + error, "QRCode Generator", MessageBoxButton.OK, MessageBoxImage.Error);
                    MessageBox.Show("Restart program", "QRCode Generator", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
        }

        private void exit_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
