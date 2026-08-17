using System;
using System.Collections.Generic;
using Rewired.Utils;
using Rewired.Utils.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace Rewired.Components
{
	[Serializable]
	[AddComponentMenu("Rewired/Player Controllers/Player Controller")]
	public class PlayerController : ComponentWrapper<Rewired.PlayerController>, IPlayerController
	{
		[Serializable]
		public class ButtonStateChangedHandler : UnityEvent<int, bool>
		{
		}

		[Serializable]
		public class AxisValueChangedHandler : UnityEvent<int, float>
		{
		}

		[Serializable]
		public class EnabledStateChangedHandler : UnityEvent<bool>
		{
		}

		[Serializable]
		[CustomObfuscation(rename = false)]
		[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = false)]
		internal class ElementWithSourceInfo
		{
			[Tooltip("The name of the element.")]
			[SerializeField]
			private string _name;

			[Tooltip("The element type.")]
			[SerializeField]
			private Rewired.PlayerController.Element.TypeWithSource _elementType;

			[Tooltip("Is this element enabled? Disabled elements return no value.")]
			[SerializeField]
			private bool _enabled = true;

			[Tooltip("The Action id of the Action which will be used as the input source for the Element.")]
			[SerializeField]
			private int _actionId = -1;

			[Tooltip("The output coordinate mode of the axis. An Absolute axis will only return value for input received from Absolute sources. A Relative axis will return value for input received from both Relative and Absolute sources. When converting from an Absolute input source to a Relative output, absoluteToRelativeSensitivity will be multiplied by the Absolute value to yield a simulated Relative value.")]
			[SerializeField]
			private AxisCoordinateMode _coordinateMode;

			[Tooltip("The absolute to relative sensitivity multiplier for axes. This is only applied when the axis coordinate mode is set to Relative and the axis receives Absolute coordinate mode input (joystick axes, keyboard keys, etc.). This is equivalent to pixels per second on a 1920x1080 screen. The final result can also be affected by Absolute To Relative Scaling Mode, and, for a mouse, Pointer Speed.")]
			[SerializeField]
			[FieldRange(0f, 3.4028235E+38f)]
			private float _absoluteToRelativeSensitivity = 1f;

			[Tooltip("Determines how a relative axis value will be scaled when controlled by an absolute axis (eg: a joystick). This can be used to scale axis value based on screen or viewport resolution. This is used for scaling mouse pointer speed. Set PlayerController.absoluteToRelativeScalingReferenceResolution to use a custom scaling reference resolution.")]
			[SerializeField]
			private Rewired.PlayerController.AbsoluteToRelativeScalingMode _absoluteToRelativeScalingMode = Rewired.PlayerController.AbsoluteToRelativeScalingMode.ScreenWidth;

			[Tooltip("The number of times per second the wheel ticks when the value source is an absolute axis value.")]
			[SerializeField]
			[FieldRange(0f, 3.4028235E+38f)]
			private float _repeatRate = 4f;

			public string name
			{
				get
				{
					return _name;
				}
				set
				{
					_name = value;
				}
			}

			public Rewired.PlayerController.Element.TypeWithSource elementType
			{
				get
				{
					return _elementType;
				}
				set
				{
					_elementType = value;
				}
			}

			public bool enabled
			{
				get
				{
					return _enabled;
				}
				set
				{
					_enabled = value;
				}
			}

			public int actionId
			{
				get
				{
					return _actionId;
				}
				set
				{
					_actionId = value;
				}
			}

			public AxisCoordinateMode coordinateMode
			{
				get
				{
					return _coordinateMode;
				}
				set
				{
					_coordinateMode = value;
				}
			}

			public float absoluteSourceSensitivity
			{
				get
				{
					return _absoluteToRelativeSensitivity;
				}
				set
				{
					_absoluteToRelativeSensitivity = value;
				}
			}

			public Rewired.PlayerController.AbsoluteToRelativeScalingMode absoluteToRelativeScalingMode
			{
				get
				{
					return _absoluteToRelativeScalingMode;
				}
				set
				{
					_absoluteToRelativeScalingMode = value;
				}
			}

			public float repeatRate
			{
				get
				{
					return _repeatRate;
				}
				set
				{
					_repeatRate = value;
				}
			}

			[Preserve]
			public ElementWithSourceInfo()
			{
			}

			public Rewired.PlayerController.Element.Definition ToDefinition()
			{
				Rewired.PlayerController.Element.Definition definition = Rewired.PlayerController.Element.CreateDefinition((Rewired.PlayerController.Element.Type)elementType);
				if (definition is Rewired.PlayerController.ElementWithSource.Definition)
				{
					((Rewired.PlayerController.ElementWithSource.Definition)definition).actionId = actionId;
				}
				if (definition is Rewired.PlayerController.Axis.Definition)
				{
					Rewired.PlayerController.Axis.Definition obj = (Rewired.PlayerController.Axis.Definition)definition;
					obj.coordinateMode = coordinateMode;
					obj.absoluteToRelativeSensitivity = absoluteSourceSensitivity;
					obj.absoluteToRelativeScalingMode = absoluteToRelativeScalingMode;
				}
				if (definition is Rewired.PlayerController.MouseWheelAxis.Definition)
				{
					((Rewired.PlayerController.MouseWheelAxis.Definition)definition).repeatRate = repeatRate;
				}
				definition.enabled = enabled;
				definition.name = name;
				return definition;
			}

			internal static ElementWithSourceInfo Create()
			{
				return new ElementWithSourceInfo
				{
					absoluteToRelativeScalingMode = Rewired.PlayerController.AbsoluteToRelativeScalingMode.None
				};
			}
		}

		[Serializable]
		[CustomObfuscation(rename = false)]
		[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = false)]
		internal sealed class ElementWithSourceInfoCreator : ElementWithSourceInfo
		{
			public ElementWithSourceInfoCreator()
			{
				base.absoluteToRelativeScalingMode = Rewired.PlayerController.AbsoluteToRelativeScalingMode.None;
			}
		}

		[Serializable]
		[CustomObfuscation(rename = false)]
		[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = false)]
		internal sealed class ElementInfo
		{
			[Tooltip("The name of the element.")]
			[SerializeField]
			private string _name;

			[Tooltip("The element type.")]
			[SerializeField]
			private Rewired.PlayerController.Element.Type _elementType;

			[Tooltip("Is this element enabled? Disabled elements return no value.")]
			[SerializeField]
			private bool _enabled = true;

			[SerializeField]
			private ElementWithSourceInfo[] _elements = new ElementWithSourceInfo[0];

			public string name
			{
				get
				{
					return _name;
				}
				set
				{
					_name = value;
				}
			}

			public Rewired.PlayerController.Element.Type elementType
			{
				get
				{
					return _elementType;
				}
				set
				{
					_elementType = value;
				}
			}

			public bool enabled
			{
				get
				{
					return _enabled;
				}
				set
				{
					_enabled = value;
				}
			}

			public ElementWithSourceInfo[] elements
			{
				get
				{
					return _elements;
				}
				set
				{
					_elements = value;
				}
			}

			public Rewired.PlayerController.Element.Definition ToDefinition()
			{
				Rewired.PlayerController.Element.Definition definition = Rewired.PlayerController.Element.CreateDefinition(elementType);
				if (definition is Rewired.PlayerController.ElementWithSource.Definition)
				{
					if (_elements == null || _elements.Length == 0)
					{
						Logger.LogError("No element source was found for element with source definition.");
						return null;
					}
					Rewired.PlayerController.ElementWithSource.Definition obj = (Rewired.PlayerController.ElementWithSource.Definition)definition;
					obj.name = _elements[0].name;
					obj.enabled = _elements[0].enabled;
					obj.actionId = _elements[0].actionId;
				}
				if (definition is Rewired.PlayerController.Axis.Definition)
				{
					Rewired.PlayerController.Axis.Definition obj2 = (Rewired.PlayerController.Axis.Definition)definition;
					obj2.coordinateMode = _elements[0].coordinateMode;
					obj2.absoluteToRelativeSensitivity = _elements[0].absoluteSourceSensitivity;
					obj2.absoluteToRelativeScalingMode = _elements[0].absoluteToRelativeScalingMode;
				}
				if (definition is Rewired.PlayerController.MouseWheelAxis.Definition)
				{
					((Rewired.PlayerController.MouseWheelAxis.Definition)definition).repeatRate = _elements[0].repeatRate;
				}
				if (definition is Rewired.PlayerController.CompoundElement.Definition)
				{
					definition.name = name;
					definition.enabled = enabled;
					if (_elements == null || _elements.Length == 0)
					{
						Logger.LogError("No element source was found for element with source definition.");
						return null;
					}
					if (definition is Rewired.PlayerController.MouseWheel.Definition)
					{
						Rewired.PlayerController.MouseWheel.Definition definition2 = definition as Rewired.PlayerController.MouseWheel.Definition;
						try
						{
							if (_elements.Length >= 1)
							{
								definition2.xAxis = (Rewired.PlayerController.MouseWheelAxis.Definition)_elements[0].ToDefinition();
							}
							if (_elements.Length >= 2)
							{
								definition2.yAxis = (Rewired.PlayerController.MouseWheelAxis.Definition)_elements[1].ToDefinition();
							}
						}
						catch
						{
							Logger.LogError("Incorrect element source type found. Expecting MouseWheelAxis.");
							return null;
						}
					}
					else
					{
						if (!(definition is Rewired.PlayerController.Axis2D.Definition))
						{
							throw new NotImplementedException();
						}
						Rewired.PlayerController.Axis2D.Definition definition3 = definition as Rewired.PlayerController.Axis2D.Definition;
						try
						{
							if (_elements.Length >= 1)
							{
								definition3.xAxis = (Rewired.PlayerController.Axis.Definition)_elements[0].ToDefinition();
							}
							if (_elements.Length >= 2)
							{
								definition3.yAxis = (Rewired.PlayerController.Axis.Definition)_elements[1].ToDefinition();
							}
						}
						catch
						{
							Logger.LogError("Incorrect element source type found. Expecting Axis.");
							return null;
						}
					}
				}
				return definition;
			}
		}

		[Tooltip("(Optional) Link the Rewired Input Manager here for easier access to Action ids, Player ids, etc.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private InputManager_Base _rewiredInputManager;

		[Tooltip("The Player id of the Player used for the source of input.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int _playerId = -1;

		[Tooltip("The elements that will be created in the controller.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<ElementInfo> _elements = new List<ElementInfo>();

		[Tooltip("Triggered the first frame the button is pressed or released.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private ButtonStateChangedHandler _onButtonStateChanged = new ButtonStateChangedHandler();

		[Tooltip("Triggered when the axis value changes.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private AxisValueChangedHandler _onAxisValueChanged = new AxisValueChangedHandler();

		[Tooltip("Triggered when the controller is enabled or disabled.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private EnabledStateChangedHandler _onEnabledStateChanged = new EnabledStateChangedHandler();

		int IPlayerController.playerId
		{
			get
			{
				if (!base.initialized)
				{
					return _playerId;
				}
				return base.source.Rewired_002EIPlayerController_002EplayerId;
			}
			set
			{
				if (ReInput.isReady && ReInput.players.GetPlayer(value) == null)
				{
					Logger.LogWarning("Player id " + value + " does not exist.");
					return;
				}
				_playerId = value;
				if (base.initialized)
				{
					base.source.Rewired_002EIPlayerController_002EplayerId = value;
				}
			}
		}

		IList<Rewired.PlayerController.Button> IPlayerController.buttons
		{
			get
			{
				if (!base.initialized)
				{
					return EmptyObjects<Rewired.PlayerController.Button>.EmptyReadOnlyIListT;
				}
				return base.source.Rewired_002EIPlayerController_002Ebuttons;
			}
		}

		IList<Rewired.PlayerController.Axis> IPlayerController.axes
		{
			get
			{
				if (!base.initialized)
				{
					return EmptyObjects<Rewired.PlayerController.Axis>.EmptyReadOnlyIListT;
				}
				return base.source.Rewired_002EIPlayerController_002Eaxes;
			}
		}

		IList<Rewired.PlayerController.Element> IPlayerController.elements
		{
			get
			{
				if (!base.initialized)
				{
					return EmptyObjects<Rewired.PlayerController.Element>.EmptyReadOnlyIListT;
				}
				return base.source.Rewired_002EIPlayerController_002Eelements;
			}
		}

		int IPlayerController.buttonCount
		{
			get
			{
				if (!base.initialized)
				{
					return 0;
				}
				return base.source.Rewired_002EIPlayerController_002EbuttonCount;
			}
		}

		int IPlayerController.axisCount
		{
			get
			{
				if (!base.initialized)
				{
					return 0;
				}
				return base.source.Rewired_002EIPlayerController_002EaxisCount;
			}
		}

		int IPlayerController.elementCount
		{
			get
			{
				if (!base.initialized)
				{
					return 0;
				}
				return base.source.Rewired_002EIPlayerController_002EelementCount;
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

		event Action<int, bool> IPlayerController.ButtonStateChangedEvent
		{
			add
			{
				if (base.initialized)
				{
					base.source.Rewired_002EIPlayerController_002EButtonStateChangedEvent += value;
				}
			}
			remove
			{
				if (base.initialized)
				{
					base.source.Rewired_002EIPlayerController_002EButtonStateChangedEvent -= value;
				}
			}
		}

		event Action<int, float> IPlayerController.AxisValueChangedEvent
		{
			add
			{
				if (base.initialized)
				{
					base.source.Rewired_002EIPlayerController_002EAxisValueChangedEvent += value;
				}
			}
			remove
			{
				if (base.initialized)
				{
					base.source.Rewired_002EIPlayerController_002EAxisValueChangedEvent -= value;
				}
			}
		}

		event Action<bool> IPlayerController.EnabledStateChangedEvent
		{
			add
			{
				if (base.initialized)
				{
					base.source.Rewired_002EIPlayerController_002EEnabledStateChangedEvent += value;
				}
			}
			remove
			{
				if (base.initialized)
				{
					base.source.Rewired_002EIPlayerController_002EEnabledStateChangedEvent -= value;
				}
			}
		}

		public bool GetButton(int index)
		{
			if (!base.initialized)
			{
				return false;
			}
			return base.source.GetButton(index);
		}

		bool IPlayerController.GetButton(int index)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetButton
			return this.GetButton(index);
		}

		public bool GetButtonDown(int index)
		{
			if (!base.initialized)
			{
				return false;
			}
			return base.source.GetButtonDown(index);
		}

		bool IPlayerController.GetButtonDown(int index)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetButtonDown
			return this.GetButtonDown(index);
		}

		public bool GetButtonUp(int index)
		{
			if (!base.initialized)
			{
				return false;
			}
			return base.source.GetButtonUp(index);
		}

		bool IPlayerController.GetButtonUp(int index)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetButtonUp
			return this.GetButtonUp(index);
		}

		public float GetAxis(int index)
		{
			if (!base.initialized)
			{
				return 0f;
			}
			return base.source.GetAxis(index);
		}

		float IPlayerController.GetAxis(int index)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetAxis
			return this.GetAxis(index);
		}

		public float GetAxisRaw(int index)
		{
			if (!base.initialized)
			{
				return 0f;
			}
			return base.source.GetAxisRaw(index);
		}

		float IPlayerController.GetAxisRaw(int index)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetAxisRaw
			return this.GetAxisRaw(index);
		}

		public Rewired.PlayerController.Element GetElement(int index)
		{
			if (!base.initialized)
			{
				return null;
			}
			return base.source.GetElement(index);
		}

		Rewired.PlayerController.Element IPlayerController.GetElement(int index)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetElement
			return this.GetElement(index);
		}

		public T GetElement<T>(int index) where T : Rewired.PlayerController.Element
		{
			if (!base.initialized)
			{
				return null;
			}
			return base.source.GetElement<T>(index);
		}

		T IPlayerController.GetElement<T>(int index)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetElement
			return this.GetElement<T>(index);
		}

		protected override void OnAwake()
		{
			yILrcytQGkpXJCyLEhNiMFWXwSvU();
			base.OnAwake();
		}

		protected override void OnAwakeFinished()
		{
			base.OnAwakeFinished();
			if (base.initialized)
			{
				kGPeODYrKjdixDRajscqDPnQGyUFA(true);
			}
		}

		protected override void OnEnabled()
		{
			base.OnEnabled();
			if (base.initialized && ReInput.isReady)
			{
				base.source.Rewired_002EIPlayerController_002Eenabled = true;
			}
		}

		protected override void OnDisabled()
		{
			base.OnDisabled();
			if (base.initialized && ReInput.isReady)
			{
				base.source.Rewired_002EIPlayerController_002Eenabled = false;
			}
		}

		protected override void OnValidated()
		{
			base.OnValidated();
			Rewired_002EIPlayerController_002EplayerId = _playerId;
			_playerId = Rewired_002EIPlayerController_002EplayerId;
		}

		protected override void OnReset()
		{
			base.OnReset();
			_rewiredInputManager = null;
			_playerId = -1;
			_elements = new List<ElementInfo>();
			_onButtonStateChanged = new ButtonStateChangedHandler();
			_onAxisValueChanged = new AxisValueChangedHandler();
			_onEnabledStateChanged = new EnabledStateChangedHandler();
			yILrcytQGkpXJCyLEhNiMFWXwSvU();
		}

		protected override void Subscribe()
		{
			base.Subscribe();
			if (base.source != null)
			{
				base.source.Rewired_002EIPlayerController_002EButtonStateChangedEvent += okoDjwDTSGmJFEVOMHkTZEpbIQsUA;
				base.source.Rewired_002EIPlayerController_002EAxisValueChangedEvent += TKLbNdPLfyAFGHtMqbzgoimOcwNk;
				base.source.Rewired_002EIPlayerController_002EEnabledStateChangedEvent += kGPeODYrKjdixDRajscqDPnQGyUFA;
			}
		}

		protected override void Unsubscribe()
		{
			base.Unsubscribe();
			if (base.source != null)
			{
				base.source.Rewired_002EIPlayerController_002EButtonStateChangedEvent -= okoDjwDTSGmJFEVOMHkTZEpbIQsUA;
				base.source.Rewired_002EIPlayerController_002EAxisValueChangedEvent -= TKLbNdPLfyAFGHtMqbzgoimOcwNk;
				base.source.Rewired_002EIPlayerController_002EEnabledStateChangedEvent -= kGPeODYrKjdixDRajscqDPnQGyUFA;
			}
		}

		protected override object GetCreateSourceArgs()
		{
			return _elements;
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
			return Rewired.PlayerController.Factory.Create(new Rewired.PlayerController.Definition
			{
				playerId = _playerId,
				elements = list2
			});
		}

		internal virtual List<ElementInfo> ovRMskjHrUwckevkCwILljUzUGHr()
		{
			List<ElementInfo> list = new List<ElementInfo>();
			list.Add(new ElementInfo
			{
				name = "Stick",
				elementType = Rewired.PlayerController.Element.Type.Axis2D,
				elements = new ElementWithSourceInfo[2]
				{
					new ElementWithSourceInfoCreator
					{
						name = "Stick Horizontal",
						elementType = Rewired.PlayerController.Element.TypeWithSource.Axis,
						coordinateMode = AxisCoordinateMode.Absolute
					},
					new ElementWithSourceInfoCreator
					{
						name = "Stick Vertical",
						elementType = Rewired.PlayerController.Element.TypeWithSource.Axis,
						coordinateMode = AxisCoordinateMode.Absolute
					}
				}
			});
			list.Add(new ElementInfo
			{
				elements = new ElementWithSourceInfo[1]
				{
					new ElementWithSourceInfoCreator
					{
						name = "Button 1",
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
						name = "Button 2",
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
						name = "Button 3",
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
						name = "Button 4",
						elementType = Rewired.PlayerController.Element.TypeWithSource.Button
					}
				}
			});
			return list;
		}

		private void okoDjwDTSGmJFEVOMHkTZEpbIQsUA(int P_0, bool P_1)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			try
			{
				if (_onButtonStateChanged != null)
				{
					_onButtonStateChanged.Invoke(P_0, P_1);
				}
			}
			catch (Exception ex)
			{
				Logger.LogError("An exception occurred in a listener of ButtonStateChangedEvent. This means an exception was thrown by your code.\n" + ex);
			}
		}

		private void TKLbNdPLfyAFGHtMqbzgoimOcwNk(int P_0, float P_1)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			try
			{
				if (_onAxisValueChanged != null)
				{
					_onAxisValueChanged.Invoke(P_0, P_1);
				}
			}
			catch (Exception ex)
			{
				Logger.LogError("An exception occurred in a listener of AxisValueChangedEvent. This means an exception was thrown by your code.\n" + ex);
			}
		}

		private void kGPeODYrKjdixDRajscqDPnQGyUFA(bool P_0)
		{
			try
			{
				if (_onEnabledStateChanged != null)
				{
					_onEnabledStateChanged.Invoke(P_0);
				}
			}
			catch (Exception ex)
			{
				Logger.LogError("An exception occurred in a listener of EnabledStateChangedEvent. This means an exception was thrown by your code.\n" + ex);
			}
		}

		private void yILrcytQGkpXJCyLEhNiMFWXwSvU()
		{
			if (_elements == null || _elements.Count <= 0)
			{
				_elements = ovRMskjHrUwckevkCwILljUzUGHr();
			}
		}
	}
}
