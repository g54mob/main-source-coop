using System;
using System.Collections.Generic;
using Rewired;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;

internal class gTyYgmXURpDluqEDVAzYahaASSNCb
{
	public class VWDvlCaaRKVAiVecuglWeVUYDEJQA
	{
		public readonly Action<InputActionEventData> gybqTscDHxcHontLfEbdWinPeCkKA;

		public readonly UpdateLoopType PiEdbgjMHSsksYjRKSGckYZEsfAE;

		public readonly InputActionEventType SxXbiAFDmpBnSaGAITcEBXyWMRDoc;

		public readonly int tlJxlOEWqQEUYpNGSwfnmndnnqSe;

		public readonly bool PHzVSdoZDkWBbZsHreJZyFBOlRY;

		public float[] ektmnULWTfCOHOGjepyUxsNkhsTX;

		public VWDvlCaaRKVAiVecuglWeVUYDEJQA(Action<InputActionEventData> P_0, UpdateLoopType P_1, InputActionEventType P_2, int P_3, object[] P_4)
		{
			PiEdbgjMHSsksYjRKSGckYZEsfAE = P_1;
			SxXbiAFDmpBnSaGAITcEBXyWMRDoc = P_2;
			tlJxlOEWqQEUYpNGSwfnmndnnqSe = P_3;
			gybqTscDHxcHontLfEbdWinPeCkKA = P_0;
			WhwByCkfMeCrJQbBIgAWkBySwSAs(P_4);
			switch (P_2)
			{
			case InputActionEventType.Update:
			case InputActionEventType.ButtonUnpressed:
			case InputActionEventType.NegativeButtonUnpressed:
			case InputActionEventType.AxisInactive:
			case InputActionEventType.AxisRawInactive:
				PHzVSdoZDkWBbZsHreJZyFBOlRY = true;
				break;
			}
		}

		public bool gIHWLPTAPlydTRisReXVWULhQkST(int P_0, out float P_1)
		{
			if (ektmnULWTfCOHOGjepyUxsNkhsTX == null || ektmnULWTfCOHOGjepyUxsNkhsTX.Length <= P_0)
			{
				P_1 = 0f;
				return false;
			}
			P_1 = ektmnULWTfCOHOGjepyUxsNkhsTX[P_0];
			return true;
		}

		private void WhwByCkfMeCrJQbBIgAWkBySwSAs(object[] P_0)
		{
			switch (SxXbiAFDmpBnSaGAITcEBXyWMRDoc)
			{
			case InputActionEventType.ButtonPressedForTime:
			case InputActionEventType.ButtonPressedForTimeJustReleased:
			case InputActionEventType.NegativeButtonPressedForTime:
			case InputActionEventType.NegativeButtonPressedForTimeJustReleased:
				if (P_0 == null || P_0.Length < 1)
				{
					throw new Exception("Wrong number of arguments passed for Input event type \"" + SxXbiAFDmpBnSaGAITcEBXyWMRDoc.ToString() + "\". 1 required argument: time [float], 1 optional argument: expireIn [float]");
				}
				ektmnULWTfCOHOGjepyUxsNkhsTX = new float[2];
				if (P_0[0] is float)
				{
					ektmnULWTfCOHOGjepyUxsNkhsTX[0] = (float)P_0[0];
				}
				else
				{
					if (!(P_0[0] is int))
					{
						throw new Exception("Wrong argument type passed for Input event type \"" + SxXbiAFDmpBnSaGAITcEBXyWMRDoc.ToString() + "\". Argument 0: time [float]");
					}
					ektmnULWTfCOHOGjepyUxsNkhsTX[0] = (int)P_0[0];
				}
				if (P_0.Length <= 1)
				{
					break;
				}
				if (P_0[1] is float)
				{
					ektmnULWTfCOHOGjepyUxsNkhsTX[1] = (float)P_0[1];
					break;
				}
				if (P_0[1] is int)
				{
					ektmnULWTfCOHOGjepyUxsNkhsTX[1] = (int)P_0[1];
					break;
				}
				throw new Exception("Wrong argument type passed for Input event type \"" + SxXbiAFDmpBnSaGAITcEBXyWMRDoc.ToString() + "\". Argument 1 (optional): expireIn [float]");
			case InputActionEventType.ButtonJustPressedForTime:
			case InputActionEventType.NegativeButtonJustPressedForTime:
				if (P_0 == null || P_0.Length < 1)
				{
					throw new Exception("Wrong number of arguments passed for Input event type \"" + SxXbiAFDmpBnSaGAITcEBXyWMRDoc.ToString() + "\". Requires 1 argument: time [float]");
				}
				ektmnULWTfCOHOGjepyUxsNkhsTX = new float[1];
				if (P_0[0] is float)
				{
					ektmnULWTfCOHOGjepyUxsNkhsTX[0] = (float)P_0[0];
					break;
				}
				if (P_0[0] is int)
				{
					ektmnULWTfCOHOGjepyUxsNkhsTX[0] = (int)P_0[0];
					break;
				}
				throw new Exception("Wrong argument type passed for Input event type \"" + SxXbiAFDmpBnSaGAITcEBXyWMRDoc.ToString() + "\". Argument 0: time [float]");
			case InputActionEventType.ButtonDoublePressed:
			case InputActionEventType.ButtonJustDoublePressed:
			case InputActionEventType.NegativeButtonDoublePressed:
			case InputActionEventType.NegativeButtonJustDoublePressed:
			case InputActionEventType.ButtonDoublePressJustReleased:
			case InputActionEventType.NegativeButtonDoublePressJustReleased:
				if (P_0 == null || P_0.Length < 1)
				{
					break;
				}
				ektmnULWTfCOHOGjepyUxsNkhsTX = new float[1];
				if (P_0[0] is float)
				{
					ektmnULWTfCOHOGjepyUxsNkhsTX[0] = (float)P_0[0];
					break;
				}
				if (P_0[0] is int)
				{
					ektmnULWTfCOHOGjepyUxsNkhsTX[0] = (int)P_0[0];
					break;
				}
				throw new Exception("Wrong argument type passed for Input event type \"" + SxXbiAFDmpBnSaGAITcEBXyWMRDoc.ToString() + "\". Argument 0 (optional): time [float]");
			}
		}
	}

	[Serializable]
	private sealed class EcwKgbMIvkHWlmVXdbHLtRWlWisK
	{
		public static readonly EcwKgbMIvkHWlmVXdbHLtRWlWisK _003C_003E9 = new EcwKgbMIvkHWlmVXdbHLtRWlWisK();

		public static Func<AList<VWDvlCaaRKVAiVecuglWeVUYDEJQA>> _003C_003E9__8_0;

		internal AList<VWDvlCaaRKVAiVecuglWeVUYDEJQA> CfsJkClZBySTayzyggTlXRjbYorA()
		{
			return new AList<VWDvlCaaRKVAiVecuglWeVUYDEJQA>();
		}
	}

	private sealed class BqFrUOuWsvkUeJXpXxVPpAijCgIy
	{
		public Action<InputActionEventData> gybqTscDHxcHontLfEbdWinPeCkKA;

		public Predicate<VWDvlCaaRKVAiVecuglWeVUYDEJQA> YxULXwVloCHLajUKCDYbiYunfKGj;

		internal bool klwyyOuVQNelShchnczczlEDelrS(VWDvlCaaRKVAiVecuglWeVUYDEJQA P_0)
		{
			return P_0.gybqTscDHxcHontLfEbdWinPeCkKA == gybqTscDHxcHontLfEbdWinPeCkKA;
		}
	}

	private sealed class oXGBLnntgosJHECwitDKOhJpgarHA
	{
		public Action<InputActionEventData> gybqTscDHxcHontLfEbdWinPeCkKA;

		public int tlJxlOEWqQEUYpNGSwfnmndnnqSe;

		public Predicate<VWDvlCaaRKVAiVecuglWeVUYDEJQA> YxULXwVloCHLajUKCDYbiYunfKGj;

		internal bool klwyyOuVQNelShchnczczlEDelrS(VWDvlCaaRKVAiVecuglWeVUYDEJQA P_0)
		{
			if (P_0.gybqTscDHxcHontLfEbdWinPeCkKA == gybqTscDHxcHontLfEbdWinPeCkKA)
			{
				return P_0.tlJxlOEWqQEUYpNGSwfnmndnnqSe == tlJxlOEWqQEUYpNGSwfnmndnnqSe;
			}
			return false;
		}
	}

	private sealed class DbhfxHlYQhDQcsMrSzKWgnMBxlad
	{
		public Action<InputActionEventData> gybqTscDHxcHontLfEbdWinPeCkKA;

		public UpdateLoopType PiEdbgjMHSsksYjRKSGckYZEsfAE;

		public Predicate<VWDvlCaaRKVAiVecuglWeVUYDEJQA> YxULXwVloCHLajUKCDYbiYunfKGj;

		internal bool klwyyOuVQNelShchnczczlEDelrS(VWDvlCaaRKVAiVecuglWeVUYDEJQA P_0)
		{
			if (P_0.gybqTscDHxcHontLfEbdWinPeCkKA == gybqTscDHxcHontLfEbdWinPeCkKA)
			{
				return P_0.PiEdbgjMHSsksYjRKSGckYZEsfAE == PiEdbgjMHSsksYjRKSGckYZEsfAE;
			}
			return false;
		}
	}

	private sealed class FyFRoaJQlsSHoQGbBiiMGcebGAZhb
	{
		public Action<InputActionEventData> gybqTscDHxcHontLfEbdWinPeCkKA;

		public InputActionEventType SxXbiAFDmpBnSaGAITcEBXyWMRDoc;

		public Predicate<VWDvlCaaRKVAiVecuglWeVUYDEJQA> YxULXwVloCHLajUKCDYbiYunfKGj;

		internal bool klwyyOuVQNelShchnczczlEDelrS(VWDvlCaaRKVAiVecuglWeVUYDEJQA P_0)
		{
			if (P_0.gybqTscDHxcHontLfEbdWinPeCkKA == gybqTscDHxcHontLfEbdWinPeCkKA)
			{
				return P_0.SxXbiAFDmpBnSaGAITcEBXyWMRDoc == SxXbiAFDmpBnSaGAITcEBXyWMRDoc;
			}
			return false;
		}
	}

	private sealed class ZxAxeGRdYOfvIHNLGKdfleqoChxT
	{
		public Action<InputActionEventData> gybqTscDHxcHontLfEbdWinPeCkKA;

		public UpdateLoopType PiEdbgjMHSsksYjRKSGckYZEsfAE;

		public int tlJxlOEWqQEUYpNGSwfnmndnnqSe;

		public Predicate<VWDvlCaaRKVAiVecuglWeVUYDEJQA> YxULXwVloCHLajUKCDYbiYunfKGj;

		internal bool klwyyOuVQNelShchnczczlEDelrS(VWDvlCaaRKVAiVecuglWeVUYDEJQA P_0)
		{
			if (P_0.gybqTscDHxcHontLfEbdWinPeCkKA == gybqTscDHxcHontLfEbdWinPeCkKA && P_0.PiEdbgjMHSsksYjRKSGckYZEsfAE == PiEdbgjMHSsksYjRKSGckYZEsfAE)
			{
				return P_0.tlJxlOEWqQEUYpNGSwfnmndnnqSe == tlJxlOEWqQEUYpNGSwfnmndnnqSe;
			}
			return false;
		}
	}

	private sealed class CkZGVoGdfbWrLqXqicPFHUTcTxmeB
	{
		public Action<InputActionEventData> gybqTscDHxcHontLfEbdWinPeCkKA;

		public UpdateLoopType PiEdbgjMHSsksYjRKSGckYZEsfAE;

		public int tlJxlOEWqQEUYpNGSwfnmndnnqSe;

		public InputActionEventType SxXbiAFDmpBnSaGAITcEBXyWMRDoc;

		public Predicate<VWDvlCaaRKVAiVecuglWeVUYDEJQA> YxULXwVloCHLajUKCDYbiYunfKGj;

		internal bool klwyyOuVQNelShchnczczlEDelrS(VWDvlCaaRKVAiVecuglWeVUYDEJQA P_0)
		{
			if (P_0.gybqTscDHxcHontLfEbdWinPeCkKA == gybqTscDHxcHontLfEbdWinPeCkKA && P_0.PiEdbgjMHSsksYjRKSGckYZEsfAE == PiEdbgjMHSsksYjRKSGckYZEsfAE && P_0.tlJxlOEWqQEUYpNGSwfnmndnnqSe == tlJxlOEWqQEUYpNGSwfnmndnnqSe)
			{
				return P_0.SxXbiAFDmpBnSaGAITcEBXyWMRDoc == SxXbiAFDmpBnSaGAITcEBXyWMRDoc;
			}
			return false;
		}
	}

	private sealed class CJypnXgasolFlPoXCZDdEHKTrXSb
	{
		public Action<InputActionEventData> gybqTscDHxcHontLfEbdWinPeCkKA;

		public UpdateLoopType PiEdbgjMHSsksYjRKSGckYZEsfAE;

		public InputActionEventType SxXbiAFDmpBnSaGAITcEBXyWMRDoc;

		public Predicate<VWDvlCaaRKVAiVecuglWeVUYDEJQA> YxULXwVloCHLajUKCDYbiYunfKGj;

		internal bool klwyyOuVQNelShchnczczlEDelrS(VWDvlCaaRKVAiVecuglWeVUYDEJQA P_0)
		{
			if (P_0.gybqTscDHxcHontLfEbdWinPeCkKA == gybqTscDHxcHontLfEbdWinPeCkKA && P_0.PiEdbgjMHSsksYjRKSGckYZEsfAE == PiEdbgjMHSsksYjRKSGckYZEsfAE)
			{
				return P_0.SxXbiAFDmpBnSaGAITcEBXyWMRDoc == SxXbiAFDmpBnSaGAITcEBXyWMRDoc;
			}
			return false;
		}
	}

	private sealed class ZemZSREDfVvwtInwNOQYtQOznsbB
	{
		public Action<InputActionEventData> gybqTscDHxcHontLfEbdWinPeCkKA;

		public int tlJxlOEWqQEUYpNGSwfnmndnnqSe;

		public InputActionEventType SxXbiAFDmpBnSaGAITcEBXyWMRDoc;

		public Predicate<VWDvlCaaRKVAiVecuglWeVUYDEJQA> YxULXwVloCHLajUKCDYbiYunfKGj;

		internal bool klwyyOuVQNelShchnczczlEDelrS(VWDvlCaaRKVAiVecuglWeVUYDEJQA P_0)
		{
			if (P_0.gybqTscDHxcHontLfEbdWinPeCkKA == gybqTscDHxcHontLfEbdWinPeCkKA && P_0.tlJxlOEWqQEUYpNGSwfnmndnnqSe == tlJxlOEWqQEUYpNGSwfnmndnnqSe)
			{
				return P_0.SxXbiAFDmpBnSaGAITcEBXyWMRDoc == SxXbiAFDmpBnSaGAITcEBXyWMRDoc;
			}
			return false;
		}
	}

	private static VWDvlCaaRKVAiVecuglWeVUYDEJQA[] KbBYeHGOWXuEEzZlNjsVlDnTdejbA;

	private bool jYTNgflwwEgvgbZuuTHYnPAnuRao;

	private AList<VWDvlCaaRKVAiVecuglWeVUYDEJQA>[] kBhbzMbozDMqWbKbyexTTMCFftHyA;

	private int[] fatnwegVNllKcXFFVtHhQPynhCA;

	private int NBoCwlKPKYgAYEOhsJEUoLGjxcutA;

	public int VulcQkzEFSldNXBvZXTerjMDDble;

	static gTyYgmXURpDluqEDVAzYahaASSNCb()
	{
		KbBYeHGOWXuEEzZlNjsVlDnTdejbA = new VWDvlCaaRKVAiVecuglWeVUYDEJQA[100];
	}

	private void zQQfvDZMmpVqPPLYlLuSJXXpwJcI()
	{
		if (!jYTNgflwwEgvgbZuuTHYnPAnuRao)
		{
			IList<InputAction> list = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.pKMZFxQiAHeZkCfHZqCmwzClFZlQ;
			int num = list?.Count ?? 0;
			kBhbzMbozDMqWbKbyexTTMCFftHyA = new AList<VWDvlCaaRKVAiVecuglWeVUYDEJQA>[num + 1];
			fatnwegVNllKcXFFVtHhQPynhCA = new int[ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.cuIqMUSdmKIejUTXChjhBrEkVSBe + 1];
			ArrayTools.Populate(kBhbzMbozDMqWbKbyexTTMCFftHyA, 0, kBhbzMbozDMqWbKbyexTTMCFftHyA.Length, EcwKgbMIvkHWlmVXdbHLtRWlWisK._003C_003E9.CfsJkClZBySTayzyggTlXRjbYorA);
			for (int i = 0; i < num; i++)
			{
				fatnwegVNllKcXFFVtHhQPynhCA[list[i].id] = i;
			}
			NBoCwlKPKYgAYEOhsJEUoLGjxcutA = num;
			jYTNgflwwEgvgbZuuTHYnPAnuRao = true;
		}
	}

	public void mAMBMpqTbYtzNRCEtBamaAccLfJGb(ItaEjuKtoATtafYloiocFOanqPQc P_0, UpdateLoopType P_1)
	{
		AList<VWDvlCaaRKVAiVecuglWeVUYDEJQA> aList = kBhbzMbozDMqWbKbyexTTMCFftHyA[fatnwegVNllKcXFFVtHhQPynhCA[P_0.JwOchbdvhsvHVdSyYjtqEJdTZzYG]];
		for (int i = 0; i < 2; i++)
		{
			if (i == 1)
			{
				aList = kBhbzMbozDMqWbKbyexTTMCFftHyA[NBoCwlKPKYgAYEOhsJEUoLGjxcutA];
			}
			int count = aList._count;
			if (KbBYeHGOWXuEEzZlNjsVlDnTdejbA.Length < count)
			{
				KbBYeHGOWXuEEzZlNjsVlDnTdejbA = new VWDvlCaaRKVAiVecuglWeVUYDEJQA[count + 50];
			}
			if (count > 0)
			{
				Array.Copy(aList._items, KbBYeHGOWXuEEzZlNjsVlDnTdejbA, count);
			}
			for (int j = 0; j < count; j++)
			{
				VWDvlCaaRKVAiVecuglWeVUYDEJQA vWDvlCaaRKVAiVecuglWeVUYDEJQA = KbBYeHGOWXuEEzZlNjsVlDnTdejbA[j];
				if (vWDvlCaaRKVAiVecuglWeVUYDEJQA == null || (!P_0.ZVIoGtYvPAqctbduSqUnwksmgLtq && !vWDvlCaaRKVAiVecuglWeVUYDEJQA.PHzVSdoZDkWBbZsHreJZyFBOlRY) || vWDvlCaaRKVAiVecuglWeVUYDEJQA.PiEdbgjMHSsksYjRKSGckYZEsfAE != P_1 || (vWDvlCaaRKVAiVecuglWeVUYDEJQA.tlJxlOEWqQEUYpNGSwfnmndnnqSe >= 0 && vWDvlCaaRKVAiVecuglWeVUYDEJQA.tlJxlOEWqQEUYpNGSwfnmndnnqSe != P_0.JwOchbdvhsvHVdSyYjtqEJdTZzYG))
				{
					continue;
				}
				bool flag = false;
				switch (vWDvlCaaRKVAiVecuglWeVUYDEJQA.SxXbiAFDmpBnSaGAITcEBXyWMRDoc)
				{
				case InputActionEventType.Update:
					flag = true;
					break;
				case InputActionEventType.ButtonPressed:
					if (P_0.IKCLDPTiIKTdmSlZPgUWaOkbUbhVA())
					{
						flag = true;
					}
					break;
				case InputActionEventType.ButtonUnpressed:
					if (!P_0.IKCLDPTiIKTdmSlZPgUWaOkbUbhVA())
					{
						flag = true;
					}
					break;
				case InputActionEventType.ButtonDoublePressed:
				{
					vWDvlCaaRKVAiVecuglWeVUYDEJQA.gIHWLPTAPlydTRisReXVWULhQkST(0, out var num5);
					if (P_0.DMfqNDOKfUnRtMPPbvMgsnegbJKE(num5))
					{
						flag = true;
					}
					break;
				}
				case InputActionEventType.ButtonPressedForTime:
				{
					if (!vWDvlCaaRKVAiVecuglWeVUYDEJQA.gIHWLPTAPlydTRisReXVWULhQkST(0, out var num11))
					{
						continue;
					}
					vWDvlCaaRKVAiVecuglWeVUYDEJQA.gIHWLPTAPlydTRisReXVWULhQkST(1, out var num12);
					if (P_0.TnGUUrcnnYBlhZVNcnTESFJZIRex(num11, num12))
					{
						flag = true;
					}
					break;
				}
				case InputActionEventType.ButtonShortPressed:
					if (P_0.QrEesxjbxPBEfHodumPCacAeRLgnb())
					{
						flag = true;
					}
					break;
				case InputActionEventType.ButtonLongPressed:
					if (P_0.MaDlmiKbebbDojyriMusfaKkBxLKA())
					{
						flag = true;
					}
					break;
				case InputActionEventType.ButtonJustPressed:
					if (P_0.LbzwmtxTiWvoFmsIMWGeWeOGjmIg())
					{
						flag = true;
					}
					break;
				case InputActionEventType.ButtonJustReleased:
					if (P_0.vJpqJdBUhGAMATiWyyWbrqfEhKUg())
					{
						flag = true;
					}
					break;
				case InputActionEventType.ButtonJustDoublePressed:
				{
					vWDvlCaaRKVAiVecuglWeVUYDEJQA.gIHWLPTAPlydTRisReXVWULhQkST(0, out var num9);
					if (P_0.fkcDkMJQbeWOyLvgzpIFQvQPiSWm(num9))
					{
						flag = true;
					}
					break;
				}
				case InputActionEventType.ButtonDoublePressJustReleased:
				{
					vWDvlCaaRKVAiVecuglWeVUYDEJQA.gIHWLPTAPlydTRisReXVWULhQkST(0, out var num6);
					if (P_0.mAykGDDTNEaZGawmdkTlpUjLjYun(num6))
					{
						flag = true;
					}
					break;
				}
				case InputActionEventType.ButtonJustPressedForTime:
				{
					if (!vWDvlCaaRKVAiVecuglWeVUYDEJQA.gIHWLPTAPlydTRisReXVWULhQkST(0, out var num4))
					{
						continue;
					}
					if (P_0.NjkdPCGmRZCYclxqltumoTakkgJRA(num4))
					{
						flag = true;
					}
					break;
				}
				case InputActionEventType.ButtonJustShortPressed:
					if (P_0.JrIKHzCKgQisRHAHGkvPbqOwnpsW())
					{
						flag = true;
					}
					break;
				case InputActionEventType.ButtonJustLongPressed:
					if (P_0.DlLZnBghBYhLlitiiJWbinuIGoTTA())
					{
						flag = true;
					}
					break;
				case InputActionEventType.ButtonPressedForTimeJustReleased:
				{
					if (!vWDvlCaaRKVAiVecuglWeVUYDEJQA.gIHWLPTAPlydTRisReXVWULhQkST(0, out var num15))
					{
						continue;
					}
					vWDvlCaaRKVAiVecuglWeVUYDEJQA.gIHWLPTAPlydTRisReXVWULhQkST(1, out var num16);
					if (P_0.SEnFNGPhTWbIpEEqDBfjyYToighH(num15, num16))
					{
						flag = true;
					}
					break;
				}
				case InputActionEventType.ButtonShortPressJustReleased:
					if (P_0.dzkeHBLJOMxVNWjvzhdVJnJMvJIr())
					{
						flag = true;
					}
					break;
				case InputActionEventType.ButtonLongPressJustReleased:
					if (P_0.VbUHFmEFVWLsPBcjQQphILbxpWgK())
					{
						flag = true;
					}
					break;
				case InputActionEventType.ButtonRepeating:
					if (P_0.eaOxZJjpbGEIJYhkaADdLNNobROD())
					{
						flag = true;
					}
					break;
				case InputActionEventType.ButtonSinglePressed:
					if (P_0.JEUbHmDevitEshAxNQgiDYvWQFQaA())
					{
						flag = true;
					}
					break;
				case InputActionEventType.ButtonJustSinglePressed:
					if (P_0.WuinqRKRejufNxAUlwSDdnuZjRHO())
					{
						flag = true;
					}
					break;
				case InputActionEventType.ButtonSinglePressJustReleased:
					if (P_0.DoAgFePMYTgosHbxmRcYfEyLjZdK())
					{
						flag = true;
					}
					break;
				case InputActionEventType.NegativeButtonPressed:
					if (P_0.tfsYXedjKMNWjMtDSqWFOsogLsAD())
					{
						flag = true;
					}
					break;
				case InputActionEventType.NegativeButtonUnpressed:
					if (!P_0.tfsYXedjKMNWjMtDSqWFOsogLsAD())
					{
						flag = true;
					}
					break;
				case InputActionEventType.NegativeButtonDoublePressed:
				{
					vWDvlCaaRKVAiVecuglWeVUYDEJQA.gIHWLPTAPlydTRisReXVWULhQkST(0, out var num3);
					if (P_0.tKDOPYeIRAdrsVuNVTLireaOVYvj(num3))
					{
						flag = true;
					}
					break;
				}
				case InputActionEventType.NegativeButtonPressedForTime:
				{
					if (!vWDvlCaaRKVAiVecuglWeVUYDEJQA.gIHWLPTAPlydTRisReXVWULhQkST(0, out var num))
					{
						continue;
					}
					vWDvlCaaRKVAiVecuglWeVUYDEJQA.gIHWLPTAPlydTRisReXVWULhQkST(1, out var num2);
					if (P_0.GhKhyORygEOxoFyUKamdTHvQxDRV(num, num2))
					{
						flag = true;
					}
					break;
				}
				case InputActionEventType.NegativeButtonShortPressed:
					if (P_0.mQtESGAbvwzPFvKThNaXpGIagmKLA())
					{
						flag = true;
					}
					break;
				case InputActionEventType.NegativeButtonLongPressed:
					if (P_0.HLDzNTMoheZQgjfpQBATellsFfgqA())
					{
						flag = true;
					}
					break;
				case InputActionEventType.NegativeButtonJustPressed:
					if (P_0.fFQjdmlZoEThlEPanCBBkaXvvEJo())
					{
						flag = true;
					}
					break;
				case InputActionEventType.NegativeButtonJustReleased:
					if (P_0.qgryZPkzxqeyRJeMpPEmurhtTyMm())
					{
						flag = true;
					}
					break;
				case InputActionEventType.NegativeButtonJustDoublePressed:
				{
					vWDvlCaaRKVAiVecuglWeVUYDEJQA.gIHWLPTAPlydTRisReXVWULhQkST(0, out var num14);
					if (P_0.peodrpJyqFiCtWcUorQbtauWgAUY(num14))
					{
						flag = true;
					}
					break;
				}
				case InputActionEventType.NegativeButtonDoublePressJustReleased:
				{
					vWDvlCaaRKVAiVecuglWeVUYDEJQA.gIHWLPTAPlydTRisReXVWULhQkST(0, out var num13);
					if (P_0.TrVLrbWLZvvdCMVlqWvqIVGIcku(num13))
					{
						flag = true;
					}
					break;
				}
				case InputActionEventType.NegativeButtonJustPressedForTime:
				{
					if (!vWDvlCaaRKVAiVecuglWeVUYDEJQA.gIHWLPTAPlydTRisReXVWULhQkST(0, out var num10))
					{
						continue;
					}
					if (P_0.pPEOmNsceKoOFIoTUUbFjaYcwZYp(num10))
					{
						flag = true;
					}
					break;
				}
				case InputActionEventType.NegativeButtonJustShortPressed:
					if (P_0.RktaJYAilGppSOFeBEUKOdSeLPwzA())
					{
						flag = true;
					}
					break;
				case InputActionEventType.NegativeButtonJustLongPressed:
					if (P_0.AlvJRiQsiKqGaNlGbeltZXqUcmwfA())
					{
						flag = true;
					}
					break;
				case InputActionEventType.NegativeButtonPressedForTimeJustReleased:
				{
					if (!vWDvlCaaRKVAiVecuglWeVUYDEJQA.gIHWLPTAPlydTRisReXVWULhQkST(0, out var num7))
					{
						continue;
					}
					vWDvlCaaRKVAiVecuglWeVUYDEJQA.gIHWLPTAPlydTRisReXVWULhQkST(1, out var num8);
					if (P_0.gRUNqPKPvraajISMnBLVgdoGjsqrB(num7, num8))
					{
						flag = true;
					}
					break;
				}
				case InputActionEventType.NegativeButtonShortPressJustReleased:
					if (P_0.KuWEOnDveqvaooBzvBmdfTbetdXdc())
					{
						flag = true;
					}
					break;
				case InputActionEventType.NegativeButtonLongPressJustReleased:
					if (P_0.GoykAvFiGOAIADROsaULUBtZXKcs())
					{
						flag = true;
					}
					break;
				case InputActionEventType.NegativeButtonRepeating:
					if (P_0.evIwcwjhrvJGlfsVJPsBlQLBWKYB())
					{
						flag = true;
					}
					break;
				case InputActionEventType.NegativeButtonSinglePressed:
					if (P_0.pktQQZGkUCJMbigqGBIhecAFGgGE())
					{
						flag = true;
					}
					break;
				case InputActionEventType.NegativeButtonJustSinglePressed:
					if (P_0.sgucTVHHhxjEnJtjublUmMwWtJBfA())
					{
						flag = true;
					}
					break;
				case InputActionEventType.NegativeButtonSinglePressJustReleased:
					if (P_0.QWLgdtzoLZvnHwRmFgFnVtCtVUhB())
					{
						flag = true;
					}
					break;
				case InputActionEventType.AxisActive:
					if (!MathTools.ApproximatelyZero(P_0.JAUpfGusnmCqpzYFDMsATYuepPVJ()))
					{
						flag = true;
					}
					break;
				case InputActionEventType.AxisInactive:
					if (MathTools.ApproximatelyZero(P_0.JAUpfGusnmCqpzYFDMsATYuepPVJ()))
					{
						flag = true;
					}
					break;
				case InputActionEventType.AxisRawActive:
					if (!MathTools.ApproximatelyZero(P_0.KQaRYvVIaIiONwzKwSPiKLhzcpVbA()))
					{
						flag = true;
					}
					break;
				case InputActionEventType.AxisRawInactive:
					if (MathTools.ApproximatelyZero(P_0.KQaRYvVIaIiONwzKwSPiKLhzcpVbA()))
					{
						flag = true;
					}
					break;
				case InputActionEventType.AxisActiveOrJustInactive:
					if (!MathTools.ApproximatelyZero(P_0.JAUpfGusnmCqpzYFDMsATYuepPVJ()) || !MathTools.ApproximatelyZero(P_0.lNAOwBCyLxBSgQJDKGMNCXnwEnDF()))
					{
						flag = true;
					}
					break;
				case InputActionEventType.AxisRawActiveOrJustInactive:
					if (!MathTools.ApproximatelyZero(P_0.KQaRYvVIaIiONwzKwSPiKLhzcpVbA()) || !MathTools.ApproximatelyZero(P_0.PRHdLCgJAJrdPtapwSgOEeJvfUdjA()))
					{
						flag = true;
					}
					break;
				default:
					throw new NotImplementedException();
				}
				try
				{
					if (flag)
					{
						InputActionEventData obj = P_0.ealuMIfOgZSIduZTAhtqsgypksER(P_1);
						obj.eventType = vWDvlCaaRKVAiVecuglWeVUYDEJQA.SxXbiAFDmpBnSaGAITcEBXyWMRDoc;
						vWDvlCaaRKVAiVecuglWeVUYDEJQA.gybqTscDHxcHontLfEbdWinPeCkKA(obj);
					}
				}
				catch (Exception exception)
				{
					ReInput.HandleCallbackException("Player input event callback", exception);
				}
			}
		}
	}

	public void BcNRXlUcIeCeQoTBnFpegDhuaAap(Action<InputActionEventData> P_0, UpdateLoopType P_1, InputActionEventType P_2, int P_3, object[] P_4)
	{
		if (!jYTNgflwwEgvgbZuuTHYnPAnuRao)
		{
			zQQfvDZMmpVqPPLYlLuSJXXpwJcI();
		}
		VWDvlCaaRKVAiVecuglWeVUYDEJQA item;
		try
		{
			if (P_3 > ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.cuIqMUSdmKIejUTXChjhBrEkVSBe)
			{
				throw new ArgumentOutOfRangeException("Invalid Action Id " + P_3);
			}
			item = new VWDvlCaaRKVAiVecuglWeVUYDEJQA(P_0, P_1, P_2, P_3, P_4);
		}
		catch (Exception ex)
		{
			Logger.LogWarning("Failed to add Input Event delegate. Reason: " + ex.Message);
			return;
		}
		if (P_3 < 0)
		{
			kBhbzMbozDMqWbKbyexTTMCFftHyA[NBoCwlKPKYgAYEOhsJEUoLGjxcutA].Add(item);
		}
		else
		{
			kBhbzMbozDMqWbKbyexTTMCFftHyA[fatnwegVNllKcXFFVtHhQPynhCA[P_3]].Add(item);
		}
		EosJWGKZJEsupSACvNTmBMCWKnqW();
	}

	public void BcNRXlUcIeCeQoTBnFpegDhuaAap(Action<InputActionEventData> P_0, UpdateLoopType P_1, InputActionEventType P_2, object[] P_3)
	{
		if (!jYTNgflwwEgvgbZuuTHYnPAnuRao)
		{
			zQQfvDZMmpVqPPLYlLuSJXXpwJcI();
		}
		VWDvlCaaRKVAiVecuglWeVUYDEJQA item;
		try
		{
			item = new VWDvlCaaRKVAiVecuglWeVUYDEJQA(P_0, P_1, P_2, -1, P_3);
		}
		catch (Exception ex)
		{
			Logger.LogWarning("Failed to add Input Event delegate. Reason: " + ex.Message);
			return;
		}
		kBhbzMbozDMqWbKbyexTTMCFftHyA[NBoCwlKPKYgAYEOhsJEUoLGjxcutA].Add(item);
		EosJWGKZJEsupSACvNTmBMCWKnqW();
	}

	public void qZjUkljUkFefeYpopqopvDZOAIkDA(Action<InputActionEventData> P_0)
	{
		BqFrUOuWsvkUeJXpXxVPpAijCgIy bqFrUOuWsvkUeJXpXxVPpAijCgIy = new BqFrUOuWsvkUeJXpXxVPpAijCgIy();
		bqFrUOuWsvkUeJXpXxVPpAijCgIy.gybqTscDHxcHontLfEbdWinPeCkKA = P_0;
		if (jYTNgflwwEgvgbZuuTHYnPAnuRao)
		{
			AList<VWDvlCaaRKVAiVecuglWeVUYDEJQA>[] array = kBhbzMbozDMqWbKbyexTTMCFftHyA;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].RemoveAll(bqFrUOuWsvkUeJXpXxVPpAijCgIy.klwyyOuVQNelShchnczczlEDelrS);
			}
			EosJWGKZJEsupSACvNTmBMCWKnqW();
		}
	}

	public void qZjUkljUkFefeYpopqopvDZOAIkDA(Action<InputActionEventData> P_0, int P_1)
	{
		oXGBLnntgosJHECwitDKOhJpgarHA oXGBLnntgosJHECwitDKOhJpgarHA2 = new oXGBLnntgosJHECwitDKOhJpgarHA();
		oXGBLnntgosJHECwitDKOhJpgarHA2.gybqTscDHxcHontLfEbdWinPeCkKA = P_0;
		oXGBLnntgosJHECwitDKOhJpgarHA2.tlJxlOEWqQEUYpNGSwfnmndnnqSe = P_1;
		if (jYTNgflwwEgvgbZuuTHYnPAnuRao && oXGBLnntgosJHECwitDKOhJpgarHA2.tlJxlOEWqQEUYpNGSwfnmndnnqSe <= ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.cuIqMUSdmKIejUTXChjhBrEkVSBe)
		{
			AList<VWDvlCaaRKVAiVecuglWeVUYDEJQA>[] array = kBhbzMbozDMqWbKbyexTTMCFftHyA;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].RemoveAll(oXGBLnntgosJHECwitDKOhJpgarHA2.klwyyOuVQNelShchnczczlEDelrS);
			}
			EosJWGKZJEsupSACvNTmBMCWKnqW();
		}
	}

	public void qZjUkljUkFefeYpopqopvDZOAIkDA(Action<InputActionEventData> P_0, UpdateLoopType P_1)
	{
		DbhfxHlYQhDQcsMrSzKWgnMBxlad dbhfxHlYQhDQcsMrSzKWgnMBxlad = new DbhfxHlYQhDQcsMrSzKWgnMBxlad();
		dbhfxHlYQhDQcsMrSzKWgnMBxlad.gybqTscDHxcHontLfEbdWinPeCkKA = P_0;
		dbhfxHlYQhDQcsMrSzKWgnMBxlad.PiEdbgjMHSsksYjRKSGckYZEsfAE = P_1;
		if (jYTNgflwwEgvgbZuuTHYnPAnuRao)
		{
			AList<VWDvlCaaRKVAiVecuglWeVUYDEJQA>[] array = kBhbzMbozDMqWbKbyexTTMCFftHyA;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].RemoveAll(dbhfxHlYQhDQcsMrSzKWgnMBxlad.klwyyOuVQNelShchnczczlEDelrS);
			}
			EosJWGKZJEsupSACvNTmBMCWKnqW();
		}
	}

	public void qZjUkljUkFefeYpopqopvDZOAIkDA(Action<InputActionEventData> P_0, InputActionEventType P_1)
	{
		FyFRoaJQlsSHoQGbBiiMGcebGAZhb fyFRoaJQlsSHoQGbBiiMGcebGAZhb = new FyFRoaJQlsSHoQGbBiiMGcebGAZhb();
		fyFRoaJQlsSHoQGbBiiMGcebGAZhb.gybqTscDHxcHontLfEbdWinPeCkKA = P_0;
		fyFRoaJQlsSHoQGbBiiMGcebGAZhb.SxXbiAFDmpBnSaGAITcEBXyWMRDoc = P_1;
		if (jYTNgflwwEgvgbZuuTHYnPAnuRao)
		{
			AList<VWDvlCaaRKVAiVecuglWeVUYDEJQA>[] array = kBhbzMbozDMqWbKbyexTTMCFftHyA;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].RemoveAll(fyFRoaJQlsSHoQGbBiiMGcebGAZhb.klwyyOuVQNelShchnczczlEDelrS);
			}
			EosJWGKZJEsupSACvNTmBMCWKnqW();
		}
	}

	public void qZjUkljUkFefeYpopqopvDZOAIkDA(Action<InputActionEventData> P_0, UpdateLoopType P_1, int P_2)
	{
		ZxAxeGRdYOfvIHNLGKdfleqoChxT zxAxeGRdYOfvIHNLGKdfleqoChxT = new ZxAxeGRdYOfvIHNLGKdfleqoChxT();
		zxAxeGRdYOfvIHNLGKdfleqoChxT.gybqTscDHxcHontLfEbdWinPeCkKA = P_0;
		zxAxeGRdYOfvIHNLGKdfleqoChxT.PiEdbgjMHSsksYjRKSGckYZEsfAE = P_1;
		zxAxeGRdYOfvIHNLGKdfleqoChxT.tlJxlOEWqQEUYpNGSwfnmndnnqSe = P_2;
		if (jYTNgflwwEgvgbZuuTHYnPAnuRao && zxAxeGRdYOfvIHNLGKdfleqoChxT.tlJxlOEWqQEUYpNGSwfnmndnnqSe <= ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.cuIqMUSdmKIejUTXChjhBrEkVSBe)
		{
			AList<VWDvlCaaRKVAiVecuglWeVUYDEJQA>[] array = kBhbzMbozDMqWbKbyexTTMCFftHyA;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].RemoveAll(zxAxeGRdYOfvIHNLGKdfleqoChxT.klwyyOuVQNelShchnczczlEDelrS);
			}
			EosJWGKZJEsupSACvNTmBMCWKnqW();
		}
	}

	public void qZjUkljUkFefeYpopqopvDZOAIkDA(Action<InputActionEventData> P_0, UpdateLoopType P_1, InputActionEventType P_2, int P_3)
	{
		CkZGVoGdfbWrLqXqicPFHUTcTxmeB ckZGVoGdfbWrLqXqicPFHUTcTxmeB = new CkZGVoGdfbWrLqXqicPFHUTcTxmeB();
		ckZGVoGdfbWrLqXqicPFHUTcTxmeB.gybqTscDHxcHontLfEbdWinPeCkKA = P_0;
		ckZGVoGdfbWrLqXqicPFHUTcTxmeB.PiEdbgjMHSsksYjRKSGckYZEsfAE = P_1;
		ckZGVoGdfbWrLqXqicPFHUTcTxmeB.tlJxlOEWqQEUYpNGSwfnmndnnqSe = P_3;
		ckZGVoGdfbWrLqXqicPFHUTcTxmeB.SxXbiAFDmpBnSaGAITcEBXyWMRDoc = P_2;
		if (jYTNgflwwEgvgbZuuTHYnPAnuRao && ckZGVoGdfbWrLqXqicPFHUTcTxmeB.tlJxlOEWqQEUYpNGSwfnmndnnqSe <= ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.cuIqMUSdmKIejUTXChjhBrEkVSBe)
		{
			AList<VWDvlCaaRKVAiVecuglWeVUYDEJQA>[] array = kBhbzMbozDMqWbKbyexTTMCFftHyA;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].RemoveAll(ckZGVoGdfbWrLqXqicPFHUTcTxmeB.klwyyOuVQNelShchnczczlEDelrS);
			}
			EosJWGKZJEsupSACvNTmBMCWKnqW();
		}
	}

	public void qZjUkljUkFefeYpopqopvDZOAIkDA(Action<InputActionEventData> P_0, UpdateLoopType P_1, InputActionEventType P_2)
	{
		CJypnXgasolFlPoXCZDdEHKTrXSb cJypnXgasolFlPoXCZDdEHKTrXSb = new CJypnXgasolFlPoXCZDdEHKTrXSb();
		cJypnXgasolFlPoXCZDdEHKTrXSb.gybqTscDHxcHontLfEbdWinPeCkKA = P_0;
		cJypnXgasolFlPoXCZDdEHKTrXSb.PiEdbgjMHSsksYjRKSGckYZEsfAE = P_1;
		cJypnXgasolFlPoXCZDdEHKTrXSb.SxXbiAFDmpBnSaGAITcEBXyWMRDoc = P_2;
		if (jYTNgflwwEgvgbZuuTHYnPAnuRao)
		{
			AList<VWDvlCaaRKVAiVecuglWeVUYDEJQA>[] array = kBhbzMbozDMqWbKbyexTTMCFftHyA;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].RemoveAll(cJypnXgasolFlPoXCZDdEHKTrXSb.klwyyOuVQNelShchnczczlEDelrS);
			}
			EosJWGKZJEsupSACvNTmBMCWKnqW();
		}
	}

	public void qZjUkljUkFefeYpopqopvDZOAIkDA(Action<InputActionEventData> P_0, InputActionEventType P_1, int P_2)
	{
		ZemZSREDfVvwtInwNOQYtQOznsbB zemZSREDfVvwtInwNOQYtQOznsbB = new ZemZSREDfVvwtInwNOQYtQOznsbB();
		zemZSREDfVvwtInwNOQYtQOznsbB.gybqTscDHxcHontLfEbdWinPeCkKA = P_0;
		zemZSREDfVvwtInwNOQYtQOznsbB.tlJxlOEWqQEUYpNGSwfnmndnnqSe = P_2;
		zemZSREDfVvwtInwNOQYtQOznsbB.SxXbiAFDmpBnSaGAITcEBXyWMRDoc = P_1;
		if (jYTNgflwwEgvgbZuuTHYnPAnuRao && zemZSREDfVvwtInwNOQYtQOznsbB.tlJxlOEWqQEUYpNGSwfnmndnnqSe <= ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.cuIqMUSdmKIejUTXChjhBrEkVSBe)
		{
			AList<VWDvlCaaRKVAiVecuglWeVUYDEJQA>[] array = kBhbzMbozDMqWbKbyexTTMCFftHyA;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].RemoveAll(zemZSREDfVvwtInwNOQYtQOznsbB.klwyyOuVQNelShchnczczlEDelrS);
			}
			EosJWGKZJEsupSACvNTmBMCWKnqW();
		}
	}

	public void SPGTRPyvIslcMdbPTItsewSLRPxx()
	{
		if (jYTNgflwwEgvgbZuuTHYnPAnuRao)
		{
			AList<VWDvlCaaRKVAiVecuglWeVUYDEJQA>[] array = kBhbzMbozDMqWbKbyexTTMCFftHyA;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Clear();
			}
			EosJWGKZJEsupSACvNTmBMCWKnqW();
		}
	}

	private void EosJWGKZJEsupSACvNTmBMCWKnqW()
	{
		int num = 0;
		for (int i = 0; i < kBhbzMbozDMqWbKbyexTTMCFftHyA.Length; i++)
		{
			num += kBhbzMbozDMqWbKbyexTTMCFftHyA[i]._count;
		}
		VulcQkzEFSldNXBvZXTerjMDDble = num;
	}
}
