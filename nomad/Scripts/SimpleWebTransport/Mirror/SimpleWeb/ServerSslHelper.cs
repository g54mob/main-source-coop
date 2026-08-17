using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace Mirror.SimpleWeb
{
	internal class ServerSslHelper
	{
		private readonly SslConfig config;

		private readonly X509Certificate2 certificate;

		public ServerSslHelper(SslConfig sslConfig)
		{
			config = sslConfig;
			if (config.enabled)
			{
				certificate = new X509Certificate2(config.certPath, config.certPassword);
				Log.Info($"[SWT-ServerSslHelper]: SSL Certificate {0} loaded with expiration of {1}", certificate.Subject, certificate.GetExpirationDateString());
			}
		}

		internal bool TryCreateStream(Connection conn)
		{
			NetworkStream stream = conn.client.GetStream();
			if (config.enabled)
			{
				try
				{
					conn.stream = CreateStream(stream);
					return true;
				}
				catch (Exception ex)
				{
					Log.Error("[SWT-ServerSslHelper]: Create SSLStream Failed: {0}", ex.Message);
					return false;
				}
			}
			conn.stream = stream;
			return true;
		}

		private Stream CreateStream(NetworkStream stream)
		{
			SslStream sslStream = new SslStream(stream, leaveInnerStreamOpen: true, acceptClient);
			sslStream.AuthenticateAsServer(certificate, clientCertificateRequired: false, config.sslProtocols, checkCertificateRevocation: false);
			return sslStream;
		}

		private bool acceptClient(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}
	}
}
