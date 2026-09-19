using Fusion;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace Features.LevelLightModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class FlashlightInitBehaviour : NetworkBehaviour
	{
		private const string AuthorityPlayerRenderLayerName = "AuthorityPlayer";

		[SerializeField]
		private Transform _flashlight;

		[SerializeField]
		private Light _flashLightLight;

		[SerializeField]
		private UniversalAdditionalLightData _universalAdditionalLightData;

		[SerializeField]
		private bool _useOld;

		private FlashlightPositionsModel _flashlightPositionsModel;

		[Inject]
		public void InjectDependencies(FlashlightPositionsModel flashlightPositionsModel)
		{
			_flashlightPositionsModel = flashlightPositionsModel;
		}

		public override void Spawned()
		{
			if (_flashlightPositionsModel.FlashlightHolders.TryGetValue(base.Object.InputAuthority.PlayerId, out var value) && value.TryGetValue(GetFlashlightPositionType(), out var value2))
			{
				InitializeFlashlight(value2);
			}
			else
			{
				_flashlightPositionsModel.OnFlashlightPositionAdded += OnFlashlightPositionAdded;
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_flashlightPositionsModel.OnFlashlightPositionAdded -= OnFlashlightPositionAdded;
		}

		private FlashlightPositionType GetFlashlightPositionType()
		{
			if (_useOld)
			{
				return FlashlightPositionType.NonAuthority;
			}
			if (!base.Object.HasInputAuthority)
			{
				return FlashlightPositionType.NonAuthority;
			}
			return FlashlightPositionType.Authority;
		}

		private void OnFlashlightPositionAdded(int playerId, FlashlightPositionType flashlightPositionType, Transform flashlightHolder)
		{
			if (playerId == base.Object.InputAuthority.PlayerId && flashlightPositionType == GetFlashlightPositionType())
			{
				InitializeFlashlight(flashlightHolder);
			}
		}

		private void InitializeFlashlight(Transform flashlightHolder)
		{
			_flashlight.SetParent(flashlightHolder, worldPositionStays: false);
			if (base.HasStateAuthority)
			{
				uint num = _universalAdditionalLightData.renderingLayers & ~RenderingLayerMask.GetMask("AuthorityPlayer");
				_flashLightLight.renderingLayerMask = (int)num;
				_universalAdditionalLightData.renderingLayers = num;
				_universalAdditionalLightData.shadowRenderingLayers = num;
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
