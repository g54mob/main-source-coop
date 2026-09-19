using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using UnityEngine;

namespace Photon.Realtime
{
	public class RegionPinger
	{
		public static int Attempts = 5;

		public static int MaxMillisecondsPerPing = 800;

		public static int PingWhenFailed = Attempts * MaxMillisecondsPerPing;

		public int CurrentAttempt;

		private Action<Region> onDoneCall;

		private PhotonPing ping;

		private List<int> rttResults;

		private Region region;

		private string regionAddress;

		public bool Done { get; private set; }

		public bool Aborted { get; internal set; }

		public RegionPinger(Region region, Action<Region> onDoneCallback)
		{
			this.region = region;
			this.region.Ping = PingWhenFailed;
			Done = false;
			onDoneCall = onDoneCallback;
		}

		private PhotonPing GetPingImplementation()
		{
			PhotonPing photonPing = null;
			if (RegionHandler.PingImplementation == null || RegionHandler.PingImplementation == typeof(PingMono))
			{
				photonPing = new PingMono();
			}
			if (photonPing == null && RegionHandler.PingImplementation != null)
			{
				photonPing = (PhotonPing)Activator.CreateInstance(RegionHandler.PingImplementation);
			}
			return photonPing;
		}

		public bool Start()
		{
			ping = GetPingImplementation();
			Done = false;
			CurrentAttempt = 0;
			rttResults = new List<int>(Attempts);
			if (Aborted)
			{
				return false;
			}
			bool flag = false;
			try
			{
				flag = ThreadPool.QueueUserWorkItem(delegate
				{
					RegionPingThreaded();
				});
			}
			catch
			{
				flag = false;
			}
			if (!flag)
			{
				Log.Error("RegionPinger.Start() failed. Could not queue region pinging. Please contact us.");
				return false;
			}
			return true;
		}

		protected internal void Abort()
		{
			Aborted = true;
			if (ping != null)
			{
				ping.Dispose();
			}
		}

		protected internal bool RegionPingThreaded()
		{
			region.Ping = PingWhenFailed;
			int num = 0;
			int num2 = 0;
			Stopwatch stopwatch = new Stopwatch();
			try
			{
				string text = region.HostAndPort;
				int num3 = text.LastIndexOf(':');
				if (num3 > 1)
				{
					text = text.Substring(0, num3);
				}
				stopwatch.Start();
				regionAddress = ResolveHost(text);
				stopwatch.Stop();
				_ = stopwatch.ElapsedMilliseconds;
				_ = 100;
			}
			catch (Exception)
			{
				Aborted = true;
			}
			CurrentAttempt = 0;
			while (CurrentAttempt < Attempts && !Aborted)
			{
				stopwatch.Reset();
				stopwatch.Start();
				try
				{
					ping.StartPing(regionAddress);
				}
				catch (Exception)
				{
					break;
				}
				while (!ping.Done() && stopwatch.ElapsedMilliseconds < MaxMillisecondsPerPing)
				{
					Thread.Sleep(1);
				}
				stopwatch.Stop();
				int num4 = (int)(ping.Successful ? stopwatch.ElapsedMilliseconds : MaxMillisecondsPerPing);
				rttResults.Add(num4);
				num += num4;
				num2++;
				region.Ping = num / num2;
				int num5 = 4;
				while (!ping.Done() && num5 > 0)
				{
					num5--;
					Thread.Sleep(100);
				}
				Thread.Sleep(10);
				CurrentAttempt++;
			}
			Done = true;
			ping.Dispose();
			if (rttResults.Count > 1 && num2 > 0)
			{
				int num6 = rttResults.Min();
				int num7 = rttResults.Max();
				int num8 = num - num7 + num6;
				region.Ping = num8 / num2;
			}
			onDoneCall(region);
			return false;
		}

		protected internal IEnumerator RegionPingCoroutine()
		{
			region.Ping = PingWhenFailed;
			int rttSum = 0;
			int replyCount = 0;
			Stopwatch sw = new Stopwatch();
			try
			{
				string text = region.HostAndPort;
				int num = text.LastIndexOf(':');
				if (num > 1)
				{
					text = text.Substring(0, num);
				}
				sw.Start();
				regionAddress = ResolveHost(text);
				sw.Stop();
				_ = sw.ElapsedMilliseconds;
				_ = 100;
			}
			catch (Exception)
			{
				Aborted = true;
			}
			for (CurrentAttempt = 0; CurrentAttempt < Attempts; CurrentAttempt++)
			{
				if (Aborted)
				{
					yield return null;
				}
				sw.Reset();
				sw.Start();
				try
				{
					ping.StartPing(regionAddress);
				}
				catch (Exception)
				{
					break;
				}
				while (!ping.Done() && sw.ElapsedMilliseconds < MaxMillisecondsPerPing)
				{
					yield return new WaitForSecondsRealtime(0.01f);
				}
				sw.Stop();
				int num2 = (int)(ping.Successful ? sw.ElapsedMilliseconds : MaxMillisecondsPerPing);
				rttResults.Add(num2);
				rttSum += num2;
				replyCount++;
				region.Ping = rttSum / replyCount;
				int i = 4;
				while (!ping.Done() && i > 0)
				{
					i--;
					yield return new WaitForSeconds(0.1f);
				}
				yield return new WaitForSeconds(0.1f);
			}
			Done = true;
			ping.Dispose();
			if (rttResults.Count > 1 && replyCount > 0)
			{
				int num3 = rttResults.Min();
				int num4 = rttResults.Max();
				int num5 = rttSum - num4 + num3;
				region.Ping = num5 / replyCount;
			}
			onDoneCall(region);
			yield return null;
		}

		public string GetResults()
		{
			return $"{region.Code}: {region.Ping} ({rttResults.ToStringFull()})";
		}

		public static string ResolveHost(string hostName)
		{
			if (hostName.StartsWith("wss://"))
			{
				hostName = hostName.Substring(6);
			}
			if (hostName.StartsWith("ws://"))
			{
				hostName = hostName.Substring(5);
			}
			string text = string.Empty;
			try
			{
				IPAddress[] hostAddresses = Dns.GetHostAddresses(hostName);
				if (hostAddresses.Length == 1)
				{
					return hostAddresses[0].ToString();
				}
				foreach (IPAddress iPAddress in hostAddresses)
				{
					if (iPAddress != null)
					{
						if (iPAddress.ToString().Contains(":"))
						{
							return iPAddress.ToString();
						}
						if (string.IsNullOrEmpty(text))
						{
							text = hostAddresses.ToString();
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return text;
		}
	}
}
