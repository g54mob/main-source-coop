using ExitGames.Client.Photon;
using UnityEngine;

namespace Photon.Voice.Unity
{
	[AddComponentMenu("Photon Voice/Voice Logger")]
	[DisallowMultipleComponent]
	public class VoiceLogger : MonoBehaviour
	{
		public DebugLevel LogLevel = DebugLevel.WARNING;

		private void Start()
		{
		}

		public static VoiceLogger FindLogger(GameObject gameObject)
		{
			GameObject gameObject2 = gameObject;
			while (gameObject2 != null)
			{
				VoiceLogger component = gameObject2.GetComponent<VoiceLogger>();
				if (component != null && component.enabled)
				{
					return component;
				}
				gameObject2 = ((gameObject2.transform.parent == null) ? null : gameObject2.transform.parent.gameObject);
			}
			VoiceLogger voiceLogger = null;
			VoiceLogger[] array = Object.FindObjectsOfType<VoiceLogger>();
			foreach (VoiceLogger voiceLogger2 in array)
			{
				if (voiceLogger2.transform.parent == null && voiceLogger2.enabled)
				{
					if (voiceLogger != null)
					{
						UnityLogger.Log(DebugLevel.INFO, voiceLogger2, "LOGGER", voiceLogger.name, "Disabling VoiceLogger duplicates at the scene root.");
						voiceLogger2.enabled = false;
					}
					else
					{
						voiceLogger = voiceLogger2;
					}
				}
			}
			return voiceLogger;
		}

		public static VoiceLogger CreateRootLogger()
		{
			return new GameObject("VoiceLogger").AddComponent<VoiceLogger>();
		}
	}
}
