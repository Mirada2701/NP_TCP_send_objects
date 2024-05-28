using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using SharedData;


namespace sync_client
{
    class Program
    {
        // адрес и порт сервера, к которому будем подключаться
        static int port = 4041; // порт сервера
        static string address = "127.0.0.1"; // адрес сервера
        static void Main(string[] args)
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
   
            TcpClient client = new TcpClient();

            // подключаемся к удаленному хосту
            client.Connect(ipPoint);
            try
            {
                while (true)
                {
                    Request request = new Request();
                    Console.Write("Enter A:");
                    request.A = double.Parse(Console.ReadLine()!);
                    Console.Write("Enter B:");
                    request.B = double.Parse(Console.ReadLine()!);
                    Console.Write("Enter Operation [1-4]:");
                    request.Operation = (OperationType)Enum.Parse(typeof(OperationType),Console.ReadLine()!);

                    NetworkStream ns = client.GetStream();
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ns, request);


                    StreamWriter sw = new StreamWriter(ns);
                    //sw.WriteLine(message);


                    sw.Flush(); // send all buffered data and clear the buffer


                    StreamReader sr = new StreamReader(ns);
                    string response = sr.ReadLine();

                    Console.WriteLine("server response: " + response);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Close();
            }
        }
    }
}
