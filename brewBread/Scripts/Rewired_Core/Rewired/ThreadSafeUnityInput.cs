using System;
using Rewired.Utils;
using UnityEngine;

namespace Rewired
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	internal static class ThreadSafeUnityInput
	{
		[CustomObfuscation(rename = false)]
		[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
		public sealed class Keyboard
		{
			private const int KSKBmcXcCBdLxyaTWHLWkjHthpEt = 132;

			public static readonly int keyValueIndex_Escape;

			public static readonly int keyValueIndex_Menu;

			public static readonly int keyValueIndex_F2;

			public static readonly int keyValueIndex_UpArrow;

			public static readonly int keyValueIndex_RightArrow;

			public static readonly int keyValueIndex_DownArrow;

			public static readonly int keyValueIndex_LeftArrow;

			private static readonly int[] ttPCQOIhBWLspEayDBReynnTjdzD;

			private readonly int ioJHYiWACZZkkzZeFRiOwxJFfdQw;

			private readonly int[] eDsnovpgDYRCEkItkogoNDRSYMgr;

			private readonly bool[] mRJsAJSfWTOusfaseyppRkuiypFP;

			private bool kKFZZElqKQSUFMZnWdEudRvTJGpo;

			private int slqCwNOADNuaziwQnpYBsLAgDkbU;

			private readonly bool sXLLyWhVtnTsJUdLYDsshzdInxBV;

			private bool qAUjLUowhhcjcfaueZHSCZUPGnLPA;

			public bool enabled
			{
				get
				{
					return kKFZZElqKQSUFMZnWdEudRvTJGpo;
				}
				set
				{
					if (value != kKFZZElqKQSUFMZnWdEudRvTJGpo)
					{
						kKFZZElqKQSUFMZnWdEudRvTJGpo = value;
						if (!kKFZZElqKQSUFMZnWdEudRvTJGpo)
						{
							Clear();
						}
					}
				}
			}

			public bool monitoring => slqCwNOADNuaziwQnpYBsLAgDkbU > 0;

			public int keyCount => 132;

			static Keyboard()
			{
				if (UnityTools.isAndroidPlatform)
				{
					int[] keyboardKeyValues = Consts._keyboardKeyValues;
					ttPCQOIhBWLspEayDBReynnTjdzD = new int[7]
					{
						(keyValueIndex_Escape = ArrayTools.IndexOf(keyboardKeyValues, 27)),
						(keyValueIndex_Menu = ArrayTools.IndexOf(keyboardKeyValues, 319)),
						(keyValueIndex_F2 = ArrayTools.IndexOf(keyboardKeyValues, 283)),
						(keyValueIndex_UpArrow = ArrayTools.IndexOf(keyboardKeyValues, 273)),
						(keyValueIndex_RightArrow = ArrayTools.IndexOf(keyboardKeyValues, 275)),
						(keyValueIndex_DownArrow = ArrayTools.IndexOf(keyboardKeyValues, 274)),
						(keyValueIndex_LeftArrow = ArrayTools.IndexOf(keyboardKeyValues, 276))
					};
				}
			}

			public Keyboard()
			{
				mRJsAJSfWTOusfaseyppRkuiypFP = new bool[132];
				int[] keyboardKeyValues = Consts._keyboardKeyValues;
				int num = keyboardKeyValues.Length;
				for (int i = 0; i < num; i++)
				{
					if (keyboardKeyValues[i] > ioJHYiWACZZkkzZeFRiOwxJFfdQw)
					{
						ioJHYiWACZZkkzZeFRiOwxJFfdQw = keyboardKeyValues[i];
					}
				}
				eDsnovpgDYRCEkItkogoNDRSYMgr = new int[ioJHYiWACZZkkzZeFRiOwxJFfdQw + 1];
				ArrayTools.Fill(eDsnovpgDYRCEkItkogoNDRSYMgr, -1);
				for (int j = 0; j < num; j++)
				{
					eDsnovpgDYRCEkItkogoNDRSYMgr[keyboardKeyValues[j]] = j;
				}
			}

			public void Initialize()
			{
				if (slqCwNOADNuaziwQnpYBsLAgDkbU != 0)
				{
					OKxvPvoURRjupSYqKtJLgoTgWMxN();
				}
				ptzeSUAYMaLUihwOYdMoDqBEoBwfb();
			}

			public void PostInitialize()
			{
				Update();
			}

			public void Update()
			{
				if (slqCwNOADNuaziwQnpYBsLAgDkbU == 0)
				{
					return;
				}
				if (Input.anyKey)
				{
					qAUjLUowhhcjcfaueZHSCZUPGnLPA = true;
					if (kKFZZElqKQSUFMZnWdEudRvTJGpo)
					{
						int[] keyboardKeyValues = Consts._keyboardKeyValues;
						for (int i = 0; i < 132; i++)
						{
							mRJsAJSfWTOusfaseyppRkuiypFP[i] = Input.GetKey((KeyCode)keyboardKeyValues[i]);
						}
					}
					else if (sXLLyWhVtnTsJUdLYDsshzdInxBV)
					{
						mRJsAJSfWTOusfaseyppRkuiypFP[keyValueIndex_Escape] = GetKey(KeyCode.Escape);
						mRJsAJSfWTOusfaseyppRkuiypFP[keyValueIndex_Menu] = GetKey(KeyCode.Menu);
						mRJsAJSfWTOusfaseyppRkuiypFP[keyValueIndex_F2] = GetKey(KeyCode.F2);
						mRJsAJSfWTOusfaseyppRkuiypFP[keyValueIndex_UpArrow] = GetKey(KeyCode.UpArrow);
						mRJsAJSfWTOusfaseyppRkuiypFP[keyValueIndex_RightArrow] = GetKey(KeyCode.RightArrow);
						mRJsAJSfWTOusfaseyppRkuiypFP[keyValueIndex_DownArrow] = GetKey(KeyCode.DownArrow);
						mRJsAJSfWTOusfaseyppRkuiypFP[keyValueIndex_LeftArrow] = GetKey(KeyCode.LeftArrow);
					}
				}
				else if (qAUjLUowhhcjcfaueZHSCZUPGnLPA)
				{
					Array.Clear(mRJsAJSfWTOusfaseyppRkuiypFP, 0, mRJsAJSfWTOusfaseyppRkuiypFP.Length);
				}
			}

			public void Monitor(bool state)
			{
				if (state)
				{
					slqCwNOADNuaziwQnpYBsLAgDkbU++;
					if (slqCwNOADNuaziwQnpYBsLAgDkbU == 1)
					{
						UqdMoMtxMcHiYaHWyMZuAUeCVDWT();
					}
					return;
				}
				slqCwNOADNuaziwQnpYBsLAgDkbU--;
				if (slqCwNOADNuaziwQnpYBsLAgDkbU < 0)
				{
					slqCwNOADNuaziwQnpYBsLAgDkbU = 0;
					RgnNmZnACSxAgrmsAdhxaroTbLTdb();
				}
				if (slqCwNOADNuaziwQnpYBsLAgDkbU == 0)
				{
					sNGJsvCFsuenrEOXDxjdGIytxIAg();
				}
			}

			public bool GetKey(KeyCode keyCode)
			{
				if (slqCwNOADNuaziwQnpYBsLAgDkbU == 0)
				{
					dLTmNLbcfcSsYBWfHMowHMrlNqOc();
					return false;
				}
				if ((uint)keyCode > (uint)ioJHYiWACZZkkzZeFRiOwxJFfdQw)
				{
					return false;
				}
				return mRJsAJSfWTOusfaseyppRkuiypFP[eDsnovpgDYRCEkItkogoNDRSYMgr[(int)keyCode]];
			}

			public void GetKeyValues(bool[] values)
			{
				if (slqCwNOADNuaziwQnpYBsLAgDkbU == 0)
				{
					dLTmNLbcfcSsYBWfHMowHMrlNqOc();
				}
				else if (values != null && values.Length >= 132)
				{
					Array.Copy(mRJsAJSfWTOusfaseyppRkuiypFP, values, 132);
				}
			}

			public void Clear()
			{
				if (sXLLyWhVtnTsJUdLYDsshzdInxBV)
				{
					for (int i = 0; i < 132; i++)
					{
						if (Array.IndexOf(ttPCQOIhBWLspEayDBReynnTjdzD, i) < 0)
						{
							mRJsAJSfWTOusfaseyppRkuiypFP[i] = false;
						}
					}
				}
				else
				{
					Array.Clear(mRJsAJSfWTOusfaseyppRkuiypFP, 0, 132);
				}
			}

			private void OKxvPvoURRjupSYqKtJLgoTgWMxN()
			{
				Array.Clear(mRJsAJSfWTOusfaseyppRkuiypFP, 0, 132);
			}

			private void ptzeSUAYMaLUihwOYdMoDqBEoBwfb()
			{
				slqCwNOADNuaziwQnpYBsLAgDkbU = 0;
				kKFZZElqKQSUFMZnWdEudRvTJGpo = true;
			}

			private void UqdMoMtxMcHiYaHWyMZuAUeCVDWT()
			{
			}

			private void sNGJsvCFsuenrEOXDxjdGIytxIAg()
			{
				OKxvPvoURRjupSYqKtJLgoTgWMxN();
			}

			private void dLTmNLbcfcSsYBWfHMowHMrlNqOc()
			{
				Logger.LogWarning("You are trying to use Keyboard without incrementing the monitor count.", requiredThreadSafety: true);
			}

			private void RgnNmZnACSxAgrmsAdhxaroTbLTdb()
			{
				Logger.LogWarning("You are decrementing the Keyboard monitor count more than you are incrementing it.", requiredThreadSafety: true);
			}
		}

		[CustomObfuscation(rename = false)]
		[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
		public sealed class Mouse
		{
			private const int VnQiGrFElxZQckFfuctAXoBRfYio = 7;

			private const int BARvFwUUgUwYIgxDIrCfGAtIYLiK = 4;

			private readonly bool[] YXuCILbbSuGPNMGgRTZoPeQmcMPsA;

			private readonly float[] bjSaUzibFNjMvUxWxsNvcnSslSIE;

			private int slqCwNOADNuaziwQnpYBsLAgDkbU;

			private Vector3 ofsfUcwRLOMfZtmGigYHgPHPYaVjA;

			private bool hqIancbNdeQahWmzJNeqgDvorJFLA;

			private bool rUwncdFqovCJQbqmfeNYndIMaweDb;

			public bool monitoring => slqCwNOADNuaziwQnpYBsLAgDkbU > 0;

			public Vector3 mousePosition => ofsfUcwRLOMfZtmGigYHgPHPYaVjA;

			public bool mousePresent => hqIancbNdeQahWmzJNeqgDvorJFLA;

			public Mouse()
			{
				YXuCILbbSuGPNMGgRTZoPeQmcMPsA = new bool[7];
				bjSaUzibFNjMvUxWxsNvcnSslSIE = new float[4];
				ptzeSUAYMaLUihwOYdMoDqBEoBwfb();
			}

			public void PostInitialize()
			{
				Update();
			}

			public void Update()
			{
				if (slqCwNOADNuaziwQnpYBsLAgDkbU == 0)
				{
					return;
				}
				if (!rUwncdFqovCJQbqmfeNYndIMaweDb)
				{
					try
					{
						for (int i = 0; i < 7; i++)
						{
							YXuCILbbSuGPNMGgRTZoPeQmcMPsA[i] = Input.GetButton(Consts.mouseButtonUnityNames[i]);
						}
						for (int j = 0; j < 3; j++)
						{
							bjSaUzibFNjMvUxWxsNvcnSslSIE[j] = Input.GetAxisRaw(Consts.mouseAxisUnityNames[j]);
						}
					}
					catch
					{
						Logger.LogError("Unity Input Manager mouse entries are missing. Rewired was not installed properly or was canceled during installation, preventing it from installing the necessary Unity Input Manager entries for mouse input or the input manager entries may have been overwritten by another package installed in your project. Mouse input will not function if native mouse input is disabled or is unavailable on this platform.");
						rUwncdFqovCJQbqmfeNYndIMaweDb = true;
					}
				}
				bjSaUzibFNjMvUxWxsNvcnSslSIE[3] = Input.mouseScrollDelta.x;
				ofsfUcwRLOMfZtmGigYHgPHPYaVjA = Input.mousePosition;
				hqIancbNdeQahWmzJNeqgDvorJFLA = Input.mousePresent;
			}

			public void Monitor(bool state)
			{
				if (state)
				{
					slqCwNOADNuaziwQnpYBsLAgDkbU++;
					if (slqCwNOADNuaziwQnpYBsLAgDkbU == 1)
					{
						UqdMoMtxMcHiYaHWyMZuAUeCVDWT();
					}
					return;
				}
				slqCwNOADNuaziwQnpYBsLAgDkbU--;
				if (slqCwNOADNuaziwQnpYBsLAgDkbU < 0)
				{
					slqCwNOADNuaziwQnpYBsLAgDkbU = 0;
					RgnNmZnACSxAgrmsAdhxaroTbLTdb();
				}
				if (slqCwNOADNuaziwQnpYBsLAgDkbU == 0)
				{
					sNGJsvCFsuenrEOXDxjdGIytxIAg();
				}
			}

			public bool GetButton(int index)
			{
				if (slqCwNOADNuaziwQnpYBsLAgDkbU == 0)
				{
					jxvCxrwjjUpCacwLcCRHqdfkrLsA();
					return false;
				}
				if ((uint)index >= 7u)
				{
					return false;
				}
				return YXuCILbbSuGPNMGgRTZoPeQmcMPsA[index];
			}

			public float GetAxisRaw(int index)
			{
				if (slqCwNOADNuaziwQnpYBsLAgDkbU == 0)
				{
					jxvCxrwjjUpCacwLcCRHqdfkrLsA();
					return 0f;
				}
				if ((uint)index >= 4u)
				{
					return 0f;
				}
				return bjSaUzibFNjMvUxWxsNvcnSslSIE[index];
			}

			public void GetButtonValues(bool[] buttons)
			{
				if (slqCwNOADNuaziwQnpYBsLAgDkbU == 0)
				{
					jxvCxrwjjUpCacwLcCRHqdfkrLsA();
				}
				else if (buttons != null && buttons.Length >= 7)
				{
					Array.Copy(YXuCILbbSuGPNMGgRTZoPeQmcMPsA, buttons, 7);
				}
			}

			public void GetAxisRawValues(float[] axes)
			{
				if (slqCwNOADNuaziwQnpYBsLAgDkbU == 0)
				{
					jxvCxrwjjUpCacwLcCRHqdfkrLsA();
				}
				else if (axes != null && axes.Length >= 4)
				{
					Array.Copy(bjSaUzibFNjMvUxWxsNvcnSslSIE, axes, 4);
				}
			}

			private void OKxvPvoURRjupSYqKtJLgoTgWMxN()
			{
				Array.Clear(YXuCILbbSuGPNMGgRTZoPeQmcMPsA, 0, 7);
				Array.Clear(bjSaUzibFNjMvUxWxsNvcnSslSIE, 0, 4);
			}

			private void ptzeSUAYMaLUihwOYdMoDqBEoBwfb()
			{
				slqCwNOADNuaziwQnpYBsLAgDkbU = 0;
				ofsfUcwRLOMfZtmGigYHgPHPYaVjA = Vector3.zero;
				hqIancbNdeQahWmzJNeqgDvorJFLA = false;
			}

			private void UqdMoMtxMcHiYaHWyMZuAUeCVDWT()
			{
			}

			private void sNGJsvCFsuenrEOXDxjdGIytxIAg()
			{
				OKxvPvoURRjupSYqKtJLgoTgWMxN();
			}

			private void jxvCxrwjjUpCacwLcCRHqdfkrLsA()
			{
				Logger.LogWarning("You are trying to use Mouse without incrementing the monitor count.", requiredThreadSafety: true);
			}

			private void RgnNmZnACSxAgrmsAdhxaroTbLTdb()
			{
				Logger.LogWarning("You are decrementing the Mouse monitor count more than you are incrementing it.", requiredThreadSafety: true);
			}
		}

		private static Mouse bKoZOjcjaRQYKDivtGCcnpEhOVte;

		private static Keyboard YweDTrCAKOQqcjdBMYBocZoZyikKA;

		public static Mouse mouse => bKoZOjcjaRQYKDivtGCcnpEhOVte ?? (bKoZOjcjaRQYKDivtGCcnpEhOVte = new Mouse());

		public static Keyboard keyboard => YweDTrCAKOQqcjdBMYBocZoZyikKA ?? (YweDTrCAKOQqcjdBMYBocZoZyikKA = new Keyboard());

		public static void Initialize()
		{
		}

		public static void PostInitialize()
		{
			if (YweDTrCAKOQqcjdBMYBocZoZyikKA != null)
			{
				YweDTrCAKOQqcjdBMYBocZoZyikKA.PostInitialize();
			}
			if (bKoZOjcjaRQYKDivtGCcnpEhOVte != null)
			{
				bKoZOjcjaRQYKDivtGCcnpEhOVte.PostInitialize();
			}
		}

		public static void PostInitialize2()
		{
		}

		public static void Deinitialize()
		{
			if (YweDTrCAKOQqcjdBMYBocZoZyikKA != null)
			{
				YweDTrCAKOQqcjdBMYBocZoZyikKA = null;
			}
			if (bKoZOjcjaRQYKDivtGCcnpEhOVte != null)
			{
				bKoZOjcjaRQYKDivtGCcnpEhOVte = null;
			}
		}

		public static void Update()
		{
			if (YweDTrCAKOQqcjdBMYBocZoZyikKA != null)
			{
				YweDTrCAKOQqcjdBMYBocZoZyikKA.enabled = ReInput.controllers.Keyboard.enabled;
				YweDTrCAKOQqcjdBMYBocZoZyikKA.Update();
			}
			if (bKoZOjcjaRQYKDivtGCcnpEhOVte != null)
			{
				bKoZOjcjaRQYKDivtGCcnpEhOVte.Update();
			}
		}
	}
}
