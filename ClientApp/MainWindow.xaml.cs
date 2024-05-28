using Microsoft.Win32;
using SharedData;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static int port = 4041; // порт сервера
        static string address = "127.0.0.1"; // адрес сервера
        IPEndPoint ipPoint;
        TcpClient client;
        public MainWindow()
        {
            InitializeComponent();
            ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
            client = new TcpClient();
            client.Connect(ipPoint);
        }

        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {            
            try
            {
                while (true)
                {
                    FileInfo info = new FileInfo(searchTb.Text);
                    InfoFile request = new InfoFile();
                    request.FileName = info.Name;
                    
                    request.Extension = info.Extension;
                    request.Data = File.ReadAllBytes(info.FullName);
                    NetworkStream ns = client.GetStream();
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ns, request);
                    MessageBox.Show("Successfully serialize");
                    

                    StreamReader sr1 = new StreamReader(ns);
                    string response = sr1.ReadLine();

                    MessageBox.Show("Server response: " + response);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Browse_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            searchTb.Text = dialog.FileName;
        }
    }
}