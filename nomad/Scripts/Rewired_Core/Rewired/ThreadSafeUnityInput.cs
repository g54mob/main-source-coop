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
			private const int ofaSorayOOuFNXyhqlqMEYZHLWPO = 132;

			public static readonly int keyValueIndex_Escape;

			public static readonly int keyValueIndex_Menu;

			public static readonly int keyValueIndex_F2;

			public static readonly int keyValueIndex_UpArrow;

			public static readonly int keyValueIndex_RightArrow;

			public static readonly int keyValueIndex_DownArrow;

			public static readonly int keyValueIndex_LeftArrow;

			private static readonly int[] aLAYtGyjxHuaRwnAePaKbuZfEEbM;

			private readonly int FlXCHNrVwmBsMztguJvPatGikenS;

			private readonly int[] siNAaQuCGkOfPpnmcOSPlnENoqmJ;

			private readonly bool[] DWZhdQIFEhFdivjUsgqXdrCADCAaA;

			private bool HJVXWqjJUDPyMfUMLjSdxTeSDMFP;

			private int ozaZxjOPlyPoIxKnjDFkTwYfRBnB;

			private readonly bool WdXfsWkgVfvcTaNIEAnjaesStzEm;

			private bool imypkeiNLGAnegprjFcTurcwXFJQA;

			public bool enabled
			{
				get
				{
					return HJVXWqjJUDPyMfUMLjSdxTeSDMFP;
				}
				set
				{
					if (value != HJVXWqjJUDPyMfUMLjSdxTeSDMFP)
					{
						HJVXWqjJUDPyMfUMLjSdxTeSDMFP = value;
						if (!HJVXWqjJUDPyMfUMLjSdxTeSDMFP)
						{
							Clear();
						}
					}
				}
			}

			public bool monitoring => ozaZxjOPlyPoIxKnjDFkTwYfRBnB > 0;

			public int keyCount => 132;

			static Keyboard()
			{
				if (UnityTools.isAndroidPlatform)
				{
					int[] keyboardKeyValues = Consts._keyboardKeyValues;
					aLAYtGyjxHuaRwnAePaKbuZfEEbM = new int[7]
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
				DWZhdQIFEhFdivjUsgqXdrCADCAaA = new bool[132];
				int[] keyboardKeyValues = Consts._keyboardKeyValues;
				int num = keyboardKeyValues.Length;
				for (int i = 0; i < num; i++)
				{
					if (keyboardKeyValues[i] > FlXCHNrVwmBsMztguJvPatGikenS)
					{
						FlXCHNrVwmBsMztguJvPatGikenS = keyboardKeyValues[i];
					}
				}
				siNAaQuCGkOfPpnmcOSPlnENoqmJ = new int[FlXCHNrVwmBsMztguJvPatGikenS + 1];
				ArrayTools.Fill(siNAaQuCGkOfPpnmcOSPlnENoqmJ, -1);
				for (int j = 0; j < num; j++)
				{
					siNAaQuCGkOfPpnmcOSPlnENoqmJ[keyboardKeyValues[j]] = j;
				}
			}

			public void Initialize()
			{
				if (ozaZxjOPlyPoIxKnjDFkTwYfRBnB != 0)
				{
					aiREnDJZyelRLnVyEozYOSNwrTDeA();
				}
				FFQQGDtREMgKjHDlGGbaqzioYJNR();
			}

			public void PostInitialize()
			{
				Update();
			}

			public void Update()
			{
				if (ozaZxjOPlyPoIxKnjDFkTwYfRBnB == 0)
				{
					return;
				}
				if (Input.anyKey)
				{
					imypkeiNLGAnegprjFcTurcwXFJQA = true;
					if (HJVXWqjJUDPyMfUMLjSdxTeSDMFP)
					{
						int[] keyboardKeyValues = Consts._keyboardKeyValues;
						for (int i = 0; i < 132; i++)
						{
							DWZhdQIFEhFdivjUsgqXdrCADCAaA[i] = Input.GetKey((KeyCode)keyboardKeyValues[i]);
						}
					}
					else if (WdXfsWkgVfvcTaNIEAnjaesStzEm)
					{
						DWZhdQIFEhFdivjUsgqXdrCADCAaA[keyValueIndex_Escape] = GetKey(KeyCode.Escape);
						DWZhdQIFEhFdivjUsgqXdrCADCAaA[keyValueIndex_Menu] = GetKey(KeyCode.Menu);
						DWZhdQIFEhFdivjUsgqXdrCADCAaA[keyValueIndex_F2] = GetKey(KeyCode.F2);
						DWZhdQIFEhFdivjUsgqXdrCADCAaA[keyValueIndex_UpArrow] = GetKey(KeyCode.UpArrow);
						DWZhdQIFEhFdivjUsgqXdrCADCAaA[keyValueIndex_RightArrow] = GetKey(KeyCode.RightArrow);
						DWZhdQIFEhFdivjUsgqXdrCADCAaA[keyValueIndex_DownArrow] = GetKey(KeyCode.DownArrow);
						DWZhdQIFEhFdivjUsgqXdrCADCAaA[keyValueIndex_LeftArrow] = GetKey(KeyCode.LeftArrow);
					}
				}
				else if (imypkeiNLGAnegprjFcTurcwXFJQA)
				{
					Array.Clear(DWZhdQIFEhFdivjUsgqXdrCADCAaA, 0, DWZhdQIFEhFdivjUsgqXdrCADCAaA.Length);
				}
			}

			public void Monitor(bool state)
			{
				if (state)
				{
					ozaZxjOPlyPoIxKnjDFkTwYfRBnB++;
					if (ozaZxjOPlyPoIxKnjDFkTwYfRBnB == 1)
					{
						RcVgbuqRkwIaErNMmdjufncUniHe();
					}
					return;
				}
				ozaZxjOPlyPoIxKnjDFkTwYfRBnB--;
				if (ozaZxjOPlyPoIxKnjDFkTwYfRBnB < 0)
				{
					ozaZxjOPlyPoIxKnjDFkTwYfRBnB = 0;
					hMZMXOIxToLyBJFYARppUNGwigkE();
				}
				if (ozaZxjOPlyPoIxKnjDFkTwYfRBnB == 0)
				{
					xllIWsIXOnsCgZGNcisAPycYvKpf();
				}
			}

			public bool GetKey(KeyCode keyCode)
			{
				if (ozaZxjOPlyPoIxKnjDFkTwYfRBnB == 0)
				{
					obNnvONoCMEdacWTnJjOpiDcDpzJA();
					return false;
				}
				if ((uint)keyCode > (uint)FlXCHNrVwmBsMztguJvPatGikenS)
				{
					return false;
				}
				return DWZhdQIFEhFdivjUsgqXdrCADCAaA[siNAaQuCGkOfPpnmcOSPlnENoqmJ[(int)keyCode]];
			}

			public void GetKeyValues(bool[] values)
			{
				if (ozaZxjOPlyPoIxKnjDFkTwYfRBnB == 0)
				{
					obNnvONoCMEdacWTnJjOpiDcDpzJA();
				}
				else if (values != null && values.Length >= 132)
				{
					Array.Copy(DWZhdQIFEhFdivjUsgqXdrCADCAaA, values, 132);
				}
			}

			public void Clear()
			{
				if (WdXfsWkgVfvcTaNIEAnjaesStzEm)
				{
					for (int i = 0; i < 132; i++)
					{
						if (Array.IndexOf(aLAYtGyjxHuaRwnAePaKbuZfEEbM, i) < 0)
						{
							DWZhdQIFEhFdivjUsgqXdrCADCAaA[i] = false;
						}
					}
				}
				else
				{
					Array.Clear(DWZhdQIFEhFdivjUsgqXdrCADCAaA, 0, 132);
				}
			}

			private void aiREnDJZyelRLnVyEozYOSNwrTDeA()
			{
				Array.Clear(DWZhdQIFEhFdivjUsgqXdrCADCAaA, 0, 132);
			}

			private void FFQQGDtREMgKjHDlGGbaqzioYJNR()
			{
				ozaZxjOPlyPoIxKnjDFkTwYfRBnB = 0;
				HJVXWqjJUDPyMfUMLjSdxTeSDMFP = true;
			}

			private void RcVgbuqRkwIaErNMmdjufncUniHe()
			{
			}

			private void xllIWsIXOnsCgZGNcisAPycYvKpf()
			{
				aiREnDJZyelRLnVyEozYOSNwrTDeA();
			}

			private void obNnvONoCMEdacWTnJjOpiDcDpzJA()
			{
				Logger.LogWarning("You are trying to use Keyboard without incrementing the monitor count.", requiredThreadSafety: true);
			}

			private void hMZMXOIxToLyBJFYARppUNGwigkE()
			{
				Logger.LogWarning("You are decrementing the Keyboard monitor count more than you are incrementing it.", requiredThreadSafety: true);
			}
		}

		[CustomObfuscation(rename = false)]
		[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
		public sealed class Mouse
		{
			private const int IrrfjyDdyXthrqqEQYZufXQQvThk = 7;

			private const int ENUyqfyaDoCbPAyNhfzMSRdswRlJA = 4;

			private readonly bool[] fcmWRBtOGVwupXXfPbspblJLfEL;

			private readonly float[] CfqvRVbfYiAxaswBEJVzCrQIVxXo;

			private int SxTiIMXQNJCxrjoJoCEjCpJWwGPo;

			private Vector3 NtRoaQgPtzVyIfViSeQkzLhoYOvL;

			private bool UFXDKpdYYDOlmsgXbAmbfGyjRjUfB;

			private bool NDVjtyHCEbVVzwezloEGcMGIcsVG;

			public bool monitoring => SxTiIMXQNJCxrjoJoCEjCpJWwGPo > 0;

			public Vector3 mousePosition => NtRoaQgPtzVyIfViSeQkzLhoYOvL;

			public bool mousePresent => UFXDKpdYYDOlmsgXbAmbfGyjRjUfB;

			public Mouse()
			{
				fcmWRBtOGVwupXXfPbspblJLfEL = new bool[7];
				CfqvRVbfYiAxaswBEJVzCrQIVxXo = new float[4];
				mpFzWcocJkghjuTeWDcYLmwuktHu();
			}

			public void PostInitialize()
			{
				Update();
			}

			public void Update()
			{
				if (SxTiIMXQNJCxrjoJoCEjCpJWwGPo == 0)
				{
					return;
				}
				if (!NDVjtyHCEbVVzwezloEGcMGIcsVG)
				{
					try
					{
						for (int i = 0; i < 7; i++)
						{
							fcmWRBtOGVwupXXfPbspblJLfEL[i] = Input.GetButton(Consts.mouseButtonUnityNames[i]);
						}
						for (int j = 0; j < 3; j++)
						{
							CfqvRVbfYiAxaswBEJVzCrQIVxXo[j] = Input.GetAxisRaw(Consts.mouseAxisUnityNames[j]);
						}
					}
					catch
					{
						Logger.LogError("Unity Input Manager mouse entries are missing. Rewired was not installed properly or was canceled during installation, preventing it from installing the necessary Unity Input Manager entries for mouse input or the input manager entries may have been overwritten by another package installed in your project. Mouse input will not function if native mouse input is disabled or is unavailable on this platform.");
						NDVjtyHCEbVVzwezloEGcMGIcsVG = true;
					}
				}
				CfqvRVbfYiAxaswBEJVzCrQIVxXo[3] = Input.mouseScrollDelta.x;
				NtRoaQgPtzVyIfViSeQkzLhoYOvL = Input.mousePosition;
				UFXDKpdYYDOlmsgXbAmbfGyjRjUfB = Input.mousePresent;
			}

			public void Monitor(bool state)
			{
				if (state)
				{
					SxTiIMXQNJCxrjoJoCEjCpJWwGPo++;
					if (SxTiIMXQNJCxrjoJoCEjCpJWwGPo == 1)
					{
						oUpHnTwyfvAlvtCfvJZZqwcNNDMM();
					}
					return;
				}
				SxTiIMXQNJCxrjoJoCEjCpJWwGPo--;
				if (SxTiIMXQNJCxrjoJoCEjCpJWwGPo < 0)
				{
					SxTiIMXQNJCxrjoJoCEjCpJWwGPo = 0;
					KpnfoduGtaSycIDqHocjYztgDabq();
				}
				if (SxTiIMXQNJCxrjoJoCEjCpJWwGPo == 0)
				{
					UGDpoZlNAvWpUZQticNKiAdHAjWS();
				}
			}

			public bool GetButton(int index)
			{
				if (SxTiIMXQNJCxrjoJoCEjCpJWwGPo == 0)
				{
					LTdjVLIBPXiPzCGmtXZyOWbadSHTA();
					return false;
				}
				if ((uint)index >= 7u)
				{
					return false;
				}
				return fcmWRBtOGVwupXXfPbspblJLfEL[index];
			}

			public float GetAxisRaw(int index)
			{
				if (SxTiIMXQNJCxrjoJoCEjCpJWwGPo == 0)
				{
					LTdjVLIBPXiPzCGmtXZyOWbadSHTA();
					return 0f;
				}
				if ((uint)index >= 4u)
				{
					return 0f;
				}
				return CfqvRVbfYiAxaswBEJVzCrQIVxXo[index];
			}

			public void GetButtonValues(bool[] buttons)
			{
				if (SxTiIMXQNJCxrjoJoCEjCpJWwGPo == 0)
				{
					LTdjVLIBPXiPzCGmtXZyOWbadSHTA();
				}
				else if (buttons != null && buttons.Length >= 7)
				{
					Array.Copy(fcmWRBtOGVwupXXfPbspblJLfEL, buttons, 7);
				}
			}

			public void GetAxisRawValues(float[] axes)
			{
				if (SxTiIMXQNJCxrjoJoCEjCpJWwGPo == 0)
				{
					LTdjVLIBPXiPzCGmtXZyOWbadSHTA();
				}
				else if (axes != null && axes.Length >= 4)
				{
					Array.Copy(CfqvRVbfYiAxaswBEJVzCrQIVxXo, axes, 4);
				}
			}

			private void KtximCiBdqTDHdLwAUPDdraqOXON()
			{
				Array.Clear(fcmWRBtOGVwupXXfPbspblJLfEL, 0, 7);
				Array.Clear(CfqvRVbfYiAxaswBEJVzCrQIVxXo, 0, 4);
			}

			private void mpFzWcocJkghjuTeWDcYLmwuktHu()
			{
				SxTiIMXQNJCxrjoJoCEjCpJWwGPo = 0;
				NtRoaQgPtzVyIfViSeQkzLhoYOvL = Vector3.zero;
				UFXDKpdYYDOlmsgXbAmbfGyjRjUfB = false;
			}

			private void oUpHnTwyfvAlvtCfvJZZqwcNNDMM()
			{
			}

			private void UGDpoZlNAvWpUZQticNKiAdHAjWS()
			{
				KtximCiBdqTDHdLwAUPDdraqOXON();
			}

			private void LTdjVLIBPXiPzCGmtXZyOWbadSHTA()
			{
				Logger.LogWarning("You are trying to use Mouse without incrementing the monitor count.", requiredThreadSafety: true);
			}

			private void KpnfoduGtaSycIDqHocjYztgDabq()
			{
				Logger.LogWarning("You are decrementing the Mouse monitor count more than you are incrementing it.", requiredThreadSafety: true);
			}
		}

		private static Mouse kGUzvVqNTFaFnkYwgjuKdJdafqUFb;

		private static Keyboard IcVGfwigxeOYCpTHnHCjRFeMrrpSA;

		public static Mouse mouse => kGUzvVqNTFaFnkYwgjuKdJdafqUFb ?? (kGUzvVqNTFaFnkYwgjuKdJdafqUFb = new Mouse());

		public static Keyboard keyboard => IcVGfwigxeOYCpTHnHCjRFeMrrpSA ?? (IcVGfwigxeOYCpTHnHCjRFeMrrpSA = new Keyboard());

		public static void Initialize()
		{
		}

		public static void PostInitialize()
		{
			if (IcVGfwigxeOYCpTHnHCjRFeMrrpSA != null)
			{
				IcVGfwigxeOYCpTHnHCjRFeMrrpSA.PostInitialize();
			}
			if (kGUzvVqNTFaFnkYwgjuKdJdafqUFb != null)
			{
				kGUzvVqNTFaFnkYwgjuKdJdafqUFb.PostInitialize();
			}
		}

		public static void PostInitialize2()
		{
		}

		public static void Deinitialize()
		{
			if (IcVGfwigxeOYCpTHnHCjRFeMrrpSA != null)
			{
				IcVGfwigxeOYCpTHnHCjRFeMrrpSA = null;
			}
			if (kGUzvVqNTFaFnkYwgjuKdJdafqUFb != null)
			{
				kGUzvVqNTFaFnkYwgjuKdJdafqUFb = null;
			}
		}

		public static void Update()
		{
			if (IcVGfwigxeOYCpTHnHCjRFeMrrpSA != null)
			{
				IcVGfwigxeOYCpTHnHCjRFeMrrpSA.enabled = ReInput.controllers.Keyboard.enabled;
				IcVGfwigxeOYCpTHnHCjRFeMrrpSA.Update();
			}
			if (kGUzvVqNTFaFnkYwgjuKdJdafqUFb != null)
			{
				kGUzvVqNTFaFnkYwgjuKdJdafqUFb.Update();
			}
		}
	}
}
