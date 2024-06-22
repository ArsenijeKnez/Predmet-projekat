using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Client
{
    class Communication
    {
        const int DEFAULT_BUFLEN = 1024;
        const int BUFFER_SIZE = 1024;
        const string SERVER_IP_ADDRESS = "127.0.0.1"; 
        const int REP1_PORT = 27016; 
        const int REP2_PORT = 27017; 

        public static Socket clientSocket;


    public static bool SendDataHandler(int ServiceID)
    {
        if (ServiceID == 0)
        {
            Console.WriteLine("Service not registered");
            return true;
        }
        Console.WriteLine("Enter message: ");
        string outgoingBuffer = Console.ReadLine();

        if (!SendData(ServiceID, outgoingBuffer))
        {
            Console.ReadLine();
            return false;
        }
        return true;
    }

    public static string CallbackHandler(int ServiceID)
    {
        if (ServiceID == 0)
        {
            Console.WriteLine("Service not registered");
            return null;
        }
        string data = "CallBack!@#$%^&&*";
        byte[] ReturnData = ReceiveData(data);
        if(ReturnData == null) 
        {
            return "No data found";
        }
        else
        {
            return System.Text.Encoding.UTF8.GetString(ReturnData);
        }
    }

    public static bool ReplicateHandler(int ServiceID)
    {
        if (ServiceID == 0)
        {
            Console.WriteLine("Service not registered");
            return false;
        }
        string data = "Replicate!@#$%^&&*";
        if(ReceiveData(data) == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static string GraphHandler(int ServiceID)
    {
        if (ServiceID == 0)
        {
            Console.WriteLine("Service not registered");
            return "null";
        }
        string data = "ReadSavedData!@#$%^&&*";
        byte[] ReturnData = ReceiveData(data);
        if (ReturnData == null)
        {
            return "No data found";
        }
        else
        {
            return System.Text.Encoding.UTF8.GetString(ReturnData);
        }
    }

        public static bool TestHandler(int ServiceID, int ServiceType)
    {
        if (ServiceID == 0)
        {
            Console.WriteLine("Service not registered");
            return true;
        }
        else if (ServiceType == 1) TestSync(ServiceID);
        else if (ServiceType == 2) TestAsync(ServiceID);
        else
        {
            Console.WriteLine("The application ran into a problem.");
            Console.ReadLine();
            return false;
        }
        return true;
    }

    public static bool CloseHandler(int ServiceID)
    {
        if (ServiceID != 0)
        {
            string data = "Close!@#$%^&&*";

            int iResult = clientSocket.Send(Encoding.UTF8.GetBytes(data));

            if (iResult <= 0)
            {
                Console.WriteLine("send failed");
                clientSocket.Close();
                return false;
            }

            clientSocket.Close();
        }
        return true;
    }

    public static bool SendData(int ServiceID, string data)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(data + "\0");
        int iResult = clientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);

        if (iResult <= 0)
        {
            Console.WriteLine("send failed");
            clientSocket.Close();
            return false;
        }
        Console.WriteLine($"Message sent to replicator {ServiceID}.");
        return true;
    }

    public static byte[] ReceiveData(string data)
    {
       byte[] buffer = Encoding.UTF8.GetBytes(data + "\0");
       int iResult = clientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);

        if (iResult <= 0)
        {
            Console.WriteLine("send failed");
            clientSocket.Close();
            return null;
        }

        byte[] recvBuffer = new byte[DEFAULT_BUFLEN];
        iResult = clientSocket.Receive(recvBuffer);

        if (iResult > 0)
        {
            Console.WriteLine($"Bytes received: {iResult}");
            Console.WriteLine($"Received data: {Encoding.UTF8.GetString(recvBuffer, 0, iResult)}");
        }
        else if (iResult == 0)
        {
            Console.WriteLine("Connection closed by the server.");
        }
        else
        {
            Console.WriteLine("recv failed");
            return null;
        }
        return recvBuffer;
    }

    public static bool RegisterService(int ServiceID, int ServiceType)
    {
        int serverPort = 0;

        if (ServiceID == 1)
        {
            Console.WriteLine("Trying to register to replicator 1.");
            serverPort = REP1_PORT;
        }
        else if (ServiceID == 2)
        {
            Console.WriteLine("Trying to register to replicator 2.");
            serverPort = REP2_PORT;
        }

        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        IPAddress ipAddress = IPAddress.Parse(SERVER_IP_ADDRESS);
        IPEndPoint remoteEP = new IPEndPoint(ipAddress, serverPort);

        try
        {
            clientSocket.Connect(remoteEP);
        }
        catch (SocketException e)
        {
            Console.WriteLine($"Unable to connect to server: {e.Message}");
            clientSocket.Close();
            return false;
        }

        string outgoingBuffer = ServiceType == 1 ? "Synchrone replication." : "Asynchrone replication.";
        byte[] buffer = Encoding.UTF8.GetBytes(outgoingBuffer + "\0");
        int iResult = clientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);


        if (iResult <= 0)
        {
            Console.WriteLine("send failed");
            clientSocket.Close();
            return false;
        }

        Console.WriteLine("Registered successfully.");
        return true;
    }

    public static byte[] TestSync(int ServiceID)
    {
        if (ServiceID == 0)
        {
            Console.WriteLine("Service not registered");
            return null;
        }
        int cnt = 0;
        string message = "Test";
        while (cnt < 50)
        {
            System.Threading.Thread.Sleep(1);
            cnt++;
            if (cnt == 50)
            {
                message = "Last";
                byte[] buffer = Encoding.UTF8.GetBytes(message + "\0");
                int iResult = clientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
                if (iResult <= 0)
                {
                    Console.WriteLine("send failed");
                    clientSocket.Close();
                    return null;
                }
            }
            else
            {
                byte[] buffer = Encoding.UTF8.GetBytes(message + "\0");
                int iResult = clientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
                if (iResult <= 0)
                {
                    Console.WriteLine("send failed");
                    clientSocket.Close();
                    return null;
                }
            }
        }
        Console.WriteLine("Last message sent");
        string data = "CallBack!@#$%^&&*";
        return ReceiveData(data);

    }

    public static byte[] TestAsync(int ServiceID)
    {
        if (ServiceID == 0)
        {
            Console.WriteLine("Service not registered");
            return null;
        }
        int cnt = 0;
        string message = "Test";
        while (cnt < 50)
        {
            System.Threading.Thread.Sleep(1);
            cnt++;
            if (cnt == 50)
            {
                message = "Last Replicated";
                byte[] buffer = Encoding.UTF8.GetBytes(message + "\0");
                int iResult = clientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
                if (iResult <= 0)
                {
                    Console.WriteLine("send failed");
                    clientSocket.Close();
                    return null;
                }
            }
            else
            {
                byte[] buffer = Encoding.UTF8.GetBytes(message + "\0");
                int iResult = clientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
                if (iResult <= 0)
                {
                    Console.WriteLine("send failed");
                    clientSocket.Close();
                    return null;
                }
            }
        }
        Console.WriteLine("Last message sent before replication");
        string data = "CallBack!@#$%^&&*";
        ReplicateHandler(ServiceID);
        Console.WriteLine("Last message after replication");
        return ReceiveData(data);
    }
    }
}

