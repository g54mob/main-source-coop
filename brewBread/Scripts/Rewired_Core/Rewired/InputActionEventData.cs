using System.Collections.Generic;

namespace Rewired
{
	public struct InputActionEventData
	{
		private ItaEjuKtoATtafYloiocFOanqPQc ztqcsjQogOGkbcfdohylerMDCirk;

		private InputActionEventType vMNdyFbeFKZgkjkoYtuNxpwHfmqv;

		public readonly int playerId;

		public readonly int actionId;

		public readonly UpdateLoopType updateLoop;

		public InputActionEventType eventType
		{
			get
			{
				return vMNdyFbeFKZgkjkoYtuNxpwHfmqv;
			}
			internal set
			{
				vMNdyFbeFKZgkjkoYtuNxpwHfmqv = inputActionEventType;
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
				return ReInput.players.GetPlayer(playerId);
			}
		}

		public string actionName
		{
			get
			{
				if (!ReInput.isReady)
				{
					return string.Empty;
				}
				return ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.SbJXHzMCJxVIoMZqRpTsxBZFrfWP(actionId).name;
			}
		}

		public string actionDescriptiveName
		{
			get
			{
				if (!ReInput.isReady)
				{
					return string.Empty;
				}
				return ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.SbJXHzMCJxVIoMZqRpTsxBZFrfWP(actionId).descriptiveName;
			}
		}

		public float GetAxis()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.JAUpfGusnmCqpzYFDMsATYuepPVJ();
		}

		public float GetAxisPrev()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.lNAOwBCyLxBSgQJDKGMNCXnwEnDF();
		}

		public float GetAxisDelta()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.PTEcMqUvDghumSPLWxVnZSoJDOVO();
		}

		public double GetAxisTimeActive()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.YFQmTaHRUJTOvdxGrEsdrTEFFEYK();
		}

		public double GetAxisTimeInactive()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.CcgXMAOkcSxRTmABDVjgbiwuOInX();
		}

		public float GetAxisRaw()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.KQaRYvVIaIiONwzKwSPiKLhzcpVbA();
		}

		public float GetAxisRawDelta()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.yaDusqwnNTMlZdCZQGwEhLjmdKKAb();
		}

		public float GetAxisRawPrev()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.PRHdLCgJAJrdPtapwSgOEeJvfUdjA();
		}

		public double GetAxisRawTimeActive()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.DgojYOiHiTSDgmWIXBmBNiILyFgH();
		}

		public double GetAxisRawTimeInactive()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.BdYbyJaiXNKywgwZXlZjtDlVuJASA();
		}

		public AxisCoordinateMode GetAxisCoordinateMode()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.XwaPBegNoHkiaflVurnbLMqGQsjy();
		}

		public AxisCoordinateMode GetAxisCoordinateModePrev()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.fuylXgIlAVMXaLWcffBmegNfnigw();
		}

		public AxisCoordinateMode GetAxisRawCoordinateMode()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.kcKIcomUYfStYolsKEjBuwouohTU();
		}

		public AxisCoordinateMode GetAxisRawCoordinateModePrev()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.XLQmlfgYhkNhdiulQkKJGprALRYv();
		}

		public bool GetButton()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.IKCLDPTiIKTdmSlZPgUWaOkbUbhVA();
		}

		public bool GetButtonPrev()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.OQBBxsOrfqCVjzijqOkpltRYRQId();
		}

		public bool GetButtonDown()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.LbzwmtxTiWvoFmsIMWGeWeOGjmIg();
		}

		public bool GetButtonUp()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.vJpqJdBUhGAMATiWyyWbrqfEhKUg();
		}

		public bool GetButtonSinglePressHold()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.JEUbHmDevitEshAxNQgiDYvWQFQaA();
		}

		public bool GetButtonSinglePressDown()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.WuinqRKRejufNxAUlwSDdnuZjRHO();
		}

		public bool GetButtonSinglePressUp()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.DoAgFePMYTgosHbxmRcYfEyLjZdK();
		}

		public bool GetButtonDoublePressDown()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.fkcDkMJQbeWOyLvgzpIFQvQPiSWm();
		}

		public bool GetButtonDoublePressDown(float speed)
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.fkcDkMJQbeWOyLvgzpIFQvQPiSWm(speed);
		}

		public bool GetButtonDoublePressHold()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.DMfqNDOKfUnRtMPPbvMgsnegbJKE();
		}

		public bool GetButtonDoublePressHold(float speed)
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.DMfqNDOKfUnRtMPPbvMgsnegbJKE(speed);
		}

		public bool GetButtonDoublePressUp()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.mAykGDDTNEaZGawmdkTlpUjLjYun();
		}

		public bool GetButtonDoublePressUp(float speed)
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.mAykGDDTNEaZGawmdkTlpUjLjYun(speed);
		}

		public bool GetButtonTimedPress(float time)
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.TnGUUrcnnYBlhZVNcnTESFJZIRex(time, 0f);
		}

		public bool GetButtonTimedPress(float time, float expireIn)
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.TnGUUrcnnYBlhZVNcnTESFJZIRex(time, expireIn);
		}

		public bool GetButtonTimedPressDown(float time)
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.NjkdPCGmRZCYclxqltumoTakkgJRA(time);
		}

		public bool GetButtonTimedPressUp(float time)
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.SEnFNGPhTWbIpEEqDBfjyYToighH(time, 0f);
		}

		public bool GetButtonTimedPressUp(float time, float expireIn)
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.SEnFNGPhTWbIpEEqDBfjyYToighH(time, expireIn);
		}

		public bool GetButtonShortPress()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.QrEesxjbxPBEfHodumPCacAeRLgnb();
		}

		public bool GetButtonShortPressDown()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.JrIKHzCKgQisRHAHGkvPbqOwnpsW();
		}

		public bool GetButtonShortPressUp()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.dzkeHBLJOMxVNWjvzhdVJnJMvJIr();
		}

		public bool GetButtonLongPress()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.MaDlmiKbebbDojyriMusfaKkBxLKA();
		}

		public bool GetButtonLongPressDown()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.DlLZnBghBYhLlitiiJWbinuIGoTTA();
		}

		public bool GetButtonLongPressUp()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.VbUHFmEFVWLsPBcjQQphILbxpWgK();
		}

		public bool GetButtonRepeating()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.eaOxZJjpbGEIJYhkaADdLNNobROD();
		}

		public double GetButtonTimePressed()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.nIZqWdKiIIQbzLPSUGpnhOOUrNLu();
		}

		public double GetButtonTimeUnpressed()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.VjMxVYmmCNVmznhtrNSsndosVDaH();
		}

		public bool GetNegativeButton()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.tfsYXedjKMNWjMtDSqWFOsogLsAD();
		}

		public bool GetNegativeButtonPrev()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.wSIRQBVAPRDurkpeGuQPDPRCpMgm();
		}

		public bool GetNegativeButtonDown()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.fFQjdmlZoEThlEPanCBBkaXvvEJo();
		}

		public bool GetNegativeButtonUp()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.qgryZPkzxqeyRJeMpPEmurhtTyMm();
		}

		public bool GetNegativeButtonSinglePressHold()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.pktQQZGkUCJMbigqGBIhecAFGgGE();
		}

		public bool GetNegativeButtonSinglePressDown()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.sgucTVHHhxjEnJtjublUmMwWtJBfA();
		}

		public bool GetNegativeButtonSinglePressUp()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.QWLgdtzoLZvnHwRmFgFnVtCtVUhB();
		}

		public bool GetNegativeButtonDoublePressDown()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.peodrpJyqFiCtWcUorQbtauWgAUY();
		}

		public bool GetNegativeButtonDoublePressDown(float speed)
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.peodrpJyqFiCtWcUorQbtauWgAUY(speed);
		}

		public bool GetNegativeButtonDoublePressHold()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.tKDOPYeIRAdrsVuNVTLireaOVYvj();
		}

		public bool GetNegativeButtonDoublePressHold(float speed)
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.tKDOPYeIRAdrsVuNVTLireaOVYvj(speed);
		}

		public bool GetNegativeButtonDoublePressUp()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.TrVLrbWLZvvdCMVlqWvqIVGIcku();
		}

		public bool GetNegativeButtonDoublePressUp(float speed)
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.TrVLrbWLZvvdCMVlqWvqIVGIcku(speed);
		}

		public bool GetNegativeButtonTimedPress(float time)
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.GhKhyORygEOxoFyUKamdTHvQxDRV(time, 0f);
		}

		public bool GetNegativeButtonTimedPress(float time, float expireIn)
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.GhKhyORygEOxoFyUKamdTHvQxDRV(time, expireIn);
		}

		public bool GetNegativeButtonTimedPressDown(float time)
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.pPEOmNsceKoOFIoTUUbFjaYcwZYp(time);
		}

		public bool GetNegativeButtonTimedPressUp(float time)
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.gRUNqPKPvraajISMnBLVgdoGjsqrB(time, 0f);
		}

		public bool GetNegativeButtonTimedPressUp(float time, float expireIn)
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.gRUNqPKPvraajISMnBLVgdoGjsqrB(time, expireIn);
		}

		public bool GetNegativeButtonShortPress()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.mQtESGAbvwzPFvKThNaXpGIagmKLA();
		}

		public bool GetNegativeButtonShortPressDown()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.RktaJYAilGppSOFeBEUKOdSeLPwzA();
		}

		public bool GetNegativeButtonShortPressUp()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.KuWEOnDveqvaooBzvBmdfTbetdXdc();
		}

		public bool GetNegativeButtonLongPress()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.HLDzNTMoheZQgjfpQBATellsFfgqA();
		}

		public bool GetNegativeButtonLongPressDown()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.AlvJRiQsiKqGaNlGbeltZXqUcmwfA();
		}

		public bool GetNegativeButtonLongPressUp()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.GoykAvFiGOAIADROsaULUBtZXKcs();
		}

		public bool GetNegativeButtonRepeating()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.evIwcwjhrvJGlfsVJPsBlQLBWKYB();
		}

		public double GetNegativeButtonTimePressed()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.CiBhHCokLNjDAjanXoNoqsoBEHkoA();
		}

		public double GetNegativeButtonTimeUnpressed()
		{
			return ztqcsjQogOGkbcfdohylerMDCirk.SUemUDmTbwTIOHBqIdxZnjNyZWjV();
		}

		public IList<InputActionSourceData> GetCurrentInputSources()
		{
			return ReInput.VmqcbbbvPImXBEBcMHWjUAXVfrUSA.IULWFKQGEhckhDtQiOajvpmQfmsO(playerId, actionId, true)?.tCIwCxveiapxaultMdrGFkMLGgofA();
		}

		public bool IsCurrentInputSource(ControllerType controllerType)
		{
			return ReInput.VmqcbbbvPImXBEBcMHWjUAXVfrUSA.IULWFKQGEhckhDtQiOajvpmQfmsO(playerId, actionId, true)?.LbgOGEMDzqxvZFsVjWqmghSWgqiA(controllerType) ?? false;
		}

		public bool IsCurrentInputSource(ControllerType controllerType, int controllerId)
		{
			return ReInput.VmqcbbbvPImXBEBcMHWjUAXVfrUSA.IULWFKQGEhckhDtQiOajvpmQfmsO(playerId, actionId, true)?.LbgOGEMDzqxvZFsVjWqmghSWgqiA(controllerType, controllerId) ?? false;
		}

		public bool IsCurrentInputSource(Controller controller)
		{
			return ReInput.VmqcbbbvPImXBEBcMHWjUAXVfrUSA.IULWFKQGEhckhDtQiOajvpmQfmsO(playerId, actionId, true)?.LbgOGEMDzqxvZFsVjWqmghSWgqiA(controller) ?? false;
		}

		internal InputActionEventData(ItaEjuKtoATtafYloiocFOanqPQc P_0, int P_1, int P_2, UpdateLoopType P_3)
		{
			vMNdyFbeFKZgkjkoYtuNxpwHfmqv = InputActionEventType.Update;
			ztqcsjQogOGkbcfdohylerMDCirk = P_0;
			playerId = P_1;
			actionId = P_2;
			updateLoop = P_3;
		}
	}
}
