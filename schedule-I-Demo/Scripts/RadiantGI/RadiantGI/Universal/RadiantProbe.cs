using UnityEngine;

namespace RadiantGI.Universal
{
	[ExecuteInEditMode]
	public class RadiantProbe : MonoBehaviour
	{
		private ReflectionProbe probe;

		private void OnEnable()
		{
			probe = GetComponent<ReflectionProbe>();
			RadiantRenderFeature.RegisterReflectionProbe(probe);
		}

		private void OnDisable()
		{
			RadiantRenderFeature.UnregisterReflectionProbe(probe);
		}
	}
}
