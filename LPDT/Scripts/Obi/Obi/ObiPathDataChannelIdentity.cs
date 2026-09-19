using System;

namespace Obi
{
	public abstract class ObiPathDataChannelIdentity<T> : ObiPathDataChannel<T, T>
	{
		public ObiPathDataChannelIdentity(ObiInterpolator<T> interpolator)
			: base(interpolator)
		{
		}

		public T GetFirstDerivative(int index)
		{
			int i = (index + 1) % base.Count;
			return EvaluateFirstDerivative(base[index], base[index], base[i], base[i], 0f);
		}

		public T GetSecondDerivative(int index)
		{
			int i = (index + 1) % base.Count;
			return EvaluateSecondDerivative(base[index], base[index], base[i], base[i], 0f);
		}

		public T GetAtMu(bool closed, float mu)
		{
			int count = base.Count;
			if (count >= 2)
			{
				float spanMu;
				int spanControlPointAtMu = GetSpanControlPointAtMu(closed, mu, out spanMu);
				int i = (spanControlPointAtMu + 1) % count;
				return Evaluate(base[spanControlPointAtMu], base[spanControlPointAtMu], base[i], base[i], spanMu);
			}
			throw new InvalidOperationException("Cannot get property in path because it has less than 2 control points.");
		}

		public T GetFirstDerivativeAtMu(bool closed, float mu)
		{
			int count = base.Count;
			if (count >= 2)
			{
				float spanMu;
				int spanControlPointAtMu = GetSpanControlPointAtMu(closed, mu, out spanMu);
				int i = (spanControlPointAtMu + 1) % count;
				return EvaluateFirstDerivative(base[spanControlPointAtMu], base[spanControlPointAtMu], base[i], base[i], spanMu);
			}
			throw new InvalidOperationException("Cannot get derivative in path because it has less than 2 control points.");
		}

		public T GetSecondDerivativeAtMu(bool closed, float mu)
		{
			int count = base.Count;
			if (count >= 2)
			{
				float spanMu;
				int spanControlPointAtMu = GetSpanControlPointAtMu(closed, mu, out spanMu);
				int i = (spanControlPointAtMu + 1) % count;
				return EvaluateSecondDerivative(base[spanControlPointAtMu], base[spanControlPointAtMu], base[i], base[i], spanMu);
			}
			throw new InvalidOperationException("Cannot get second derivative in path because it has less than 2 control points.");
		}
	}
}
