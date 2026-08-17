using UnityEngine;

namespace Rewired
{
	public struct ControllerPollingInfo
	{
		private bool RgJlwlHPWKGUDrAhTMwzcxKAffwI;

		private int DCGJXzigEdTcwzFynDKUcORiHGSOA;

		private int UceKlChkUHGjQaNwpAzeqaieFMpt;

		private string tnqDtnjrRmrTJiUPBcWlFVtippWic;

		private ControllerType KHkewZTAeuvHVDDJaWvnQRRxxDiF;

		private ControllerElementType RUyyQSEmqRidLNphokXShBrzehNy;

		private int FBOOknsxpcCbVstgmGENdPFIgrbiA;

		private Pole wthecuAoLwkFVagmarzrbcdkdZbFc;

		private string PslWOFZWArVhPwzuPAbExXVaDTAn;

		private int NPuaUGjYHfmjnFZazhpMoRbThWzG;

		private KeyCode UdwLoYILOubLSIDfSvbdnKJFoCVNA;

		public bool success
		{
			get
			{
				return RgJlwlHPWKGUDrAhTMwzcxKAffwI;
			}
			internal set
			{
				RgJlwlHPWKGUDrAhTMwzcxKAffwI = rgJlwlHPWKGUDrAhTMwzcxKAffwI;
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

		public string controllerName
		{
			get
			{
				return tnqDtnjrRmrTJiUPBcWlFVtippWic;
			}
			internal set
			{
				tnqDtnjrRmrTJiUPBcWlFVtippWic = text;
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

		public int elementIndex
		{
			get
			{
				return FBOOknsxpcCbVstgmGENdPFIgrbiA;
			}
			internal set
			{
				FBOOknsxpcCbVstgmGENdPFIgrbiA = fBOOknsxpcCbVstgmGENdPFIgrbiA;
			}
		}

		public Pole axisPole
		{
			get
			{
				return wthecuAoLwkFVagmarzrbcdkdZbFc;
			}
			internal set
			{
				wthecuAoLwkFVagmarzrbcdkdZbFc = pole;
			}
		}

		public string elementIdentifierName
		{
			get
			{
				return PslWOFZWArVhPwzuPAbExXVaDTAn;
			}
			internal set
			{
				PslWOFZWArVhPwzuPAbExXVaDTAn = pslWOFZWArVhPwzuPAbExXVaDTAn;
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

		public KeyCode keyboardKey
		{
			get
			{
				return UdwLoYILOubLSIDfSvbdnKJFoCVNA;
			}
			internal set
			{
				UdwLoYILOubLSIDfSvbdnKJFoCVNA = udwLoYILOubLSIDfSvbdnKJFoCVNA;
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
				if (!ReInput.hqGXmYxZUrSrCxDobJTGEPCWvHAsA.fVVcnKcITKFRLpCybjQgHHsavAvUA(DCGJXzigEdTcwzFynDKUcORiHGSOA))
				{
					return null;
				}
				return ReInput.hqGXmYxZUrSrCxDobJTGEPCWvHAsA.ynWFEmrsktqGecVuJbofDaNeQFxn(DCGJXzigEdTcwzFynDKUcORiHGSOA);
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

		public ControllerElementIdentifier elementIdentifier
		{
			get
			{
				if (!ReInput.isReady)
				{
					return null;
				}
				return controller?.GetElementIdentifierById(NPuaUGjYHfmjnFZazhpMoRbThWzG);
			}
		}

		internal ControllerPollingInfo(bool P_0, int P_1, int P_2, string P_3, ControllerType P_4, ControllerElementType P_5, int P_6, Pole P_7, string P_8, int P_9, KeyCode P_10)
		{
			RgJlwlHPWKGUDrAhTMwzcxKAffwI = P_0;
			DCGJXzigEdTcwzFynDKUcORiHGSOA = P_1;
			UceKlChkUHGjQaNwpAzeqaieFMpt = P_2;
			tnqDtnjrRmrTJiUPBcWlFVtippWic = P_3;
			KHkewZTAeuvHVDDJaWvnQRRxxDiF = P_4;
			RUyyQSEmqRidLNphokXShBrzehNy = P_5;
			FBOOknsxpcCbVstgmGENdPFIgrbiA = P_6;
			wthecuAoLwkFVagmarzrbcdkdZbFc = P_7;
			PslWOFZWArVhPwzuPAbExXVaDTAn = P_8;
			NPuaUGjYHfmjnFZazhpMoRbThWzG = P_9;
			UdwLoYILOubLSIDfSvbdnKJFoCVNA = P_10;
		}

		internal ControllerPollingInfo(ControllerPollingInfo P_0)
		{
			RgJlwlHPWKGUDrAhTMwzcxKAffwI = P_0.RgJlwlHPWKGUDrAhTMwzcxKAffwI;
			DCGJXzigEdTcwzFynDKUcORiHGSOA = P_0.DCGJXzigEdTcwzFynDKUcORiHGSOA;
			UceKlChkUHGjQaNwpAzeqaieFMpt = P_0.UceKlChkUHGjQaNwpAzeqaieFMpt;
			tnqDtnjrRmrTJiUPBcWlFVtippWic = P_0.tnqDtnjrRmrTJiUPBcWlFVtippWic;
			KHkewZTAeuvHVDDJaWvnQRRxxDiF = P_0.KHkewZTAeuvHVDDJaWvnQRRxxDiF;
			RUyyQSEmqRidLNphokXShBrzehNy = P_0.RUyyQSEmqRidLNphokXShBrzehNy;
			FBOOknsxpcCbVstgmGENdPFIgrbiA = P_0.FBOOknsxpcCbVstgmGENdPFIgrbiA;
			wthecuAoLwkFVagmarzrbcdkdZbFc = P_0.wthecuAoLwkFVagmarzrbcdkdZbFc;
			PslWOFZWArVhPwzuPAbExXVaDTAn = P_0.PslWOFZWArVhPwzuPAbExXVaDTAn;
			NPuaUGjYHfmjnFZazhpMoRbThWzG = P_0.NPuaUGjYHfmjnFZazhpMoRbThWzG;
			UdwLoYILOubLSIDfSvbdnKJFoCVNA = P_0.UdwLoYILOubLSIDfSvbdnKJFoCVNA;
		}

		internal static ControllerPollingInfo TCcbVoDrsLjCcBTyyQNnWzCBaEGA()
		{
			return new ControllerPollingInfo(false, -1, -1, string.Empty, ControllerType.Keyboard, ControllerElementType.Axis, -1, Pole.Positive, string.Empty, -1, KeyCode.None);
		}
	}
}
