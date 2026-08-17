using System.Collections;
using NWH.Common.Vehicles;
using UnityEngine;

namespace NWH.WheelController3D
{
	[DisallowMultipleComponent]
	public class TyreWear : MonoBehaviour
	{
		[Tooltip("Wear rate coefficient. Unitless.")]
		public float wearRate = 0.01f;

		[Tooltip("Grip coefficient at the 100% wear level.")]
		public float maxGripReduction = 0.4f;

		[Tooltip("The effect of load on the tire wear.")]
		public float loadWearContribution = 1f;

		[Tooltip("Wear coefficient for lateral slip.")]
		public float lateralSlipWearContribution = 1f;

		[Tooltip("Wear coefficient for longitudinal slip.")]
		public float longitudinalSlipWearContribution = 1f;

		[Range(0.01f, 0.5f)]
		[Tooltip("Coroutine update frequency in seconds.")]
		public float updateRate = 0.1f;

		[Range(0f, 1f)]
		[Tooltip("Current tire wear. 0 = no wear, 1 = fully worn.")]
		public float wear;

		private WheelUAPI _wc;

		private float _initLatGrip;

		private float _initLngGrip;

		private void Awake()
		{
			_wc = GetComponent<WheelUAPI>();
			_initLatGrip = _wc.LateralFrictionGrip;
			_initLngGrip = _wc.LongitudinalFrictionGrip;
		}

		private void OnEnable()
		{
			StartCoroutine(TyreWearCoroutine());
		}

		private void OnDisable()
		{
			StopCoroutine(TyreWearCoroutine());
		}

		private IEnumerator TyreWearCoroutine()
		{
			while (true)
			{
				if (_wc.TargetRigidbody.linearVelocity.sqrMagnitude > 0.5f)
				{
					float num = Mathf.Clamp01(_wc.Load / _wc.MaxLoad) * loadWearContribution;
					float num2 = Mathf.Abs(_wc.LongitudinalSlip) * longitudinalSlipWearContribution;
					float num3 = Mathf.Abs(_wc.LateralSlip) * lateralSlipWearContribution;
					wear += (num2 + num3) * num * updateRate * wearRate;
					wear = Mathf.Clamp01(wear);
				}
				_wc.LateralFrictionGrip = _initLatGrip - wear * maxGripReduction;
				_wc.LongitudinalFrictionGrip = _initLngGrip - wear * maxGripReduction;
				yield return new WaitForSeconds(updateRate);
			}
		}
	}
}
