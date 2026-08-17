using System.Collections.Generic;
using System.Linq;
using IO.Swagger.Model;
using UnityEngine.UIElements;

namespace Edgegap
{
	public static class EdgegapServerDataManager
	{
		internal static Status _serverData;

		private static ApiEnvironment _apiEnvironment;

		private static readonly StyleSheet _serverDataStylesheet;

		private static readonly List<VisualElement> _serverDataContainers;

		public static Status GetServerStatus()
		{
			return _serverData;
		}

		static EdgegapServerDataManager()
		{
			_serverDataContainers = new List<VisualElement>();
		}

		public static void RegisterServerDataContainer(VisualElement serverDataContainer)
		{
			_serverDataContainers.Add(serverDataContainer);
		}

		public static void DeregisterServerDataContainer(VisualElement serverDataContainer)
		{
			_serverDataContainers.Remove(serverDataContainer);
		}

		public static void SetServerData(Status serverData, ApiEnvironment apiEnvironment)
		{
			_serverData = serverData;
			_apiEnvironment = apiEnvironment;
			RefreshServerDataContainers();
		}

		private static VisualElement GetStatusSection()
		{
			ServerStatus serverStatus = _serverData.GetServerStatus();
			string dashboardUrl = _apiEnvironment.GetDashboardUrl();
			string requestId = _serverData.RequestId;
			string text = "";
			if (!string.IsNullOrEmpty(requestId) && !string.IsNullOrEmpty(dashboardUrl))
			{
				text = dashboardUrl + "/arbitrium/deployment/read/" + requestId + "/";
			}
			VisualElement visualElement = new VisualElement();
			visualElement.AddToClassList("container");
			visualElement.Add(EdgegapServerDataManagerUtils.GetHeader("Server Status"));
			VisualElement visualElement2 = new VisualElement();
			visualElement2.AddToClassList("row__status");
			Label label = new Label(serverStatus.GetLabelText());
			label.AddToClassList(serverStatus.GetStatusBgClass());
			label.AddToClassList("label__status");
			visualElement2.Add(label);
			if (!string.IsNullOrEmpty(text))
			{
				visualElement2.Add(EdgegapServerDataManagerUtils.GetLinkButton("See in the dashboard", text));
			}
			else
			{
				visualElement2.Add(new Label("Could not resolve link to this deployment"));
			}
			visualElement.Add(visualElement2);
			return visualElement;
		}

		private static VisualElement GetDnsSection()
		{
			string fqdn = _serverData.Fqdn;
			VisualElement visualElement = new VisualElement();
			visualElement.AddToClassList("container");
			visualElement.Add(EdgegapServerDataManagerUtils.GetHeader("Server DNS"));
			VisualElement visualElement2 = new VisualElement();
			visualElement2.AddToClassList("row__dns");
			visualElement2.AddToClassList("focusable");
			visualElement2.Add(new Label(fqdn));
			visualElement2.Add(EdgegapServerDataManagerUtils.GetCopyButton("Copy", fqdn));
			visualElement.Add(visualElement2);
			return visualElement;
		}

		private static VisualElement GetPortsSection()
		{
			List<PortMapping> list = _serverData.Ports.Values.ToList();
			VisualElement visualElement = new VisualElement();
			visualElement.AddToClassList("container");
			visualElement.Add(EdgegapServerDataManagerUtils.GetHeader("Server PortsDict"));
			visualElement.Add(EdgegapServerDataManagerUtils.GetHeaderRow());
			VisualElement visualElement2 = new VisualElement();
			if (list.Count > 0)
			{
				foreach (PortMapping item in list)
				{
					visualElement2.Add(EdgegapServerDataManagerUtils.GetRowFromPortResponse(item));
				}
			}
			else
			{
				visualElement2.Add(new Label("No port configured for this app version."));
			}
			visualElement.Add(visualElement2);
			return visualElement;
		}

		public static VisualElement GetServerDataVisualTree()
		{
			VisualElement visualElement = new VisualElement();
			visualElement.styleSheets.Add(_serverDataStylesheet);
			bool flag = _serverData != null;
			bool flag2 = flag && _serverData.GetServerStatus().IsOneOf(ServerStatus.Ready, ServerStatus.Error);
			if (flag)
			{
				visualElement.Add(GetStatusSection());
				if (flag2)
				{
					visualElement.Add(GetDnsSection());
					visualElement.Add(GetPortsSection());
				}
				else
				{
					visualElement.Add(EdgegapServerDataManagerUtils.GetInfoText("Additional information will be displayed when the server is ready."));
				}
			}
			else
			{
				visualElement.Add(EdgegapServerDataManagerUtils.GetInfoText("Server data will be displayed here when a server is running."));
			}
			return visualElement;
		}

		private static void RefreshServerDataContainers()
		{
			foreach (VisualElement serverDataContainer in _serverDataContainers)
			{
				serverDataContainer.Clear();
				serverDataContainer.Add(GetServerDataVisualTree());
			}
		}
	}
}
