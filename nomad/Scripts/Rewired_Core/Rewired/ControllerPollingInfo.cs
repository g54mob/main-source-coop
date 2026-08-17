using UnityEngine;

namespace Rewired
{
	public struct ControllerPollingInfo
	{
		private bool dqJfpmBrJYZCALapXnOHWduMThJc;

		private int TPtTOVgcKexSgjbRnpTYumnpKxQB;

		private int tVylAybmhufiWHRWJSHHuMUMINYYA;

		private string lgMJTIgTZpROlUPTsCdSotquxyzG;

		private ControllerType UNpDkHtEkDLOAaUwhFdcGeunFyKGA;

		private ControllerElementType zbOIZOOIQdXOfekpOyknxmiJXXIJ;

		private int fodkPUpBslGDuGYHKERUzanyHqYv;

		private Pole kjiuELzMbHNxAuDeriLPbAfulHQAb;

		private string owtNwMivJLNliwbllewvJmsGGmAK;

		private int KBWappiHtgmcVBjuBOOLsnjvvzttB;

		private KeyCode oDjDjAgvlCZaehHscnwquQqFKiEJA;

		public bool success
		{
			get
			{
				return dqJfpmBrJYZCALapXnOHWduMThJc;
			}
			internal set
			{
				dqJfpmBrJYZCALapXnOHWduMThJc = flag;
			}
		}

		public int playerId
		{
			get
			{
				return TPtTOVgcKexSgjbRnpTYumnpKxQB;
			}
			internal set
			{
				TPtTOVgcKexSgjbRnpTYumnpKxQB = tPtTOVgcKexSgjbRnpTYumnpKxQB;
			}
		}

		public int controllerId
		{
			get
			{
				return tVylAybmhufiWHRWJSHHuMUMINYYA;
			}
			internal set
			{
				tVylAybmhufiWHRWJSHHuMUMINYYA = num;
			}
		}

		public string controllerName
		{
			get
			{
				return lgMJTIgTZpROlUPTsCdSotquxyzG;
			}
			internal set
			{
				lgMJTIgTZpROlUPTsCdSotquxyzG = text;
			}
		}

		public ControllerType controllerType
		{
			get
			{
				return UNpDkHtEkDLOAaUwhFdcGeunFyKGA;
			}
			internal set
			{
				UNpDkHtEkDLOAaUwhFdcGeunFyKGA = uNpDkHtEkDLOAaUwhFdcGeunFyKGA;
			}
		}

		public ControllerElementType elementType
		{
			get
			{
				return zbOIZOOIQdXOfekpOyknxmiJXXIJ;
			}
			internal set
			{
				zbOIZOOIQdXOfekpOyknxmiJXXIJ = controllerElementType;
			}
		}

		public int elementIndex
		{
			get
			{
				return fodkPUpBslGDuGYHKERUzanyHqYv;
			}
			internal set
			{
				fodkPUpBslGDuGYHKERUzanyHqYv = num;
			}
		}

		public Pole axisPole
		{
			get
			{
				return kjiuELzMbHNxAuDeriLPbAfulHQAb;
			}
			internal set
			{
				kjiuELzMbHNxAuDeriLPbAfulHQAb = pole;
			}
		}

		public string elementIdentifierName
		{
			get
			{
				return owtNwMivJLNliwbllewvJmsGGmAK;
			}
			internal set
			{
				owtNwMivJLNliwbllewvJmsGGmAK = text;
			}
		}

		public int elementIdentifierId
		{
			get
			{
				return KBWappiHtgmcVBjuBOOLsnjvvzttB;
			}
			internal set
			{
				KBWappiHtgmcVBjuBOOLsnjvvzttB = kBWappiHtgmcVBjuBOOLsnjvvzttB;
			}
		}

		public KeyCode keyboardKey
		{
			get
			{
				return oDjDjAgvlCZaehHscnwquQqFKiEJA;
			}
			internal set
			{
				oDjDjAgvlCZaehHscnwquQqFKiEJA = keyCode;
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
				if (!ReInput.KTVqyytqGISutLJQbfhSaKddBlfv.XxOxiaujmYdKOWQmRpJTtLcvbtRy(TPtTOVgcKexSgjbRnpTYumnpKxQB))
				{
					return null;
				}
				return ReInput.KTVqyytqGISutLJQbfhSaKddBlfv.LcqJOYavcMfniFdJGbmbfGcBCdPHA(TPtTOVgcKexSgjbRnpTYumnpKxQB);
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
				return ReInput.controllers.GetController(UNpDkHtEkDLOAaUwhFdcGeunFyKGA, tVylAybmhufiWHRWJSHHuMUMINYYA);
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
				return controller?.GetElementIdentifierById(KBWappiHtgmcVBjuBOOLsnjvvzttB);
			}
		}

		internal ControllerPollingInfo(bool P_0, int P_1, int P_2, string P_3, ControllerType P_4, ControllerElementType P_5, int P_6, Pole P_7, string P_8, int P_9, KeyCode P_10)
		{
			dqJfpmBrJYZCALapXnOHWduMThJc = P_0;
			TPtTOVgcKexSgjbRnpTYumnpKxQB = P_1;
			tVylAybmhufiWHRWJSHHuMUMINYYA = P_2;
			lgMJTIgTZpROlUPTsCdSotquxyzG = P_3;
			UNpDkHtEkDLOAaUwhFdcGeunFyKGA = P_4;
			zbOIZOOIQdXOfekpOyknxmiJXXIJ = P_5;
			fodkPUpBslGDuGYHKERUzanyHqYv = P_6;
			kjiuELzMbHNxAuDeriLPbAfulHQAb = P_7;
			owtNwMivJLNliwbllewvJmsGGmAK = P_8;
			KBWappiHtgmcVBjuBOOLsnjvvzttB = P_9;
			oDjDjAgvlCZaehHscnwquQqFKiEJA = P_10;
		}

		internal ControllerPollingInfo(ControllerPollingInfo P_0)
		{
			dqJfpmBrJYZCALapXnOHWduMThJc = P_0.dqJfpmBrJYZCALapXnOHWduMThJc;
			TPtTOVgcKexSgjbRnpTYumnpKxQB = P_0.TPtTOVgcKexSgjbRnpTYumnpKxQB;
			tVylAybmhufiWHRWJSHHuMUMINYYA = P_0.tVylAybmhufiWHRWJSHHuMUMINYYA;
			lgMJTIgTZpROlUPTsCdSotquxyzG = P_0.lgMJTIgTZpROlUPTsCdSotquxyzG;
			UNpDkHtEkDLOAaUwhFdcGeunFyKGA = P_0.UNpDkHtEkDLOAaUwhFdcGeunFyKGA;
			zbOIZOOIQdXOfekpOyknxmiJXXIJ = P_0.zbOIZOOIQdXOfekpOyknxmiJXXIJ;
			fodkPUpBslGDuGYHKERUzanyHqYv = P_0.fodkPUpBslGDuGYHKERUzanyHqYv;
			kjiuELzMbHNxAuDeriLPbAfulHQAb = P_0.kjiuELzMbHNxAuDeriLPbAfulHQAb;
			owtNwMivJLNliwbllewvJmsGGmAK = P_0.owtNwMivJLNliwbllewvJmsGGmAK;
			KBWappiHtgmcVBjuBOOLsnjvvzttB = P_0.KBWappiHtgmcVBjuBOOLsnjvvzttB;
			oDjDjAgvlCZaehHscnwquQqFKiEJA = P_0.oDjDjAgvlCZaehHscnwquQqFKiEJA;
		}

		internal static ControllerPollingInfo AqzEIPyMjpyXUFHmMVsymezoLmoQ()
		{
			return new ControllerPollingInfo(false, -1, -1, string.Empty, ControllerType.Keyboard, ControllerElementType.Axis, -1, Pole.Positive, string.Empty, -1, KeyCode.None);
		}
	}
}
