using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Rewired.Interfaces;
using Rewired.Utils;
using UnityEngine;

namespace Rewired
{
	public sealed class Keyboard : ControllerWithMap
	{
		private sealed class yuYWUacflKxmBhmfoesMsBjdZOOb : IEnumerable<ControllerPollingInfo>, IEnumerator<ControllerPollingInfo>, IDisposable, IEnumerable, IEnumerator
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private ControllerPollingInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public Keyboard TtytLoUfsgUyhsklaKccrnoMiiek;

			private int sArDJaApjHHOXVwYydQNgVrLbneLA;

			private int fIMVaffCgsuIJcnrkMmGGKfPwwel;

			ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			[DebuggerHidden]
			public yuYWUacflKxmBhmfoesMsBjdZOOb(int P_0)
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
				wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
				Keyboard ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
				{
					if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
					{
						return false;
					}
					RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
					goto IL_00bf;
				}
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (ReInput._id != ttytLoUfsgUyhsklaKccrnoMiiek.QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(ttytLoUfsgUyhsklaKccrnoMiiek.QajDFFaomlkLHzaostfWYpGUioys);
					return false;
				}
				sArDJaApjHHOXVwYydQNgVrLbneLA = Consts.keyboardKeyValues.Count;
				fIMVaffCgsuIJcnrkMmGGKfPwwel = 0;
				goto IL_00cf;
				IL_00cf:
				if (fIMVaffCgsuIJcnrkMmGGKfPwwel < sArDJaApjHHOXVwYydQNgVrLbneLA)
				{
					KeyCode keyCode = (KeyCode)Consts.keyboardKeyValues[fIMVaffCgsuIJcnrkMmGGKfPwwel];
					if (ttytLoUfsgUyhsklaKccrnoMiiek.GetKey(keyCode))
					{
						VqEePGSMyrGKIqkWibsjeHcWPSIx = new ControllerPollingInfo(true, -1, ttytLoUfsgUyhsklaKccrnoMiiek.id, ttytLoUfsgUyhsklaKccrnoMiiek._name, ttytLoUfsgUyhsklaKccrnoMiiek._type, ControllerElementType.Button, fIMVaffCgsuIJcnrkMmGGKfPwwel, Pole.Positive, GetKeyName(keyCode), ttytLoUfsgUyhsklaKccrnoMiiek.yPbTGFEOQNqKOEHVnHaIUhnHjgYD.buttonElementIdentifierIds[fIMVaffCgsuIJcnrkMmGGKfPwwel], keyCode);
						RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
						return true;
					}
					goto IL_00bf;
				}
				return false;
				IL_00bf:
				fIMVaffCgsuIJcnrkMmGGKfPwwel++;
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
				yuYWUacflKxmBhmfoesMsBjdZOOb yuYWUacflKxmBhmfoesMsBjdZOOb2;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					yuYWUacflKxmBhmfoesMsBjdZOOb2 = this;
				}
				else
				{
					yuYWUacflKxmBhmfoesMsBjdZOOb2 = new yuYWUacflKxmBhmfoesMsBjdZOOb(0);
					yuYWUacflKxmBhmfoesMsBjdZOOb2.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				return yuYWUacflKxmBhmfoesMsBjdZOOb2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
			}
		}

		private sealed class LglsUBPFysTsItjzpEZCVsaBmKzO : IEnumerable<ControllerPollingInfo>, IEnumerator<ControllerPollingInfo>, IDisposable, IEnumerable, IEnumerator
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private ControllerPollingInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public Keyboard TtytLoUfsgUyhsklaKccrnoMiiek;

			private int sArDJaApjHHOXVwYydQNgVrLbneLA;

			private int fIMVaffCgsuIJcnrkMmGGKfPwwel;

			ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			[DebuggerHidden]
			public LglsUBPFysTsItjzpEZCVsaBmKzO(int P_0)
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
				wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
				Keyboard ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
				{
					if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
					{
						return false;
					}
					RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
					goto IL_00bf;
				}
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (ReInput._id != ttytLoUfsgUyhsklaKccrnoMiiek.QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(ttytLoUfsgUyhsklaKccrnoMiiek.QajDFFaomlkLHzaostfWYpGUioys);
					return false;
				}
				sArDJaApjHHOXVwYydQNgVrLbneLA = Consts.keyboardKeyValues.Count;
				fIMVaffCgsuIJcnrkMmGGKfPwwel = 0;
				goto IL_00cf;
				IL_00cf:
				if (fIMVaffCgsuIJcnrkMmGGKfPwwel < sArDJaApjHHOXVwYydQNgVrLbneLA)
				{
					KeyCode keyCode = (KeyCode)Consts.keyboardKeyValues[fIMVaffCgsuIJcnrkMmGGKfPwwel];
					if (ttytLoUfsgUyhsklaKccrnoMiiek.GetKeyDown(keyCode))
					{
						VqEePGSMyrGKIqkWibsjeHcWPSIx = new ControllerPollingInfo(true, -1, ttytLoUfsgUyhsklaKccrnoMiiek.id, ttytLoUfsgUyhsklaKccrnoMiiek._name, ttytLoUfsgUyhsklaKccrnoMiiek._type, ControllerElementType.Button, fIMVaffCgsuIJcnrkMmGGKfPwwel, Pole.Positive, GetKeyName(keyCode), ttytLoUfsgUyhsklaKccrnoMiiek.yPbTGFEOQNqKOEHVnHaIUhnHjgYD.buttonElementIdentifierIds[fIMVaffCgsuIJcnrkMmGGKfPwwel], keyCode);
						RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
						return true;
					}
					goto IL_00bf;
				}
				return false;
				IL_00bf:
				fIMVaffCgsuIJcnrkMmGGKfPwwel++;
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
				LglsUBPFysTsItjzpEZCVsaBmKzO lglsUBPFysTsItjzpEZCVsaBmKzO;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					lglsUBPFysTsItjzpEZCVsaBmKzO = this;
				}
				else
				{
					lglsUBPFysTsItjzpEZCVsaBmKzO = new LglsUBPFysTsItjzpEZCVsaBmKzO(0);
					lglsUBPFysTsItjzpEZCVsaBmKzO.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				return lglsUBPFysTsItjzpEZCVsaBmKzO;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
			}
		}

		private static Keyboard JJpYEzvjUtGECvHgvcGIgXvyVWiSA;

		private readonly IUnifiedKeyboardSource eyyVWBZaxsCJCQMmwhCWcagfYfzWA;

		private ModifierKeyFlags boVDfwknXCoctMvohoPrHRSiyPbi;

		private ModifierKeyFlags XwpuFFBHeLMpRjDvJfMDQJtOXKjA;

		private Func<KeyboardKeyCode, int> ifrWFigIsftrgtIztMiWvrvQQqL;

		private readonly int[] eDsnovpgDYRCEkItkogoNDRSYMgr;

		private static KeyboardKeyCode[] fzwYAxzrcqhmaApcRmPhwHCTgEfKA;

		private readonly int ioJHYiWACZZkkzZeFRiOwxJFfdQw;

		private static Guid prVaMvCMokqAbWLeJcbpooSHlNBE;

		private static KeyboardKeyCode[] ogbVLzWCRfeTihmGeCdFpUNKDXuPA
		{
			get
			{
				if (fzwYAxzrcqhmaApcRmPhwHCTgEfKA == null)
				{
					int[] keyboardKeyValues = Consts._keyboardKeyValues;
					int num = keyboardKeyValues.Length;
					fzwYAxzrcqhmaApcRmPhwHCTgEfKA = new KeyboardKeyCode[num];
					for (int i = 0; i < num; i++)
					{
						fzwYAxzrcqhmaApcRmPhwHCTgEfKA[i] = (KeyboardKeyCode)keyboardKeyValues[i];
					}
				}
				return fzwYAxzrcqhmaApcRmPhwHCTgEfKA;
			}
		}

		public override Guid deviceInstanceGuid
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return Guid.Empty;
				}
				return prVaMvCMokqAbWLeJcbpooSHlNBE;
			}
		}

		internal Keyboard(string P_0, IUnifiedKeyboardSource P_1)
			: this(0, P_1.inputSource, P_0, InputTools.FormatHardwareIdentifierString(P_0), P_1.hardwareMap, 132, P_1?.controllerExtension, new ControllerDataUpdater(P_1.inputSource, 0, 132, null))
		{
			prVaMvCMokqAbWLeJcbpooSHlNBE = MiscTools.CreateGuidHashSHA1("[Universal Keyboard]");
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
			eyyVWBZaxsCJCQMmwhCWcagfYfzWA = P_1;
			VFZCTlETAOxSLbCyseetbWbCRTGUb();
		}

		private Keyboard(int P_0, InputSource P_1, string P_2, string P_3, HardwareControllerMap_Game P_4, int P_5, Extension P_6, ControllerDataUpdater P_7)
			: base(P_0, P_1, P_2, P_2, P_3, ControllerType.Keyboard, Consts.hardwareTypeGuid_universalKeyboard, P_5, null, P_4, P_6, P_7)
		{
			JJpYEzvjUtGECvHgvcGIgXvyVWiSA = this;
		}

		public bool GetKey(KeyCode keyCode)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if ((uint)keyCode > (uint)ioJHYiWACZZkkzZeFRiOwxJFfdQw)
			{
				return false;
			}
			int num = eDsnovpgDYRCEkItkogoNDRSYMgr[(int)keyCode];
			if (num < 0)
			{
				return false;
			}
			return buttons[num].value;
		}

		public bool GetKeyDown(KeyCode keyCode)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if ((uint)keyCode > (uint)ioJHYiWACZZkkzZeFRiOwxJFfdQw)
			{
				return false;
			}
			int num = eDsnovpgDYRCEkItkogoNDRSYMgr[(int)keyCode];
			if (num < 0)
			{
				return false;
			}
			return buttons[num].justPressed;
		}

		public bool GetKeyUp(KeyCode keyCode)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if ((uint)keyCode > (uint)ioJHYiWACZZkkzZeFRiOwxJFfdQw)
			{
				return false;
			}
			int num = eDsnovpgDYRCEkItkogoNDRSYMgr[(int)keyCode];
			if (num < 0)
			{
				return false;
			}
			return buttons[num].justReleased;
		}

		public bool GetKeyDoublePressHold(KeyCode keyCode, float speed)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if ((uint)keyCode > (uint)ioJHYiWACZZkkzZeFRiOwxJFfdQw)
			{
				return false;
			}
			int num = eDsnovpgDYRCEkItkogoNDRSYMgr[(int)keyCode];
			if (num < 0)
			{
				return false;
			}
			return buttons[num].DoublePressedAndHeld(speed);
		}

		public bool GetKeyDoublePressHold(KeyCode keyCode)
		{
			if ((uint)keyCode > (uint)ioJHYiWACZZkkzZeFRiOwxJFfdQw)
			{
				return false;
			}
			int num = eDsnovpgDYRCEkItkogoNDRSYMgr[(int)keyCode];
			if (num < 0)
			{
				return false;
			}
			return buttons[num].DoublePressedAndHeld(0f);
		}

		public bool GetKeyDoublePressDown(KeyCode keyCode, float speed)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if ((uint)keyCode > (uint)ioJHYiWACZZkkzZeFRiOwxJFfdQw)
			{
				return false;
			}
			int num = eDsnovpgDYRCEkItkogoNDRSYMgr[(int)keyCode];
			if (num < 0)
			{
				return false;
			}
			return buttons[num].JustDoublePressed(speed);
		}

		public bool GetKeyDoublePressDown(KeyCode keyCode)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if ((uint)keyCode > (uint)ioJHYiWACZZkkzZeFRiOwxJFfdQw)
			{
				return false;
			}
			int num = eDsnovpgDYRCEkItkogoNDRSYMgr[(int)keyCode];
			if (num < 0)
			{
				return false;
			}
			return buttons[num].JustDoublePressed(0f);
		}

		public bool GetKeyPrev(KeyCode keyCode)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if ((uint)keyCode > (uint)ioJHYiWACZZkkzZeFRiOwxJFfdQw)
			{
				return false;
			}
			int num = eDsnovpgDYRCEkItkogoNDRSYMgr[(int)keyCode];
			if (num < 0)
			{
				return false;
			}
			return buttons[num].valuePrev;
		}

		public double GetKeyTimePressed(KeyCode keyCode)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			if ((uint)keyCode > (uint)ioJHYiWACZZkkzZeFRiOwxJFfdQw)
			{
				return 0.0;
			}
			int num = eDsnovpgDYRCEkItkogoNDRSYMgr[(int)keyCode];
			if (num < 0)
			{
				return 0.0;
			}
			return buttons[num].timePressed;
		}

		public double GetKeyTimeUnpressed(KeyCode keyCode)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			if ((uint)keyCode > (uint)ioJHYiWACZZkkzZeFRiOwxJFfdQw)
			{
				return 0.0;
			}
			int num = eDsnovpgDYRCEkItkogoNDRSYMgr[(int)keyCode];
			if (num < 0)
			{
				return 0.0;
			}
			return buttons[num].timeUnpressed;
		}

		public bool GetModifierKey(ModifierKey key)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if (!WlSFvyDKnvpozxUvgdMGAIblCodzA(out var button, out var button2, key))
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
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if (!WlSFvyDKnvpozxUvgdMGAIblCodzA(out var button, out var button2, key))
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
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if (!WlSFvyDKnvpozxUvgdMGAIblCodzA(out var button, out var button2, key))
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
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if (!WlSFvyDKnvpozxUvgdMGAIblCodzA(out var button, out var button2, key))
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
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			if (!WlSFvyDKnvpozxUvgdMGAIblCodzA(out var button, out var button2, key))
			{
				return 0.0;
			}
			return MathTools.Max(button.timePressed, button2.timePressed);
		}

		public double GetModifierKeyTimeUnpressed(ModifierKey key)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			if (!WlSFvyDKnvpozxUvgdMGAIblCodzA(out var button, out var button2, key))
			{
				return 0.0;
			}
			return MathTools.Min(button.timeUnpressed, button2.timeUnpressed);
		}

		public KeyCode GetKeyCodeByButtonIndex(int buttonIndex)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return KeyCode.None;
			}
			return JopyRNKJDwgoEWSurnWzJODKdzOk(GetKeyboardKeyCodeByButtonIndex(buttonIndex));
		}

		public KeyCode GetKeyCodeById(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return KeyCode.None;
			}
			return GetKeyCodeByButtonIndex(GetButtonIndexById(elementIdentifierId));
		}

		public int GetButtonIndexByKeyCode(KeyCode keyCode)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return -1;
			}
			if ((uint)keyCode > (uint)ioJHYiWACZZkkzZeFRiOwxJFfdQw)
			{
				return -1;
			}
			return eDsnovpgDYRCEkItkogoNDRSYMgr[(int)keyCode];
		}

		public ControllerElementIdentifier GetElementIdentifierByKeyCode(KeyCode keyCode)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			if ((uint)keyCode > (uint)ioJHYiWACZZkkzZeFRiOwxJFfdQw)
			{
				return null;
			}
			int num = eDsnovpgDYRCEkItkogoNDRSYMgr[(int)keyCode];
			if (num < 0)
			{
				return null;
			}
			return yPbTGFEOQNqKOEHVnHaIUhnHjgYD.buttonElementIdentifiers_cache[num];
		}

		public ControllerPollingInfo PollForFirstKey()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
			}
			int count = Consts.keyboardKeyValues.Count;
			for (int i = 0; i < count; i++)
			{
				KeyCode keyCode = (KeyCode)Consts.keyboardKeyValues[i];
				if (GetKey(keyCode))
				{
					return new ControllerPollingInfo(true, -1, id, _name, _type, ControllerElementType.Button, i, Pole.Positive, GetKeyName(keyCode), yPbTGFEOQNqKOEHVnHaIUhnHjgYD.buttonElementIdentifierIds[i], keyCode);
				}
			}
			return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
		}

		public IEnumerable<ControllerPollingInfo> PollForAllKeys()
		{
			return new yuYWUacflKxmBhmfoesMsBjdZOOb(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this
			};
		}

		public IEnumerable<ControllerPollingInfo> PollForAllKeysDown()
		{
			return new LglsUBPFysTsItjzpEZCVsaBmKzO(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this
			};
		}

		public ControllerPollingInfo PollForFirstKeyDown()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
			}
			int count = Consts.keyboardKeyValues.Count;
			for (int i = 0; i < count; i++)
			{
				KeyCode keyCode = (KeyCode)Consts.keyboardKeyValues[i];
				if (GetKeyDown(keyCode))
				{
					return new ControllerPollingInfo(true, -1, id, _name, _type, ControllerElementType.Button, i, Pole.Positive, GetKeyName(keyCode), yPbTGFEOQNqKOEHVnHaIUhnHjgYD.buttonElementIdentifierIds[i], keyCode);
				}
			}
			return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
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
			case KeyCode.RightCommand:
			case KeyCode.LeftCommand:
				return true;
			default:
				return false;
			}
		}

		internal static bool trZafsVxHDJMaoTvWPAzFzJbrlbl(KeyboardKeyCode P_0)
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
			case KeyCode.RightCommand:
			case KeyCode.LeftCommand:
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
				KeyCode.LeftCommand => ModifierKeyFlags.LeftCommand, 
				KeyCode.RightCommand => ModifierKeyFlags.RightCommand, 
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
			case KeyCode.LeftCommand:
				if ((flags & ModifierKeyFlags.LeftCommand) == ModifierKeyFlags.LeftCommand)
				{
					return true;
				}
				return false;
			case KeyCode.RightCommand:
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
				return KeyCode.LeftCommand;
			}
			if ((flags & ModifierKeyFlags.RightCommand) == ModifierKeyFlags.RightCommand)
			{
				return KeyCode.RightCommand;
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
			if (JJpYEzvjUtGECvHgvcGIgXvyVWiSA == null)
			{
				return string.Empty;
			}
			int buttonIndex = JJpYEzvjUtGECvHgvcGIgXvyVWiSA.GetButtonIndex(lbKrYDonKuonbZuyvZjwCetiVeki(key));
			if (buttonIndex < 0)
			{
				return string.Empty;
			}
			return JJpYEzvjUtGECvHgvcGIgXvyVWiSA.ButtonElementIdentifiers[buttonIndex].name;
		}

		public static string GetKeyName(KeyCode key, ModifierKeyFlags flags)
		{
			string text = GetKeyName(key);
			if (flags != ModifierKeyFlags.None)
			{
				text = text + " + " + ModifierKeyFlagsToString(flags);
			}
			return text;
		}

		public static string ModifierKeyFlagsToString(ModifierKeyFlags flags, bool abbreviate)
		{
			int num = 0;
			string text = string.Empty;
			if (ModifierKeyFlagsContain(flags, ModifierKey.Control))
			{
				text = (abbreviate ? (text + "Ctrl") : (text + "Control"));
				num++;
			}
			if (ModifierKeyFlagsContain(flags, ModifierKey.Command))
			{
				if (num > 0)
				{
					text += " + ";
				}
				text = (abbreviate ? (text + "Cmd") : (text + "Command"));
				num++;
			}
			if (ModifierKeyFlagsContain(flags, ModifierKey.Alt))
			{
				if (num > 0)
				{
					text += " + ";
				}
				text += "Alt";
				num++;
			}
			if (num >= 3)
			{
				return text;
			}
			if (ModifierKeyFlagsContain(flags, ModifierKey.Shift))
			{
				if (num > 0)
				{
					text += " + ";
				}
				text += "Shift";
				num++;
			}
			return text;
		}

		public static string ModifierKeyFlagsToString(ModifierKeyFlags flags)
		{
			return ModifierKeyFlagsToString(flags, abbreviate: false);
		}

		internal static KeyboardKeyCode lbKrYDonKuonbZuyvZjwCetiVeki(KeyCode P_0)
		{
			return (KeyboardKeyCode)P_0;
		}

		internal static KeyCode JopyRNKJDwgoEWSurnWzJODKdzOk(KeyboardKeyCode P_0)
		{
			return (KeyCode)P_0;
		}

		internal static ModifierKeyFlags mKNIIexTHIbsnigjJPesRfqSFIzkA(ModifierKeyFlags P_0)
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

		internal static int HVRdbMGJASvNJRKWhACVREVAjGDe(ModifierKeyFlags P_0)
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
			return ogbVLzWCRfeTihmGeCdFpUNKDXuPA[buttonIndex];
		}

		internal static int qLwqAFVXIrkFTLxKuwVLiCCndNbkA(KeyboardKeyCode P_0)
		{
			int buttonIndex = JJpYEzvjUtGECvHgvcGIgXvyVWiSA.GetButtonIndex(P_0);
			if (buttonIndex < 0)
			{
				return -1;
			}
			return JJpYEzvjUtGECvHgvcGIgXvyVWiSA.ButtonElementIdentifiers[buttonIndex].id;
		}

		internal static void CYCUDNoanHYzGAabMMwoCdyIUash(ref int P_0, ref KeyCode P_1)
		{
			if (P_1 != KeyCode.None)
			{
				P_0 = qLwqAFVXIrkFTLxKuwVLiCCndNbkA(lbKrYDonKuonbZuyvZjwCetiVeki(P_1));
			}
			else
			{
				P_1 = ReInput.VmqcbbbvPImXBEBcMHWjUAXVfrUSA.OurrBprwSKVEMTXHNSeMGLuBHall.GetKeyCodeById(P_0);
			}
		}

		internal override void PwSVyxiNOmkyuuXUHWyuymUyabog(UpdateLoopType P_0)
		{
			eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UpdateInputData(BkAqQtJmzNvLobJflzLPUhNYRphu);
			base.PwSVyxiNOmkyuuXUHWyuymUyabog(P_0);
			JWatcBsgqUJfAeASrBdOcCtHeoGCA();
		}

		internal void DtgpVXPyTPhskVKHNnJAXrucMrXt(UpdateLoopType P_0)
		{
			buttons[ThreadSafeUnityInput.Keyboard.keyValueIndex_Escape].tzfYrQaBdsWHACSbQzIxoLZqqtry(P_0, ThreadSafeUnityInput.Keyboard.keyValueIndex_Escape, BkAqQtJmzNvLobJflzLPUhNYRphu);
			buttons[ThreadSafeUnityInput.Keyboard.keyValueIndex_Menu].tzfYrQaBdsWHACSbQzIxoLZqqtry(P_0, ThreadSafeUnityInput.Keyboard.keyValueIndex_Menu, BkAqQtJmzNvLobJflzLPUhNYRphu);
			buttons[ThreadSafeUnityInput.Keyboard.keyValueIndex_F2].tzfYrQaBdsWHACSbQzIxoLZqqtry(P_0, ThreadSafeUnityInput.Keyboard.keyValueIndex_F2, BkAqQtJmzNvLobJflzLPUhNYRphu);
			buttons[ThreadSafeUnityInput.Keyboard.keyValueIndex_UpArrow].tzfYrQaBdsWHACSbQzIxoLZqqtry(P_0, ThreadSafeUnityInput.Keyboard.keyValueIndex_UpArrow, BkAqQtJmzNvLobJflzLPUhNYRphu);
			buttons[ThreadSafeUnityInput.Keyboard.keyValueIndex_RightArrow].tzfYrQaBdsWHACSbQzIxoLZqqtry(P_0, ThreadSafeUnityInput.Keyboard.keyValueIndex_RightArrow, BkAqQtJmzNvLobJflzLPUhNYRphu);
			buttons[ThreadSafeUnityInput.Keyboard.keyValueIndex_DownArrow].tzfYrQaBdsWHACSbQzIxoLZqqtry(P_0, ThreadSafeUnityInput.Keyboard.keyValueIndex_DownArrow, BkAqQtJmzNvLobJflzLPUhNYRphu);
			buttons[ThreadSafeUnityInput.Keyboard.keyValueIndex_LeftArrow].tzfYrQaBdsWHACSbQzIxoLZqqtry(P_0, ThreadSafeUnityInput.Keyboard.keyValueIndex_LeftArrow, BkAqQtJmzNvLobJflzLPUhNYRphu);
		}

		internal bool NqnspTmKVTuOenOXgaESJYqWWgyMA(KeyboardKeyCode P_0)
		{
			if ((uint)P_0 > (uint)ioJHYiWACZZkkzZeFRiOwxJFfdQw)
			{
				return false;
			}
			int num = eDsnovpgDYRCEkItkogoNDRSYMgr[(int)P_0];
			if (num < 0)
			{
				return false;
			}
			return buttons[num].value;
		}

		internal bool qOwExPSwfeUFsokuDNGJcBzXkgQt(KeyboardKeyCode P_0)
		{
			if ((uint)P_0 > (uint)ioJHYiWACZZkkzZeFRiOwxJFfdQw)
			{
				return false;
			}
			int num = eDsnovpgDYRCEkItkogoNDRSYMgr[(int)P_0];
			if (num < 0)
			{
				return false;
			}
			return buttons[num].valuePrev;
		}

		internal bool ngSbgQwwHNlDRfcEWdGhCacxQsPyA(KeyboardKeyCode P_0, ModifierKeyFlags P_1)
		{
			if (!NqnspTmKVTuOenOXgaESJYqWWgyMA(P_0))
			{
				return false;
			}
			if (P_1 == ModifierKeyFlags.None)
			{
				return true;
			}
			if ((P_1 & XwpuFFBHeLMpRjDvJfMDQJtOXKjA) != P_1)
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

		internal bool rLrNJvDZCOJfXgDctUQhRvIBelQJ(KeyboardKeyCode P_0, ModifierKeyFlags P_1)
		{
			if (NqnspTmKVTuOenOXgaESJYqWWgyMA(P_0))
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
			if ((uint)keyCode > (uint)ioJHYiWACZZkkzZeFRiOwxJFfdQw)
			{
				return -1;
			}
			return eDsnovpgDYRCEkItkogoNDRSYMgr[(int)keyCode];
		}

		[CustomObfuscation(rename = false)]
		internal void BakeMap(ControllerMap controllerMap)
		{
			if (controllerMap != null)
			{
				IList<ActionElementMap> list = controllerMap.cFGJUwYhcfUZYvBnprhEpVaWEPom;
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					JpxRPMmkCiJxotCLchUqPLcjlZiQ(controllerMap, list[i]);
				}
			}
		}

		[CustomObfuscation(rename = false)]
		internal void BakeActionElementMap(ControllerMap controllerMap, ActionElementMap map)
		{
			map?.nCUqazOObDuxSjUNBaQhabwMxrSG(controllerMap);
		}

		internal override void SPGTRPyvIslcMdbPTItsewSLRPxx()
		{
			base.SPGTRPyvIslcMdbPTItsewSLRPxx();
			boVDfwknXCoctMvohoPrHRSiyPbi = ModifierKeyFlags.None;
			XwpuFFBHeLMpRjDvJfMDQJtOXKjA = ModifierKeyFlags.None;
		}

		internal override bool RQRRqZiAdoXIQMUtzTIWsndPGdwX(bool P_0)
		{
			if (!base.RQRRqZiAdoXIQMUtzTIWsndPGdwX(P_0))
			{
				return false;
			}
			if (eyyVWBZaxsCJCQMmwhCWcagfYfzWA is IGetSetEnabled)
			{
				(eyyVWBZaxsCJCQMmwhCWcagfYfzWA as IGetSetEnabled).enabled = P_0;
			}
			return true;
		}

		private bool WlSFvyDKnvpozxUvgdMGAIblCodzA(out Button P_0, out Button P_1, ModifierKey P_2)
		{
			P_0 = null;
			P_1 = null;
			switch (P_2)
			{
			case ModifierKey.None:
				return false;
			case ModifierKey.Control:
				P_0 = buttons[eDsnovpgDYRCEkItkogoNDRSYMgr[306]];
				P_1 = buttons[eDsnovpgDYRCEkItkogoNDRSYMgr[305]];
				return true;
			case ModifierKey.Alt:
				P_0 = buttons[eDsnovpgDYRCEkItkogoNDRSYMgr[308]];
				P_1 = buttons[eDsnovpgDYRCEkItkogoNDRSYMgr[307]];
				return true;
			case ModifierKey.Command:
				P_0 = buttons[eDsnovpgDYRCEkItkogoNDRSYMgr[310]];
				P_1 = buttons[eDsnovpgDYRCEkItkogoNDRSYMgr[309]];
				return true;
			case ModifierKey.Shift:
				P_0 = buttons[eDsnovpgDYRCEkItkogoNDRSYMgr[304]];
				P_1 = buttons[eDsnovpgDYRCEkItkogoNDRSYMgr[303]];
				return true;
			default:
				return false;
			}
		}

		private void JWatcBsgqUJfAeASrBdOcCtHeoGCA()
		{
			ModifierKeyFlags modifierKeyFlags = ModifierKeyFlags.None;
			if (buttons[eDsnovpgDYRCEkItkogoNDRSYMgr[306]].value)
			{
				modifierKeyFlags |= ModifierKeyFlags.LeftControl;
			}
			if (buttons[eDsnovpgDYRCEkItkogoNDRSYMgr[305]].value)
			{
				modifierKeyFlags |= ModifierKeyFlags.RightControl;
			}
			if (buttons[eDsnovpgDYRCEkItkogoNDRSYMgr[310]].value)
			{
				modifierKeyFlags |= ModifierKeyFlags.LeftCommand;
			}
			if (buttons[eDsnovpgDYRCEkItkogoNDRSYMgr[309]].value)
			{
				modifierKeyFlags |= ModifierKeyFlags.RightCommand;
			}
			if (buttons[eDsnovpgDYRCEkItkogoNDRSYMgr[308]].value)
			{
				modifierKeyFlags |= ModifierKeyFlags.LeftAlt;
			}
			if (buttons[eDsnovpgDYRCEkItkogoNDRSYMgr[307]].value)
			{
				modifierKeyFlags |= ModifierKeyFlags.RightAlt;
			}
			if (buttons[eDsnovpgDYRCEkItkogoNDRSYMgr[304]].value)
			{
				modifierKeyFlags |= ModifierKeyFlags.LeftShift;
			}
			if (buttons[eDsnovpgDYRCEkItkogoNDRSYMgr[303]].value)
			{
				modifierKeyFlags |= ModifierKeyFlags.RightShift;
			}
			boVDfwknXCoctMvohoPrHRSiyPbi = modifierKeyFlags;
			XwpuFFBHeLMpRjDvJfMDQJtOXKjA = mKNIIexTHIbsnigjJPesRfqSFIzkA(modifierKeyFlags);
		}
	}
}
