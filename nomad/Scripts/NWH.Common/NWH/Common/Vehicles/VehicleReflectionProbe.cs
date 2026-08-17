using UnityEngine;
using UnityEngine.Rendering;

namespace NWH.Common.Vehicles
{
	[RequireComponent(typeof(ReflectionProbe))]
	[DefaultExecutionOrder(19)]
	public class VehicleReflectionProbe : MonoBehaviour
	{
		public enum ProbeType
		{
			Baked = 0,
			Realtime = 1
		}

		public ProbeType awakeProbeType = ProbeType.Realtime;

		public ProbeType asleepProbeType;

		public bool bakeOnStart = true;

		public bool bakeOnSleep = true;

		private ReflectionProbe _reflectionProbe;

		private Vehicle _vc;

		private void OnEnable()
		{
			_vc = GetComponentInParent<Vehicle>();
			if (_vc == null)
			{
				Debug.LogError("VehicleController not found.");
			}
			_reflectionProbe = GetComponent<ReflectionProbe>();
			_vc.onEnable.AddListener(OnVehicleEnable);
			_vc.onDisable.AddListener(OnVehicleDisable);
			_reflectionProbe.refreshMode = ReflectionProbeRefreshMode.EveryFrame;
			if (bakeOnStart)
			{
				_reflectionProbe.RenderProbe();
			}
		}

		private void OnVehicleEnable()
		{
			ReflectionProbe reflectionProbe = _reflectionProbe;
			int mode;
			if (awakeProbeType != ProbeType.Baked)
			{
				mode = 1;
			}
			else
			{
				ReflectionProbeMode reflectionProbeMode = (_reflectionProbe.mode = ReflectionProbeMode.Baked);
				mode = (int)reflectionProbeMode;
			}
			reflectionProbe.mode = (ReflectionProbeMode)mode;
		}

		private void OnVehicleDisable()
		{
			ReflectionProbe reflectionProbe = _reflectionProbe;
			int mode;
			if (asleepProbeType != ProbeType.Baked)
			{
				mode = 1;
			}
			else
			{
				ReflectionProbeMode reflectionProbeMode = (_reflectionProbe.mode = ReflectionProbeMode.Baked);
				mode = (int)reflectionProbeMode;
			}
			reflectionProbe.mode = (ReflectionProbeMode)mode;
			if (bakeOnSleep && _reflectionProbe.isActiveAndEnabled)
			{
				_reflectionProbe.RenderProbe();
			}
		}
	}
}
