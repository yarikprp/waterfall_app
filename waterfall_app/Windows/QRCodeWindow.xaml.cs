using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace waterfall_app.Windows
{
    public partial class QRCodeWindow : Window
    {
        public QRCodeWindow(BitmapImage qrCodeImage)
        {
            InitializeComponent();
            ImageQRCode.Source = qrCodeImage;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(ImageQRCode, "Печать QR-кода");
            }
        }
    }
}