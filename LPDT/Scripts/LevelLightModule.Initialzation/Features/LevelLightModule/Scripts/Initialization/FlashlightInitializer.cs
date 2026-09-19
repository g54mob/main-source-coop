using Fusion;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Features.LevelLightModule.Scripts.Initialization
{
	public class FlashlightInitializer : MonoBehaviour
	{
		private const string AuthorityPlayerRenderLayerName = "AuthorityPlayer";

		[SerializeField]
		private NetworkObject _networkObject;

		[SerializeField]
		private Light _flashLight;

		[SerializeField]
		private UniversalAdditionalLightData _universalAdditionalLightData;

		private void Start()
		{
			if (_networkObject.HasInputAuthority)
			{
				uint mask = RenderingLayerMask.GetMask("AuthorityPlayer");
				_flashLight.renderingLayerMask = (int)mask;
				_universalAdditionalLightData.renderingLayers = mask;
				_universalAdditionalLightData.shadowRenderingLayers = mask;
			}
		}
	}
}
