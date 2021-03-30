using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Transmitter
{
    public class Transmit
    {
        int port;
        string hostName;
        TcpClient client;
        System.IO.StreamWriter sWriter;
        public Transmit(string hostName, int port)
        {
            this.hostName = hostName;
            this.port = port;
        }
        public void Connect()
        {
            this.client = new TcpClient();
            try
            {
                client.Connect(hostName, port);
                sWriter = new StreamWriter(client.GetStream());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public void SendData(string data)
        {
            try
            {
                sWriter.WriteLine(data);
                sWriter.Flush();
            }
            catch (Exception e)
            {
                Console.WriteLine("the system cannot connect to the port");
            }
        }
        public void CloseConnection()
        {
            this.client.Close();
            sWriter.Close();
        }
    }

    public class FileReader
    {
        string fileStr = "";
        List<string> dataByLines = new List<string>();
        public List<string> LinesOfData
        {
            get
            {
                return dataByLines;
            }
        }
        public void ReadFile(string path)
        {
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            string line;
            //write the data of the file to fileStr and dataByLines
            while ((line = file.ReadLine()) != null)
            {
                this.fileStr += (line + "\n");
                dataByLines.Add(line);
            }
            file.Close();
        }
    }
}

