using Mimicraft.UI;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	public class PlayerEffortVoice : MonoBehaviour
	{
		private const float FilterOffAt = 21999f;

		private AudioSource source;

		private AudioLowPassFilter filter;

		private NetworkObject netObject;

		private bool IsOtherPlayer
		{
			get
			{
				if (netObject != null && netObject.IsSpawned)
				{
					return !netObject.IsOwner;
				}
				return false;
			}
		}

		public static void Play(Component player, AudioClip clip)
		{
			if (!(player == null) && !(clip == null))
			{
				PlayerEffortVoice playerEffortVoice = player.GetComponent<PlayerEffortVoice>();
				if (playerEffortVoice == null)
				{
					playerEffortVoice = player.gameObject.AddComponent<PlayerEffortVoice>();
				}
				playerEffortVoice.PlayClip(clip);
			}
		}

		private void PlayClip(AudioClip clip)
		{
			EnsureSource();
			AudioLibrary instance = AudioLibrary.Instance;
			bool isOtherPlayer = IsOtherPlayer;
			float num = ((instance != null) ? instance.otherPlayerEffortCutoff : 22000f);
			filter.cutoffFrequency = Mathf.Min(num, 22000f);
			filter.enabled = isOtherPlayer && num < 21999f;
			float num2 = ((isOtherPlayer && instance != null) ? instance.otherPlayerEffortVolume : 1f);
			source.PlayOneShot(clip, AudioLibrary.VolumeOf(clip) * num2);
		}

		private void EnsureSource()
		{
			if (!(source != null) || !(filter != null))
			{
				GameObject gameObject = new GameObject("EffortVoice");
				gameObject.transform.SetParent(base.transform, worldPositionStays: false);
				source = gameObject.AddComponent<AudioSource>();
				source.playOnAwake = false;
				SpatialAudio.Apply(source);
				filter = gameObject.AddComponent<AudioLowPassFilter>();
				filter.enabled = false;
				netObject = GetComponentInParent<NetworkObject>();
			}
		}
	}
}
