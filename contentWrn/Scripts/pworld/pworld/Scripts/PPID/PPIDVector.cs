using System;
using UnityEngine;

namespace pworld.Scripts.PPID
{
	[Serializable]
	public class PPIDVector
	{
		public float P;

		public float I;

		public float D;

		private PPID xPid = new PPID();

		private PPID yPid = new PPID();

		private PPID zPid = new PPID();

		public PPIDVector(float P, float I, float D)
		{
			this.P = P;
			this.I = I;
			this.D = D;
			UpdatePIDValues();
		}

		public PPIDVector()
		{
			UpdatePIDValues();
		}

		public void UpdateValues(float p, float i, float d)
		{
			P = p;
			I = i;
			D = d;
			UpdatePIDValues();
		}

		private void UpdatePIDValues()
		{
			xPid.UpdateValues(P, I, D);
			yPid.UpdateValues(P, I, D);
			zPid.UpdateValues(P, I, D);
		}

		public Vector3 GetOutput(Vector3 currentError, float dt)
		{
			Vector3 zero = Vector3.zero;
			UpdatePIDValues();
			zero.x = xPid.GetOutput(currentError.x, dt);
			zero.y = yPid.GetOutput(currentError.y, dt);
			zero.z = zPid.GetOutput(currentError.z, dt);
			return zero;
		}
	}
}
