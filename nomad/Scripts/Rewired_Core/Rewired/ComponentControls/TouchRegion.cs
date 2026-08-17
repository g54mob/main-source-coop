using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Rewired.ComponentControls
{
	[Serializable]
	[DisallowMultipleComponent]
	[AddComponentMenu("Rewired/Touch Controls/Touch Region")]
	public sealed class TouchRegion : TouchInteractable
	{
		[Serializable]
		private class QHluhNxkkoTduLIOWHXzECazwsWP : UnityEvent<PointerEventData>
		{
		}

		[Serializable]
		private class XiMOrFItaCrHzrMsSgJsZxAUtsAc : UnityEvent<PointerEventData>
		{
		}

		[Serializable]
		private class mbCOmJmPjCjAfxmjgkeMNTcdpIl : UnityEvent<PointerEventData>
		{
		}

		[Serializable]
		private class CoDMuLwXSWNqGyGvkGffjIbsQrzO : UnityEvent<PointerEventData>
		{
		}

		[Serializable]
		private class DbRCpPBfgXYqEEMPOdcKbrUbpLDs : UnityEvent<PointerEventData>
		{
		}

		[Serializable]
		private class jmDCoHfEnxxpWxaYzexjLLwtoAoo : UnityEvent<PointerEventData>
		{
		}

		[Serializable]
		private class ejlkmfLRElHqeBqjdVfixIvJvjsr : UnityEvent<PointerEventData>
		{
		}

		[Tooltip("If enabled, the Touch Region will be hidden when gameplay starts.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _hideAtRuntime = true;

		private QHluhNxkkoTduLIOWHXzECazwsWP _onPointerDown = new QHluhNxkkoTduLIOWHXzECazwsWP();

		private XiMOrFItaCrHzrMsSgJsZxAUtsAc _onPointerUp = new XiMOrFItaCrHzrMsSgJsZxAUtsAc();

		private mbCOmJmPjCjAfxmjgkeMNTcdpIl _onPointerEnter = new mbCOmJmPjCjAfxmjgkeMNTcdpIl();

		private CoDMuLwXSWNqGyGvkGffjIbsQrzO _onPointerExit = new CoDMuLwXSWNqGyGvkGffjIbsQrzO();

		private DbRCpPBfgXYqEEMPOdcKbrUbpLDs _onBeginDrag = new DbRCpPBfgXYqEEMPOdcKbrUbpLDs();

		private jmDCoHfEnxxpWxaYzexjLLwtoAoo _onDrag = new jmDCoHfEnxxpWxaYzexjLLwtoAoo();

		private ejlkmfLRElHqeBqjdVfixIvJvjsr _onEndDrag = new ejlkmfLRElHqeBqjdVfixIvJvjsr();

		public bool hideAtRuntime
		{
			get
			{
				return _hideAtRuntime;
			}
			set
			{
				if (!(_hideAtRuntime = value))
				{
					_hideAtRuntime = true;
					TmwAiSAtmeUBheGXtgadcPKUnBaqA();
				}
			}
		}

		public event UnityAction<PointerEventData> PointerDownEvent
		{
			add
			{
				_onPointerDown.AddListener(value);
			}
			remove
			{
				_onPointerDown.RemoveListener(value);
			}
		}

		public event UnityAction<PointerEventData> PointerUpEvent
		{
			add
			{
				_onPointerUp.AddListener(value);
			}
			remove
			{
				_onPointerUp.RemoveListener(value);
			}
		}

		public event UnityAction<PointerEventData> PointerEnterEvent
		{
			add
			{
				_onPointerEnter.AddListener(value);
			}
			remove
			{
				_onPointerEnter.RemoveListener(value);
			}
		}

		public event UnityAction<PointerEventData> PointerExitEvent
		{
			add
			{
				_onPointerExit.AddListener(value);
			}
			remove
			{
				_onPointerExit.RemoveListener(value);
			}
		}

		public event UnityAction<PointerEventData> BeginDragEvent
		{
			add
			{
				_onBeginDrag.AddListener(value);
			}
			remove
			{
				_onBeginDrag.RemoveListener(value);
			}
		}

		public event UnityAction<PointerEventData> DragEvent
		{
			add
			{
				_onDrag.AddListener(value);
			}
			remove
			{
				_onDrag.RemoveListener(value);
			}
		}

		public event UnityAction<PointerEventData> EndDragEvent
		{
			add
			{
				_onEndDrag.AddListener(value);
			}
			remove
			{
				_onEndDrag.RemoveListener(value);
			}
		}

		[CustomObfuscation(rename = false)]
		private TouchRegion()
		{
		}

		[CustomObfuscation(rename = false)]
		internal override void Awake()
		{
			base.Awake();
			if (Application.isPlaying && _hideAtRuntime)
			{
				base.visible = false;
			}
		}

		public override void ClearValue()
		{
		}

		internal void PnRhVzfIqKDaDFhGnsxFXdyhzMGJ()
		{
		}

		internal void wOnOhxebMXLhKOOmZtyGfuxrlTTv(PointerEventData P_0)
		{
			base.OnPointerDown(P_0);
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && IcqbeYEmGpfkqqAVukZKtDJbdtuLA() && IsInteractable() && TouchInteractable.MsaNPvMHfyDplAeqNkzTmIDcJhWQ(P_0.pointerId, base.allowedMouseButtons, EventTriggerType.PointerDown) && _onPointerDown != null)
			{
				_onPointerDown.Invoke(P_0);
			}
		}

		internal void GYOGXmTgFgKaYirqginXgJrSHzCg(PointerEventData P_0)
		{
			base.OnPointerUp(P_0);
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && IcqbeYEmGpfkqqAVukZKtDJbdtuLA() && IsInteractable() && TouchInteractable.MsaNPvMHfyDplAeqNkzTmIDcJhWQ(P_0.pointerId, base.allowedMouseButtons, EventTriggerType.PointerUp) && _onPointerUp != null)
			{
				_onPointerUp.Invoke(P_0);
			}
		}

		internal void nCYDxEUQCbhTrWUYnhMORxUOKkAt(PointerEventData P_0)
		{
			base.OnPointerEnter(P_0);
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && IcqbeYEmGpfkqqAVukZKtDJbdtuLA() && IsInteractable() && TouchInteractable.MsaNPvMHfyDplAeqNkzTmIDcJhWQ(P_0.pointerId, base.allowedMouseButtons, EventTriggerType.PointerEnter) && _onPointerEnter != null)
			{
				_onPointerEnter.Invoke(P_0);
			}
		}

		internal void BWqFsKXiieGAtNJEKdnPGUeyJHdm(PointerEventData P_0)
		{
			base.OnPointerExit(P_0);
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && IcqbeYEmGpfkqqAVukZKtDJbdtuLA() && IsInteractable() && TouchInteractable.MsaNPvMHfyDplAeqNkzTmIDcJhWQ(P_0.pointerId, base.allowedMouseButtons, EventTriggerType.PointerExit) && _onPointerExit != null)
			{
				_onPointerExit.Invoke(P_0);
			}
		}

		internal void VAKOZIcCBfjwYEckFnmCoRAgsqgfA(PointerEventData P_0)
		{
			base.OnBeginDrag(P_0);
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && IcqbeYEmGpfkqqAVukZKtDJbdtuLA() && IsInteractable() && TouchInteractable.MsaNPvMHfyDplAeqNkzTmIDcJhWQ(P_0.pointerId, base.allowedMouseButtons, EventTriggerType.BeginDrag) && _onBeginDrag != null)
			{
				_onBeginDrag.Invoke(P_0);
			}
		}

		internal void pURGhblcLVqAMMUwNEBiUDoUkKTI(PointerEventData P_0)
		{
			base.OnDrag(P_0);
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && IcqbeYEmGpfkqqAVukZKtDJbdtuLA() && IsInteractable() && TouchInteractable.MsaNPvMHfyDplAeqNkzTmIDcJhWQ(P_0.pointerId, base.allowedMouseButtons, EventTriggerType.Drag) && _onDrag != null)
			{
				_onDrag.Invoke(P_0);
			}
		}

		internal void DBJRJCVOGRcCzzHGySGegEiJnBSx(PointerEventData P_0)
		{
			base.OnEndDrag(P_0);
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && IcqbeYEmGpfkqqAVukZKtDJbdtuLA() && IsInteractable() && TouchInteractable.MsaNPvMHfyDplAeqNkzTmIDcJhWQ(P_0.pointerId, base.allowedMouseButtons, EventTriggerType.EndDrag) && _onEndDrag != null)
			{
				_onEndDrag.Invoke(P_0);
			}
		}
	}
}
