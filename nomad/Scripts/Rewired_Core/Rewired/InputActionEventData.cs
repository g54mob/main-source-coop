using System.Collections.Generic;

namespace Rewired
{
	public struct InputActionEventData
	{
		private kBOilrfmQspwwsLlQucgVePHzaAKA XFdfHrePuRSLvpeFpIhQdVggjjnLb;

		private InputActionEventType zqTZvIykDOhAvqOclxLoKZoLJxwF;

		public readonly int playerId;

		public readonly int actionId;

		public readonly UpdateLoopType updateLoop;

		public InputActionEventType eventType
		{
			get
			{
				return zqTZvIykDOhAvqOclxLoKZoLJxwF;
			}
			internal set
			{
				zqTZvIykDOhAvqOclxLoKZoLJxwF = inputActionEventType;
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
				return ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.xZfXTcilMeUxJbAzlmlraJwZALAIA(actionId).name;
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
				return ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.xZfXTcilMeUxJbAzlmlraJwZALAIA(actionId).descriptiveName;
			}
		}

		public float GetAxis()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.RvRTgrdjWTZRmjoKbsOcGREaiXDi();
		}

		public float GetAxisPrev()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.lywVsHFwvRHDyAOYaAHUvBjHQYpD();
		}

		public float GetAxisDelta()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.BACwvsUbbeKukgnoPhsIIDwRnnqEA();
		}

		public double GetAxisTimeActive()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.SrhbCmEKNSsPfzfldsYwwlVRjIPw();
		}

		public double GetAxisTimeInactive()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.YRlmdmsBYvPRMgsSPGzadiYsBgND();
		}

		public float GetAxisRaw()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.IWvqBQApuMVSUeHFsYcOZGosazGeA();
		}

		public float GetAxisRawDelta()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.EcPdvFDjVKZgYaUueGlcfyniybkMA();
		}

		public float GetAxisRawPrev()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.RfXcmkCBFuBWUxLdultQzbgNZpmN();
		}

		public double GetAxisRawTimeActive()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.LIGFjSCuufpsUrrlkvEjDRcJaftQA();
		}

		public double GetAxisRawTimeInactive()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.ymaDYEBuDUjLBukOIukrtcbLmuCS();
		}

		public AxisCoordinateMode GetAxisCoordinateMode()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.SWVAbFKGBmrkqcRaBxMpGpAuomJu();
		}

		public AxisCoordinateMode GetAxisCoordinateModePrev()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.MsrrdOZikLJjhWrbptjAYjOtAZQR();
		}

		public AxisCoordinateMode GetAxisRawCoordinateMode()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.mNVutZbAuNuIbUVTXQAfvbUOtHuT();
		}

		public AxisCoordinateMode GetAxisRawCoordinateModePrev()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.aFCUOxEQfRrVLIItkuCjbBMJxSIX();
		}

		public bool GetButton()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.BRgJAcEyxDdRJtEmfVewtjEoYNqt();
		}

		public bool GetButtonPrev()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.ooKSDMuVHpVciXXBbIhumspcSpRM();
		}

		public bool GetButtonDown()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.ulNzhMMaXtAXOxcBFAnWyCDHeqoN();
		}

		public bool GetButtonUp()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.aOLFHKiGReYtLuVqzyyHLHbfKQYab();
		}

		public bool GetButtonSinglePressHold()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.ZbWxWKzrywKLPNgJCRBudWaWVnQo();
		}

		public bool GetButtonSinglePressDown()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.DzHscFSLxdVERLjLMZVUWfQUTaSF();
		}

		public bool GetButtonSinglePressUp()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.LZPEdseAeYHhrEFJMyIMBhfZCwvn();
		}

		public bool GetButtonDoublePressDown()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.TXVeAbCoUNfbogahcWnAAZQOcKFC();
		}

		public bool GetButtonDoublePressDown(float speed)
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.tchZIqxqMcltzqTiihcJYKwuaAei(speed);
		}

		public bool GetButtonDoublePressHold()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.yXfTvqgnWyKHSZrqYMnhrgHjLOKd();
		}

		public bool GetButtonDoublePressHold(float speed)
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.eSRbSlRIiNAqSZHieyTWJIkiWXff(speed);
		}

		public bool GetButtonDoublePressUp()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.TWcJzzurtMosSXdBkPtobsUtrwOT();
		}

		public bool GetButtonDoublePressUp(float speed)
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.VeFehoBAikgOZbeXFHZCDKEIPZGKC(speed);
		}

		public bool GetButtonTimedPress(float time)
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.jTXtRIsJnPihrseaUxpjqKnCOoPC(time, 0f);
		}

		public bool GetButtonTimedPress(float time, float expireIn)
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.jTXtRIsJnPihrseaUxpjqKnCOoPC(time, expireIn);
		}

		public bool GetButtonTimedPressDown(float time)
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.kBBCZOnptttxaQSaLliSLAyVIslL(time);
		}

		public bool GetButtonTimedPressUp(float time)
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.RCOCLdDZYLOfkVVZPPcNmYplMPqF(time, 0f);
		}

		public bool GetButtonTimedPressUp(float time, float expireIn)
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.RCOCLdDZYLOfkVVZPPcNmYplMPqF(time, expireIn);
		}

		public bool GetButtonShortPress()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.FIsgvKBIeQboRaWNaSyVaektTMzzA();
		}

		public bool GetButtonShortPressDown()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.JNUXlbFKdcnOumpOjdJBlERkDRZCA();
		}

		public bool GetButtonShortPressUp()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.lekaYDUvsTYCOiLPOfTfzDVLfDuc();
		}

		public bool GetButtonLongPress()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.UsNLWxwEABEtvLvDfRUDvXVPnCdx();
		}

		public bool GetButtonLongPressDown()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.gOpFTmMabCeWBnCmcrsVKBMyxMQF();
		}

		public bool GetButtonLongPressUp()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.jWCwyVMAyGNFusPDQDolSrygEocX();
		}

		public bool GetButtonRepeating()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.luHCfXeDrbtNBOhpwjjGImBAgXznA();
		}

		public double GetButtonTimePressed()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.eQYIpHgMDFhqqRZhPeGNYPzxXVnc();
		}

		public double GetButtonTimeUnpressed()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.DEkfVfoTkufJnSXSodctvAYanAfg();
		}

		public bool GetNegativeButton()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.VRiBjNoimKLqMJcOigBnTGHHrHub();
		}

		public bool GetNegativeButtonPrev()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.tnXlVtxyaxIfLqFVXAJLDtiGTaUf();
		}

		public bool GetNegativeButtonDown()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.VYSikZzuXqPelkspEaGEPYtFZADJ();
		}

		public bool GetNegativeButtonUp()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.KOICBttxgehAkePueGwrcVfAlxWCb();
		}

		public bool GetNegativeButtonSinglePressHold()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.ofLgjxbAiLFczqfdeXZQnDffqjaAb();
		}

		public bool GetNegativeButtonSinglePressDown()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.WuHTXaSrrQTzEyeAyWMAMFzZWqGR();
		}

		public bool GetNegativeButtonSinglePressUp()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.SyxgdBgGAcWsYgijIiZkjXGkreFJA();
		}

		public bool GetNegativeButtonDoublePressDown()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.TYkTxmordTLGaeDPwalTtOPlLjBA();
		}

		public bool GetNegativeButtonDoublePressDown(float speed)
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.ZnwZnJXiZrPcerRlvjBWaMetofsY(speed);
		}

		public bool GetNegativeButtonDoublePressHold()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.uytoRKSwroIPsfhJUBhjkoEEjojV();
		}

		public bool GetNegativeButtonDoublePressHold(float speed)
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.VosydDNNmgXAqxlxaeIqKRirlQgdA(speed);
		}

		public bool GetNegativeButtonDoublePressUp()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.yFeFaIQPjwnfNpSYdbbEyAOBhMqv();
		}

		public bool GetNegativeButtonDoublePressUp(float speed)
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.xehengRSBDHzLfImHDBStcbEAmlIb(speed);
		}

		public bool GetNegativeButtonTimedPress(float time)
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.DpGjKMVCONaAXZmoEjjceWwJoSVj(time, 0f);
		}

		public bool GetNegativeButtonTimedPress(float time, float expireIn)
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.DpGjKMVCONaAXZmoEjjceWwJoSVj(time, expireIn);
		}

		public bool GetNegativeButtonTimedPressDown(float time)
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.kEjagsDgZituqfuLKPjZGOHbrQSIA(time);
		}

		public bool GetNegativeButtonTimedPressUp(float time)
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.LVqGPOKOkUJddoPzWyZInGbDACPXA(time, 0f);
		}

		public bool GetNegativeButtonTimedPressUp(float time, float expireIn)
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.LVqGPOKOkUJddoPzWyZInGbDACPXA(time, expireIn);
		}

		public bool GetNegativeButtonShortPress()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.oMTJrgKTbTdWHoFmJuSgxgShPrm();
		}

		public bool GetNegativeButtonShortPressDown()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.kslciTTidvoMyIZgtSTdFtuZsUoP();
		}

		public bool GetNegativeButtonShortPressUp()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.LDMsGESyNgOlMulQqSikAlFjYnii();
		}

		public bool GetNegativeButtonLongPress()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.QnupUpitKImkVGNVIPtpBxzUcDXf();
		}

		public bool GetNegativeButtonLongPressDown()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.shokBLMVDXLmddfYxVRvxaLiVrib();
		}

		public bool GetNegativeButtonLongPressUp()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.BMXFPjDOFTDTTfHBGMkTmqmNPqEX();
		}

		public bool GetNegativeButtonRepeating()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.uakFyBehSWuTSJarHLAsQDuocrmd();
		}

		public double GetNegativeButtonTimePressed()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.uKZGxIdxHqFkqZipNXnmUkllIwSn();
		}

		public double GetNegativeButtonTimeUnpressed()
		{
			return XFdfHrePuRSLvpeFpIhQdVggjjnLb.OlOkRsACaykIHFyuncCFeVAojcvN();
		}

		public IList<InputActionSourceData> GetCurrentInputSources()
		{
			return ReInput.FoarDfUMCtoVFquEtrllUhEjZUUn.FfgVHeyzXYOBalgpoIeyNyHpAHaO(playerId, actionId, true)?.hlzGvsjjwSlGrgcCDkJICfVVspVf();
		}

		public bool IsCurrentInputSource(ControllerType controllerType)
		{
			return ReInput.FoarDfUMCtoVFquEtrllUhEjZUUn.FfgVHeyzXYOBalgpoIeyNyHpAHaO(playerId, actionId, true)?.eGhJyWdvqDhIKIDlJfQLHrEKWHdq(controllerType) ?? false;
		}

		public bool IsCurrentInputSource(ControllerType controllerType, int controllerId)
		{
			return ReInput.FoarDfUMCtoVFquEtrllUhEjZUUn.FfgVHeyzXYOBalgpoIeyNyHpAHaO(playerId, actionId, true)?.iiraDRzsYkgmogHACIatHsoAhRUKc(controllerType, controllerId) ?? false;
		}

		public bool IsCurrentInputSource(Controller controller)
		{
			return ReInput.FoarDfUMCtoVFquEtrllUhEjZUUn.FfgVHeyzXYOBalgpoIeyNyHpAHaO(playerId, actionId, true)?.xHIjoijXjTBHMhIexUzlnrIsLruq(controller) ?? false;
		}

		internal InputActionEventData(kBOilrfmQspwwsLlQucgVePHzaAKA P_0, int P_1, int P_2, UpdateLoopType P_3)
		{
			zqTZvIykDOhAvqOclxLoKZoLJxwF = InputActionEventType.Update;
			XFdfHrePuRSLvpeFpIhQdVggjjnLb = P_0;
			playerId = P_1;
			actionId = P_2;
			updateLoop = P_3;
		}
	}
}
