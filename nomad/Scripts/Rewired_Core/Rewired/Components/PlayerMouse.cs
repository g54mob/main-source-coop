using System;
using System.Collections.Generic;
using Rewired.UI;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Rewired.Components
{
	[Serializable]
	[AddComponentMenu("Rewired/Player Controllers/Player Mouse")]
	public sealed class PlayerMouse : PlayerController, IPlayerMouse, IPlayerController, IMouseInputSource
	{
		[Serializable]
		public class ScreenPositionChangedHandler : UnityEvent<Vector2>
		{
		}

		[Tooltip("If enabled, the screen position will default to the center of the allowed movement area. Otherwise, it will default to the lower-left corner of the allowed movement area.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _defaultToCenter = true;

		[Tooltip("The pointer speed. This does not affect the speed of input from the mouse x/y axes if useHardwarePointerPosition is enabled. It only affects the speed from input sources other than mouse x/y or if mouse x/y are mapped to Actions assigned to Axes. ")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private float _pointerSpeed = 1f;

		[Tooltip("If enabled, the hardware pointer position will be used for mouse input. Otherwise, the position of the pointer will be calculated only from the Axis Action values. The Player that owns this Player Mouse must have the physical mouse assigned to it in order for the hardware position to be used, ex: player.controllers.hasMouse == true.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _useHardwarePointerPosition = true;

		[Tooltip("If enabled, movement will be clamped to the Movement Area.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _clampToMovementArea = true;

		[Tooltip("The allowed movement area for the mouse pointer. Set Movement Area Unit to determine the data format of this value. This rect is a screen-space rect with 0, 0 at the lower-left corner.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private Rect _movementArea = new Rect(0f, 0f, 1f, 1f);

		[Tooltip("The unit format of the movement area. This is used to determine the data format of Movement Area.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private Rewired.PlayerMouse.MovementAreaUnit _movementAreaUnit;

		[Tooltip("Triggered when the screen position changes. Link this to your pointer to drive its position.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private ScreenPositionChangedHandler _onScreenPositionChanged = new ScreenPositionChangedHandler();

		private Rewired.PlayerMouse UfdjMmpkvDVaJwACmaGYNmWwyFKG => base.source as Rewired.PlayerMouse;

		bool IPlayerMouse.defaultToCenter
		{
			get
			{
				if (!base.initialized)
				{
					return _defaultToCenter;
				}
				return UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002EdefaultToCenter;
			}
			set
			{
				if (UfdjMmpkvDVaJwACmaGYNmWwyFKG == null)
				{
					_defaultToCenter = value;
					return;
				}
				UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002EdefaultToCenter = value;
				_defaultToCenter = UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002EdefaultToCenter;
			}
		}

		public bool clampToMovementArea
		{
			get
			{
				if (!base.initialized)
				{
					return _clampToMovementArea;
				}
				return UfdjMmpkvDVaJwACmaGYNmWwyFKG.clampToMovementArea;
			}
			set
			{
				if (UfdjMmpkvDVaJwACmaGYNmWwyFKG == null)
				{
					_clampToMovementArea = value;
					return;
				}
				UfdjMmpkvDVaJwACmaGYNmWwyFKG.clampToMovementArea = value;
				_clampToMovementArea = UfdjMmpkvDVaJwACmaGYNmWwyFKG.clampToMovementArea;
			}
		}

		ScreenRect IPlayerMouse.movementArea
		{
			get
			{
				if (!base.initialized)
				{
					return new ScreenRect(_movementArea.xMin, _movementArea.yMin, _movementArea.width, _movementArea.height);
				}
				return UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002EmovementArea;
			}
			set
			{
				if (UfdjMmpkvDVaJwACmaGYNmWwyFKG == null)
				{
					_movementArea = new Rect(value.xMin, value.yMin, value.width, value.height);
					return;
				}
				UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002EmovementArea = value;
				_movementArea = new Rect(UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002EmovementArea.xMin, UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002EmovementArea.yMin, UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002EmovementArea.width, UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002EmovementArea.height);
			}
		}

		Rewired.PlayerMouse.MovementAreaUnit IPlayerMouse.movementAreaUnit
		{
			get
			{
				if (!base.initialized)
				{
					return _movementAreaUnit;
				}
				return UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002EmovementAreaUnit;
			}
			set
			{
				if (UfdjMmpkvDVaJwACmaGYNmWwyFKG == null)
				{
					_movementAreaUnit = value;
					return;
				}
				UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002EmovementAreaUnit = value;
				_movementAreaUnit = UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002EmovementAreaUnit;
			}
		}

		Vector2 IPlayerMouse.screenPosition
		{
			get
			{
				if (!base.initialized)
				{
					return Vector2.zero;
				}
				return UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002EscreenPosition;
			}
			set
			{
				if (UfdjMmpkvDVaJwACmaGYNmWwyFKG != null)
				{
					UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002EscreenPosition = value;
				}
			}
		}

		Vector2 IPlayerMouse.screenPositionPrev
		{
			get
			{
				if (!base.initialized)
				{
					return Vector2.zero;
				}
				return UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002EscreenPositionPrev;
			}
		}

		Vector2 IPlayerMouse.screenPositionDelta
		{
			get
			{
				if (!base.initialized)
				{
					return Vector2.zero;
				}
				return UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002EscreenPositionDelta;
			}
		}

		Rewired.PlayerController.MouseAxis IPlayerMouse.xAxis
		{
			get
			{
				if (!base.initialized)
				{
					return null;
				}
				return UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002ExAxis;
			}
		}

		Rewired.PlayerController.MouseAxis IPlayerMouse.yAxis
		{
			get
			{
				if (!base.initialized)
				{
					return null;
				}
				return UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002EyAxis;
			}
		}

		Rewired.PlayerController.MouseWheel IPlayerMouse.wheel
		{
			get
			{
				if (!base.initialized)
				{
					return null;
				}
				return UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002Ewheel;
			}
		}

		Rewired.PlayerController.Button IPlayerMouse.leftButton
		{
			get
			{
				if (!base.initialized)
				{
					return null;
				}
				return UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002EleftButton;
			}
		}

		Rewired.PlayerController.Button IPlayerMouse.rightButton
		{
			get
			{
				if (!base.initialized)
				{
					return null;
				}
				return UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002ErightButton;
			}
		}

		Rewired.PlayerController.Button IPlayerMouse.middleButton
		{
			get
			{
				if (!base.initialized)
				{
					return null;
				}
				return UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002EmiddleButton;
			}
		}

		float IPlayerMouse.pointerSpeed
		{
			get
			{
				if (!base.initialized)
				{
					return _pointerSpeed;
				}
				return UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002EpointerSpeed;
			}
			set
			{
				if (value < 0f)
				{
					value = 0f;
				}
				_pointerSpeed = value;
				if (base.initialized)
				{
					UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002EpointerSpeed = value;
					_pointerSpeed = UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002EpointerSpeed;
				}
			}
		}

		bool IPlayerMouse.useHardwarePointerPosition
		{
			get
			{
				if (!base.initialized)
				{
					return _useHardwarePointerPosition;
				}
				return UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002EuseHardwarePointerPosition;
			}
			set
			{
				_useHardwarePointerPosition = value;
				if (base.initialized)
				{
					UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002EuseHardwarePointerPosition = value;
				}
			}
		}

		bool IMouseInputSource.enabled
		{
			get
			{
				if (!base.initialized)
				{
					return false;
				}
				return ((IMouseInputSource)UfdjMmpkvDVaJwACmaGYNmWwyFKG).enabled;
			}
		}

		Vector2 IMouseInputSource.screenPosition
		{
			get
			{
				if (!base.initialized)
				{
					return Vector2.zero;
				}
				return ((IMouseInputSource)UfdjMmpkvDVaJwACmaGYNmWwyFKG).screenPosition;
			}
		}

		Vector2 IMouseInputSource.screenPositionDelta
		{
			get
			{
				if (!base.initialized)
				{
					return Vector2.zero;
				}
				return ((IMouseInputSource)UfdjMmpkvDVaJwACmaGYNmWwyFKG).screenPositionDelta;
			}
		}

		Vector2 IMouseInputSource.wheelDelta
		{
			get
			{
				if (!base.initialized)
				{
					return Vector2.zero;
				}
				return ((IMouseInputSource)UfdjMmpkvDVaJwACmaGYNmWwyFKG).wheelDelta;
			}
		}

		bool IMouseInputSource.locked
		{
			get
			{
				if (!base.initialized)
				{
					return false;
				}
				return ((IMouseInputSource)UfdjMmpkvDVaJwACmaGYNmWwyFKG).locked;
			}
		}

		bool IPlayerController.enabled
		{
			get
			{
				return base.enabled;
			}
			set
			{
				base.enabled = value;
			}
		}

		event Action<Vector2> IPlayerMouse.ScreenPositionChangedEvent
		{
			add
			{
				if (base.initialized)
				{
					UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002EScreenPositionChangedEvent += value;
				}
			}
			remove
			{
				if (base.initialized)
				{
					UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002EScreenPositionChangedEvent -= value;
				}
			}
		}

		protected override void OnValidated()
		{
			base.OnValidated();
			Rewired_002EIPlayerMouse_002EdefaultToCenter = _defaultToCenter;
			clampToMovementArea = _clampToMovementArea;
			((IPlayerMouse)this).movementArea = new ScreenRect(_movementArea.xMin, _movementArea.yMin, _movementArea.width, _movementArea.height);
			((IPlayerMouse)this).movementAreaUnit = _movementAreaUnit;
			Rewired_002EIPlayerMouse_002EpointerSpeed = _pointerSpeed;
			Rewired_002EIPlayerMouse_002EuseHardwarePointerPosition = _useHardwarePointerPosition;
		}

		protected override void OnReset()
		{
			base.OnReset();
			_clampToMovementArea = true;
			_defaultToCenter = true;
			_pointerSpeed = 1f;
			_useHardwarePointerPosition = true;
			_movementArea = new Rect(0f, 0f, 1f, 1f);
			_movementAreaUnit = Rewired.PlayerMouse.MovementAreaUnit.Screen;
			_onScreenPositionChanged = new ScreenPositionChangedHandler();
		}

		protected override Rewired.PlayerController CreateSource(object args)
		{
			IList<ElementInfo> list = args as IList<ElementInfo>;
			if (list == null || list.Count == 0)
			{
				Logger.LogWarning("Invalid element information. Did you configure elements in the inspector? Using defaults.");
				list = ovRMskjHrUwckevkCwILljUzUGHr();
			}
			List<Rewired.PlayerController.Element.Definition> list2 = new List<Rewired.PlayerController.Element.Definition>(list.Count);
			foreach (ElementInfo item in list)
			{
				list2.Add(item.ToDefinition());
			}
			return Rewired.PlayerMouse.Factory.Create(new Rewired.PlayerMouse.Definition
			{
				playerId = base.Rewired_002EIPlayerController_002EplayerId,
				elements = list2,
				defaultToCenter = _defaultToCenter,
				clampToMovementArea = _clampToMovementArea,
				movementArea = new ScreenRect(_movementArea.xMin, _movementArea.yMin, _movementArea.width, _movementArea.height),
				movementAreaUnit = _movementAreaUnit,
				pointerSpeed = _pointerSpeed,
				useHardwarePointerPosition = _useHardwarePointerPosition
			});
		}

		protected override void Deinitialize()
		{
			base.Deinitialize();
		}

		protected override void Subscribe()
		{
			base.Subscribe();
			if (UfdjMmpkvDVaJwACmaGYNmWwyFKG != null)
			{
				UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002EScreenPositionChangedEvent += ETADEPuzJJOEjuWuOtUEiQfTtVyw;
			}
		}

		protected override void Unsubscribe()
		{
			base.Unsubscribe();
			if (UfdjMmpkvDVaJwACmaGYNmWwyFKG != null)
			{
				UfdjMmpkvDVaJwACmaGYNmWwyFKG.Rewired_002EIPlayerMouse_002EScreenPositionChangedEvent -= ETADEPuzJJOEjuWuOtUEiQfTtVyw;
			}
		}

		internal List<ElementInfo> TwERIPQTsXFQURIawnjiwMlTULdy()
		{
			List<ElementInfo> list = new List<ElementInfo>();
			list.Add(new ElementInfo
			{
				name = "Movement",
				elementType = Rewired.PlayerController.Element.Type.MouseAxis2D,
				elements = new ElementWithSourceInfo[2]
				{
					new ElementWithSourceInfoCreator
					{
						name = "Horizontal",
						elementType = Rewired.PlayerController.Element.TypeWithSource.MouseAxis,
						coordinateMode = AxisCoordinateMode.Relative,
						absoluteSourceSensitivity = 600f,
						absoluteToRelativeScalingMode = Rewired.PlayerController.AbsoluteToRelativeScalingMode.ScreenWidth
					},
					new ElementWithSourceInfoCreator
					{
						name = "Vertical",
						elementType = Rewired.PlayerController.Element.TypeWithSource.MouseAxis,
						coordinateMode = AxisCoordinateMode.Relative,
						absoluteSourceSensitivity = 600f,
						absoluteToRelativeScalingMode = Rewired.PlayerController.AbsoluteToRelativeScalingMode.ScreenWidth
					}
				}
			});
			list.Add(new ElementInfo
			{
				name = "Wheel",
				elementType = Rewired.PlayerController.Element.Type.MouseWheel,
				elements = new ElementWithSourceInfo[2]
				{
					new ElementWithSourceInfoCreator
					{
						name = "Wheel Horizontal",
						elementType = Rewired.PlayerController.Element.TypeWithSource.MouseWheelAxis,
						coordinateMode = AxisCoordinateMode.Relative
					},
					new ElementWithSourceInfoCreator
					{
						name = "Wheel Vertical",
						elementType = Rewired.PlayerController.Element.TypeWithSource.MouseWheelAxis,
						coordinateMode = AxisCoordinateMode.Relative
					}
				}
			});
			list.Add(new ElementInfo
			{
				elements = new ElementWithSourceInfo[1]
				{
					new ElementWithSourceInfoCreator
					{
						name = "Left Button",
						elementType = Rewired.PlayerController.Element.TypeWithSource.Button
					}
				}
			});
			list.Add(new ElementInfo
			{
				elements = new ElementWithSourceInfo[1]
				{
					new ElementWithSourceInfoCreator
					{
						name = "Right Button",
						elementType = Rewired.PlayerController.Element.TypeWithSource.Button
					}
				}
			});
			list.Add(new ElementInfo
			{
				elements = new ElementWithSourceInfo[1]
				{
					new ElementWithSourceInfoCreator
					{
						name = "Middle Button",
						elementType = Rewired.PlayerController.Element.TypeWithSource.Button
					}
				}
			});
			return list;
		}

		private void ETADEPuzJJOEjuWuOtUEiQfTtVyw(Vector2 P_0)
		{
			if (!UnityTools.IsActiveAndEnabled(this))
			{
				return;
			}
			try
			{
				if (_onScreenPositionChanged != null)
				{
					_onScreenPositionChanged.Invoke(P_0);
				}
			}
			catch (Exception ex)
			{
				Logger.LogError("An exception occurred in a listener of ScreenPositionChangedEvent. This means an exception was thrown by your code.\n" + ex);
			}
		}

		bool IMouseInputSource.GetButtonDown(int button)
		{
			if (!base.initialized)
			{
				return false;
			}
			return ((IMouseInputSource)UfdjMmpkvDVaJwACmaGYNmWwyFKG).GetButtonDown(button);
		}

		bool IMouseInputSource.GetButtonUp(int button)
		{
			if (!base.initialized)
			{
				return false;
			}
			return ((IMouseInputSource)UfdjMmpkvDVaJwACmaGYNmWwyFKG).GetButtonUp(button);
		}

		bool IMouseInputSource.GetButton(int button)
		{
			if (!base.initialized)
			{
				return false;
			}
			return ((IMouseInputSource)UfdjMmpkvDVaJwACmaGYNmWwyFKG).GetButton(button);
		}
	}
}
