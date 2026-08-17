using System;
using UnityEngine;

namespace Rewired
{
	public sealed class ElementAssignmentInfo
	{
		private readonly ControllerMap ZZiynRcjxiFAvbkREgtChYqEBfarb;

		private readonly ControllerElementType wJbbzzGREDXYwJEwUDWJvLMDwUnS;

		private readonly int BgIuxtdSRAtvyYzaHiHsZzKAXgSc;

		private readonly int zMCyDGlLPSGKZuZzFkRBMKcsJBJR;

		private readonly AxisRange sBiaFgAomWytIuziMhHcQvkpOOpi;

		private readonly KeyCode gXFyaLdbbyGMwSMZocbOpqGjhZrY;

		private readonly ModifierKeyFlags YBfXNNARrGGDuyUCPfmlUfDueasKA;

		private readonly int mGLvtujujDvRZOuuDTVdvNLWsBcP;

		private readonly Pole ZyRfzdiLWYaVgSQyQSeCDTCZjWbH;

		private readonly bool vvYDrfpKyOndiPczREmOIebAGWGX;

		public Player player
		{
			get
			{
				if (!ReInput.isReady)
				{
					return null;
				}
				if (ZZiynRcjxiFAvbkREgtChYqEBfarb == null)
				{
					return null;
				}
				return ZZiynRcjxiFAvbkREgtChYqEBfarb.player;
			}
		}

		public InputAction action
		{
			get
			{
				if (!ReInput.isReady)
				{
					return null;
				}
				return ReInput.mapping.GetAction(mGLvtujujDvRZOuuDTVdvNLWsBcP);
			}
		}

		public Controller controller
		{
			get
			{
				if (!ReInput.isReady)
				{
					return null;
				}
				if (ZZiynRcjxiFAvbkREgtChYqEBfarb == null)
				{
					return null;
				}
				return ReInput.controllers.GetController(ZZiynRcjxiFAvbkREgtChYqEBfarb.controllerType, ZZiynRcjxiFAvbkREgtChYqEBfarb.controllerId);
			}
		}

		public ControllerType controllerType
		{
			get
			{
				if (!ReInput.isReady || ZZiynRcjxiFAvbkREgtChYqEBfarb == null)
				{
					return ControllerType.Keyboard;
				}
				return ZZiynRcjxiFAvbkREgtChYqEBfarb.controllerType;
			}
		}

		public int controllerId
		{
			get
			{
				if (!ReInput.isReady || ZZiynRcjxiFAvbkREgtChYqEBfarb == null)
				{
					return -1;
				}
				return ZZiynRcjxiFAvbkREgtChYqEBfarb.controllerId;
			}
		}

		public ControllerMap controllerMap => ZZiynRcjxiFAvbkREgtChYqEBfarb;

		public ControllerElementIdentifier elementIdentifier
		{
			get
			{
				if (controller == null)
				{
					return null;
				}
				return controller.GetElementIdentifierById(zMCyDGlLPSGKZuZzFkRBMKcsJBJR);
			}
		}

		public ActionElementMap elementMap
		{
			get
			{
				if (ZZiynRcjxiFAvbkREgtChYqEBfarb == null)
				{
					return null;
				}
				return ZZiynRcjxiFAvbkREgtChYqEBfarb.GetElementMap(BgIuxtdSRAtvyYzaHiHsZzKAXgSc);
			}
		}

		public ControllerElementType elementType => wJbbzzGREDXYwJEwUDWJvLMDwUnS;

		public Pole axisContribution => ZyRfzdiLWYaVgSQyQSeCDTCZjWbH;

		public AxisRange axisRange => sBiaFgAomWytIuziMhHcQvkpOOpi;

		public bool invert => vvYDrfpKyOndiPczREmOIebAGWGX;

		public KeyCode keyCode => gXFyaLdbbyGMwSMZocbOpqGjhZrY;

		public ModifierKeyFlags modifierKeyFlags => YBfXNNARrGGDuyUCPfmlUfDueasKA;

		public string elementDisplayName
		{
			get
			{
				if (ZZiynRcjxiFAvbkREgtChYqEBfarb == null)
				{
					return string.Empty;
				}
				if (controllerType == ControllerType.Keyboard)
				{
					return Keyboard.GetKeyName(keyCode, modifierKeyFlags);
				}
				Controller controller = this.controller;
				if (controller == null)
				{
					return string.Empty;
				}
				ControllerElementIdentifier elementIdentifierById = controller.GetElementIdentifierById(zMCyDGlLPSGKZuZzFkRBMKcsJBJR);
				if (elementIdentifierById == null)
				{
					return string.Empty;
				}
				if (wJbbzzGREDXYwJEwUDWJvLMDwUnS == ControllerElementType.Axis)
				{
					if (sBiaFgAomWytIuziMhHcQvkpOOpi == AxisRange.Full)
					{
						return elementIdentifierById.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Ename;
					}
					if (sBiaFgAomWytIuziMhHcQvkpOOpi == AxisRange.Positive)
					{
						return elementIdentifierById.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002EpositiveName;
					}
					if (sBiaFgAomWytIuziMhHcQvkpOOpi == AxisRange.Negative)
					{
						return elementIdentifierById.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002EnegativeName;
					}
				}
				return elementIdentifierById.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Ename;
			}
		}

		internal ElementAssignmentInfo(ControllerMap P_0, ElementAssignment P_1)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("controllerMap");
			}
			mGLvtujujDvRZOuuDTVdvNLWsBcP = P_1.actionId;
			ZZiynRcjxiFAvbkREgtChYqEBfarb = P_0;
			BgIuxtdSRAtvyYzaHiHsZzKAXgSc = P_1.elementMapId;
			zMCyDGlLPSGKZuZzFkRBMKcsJBJR = P_1.elementIdentifierId;
			gXFyaLdbbyGMwSMZocbOpqGjhZrY = P_1.keyboardKey;
			YBfXNNARrGGDuyUCPfmlUfDueasKA = P_1.modifierKeyFlags;
			vvYDrfpKyOndiPczREmOIebAGWGX = P_1.invert;
			wJbbzzGREDXYwJEwUDWJvLMDwUnS = cVDyIiOsEfJNYzVuZSmuEXqylgT.qKYDwIHBJWlecbzxaQOcnVEuKZDVb(P_1.type);
			ZyRfzdiLWYaVgSQyQSeCDTCZjWbH = P_1.axisContribution;
			sBiaFgAomWytIuziMhHcQvkpOOpi = P_1.axisRange;
			if (ZZiynRcjxiFAvbkREgtChYqEBfarb.controllerType == ControllerType.Keyboard)
			{
				Keyboard.LaQHNKAihvpLcKIdMLsAabBjrttvb(ref zMCyDGlLPSGKZuZzFkRBMKcsJBJR, ref gXFyaLdbbyGMwSMZocbOpqGjhZrY);
			}
		}
	}
}
