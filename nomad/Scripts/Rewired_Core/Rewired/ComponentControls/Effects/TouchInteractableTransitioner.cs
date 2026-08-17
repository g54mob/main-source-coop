using Rewired.UI;
using Rewired.Utils;
using Rewired.Utils.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.ComponentControls.Effects
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	[DisallowMultipleComponent]
	[AddComponentMenu("Rewired/Touch Controls/Effects/Touch Interactable Transitioner")]
	public sealed class TouchInteractableTransitioner : MonoBehaviour, IVisibilityChangedHandler, TouchInteractable.IInteractionStateTransitionHandler
	{
		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("Toggles visibility. An invisible control can still be interacted with. This property only has any effect when used with an Image Component.")]
		private bool _visible = true;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("The transition type(s) to be used when transitioning to various states. Multiple transition types can be used simultaneously.")]
		[Bitmask(typeof(TouchInteractable.TransitionTypeFlags))]
		private TouchInteractable.TransitionTypeFlags _transitionType;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("Settings using for Color Tint transitions.")]
		private ColorBlock _transitionColorTint = new ColorBlock
		{
			colorMultiplier = 1f,
			disabledColor = new Color(25f / 32f, 25f / 32f, 25f / 32f, 0.5f),
			highlightedColor = Color.white,
			normalColor = Color.white,
			pressedColor = Color.white,
			fadeDuration = 0.1f
		};

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("Settings using for Sprite State transitions.")]
		private SpriteState _transitionSpriteState;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("Settings using for Animation Trigger transitions.")]
		private AnimationTriggers _transitionAnimationTriggers = new AnimationTriggers();

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("The target Graphic component for interaction state transitions. This should normally be set to an Image component on this GameObject.")]
		private Graphic _targetGraphic;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("Toggles whether the fade duration is set by incoming transition events. If enabled, the duration of fades for visibility and Color Tint transitions will be synchronized with the event sender.")]
		private bool _syncFadeDurationWithTransitionEvent = true;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("Toggles whether the color tint is set by incoming transition events. If enabled, the color tint transition of the event sender will override any color tint setting here. This setting overrides Sync Fade Duration With Transition Event.")]
		private bool _syncColorTintWithTransitionEvent;

		private TouchInteractable.InteractionState PCqpZLRUnqHEEgfEKZcJnMlnBJGM;

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
					eLZRjBUdBledYjGEwNNxlrhragGs(value, false);
					ccIgsMejUXAuGBMbRoqEyMzVAXndA();
				}
			}
		}

		public TouchInteractable.TransitionTypeFlags transitionType
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
					ccIgsMejUXAuGBMbRoqEyMzVAXndA();
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
				ccIgsMejUXAuGBMbRoqEyMzVAXndA();
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
					ccIgsMejUXAuGBMbRoqEyMzVAXndA();
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
					ccIgsMejUXAuGBMbRoqEyMzVAXndA();
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
					ccIgsMejUXAuGBMbRoqEyMzVAXndA();
				}
			}
		}

		public bool syncFadeDurationWithTransitionEvent
		{
			get
			{
				return _syncFadeDurationWithTransitionEvent;
			}
			set
			{
				if (_syncFadeDurationWithTransitionEvent != value)
				{
					_syncFadeDurationWithTransitionEvent = value;
					ccIgsMejUXAuGBMbRoqEyMzVAXndA();
				}
			}
		}

		public bool syncColorTintWithTransitionEvent
		{
			get
			{
				return _syncColorTintWithTransitionEvent;
			}
			set
			{
				if (_syncColorTintWithTransitionEvent != value)
				{
					_syncColorTintWithTransitionEvent = value;
					ccIgsMejUXAuGBMbRoqEyMzVAXndA();
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
					ccIgsMejUXAuGBMbRoqEyMzVAXndA();
				}
			}
		}

		public Animator animator => base.gameObject.GetComponent<Animator>();

		[CustomObfuscation(rename = false)]
		private TouchInteractableTransitioner()
		{
		}

		[CustomObfuscation(rename = false)]
		private void Awake()
		{
			if (Application.isPlaying)
			{
				if (_targetGraphic == null)
				{
					_targetGraphic = base.gameObject.GetComponent<Graphic>();
				}
				eLZRjBUdBledYjGEwNNxlrhragGs(_visible, true);
			}
		}

		[CustomObfuscation(rename = false)]
		private void OnEnable()
		{
			if (!Application.isPlaying)
			{
				eLZRjBUdBledYjGEwNNxlrhragGs(_visible, true);
			}
			DUjntdjROQjOWexMznfzQHwtGiXO(true);
		}

		[CustomObfuscation(rename = false)]
		private void OnDisable()
		{
			xxnUmbnCjpjoLJoJiFMmsclDNIfg();
		}

		[CustomObfuscation(rename = false)]
		private void OnValidate()
		{
			_transitionColorTint.fadeDuration = Mathf.Max(_transitionColorTint.fadeDuration, 0f);
			if (UnityTools.IsActiveAndEnabled(this))
			{
				VdjEODBsSYolxaYmHbCyUniexURL(null);
				sXzneeqaQqXSZcfOMQhWEDQqTeuw(Color.white, true);
				evjiKKesMDdNIEMbvkrlNHkdjyCFA(_transitionAnimationTriggers.normalTrigger);
				DUjntdjROQjOWexMznfzQHwtGiXO(true);
			}
			SfosjZvnWygtozpKrIiEZpjzISOh();
		}

		[CustomObfuscation(rename = false)]
		private void Reset()
		{
			_targetGraphic = base.gameObject.GetComponent<Graphic>();
		}

		[CustomObfuscation(rename = false)]
		private void OnCanvasGroupWasChanged()
		{
			SfosjZvnWygtozpKrIiEZpjzISOh();
		}

		[CustomObfuscation(rename = false)]
		private void OnAnimationPropertiesWereApplied()
		{
			SfosjZvnWygtozpKrIiEZpjzISOh();
		}

		private void ccIgsMejUXAuGBMbRoqEyMzVAXndA()
		{
			SfosjZvnWygtozpKrIiEZpjzISOh();
		}

		private void SfosjZvnWygtozpKrIiEZpjzISOh()
		{
			if (!Application.isPlaying)
			{
				DUjntdjROQjOWexMznfzQHwtGiXO(true);
			}
			else
			{
				DUjntdjROQjOWexMznfzQHwtGiXO(false);
			}
		}

		private void DUjntdjROQjOWexMznfzQHwtGiXO(bool P_0)
		{
			YPNRgBrRCroWxgUVEBFXHFXtLTNG(PCqpZLRUnqHEEgfEKZcJnMlnBJGM, P_0);
		}

		private void eLZRjBUdBledYjGEwNNxlrhragGs(bool P_0, bool P_1)
		{
			if (_visible != P_0 || P_1)
			{
				_visible = P_0;
			}
		}

		private bool yINzqFVNeCFqlqyBxxfBsBDMDHpo()
		{
			return UnityTools.IsActiveAndEnabled(this);
		}

		private void xxnUmbnCjpjoLJoJiFMmsclDNIfg()
		{
			string normalTrigger = _transitionAnimationTriggers.normalTrigger;
			if ((_transitionType & TouchInteractable.TransitionTypeFlags.ColorTint) != TouchInteractable.TransitionTypeFlags.None)
			{
				sXzneeqaQqXSZcfOMQhWEDQqTeuw(Color.white, true);
			}
			if ((_transitionType & TouchInteractable.TransitionTypeFlags.SpriteSwap) != TouchInteractable.TransitionTypeFlags.None)
			{
				VdjEODBsSYolxaYmHbCyUniexURL(null);
			}
			if ((_transitionType & TouchInteractable.TransitionTypeFlags.Animation) != TouchInteractable.TransitionTypeFlags.None)
			{
				evjiKKesMDdNIEMbvkrlNHkdjyCFA(normalTrigger);
			}
		}

		private void YPNRgBrRCroWxgUVEBFXHFXtLTNG(TouchInteractable.InteractionState P_0, bool P_1)
		{
			Color color;
			Sprite sprite;
			string text;
			switch (P_0)
			{
			case TouchInteractable.InteractionState.Normal:
				color = _transitionColorTint.normalColor;
				sprite = null;
				text = _transitionAnimationTriggers.normalTrigger;
				break;
			case TouchInteractable.InteractionState.Highlighted:
				color = _transitionColorTint.highlightedColor;
				sprite = _transitionSpriteState.highlightedSprite;
				text = _transitionAnimationTriggers.highlightedTrigger;
				break;
			case TouchInteractable.InteractionState.Pressed:
				color = _transitionColorTint.pressedColor;
				sprite = _transitionSpriteState.pressedSprite;
				text = _transitionAnimationTriggers.pressedTrigger;
				break;
			case TouchInteractable.InteractionState.Disabled:
				color = _transitionColorTint.disabledColor;
				sprite = _transitionSpriteState.disabledSprite;
				text = _transitionAnimationTriggers.disabledTrigger;
				break;
			default:
				color = Color.black;
				sprite = null;
				text = string.Empty;
				break;
			}
			bool flag = (_transitionType & TouchInteractable.TransitionTypeFlags.ColorTint) != 0;
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
					sXzneeqaQqXSZcfOMQhWEDQqTeuw(color * _transitionColorTint.colorMultiplier, P_1);
				}
				else
				{
					sXzneeqaQqXSZcfOMQhWEDQqTeuw(color, P_1);
				}
				if ((_transitionType & TouchInteractable.TransitionTypeFlags.SpriteSwap) != TouchInteractable.TransitionTypeFlags.None)
				{
					VdjEODBsSYolxaYmHbCyUniexURL(sprite);
				}
				if ((_transitionType & TouchInteractable.TransitionTypeFlags.Animation) != TouchInteractable.TransitionTypeFlags.None)
				{
					evjiKKesMDdNIEMbvkrlNHkdjyCFA(text);
				}
			}
		}

		private void sXzneeqaQqXSZcfOMQhWEDQqTeuw(Color P_0, bool P_1)
		{
			if (!(_targetGraphic == null))
			{
				_targetGraphic.CrossFadeColor(P_0, P_1 ? 0f : _transitionColorTint.fadeDuration, ignoreTimeScale: true, useAlpha: true);
			}
		}

		private void VdjEODBsSYolxaYmHbCyUniexURL(Sprite P_0)
		{
			if (!(image == null))
			{
				image.overrideSprite = P_0;
			}
		}

		private void evjiKKesMDdNIEMbvkrlNHkdjyCFA(string P_0)
		{
			if ((_transitionType & TouchInteractable.TransitionTypeFlags.Animation) != TouchInteractable.TransitionTypeFlags.None && !(animator == null) && UnityTools.IsActiveAndEnabled(animator) && !(animator.runtimeAnimatorController == null) && !string.IsNullOrEmpty(P_0))
			{
				animator.ResetTrigger(_transitionAnimationTriggers.normalTrigger);
				animator.ResetTrigger(_transitionAnimationTriggers.pressedTrigger);
				animator.ResetTrigger(_transitionAnimationTriggers.highlightedTrigger);
				animator.ResetTrigger(_transitionAnimationTriggers.disabledTrigger);
				animator.SetTrigger(P_0);
			}
		}

		public void OnInteractionStateTransition(TouchInteractable.InteractionStateTransitionArgs args)
		{
			PCqpZLRUnqHEEgfEKZcJnMlnBJGM = args.state;
			if (_syncFadeDurationWithTransitionEvent)
			{
				_transitionColorTint.fadeDuration = args.duration;
			}
			if (_syncColorTintWithTransitionEvent)
			{
				if ((_transitionType & TouchInteractable.TransitionTypeFlags.ColorTint) == 0)
				{
					_transitionType |= TouchInteractable.TransitionTypeFlags.ColorTint;
				}
				if (args.sender != null)
				{
					_transitionColorTint = args.sender.transitionColorTint;
				}
			}
			if (Application.isPlaying)
			{
				DUjntdjROQjOWexMznfzQHwtGiXO(false);
			}
			else
			{
				OnValidate();
			}
		}

		void TouchInteractable.IInteractionStateTransitionHandler.OnInteractionStateTransition(TouchInteractable.InteractionStateTransitionArgs args)
		{
			//ILSpy generated this explicit interface implementation from .override directive in OnInteractionStateTransition
			this.OnInteractionStateTransition(args);
		}

		public void OnVisibilityChanged(bool state)
		{
			visible = state;
		}

		void IVisibilityChangedHandler.OnVisibilityChanged(bool state)
		{
			//ILSpy generated this explicit interface implementation from .override directive in OnVisibilityChanged
			this.OnVisibilityChanged(state);
		}
	}
}
