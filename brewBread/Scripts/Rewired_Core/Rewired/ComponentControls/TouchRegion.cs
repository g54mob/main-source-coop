using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Rewired.ComponentControls
{
	[Serializable]
	[DisallowMultipleComponent]
	[AddComponentMenu("Rewired/Touch Region")]
	public sealed class TouchRegion : TouchInteractable
	{
		[Serializable]
		private class ghVKEZJBJYQJvvstwXJnrgnXfsDt : UnityEvent<PointerEventData>
		{
		}

		[Serializable]
		private class lTQAltguQayUUSZjuRqVFyuoqtzBA : UnityEvent<PointerEventData>
		{
		}

		[Serializable]
		private class INFDjiJnyNyrBINFJecsvtYIqrPeA : UnityEvent<PointerEventData>
		{
		}

		[Serializable]
		private class clhedBWPziouPAAOAIrdSYmUdssL : UnityEvent<PointerEventData>
		{
		}

		[Serializable]
		private class bMdkKDfmBnVLDeOqapsCIEZDnLIq : UnityEvent<PointerEventData>
		{
		}

		[Serializable]
		private class XFhydDPwQZlOXNQvZDlvktxFlUpT : UnityEvent<PointerEventData>
		{
		}

		[Serializable]
		private class ItJCBvpQxToTdxKYPgpoCjgfAhxHA : UnityEvent<PointerEventData>
		{
		}

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("If enabled, the Touch Region will be hidden when gameplay starts.")]
		private bool _hideAtRuntime = true;

		private ghVKEZJBJYQJvvstwXJnrgnXfsDt _onPointerDown = new ghVKEZJBJYQJvvstwXJnrgnXfsDt();

		private lTQAltguQayUUSZjuRqVFyuoqtzBA _onPointerUp = new lTQAltguQayUUSZjuRqVFyuoqtzBA();

		private INFDjiJnyNyrBINFJecsvtYIqrPeA _onPointerEnter = new INFDjiJnyNyrBINFJecsvtYIqrPeA();

		private clhedBWPziouPAAOAIrdSYmUdssL _onPointerExit = new clhedBWPziouPAAOAIrdSYmUdssL();

		private bMdkKDfmBnVLDeOqapsCIEZDnLIq _onBeginDrag = new bMdkKDfmBnVLDeOqapsCIEZDnLIq();

		private XFhydDPwQZlOXNQvZDlvktxFlUpT _onDrag = new XFhydDPwQZlOXNQvZDlvktxFlUpT();

		private ItJCBvpQxToTdxKYPgpoCjgfAhxHA _onEndDrag = new ItJCBvpQxToTdxKYPgpoCjgfAhxHA();

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
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
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

		internal override void xhHhPOcavYUrBJGXQmMDzQMMGISaA()
		{
		}

		internal void jdTaWudIAtigvcgWdtrrcBEKJLcPB(PointerEventData P_0)
		{
			base.OnPointerDown(P_0);
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && EDufGzVNigBlMAOWvMGsHZmtQaph() && IsInteractable() && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(P_0.pointerId, base.allowedMouseButtons, EventTriggerType.PointerDown) && _onPointerDown != null)
			{
				_onPointerDown.Invoke(P_0);
			}
		}

		internal void MbXbjtguZaBfbfqmjlBSCyVMORejB(PointerEventData P_0)
		{
			base.OnPointerUp(P_0);
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && EDufGzVNigBlMAOWvMGsHZmtQaph() && IsInteractable() && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(P_0.pointerId, base.allowedMouseButtons, EventTriggerType.PointerUp) && _onPointerUp != null)
			{
				_onPointerUp.Invoke(P_0);
			}
		}

		internal void vpQLxEpNsNjHEQUMTNafaJqaWgfe(PointerEventData P_0)
		{
			base.OnPointerEnter(P_0);
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && EDufGzVNigBlMAOWvMGsHZmtQaph() && IsInteractable() && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(P_0.pointerId, base.allowedMouseButtons, EventTriggerType.PointerEnter) && _onPointerEnter != null)
			{
				_onPointerEnter.Invoke(P_0);
			}
		}

		internal void OCiTmGENfjrRiUQnHUcKDaHRzTDj(PointerEventData P_0)
		{
			base.OnPointerExit(P_0);
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && EDufGzVNigBlMAOWvMGsHZmtQaph() && IsInteractable() && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(P_0.pointerId, base.allowedMouseButtons, EventTriggerType.PointerExit) && _onPointerExit != null)
			{
				_onPointerExit.Invoke(P_0);
			}
		}

		internal void CeCjfxfrPRDIAfCVrQPnmMvELNpzA(PointerEventData P_0)
		{
			base.OnBeginDrag(P_0);
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && EDufGzVNigBlMAOWvMGsHZmtQaph() && IsInteractable() && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(P_0.pointerId, base.allowedMouseButtons, EventTriggerType.BeginDrag) && _onBeginDrag != null)
			{
				_onBeginDrag.Invoke(P_0);
			}
		}

		internal void JuzlkzmoAOuJMYLEkFxWgiAjnolW(PointerEventData P_0)
		{
			base.OnDrag(P_0);
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && EDufGzVNigBlMAOWvMGsHZmtQaph() && IsInteractable() && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(P_0.pointerId, base.allowedMouseButtons, EventTriggerType.Drag) && _onDrag != null)
			{
				_onDrag.Invoke(P_0);
			}
		}

		internal void LgBQosPAyBTzQOLHtYNFjeRHthOd(PointerEventData P_0)
		{
			base.OnEndDrag(P_0);
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && EDufGzVNigBlMAOWvMGsHZmtQaph() && IsInteractable() && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(P_0.pointerId, base.allowedMouseButtons, EventTriggerType.EndDrag) && _onEndDrag != null)
			{
				_onEndDrag.Invoke(P_0);
			}
		}
	}
}
