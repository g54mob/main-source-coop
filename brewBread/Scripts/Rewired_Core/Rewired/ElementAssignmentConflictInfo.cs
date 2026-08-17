using UnityEngine;

namespace Rewired
{
	public struct ElementAssignmentConflictInfo
	{
		private bool rtKmAKSFItMMpRWEuhaFffvHwDsu;

		private bool cKcmQFFTLKYbwUkpKgtibAUiudpb;

		private int DCGJXzigEdTcwzFynDKUcORiHGSOA;

		private ControllerType KHkewZTAeuvHVDDJaWvnQRRxxDiF;

		private int UceKlChkUHGjQaNwpAzeqaieFMpt;

		private int ckPQPTauGgjqsjnoEwPMbqktUGkt;

		private int HMJUwIEbjwQyUZsaCSgzsDgauWV;

		private ControllerElementType RUyyQSEmqRidLNphokXShBrzehNy;

		private int NPuaUGjYHfmjnFZazhpMoRbThWzG;

		private KeyCode KXtzntqEOewzFQVRaxggtOiPPVwx;

		private ModifierKeyFlags njfMvUCuswHuLtHoZcdqAwTyNgamA;

		private int JwOchbdvhsvHVdSyYjtqEJdTZzYG;

		public bool isConflict
		{
			get
			{
				return rtKmAKSFItMMpRWEuhaFffvHwDsu;
			}
			internal set
			{
				rtKmAKSFItMMpRWEuhaFffvHwDsu = flag;
			}
		}

		public bool isUserAssignable
		{
			get
			{
				return cKcmQFFTLKYbwUkpKgtibAUiudpb;
			}
			internal set
			{
				cKcmQFFTLKYbwUkpKgtibAUiudpb = flag;
			}
		}

		public int playerId
		{
			get
			{
				return DCGJXzigEdTcwzFynDKUcORiHGSOA;
			}
			internal set
			{
				DCGJXzigEdTcwzFynDKUcORiHGSOA = dCGJXzigEdTcwzFynDKUcORiHGSOA;
			}
		}

		public ControllerType controllerType
		{
			get
			{
				return KHkewZTAeuvHVDDJaWvnQRRxxDiF;
			}
			internal set
			{
				KHkewZTAeuvHVDDJaWvnQRRxxDiF = kHkewZTAeuvHVDDJaWvnQRRxxDiF;
			}
		}

		public int controllerId
		{
			get
			{
				return UceKlChkUHGjQaNwpAzeqaieFMpt;
			}
			internal set
			{
				UceKlChkUHGjQaNwpAzeqaieFMpt = uceKlChkUHGjQaNwpAzeqaieFMpt;
			}
		}

		public int controllerMapId
		{
			get
			{
				return ckPQPTauGgjqsjnoEwPMbqktUGkt;
			}
			internal set
			{
				ckPQPTauGgjqsjnoEwPMbqktUGkt = num;
			}
		}

		public int elementMapId
		{
			get
			{
				return HMJUwIEbjwQyUZsaCSgzsDgauWV;
			}
			internal set
			{
				HMJUwIEbjwQyUZsaCSgzsDgauWV = hMJUwIEbjwQyUZsaCSgzsDgauWV;
			}
		}

		public ControllerElementType elementType
		{
			get
			{
				return RUyyQSEmqRidLNphokXShBrzehNy;
			}
			internal set
			{
				RUyyQSEmqRidLNphokXShBrzehNy = rUyyQSEmqRidLNphokXShBrzehNy;
			}
		}

		public int elementIdentifierId
		{
			get
			{
				return NPuaUGjYHfmjnFZazhpMoRbThWzG;
			}
			internal set
			{
				NPuaUGjYHfmjnFZazhpMoRbThWzG = nPuaUGjYHfmjnFZazhpMoRbThWzG;
			}
		}

		public KeyCode keyCode
		{
			get
			{
				return KXtzntqEOewzFQVRaxggtOiPPVwx;
			}
			internal set
			{
				KXtzntqEOewzFQVRaxggtOiPPVwx = kXtzntqEOewzFQVRaxggtOiPPVwx;
			}
		}

		public ModifierKeyFlags modifierKeyFlags
		{
			get
			{
				return njfMvUCuswHuLtHoZcdqAwTyNgamA;
			}
			internal set
			{
				njfMvUCuswHuLtHoZcdqAwTyNgamA = modifierKeyFlags;
			}
		}

		public int actionId
		{
			get
			{
				return JwOchbdvhsvHVdSyYjtqEJdTZzYG;
			}
			internal set
			{
				JwOchbdvhsvHVdSyYjtqEJdTZzYG = jwOchbdvhsvHVdSyYjtqEJdTZzYG;
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
				return ReInput.players.GetPlayer(DCGJXzigEdTcwzFynDKUcORiHGSOA);
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
				return ReInput.controllers.GetController(KHkewZTAeuvHVDDJaWvnQRRxxDiF, UceKlChkUHGjQaNwpAzeqaieFMpt);
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
				return player.controllers.maps.GetMap(KHkewZTAeuvHVDDJaWvnQRRxxDiF, UceKlChkUHGjQaNwpAzeqaieFMpt, ckPQPTauGgjqsjnoEwPMbqktUGkt);
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
				return controller.GetElementIdentifierById(NPuaUGjYHfmjnFZazhpMoRbThWzG);
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
				return controllerMap.GetElementMap(HMJUwIEbjwQyUZsaCSgzsDgauWV);
			}
		}

		public string elementDisplayName
		{
			get
			{
				if (KHkewZTAeuvHVDDJaWvnQRRxxDiF == ControllerType.Keyboard)
				{
					return Keyboard.GetKeyName(KXtzntqEOewzFQVRaxggtOiPPVwx, njfMvUCuswHuLtHoZcdqAwTyNgamA);
				}
				if (controller == null)
				{
					return string.Empty;
				}
				ControllerElementIdentifier elementIdentifierById = controller.GetElementIdentifierById(NPuaUGjYHfmjnFZazhpMoRbThWzG);
				if (elementIdentifierById == null)
				{
					return string.Empty;
				}
				return elementIdentifierById.name;
			}
		}

		public ElementAssignmentConflictInfo(bool P_0, bool P_1, int P_2, ControllerType P_3, int P_4, int P_5, int P_6, int P_7, ControllerElementType P_8, int P_9, KeyCode P_10, ModifierKeyFlags P_11)
		{
			rtKmAKSFItMMpRWEuhaFffvHwDsu = P_0;
			cKcmQFFTLKYbwUkpKgtibAUiudpb = P_1;
			DCGJXzigEdTcwzFynDKUcORiHGSOA = P_2;
			KHkewZTAeuvHVDDJaWvnQRRxxDiF = P_3;
			UceKlChkUHGjQaNwpAzeqaieFMpt = P_4;
			ckPQPTauGgjqsjnoEwPMbqktUGkt = P_5;
			HMJUwIEbjwQyUZsaCSgzsDgauWV = P_6;
			JwOchbdvhsvHVdSyYjtqEJdTZzYG = P_7;
			RUyyQSEmqRidLNphokXShBrzehNy = P_8;
			NPuaUGjYHfmjnFZazhpMoRbThWzG = P_9;
			KXtzntqEOewzFQVRaxggtOiPPVwx = P_10;
			njfMvUCuswHuLtHoZcdqAwTyNgamA = P_11;
		}

		public ElementAssignmentConflictInfo(ElementAssignmentConflictInfo P_0)
		{
			rtKmAKSFItMMpRWEuhaFffvHwDsu = P_0.rtKmAKSFItMMpRWEuhaFffvHwDsu;
			cKcmQFFTLKYbwUkpKgtibAUiudpb = P_0.cKcmQFFTLKYbwUkpKgtibAUiudpb;
			DCGJXzigEdTcwzFynDKUcORiHGSOA = P_0.DCGJXzigEdTcwzFynDKUcORiHGSOA;
			KHkewZTAeuvHVDDJaWvnQRRxxDiF = P_0.KHkewZTAeuvHVDDJaWvnQRRxxDiF;
			UceKlChkUHGjQaNwpAzeqaieFMpt = P_0.UceKlChkUHGjQaNwpAzeqaieFMpt;
			ckPQPTauGgjqsjnoEwPMbqktUGkt = P_0.ckPQPTauGgjqsjnoEwPMbqktUGkt;
			HMJUwIEbjwQyUZsaCSgzsDgauWV = P_0.HMJUwIEbjwQyUZsaCSgzsDgauWV;
			JwOchbdvhsvHVdSyYjtqEJdTZzYG = P_0.JwOchbdvhsvHVdSyYjtqEJdTZzYG;
			RUyyQSEmqRidLNphokXShBrzehNy = P_0.RUyyQSEmqRidLNphokXShBrzehNy;
			NPuaUGjYHfmjnFZazhpMoRbThWzG = P_0.NPuaUGjYHfmjnFZazhpMoRbThWzG;
			KXtzntqEOewzFQVRaxggtOiPPVwx = P_0.KXtzntqEOewzFQVRaxggtOiPPVwx;
			njfMvUCuswHuLtHoZcdqAwTyNgamA = P_0.njfMvUCuswHuLtHoZcdqAwTyNgamA;
		}
	}
}
