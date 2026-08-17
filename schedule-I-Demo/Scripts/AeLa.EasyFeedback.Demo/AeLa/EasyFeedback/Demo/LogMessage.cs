using UnityEngine;

namespace AeLa.EasyFeedback.Demo
{
	public class LogMessage : MonoBehaviour
	{
		public void LogMessages()
		{
			Debug.Log("Test Log");
			Debug.LogWarning("Test Warning");
			Debug.LogError("Test Error");
		}
	}
}
