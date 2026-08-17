using System;
using FishNet.Managing.Statistic;
using UnityEngine;

namespace FishNet.Component.Utility
{
	[AddComponentMenu("FishNet/Component/BandwidthDisplay")]
	public class BandwidthDisplay : MonoBehaviour
	{
		private enum Corner
		{
			TopLeft = 0,
			TopRight = 1,
			BottomLeft = 2,
			BottomRight = 3
		}

		[Tooltip("Color for text.")]
		[SerializeField]
		private Color _color = Color.white;

		[Tooltip("Which corner to display network statistics in.")]
		[SerializeField]
		private Corner _placement = Corner.TopRight;

		[Tooltip("True to show outgoing data bytes.")]
		[SerializeField]
		private bool _showOutgoing = true;

		[Tooltip("True to show incoming data bytes.")]
		[SerializeField]
		private bool _showIncoming = true;

		private GUIStyle _style = new GUIStyle();

		private string _clientText;

		private string _serverText;

		private NetworkTraficStatistics _networkTrafficStatistics;

		public void SetShowOutgoing(bool value)
		{
			_showOutgoing = value;
		}

		public void SetShowIncoming(bool value)
		{
			_showIncoming = value;
		}

		private void Start()
		{
			_networkTrafficStatistics = InstanceFinder.NetworkManager.StatisticsManager.NetworkTraffic;
			_networkTrafficStatistics.OnClientNetworkTraffic += NetworkTraffic_OnClientNetworkTraffic;
			_networkTrafficStatistics.OnServerNetworkTraffic += NetworkTraffic_OnServerNetworkTraffic;
			if (!_networkTrafficStatistics.UpdateClient && !_networkTrafficStatistics.UpdateServer)
			{
				Debug.LogWarning("StatisticsManager.NetworkTraffic is not updating for client nor server. To see results ensure your NetworkManager has a StatisticsManager component added with the NetworkTraffic values configured.");
			}
		}

		private void OnDestroy()
		{
			if (_networkTrafficStatistics != null)
			{
				_networkTrafficStatistics.OnClientNetworkTraffic -= NetworkTraffic_OnClientNetworkTraffic;
				_networkTrafficStatistics.OnServerNetworkTraffic -= NetworkTraffic_OnServerNetworkTraffic;
			}
		}

		private void NetworkTraffic_OnClientNetworkTraffic(NetworkTrafficArgs obj)
		{
			string newLine = Environment.NewLine;
			string text = string.Empty;
			if (_showIncoming)
			{
				text = text + "Client In: " + NetworkTraficStatistics.FormatBytesToLargest(obj.FromServerBytes) + "/s" + newLine;
			}
			if (_showOutgoing)
			{
				text = text + "Client Out: " + NetworkTraficStatistics.FormatBytesToLargest(obj.ToServerBytes) + "/s" + newLine;
			}
			_clientText = text;
		}

		private void NetworkTraffic_OnServerNetworkTraffic(NetworkTrafficArgs obj)
		{
			string newLine = Environment.NewLine;
			string text = string.Empty;
			if (_showIncoming)
			{
				text = text + "Server In: " + NetworkTraficStatistics.FormatBytesToLargest(obj.ToServerBytes) + "/s" + newLine;
			}
			if (_showOutgoing)
			{
				text = text + "Server Out: " + NetworkTraficStatistics.FormatBytesToLargest(obj.FromServerBytes) + "/s" + newLine;
			}
			_serverText = text;
		}

		private void OnGUI()
		{
			_style.normal.textColor = _color;
			_style.fontSize = 15;
			float num = 100f;
			float num2 = 0f;
			if (_showIncoming)
			{
				num2 += 15f;
			}
			if (_showOutgoing)
			{
				num2 += 15f;
			}
			bool isClient = InstanceFinder.IsClient;
			bool isServer = InstanceFinder.IsServer;
			if (!isClient)
			{
				_clientText = string.Empty;
			}
			if (!isServer)
			{
				_serverText = string.Empty;
			}
			if (isServer && isClient)
			{
				num2 *= 2f;
			}
			float num3 = 10f;
			float x;
			float y;
			if (_placement == Corner.TopLeft)
			{
				x = 10f;
				y = 10f;
				_style.alignment = TextAnchor.UpperLeft;
			}
			else if (_placement == Corner.TopRight)
			{
				x = (float)Screen.width - num - num3;
				y = 10f;
				_style.alignment = TextAnchor.UpperRight;
			}
			else if (_placement == Corner.BottomLeft)
			{
				x = 10f;
				y = (float)Screen.height - num2 - num3;
				_style.alignment = TextAnchor.LowerLeft;
			}
			else
			{
				x = (float)Screen.width - num - num3;
				y = (float)Screen.height - num2 - num3;
				_style.alignment = TextAnchor.LowerRight;
			}
			GUI.Label(new Rect(x, y, num, num2), _clientText + _serverText, _style);
		}
	}
}
