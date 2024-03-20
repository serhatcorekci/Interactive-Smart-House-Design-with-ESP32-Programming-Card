using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;


namespace Test_SmartHome
{
    public partial class Form2 : Form
    {
        private SerialPort serialPort;
        private string data;   //data değişkenini oluştur

        string[] bulb  = { "Close", "Low", "Medium", "High" };
        string[] bulb1 = { "Close", "Low", "Medium", "High" };
        string[] bulb2 = { "Close", "Low", "Medium", "High" };
        string[] bulb3 = { "Close", "Low", "Medium", "High" };
        string[] siren = { "Close", "Slow", "Fast" };
        string[] rain  = { "Close", "Open" };
        string[] cam   = { "Close", "Open" };

        public Form2()
        {
            InitializeComponent();
            serialPortESP32 = new SerialPort();
            //serialPort = new SerialPort("COM4", 11500); // COM3, Arduino'nun bağlı olduğu portu temsil eder
            //try
            //{
            //    serialPort.Open(); // Seri portu açın
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Seri port açma hatası: " + ex.Message);
            //}
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            comboBox_bulb.Text = "Select Level";
            comboBox_bulb.Items.AddRange(bulb);
            comboBox_bulb1.Text = "Select Level";
            comboBox_bulb1.Items.AddRange(bulb1);
            comboBox_bulb2.Text = "Select Level";
            comboBox_bulb2.Items.AddRange(bulb2);
            comboBox_bulb3.Text = "Select Level";
            comboBox_bulb3.Items.AddRange(bulb3);
            comboBox_siren.Text = "Select Level";
            comboBox_siren.Items.AddRange(siren);
            comboBox_rain.Text = "Select Level";
            comboBox_rain.Items.AddRange(rain);
            comboBox_cam.Text = "Select Operation";
            comboBox_cam.Items.AddRange(cam);

            lbl_blg.Text = "";
            

            string[] ports = SerialPort.GetPortNames();  //Seri portları diziye ekleme
            foreach (string port in ports)
                comboBox_ports.Items.Add(port);               //Seri portları comboBox_ports'e ekleme
                comboBox_ports.SelectedIndex = 0;

            serialPortESP32.DataReceived += new SerialDataReceivedEventHandler(serialPort32_DataReceived);       //DataReceived eventini oluşturma
        }

        private void btn_return_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
            if (serialPortESP32.IsOpen) 
                serialPortESP32.Close();    //Seri port açıksa kapat
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPortESP32.IsOpen)
                {
                    serialPortESP32.Close();  // Eğer açıksa seri portu kapat
                }

                Application.Exit();  // Uygulamadan çıkış yap
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Çıkış Sırasında Hata");  // Bir istisna oluştuğunda hata mesajını gösterin
            }
        }

        private void serialPort32_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string[] splitted_data;
            data = serialPortESP32.ReadLine();
            splitted_data = data.Split('/');

            if (splitted_data.Length >= 7)
            {
                // MOTION SENSOR___________________________________________
                if (int.TryParse(splitted_data[0], out int result) && result == 1)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        textBox_pir.Text = "MOTION DETECTED!";
                        lbl_siren.Text = "There is someone. Do you want to activate the siren?";

                        // Motion durumu için metin rengini kırmızı yap
                        textBox_pir.ForeColor = Color.Red;
                        lbl_siren.ForeColor = Color.Red;
                    });
                }
                else if (int.TryParse(splitted_data[0], out int resultZero) && resultZero == 0)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        textBox_pir.Text = "NO MOTION DETECTED!";
                        lbl_siren.Text = "No one is around.";

                        // No Motion durumu için metin rengini siyah yap
                        textBox_pir.ForeColor = Color.Black;
                        lbl_siren.ForeColor = Color.Black;
                    });
                }
                // MOTION SENSOR END_______________________________________

                // RAIN SENSOR_____________________________________________
                if (int.TryParse(splitted_data[1], out int rainValue))
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        if (rainValue > 350 && rainValue < 800)
                        {
                            textBox_rain.Text = "IT'S RAINING!";
                            lbl_rain.Text = "It's raining. Do you want to close the window?";

                            // Raining durumu için metin rengini kırmızı yap
                            textBox_rain.ForeColor = Color.Red;
                            lbl_rain.ForeColor = Color.Red;
                        }
                        else if (rainValue < 350)
                        {
                            textBox_rain.Text = "HEAVY RAIN DETECTED!";
                            lbl_rain.Text = "It's heavy raining. Do you want to close the window?";

                            // Heavy Raining durumu için metin rengini kırmızı yap
                            textBox_rain.ForeColor = Color.Red;
                            lbl_rain.ForeColor = Color.Red;
                        }
                        else if (rainValue > 800)
                        {
                            textBox_rain.Text = "NO RAIN!";
                            lbl_rain.Text = "It's sunny. Do you want to open the window?";

                            // No Rain durumu için metin rengini siyah yap
                            textBox_rain.ForeColor = Color.Black;
                            lbl_rain.ForeColor = Color.Black;
                        }
                    });
                }
                // RAIN SENSOR END_________________________________________

                // DISTANCE SENSOR_________________________________________
                if (long.TryParse(splitted_data[2], out long distanceValue))
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        if (distanceValue <= 15)
                        {
                            textBox_distance.Text = $"SOMEONE DETECTED!  {distanceValue} cm"; ;
                            lbl_cam.Text = "There is someone. Do you want to take a photo of it?";

                            // Someone Detected durumu için metin rengini kırmızı yap
                            textBox_distance.ForeColor = Color.Red;
                            lbl_cam.ForeColor = Color.Red;
                        }
                        else if (distanceValue > 15)
                        {
                            textBox_distance.Text = "NOTHING DETECTED!";
                            lbl_cam.Text = "There is no someone.";

                            // Nothing Detected durumu için metin rengini siyah yap
                            textBox_distance.ForeColor = Color.Black;
                            lbl_cam.ForeColor = Color.Black;
                        }
                    });
                }
                // DISTANCE SENSOR END_____________________________________

                // LDR SENSOR______________________________________________
                if (int.TryParse(splitted_data[3], out int ldrValue))
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        // Yüzdelik değeri hesapla
                        float isik_yuzdelik = (float)ldrValue * 100 / 4095;

                        // TextBox'ta göster
                        textBox_lightper.Text = $"{isik_yuzdelik:F2}";

                        if (ldrValue < 25)
                        {
                            textBox_lightint.Text = "IT'S DARK!";
                            lbl_bulb.Text = "It's dark. Do you want to turn on the lights?";

                            // It's Dark durumu için metin rengini siyah yap
                            textBox_lightint.ForeColor = Color.Red;
                            lbl_bulb.ForeColor = Color.Red;
                        }
                        else if (ldrValue < 450)
                        {
                            textBox_lightint.Text = "IT'S DIM!";
                            lbl_bulb.Text = "It's dim. Do you want to turn on the lights?";

                            // It's Dim durumu için metin rengi
                            textBox_lightint.ForeColor = Color.DarkOrange;
                            lbl_bulb.ForeColor = Color.DarkOrange;
                        }
                        else
                        {
                            textBox_lightint.Text = "IT'S BRIGHT!";
                            lbl_bulb.Text = "It's bright. Do you want to turn off the lights?";

                            // It's Bright durumu için metin rengi
                            textBox_lightint.ForeColor = Color.Orange;
                            lbl_bulb.ForeColor = Color.Orange;
                        }
                    });
                }
                // LDR SENSOR END__________________________________________

                // LDR SENSOR1_____________________________________________
                if (int.TryParse(splitted_data[4], out int ldrValue1))
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        // Yüzdelik değeri hesapla
                        float isik_yuzdelik1 = (float)ldrValue1 * 100 / 4095;

                        // TextBox'ta göster
                        textBox_lightper1.Text = $"{isik_yuzdelik1:F2}";

                        if (ldrValue1 < 500)
                        {
                            textBox_lightint1.Text = "IT'S DARK!";
                            lbl_bulb1.Text = "It's dark. Do you want to turn on the lights?";

                            // It's Dark durumu için metin rengini siyah yap
                            textBox_lightint1.ForeColor = Color.Red;
                            lbl_bulb1.ForeColor = Color.Red;
                        }
                        else if (ldrValue1 < 780)
                        {
                            textBox_lightint1.Text = "IT'S DIM!";
                            lbl_bulb1.Text = "It's dim. Do you want to turn on the lights?";

                            // It's Dim durumu için metin rengi
                            textBox_lightint1.ForeColor = Color.DarkOrange;
                            lbl_bulb1.ForeColor = Color.DarkOrange;
                        }
                        else
                        {
                            textBox_lightint1.Text = "IT'S BRIGHT!";
                            lbl_bulb1.Text = "It's bright. Do you want to turn off the lights?";

                            // It's Bright durumu için metin rengi
                            textBox_lightint1.ForeColor = Color.Orange;
                            lbl_bulb1.ForeColor = Color.Orange;
                        }
                    });
                }
                // LDR SENSOR1 END_________________________________________

                // LDR SENSOR2_____________________________________________
                if (int.TryParse(splitted_data[5], out int ldrValue2))
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        // Yüzdelik değeri hesapla
                        float isik_yuzdelik2 = (float)ldrValue2 * 100 / 4095;

                        // TextBox'ta göster
                        textBox_lightper2.Text = $"{isik_yuzdelik2:F2}";

                        if (ldrValue2 < 5)
                        {
                            textBox_lightint2.Text = "IT'S DARK!";
                            lbl_bulb2.Text = "It's dark. Do you want to turn on the lights?";

                            // It's Dark durumu için metin rengini siyah yap
                            textBox_lightint2.ForeColor = Color.Red;
                            lbl_bulb2.ForeColor = Color.Red;
                        }
                        else if (ldrValue2 < 220)
                        {
                            textBox_lightint2.Text = "IT'S DIM!";
                            lbl_bulb2.Text = "It's dim. Do you want to turn on the lights?";

                            // It's Dim durumu için metin rengi
                            textBox_lightint2.ForeColor = Color.DarkOrange;
                            lbl_bulb2.ForeColor = Color.DarkOrange;
                        }
                        else
                        {
                            textBox_lightint2.Text = "IT'S BRIGHT!";
                            lbl_bulb2.Text = "It's briSSght. Do you want to turn off the lights?";

                            // It's Bright durumu için metin rengi
                            textBox_lightint2.ForeColor = Color.Orange;
                            lbl_bulb2.ForeColor = Color.Orange;
                        }
                    });
                }
                // LDR SENSOR2 END_________________________________________

                // LDR SENSOR3_____________________________________________
                if (int.TryParse(splitted_data[6], out int ldrValue3))
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        // Yüzdelik değeri hesapla
                        float isik_yuzdelik3 = (float)ldrValue3 * 100 / 4095;

                        // TextBox'ta göster
                        textBox_lightper3.Text = $"{isik_yuzdelik3:F2}";

                        if (ldrValue3 < 150)
                        {
                            textBox_lightint3.Text = "IT'S DARK!";
                            lbl_bulb3.Text = "It's dark. Do you want to turn on the lights?";

                            // It's Dark durumu için metin rengini siyah yap
                            textBox_lightint3.ForeColor = Color.Red;
                            lbl_bulb3.ForeColor = Color.Red;
                        }
                        else if (ldrValue3 < 300)
                        {
                            textBox_lightint3.Text = "IT'S DIM!";
                            lbl_bulb3.Text = "It's dim. Do you want to turn on the lights?";

                            // It's Dim durumu için metin rengi
                            textBox_lightint3.ForeColor = Color.DarkOrange;
                            lbl_bulb3.ForeColor = Color.DarkOrange;
                        }
                        else
                        {
                            textBox_lightint3.Text = "IT'S BRIGHT!";
                            lbl_bulb3.Text = "It's bright. Do you want to turn off the lights?";

                            // It's Bright durumu için metin rengi
                            textBox_lightint3.ForeColor = Color.Orange;
                            lbl_bulb3.ForeColor = Color.Orange;
                        }
                    });
                }
                // LDR SENSOR3 END_________________________________________
            }
            else
            {
                // Hata durumunu işle
                Console.WriteLine("splitted_data dizisinin boyutu beklenenden küçük.");
            }
    }

        private void btnedit_rain_Click(object sender, EventArgs e)
        {
            int choosewindowposition = comboBox_rain.SelectedIndex;
            switch (choosewindowposition)
            {
                case 0:
                    textBox_window.Text = "WINDOW IS CLOSED";
                    textBox_window.ForeColor = Color.Black;
                    serialPortESP32.Write("a");    //Seri porta "0" gönder
                    break;
                case 1:
                    textBox_window.Text = "WINDOW IS OPEN";
                    textBox_window.ForeColor = Color.Red;
                    serialPortESP32.Write("b");    //Seri porta "1" gönder
                    break;
            }
        }

        private void btnedit_siren_Click(object sender, EventArgs e)
        {
            int choosesirenspeed = comboBox_siren.SelectedIndex;
            switch (choosesirenspeed)
            {
                case 0:
                    textBox_siren.Text = "SIREN IS CLOSED";
                    textBox_siren.ForeColor = Color.Black;
                    serialPortESP32.Write("c");    //Seri porta "0" gönder
                    break;
                case 1:
                    textBox_siren.Text = "SIREN IS SLOW";
                    textBox_siren.ForeColor = Color.DarkOrange;
                    serialPortESP32.Write("d");    //Seri porta "1" gönder
                    break;
                case 2:
                    textBox_siren.Text = "SIREN IS FAST";
                    textBox_siren.ForeColor = Color.Red;
                    serialPortESP32.Write("e");    //Seri porta "1" gönder
                    break;
            }
        }

        private void btnedit_cam_Click(object sender, EventArgs e)
        {
            int choosecamoperation = comboBox_cam.SelectedIndex;
            switch (choosecamoperation)
            {
                case 0:
                    textBox_cam.Text = "CAMERA IS CLOSED";
                    textBox_cam.ForeColor = Color.Black;
                    //serialPortESP32.Write("f");    //Seri porta "f" gönder
                    break;
                case 1:
                    textBox_cam.Text = "CAMERA IS OPEN";
                    textBox_cam.ForeColor = Color.Red;
                    Form3 form3 = new Form3();
                    form3.Show();
                    break;
            }
        }

        private void btnedit_bulb_Click(object sender, EventArgs e)
        {
            int choosebulblight = comboBox_bulb.SelectedIndex;
            switch (choosebulblight)
            {
                case 0:
                    textBox_bulbint.Text = "BULB IS CLOSED";
                    textBox_bulbint.ForeColor = Color.Black;
                    serialPortESP32.Write("h");    //Seri porta "h" gönder
                    break;
                case 1:
                    textBox_bulbint.Text = "BULB IS LOW BRIGHTNESS";
                    textBox_bulbint.ForeColor = Color.Red;
                    serialPortESP32.Write("i");    //Seri porta "i" gönder
                    break;
                case 2:
                    textBox_bulbint.Text = "BULB IS MEDIUM BRIGHTNESS";
                    textBox_bulbint.ForeColor = Color.DarkOrange;
                    serialPortESP32.Write("j");    //Seri porta "j" gönder
                    break;
                case 3:
                    textBox_bulbint.Text = "BULB IS HIGH BRIGHTNESS";
                    textBox_bulbint.ForeColor = Color.Orange;
                    serialPortESP32.Write("k");    //Seri porta "k" gönder
                    break;
            }
        }

        private void btnedit_bulb1_Click(object sender, EventArgs e)
        {
            int choosebulblight1 = comboBox_bulb1.SelectedIndex;
            switch (choosebulblight1)
            {
                case 0:
                    textBox_bulbint1.Text = "BULB IS CLOSED";
                    textBox_bulbint1.ForeColor = Color.Black;
                    serialPortESP32.Write("l");    //Seri porta "l" gönder
                    break;
                case 1:
                    textBox_bulbint1.Text = "BULB IS LOW BRIGHTNESS";
                    textBox_bulbint1.ForeColor = Color.Red;
                    serialPortESP32.Write("m");    //Seri porta "m" gönder
                    break;
                case 2:
                    textBox_bulbint1.Text = "BULB IS MEDIUM BRIGHTNESS";
                    textBox_bulbint1.ForeColor = Color.DarkOrange;
                    serialPortESP32.Write("n");    //Seri porta "n" gönder
                    break;
                case 3:
                    textBox_bulbint1.Text = "BULB IS HIGH BRIGHTNESS";
                    textBox_bulbint1.ForeColor = Color.Orange;
                    serialPortESP32.Write("o");    //Seri porta "o" gönder
                    break;
            }
        }

        private void btnedit_bulb2_Click(object sender, EventArgs e)
        {
            int choosebulblight2 = comboBox_bulb2.SelectedIndex;
            switch (choosebulblight2)
            {
                case 0:
                    textBox_bulbint2.Text = "BULB IS CLOSED";
                    textBox_bulbint2.ForeColor = Color.Black;
                    serialPortESP32.Write("p");    //Seri porta "p" gönder
                    break;
                case 1:
                    textBox_bulbint2.Text = "BULB IS LOW BRIGHTNESS";
                    textBox_bulbint2.ForeColor = Color.Red;
                    serialPortESP32.Write("r");    //Seri porta "r" gönder
                    break;
                case 2:
                    textBox_bulbint2.Text = "BULB IS MEDIUM BRIGHTNESS";
                    textBox_bulbint2.ForeColor = Color.DarkOrange;
                    serialPortESP32.Write("s");    //Seri porta "s" gönder
                    break;
                case 3:
                    textBox_bulbint2.Text = "BULB IS HIGH BRIGHTNESS";
                    textBox_bulbint2.ForeColor = Color.Orange;
                    serialPortESP32.Write("t");    //Seri porta "t" gönder
                    break;
            }
        }

        private void btnedit_bulb3_Click(object sender, EventArgs e)
        {
            int choosebulblight3 = comboBox_bulb3.SelectedIndex;
            switch (choosebulblight3)
            {
                case 0:
                    textBox_bulbint3.Text = "BULB IS CLOSED";
                    textBox_bulbint3.ForeColor = Color.Black;
                    serialPortESP32.Write("u");    //Seri porta "u" gönder
                    break;
                case 1:
                    textBox_bulbint3.Text = "BULB IS LOW BRIGHTNESS";
                    textBox_bulbint3.ForeColor = Color.Red;
                    serialPortESP32.Write("v");    //Seri porta "v" gönder
                    break;
                case 2:
                    textBox_bulbint3.Text = "BULB IS MEDIUM BRIGHTNESS";
                    textBox_bulbint3.ForeColor = Color.DarkOrange;
                    serialPortESP32.Write("y");    //Seri porta "y" gönder
                    break;
                case 3:
                    textBox_bulbint3.Text = "BULB IS HIGH BRIGHTNESS";
                    textBox_bulbint3.ForeColor = Color.Orange;
                    serialPortESP32.Write("z");    //Seri porta "z" gönder
                    break;
            }
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            try
            {
                serialPortESP32.PortName = comboBox_ports.Text;  //Port ismini comboBox1'in text'i olarak belirle
                serialPortESP32.Open();                          //Seri portu aç
                serialPortESP32.BaudRate = 115200;
                btn_disconnect.Enabled = true;               //"Disconnect" butonunu tıklanabilir yap
                btn_connect.Enabled = false;                 //"Connect" butonunu tıklanamaz yap
                lbl_blg.Text = "Connected";
                lbl_blg.ForeColor = Color.Green;              //lbl_blg yazı rengini yeşil yap

                textBox_window.Text = "WINDOW IS CLOSED";
                textBox_window.ForeColor = Color.Black;
                textBox_siren.Text = "SIREN IS CLOSED";
                textBox_siren.ForeColor = Color.Black;
                textBox_cam.Text = "CAMERA IS CLOSED";
                textBox_cam.ForeColor = Color.Black;
                textBox_bulbint.Text = "BULB IS CLOSED";
                textBox_bulbint.ForeColor = Color.Black;
                textBox_bulbint1.Text = "BULB IS CLOSED";
                textBox_bulbint1.ForeColor = Color.Black;
                textBox_bulbint2.Text = "BULB IS CLOSED";
                textBox_bulbint2.ForeColor = Color.Black;
                textBox_bulbint3.Text = "BULB IS CLOSED";
                textBox_bulbint3.ForeColor = Color.Black;
            }
            catch (Exception ex2)
            {
                MessageBox.Show(ex2.Message, "Hata");  //Hata mesajı
            }
        }

        private void btn_disconnect_Click(object sender, EventArgs e)
        {
            try
            {
                lbl_blg.Text = "Disconnecting...";
                lbl_blg.ForeColor = Color.Black;

                if (serialPortESP32.IsOpen)
                {
                    serialPortESP32.Write("a");    //Seri porta "a" gönder
                    serialPortESP32.Write("c");    //Seri porta "c" gönder
                    serialPortESP32.Write("f");    //Seri porta "f" gönder
                    serialPortESP32.Write("h");    //Seri porta "h" gönder
                    serialPortESP32.Write("l");    //Seri porta "l" gönder
                    serialPortESP32.Write("p");    //Seri porta "p" gönder
                    serialPortESP32.Write("u");    //Seri porta "u" gönder
                    serialPortESP32.Close();
                    lbl_blg.Text = "Disconnected";
                    lbl_blg.ForeColor = Color.Red;
                    btn_disconnect.Enabled = false;
                    btn_connect.Enabled = true;
                }
                else
                {
                    lbl_blg.Text = "Connection Already Closed";
                    lbl_blg.ForeColor = Color.Black;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error During Disconnection");
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serialPortESP32.IsOpen)
                serialPortESP32.Write("a");    //Seri porta "a" gönder
                serialPortESP32.Write("c");    //Seri porta "c" gönder
                serialPortESP32.Write("f");    //Seri porta "f" gönder
                serialPortESP32.Write("h");    //Seri porta "h" gönder
                serialPortESP32.Write("l");    //Seri porta "l" gönder
                serialPortESP32.Write("p");    //Seri porta "p" gönder
                serialPortESP32.Write("u");    //Seri porta "u" gönder
                serialPortESP32.Close();    //Seri port açıksa kapat
        }
    }
}
