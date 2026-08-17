using System;
using Rewired.Config;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;
using UnityEngine;

namespace Rewired.HID
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal class HIDGyroscope : HIDControllerElementWithDataSet
	{
		internal class OQiOFpnPtjXvpNWVHDFBSGTKMNKD : kGaQcrZjSXfDykZKCSLIIAsSOyoLA
		{
			private int TJXDJnDZdrVtxlVDYZNakPPtjlmw;

			private int FXygRPjBHulImYLuipCtAeXZJpzKA;

			public float[] XMLhFdSuDtJBozGJAApmtuOmajzY => (SgJVPHrjFyHgCSEpxtgUijWhNNop as dtXZnvXaPDaeobvPuoEIeMSANOKXB).XMLhFdSuDtJBozGJAApmtuOmajzY;

			public ExpandableArray_DataContainer<vumlNYaXVVyzQKQsUZwqOQVbUSLf> hLXLQsserweYhaojhNuEdEhHYDYFb => (SgJVPHrjFyHgCSEpxtgUijWhNNop as dtXZnvXaPDaeobvPuoEIeMSANOKXB).hLXLQsserweYhaojhNuEdEhHYDYFb;

			public OQiOFpnPtjXvpNWVHDFBSGTKMNKD(UpdateLoopSetting P_0, int P_1, int P_2)
			{
				TJXDJnDZdrVtxlVDYZNakPPtjlmw = P_1;
				FXygRPjBHulImYLuipCtAeXZJpzKA = P_2;
				tZBbwtEuDoJAugBSKQuoLXJXFmdBA(P_0, SbLfxtBNmTGyHepGdcqnKKIBADKYA);
			}

			public override void jRaYtHNVcykNMAbqOnSGaKIIGSEaA(UpdateLoopType P_0)
			{
				base.jRaYtHNVcykNMAbqOnSGaKIIGSEaA(P_0);
				(SgJVPHrjFyHgCSEpxtgUijWhNNop as dtXZnvXaPDaeobvPuoEIeMSANOKXB).jRaYtHNVcykNMAbqOnSGaKIIGSEaA();
			}

			public void zmXnXZgJmRPZXYrtqsPNHBAYETgs(float[] P_0, float P_1)
			{
				for (int i = 0; i < ReyFXLxUNcPvXNGMfpODOqFiLRfR.Length; i++)
				{
					(ReyFXLxUNcPvXNGMfpODOqFiLRfR[i] as dtXZnvXaPDaeobvPuoEIeMSANOKXB).eetDdHJxiOoZwkumQkfHIcRqxUXDA(P_0, P_1);
				}
			}

			private OlubodalIgGxzKDJlbCWXQONFDog SbLfxtBNmTGyHepGdcqnKKIBADKYA(UpdateLoopType P_0)
			{
				return new dtXZnvXaPDaeobvPuoEIeMSANOKXB(P_0, TJXDJnDZdrVtxlVDYZNakPPtjlmw, FXygRPjBHulImYLuipCtAeXZJpzKA);
			}
		}

		internal class dtXZnvXaPDaeobvPuoEIeMSANOKXB : OlubodalIgGxzKDJlbCWXQONFDog
		{
			private float[] gehpjCOClIDWIIzXxEkjolsVeaBG;

			public float[] XMLhFdSuDtJBozGJAApmtuOmajzY;

			public ExpandableArray_DataContainer<vumlNYaXVVyzQKQsUZwqOQVbUSLf> hLXLQsserweYhaojhNuEdEhHYDYFb;

			private ExpandableArray_DataContainer<vumlNYaXVVyzQKQsUZwqOQVbUSLf> PbBnVRyFWfOgOTOTtUtREnHXRlze;

			public dtXZnvXaPDaeobvPuoEIeMSANOKXB(UpdateLoopType P_0, int P_1, int P_2)
				: base(P_0)
			{
				XMLhFdSuDtJBozGJAApmtuOmajzY = new float[P_1];
				gehpjCOClIDWIIzXxEkjolsVeaBG = new float[P_1];
				hLXLQsserweYhaojhNuEdEhHYDYFb = new ExpandableArray_DataContainer<vumlNYaXVVyzQKQsUZwqOQVbUSLf>(P_2, false, 20);
				PbBnVRyFWfOgOTOTtUtREnHXRlze = new ExpandableArray_DataContainer<vumlNYaXVVyzQKQsUZwqOQVbUSLf>(P_2, false, 20);
			}

			public void jRaYtHNVcykNMAbqOnSGaKIIGSEaA()
			{
				for (int i = 0; i < gehpjCOClIDWIIzXxEkjolsVeaBG.Length; i++)
				{
					XMLhFdSuDtJBozGJAApmtuOmajzY[i] = gehpjCOClIDWIIzXxEkjolsVeaBG[i];
					gehpjCOClIDWIIzXxEkjolsVeaBG[i] = 0f;
				}
				hLXLQsserweYhaojhNuEdEhHYDYFb.Clear();
				int count = PbBnVRyFWfOgOTOTtUtREnHXRlze.Count;
				for (int j = 0; j < count; j++)
				{
					hLXLQsserweYhaojhNuEdEhHYDYFb.AddData(PbBnVRyFWfOgOTOTtUtREnHXRlze[j]);
				}
				PbBnVRyFWfOgOTOTtUtREnHXRlze.Clear();
			}

			public void eetDdHJxiOoZwkumQkfHIcRqxUXDA(float[] P_0, float P_1)
			{
				for (int i = 0; i < gehpjCOClIDWIIzXxEkjolsVeaBG.Length; i++)
				{
					gehpjCOClIDWIIzXxEkjolsVeaBG[i] += P_0[i];
				}
				PbBnVRyFWfOgOTOTtUtREnHXRlze.injector.MPIdpLqhJeCJHWkSZORdXwngeGgr(P_0, P_1);
				PbBnVRyFWfOgOTOTtUtREnHXRlze.Inject();
			}

			public override void jpwugzufXqktYbXkMYboQpqCbQgL()
			{
				Array.Clear(XMLhFdSuDtJBozGJAApmtuOmajzY, 0, XMLhFdSuDtJBozGJAApmtuOmajzY.Length);
				PbBnVRyFWfOgOTOTtUtREnHXRlze.Clear();
				hLXLQsserweYhaojhNuEdEhHYDYFb.Clear();
			}
		}

		public class vumlNYaXVVyzQKQsUZwqOQVbUSLf : ExpandableArray_DataContainer<vumlNYaXVVyzQKQsUZwqOQVbUSLf>.qARAlhhogsFTixiCVGpIfnVauIuJb, IComparable<vumlNYaXVVyzQKQsUZwqOQVbUSLf>
		{
			public Vector3 XMLhFdSuDtJBozGJAApmtuOmajzY;

			public float cXGCxvAxxQZVPCFzsmouaVkAGZKV;

			public vumlNYaXVVyzQKQsUZwqOQVbUSLf()
			{
			}

			public vumlNYaXVVyzQKQsUZwqOQVbUSLf(float[] P_0, float P_1)
			{
				MPIdpLqhJeCJHWkSZORdXwngeGgr(P_0, P_1);
			}

			public void MPIdpLqhJeCJHWkSZORdXwngeGgr(float[] P_0, float P_1)
			{
				int num = MathTools.Min(P_0.Length, 3);
				for (int i = 0; i < num; i++)
				{
					XMLhFdSuDtJBozGJAApmtuOmajzY[i] = P_0[i];
				}
				cXGCxvAxxQZVPCFzsmouaVkAGZKV = P_1;
			}

			public void Set(vumlNYaXVVyzQKQsUZwqOQVbUSLf P_0)
			{
				XMLhFdSuDtJBozGJAApmtuOmajzY = P_0.XMLhFdSuDtJBozGJAApmtuOmajzY;
				cXGCxvAxxQZVPCFzsmouaVkAGZKV = P_0.cXGCxvAxxQZVPCFzsmouaVkAGZKV;
			}

			public bool Equals(vumlNYaXVVyzQKQsUZwqOQVbUSLf P_0)
			{
				if (cXGCxvAxxQZVPCFzsmouaVkAGZKV == P_0.cXGCxvAxxQZVPCFzsmouaVkAGZKV)
				{
					return XMLhFdSuDtJBozGJAApmtuOmajzY == P_0.XMLhFdSuDtJBozGJAApmtuOmajzY;
				}
				return false;
			}

			public void Clear()
			{
				XMLhFdSuDtJBozGJAApmtuOmajzY.x = 0f;
				XMLhFdSuDtJBozGJAApmtuOmajzY.y = 0f;
				XMLhFdSuDtJBozGJAApmtuOmajzY.z = 0f;
				cXGCxvAxxQZVPCFzsmouaVkAGZKV = 0f;
			}

			public int CompareTo(vumlNYaXVVyzQKQsUZwqOQVbUSLf other)
			{
				return 0;
			}
		}

		public double timestamp;

		public readonly float[] lastRawValue;

		public readonly int valueLength;

		private readonly byte[] RqatsqOAFuCjvKgyqSwebaixwnjRA;

		private readonly float[] bYDtMMbooEWmfouaClXfNvsUEHRD;

		private readonly int EjWOnLLPZYKdKTVZgswlsDqWiQFH;

		private readonly int UYulFtHjcgukgspsqfJyRAXnnmIV;

		private readonly Action<byte[], float[]> yYsIzhEAunEeadgbIpStWaJjrpBe;

		private readonly Func<float> vdJWvgvIeImBMGjneTTDOtNTauaj;

		public float[] rawValue => (dataSet as OQiOFpnPtjXvpNWVHDFBSGTKMNKD).XMLhFdSuDtJBozGJAApmtuOmajzY;

		public ExpandableArray_DataContainer<vumlNYaXVVyzQKQsUZwqOQVbUSLf> events => (dataSet as OQiOFpnPtjXvpNWVHDFBSGTKMNKD).hLXLQsserweYhaojhNuEdEhHYDYFb;

		public HIDGyroscope(UpdateLoopSetting P_0, byte P_1, HIDInfo P_2, int P_3, int P_4, Action<byte[], float[]> P_5, Func<float> P_6)
			: base(new OQiOFpnPtjXvpNWVHDFBSGTKMNKD(P_0, P_3, P_4), P_1, P_2)
		{
			valueLength = P_3;
			yYsIzhEAunEeadgbIpStWaJjrpBe = P_5;
			vdJWvgvIeImBMGjneTTDOtNTauaj = P_6;
			EjWOnLLPZYKdKTVZgswlsDqWiQFH = ((P_2.bitSize > 0) ? ((P_2.bitSize + 8 - 1) / 8) : 0);
			UYulFtHjcgukgspsqfJyRAXnnmIV = P_2.dataIndex;
			RqatsqOAFuCjvKgyqSwebaixwnjRA = new byte[EjWOnLLPZYKdKTVZgswlsDqWiQFH];
			bYDtMMbooEWmfouaClXfNvsUEHRD = new float[P_3];
			lastRawValue = new float[P_3];
		}

		public override void UpdateValue(NativeBuffer inputReport, double timestamp)
		{
			if (inputReport != null && inputReport[0] == reportId)
			{
				this.timestamp = timestamp;
				for (int i = 0; i < EjWOnLLPZYKdKTVZgswlsDqWiQFH; i++)
				{
					RqatsqOAFuCjvKgyqSwebaixwnjRA[i] = inputReport[UYulFtHjcgukgspsqfJyRAXnnmIV + i];
				}
				if (yYsIzhEAunEeadgbIpStWaJjrpBe != null)
				{
					yYsIzhEAunEeadgbIpStWaJjrpBe(RqatsqOAFuCjvKgyqSwebaixwnjRA, bYDtMMbooEWmfouaClXfNvsUEHRD);
				}
				float num = ((vdJWvgvIeImBMGjneTTDOtNTauaj != null) ? vdJWvgvIeImBMGjneTTDOtNTauaj() : 0f);
				(dataSet as OQiOFpnPtjXvpNWVHDFBSGTKMNKD).zmXnXZgJmRPZXYrtqsPNHBAYETgs(bYDtMMbooEWmfouaClXfNvsUEHRD, num);
				for (int j = 0; j < valueLength; j++)
				{
					lastRawValue[j] = bYDtMMbooEWmfouaClXfNvsUEHRD[j];
				}
			}
		}

		public void UpdateValueManual(float[] value, double timestamp)
		{
			this.timestamp = timestamp;
			float num = ((vdJWvgvIeImBMGjneTTDOtNTauaj != null) ? vdJWvgvIeImBMGjneTTDOtNTauaj() : 0f);
			for (int i = 0; i < valueLength; i++)
			{
				bYDtMMbooEWmfouaClXfNvsUEHRD[i] = value[i];
			}
			(dataSet as OQiOFpnPtjXvpNWVHDFBSGTKMNKD).zmXnXZgJmRPZXYrtqsPNHBAYETgs(bYDtMMbooEWmfouaClXfNvsUEHRD, num);
			for (int j = 0; j < valueLength; j++)
			{
				lastRawValue[j] = bYDtMMbooEWmfouaClXfNvsUEHRD[j];
			}
		}
	}
}
