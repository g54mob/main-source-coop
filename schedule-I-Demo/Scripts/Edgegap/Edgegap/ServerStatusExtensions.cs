using System;
using System.Linq;
using IO.Swagger.Model;
using UnityEngine;

namespace Edgegap
{
	public static class ServerStatusExtensions
	{
		private static string GetServerStatusLabel(this Status serverStatusResponse)
		{
			return char.ToUpper(serverStatusResponse.CurrentStatus[7]) + serverStatusResponse.CurrentStatus.Substring(8).ToLower();
		}

		public static ServerStatus GetServerStatus(this Status serverStatusResponse)
		{
			try
			{
				return (ServerStatus)Enum.Parse(typeof(ServerStatus), serverStatusResponse.GetServerStatusLabel());
			}
			catch (Exception)
			{
				Debug.LogError("Got unexpected server status: " + serverStatusResponse.CurrentStatus + ". Considering the deployment to be terminated.");
				return ServerStatus.Terminated;
			}
		}

		public static string GetStatusBgClass(this ServerStatus serverStatus)
		{
			switch (serverStatus)
			{
			case ServerStatus.NA:
			case ServerStatus.Terminated:
				return "bg--secondary";
			case ServerStatus.Ready:
				return "bg--success";
			case ServerStatus.Error:
				return "bg--danger";
			default:
				return "bg--warning";
			}
		}

		public static string GetLabelText(this ServerStatus serverStatus)
		{
			if (serverStatus == ServerStatus.NA)
			{
				return "N/A";
			}
			return Enum.GetName(typeof(ServerStatus), serverStatus);
		}

		public static bool IsOneOf(this ServerStatus serverStatus, params ServerStatus[] serverStatusOptions)
		{
			return serverStatusOptions.Contains(serverStatus);
		}
	}
}
