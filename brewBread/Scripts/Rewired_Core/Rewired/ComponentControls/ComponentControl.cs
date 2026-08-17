using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Rewired.Utils;
using UnityEngine;

namespace Rewired.ComponentControls
{
	[Serializable]
	[DisallowMultipleComponent]
	public abstract class ComponentControl : MonoBehaviour, IComponentControl
	{
		private sealed class DGQGkJdqAwwNsKpEeYWflohTBBwc : IDisposable, IEnumerator, IEnumerator<object>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private object VqEePGSMyrGKIqkWibsjeHcWPSIx;

			public ComponentControl TtytLoUfsgUyhsklaKccrnoMiiek;

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
			public DGQGkJdqAwwNsKpEeYWflohTBBwc(int P_0)
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
				ComponentControl ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
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
					if (!ttytLoUfsgUyhsklaKccrnoMiiek.EDufGzVNigBlMAOWvMGsHZmtQaph())
					{
						return false;
					}
					ttytLoUfsgUyhsklaKccrnoMiiek.OnEnable();
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

		private IComponentController _controller;

		[NonSerialized]
		private bool obpzIVquVRQseulcTcTZaHvizTjRA;

		[NonSerialized]
		private bool RePRCVbBZgCcBPtVnJbUMgMOGrtL;

		private int _lastUpdateFrame = -1;

		internal abstract bool osKcqUcyYlVGlGygpMaOnUYNqJDBA { get; }

		internal bool jYTNgflwwEgvgbZuuTHYnPAnuRao => obpzIVquVRQseulcTcTZaHvizTjRA;

		[CustomObfuscation(rename = false)]
		internal ComponentControl()
		{
		}

		public abstract void ClearValue();

		void IComponentControl.Update()
		{
			int frameCount = Time.frameCount;
			if (_lastUpdateFrame != frameCount)
			{
				_lastUpdateFrame = frameCount;
				ZCGETbjMQZUkyflRtYAqwQNUBPQIb();
			}
		}

		[CustomObfuscation(rename = false)]
		internal virtual void Awake()
		{
			RePRCVbBZgCcBPtVnJbUMgMOGrtL = true;
		}

		[CustomObfuscation(rename = false)]
		internal virtual void Start()
		{
		}

		[CustomObfuscation(rename = false)]
		internal virtual void OnEnable()
		{
			if (!RePRCVbBZgCcBPtVnJbUMgMOGrtL)
			{
				obpzIVquVRQseulcTcTZaHvizTjRA = false;
				StartCoroutine(pszdJEkXjPTJncstTUIWtRdIvwUM());
				RePRCVbBZgCcBPtVnJbUMgMOGrtL = true;
			}
			else if (Application.isPlaying)
			{
				qhuNDonLyhwaOUJVhxsLRGkQPgDT();
			}
		}

		[CustomObfuscation(rename = false)]
		internal virtual void OnDisable()
		{
			if (Application.isPlaying)
			{
				lelFPleVPpiYwgZaVbxuBHPfxsFeA();
			}
		}

		[CustomObfuscation(rename = false)]
		internal virtual void OnDestroy()
		{
		}

		[CustomObfuscation(rename = false)]
		internal virtual void OnValidate()
		{
			if (obpzIVquVRQseulcTcTZaHvizTjRA)
			{
				khkdMwnTAvlDqPvCkDAZhRJFGYZR();
			}
		}

		[CustomObfuscation(rename = false)]
		internal virtual void OnCanvasGroupChanged()
		{
			if (obpzIVquVRQseulcTcTZaHvizTjRA)
			{
				nGHzehsIbiveZqWLVMehuidueiKkA(false, false);
			}
		}

		[CustomObfuscation(rename = false)]
		internal virtual void OnTransformParentChanged()
		{
			if (obpzIVquVRQseulcTcTZaHvizTjRA)
			{
				nGHzehsIbiveZqWLVMehuidueiKkA(false, false);
			}
		}

		[CustomObfuscation(rename = false)]
		internal virtual void OnDidApplyAnimationProperties()
		{
			_ = obpzIVquVRQseulcTcTZaHvizTjRA;
		}

		[CustomObfuscation(rename = false)]
		internal virtual void Reset()
		{
			_ = obpzIVquVRQseulcTcTZaHvizTjRA;
		}

		internal virtual void ZCGETbjMQZUkyflRtYAqwQNUBPQIb()
		{
		}

		internal virtual bool lpOPYPkfRAdylCMSLphTlIgUWIWy()
		{
			obpzIVquVRQseulcTcTZaHvizTjRA = false;
			if (!nGHzehsIbiveZqWLVMehuidueiKkA(true, true))
			{
				return false;
			}
			_controller.Register(this);
			return true;
		}

		internal virtual void lelFPleVPpiYwgZaVbxuBHPfxsFeA()
		{
			ClearValue();
			if (!_controller.IsNullOrDestroyed())
			{
				_controller.Deregister(this);
			}
			LYluBUMWPKbwashzGpNPLTeasipd();
			obpzIVquVRQseulcTcTZaHvizTjRA = false;
		}

		internal virtual void sGOXUkdDJMvaXZNxRxZLevjalIqm()
		{
			if (!_controller.IsNullOrDestroyed())
			{
				LYluBUMWPKbwashzGpNPLTeasipd();
			}
		}

		internal virtual void LYluBUMWPKbwashzGpNPLTeasipd()
		{
			_controller.IsNullOrDestroyed();
		}

		internal virtual void FyKKzlnIjsJaSimYwuqKSwNJHIHx()
		{
			if (obpzIVquVRQseulcTcTZaHvizTjRA)
			{
				khkdMwnTAvlDqPvCkDAZhRJFGYZR();
			}
		}

		internal virtual void vXIboQFSGKdCLZnWmauizJnaOLuX()
		{
			_ = obpzIVquVRQseulcTcTZaHvizTjRA;
		}

		internal virtual void AOKLCSwSOHWHrUKFiqLEizXMqAyJ()
		{
		}

		internal bool EDufGzVNigBlMAOWvMGsHZmtQaph()
		{
			return UnityTools.IsActiveAndEnabled(this);
		}

		internal bool RyWbwvHKmMifMfqUEuQHCCdFAIHzD()
		{
			return this == null;
		}

		internal IComponentController kVndxYkOyKrGmxUDdlmyfMvjGUQDA()
		{
			return _controller;
		}

		[CustomObfuscation(rename = false)]
		internal abstract IComponentController FindController();

		[CustomObfuscation(rename = false)]
		internal abstract Type GetRequiredControllerType();

		private IEnumerator pszdJEkXjPTJncstTUIWtRdIvwUM()
		{
			return new DGQGkJdqAwwNsKpEeYWflohTBBwc(0)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this
			};
		}

		private void qhuNDonLyhwaOUJVhxsLRGkQPgDT()
		{
			if (lpOPYPkfRAdylCMSLphTlIgUWIWy())
			{
				AOKLCSwSOHWHrUKFiqLEizXMqAyJ();
				obpzIVquVRQseulcTcTZaHvizTjRA = true;
				sGOXUkdDJMvaXZNxRxZLevjalIqm();
			}
		}

		private bool nGHzehsIbiveZqWLVMehuidueiKkA(bool P_0, bool P_1)
		{
			bool flag = false;
			try
			{
				IComponentController componentController = FindController();
				if (!_controller.IsNullOrDestroyed() && _controller != componentController)
				{
					flag = true;
				}
				_controller = componentController;
				if (_controller == null)
				{
					Type type = GetRequiredControllerType();
					if ((object)type == null)
					{
						type = typeof(IComponentController);
					}
					if (P_1)
					{
						Logger.LogError(type.Name + " could not be found. You must have a component that extends from " + type.Name + " on this or a parent GameObject.");
					}
					throw new Exception();
				}
				if (!P_0 && flag)
				{
					qhuNDonLyhwaOUJVhxsLRGkQPgDT();
				}
				return true;
			}
			catch
			{
				lelFPleVPpiYwgZaVbxuBHPfxsFeA();
				return false;
			}
		}

		private void khkdMwnTAvlDqPvCkDAZhRJFGYZR()
		{
			nGHzehsIbiveZqWLVMehuidueiKkA(false, true);
		}

		private void mymCkzUBjdFeOqwLJiYHNYJyCAjc()
		{
			if (!RyWbwvHKmMifMfqUEuQHCCdFAIHzD() && EDufGzVNigBlMAOWvMGsHZmtQaph())
			{
				ZCGETbjMQZUkyflRtYAqwQNUBPQIb();
			}
		}
	}
}
