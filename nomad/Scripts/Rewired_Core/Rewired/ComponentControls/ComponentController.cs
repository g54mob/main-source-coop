using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rewired.Utils;
using Rewired.Utils.Interfaces;
using UnityEngine;

namespace Rewired.ComponentControls
{
	[Serializable]
	[DisallowMultipleComponent]
	public abstract class ComponentController : MonoBehaviour, IComponentController, IRegistrar<IComponentControl>
	{
		private sealed class hbCpcTduxFquwwPvAgCbHiMrNTtOA : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int IHVYgRXzjYrpmZHioaKChSjYhGKRA;

			private object WyVCIFOzNLHJaKbxPOjVQoLxPrsJ;

			public ComponentController QplLANzcCWNaInbdZefRNqoyLPtr;

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return WyVCIFOzNLHJaKbxPOjVQoLxPrsJ;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return WyVCIFOzNLHJaKbxPOjVQoLxPrsJ;
				}
			}

			[DebuggerHidden]
			public hbCpcTduxFquwwPvAgCbHiMrNTtOA(int P_0)
			{
				IHVYgRXzjYrpmZHioaKChSjYhGKRA = P_0;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				IHVYgRXzjYrpmZHioaKChSjYhGKRA = -2;
			}

			private bool MoveNext()
			{
				int iHVYgRXzjYrpmZHioaKChSjYhGKRA = IHVYgRXzjYrpmZHioaKChSjYhGKRA;
				ComponentController qplLANzcCWNaInbdZefRNqoyLPtr = QplLANzcCWNaInbdZefRNqoyLPtr;
				switch (iHVYgRXzjYrpmZHioaKChSjYhGKRA)
				{
				default:
					return false;
				case 0:
					IHVYgRXzjYrpmZHioaKChSjYhGKRA = -1;
					WyVCIFOzNLHJaKbxPOjVQoLxPrsJ = null;
					IHVYgRXzjYrpmZHioaKChSjYhGKRA = 1;
					return true;
				case 1:
					IHVYgRXzjYrpmZHioaKChSjYhGKRA = -1;
					qplLANzcCWNaInbdZefRNqoyLPtr.qPzMzUjDhFNlQOStJtCJTkIYOJuW();
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
		private bool GCSWViIsIokvfQMADVhHqvmSVvem;

		[NonSerialized]
		private bool waDIJuoHpWQRWqdzqfYqtmNPmoFq;

		private List<IComponentControl> _controls = new List<IComponentControl>(10);

		internal bool qnvpaSZJXiOHXHHvNKlMjyoMoMcS => GCSWViIsIokvfQMADVhHqvmSVvem;

		[CustomObfuscation(rename = false)]
		internal ComponentController()
		{
		}

		[CustomObfuscation(rename = false)]
		internal virtual void Awake()
		{
			waDIJuoHpWQRWqdzqfYqtmNPmoFq = true;
		}

		[CustomObfuscation(rename = false)]
		internal virtual void Update()
		{
			if (!GCSWViIsIokvfQMADVhHqvmSVvem)
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
			if (!waDIJuoHpWQRWqdzqfYqtmNPmoFq)
			{
				StartCoroutine(iDUGJIBgWwfXpXODdAkCncpSdHrQ());
				waDIJuoHpWQRWqdzqfYqtmNPmoFq = true;
			}
			else
			{
				qPzMzUjDhFNlQOStJtCJTkIYOJuW();
			}
		}

		[CustomObfuscation(rename = false)]
		internal virtual void OnDisable()
		{
			if (GCSWViIsIokvfQMADVhHqvmSVvem)
			{
				CqiGYPTZeUpHdwdRKzVRvGmXZFAt();
			}
		}

		[CustomObfuscation(rename = false)]
		internal virtual void OnValidate()
		{
			if (GCSWViIsIokvfQMADVhHqvmSVvem)
			{
				qoDBSdPcPEEFMEqoiAeXEwVpjAup();
			}
		}

		[CustomObfuscation(rename = false)]
		internal virtual void OnDestroy()
		{
			_controls.Clear();
		}

		internal virtual bool nOOsAqwpQqySAZWspjurFNceJHMR()
		{
			return true;
		}

		internal virtual void QTVSlGdkixxffTWWemtJmqnfNogq()
		{
			CqiGYPTZeUpHdwdRKzVRvGmXZFAt();
		}

		internal virtual void CqiGYPTZeUpHdwdRKzVRvGmXZFAt()
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
			if (!GCSWViIsIokvfQMADVhHqvmSVvem)
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

		void IComponentController.ClearControlValues()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ClearControlValues
			this.ClearControlValues();
		}

		private void qPzMzUjDhFNlQOStJtCJTkIYOJuW()
		{
			if (nOOsAqwpQqySAZWspjurFNceJHMR())
			{
				GCSWViIsIokvfQMADVhHqvmSVvem = true;
				QTVSlGdkixxffTWWemtJmqnfNogq();
			}
		}

		private void qoDBSdPcPEEFMEqoiAeXEwVpjAup()
		{
			_ = qnvpaSZJXiOHXHHvNKlMjyoMoMcS;
		}

		[IteratorStateMachine(typeof(hbCpcTduxFquwwPvAgCbHiMrNTtOA))]
		private IEnumerator iDUGJIBgWwfXpXODdAkCncpSdHrQ()
		{
			return new hbCpcTduxFquwwPvAgCbHiMrNTtOA(0)
			{
				QplLANzcCWNaInbdZefRNqoyLPtr = this
			};
		}
	}
}
