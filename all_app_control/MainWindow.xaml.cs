using System;
using System.Windows;
using System.IO.Ports;

namespace all_app_control
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // khai báo các biến serialPort

        static public SerialPort Card_barcode = new SerialPort();
        static public SerialPort Book_barcode = new SerialPort();
        static public SerialPort Arduino = new SerialPort();
        public enum SIGNAL:Int32
        {
            SIGNAL_CARDBARCODE = 0,
            SIGNAL_BOOKBARCODE = 1,
            SIGNAL_FINISH = 2
        }
        // Tìm cổng com khả dụng
        void getAvailablePort()
        {
            com1.Items.Clear();
            com2.Items.Clear();
            com3.Items.Clear();
            String[] ports= SerialPort.GetPortNames();
            foreach (String port in ports)
            {
                com1.Items.Add(port);
                com2.Items.Add(port);
                com3.Items.Add(port);
            }
        }


        // Nhận dữ liệu
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
          //  DataReciever.Text = indata;
            Arduino.Write("0");
        }
        private void DataReceivedHandler1(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
           // DataReciever.Text = indata;
            Arduino.Write("1");
        }


        public MainWindow()
        {
            InitializeComponent();
            Data.Text = "No action did!";
            getAvailablePort();
            Card_barcode.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            Book_barcode.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler1);
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (Data.Text == "Connected")
            {
                Data.Text = "ngắt kết nối đi đã bạn ơi!";
            }
            else if (com1.Text == "" || com2.Text == "")
            {
                Data.Text = "Chưa chọn cổng com kìa BRO!";
            }
            else
            {
                try
                {
                    //_____dinh cau hinh cho com1___________________
                    string Baud = "9600";
                    Card_barcode.PortName = com1.Text;
                    Card_barcode.BaudRate = Convert.ToInt32(Baud);
                    Card_barcode.RtsEnable = true;
                    Card_barcode.DtrEnable = true;

                    Card_barcode.Open();
                    //textBox1.Enabled = true;
                    //____dinh cau hinh cho com2______________________
                    string Baud2 = "9600";
                    Book_barcode.PortName = com2.Text;
                    Book_barcode.BaudRate = Convert.ToInt32(Baud2);
                    Book_barcode.RtsEnable = true;
                    Book_barcode.DtrEnable = true;
                    Book_barcode.Open();
                    Data.Text = "Connected";

                    //textBox1.Enabled = true;
                    //____dinh cau hinh cho com3______________________
                    string Baud3 = "9600";
                    Arduino.PortName = com3.Text;
                    Arduino.BaudRate = Convert.ToInt32(Baud3);
                    Arduino.RtsEnable = true;
                    Arduino.DtrEnable = true;
                    Arduino.Open();
                    Data.Text = "Connected";

                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("Lỗi kết nối hoặc cổng COM đang được truy cập bởi 1 phần mềm khác!");
                }
            }
        }

        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
            Card_barcode.Close();
            Book_barcode.Close();
            Arduino.Close();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Card_barcode.Close();
            Book_barcode.Close();
            Arduino.Close();
            getAvailablePort();
            Data.Text = "refreshed!";
        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Arduino.Write("2");
            }
            catch
            {
                MessageBox.Show("Arduino chưa được kết nối");
            }
        }
    }
}
