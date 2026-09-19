namespace Obi
{
	public class ObiConstantInterpolator : ObiInterpolator<int>
	{
		public int Evaluate(int y0, int y1, int y2, int y3, float mu)
		{
			if (!(mu < 0.5f))
			{
				return y2;
			}
			return y1;
		}

		public int EvaluateFirstDerivative(int y0, int y1, int y2, int y3, float mu)
		{
			return 0;
		}

		public int EvaluateSecondDerivative(int y0, int y1, int y2, int y3, float mu)
		{
			return 0;
		}
	}
}
