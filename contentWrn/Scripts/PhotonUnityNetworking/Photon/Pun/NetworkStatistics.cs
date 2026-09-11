using System.Collections.Generic;
using UnityEngine;

namespace Photon.Pun
{
	public static class NetworkStatistics
	{
		private static Dictionary<string, int> m_RPCsCalledDictionary = new Dictionary<string, int>();

		private static Dictionary<string, int> m_RPCsReceivedDictionary = new Dictionary<string, int>();

		private static Dictionary<string, int> m_EventsCalledDictionary = new Dictionary<string, int>();

		private static Dictionary<string, int> m_EventsReceivedDictionary = new Dictionary<string, int>();

		public static void ResetStats()
		{
			m_RPCsCalledDictionary = new Dictionary<string, int>();
			m_RPCsReceivedDictionary = new Dictionary<string, int>();
			m_EventsCalledDictionary = new Dictionary<string, int>();
			m_EventsReceivedDictionary = new Dictionary<string, int>();
		}

		public static void AddRPC_Called(string rpcName)
		{
			if (!m_RPCsCalledDictionary.ContainsKey(rpcName))
			{
				m_RPCsCalledDictionary.Add(rpcName, 0);
			}
			m_RPCsCalledDictionary[rpcName]++;
		}

		public static void AddRPC_Received(string rpcName)
		{
			if (!m_RPCsReceivedDictionary.ContainsKey(rpcName))
			{
				m_RPCsReceivedDictionary.Add(rpcName, 0);
			}
			m_RPCsReceivedDictionary[rpcName]++;
		}

		public static void AddEvent_Called(string eventName)
		{
			if (!m_EventsCalledDictionary.ContainsKey(eventName))
			{
				m_EventsCalledDictionary.Add(eventName, 0);
			}
			m_EventsCalledDictionary[eventName]++;
		}

		public static void AddEvent_Received(string eventName)
		{
			if (!m_EventsReceivedDictionary.ContainsKey(eventName))
			{
				m_EventsReceivedDictionary.Add(eventName, 0);
			}
			m_EventsReceivedDictionary[eventName]++;
		}

		public static void PrintStatistcs()
		{
			string text = "[MATCH REPORT]\n";
			text += "RPCs Called\n";
			foreach (KeyValuePair<string, int> item in m_RPCsCalledDictionary)
			{
				text = text + item.Key + " Called: " + item.Value + " Times\n";
			}
			text += "Events Called\n";
			foreach (KeyValuePair<string, int> item2 in m_EventsCalledDictionary)
			{
				text = text + item2.Key + " Called: " + item2.Value + " Times\n";
			}
			text += "RPCs Received\n";
			foreach (KeyValuePair<string, int> item3 in m_RPCsReceivedDictionary)
			{
				text = text + item3.Key + " Received: " + item3.Value + " Times\n";
			}
			text += "[END OF MATCH REPORT]";
			Debug.Log(text);
		}
	}
}
