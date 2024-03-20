using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using AForge.Video;
using AForge.Video.DirectShow;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.IO.Ports;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;


namespace Test_SmartHome
{
    public partial class Form3 : Form
    {
        // Serial port bağlantısı için gerekli nesne
        private SerialPort serialPort;

        // E-posta gönderim bilgileri
        private string senderEmail = "testcsharp2023@gmail.com";
        private string password = "bleg oqfb vehn ptbv";
        private string recipientEmail;
        private string subject = "Subject";
        private string body = "Someone detected at the door.";

        public Form3()
        {
            InitializeComponent();

            // ESP32-CAM ile haberleşme için kullanılacak seri port adı ve bağlantı hızı
            string portName = "COM3";
            serialPort = new SerialPort(portName, 115200);

            // Serial port'u aç
            serialPort.Open();
            Console.WriteLine("Serial port opened.");

            // PictureBox ayarları
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;


        }

        private void textBox_mail_TextChanged(object sender, EventArgs e)
        {
            // textBox_mail içeriği değiştiğinde alıcı e-posta adresini güncelle
            recipientEmail = textBox_mail.Text;
        }


        private void Form3_Load(object sender, EventArgs e)
        {
            serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
        }
        // 'serialPort_DataReceived' olay işleyici yöntemini ekleyin
        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Alınan veriyi işlemek için uygun mantığı uygulayın
            // Bu yöntem, seri port üzerinde veri alındığında çağrılacaktır
        }
        private void btnsend_mail_Click(object sender, EventArgs e)
        {
            // 'g' komutunu ESP32-CAM'e gönder
            serialPort.Write("g");

            // ESP32-CAM'den Base64 kodlu fotoğrafı oku
            string base64Image = serialPort.ReadLine();

            // Base64 verisini byte dizisine çevir
            byte[] imageBytes = Convert.FromBase64String(base64Image);

            // Base64 kodlu veriyi MemoryStream'e çevir
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                // MemoryStream üzerinden resmi oluştur
                Image image = Image.FromStream(ms);

                // Resmi JPEG formatında MemoryStream'e kaydet
                using (MemoryStream jpegStream = new MemoryStream())
                {
                    image.Save(jpegStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                    // MemoryStream'i byte dizisine çevir
                    byte[] jpegBytes = jpegStream.ToArray();

                    // Alıcı e-posta adresi kontrolü
                    if (string.IsNullOrEmpty(recipientEmail))
                    {
                        Console.WriteLine("Error: Recipient email is null or empty.");
                        MessageBox.Show("Alıcı e-posta adresi belirtilmemiş!");
                        return;
                    }

                    // Base64 kodlu veriyi e-posta ile gönder
                    SendEmailWithImage(jpegBytes);

                    // Görüntüyü pictureBox1'e atayarak göster
                    pictureBox1.Image = Image.FromStream(new MemoryStream(jpegBytes));
                }
            }

            Console.WriteLine("Photo sent via email.");
        }


        private void SendEmailWithImage(byte[] imageBytes)
        {
            try
            {
                // E-posta gönderme işlemleri
                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential(senderEmail, password);
                    smtpClient.EnableSsl = true;

                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(senderEmail);
                        mailMessage.To.Add(recipientEmail);
                        mailMessage.Subject = subject;
                        mailMessage.Body = body;

                        // E-posta ekini ekle
                        Attachment attachment = new Attachment(new MemoryStream(imageBytes), "photo.jpeg", "image/jpeg");
                        mailMessage.Attachments.Add(attachment);

                        smtpClient.Send(mailMessage);
                    }
                }

                MessageBox.Show("E-posta gönderildi!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.GetType().Name} - {ex.Message}");
                MessageBox.Show($"E-posta gönderme başarısız!\nHata: {ex.Message}");
            }
        }
        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Form3 kapatıldığında seri portu kapat
            if (serialPort.IsOpen)
            {
                serialPort.Close();
                Console.WriteLine("Serial port closed.");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // PictureBox'a tıklanınca yapılacak işlemler buraya eklenebilir (isteğe bağlı)

        }
    }
}

