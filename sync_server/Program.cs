using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using SharedData;


namespace sync_server
{
    class Program
    {
        static int port = 4041; // порт для приема входящих запросов
        static void Main(string[] args)
        {
            // получаем адреса для запуска сокета
            IPAddress iPAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipPoint = new IPEndPoint(iPAddress, port);

            TcpListener listener = new TcpListener(ipPoint); // bind

            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                listener.Start(10);

                Console.WriteLine("Server started! Waiting for connection...");
                TcpClient client = listener.AcceptTcpClient();

                while (client.Connected)
                {
                    NetworkStream ns = client.GetStream();
                    BinaryFormatter bf = new BinaryFormatter();
                    InfoFile request = (InfoFile)bf.Deserialize(ns);

                   
                    Console.WriteLine($"{client.Client.RemoteEndPoint} \n " +
                        $"Filename = {request.FileName} . Extension = {request.Extension} at {DateTime.Now.ToShortTimeString()}");

                    string dir = @"C:\\Users\\M I R A D A\\Downloads";
                   //string fullname = request.FileName + request.Extension;
                    string path = Path.Combine(dir,request.FileName);
                    File.WriteAllBytes(path,request.Data);

                    
                    string message = $"Successfully saved";
                    StreamWriter sw = new StreamWriter(ns);
                    sw.WriteLine(message);
                    sw.Flush();

                }
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            listener.Stop();
        }
    }
}
