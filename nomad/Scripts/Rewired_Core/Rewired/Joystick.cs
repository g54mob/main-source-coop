using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rewired.Data.Mapping;
using Rewired.Interfaces;
using Rewired.Internal.Localization;
using Rewired.Utils;
using Rewired.Utils.Classes.Utility;

namespace Rewired
{
	public class Joystick : ControllerWithAxes
	{
		private const int kcOKThJFCpAQyamHoDZSDjEOjVuab = 0;

		private const int HYxIDfItHBtLeEuOICJBkkJhbmFI = 1;

		private IInputManagerJoystickPublic XMKOzuKfQKutzfgehEKbaNsAxoHy;

		private readonly JoystickType[] RSIATbyOxgZLDHUpTrSsPLUCBEPb;

		private readonly ReadOnlyCollection<JoystickType> nowPbzdZBItelzelQzqAOtpJfHsI;

		private readonly bool SryCRRhRrlagXOkqwitmIDftjycgb;

		private readonly bool JQLeROgPCGguGkzaUhVCnlIJprgjA;

		private readonly bool gSrmacRDLhAiNYvjVjQYAlVVSnmrA;

		private readonly int NbfjINdiZESNbfCeZXhXjiwnsfuu;

		private readonly float[] cZmVHnRAAzFPlhnynjiRBlZgNhdsA;

		private readonly TimerAbs[] HAjFuVeWudMzvJaIRhRJaXRagaVLB;

		private readonly int LyCybslyaJNSIwyZQfCxXQYjoQif;

		private readonly Hat[] XigPFTYGerSLlOfXJBpyDAxDgroT;

		private readonly ReadOnlyCollection<Hat> UCFEvEqIrQImIpFoxSOAVMAmGgqT;

		private readonly int PosXyQZssaeHYCitewzZTcSecGTHA;

		private readonly DirectionalPad[] jtxeEOJSzaGHGSoTZFMdandIaqusB;

		private readonly ReadOnlyCollection<DirectionalPad> aUlUOLROvcDWkKwLiHeycqZVlkmGA;

		internal IList<JoystickType> OPnCKKgLUVPQBSYjoEOzDpHTekke
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return EmptyObjects<JoystickType>.EmptyReadOnlyIListT;
				}
				return nowPbzdZBItelzelQzqAOtpJfHsI;
			}
		}

		public long? systemId
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return -1L;
				}
				return XMKOzuKfQKutzfgehEKbaNsAxoHy.systemId;
			}
		}

		public int unityId
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return -1;
				}
				return XMKOzuKfQKutzfgehEKbaNsAxoHy.unityId;
			}
		}

		Guid Controller.deviceInstanceGuid
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return Guid.Empty;
				}
				return XMKOzuKfQKutzfgehEKbaNsAxoHy.persistentGuid;
			}
		}

		public bool supportsVibration
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return false;
				}
				return SryCRRhRrlagXOkqwitmIDftjycgb;
			}
		}

		public float vibrationLeftMotor
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return 0f;
				}
				if (!SryCRRhRrlagXOkqwitmIDftjycgb)
				{
					return 0f;
				}
				if (base.extension is IControllerVibrator { vibrationMotorCount: >0 } controllerVibrator)
				{
					return controllerVibrator.GetVibration(0);
				}
				if (!JQLeROgPCGguGkzaUhVCnlIJprgjA)
				{
					return 0f;
				}
				if (NbfjINdiZESNbfCeZXhXjiwnsfuu > 0)
				{
					return cZmVHnRAAzFPlhnynjiRBlZgNhdsA[0];
				}
				return 0f;
			}
			set
			{
				if (SryCRRhRrlagXOkqwitmIDftjycgb)
				{
					value = MathTools.Clamp(value, 0f, 1f);
					if (base.extension is IControllerVibrator { vibrationMotorCount: >0 } controllerVibrator)
					{
						controllerVibrator.SetVibration(0, value);
					}
					else if (JQLeROgPCGguGkzaUhVCnlIJprgjA && 0 < NbfjINdiZESNbfCeZXhXjiwnsfuu)
					{
						MEwixnLFwoHnoJqkSwWzDeaDIisM(0, value, 0f, false, true);
					}
				}
			}
		}

		public float vibrationRightMotor
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return 0f;
				}
				if (!SryCRRhRrlagXOkqwitmIDftjycgb)
				{
					return 0f;
				}
				if (base.extension is IControllerVibrator { vibrationMotorCount: >1 } controllerVibrator)
				{
					return controllerVibrator.GetVibration(1);
				}
				if (!JQLeROgPCGguGkzaUhVCnlIJprgjA)
				{
					return 0f;
				}
				if (NbfjINdiZESNbfCeZXhXjiwnsfuu > 1)
				{
					return cZmVHnRAAzFPlhnynjiRBlZgNhdsA[1];
				}
				return 0f;
			}
			set
			{
				if (SryCRRhRrlagXOkqwitmIDftjycgb)
				{
					value = MathTools.Clamp(value, 0f, 1f);
					if (base.extension is IControllerVibrator { vibrationMotorCount: >1 } controllerVibrator)
					{
						controllerVibrator.SetVibration(1, value);
					}
					else if (JQLeROgPCGguGkzaUhVCnlIJprgjA && 1 < NbfjINdiZESNbfCeZXhXjiwnsfuu)
					{
						MEwixnLFwoHnoJqkSwWzDeaDIisM(1, value, 0f, false, true);
					}
				}
			}
		}

		public int vibrationMotorCount
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return 0;
				}
				if (base.extension is IControllerVibrator)
				{
					return (base.extension as IControllerVibrator).vibrationMotorCount;
				}
				return NbfjINdiZESNbfCeZXhXjiwnsfuu;
			}
		}

		public int hatCount
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return 0;
				}
				return LyCybslyaJNSIwyZQfCxXQYjoQif;
			}
		}

		public IList<Hat> Hats
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return EmptyObjects<Hat>.EmptyReadOnlyIListT;
				}
				return UCFEvEqIrQImIpFoxSOAVMAmGgqT;
			}
		}

		public int directionalPadCount
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return 0;
				}
				return PosXyQZssaeHYCitewzZTcSecGTHA;
			}
		}

		public IList<DirectionalPad> DirectionalPads
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return EmptyObjects<DirectionalPad>.EmptyReadOnlyIListT;
				}
				return aUlUOLROvcDWkKwLiHeycqZVlkmGA;
			}
		}

		internal int BFfYMETdxToenAnLoAiBVAfvIxMw => XMKOzuKfQKutzfgehEKbaNsAxoHy.inputManagerId;

		internal HardwareControllerMapIdentifier BbaRKBqKWlkxZvWWKhByvwbeMuIC
		{
			get
			{
				if (UNRIOyvPojfCPrjRsEYcHBwwkZqS == null)
				{
					return default(HardwareControllerMapIdentifier);
				}
				return UNRIOyvPojfCPrjRsEYcHBwwkZqS.hardwareMapIdentifier;
			}
		}

		internal Joystick(BridgedController P_0)
			: this(P_0.sourceJoystick.rewiredId, P_0.inputSource, P_0.sourceJoystick.name, (P_0.hw_isBluetoothDevice && !string.IsNullOrEmpty(P_0.hw_bluetoothDeviceName)) ? P_0.hw_bluetoothDeviceName : P_0.productName, P_0.hardwareIdentifier, P_0.controllerTypeGuid, P_0.axisCount, P_0.buttonCount, P_0.isButtonPressureSensitive, P_0.gameHardwareMap, P_0.controllerExtension, new ControllerDataUpdater(P_0.inputManagerSource, P_0.axisCount, P_0.buttonCount, P_0.unknownControllerHats))
		{
			XMKOzuKfQKutzfgehEKbaNsAxoHy = P_0.sourceJoystick;
			base.CnhzsmaaLAXWKxkRoAdJnqWWsbef = XMKOzuKfQKutzfgehEKbaNsAxoHy as ITryGetLocalizedName;
			SryCRRhRrlagXOkqwitmIDftjycgb = P_0.hw_supportsVibration;
			gSrmacRDLhAiNYvjVjQYAlVVSnmrA = P_0.hw_supportsVoice;
			NbfjINdiZESNbfCeZXhXjiwnsfuu = ((!(P_0.controllerExtension is IControllerVibrator)) ? P_0.hw_localVibrationMotorCount : 0);
			if (SryCRRhRrlagXOkqwitmIDftjycgb && NbfjINdiZESNbfCeZXhXjiwnsfuu > 0)
			{
				cZmVHnRAAzFPlhnynjiRBlZgNhdsA = new float[NbfjINdiZESNbfCeZXhXjiwnsfuu];
				HAjFuVeWudMzvJaIRhRJaXRagaVLB = new TimerAbs[NbfjINdiZESNbfCeZXhXjiwnsfuu];
				ArrayTools.Populate(HAjFuVeWudMzvJaIRhRJaXRagaVLB, 0, NbfjINdiZESNbfCeZXhXjiwnsfuu);
				JQLeROgPCGguGkzaUhVCnlIJprgjA = true;
			}
			if (zyYehdPaDXciYCtKVPxEsznJTyqP != Guid.Empty)
			{
				IList<KvKIjjJtUTuaYVUulSaPgImHJaaT> list = ReInput.ZIgVwzVQWHAcYVhSlyysyIYalOxM(zyYehdPaDXciYCtKVPxEsznJTyqP);
				if (list != null)
				{
					List<IControllerTemplate> list2 = null;
					for (int i = 0; i < list.Count; i++)
					{
						KvKIjjJtUTuaYVUulSaPgImHJaaT kvKIjjJtUTuaYVUulSaPgImHJaaT = list[i];
						if (kvKIjjJtUTuaYVUulSaPgImHJaaT == null)
						{
							continue;
						}
						IControllerTemplate controllerTemplate;
						try
						{
							controllerTemplate = UnityTools.externalTools.CreateControllerTemplate(kvKIjjJtUTuaYVUulSaPgImHJaaT.EYmvhOajHQdZnATWhPkTwQeosUoab, new ControllerTemplate.OdAKxDscjuzVRrIHPbBVncTTNlyo(this, kvKIjjJtUTuaYVUulSaPgImHJaaT));
							if (controllerTemplate == null)
							{
								throw new Exception("Controller Template for guid " + kvKIjjJtUTuaYVUulSaPgImHJaaT.EYmvhOajHQdZnATWhPkTwQeosUoab.ToString() + " was not found. If you are using custom Controller Templates, did you export the Controller Templates from the Controller Data Files inspector?");
							}
						}
						catch (Exception ex)
						{
							Logger.LogErrorEditor(ex.Message);
							continue;
						}
						if (list2 == null)
						{
							list2 = new List<IControllerTemplate>();
						}
						list2.Add(controllerTemplate);
					}
					if (list2 != null)
					{
						KbegYtcCiIutDJXwQSVkDlTUgErR(list2.ToArray());
					}
				}
			}
			sXPBxAVgVVidzfPmKZUCZYhRwaIf();
		}

		private Joystick(int P_0, InputSource P_1, string P_2, string P_3, string P_4, Guid P_5, int P_6, int P_7, bool[] P_8, HardwareControllerMap_Game P_9, Extension P_10, ControllerDataUpdater P_11)
			: base(P_0, P_1, P_2, P_3, P_4, ControllerType.Joystick, P_5, P_6, P_7, P_8, P_9, P_10, P_11)
		{
			if (P_9 == null || P_9.joystickTypes == null || P_9.joystickTypes.Length == 0)
			{
				RSIATbyOxgZLDHUpTrSsPLUCBEPb = new JoystickType[1];
			}
			else
			{
				RSIATbyOxgZLDHUpTrSsPLUCBEPb = P_9.joystickTypes;
			}
			nowPbzdZBItelzelQzqAOtpJfHsI = new ReadOnlyCollection<JoystickType>(RSIATbyOxgZLDHUpTrSsPLUCBEPb);
			LyCybslyaJNSIwyZQfCxXQYjoQif = P_9.hatCount;
			XigPFTYGerSLlOfXJBpyDAxDgroT = new Hat[LyCybslyaJNSIwyZQfCxXQYjoQif];
			for (int i = 0; i < LyCybslyaJNSIwyZQfCxXQYjoQif; i++)
			{
				HardwareJoystickMap.CompoundElement hatData = P_9.GetHatData(i);
				try
				{
					if (hatData == null)
					{
						Logger.LogError("Error creating Hat from hardware map! CompoundElement is null!");
						XigPFTYGerSLlOfXJBpyDAxDgroT[i] = new Hat(this, hatData.elementIdentifier, "Hat " + i, new Button[0], new int[0]);
						continue;
					}
					List<Button> list = new List<Button>();
					List<int> list2 = new List<int>();
					for (int j = 0; j < hatData.elementCount; j++)
					{
						int componentElementIdentifierId = hatData.GetComponentElementIdentifierId(j);
						if (!ArrayTools.Contains(P_9.buttonElementIdentifierIds, componentElementIdentifierId))
						{
							list.Add(null);
							list2.Add(-1);
							continue;
						}
						int buttonIndex = P_9.GetButtonIndex(componentElementIdentifierId);
						if (buttonIndex < 0)
						{
							list.Add(null);
							list2.Add(-1);
						}
						else
						{
							list.Add(buttons[buttonIndex]);
							list2.Add(buttonIndex);
						}
					}
					try
					{
						XigPFTYGerSLlOfXJBpyDAxDgroT[i] = new Hat(this, hatData.elementIdentifier, "Hat " + i, list.ToArray(), list2.ToArray());
					}
					catch
					{
						Logger.LogError("Error creating Hat from hardware map! Exception thrown when creating Hat.");
						XigPFTYGerSLlOfXJBpyDAxDgroT[i] = new Hat(this, hatData.elementIdentifier, "Hat " + i, new Button[0], new int[0]);
					}
				}
				finally
				{
					eTsBnmdKizZCCukLuZJWklbrvzdOA(XigPFTYGerSLlOfXJBpyDAxDgroT[i]);
				}
			}
			UCFEvEqIrQImIpFoxSOAVMAmGgqT = new ReadOnlyCollection<Hat>(XigPFTYGerSLlOfXJBpyDAxDgroT);
			PosXyQZssaeHYCitewzZTcSecGTHA = P_9.dpadCount;
			jtxeEOJSzaGHGSoTZFMdandIaqusB = new DirectionalPad[PosXyQZssaeHYCitewzZTcSecGTHA];
			for (int k = 0; k < PosXyQZssaeHYCitewzZTcSecGTHA; k++)
			{
				HardwareJoystickMap.CompoundElement dPadData = P_9.GetDPadData(k);
				try
				{
					if (dPadData == null)
					{
						Logger.LogError("Error creating D-Pad from hardware map! CompoundElement is null!");
						jtxeEOJSzaGHGSoTZFMdandIaqusB[k] = new DirectionalPad(this, dPadData.elementIdentifier, "D-Pad " + k, new Button[0], new int[0]);
						continue;
					}
					List<Button> list3 = new List<Button>();
					List<int> list4 = new List<int>();
					for (int l = 0; l < dPadData.elementCount; l++)
					{
						int componentElementIdentifierId2 = dPadData.GetComponentElementIdentifierId(l);
						if (!ArrayTools.Contains(P_9.buttonElementIdentifierIds, componentElementIdentifierId2))
						{
							list3.Add(null);
							list4.Add(-1);
							continue;
						}
						int buttonIndex2 = P_9.GetButtonIndex(componentElementIdentifierId2);
						if (buttonIndex2 < 0)
						{
							list3.Add(null);
							list4.Add(-1);
						}
						else
						{
							list3.Add(buttons[buttonIndex2]);
							list4.Add(buttonIndex2);
						}
					}
					try
					{
						jtxeEOJSzaGHGSoTZFMdandIaqusB[k] = new DirectionalPad(this, dPadData.elementIdentifier, "D-Pad " + k, list3.ToArray(), list4.ToArray());
					}
					catch
					{
						Logger.LogError("Error creating D-Pad from hardware map! Exception thrown when creating D-Pad.");
						jtxeEOJSzaGHGSoTZFMdandIaqusB[k] = new DirectionalPad(this, dPadData.elementIdentifier, "D-Pad " + k, new Button[0], new int[0]);
					}
				}
				finally
				{
					eTsBnmdKizZCCukLuZJWklbrvzdOA(jtxeEOJSzaGHGSoTZFMdandIaqusB[k]);
				}
			}
			aUlUOLROvcDWkKwLiHeycqZVlkmGA = new ReadOnlyCollection<DirectionalPad>(jtxeEOJSzaGHGSoTZFMdandIaqusB);
		}

		internal bool VObHmYZvkDMeEocKdlqEJoPmrLlr(JoystickType P_0)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			int num = RSIATbyOxgZLDHUpTrSsPLUCBEPb.Length;
			for (int i = 0; i < num; i++)
			{
				if (RSIATbyOxgZLDHUpTrSsPLUCBEPb[i] == P_0)
				{
					return true;
				}
			}
			return false;
		}

		public JoystickCalibrationMapSaveData GetCalibrationMapSaveData()
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return null;
			}
			return new JoystickCalibrationMapSaveData(base.calibrationMap, _type, _hardwareIdentifier, base.hardwareTypeGuid);
		}

		public void SetVibration(float leftMotorLevel, float rightMotorLevel)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
			}
			else
			{
				SetVibration(leftMotorLevel, rightMotorLevel, 0f, 0f);
			}
		}

		public void SetVibration(float leftMotorLevel, float rightMotorLevel, float leftMotorDuration, float rightMotorDuration)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
			}
			else
			{
				if (!SryCRRhRrlagXOkqwitmIDftjycgb)
				{
					return;
				}
				if (base.extension is IControllerVibrator { vibrationMotorCount: var num } controllerVibrator)
				{
					if (num > 0)
					{
						controllerVibrator.SetVibration(0, leftMotorLevel, leftMotorDuration);
					}
					if (num > 1)
					{
						controllerVibrator.SetVibration(1, rightMotorLevel, rightMotorDuration);
					}
				}
				if (JQLeROgPCGguGkzaUhVCnlIJprgjA)
				{
					if (NbfjINdiZESNbfCeZXhXjiwnsfuu > 0)
					{
						MEwixnLFwoHnoJqkSwWzDeaDIisM(0, leftMotorLevel, leftMotorDuration, false, false);
					}
					if (NbfjINdiZESNbfCeZXhXjiwnsfuu > 1)
					{
						MEwixnLFwoHnoJqkSwWzDeaDIisM(1, rightMotorLevel, rightMotorDuration, false, false);
					}
					ilVZBFdlHZrPRhSuoAFVysVhGjHB();
				}
			}
		}

		public void SetVibration(int motorIndex, float motorLevel)
		{
			SetVibration(motorIndex, motorLevel, 0f, stopOtherMotors: false);
		}

		public void SetVibration(int motorIndex, float motorLevel, float duration)
		{
			SetVibration(motorIndex, motorLevel, duration, stopOtherMotors: false);
		}

		public void SetVibration(int motorIndex, float motorLevel, bool stopOtherMotors)
		{
			SetVibration(motorIndex, motorLevel, 0f, stopOtherMotors);
		}

		public void SetVibration(int motorIndex, float motorLevel, float duration, bool stopOtherMotors)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
			}
			else if (SryCRRhRrlagXOkqwitmIDftjycgb && motorIndex >= 0)
			{
				if (base.extension is IControllerVibrator controllerVibrator)
				{
					controllerVibrator.SetVibration(motorIndex, motorLevel, duration, stopOtherMotors);
				}
				if (JQLeROgPCGguGkzaUhVCnlIJprgjA && motorIndex < NbfjINdiZESNbfCeZXhXjiwnsfuu)
				{
					MEwixnLFwoHnoJqkSwWzDeaDIisM(motorIndex, motorLevel, duration, stopOtherMotors, true);
				}
			}
		}

		public float GetVibration(int motorIndex)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0f;
			}
			if (!SryCRRhRrlagXOkqwitmIDftjycgb || motorIndex < 0)
			{
				return 0f;
			}
			if (base.extension is IControllerVibrator controllerVibrator && motorIndex < controllerVibrator.vibrationMotorCount)
			{
				return controllerVibrator.GetVibration(motorIndex);
			}
			if (!JQLeROgPCGguGkzaUhVCnlIJprgjA)
			{
				return 0f;
			}
			if (motorIndex >= NbfjINdiZESNbfCeZXhXjiwnsfuu)
			{
				return 0f;
			}
			return cZmVHnRAAzFPlhnynjiRBlZgNhdsA[motorIndex];
		}

		public void StopVibration()
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
			}
			else
			{
				if (!SryCRRhRrlagXOkqwitmIDftjycgb)
				{
					return;
				}
				if (base.extension is IControllerVibrator controllerVibrator)
				{
					controllerVibrator.StopVibration();
				}
				if (JQLeROgPCGguGkzaUhVCnlIJprgjA)
				{
					Array.Clear(cZmVHnRAAzFPlhnynjiRBlZgNhdsA, 0, cZmVHnRAAzFPlhnynjiRBlZgNhdsA.Length);
					for (int i = 0; i < NbfjINdiZESNbfCeZXhXjiwnsfuu; i++)
					{
						HAjFuVeWudMzvJaIRhRJaXRagaVLB[i].Clear();
					}
				}
				if (XMKOzuKfQKutzfgehEKbaNsAxoHy != null)
				{
					XMKOzuKfQKutzfgehEKbaNsAxoHy.StopVibration();
				}
			}
		}

		internal virtual void UemGeWPiGqAvEbfVrLUcyyhuOrWZ(UpdateLoopType P_0)
		{
			ZHABOdGNpKMPRYvZWajChRBuWCjCA(P_0);
			for (int i = 0; i < LyCybslyaJNSIwyZQfCxXQYjoQif; i++)
			{
				if (XigPFTYGerSLlOfXJBpyDAxDgroT[i] != null)
				{
					XigPFTYGerSLlOfXJBpyDAxDgroT[i].DzCfshFMjzYYSClLiqDxyGpzkzkQA(P_0, yZwGORAVRJPjNCmxxWIIoQgNomuqA);
				}
			}
			for (int j = 0; j < PosXyQZssaeHYCitewzZTcSecGTHA; j++)
			{
				if (jtxeEOJSzaGHGSoTZFMdandIaqusB[j] != null)
				{
					jtxeEOJSzaGHGSoTZFMdandIaqusB[j].vVeVuMQDOLyQhDuvXFZdaVjthqIP(P_0, yZwGORAVRJPjNCmxxWIIoQgNomuqA);
				}
			}
			tHOiTshjRPbijlCQmTpwoEKXCKSbA();
		}

		internal void YnsJhbtxxMkNJRWEyLbLNCgXzish(UpdateControllerInfoEventArgs P_0)
		{
			if (P_0 != null)
			{
				gaGEVffgltNshmbHnUXyhSVXzIss(P_0.sourceJoystick);
			}
		}

		internal void ZCHOihHaYWvbmwhgSXfIQcsWlmCF(BridgedController P_0)
		{
			if (P_0 != null)
			{
				gaGEVffgltNshmbHnUXyhSVXzIss(P_0.sourceJoystick);
			}
		}

		private void gaGEVffgltNshmbHnUXyhSVXzIss(IInputManagerJoystickPublic P_0)
		{
			XMKOzuKfQKutzfgehEKbaNsAxoHy = P_0;
			base.CnhzsmaaLAXWKxkRoAdJnqWWsbef = P_0 as ITryGetLocalizedName;
			if (P_0 != null)
			{
				if (base.extension != null)
				{
					KSZIZVWOlOLbvBwrSsoUdeHPfSXb(P_0.extension);
				}
				else
				{
					MFRDThVvTPdToHImVrXolFRUxuPCA(P_0.extension);
				}
				if (P_0.name != string.Empty)
				{
					_name = P_0.name;
				}
			}
		}

		internal virtual void UrfmCmfxQKLAfOzTsGMlhudpsJjT()
		{
			hlKcVTBoQXLWkqiUgXEameLeIdsf();
			StopVibration();
		}

		internal virtual void WYiFFEqTDnwBNqKJBsrLNUQUSGqm(bool P_0)
		{
			base.rlUNcxrpXspwUOOiDFKtvkpmClqcA(P_0);
			if (!P_0 && !ReInput.applicationRunInBackground)
			{
				StopVibration();
			}
		}

		protected override void Disconnected()
		{
			base.Disconnected();
			if (JQLeROgPCGguGkzaUhVCnlIJprgjA)
			{
				Array.Clear(cZmVHnRAAzFPlhnynjiRBlZgNhdsA, 0, cZmVHnRAAzFPlhnynjiRBlZgNhdsA.Length);
				for (int i = 0; i < NbfjINdiZESNbfCeZXhXjiwnsfuu; i++)
				{
					HAjFuVeWudMzvJaIRhRJaXRagaVLB[i].Clear();
				}
			}
			if (base.extension is IControllerVibrator controllerVibrator)
			{
				controllerVibrator.StopVibration();
			}
		}

		private void tHOiTshjRPbijlCQmTpwoEKXCKSbA()
		{
			if (!SryCRRhRrlagXOkqwitmIDftjycgb || !JQLeROgPCGguGkzaUhVCnlIJprgjA)
			{
				return;
			}
			for (int i = 0; i < NbfjINdiZESNbfCeZXhXjiwnsfuu; i++)
			{
				if (HAjFuVeWudMzvJaIRhRJaXRagaVLB[i].Update())
				{
					SetVibration(i, 0f, stopOtherMotors: false);
				}
			}
		}

		private void MEwixnLFwoHnoJqkSwWzDeaDIisM(int P_0, float P_1, float P_2, bool P_3, bool P_4)
		{
			if (!JQLeROgPCGguGkzaUhVCnlIJprgjA || P_0 < 0 || P_0 >= NbfjINdiZESNbfCeZXhXjiwnsfuu)
			{
				return;
			}
			if (P_3)
			{
				Array.Clear(cZmVHnRAAzFPlhnynjiRBlZgNhdsA, 0, cZmVHnRAAzFPlhnynjiRBlZgNhdsA.Length);
				for (int i = 0; i < NbfjINdiZESNbfCeZXhXjiwnsfuu; i++)
				{
					HAjFuVeWudMzvJaIRhRJaXRagaVLB[i].Clear();
				}
			}
			cZmVHnRAAzFPlhnynjiRBlZgNhdsA[P_0] = MathTools.Clamp01(P_1);
			if (P_1 <= 0f || P_2 <= 0f)
			{
				HAjFuVeWudMzvJaIRhRJaXRagaVLB[P_0].Clear();
			}
			else
			{
				HAjFuVeWudMzvJaIRhRJaXRagaVLB[P_0].Start(P_2);
			}
			if (P_4)
			{
				ilVZBFdlHZrPRhSuoAFVysVhGjHB();
			}
		}

		private void ilVZBFdlHZrPRhSuoAFVysVhGjHB()
		{
			if (SryCRRhRrlagXOkqwitmIDftjycgb && JQLeROgPCGguGkzaUhVCnlIJprgjA && XMKOzuKfQKutzfgehEKbaNsAxoHy != null)
			{
				for (int i = 0; i < cZmVHnRAAzFPlhnynjiRBlZgNhdsA.Length; i++)
				{
					XMKOzuKfQKutzfgehEKbaNsAxoHy.SetVibration(cZmVHnRAAzFPlhnynjiRBlZgNhdsA[i], i);
				}
			}
		}

		private void KAxSxrUCIMNACIZAFlffBiFHpxhl()
		{
		}

		internal static int uEfjfrrziwMGjoyYJmOwuuEXFfJt(Joystick P_0, Joystick P_1)
		{
			if (P_0.BFfYMETdxToenAnLoAiBVAfvIxMw < P_1.BFfYMETdxToenAnLoAiBVAfvIxMw)
			{
				return -1;
			}
			if (P_0.BFfYMETdxToenAnLoAiBVAfvIxMw > P_1.BFfYMETdxToenAnLoAiBVAfvIxMw)
			{
				return 1;
			}
			return 0;
		}
	}
}
