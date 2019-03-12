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
        public Action<string> ClientConnected;
        public Action<string> ClientAborted;
        private Client client;
        private Server server;
        private Host host;
        public bool IsConnected{get{return client.IsConnected;}}
        protected override void OnSingletonInit(){}
        public void LaunchAsClient(string ip, int port)
        {
            client = new Client(MaxBufferSize);
            client.ConnectSuccess = ConnectSuccess;
            client.ConnectFailed = ConnectFailed;
            client.ConnectAbort = ConnectAbort;
            client.Connect(ip, port);
            StartCoroutine(CheckTimeout(Timeout));
        }
        public void LaunchAsServer(int port){
            server = new Server(MaxBufferSize,MaxConnections);
            server.ClientConnected = ClientConnected;
            server.ClientAborted = ClientAborted;
            server.Launch(port);
        }
        public void LaunchAsHost(int port){
            host = new Host();
            host.Launch(port);
        }
        public void SendToServer(PackHead head,byte[] bodyData){
            client.Send(head,bodyData);
        }
        public void SendToClient(PackHead head,byte[] bodyData){
            server.Send(head,bodyData);
        }
        public void SendToRemoteIP(PackHead head,byte[] bodyData,string remoteIP,int remotePort){
            host.Send(head,bodyData,remoteIP,remotePort);
        }
        IEnumerator CheckTimeout(int timeout)
        {
            yield return new WaitForSeconds(timeout);
            if (client.IsConnecting)
            {
                Debug.Log("请求超时");
                Close();
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
            if (client != null)
            {
                client.Close();
            }
            if (server != null)
            {
                server.Close();
            }
            if (host != null)
            {
                host.Close();
            }
        }
        private void OnDestroy()
        {
            Close();
        }
    }
}
