using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rewired.Utils;
using UnityEngine;

namespace Rewired.ComponentControls
{
	[Serializable]
	[DisallowMultipleComponent]
	public abstract class ComponentControl : MonoBehaviour, IComponentControl
	{
		private sealed class hIkaloBFBYIBWCoUaSSKFOvvrJGdB : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int vFwnZfecLCDKpAzvEuiAyIlRxpQH;

			private object MbNhBExGhoEWGbtuCOaNFeYOJCpF;

			public ComponentControl LQyQRGvGfaDlIcRPUoIHWJsGIHMMA;

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return MbNhBExGhoEWGbtuCOaNFeYOJCpF;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return MbNhBExGhoEWGbtuCOaNFeYOJCpF;
				}
			}

			[DebuggerHidden]
			public hIkaloBFBYIBWCoUaSSKFOvvrJGdB(int P_0)
			{
				vFwnZfecLCDKpAzvEuiAyIlRxpQH = P_0;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				vFwnZfecLCDKpAzvEuiAyIlRxpQH = -2;
			}

			private bool MoveNext()
			{
				int num = vFwnZfecLCDKpAzvEuiAyIlRxpQH;
				ComponentControl lQyQRGvGfaDlIcRPUoIHWJsGIHMMA = LQyQRGvGfaDlIcRPUoIHWJsGIHMMA;
				switch (num)
				{
				default:
					return false;
				case 0:
					vFwnZfecLCDKpAzvEuiAyIlRxpQH = -1;
					MbNhBExGhoEWGbtuCOaNFeYOJCpF = null;
					vFwnZfecLCDKpAzvEuiAyIlRxpQH = 1;
					return true;
				case 1:
					vFwnZfecLCDKpAzvEuiAyIlRxpQH = -1;
					if (!lQyQRGvGfaDlIcRPUoIHWJsGIHMMA.IcqbeYEmGpfkqqAVukZKtDJbdtuLA())
					{
						return false;
					}
					lQyQRGvGfaDlIcRPUoIHWJsGIHMMA.OnEnable();
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
		private bool CTOnUkeLJWaoFhFSZhZYUzHZMedEA;

		[NonSerialized]
		private bool zPhUbesvjLRLXdiRMucDSplQRHmW;

		private int _lastUpdateFrame = -1;

		internal abstract bool ghAsJwRhDmClYOgMqzKuSmomibZfA { get; }

		internal bool jZvmQixnOgbmYeDtNVyCjgZHOfdeA => CTOnUkeLJWaoFhFSZhZYUzHZMedEA;

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
				MJCnBosQYbUlJanFrVIDjtRnnxaI();
			}
		}

		[CustomObfuscation(rename = false)]
		internal virtual void Awake()
		{
			zPhUbesvjLRLXdiRMucDSplQRHmW = true;
		}

		[CustomObfuscation(rename = false)]
		internal virtual void Start()
		{
		}

		[CustomObfuscation(rename = false)]
		internal virtual void OnEnable()
		{
			if (!zPhUbesvjLRLXdiRMucDSplQRHmW)
			{
				CTOnUkeLJWaoFhFSZhZYUzHZMedEA = false;
				StartCoroutine(QfRAEnWBgeiMzcvWiVxasGWylLBCb());
				zPhUbesvjLRLXdiRMucDSplQRHmW = true;
			}
			else if (Application.isPlaying)
			{
				GqsSwWLmQSwsSHeEkajTKVkUhkhyA();
			}
		}

		[CustomObfuscation(rename = false)]
		internal virtual void OnDisable()
		{
			if (Application.isPlaying)
			{
				rlhnesHIiClkaedBWUCjAEWvApgr();
			}
		}

		[CustomObfuscation(rename = false)]
		internal virtual void OnDestroy()
		{
		}

		[CustomObfuscation(rename = false)]
		internal virtual void OnValidate()
		{
			if (CTOnUkeLJWaoFhFSZhZYUzHZMedEA)
			{
				HadBWnCrxgdoWQnglUiOOyJMbEwsA();
			}
		}

		[CustomObfuscation(rename = false)]
		internal virtual void OnCanvasGroupChanged()
		{
			if (CTOnUkeLJWaoFhFSZhZYUzHZMedEA)
			{
				bgzpRlNoFuNmuSFQzeTbSIwsBmcq(false, false);
			}
		}

		[CustomObfuscation(rename = false)]
		internal virtual void OnTransformParentChanged()
		{
			if (CTOnUkeLJWaoFhFSZhZYUzHZMedEA)
			{
				bgzpRlNoFuNmuSFQzeTbSIwsBmcq(false, false);
			}
		}

		[CustomObfuscation(rename = false)]
		internal virtual void OnDidApplyAnimationProperties()
		{
			_ = CTOnUkeLJWaoFhFSZhZYUzHZMedEA;
		}

		[CustomObfuscation(rename = false)]
		internal virtual void Reset()
		{
			_ = CTOnUkeLJWaoFhFSZhZYUzHZMedEA;
		}

		internal virtual void MJCnBosQYbUlJanFrVIDjtRnnxaI()
		{
		}

		internal virtual bool pxdeSVsyHcDVUmhrbsZVhbRyToYn()
		{
			CTOnUkeLJWaoFhFSZhZYUzHZMedEA = false;
			if (!bgzpRlNoFuNmuSFQzeTbSIwsBmcq(true, true))
			{
				return false;
			}
			_controller.Register(this);
			return true;
		}

		internal virtual void rlhnesHIiClkaedBWUCjAEWvApgr()
		{
			ClearValue();
			if (!_controller.IsNullOrDestroyed())
			{
				_controller.Deregister(this);
			}
			aIvGAdqbAxMoEfcxxqSVxbEqtEqO();
			CTOnUkeLJWaoFhFSZhZYUzHZMedEA = false;
		}

		internal virtual void UgYfXMHEewSKZnKGFlKMEgiFeKHdA()
		{
			if (!_controller.IsNullOrDestroyed())
			{
				aIvGAdqbAxMoEfcxxqSVxbEqtEqO();
			}
		}

		internal virtual void aIvGAdqbAxMoEfcxxqSVxbEqtEqO()
		{
			_controller.IsNullOrDestroyed();
		}

		internal virtual void TmwAiSAtmeUBheGXtgadcPKUnBaqA()
		{
			if (CTOnUkeLJWaoFhFSZhZYUzHZMedEA)
			{
				HadBWnCrxgdoWQnglUiOOyJMbEwsA();
			}
		}

		internal virtual void oupESpAKdTodzbcjadIjdJbxShhrb()
		{
			_ = CTOnUkeLJWaoFhFSZhZYUzHZMedEA;
		}

		internal virtual void aDXIBIkpoKvZCbEJybRsDbebnqPpA()
		{
		}

		internal bool IcqbeYEmGpfkqqAVukZKtDJbdtuLA()
		{
			return UnityTools.IsActiveAndEnabled(this);
		}

		internal bool xpGKelZOWtEsrJtduJSHNrwIpnAsA()
		{
			return this == null;
		}

		internal IComponentController dtArSmnbMejNAcEKpEMEOdLulUFo()
		{
			return _controller;
		}

		[CustomObfuscation(rename = false)]
		internal abstract IComponentController FindController();

		[CustomObfuscation(rename = false)]
		internal abstract Type GetRequiredControllerType();

		[IteratorStateMachine(typeof(hIkaloBFBYIBWCoUaSSKFOvvrJGdB))]
		private IEnumerator QfRAEnWBgeiMzcvWiVxasGWylLBCb()
		{
			return new hIkaloBFBYIBWCoUaSSKFOvvrJGdB(0)
			{
				LQyQRGvGfaDlIcRPUoIHWJsGIHMMA = this
			};
		}

		private void GqsSwWLmQSwsSHeEkajTKVkUhkhyA()
		{
			if (pxdeSVsyHcDVUmhrbsZVhbRyToYn())
			{
				aDXIBIkpoKvZCbEJybRsDbebnqPpA();
				CTOnUkeLJWaoFhFSZhZYUzHZMedEA = true;
				UgYfXMHEewSKZnKGFlKMEgiFeKHdA();
			}
		}

		private bool bgzpRlNoFuNmuSFQzeTbSIwsBmcq(bool P_0, bool P_1)
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
					if (type == null)
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
					GqsSwWLmQSwsSHeEkajTKVkUhkhyA();
				}
				return true;
			}
			catch
			{
				rlhnesHIiClkaedBWUCjAEWvApgr();
				return false;
			}
		}

		private void HadBWnCrxgdoWQnglUiOOyJMbEwsA()
		{
			bgzpRlNoFuNmuSFQzeTbSIwsBmcq(false, true);
		}

		private void BVmasCKmjpbRcsHXFjsLGhzvrVBj()
		{
			if (!xpGKelZOWtEsrJtduJSHNrwIpnAsA() && IcqbeYEmGpfkqqAVukZKtDJbdtuLA())
			{
				MJCnBosQYbUlJanFrVIDjtRnnxaI();
			}
		}
	}
}
