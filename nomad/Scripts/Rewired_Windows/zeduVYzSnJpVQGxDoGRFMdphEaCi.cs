using System;
using Rewired;
using Rewired.Config;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;
using Rewired.Utils.Classes.Utility;
using UnityEngine;

internal class zeduVYzSnJpVQGxDoGRFMdphEaCi : hbZPQLtaSNORhjIfKDnNgXuOkWyd
{
	internal class QvNUGogGOMluZJeMtAkreXbkRbQw : mdJxcuSvBwFnERIGiHmufaVmYVwHA
	{
		private int OlKgOFprUMeuGxjyJMsjzNoZJWed;

		private int rbapYHfYPaBsehvHSuQhXZrOlEgH;

		public float[] cxzOSwLGHtLWXAuMqdsMgNqvdZdAA => (QXQwwaATLYfidsLWJWzyoKftgSWJ as bOcktiAfMgTEYrPNGwjkwftjXlEt).GzHmZLnpgSdFOVsTrvhnfRMJOCLj;

		public RingBuffer<hOJhFTpGFkIeuGuGckkEoiyPlXuc> EnrWIrZCRwzKrhjFwFPNiDYdzRxMA => (QXQwwaATLYfidsLWJWzyoKftgSWJ as bOcktiAfMgTEYrPNGwjkwftjXlEt).MhacXUeUmstUFcCUlQxWNWErpYtQ;

		public QvNUGogGOMluZJeMtAkreXbkRbQw(UpdateLoopSetting P_0, int P_1, int P_2)
		{
			OlKgOFprUMeuGxjyJMsjzNoZJWed = P_1;
			rbapYHfYPaBsehvHSuQhXZrOlEgH = P_2;
			RrfyasJXRbuSVOUXSIgFqrLRnnFl(P_0, JpoNARQCVYOkYrajNNTRhtHHQzBr);
		}

		public virtual void gKhSheIuGhRAFhxFUwHeGGyuOBso(UpdateLoopType P_0)
		{
			base.KSeWPXQBXetJhSAfPModLxzjazzT(P_0);
			(QXQwwaATLYfidsLWJWzyoKftgSWJ as bOcktiAfMgTEYrPNGwjkwftjXlEt).oDscGUoqMWulOothzsnVAfRZJPpH();
		}

		public void VuilXIEYaVZxnxYEVFPoAfskmmWg(float[] P_0, float P_1)
		{
			for (int i = 0; i < OdbrPCRFWXQUboXWVEJvvNuudOAY.Length; i++)
			{
				(OdbrPCRFWXQUboXWVEJvvNuudOAY[i] as bOcktiAfMgTEYrPNGwjkwftjXlEt).kdURDcNpOQEtsbcuZgFYunCSmIzgb(P_0, P_1);
			}
		}

		private CvBbYwvZfLeRTAPLFSXaxjnvmVmC JpoNARQCVYOkYrajNNTRhtHHQzBr(UpdateLoopType P_0)
		{
			return new bOcktiAfMgTEYrPNGwjkwftjXlEt(P_0, OlKgOFprUMeuGxjyJMsjzNoZJWed, rbapYHfYPaBsehvHSuQhXZrOlEgH);
		}
	}

	internal class bOcktiAfMgTEYrPNGwjkwftjXlEt : CvBbYwvZfLeRTAPLFSXaxjnvmVmC
	{
		[Serializable]
		private sealed class zwRwSXKrNyJurttOWKjrtTQtLrqm
		{
			public static readonly zwRwSXKrNyJurttOWKjrtTQtLrqm _003C_003E9 = new zwRwSXKrNyJurttOWKjrtTQtLrqm();

			public static Func<hOJhFTpGFkIeuGuGckkEoiyPlXuc> _003C_003E9__5_0;

			internal hOJhFTpGFkIeuGuGckkEoiyPlXuc NNiekdkdClMTIYldqYJqehUBjuwkA()
			{
				return new hOJhFTpGFkIeuGuGckkEoiyPlXuc();
			}
		}

		private float[] XRedVKCDXHEjYaWjcLRdNmsGRvLG;

		public float[] GzHmZLnpgSdFOVsTrvhnfRMJOCLj;

		public RingBuffer<hOJhFTpGFkIeuGuGckkEoiyPlXuc> MhacXUeUmstUFcCUlQxWNWErpYtQ;

		private RingBuffer<hOJhFTpGFkIeuGuGckkEoiyPlXuc> ZaSngQlSWxOuGaicCkHdwWQiFhhT;

		private ObjectPool<hOJhFTpGFkIeuGuGckkEoiyPlXuc> PAALQWNwbfOPfHKpQyfsRvgBraMt;

		public bOcktiAfMgTEYrPNGwjkwftjXlEt(UpdateLoopType P_0, int P_1, int P_2)
			: base(P_0)
		{
			GzHmZLnpgSdFOVsTrvhnfRMJOCLj = new float[P_1];
			XRedVKCDXHEjYaWjcLRdNmsGRvLG = new float[P_1];
			MhacXUeUmstUFcCUlQxWNWErpYtQ = new RingBuffer<hOJhFTpGFkIeuGuGckkEoiyPlXuc>(P_2);
			ZaSngQlSWxOuGaicCkHdwWQiFhhT = new RingBuffer<hOJhFTpGFkIeuGuGckkEoiyPlXuc>(P_2);
			PAALQWNwbfOPfHKpQyfsRvgBraMt = new ObjectPool<hOJhFTpGFkIeuGuGckkEoiyPlXuc>(P_2, zwRwSXKrNyJurttOWKjrtTQtLrqm._003C_003E9.NNiekdkdClMTIYldqYJqehUBjuwkA);
		}

		public void oDscGUoqMWulOothzsnVAfRZJPpH()
		{
			for (int i = 0; i < XRedVKCDXHEjYaWjcLRdNmsGRvLG.Length; i++)
			{
				GzHmZLnpgSdFOVsTrvhnfRMJOCLj[i] = XRedVKCDXHEjYaWjcLRdNmsGRvLG[i];
				XRedVKCDXHEjYaWjcLRdNmsGRvLG[i] = 0f;
			}
			CollectionTools.Clear(PAALQWNwbfOPfHKpQyfsRvgBraMt, MhacXUeUmstUFcCUlQxWNWErpYtQ);
			int count = ZaSngQlSWxOuGaicCkHdwWQiFhhT.Count;
			for (int j = 0; j < count; j++)
			{
				hOJhFTpGFkIeuGuGckkEoiyPlXuc hOJhFTpGFkIeuGuGckkEoiyPlXuc2 = PAALQWNwbfOPfHKpQyfsRvgBraMt.Get();
				hOJhFTpGFkIeuGuGckkEoiyPlXuc2.jmMYRbeXfukunOcevvuPZhAsWQGA(ZaSngQlSWxOuGaicCkHdwWQiFhhT[j]);
				CollectionTools.Enqueue(PAALQWNwbfOPfHKpQyfsRvgBraMt, MhacXUeUmstUFcCUlQxWNWErpYtQ, hOJhFTpGFkIeuGuGckkEoiyPlXuc2, out var _);
			}
			CollectionTools.Clear(PAALQWNwbfOPfHKpQyfsRvgBraMt, ZaSngQlSWxOuGaicCkHdwWQiFhhT);
		}

		public void kdURDcNpOQEtsbcuZgFYunCSmIzgb(float[] P_0, float P_1)
		{
			for (int i = 0; i < XRedVKCDXHEjYaWjcLRdNmsGRvLG.Length; i++)
			{
				XRedVKCDXHEjYaWjcLRdNmsGRvLG[i] += P_0[i];
			}
			hOJhFTpGFkIeuGuGckkEoiyPlXuc hOJhFTpGFkIeuGuGckkEoiyPlXuc2 = PAALQWNwbfOPfHKpQyfsRvgBraMt.Get();
			hOJhFTpGFkIeuGuGckkEoiyPlXuc2.XLmGvchAYnxnMXTHWdbmlqOqdtxeA(P_0, P_1);
			CollectionTools.Enqueue(PAALQWNwbfOPfHKpQyfsRvgBraMt, ZaSngQlSWxOuGaicCkHdwWQiFhhT, hOJhFTpGFkIeuGuGckkEoiyPlXuc2, out var _);
		}
	}

	public class hOJhFTpGFkIeuGuGckkEoiyPlXuc
	{
		public Vector3 ZCNwuekgJmAkwEDhmlrFhlleBLIy;

		public float pEjIWtERgNgCAQHSarniWniWPwXdb;

		public void XLmGvchAYnxnMXTHWdbmlqOqdtxeA(float[] P_0, float P_1)
		{
			int num = MathTools.Min(P_0.Length, 3);
			for (int i = 0; i < num; i++)
			{
				ZCNwuekgJmAkwEDhmlrFhlleBLIy[i] = P_0[i];
			}
			pEjIWtERgNgCAQHSarniWniWPwXdb = P_1;
		}

		public void jmMYRbeXfukunOcevvuPZhAsWQGA(hOJhFTpGFkIeuGuGckkEoiyPlXuc P_0)
		{
			ZCNwuekgJmAkwEDhmlrFhlleBLIy = P_0.ZCNwuekgJmAkwEDhmlrFhlleBLIy;
			pEjIWtERgNgCAQHSarniWniWPwXdb = P_0.pEjIWtERgNgCAQHSarniWniWPwXdb;
		}
	}

	public double sciYFsxEAKjhphJesnlwBawCfBDCB;

	public readonly float[] HViVbfIrDiBECuxWlqkhZscDKxn;

	public readonly int iTpGZghAGXrXxXzLlPVWVbuNgZkFA;

	private readonly byte[] JkmapcJtnoZTbNbMakFtPqdbfLibA;

	private readonly float[] ytsSrIWkgScpodrddlrTtnQlMPlIb;

	private readonly int ooBFTBPXUUGQHxhwiSVgFCJZiKvCA;

	private readonly int yRdwosthUHOANHUXpPJwdtxHoZuT;

	private readonly Action<byte[], float[]> lEkFPuWwBxkEwbcpheJouImOaPCO;

	private readonly Func<float> JCbYQAicieCPvYjWafgGghlgVNKF;

	public float[] OrthfcEpPRtmJfLlFdtCctIoezeQ => (wJDNgHtDIaBpUwHjXVHLEHnCDlvIA as QvNUGogGOMluZJeMtAkreXbkRbQw).cxzOSwLGHtLWXAuMqdsMgNqvdZdAA;

	public RingBuffer<hOJhFTpGFkIeuGuGckkEoiyPlXuc> garhibHNwyDACbuxuiOfayIWtbZD => (wJDNgHtDIaBpUwHjXVHLEHnCDlvIA as QvNUGogGOMluZJeMtAkreXbkRbQw).EnrWIrZCRwzKrhjFwFPNiDYdzRxMA;

	public zeduVYzSnJpVQGxDoGRFMdphEaCi(UpdateLoopSetting P_0, byte P_1, HIDInfo P_2, int P_3, int P_4, Action<byte[], float[]> P_5, Func<float> P_6)
		: base(new QvNUGogGOMluZJeMtAkreXbkRbQw(P_0, P_3, P_4), P_1, P_2)
	{
		iTpGZghAGXrXxXzLlPVWVbuNgZkFA = P_3;
		lEkFPuWwBxkEwbcpheJouImOaPCO = P_5;
		JCbYQAicieCPvYjWafgGghlgVNKF = P_6;
		ooBFTBPXUUGQHxhwiSVgFCJZiKvCA = ((P_2.bitSize > 0) ? ((P_2.bitSize + 8 - 1) / 8) : 0);
		yRdwosthUHOANHUXpPJwdtxHoZuT = P_2.dataIndex;
		JkmapcJtnoZTbNbMakFtPqdbfLibA = new byte[ooBFTBPXUUGQHxhwiSVgFCJZiKvCA];
		ytsSrIWkgScpodrddlrTtnQlMPlIb = new float[P_3];
		HViVbfIrDiBECuxWlqkhZscDKxn = new float[P_3];
	}

	public virtual void bMdgOogcNthUJwpwrXRRuDUvziEqA(NativeBuffer P_0, double P_1)
	{
		if (P_0 != null && P_0[0] == jSoHFXcXXwbGoxIhzdRXdkHeQAsb)
		{
			sciYFsxEAKjhphJesnlwBawCfBDCB = P_1;
			for (int i = 0; i < ooBFTBPXUUGQHxhwiSVgFCJZiKvCA; i++)
			{
				JkmapcJtnoZTbNbMakFtPqdbfLibA[i] = P_0[yRdwosthUHOANHUXpPJwdtxHoZuT + i];
			}
			if (lEkFPuWwBxkEwbcpheJouImOaPCO != null)
			{
				lEkFPuWwBxkEwbcpheJouImOaPCO(JkmapcJtnoZTbNbMakFtPqdbfLibA, ytsSrIWkgScpodrddlrTtnQlMPlIb);
			}
			float num = ((JCbYQAicieCPvYjWafgGghlgVNKF != null) ? JCbYQAicieCPvYjWafgGghlgVNKF() : 0f);
			(wJDNgHtDIaBpUwHjXVHLEHnCDlvIA as QvNUGogGOMluZJeMtAkreXbkRbQw).VuilXIEYaVZxnxYEVFPoAfskmmWg(ytsSrIWkgScpodrddlrTtnQlMPlIb, num);
			for (int j = 0; j < iTpGZghAGXrXxXzLlPVWVbuNgZkFA; j++)
			{
				HViVbfIrDiBECuxWlqkhZscDKxn[j] = ytsSrIWkgScpodrddlrTtnQlMPlIb[j];
			}
		}
	}
}
