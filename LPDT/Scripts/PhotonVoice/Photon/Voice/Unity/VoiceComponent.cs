using UnityEngine;

namespace Photon.Voice.Unity
{
	[HelpURL("https://doc.photonengine.com/en-us/voice/v2")]
	public abstract class VoiceComponent : MonoBehaviour
	{
		private VoiceComponentImpl impl = new VoiceComponentImpl();

		public ILogger Logger => impl.Logger;

		public VoiceLogger VoiceLogger => impl.VoiceLogger;

		public string Name
		{
			set
			{
				base.name = value;
				impl.Name = value;
			}
		}

		protected virtual void Awake()
		{
			impl.Awake(this);
		}
	}
}
