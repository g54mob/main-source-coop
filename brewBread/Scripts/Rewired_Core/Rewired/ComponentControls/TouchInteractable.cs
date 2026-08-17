using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Rewired.UI;
using Rewired.Utils;
using Rewired.Utils.Attributes;
using Rewired.Utils.Classes.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Rewired.ComponentControls
{
	[Serializable]
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	public abstract class TouchInteractable : TouchControl, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		public enum InteractionState
		{
			Normal = 0,
			Highlighted = 1,
			Pressed = 2,
			Disabled = 3
		}

		[Flags]
		public enum TransitionTypeFlags
		{
			None = 0,
			ColorTint = 1,
			SpriteSwap = 2,
			Animation = 4
		}

		[Flags]
		public enum MouseButtonFlags
		{
			None = 0,
			LeftButton = 1,
			RightButton = 2,
			MiddleButton = 4,
			AnyButton = -1
		}

		[Serializable]
		public class InteractionStateTransitionEventHandler : UnityEvent<InteractionStateTransitionArgs>
		{
		}

		[Serializable]
		public class VisibilityChangedEventHandler : UnityEvent<bool>
		{
		}

		public class InteractionStateTransitionArgs
		{
			private TouchInteractable mdNLcIIFZogxyEqNhtpLBiKIlHKdb;

			private InteractionState IcXfJYgXoAwFSLAcAfqjnBioHaOP;

			private float hRUQtuehVIxYwRwXawPyYQlptzd;

			public TouchInteractable sender => mdNLcIIFZogxyEqNhtpLBiKIlHKdb;

			public InteractionState state => IcXfJYgXoAwFSLAcAfqjnBioHaOP;

			public float duration => hRUQtuehVIxYwRwXawPyYQlptzd;

			internal InteractionStateTransitionArgs()
			{
			}

			internal void MPIdpLqhJeCJHWkSZORdXwngeGgr(TouchInteractable P_0, InteractionState P_1, float P_2)
			{
				mdNLcIIFZogxyEqNhtpLBiKIlHKdb = P_0;
				IcXfJYgXoAwFSLAcAfqjnBioHaOP = P_1;
				hRUQtuehVIxYwRwXawPyYQlptzd = P_2;
			}
		}

		public interface IInteractionStateTransitionHandler
		{
			void OnInteractionStateTransition(InteractionStateTransitionArgs data);
		}

		[Serializable]
		private sealed class sAYmuinILTdHAaVFmcDvAtpjkrKib
		{
			public static readonly sAYmuinILTdHAaVFmcDvAtpjkrKib _003C_003E9 = new sAYmuinILTdHAaVFmcDvAtpjkrKib();

			public static tASbVocwrkZfBjDhttUGkjNilJQyA.EventFunction<IInteractionStateTransitionHandler, InteractionStateTransitionArgs> _003C_003E9__152_0;

			internal void sKyZmbYJtmutnzXBpDUqXErvBbehA(IInteractionStateTransitionHandler P_0, InteractionStateTransitionArgs P_1)
			{
				P_0.OnInteractionStateTransition(P_1);
			}
		}

		public const int POINTER_ID_NULL = int.MinValue;

		public const int POINTER_ID_MOUSE_LEFT_BUTTON = -1;

		public const int POINTER_ID_MOUSE_RIGHT_BUTTON = -2;

		public const int POINTER_ID_MOUSE_MIDDLE_BUTTON = -3;

		internal const int MAX_MOUSE_BUTTONS = 3;

		[Tooltip("Toggles whether the control can be interacted with by the user.")]
		[CustomObfuscation(rename = false)]
		[SerializeField]
		private bool _interactable = true;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		[Tooltip("Toggles visibility. An invisible control can still be interacted with. This property only has any effect when used with an Image Component.")]
		private bool _visible = true;

		[SerializeField]
		[Tooltip("Sets visibility to False when the control is idle. When the control is no longer idle, visibility will be set to True again.")]
		[CustomObfuscation(rename = false)]
		private bool _hideWhenIdle;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		[Tooltip("The mouse buttons that are allowed to interact with this control.")]
		[Bitmask(typeof(MouseButtonFlags))]
		private MouseButtonFlags _allowedMouseButtons = MouseButtonFlags.LeftButton;

		[Tooltip("The transition type(s) to be used when transitioning to various states. Multiple transition types can be used simultaneously.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Bitmask(typeof(TransitionTypeFlags))]
		private TransitionTypeFlags _transitionType;

		[SerializeField]
		[Tooltip("Settings using for Color Tint transitions.")]
		[CustomObfuscation(rename = false)]
		private ColorBlock _transitionColorTint = new ColorBlock
		{
			colorMultiplier = 1f,
			disabledColor = new Color(25f / 32f, 25f / 32f, 25f / 32f, 0.5f),
			highlightedColor = Color.white,
			normalColor = Color.white,
			pressedColor = Color.white,
			fadeDuration = 0.1f
		};

		[Tooltip("Settings using for Sprite State transitions.")]
		[CustomObfuscation(rename = false)]
		[SerializeField]
		private SpriteState _transitionSpriteState;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		[Tooltip("Settings using for Animation Trigger transitions.")]
		private AnimationTriggers _transitionAnimationTriggers = new AnimationTriggers();

		[SerializeField]
		[Tooltip("The target Graphic component for interaction state transitions. This should normally be set to an Image component on this GameObject.")]
		[CustomObfuscation(rename = false)]
		private Graphic _targetGraphic;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("Event sent when the Interaction State changes.")]
		private InteractionStateTransitionEventHandler _onInteractionStateTransition = new InteractionStateTransitionEventHandler();

		[Tooltip("Event sent when visibility changes.")]
		[CustomObfuscation(rename = false)]
		[SerializeField]
		private VisibilityChangedEventHandler _onVisibilityChanged = new VisibilityChangedEventHandler();

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("Event sent when interaction state changes to Normal.")]
		private UnityEvent _onInteractionStateChangedToNormal = new UnityEvent();

		[SerializeField]
		[Tooltip("Event sent when interaction state changes to Highlighted.")]
		[CustomObfuscation(rename = false)]
		private UnityEvent _onInteractionStateChangedToHighlighted = new UnityEvent();

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("Event sent when interaction state changes to Pressed.")]
		private UnityEvent _onInteractionStateChangedToPressed = new UnityEvent();

		[Tooltip("Event sent when interaction state changes to Disabled.")]
		[CustomObfuscation(rename = false)]
		[SerializeField]
		private UnityEvent _onInteractionStateChangedToDisabled = new UnityEvent();

		private readonly List<CanvasGroup> _canvasGroupCache = new List<CanvasGroup>();

		private bool _groupsAllowInteraction = true;

		private InteractionState _interactionState;

		[NonSerialized]
		private bool zmNsczlnaJKPNGfDEeyyFdsXhpjgb;

		[NonSerialized]
		private bool ALBJgfsRYJdMVJwePTiUwIfQxDNM;

		private bool _varWatch_visible;

		private bool _varWatch_interactable;

		private bool _allowSendingEvents = true;

		private static InteractionStateTransitionArgs _transitionArgs = new InteractionStateTransitionArgs();

		private tASbVocwrkZfBjDhttUGkjNilJQyA.HierarchyEventHelper<IVisibilityChangedHandler, bool> __hierarchyVisibilityChangedHandlers;

		private tASbVocwrkZfBjDhttUGkjNilJQyA.HierarchyEventHelper<IInteractionStateTransitionHandler, InteractionStateTransitionArgs> __hierarchyInteractionStateTransitionHandlers;

		private static tASbVocwrkZfBjDhttUGkjNilJQyA.EventFunction<IInteractionStateTransitionHandler, InteractionStateTransitionArgs> __interactionStateTransitionHandlerDelegate;

		private tASbVocwrkZfBjDhttUGkjNilJQyA.HierarchyEventHelper<IVisibilityChangedHandler, bool> wLIJGrhiyTLONXbMWiyIGwppCVOk
		{
			get
			{
				if (__hierarchyVisibilityChangedHandlers == null)
				{
					__hierarchyVisibilityChangedHandlers = new tASbVocwrkZfBjDhttUGkjNilJQyA.HierarchyEventHelper<IVisibilityChangedHandler, bool>(RklasgGeTvtYFbFRQkScwuFFaDKN.dlEAhqkPbPrIBkDddEHiKkhspEspA);
					__hierarchyVisibilityChangedHandlers.GetHandlers(base.transform);
				}
				return __hierarchyVisibilityChangedHandlers;
			}
		}

		private tASbVocwrkZfBjDhttUGkjNilJQyA.HierarchyEventHelper<IInteractionStateTransitionHandler, InteractionStateTransitionArgs> ffhjypFMUoylOtTdlgaohUZSHIqR
		{
			get
			{
				if (__hierarchyInteractionStateTransitionHandlers == null)
				{
					__hierarchyInteractionStateTransitionHandlers = new tASbVocwrkZfBjDhttUGkjNilJQyA.HierarchyEventHelper<IInteractionStateTransitionHandler, InteractionStateTransitionArgs>(acccfaKFznMHaCXBovceQcVIvoxB);
					__hierarchyInteractionStateTransitionHandlers.GetHandlers(base.transform);
				}
				return __hierarchyInteractionStateTransitionHandlers;
			}
		}

		public bool interactable
		{
			get
			{
				return _interactable;
			}
			set
			{
				if (_interactable != value)
				{
					_interactable = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public bool visible
		{
			get
			{
				return _visible;
			}
			set
			{
				if (visible != value)
				{
					nnXqcpdOcMbRXjEWYqRtWFgsBQGCb(value, false);
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public bool hideWhenIdle
		{
			get
			{
				return _hideWhenIdle;
			}
			set
			{
				if (_hideWhenIdle != value)
				{
					_hideWhenIdle = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public MouseButtonFlags allowedMouseButtons
		{
			get
			{
				return _allowedMouseButtons;
			}
			set
			{
				if (_allowedMouseButtons != value)
				{
					_allowedMouseButtons = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public TransitionTypeFlags transitionType
		{
			get
			{
				return _transitionType;
			}
			set
			{
				if (_transitionType != value)
				{
					_transitionType = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public ColorBlock transitionColorTint
		{
			get
			{
				return _transitionColorTint;
			}
			set
			{
				_transitionColorTint = value;
				FyKKzlnIjsJaSimYwuqKSwNJHIHx();
			}
		}

		public SpriteState transitionSpriteState
		{
			get
			{
				return _transitionSpriteState;
			}
			set
			{
				if (!_transitionSpriteState.Equals(value))
				{
					_transitionSpriteState = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public AnimationTriggers transitionAnimationTriggers
		{
			get
			{
				return _transitionAnimationTriggers;
			}
			set
			{
				if (_transitionAnimationTriggers != value)
				{
					_transitionAnimationTriggers = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public Graphic targetGraphic
		{
			get
			{
				return _targetGraphic;
			}
			set
			{
				if (!(_targetGraphic == value))
				{
					_targetGraphic = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public Image image
		{
			get
			{
				return _targetGraphic as Image;
			}
			set
			{
				if (!(_targetGraphic == value))
				{
					_targetGraphic = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public Animator animator => base.gameObject.GetComponent<Animator>();

		public InteractionState interactionState => _interactionState;

		internal static tASbVocwrkZfBjDhttUGkjNilJQyA.EventFunction<IInteractionStateTransitionHandler, InteractionStateTransitionArgs> acccfaKFznMHaCXBovceQcVIvoxB
		{
			get
			{
				if (__interactionStateTransitionHandlerDelegate == null)
				{
					__interactionStateTransitionHandlerDelegate = sAYmuinILTdHAaVFmcDvAtpjkrKib._003C_003E9.sKyZmbYJtmutnzXBpDUqXErvBbehA;
				}
				return __interactionStateTransitionHandlerDelegate;
			}
		}

		public event UnityAction<InteractionStateTransitionArgs> InteractionStateSetEvent
		{
			add
			{
				_onInteractionStateTransition.AddListener(value);
			}
			remove
			{
				_onInteractionStateTransition.RemoveListener(value);
			}
		}

		public event UnityAction<bool> VisibilityChangedEvent
		{
			add
			{
				_onVisibilityChanged.AddListener(value);
			}
			remove
			{
				_onVisibilityChanged.RemoveListener(value);
			}
		}

		public event UnityAction InteractionStateChangedToNormal
		{
			add
			{
				_onInteractionStateChangedToNormal.AddListener(value);
			}
			remove
			{
				_onInteractionStateChangedToNormal.RemoveListener(value);
			}
		}

		public event UnityAction InteractionStateChangedToHighlighted
		{
			add
			{
				_onInteractionStateChangedToHighlighted.AddListener(value);
			}
			remove
			{
				_onInteractionStateChangedToHighlighted.RemoveListener(value);
			}
		}

		public event UnityAction InteractionStateChangedToPressed
		{
			add
			{
				_onInteractionStateChangedToPressed.AddListener(value);
			}
			remove
			{
				_onInteractionStateChangedToPressed.RemoveListener(value);
			}
		}

		public event UnityAction InteractionStateChangedToDisabled
		{
			add
			{
				_onInteractionStateChangedToDisabled.AddListener(value);
			}
			remove
			{
				_onInteractionStateChangedToDisabled.RemoveListener(value);
			}
		}

		[CustomObfuscation(rename = false)]
		internal TouchInteractable()
		{
		}

		[CustomObfuscation(rename = false)]
		internal override void Awake()
		{
			base.Awake();
			if (Application.isPlaying)
			{
				if (_targetGraphic == null)
				{
					_targetGraphic = base.gameObject.GetComponent<Graphic>();
				}
				ZOHvREfiEGuSoZkpvCoiaVVliwbJ();
			}
		}

		[CustomObfuscation(rename = false)]
		internal override void OnCanvasGroupChanged()
		{
			base.OnCanvasGroupChanged();
			bool flag = true;
			Transform parent = base.transform;
			while (parent != null)
			{
				parent.GetComponents(_canvasGroupCache);
				bool flag2 = false;
				for (int i = 0; i < _canvasGroupCache.Count; i++)
				{
					if (!_canvasGroupCache[i].interactable)
					{
						flag = false;
						flag2 = true;
					}
					if (_canvasGroupCache[i].ignoreParentGroups)
					{
						flag2 = true;
					}
				}
				if (flag2)
				{
					break;
				}
				parent = parent.parent;
			}
			if (flag != _groupsAllowInteraction)
			{
				_groupsAllowInteraction = flag;
				khkdMwnTAvlDqPvCkDAZhRJFGYZR();
			}
		}

		[CustomObfuscation(rename = false)]
		internal override void OnDidApplyAnimationProperties()
		{
			base.OnDidApplyAnimationProperties();
			khkdMwnTAvlDqPvCkDAZhRJFGYZR();
		}

		[CustomObfuscation(rename = false)]
		internal override void OnEnable()
		{
			base.OnEnable();
			if (!Application.isPlaying)
			{
				ZOHvREfiEGuSoZkpvCoiaVVliwbJ();
			}
			nZWiGUpTvdikApDJloojrjuVmTkT(InteractionState.Normal);
			SLncskcXOyNomyPEQOTFtPPeUMxl(true);
		}

		[CustomObfuscation(rename = false)]
		internal override void OnDisable()
		{
			INwDgTcjRJhaRDZhFMUCkMFDOPNsb();
			base.OnDisable();
		}

		[CustomObfuscation(rename = false)]
		internal override void OnValidate()
		{
			base.OnValidate();
			_transitionColorTint.fadeDuration = Mathf.Max(_transitionColorTint.fadeDuration, 0f);
			if (EDufGzVNigBlMAOWvMGsHZmtQaph())
			{
				if (!_interactable && EventSystem.current != null && EventSystem.current.currentSelectedGameObject == base.gameObject)
				{
					EventSystem.current.SetSelectedGameObject(null);
				}
				iPNYmRKNmVKLLvYVczPJNDOhpNp(null);
				xlgQtdzFtohQezQAfGKiojQhbSkC(Color.white, true);
				nDvULZqHixCpCetWuBUNEVUJxDFfA(_transitionAnimationTriggers.normalTrigger);
				SLncskcXOyNomyPEQOTFtPPeUMxl(true);
			}
			VeGtVHSUfBgpypkoecTaTsuYNRxB();
			khkdMwnTAvlDqPvCkDAZhRJFGYZR();
		}

		[CustomObfuscation(rename = false)]
		internal override void Reset()
		{
			_targetGraphic = base.gameObject.GetComponent<Graphic>();
			_allowedMouseButtons = MouseButtonFlags.LeftButton;
			base.Reset();
		}

		internal override void FyKKzlnIjsJaSimYwuqKSwNJHIHx()
		{
			base.FyKKzlnIjsJaSimYwuqKSwNJHIHx();
			khkdMwnTAvlDqPvCkDAZhRJFGYZR();
		}

		internal override void AOKLCSwSOHWHrUKFiqLEizXMqAyJ()
		{
			base.AOKLCSwSOHWHrUKFiqLEizXMqAyJ();
			VeGtVHSUfBgpypkoecTaTsuYNRxB();
		}

		private void INwDgTcjRJhaRDZhFMUCkMFDOPNsb()
		{
			string normalTrigger = _transitionAnimationTriggers.normalTrigger;
			zmNsczlnaJKPNGfDEeyyFdsXhpjgb = false;
			ALBJgfsRYJdMVJwePTiUwIfQxDNM = false;
			if ((_transitionType & TransitionTypeFlags.ColorTint) != TransitionTypeFlags.None)
			{
				xlgQtdzFtohQezQAfGKiojQhbSkC(Color.white, true);
			}
			if ((_transitionType & TransitionTypeFlags.SpriteSwap) != TransitionTypeFlags.None)
			{
				iPNYmRKNmVKLLvYVczPJNDOhpNp(null);
			}
			if ((_transitionType & TransitionTypeFlags.Animation) != TransitionTypeFlags.None)
			{
				nDvULZqHixCpCetWuBUNEVUJxDFfA(normalTrigger);
			}
		}

		private void hglCsrIBOMheHfVQdYkhhXZRzOHp(InteractionState P_0, bool P_1)
		{
			Color color;
			Sprite sprite;
			string text;
			UnityEvent unityEvent;
			switch (P_0)
			{
			case InteractionState.Normal:
				color = _transitionColorTint.normalColor;
				sprite = null;
				text = _transitionAnimationTriggers.normalTrigger;
				unityEvent = _onInteractionStateChangedToNormal;
				break;
			case InteractionState.Highlighted:
				color = _transitionColorTint.highlightedColor;
				sprite = _transitionSpriteState.highlightedSprite;
				text = _transitionAnimationTriggers.highlightedTrigger;
				unityEvent = _onInteractionStateChangedToHighlighted;
				break;
			case InteractionState.Pressed:
				color = _transitionColorTint.pressedColor;
				sprite = _transitionSpriteState.pressedSprite;
				text = _transitionAnimationTriggers.pressedTrigger;
				unityEvent = _onInteractionStateChangedToPressed;
				break;
			case InteractionState.Disabled:
				color = _transitionColorTint.disabledColor;
				sprite = _transitionSpriteState.disabledSprite;
				text = _transitionAnimationTriggers.disabledTrigger;
				unityEvent = _onInteractionStateChangedToDisabled;
				break;
			default:
				color = Color.black;
				sprite = null;
				text = string.Empty;
				unityEvent = null;
				break;
			}
			bool flag = (_transitionType & TransitionTypeFlags.ColorTint) != 0;
			if (!flag)
			{
				color = Color.white;
			}
			if (!_visible)
			{
				color.a = 0f;
			}
			if (base.gameObject.activeInHierarchy)
			{
				if (flag)
				{
					xlgQtdzFtohQezQAfGKiojQhbSkC(color * _transitionColorTint.colorMultiplier, P_1);
				}
				else
				{
					xlgQtdzFtohQezQAfGKiojQhbSkC(color, P_1);
				}
				if ((_transitionType & TransitionTypeFlags.SpriteSwap) != TransitionTypeFlags.None)
				{
					iPNYmRKNmVKLLvYVczPJNDOhpNp(sprite);
				}
				if ((_transitionType & TransitionTypeFlags.Animation) != TransitionTypeFlags.None)
				{
					nDvULZqHixCpCetWuBUNEVUJxDFfA(text);
				}
			}
			if (_allowSendingEvents)
			{
				_transitionArgs.MPIdpLqhJeCJHWkSZORdXwngeGgr(this, P_0, P_1 ? 0f : _transitionColorTint.fadeDuration);
				ffhjypFMUoylOtTdlgaohUZSHIqR.ExecuteOnAll(_transitionArgs);
				if (_onInteractionStateTransition != null)
				{
					_onInteractionStateTransition.Invoke(_transitionArgs);
				}
				unityEvent?.Invoke();
			}
		}

		private void xlgQtdzFtohQezQAfGKiojQhbSkC(Color P_0, bool P_1)
		{
			if (!(_targetGraphic == null))
			{
				_targetGraphic.CrossFadeColor(P_0, P_1 ? 0f : _transitionColorTint.fadeDuration, ignoreTimeScale: true, useAlpha: true);
			}
		}

		private void iPNYmRKNmVKLLvYVczPJNDOhpNp(Sprite P_0)
		{
			if (!(image == null))
			{
				image.overrideSprite = P_0;
			}
		}

		private void nDvULZqHixCpCetWuBUNEVUJxDFfA(string P_0)
		{
			if ((_transitionType & TransitionTypeFlags.Animation) != TransitionTypeFlags.None && !(animator == null) && UnityTools.IsActiveAndEnabled(animator) && !(animator.runtimeAnimatorController == null) && !string.IsNullOrEmpty(P_0))
			{
				animator.ResetTrigger(_transitionAnimationTriggers.normalTrigger);
				animator.ResetTrigger(_transitionAnimationTriggers.pressedTrigger);
				animator.ResetTrigger(_transitionAnimationTriggers.highlightedTrigger);
				animator.ResetTrigger(_transitionAnimationTriggers.disabledTrigger);
				animator.SetTrigger(P_0);
			}
		}

		private void SLncskcXOyNomyPEQOTFtPPeUMxl(bool P_0)
		{
			InteractionState interactionState = _interactionState;
			if (EDufGzVNigBlMAOWvMGsHZmtQaph() && !IsInteractable())
			{
				interactionState = InteractionState.Disabled;
			}
			hglCsrIBOMheHfVQdYkhhXZRzOHp(interactionState, P_0);
		}

		public bool IsInteractable()
		{
			if (_groupsAllowInteraction)
			{
				return _interactable;
			}
			return false;
		}

		internal virtual bool jlMJYbxJlzjsBwCBJxvDZPhnPAZF()
		{
			if (!EDufGzVNigBlMAOWvMGsHZmtQaph())
			{
				return false;
			}
			if (zmNsczlnaJKPNGfDEeyyFdsXhpjgb)
			{
				return ALBJgfsRYJdMVJwePTiUwIfQxDNM;
			}
			return false;
		}

		internal void MyVxLtVZLEiRaloNqnuyxYfDkCJB(BaseEventData P_0)
		{
			if (EDufGzVNigBlMAOWvMGsHZmtQaph() && IsInteractable())
			{
				InteractionState interactionState = BsKBAlIXlthRFOWqhXlGmOZChYRP(P_0);
				if (interactionState != _interactionState)
				{
					nZWiGUpTvdikApDJloojrjuVmTkT(interactionState);
					SLncskcXOyNomyPEQOTFtPPeUMxl(false);
				}
			}
		}

		internal virtual bool mxaQUajJeAklKpqxemiTJFTBFCaq(GameObject P_0)
		{
			return base.gameObject == P_0;
		}

		private bool eLaloPrgLtqtgnSHTUWIpzqjkpdg(BaseEventData P_0)
		{
			bool flag = P_0 is PointerEventData;
			return eLaloPrgLtqtgnSHTUWIpzqjkpdg(flag, flag ? (P_0 as PointerEventData).pointerPress : null);
		}

		private bool eLaloPrgLtqtgnSHTUWIpzqjkpdg(bool P_0, GameObject P_1)
		{
			if (!EDufGzVNigBlMAOWvMGsHZmtQaph())
			{
				return false;
			}
			if (jlMJYbxJlzjsBwCBJxvDZPhnPAZF())
			{
				return false;
			}
			bool flag = false;
			if (P_0)
			{
				return flag | ((ALBJgfsRYJdMVJwePTiUwIfQxDNM && !zmNsczlnaJKPNGfDEeyyFdsXhpjgb && mxaQUajJeAklKpqxemiTJFTBFCaq(P_1)) || (!ALBJgfsRYJdMVJwePTiUwIfQxDNM && zmNsczlnaJKPNGfDEeyyFdsXhpjgb && mxaQUajJeAklKpqxemiTJFTBFCaq(P_1)) || (!ALBJgfsRYJdMVJwePTiUwIfQxDNM && zmNsczlnaJKPNGfDEeyyFdsXhpjgb && P_1 == null));
			}
			return flag | zmNsczlnaJKPNGfDEeyyFdsXhpjgb;
		}

		private InteractionState BsKBAlIXlthRFOWqhXlGmOZChYRP(BaseEventData P_0)
		{
			if (jlMJYbxJlzjsBwCBJxvDZPhnPAZF())
			{
				return InteractionState.Pressed;
			}
			if (eLaloPrgLtqtgnSHTUWIpzqjkpdg(P_0))
			{
				return InteractionState.Highlighted;
			}
			return InteractionState.Normal;
		}

		private bool nZWiGUpTvdikApDJloojrjuVmTkT(InteractionState P_0)
		{
			if (_interactionState == P_0)
			{
				return false;
			}
			_interactionState = P_0;
			qYjwmIsGNqaqsrYpkWNnmZsdgRjL();
			return true;
		}

		private void qYjwmIsGNqaqsrYpkWNnmZsdgRjL()
		{
			omTzsBWpbINbKcsCkDetNsakDXSg();
		}

		private void omTzsBWpbINbKcsCkDetNsakDXSg()
		{
			if (Application.isPlaying && _hideWhenIdle)
			{
				nnXqcpdOcMbRXjEWYqRtWFgsBQGCb(_interactionState == InteractionState.Pressed, false);
			}
		}

		private void nnXqcpdOcMbRXjEWYqRtWFgsBQGCb(bool P_0, bool P_1)
		{
			if (_visible == P_0 && !P_1)
			{
				return;
			}
			_visible = P_0;
			_varWatch_visible = P_0;
			if (_allowSendingEvents)
			{
				wLIJGrhiyTLONXbMWiyIGwppCVOk.ExecuteOnAll(P_0);
				if (_onVisibilityChanged != null)
				{
					_onVisibilityChanged.Invoke(P_0);
				}
			}
		}

		private void ZOHvREfiEGuSoZkpvCoiaVVliwbJ()
		{
			_varWatch_visible = _visible;
			_varWatch_interactable = IsInteractable();
			using (new SetAndRestoreVar<bool>(_allowSendingEvents, false, delegate(bool P_0)
			{
				_allowSendingEvents = P_0;
			}))
			{
				nnXqcpdOcMbRXjEWYqRtWFgsBQGCb(_visible, true);
				omTzsBWpbINbKcsCkDetNsakDXSg();
			}
			VeGtVHSUfBgpypkoecTaTsuYNRxB();
			if (_allowSendingEvents)
			{
				wLIJGrhiyTLONXbMWiyIGwppCVOk.ExecuteOnAll(_visible);
				if (_onVisibilityChanged != null)
				{
					_onVisibilityChanged.Invoke(_visible);
				}
			}
		}

		private void ZESmGDtKgyfxEWszxkXiDLpKTdRJ()
		{
			if (_varWatch_visible != _visible)
			{
				_varWatch_visible = _visible;
				if (_allowSendingEvents && _onVisibilityChanged != null)
				{
					wLIJGrhiyTLONXbMWiyIGwppCVOk.ExecuteOnAll(_visible);
					_onVisibilityChanged.Invoke(_visible);
				}
			}
		}

		private void khkdMwnTAvlDqPvCkDAZhRJFGYZR()
		{
			ZESmGDtKgyfxEWszxkXiDLpKTdRJ();
			omTzsBWpbINbKcsCkDetNsakDXSg();
			if (!Application.isPlaying)
			{
				SLncskcXOyNomyPEQOTFtPPeUMxl(true);
			}
			else
			{
				SLncskcXOyNomyPEQOTFtPPeUMxl(false);
			}
		}

		private void VeGtVHSUfBgpypkoecTaTsuYNRxB()
		{
			wLIJGrhiyTLONXbMWiyIGwppCVOk.GetHandlers(base.transform);
			ffhjypFMUoylOtTdlgaohUZSHIqR.GetHandlers(base.transform);
		}

		internal virtual void OnPointerDown(PointerEventData eventData)
		{
			if (AgilIEWjYyfxLMkJfmkwQMfVpnhH(eventData.pointerId, _allowedMouseButtons, EventTriggerType.PointerDown))
			{
				ALBJgfsRYJdMVJwePTiUwIfQxDNM = true;
				MyVxLtVZLEiRaloNqnuyxYfDkCJB(eventData);
			}
		}

		internal virtual void OnPointerUp(PointerEventData eventData)
		{
			if (AgilIEWjYyfxLMkJfmkwQMfVpnhH(eventData.pointerId, _allowedMouseButtons, EventTriggerType.PointerUp))
			{
				ALBJgfsRYJdMVJwePTiUwIfQxDNM = false;
				MyVxLtVZLEiRaloNqnuyxYfDkCJB(eventData);
			}
		}

		internal virtual void OnPointerEnter(PointerEventData eventData)
		{
			if (AgilIEWjYyfxLMkJfmkwQMfVpnhH(eventData.pointerId, _allowedMouseButtons, EventTriggerType.PointerEnter))
			{
				zmNsczlnaJKPNGfDEeyyFdsXhpjgb = true;
				MyVxLtVZLEiRaloNqnuyxYfDkCJB(eventData);
			}
		}

		internal virtual void OnPointerExit(PointerEventData eventData)
		{
			if (AgilIEWjYyfxLMkJfmkwQMfVpnhH(eventData.pointerId, _allowedMouseButtons, EventTriggerType.PointerExit))
			{
				zmNsczlnaJKPNGfDEeyyFdsXhpjgb = false;
				MyVxLtVZLEiRaloNqnuyxYfDkCJB(eventData);
			}
		}

		internal virtual void OnBeginDrag(PointerEventData eventData)
		{
			AgilIEWjYyfxLMkJfmkwQMfVpnhH(eventData.pointerId, _allowedMouseButtons, EventTriggerType.BeginDrag);
		}

		internal virtual void OnDrag(PointerEventData eventData)
		{
			AgilIEWjYyfxLMkJfmkwQMfVpnhH(eventData.pointerId, _allowedMouseButtons, EventTriggerType.Drag);
		}

		internal virtual void OnEndDrag(PointerEventData eventData)
		{
			AgilIEWjYyfxLMkJfmkwQMfVpnhH(eventData.pointerId, _allowedMouseButtons, EventTriggerType.EndDrag);
		}

		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			OnPointerDown(eventData);
		}

		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			OnPointerUp(eventData);
		}

		void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
		{
			OnPointerEnter(eventData);
		}

		void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
		{
			OnPointerExit(eventData);
		}

		void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
		{
			OnBeginDrag(eventData);
		}

		void IDragHandler.OnDrag(PointerEventData eventData)
		{
			OnDrag(eventData);
		}

		void IEndDragHandler.OnEndDrag(PointerEventData eventData)
		{
			OnEndDrag(eventData);
		}

		internal static bool uwuMHSNgPoeyIZKBbzpvCZyfdwOl(int P_0)
		{
			if (P_0 == int.MinValue)
			{
				return false;
			}
			if (pmexXrLSDjTQXxgZQgGydOXadJnJ(P_0))
			{
				int num = YTEbllrnkaeDPiJskuNKvBBwFwSGb(P_0);
				if (num < 0)
				{
					return false;
				}
				Touch touch = Input.GetTouch(num);
				if (touch.phase != TouchPhase.Ended)
				{
					return touch.phase != TouchPhase.Canceled;
				}
				return false;
			}
			if (sWBkWrgBUhjXAreNdBTEqWiNGgHjA(P_0) && Input.mousePresent)
			{
				int num2 = ZZKspXACPzCYUfirNRryGoDTWAdNA(P_0);
				if (num2 >= 0)
				{
					return Input.GetMouseButton(num2);
				}
			}
			return false;
		}

		internal static Vector3 TgEgBYQvFnhTTkcyuYMooLsCFzzE(int P_0)
		{
			if (pmexXrLSDjTQXxgZQgGydOXadJnJ(P_0))
			{
				int num = YTEbllrnkaeDPiJskuNKvBBwFwSGb(P_0);
				if (num >= 0 && Input.touchCount > num)
				{
					return Input.touches[num].position;
				}
			}
			else if (sWBkWrgBUhjXAreNdBTEqWiNGgHjA(P_0) && Input.mousePresent)
			{
				return Input.mousePosition;
			}
			return Vector3.zero;
		}

		internal static bool pmexXrLSDjTQXxgZQgGydOXadJnJ(int P_0)
		{
			return P_0 >= 0;
		}

		internal static bool sWBkWrgBUhjXAreNdBTEqWiNGgHjA(int P_0)
		{
			if (P_0 != -1 && P_0 != -3)
			{
				return P_0 == -2;
			}
			return true;
		}

		private static int YTEbllrnkaeDPiJskuNKvBBwFwSGb(int P_0)
		{
			if (!pmexXrLSDjTQXxgZQgGydOXadJnJ(P_0))
			{
				return -1;
			}
			int touchCount = Input.touchCount;
			for (int i = 0; i < touchCount; i++)
			{
				if (Input.GetTouch(i).fingerId == P_0)
				{
					return i;
				}
			}
			return -1;
		}

		internal static bool AgilIEWjYyfxLMkJfmkwQMfVpnhH(MouseButtonFlags P_0, int P_1)
		{
			if (sWBkWrgBUhjXAreNdBTEqWiNGgHjA(P_1))
			{
				if (!Cursor.visible)
				{
					return false;
				}
				if (!Input.mousePresent)
				{
					return false;
				}
			}
			if (pmexXrLSDjTQXxgZQgGydOXadJnJ(P_1))
			{
				return true;
			}
			if (manQAANcvozYeLgCmwQAxmgWUWgm(P_0, P_1))
			{
				return true;
			}
			return false;
		}

		private static bool manQAANcvozYeLgCmwQAxmgWUWgm(MouseButtonFlags P_0, int P_1)
		{
			return P_1 switch
			{
				-1 => (P_0 & MouseButtonFlags.LeftButton) != 0, 
				-2 => (P_0 & MouseButtonFlags.RightButton) != 0, 
				-3 => (P_0 & MouseButtonFlags.MiddleButton) != 0, 
				_ => false, 
			};
		}

		private static int ZZKspXACPzCYUfirNRryGoDTWAdNA(int P_0)
		{
			if (!sWBkWrgBUhjXAreNdBTEqWiNGgHjA(P_0))
			{
				return -1;
			}
			return P_0 switch
			{
				-1 => 0, 
				-2 => 1, 
				-3 => 2, 
				_ => -1, 
			};
		}

		internal static bool ZbAsyupCrOtBnZlFjdZxrpdrFStI(MouseButtonFlags P_0, out int P_1)
		{
			for (int i = 0; i < 3; i++)
			{
				if (((uint)P_0 & (uint)(1 << i)) != 0 && Input.GetMouseButton(i))
				{
					P_1 = (i + 1) * -1;
					return true;
				}
			}
			P_1 = int.MinValue;
			return false;
		}

		internal static bool AgilIEWjYyfxLMkJfmkwQMfVpnhH(int P_0, MouseButtonFlags P_1, EventTriggerType P_2)
		{
			if (sWBkWrgBUhjXAreNdBTEqWiNGgHjA(P_0) && (P_2 == EventTriggerType.PointerEnter || P_2 == EventTriggerType.PointerExit) && P_1 != MouseButtonFlags.None)
			{
				P_1 |= MouseButtonFlags.LeftButton;
			}
			return AgilIEWjYyfxLMkJfmkwQMfVpnhH(P_1, P_0);
		}

		internal static bool LTWUvUGhphDXhnFmUjsOImULiiXRA(MouseButtonFlags P_0)
		{
			int num;
			return ZbAsyupCrOtBnZlFjdZxrpdrFStI(P_0, out num);
		}

		[CompilerGenerated]
		private void jAefcAGxJBlZZkeVtQJgVULtgxiH(bool P_0)
		{
			_allowSendingEvents = P_0;
		}
	}
}
