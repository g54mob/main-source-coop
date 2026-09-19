using System;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Threading;

namespace Photon.Client
{
	public class SocketTcp : PhotonSocket, IDisposable
	{
		private Socket sock;

		private readonly object syncer = new object();

		[Preserve]
		public SocketTcp(PeerBase npeer)
			: base(npeer)
		{
			if (ReportDebugOfLevel(LogLevel.Info))
			{
				base.Listener.DebugReturn(LogLevel.Info, "SocketTcp, .Net, Unity.");
			}
			PollReceive = false;
		}

		~SocketTcp()
		{
			Dispose();
		}

		public void Dispose()
		{
			base.State = PhotonSocketState.Disconnecting;
			if (sock != null)
			{
				try
				{
					if (sock.Connected)
					{
						sock.Close();
					}
				}
				catch (Exception arg)
				{
					if (ReportDebugOfLevel(LogLevel.Info))
					{
						EnqueueDebugReturn(LogLevel.Info, $"Exception caught in Dispose(): {arg}");
					}
				}
			}
			sock = null;
			base.State = PhotonSocketState.Disconnected;
		}

		public override bool Connect()
		{
			lock (syncer)
			{
				if (!base.Connect())
				{
					return false;
				}
				base.State = PhotonSocketState.Connecting;
			}
			Thread thread = new Thread(DnsAndConnect);
			thread.IsBackground = true;
			thread.Start();
			return true;
		}

		public override bool Disconnect()
		{
			if (ReportDebugOfLevel(LogLevel.Info))
			{
				EnqueueDebugReturn(LogLevel.Info, "SocketTcp.Disconnect()");
			}
			lock (syncer)
			{
				base.State = PhotonSocketState.Disconnecting;
				if (sock != null)
				{
					try
					{
						sock.Close();
					}
					catch (Exception arg)
					{
						if (ReportDebugOfLevel(LogLevel.Info))
						{
							EnqueueDebugReturn(LogLevel.Info, $"Exception caught in Disconnect(): {arg}");
						}
					}
				}
				base.State = PhotonSocketState.Disconnected;
			}
			return true;
		}

		public override PhotonSocketError Send(byte[] data, int length)
		{
			try
			{
				if (sock == null || !sock.Connected)
				{
					return PhotonSocketError.Skipped;
				}
				sock.Send(data, 0, length, SocketFlags.None);
			}
			catch (Exception ex)
			{
				if (base.State != PhotonSocketState.Disconnecting && base.State != PhotonSocketState.Disconnected)
				{
					if (ReportDebugOfLevel(LogLevel.Info))
					{
						SocketException ex2 = ex as SocketException;
						string text = "";
						string text2 = "";
						text = ((ex2 != null) ? $"ErrorCode {ex2.ErrorCode} Message {ex2.Message}" : ex.ToString());
						if (sock != null)
						{
							text2 = $"Local: {sock.LocalEndPoint} Remote: {sock.RemoteEndPoint}.";
						}
						EnqueueDebugReturn(LogLevel.Info, "Caught exception sending to: " + base.ServerAddress + ". " + text2 + " Exception: " + text);
					}
					HandleException(StatusCode.SendError);
				}
				return PhotonSocketError.Exception;
			}
			return PhotonSocketError.Success;
		}

		public override PhotonSocketError Receive(out byte[] data)
		{
			data = null;
			return PhotonSocketError.NoData;
		}

		internal void DnsAndConnect()
		{
			IPAddress[] ipAddresses = GetIpAddresses(base.ServerAddress);
			if (ipAddresses == null)
			{
				return;
			}
			string text = string.Empty;
			IPAddress[] array = ipAddresses;
			foreach (IPAddress iPAddress in array)
			{
				try
				{
					sock = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
					sock.NoDelay = true;
					sock.ReceiveTimeout = peerBase.DisconnectTimeout;
					sock.SendTimeout = peerBase.DisconnectTimeout;
					sock.Connect(iPAddress, base.ServerPort);
					if (sock != null && sock.Connected)
					{
						break;
					}
				}
				catch (SecurityException ex)
				{
					if (ReportDebugOfLevel(LogLevel.Error))
					{
						text = text + ex?.ToString() + " ";
						EnqueueDebugReturn(LogLevel.Warning, $"SecurityException caught: {ex}");
					}
				}
				catch (SocketException ex2)
				{
					if (ReportDebugOfLevel(LogLevel.Warning))
					{
						text = text + ex2?.ToString() + " " + ex2.ErrorCode + "; ";
						EnqueueDebugReturn(LogLevel.Warning, $"SocketException caught: {ex2} ErrorCode: {ex2.ErrorCode}");
					}
				}
				catch (Exception ex3)
				{
					if (ReportDebugOfLevel(LogLevel.Warning))
					{
						text = text + ex3?.ToString() + "; ";
						EnqueueDebugReturn(LogLevel.Warning, $"Exception caught: {ex3}");
					}
				}
			}
			if (sock == null || !sock.Connected)
			{
				if (ReportDebugOfLevel(LogLevel.Error))
				{
					EnqueueDebugReturn(LogLevel.Error, $"Failed to connect to server {base.ServerAddress} after testing each known IP ({ipAddresses.Length}). Error(s): {text}");
				}
				HandleException(StatusCode.ExceptionOnConnect);
			}
			else
			{
				base.AddressResolvedAsIpv6 = sock.AddressFamily == AddressFamily.InterNetworkV6;
				base.ServerIpAddress = sock.RemoteEndPoint.ToString();
				base.State = PhotonSocketState.Connected;
				Thread thread = new Thread(ReceiveLoop);
				thread.IsBackground = true;
				thread.Start();
			}
		}

		public void ReceiveLoop()
		{
			StreamBuffer streamBuffer = new StreamBuffer(base.MTU);
			byte[] array = new byte[9];
			while (base.State == PhotonSocketState.Connected)
			{
				streamBuffer.SetLength(0L);
				try
				{
					int num = 0;
					int num2 = 0;
					while (num < 9)
					{
						try
						{
							num2 = sock.Receive(array, num, 9 - num, SocketFlags.None);
						}
						catch (SocketException ex)
						{
							if (base.State != PhotonSocketState.Disconnecting && base.State > PhotonSocketState.Disconnected && ex.SocketErrorCode == SocketError.WouldBlock)
							{
								if (ReportDebugOfLevel(LogLevel.Error))
								{
									EnqueueDebugReturn(LogLevel.Error, "ReceiveLoop() got a WouldBlock exception. This is non-fatal. Going to continue.");
								}
								continue;
							}
							throw;
						}
						num += num2;
						if (num2 == 0)
						{
							throw new SocketException(10054);
						}
					}
					if (array[0] == 240)
					{
						HandleReceivedDatagram(array, array.Length, willBeReused: true);
						continue;
					}
					int num3 = (array[1] << 24) | (array[2] << 16) | (array[3] << 8) | array[4];
					if (ReportDebugOfLevel(LogLevel.Debug))
					{
						EnqueueDebugReturn(LogLevel.Debug, $"TCP < {num3}");
					}
					streamBuffer.SetCapacityMinimum(num3 - 7);
					streamBuffer.Write(array, 7, num - 7);
					num = 0;
					num3 -= 9;
					while (num < num3)
					{
						try
						{
							num2 = sock.Receive(streamBuffer.GetBuffer(), streamBuffer.Position, num3 - num, SocketFlags.None);
						}
						catch (SocketException ex2)
						{
							if (base.State != PhotonSocketState.Disconnecting && base.State > PhotonSocketState.Disconnected && ex2.SocketErrorCode == SocketError.WouldBlock)
							{
								if (ReportDebugOfLevel(LogLevel.Error))
								{
									EnqueueDebugReturn(LogLevel.Error, "ReceiveLoop() got a WouldBlock exception. This is non-fatal. Going to continue.");
								}
								continue;
							}
							throw;
						}
						streamBuffer.Position += num2;
						num += num2;
						if (num2 == 0)
						{
							throw new SocketException(10054);
						}
					}
					HandleReceivedDatagram(streamBuffer.ToArray(), streamBuffer.Length, willBeReused: false);
					if (ReportDebugOfLevel(LogLevel.Debug))
					{
						EnqueueDebugReturn(LogLevel.Debug, string.Format("TCP < {0}{1}", streamBuffer.Length, (streamBuffer.Length == num3 + 2) ? " OK" : " BAD"));
					}
				}
				catch (SocketException ex3)
				{
					if (base.State != PhotonSocketState.Disconnecting && base.State != PhotonSocketState.Disconnected)
					{
						if (ReportDebugOfLevel(LogLevel.Error))
						{
							EnqueueDebugReturn(LogLevel.Error, $"Receiving failed. SocketException: {ex3.SocketErrorCode}");
						}
						if (ex3.SocketErrorCode == SocketError.ConnectionReset || ex3.SocketErrorCode == SocketError.ConnectionAborted)
						{
							HandleException(StatusCode.DisconnectByServerTimeout);
						}
						else
						{
							HandleException(StatusCode.ExceptionOnReceive);
						}
					}
				}
				catch (Exception arg)
				{
					if (base.State != PhotonSocketState.Disconnecting && base.State != PhotonSocketState.Disconnected)
					{
						if (ReportDebugOfLevel(LogLevel.Error))
						{
							EnqueueDebugReturn(LogLevel.Error, $"Receive issue. State: {base.State}. Server: '{base.ServerAddress}' Exception: {arg}");
						}
						HandleException(StatusCode.ExceptionOnReceive);
					}
				}
			}
			lock (syncer)
			{
				if (base.State != PhotonSocketState.Disconnecting && base.State != PhotonSocketState.Disconnected)
				{
					Disconnect();
				}
			}
		}
	}
}
