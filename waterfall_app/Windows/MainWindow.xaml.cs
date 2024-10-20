using System.IO;
using System.Windows;  
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using waterfall_app.Classes;
using System.Drawing;
using System.Drawing.Imaging;
using QRCoder;
using System.Windows.Controls;

namespace waterfall_app.Windows
{
    public partial class MainWindow : Window
    {
        private WaterfallDbContext _context;
        private List<Shedule> _timeSlots;
        private Shedule _selectedTimeSlot;

        public MainWindow()
        {
            InitializeComponent();
            _context = new WaterfallDbContext();
            FillTimeSlots();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var context = new WaterfallDbContext())
            {
                var ticketTypes = context.TypeTickets.ToList();
                ticketType.ItemsSource = ticketTypes;
            }
        }

        private void FillTimeSlots()
        {
            _timeSlots = _context.Shedules
                .Where(s => s.NumberOfPeople < 15 && s.EntranceTime > DateTime.Now)
                .OrderBy(s => s.IdShedule)
                .ToList();
            listViewTimeSlots.ItemsSource = _timeSlots;
        }

        private void printTicketButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedTimeSlot != null &&
                !string.IsNullOrWhiteSpace(visitorName.Text) &&
                !string.IsNullOrWhiteSpace(visitorSurname.Text) &&
                !string.IsNullOrWhiteSpace(visitorEmail.Text) &&
                IsValidEmail(visitorEmail.Text) &&
                !string.IsNullOrWhiteSpace(visitorCountry.Text) &&
                int.TryParse(visitorAge.Text, out int ageValue))
            {
                var visitor = new Visiter
                {
                    Firstname = visitorName.Text,
                    Surname = visitorSurname.Text,
                    Fathersname = visitorFathersName.Text,
                    Email = visitorEmail.Text,
                    Country = visitorCountry.Text,
                    Age = ageValue
                };

                _context.Visiters.Add(visitor);
                _context.SaveChanges();

                var ticket = new Ticket
                {
                    NumberTicket = GenerateRandomTicketNumber(),
                    IdVisiter = visitor.IdVisiter,
                    IdShedule = _selectedTimeSlot.IdShedule
                };

                _context.Tickets.Add(ticket);
                _selectedTimeSlot.NumberOfPeople++;
                _context.SaveChanges();

                MessageBox.Show("Вы успешно зарегистрировались");

                ClearData();
                FillTimeSlots();

                string qrCodeData = $"Номер билета: {ticket.NumberTicket}\nПосещающий: {visitor.Firstname} {visitor.Surname}\nПочта: {visitor.Email}";
                BitmapImage qrCodeImage = GenerateQRCode(qrCodeData);

                QRCodeWindow qrCodeWindow = new QRCodeWindow(qrCodeImage);
                qrCodeWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Заполните все поля, введите корректный адрес электронной почты и выберите время");
            }
        }

        private void listViewTimeSlots_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (listViewTimeSlots.SelectedItem != null)
            {
                _selectedTimeSlot = listViewTimeSlots.SelectedItem as Shedule;
            }
        }

        private string GenerateRandomTicketNumber()
        {
            Random random = new Random();
            return new string(Enumerable.Range(0, 8).Select(_ => (char)('0' + random.Next(0, 10))).ToArray());
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern);
            }
            catch
            {
                return false;
            }
        }

        private void ClearData()
        {
            visitorName.Text = string.Empty;
            visitorSurname.Text = string.Empty;
            visitorFathersName.Text = string.Empty;
            visitorEmail.Text = string.Empty;
            visitorCountry.Text = string.Empty;
            visitorAge.Text = string.Empty;
            _selectedTimeSlot = null;
        }

        private BitmapImage GenerateQRCode(string data)
        {
            using (var qrGenerator = new QRCodeGenerator())
            {
                using (var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q))
                {
                    using (var qrCode = new QRCode(qrCodeData))
                    {
                        using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
                        {
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                qrCodeImage.Save(memoryStream, ImageFormat.Png);
                                memoryStream.Position = 0;

                                BitmapImage bitmapImage = new BitmapImage();
                                bitmapImage.BeginInit();
                                bitmapImage.StreamSource = memoryStream;
                                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                                bitmapImage.EndInit();
                                bitmapImage.Freeze();

                                return bitmapImage;
                            }
                        }
                    }
                }
            }
        }
    }
}

