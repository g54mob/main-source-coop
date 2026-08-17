using EvilCore.Networking;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Project.Networking.EOS
{
	public class VoiceChatInfoPanel : MonoBehaviour
	{
		[SerializeField]
		private Image voiceChatInfoImage;

		[SerializeField]
		private Sprite muteSprite;

		[SerializeField]
		private Sprite unmuteSprite;

		[Inject]
		private IVoiceChatManager _voiceChatManager;

		[Inject]
		private INetworkManager _networkManager;

		private void OnEnable()
		{
			if (_networkManager != null && _networkManager.IsSingleplayerSession)
			{
				base.gameObject.SetActive(value: false);
				return;
			}
			_voiceChatManager.OnMuteStateChanged += SetInfoImage;
			SetInfoImage(_voiceChatManager.IsMuted);
		}

		private void OnDisable()
		{
			_voiceChatManager.OnMuteStateChanged -= SetInfoImage;
		}

		private void SetInfoImage(bool isMuted)
		{
			voiceChatInfoImage.sprite = (isMuted ? muteSprite : unmuteSprite);
		}
	}
}
