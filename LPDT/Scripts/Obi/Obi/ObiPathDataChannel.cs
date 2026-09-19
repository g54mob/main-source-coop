using System.Collections.Generic;

namespace Obi
{
	public abstract class ObiPathDataChannel<T, U> : IObiPathDataChannel
	{
		protected ObiInterpolator<U> interpolator;

		protected bool dirty;

		public List<T> data = new List<T>();

		public int Count => data.Count;

		public bool Dirty => dirty;

		public T this[int i]
		{
			get
			{
				return data[i];
			}
			set
			{
				data[i] = value;
				dirty = true;
			}
		}

		public void Clean()
		{
			dirty = false;
		}

		public ObiPathDataChannel(ObiInterpolator<U> interpolator)
		{
			this.interpolator = interpolator;
		}

		public void RemoveAt(int index)
		{
			data.RemoveAt(index);
			dirty = true;
		}

		public U Evaluate(U v0, U v1, U v2, U v3, float mu)
		{
			return interpolator.Evaluate(v0, v1, v2, v3, mu);
		}

		public U EvaluateFirstDerivative(U v0, U v1, U v2, U v3, float mu)
		{
			return interpolator.EvaluateFirstDerivative(v0, v1, v2, v3, mu);
		}

		public U EvaluateSecondDerivative(U v0, U v1, U v2, U v3, float mu)
		{
			return interpolator.EvaluateSecondDerivative(v0, v1, v2, v3, mu);
		}

		public int GetSpanCount(bool closed)
		{
			int count = Count;
			if (count < 2)
			{
				return 0;
			}
			if (!closed)
			{
				return count - 1;
			}
			return count;
		}

		public int GetSpanControlPointAtMu(bool closed, float mu, out float spanMu)
		{
			int spanCount = GetSpanCount(closed);
			spanMu = mu * (float)spanCount;
			int num = ((mu >= 1f) ? (spanCount - 1) : ((int)spanMu));
			spanMu -= num;
			return num;
		}
	}
}
