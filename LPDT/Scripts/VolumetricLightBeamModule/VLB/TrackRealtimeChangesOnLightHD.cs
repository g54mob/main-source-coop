using UnityEngine;
using VolumetricLightBeam.Scripts.HD;

namespace VLB
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Light), typeof(VolumetricLightBeamHd))]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-trackrealtimechanges-hd/")]
	[AddComponentMenu("VLB/HD/Track Realtime Changes On Light")]
	public class TrackRealtimeChangesOnLightHD : MonoBehaviour
	{
		public const string ClassName = "TrackRealtimeChangesOnLightHD";

		private VolumetricLightBeamHd m_Master;

		private void Awake()
		{
			m_Master = GetComponent<VolumetricLightBeamHd>();
		}

		private void Update()
		{
			if (m_Master.enabled)
			{
				m_Master.AssignPropertiesFromAttachedSpotLight();
			}
		}
	}
}
