using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using UnityEngine;
namespace MiniFramework
{
    public class SocketManager : MonoSingleton<SocketManager>
    {
        public int Timeout = 5;
        public int MaxBufferSize = 1024;
        public int MaxConnections = 12;
        public Action ConnectSuccess;
        public Action ConnectFailed;
        public Action ConnectAbort;
        public Client Client;
        public Server Server;
        public Host Host;
        protected override void OnSingletonInit(){}
        public void LaunchAsClient(string ip, int port)
        {
            Client = new Client(MaxBufferSize);
            Client.Connect(ip, port);
            StartCoroutine(CheckTimeout(Timeout));
        }
        public void LaunchAsServer(int port){
            Server = new Server(MaxBufferSize,MaxConnections);
            Server.Launch(port);
        }
        public void LaunchAsHost(int port){
            Host = new Host();
            Host.Launch(port);
        }
        IEnumerator CheckTimeout(int timeout)
        {
            yield return new WaitForSeconds(timeout);
            if (Client.IsConnecting)
            {
                Client.Close();
            }
        }
        public string GetLocalIP()
        {
            string hostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(hostName);
            for (int i = 0; i < ipEntry.AddressList.Length; i++)
            {
                if (ipEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                {
                    return ipEntry.AddressList[i].ToString();
                }
            }
            return "";
        }
        public void Close()
        {
            if (Client != null)
            {
                Client.Close();
            }
            if (Server != null)
            {
                Server.Close();
            }
            if (Host != null)
            {
                Host.Close();
            }
        }
        private void OnDestroy()
        {
            Close();
        }
    }
}
