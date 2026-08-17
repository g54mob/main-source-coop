using System;
using System.Collections.Generic;
using Rewired.Utils;

namespace Rewired.Platforms.Custom
{
	public abstract class CustomPlatformUnifiedControllerSource : IDisposable
	{
		private readonly int XDyXDjVCirNGzCliiJhLeAzLDKsaA;

		private readonly int GLKOPazChorPWRxCFBfagkRFxSgh;

		private readonly bool[] GzPgkKeXmqCSgVgtzemQdDNakWdU;

		private readonly bool[] zDzKqtfsjiHvRiTMSHbYdkcFhmggA;

		private readonly float[] JdCRinHfxMnLCOnnkNACVQxBaKLfA;

		private bool RGSWatbACwXjEqhAijSNHnbIGSLab;

		public int axisCount => XDyXDjVCirNGzCliiJhLeAzLDKsaA;

		public int buttonCount => GLKOPazChorPWRxCFBfagkRFxSgh;

		public virtual Controller.Extension controllerExtension => null;

		public CustomPlatformUnifiedControllerSource(int P_0, int P_1)
		{
			if (P_0 < 0)
			{
				P_0 = 0;
			}
			if (P_1 < 0)
			{
				P_1 = 0;
			}
			XDyXDjVCirNGzCliiJhLeAzLDKsaA = P_0;
			GLKOPazChorPWRxCFBfagkRFxSgh = P_1;
			JdCRinHfxMnLCOnnkNACVQxBaKLfA = new float[P_0];
			GzPgkKeXmqCSgVgtzemQdDNakWdU = new bool[P_1];
			zDzKqtfsjiHvRiTMSHbYdkcFhmggA = new bool[P_1];
		}

		protected abstract void Update();

		internal virtual void teTpmqVqEpUVjdZLwlCvmChBpAaf()
		{
		}

		protected virtual void OnInitialize()
		{
		}

		protected virtual void Clear()
		{
			Array.Clear(JdCRinHfxMnLCOnnkNACVQxBaKLfA, 0, XDyXDjVCirNGzCliiJhLeAzLDKsaA);
			Array.Clear(GzPgkKeXmqCSgVgtzemQdDNakWdU, 0, GLKOPazChorPWRxCFBfagkRFxSgh);
			Array.Clear(zDzKqtfsjiHvRiTMSHbYdkcFhmggA, 0, GLKOPazChorPWRxCFBfagkRFxSgh);
		}

		protected float GetAxisValue(int index)
		{
			if ((uint)index >= (uint)axisCount)
			{
				return 0f;
			}
			return JdCRinHfxMnLCOnnkNACVQxBaKLfA[index];
		}

		protected bool GetButtonValue(int index)
		{
			if ((uint)index >= (uint)buttonCount)
			{
				return false;
			}
			return GzPgkKeXmqCSgVgtzemQdDNakWdU[index];
		}

		protected void SetAxisValue(int index, float value)
		{
			if ((uint)index < (uint)axisCount)
			{
				JdCRinHfxMnLCOnnkNACVQxBaKLfA[index] = value;
			}
		}

		protected void SetAxisValues(IList<float> values)
		{
			if (values == null)
			{
				return;
			}
			int num = MathTools.Min(values.Count, axisCount);
			if (values is float[])
			{
				Array.Copy(values as float[], JdCRinHfxMnLCOnnkNACVQxBaKLfA, num);
				return;
			}
			for (int i = 0; i < num; i++)
			{
				JdCRinHfxMnLCOnnkNACVQxBaKLfA[i] = values[i];
			}
		}

		protected void SetButtonValue(int index, bool value)
		{
			if ((uint)index < (uint)buttonCount)
			{
				if (!GzPgkKeXmqCSgVgtzemQdDNakWdU[index] && value)
				{
					zDzKqtfsjiHvRiTMSHbYdkcFhmggA[index] = true;
				}
				GzPgkKeXmqCSgVgtzemQdDNakWdU[index] = value;
			}
		}

		protected void SetButtonValues(IList<bool> values)
		{
			if (values == null)
			{
				return;
			}
			int num = MathTools.Min(values.Count, buttonCount);
			if (values is bool[])
			{
				for (int i = 0; i < num; i++)
				{
					if (!GzPgkKeXmqCSgVgtzemQdDNakWdU[i] && values[i])
					{
						zDzKqtfsjiHvRiTMSHbYdkcFhmggA[i] = true;
					}
				}
				Array.Copy(values as bool[], GzPgkKeXmqCSgVgtzemQdDNakWdU, num);
				return;
			}
			for (int j = 0; j < num; j++)
			{
				bool flag = values[j];
				if (!GzPgkKeXmqCSgVgtzemQdDNakWdU[j] && flag)
				{
					zDzKqtfsjiHvRiTMSHbYdkcFhmggA[j] = true;
				}
				GzPgkKeXmqCSgVgtzemQdDNakWdU[j] = flag;
			}
		}

		internal void pOiaNcIMMZahjlhCneHRJBdSieLf()
		{
			OnInitialize();
		}

		internal void ciMcgCoDTiPEvKMPuLFgSXnANuBF()
		{
			Clear();
		}

		internal void PQAOuAbNmOhBwIbRhIASuNkNGPE(ControllerDataUpdater P_0)
		{
			ONkVMJZwpVbRRAfxSXQwkiTeKljR();
			Update();
			teTpmqVqEpUVjdZLwlCvmChBpAaf();
			Array.Copy(JdCRinHfxMnLCOnnkNACVQxBaKLfA, P_0.axisValues, XDyXDjVCirNGzCliiJhLeAzLDKsaA);
			for (int i = 0; i < XDyXDjVCirNGzCliiJhLeAzLDKsaA; i++)
			{
				if (JdCRinHfxMnLCOnnkNACVQxBaKLfA[i] != 0f && !P_0.hasReceivedInput)
				{
					P_0.hasReceivedInput = true;
				}
			}
			Array.Copy(GzPgkKeXmqCSgVgtzemQdDNakWdU, P_0.buttonValues, GLKOPazChorPWRxCFBfagkRFxSgh);
			for (int j = 0; j < GLKOPazChorPWRxCFBfagkRFxSgh; j++)
			{
				if (GzPgkKeXmqCSgVgtzemQdDNakWdU[j] && !P_0.hasReceivedInput)
				{
					P_0.hasReceivedInput = true;
				}
				if (zDzKqtfsjiHvRiTMSHbYdkcFhmggA[j] && !GzPgkKeXmqCSgVgtzemQdDNakWdU[j])
				{
					GzPgkKeXmqCSgVgtzemQdDNakWdU[j] = true;
				}
			}
		}

		private void ONkVMJZwpVbRRAfxSXQwkiTeKljR()
		{
			Array.Clear(zDzKqtfsjiHvRiTMSHbYdkcFhmggA, 0, zDzKqtfsjiHvRiTMSHbYdkcFhmggA.Length);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!RGSWatbACwXjEqhAijSNHnbIGSLab)
			{
				RGSWatbACwXjEqhAijSNHnbIGSLab = true;
			}
		}

		void IDisposable.Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
