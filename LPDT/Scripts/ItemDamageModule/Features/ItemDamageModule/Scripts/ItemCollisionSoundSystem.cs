using System;
using Features.AudioServiceModule.Scripts;
using Features.ItemCollisionModule.Scripts;
using Features.ItemsModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.ScreenShakeModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.ItemDamageModule.Scripts
{
	public class ItemCollisionSoundSystem : IInitializable, IDisposable
	{
		private const float THRESHOLD = 0.3f;

		private readonly ItemCollisionModel _itemCollisionModel;

		private readonly ItemCostLossNetworkEvent _itemCostLossNetworkEvent;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly IScreenShakeService _screenShakeService;

		private readonly IAudioService _audioService;

		public ItemCollisionSoundSystem(ItemCollisionModel itemCollisionModel, ItemCostLossNetworkEvent itemCostLossNetworkEvent, MultiplayerModel multiplayerModel, IScreenShakeService screenShakeService, IAudioService audioService)
		{
			_itemCollisionModel = itemCollisionModel;
			_itemCostLossNetworkEvent = itemCostLossNetworkEvent;
			_multiplayerModel = multiplayerModel;
			_screenShakeService = screenShakeService;
			_audioService = audioService;
		}

		public void Initialize()
		{
			_itemCollisionModel.OnCollisionAdded += PlaySound;
			_itemCostLossNetworkEvent.OnNetworkEventSend += PlaySoundAndParticle;
		}

		public void Dispose()
		{
			_itemCollisionModel.OnCollisionAdded -= PlaySound;
			_itemCostLossNetworkEvent.OnNetworkEventSend -= PlaySoundAndParticle;
		}

		private void PlaySound(ItemCollisionData collisionData)
		{
			if (!(collisionData.Force < 0.3f) && !collisionData.Item.CollisionSound.IsNull && collisionData.Item.IsSoundOnAnyCollision)
			{
				_audioService.PlayOneShot(collisionData.Item.CollisionSound, collisionData.Item.SoundSource);
			}
		}

		private void PlaySoundAndParticle(ItemCostLossNetworkEvent itemCostLossNetworkEvent)
		{
			NetworkObject networkObject = _multiplayerModel.NetworkRunner.FindObject(itemCostLossNetworkEvent.NetworkObjectId);
			if (!(networkObject == null))
			{
				MonoItem component = networkObject.GetComponent<MonoItem>();
				if (component.CinemachineImpulseSource != null)
				{
					_screenShakeService.TriggerScreenShake(component.CinemachineImpulseSource, component.ScreenShakeData);
				}
				if (!component.CollisionSound.IsNull && !component.IsSoundOnAnyCollision)
				{
					_audioService.PlayOneShot(component.CollisionSound, new GenericSoundSource(itemCostLossNetworkEvent.HitPosition, component.SoundSource.ID));
				}
				if (component.CollisionParticle != null)
				{
					UnityEngine.Object.Instantiate(component.CollisionParticle, itemCostLossNetworkEvent.HitPosition, Quaternion.identity);
				}
			}
		}
	}
}
