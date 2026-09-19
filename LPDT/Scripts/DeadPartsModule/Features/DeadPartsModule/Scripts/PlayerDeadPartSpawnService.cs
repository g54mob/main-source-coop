using System;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Features.DeadPartsModule.Data;
using Features.Extensions;
using Features.MultiplayerSessionServices.Scripts;
using Features.RagdollModule.Scripts;
using Features.SkinConfiguration.Scripts;
using Fusion;
using PlayerCustomization;
using UnityEngine;

namespace Features.DeadPartsModule.Scripts
{
	public class PlayerDeadPartSpawnService : IPlayerDeadPartSpawnService
	{
		private const float INITIALIZATION_TIMEOUT = 5f;

		private static readonly Color _defaultDeadPartColor = Color.white;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly PlayerDeadPartsConfiguration _deadPartsConfiguration;

		private readonly PlayerDeadPartModel _playerDeadPartModel;

		private readonly PlayerCustomizationModel _playerCustomizationModel;

		private readonly PlayerDeadPartTypes _playerDeadPartTypes;

		public PlayerDeadPartSpawnService(MultiplayerModel multiplayerModel, PlayerDeadPartsConfiguration deadPartsConfiguration, PlayerDeadPartModel playerDeadPartModel, PlayerCustomizationModel playerCustomizationModel, PlayerDeadPartTypes playerDeadPartTypes)
		{
			_multiplayerModel = multiplayerModel;
			_deadPartsConfiguration = deadPartsConfiguration;
			_playerDeadPartModel = playerDeadPartModel;
			_playerCustomizationModel = playerCustomizationModel;
			_playerDeadPartTypes = playerDeadPartTypes;
		}

		public void ReapplySavedDeadPartBooster()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			int playerId = networkRunner.LocalPlayer.PlayerId;
			if (!_playerDeadPartTypes.HasConnectedPart(playerId))
			{
				return;
			}
			DeadPartType deadPartType = _playerDeadPartTypes.GetDeadPartType(playerId);
			if (!_deadPartsConfiguration.ButtPool.TryGetValue(deadPartType, out var value))
			{
				return;
			}
			PlayerAlivePart playerAlivePart = _playerDeadPartModel.PlayerAlivePart;
			if (!(playerAlivePart == null))
			{
				NetworkObject networkObject = networkRunner.Prefabs.Load(networkRunner.Prefabs.GetId((NetworkObjectGuid)value), isSynchronous: true);
				if (!(networkObject == null) && networkObject.TryGetComponent<PlayerDeadPart>(out var component))
				{
					playerAlivePart.SetDeadPartUsageCount(_playerDeadPartTypes.GetUsageCount(playerId));
					playerAlivePart.DeadPartJoinBoosterBehaviour.DisableDeadPartEffect();
					playerAlivePart.DeadPartJoinBoosterBehaviour.ApplyDeadPartEffect(component.BoosterSetting);
				}
			}
		}

		public async void SpawnPlayerDeadPartForCurrentPlayer()
		{
			if (PlayerSessionPrefs.IsDeadPartSpawned())
			{
				return;
			}
			PlayerSessionPrefs.SetDeadPartSpawned(isSpawned: true);
			PlayerAlivePart playerAlivePart = _playerDeadPartModel.PlayerAlivePart;
			int localPlayerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			DeadPartType deadPartType = _playerDeadPartTypes.GetDeadPartType(localPlayerId);
			int num = Math.Max(_playerDeadPartTypes.GetUsageCount(localPlayerId), playerAlivePart.DeadPartUsageCount);
			if (num < _deadPartsConfiguration.MaxDeadPartUsageCount[deadPartType])
			{
				PlayerCustomizationSlotData slotData = _playerCustomizationModel.Slots.FirstOrDefault((PlayerCustomizationSlotData x) => x.PlayerId == localPlayerId);
				await SpawnPlayerDeadPart(deadPartType, playerAlivePart.DownPos.position, num + 1, slotData, addForce: true);
			}
		}

		public Task<PlayerDeadPart> SpawnPlayerDeadPart(DeadPartType deadPartType, Vector3 position, int usageCound, bool addForce = false)
		{
			return SpawnPlayerDeadPart(deadPartType, position, usageCound, null, addForce);
		}

		public async Task<PlayerDeadPart> SpawnPlayerDeadPart(DeadPartType deadPartType, Vector3 position, int usageCound, PlayerCustomizationSlotData slotData, bool addForce = false)
		{
			if (!_deadPartsConfiguration.ButtPool.TryGetValue(deadPartType, out var value))
			{
				return null;
			}
			NetworkObject spawnedObject = await _multiplayerModel.NetworkRunner.SpawnAsync(value, position, Quaternion.identity, _multiplayerModel.NetworkRunner.LocalPlayer);
			if (spawnedObject == null)
			{
				return null;
			}
			PlayerDeadPart playerDeadPart = spawnedObject.GetComponent<PlayerDeadPart>();
			if (slotData != null)
			{
				DeadPartCustomizationData customizationData = new DeadPartCustomizationData
				{
					ButtColor = BuildVisibleDeadPartColor(slotData.VariableColor),
					ButtSkinId = slotData.BottomPartSkinId,
					ButtTexturePreset = slotData.ButtTexturePreset,
					ButtMeshPreset = slotData.ButtMeshPreset
				};
				customizationData.ButtColor = ((slotData.VariableColor != default(Color)) ? BuildVisibleDeadPartColor(slotData.VariableColor) : playerDeadPart.CustomizationData.ButtColor);
				customizationData.ButtSkinId = ((slotData.BottomPartSkinId != SkinType.None) ? slotData.BottomPartSkinId : playerDeadPart.CustomizationData.ButtSkinId);
				customizationData.ButtTexturePreset = ((slotData.ButtTexturePreset != ButtTexturePreset.None) ? slotData.ButtTexturePreset : playerDeadPart.CustomizationData.ButtTexturePreset);
				customizationData.ButtMeshPreset = ((slotData.ButtMeshPreset != ButtMeshPreset.None) ? slotData.ButtMeshPreset : playerDeadPart.CustomizationData.ButtMeshPreset);
				playerDeadPart.OverrideCustomizationData(customizationData);
			}
			playerDeadPart.SetUsageCount(usageCound);
			await UniTask.WaitForFixedUpdate();
			await UniTask.WaitUntil(() => playerDeadPart.RagdollEntity.IsInitialized).TimeoutWithoutException(TimeSpan.FromSeconds(5.0));
			if (!playerDeadPart.RagdollEntity.IsInitialized || spawnedObject == null || !spawnedObject.IsValid)
			{
				if (spawnedObject != null && spawnedObject.HasStateAuthority)
				{
					_multiplayerModel.NetworkRunner.Despawn(spawnedObject);
				}
				return null;
			}
			if (slotData == null && playerDeadPart.DeadPartType != DeadPartType.GoldenButt && playerDeadPart.DeadPartType != DeadPartType.IronButt)
			{
				DeadPartCustomizationData customizationData2 = playerDeadPart.CustomizationData;
				customizationData2.ButtColor = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
				playerDeadPart.OverrideCustomizationData(customizationData2);
			}
			playerDeadPart.RagdollEntity.AddSimulationReason(RagdollSimulationReasonEnum.DeadPart);
			playerDeadPart.RagdollEntity.SetInterpolation(RigidbodyInterpolation.Interpolate);
			if (!addForce)
			{
				return playerDeadPart;
			}
			Vector3 vector = playerDeadPart.BodyCollider.CapsuleTop();
			Vector3 normalized = (playerDeadPart.BodyCollider.CapsuleBottom() - vector).normalized;
			playerDeadPart.BodyRigidbody.AddForce(normalized * _deadPartsConfiguration.PlayerDeadPartSpawnForce, ForceMode.Impulse);
			playerDeadPart.BodyRigidbody.AddTorque(UnityEngine.Random.insideUnitSphere.normalized * _deadPartsConfiguration.PlayerDeadPartSpawnTorque, ForceMode.Impulse);
			return playerDeadPart;
		}

		private static Color BuildVisibleDeadPartColor(Color sourceColor)
		{
			if (sourceColor.r <= 0f && sourceColor.g <= 0f && sourceColor.b <= 0f && sourceColor.a <= 0f)
			{
				sourceColor = _defaultDeadPartColor;
			}
			sourceColor.a = 1f;
			return sourceColor;
		}
	}
}
