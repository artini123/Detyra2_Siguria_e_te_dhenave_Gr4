﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Serveri
{
    public class UDP
    {
        private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private const int bufferSize = 8 * 1024;
        private State state = new State();
        public EndPoint epFrom = new IPEndPoint(IPAddress.Any, 0);
        private AsyncCallback recv = null;
        public static string message;
        public class State
        {
            public byte[] buffer = new byte[bufferSize];
        }

        public void Server(string address, int port)
        {
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
            message = Receive();
        }

        public void Client(string address, int port)
        {
            socket.Connect(IPAddress.Parse(address), port);

            //Receive();
        }

        public void Send(string text)
        {
            byte[] data = Encoding.ASCII.GetBytes(text);
            socket.BeginSend(data, 0, data.Length, SocketFlags.None, (ar) =>
            {
                State so = (State)ar.AsyncState;
                int bytes = socket.EndSend(ar);
                Console.WriteLine("SEND: {0}, {1}", bytes, text);
            }, state);
        }
        string rs;
        public string Receive()
        {
            socket.BeginReceiveFrom(state.buffer, 0, bufferSize, SocketFlags.None, ref epFrom, recv = (ar) =>
            {
                State so = (State)ar.AsyncState;
                int bytes = socket.EndReceiveFrom(ar, ref epFrom);
                socket.BeginReceiveFrom(so.buffer, 0, bufferSize, SocketFlags.None, ref epFrom, recv, so);
                rs += Encoding.ASCII.GetString(so.buffer, 0, bytes);
            }, state);
            return rs;
        }
        public void ServerSend(string text)
        {
            byte[] data = Encoding.ASCII.GetBytes(text);
            socket.SendTo(data, data.Length, SocketFlags.None, epFrom);

        }
    }
}
