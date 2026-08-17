using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Rewired.Utils;
using Rewired.Utils.Interfaces;
using UnityEngine;

namespace Rewired.ComponentControls
{
	[Serializable]
	[DisallowMultipleComponent]
	public abstract class ComponentController : MonoBehaviour, IRegistrar<IComponentControl>, IComponentController
	{
		private sealed class HoePfFXnUnFevIlMoWpfJiTDGFeU : IDisposable, IEnumerator, IEnumerator<object>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private object VqEePGSMyrGKIqkWibsjeHcWPSIx;

			public ComponentController TtytLoUfsgUyhsklaKccrnoMiiek;

			object IEnumerator<object>.Current
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
			public HoePfFXnUnFevIlMoWpfJiTDGFeU(int P_0)
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
				ComponentController ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				switch (rxAoyfYzYDsYonLGXsvUgwChukLk)
				{
				default:
					return false;
				case 0:
					RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
					VqEePGSMyrGKIqkWibsjeHcWPSIx = null;
					RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
					return true;
				case 1:
					RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
					ttytLoUfsgUyhsklaKccrnoMiiek.qhuNDonLyhwaOUJVhxsLRGkQPgDT();
					return false;
				}
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
		}

		[NonSerialized]
		private bool obpzIVquVRQseulcTcTZaHvizTjRA;

		[NonSerialized]
		private bool RePRCVbBZgCcBPtVnJbUMgMOGrtL;

		private List<IComponentControl> _controls = new List<IComponentControl>(10);

		internal bool jYTNgflwwEgvgbZuuTHYnPAnuRao => obpzIVquVRQseulcTcTZaHvizTjRA;

		[CustomObfuscation(rename = false)]
		internal ComponentController()
		{
		}

		[CustomObfuscation(rename = false)]
		internal virtual void Awake()
		{
			RePRCVbBZgCcBPtVnJbUMgMOGrtL = true;
		}

		[CustomObfuscation(rename = false)]
		internal virtual void Update()
		{
			if (!obpzIVquVRQseulcTcTZaHvizTjRA)
			{
				return;
			}
			for (int num = _controls.Count - 1; num >= 0; num--)
			{
				IComponentControl componentControl = _controls[num];
				if (componentControl.IsNullOrDestroyed())
				{
					_controls.RemoveAt(num);
				}
				else
				{
					componentControl.Update();
				}
			}
		}

		[CustomObfuscation(rename = false)]
		internal virtual void OnEnable()
		{
			if (!RePRCVbBZgCcBPtVnJbUMgMOGrtL)
			{
				StartCoroutine(tkaLgOgyfPesbfcHrqkIAwRWsggK());
				RePRCVbBZgCcBPtVnJbUMgMOGrtL = true;
			}
			else
			{
				qhuNDonLyhwaOUJVhxsLRGkQPgDT();
			}
		}

		[CustomObfuscation(rename = false)]
		internal virtual void OnDisable()
		{
			if (obpzIVquVRQseulcTcTZaHvizTjRA)
			{
				LYluBUMWPKbwashzGpNPLTeasipd();
			}
		}

		[CustomObfuscation(rename = false)]
		internal virtual void OnValidate()
		{
			if (obpzIVquVRQseulcTcTZaHvizTjRA)
			{
				FyKKzlnIjsJaSimYwuqKSwNJHIHx();
			}
		}

		[CustomObfuscation(rename = false)]
		internal virtual void OnDestroy()
		{
			_controls.Clear();
		}

		internal virtual bool lpOPYPkfRAdylCMSLphTlIgUWIWy()
		{
			return true;
		}

		internal virtual void sGOXUkdDJMvaXZNxRxZLevjalIqm()
		{
			LYluBUMWPKbwashzGpNPLTeasipd();
		}

		internal virtual void LYluBUMWPKbwashzGpNPLTeasipd()
		{
		}

		void IRegistrar<IComponentControl>.Register(IComponentControl control)
		{
			if (!control.IsNullOrDestroyed())
			{
				ListTools.AddIfUnique(_controls, control);
			}
		}

		void IRegistrar<IComponentControl>.Deregister(IComponentControl control)
		{
			if (!control.IsNullOrDestroyed())
			{
				_controls.Remove(control);
			}
		}

		public virtual void ClearControlValues()
		{
			if (!obpzIVquVRQseulcTcTZaHvizTjRA)
			{
				return;
			}
			for (int num = _controls.Count - 1; num >= 0; num--)
			{
				if (_controls[num].IsNullOrDestroyed())
				{
					_controls.RemoveAt(num);
				}
				else
				{
					_controls[num].ClearValue();
				}
			}
		}

		private void qhuNDonLyhwaOUJVhxsLRGkQPgDT()
		{
			if (lpOPYPkfRAdylCMSLphTlIgUWIWy())
			{
				obpzIVquVRQseulcTcTZaHvizTjRA = true;
				sGOXUkdDJMvaXZNxRxZLevjalIqm();
			}
		}

		private void FyKKzlnIjsJaSimYwuqKSwNJHIHx()
		{
			_ = jYTNgflwwEgvgbZuuTHYnPAnuRao;
		}

		private IEnumerator tkaLgOgyfPesbfcHrqkIAwRWsggK()
		{
			return new HoePfFXnUnFevIlMoWpfJiTDGFeU(0)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this
			};
		}
	}
}
