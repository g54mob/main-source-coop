using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rewired;
using Rewired.Data;
using Rewired.Data.Mapping;
using Rewired.Interfaces;
using Rewired.Internal.Localization;
using Rewired.Utils;

internal class teyNHYFqhXTPcVcPPjgZwrLLedhK : PlatformInputManager
{
	private class ZLNaniVinNHUoanvGBczlsNkFqSi : IInputManagerJoystick, IInputManagerJoystickPublic, IDisposable
	{
		private int JBZQyInHLYdWEJGXqFgpsSChnwHR;

		private int FNTPYdzWtAKBadZblICAMNvrrTju;

		public Guid BnqBmbkUjfRnQqIVVtvPyJXxVURS;

		public string YWBrKErSwYZNBpoQHCAIFAMzHvVf;

		public MeGehmGvtoXRlfGQhxxMoBPtYUNiA LGYOCvtIlreYITJNoUEcEWhVeuws;

		public string VSwXSUgqUOIAbYQKKiaGMHdlLKyq;

		public string stXRfKdMISKtaFdVjcUSAFGRBWiPA;

		public Guid TIwztxvGbEHynbFiHqnBSFYFFOpV;

		public PidVid aeYjfBwZssytKWfWYfLmmhGPaLZd;

		public Guid OURdNuLMAeqlIwzaCgvlTDelGiMiA;

		public int IjUwbQhBbRGnsINjrOWqqweBcOvN;

		public int OAUbbUzMskEygGpaCpCWtwijGcyd;

		public int cXlBJTkbOxyajoGpCknJwjopNRmeb;

		public int NLiSgdFFwZFHeJvBQAOLBVhHHoAjA;

		public int pKUdfhHvJcBJLUeLyZLLQcdXpmgHb;

		public int GYAjEKPjdQeVCHacbysCqIGRJOBJb;

		public bool WEQAwYHdctwEnWTALXBoJdvcNOIZA;

		public int LJEAqyhFaEJTmocOcvwjtdjAwJEA;

		private float[] QXhltQmjnXWVlzJAGgkWiarfDGMI;

		private float[] GEVMAPHBtfAXzeJtOIidUHqnpfKpA;

		private bool[] fYspASseBTpVDWeMexCXZrYJHfBq;

		private HardwareJoystickMap_InputManager lnQijvSOzkywNLNKGQtfLoeTzEDJ;

		private Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> UTAXlTjhsSvXNqhYfdjfclJBlElV;

		private bool UUebtwLfpXXbpQoOnZxkutpCGvGI;

		private bool umTKnbGkcjNaxLgYzUrjOgjiOZjC;

		[CompilerGenerated]
		private Controller.Extension MmbxcgRSuxCDDCWielHkPehRkpBe;

		private bool UoJWVrILSoivXGTqjmcQbYZaUnlE;

		[CustomObfuscation(rename = false)]
		int IInputManagerJoystickPublic.rewiredId
		{
			get
			{
				return JBZQyInHLYdWEJGXqFgpsSChnwHR;
			}
			set
			{
				JBZQyInHLYdWEJGXqFgpsSChnwHR = value;
			}
		}

		[CustomObfuscation(rename = false)]
		int IInputManagerJoystickPublic.inputManagerId
		{
			get
			{
				return FNTPYdzWtAKBadZblICAMNvrrTju;
			}
			set
			{
				FNTPYdzWtAKBadZblICAMNvrrTju = value;
			}
		}

		[CustomObfuscation(rename = false)]
		string IInputManagerJoystickPublic.name
		{
			get
			{
				if (!(YWBrKErSwYZNBpoQHCAIFAMzHvVf != "Unknown Controller"))
				{
					return stXRfKdMISKtaFdVjcUSAFGRBWiPA;
				}
				return YWBrKErSwYZNBpoQHCAIFAMzHvVf;
			}
		}

		[CustomObfuscation(rename = false)]
		long? IInputManagerJoystickPublic.systemId
		{
			get
			{
				if (FNTPYdzWtAKBadZblICAMNvrrTju < 0)
				{
					return null;
				}
				return FNTPYdzWtAKBadZblICAMNvrrTju;
			}
		}

		[CustomObfuscation(rename = false)]
		int IInputManagerJoystickPublic.unityId => 0;

		[CustomObfuscation(rename = false)]
		Guid IInputManagerJoystickPublic.instanceGuid => TIwztxvGbEHynbFiHqnBSFYFFOpV;

		[CustomObfuscation(rename = false)]
		Guid IInputManagerJoystickPublic.persistentGuid
		{
			get
			{
				if (LGYOCvtIlreYITJNoUEcEWhVeuws == null)
				{
					return Guid.Empty;
				}
				return LGYOCvtIlreYITJNoUEcEWhVeuws.kqSlvVQBuUEuOgJaIZoCNgjIlQCQA;
			}
		}

		[CustomObfuscation(rename = false)]
		Controller.Extension IInputManagerJoystickPublic.extension
		{
			[CompilerGenerated]
			get
			{
				return MmbxcgRSuxCDDCWielHkPehRkpBe;
			}
			[CompilerGenerated]
			set
			{
				MmbxcgRSuxCDDCWielHkPehRkpBe = value;
			}
		}

		[CustomObfuscation(rename = false)]
		public void SetVibration(float amount, int motorIndex)
		{
			if (WEQAwYHdctwEnWTALXBoJdvcNOIZA)
			{
				LGYOCvtIlreYITJNoUEcEWhVeuws.mCCcIQdkkIFaoaCorqUsHTdEOsFA(motorIndex, amount, false);
			}
		}

		void IInputManagerJoystickPublic.SetVibration(float amount, int motorIndex)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetVibration
			this.SetVibration(amount, motorIndex);
		}

		[CustomObfuscation(rename = false)]
		public void StopVibration()
		{
			if (WEQAwYHdctwEnWTALXBoJdvcNOIZA)
			{
				LGYOCvtIlreYITJNoUEcEWhVeuws.eKSntAigxVnvZQvAnPMXerCNFucS();
			}
		}

		void IInputManagerJoystickPublic.StopVibration()
		{
			//ILSpy generated this explicit interface implementation from .override directive in StopVibration
			this.StopVibration();
		}

		public ZLNaniVinNHUoanvGBczlsNkFqSi(Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> P_0)
		{
			UTAXlTjhsSvXNqhYfdjfclJBlElV = P_0;
			FNTPYdzWtAKBadZblICAMNvrrTju = -1;
			JBZQyInHLYdWEJGXqFgpsSChnwHR = -1;
		}

		public void comWoeDEyhTBLjggcEJFCSAkczzZ()
		{
			OURdNuLMAeqlIwzaCgvlTDelGiMiA = MiscTools.CreateGuidHashSHA1(stXRfKdMISKtaFdVjcUSAFGRBWiPA + aeYjfBwZssytKWfWYfLmmhGPaLZd.ToProductGuid().ToString());
			OAUbbUzMskEygGpaCpCWtwijGcyd = NLiSgdFFwZFHeJvBQAOLBVhHHoAjA;
			cXlBJTkbOxyajoGpCknJwjopNRmeb = pKUdfhHvJcBJLUeLyZLLQcdXpmgHb + GYAjEKPjdQeVCHacbysCqIGRJOBJb * 8;
			LVAUhmvytjsDDEXhsjCiLjxHKbUx();
			BnqBmbkUjfRnQqIVVtvPyJXxVURS = lnQijvSOzkywNLNKGQtfLoeTzEDJ.hardwareMapIdentifier.guid;
			YWBrKErSwYZNBpoQHCAIFAMzHvVf = lnQijvSOzkywNLNKGQtfLoeTzEDJ.controllerName;
			UUebtwLfpXXbpQoOnZxkutpCGvGI = BnqBmbkUjfRnQqIVVtvPyJXxVURS == Guid.Empty;
			QXhltQmjnXWVlzJAGgkWiarfDGMI = new float[OAUbbUzMskEygGpaCpCWtwijGcyd];
			GEVMAPHBtfAXzeJtOIidUHqnpfKpA = new float[cXlBJTkbOxyajoGpCknJwjopNRmeb];
			fYspASseBTpVDWeMexCXZrYJHfBq = new bool[cXlBJTkbOxyajoGpCknJwjopNRmeb];
			if (cXlBJTkbOxyajoGpCknJwjopNRmeb > 0)
			{
				HardwareJoystickMap.Platform_WindowsWGI_Base.Button[] buttons_orig = ((HardwareJoystickMap.Platform_WindowsWGI_Base)lnQijvSOzkywNLNKGQtfLoeTzEDJ.map).Buttons_orig;
				if (buttons_orig != null)
				{
					for (int i = 0; i < buttons_orig.Length; i++)
					{
						fYspASseBTpVDWeMexCXZrYJHfBq[i] = buttons_orig[i].buttonInfo.isPressureSensitive;
					}
				}
			}
			Update();
		}

		public void rftNTEiaqRcMovSDtsigYYVOwfqj(ZLNaniVinNHUoanvGBczlsNkFqSi P_0)
		{
			if (P_0 != null)
			{
				FNTPYdzWtAKBadZblICAMNvrrTju = P_0.FNTPYdzWtAKBadZblICAMNvrrTju;
				JBZQyInHLYdWEJGXqFgpsSChnwHR = P_0.JBZQyInHLYdWEJGXqFgpsSChnwHR;
				for (int i = 0; i < MathTools.Min(GEVMAPHBtfAXzeJtOIidUHqnpfKpA.Length, P_0.GEVMAPHBtfAXzeJtOIidUHqnpfKpA.Length); i++)
				{
					GEVMAPHBtfAXzeJtOIidUHqnpfKpA[i] = P_0.GEVMAPHBtfAXzeJtOIidUHqnpfKpA[i];
				}
				for (int j = 0; j < MathTools.Min(fYspASseBTpVDWeMexCXZrYJHfBq.Length, P_0.fYspASseBTpVDWeMexCXZrYJHfBq.Length); j++)
				{
					fYspASseBTpVDWeMexCXZrYJHfBq[j] = P_0.fYspASseBTpVDWeMexCXZrYJHfBq[j];
				}
				for (int k = 0; k < MathTools.Min(QXhltQmjnXWVlzJAGgkWiarfDGMI.Length, P_0.QXhltQmjnXWVlzJAGgkWiarfDGMI.Length); k++)
				{
					QXhltQmjnXWVlzJAGgkWiarfDGMI[k] = P_0.QXhltQmjnXWVlzJAGgkWiarfDGMI[k];
				}
				umTKnbGkcjNaxLgYzUrjOgjiOZjC = P_0.umTKnbGkcjNaxLgYzUrjOgjiOZjC;
			}
		}

		[CustomObfuscation(rename = false)]
		public void Update()
		{
			torzCEqKSkAUUBuHYjOsAUYOhGfi();
			lhdcLuHDBRVUBciVQaZDGGRFNCJq();
		}

		void IInputManagerJoystick.Update()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Update
			this.Update();
		}

		[CustomObfuscation(rename = false)]
		public void FillData(ControllerDataUpdater dataUpdater)
		{
			if (OAUbbUzMskEygGpaCpCWtwijGcyd != dataUpdater.axisCount || cXlBJTkbOxyajoGpCknJwjopNRmeb != dataUpdater.buttonCount)
			{
				throw new Exception("This controller signature does not match the data object!");
			}
			for (int i = 0; i < OAUbbUzMskEygGpaCpCWtwijGcyd; i++)
			{
				dataUpdater.axisValues[i] = QXhltQmjnXWVlzJAGgkWiarfDGMI[i];
			}
			for (int j = 0; j < cXlBJTkbOxyajoGpCknJwjopNRmeb; j++)
			{
				if (fYspASseBTpVDWeMexCXZrYJHfBq[j])
				{
					dataUpdater.buttonPressureValues[j] = GEVMAPHBtfAXzeJtOIidUHqnpfKpA[j];
				}
				else
				{
					dataUpdater.buttonValues[j] = GEVMAPHBtfAXzeJtOIidUHqnpfKpA[j] > 0f;
				}
			}
			if (umTKnbGkcjNaxLgYzUrjOgjiOZjC && !dataUpdater.hasReceivedInput)
			{
				dataUpdater.hasReceivedInput = true;
			}
		}

		void IInputManagerJoystick.FillData(ControllerDataUpdater dataUpdater)
		{
			//ILSpy generated this explicit interface implementation from .override directive in FillData
			this.FillData(dataUpdater);
		}

		public int jEWApEeFNXdKSxTjCmxQdtzBvatNA(ZLNaniVinNHUoanvGBczlsNkFqSi P_0)
		{
			if (P_0.JBZQyInHLYdWEJGXqFgpsSChnwHR == JBZQyInHLYdWEJGXqFgpsSChnwHR)
			{
				return 2;
			}
			if (NLiSgdFFwZFHeJvBQAOLBVhHHoAjA != P_0.NLiSgdFFwZFHeJvBQAOLBVhHHoAjA)
			{
				return 0;
			}
			if (pKUdfhHvJcBJLUeLyZLLQcdXpmgHb != P_0.pKUdfhHvJcBJLUeLyZLLQcdXpmgHb)
			{
				return 0;
			}
			if (GYAjEKPjdQeVCHacbysCqIGRJOBJb != P_0.GYAjEKPjdQeVCHacbysCqIGRJOBJb)
			{
				return 0;
			}
			if (P_0.TIwztxvGbEHynbFiHqnBSFYFFOpV == TIwztxvGbEHynbFiHqnBSFYFFOpV)
			{
				return 2;
			}
			if (P_0.OURdNuLMAeqlIwzaCgvlTDelGiMiA == OURdNuLMAeqlIwzaCgvlTDelGiMiA)
			{
				return 1;
			}
			return 0;
		}

		private BridgedControllerHWInfo mufIiaHdTJGXPWzCSZjRXxLbrhPE()
		{
			BridgedControllerHWInfo bridgedControllerHWInfo = new BridgedControllerHWInfo();
			wgxeDZEqXljrqaLNlcpvFldLowNu(bridgedControllerHWInfo);
			return bridgedControllerHWInfo;
		}

		[CustomObfuscation(rename = false)]
		public BridgedController ToBridgedController()
		{
			BridgedController bridgedController = new BridgedController();
			NyPAcHhiHpEPQluFyYvAxnkmBEXbA(bridgedController);
			return bridgedController;
		}

		BridgedController IInputManagerJoystick.ToBridgedController()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ToBridgedController
			return this.ToBridgedController();
		}

		[CustomObfuscation(rename = false)]
		public ControllerDisconnectedEventArgs ToControllerDisconnectedEventArgs()
		{
			return new ControllerDisconnectedEventArgs(JBZQyInHLYdWEJGXqFgpsSChnwHR);
		}

		ControllerDisconnectedEventArgs IInputManagerJoystick.ToControllerDisconnectedEventArgs()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ToControllerDisconnectedEventArgs
			return this.ToControllerDisconnectedEventArgs();
		}

		private void torzCEqKSkAUUBuHYjOsAUYOhGfi()
		{
			if (OAUbbUzMskEygGpaCpCWtwijGcyd <= 0)
			{
				return;
			}
			HardwareJoystickMap.Platform_WindowsWGI_Base.Axis[] axes_orig = ((HardwareJoystickMap.Platform_WindowsWGI_Base)lnQijvSOzkywNLNKGQtfLoeTzEDJ.map).Axes_orig;
			if (axes_orig != null)
			{
				for (int i = 0; i < axes_orig.Length; i++)
				{
					rsKWqivVCKdkECsDGJXpzFdBSMZb(axes_orig[i], i);
				}
			}
		}

		private void lhdcLuHDBRVUBciVQaZDGGRFNCJq()
		{
			if (cXlBJTkbOxyajoGpCknJwjopNRmeb <= 0)
			{
				return;
			}
			HardwareJoystickMap.Platform_WindowsWGI_Base.Button[] buttons_orig = ((HardwareJoystickMap.Platform_WindowsWGI_Base)lnQijvSOzkywNLNKGQtfLoeTzEDJ.map).Buttons_orig;
			if (buttons_orig != null)
			{
				for (int i = 0; i < buttons_orig.Length; i++)
				{
					mwXQjoQofyKzWHodSSYGseQrMHgx(buttons_orig[i], i);
				}
			}
		}

		private void rsKWqivVCKdkECsDGJXpzFdBSMZb(HardwareJoystickMap.Platform_WindowsWGI_Base.Axis P_0, int P_1)
		{
			if (P_1 >= OAUbbUzMskEygGpaCpCWtwijGcyd)
			{
				throw new Exception("Number of axes in hardware map does not match number of axes found in controller!");
			}
			QXhltQmjnXWVlzJAGgkWiarfDGMI[P_1] = wrMKhxEWRrugtyPHdJKzayvxjPmq(P_0);
			if (!umTKnbGkcjNaxLgYzUrjOgjiOZjC && QXhltQmjnXWVlzJAGgkWiarfDGMI[P_1] != 0f)
			{
				umTKnbGkcjNaxLgYzUrjOgjiOZjC = true;
			}
		}

		private void mwXQjoQofyKzWHodSSYGseQrMHgx(HardwareJoystickMap.Platform_WindowsWGI_Base.Button P_0, int P_1)
		{
			if (P_1 >= cXlBJTkbOxyajoGpCknJwjopNRmeb)
			{
				throw new Exception("Number of buttons in hardware map does not match number of buttons found in controller!");
			}
			GEVMAPHBtfAXzeJtOIidUHqnpfKpA[P_1] = oAYtjVAIDJHsWljUdWgXqwMOUrYM(P_0);
			if (!umTKnbGkcjNaxLgYzUrjOgjiOZjC && GEVMAPHBtfAXzeJtOIidUHqnpfKpA[P_1] != 0f)
			{
				umTKnbGkcjNaxLgYzUrjOgjiOZjC = true;
			}
		}

		private float wrMKhxEWRrugtyPHdJKzayvxjPmq(HardwareJoystickMap.Platform_WindowsWGI_Base.Axis P_0)
		{
			if (P_0.sourceType == 1)
			{
				int sourceAxis = P_0.sourceAxis;
				if (sourceAxis < 0)
				{
					return 0f;
				}
				return JPAgoElLymHXOkdJiBsFhmGyavqV(sourceAxis);
			}
			if (P_0.sourceType == 0)
			{
				int sourceButton = P_0.sourceButton;
				if (sourceButton < 0 || sourceButton >= pKUdfhHvJcBJLUeLyZLLQcdXpmgHb || sourceButton >= 256)
				{
					return 0f;
				}
				if (!LGYOCvtIlreYITJNoUEcEWhVeuws.oCSoDcyeqHztfDFCcTQTjBGrTFql(sourceButton))
				{
					return 0f;
				}
				if (P_0.buttonAxisContribution == Pole.Positive)
				{
					return 1f;
				}
				return -1f;
			}
			if (P_0.sourceType == 2)
			{
				int sourceHat = P_0.sourceHat;
				if (sourceHat < 0 || sourceHat >= GYAjEKPjdQeVCHacbysCqIGRJOBJb || sourceHat >= 4)
				{
					return 0f;
				}
				int num = LGYOCvtIlreYITJNoUEcEWhVeuws.zAfMjvTuCJjZvtPMIXoJeIDIkYbU(sourceHat);
				if (num < 0)
				{
					return 0f;
				}
				float num2;
				if (P_0.sourceHatDirection == AxisDirection.Horizontal)
				{
					num2 = gLBiEbSHsGUaxpAnHhlTJMUpZHctA(num, AxisDirection.Horizontal);
					if (P_0.sourceHatRange != AxisRange.Full)
					{
						if (P_0.sourceHatRange == AxisRange.Positive)
						{
							if (num2 < 0f)
							{
								return 0f;
							}
						}
						else if (num2 > 0f)
						{
							return 0f;
						}
					}
				}
				else
				{
					num2 = gLBiEbSHsGUaxpAnHhlTJMUpZHctA(num, AxisDirection.Vertical);
					if (P_0.sourceHatRange != AxisRange.Full)
					{
						if (P_0.sourceHatRange == AxisRange.Positive)
						{
							if (num2 < 0f)
							{
								return 0f;
							}
						}
						else if (num2 > 0f)
						{
							return 0f;
						}
					}
				}
				if (P_0.invert)
				{
					num2 *= -1f;
				}
				return num2;
			}
			return 0f;
		}

		private float JPAgoElLymHXOkdJiBsFhmGyavqV(int P_0)
		{
			if (P_0 < 0 || P_0 >= LGYOCvtIlreYITJNoUEcEWhVeuws.vKsCDMOVQygjIFNoTKLMflcGtQGCA)
			{
				return 0f;
			}
			return LGYOCvtIlreYITJNoUEcEWhVeuws.QypnoSbCUJopcyAwXisguysIQQfU(P_0);
		}

		private float oAYtjVAIDJHsWljUdWgXqwMOUrYM(HardwareJoystickMap.Platform_WindowsWGI_Base.Button P_0)
		{
			if (P_0.sourceType == 0)
			{
				if (P_0.ignoreIfButtonsActive)
				{
					for (int i = 0; i < P_0.ignoreIfButtonsActiveButtons.Length; i++)
					{
						if (LGYOCvtIlreYITJNoUEcEWhVeuws.oCSoDcyeqHztfDFCcTQTjBGrTFql(P_0.ignoreIfButtonsActiveButtons[i]))
						{
							return 0f;
						}
					}
				}
				if (P_0.requireMultipleButtons)
				{
					bool flag = false;
					for (int j = 0; j < P_0.requiredButtons.Length; j++)
					{
						if (!LGYOCvtIlreYITJNoUEcEWhVeuws.oCSoDcyeqHztfDFCcTQTjBGrTFql(P_0.requiredButtons[j]))
						{
							return 0f;
						}
						flag = true;
					}
					if (flag)
					{
						return 1f;
					}
					return 0f;
				}
				int sourceButton = P_0.sourceButton;
				if (sourceButton < 0 || sourceButton >= pKUdfhHvJcBJLUeLyZLLQcdXpmgHb || sourceButton >= 256)
				{
					return 0f;
				}
				if (!LGYOCvtIlreYITJNoUEcEWhVeuws.oCSoDcyeqHztfDFCcTQTjBGrTFql(sourceButton))
				{
					return 0f;
				}
				return 1f;
			}
			if (P_0.sourceType == 1)
			{
				int sourceAxis = P_0.sourceAxis;
				if (sourceAxis < 0)
				{
					return 0f;
				}
				float num = JPAgoElLymHXOkdJiBsFhmGyavqV(sourceAxis);
				float num2 = MathTools.Abs(num);
				if (num2 <= P_0.axisDeadZone)
				{
					return 0f;
				}
				if (P_0.sourceAxisPole == Pole.Positive)
				{
					if (num < 0f)
					{
						return 0f;
					}
				}
				else if (num > 0f)
				{
					return 0f;
				}
				return num2;
			}
			if (P_0.sourceType == 2)
			{
				int sourceHat = P_0.sourceHat;
				if (sourceHat < 0 || sourceHat >= GYAjEKPjdQeVCHacbysCqIGRJOBJb || sourceHat >= 4)
				{
					return 0f;
				}
				switch (P_0.sourceHatDirection)
				{
				case HatDirection.Up:
					return YrFaHiulheyHgaYVWgiSiBwfjDuU(LGYOCvtIlreYITJNoUEcEWhVeuws.zAfMjvTuCJjZvtPMIXoJeIDIkYbU(sourceHat), 0, P_0.sourceHatType);
				case HatDirection.UpRight:
					return YrFaHiulheyHgaYVWgiSiBwfjDuU(LGYOCvtIlreYITJNoUEcEWhVeuws.zAfMjvTuCJjZvtPMIXoJeIDIkYbU(sourceHat), 1, P_0.sourceHatType);
				case HatDirection.Right:
					return YrFaHiulheyHgaYVWgiSiBwfjDuU(LGYOCvtIlreYITJNoUEcEWhVeuws.zAfMjvTuCJjZvtPMIXoJeIDIkYbU(sourceHat), 2, P_0.sourceHatType);
				case HatDirection.DownRight:
					return YrFaHiulheyHgaYVWgiSiBwfjDuU(LGYOCvtIlreYITJNoUEcEWhVeuws.zAfMjvTuCJjZvtPMIXoJeIDIkYbU(sourceHat), 3, P_0.sourceHatType);
				case HatDirection.Down:
					return YrFaHiulheyHgaYVWgiSiBwfjDuU(LGYOCvtIlreYITJNoUEcEWhVeuws.zAfMjvTuCJjZvtPMIXoJeIDIkYbU(sourceHat), 4, P_0.sourceHatType);
				case HatDirection.DownLeft:
					return YrFaHiulheyHgaYVWgiSiBwfjDuU(LGYOCvtIlreYITJNoUEcEWhVeuws.zAfMjvTuCJjZvtPMIXoJeIDIkYbU(sourceHat), 5, P_0.sourceHatType);
				case HatDirection.Left:
					return YrFaHiulheyHgaYVWgiSiBwfjDuU(LGYOCvtIlreYITJNoUEcEWhVeuws.zAfMjvTuCJjZvtPMIXoJeIDIkYbU(sourceHat), 6, P_0.sourceHatType);
				case HatDirection.UpLeft:
					return YrFaHiulheyHgaYVWgiSiBwfjDuU(LGYOCvtIlreYITJNoUEcEWhVeuws.zAfMjvTuCJjZvtPMIXoJeIDIkYbU(sourceHat), 7, P_0.sourceHatType);
				}
			}
			return 0f;
		}

		private float YrFaHiulheyHgaYVWgiSiBwfjDuU(int P_0, int P_1, HatType P_2)
		{
			if (P_0 < 0)
			{
				return 0f;
			}
			if (lnQijvSOzkywNLNKGQtfLoeTzEDJ.isUnknownController && !InputTools.HandleForced4WayHatsOnUnknownControllers(P_1, ref P_2))
			{
				return 0f;
			}
			int num = 4500 * P_1;
			if (P_2 == HatType.EightWay && P_0 != num)
			{
				return 0f;
			}
			int num2;
			int num3;
			if (P_2 == HatType.EightWay)
			{
				num2 = 31500;
				num3 = 4500;
			}
			else
			{
				num2 = 27000;
				num3 = 9000;
			}
			if (P_1 == 0 && P_0 > num2)
			{
				P_0 -= 36000;
			}
			if (P_0 < num + num3 && P_0 > num - num3)
			{
				return 1f;
			}
			return 0f;
		}

		private float gLBiEbSHsGUaxpAnHhlTJMUpZHctA(int P_0, AxisDirection P_1)
		{
			if (P_0 < 0)
			{
				return 0f;
			}
			if (P_1 == AxisDirection.Vertical)
			{
				if (P_0 > 27000 || P_0 < 9000)
				{
					return 1f;
				}
				if (P_0 < 27000 && P_0 > 9000)
				{
					return -1f;
				}
				return 0f;
			}
			if (P_0 > 0 && P_0 < 18000)
			{
				return 1f;
			}
			if (P_0 > 18000)
			{
				return -1f;
			}
			return 0f;
		}

		private void LVAUhmvytjsDDEXhsjCiLjxHKbUx()
		{
			BridgedControllerHWInfo bridgedControllerHWInfo = mufIiaHdTJGXPWzCSZjRXxLbrhPE();
			lnQijvSOzkywNLNKGQtfLoeTzEDJ = UTAXlTjhsSvXNqhYfdjfclJBlElV(bridgedControllerHWInfo);
			bool flag = false;
			bool flag2 = false;
			if (lnQijvSOzkywNLNKGQtfLoeTzEDJ == null || lnQijvSOzkywNLNKGQtfLoeTzEDJ.hardwareMapIdentifier.guid == Consts.joystickGuid_unknownController)
			{
				if (LGYOCvtIlreYITJNoUEcEWhVeuws.eKcWuKRtMbUDcepLxeVbQUfLMiVv)
				{
					bridgedControllerHWInfo.hw_pidVid = new PidVid(4607, 10462);
					bridgedControllerHWInfo.hw_productId = bridgedControllerHWInfo.hw_pidVid.productId;
					bridgedControllerHWInfo.hw_vendorId = bridgedControllerHWInfo.hw_pidVid.vendorId;
					lnQijvSOzkywNLNKGQtfLoeTzEDJ = UTAXlTjhsSvXNqhYfdjfclJBlElV(bridgedControllerHWInfo);
					flag2 = true;
				}
				if (lnQijvSOzkywNLNKGQtfLoeTzEDJ == null || lnQijvSOzkywNLNKGQtfLoeTzEDJ.hardwareMapIdentifier.guid == Consts.joystickGuid_unknownController)
				{
					bridgedControllerHWInfo.hw_pidVid = new PidVid(736, 1118);
					bridgedControllerHWInfo.hw_productId = bridgedControllerHWInfo.hw_pidVid.productId;
					bridgedControllerHWInfo.hw_vendorId = bridgedControllerHWInfo.hw_pidVid.vendorId;
					bridgedControllerHWInfo.definitionMatchTag = string.Empty;
					lnQijvSOzkywNLNKGQtfLoeTzEDJ = UTAXlTjhsSvXNqhYfdjfclJBlElV(bridgedControllerHWInfo);
					flag = true;
				}
			}
			if (lnQijvSOzkywNLNKGQtfLoeTzEDJ == null)
			{
				Logger.LogError("Default hardware map not found!");
				return;
			}
			if (flag)
			{
				string text = string.Format("{0}:{1}", LGYOCvtIlreYITJNoUEcEWhVeuws.PZhHSABdhmuDfsDZiEIuMLJMqfdB.vendorId.ToString("x4"), LGYOCvtIlreYITJNoUEcEWhVeuws.PZhHSABdhmuDfsDZiEIuMLJMqfdB.productId.ToString("x4"));
				string key = LocalizationManager.AppendToKeyAsPath("windows_gaming_input_gamepad", text);
				lnQijvSOzkywNLNKGQtfLoeTzEDJ.deviceLocalizationInfo.InsertParentKey(0, key);
				lnQijvSOzkywNLNKGQtfLoeTzEDJ.deviceLocalizationInfo.InsertParentKey(1, "windows_gaming_input_gamepad");
				lnQijvSOzkywNLNKGQtfLoeTzEDJ.deviceLocalizationInfo.additionalIdentifyingInformation = $"[{text}]";
			}
			else if (LGYOCvtIlreYITJNoUEcEWhVeuws.eKcWuKRtMbUDcepLxeVbQUfLMiVv && (flag2 || lnQijvSOzkywNLNKGQtfLoeTzEDJ.hardwareMapIdentifier.guid == Consts.joystickGuid_steamController))
			{
				string text2 = string.Format("{0}:{1}", LGYOCvtIlreYITJNoUEcEWhVeuws.PZhHSABdhmuDfsDZiEIuMLJMqfdB.vendorId.ToString("x4"), LGYOCvtIlreYITJNoUEcEWhVeuws.PZhHSABdhmuDfsDZiEIuMLJMqfdB.productId.ToString("x4"));
				string key2 = LocalizationManager.AppendToKeyAsPath((lnQijvSOzkywNLNKGQtfLoeTzEDJ.deviceLocalizationInfo.parentKeys.Count > 0 && !string.IsNullOrEmpty(lnQijvSOzkywNLNKGQtfLoeTzEDJ.deviceLocalizationInfo.parentKeys[0])) ? lnQijvSOzkywNLNKGQtfLoeTzEDJ.deviceLocalizationInfo.parentKeys[0] : "steam_controller", text2);
				lnQijvSOzkywNLNKGQtfLoeTzEDJ.deviceLocalizationInfo.InsertParentKey(0, key2);
				lnQijvSOzkywNLNKGQtfLoeTzEDJ.deviceLocalizationInfo.additionalIdentifyingInformation = $"[{text2}]";
			}
			OAUbbUzMskEygGpaCpCWtwijGcyd = lnQijvSOzkywNLNKGQtfLoeTzEDJ.axisCount;
			cXlBJTkbOxyajoGpCknJwjopNRmeb = lnQijvSOzkywNLNKGQtfLoeTzEDJ.buttonCount;
		}

		private string FDsKCkmDuUFwgHuYzhnTQNzpIjGEb()
		{
			return InputTools.FormatHardwareIdentifierString($"{ReInput.currentPlatform.ToString()}{InputSource.WindowsGamingInput}{LGYOCvtIlreYITJNoUEcEWhVeuws.zfKAPPUGTkSDwuUzdnCkeMvYkEqT}{stXRfKdMISKtaFdVjcUSAFGRBWiPA}{aeYjfBwZssytKWfWYfLmmhGPaLZd.ToString()}");
		}

		private void wgxeDZEqXljrqaLNlcpvFldLowNu(BridgedControllerHWInfo P_0)
		{
			P_0.inputManagerSource = InputSource.WindowsGamingInput;
			P_0.inputSource = LGYOCvtIlreYITJNoUEcEWhVeuws.LMXRTLhCsapPWnAwGjutymfCGrXkA;
			P_0.deviceType = (ControlDeviceType)LGYOCvtIlreYITJNoUEcEWhVeuws.zfKAPPUGTkSDwuUzdnCkeMvYkEqT;
			P_0.hardwareIdentifier = FDsKCkmDuUFwgHuYzhnTQNzpIjGEb();
			P_0.hardwareAxisCount = NLiSgdFFwZFHeJvBQAOLBVhHHoAjA;
			P_0.hardwareButtonCount = pKUdfhHvJcBJLUeLyZLLQcdXpmgHb;
			P_0.hardwareHatCount = GYAjEKPjdQeVCHacbysCqIGRJOBJb;
			if (LGYOCvtIlreYITJNoUEcEWhVeuws.eKcWuKRtMbUDcepLxeVbQUfLMiVv)
			{
				P_0.definitionMatchTag = "[STEAMCONFIGURED]";
			}
			P_0.hw_productName = stXRfKdMISKtaFdVjcUSAFGRBWiPA;
			P_0.hw_deviceGuid = TIwztxvGbEHynbFiHqnBSFYFFOpV;
			P_0.hw_productId = aeYjfBwZssytKWfWYfLmmhGPaLZd.productId;
			P_0.hw_vendorId = aeYjfBwZssytKWfWYfLmmhGPaLZd.vendorId;
			P_0.hw_pidVid = aeYjfBwZssytKWfWYfLmmhGPaLZd;
			P_0.hw_isBluetoothDevice = false;
			P_0.hw_bluetoothDeviceName = stXRfKdMISKtaFdVjcUSAFGRBWiPA;
			P_0.hw_supportsVibration = WEQAwYHdctwEnWTALXBoJdvcNOIZA;
			P_0.hw_localVibrationMotorCount = LJEAqyhFaEJTmocOcvwjtdjAwJEA;
		}

		private void NyPAcHhiHpEPQluFyYvAxnkmBEXbA(BridgedController P_0)
		{
			wgxeDZEqXljrqaLNlcpvFldLowNu(P_0);
			P_0.sourceJoystick = this;
			P_0.gameHardwareMap = lnQijvSOzkywNLNKGQtfLoeTzEDJ.ToGameHardwareControllerMap();
			P_0.instanceName = VSwXSUgqUOIAbYQKKiaGMHdlLKyq;
			P_0.productName = stXRfKdMISKtaFdVjcUSAFGRBWiPA;
			P_0.axisCount = OAUbbUzMskEygGpaCpCWtwijGcyd;
			P_0.buttonCount = cXlBJTkbOxyajoGpCknJwjopNRmeb;
			P_0.isButtonPressureSensitive = new bool[cXlBJTkbOxyajoGpCknJwjopNRmeb];
			Array.Copy(fYspASseBTpVDWeMexCXZrYJHfBq, P_0.isButtonPressureSensitive, cXlBJTkbOxyajoGpCknJwjopNRmeb);
			P_0.unknownControllerHats = mHIQaJHLRFAATIhcJSTFEAdOjEWh();
			P_0.controllerTypeGuid = BnqBmbkUjfRnQqIVVtvPyJXxVURS;
			P_0.controllerExtension = Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002Eextension;
		}

		private UnknownControllerHat[] mHIQaJHLRFAATIhcJSTFEAdOjEWh()
		{
			if (!UUebtwLfpXXbpQoOnZxkutpCGvGI)
			{
				return null;
			}
			UnknownControllerHat[] array = new UnknownControllerHat[2];
			for (int i = 0; i < 2; i++)
			{
				int num = 128 + i * 8;
				UnknownControllerHat.HatButtons hatButtons = new UnknownControllerHat.HatButtons(new int[8]
				{
					num,
					num + 1,
					num + 2,
					num + 3,
					num + 4,
					num + 5,
					num + 6,
					num + 7
				});
				array[i] = new UnknownControllerHat(hatButtons);
			}
			return array;
		}

		public void Dispose()
		{
			LodLFvuHDZavbokUOFYqhUxuqjQFA(true);
			GC.SuppressFinalize(this);
		}

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Dispose
			this.Dispose();
		}

		protected virtual void SBhAAEahcnutiMxrBAuIIoKmvhsmA()
		{
			try
			{
				LodLFvuHDZavbokUOFYqhUxuqjQFA(false);
			}
			finally
			{
				base.Finalize();
			}
		}

		protected virtual void LodLFvuHDZavbokUOFYqhUxuqjQFA(bool P_0)
		{
			if (!UoJWVrILSoivXGTqjmcQbYZaUnlE)
			{
				if (P_0 && LGYOCvtIlreYITJNoUEcEWhVeuws != null)
				{
					LGYOCvtIlreYITJNoUEcEWhVeuws.Dispose();
				}
				UoJWVrILSoivXGTqjmcQbYZaUnlE = true;
			}
		}

		public static int fMkbGAOKNKDKxWDBaACWpnaAemcI(ZLNaniVinNHUoanvGBczlsNkFqSi P_0, ZLNaniVinNHUoanvGBczlsNkFqSi P_1)
		{
			if (P_0.FNTPYdzWtAKBadZblICAMNvrrTju < P_1.FNTPYdzWtAKBadZblICAMNvrrTju)
			{
				return -1;
			}
			if (P_0.FNTPYdzWtAKBadZblICAMNvrrTju > P_1.FNTPYdzWtAKBadZblICAMNvrrTju)
			{
				return 1;
			}
			return 0;
		}

		public static int YXznaDpNacfcMoIrrgTjWpNDagwM(ZLNaniVinNHUoanvGBczlsNkFqSi P_0, ZLNaniVinNHUoanvGBczlsNkFqSi P_1)
		{
			if (P_0.IjUwbQhBbRGnsINjrOWqqweBcOvN < P_1.IjUwbQhBbRGnsINjrOWqqweBcOvN)
			{
				return -1;
			}
			if (P_0.IjUwbQhBbRGnsINjrOWqqweBcOvN > P_1.IjUwbQhBbRGnsINjrOWqqweBcOvN)
			{
				return 1;
			}
			return 0;
		}
	}

	private class fdtTKvNfzCacOhUyvdUmdxLVtLXuA
	{
		public enum PvgWMDQBkvwKHQttzSGNtmiOSjwV
		{
			Exact = 0,
			Approximate = 1
		}

		public class bUAzWyQaCpvpctoYYouezvrFOUjI
		{
			public int mPvEbPitERTXGajROPfJzbMbJjbS;

			public Guid tdufjtajiucLfhlDLXxciTnbXndr;

			public Guid RnefVuDHPPhTrCSbSHfLasiVoljz;

			public int wVLdvVGJPWGYFsXImXgSRBxziVUg;

			public int FcodmCPYHjlLRYleQoFTjoBEBaZo;

			public int fnyBEkjtFAoHGjLBwBKZrQhBOHFFb;

			public int CTBouQCoNFbtDzsZgPzNQlscNoGP;

			public int vsTfLJupoghLeekXhAedbgIMQuDp;

			public int nlTIzJQzyVJBqWvdilzbWdvgddzI;

			public bool HbfCjgAqVESOHDpkHRUdJFAjBWsW(ZLNaniVinNHUoanvGBczlsNkFqSi P_0, PvgWMDQBkvwKHQttzSGNtmiOSjwV P_1)
			{
				if (FcodmCPYHjlLRYleQoFTjoBEBaZo != P_0.NLiSgdFFwZFHeJvBQAOLBVhHHoAjA)
				{
					return false;
				}
				if (fnyBEkjtFAoHGjLBwBKZrQhBOHFFb != P_0.pKUdfhHvJcBJLUeLyZLLQcdXpmgHb)
				{
					return false;
				}
				if (CTBouQCoNFbtDzsZgPzNQlscNoGP != P_0.GYAjEKPjdQeVCHacbysCqIGRJOBJb)
				{
					return false;
				}
				if (vsTfLJupoghLeekXhAedbgIMQuDp != P_0.cXlBJTkbOxyajoGpCknJwjopNRmeb)
				{
					return false;
				}
				if (nlTIzJQzyVJBqWvdilzbWdvgddzI != P_0.OAUbbUzMskEygGpaCpCWtwijGcyd)
				{
					return false;
				}
				if (P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId == mPvEbPitERTXGajROPfJzbMbJjbS)
				{
					return true;
				}
				return P_1 switch
				{
					PvgWMDQBkvwKHQttzSGNtmiOSjwV.Exact => tdufjtajiucLfhlDLXxciTnbXndr == P_0.TIwztxvGbEHynbFiHqnBSFYFFOpV, 
					PvgWMDQBkvwKHQttzSGNtmiOSjwV.Approximate => RnefVuDHPPhTrCSbSHfLasiVoljz == P_0.OURdNuLMAeqlIwzaCgvlTDelGiMiA, 
					_ => throw new NotImplementedException(), 
				};
			}
		}

		private sealed class PxjcDyhQgXhTiAPscWilurxWNVhMA : IEnumerable<bUAzWyQaCpvpctoYYouezvrFOUjI>, IEnumerable, IEnumerator<bUAzWyQaCpvpctoYYouezvrFOUjI>, IEnumerator, IDisposable
		{
			private int cGfFpGYKtiUpGrZVOckuLagIdsVgA;

			private bUAzWyQaCpvpctoYYouezvrFOUjI HCEENYcakCvHviqfEGDnehhcUYsnc;

			private int AbbItcHPlXUCSUuabWOIxmUcfcuab;

			public fdtTKvNfzCacOhUyvdUmdxLVtLXuA pwvKXvITbefTPZaHfAboXovrggqaA;

			private ZLNaniVinNHUoanvGBczlsNkFqSi jbcCCuWcMUZIfPnstupbvCTYGSzBA;

			public ZLNaniVinNHUoanvGBczlsNkFqSi IzSQwuxfpoDDbZKhiZoKbUpOYwHH;

			private PvgWMDQBkvwKHQttzSGNtmiOSjwV SSLDyxStdZBVpGTVWtHCpIXwNNzV;

			public PvgWMDQBkvwKHQttzSGNtmiOSjwV xjEdtuOcPBeYCCeCHaatlmXKNxYv;

			private int bLgKGTrSigCDhuMCWOvZddPCUpuM;

			private int tHrvTCKxmUgBZAaBHXLqJAGRhGqo;

			bUAzWyQaCpvpctoYYouezvrFOUjI IEnumerator<bUAzWyQaCpvpctoYYouezvrFOUjI>.Current
			{
				[DebuggerHidden]
				get
				{
					return HCEENYcakCvHviqfEGDnehhcUYsnc;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return HCEENYcakCvHviqfEGDnehhcUYsnc;
				}
			}

			[DebuggerHidden]
			public PxjcDyhQgXhTiAPscWilurxWNVhMA(int P_0)
			{
				cGfFpGYKtiUpGrZVOckuLagIdsVgA = P_0;
				AbbItcHPlXUCSUuabWOIxmUcfcuab = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				cGfFpGYKtiUpGrZVOckuLagIdsVgA = -2;
			}

			private bool MoveNext()
			{
				int num = cGfFpGYKtiUpGrZVOckuLagIdsVgA;
				fdtTKvNfzCacOhUyvdUmdxLVtLXuA fdtTKvNfzCacOhUyvdUmdxLVtLXuA2 = pwvKXvITbefTPZaHfAboXovrggqaA;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					cGfFpGYKtiUpGrZVOckuLagIdsVgA = -1;
					goto IL_0083;
				}
				cGfFpGYKtiUpGrZVOckuLagIdsVgA = -1;
				bLgKGTrSigCDhuMCWOvZddPCUpuM = fdtTKvNfzCacOhUyvdUmdxLVtLXuA2.NtJvMbEdvNUubWtNsjYpdmaqKftu.Count;
				tHrvTCKxmUgBZAaBHXLqJAGRhGqo = 0;
				goto IL_0093;
				IL_0083:
				tHrvTCKxmUgBZAaBHXLqJAGRhGqo++;
				goto IL_0093;
				IL_0093:
				if (tHrvTCKxmUgBZAaBHXLqJAGRhGqo < bLgKGTrSigCDhuMCWOvZddPCUpuM)
				{
					if (fdtTKvNfzCacOhUyvdUmdxLVtLXuA2.NtJvMbEdvNUubWtNsjYpdmaqKftu[tHrvTCKxmUgBZAaBHXLqJAGRhGqo].HbfCjgAqVESOHDpkHRUdJFAjBWsW(jbcCCuWcMUZIfPnstupbvCTYGSzBA, SSLDyxStdZBVpGTVWtHCpIXwNNzV))
					{
						HCEENYcakCvHviqfEGDnehhcUYsnc = fdtTKvNfzCacOhUyvdUmdxLVtLXuA2.NtJvMbEdvNUubWtNsjYpdmaqKftu[tHrvTCKxmUgBZAaBHXLqJAGRhGqo];
						cGfFpGYKtiUpGrZVOckuLagIdsVgA = 1;
						return true;
					}
					goto IL_0083;
				}
				return false;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator<bUAzWyQaCpvpctoYYouezvrFOUjI> IEnumerable<bUAzWyQaCpvpctoYYouezvrFOUjI>.GetEnumerator()
			{
				PxjcDyhQgXhTiAPscWilurxWNVhMA pxjcDyhQgXhTiAPscWilurxWNVhMA;
				if (cGfFpGYKtiUpGrZVOckuLagIdsVgA == -2 && AbbItcHPlXUCSUuabWOIxmUcfcuab == Environment.CurrentManagedThreadId)
				{
					cGfFpGYKtiUpGrZVOckuLagIdsVgA = 0;
					pxjcDyhQgXhTiAPscWilurxWNVhMA = this;
				}
				else
				{
					pxjcDyhQgXhTiAPscWilurxWNVhMA = new PxjcDyhQgXhTiAPscWilurxWNVhMA(0);
					pxjcDyhQgXhTiAPscWilurxWNVhMA.pwvKXvITbefTPZaHfAboXovrggqaA = pwvKXvITbefTPZaHfAboXovrggqaA;
				}
				pxjcDyhQgXhTiAPscWilurxWNVhMA.jbcCCuWcMUZIfPnstupbvCTYGSzBA = IzSQwuxfpoDDbZKhiZoKbUpOYwHH;
				pxjcDyhQgXhTiAPscWilurxWNVhMA.SSLDyxStdZBVpGTVWtHCpIXwNNzV = xjEdtuOcPBeYCCeCHaatlmXKNxYv;
				return pxjcDyhQgXhTiAPscWilurxWNVhMA;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<bUAzWyQaCpvpctoYYouezvrFOUjI>)this).GetEnumerator();
			}
		}

		private List<bUAzWyQaCpvpctoYYouezvrFOUjI> NtJvMbEdvNUubWtNsjYpdmaqKftu;

		public fdtTKvNfzCacOhUyvdUmdxLVtLXuA()
		{
			NtJvMbEdvNUubWtNsjYpdmaqKftu = new List<bUAzWyQaCpvpctoYYouezvrFOUjI>();
		}

		public void amLYOTOcqaEBdrzSafJaulDnFzIN(ZLNaniVinNHUoanvGBczlsNkFqSi P_0)
		{
			if (P_0 == null)
			{
				return;
			}
			int count = NtJvMbEdvNUubWtNsjYpdmaqKftu.Count;
			for (int i = 0; i < count; i++)
			{
				if (NtJvMbEdvNUubWtNsjYpdmaqKftu[i].HbfCjgAqVESOHDpkHRUdJFAjBWsW(P_0, PvgWMDQBkvwKHQttzSGNtmiOSjwV.Exact))
				{
					NtJvMbEdvNUubWtNsjYpdmaqKftu[i].mPvEbPitERTXGajROPfJzbMbJjbS = P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId;
					NtJvMbEdvNUubWtNsjYpdmaqKftu[i].tdufjtajiucLfhlDLXxciTnbXndr = P_0.TIwztxvGbEHynbFiHqnBSFYFFOpV;
					NtJvMbEdvNUubWtNsjYpdmaqKftu[i].RnefVuDHPPhTrCSbSHfLasiVoljz = P_0.OURdNuLMAeqlIwzaCgvlTDelGiMiA;
					NtJvMbEdvNUubWtNsjYpdmaqKftu[i].wVLdvVGJPWGYFsXImXgSRBxziVUg = P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId;
					NtJvMbEdvNUubWtNsjYpdmaqKftu[i].FcodmCPYHjlLRYleQoFTjoBEBaZo = P_0.NLiSgdFFwZFHeJvBQAOLBVhHHoAjA;
					NtJvMbEdvNUubWtNsjYpdmaqKftu[i].fnyBEkjtFAoHGjLBwBKZrQhBOHFFb = P_0.pKUdfhHvJcBJLUeLyZLLQcdXpmgHb;
					NtJvMbEdvNUubWtNsjYpdmaqKftu[i].CTBouQCoNFbtDzsZgPzNQlscNoGP = P_0.GYAjEKPjdQeVCHacbysCqIGRJOBJb;
					NtJvMbEdvNUubWtNsjYpdmaqKftu[i].vsTfLJupoghLeekXhAedbgIMQuDp = P_0.cXlBJTkbOxyajoGpCknJwjopNRmeb;
					NtJvMbEdvNUubWtNsjYpdmaqKftu[i].nlTIzJQzyVJBqWvdilzbWdvgddzI = P_0.OAUbbUzMskEygGpaCpCWtwijGcyd;
					DPfuDLkXNKswEjRpuiaxXFbBCAen(P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId, P_0.TIwztxvGbEHynbFiHqnBSFYFFOpV, i);
					return;
				}
			}
			NtJvMbEdvNUubWtNsjYpdmaqKftu.Add(new bUAzWyQaCpvpctoYYouezvrFOUjI
			{
				mPvEbPitERTXGajROPfJzbMbJjbS = P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId,
				tdufjtajiucLfhlDLXxciTnbXndr = P_0.TIwztxvGbEHynbFiHqnBSFYFFOpV,
				RnefVuDHPPhTrCSbSHfLasiVoljz = P_0.OURdNuLMAeqlIwzaCgvlTDelGiMiA,
				wVLdvVGJPWGYFsXImXgSRBxziVUg = P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId,
				FcodmCPYHjlLRYleQoFTjoBEBaZo = P_0.NLiSgdFFwZFHeJvBQAOLBVhHHoAjA,
				fnyBEkjtFAoHGjLBwBKZrQhBOHFFb = P_0.pKUdfhHvJcBJLUeLyZLLQcdXpmgHb,
				CTBouQCoNFbtDzsZgPzNQlscNoGP = P_0.GYAjEKPjdQeVCHacbysCqIGRJOBJb,
				vsTfLJupoghLeekXhAedbgIMQuDp = P_0.cXlBJTkbOxyajoGpCknJwjopNRmeb,
				nlTIzJQzyVJBqWvdilzbWdvgddzI = P_0.OAUbbUzMskEygGpaCpCWtwijGcyd
			});
			DPfuDLkXNKswEjRpuiaxXFbBCAen(P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId, P_0.TIwztxvGbEHynbFiHqnBSFYFFOpV, NtJvMbEdvNUubWtNsjYpdmaqKftu.Count - 1);
		}

		[IteratorStateMachine(typeof(PxjcDyhQgXhTiAPscWilurxWNVhMA))]
		public IEnumerable<bUAzWyQaCpvpctoYYouezvrFOUjI> frhAWsnykuDPTdYsWLLreBIoxgpF(ZLNaniVinNHUoanvGBczlsNkFqSi P_0, PvgWMDQBkvwKHQttzSGNtmiOSjwV P_1)
		{
			return new PxjcDyhQgXhTiAPscWilurxWNVhMA(-2)
			{
				pwvKXvITbefTPZaHfAboXovrggqaA = this,
				IzSQwuxfpoDDbZKhiZoKbUpOYwHH = P_0,
				xjEdtuOcPBeYCCeCHaatlmXKNxYv = P_1
			};
		}

		private void DPfuDLkXNKswEjRpuiaxXFbBCAen(int P_0, Guid P_1, int P_2)
		{
			for (int num = NtJvMbEdvNUubWtNsjYpdmaqKftu.Count - 1; num >= 0; num--)
			{
				if (num != P_2 && (NtJvMbEdvNUubWtNsjYpdmaqKftu[num].mPvEbPitERTXGajROPfJzbMbJjbS == P_0 || NtJvMbEdvNUubWtNsjYpdmaqKftu[num].tdufjtajiucLfhlDLXxciTnbXndr == P_1))
				{
					NtJvMbEdvNUubWtNsjYpdmaqKftu.RemoveAt(num);
				}
			}
		}
	}

	private mZoUefHoHQZMVPydZmJcbUbNJKUP npfznSbCyvUhvGtyPswGUOsyIheA;

	private List<ZLNaniVinNHUoanvGBczlsNkFqSi> uDKjuVPKduUzEsLjxSlPakSflMqV;

	private int aqJpSQCBbqCfQDhabpdnjgWCaNOi;

	private fdtTKvNfzCacOhUyvdUmdxLVtLXuA mriNkZQYlmsuekiAbsBpVbmYFjBx;

	private bool pwIvzJzidNzHPFjYFfFglJJPWuaS;

	private ConfigVars UtZaaQiWztKbIOoSaDmNeTijdXEmA;

	private Action<int, ControllerDataUpdater> LfAyEzmgbLcKFfrYoMnTJbiFfWdOA;

	private PlatformInputManager tJzGRdBEdbuKwNjorYHksuIIqseCA;

	private readonly Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> BmQdPXVOuCzbaCIDGiCFXiFstwFq;

	private readonly Func<int> mMRsNjaHJdzNivnYUaxGHUZlfcbGA;

	private Func<PidVid, bool> sXvYxOlYOZUGWEzqaGQXoyUSiopJ;

	[CustomObfuscation(rename = false)]
	int PlatformInputManager.deviceCount => aqJpSQCBbqCfQDhabpdnjgWCaNOi;

	[CustomObfuscation(rename = false)]
	PlatformInputManager PlatformInputManager.primaryInputManager => tJzGRdBEdbuKwNjorYHksuIIqseCA;

	[CustomObfuscation(rename = false)]
	IInputSource PlatformInputManager.inputSource => npfznSbCyvUhvGtyPswGUOsyIheA;

	[CustomObfuscation(rename = false)]
	InputSource PlatformInputManager.inputSourceType => InputSource.WindowsGamingInput;

	protected mZoUefHoHQZMVPydZmJcbUbNJKUP sRHgXXdDQjGxiYfhNdHUuFjOVMpvA => npfznSbCyvUhvGtyPswGUOsyIheA;

	public teyNHYFqhXTPcVcPPjgZwrLLedhK(ConfigVars P_0, Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> P_1, Func<int> P_2, Func<PidVid, bool> P_3)
	{
		try
		{
			UtZaaQiWztKbIOoSaDmNeTijdXEmA = P_0;
			BmQdPXVOuCzbaCIDGiCFXiFstwFq = P_1;
			mMRsNjaHJdzNivnYUaxGHUZlfcbGA = P_2;
			sXvYxOlYOZUGWEzqaGQXoyUSiopJ = P_3;
			tJzGRdBEdbuKwNjorYHksuIIqseCA = this;
			npfznSbCyvUhvGtyPswGUOsyIheA = new mZoUefHoHQZMVPydZmJcbUbNJKUP(P_0, true, false, false);
			npfznSbCyvUhvGtyPswGUOsyIheA.Rewired_002EInterfaces_002EIInputSource_002EDeviceChangedEvent += SystemDeviceConnected;
			LfAyEzmgbLcKFfrYoMnTJbiFfWdOA = UpdateControllerData;
		}
		catch (Exception)
		{
			OnDestroy();
			throw;
		}
	}

	[CustomObfuscation(rename = false)]
	public override void Initialize()
	{
		mriNkZQYlmsuekiAbsBpVbmYFjBx = new fdtTKvNfzCacOhUyvdUmdxLVtLXuA();
		npfznSbCyvUhvGtyPswGUOsyIheA.lYwicCWmrcxLkPlEtaxPFNHGISUg();
		kGiBHjrSVefoSzXfOGIjIAEodKlW();
	}

	[CustomObfuscation(rename = false)]
	public override void Update(UpdateLoopType updateLoop)
	{
		if (npfznSbCyvUhvGtyPswGUOsyIheA != null)
		{
			npfznSbCyvUhvGtyPswGUOsyIheA.Update();
		}
		if (pwIvzJzidNzHPFjYFfFglJJPWuaS)
		{
			dkvCSIhNufBwxMDamcskrnIPcxQIA();
		}
		if (npfznSbCyvUhvGtyPswGUOsyIheA != null)
		{
			npfznSbCyvUhvGtyPswGUOsyIheA.UpdateDevices(updateLoop);
		}
		kJYeXqPxFFfgJgcMuUJAuFCJwzLE();
		if (npfznSbCyvUhvGtyPswGUOsyIheA != null)
		{
			npfznSbCyvUhvGtyPswGUOsyIheA.UpdateFinished();
		}
	}

	[CustomObfuscation(rename = false)]
	public override void OnDestroy()
	{
		if (uDKjuVPKduUzEsLjxSlPakSflMqV != null)
		{
			int count = uDKjuVPKduUzEsLjxSlPakSflMqV.Count;
			for (int i = 0; i < count; i++)
			{
				if (uDKjuVPKduUzEsLjxSlPakSflMqV[i] != null)
				{
					uDKjuVPKduUzEsLjxSlPakSflMqV[i].Dispose();
				}
			}
		}
		if (npfznSbCyvUhvGtyPswGUOsyIheA != null)
		{
			npfznSbCyvUhvGtyPswGUOsyIheA.Dispose();
		}
	}

	[CustomObfuscation(rename = false)]
	public override Action<int, ControllerDataUpdater> GetInputDataUpdateDelegate()
	{
		return LfAyEzmgbLcKFfrYoMnTJbiFfWdOA;
	}

	[CustomObfuscation(rename = false)]
	public override void UpdateControllerData(int inputManagerId, ControllerDataUpdater data)
	{
		for (int i = 0; i < aqJpSQCBbqCfQDhabpdnjgWCaNOi; i++)
		{
			if (uDKjuVPKduUzEsLjxSlPakSflMqV[i].Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId == inputManagerId)
			{
				uDKjuVPKduUzEsLjxSlPakSflMqV[i].FillData(data);
				return;
			}
		}
		Logger.LogError("Invalid joystick Id " + inputManagerId + "!");
	}

	[CustomObfuscation(rename = false)]
	public override void SystemDeviceConnected()
	{
		pwIvzJzidNzHPFjYFfFglJJPWuaS = true;
		if (_SystemDeviceConnectedEvent != null)
		{
			_SystemDeviceConnectedEvent();
		}
	}

	[CustomObfuscation(rename = false)]
	public override void SystemDeviceDisconnected()
	{
		pwIvzJzidNzHPFjYFfFglJJPWuaS = true;
		if (_SystemDeviceDisconnectedEvent != null)
		{
			_SystemDeviceDisconnectedEvent();
		}
	}

	[CustomObfuscation(rename = false)]
	public override void SetUnityJoystickId(int joystickId, int unityJoystickId)
	{
	}

	[CustomObfuscation(rename = false)]
	public override IUnifiedMouseSource GetUnifiedMouseSource()
	{
		return npfznSbCyvUhvGtyPswGUOsyIheA.JLhKAeUIShmziyIEjDNrHdwEXpuD;
	}

	[CustomObfuscation(rename = false)]
	public override IUnifiedKeyboardSource GetUnifiedKeyboardSource()
	{
		return npfznSbCyvUhvGtyPswGUOsyIheA.ARMePJTasZBcDwrzdQmdOtCaFxsK;
	}

	protected bool xEHEgeeGRUumCjyqdeqeKXOdjxQaA(PidVid P_0)
	{
		return sXvYxOlYOZUGWEzqaGQXoyUSiopJ(P_0);
	}

	private void kGiBHjrSVefoSzXfOGIjIAEodKlW()
	{
		cdYLAQmIcueKfOlttHFlAzmuJvGy(enaznzKSUEkienWVhmPrkSsYcYrn());
	}

	private void cdYLAQmIcueKfOlttHFlAzmuJvGy(IList<MeGehmGvtoXRlfGQhxxMoBPtYUNiA> P_0)
	{
		int num = 0;
		List<ZLNaniVinNHUoanvGBczlsNkFqSi> list = uDKjuVPKduUzEsLjxSlPakSflMqV;
		int num2 = aqJpSQCBbqCfQDhabpdnjgWCaNOi;
		uDKjuVPKduUzEsLjxSlPakSflMqV = new List<ZLNaniVinNHUoanvGBczlsNkFqSi>();
		int count = P_0.Count;
		for (int i = 0; i < count; i++)
		{
			if (P_0[i] != null)
			{
				MeGehmGvtoXRlfGQhxxMoBPtYUNiA meGehmGvtoXRlfGQhxxMoBPtYUNiA = P_0[i];
				ZLNaniVinNHUoanvGBczlsNkFqSi zLNaniVinNHUoanvGBczlsNkFqSi = new ZLNaniVinNHUoanvGBczlsNkFqSi(BmQdPXVOuCzbaCIDGiCFXiFstwFq);
				zLNaniVinNHUoanvGBczlsNkFqSi.LGYOCvtIlreYITJNoUEcEWhVeuws = meGehmGvtoXRlfGQhxxMoBPtYUNiA;
				zLNaniVinNHUoanvGBczlsNkFqSi.TIwztxvGbEHynbFiHqnBSFYFFOpV = meGehmGvtoXRlfGQhxxMoBPtYUNiA.WGnHpWtoqiFLkATqqfJNSpjJutdUA;
				zLNaniVinNHUoanvGBczlsNkFqSi.VSwXSUgqUOIAbYQKKiaGMHdlLKyq = meGehmGvtoXRlfGQhxxMoBPtYUNiA.nmueGiNzabkHvAozaBaeKArAmtMcb;
				zLNaniVinNHUoanvGBczlsNkFqSi.stXRfKdMISKtaFdVjcUSAFGRBWiPA = meGehmGvtoXRlfGQhxxMoBPtYUNiA.nmueGiNzabkHvAozaBaeKArAmtMcb;
				zLNaniVinNHUoanvGBczlsNkFqSi.aeYjfBwZssytKWfWYfLmmhGPaLZd = meGehmGvtoXRlfGQhxxMoBPtYUNiA.PZhHSABdhmuDfsDZiEIuMLJMqfdB;
				zLNaniVinNHUoanvGBczlsNkFqSi.IjUwbQhBbRGnsINjrOWqqweBcOvN = meGehmGvtoXRlfGQhxxMoBPtYUNiA.PlBelgOInKBCWFRLdhGbFtZBrXvJb;
				zLNaniVinNHUoanvGBczlsNkFqSi.NLiSgdFFwZFHeJvBQAOLBVhHHoAjA = meGehmGvtoXRlfGQhxxMoBPtYUNiA.vKsCDMOVQygjIFNoTKLMflcGtQGCA;
				zLNaniVinNHUoanvGBczlsNkFqSi.pKUdfhHvJcBJLUeLyZLLQcdXpmgHb = meGehmGvtoXRlfGQhxxMoBPtYUNiA.ltGKGMIhSExZgAUCLFjKPcqYrBJb;
				zLNaniVinNHUoanvGBczlsNkFqSi.GYAjEKPjdQeVCHacbysCqIGRJOBJb = meGehmGvtoXRlfGQhxxMoBPtYUNiA.tvVKvxZENWYixhjTtsQCwxdpjOzL;
				zLNaniVinNHUoanvGBczlsNkFqSi.WEQAwYHdctwEnWTALXBoJdvcNOIZA = meGehmGvtoXRlfGQhxxMoBPtYUNiA.mwSTEYFdwmAqNdvTEnqYkZsWmeAZ;
				zLNaniVinNHUoanvGBczlsNkFqSi.LJEAqyhFaEJTmocOcvwjtdjAwJEA = meGehmGvtoXRlfGQhxxMoBPtYUNiA.KAQfDsHHCmLXoqrAeUubPDrUGNgVA;
				zLNaniVinNHUoanvGBczlsNkFqSi.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002Eextension = meGehmGvtoXRlfGQhxxMoBPtYUNiA.nkqWEADQYgjxnlhMqKhcTvacKKMF;
				zLNaniVinNHUoanvGBczlsNkFqSi.LGYOCvtIlreYITJNoUEcEWhVeuws = meGehmGvtoXRlfGQhxxMoBPtYUNiA;
				zLNaniVinNHUoanvGBczlsNkFqSi.comWoeDEyhTBLjggcEJFCSAkczzZ();
				uDKjuVPKduUzEsLjxSlPakSflMqV.Add(zLNaniVinNHUoanvGBczlsNkFqSi);
				num++;
			}
		}
		aqJpSQCBbqCfQDhabpdnjgWCaNOi = num;
		bsggLfVSMlgbSavtRMPtVezXMYkf(num2, num, list, uDKjuVPKduUzEsLjxSlPakSflMqV);
		for (int j = 0; j < num; j++)
		{
			if (_UpdateControllerInfoEvent != null)
			{
				_UpdateControllerInfoEvent(new UpdateControllerInfoEventArgs(uDKjuVPKduUzEsLjxSlPakSflMqV[j]));
			}
		}
		KGmAFDzhVKqgayVyPBvwHdrxyWOH(list, uDKjuVPKduUzEsLjxSlPakSflMqV, false);
		KGmAFDzhVKqgayVyPBvwHdrxyWOH(uDKjuVPKduUzEsLjxSlPakSflMqV, list, true);
	}

	private void kJYeXqPxFFfgJgcMuUJAuFCJwzLE()
	{
		for (int i = 0; i < aqJpSQCBbqCfQDhabpdnjgWCaNOi; i++)
		{
			uDKjuVPKduUzEsLjxSlPakSflMqV[i]?.Update();
		}
	}

	private IList<MeGehmGvtoXRlfGQhxxMoBPtYUNiA> enaznzKSUEkienWVhmPrkSsYcYrn()
	{
		return npfznSbCyvUhvGtyPswGUOsyIheA.GetJoysticks<MeGehmGvtoXRlfGQhxxMoBPtYUNiA>();
	}

	private void bsggLfVSMlgbSavtRMPtVezXMYkf(int P_0, int P_1, List<ZLNaniVinNHUoanvGBczlsNkFqSi> P_2, List<ZLNaniVinNHUoanvGBczlsNkFqSi> P_3)
	{
		if (P_1 > 0)
		{
			P_3.Sort(ZLNaniVinNHUoanvGBczlsNkFqSi.YXznaDpNacfcMoIrrgTjWpNDagwM);
		}
		if (P_0 > 0 && P_1 > 0)
		{
			mSSOYTHnxnPRVzYhucYdMhnmQfJK(P_1, P_3, P_0, P_2, fdtTKvNfzCacOhUyvdUmdxLVtLXuA.PvgWMDQBkvwKHQttzSGNtmiOSjwV.Exact);
			mSSOYTHnxnPRVzYhucYdMhnmQfJK(P_1, P_3, P_0, P_2, fdtTKvNfzCacOhUyvdUmdxLVtLXuA.PvgWMDQBkvwKHQttzSGNtmiOSjwV.Approximate);
		}
		VGebfUJedoYxnAkICalfMZkBQxnC(P_1, P_3, fdtTKvNfzCacOhUyvdUmdxLVtLXuA.PvgWMDQBkvwKHQttzSGNtmiOSjwV.Exact);
		VGebfUJedoYxnAkICalfMZkBQxnC(P_1, P_3, fdtTKvNfzCacOhUyvdUmdxLVtLXuA.PvgWMDQBkvwKHQttzSGNtmiOSjwV.Approximate);
		for (int i = 0; i < P_1; i++)
		{
			ZLNaniVinNHUoanvGBczlsNkFqSi zLNaniVinNHUoanvGBczlsNkFqSi = P_3[i];
			if (zLNaniVinNHUoanvGBczlsNkFqSi != null && zLNaniVinNHUoanvGBczlsNkFqSi.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId < 0)
			{
				zLNaniVinNHUoanvGBczlsNkFqSi.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId = uVcMAFzSnjLabKMHpiWrExtSHyiEA(P_3);
				zLNaniVinNHUoanvGBczlsNkFqSi.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId = mMRsNjaHJdzNivnYUaxGHUZlfcbGA();
				mriNkZQYlmsuekiAbsBpVbmYFjBx.amLYOTOcqaEBdrzSafJaulDnFzIN(zLNaniVinNHUoanvGBczlsNkFqSi);
			}
		}
		P_3.Sort(ZLNaniVinNHUoanvGBczlsNkFqSi.fMkbGAOKNKDKxWDBaACWpnaAemcI);
	}

	private bool arukQeAnWwxgVUFYzOqrRywSArFH(List<ZLNaniVinNHUoanvGBczlsNkFqSi> P_0, int P_1)
	{
		int count = P_0.Count;
		for (int i = 0; i < count; i++)
		{
			if (P_0[i] != null && P_0[i].Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId == P_1)
			{
				return false;
			}
		}
		return true;
	}

	private int uVcMAFzSnjLabKMHpiWrExtSHyiEA(List<ZLNaniVinNHUoanvGBczlsNkFqSi> P_0)
	{
		int num = 0;
		while (true)
		{
			bool flag = false;
			int count = P_0.Count;
			for (int i = 0; i < count; i++)
			{
				if (P_0[i] != null && P_0[i].Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId == num)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				break;
			}
			num++;
		}
		return num;
	}

	private bool nVdxLlPxGPxKVQESUgbRcrjwjffm(List<ZLNaniVinNHUoanvGBczlsNkFqSi> P_0, int P_1)
	{
		if (P_0 == null)
		{
			return false;
		}
		for (int i = 0; i < P_0.Count; i++)
		{
			if (P_0[i].Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId == P_1)
			{
				return true;
			}
		}
		return false;
	}

	private void mSSOYTHnxnPRVzYhucYdMhnmQfJK(int P_0, List<ZLNaniVinNHUoanvGBczlsNkFqSi> P_1, int P_2, List<ZLNaniVinNHUoanvGBczlsNkFqSi> P_3, fdtTKvNfzCacOhUyvdUmdxLVtLXuA.PvgWMDQBkvwKHQttzSGNtmiOSjwV P_4)
	{
		int num = ((P_4 != fdtTKvNfzCacOhUyvdUmdxLVtLXuA.PvgWMDQBkvwKHQttzSGNtmiOSjwV.Exact) ? 1 : 2);
		for (int i = 0; i < P_0; i++)
		{
			ZLNaniVinNHUoanvGBczlsNkFqSi zLNaniVinNHUoanvGBczlsNkFqSi = P_1[i];
			if (zLNaniVinNHUoanvGBczlsNkFqSi == null || zLNaniVinNHUoanvGBczlsNkFqSi.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId >= 0)
			{
				continue;
			}
			for (int j = 0; j < P_2; j++)
			{
				ZLNaniVinNHUoanvGBczlsNkFqSi zLNaniVinNHUoanvGBczlsNkFqSi2 = P_3[j];
				if (zLNaniVinNHUoanvGBczlsNkFqSi2 != null && !nVdxLlPxGPxKVQESUgbRcrjwjffm(P_1, zLNaniVinNHUoanvGBczlsNkFqSi2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId) && zLNaniVinNHUoanvGBczlsNkFqSi.jEWApEeFNXdKSxTjCmxQdtzBvatNA(zLNaniVinNHUoanvGBczlsNkFqSi2) >= num)
				{
					zLNaniVinNHUoanvGBczlsNkFqSi.rftNTEiaqRcMovSDtsigYYVOwfqj(zLNaniVinNHUoanvGBczlsNkFqSi2);
					mriNkZQYlmsuekiAbsBpVbmYFjBx.amLYOTOcqaEBdrzSafJaulDnFzIN(zLNaniVinNHUoanvGBczlsNkFqSi);
				}
			}
		}
	}

	private void VGebfUJedoYxnAkICalfMZkBQxnC(int P_0, List<ZLNaniVinNHUoanvGBczlsNkFqSi> P_1, fdtTKvNfzCacOhUyvdUmdxLVtLXuA.PvgWMDQBkvwKHQttzSGNtmiOSjwV P_2)
	{
		for (int i = 0; i < P_0; i++)
		{
			ZLNaniVinNHUoanvGBczlsNkFqSi zLNaniVinNHUoanvGBczlsNkFqSi = P_1[i];
			if (zLNaniVinNHUoanvGBczlsNkFqSi == null || zLNaniVinNHUoanvGBczlsNkFqSi.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId >= 0)
			{
				continue;
			}
			fdtTKvNfzCacOhUyvdUmdxLVtLXuA.bUAzWyQaCpvpctoYYouezvrFOUjI bUAzWyQaCpvpctoYYouezvrFOUjI = null;
			foreach (fdtTKvNfzCacOhUyvdUmdxLVtLXuA.bUAzWyQaCpvpctoYYouezvrFOUjI item in mriNkZQYlmsuekiAbsBpVbmYFjBx.frhAWsnykuDPTdYsWLLreBIoxgpF(zLNaniVinNHUoanvGBczlsNkFqSi, P_2))
			{
				if (!nVdxLlPxGPxKVQESUgbRcrjwjffm(P_1, item.mPvEbPitERTXGajROPfJzbMbJjbS) && item.wVLdvVGJPWGYFsXImXgSRBxziVUg >= 0)
				{
					bUAzWyQaCpvpctoYYouezvrFOUjI = item;
					break;
				}
			}
			if (bUAzWyQaCpvpctoYYouezvrFOUjI != null)
			{
				int num = bUAzWyQaCpvpctoYYouezvrFOUjI.wVLdvVGJPWGYFsXImXgSRBxziVUg;
				if (!arukQeAnWwxgVUFYzOqrRywSArFH(P_1, num))
				{
					num = (bUAzWyQaCpvpctoYYouezvrFOUjI.wVLdvVGJPWGYFsXImXgSRBxziVUg = uVcMAFzSnjLabKMHpiWrExtSHyiEA(P_1));
				}
				zLNaniVinNHUoanvGBczlsNkFqSi.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId = num;
				zLNaniVinNHUoanvGBczlsNkFqSi.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId = bUAzWyQaCpvpctoYYouezvrFOUjI.mPvEbPitERTXGajROPfJzbMbJjbS;
				mriNkZQYlmsuekiAbsBpVbmYFjBx.amLYOTOcqaEBdrzSafJaulDnFzIN(zLNaniVinNHUoanvGBczlsNkFqSi);
			}
		}
	}

	private void dkvCSIhNufBwxMDamcskrnIPcxQIA()
	{
		npfznSbCyvUhvGtyPswGUOsyIheA.lYwicCWmrcxLkPlEtaxPFNHGISUg();
		IList<MeGehmGvtoXRlfGQhxxMoBPtYUNiA> list = enaznzKSUEkienWVhmPrkSsYcYrn();
		if (zaUxbGGOKkizHdgobratBPzxTFuW(list))
		{
			cdYLAQmIcueKfOlttHFlAzmuJvGy(list);
		}
		pwIvzJzidNzHPFjYFfFglJJPWuaS = false;
	}

	private bool zaUxbGGOKkizHdgobratBPzxTFuW(IList<MeGehmGvtoXRlfGQhxxMoBPtYUNiA> P_0)
	{
		int count = P_0.Count;
		for (int i = 0; i < count; i++)
		{
			if (P_0[i] != null && !yZVhdfcadXzOEIbLHbpKeUuTmEZAb(P_0[i].WGnHpWtoqiFLkATqqfJNSpjJutdUA))
			{
				return true;
			}
		}
		int count2 = uDKjuVPKduUzEsLjxSlPakSflMqV.Count;
		for (int j = 0; j < count2; j++)
		{
			if (uDKjuVPKduUzEsLjxSlPakSflMqV[j] != null && !EPHfSYZJKhGiXdqqCbnTwbYnJlxdb(P_0, uDKjuVPKduUzEsLjxSlPakSflMqV[j].TIwztxvGbEHynbFiHqnBSFYFFOpV))
			{
				return true;
			}
		}
		return false;
	}

	private bool yZVhdfcadXzOEIbLHbpKeUuTmEZAb(Guid P_0)
	{
		int count = uDKjuVPKduUzEsLjxSlPakSflMqV.Count;
		for (int i = 0; i < count; i++)
		{
			if (uDKjuVPKduUzEsLjxSlPakSflMqV[i] != null && uDKjuVPKduUzEsLjxSlPakSflMqV[i].TIwztxvGbEHynbFiHqnBSFYFFOpV == P_0)
			{
				return true;
			}
		}
		return false;
	}

	private bool EPHfSYZJKhGiXdqqCbnTwbYnJlxdb(IList<MeGehmGvtoXRlfGQhxxMoBPtYUNiA> P_0, Guid P_1)
	{
		int count = P_0.Count;
		for (int i = 0; i < count; i++)
		{
			if (P_0[i] != null && P_0[i].WGnHpWtoqiFLkATqqfJNSpjJutdUA == P_1)
			{
				return true;
			}
		}
		return false;
	}

	private void KGmAFDzhVKqgayVyPBvwHdrxyWOH(List<ZLNaniVinNHUoanvGBczlsNkFqSi> P_0, List<ZLNaniVinNHUoanvGBczlsNkFqSi> P_1, bool P_2)
	{
		if (P_0 == null)
		{
			return;
		}
		int num = P_0?.Count ?? 0;
		int num2 = P_1?.Count ?? 0;
		for (int i = 0; i < num; i++)
		{
			ZLNaniVinNHUoanvGBczlsNkFqSi zLNaniVinNHUoanvGBczlsNkFqSi = P_0[i];
			if (zLNaniVinNHUoanvGBczlsNkFqSi == null)
			{
				continue;
			}
			bool flag = false;
			if (P_1 != null)
			{
				for (int j = 0; j < num2; j++)
				{
					ZLNaniVinNHUoanvGBczlsNkFqSi zLNaniVinNHUoanvGBczlsNkFqSi2 = P_1[j];
					if (zLNaniVinNHUoanvGBczlsNkFqSi2 != null && zLNaniVinNHUoanvGBczlsNkFqSi.TIwztxvGbEHynbFiHqnBSFYFFOpV == zLNaniVinNHUoanvGBczlsNkFqSi2.TIwztxvGbEHynbFiHqnBSFYFFOpV)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				TnOuamekVugwiKtyYCRCgoAkfUas(P_0[i], P_2);
			}
		}
	}

	private void TnOuamekVugwiKtyYCRCgoAkfUas(ZLNaniVinNHUoanvGBczlsNkFqSi P_0, bool P_1)
	{
		if (P_1)
		{
			if (_DeviceConnectedEvent != null)
			{
				_DeviceConnectedEvent(P_0.ToBridgedController());
			}
		}
		else if (_DeviceDisconnectedEvent != null)
		{
			_DeviceDisconnectedEvent(P_0.ToControllerDisconnectedEventArgs());
		}
	}
}
