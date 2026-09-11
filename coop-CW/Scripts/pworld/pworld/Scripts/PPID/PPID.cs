using System;

namespace pworld.Scripts.PPID
{
	[Serializable]
	public class PPID
	{
		public float p = 1f;

		public float i;

		public float d = 0.1f;

		private float prevError;

		private float proportion;

		private float integral;

		private float derivative;

		public void UpdateValues(float p, float i, float d)
		{
			this.p = p;
			this.i = i;
			this.d = d;
		}

		public float GetOutput(float currentError, float dt)
		{
			proportion = currentError;
			integral += proportion * dt;
			derivative = (proportion - prevError) / dt;
			prevError = currentError;
			return proportion * p + integral * i + derivative * d;
		}
	}
}
