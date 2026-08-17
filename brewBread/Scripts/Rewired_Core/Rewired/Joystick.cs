using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rewired.Data.Mapping;
using Rewired.Interfaces;
using Rewired.Utils;
using Rewired.Utils.Classes.Utility;

namespace Rewired
{
	public class Joystick : ControllerWithAxes
	{
		private const int OhrFFSZMzOTDmDfeFKMpnIEqEzKW = 0;

		private const int dXWFHhEjXVHNkmSNuzPizcIEcTZH = 1;

		private IInputManagerJoystickPublic EatmzbjgpSKzrpMZwcWwOZUZiihP;

		private readonly JoystickType[] HMZZjJkCjEaHRLhyIqzCkjuwNAwd;

		private readonly ReadOnlyCollection<JoystickType> eXSMJXNrBEOMtsJspAcSHHvOvIdi;

		private readonly bool OlbVRqsSBEtRqSittUWERZaqzxXk;

		private readonly bool sWgekvUoGxEjnAXOtkFlKFVqFVAl;

		private readonly bool riIYUkZICekLoeJBrMrGBAvCabsX;

		private readonly int RQswxjUgvoJzPcaSYFLfcfxGvkxsA;

		private readonly float[] cHApXOnOYvppOsbuqcPjWIcGOsIi;

		private readonly TimerAbs[] ifDixddKyelvbcKEcCwEkmKwiwjLb;

		private readonly int nGzbpVDglcowGGmUjmoNUvXQietbA;

		private readonly Hat[] lQCkuukVwwDBOwsdnsTAxXUgANcx;

		private readonly ReadOnlyCollection<Hat> JaHbtlSQSGxZzASGripXpCSsrmkd;

		internal IList<JoystickType> vzEjHwyevBrEEQfTCHiUCLlmPDng
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return EmptyObjects<JoystickType>.EmptyReadOnlyIListT;
				}
				return eXSMJXNrBEOMtsJspAcSHHvOvIdi;
			}
		}

		public long? systemId
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return -1L;
				}
				return EatmzbjgpSKzrpMZwcWwOZUZiihP.systemId;
			}
		}

		public int unityId
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return -1;
				}
				return EatmzbjgpSKzrpMZwcWwOZUZiihP.unityId;
			}
		}

		public override Guid deviceInstanceGuid
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return Guid.Empty;
				}
				return EatmzbjgpSKzrpMZwcWwOZUZiihP.persistentGuid;
			}
		}

		public bool supportsVibration
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return false;
				}
				return OlbVRqsSBEtRqSittUWERZaqzxXk;
			}
		}

		public float vibrationLeftMotor
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return 0f;
				}
				if (!OlbVRqsSBEtRqSittUWERZaqzxXk)
				{
					return 0f;
				}
				if (base.extension is IControllerVibrator { vibrationMotorCount: >0 } controllerVibrator)
				{
					return controllerVibrator.GetVibration(0);
				}
				if (!sWgekvUoGxEjnAXOtkFlKFVqFVAl)
				{
					return 0f;
				}
				if (RQswxjUgvoJzPcaSYFLfcfxGvkxsA > 0)
				{
					return cHApXOnOYvppOsbuqcPjWIcGOsIi[0];
				}
				return 0f;
			}
			set
			{
				if (OlbVRqsSBEtRqSittUWERZaqzxXk)
				{
					value = MathTools.Clamp(value, 0f, 1f);
					if (base.extension is IControllerVibrator { vibrationMotorCount: >0 } controllerVibrator)
					{
						controllerVibrator.SetVibration(0, value);
					}
					else if (sWgekvUoGxEjnAXOtkFlKFVqFVAl && 0 < RQswxjUgvoJzPcaSYFLfcfxGvkxsA)
					{
						bHBLahoTRLPtRodWlXSmrvvlJxqN(0, value, 0f, false, true);
					}
				}
			}
		}

		public float vibrationRightMotor
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return 0f;
				}
				if (!OlbVRqsSBEtRqSittUWERZaqzxXk)
				{
					return 0f;
				}
				if (base.extension is IControllerVibrator { vibrationMotorCount: >1 } controllerVibrator)
				{
					return controllerVibrator.GetVibration(1);
				}
				if (!sWgekvUoGxEjnAXOtkFlKFVqFVAl)
				{
					return 0f;
				}
				if (RQswxjUgvoJzPcaSYFLfcfxGvkxsA > 1)
				{
					return cHApXOnOYvppOsbuqcPjWIcGOsIi[1];
				}
				return 0f;
			}
			set
			{
				if (OlbVRqsSBEtRqSittUWERZaqzxXk)
				{
					value = MathTools.Clamp(value, 0f, 1f);
					if (base.extension is IControllerVibrator { vibrationMotorCount: >1 } controllerVibrator)
					{
						controllerVibrator.SetVibration(1, value);
					}
					else if (sWgekvUoGxEjnAXOtkFlKFVqFVAl && 1 < RQswxjUgvoJzPcaSYFLfcfxGvkxsA)
					{
						bHBLahoTRLPtRodWlXSmrvvlJxqN(1, value, 0f, false, true);
					}
				}
			}
		}

		public int vibrationMotorCount
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return 0;
				}
				if (base.extension is IControllerVibrator)
				{
					return (base.extension as IControllerVibrator).vibrationMotorCount;
				}
				return RQswxjUgvoJzPcaSYFLfcfxGvkxsA;
			}
		}

		public int hatCount
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return 0;
				}
				return nGzbpVDglcowGGmUjmoNUvXQietbA;
			}
		}

		public IList<Hat> Hats
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return EmptyObjects<Hat>.EmptyReadOnlyIListT;
				}
				return JaHbtlSQSGxZzASGripXpCSsrmkd;
			}
		}

		internal int zfwMqMcMdqXTQMwrxKsBjTZHvIF => EatmzbjgpSKzrpMZwcWwOZUZiihP.inputManagerId;

		internal HardwareControllerMapIdentifier BBOiFMbIkLuQbioSkwqyhykKdIjtA
		{
			get
			{
				if (yPbTGFEOQNqKOEHVnHaIUhnHjgYD == null)
				{
					return default(HardwareControllerMapIdentifier);
				}
				return yPbTGFEOQNqKOEHVnHaIUhnHjgYD.hardwareMapIdentifier;
			}
		}

		internal Joystick(BridgedController P_0)
			: this(P_0.sourceJoystick.rewiredId, P_0.inputSource, P_0.sourceJoystick.name, (P_0.hw_isBluetoothDevice && !string.IsNullOrEmpty(P_0.hw_bluetoothDeviceName)) ? P_0.hw_bluetoothDeviceName : P_0.productName, P_0.hardwareIdentifier, P_0.controllerTypeGuid, P_0.axisCount, P_0.buttonCount, P_0.isButtonPressureSensitive, P_0.gameHardwareMap, P_0.controllerExtension, new ControllerDataUpdater(P_0.inputManagerSource, P_0.axisCount, P_0.buttonCount, P_0.unknownControllerHats))
		{
			EatmzbjgpSKzrpMZwcWwOZUZiihP = P_0.sourceJoystick;
			OlbVRqsSBEtRqSittUWERZaqzxXk = P_0.hw_supportsVibration;
			riIYUkZICekLoeJBrMrGBAvCabsX = P_0.hw_supportsVoice;
			RQswxjUgvoJzPcaSYFLfcfxGvkxsA = ((!(P_0.controllerExtension is IControllerVibrator)) ? P_0.hw_localVibrationMotorCount : 0);
			if (OlbVRqsSBEtRqSittUWERZaqzxXk && RQswxjUgvoJzPcaSYFLfcfxGvkxsA > 0)
			{
				cHApXOnOYvppOsbuqcPjWIcGOsIi = new float[RQswxjUgvoJzPcaSYFLfcfxGvkxsA];
				ifDixddKyelvbcKEcCwEkmKwiwjLb = new TimerAbs[RQswxjUgvoJzPcaSYFLfcfxGvkxsA];
				ArrayTools.Populate(ifDixddKyelvbcKEcCwEkmKwiwjLb, 0, RQswxjUgvoJzPcaSYFLfcfxGvkxsA);
				sWgekvUoGxEjnAXOtkFlKFVqFVAl = true;
			}
			if (fMxZVPLmyEupjctdQIaGgbDJdlvHA != Guid.Empty)
			{
				IList<HardwareJoystickTemplateMap> list = ReInput.NyMhJvALUKLeLLDFuwVjXLerIxgdA(fMxZVPLmyEupjctdQIaGgbDJdlvHA);
				if (list != null)
				{
					List<IControllerTemplate> list2 = null;
					for (int i = 0; i < list.Count; i++)
					{
						HardwareJoystickTemplateMap hardwareJoystickTemplateMap = list[i];
						if (hardwareJoystickTemplateMap == null)
						{
							continue;
						}
						IControllerTemplate controllerTemplate;
						try
						{
							controllerTemplate = UnityTools.externalTools.CreateControllerTemplate(hardwareJoystickTemplateMap.Guid, new ControllerTemplate.mMyCOJEBAEHmGFJutMHNAUKrIxjSA(this, hardwareJoystickTemplateMap));
							if (controllerTemplate == null)
							{
								throw new Exception("Controller Template for guid " + hardwareJoystickTemplateMap.Guid.ToString() + " was not found. If you are using custom Controller Templates, did you export the Controller Templates from the Controller Data Files inspector?");
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
						UEbGwSCSBxwfLJkvPMrrMkcOzlpMA(list2.ToArray());
					}
				}
			}
			VFZCTlETAOxSLbCyseetbWbCRTGUb();
		}

		private Joystick(int P_0, InputSource P_1, string P_2, string P_3, string P_4, Guid P_5, int P_6, int P_7, bool[] P_8, HardwareControllerMap_Game P_9, Extension P_10, ControllerDataUpdater P_11)
			: base(P_0, P_1, P_2, P_3, P_4, ControllerType.Joystick, P_5, P_6, P_7, P_8, P_9, P_10, P_11)
		{
			if (P_9 == null || P_9.joystickTypes == null || P_9.joystickTypes.Length == 0)
			{
				HMZZjJkCjEaHRLhyIqzCkjuwNAwd = new JoystickType[1];
			}
			else
			{
				HMZZjJkCjEaHRLhyIqzCkjuwNAwd = P_9.joystickTypes;
			}
			eXSMJXNrBEOMtsJspAcSHHvOvIdi = new ReadOnlyCollection<JoystickType>(HMZZjJkCjEaHRLhyIqzCkjuwNAwd);
			nGzbpVDglcowGGmUjmoNUvXQietbA = P_9.hatCount;
			lQCkuukVwwDBOwsdnsTAxXUgANcx = new Hat[nGzbpVDglcowGGmUjmoNUvXQietbA];
			for (int i = 0; i < nGzbpVDglcowGGmUjmoNUvXQietbA; i++)
			{
				HardwareJoystickMap.CompoundElement hatData = P_9.GetHatData(i);
				try
				{
					if (hatData == null)
					{
						Logger.LogError("Error creating Hat from hardware map! CompoundElement is null!");
						lQCkuukVwwDBOwsdnsTAxXUgANcx[i] = new Hat(this, hatData.elementIdentifier, "Hat " + i, new Button[0], new int[0]);
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
						lQCkuukVwwDBOwsdnsTAxXUgANcx[i] = new Hat(this, hatData.elementIdentifier, "Hat " + i, list.ToArray(), list2.ToArray());
					}
					catch
					{
						Logger.LogError("Error creating Hat from hardware map! Exception thrown when creating Hat.");
						lQCkuukVwwDBOwsdnsTAxXUgANcx[i] = new Hat(this, hatData.elementIdentifier, "Hat " + i, new Button[0], new int[0]);
					}
				}
				finally
				{
					ilFFizeJvQVyRFyOMjSxGFTfdnmy(lQCkuukVwwDBOwsdnsTAxXUgANcx[i]);
				}
			}
			JaHbtlSQSGxZzASGripXpCSsrmkd = new ReadOnlyCollection<Hat>(lQCkuukVwwDBOwsdnsTAxXUgANcx);
		}

		internal bool zURuCIlbnOIyEufZyQzAmPdtgWUU(JoystickType P_0)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			int num = HMZZjJkCjEaHRLhyIqzCkjuwNAwd.Length;
			for (int i = 0; i < num; i++)
			{
				if (HMZZjJkCjEaHRLhyIqzCkjuwNAwd[i] == P_0)
				{
					return true;
				}
			}
			return false;
		}

		public JoystickCalibrationMapSaveData GetCalibrationMapSaveData()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			return new JoystickCalibrationMapSaveData(base.calibrationMap, _type, _hardwareIdentifier, base.hardwareTypeGuid);
		}

		public void SetVibration(float leftMotorLevel, float rightMotorLevel)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
			}
			else
			{
				SetVibration(leftMotorLevel, rightMotorLevel, 0f, 0f);
			}
		}

		public void SetVibration(float leftMotorLevel, float rightMotorLevel, float leftMotorDuration, float rightMotorDuration)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
			}
			else
			{
				if (!OlbVRqsSBEtRqSittUWERZaqzxXk)
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
				if (sWgekvUoGxEjnAXOtkFlKFVqFVAl)
				{
					if (RQswxjUgvoJzPcaSYFLfcfxGvkxsA > 0)
					{
						bHBLahoTRLPtRodWlXSmrvvlJxqN(0, leftMotorLevel, leftMotorDuration, false, false);
					}
					if (RQswxjUgvoJzPcaSYFLfcfxGvkxsA > 1)
					{
						bHBLahoTRLPtRodWlXSmrvvlJxqN(1, rightMotorLevel, rightMotorDuration, false, false);
					}
					BgjoCYeKFJqCzlQmKKzPOUmZoLiG();
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
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
			}
			else if (OlbVRqsSBEtRqSittUWERZaqzxXk && motorIndex >= 0)
			{
				if (base.extension is IControllerVibrator controllerVibrator)
				{
					controllerVibrator.SetVibration(motorIndex, motorLevel, duration, stopOtherMotors);
				}
				if (sWgekvUoGxEjnAXOtkFlKFVqFVAl && motorIndex < RQswxjUgvoJzPcaSYFLfcfxGvkxsA)
				{
					bHBLahoTRLPtRodWlXSmrvvlJxqN(motorIndex, motorLevel, duration, stopOtherMotors, true);
				}
			}
		}

		public float GetVibration(int motorIndex)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0f;
			}
			if (!OlbVRqsSBEtRqSittUWERZaqzxXk || motorIndex < 0)
			{
				return 0f;
			}
			if (base.extension is IControllerVibrator controllerVibrator && motorIndex < controllerVibrator.vibrationMotorCount)
			{
				return controllerVibrator.GetVibration(motorIndex);
			}
			if (!sWgekvUoGxEjnAXOtkFlKFVqFVAl)
			{
				return 0f;
			}
			if (motorIndex >= RQswxjUgvoJzPcaSYFLfcfxGvkxsA)
			{
				return 0f;
			}
			return cHApXOnOYvppOsbuqcPjWIcGOsIi[motorIndex];
		}

		public void StopVibration()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
			}
			else
			{
				if (!OlbVRqsSBEtRqSittUWERZaqzxXk)
				{
					return;
				}
				if (base.extension is IControllerVibrator controllerVibrator)
				{
					controllerVibrator.StopVibration();
				}
				if (sWgekvUoGxEjnAXOtkFlKFVqFVAl)
				{
					Array.Clear(cHApXOnOYvppOsbuqcPjWIcGOsIi, 0, cHApXOnOYvppOsbuqcPjWIcGOsIi.Length);
					for (int i = 0; i < RQswxjUgvoJzPcaSYFLfcfxGvkxsA; i++)
					{
						ifDixddKyelvbcKEcCwEkmKwiwjLb[i].Clear();
					}
				}
				if (EatmzbjgpSKzrpMZwcWwOZUZiihP != null)
				{
					EatmzbjgpSKzrpMZwcWwOZUZiihP.StopVibration();
				}
			}
		}

		internal override void PwSVyxiNOmkyuuXUHWyuymUyabog(UpdateLoopType P_0)
		{
			base.PwSVyxiNOmkyuuXUHWyuymUyabog(P_0);
			for (int i = 0; i < nGzbpVDglcowGGmUjmoNUvXQietbA; i++)
			{
				if (lQCkuukVwwDBOwsdnsTAxXUgANcx[i] != null)
				{
					lQCkuukVwwDBOwsdnsTAxXUgANcx[i].SiJfFLPgexWkpZJLIICzdmwvQQeLA(P_0, BkAqQtJmzNvLobJflzLPUhNYRphu);
				}
			}
			RkjJWtCPffqNHKYZiSpumVKUPDFy();
		}

		internal void UByBUxdeeRfyWdxWnNFetplCMBqd(UpdateControllerInfoEventArgs P_0)
		{
			if (P_0 != null)
			{
				UByBUxdeeRfyWdxWnNFetplCMBqd(P_0.sourceJoystick);
			}
		}

		internal void UByBUxdeeRfyWdxWnNFetplCMBqd(BridgedController P_0)
		{
			if (P_0 != null)
			{
				UByBUxdeeRfyWdxWnNFetplCMBqd(P_0.sourceJoystick);
			}
		}

		private void UByBUxdeeRfyWdxWnNFetplCMBqd(IInputManagerJoystickPublic P_0)
		{
			EatmzbjgpSKzrpMZwcWwOZUZiihP = P_0;
			if (P_0 != null)
			{
				if (base.extension != null)
				{
					VzvRQsucdCcVUuEaxTEkFVSKLlQM(P_0.extension);
				}
				else
				{
					VFmrvNCYadPaAhlYPgWJbhhQLtSOA(P_0.extension);
				}
				if (P_0.name != string.Empty)
				{
					_name = P_0.name;
				}
			}
		}

		internal override void SPGTRPyvIslcMdbPTItsewSLRPxx()
		{
			base.SPGTRPyvIslcMdbPTItsewSLRPxx();
			StopVibration();
		}

		internal override void xKXEQmiiNXKorUsTsPlIOtwcwnfr(bool P_0)
		{
			base.xKXEQmiiNXKorUsTsPlIOtwcwnfr(P_0);
			if (!P_0 && !ReInput.applicationRunInBackground)
			{
				StopVibration();
			}
		}

		protected override void Disconnected()
		{
			base.Disconnected();
			if (sWgekvUoGxEjnAXOtkFlKFVqFVAl)
			{
				Array.Clear(cHApXOnOYvppOsbuqcPjWIcGOsIi, 0, cHApXOnOYvppOsbuqcPjWIcGOsIi.Length);
				for (int i = 0; i < RQswxjUgvoJzPcaSYFLfcfxGvkxsA; i++)
				{
					ifDixddKyelvbcKEcCwEkmKwiwjLb[i].Clear();
				}
			}
			if (base.extension is IControllerVibrator controllerVibrator)
			{
				controllerVibrator.StopVibration();
			}
		}

		private void RkjJWtCPffqNHKYZiSpumVKUPDFy()
		{
			if (!OlbVRqsSBEtRqSittUWERZaqzxXk || !sWgekvUoGxEjnAXOtkFlKFVqFVAl)
			{
				return;
			}
			for (int i = 0; i < RQswxjUgvoJzPcaSYFLfcfxGvkxsA; i++)
			{
				if (ifDixddKyelvbcKEcCwEkmKwiwjLb[i].Update())
				{
					SetVibration(i, 0f, stopOtherMotors: false);
				}
			}
		}

		private void bHBLahoTRLPtRodWlXSmrvvlJxqN(int P_0, float P_1, float P_2, bool P_3, bool P_4)
		{
			if (!sWgekvUoGxEjnAXOtkFlKFVqFVAl || P_0 < 0 || P_0 >= RQswxjUgvoJzPcaSYFLfcfxGvkxsA)
			{
				return;
			}
			if (P_3)
			{
				Array.Clear(cHApXOnOYvppOsbuqcPjWIcGOsIi, 0, cHApXOnOYvppOsbuqcPjWIcGOsIi.Length);
				for (int i = 0; i < RQswxjUgvoJzPcaSYFLfcfxGvkxsA; i++)
				{
					ifDixddKyelvbcKEcCwEkmKwiwjLb[i].Clear();
				}
			}
			cHApXOnOYvppOsbuqcPjWIcGOsIi[P_0] = MathTools.Clamp01(P_1);
			if (P_1 <= 0f || P_2 <= 0f)
			{
				ifDixddKyelvbcKEcCwEkmKwiwjLb[P_0].Clear();
			}
			else
			{
				ifDixddKyelvbcKEcCwEkmKwiwjLb[P_0].Start(P_2);
			}
			if (P_4)
			{
				BgjoCYeKFJqCzlQmKKzPOUmZoLiG();
			}
		}

		private void BgjoCYeKFJqCzlQmKKzPOUmZoLiG()
		{
			if (OlbVRqsSBEtRqSittUWERZaqzxXk && sWgekvUoGxEjnAXOtkFlKFVqFVAl && EatmzbjgpSKzrpMZwcWwOZUZiihP != null)
			{
				for (int i = 0; i < cHApXOnOYvppOsbuqcPjWIcGOsIi.Length; i++)
				{
					EatmzbjgpSKzrpMZwcWwOZUZiihP.SetVibration(cHApXOnOYvppOsbuqcPjWIcGOsIi[i], i);
				}
			}
		}

		private void rDWRWjTgPbEeVRYHfgILTzUUDvBL()
		{
		}

		internal static int BLBeFCaTjYjfxFliEgaztNfvQKfCA(Joystick P_0, Joystick P_1)
		{
			if (P_0.zfwMqMcMdqXTQMwrxKsBjTZHvIF < P_1.zfwMqMcMdqXTQMwrxKsBjTZHvIF)
			{
				return -1;
			}
			if (P_0.zfwMqMcMdqXTQMwrxKsBjTZHvIF > P_1.zfwMqMcMdqXTQMwrxKsBjTZHvIF)
			{
				return 1;
			}
			return 0;
		}
	}
}
