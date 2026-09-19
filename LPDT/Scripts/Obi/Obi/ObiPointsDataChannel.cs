using System;
using UnityEngine;

namespace Obi
{
	[Serializable]
	public class ObiPointsDataChannel : ObiPathDataChannel<ObiWingedPoint, Vector3>
	{
		public ObiPointsDataChannel()
			: base((ObiInterpolator<Vector3>)new ObiCatmullRomInterpolator3D())
		{
		}

		public Vector3 GetTangent(int index)
		{
			int i = (index + 1) % base.Count;
			ObiWingedPoint obiWingedPoint = base[index];
			ObiWingedPoint obiWingedPoint2 = base[i];
			return EvaluateFirstDerivative(obiWingedPoint.position, obiWingedPoint.outTangentEndpoint, obiWingedPoint2.inTangentEndpoint, obiWingedPoint2.position, 0f);
		}

		public Vector3 GetAcceleration(int index)
		{
			int i = (index + 1) % base.Count;
			ObiWingedPoint obiWingedPoint = base[index];
			ObiWingedPoint obiWingedPoint2 = base[i];
			return EvaluateSecondDerivative(obiWingedPoint.position, obiWingedPoint.outTangentEndpoint, obiWingedPoint2.inTangentEndpoint, obiWingedPoint2.position, 0f);
		}

		public Vector3 GetPositionAtMu(bool closed, float mu)
		{
			int count = base.Count;
			if (count >= 2)
			{
				float spanMu;
				int spanControlPointAtMu = GetSpanControlPointAtMu(closed, mu, out spanMu);
				int i = (spanControlPointAtMu + 1) % count;
				ObiWingedPoint obiWingedPoint = base[spanControlPointAtMu];
				ObiWingedPoint obiWingedPoint2 = base[i];
				return Evaluate(obiWingedPoint.position, obiWingedPoint.outTangentEndpoint, obiWingedPoint2.inTangentEndpoint, obiWingedPoint2.position, spanMu);
			}
			throw new InvalidOperationException("Cannot get position in path because it has zero control points.");
		}

		public Vector3 GetTangentAtMu(bool closed, float mu)
		{
			int count = base.Count;
			if (count >= 2)
			{
				float spanMu;
				int spanControlPointAtMu = GetSpanControlPointAtMu(closed, mu, out spanMu);
				int i = (spanControlPointAtMu + 1) % count;
				ObiWingedPoint obiWingedPoint = base[spanControlPointAtMu];
				ObiWingedPoint obiWingedPoint2 = base[i];
				return EvaluateFirstDerivative(obiWingedPoint.position, obiWingedPoint.outTangentEndpoint, obiWingedPoint2.inTangentEndpoint, obiWingedPoint2.position, spanMu);
			}
			throw new InvalidOperationException("Cannot get derivative in path because it has less than 2 control points.");
		}

		public Vector3 GetAccelerationAtMu(bool closed, float mu)
		{
			int count = base.Count;
			if (count >= 2)
			{
				float spanMu;
				int spanControlPointAtMu = GetSpanControlPointAtMu(closed, mu, out spanMu);
				int i = (spanControlPointAtMu + 1) % count;
				ObiWingedPoint obiWingedPoint = base[spanControlPointAtMu];
				ObiWingedPoint obiWingedPoint2 = base[i];
				return EvaluateSecondDerivative(obiWingedPoint.position, obiWingedPoint.outTangentEndpoint, obiWingedPoint2.inTangentEndpoint, obiWingedPoint2.position, spanMu);
			}
			throw new InvalidOperationException("Cannot get second derivative in path because it has less than 2 control points.");
		}
	}
}
