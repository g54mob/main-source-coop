using System.Collections.Generic;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.ScreenShakeModule.Scripts;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Features.QuotaModule.Scripts
{
	public class QuotaSoundController : MonoBehaviour
	{
		[SerializeField]
		private EventReference _itemAddedSound;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		[SerializeField]
		private float _soundCooldown = 0.2f;

		[SerializeField]
		private CinemachineImpulseSource _cinemachineImpulseSource;

		[SerializeField]
		private ScreenShakeData _screenShakeData;

		private float _lastPlayTime;

		private QuotaContainerModel _quotaContainerModel;

		private IScreenShakeService _screenShakeService;

		private IAudioService _audioService;

		private List<QuotaContainerItemData> _freeContainerItems = new List<QuotaContainerItemData>();

		[Inject]
		private void InjectDependencies(QuotaContainerModel quotaContainerModel, IScreenShakeService screenShakeService, IAudioService audioService)
		{
			_quotaContainerModel = quotaContainerModel;
			_screenShakeService = screenShakeService;
			_audioService = audioService;
		}

		public void Start()
		{
			_quotaContainerModel.OnFreeContainerItemsUpdated += PlaySoundOnItemAdded;
		}

		public void OnDestroy()
		{
			_quotaContainerModel.OnFreeContainerItemsUpdated -= PlaySoundOnItemAdded;
		}

		private void PlaySoundOnItemAdded()
		{
			List<QuotaContainerItemData> currentItems = _quotaContainerModel.FreeContainerItems;
			bool flag = false;
			foreach (QuotaContainerItemData item in currentItems)
			{
				if (!_freeContainerItems.Contains(item))
				{
					flag = true;
					break;
				}
			}
			if (flag && Time.time - _lastPlayTime >= _soundCooldown && _cinemachineImpulseSource != null)
			{
				_screenShakeService.TriggerScreenShake(_cinemachineImpulseSource, _screenShakeData);
				_audioService.PlayOneShotAttached(_itemAddedSound, _soundSourceBehaviour);
				_lastPlayTime = Time.time;
			}
			_freeContainerItems.RemoveAll((QuotaContainerItemData item) => !currentItems.Contains(item));
			foreach (QuotaContainerItemData item2 in currentItems)
			{
				if (!_freeContainerItems.Contains(item2))
				{
					_freeContainerItems.Add(item2);
				}
			}
		}
	}
}
