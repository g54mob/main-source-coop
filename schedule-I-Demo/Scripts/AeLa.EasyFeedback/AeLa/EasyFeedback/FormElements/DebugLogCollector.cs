using System.Text;
using UnityEngine;

namespace AeLa.EasyFeedback.FormElements
{
	internal class DebugLogCollector : FormElement
	{
		private StringBuilder log;

		public static string[] IgnoreList = new string[7] { "Failed to create agent because there is no valid NavMesh", "Failed to create agent because it is not close enough to the NavMesh", "Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.", "Coroutine continue failure", "BoxCollider does not support negative scale or size.", "Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.", "Cannot complete action because you are not the owner of this object" };

		public override void Awake()
		{
			base.Awake();
			log = new StringBuilder();
			Application.logMessageReceived += HandleLog;
		}

		protected override void FormClosed()
		{
		}

		protected override void FormOpened()
		{
		}

		protected override void FormSubmitted()
		{
			byte[] bytes = Encoding.ASCII.GetBytes(log.ToString());
			Form.CurrentReport.AttachFile("log.txt", bytes);
		}

		private void HandleLog(string logString, string stackTrace, LogType logType)
		{
			string[] ignoreList = IgnoreList;
			foreach (string value in ignoreList)
			{
				if (logString.Contains(value))
				{
					return;
				}
			}
			if (logType != LogType.Exception)
			{
				log.AppendFormat("{0}: {1}", logType.ToString(), logString);
			}
			else
			{
				log.AppendLine(logString);
			}
			log.AppendLine(stackTrace);
		}
	}
}
