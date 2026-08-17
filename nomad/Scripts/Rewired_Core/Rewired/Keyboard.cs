using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Rewired.Interfaces;
using Rewired.Internal;
using Rewired.Internal.Glyphs;
using Rewired.Internal.Localization;
using Rewired.Utils;
using Rewired.Utils.Interfaces;
using UnityEngine;

namespace Rewired
{
	public sealed class Keyboard : ControllerWithMap
	{
		[CustomObfuscation(rename = false)]
		[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
		internal class ModifierKeyInfo
		{
			public readonly string shortName;

			public readonly string longName;

			public readonly string shortKey;

			public readonly string longKey;

			public ModifierKeyInfo(string P_0, string P_1, string P_2, string P_3)
			{
				shortName = P_0;
				longName = P_1;
				shortKey = P_2;
				longKey = P_3;
			}

			public string GetName(bool useShort)
			{
				if (!useShort)
				{
					return longName;
				}
				return shortName;
			}

			public string GetKey(bool useShort)
			{
				if (!useShort)
				{
					return longKey;
				}
				return shortKey;
			}
		}

		private class hKTgpYgxXoyKqixmyOKxoqQRgevz
		{
			public readonly vCSAbTdyeEeiafFQJchQEZOiqylR yAFaUuNCWeCHgZXMtofykrLdieOH;

			public readonly vCSAbTdyeEeiafFQJchQEZOiqylR isvUGlWCeRDyNcyoxlicuxrOAheyA;

			public hKTgpYgxXoyKqixmyOKxoqQRgevz(string P_0, string P_1)
			{
				if (!string.IsNullOrEmpty(P_0))
				{
					yAFaUuNCWeCHgZXMtofykrLdieOH = new vCSAbTdyeEeiafFQJchQEZOiqylR(new LocalizedString());
				}
				if (!string.IsNullOrEmpty(P_1))
				{
					isvUGlWCeRDyNcyoxlicuxrOAheyA = new vCSAbTdyeEeiafFQJchQEZOiqylR(new LocalizedString());
				}
			}
		}

		private sealed class vCSAbTdyeEeiafFQJchQEZOiqylR
		{
			public readonly LocalizedString vJBvocnRnBEzWRTRvcRPtuEWfLkn;

			public bool KJWNNWyAWMvuMSnNtOgGoBkrCsjv;

			public vCSAbTdyeEeiafFQJchQEZOiqylR(LocalizedString P_0)
			{
				vJBvocnRnBEzWRTRvcRPtuEWfLkn = P_0;
			}
		}

		private sealed class KRUBTtmepKkaxYgilRfXuIvpSzsp
		{
			public readonly KeyedGlyph wNIYTfDmqPPqDLcYniMqepxYCtWG;

			public bool WpRRNfPyEpJcGeLfIZJurShhnupV;

			public KRUBTtmepKkaxYgilRfXuIvpSzsp(KeyedGlyph P_0)
			{
				wNIYTfDmqPPqDLcYniMqepxYCtWG = P_0;
			}
		}

		private sealed class flKoFuNjAuzINtgXHskZrFVmxrnq : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
		{
			private int wFtQoOVDienvvDFsTqfpZuFIoSep;

			private ControllerPollingInfo wYrgZEHrlMzPeqnwsXXUHnbULniBb;

			private int jZrBvpcgjVKfmGFakWwRQRbfMrOFA;

			public Keyboard KSQBxvcyAvdcHASDfPZHChUFxzziB;

			private int cCawlaflyQKVBGAgZqslFeNxcLyM;

			private int DTrlFXpHKVwtBzStdXFCBnzBRHZJ;

			ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
			{
				[DebuggerHidden]
				get
				{
					return wYrgZEHrlMzPeqnwsXXUHnbULniBb;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return wYrgZEHrlMzPeqnwsXXUHnbULniBb;
				}
			}

			[DebuggerHidden]
			public flKoFuNjAuzINtgXHskZrFVmxrnq(int P_0)
			{
				wFtQoOVDienvvDFsTqfpZuFIoSep = P_0;
				jZrBvpcgjVKfmGFakWwRQRbfMrOFA = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				wFtQoOVDienvvDFsTqfpZuFIoSep = -2;
			}

			private bool MoveNext()
			{
				int num = wFtQoOVDienvvDFsTqfpZuFIoSep;
				Keyboard kSQBxvcyAvdcHASDfPZHChUFxzziB = KSQBxvcyAvdcHASDfPZHChUFxzziB;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					wFtQoOVDienvvDFsTqfpZuFIoSep = -1;
					goto IL_00bf;
				}
				wFtQoOVDienvvDFsTqfpZuFIoSep = -1;
				if (ReInput._id != kSQBxvcyAvdcHASDfPZHChUFxzziB.SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(kSQBxvcyAvdcHASDfPZHChUFxzziB.SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return false;
				}
				cCawlaflyQKVBGAgZqslFeNxcLyM = Consts.keyboardKeyValues.Count;
				DTrlFXpHKVwtBzStdXFCBnzBRHZJ = 0;
				goto IL_00cf;
				IL_00cf:
				if (DTrlFXpHKVwtBzStdXFCBnzBRHZJ < cCawlaflyQKVBGAgZqslFeNxcLyM)
				{
					KeyCode keyCode = (KeyCode)Consts.keyboardKeyValues[DTrlFXpHKVwtBzStdXFCBnzBRHZJ];
					if (kSQBxvcyAvdcHASDfPZHChUFxzziB.GetKey(keyCode))
					{
						wYrgZEHrlMzPeqnwsXXUHnbULniBb = new ControllerPollingInfo(true, -1, kSQBxvcyAvdcHASDfPZHChUFxzziB.id, kSQBxvcyAvdcHASDfPZHChUFxzziB._name, kSQBxvcyAvdcHASDfPZHChUFxzziB._type, ControllerElementType.Button, DTrlFXpHKVwtBzStdXFCBnzBRHZJ, Pole.Positive, GetKeyName(keyCode), kSQBxvcyAvdcHASDfPZHChUFxzziB.UNRIOyvPojfCPrjRsEYcHBwwkZqS.buttonElementIdentifierIds[DTrlFXpHKVwtBzStdXFCBnzBRHZJ], keyCode);
						wFtQoOVDienvvDFsTqfpZuFIoSep = 1;
						return true;
					}
					goto IL_00bf;
				}
				return false;
				IL_00bf:
				DTrlFXpHKVwtBzStdXFCBnzBRHZJ++;
				goto IL_00cf;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
			{
				flKoFuNjAuzINtgXHskZrFVmxrnq flKoFuNjAuzINtgXHskZrFVmxrnq2;
				if (wFtQoOVDienvvDFsTqfpZuFIoSep == -2 && jZrBvpcgjVKfmGFakWwRQRbfMrOFA == Environment.CurrentManagedThreadId)
				{
					wFtQoOVDienvvDFsTqfpZuFIoSep = 0;
					flKoFuNjAuzINtgXHskZrFVmxrnq2 = this;
				}
				else
				{
					flKoFuNjAuzINtgXHskZrFVmxrnq2 = new flKoFuNjAuzINtgXHskZrFVmxrnq(0);
					flKoFuNjAuzINtgXHskZrFVmxrnq2.KSQBxvcyAvdcHASDfPZHChUFxzziB = KSQBxvcyAvdcHASDfPZHChUFxzziB;
				}
				return flKoFuNjAuzINtgXHskZrFVmxrnq2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
			}
		}

		private sealed class MibcxHYlElBMgayNjhEXcRugpquM : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
		{
			private int onklBiDqVJXRXadLldkPsfqNVfVg;

			private ControllerPollingInfo vieDIGdBRjAotcfLiERvHrKtajBDB;

			private int YHpfKiBSXDziIHifdrjuTFXyADsUA;

			public Keyboard yooQNnXCAViGpQTkmmenbjtnqFGS;

			private int BAEIuFueUFcsnKGFjhHXPGjhCtrw;

			private int QWyArNwPUjGFsDgeqmfKEjGKhdoAb;

			ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
			{
				[DebuggerHidden]
				get
				{
					return vieDIGdBRjAotcfLiERvHrKtajBDB;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return vieDIGdBRjAotcfLiERvHrKtajBDB;
				}
			}

			[DebuggerHidden]
			public MibcxHYlElBMgayNjhEXcRugpquM(int P_0)
			{
				onklBiDqVJXRXadLldkPsfqNVfVg = P_0;
				YHpfKiBSXDziIHifdrjuTFXyADsUA = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				onklBiDqVJXRXadLldkPsfqNVfVg = -2;
			}

			private bool MoveNext()
			{
				int num = onklBiDqVJXRXadLldkPsfqNVfVg;
				Keyboard keyboard = yooQNnXCAViGpQTkmmenbjtnqFGS;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					onklBiDqVJXRXadLldkPsfqNVfVg = -1;
					goto IL_00bf;
				}
				onklBiDqVJXRXadLldkPsfqNVfVg = -1;
				if (ReInput._id != keyboard.SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(keyboard.SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return false;
				}
				BAEIuFueUFcsnKGFjhHXPGjhCtrw = Consts.keyboardKeyValues.Count;
				QWyArNwPUjGFsDgeqmfKEjGKhdoAb = 0;
				goto IL_00cf;
				IL_00cf:
				if (QWyArNwPUjGFsDgeqmfKEjGKhdoAb < BAEIuFueUFcsnKGFjhHXPGjhCtrw)
				{
					KeyCode keyCode = (KeyCode)Consts.keyboardKeyValues[QWyArNwPUjGFsDgeqmfKEjGKhdoAb];
					if (keyboard.GetKeyDown(keyCode))
					{
						vieDIGdBRjAotcfLiERvHrKtajBDB = new ControllerPollingInfo(true, -1, keyboard.id, keyboard._name, keyboard._type, ControllerElementType.Button, QWyArNwPUjGFsDgeqmfKEjGKhdoAb, Pole.Positive, GetKeyName(keyCode), keyboard.UNRIOyvPojfCPrjRsEYcHBwwkZqS.buttonElementIdentifierIds[QWyArNwPUjGFsDgeqmfKEjGKhdoAb], keyCode);
						onklBiDqVJXRXadLldkPsfqNVfVg = 1;
						return true;
					}
					goto IL_00bf;
				}
				return false;
				IL_00bf:
				QWyArNwPUjGFsDgeqmfKEjGKhdoAb++;
				goto IL_00cf;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
			{
				MibcxHYlElBMgayNjhEXcRugpquM mibcxHYlElBMgayNjhEXcRugpquM;
				if (onklBiDqVJXRXadLldkPsfqNVfVg == -2 && YHpfKiBSXDziIHifdrjuTFXyADsUA == Environment.CurrentManagedThreadId)
				{
					onklBiDqVJXRXadLldkPsfqNVfVg = 0;
					mibcxHYlElBMgayNjhEXcRugpquM = this;
				}
				else
				{
					mibcxHYlElBMgayNjhEXcRugpquM = new MibcxHYlElBMgayNjhEXcRugpquM(0);
					mibcxHYlElBMgayNjhEXcRugpquM.yooQNnXCAViGpQTkmmenbjtnqFGS = yooQNnXCAViGpQTkmmenbjtnqFGS;
				}
				return mibcxHYlElBMgayNjhEXcRugpquM;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
			}
		}

		private const string uDzWRlGOfxdshVHmlwhUQECTPzmc = " + ";

		private static Keyboard MsYNDZQSGtlUWILHMuwLaAOEcSoKA;

		private static KeyboardKeyCode[] vdzcXZWHcWWaoWKpHnUyNinaKGjH;

		private static Guid YhoZWRpzdUdnHJNLEEiMNveAAEyj;

		private readonly IUnifiedKeyboardSource PmfpAVrMbUeEljHNTyoVAMvKAPix;

		private ModifierKeyFlags ByjNwkBBdZiVmsklegJbBWSOyUDN;

		private ModifierKeyFlags qkviPSvkzzvCiSzeeoQhMmEFpltP;

		private Func<KeyboardKeyCode, int> WtHaLKPWjkBvzCEUxsawYzTeEYGR;

		private readonly int[] JVvCZJNdfnjhRQKIJusNFlTJcDPGA;

		private readonly int lPHThaplHwlJHQTQQvjJMSKMwolA;

		private readonly zilgekDXvTkMzThzckfvYmnJXPaEA MkJdZwANHKdkzjdlEkifBFyFznrUb;

		private readonly vNAwabfVGwBmZXFoOexLVfwdqKMS QpFHdDNgAJEmnUGLEGDKEDugCLNHA;

		private Dictionary<int, hKTgpYgxXoyKqixmyOKxoqQRgevz> FnwTAwjIuJLOrziMLPtRAbOqeJPjA;

		private Dictionary<int, KRUBTtmepKkaxYgilRfXuIvpSzsp> DAYPOHCkiZIYUCuXwPmJUOayzdybA;

		private static KeyboardKeyCode[] iYePDMCuPVhCqmzyFDPkEIENRxyk
		{
			get
			{
				if (vdzcXZWHcWWaoWKpHnUyNinaKGjH == null)
				{
					int[] keyboardKeyValues = Consts._keyboardKeyValues;
					int num = keyboardKeyValues.Length;
					vdzcXZWHcWWaoWKpHnUyNinaKGjH = new KeyboardKeyCode[num];
					for (int i = 0; i < num; i++)
					{
						vdzcXZWHcWWaoWKpHnUyNinaKGjH[i] = (KeyboardKeyCode)keyboardKeyValues[i];
					}
				}
				return vdzcXZWHcWWaoWKpHnUyNinaKGjH;
			}
		}

		private Dictionary<int, hKTgpYgxXoyKqixmyOKxoqQRgevz> GvimtgFeccJlfskDQKwXaBDyaqJ
		{
			get
			{
				if (FnwTAwjIuJLOrziMLPtRAbOqeJPjA == null)
				{
					Rewired.Utils.Interfaces.IReadOnlyDictionary<int, ModifierKeyInfo> modifierKeyInfo = Consts.modifierKeyInfo;
					Dictionary<int, hKTgpYgxXoyKqixmyOKxoqQRgevz> dictionary = new Dictionary<int, hKTgpYgxXoyKqixmyOKxoqQRgevz>();
					foreach (KeyValuePair<int, ModifierKeyInfo> item in modifierKeyInfo)
					{
						if (item.Key != 0)
						{
							dictionary.Add(item.Key, new hKTgpYgxXoyKqixmyOKxoqQRgevz(item.Value.shortKey, item.Value.longKey));
						}
					}
					FnwTAwjIuJLOrziMLPtRAbOqeJPjA = dictionary;
				}
				return FnwTAwjIuJLOrziMLPtRAbOqeJPjA;
			}
		}

		private Dictionary<int, KRUBTtmepKkaxYgilRfXuIvpSzsp> KZxBNkvvuOaeWhMcYGvQXYIvFJDEA
		{
			get
			{
				if (DAYPOHCkiZIYUCuXwPmJUOayzdybA == null)
				{
					Rewired.Utils.Interfaces.IReadOnlyDictionary<int, ModifierKeyInfo> modifierKeyInfo = Consts.modifierKeyInfo;
					Dictionary<int, KRUBTtmepKkaxYgilRfXuIvpSzsp> dictionary = new Dictionary<int, KRUBTtmepKkaxYgilRfXuIvpSzsp>();
					foreach (KeyValuePair<int, ModifierKeyInfo> item in modifierKeyInfo)
					{
						if (item.Key != 0)
						{
							KRUBTtmepKkaxYgilRfXuIvpSzsp value = new KRUBTtmepKkaxYgilRfXuIvpSzsp(new KeyedGlyph());
							dictionary.Add(item.Key, value);
						}
					}
					DAYPOHCkiZIYUCuXwPmJUOayzdybA = dictionary;
				}
				return DAYPOHCkiZIYUCuXwPmJUOayzdybA;
			}
		}

		Guid Controller.deviceInstanceGuid
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return Guid.Empty;
				}
				return YhoZWRpzdUdnHJNLEEiMNveAAEyj;
			}
		}

		internal Keyboard(string P_0, IUnifiedKeyboardSource P_1)
			: this(0, P_1.inputSource, P_0, InputTools.FormatHardwareIdentifierString(P_0), P_1.hardwareMap, 132, P_1?.controllerExtension, new ControllerDataUpdater(P_1.inputSource, 0, 132, null))
		{
			YhoZWRpzdUdnHJNLEEiMNveAAEyj = MiscTools.CreateGuidHashSHA1("[Universal Keyboard]");
			MkJdZwANHKdkzjdlEkifBFyFznrUb = new zilgekDXvTkMzThzckfvYmnJXPaEA(delegate
			{
				IList<ModifierKey> values = EnumValueHelper<ModifierKey>.Default.values;
				for (int i = 0; i < values.Count; i++)
				{
					lomnRghkRUsasIDrsLJMlCcqwNOi(values[i], true);
					lomnRghkRUsasIDrsLJMlCcqwNOi(values[i], false);
				}
			});
			QpFHdDNgAJEmnUGLEGDKEDugCLNHA = new vNAwabfVGwBmZXFoOexLVfwdqKMS(delegate
			{
				IList<ModifierKey> values = EnumValueHelper<ModifierKey>.Default.values;
				for (int i = 0; i < values.Count; i++)
				{
					PgxuvwIqTmNJYLamUqWAjpFBmSJJ(values[i]);
				}
			});
			int[] keyboardKeyValues = Consts._keyboardKeyValues;
			int num = keyboardKeyValues.Length;
			for (int num2 = 0; num2 < num; num2++)
			{
				if (keyboardKeyValues[num2] > lPHThaplHwlJHQTQQvjJMSKMwolA)
				{
					lPHThaplHwlJHQTQQvjJMSKMwolA = keyboardKeyValues[num2];
				}
			}
			JVvCZJNdfnjhRQKIJusNFlTJcDPGA = new int[lPHThaplHwlJHQTQQvjJMSKMwolA + 1];
			ArrayTools.Fill(JVvCZJNdfnjhRQKIJusNFlTJcDPGA, -1);
			for (int num3 = 0; num3 < num; num3++)
			{
				JVvCZJNdfnjhRQKIJusNFlTJcDPGA[keyboardKeyValues[num3]] = num3;
			}
			PmfpAVrMbUeEljHNTyoVAMvKAPix = P_1;
			if (LocalizationManager.isEnabled && LocalizationManager.autoPrefetch)
			{
				((cAwfhgIDGfMqIqwFGxVCNiWfViqT)MkJdZwANHKdkzjdlEkifBFyFznrUb).Localize();
			}
			if (GlyphManager.isEnabled && GlyphManager.autoPrefetch)
			{
				((IPrefetch)QpFHdDNgAJEmnUGLEGDKEDugCLNHA).Prefetch();
			}
			sXPBxAVgVVidzfPmKZUCZYhRwaIf();
		}

		private Keyboard(int P_0, InputSource P_1, string P_2, string P_3, HardwareControllerMap_Game P_4, int P_5, Extension P_6, ControllerDataUpdater P_7)
			: base(P_0, P_1, P_2, P_2, P_3, ControllerType.Keyboard, Consts.hardwareTypeGuid_universalKeyboard, P_5, null, P_4, P_6, P_7)
		{
			MsYNDZQSGtlUWILHMuwLaAOEcSoKA = this;
		}

		public bool GetKey(KeyCode keyCode)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			if ((uint)keyCode > (uint)lPHThaplHwlJHQTQQvjJMSKMwolA)
			{
				return false;
			}
			int num = JVvCZJNdfnjhRQKIJusNFlTJcDPGA[(int)keyCode];
			if (num < 0)
			{
				return false;
			}
			return buttons[num].value;
		}

		public bool GetKeyDown(KeyCode keyCode)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			if ((uint)keyCode > (uint)lPHThaplHwlJHQTQQvjJMSKMwolA)
			{
				return false;
			}
			int num = JVvCZJNdfnjhRQKIJusNFlTJcDPGA[(int)keyCode];
			if (num < 0)
			{
				return false;
			}
			return buttons[num].justPressed;
		}

		public bool GetKeyUp(KeyCode keyCode)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			if ((uint)keyCode > (uint)lPHThaplHwlJHQTQQvjJMSKMwolA)
			{
				return false;
			}
			int num = JVvCZJNdfnjhRQKIJusNFlTJcDPGA[(int)keyCode];
			if (num < 0)
			{
				return false;
			}
			return buttons[num].justReleased;
		}

		public bool GetKeyDoublePressHold(KeyCode keyCode, float speed)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			if ((uint)keyCode > (uint)lPHThaplHwlJHQTQQvjJMSKMwolA)
			{
				return false;
			}
			int num = JVvCZJNdfnjhRQKIJusNFlTJcDPGA[(int)keyCode];
			if (num < 0)
			{
				return false;
			}
			return buttons[num].DoublePressedAndHeld(speed);
		}

		public bool GetKeyDoublePressHold(KeyCode keyCode)
		{
			if ((uint)keyCode > (uint)lPHThaplHwlJHQTQQvjJMSKMwolA)
			{
				return false;
			}
			int num = JVvCZJNdfnjhRQKIJusNFlTJcDPGA[(int)keyCode];
			if (num < 0)
			{
				return false;
			}
			return buttons[num].DoublePressedAndHeld(0f);
		}

		public bool GetKeyDoublePressDown(KeyCode keyCode, float speed)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			if ((uint)keyCode > (uint)lPHThaplHwlJHQTQQvjJMSKMwolA)
			{
				return false;
			}
			int num = JVvCZJNdfnjhRQKIJusNFlTJcDPGA[(int)keyCode];
			if (num < 0)
			{
				return false;
			}
			return buttons[num].JustDoublePressed(speed);
		}

		public bool GetKeyDoublePressDown(KeyCode keyCode)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			if ((uint)keyCode > (uint)lPHThaplHwlJHQTQQvjJMSKMwolA)
			{
				return false;
			}
			int num = JVvCZJNdfnjhRQKIJusNFlTJcDPGA[(int)keyCode];
			if (num < 0)
			{
				return false;
			}
			return buttons[num].JustDoublePressed(0f);
		}

		public bool GetKeyPrev(KeyCode keyCode)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			if ((uint)keyCode > (uint)lPHThaplHwlJHQTQQvjJMSKMwolA)
			{
				return false;
			}
			int num = JVvCZJNdfnjhRQKIJusNFlTJcDPGA[(int)keyCode];
			if (num < 0)
			{
				return false;
			}
			return buttons[num].valuePrev;
		}

		public double GetKeyTimePressed(KeyCode keyCode)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0.0;
			}
			if ((uint)keyCode > (uint)lPHThaplHwlJHQTQQvjJMSKMwolA)
			{
				return 0.0;
			}
			int num = JVvCZJNdfnjhRQKIJusNFlTJcDPGA[(int)keyCode];
			if (num < 0)
			{
				return 0.0;
			}
			return buttons[num].timePressed;
		}

		public double GetKeyTimeUnpressed(KeyCode keyCode)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0.0;
			}
			if ((uint)keyCode > (uint)lPHThaplHwlJHQTQQvjJMSKMwolA)
			{
				return 0.0;
			}
			int num = JVvCZJNdfnjhRQKIJusNFlTJcDPGA[(int)keyCode];
			if (num < 0)
			{
				return 0.0;
			}
			return buttons[num].timeUnpressed;
		}

		public bool GetModifierKey(ModifierKey key)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			if (!mmrFodFGHJFsYXPiVypjHQqfwNWd(out var button, out var button2, key))
			{
				return false;
			}
			if (button.value || button2.value)
			{
				return true;
			}
			return false;
		}

		public bool GetModifierKeyDown(ModifierKey key)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			if (!mmrFodFGHJFsYXPiVypjHQqfwNWd(out var button, out var button2, key))
			{
				return false;
			}
			if (!button.value && !button2.value)
			{
				return false;
			}
			if (button.valuePrev || button2.valuePrev)
			{
				return false;
			}
			return true;
		}

		public bool GetModifierKeyUp(ModifierKey key)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			if (!mmrFodFGHJFsYXPiVypjHQqfwNWd(out var button, out var button2, key))
			{
				return false;
			}
			if (button.value || button2.value)
			{
				return false;
			}
			if (!button.valuePrev && !button2.valuePrev)
			{
				return false;
			}
			return true;
		}

		public bool GetModifierKeyPrev(ModifierKey key)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			if (!mmrFodFGHJFsYXPiVypjHQqfwNWd(out var button, out var button2, key))
			{
				return false;
			}
			if (button.valuePrev || button2.valuePrev)
			{
				return true;
			}
			return false;
		}

		public double GetModifierKeyTimePressed(ModifierKey key)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0.0;
			}
			if (!mmrFodFGHJFsYXPiVypjHQqfwNWd(out var button, out var button2, key))
			{
				return 0.0;
			}
			return MathTools.Max(button.timePressed, button2.timePressed);
		}

		public double GetModifierKeyTimeUnpressed(ModifierKey key)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0.0;
			}
			if (!mmrFodFGHJFsYXPiVypjHQqfwNWd(out var button, out var button2, key))
			{
				return 0.0;
			}
			return MathTools.Min(button.timeUnpressed, button2.timeUnpressed);
		}

		public KeyCode GetKeyCodeByButtonIndex(int buttonIndex)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return KeyCode.None;
			}
			return LEsjdrBwtqnvhqHMemfJpPhGlorHb(GetKeyboardKeyCodeByButtonIndex(buttonIndex));
		}

		public KeyCode GetKeyCodeById(int elementIdentifierId)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return KeyCode.None;
			}
			return GetKeyCodeByButtonIndex(GetButtonIndexById(elementIdentifierId));
		}

		public int GetButtonIndexByKeyCode(KeyCode keyCode)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return -1;
			}
			if ((uint)keyCode > (uint)lPHThaplHwlJHQTQQvjJMSKMwolA)
			{
				return -1;
			}
			return JVvCZJNdfnjhRQKIJusNFlTJcDPGA[(int)keyCode];
		}

		public ControllerElementIdentifier GetElementIdentifierByKeyCode(KeyCode keyCode)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return null;
			}
			if ((uint)keyCode > (uint)lPHThaplHwlJHQTQQvjJMSKMwolA)
			{
				return null;
			}
			int num = JVvCZJNdfnjhRQKIJusNFlTJcDPGA[(int)keyCode];
			if (num < 0)
			{
				return null;
			}
			return UNRIOyvPojfCPrjRsEYcHBwwkZqS.buttonElementIdentifiers_cache[num];
		}

		public ControllerPollingInfo PollForFirstKey()
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
			}
			int count = Consts.keyboardKeyValues.Count;
			for (int i = 0; i < count; i++)
			{
				KeyCode keyCode = (KeyCode)Consts.keyboardKeyValues[i];
				if (GetKey(keyCode))
				{
					return new ControllerPollingInfo(true, -1, id, _name, _type, ControllerElementType.Button, i, Pole.Positive, GetKeyName(keyCode), UNRIOyvPojfCPrjRsEYcHBwwkZqS.buttonElementIdentifierIds[i], keyCode);
				}
			}
			return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
		}

		[IteratorStateMachine(typeof(flKoFuNjAuzINtgXHskZrFVmxrnq))]
		public IEnumerable<ControllerPollingInfo> PollForAllKeys()
		{
			return new flKoFuNjAuzINtgXHskZrFVmxrnq(-2)
			{
				KSQBxvcyAvdcHASDfPZHChUFxzziB = this
			};
		}

		[IteratorStateMachine(typeof(MibcxHYlElBMgayNjhEXcRugpquM))]
		public IEnumerable<ControllerPollingInfo> PollForAllKeysDown()
		{
			return new MibcxHYlElBMgayNjhEXcRugpquM(-2)
			{
				yooQNnXCAViGpQTkmmenbjtnqFGS = this
			};
		}

		public ControllerPollingInfo PollForFirstKeyDown()
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
			}
			int count = Consts.keyboardKeyValues.Count;
			for (int i = 0; i < count; i++)
			{
				KeyCode keyCode = (KeyCode)Consts.keyboardKeyValues[i];
				if (GetKeyDown(keyCode))
				{
					return new ControllerPollingInfo(true, -1, id, _name, _type, ControllerElementType.Button, i, Pole.Positive, GetKeyName(keyCode), UNRIOyvPojfCPrjRsEYcHBwwkZqS.buttonElementIdentifierIds[i], keyCode);
				}
			}
			return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
		}

		public override ControllerPollingInfo PollForFirstButton()
		{
			return PollForFirstKey();
		}

		public override ControllerPollingInfo PollForFirstButtonDown()
		{
			return PollForFirstKeyDown();
		}

		public override IEnumerable<ControllerPollingInfo> PollForAllButtons()
		{
			return PollForAllKeys();
		}

		public override IEnumerable<ControllerPollingInfo> PollForAllButtonsDown()
		{
			return PollForAllKeysDown();
		}

		public static bool IsModifierKey(KeyCode key)
		{
			switch (key)
			{
			case KeyCode.None:
				return false;
			case KeyCode.RightShift:
			case KeyCode.LeftShift:
			case KeyCode.RightControl:
			case KeyCode.LeftControl:
			case KeyCode.RightAlt:
			case KeyCode.LeftAlt:
			case KeyCode.RightMeta:
			case KeyCode.LeftMeta:
				return true;
			default:
				return false;
			}
		}

		internal static bool PNRNpjuyOsRxxkmimfMjvioOgigM(KeyboardKeyCode P_0)
		{
			switch (P_0)
			{
			case KeyboardKeyCode.None:
				return false;
			case KeyboardKeyCode.RightShift:
			case KeyboardKeyCode.LeftShift:
			case KeyboardKeyCode.RightControl:
			case KeyboardKeyCode.LeftControl:
			case KeyboardKeyCode.RightAlt:
			case KeyboardKeyCode.LeftAlt:
			case KeyboardKeyCode.RightCommand:
			case KeyboardKeyCode.LeftCommand:
				return true;
			default:
				return false;
			}
		}

		public static ModifierKey KeyCodeToModifierKey(KeyCode key)
		{
			switch (key)
			{
			case KeyCode.None:
				return ModifierKey.None;
			case KeyCode.RightControl:
			case KeyCode.LeftControl:
				return ModifierKey.Control;
			case KeyCode.RightAlt:
			case KeyCode.LeftAlt:
				return ModifierKey.Alt;
			case KeyCode.RightMeta:
			case KeyCode.LeftMeta:
				return ModifierKey.Command;
			case KeyCode.RightShift:
			case KeyCode.LeftShift:
				return ModifierKey.Shift;
			default:
				return ModifierKey.None;
			}
		}

		public static ModifierKeyFlags KeyCodeToModifierKeyFlags(KeyCode key)
		{
			return key switch
			{
				KeyCode.LeftControl => ModifierKeyFlags.LeftControl, 
				KeyCode.RightControl => ModifierKeyFlags.RightControl, 
				KeyCode.LeftAlt => ModifierKeyFlags.LeftAlt, 
				KeyCode.RightAlt => ModifierKeyFlags.RightAlt, 
				KeyCode.LeftShift => ModifierKeyFlags.LeftShift, 
				KeyCode.RightShift => ModifierKeyFlags.RightShift, 
				KeyCode.LeftMeta => ModifierKeyFlags.LeftCommand, 
				KeyCode.RightMeta => ModifierKeyFlags.RightCommand, 
				_ => ModifierKeyFlags.None, 
			};
		}

		public static bool ModifierKeyFlagsContain(ModifierKeyFlags flags, ModifierKey key)
		{
			switch (key)
			{
			case ModifierKey.None:
				return false;
			case ModifierKey.Control:
				if ((flags & ModifierKeyFlags.LeftControl) == ModifierKeyFlags.LeftControl)
				{
					return true;
				}
				if ((flags & ModifierKeyFlags.RightControl) == ModifierKeyFlags.RightControl)
				{
					return true;
				}
				return false;
			case ModifierKey.Alt:
				if ((flags & ModifierKeyFlags.LeftAlt) == ModifierKeyFlags.LeftAlt)
				{
					return true;
				}
				if ((flags & ModifierKeyFlags.RightAlt) == ModifierKeyFlags.RightAlt)
				{
					return true;
				}
				return false;
			case ModifierKey.Shift:
				if ((flags & ModifierKeyFlags.LeftShift) == ModifierKeyFlags.LeftShift)
				{
					return true;
				}
				if ((flags & ModifierKeyFlags.RightShift) == ModifierKeyFlags.RightShift)
				{
					return true;
				}
				return false;
			case ModifierKey.Command:
				if ((flags & ModifierKeyFlags.LeftCommand) == ModifierKeyFlags.LeftCommand)
				{
					return true;
				}
				if ((flags & ModifierKeyFlags.RightCommand) == ModifierKeyFlags.RightCommand)
				{
					return true;
				}
				return false;
			default:
				return false;
			}
		}

		public static bool ModifierKeyFlagsContain(ModifierKeyFlags flags, KeyCode key)
		{
			switch (key)
			{
			case KeyCode.None:
				return false;
			case KeyCode.LeftControl:
				if ((flags & ModifierKeyFlags.LeftControl) == ModifierKeyFlags.LeftControl)
				{
					return true;
				}
				return false;
			case KeyCode.RightControl:
				if ((flags & ModifierKeyFlags.RightControl) == ModifierKeyFlags.RightControl)
				{
					return true;
				}
				return false;
			case KeyCode.LeftAlt:
				if ((flags & ModifierKeyFlags.LeftAlt) == ModifierKeyFlags.LeftAlt)
				{
					return true;
				}
				return false;
			case KeyCode.RightAlt:
				if ((flags & ModifierKeyFlags.RightAlt) == ModifierKeyFlags.RightAlt)
				{
					return true;
				}
				return false;
			case KeyCode.LeftShift:
				if ((flags & ModifierKeyFlags.LeftShift) == ModifierKeyFlags.LeftShift)
				{
					return true;
				}
				return false;
			case KeyCode.RightShift:
				if ((flags & ModifierKeyFlags.RightShift) == ModifierKeyFlags.RightShift)
				{
					return true;
				}
				return false;
			case KeyCode.LeftMeta:
				if ((flags & ModifierKeyFlags.LeftCommand) == ModifierKeyFlags.LeftCommand)
				{
					return true;
				}
				return false;
			case KeyCode.RightMeta:
				if ((flags & ModifierKeyFlags.RightCommand) == ModifierKeyFlags.RightCommand)
				{
					return true;
				}
				return false;
			default:
				return false;
			}
		}

		public static ModifierKey ModifierKeyFlagsToModifierKey(ModifierKeyFlags flags)
		{
			if ((flags & ModifierKeyFlags.LeftControl) == ModifierKeyFlags.LeftControl)
			{
				return ModifierKey.Control;
			}
			if ((flags & ModifierKeyFlags.RightControl) == ModifierKeyFlags.RightControl)
			{
				return ModifierKey.Control;
			}
			if ((flags & ModifierKeyFlags.LeftAlt) == ModifierKeyFlags.LeftAlt)
			{
				return ModifierKey.Alt;
			}
			if ((flags & ModifierKeyFlags.RightAlt) == ModifierKeyFlags.RightAlt)
			{
				return ModifierKey.Alt;
			}
			if ((flags & ModifierKeyFlags.LeftShift) == ModifierKeyFlags.LeftShift)
			{
				return ModifierKey.Shift;
			}
			if ((flags & ModifierKeyFlags.RightShift) == ModifierKeyFlags.RightShift)
			{
				return ModifierKey.Shift;
			}
			if ((flags & ModifierKeyFlags.LeftCommand) == ModifierKeyFlags.LeftCommand)
			{
				return ModifierKey.Command;
			}
			if ((flags & ModifierKeyFlags.RightCommand) == ModifierKeyFlags.RightCommand)
			{
				return ModifierKey.Command;
			}
			return ModifierKey.None;
		}

		public static KeyCode ModifierKeyFlagsToKeyCode(ModifierKeyFlags flags)
		{
			if ((flags & ModifierKeyFlags.LeftControl) == ModifierKeyFlags.LeftControl)
			{
				return KeyCode.LeftControl;
			}
			if ((flags & ModifierKeyFlags.RightControl) == ModifierKeyFlags.RightControl)
			{
				return KeyCode.RightControl;
			}
			if ((flags & ModifierKeyFlags.LeftAlt) == ModifierKeyFlags.LeftAlt)
			{
				return KeyCode.LeftAlt;
			}
			if ((flags & ModifierKeyFlags.RightAlt) == ModifierKeyFlags.RightAlt)
			{
				return KeyCode.RightAlt;
			}
			if ((flags & ModifierKeyFlags.LeftShift) == ModifierKeyFlags.LeftShift)
			{
				return KeyCode.LeftShift;
			}
			if ((flags & ModifierKeyFlags.RightShift) == ModifierKeyFlags.RightShift)
			{
				return KeyCode.RightShift;
			}
			if ((flags & ModifierKeyFlags.LeftCommand) == ModifierKeyFlags.LeftCommand)
			{
				return KeyCode.LeftMeta;
			}
			if ((flags & ModifierKeyFlags.RightCommand) == ModifierKeyFlags.RightCommand)
			{
				return KeyCode.RightMeta;
			}
			return KeyCode.None;
		}

		public static ModifierKeyFlags ModifierKeyToModifierKeyFlags(ModifierKey key)
		{
			return key switch
			{
				ModifierKey.None => ModifierKeyFlags.None, 
				ModifierKey.Control => ModifierKeyFlags.LeftControl | ModifierKeyFlags.RightControl, 
				ModifierKey.Alt => ModifierKeyFlags.LeftAlt | ModifierKeyFlags.RightAlt, 
				ModifierKey.Shift => ModifierKeyFlags.LeftShift | ModifierKeyFlags.RightShift, 
				ModifierKey.Command => ModifierKeyFlags.LeftCommand | ModifierKeyFlags.RightCommand, 
				_ => ModifierKeyFlags.None, 
			};
		}

		public static string GetKeyName(KeyCode key)
		{
			if (MsYNDZQSGtlUWILHMuwLaAOEcSoKA == null)
			{
				return string.Empty;
			}
			int buttonIndex = MsYNDZQSGtlUWILHMuwLaAOEcSoKA.GetButtonIndex(PwxvwLyqqaZuWBnWTwcdeSZvdrjb(key));
			if (buttonIndex < 0)
			{
				return string.Empty;
			}
			return MsYNDZQSGtlUWILHMuwLaAOEcSoKA.ButtonElementIdentifiers[buttonIndex].Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Ename;
		}

		public static string GetKeyName(KeyCode key, ModifierKeyFlags flags)
		{
			string text = GetKeyName(key);
			if (flags != ModifierKeyFlags.None)
			{
				StringBuilder stringBuilder = new StringBuilder(text);
				stringBuilder.Append(" + ");
				stringBuilder.Append(ModifierKeyFlagsToString(flags));
				text = stringBuilder.ToString();
			}
			return text;
		}

		public static string GetModifierKeyName(ModifierKey modifierKey)
		{
			if (MsYNDZQSGtlUWILHMuwLaAOEcSoKA == null)
			{
				return string.Empty;
			}
			return MsYNDZQSGtlUWILHMuwLaAOEcSoKA.lomnRghkRUsasIDrsLJMlCcqwNOi(modifierKey, false);
		}

		public static string GetModifierKeyName(ModifierKey modifierKey, bool getShortName)
		{
			if (MsYNDZQSGtlUWILHMuwLaAOEcSoKA == null)
			{
				return string.Empty;
			}
			return MsYNDZQSGtlUWILHMuwLaAOEcSoKA.lomnRghkRUsasIDrsLJMlCcqwNOi(modifierKey, getShortName);
		}

		public static string ModifierKeyFlagsToString(ModifierKeyFlags flags, bool getShortName)
		{
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder();
			if (ModifierKeyFlagsContain(flags, ModifierKey.Control))
			{
				stringBuilder.Append(GetModifierKeyName(ModifierKey.Control, getShortName));
				num++;
			}
			if (ModifierKeyFlagsContain(flags, ModifierKey.Command))
			{
				if (num > 0)
				{
					stringBuilder.Append(" + ");
				}
				stringBuilder.Append(GetModifierKeyName(ModifierKey.Command, getShortName));
				num++;
			}
			if (ModifierKeyFlagsContain(flags, ModifierKey.Alt))
			{
				if (num > 0)
				{
					stringBuilder.Append(" + ");
				}
				stringBuilder.Append(GetModifierKeyName(ModifierKey.Alt, getShortName));
				num++;
			}
			if (num >= 3)
			{
				return stringBuilder.ToString();
			}
			if (ModifierKeyFlagsContain(flags, ModifierKey.Shift))
			{
				if (num > 0)
				{
					stringBuilder.Append(" + ");
				}
				stringBuilder.Append(GetModifierKeyName(ModifierKey.Shift, getShortName));
				num++;
			}
			return stringBuilder.ToString();
		}

		public static string ModifierKeyFlagsToString(ModifierKeyFlags flags)
		{
			return ModifierKeyFlagsToString(flags, getShortName: false);
		}

		public static object GetModifierKeyGlyph(ModifierKey modifierKey)
		{
			if (MsYNDZQSGtlUWILHMuwLaAOEcSoKA == null)
			{
				return null;
			}
			return MsYNDZQSGtlUWILHMuwLaAOEcSoKA.PgxuvwIqTmNJYLamUqWAjpFBmSJJ(modifierKey);
		}

		internal static string PlUzctevrihyWuRRIquNsVgXsNdv(ModifierKey P_0)
		{
			if (MsYNDZQSGtlUWILHMuwLaAOEcSoKA == null)
			{
				return string.Empty;
			}
			return MsYNDZQSGtlUWILHMuwLaAOEcSoKA.FNgcbYFikGVLODHiuAgrOtXkriGq(P_0);
		}

		internal static KeyboardKeyCode PwxvwLyqqaZuWBnWTwcdeSZvdrjb(KeyCode P_0)
		{
			return (KeyboardKeyCode)P_0;
		}

		internal static KeyCode LEsjdrBwtqnvhqHMemfJpPhGlorHb(KeyboardKeyCode P_0)
		{
			return (KeyCode)P_0;
		}

		internal static ModifierKeyFlags KZGGDTlKdXOOOfDRXfdLUWnhkIff(ModifierKeyFlags P_0)
		{
			if ((P_0 & ModifierKeyFlags.LeftControl) == ModifierKeyFlags.LeftControl)
			{
				P_0 |= ModifierKeyFlags.RightControl;
			}
			if ((P_0 & ModifierKeyFlags.RightControl) == ModifierKeyFlags.RightControl)
			{
				P_0 |= ModifierKeyFlags.LeftControl;
			}
			if ((P_0 & ModifierKeyFlags.LeftCommand) == ModifierKeyFlags.LeftCommand)
			{
				P_0 |= ModifierKeyFlags.RightCommand;
			}
			if ((P_0 & ModifierKeyFlags.RightCommand) == ModifierKeyFlags.RightCommand)
			{
				P_0 |= ModifierKeyFlags.LeftCommand;
			}
			if ((P_0 & ModifierKeyFlags.LeftAlt) == ModifierKeyFlags.LeftAlt)
			{
				P_0 |= ModifierKeyFlags.RightAlt;
			}
			if ((P_0 & ModifierKeyFlags.RightAlt) == ModifierKeyFlags.RightAlt)
			{
				P_0 |= ModifierKeyFlags.LeftAlt;
			}
			if ((P_0 & ModifierKeyFlags.LeftShift) == ModifierKeyFlags.LeftShift)
			{
				P_0 |= ModifierKeyFlags.RightShift;
			}
			if ((P_0 & ModifierKeyFlags.RightShift) == ModifierKeyFlags.RightShift)
			{
				P_0 |= ModifierKeyFlags.LeftShift;
			}
			return P_0;
		}

		internal static int coxaWKblOwgjwYqyMKIlfdTnNFQVA(ModifierKeyFlags P_0)
		{
			if (P_0 == ModifierKeyFlags.None)
			{
				return 0;
			}
			int num = 0;
			if ((P_0 & ModifierKeyFlags.LeftControl) == ModifierKeyFlags.LeftControl)
			{
				num++;
			}
			else if ((P_0 & ModifierKeyFlags.RightControl) == ModifierKeyFlags.RightControl)
			{
				num++;
			}
			if ((P_0 & ModifierKeyFlags.LeftCommand) == ModifierKeyFlags.LeftCommand)
			{
				num++;
			}
			else if ((P_0 & ModifierKeyFlags.RightCommand) == ModifierKeyFlags.RightCommand)
			{
				num++;
			}
			if ((P_0 & ModifierKeyFlags.LeftAlt) == ModifierKeyFlags.LeftAlt)
			{
				num++;
			}
			else if ((P_0 & ModifierKeyFlags.RightAlt) == ModifierKeyFlags.RightAlt)
			{
				num++;
			}
			if ((P_0 & ModifierKeyFlags.LeftShift) == ModifierKeyFlags.LeftShift)
			{
				num++;
			}
			else if ((P_0 & ModifierKeyFlags.RightShift) == ModifierKeyFlags.RightShift)
			{
				num++;
			}
			return num;
		}

		[CustomObfuscation(rename = false)]
		internal static KeyboardKeyCode GetKeyboardKeyCodeByButtonIndex(int buttonIndex)
		{
			if ((uint)buttonIndex > 132u)
			{
				return KeyboardKeyCode.None;
			}
			return iYePDMCuPVhCqmzyFDPkEIENRxyk[buttonIndex];
		}

		internal static int MMNqiODvdWTogJjoqPDqgGggOGKA(KeyboardKeyCode P_0)
		{
			int buttonIndex = MsYNDZQSGtlUWILHMuwLaAOEcSoKA.GetButtonIndex(P_0);
			if (buttonIndex < 0)
			{
				return -1;
			}
			return MsYNDZQSGtlUWILHMuwLaAOEcSoKA.ButtonElementIdentifiers[buttonIndex].Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid;
		}

		internal static void LaQHNKAihvpLcKIdMLsAabBjrttvb(ref int P_0, ref KeyCode P_1)
		{
			if (P_1 != KeyCode.None)
			{
				P_0 = MMNqiODvdWTogJjoqPDqgGggOGKA(PwxvwLyqqaZuWBnWTwcdeSZvdrjb(P_1));
			}
			else
			{
				P_1 = ReInput.FoarDfUMCtoVFquEtrllUhEjZUUn.TVvLxBfEgOqnloHdRFcagvmpmnZT.GetKeyCodeById(P_0);
			}
		}

		internal void bisEPjBplkJsAQISkrRusPWfmohcA(UpdateLoopType P_0)
		{
			PmfpAVrMbUeEljHNTyoVAMvKAPix.UpdateInputData(yZwGORAVRJPjNCmxxWIIoQgNomuqA);
			base.PTpLZPTdIGBCXbVzlMCHCqylApVQA(P_0);
			rEUHNNDKwwvJGDowlCWBxVpGuRwv();
		}

		internal void WNoRRIZbwaCOlzIxscrCaTKXKxmI(UpdateLoopType P_0)
		{
			buttons[ThreadSafeUnityInput.Keyboard.keyValueIndex_Escape].afeJkBinlRdwvHJUmFnGIJdBjiWVb(P_0, ThreadSafeUnityInput.Keyboard.keyValueIndex_Escape, yZwGORAVRJPjNCmxxWIIoQgNomuqA);
			buttons[ThreadSafeUnityInput.Keyboard.keyValueIndex_Menu].afeJkBinlRdwvHJUmFnGIJdBjiWVb(P_0, ThreadSafeUnityInput.Keyboard.keyValueIndex_Menu, yZwGORAVRJPjNCmxxWIIoQgNomuqA);
			buttons[ThreadSafeUnityInput.Keyboard.keyValueIndex_F2].afeJkBinlRdwvHJUmFnGIJdBjiWVb(P_0, ThreadSafeUnityInput.Keyboard.keyValueIndex_F2, yZwGORAVRJPjNCmxxWIIoQgNomuqA);
			buttons[ThreadSafeUnityInput.Keyboard.keyValueIndex_UpArrow].afeJkBinlRdwvHJUmFnGIJdBjiWVb(P_0, ThreadSafeUnityInput.Keyboard.keyValueIndex_UpArrow, yZwGORAVRJPjNCmxxWIIoQgNomuqA);
			buttons[ThreadSafeUnityInput.Keyboard.keyValueIndex_RightArrow].afeJkBinlRdwvHJUmFnGIJdBjiWVb(P_0, ThreadSafeUnityInput.Keyboard.keyValueIndex_RightArrow, yZwGORAVRJPjNCmxxWIIoQgNomuqA);
			buttons[ThreadSafeUnityInput.Keyboard.keyValueIndex_DownArrow].afeJkBinlRdwvHJUmFnGIJdBjiWVb(P_0, ThreadSafeUnityInput.Keyboard.keyValueIndex_DownArrow, yZwGORAVRJPjNCmxxWIIoQgNomuqA);
			buttons[ThreadSafeUnityInput.Keyboard.keyValueIndex_LeftArrow].afeJkBinlRdwvHJUmFnGIJdBjiWVb(P_0, ThreadSafeUnityInput.Keyboard.keyValueIndex_LeftArrow, yZwGORAVRJPjNCmxxWIIoQgNomuqA);
		}

		internal bool vyVDNYhSQnxupBsXRwjoODaobnaeb(KeyboardKeyCode P_0)
		{
			if ((uint)P_0 > (uint)lPHThaplHwlJHQTQQvjJMSKMwolA)
			{
				return false;
			}
			int num = JVvCZJNdfnjhRQKIJusNFlTJcDPGA[(int)P_0];
			if (num < 0)
			{
				return false;
			}
			return buttons[num].value;
		}

		internal bool UdeQIolUHEjUyrgAiSuRCxyDOFCE(KeyboardKeyCode P_0)
		{
			if ((uint)P_0 > (uint)lPHThaplHwlJHQTQQvjJMSKMwolA)
			{
				return false;
			}
			int num = JVvCZJNdfnjhRQKIJusNFlTJcDPGA[(int)P_0];
			if (num < 0)
			{
				return false;
			}
			return buttons[num].valuePrev;
		}

		internal bool zkOdMTnuSDBkqLNykibsCvZeIHhV(KeyboardKeyCode P_0, ModifierKeyFlags P_1)
		{
			if (!vyVDNYhSQnxupBsXRwjoODaobnaeb(P_0))
			{
				return false;
			}
			if (P_1 == ModifierKeyFlags.None)
			{
				return true;
			}
			if ((P_1 & qkviPSvkzzvCiSzeeoQhMmEFpltP) != P_1)
			{
				return false;
			}
			double keyTimePressed = GetKeyTimePressed((KeyCode)P_0);
			if ((P_1 & ModifierKeyFlags.LeftControl) == ModifierKeyFlags.LeftControl && keyTimePressed > GetModifierKeyTimePressed(ModifierKey.Control))
			{
				return false;
			}
			if ((P_1 & ModifierKeyFlags.LeftCommand) == ModifierKeyFlags.LeftCommand && keyTimePressed > GetModifierKeyTimePressed(ModifierKey.Command))
			{
				return false;
			}
			if ((P_1 & ModifierKeyFlags.LeftAlt) == ModifierKeyFlags.LeftAlt && keyTimePressed > GetModifierKeyTimePressed(ModifierKey.Alt))
			{
				return false;
			}
			if ((P_1 & ModifierKeyFlags.LeftShift) == ModifierKeyFlags.LeftShift && keyTimePressed > GetModifierKeyTimePressed(ModifierKey.Shift))
			{
				return false;
			}
			return true;
		}

		internal bool JYYJPkZCafvATfrODHCGWmLBdoyk(KeyboardKeyCode P_0, ModifierKeyFlags P_1)
		{
			if (vyVDNYhSQnxupBsXRwjoODaobnaeb(P_0))
			{
				return true;
			}
			if (GetModifierKey(ModifierKeyFlagsToModifierKey(P_1)))
			{
				return true;
			}
			return false;
		}

		[CustomObfuscation(rename = false)]
		internal int GetButtonIndex(KeyboardKeyCode keyCode)
		{
			if ((uint)keyCode > (uint)lPHThaplHwlJHQTQQvjJMSKMwolA)
			{
				return -1;
			}
			return JVvCZJNdfnjhRQKIJusNFlTJcDPGA[(int)keyCode];
		}

		[CustomObfuscation(rename = false)]
		internal void BakeMap(ControllerMap controllerMap)
		{
			if (controllerMap != null)
			{
				IList<ActionElementMap> list = controllerMap.LrIuPDSCedgUWTQghItEJWRAaExrA;
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					VnQORoKBKYcDfQniJOyRPalZgtMZ(controllerMap, list[i]);
				}
			}
		}

		[CustomObfuscation(rename = false)]
		internal void BakeActionElementMap(ControllerMap controllerMap, ActionElementMap map)
		{
			map?.CeDmmMmdwtjVdcVPRFXshDHqgijv(controllerMap);
		}

		internal void MeKBXWVmLOBdjdaucQqglkUjTIqq()
		{
			base.bglRweWaaTFfEiIQjwyzpBARhNXC();
			ByjNwkBBdZiVmsklegJbBWSOyUDN = ModifierKeyFlags.None;
			qkviPSvkzzvCiSzeeoQhMmEFpltP = ModifierKeyFlags.None;
		}

		internal bool VDAeuKfYCNuLgkCYQiRTEaMArQDJA(bool P_0)
		{
			if (!base.YMYaXjiCPrJkpmbpKNcXpVseIAcFA(P_0))
			{
				return false;
			}
			if (PmfpAVrMbUeEljHNTyoVAMvKAPix is IGetSetEnabled)
			{
				(PmfpAVrMbUeEljHNTyoVAMvKAPix as IGetSetEnabled).enabled = P_0;
			}
			return true;
		}

		private bool mmrFodFGHJFsYXPiVypjHQqfwNWd(out Button P_0, out Button P_1, ModifierKey P_2)
		{
			P_0 = null;
			P_1 = null;
			switch (P_2)
			{
			case ModifierKey.None:
				return false;
			case ModifierKey.Control:
				P_0 = buttons[JVvCZJNdfnjhRQKIJusNFlTJcDPGA[306]];
				P_1 = buttons[JVvCZJNdfnjhRQKIJusNFlTJcDPGA[305]];
				return true;
			case ModifierKey.Alt:
				P_0 = buttons[JVvCZJNdfnjhRQKIJusNFlTJcDPGA[308]];
				P_1 = buttons[JVvCZJNdfnjhRQKIJusNFlTJcDPGA[307]];
				return true;
			case ModifierKey.Command:
				P_0 = buttons[JVvCZJNdfnjhRQKIJusNFlTJcDPGA[310]];
				P_1 = buttons[JVvCZJNdfnjhRQKIJusNFlTJcDPGA[309]];
				return true;
			case ModifierKey.Shift:
				P_0 = buttons[JVvCZJNdfnjhRQKIJusNFlTJcDPGA[304]];
				P_1 = buttons[JVvCZJNdfnjhRQKIJusNFlTJcDPGA[303]];
				return true;
			default:
				return false;
			}
		}

		private void rEUHNNDKwwvJGDowlCWBxVpGuRwv()
		{
			ModifierKeyFlags modifierKeyFlags = ModifierKeyFlags.None;
			if (buttons[JVvCZJNdfnjhRQKIJusNFlTJcDPGA[306]].value)
			{
				modifierKeyFlags |= ModifierKeyFlags.LeftControl;
			}
			if (buttons[JVvCZJNdfnjhRQKIJusNFlTJcDPGA[305]].value)
			{
				modifierKeyFlags |= ModifierKeyFlags.RightControl;
			}
			if (buttons[JVvCZJNdfnjhRQKIJusNFlTJcDPGA[310]].value)
			{
				modifierKeyFlags |= ModifierKeyFlags.LeftCommand;
			}
			if (buttons[JVvCZJNdfnjhRQKIJusNFlTJcDPGA[309]].value)
			{
				modifierKeyFlags |= ModifierKeyFlags.RightCommand;
			}
			if (buttons[JVvCZJNdfnjhRQKIJusNFlTJcDPGA[308]].value)
			{
				modifierKeyFlags |= ModifierKeyFlags.LeftAlt;
			}
			if (buttons[JVvCZJNdfnjhRQKIJusNFlTJcDPGA[307]].value)
			{
				modifierKeyFlags |= ModifierKeyFlags.RightAlt;
			}
			if (buttons[JVvCZJNdfnjhRQKIJusNFlTJcDPGA[304]].value)
			{
				modifierKeyFlags |= ModifierKeyFlags.LeftShift;
			}
			if (buttons[JVvCZJNdfnjhRQKIJusNFlTJcDPGA[303]].value)
			{
				modifierKeyFlags |= ModifierKeyFlags.RightShift;
			}
			ByjNwkBBdZiVmsklegJbBWSOyUDN = modifierKeyFlags;
			qkviPSvkzzvCiSzeeoQhMmEFpltP = KZGGDTlKdXOOOfDRXfdLUWnhkIff(modifierKeyFlags);
		}

		private string lomnRghkRUsasIDrsLJMlCcqwNOi(ModifierKey P_0, bool P_1)
		{
			if (P_0 == ModifierKey.None)
			{
				return string.Empty;
			}
			ModifierKeyInfo modifierKeyInfo = Consts.modifierKeyInfo[(int)P_0];
			string result = modifierKeyInfo.GetName(P_1);
			if (!LocalizationManager.isEnabled)
			{
				return result;
			}
			if (!GvimtgFeccJlfskDQKwXaBDyaqJ.TryGetValue((int)P_0, out var value))
			{
				return result;
			}
			string result2;
			if (P_1)
			{
				if (value.yAFaUuNCWeCHgZXMtofykrLdieOH != null && IoDwLaqGWusRYDkLARGwqdHPySIf(value.yAFaUuNCWeCHgZXMtofykrLdieOH, modifierKeyInfo.shortKey, modifierKeyInfo.shortName, UNRIOyvPojfCPrjRsEYcHBwwkZqS.deviceLocalizationInfo, out result2))
				{
					return result2;
				}
				if (value.isvUGlWCeRDyNcyoxlicuxrOAheyA != null && IoDwLaqGWusRYDkLARGwqdHPySIf(value.isvUGlWCeRDyNcyoxlicuxrOAheyA, modifierKeyInfo.longKey, modifierKeyInfo.longName, UNRIOyvPojfCPrjRsEYcHBwwkZqS.deviceLocalizationInfo, out result2))
				{
					return result2;
				}
				return result;
			}
			if (value.isvUGlWCeRDyNcyoxlicuxrOAheyA == null)
			{
				return result;
			}
			IoDwLaqGWusRYDkLARGwqdHPySIf(value.isvUGlWCeRDyNcyoxlicuxrOAheyA, modifierKeyInfo.longKey, modifierKeyInfo.longName, UNRIOyvPojfCPrjRsEYcHBwwkZqS.deviceLocalizationInfo, out result2);
			return result2;
		}

		private static bool IoDwLaqGWusRYDkLARGwqdHPySIf(vCSAbTdyeEeiafFQJchQEZOiqylR P_0, string P_1, string P_2, DeviceLocalizationInfo P_3, out string P_4)
		{
			LocalizationManager.GetAndUpdateLocalizedStringResultFlags getAndUpdateLocalizedStringResultFlags = fNDBBZXbOAvGiTXVzfEmFadoOOjj.UAWmqofIfwJFRJqjERIiiqQCpcOG(P_0.vJBvocnRnBEzWRTRvcRPtuEWfLkn, P_1, "controller", P_2, P_3, cBFxQChnAZFRRQeDStCHagOAAZyI.Keyboard, -1, AxisRange.Full, -1, out P_4);
			if ((getAndUpdateLocalizedStringResultFlags & LocalizationManager.GetAndUpdateLocalizedStringResultFlags.Changed) != LocalizationManager.GetAndUpdateLocalizedStringResultFlags.None)
			{
				P_0.KJWNNWyAWMvuMSnNtOgGoBkrCsjv = (getAndUpdateLocalizedStringResultFlags & LocalizationManager.GetAndUpdateLocalizedStringResultFlags.JustLocalized) != 0;
			}
			return P_0.KJWNNWyAWMvuMSnNtOgGoBkrCsjv;
		}

		private object PgxuvwIqTmNJYLamUqWAjpFBmSJJ(ModifierKey P_0)
		{
			if (P_0 == ModifierKey.None)
			{
				return null;
			}
			ModifierKeyInfo modifierKeyInfo = Consts.modifierKeyInfo[(int)P_0];
			if (!GlyphManager.isEnabled)
			{
				return null;
			}
			if (!KZxBNkvvuOaeWhMcYGvQXYIvFJDEA.TryGetValue((int)P_0, out var value))
			{
				return null;
			}
			if (oahDotjCNdDoppZeDbauOdOgYgajA(value, modifierKeyInfo.longKey, UNRIOyvPojfCPrjRsEYcHBwwkZqS.deviceLocalizationInfo, out var result))
			{
				return result;
			}
			return null;
		}

		private string FNgcbYFikGVLODHiuAgrOtXkriGq(ModifierKey P_0)
		{
			if (P_0 == ModifierKey.None)
			{
				return null;
			}
			ModifierKeyInfo modifierKeyInfo = Consts.modifierKeyInfo[(int)P_0];
			if (!GlyphManager.isEnabled)
			{
				return null;
			}
			if (!KZxBNkvvuOaeWhMcYGvQXYIvFJDEA.TryGetValue((int)P_0, out var value))
			{
				return null;
			}
			if (aaLILeANTrwvGYPCvRKHbwTgLpXD(value, modifierKeyInfo.longKey, UNRIOyvPojfCPrjRsEYcHBwwkZqS.deviceLocalizationInfo, out var result))
			{
				return result;
			}
			return null;
		}

		private static bool oahDotjCNdDoppZeDbauOdOgYgajA(KRUBTtmepKkaxYgilRfXuIvpSzsp P_0, string P_1, DeviceLocalizationInfo P_2, out object P_3)
		{
			GlyphManager.GetAndUpdateGlyphResultFlags getAndUpdateGlyphResultFlags = VpLaXpbWubZjWnoTrMYiecLKpQbXb.gjMLaFWJEEdJqjcgpgIUDoroPEIDb(P_0.wNIYTfDmqPPqDLcYniMqepxYCtWG, P_1, "controller", P_2, cBFxQChnAZFRRQeDStCHagOAAZyI.Keyboard, -1, AxisRange.Full, -1, out P_3);
			if ((getAndUpdateGlyphResultFlags & GlyphManager.GetAndUpdateGlyphResultFlags.Changed) != GlyphManager.GetAndUpdateGlyphResultFlags.None)
			{
				P_0.WpRRNfPyEpJcGeLfIZJurShhnupV = (getAndUpdateGlyphResultFlags & GlyphManager.GetAndUpdateGlyphResultFlags.JustGot) != 0;
			}
			return P_0.WpRRNfPyEpJcGeLfIZJurShhnupV;
		}

		private static bool aaLILeANTrwvGYPCvRKHbwTgLpXD(KRUBTtmepKkaxYgilRfXuIvpSzsp P_0, string P_1, DeviceLocalizationInfo P_2, out string P_3)
		{
			object obj;
			bool result = oahDotjCNdDoppZeDbauOdOgYgajA(P_0, P_1, P_2, out obj);
			P_3 = P_0.wNIYTfDmqPPqDLcYniMqepxYCtWG.cachedKey;
			return result;
		}

		[CompilerGenerated]
		private void YYllHfWkLOEoYZZezinWzenZfZMv()
		{
			IList<ModifierKey> values = EnumValueHelper<ModifierKey>.Default.values;
			for (int i = 0; i < values.Count; i++)
			{
				lomnRghkRUsasIDrsLJMlCcqwNOi(values[i], true);
				lomnRghkRUsasIDrsLJMlCcqwNOi(values[i], false);
			}
		}

		[CompilerGenerated]
		private void LVXcoMEpiuxJKDZHpIBUQuqoMjbib()
		{
			IList<ModifierKey> values = EnumValueHelper<ModifierKey>.Default.values;
			for (int i = 0; i < values.Count; i++)
			{
				PgxuvwIqTmNJYLamUqWAjpFBmSJJ(values[i]);
			}
		}
	}
}
