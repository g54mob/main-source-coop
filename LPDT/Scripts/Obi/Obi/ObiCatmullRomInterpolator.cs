namespace Obi
{
	public class ObiCatmullRomInterpolator : ObiInterpolator<float>
	{
		public float Evaluate(float y0, float y1, float y2, float y3, float mu)
		{
			float num = 1f - mu;
			return num * num * num * y0 + 3f * num * num * mu * y1 + 3f * num * mu * mu * y2 + mu * mu * mu * y3;
		}

		public float EvaluateFirstDerivative(float y0, float y1, float y2, float y3, float mu)
		{
			float num = 1f - mu;
			return 3f * num * num * (y1 - y0) + 6f * num * mu * (y2 - y1) + 3f * mu * mu * (y3 - y2);
		}

		public float EvaluateSecondDerivative(float y0, float y1, float y2, float y3, float mu)
		{
			float num = 1f - mu;
			return 3f * num * num * (y1 - y0) + 6f * num * mu * (y2 - y1) + 3f * mu * mu * (y3 - y2);
		}
	}
}
