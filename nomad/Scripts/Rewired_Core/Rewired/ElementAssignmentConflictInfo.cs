using UnityEngine;

namespace Rewired
{
	public struct ElementAssignmentConflictInfo
	{
		private bool WaNfcsHZFdphLRTHkZjVYbMGgvBCA;

		private bool WgJHhvngEVttnevuupjZhqQZFMQK;

		private int sqiCLBEusealupvOfwsCplVSEeNU;

		private ControllerType BiOomBMhCtQfynVkXvhSIalsIONz;

		private int IdFYaycQBLuPErjgtGyQdrvtaqTTA;

		private int jjrvauZlUlMFLpfgCsWVshGQlHdj;

		private int wVhemwzxPVKvpEjKoZNWLeaPzbAY;

		private ControllerElementType tNnBuJAeontcOoEzkzBtlDxTpyvw;

		private int gIfjLTHmtbJAeHBWtEoAfukVnbUk;

		private KeyCode htikdAvziVAMwdhoreyIeLUzpoKc;

		private ModifierKeyFlags xCASqjzKQyYsWmxfHwRtaGWLNEaH;

		private int pHhxEAtoHLBEFThFvroOoFXbdjHx;

		public bool isConflict
		{
			get
			{
				return WaNfcsHZFdphLRTHkZjVYbMGgvBCA;
			}
			internal set
			{
				WaNfcsHZFdphLRTHkZjVYbMGgvBCA = waNfcsHZFdphLRTHkZjVYbMGgvBCA;
			}
		}

		public bool isUserAssignable
		{
			get
			{
				return WgJHhvngEVttnevuupjZhqQZFMQK;
			}
			internal set
			{
				WgJHhvngEVttnevuupjZhqQZFMQK = wgJHhvngEVttnevuupjZhqQZFMQK;
			}
		}

		public int playerId
		{
			get
			{
				return sqiCLBEusealupvOfwsCplVSEeNU;
			}
			internal set
			{
				sqiCLBEusealupvOfwsCplVSEeNU = num;
			}
		}

		public ControllerType controllerType
		{
			get
			{
				return BiOomBMhCtQfynVkXvhSIalsIONz;
			}
			internal set
			{
				BiOomBMhCtQfynVkXvhSIalsIONz = biOomBMhCtQfynVkXvhSIalsIONz;
			}
		}

		public int controllerId
		{
			get
			{
				return IdFYaycQBLuPErjgtGyQdrvtaqTTA;
			}
			internal set
			{
				IdFYaycQBLuPErjgtGyQdrvtaqTTA = idFYaycQBLuPErjgtGyQdrvtaqTTA;
			}
		}

		public int controllerMapId
		{
			get
			{
				return jjrvauZlUlMFLpfgCsWVshGQlHdj;
			}
			internal set
			{
				jjrvauZlUlMFLpfgCsWVshGQlHdj = num;
			}
		}

		public int elementMapId
		{
			get
			{
				return wVhemwzxPVKvpEjKoZNWLeaPzbAY;
			}
			internal set
			{
				wVhemwzxPVKvpEjKoZNWLeaPzbAY = num;
			}
		}

		public ControllerElementType elementType
		{
			get
			{
				return tNnBuJAeontcOoEzkzBtlDxTpyvw;
			}
			internal set
			{
				tNnBuJAeontcOoEzkzBtlDxTpyvw = controllerElementType;
			}
		}

		public int elementIdentifierId
		{
			get
			{
				return gIfjLTHmtbJAeHBWtEoAfukVnbUk;
			}
			internal set
			{
				gIfjLTHmtbJAeHBWtEoAfukVnbUk = num;
			}
		}

		public KeyCode keyCode
		{
			get
			{
				return htikdAvziVAMwdhoreyIeLUzpoKc;
			}
			internal set
			{
				htikdAvziVAMwdhoreyIeLUzpoKc = keyCode;
			}
		}

		public ModifierKeyFlags modifierKeyFlags
		{
			get
			{
				return xCASqjzKQyYsWmxfHwRtaGWLNEaH;
			}
			internal set
			{
				xCASqjzKQyYsWmxfHwRtaGWLNEaH = modifierKeyFlags;
			}
		}

		public int actionId
		{
			get
			{
				return pHhxEAtoHLBEFThFvroOoFXbdjHx;
			}
			internal set
			{
				pHhxEAtoHLBEFThFvroOoFXbdjHx = num;
			}
		}

		public Player player
		{
			get
			{
				if (!ReInput.isReady)
				{
					return null;
				}
				return ReInput.players.GetPlayer(sqiCLBEusealupvOfwsCplVSEeNU);
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
				return ReInput.mapping.GetAction(pHhxEAtoHLBEFThFvroOoFXbdjHx);
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
				return ReInput.controllers.GetController(BiOomBMhCtQfynVkXvhSIalsIONz, IdFYaycQBLuPErjgtGyQdrvtaqTTA);
			}
		}

		public ControllerMap controllerMap
		{
			get
			{
				if (player == null)
				{
					return null;
				}
				return player.controllers.maps.GetMap(BiOomBMhCtQfynVkXvhSIalsIONz, IdFYaycQBLuPErjgtGyQdrvtaqTTA, jjrvauZlUlMFLpfgCsWVshGQlHdj);
			}
		}

		public ControllerElementIdentifier elementIdentifier
		{
			get
			{
				if (controller == null)
				{
					return null;
				}
				return controller.GetElementIdentifierById(gIfjLTHmtbJAeHBWtEoAfukVnbUk);
			}
		}

		public ActionElementMap elementMap
		{
			get
			{
				if (controllerMap == null)
				{
					return null;
				}
				return controllerMap.GetElementMap(wVhemwzxPVKvpEjKoZNWLeaPzbAY);
			}
		}

		public string elementDisplayName
		{
			get
			{
				if (BiOomBMhCtQfynVkXvhSIalsIONz == ControllerType.Keyboard)
				{
					return Keyboard.GetKeyName(htikdAvziVAMwdhoreyIeLUzpoKc, xCASqjzKQyYsWmxfHwRtaGWLNEaH);
				}
				if (controller == null)
				{
					return string.Empty;
				}
				ControllerElementIdentifier elementIdentifierById = controller.GetElementIdentifierById(gIfjLTHmtbJAeHBWtEoAfukVnbUk);
				if (elementIdentifierById == null)
				{
					return string.Empty;
				}
				return elementIdentifierById.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Ename;
			}
		}

		public ElementAssignmentConflictInfo(bool P_0, bool P_1, int P_2, ControllerType P_3, int P_4, int P_5, int P_6, int P_7, ControllerElementType P_8, int P_9, KeyCode P_10, ModifierKeyFlags P_11)
		{
			WaNfcsHZFdphLRTHkZjVYbMGgvBCA = P_0;
			WgJHhvngEVttnevuupjZhqQZFMQK = P_1;
			sqiCLBEusealupvOfwsCplVSEeNU = P_2;
			BiOomBMhCtQfynVkXvhSIalsIONz = P_3;
			IdFYaycQBLuPErjgtGyQdrvtaqTTA = P_4;
			jjrvauZlUlMFLpfgCsWVshGQlHdj = P_5;
			wVhemwzxPVKvpEjKoZNWLeaPzbAY = P_6;
			pHhxEAtoHLBEFThFvroOoFXbdjHx = P_7;
			tNnBuJAeontcOoEzkzBtlDxTpyvw = P_8;
			gIfjLTHmtbJAeHBWtEoAfukVnbUk = P_9;
			htikdAvziVAMwdhoreyIeLUzpoKc = P_10;
			xCASqjzKQyYsWmxfHwRtaGWLNEaH = P_11;
		}

		public ElementAssignmentConflictInfo(ElementAssignmentConflictInfo P_0)
		{
			WaNfcsHZFdphLRTHkZjVYbMGgvBCA = P_0.WaNfcsHZFdphLRTHkZjVYbMGgvBCA;
			WgJHhvngEVttnevuupjZhqQZFMQK = P_0.WgJHhvngEVttnevuupjZhqQZFMQK;
			sqiCLBEusealupvOfwsCplVSEeNU = P_0.sqiCLBEusealupvOfwsCplVSEeNU;
			BiOomBMhCtQfynVkXvhSIalsIONz = P_0.BiOomBMhCtQfynVkXvhSIalsIONz;
			IdFYaycQBLuPErjgtGyQdrvtaqTTA = P_0.IdFYaycQBLuPErjgtGyQdrvtaqTTA;
			jjrvauZlUlMFLpfgCsWVshGQlHdj = P_0.jjrvauZlUlMFLpfgCsWVshGQlHdj;
			wVhemwzxPVKvpEjKoZNWLeaPzbAY = P_0.wVhemwzxPVKvpEjKoZNWLeaPzbAY;
			pHhxEAtoHLBEFThFvroOoFXbdjHx = P_0.pHhxEAtoHLBEFThFvroOoFXbdjHx;
			tNnBuJAeontcOoEzkzBtlDxTpyvw = P_0.tNnBuJAeontcOoEzkzBtlDxTpyvw;
			gIfjLTHmtbJAeHBWtEoAfukVnbUk = P_0.gIfjLTHmtbJAeHBWtEoAfukVnbUk;
			htikdAvziVAMwdhoreyIeLUzpoKc = P_0.htikdAvziVAMwdhoreyIeLUzpoKc;
			xCASqjzKQyYsWmxfHwRtaGWLNEaH = P_0.xCASqjzKQyYsWmxfHwRtaGWLNEaH;
		}
	}
}
