using System;
using UnityEngine;

namespace Rewired
{
	public sealed class ElementAssignmentInfo
	{
		private readonly ControllerMap mtCzlvnEPTtCfVpotNRemSaRaiHy;

		private readonly ControllerElementType RUyyQSEmqRidLNphokXShBrzehNy;

		private readonly int xVHNxtfyyXJdLbTTurZRUrXDWlQl;

		private readonly int NPuaUGjYHfmjnFZazhpMoRbThWzG;

		private readonly AxisRange dnqcDaKuDzTeZpkjWrulGAfmNtHmA;

		private readonly KeyCode KXtzntqEOewzFQVRaxggtOiPPVwx;

		private readonly ModifierKeyFlags njfMvUCuswHuLtHoZcdqAwTyNgamA;

		private readonly int JwOchbdvhsvHVdSyYjtqEJdTZzYG;

		private readonly Pole ysANvLtALhiLvQPHMrzVONBSGlf;

		private readonly bool GYjNfJzKOfaDDnMNUsNgyurjLVIT;

		public Player player
		{
			get
			{
				if (!ReInput.isReady)
				{
					return null;
				}
				if (mtCzlvnEPTtCfVpotNRemSaRaiHy == null)
				{
					return null;
				}
				return mtCzlvnEPTtCfVpotNRemSaRaiHy.player;
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
				return ReInput.mapping.GetAction(JwOchbdvhsvHVdSyYjtqEJdTZzYG);
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
				if (mtCzlvnEPTtCfVpotNRemSaRaiHy == null)
				{
					return null;
				}
				return ReInput.controllers.GetController(mtCzlvnEPTtCfVpotNRemSaRaiHy.controllerType, mtCzlvnEPTtCfVpotNRemSaRaiHy.controllerId);
			}
		}

		public ControllerType controllerType
		{
			get
			{
				if (!ReInput.isReady || mtCzlvnEPTtCfVpotNRemSaRaiHy == null)
				{
					return ControllerType.Keyboard;
				}
				return mtCzlvnEPTtCfVpotNRemSaRaiHy.controllerType;
			}
		}

		public int controllerId
		{
			get
			{
				if (!ReInput.isReady || mtCzlvnEPTtCfVpotNRemSaRaiHy == null)
				{
					return -1;
				}
				return mtCzlvnEPTtCfVpotNRemSaRaiHy.controllerId;
			}
		}

		public ControllerMap controllerMap => mtCzlvnEPTtCfVpotNRemSaRaiHy;

		public ControllerElementIdentifier elementIdentifier
		{
			get
			{
				if (controller == null)
				{
					return null;
				}
				return controller.GetElementIdentifierById(NPuaUGjYHfmjnFZazhpMoRbThWzG);
			}
		}

		public ActionElementMap elementMap
		{
			get
			{
				if (mtCzlvnEPTtCfVpotNRemSaRaiHy == null)
				{
					return null;
				}
				return mtCzlvnEPTtCfVpotNRemSaRaiHy.GetElementMap(xVHNxtfyyXJdLbTTurZRUrXDWlQl);
			}
		}

		public ControllerElementType elementType => RUyyQSEmqRidLNphokXShBrzehNy;

		public Pole axisContribution => ysANvLtALhiLvQPHMrzVONBSGlf;

		public AxisRange axisRange => dnqcDaKuDzTeZpkjWrulGAfmNtHmA;

		public bool invert => GYjNfJzKOfaDDnMNUsNgyurjLVIT;

		public KeyCode keyCode => KXtzntqEOewzFQVRaxggtOiPPVwx;

		public ModifierKeyFlags modifierKeyFlags => njfMvUCuswHuLtHoZcdqAwTyNgamA;

		public string elementDisplayName
		{
			get
			{
				if (mtCzlvnEPTtCfVpotNRemSaRaiHy == null)
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
				ControllerElementIdentifier elementIdentifierById = controller.GetElementIdentifierById(NPuaUGjYHfmjnFZazhpMoRbThWzG);
				if (elementIdentifierById == null)
				{
					return string.Empty;
				}
				if (RUyyQSEmqRidLNphokXShBrzehNy == ControllerElementType.Axis)
				{
					if (dnqcDaKuDzTeZpkjWrulGAfmNtHmA == AxisRange.Full)
					{
						return elementIdentifierById.name;
					}
					if (dnqcDaKuDzTeZpkjWrulGAfmNtHmA == AxisRange.Positive)
					{
						return elementIdentifierById.positiveName;
					}
					if (dnqcDaKuDzTeZpkjWrulGAfmNtHmA == AxisRange.Negative)
					{
						return elementIdentifierById.negativeName;
					}
				}
				return elementIdentifierById.name;
			}
		}

		internal ElementAssignmentInfo(ControllerMap P_0, ElementAssignment P_1)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("controllerMap");
			}
			JwOchbdvhsvHVdSyYjtqEJdTZzYG = P_1.actionId;
			mtCzlvnEPTtCfVpotNRemSaRaiHy = P_0;
			xVHNxtfyyXJdLbTTurZRUrXDWlQl = P_1.elementMapId;
			NPuaUGjYHfmjnFZazhpMoRbThWzG = P_1.elementIdentifierId;
			KXtzntqEOewzFQVRaxggtOiPPVwx = P_1.keyboardKey;
			njfMvUCuswHuLtHoZcdqAwTyNgamA = P_1.modifierKeyFlags;
			GYjNfJzKOfaDDnMNUsNgyurjLVIT = P_1.invert;
			RUyyQSEmqRidLNphokXShBrzehNy = OzpbBOHoTkskKeBaOXWaTXQCmjjBA.jabYOprZCpTijtFrYngdXNagQtNF(P_1.type);
			ysANvLtALhiLvQPHMrzVONBSGlf = P_1.axisContribution;
			dnqcDaKuDzTeZpkjWrulGAfmNtHmA = P_1.axisRange;
			if (mtCzlvnEPTtCfVpotNRemSaRaiHy.controllerType == ControllerType.Keyboard)
			{
				Keyboard.CYCUDNoanHYzGAabMMwoCdyIUash(ref NPuaUGjYHfmjnFZazhpMoRbThWzG, ref KXtzntqEOewzFQVRaxggtOiPPVwx);
			}
		}
	}
}
