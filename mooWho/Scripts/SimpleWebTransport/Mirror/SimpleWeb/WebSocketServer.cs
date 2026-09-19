using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

namespace Mirror.SimpleWeb
{
	public class WebSocketServer
	{
		public readonly ConcurrentQueue<Message> receiveQueue = new ConcurrentQueue<Message>();

		private readonly TcpConfig tcpConfig;

		private readonly int maxMessageSize;

		private TcpListener listener;

		private Thread acceptThread;

		private bool serverStopped;

		private readonly ServerHandshake handShake;

		private readonly ServerSslHelper sslHelper;

		private readonly BufferPool bufferPool;

		private readonly ConcurrentDictionary<int, Connection> connections = new ConcurrentDictionary<int, Connection>();

		private int _idCounter;

		public WebSocketServer(TcpConfig tcpConfig, int maxMessageSize, int handshakeMaxSize, SslConfig sslConfig, BufferPool bufferPool)
		{
			this.tcpConfig = tcpConfig;
			this.maxMessageSize = maxMessageSize;
			sslHelper = new ServerSslHelper(sslConfig);
			this.bufferPool = bufferPool;
			handShake = new ServerHandshake(this.bufferPool, handshakeMaxSize);
		}

		public void Listen(int port)
		{
			listener = TcpListener.Create(port);
			listener.Start();
			Log.Verbose("[SWT-WebSocketServer]: Server Started on {0}", port);
			acceptThread = new Thread(acceptLoop);
			acceptThread.IsBackground = true;
			acceptThread.Start();
		}

		public void Stop()
		{
			serverStopped = true;
			acceptThread?.Interrupt();
			listener?.Stop();
			acceptThread = null;
			Log.Verbose("[SWT-WebSocketServer]: Server stopped...closing all connections.");
			Connection[] array = connections.Values.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Dispose();
			}
			connections.Clear();
		}

		private void acceptLoop()
		{
			try
			{
				try
				{
					while (true)
					{
						TcpClient client = listener.AcceptTcpClient();
						tcpConfig.ApplyTo(client);
						Connection conn = new Connection(client, AfterConnectionDisposed);
						Log.Verbose("[SWT-WebSocketServer]: A client connected from {0}", conn);
						Thread thread = new Thread((ThreadStart)delegate
						{
							HandshakeAndReceiveLoop(conn);
						});
						conn.receiveThread = thread;
						thread.IsBackground = true;
						thread.Start();
					}
				}
				catch (SocketException)
				{
					Utils.CheckForInterupt();
					throw;
				}
			}
			catch (ThreadInterruptedException e)
			{
				Log.InfoException(e);
			}
			catch (ThreadAbortException)
			{
				Log.Error("[SWT-WebSocketServer]: Thread Abort Exception");
			}
			catch (Exception e2)
			{
				Log.Exception(e2);
			}
		}

		private void HandshakeAndReceiveLoop(Connection conn)
		{
			try
			{
				if (!sslHelper.TryCreateStream(conn))
				{
					Log.Warn("[SWT-WebSocketServer]: Failed to create SSL Stream {0}", conn);
					conn.Dispose();
				}
				else if (handShake.TryHandshake(conn))
				{
					Log.Verbose("[SWT-WebSocketServer]: Sent Handshake {0}, false", conn);
					if (serverStopped)
					{
						Log.Warn("[SWT-WebSocketServer]: Server stopped after successful handshake");
						return;
					}
					conn.connId = Interlocked.Increment(ref _idCounter);
					connections.TryAdd(conn.connId, conn);
					receiveQueue.Enqueue(new Message(conn.connId, EventType.Connected));
					Thread thread = new Thread((ThreadStart)delegate
					{
						SendLoop.Loop(new SendLoop.Config(conn, 4 + maxMessageSize, setMask: false));
					});
					conn.sendThread = thread;
					thread.IsBackground = true;
					thread.Name = $"SendThread {conn.connId}";
					thread.Start();
					ReceiveLoop.Loop(new ReceiveLoop.Config(conn, maxMessageSize, expectMask: true, receiveQueue, bufferPool));
				}
				else
				{
					Log.Warn("[SWT-WebSocketServer]: Handshake Failed {0}", conn);
					conn.Dispose();
				}
			}
			catch (ThreadInterruptedException e)
			{
				Log.InfoException(e);
			}
			catch (ThreadAbortException)
			{
				Log.Error("[SWT-WebSocketServer]: Thread Abort Exception");
			}
			catch (Exception e2)
			{
				Log.Exception(e2);
			}
			finally
			{
				conn.Dispose();
			}
		}

		private void AfterConnectionDisposed(Connection conn)
		{
			if (conn.connId != -1)
			{
				receiveQueue.Enqueue(new Message(conn.connId, EventType.Disconnected));
				connections.TryRemove(conn.connId, out var _);
			}
		}

		public void Send(int id, ArrayBuffer buffer)
		{
			if (connections.TryGetValue(id, out var value))
			{
				value.sendQueue.Enqueue(buffer);
				value.sendPending.Set();
			}
			else
			{
				Log.Warn("[SWT-WebSocketServer]: Cannot send message to {0} because connection was not found in dictionary. Maybe it disconnected.", id);
			}
		}

		public bool CloseConnection(int id)
		{
			if (connections.TryGetValue(id, out var value))
			{
				Log.Info($"[SWT-WebSocketServer]: Disconnecting connection {0}", id);
				value.Dispose();
				return true;
			}
			Log.Warn("[SWT-WebSocketServer]: Failed to kick {0} because id not found.", id);
			return false;
		}

		public string GetClientAddress(int id)
		{
			if (!connections.TryGetValue(id, out var value))
			{
				Log.Warn("[SWT-WebSocketServer]: Cannot get address of connection {0} because connection was not found in dictionary.", id);
				return null;
			}
			return value.remoteAddress;
		}

		public Request GetClientRequest(int id)
		{
			if (!connections.TryGetValue(id, out var value))
			{
				Log.Warn("[SWT-WebSocketServer]: Cannot get request of connection {0} because connection was not found in dictionary.", id);
				return null;
			}
			return value.request;
		}
	}
}
