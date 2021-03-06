﻿using System;
using System.Net;
using System.Net.Sockets; 
using System.Threading.Tasks;
using System.IO; 
namespace FileWebShare
{
	public abstract class Server
	{
		//Thread 
		private Task _taskListen = null;
		private Task _taskAccept = null; 
		
		private TcpListener _tcpListener; 

		public ServerSetting ServerSetting { get; protected set; }

		public bool Started { get; private set; }

		public Server(ServerSetting settings)
		{
			ServerSetting = settings;
			Started = false; 
		}

		public bool Start()
		{
			if(ServerSetting.IPAddress == null)
				throw new Exception("IPAdress가 NULL 값 입니다. ");
			if ( ServerSetting.Port == 0)
				throw new Exception("포트가 지정되어 있지 않습니다.");

			Started = true;
			_taskListen = Task.Run(() => ListenThread(ServerSetting.IPAddress, ServerSetting.Port));
			return true;
		}

		private async Task ListenThread(IPAddress ipAddress, int port) 
		{
			_tcpListener = new TcpListener(ipAddress, port);
			_tcpListener.Start(); 
			while (Started) 
			{ 
				try
				{ 
					TcpClient tcpClient = await _tcpListener.AcceptTcpClientAsync(); 
					_taskAccept = Task.Run(() => AcceptHandle(tcpClient));
				}
				catch (SocketException e)
				{
					Console.WriteLine(e.ToString());
				}
			}
		}

		private void AcceptHandle(TcpClient tcpClient) 
		{
			if (tcpClient.Connected) 
			{
				HttpHandler httpHandler =  new HttpHandler(tcpClient, ServerSetting);

				httpHandler.RequestProcess();
				 
				tcpClient.Close(); 
			}
		}
	}
}
