using System;
using System.Net;
using System.Net.Sockets;
using System.IO.Ports;
using System.Runtime.Remoting.Messaging;

namespace TCPClient
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			ClientSendMsg ("Hello World!!");
			ClientSendMsg ("This is a TCP/IP Server and Client DEMO! We are testing it. It uses blocked communication method!");
		}

		private static void ClientSendMsg(String message)
		{
			String serverIP = "127.0.0.1";
			Int32 port = 14928;
			try {
				TcpClient client = new TcpClient (serverIP, port);
				Console.WriteLine ("Connected to " + client.Client.RemoteEndPoint.ToString ());
				Console.WriteLine (client.Client.LocalEndPoint.ToString ());
				Byte[] msg = System.Text.Encoding.ASCII.GetBytes (message);
				NetworkStream ns = client.GetStream ();
				ns.Write (msg, 0, msg.Length);
				Console.WriteLine ("Send: " + message);
				
				Byte[] response_buffer = new byte[20];
				String responseStr = String.Empty;
				int counter;
				counter = ns.Read (response_buffer, 0, response_buffer.Length);
				do {
					responseStr += System.Text.Encoding.ASCII.GetString (response_buffer, 0, counter);
					counter = ns.Read (response_buffer, 0, response_buffer.Length);
				} while (ns.DataAvailable);
				if (counter != 0) {
					responseStr += System.Text.Encoding.ASCII.GetString (response_buffer, 0, counter);
				}
				Console.WriteLine ("Received: " + responseStr);
				ns.Close ();
				client.Close ();
			} catch (SocketException ex) {
				if (ex.SocketErrorCode == SocketError.ConnectionRefused) {
					Console.WriteLine ("Connection Refused!\nPlease check if the server has been startup or the port was blocked by firewall!");
				}
			}
		}
	}
}
