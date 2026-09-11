using UnityEngine;

namespace Portningsbolaget
{
	[RequireComponent(typeof(ParticleSystem))]
	public class ParticleSystemOptimizer : MonoBehaviour
	{
		private void Start()
		{
		}

		private ParticleSystem.MinMaxCurve GetEmissionCurve(ParticleSystem.MinMaxCurve curve)
		{
			switch (curve.mode)
			{
			case ParticleSystemCurveMode.Constant:
				curve.constant *= GetEmissionRateMultiplier();
				break;
			case ParticleSystemCurveMode.TwoConstants:
				curve.constantMin *= GetEmissionRateMultiplier();
				curve.constantMax *= GetEmissionRateMultiplier();
				break;
			case ParticleSystemCurveMode.Curve:
			case ParticleSystemCurveMode.TwoCurves:
				curve.curveMultiplier *= GetEmissionRateMultiplier();
				break;
			}
			return curve;
		}

		private float GetEmissionRateMultiplier()
		{
			return 1f;
		}
	}
}
