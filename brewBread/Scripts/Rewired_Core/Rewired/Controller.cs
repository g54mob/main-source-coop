using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using Rewired.Config;
using Rewired.Data.Mapping;
using Rewired.Interfaces;
using Rewired.Utils;
using UnityEngine;

namespace Rewired
{
	public abstract class Controller
	{
		public abstract class Element
		{
			internal abstract class HaCAJRkznbyRXPNUuqdpPcxXLSJVA
			{
				public abstract class WHJOnpKGSYnQhsBBqqilENNrUZGe
				{
					public abstract void jpwugzufXqktYbXkMYboQpqCbQgL();
				}

				protected readonly int rDwgYRBSkOPRjVvObsjyXpnizJLT;

				protected readonly int[] hOdpiFRUiFyATBokejgRyDMQCvoc;

				protected WHJOnpKGSYnQhsBBqqilENNrUZGe[] ReyFXLxUNcPvXNGMfpODOqFiLRfR;

				public WHJOnpKGSYnQhsBBqqilENNrUZGe SgJVPHrjFyHgCSEpxtgUijWhNNop;

				private int aVOlNuRVvCOJdZFGeiVVgfpKNgOsA;

				public int yVrZIXjSxHcfgFdpNDzhuqBYIrTH = -1;

				protected ReadOnlyCollection<WHJOnpKGSYnQhsBBqqilENNrUZGe> lAbUcnBzcWBHFTsXYerMkXisUAwj;

				public IList<WHJOnpKGSYnQhsBBqqilENNrUZGe> LODtCJwLEBFFdekKvOQAcGWNoNrhA => lAbUcnBzcWBHFTsXYerMkXisUAwj;

				public UpdateLoopType PiEdbgjMHSsksYjRKSGckYZEsfAE
				{
					set
					{
						if (yVrZIXjSxHcfgFdpNDzhuqBYIrTH != (int)updateLoopType)
						{
							yVrZIXjSxHcfgFdpNDzhuqBYIrTH = (int)updateLoopType;
							aVOlNuRVvCOJdZFGeiVVgfpKNgOsA = hOdpiFRUiFyATBokejgRyDMQCvoc[(int)updateLoopType];
							SgJVPHrjFyHgCSEpxtgUijWhNNop = ReyFXLxUNcPvXNGMfpODOqFiLRfR[aVOlNuRVvCOJdZFGeiVVgfpKNgOsA];
						}
					}
				}

				public HaCAJRkznbyRXPNUuqdpPcxXLSJVA(UpdateLoopSetting P_0)
				{
					hOdpiFRUiFyATBokejgRyDMQCvoc = new int[3];
					rDwgYRBSkOPRjVvObsjyXpnizJLT = 0;
					using (TempListPool.TList<UpdateLoopType> tList = TempListPool.GetTList<UpdateLoopType>(3))
					{
						List<UpdateLoopType> list = tList.list;
						EnumConverter.ToUpdateLoopTypes(P_0, list);
						for (int i = 0; i < list.Count; i++)
						{
							hOdpiFRUiFyATBokejgRyDMQCvoc[(int)list[i]] = rDwgYRBSkOPRjVvObsjyXpnizJLT;
							rDwgYRBSkOPRjVvObsjyXpnizJLT++;
						}
					}
					ReyFXLxUNcPvXNGMfpODOqFiLRfR = new WHJOnpKGSYnQhsBBqqilENNrUZGe[rDwgYRBSkOPRjVvObsjyXpnizJLT];
					lAbUcnBzcWBHFTsXYerMkXisUAwj = new ReadOnlyCollection<WHJOnpKGSYnQhsBBqqilENNrUZGe>(ReyFXLxUNcPvXNGMfpODOqFiLRfR);
				}

				public void jpwugzufXqktYbXkMYboQpqCbQgL()
				{
					for (int i = 0; i < rDwgYRBSkOPRjVvObsjyXpnizJLT; i++)
					{
						ReyFXLxUNcPvXNGMfpODOqFiLRfR[i].jpwugzufXqktYbXkMYboQpqCbQgL();
					}
				}
			}

			public readonly int id;

			public readonly string name;

			public readonly ControllerElementType type;

			internal HaCAJRkznbyRXPNUuqdpPcxXLSJVA ojyRDEyxfmuKcQCdQAQNJvBaHFVfA;

			internal int HKnKeWNkqTdTiwoOhLnaWaFipaWu;

			internal Controller oBPMdftKtJLWDwKjOKgLuimxBfUKA;

			internal readonly int QajDFFaomlkLHzaostfWYpGUioys;

			private CompoundElement jBhHGhrCRTACkQnwqMopaNalQUDY;

			public ControllerElementIdentifier elementIdentifier
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					ControllerElementIdentifier elementIdentifierById = oBPMdftKtJLWDwKjOKgLuimxBfUKA.GetElementIdentifierById(id);
					if (elementIdentifierById == null)
					{
						return ControllerElementIdentifier.BlankReadOnly;
					}
					return elementIdentifierById;
				}
			}

			public bool isMemberElement
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return false;
					}
					return HKnKeWNkqTdTiwoOhLnaWaFipaWu > 0;
				}
			}

			public CompoundElement compoundElement => jBhHGhrCRTACkQnwqMopaNalQUDY;

			internal Element(Controller P_0, int P_1, string P_2, ControllerElementType P_3)
			{
				oBPMdftKtJLWDwKjOKgLuimxBfUKA = P_0;
				id = P_1;
				name = P_2;
				type = P_3;
				QajDFFaomlkLHzaostfWYpGUioys = ReInput.id;
			}

			public void Reset()
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				}
				else if (ojyRDEyxfmuKcQCdQAQNJvBaHFVfA != null)
				{
					ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.jpwugzufXqktYbXkMYboQpqCbQgL();
				}
			}

			internal void esISkaZLgvXMyMkBOYdQGoXSYVWq(CompoundElement P_0)
			{
				if (HKnKeWNkqTdTiwoOhLnaWaFipaWu > 0)
				{
					Logger.LogWarning("This element is already a member of a compound element! This is not supported. Resulting values may be unpredictable.");
				}
				HKnKeWNkqTdTiwoOhLnaWaFipaWu++;
				if (jBhHGhrCRTACkQnwqMopaNalQUDY != null)
				{
					jBhHGhrCRTACkQnwqMopaNalQUDY = P_0;
				}
			}

			internal void TfcJjzJPZRzfwXAHvqJqpCeijFbb(CompoundElement P_0)
			{
				if (HKnKeWNkqTdTiwoOhLnaWaFipaWu == 0)
				{
					Logger.LogWarning("This element is not a member of a compound element!");
					HKnKeWNkqTdTiwoOhLnaWaFipaWu = 0;
					return;
				}
				HKnKeWNkqTdTiwoOhLnaWaFipaWu--;
				if (jBhHGhrCRTACkQnwqMopaNalQUDY == P_0)
				{
					jBhHGhrCRTACkQnwqMopaNalQUDY = null;
				}
			}
		}

		public sealed class Axis : Element
		{
			internal class OsEuISpNkdfnNIHvUtHIavDtPcibA : HaCAJRkznbyRXPNUuqdpPcxXLSJVA
			{
				public class RMQfTwHmgJLdQGIzhZOpzLrPBpgJB : WHJOnpKGSYnQhsBBqqilENNrUZGe
				{
					private const float pyEeBBbejpZuiDeCCgTDhjwIukt = 0.001f;

					public float yYOUbwIbBcyPAQvjeAEXVsjzLAln;

					public float oQGfjHJkbCQKodRETgOXZGbyQfIl;

					public float cJcuFiGqILsQfQKYrbiPfelYgCKk;

					public float NojvecSaNkfpfdIZUDYaHPzfimNvA;

					public float BZPcWmWJPkSAZIJAHfWQQAkNmyPk;

					public float uqoqgQgzXpOhClEoNgQUEhQMNMQBb;

					public double ZIMbjRJLuWinRbzFDVTxzEbKBDloA;

					public double fWkFzVpDmXdhUgqORHbBMdKbBGXPA;

					public double abAabALTIIeDvAiBRCaiNTHfbFcIA;

					public double ugJOkYiwCcPkYQsgqlFdrREptCgM;

					public double ZFRaMtkdvMpCldrneuMGPmQqfvqmA;

					public double BJRGtrSWHDIHsYeFSkMbDUMEvGMB;

					public double JXWPobRqAhDdauEioEJmDrdSVWCY
					{
						get
						{
							if ((double)yYOUbwIbBcyPAQvjeAEXVsjzLAln == 0.0)
							{
								return 0.0;
							}
							return ReInput.unscaledTime - abAabALTIIeDvAiBRCaiNTHfbFcIA;
						}
					}

					public double EPGWGnAjrAasmdFIuqDGOezWnWLr
					{
						get
						{
							if ((double)cJcuFiGqILsQfQKYrbiPfelYgCKk == 0.0)
							{
								return 0.0;
							}
							return ReInput.unscaledTime - ugJOkYiwCcPkYQsgqlFdrREptCgM;
						}
					}

					public double ZcocVhibFCpKsNAEnEvJYxBDvlnM
					{
						get
						{
							if (yYOUbwIbBcyPAQvjeAEXVsjzLAln != 0f)
							{
								return 0.0;
							}
							return ReInput.unscaledTime - ZIMbjRJLuWinRbzFDVTxzEbKBDloA;
						}
					}

					public double vpZYCMAsUEKeZjLjyCtEvoEOghQU
					{
						get
						{
							if ((double)cJcuFiGqILsQfQKYrbiPfelYgCKk != 0.0)
							{
								return 0.0;
							}
							return ReInput.unscaledTime - fWkFzVpDmXdhUgqORHbBMdKbBGXPA;
						}
					}

					public void jRaYtHNVcykNMAbqOnSGaKIIGSEaA(bool P_0)
					{
						double unscaledTime = ReInput.unscaledTime;
						if (P_0)
						{
							if (!MathTools.Approximately(BZPcWmWJPkSAZIJAHfWQQAkNmyPk, 0f))
							{
								ZIMbjRJLuWinRbzFDVTxzEbKBDloA = unscaledTime;
							}
							else
							{
								abAabALTIIeDvAiBRCaiNTHfbFcIA = unscaledTime;
							}
							if (!MathTools.IsNear(BZPcWmWJPkSAZIJAHfWQQAkNmyPk, uqoqgQgzXpOhClEoNgQUEhQMNMQBb, 0.001f))
							{
								ZFRaMtkdvMpCldrneuMGPmQqfvqmA = unscaledTime;
							}
						}
						else
						{
							if (!MathTools.Approximately(yYOUbwIbBcyPAQvjeAEXVsjzLAln, 0f))
							{
								ZIMbjRJLuWinRbzFDVTxzEbKBDloA = unscaledTime;
							}
							else
							{
								abAabALTIIeDvAiBRCaiNTHfbFcIA = unscaledTime;
							}
							if (!MathTools.IsNear(yYOUbwIbBcyPAQvjeAEXVsjzLAln, oQGfjHJkbCQKodRETgOXZGbyQfIl, 0.001f))
							{
								ZFRaMtkdvMpCldrneuMGPmQqfvqmA = unscaledTime;
							}
						}
						if (!MathTools.Approximately(cJcuFiGqILsQfQKYrbiPfelYgCKk, 0f))
						{
							fWkFzVpDmXdhUgqORHbBMdKbBGXPA = unscaledTime;
						}
						else
						{
							ugJOkYiwCcPkYQsgqlFdrREptCgM = unscaledTime;
						}
						if (!MathTools.IsNear(cJcuFiGqILsQfQKYrbiPfelYgCKk, NojvecSaNkfpfdIZUDYaHPzfimNvA, 0.001f))
						{
							BJRGtrSWHDIHsYeFSkMbDUMEvGMB = unscaledTime;
						}
					}

					public void VPWZJxJyPrOYmxhQUKrwKcepGGGdA(float P_0)
					{
						if (NojvecSaNkfpfdIZUDYaHPzfimNvA != cJcuFiGqILsQfQKYrbiPfelYgCKk)
						{
							NojvecSaNkfpfdIZUDYaHPzfimNvA = cJcuFiGqILsQfQKYrbiPfelYgCKk;
						}
						if (cJcuFiGqILsQfQKYrbiPfelYgCKk != P_0)
						{
							cJcuFiGqILsQfQKYrbiPfelYgCKk = P_0;
						}
					}

					public override void jpwugzufXqktYbXkMYboQpqCbQgL()
					{
						yYOUbwIbBcyPAQvjeAEXVsjzLAln = 0f;
						oQGfjHJkbCQKodRETgOXZGbyQfIl = 0f;
						cJcuFiGqILsQfQKYrbiPfelYgCKk = 0f;
						NojvecSaNkfpfdIZUDYaHPzfimNvA = 0f;
						ZIMbjRJLuWinRbzFDVTxzEbKBDloA = 0.0;
						fWkFzVpDmXdhUgqORHbBMdKbBGXPA = 0.0;
						abAabALTIIeDvAiBRCaiNTHfbFcIA = 0.0;
						ugJOkYiwCcPkYQsgqlFdrREptCgM = 0.0;
						ZFRaMtkdvMpCldrneuMGPmQqfvqmA = 0.0;
						BJRGtrSWHDIHsYeFSkMbDUMEvGMB = 0.0;
					}
				}

				public OsEuISpNkdfnNIHvUtHIavDtPcibA(UpdateLoopSetting P_0)
					: base(P_0)
				{
					for (int i = 0; i < rDwgYRBSkOPRjVvObsjyXpnizJLT; i++)
					{
						ReyFXLxUNcPvXNGMfpODOqFiLRfR[i] = new RMQfTwHmgJLdQGIzhZOpzLrPBpgJB();
					}
					SgJVPHrjFyHgCSEpxtgUijWhNNop = ReyFXLxUNcPvXNGMfpODOqFiLRfR[0];
				}
			}

			internal readonly AxisRange fHLnmlqZlxUGHumVxAlzXLaaqYid;

			internal readonly HardwareAxisInfo EKLxpZvxKyeifPSbgLSzhhhiYuui;

			public float value
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0f;
					}
					if (base.isMemberElement)
					{
						return ((OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).BZPcWmWJPkSAZIJAHfWQQAkNmyPk;
					}
					return ((OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).yYOUbwIbBcyPAQvjeAEXVsjzLAln;
				}
			}

			public float valuePrev
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0f;
					}
					if (base.isMemberElement)
					{
						return ((OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).uqoqgQgzXpOhClEoNgQUEhQMNMQBb;
					}
					return ((OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).oQGfjHJkbCQKodRETgOXZGbyQfIl;
				}
			}

			public float valueRaw
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0f;
					}
					return ((OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).cJcuFiGqILsQfQKYrbiPfelYgCKk;
				}
				internal set
				{
					((OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).VPWZJxJyPrOYmxhQUKrwKcepGGGdA(num);
				}
			}

			public float valueRawPrev
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0f;
					}
					return ((OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).NojvecSaNkfpfdIZUDYaHPzfimNvA;
				}
			}

			public float valueDelta
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0f;
					}
					return value - valuePrev;
				}
			}

			public float valueDeltaRaw
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0f;
					}
					return ((OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).cJcuFiGqILsQfQKYrbiPfelYgCKk - ((OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).NojvecSaNkfpfdIZUDYaHPzfimNvA;
				}
			}

			public double lastTimeActive
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0.0;
					}
					return ((OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).ZIMbjRJLuWinRbzFDVTxzEbKBDloA;
				}
			}

			public double lastTimeActiveRaw
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0.0;
					}
					return ((OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).fWkFzVpDmXdhUgqORHbBMdKbBGXPA;
				}
			}

			public double lastTimeInactive
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0.0;
					}
					return ((OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).abAabALTIIeDvAiBRCaiNTHfbFcIA;
				}
			}

			public double lastTimeInactiveRaw
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0.0;
					}
					return ((OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).ugJOkYiwCcPkYQsgqlFdrREptCgM;
				}
			}

			public double lastTimeValueChanged
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0.0;
					}
					return ((OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).ZFRaMtkdvMpCldrneuMGPmQqfvqmA;
				}
			}

			public double lastTimeValueChangedRaw
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0.0;
					}
					return ((OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).BJRGtrSWHDIHsYeFSkMbDUMEvGMB;
				}
			}

			public double timeActive
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0.0;
					}
					return ((OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).JXWPobRqAhDdauEioEJmDrdSVWCY;
				}
			}

			public double timeActiveRaw
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0.0;
					}
					return ((OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).JXWPobRqAhDdauEioEJmDrdSVWCY;
				}
			}

			public double timeInactive
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0.0;
					}
					return ((OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).ZcocVhibFCpKsNAEnEvJYxBDvlnM;
				}
			}

			public double timeInactiveRaw
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0.0;
					}
					return ((OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).vpZYCMAsUEKeZjLjyCtEvoEOghQU;
				}
			}

			public float pollingDeadZone
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0f;
					}
					if (EKLxpZvxKyeifPSbgLSzhhhiYuui == null)
					{
						return -1f;
					}
					return EKLxpZvxKyeifPSbgLSzhhhiYuui._pollingDeadZone;
				}
				set
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return;
					}
					if (value < 0f)
					{
						value = -1f;
					}
					if (EKLxpZvxKyeifPSbgLSzhhhiYuui != null)
					{
						EKLxpZvxKyeifPSbgLSzhhhiYuui._pollingDeadZone = value;
					}
				}
			}

			internal float gjaDXqBcsEqQjcCPeBSyVPMJVYcdb => ((OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).yYOUbwIbBcyPAQvjeAEXVsjzLAln;

			internal float KJTQlSVUdheSGMGbWBNeEicnUgFuA => ((OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).oQGfjHJkbCQKodRETgOXZGbyQfIl;

			internal float rtcgmrWiaobuHyScaIvcDhJnPmXHA
			{
				get
				{
					if (EKLxpZvxKyeifPSbgLSzhhhiYuui == null)
					{
						return ReInput.configuration.defaultAbsoluteAxisPollingDeadZone;
					}
					if (EKLxpZvxKyeifPSbgLSzhhhiYuui._pollingDeadZone >= 0f)
					{
						return EKLxpZvxKyeifPSbgLSzhhhiYuui._pollingDeadZone;
					}
					return EKLxpZvxKyeifPSbgLSzhhhiYuui._dataFormat switch
					{
						AxisCoordinateMode.Absolute => ReInput.configuration.defaultAbsoluteAxisPollingDeadZone, 
						AxisCoordinateMode.Relative => ReInput.configuration.defaultRelativeAxisPollingDeadZone, 
						_ => throw new NotImplementedException(), 
					};
				}
			}

			internal void pHaZQUgmODsRYrJJZXIDpgKsQFlG(float P_0)
			{
				OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB obj = (OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop;
				obj.uqoqgQgzXpOhClEoNgQUEhQMNMQBb = obj.BZPcWmWJPkSAZIJAHfWQQAkNmyPk;
				obj.BZPcWmWJPkSAZIJAHfWQQAkNmyPk = P_0;
			}

			internal Axis(Controller P_0, int P_1, string P_2, AxisRange P_3, HardwareAxisInfo P_4)
				: base(P_0, P_1, P_2, ControllerElementType.Axis)
			{
				ojyRDEyxfmuKcQCdQAQNJvBaHFVfA = new OsEuISpNkdfnNIHvUtHIavDtPcibA(ReInput.configVars.updateLoop);
				fHLnmlqZlxUGHumVxAlzXLaaqYid = P_3;
				EKLxpZvxKyeifPSbgLSzhhhiYuui = P_4;
			}

			internal void OSdaMXbKzaxocQhQUwpRqqjiKWMZ(UpdateLoopType P_0)
			{
				if (ojyRDEyxfmuKcQCdQAQNJvBaHFVfA != null && ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.yVrZIXjSxHcfgFdpNDzhuqBYIrTH != (int)P_0)
				{
					ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.PiEdbgjMHSsksYjRKSGckYZEsfAE = P_0;
				}
			}

			internal void dQOebbIzlPxLQZJPccrvmfbbpCPJ(AxisCalibration P_0)
			{
				OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB rMQfTwHmgJLdQGIzhZOpzLrPBpgJB = (OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop;
				rMQfTwHmgJLdQGIzhZOpzLrPBpgJB.oQGfjHJkbCQKodRETgOXZGbyQfIl = rMQfTwHmgJLdQGIzhZOpzLrPBpgJB.yYOUbwIbBcyPAQvjeAEXVsjzLAln;
				float yYOUbwIbBcyPAQvjeAEXVsjzLAln = P_0.GetCalibratedValue(rMQfTwHmgJLdQGIzhZOpzLrPBpgJB.cJcuFiGqILsQfQKYrbiPfelYgCKk, fHLnmlqZlxUGHumVxAlzXLaaqYid);
				if (P_0.applyRangeCalibration)
				{
					yYOUbwIbBcyPAQvjeAEXVsjzLAln = MathTools.Clamp(yYOUbwIbBcyPAQvjeAEXVsjzLAln, -1f, 1f);
				}
				rMQfTwHmgJLdQGIzhZOpzLrPBpgJB.yYOUbwIbBcyPAQvjeAEXVsjzLAln = yYOUbwIbBcyPAQvjeAEXVsjzLAln;
			}

			internal void dQOebbIzlPxLQZJPccrvmfbbpCPJ()
			{
				OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB obj = (OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop;
				obj.oQGfjHJkbCQKodRETgOXZGbyQfIl = obj.yYOUbwIbBcyPAQvjeAEXVsjzLAln;
				obj.yYOUbwIbBcyPAQvjeAEXVsjzLAln = obj.cJcuFiGqILsQfQKYrbiPfelYgCKk;
			}

			internal void OfBGVYEcqDtxKFrYpGLjfDFRRFgR()
			{
				OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB obj = (OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop;
				obj.oQGfjHJkbCQKodRETgOXZGbyQfIl = obj.yYOUbwIbBcyPAQvjeAEXVsjzLAln;
				obj.yYOUbwIbBcyPAQvjeAEXVsjzLAln = 0f;
			}

			internal void UtptVpcXhrlwQJVIcqtgoEuszodI()
			{
				((OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).jRaYtHNVcykNMAbqOnSGaKIIGSEaA(base.isMemberElement);
			}

			internal void iOLOsebWNmbQbwGLDLaulbenSgxs(float P_0)
			{
				for (int i = 0; i < ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.LODtCJwLEBFFdekKvOQAcGWNoNrhA.Count; i++)
				{
					if (ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.LODtCJwLEBFFdekKvOQAcGWNoNrhA[i] is OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB rMQfTwHmgJLdQGIzhZOpzLrPBpgJB)
					{
						rMQfTwHmgJLdQGIzhZOpzLrPBpgJB.VPWZJxJyPrOYmxhQUKrwKcepGGGdA(P_0);
						rMQfTwHmgJLdQGIzhZOpzLrPBpgJB.oQGfjHJkbCQKodRETgOXZGbyQfIl = rMQfTwHmgJLdQGIzhZOpzLrPBpgJB.yYOUbwIbBcyPAQvjeAEXVsjzLAln;
						rMQfTwHmgJLdQGIzhZOpzLrPBpgJB.yYOUbwIbBcyPAQvjeAEXVsjzLAln = 0f;
						rMQfTwHmgJLdQGIzhZOpzLrPBpgJB.jRaYtHNVcykNMAbqOnSGaKIIGSEaA(base.isMemberElement);
					}
				}
			}

			internal float xAdhNpJZqNksTfCDJERsuTOMoYVSA(UpdateLoopType P_0, AxisCalibration P_1)
			{
				OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB rMQfTwHmgJLdQGIzhZOpzLrPBpgJB = (OsEuISpNkdfnNIHvUtHIavDtPcibA.RMQfTwHmgJLdQGIzhZOpzLrPBpgJB)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.LODtCJwLEBFFdekKvOQAcGWNoNrhA[(int)P_0];
				float result = P_1.GetCalibratedValue(rMQfTwHmgJLdQGIzhZOpzLrPBpgJB.cJcuFiGqILsQfQKYrbiPfelYgCKk, fHLnmlqZlxUGHumVxAlzXLaaqYid, P_1.deadZone, applySensitivity: false, applyInversion: true);
				if (P_1.applyRangeCalibration)
				{
					result = MathTools.Clamp(result, -1f, 1f);
				}
				return result;
			}
		}

		public sealed class Button : Element
		{
			internal class TpFRHAZDqhKnadPveTTupVCkWTwf : HaCAJRkznbyRXPNUuqdpPcxXLSJVA
			{
				public class SsFpsCFOWIUbZqxDdHaqmvTYHmBC : WHJOnpKGSYnQhsBBqqilENNrUZGe
				{
					public bool yYOUbwIbBcyPAQvjeAEXVsjzLAln;

					public bool oQGfjHJkbCQKodRETgOXZGbyQfIl;

					public ButtonStateRecorder inINMWMJaOkxHoXsTGElaKhNSZlJ;

					public ObpwRPtneCPuDKEjcrJFQeEdeQUi hQILoYGOoTCTqJfWzBIgMCIFVMlh;

					public SsFpsCFOWIUbZqxDdHaqmvTYHmBC()
					{
						inINMWMJaOkxHoXsTGElaKhNSZlJ = new ButtonStateRecorder();
						hQILoYGOoTCTqJfWzBIgMCIFVMlh = new ObpwRPtneCPuDKEjcrJFQeEdeQUi(0.3f);
					}

					public void tzfYrQaBdsWHACSbQzIxoLZqqtry(bool P_0)
					{
						if (oQGfjHJkbCQKodRETgOXZGbyQfIl != yYOUbwIbBcyPAQvjeAEXVsjzLAln)
						{
							oQGfjHJkbCQKodRETgOXZGbyQfIl = yYOUbwIbBcyPAQvjeAEXVsjzLAln;
						}
						if (yYOUbwIbBcyPAQvjeAEXVsjzLAln != P_0)
						{
							yYOUbwIbBcyPAQvjeAEXVsjzLAln = P_0;
						}
						inINMWMJaOkxHoXsTGElaKhNSZlJ.jRaYtHNVcykNMAbqOnSGaKIIGSEaA(P_0 && !oQGfjHJkbCQKodRETgOXZGbyQfIl, P_0, ReInput.unscaledTime);
						hQILoYGOoTCTqJfWzBIgMCIFVMlh.jRaYtHNVcykNMAbqOnSGaKIIGSEaA(0.3f, P_0 && !oQGfjHJkbCQKodRETgOXZGbyQfIl, P_0);
					}

					public override void jpwugzufXqktYbXkMYboQpqCbQgL()
					{
						yYOUbwIbBcyPAQvjeAEXVsjzLAln = false;
						oQGfjHJkbCQKodRETgOXZGbyQfIl = false;
						inINMWMJaOkxHoXsTGElaKhNSZlJ.jpwugzufXqktYbXkMYboQpqCbQgL();
						hQILoYGOoTCTqJfWzBIgMCIFVMlh.jpwugzufXqktYbXkMYboQpqCbQgL();
					}
				}

				public class vzEvjyeAjWuUAdBRGhiuiaQGYjjP : SsFpsCFOWIUbZqxDdHaqmvTYHmBC
				{
					public float RloGhVPhyAErIIkeQHiGbHXYGXEU;

					public float ugpQEmqRosuQsdtKNWsgsqFHggoFA;

					public void tzfYrQaBdsWHACSbQzIxoLZqqtry(float P_0)
					{
						if (ugpQEmqRosuQsdtKNWsgsqFHggoFA != RloGhVPhyAErIIkeQHiGbHXYGXEU)
						{
							ugpQEmqRosuQsdtKNWsgsqFHggoFA = RloGhVPhyAErIIkeQHiGbHXYGXEU;
						}
						if (RloGhVPhyAErIIkeQHiGbHXYGXEU != P_0)
						{
							RloGhVPhyAErIIkeQHiGbHXYGXEU = ((P_0 > 0.001f) ? P_0 : 0f);
						}
						tzfYrQaBdsWHACSbQzIxoLZqqtry(RloGhVPhyAErIIkeQHiGbHXYGXEU > 0f);
					}

					public override void jpwugzufXqktYbXkMYboQpqCbQgL()
					{
						base.jpwugzufXqktYbXkMYboQpqCbQgL();
						RloGhVPhyAErIIkeQHiGbHXYGXEU = 0f;
						ugpQEmqRosuQsdtKNWsgsqFHggoFA = 0f;
					}
				}

				public TpFRHAZDqhKnadPveTTupVCkWTwf(UpdateLoopSetting P_0, bool P_1)
					: base(P_0)
				{
					for (int i = 0; i < rDwgYRBSkOPRjVvObsjyXpnizJLT; i++)
					{
						if (P_1)
						{
							ReyFXLxUNcPvXNGMfpODOqFiLRfR[i] = new vzEvjyeAjWuUAdBRGhiuiaQGYjjP();
						}
						else
						{
							ReyFXLxUNcPvXNGMfpODOqFiLRfR[i] = new SsFpsCFOWIUbZqxDdHaqmvTYHmBC();
						}
					}
					SgJVPHrjFyHgCSEpxtgUijWhNNop = ReyFXLxUNcPvXNGMfpODOqFiLRfR[0];
				}

				public void PwywpCanYtGZzVZElnDKHSazZKlh(float P_0)
				{
					for (int i = 0; i < ReyFXLxUNcPvXNGMfpODOqFiLRfR.Length; i++)
					{
						((SsFpsCFOWIUbZqxDdHaqmvTYHmBC)ReyFXLxUNcPvXNGMfpODOqFiLRfR[i]).hQILoYGOoTCTqJfWzBIgMCIFVMlh.UwqxcZUlQStsuTcYTekDKddXUYTk(P_0);
					}
				}

				public void tXjsduWsTMKXfQuygXHOIMPpsdsD()
				{
					for (int i = 0; i < ReyFXLxUNcPvXNGMfpODOqFiLRfR.Length; i++)
					{
						((SsFpsCFOWIUbZqxDdHaqmvTYHmBC)ReyFXLxUNcPvXNGMfpODOqFiLRfR[i]).hQILoYGOoTCTqJfWzBIgMCIFVMlh.UwqxcZUlQStsuTcYTekDKddXUYTk(0.3f);
					}
				}
			}

			internal readonly bool rWaOJwvBsTPxWUGJsQKQYZrXeCRj;

			internal readonly HardwareButtonInfo uTuDqdqmRErAFbVwkHOWZNOolvqi;

			public bool valuePrev
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return false;
					}
					return ((TpFRHAZDqhKnadPveTTupVCkWTwf.SsFpsCFOWIUbZqxDdHaqmvTYHmBC)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).oQGfjHJkbCQKodRETgOXZGbyQfIl;
				}
			}

			public bool value
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return false;
					}
					return ((TpFRHAZDqhKnadPveTTupVCkWTwf.SsFpsCFOWIUbZqxDdHaqmvTYHmBC)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).yYOUbwIbBcyPAQvjeAEXVsjzLAln;
				}
			}

			public float pressure
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0f;
					}
					if (!rWaOJwvBsTPxWUGJsQKQYZrXeCRj)
					{
						if (!((TpFRHAZDqhKnadPveTTupVCkWTwf.SsFpsCFOWIUbZqxDdHaqmvTYHmBC)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).yYOUbwIbBcyPAQvjeAEXVsjzLAln)
						{
							return 0f;
						}
						return 1f;
					}
					return ((TpFRHAZDqhKnadPveTTupVCkWTwf.vzEvjyeAjWuUAdBRGhiuiaQGYjjP)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).RloGhVPhyAErIIkeQHiGbHXYGXEU;
				}
			}

			public float pressurePrev
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0f;
					}
					if (!rWaOJwvBsTPxWUGJsQKQYZrXeCRj)
					{
						if (!((TpFRHAZDqhKnadPveTTupVCkWTwf.SsFpsCFOWIUbZqxDdHaqmvTYHmBC)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).oQGfjHJkbCQKodRETgOXZGbyQfIl)
						{
							return 0f;
						}
						return 1f;
					}
					return ((TpFRHAZDqhKnadPveTTupVCkWTwf.vzEvjyeAjWuUAdBRGhiuiaQGYjjP)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).ugpQEmqRosuQsdtKNWsgsqFHggoFA;
				}
			}

			public bool isPressureSensitive
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return false;
					}
					return rWaOJwvBsTPxWUGJsQKQYZrXeCRj;
				}
			}

			public bool justPressed
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return false;
					}
					if (!((TpFRHAZDqhKnadPveTTupVCkWTwf.SsFpsCFOWIUbZqxDdHaqmvTYHmBC)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).oQGfjHJkbCQKodRETgOXZGbyQfIl && ((TpFRHAZDqhKnadPveTTupVCkWTwf.SsFpsCFOWIUbZqxDdHaqmvTYHmBC)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).yYOUbwIbBcyPAQvjeAEXVsjzLAln)
					{
						return true;
					}
					return false;
				}
			}

			public bool justReleased
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return false;
					}
					if (((TpFRHAZDqhKnadPveTTupVCkWTwf.SsFpsCFOWIUbZqxDdHaqmvTYHmBC)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).oQGfjHJkbCQKodRETgOXZGbyQfIl && !((TpFRHAZDqhKnadPveTTupVCkWTwf.SsFpsCFOWIUbZqxDdHaqmvTYHmBC)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).yYOUbwIbBcyPAQvjeAEXVsjzLAln)
					{
						return true;
					}
					return false;
				}
			}

			public bool justChangedState
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return false;
					}
					if (((TpFRHAZDqhKnadPveTTupVCkWTwf.SsFpsCFOWIUbZqxDdHaqmvTYHmBC)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).oQGfjHJkbCQKodRETgOXZGbyQfIl != ((TpFRHAZDqhKnadPveTTupVCkWTwf.SsFpsCFOWIUbZqxDdHaqmvTYHmBC)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).yYOUbwIbBcyPAQvjeAEXVsjzLAln)
					{
						return true;
					}
					return false;
				}
			}

			public bool doublePressedAndHeld
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return false;
					}
					return ((TpFRHAZDqhKnadPveTTupVCkWTwf.SsFpsCFOWIUbZqxDdHaqmvTYHmBC)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).hQILoYGOoTCTqJfWzBIgMCIFVMlh.lTCBRXelLSvizDqDRxqlnXgCFFmx;
				}
			}

			public bool justDoublePressed
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return false;
					}
					if (!justPressed)
					{
						return false;
					}
					return ((TpFRHAZDqhKnadPveTTupVCkWTwf.SsFpsCFOWIUbZqxDdHaqmvTYHmBC)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).hQILoYGOoTCTqJfWzBIgMCIFVMlh.lTCBRXelLSvizDqDRxqlnXgCFFmx;
				}
			}

			public double timePressed
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0.0;
					}
					return ((TpFRHAZDqhKnadPveTTupVCkWTwf.SsFpsCFOWIUbZqxDdHaqmvTYHmBC)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).inINMWMJaOkxHoXsTGElaKhNSZlJ.qhxaXWyGXvRZvWYjPccsfJPvxIfG;
				}
			}

			public double timeUnpressed
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0.0;
					}
					return ((TpFRHAZDqhKnadPveTTupVCkWTwf.SsFpsCFOWIUbZqxDdHaqmvTYHmBC)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).inINMWMJaOkxHoXsTGElaKhNSZlJ.lHFOKiYyNYbiKbJxZDdYyGlrfuIf;
				}
			}

			public double lastTimePressed
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0.0;
					}
					return ((TpFRHAZDqhKnadPveTTupVCkWTwf.SsFpsCFOWIUbZqxDdHaqmvTYHmBC)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).inINMWMJaOkxHoXsTGElaKhNSZlJ.FxvCnBjheKnETBGpeJsvIacEqnEAe;
				}
			}

			public double lastTimeUnpressed
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0.0;
					}
					return ((TpFRHAZDqhKnadPveTTupVCkWTwf.SsFpsCFOWIUbZqxDdHaqmvTYHmBC)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).inINMWMJaOkxHoXsTGElaKhNSZlJ.SfNjqREZcHnifSmcJlmWxazxAJsyA;
				}
			}

			public double lastTimeStateChanged
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0.0;
					}
					return ((TpFRHAZDqhKnadPveTTupVCkWTwf.SsFpsCFOWIUbZqxDdHaqmvTYHmBC)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).inINMWMJaOkxHoXsTGElaKhNSZlJ.xHHxHsEGXRLTZfqnZFGxafTAAQas;
				}
			}

			internal ButtonStateFlags UFCGiNgJQTBJXbozkINBMenjSxAW
			{
				get
				{
					TpFRHAZDqhKnadPveTTupVCkWTwf.SsFpsCFOWIUbZqxDdHaqmvTYHmBC ssFpsCFOWIUbZqxDdHaqmvTYHmBC = (TpFRHAZDqhKnadPveTTupVCkWTwf.SsFpsCFOWIUbZqxDdHaqmvTYHmBC)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop;
					ButtonStateFlags buttonStateFlags = ButtonStateFlags.Off;
					if (ssFpsCFOWIUbZqxDdHaqmvTYHmBC.yYOUbwIbBcyPAQvjeAEXVsjzLAln)
					{
						buttonStateFlags |= ButtonStateFlags.On;
						if (!ssFpsCFOWIUbZqxDdHaqmvTYHmBC.oQGfjHJkbCQKodRETgOXZGbyQfIl)
						{
							buttonStateFlags |= ButtonStateFlags.Down;
						}
					}
					else if (ssFpsCFOWIUbZqxDdHaqmvTYHmBC.oQGfjHJkbCQKodRETgOXZGbyQfIl)
					{
						buttonStateFlags |= ButtonStateFlags.Up;
					}
					return buttonStateFlags;
				}
			}

			internal Button(Controller P_0, int P_1, string P_2, HardwareButtonInfo P_3)
				: base(P_0, P_1, P_2, ControllerElementType.Button)
			{
				uTuDqdqmRErAFbVwkHOWZNOolvqi = P_3;
				ojyRDEyxfmuKcQCdQAQNJvBaHFVfA = new TpFRHAZDqhKnadPveTTupVCkWTwf(ReInput.configVars.updateLoop, false);
			}

			internal Button(Controller P_0, int P_1, string P_2, bool P_3, HardwareButtonInfo P_4)
				: base(P_0, P_1, P_2, ControllerElementType.Button)
			{
				uTuDqdqmRErAFbVwkHOWZNOolvqi = P_4;
				rWaOJwvBsTPxWUGJsQKQYZrXeCRj = P_3;
				ojyRDEyxfmuKcQCdQAQNJvBaHFVfA = new TpFRHAZDqhKnadPveTTupVCkWTwf(ReInput.configVars.updateLoop, P_3);
			}

			public bool DoublePressedAndHeld(float speed)
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return false;
				}
				if (speed <= 0f)
				{
					return ((TpFRHAZDqhKnadPveTTupVCkWTwf.SsFpsCFOWIUbZqxDdHaqmvTYHmBC)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).hQILoYGOoTCTqJfWzBIgMCIFVMlh.lTCBRXelLSvizDqDRxqlnXgCFFmx;
				}
				return ((TpFRHAZDqhKnadPveTTupVCkWTwf.SsFpsCFOWIUbZqxDdHaqmvTYHmBC)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).inINMWMJaOkxHoXsTGElaKhNSZlJ.rbzAqQgquJTHZQzYluEcBrlbXpmuA(speed);
			}

			public bool JustDoublePressed(float speed)
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return false;
				}
				if (!justPressed)
				{
					return false;
				}
				if (speed <= 0f)
				{
					return ((TpFRHAZDqhKnadPveTTupVCkWTwf.SsFpsCFOWIUbZqxDdHaqmvTYHmBC)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).hQILoYGOoTCTqJfWzBIgMCIFVMlh.lTCBRXelLSvizDqDRxqlnXgCFFmx;
				}
				return ((TpFRHAZDqhKnadPveTTupVCkWTwf.SsFpsCFOWIUbZqxDdHaqmvTYHmBC)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).inINMWMJaOkxHoXsTGElaKhNSZlJ.rbzAqQgquJTHZQzYluEcBrlbXpmuA(speed);
			}

			internal void tzfYrQaBdsWHACSbQzIxoLZqqtry(UpdateLoopType P_0, int P_1, ControllerDataUpdater P_2)
			{
				if (ojyRDEyxfmuKcQCdQAQNJvBaHFVfA != null && ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.yVrZIXjSxHcfgFdpNDzhuqBYIrTH != (int)P_0)
				{
					ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.PiEdbgjMHSsksYjRKSGckYZEsfAE = P_0;
				}
				if (rWaOJwvBsTPxWUGJsQKQYZrXeCRj)
				{
					((TpFRHAZDqhKnadPveTTupVCkWTwf.vzEvjyeAjWuUAdBRGhiuiaQGYjjP)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).tzfYrQaBdsWHACSbQzIxoLZqqtry(P_2.buttonPressureValues[P_1]);
				}
				else
				{
					((TpFRHAZDqhKnadPveTTupVCkWTwf.SsFpsCFOWIUbZqxDdHaqmvTYHmBC)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).tzfYrQaBdsWHACSbQzIxoLZqqtry(P_2.buttonValues[P_1]);
				}
			}

			internal void qFFjteInwJBtWaXvxhnWFfmOLOfx(UpdateLoopType P_0)
			{
				if (ojyRDEyxfmuKcQCdQAQNJvBaHFVfA != null && ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.yVrZIXjSxHcfgFdpNDzhuqBYIrTH != (int)P_0)
				{
					ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.PiEdbgjMHSsksYjRKSGckYZEsfAE = P_0;
				}
				if (rWaOJwvBsTPxWUGJsQKQYZrXeCRj)
				{
					((TpFRHAZDqhKnadPveTTupVCkWTwf.vzEvjyeAjWuUAdBRGhiuiaQGYjjP)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).tzfYrQaBdsWHACSbQzIxoLZqqtry(0f);
				}
				else
				{
					((TpFRHAZDqhKnadPveTTupVCkWTwf.SsFpsCFOWIUbZqxDdHaqmvTYHmBC)ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.SgJVPHrjFyHgCSEpxtgUijWhNNop).tzfYrQaBdsWHACSbQzIxoLZqqtry(false);
				}
			}

			internal void iOLOsebWNmbQbwGLDLaulbenSgxs()
			{
				for (int i = 0; i < ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.LODtCJwLEBFFdekKvOQAcGWNoNrhA.Count; i++)
				{
					HaCAJRkznbyRXPNUuqdpPcxXLSJVA.WHJOnpKGSYnQhsBBqqilENNrUZGe wHJOnpKGSYnQhsBBqqilENNrUZGe = ojyRDEyxfmuKcQCdQAQNJvBaHFVfA.LODtCJwLEBFFdekKvOQAcGWNoNrhA[i];
					if (wHJOnpKGSYnQhsBBqqilENNrUZGe != null)
					{
						if (rWaOJwvBsTPxWUGJsQKQYZrXeCRj)
						{
							((TpFRHAZDqhKnadPveTTupVCkWTwf.vzEvjyeAjWuUAdBRGhiuiaQGYjjP)wHJOnpKGSYnQhsBBqqilENNrUZGe).tzfYrQaBdsWHACSbQzIxoLZqqtry(0f);
						}
						else
						{
							((TpFRHAZDqhKnadPveTTupVCkWTwf.SsFpsCFOWIUbZqxDdHaqmvTYHmBC)wHJOnpKGSYnQhsBBqqilENNrUZGe).tzfYrQaBdsWHACSbQzIxoLZqqtry(false);
						}
					}
				}
			}
		}

		public abstract class CompoundElement
		{
			private class WRiagWCOpOECzQDWEWNxarJXbHmM
			{
				public readonly Element IeBiSvPSlvevRBzqkYknNspTYxmIA;

				public readonly int fpRaWzdHfXPowZWkJxwTZPqeXOGK;

				public WRiagWCOpOECzQDWEWNxarJXbHmM(Element P_0, int P_1)
				{
					IeBiSvPSlvevRBzqkYknNspTYxmIA = P_0;
					fpRaWzdHfXPowZWkJxwTZPqeXOGK = P_1;
				}
			}

			private int NPuaUGjYHfmjnFZazhpMoRbThWzG;

			private string hEJFspqWqeBOdedbqHNRpPNEbfBG;

			private CompoundControllerElementType TIpqQbLCpFnyVWkQInNfNRXErYkK;

			private int pQhFxcuPrcDhCdBqMHKrOfeGZOVtA;

			private WRiagWCOpOECzQDWEWNxarJXbHmM[] cnRAHhGwbBYXolTSTEoaPYuDorOJA;

			private Controller oBPMdftKtJLWDwKjOKgLuimxBfUKA;

			internal readonly int QajDFFaomlkLHzaostfWYpGUioys;

			public int id
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return -1;
					}
					return NPuaUGjYHfmjnFZazhpMoRbThWzG;
				}
			}

			public string name
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return string.Empty;
					}
					return hEJFspqWqeBOdedbqHNRpPNEbfBG;
				}
			}

			public CompoundControllerElementType type
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return CompoundControllerElementType.Axis2D;
					}
					return TIpqQbLCpFnyVWkQInNfNRXErYkK;
				}
			}

			public bool hasElements
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return false;
					}
					return pQhFxcuPrcDhCdBqMHKrOfeGZOVtA > 0;
				}
			}

			public int elementCount
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0;
					}
					return pQhFxcuPrcDhCdBqMHKrOfeGZOVtA;
				}
			}

			public abstract int elementCapacity { get; }

			public ControllerElementIdentifier elementIdentifier
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					ControllerElementIdentifier elementIdentifierById = oBPMdftKtJLWDwKjOKgLuimxBfUKA.GetElementIdentifierById(NPuaUGjYHfmjnFZazhpMoRbThWzG);
					if (elementIdentifierById == null)
					{
						return ControllerElementIdentifier.BlankReadOnly;
					}
					return elementIdentifierById;
				}
			}

			internal CompoundElement(Controller P_0, int P_1, string P_2, CompoundControllerElementType P_3)
			{
				oBPMdftKtJLWDwKjOKgLuimxBfUKA = P_0;
				NPuaUGjYHfmjnFZazhpMoRbThWzG = P_1;
				hEJFspqWqeBOdedbqHNRpPNEbfBG = P_2;
				TIpqQbLCpFnyVWkQInNfNRXErYkK = P_3;
				cnRAHhGwbBYXolTSTEoaPYuDorOJA = new WRiagWCOpOECzQDWEWNxarJXbHmM[elementCapacity];
				QajDFFaomlkLHzaostfWYpGUioys = ReInput.id;
			}

			internal Element pAGcyUYRwJwYiiycWKSARieMOZIo(int P_0)
			{
				if (P_0 < 0 || P_0 >= cnRAHhGwbBYXolTSTEoaPYuDorOJA.Length)
				{
					return null;
				}
				if (cnRAHhGwbBYXolTSTEoaPYuDorOJA[P_0] == null)
				{
					return null;
				}
				return cnRAHhGwbBYXolTSTEoaPYuDorOJA[P_0].IeBiSvPSlvevRBzqkYknNspTYxmIA;
			}

			internal _0001 pAGcyUYRwJwYiiycWKSARieMOZIo<_0001>(int P_0) where _0001 : Element
			{
				if (P_0 < 0 || P_0 >= cnRAHhGwbBYXolTSTEoaPYuDorOJA.Length)
				{
					return null;
				}
				if (cnRAHhGwbBYXolTSTEoaPYuDorOJA[P_0] == null)
				{
					return null;
				}
				return cnRAHhGwbBYXolTSTEoaPYuDorOJA[P_0].IeBiSvPSlvevRBzqkYknNspTYxmIA as _0001;
			}

			internal _0001 beInWrXNGVmJotqqTnEvyCeXBSVkA<_0001>(int P_0, out int P_1) where _0001 : Element
			{
				P_1 = -1;
				if (P_0 < 0 || P_0 >= cnRAHhGwbBYXolTSTEoaPYuDorOJA.Length)
				{
					return null;
				}
				if (cnRAHhGwbBYXolTSTEoaPYuDorOJA[P_0] == null)
				{
					return null;
				}
				P_1 = cnRAHhGwbBYXolTSTEoaPYuDorOJA[P_0].fpRaWzdHfXPowZWkJxwTZPqeXOGK;
				return cnRAHhGwbBYXolTSTEoaPYuDorOJA[P_0].IeBiSvPSlvevRBzqkYknNspTYxmIA as _0001;
			}

			internal bool JymOYvJCaUZfrfNvqSliYMwhgCGz(Element P_0, int P_1)
			{
				if (P_0 == null)
				{
					return false;
				}
				if (pQhFxcuPrcDhCdBqMHKrOfeGZOVtA >= elementCapacity)
				{
					Logger.LogWarning("Cannot add element! This Compound Element already contains the maximum number of elements.");
					return false;
				}
				if (P_0.isMemberElement)
				{
					Logger.LogWarning("Cannot add element! The element you are trying to add is already a member of another compound element.");
					return false;
				}
				if (yauKhkbBGjIEDCJSIEtSjLnuhXfUA(P_0) >= 0)
				{
					Logger.LogWarning("Cannot add element! This Compound Element already contains the element you are trying to add.");
					return false;
				}
				int num = OigIWkXcFCehUnpcqaGQJTjxwXQP();
				if (num < 0)
				{
					Logger.LogWarning("Cannot add element! This Compound Element already contains the maximum number of elements.");
					return false;
				}
				return gxZurnYvCAtFnmjcAkIuRLDpjkFU(P_0, P_1, num);
			}

			internal bool rbmxkJwGJLFTaFjJNLmzTpEtNGrz(Element P_0)
			{
				if (P_0 == null)
				{
					return false;
				}
				if (pQhFxcuPrcDhCdBqMHKrOfeGZOVtA == 0)
				{
					Logger.LogWarning("Cannot remove element! This Compound Element has no elements.");
					return false;
				}
				int num = yauKhkbBGjIEDCJSIEtSjLnuhXfUA(P_0);
				if (num < 0)
				{
					Logger.LogWarning("Cannot remove element! This Compound Element does not contain the element you are trying to remove.");
					return false;
				}
				return ZJxxvqtwqcixEDzayCnSDelWZfvV(num);
			}

			internal void bMfHJKzphtBxXbRoZiuJokyYNULjA()
			{
				for (int i = 0; i < cnRAHhGwbBYXolTSTEoaPYuDorOJA.Length; i++)
				{
					ZJxxvqtwqcixEDzayCnSDelWZfvV(i);
				}
				pQhFxcuPrcDhCdBqMHKrOfeGZOVtA = 0;
			}

			private int yauKhkbBGjIEDCJSIEtSjLnuhXfUA(Element P_0)
			{
				if (P_0 == null)
				{
					return -1;
				}
				for (int i = 0; i < cnRAHhGwbBYXolTSTEoaPYuDorOJA.Length; i++)
				{
					if (cnRAHhGwbBYXolTSTEoaPYuDorOJA[i] != null && cnRAHhGwbBYXolTSTEoaPYuDorOJA[i].IeBiSvPSlvevRBzqkYknNspTYxmIA == P_0)
					{
						return i;
					}
				}
				return -1;
			}

			private bool gxZurnYvCAtFnmjcAkIuRLDpjkFU(Element P_0, int P_1, int P_2)
			{
				if (P_2 < 0 || P_2 >= cnRAHhGwbBYXolTSTEoaPYuDorOJA.Length)
				{
					return false;
				}
				if (cnRAHhGwbBYXolTSTEoaPYuDorOJA[P_2] != null)
				{
					return false;
				}
				cnRAHhGwbBYXolTSTEoaPYuDorOJA[P_2] = new WRiagWCOpOECzQDWEWNxarJXbHmM(P_0, P_1);
				P_0.esISkaZLgvXMyMkBOYdQGoXSYVWq(this);
				pQhFxcuPrcDhCdBqMHKrOfeGZOVtA++;
				return true;
			}

			private bool ZJxxvqtwqcixEDzayCnSDelWZfvV(int P_0)
			{
				if (P_0 < 0 || P_0 >= cnRAHhGwbBYXolTSTEoaPYuDorOJA.Length)
				{
					return false;
				}
				if (cnRAHhGwbBYXolTSTEoaPYuDorOJA[P_0] == null)
				{
					return false;
				}
				if (cnRAHhGwbBYXolTSTEoaPYuDorOJA[P_0].IeBiSvPSlvevRBzqkYknNspTYxmIA != null)
				{
					cnRAHhGwbBYXolTSTEoaPYuDorOJA[P_0].IeBiSvPSlvevRBzqkYknNspTYxmIA.TfcJjzJPZRzfwXAHvqJqpCeijFbb(this);
				}
				cnRAHhGwbBYXolTSTEoaPYuDorOJA[P_0] = null;
				pQhFxcuPrcDhCdBqMHKrOfeGZOVtA--;
				return true;
			}

			private int OigIWkXcFCehUnpcqaGQJTjxwXQP()
			{
				for (int i = 0; i < cnRAHhGwbBYXolTSTEoaPYuDorOJA.Length; i++)
				{
					if (cnRAHhGwbBYXolTSTEoaPYuDorOJA[i] == null)
					{
						return i;
					}
				}
				return -1;
			}
		}

		public sealed class Axis2D : CompoundElement
		{
			private const int nvWEjxsmdBcbTgqxtlHUFCZaIUcDA = 2;

			private CalibrationMap WWanLzIVGDvxddPwuPDpbBeJkJVw;

			public override int elementCapacity => 2;

			public Axis xAxis
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return pAGcyUYRwJwYiiycWKSARieMOZIo<Axis>(0);
				}
			}

			public Axis yAxis
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return pAGcyUYRwJwYiiycWKSARieMOZIo<Axis>(1);
				}
			}

			public Vector2 value
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return Vector2.zero;
					}
					return HlNnVRnpSGYkKeCqXWuzdMfCFLbb();
				}
			}

			public Vector2 valuePrev
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return Vector2.zero;
					}
					return RluBPsnKdmzWOOtCjtpMCVvtngwm();
				}
			}

			public Vector2 valueRaw
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return Vector2.zero;
					}
					return new Vector2((xAxis != null) ? xAxis.valueRaw : 0f, (yAxis != null) ? yAxis.valueRaw : 0f);
				}
			}

			public Vector2 valueRawPrev
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return Vector2.zero;
					}
					return new Vector2((xAxis != null) ? xAxis.valueRawPrev : 0f, (yAxis != null) ? yAxis.valueRawPrev : 0f);
				}
			}

			internal Axis2D(Controller P_0, int P_1, string P_2, Axis P_3, Axis P_4, int P_5, int P_6, CalibrationMap P_7)
				: base(P_0, P_1, P_2, CompoundControllerElementType.Axis2D)
			{
				JymOYvJCaUZfrfNvqSliYMwhgCGz(P_3, P_5);
				JymOYvJCaUZfrfNvqSliYMwhgCGz(P_4, P_6);
				WWanLzIVGDvxddPwuPDpbBeJkJVw = P_7;
			}

			internal void SiJfFLPgexWkpZJLIICzdmwvQQeLA()
			{
				Vector2 vector = value;
				if (xAxis != null)
				{
					xAxis.pHaZQUgmODsRYrJJZXIDpgKsQFlG(vector.x);
				}
				if (yAxis != null)
				{
					yAxis.pHaZQUgmODsRYrJJZXIDpgKsQFlG(vector.y);
				}
			}

			private Vector2 HlNnVRnpSGYkKeCqXWuzdMfCFLbb()
			{
				if (WWanLzIVGDvxddPwuPDpbBeJkJVw == null)
				{
					return default(Vector2);
				}
				int xAxisIndex;
				Axis axis = beInWrXNGVmJotqqTnEvyCeXBSVkA<Axis>(0, out xAxisIndex);
				int yAxisIndex;
				Axis axis2 = beInWrXNGVmJotqqTnEvyCeXBSVkA<Axis>(1, out yAxisIndex);
				DeadZone2DType defaultJoystickAxis2DDeadZoneType = ReInput.configVars.defaultJoystickAxis2DDeadZoneType;
				AxisSensitivity2DType defaultJoystickAxis2DSensitivityType = ReInput.configVars.defaultJoystickAxis2DSensitivityType;
				float valueRawX = axis?.valueRaw ?? 0f;
				float valueRawY = axis2?.valueRaw ?? 0f;
				return WWanLzIVGDvxddPwuPDpbBeJkJVw.GetCalibrated2DValue(xAxisIndex, yAxisIndex, valueRawX, valueRawY, defaultJoystickAxis2DDeadZoneType, defaultJoystickAxis2DSensitivityType);
			}

			private Vector2 RluBPsnKdmzWOOtCjtpMCVvtngwm()
			{
				if (WWanLzIVGDvxddPwuPDpbBeJkJVw == null)
				{
					return default(Vector2);
				}
				int xAxisIndex;
				Axis axis = beInWrXNGVmJotqqTnEvyCeXBSVkA<Axis>(0, out xAxisIndex);
				int yAxisIndex;
				Axis axis2 = beInWrXNGVmJotqqTnEvyCeXBSVkA<Axis>(1, out yAxisIndex);
				DeadZone2DType defaultJoystickAxis2DDeadZoneType = ReInput.configVars.defaultJoystickAxis2DDeadZoneType;
				AxisSensitivity2DType defaultJoystickAxis2DSensitivityType = ReInput.configVars.defaultJoystickAxis2DSensitivityType;
				float valueRawX = axis?.valueRawPrev ?? 0f;
				float valueRawY = axis2?.valueRawPrev ?? 0f;
				return WWanLzIVGDvxddPwuPDpbBeJkJVw.GetCalibrated2DValue(xAxisIndex, yAxisIndex, valueRawX, valueRawY, defaultJoystickAxis2DDeadZoneType, defaultJoystickAxis2DSensitivityType);
			}
		}

		public sealed class Hat : CompoundElement
		{
			private const int nvWEjxsmdBcbTgqxtlHUFCZaIUcDA = 8;

			private const int OoAfvUSvrOdiuFWHaBoJmkZngVDyA = 0;

			private const int vMCKNfvEliCmtYazEcmEZYGRkOsk = 1;

			private const int YZXfgONLSjagsmoVsNWNyhYGCvkr = 2;

			private const int nXFSzfNtTLqYodRyamnbYTWigRGI = 3;

			private const int GRhbSvDhmkWOHqTUknCfjCUnWQixA = 4;

			private const int WUvwjWHhPFOjbjkMVGGmofauIMqU = 5;

			private const int MbVPTbTmvHzYPFQPCOzujDwKDpQhA = 6;

			private const int pQddZZMCDntRVzZJbqpDqGlBlYik = 7;

			private readonly int QJpgCvdDxIIaukCWckDkuElRUIGE;

			private readonly Button[] YXuCILbbSuGPNMGgRTZoPeQmcMPsA;

			private readonly ReadOnlyCollection<Button> fFolTZgpmbrZOPeygkjnJAKrjhrgA;

			private readonly int[] ixLyEzlxiiWmIBBdXholHAADiAVIA;

			private bool UvOkJyvglmSNrJTZHeZebuPGCOeo;

			public override int elementCapacity => 8;

			public bool force4Way
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return false;
					}
					return UvOkJyvglmSNrJTZHeZebuPGCOeo;
				}
				set
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					}
					else
					{
						UvOkJyvglmSNrJTZHeZebuPGCOeo = value;
					}
				}
			}

			public int directionCount
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0;
					}
					return QJpgCvdDxIIaukCWckDkuElRUIGE;
				}
			}

			public IList<Button> Buttons
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return EmptyObjects<Button>.EmptyReadOnlyIListT;
					}
					return fFolTZgpmbrZOPeygkjnJAKrjhrgA;
				}
			}

			public Button buttonUp
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return pAGcyUYRwJwYiiycWKSARieMOZIo<Button>(0);
				}
			}

			public Button buttonRight
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return pAGcyUYRwJwYiiycWKSARieMOZIo<Button>(2);
				}
			}

			public Button buttonDown
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return pAGcyUYRwJwYiiycWKSARieMOZIo<Button>(4);
				}
			}

			public Button buttonLeft
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return pAGcyUYRwJwYiiycWKSARieMOZIo<Button>(6);
				}
			}

			public Button buttonUpRight
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return pAGcyUYRwJwYiiycWKSARieMOZIo<Button>(1);
				}
			}

			public Button buttonDownRight
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return pAGcyUYRwJwYiiycWKSARieMOZIo<Button>(3);
				}
			}

			public Button buttonDownLeft
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return pAGcyUYRwJwYiiycWKSARieMOZIo<Button>(5);
				}
			}

			public Button buttonUpLeft
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return pAGcyUYRwJwYiiycWKSARieMOZIo<Button>(7);
				}
			}

			internal Hat(Controller P_0, int P_1, string P_2, Button[] P_3, int[] P_4)
				: base(P_0, P_1, P_2, CompoundControllerElementType.Hat)
			{
				int num = ((P_3 != null) ? P_3.Length : 0);
				if (num != ((P_4 != null) ? P_4.Length : 0))
				{
					throw new ArgumentException("button.Length must equal buttonIndices.Length!");
				}
				if (num != 0 && num != 4 && num != 8)
				{
					throw new ArgumentException("button.Length must be 0, 4, or 8! Length: " + num);
				}
				for (int i = 0; i < num; i++)
				{
					JymOYvJCaUZfrfNvqSliYMwhgCGz(P_3[i], P_4[i]);
				}
				YXuCILbbSuGPNMGgRTZoPeQmcMPsA = P_3;
				ixLyEzlxiiWmIBBdXholHAADiAVIA = P_4;
				QJpgCvdDxIIaukCWckDkuElRUIGE = num;
				fFolTZgpmbrZOPeygkjnJAKrjhrgA = new ReadOnlyCollection<Button>(P_3);
			}

			internal void SiJfFLPgexWkpZJLIICzdmwvQQeLA(UpdateLoopType P_0, ControllerDataUpdater P_1)
			{
				if (QJpgCvdDxIIaukCWckDkuElRUIGE == 0)
				{
					return;
				}
				if (QJpgCvdDxIIaukCWckDkuElRUIGE == 8 && (UvOkJyvglmSNrJTZHeZebuPGCOeo || ReInput.configVars.force4WayHats))
				{
					xHACmeLUReZxqlAWUVNNYYcqMYSQ(YXuCILbbSuGPNMGgRTZoPeQmcMPsA[0], ixLyEzlxiiWmIBBdXholHAADiAVIA[0], ixLyEzlxiiWmIBBdXholHAADiAVIA[7], ixLyEzlxiiWmIBBdXholHAADiAVIA[1], P_0, P_1);
					xHACmeLUReZxqlAWUVNNYYcqMYSQ(YXuCILbbSuGPNMGgRTZoPeQmcMPsA[2], ixLyEzlxiiWmIBBdXholHAADiAVIA[2], ixLyEzlxiiWmIBBdXholHAADiAVIA[1], ixLyEzlxiiWmIBBdXholHAADiAVIA[3], P_0, P_1);
					xHACmeLUReZxqlAWUVNNYYcqMYSQ(YXuCILbbSuGPNMGgRTZoPeQmcMPsA[4], ixLyEzlxiiWmIBBdXholHAADiAVIA[4], ixLyEzlxiiWmIBBdXholHAADiAVIA[5], ixLyEzlxiiWmIBBdXholHAADiAVIA[3], P_0, P_1);
					xHACmeLUReZxqlAWUVNNYYcqMYSQ(YXuCILbbSuGPNMGgRTZoPeQmcMPsA[6], ixLyEzlxiiWmIBBdXholHAADiAVIA[6], ixLyEzlxiiWmIBBdXholHAADiAVIA[5], ixLyEzlxiiWmIBBdXholHAADiAVIA[7], P_0, P_1);
					etArhqZkTLmsvFIaqtJzWzoURgho(YXuCILbbSuGPNMGgRTZoPeQmcMPsA[1], ixLyEzlxiiWmIBBdXholHAADiAVIA[1], P_0, P_1);
					etArhqZkTLmsvFIaqtJzWzoURgho(YXuCILbbSuGPNMGgRTZoPeQmcMPsA[3], ixLyEzlxiiWmIBBdXholHAADiAVIA[3], P_0, P_1);
					etArhqZkTLmsvFIaqtJzWzoURgho(YXuCILbbSuGPNMGgRTZoPeQmcMPsA[5], ixLyEzlxiiWmIBBdXholHAADiAVIA[5], P_0, P_1);
					etArhqZkTLmsvFIaqtJzWzoURgho(YXuCILbbSuGPNMGgRTZoPeQmcMPsA[7], ixLyEzlxiiWmIBBdXholHAADiAVIA[7], P_0, P_1);
					return;
				}
				for (int i = 0; i < YXuCILbbSuGPNMGgRTZoPeQmcMPsA.Length; i++)
				{
					if (YXuCILbbSuGPNMGgRTZoPeQmcMPsA[i] != null)
					{
						YXuCILbbSuGPNMGgRTZoPeQmcMPsA[i].tzfYrQaBdsWHACSbQzIxoLZqqtry(P_0, ixLyEzlxiiWmIBBdXholHAADiAVIA[i], P_1);
					}
				}
			}

			private void xHACmeLUReZxqlAWUVNNYYcqMYSQ(Button P_0, int P_1, int P_2, int P_3, UpdateLoopType P_4, ControllerDataUpdater P_5)
			{
				if (P_0 == null || P_1 < 0 || P_1 >= P_5.buttonCount)
				{
					return;
				}
				if (!P_0.isPressureSensitive)
				{
					if (P_2 >= 0 && P_2 < P_5.buttonCount)
					{
						ref bool reference = ref P_5.buttonValues[P_1];
						reference |= P_5.buttonValues[P_2];
					}
					if (P_3 >= 0 && P_3 < P_5.buttonCount)
					{
						ref bool reference2 = ref P_5.buttonValues[P_1];
						reference2 |= P_5.buttonValues[P_3];
					}
				}
				else
				{
					P_5.buttonPressureValues[P_1] = MathTools.MaxMagnitude(P_5.buttonPressureValues[P_1], MathTools.MaxMagnitude((P_2 >= 0 && P_2 < P_5.buttonCount) ? P_5.buttonPressureValues[P_2] : 0f, (P_3 >= 0 && P_3 < P_5.buttonCount) ? P_5.buttonPressureValues[P_3] : 0f));
				}
				P_0.tzfYrQaBdsWHACSbQzIxoLZqqtry(P_4, P_1, P_5);
			}

			private void etArhqZkTLmsvFIaqtJzWzoURgho(Button P_0, int P_1, UpdateLoopType P_2, ControllerDataUpdater P_3)
			{
				if (P_0 != null && P_1 >= 0 && P_1 < P_3.buttonCount)
				{
					if (!P_0.isPressureSensitive)
					{
						P_3.buttonValues[P_1] = false;
					}
					else
					{
						P_3.buttonPressureValues[P_1] = 0f;
					}
					P_0.tzfYrQaBdsWHACSbQzIxoLZqqtry(P_2, P_1, P_3);
				}
			}
		}

		[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
		public abstract class Extension
		{
			private Controller oBPMdftKtJLWDwKjOKgLuimxBfUKA;

			private IControllerExtensionSource eyyVWBZaxsCJCQMmwhCWcagfYfzWA;

			internal readonly int _reInputId;

			internal bool isJoystickConnected
			{
				get
				{
					if (oBPMdftKtJLWDwKjOKgLuimxBfUKA == null)
					{
						return false;
					}
					return oBPMdftKtJLWDwKjOKgLuimxBfUKA._isConnected;
				}
			}

			internal bool enabled
			{
				get
				{
					if (oBPMdftKtJLWDwKjOKgLuimxBfUKA == null)
					{
						return false;
					}
					return oBPMdftKtJLWDwKjOKgLuimxBfUKA.enabled;
				}
			}

			internal Controller controller => oBPMdftKtJLWDwKjOKgLuimxBfUKA;

			internal Extension(IControllerExtensionSource P_0)
			{
				_reInputId = ReInput.id;
				MGMRuRGPOTjgdgcTFJxoqGNKLCObA(P_0);
			}

			internal Extension(Extension P_0)
				: this(P_0.eyyVWBZaxsCJCQMmwhCWcagfYfzWA)
			{
				oBPMdftKtJLWDwKjOKgLuimxBfUKA = P_0.oBPMdftKtJLWDwKjOKgLuimxBfUKA;
			}

			internal T GetController<T>() where T : Controller
			{
				if (oBPMdftKtJLWDwKjOKgLuimxBfUKA == null)
				{
					return null;
				}
				return oBPMdftKtJLWDwKjOKgLuimxBfUKA as T;
			}

			internal void SetController(Controller controller)
			{
				oBPMdftKtJLWDwKjOKgLuimxBfUKA = controller;
			}

			[CustomObfuscation(rename = false)]
			internal IControllerExtensionSource GetSource()
			{
				return eyyVWBZaxsCJCQMmwhCWcagfYfzWA;
			}

			internal void SetSource(Extension extension)
			{
				if (extension == null)
				{
					MGMRuRGPOTjgdgcTFJxoqGNKLCObA(null);
				}
				else
				{
					MGMRuRGPOTjgdgcTFJxoqGNKLCObA(extension.eyyVWBZaxsCJCQMmwhCWcagfYfzWA);
				}
			}

			private void MGMRuRGPOTjgdgcTFJxoqGNKLCObA(IControllerExtensionSource P_0)
			{
				eyyVWBZaxsCJCQMmwhCWcagfYfzWA = P_0;
				SourceUpdated(eyyVWBZaxsCJCQMmwhCWcagfYfzWA);
			}

			internal virtual void Clear()
			{
			}

			internal abstract void SourceUpdated(IControllerExtensionSource source);

			internal abstract void UpdateData(UpdateLoopType updateLoop);

			internal abstract Extension Clone();
		}

		[Serializable]
		private sealed class rbdzevCvFXcYDnsuQXlHqUUPmyAE
		{
			public static readonly rbdzevCvFXcYDnsuQXlHqUUPmyAE _003C_003E9 = new rbdzevCvFXcYDnsuQXlHqUUPmyAE();

			public static Func<Controller, Guid, bool> _003C_003E9__158_0;

			public static Func<Controller, Type, bool> _003C_003E9__161_0;

			internal bool oAKUgZIvlQOcnJFlvtRtFYcPvEXV(Controller P_0, Guid P_1)
			{
				return P_0.ImplementsTemplate(P_1);
			}

			internal bool DhEakMUBfEQHTKOzGVMIcIAzqpbl(Controller P_0, Type P_1)
			{
				return P_0.ImplementsTemplate(P_1);
			}
		}

		private sealed class xcmNUnYrzbmBpFBeeBycrfhodYvl : IEnumerable<ControllerPollingInfo>, IEnumerator<ControllerPollingInfo>, IDisposable, IEnumerable, IEnumerator
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private ControllerPollingInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public Controller TtytLoUfsgUyhsklaKccrnoMiiek;

			private int hWZNbLCFLBBWXNZxiKeGtzgrnTsg;

			ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			[DebuggerHidden]
			public xcmNUnYrzbmBpFBeeBycrfhodYvl(int P_0)
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
				wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
				Controller ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
				{
					if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
					{
						return false;
					}
					RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
					goto IL_00a0;
				}
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (ReInput._id != ttytLoUfsgUyhsklaKccrnoMiiek.QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(ttytLoUfsgUyhsklaKccrnoMiiek.QajDFFaomlkLHzaostfWYpGUioys);
					return false;
				}
				ttytLoUfsgUyhsklaKccrnoMiiek.UpdatePollingFrameTracking();
				hWZNbLCFLBBWXNZxiKeGtzgrnTsg = 0;
				goto IL_00b0;
				IL_00b0:
				if (hWZNbLCFLBBWXNZxiKeGtzgrnTsg < ttytLoUfsgUyhsklaKccrnoMiiek._buttonCount)
				{
					if (ttytLoUfsgUyhsklaKccrnoMiiek.sVnBSRZxBIImwjxjwYkaJXrsPSmo(hWZNbLCFLBBWXNZxiKeGtzgrnTsg, out var num))
					{
						VqEePGSMyrGKIqkWibsjeHcWPSIx = new ControllerPollingInfo(true, -1, ttytLoUfsgUyhsklaKccrnoMiiek.id, ttytLoUfsgUyhsklaKccrnoMiiek._name, ttytLoUfsgUyhsklaKccrnoMiiek._type, ControllerElementType.Button, hWZNbLCFLBBWXNZxiKeGtzgrnTsg, Pole.Positive, ttytLoUfsgUyhsklaKccrnoMiiek.yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetElementIdentifierName(num), num, KeyCode.None);
						RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
						return true;
					}
					goto IL_00a0;
				}
				return false;
				IL_00a0:
				hWZNbLCFLBBWXNZxiKeGtzgrnTsg++;
				goto IL_00b0;
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
			IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
			{
				xcmNUnYrzbmBpFBeeBycrfhodYvl xcmNUnYrzbmBpFBeeBycrfhodYvl2;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					xcmNUnYrzbmBpFBeeBycrfhodYvl2 = this;
				}
				else
				{
					xcmNUnYrzbmBpFBeeBycrfhodYvl2 = new xcmNUnYrzbmBpFBeeBycrfhodYvl(0);
					xcmNUnYrzbmBpFBeeBycrfhodYvl2.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				return xcmNUnYrzbmBpFBeeBycrfhodYvl2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
			}
		}

		private sealed class fUiarBgAEIUPlRGfIoVhMItVLAJeA : IEnumerable<ControllerPollingInfo>, IEnumerator<ControllerPollingInfo>, IDisposable, IEnumerable, IEnumerator
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private ControllerPollingInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public Controller TtytLoUfsgUyhsklaKccrnoMiiek;

			private int hWZNbLCFLBBWXNZxiKeGtzgrnTsg;

			ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			[DebuggerHidden]
			public fUiarBgAEIUPlRGfIoVhMItVLAJeA(int P_0)
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
				wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
				Controller ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
				{
					if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
					{
						return false;
					}
					RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
					goto IL_00a0;
				}
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (ReInput._id != ttytLoUfsgUyhsklaKccrnoMiiek.QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(ttytLoUfsgUyhsklaKccrnoMiiek.QajDFFaomlkLHzaostfWYpGUioys);
					return false;
				}
				ttytLoUfsgUyhsklaKccrnoMiiek.UpdatePollingFrameTracking();
				hWZNbLCFLBBWXNZxiKeGtzgrnTsg = 0;
				goto IL_00b0;
				IL_00b0:
				if (hWZNbLCFLBBWXNZxiKeGtzgrnTsg < ttytLoUfsgUyhsklaKccrnoMiiek._buttonCount)
				{
					if (ttytLoUfsgUyhsklaKccrnoMiiek.gCuAnzWoyHDElAQuCxijoMWbIyQsA(hWZNbLCFLBBWXNZxiKeGtzgrnTsg, out var num))
					{
						VqEePGSMyrGKIqkWibsjeHcWPSIx = new ControllerPollingInfo(true, -1, ttytLoUfsgUyhsklaKccrnoMiiek.id, ttytLoUfsgUyhsklaKccrnoMiiek._name, ttytLoUfsgUyhsklaKccrnoMiiek._type, ControllerElementType.Button, hWZNbLCFLBBWXNZxiKeGtzgrnTsg, Pole.Positive, ttytLoUfsgUyhsklaKccrnoMiiek.yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetElementIdentifierName(num), num, KeyCode.None);
						RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
						return true;
					}
					goto IL_00a0;
				}
				return false;
				IL_00a0:
				hWZNbLCFLBBWXNZxiKeGtzgrnTsg++;
				goto IL_00b0;
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
			IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
			{
				fUiarBgAEIUPlRGfIoVhMItVLAJeA fUiarBgAEIUPlRGfIoVhMItVLAJeA2;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					fUiarBgAEIUPlRGfIoVhMItVLAJeA2 = this;
				}
				else
				{
					fUiarBgAEIUPlRGfIoVhMItVLAJeA2 = new fUiarBgAEIUPlRGfIoVhMItVLAJeA(0);
					fUiarBgAEIUPlRGfIoVhMItVLAJeA2.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				return fUiarBgAEIUPlRGfIoVhMItVLAJeA2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
			}
		}

		public readonly int id;

		protected string _tag;

		protected string _name;

		protected string _hardwareName;

		protected readonly ControllerType _type;

		internal readonly Guid fMxZVPLmyEupjctdQIaGgbDJdlvHA;

		protected string _hardwareIdentifier;

		protected bool _isConnected;

		private Extension ieNssgrFeJiIpjsZwbsJdGwhvyJU;

		private bool kKFZZElqKQSUFMZnWdEudRvTJGpo;

		private ControllerIdentifier ZWKAKEiIYJobIYFMTEwXbqzBPyJCA;

		internal int QajDFFaomlkLHzaostfWYpGUioys;

		protected readonly int _buttonCount;

		protected readonly Button[] buttons;

		protected readonly ReadOnlyCollection<Button> buttons_readOnly;

		private readonly IList<Element> hObmyuQkxgNXDiBtSEwFNgLuBsoZ;

		private readonly ReadOnlyCollection<Element> VewvmtxDhBKjfOztXaGoFFLyxNDV;

		private readonly IList<CompoundElement> WmzLsZTiHtXhWsarHfHNeIilmoMX;

		private readonly ReadOnlyCollection<CompoundElement> czCmTURWGYOxRAbxeoxWOJwgMnPs;

		[CustomObfuscation(rename = false)]
		internal readonly InputSource inputSource;

		internal readonly ControllerDataUpdater BkAqQtJmzNvLobJflzLPUhNYRphu;

		internal readonly HardwareControllerMap_Game yPbTGFEOQNqKOEHVnHaIUhnHjgYD;

		internal uint uypLrHiCVLoiDTqTLBgqBIQjfPUGb;

		private uint uDpZemkhMQIbgEfUgDitvsITBpOg;

		private uint xszhRiGkPkqOHSiZVbURULfUhzIdA;

		private Action<bool> eneVgSlIatJCOIDcAjGhRLwviGQy;

		private IControllerTemplate[] VQZKpibEZbbTYjeSLBjyauxAsrVEA;

		private ReadOnlyCollection<IControllerTemplate> TkPCsfblXRmCKiRlSYUmJkYsPUwdb;

		private static Func<Controller, Guid, bool> wPglPbhGEOjaMfdmeDZejLVLluDDA;

		private static Func<Controller, Type, bool> dsGeHCsUXpayaLxcIaOoEOhDEDkJ;

		internal bool bqBVvAjBJdFrKbReiGkrAWyUTDkhb => uDpZemkhMQIbgEfUgDitvsITBpOg == ReInput.previousFrame;

		public bool enabled
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return false;
				}
				return kKFZZElqKQSUFMZnWdEudRvTJGpo;
			}
			set
			{
				RQRRqZiAdoXIQMUtzTIWsndPGdwX(value);
			}
		}

		public string name
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return string.Empty;
				}
				return _name;
			}
			internal set
			{
				_name = text;
			}
		}

		public string tag
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return string.Empty;
				}
				return _tag;
			}
			set
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				}
				else
				{
					_tag = value;
				}
			}
		}

		public string hardwareName
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return string.Empty;
				}
				return _hardwareName;
			}
		}

		public ControllerType type
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return ControllerType.Keyboard;
				}
				return _type;
			}
		}

		public Guid hardwareTypeGuid
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return Guid.Empty;
				}
				return fMxZVPLmyEupjctdQIaGgbDJdlvHA;
			}
		}

		public abstract Guid deviceInstanceGuid { get; }

		public ControllerIdentifier identifier => ZWKAKEiIYJobIYFMTEwXbqzBPyJCA;

		public bool isConnected
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return false;
				}
				return _isConnected;
			}
			internal set
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				}
				else if (!flag)
				{
					Disconnected();
				}
				else
				{
					Connected();
				}
			}
		}

		public string hardwareIdentifier
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return string.Empty;
				}
				return _hardwareIdentifier;
			}
		}

		public string mapTypeString => _type.ToString() + "Map";

		public int elementCount
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return 0;
				}
				return hObmyuQkxgNXDiBtSEwFNgLuBsoZ.Count;
			}
		}

		public int buttonCount
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return 0;
				}
				return _buttonCount;
			}
		}

		public IList<Element> Elements
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return EmptyObjects<Element>.EmptyReadOnlyIListT;
				}
				return VewvmtxDhBKjfOztXaGoFFLyxNDV;
			}
		}

		public IList<CompoundElement> CompoundElements
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return EmptyObjects<CompoundElement>.EmptyReadOnlyIListT;
				}
				return czCmTURWGYOxRAbxeoxWOJwgMnPs;
			}
		}

		public IList<Button> Buttons
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return EmptyObjects<Button>.EmptyReadOnlyIListT;
				}
				return buttons_readOnly;
			}
		}

		public Extension extension
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return null;
				}
				return ieNssgrFeJiIpjsZwbsJdGwhvyJU;
			}
		}

		public IList<ControllerElementIdentifier> ElementIdentifiers
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return EmptyObjects<ControllerElementIdentifier>.EmptyReadOnlyIListT;
				}
				return yPbTGFEOQNqKOEHVnHaIUhnHjgYD.elementIdentifiers_readOnly;
			}
		}

		public IList<ControllerElementIdentifier> ButtonElementIdentifiers
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return EmptyObjects<ControllerElementIdentifier>.EmptyReadOnlyIListT;
				}
				return yPbTGFEOQNqKOEHVnHaIUhnHjgYD.buttonElementIdentifiers_readOnly;
			}
		}

		public IList<IControllerTemplate> Templates
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return EmptyObjects<IControllerTemplate>.EmptyReadOnlyIListT;
				}
				return TkPCsfblXRmCKiRlSYUmJkYsPUwdb;
			}
		}

		public int templateCount
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return 0;
				}
				return VQZKpibEZbbTYjeSLBjyauxAsrVEA.Length;
			}
		}

		internal static Func<Controller, Guid, bool> uZJFDdTnwAWcajjvhUkdUackbFjy => rbdzevCvFXcYDnsuQXlHqUUPmyAE._003C_003E9.oAKUgZIvlQOcnJFlvtRtFYcPvEXV;

		internal static Func<Controller, Type, bool> UNwdXnYhTENqDxNuKFryJzcXNoMZ => rbdzevCvFXcYDnsuQXlHqUUPmyAE._003C_003E9.DhEakMUBfEQHTKOzGVMIcIAzqpbl;

		internal event Action<bool> XzlJaAqgaEXTyCbUKbmUFIQZYLfT
		{
			add
			{
				eneVgSlIatJCOIDcAjGhRLwviGQy = (Action<bool>)Delegate.Combine(eneVgSlIatJCOIDcAjGhRLwviGQy, b);
			}
			remove
			{
				eneVgSlIatJCOIDcAjGhRLwviGQy = (Action<bool>)Delegate.Remove(eneVgSlIatJCOIDcAjGhRLwviGQy, value2);
			}
		}

		internal Controller(int P_0, InputSource P_1, string P_2, string P_3, string P_4, ControllerType P_5, Guid P_6, int P_7, bool[] P_8, HardwareButtonInfo[] P_9, HardwareControllerMap_Game P_10, Extension P_11, ControllerDataUpdater P_12)
		{
			id = P_0;
			inputSource = P_1;
			_type = P_5;
			fMxZVPLmyEupjctdQIaGgbDJdlvHA = P_6;
			_buttonCount = P_7;
			_name = P_2;
			_hardwareName = P_3;
			_hardwareIdentifier = P_4;
			BkAqQtJmzNvLobJflzLPUhNYRphu = P_12;
			yPbTGFEOQNqKOEHVnHaIUhnHjgYD = P_10;
			kKFZZElqKQSUFMZnWdEudRvTJGpo = true;
			QajDFFaomlkLHzaostfWYpGUioys = ReInput.id;
			VFmrvNCYadPaAhlYPgWJbhhQLtSOA(P_11);
			hObmyuQkxgNXDiBtSEwFNgLuBsoZ = new List<Element>(P_7);
			VewvmtxDhBKjfOztXaGoFFLyxNDV = new ReadOnlyCollection<Element>(hObmyuQkxgNXDiBtSEwFNgLuBsoZ);
			WmzLsZTiHtXhWsarHfHNeIilmoMX = new List<CompoundElement>();
			czCmTURWGYOxRAbxeoxWOJwgMnPs = new ReadOnlyCollection<CompoundElement>(WmzLsZTiHtXhWsarHfHNeIilmoMX);
			buttons = new Button[P_7];
			if (P_8 == null || P_8.Length < P_7)
			{
				for (int i = 0; i < P_7; i++)
				{
					buttons[i] = new Button(this, P_10.buttonElementIdentifierIds[i], "Button " + i, false, (P_9 != null) ? P_9[i] : new HardwareButtonInfo());
					JymOYvJCaUZfrfNvqSliYMwhgCGz(buttons[i]);
				}
			}
			else
			{
				for (int j = 0; j < P_7; j++)
				{
					buttons[j] = new Button(this, P_10.buttonElementIdentifierIds[j], "Button " + j, P_8[j], (P_9 != null) ? P_9[j] : new HardwareButtonInfo());
					JymOYvJCaUZfrfNvqSliYMwhgCGz(buttons[j]);
				}
			}
			buttons_readOnly = new ReadOnlyCollection<Button>(buttons);
			VQZKpibEZbbTYjeSLBjyauxAsrVEA = EmptyObjects<IControllerTemplate>.array;
			TkPCsfblXRmCKiRlSYUmJkYsPUwdb = new ReadOnlyCollection<IControllerTemplate>(VQZKpibEZbbTYjeSLBjyauxAsrVEA);
			Connected();
		}

		internal virtual void VFZCTlETAOxSLbCyseetbWbCRTGUb()
		{
			ZWKAKEiIYJobIYFMTEwXbqzBPyJCA = new ControllerIdentifier(this);
		}

		public virtual Element GetElementById(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			if (yPbTGFEOQNqKOEHVnHaIUhnHjgYD == null)
			{
				return null;
			}
			int buttonIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetButtonIndex(elementIdentifierId);
			if (buttonIndex < 0)
			{
				return null;
			}
			return buttons[buttonIndex];
		}

		public virtual CompoundElement GetCompundElementById(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			int count = WmzLsZTiHtXhWsarHfHNeIilmoMX.Count;
			for (int i = 0; i < count; i++)
			{
				if (WmzLsZTiHtXhWsarHfHNeIilmoMX[i] != null && WmzLsZTiHtXhWsarHfHNeIilmoMX[i].id == elementIdentifierId)
				{
					return WmzLsZTiHtXhWsarHfHNeIilmoMX[i];
				}
			}
			return null;
		}

		public int GetButtonIndexById(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return -1;
			}
			return yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetButtonIndex(elementIdentifierId);
		}

		public ControllerElementIdentifier GetElementIdentifierById(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			return yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetElementIdentifierById(elementIdentifierId);
		}

		public virtual bool GetButton(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if (index < 0 || index >= _buttonCount)
			{
				return false;
			}
			return buttons[index].value;
		}

		public virtual bool GetButtonDown(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if (index < 0 || index >= _buttonCount)
			{
				return false;
			}
			return buttons[index].justPressed;
		}

		public virtual bool GetButtonUp(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if (index < 0 || index >= _buttonCount)
			{
				return false;
			}
			return buttons[index].justReleased;
		}

		public virtual bool GetButtonChanged(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if (index < 0 || index >= _buttonCount)
			{
				return false;
			}
			return buttons[index].value != buttons[index].valuePrev;
		}

		public virtual bool GetButtonPrev(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if (index < 0 || index >= _buttonCount)
			{
				return false;
			}
			return buttons[index].valuePrev;
		}

		public virtual bool GetButtonDoublePressHold(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			return GetButtonDoublePressHold(index, 0f);
		}

		public virtual bool GetButtonDoublePressHold(int index, float speed)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if (index < 0 || index >= _buttonCount)
			{
				return false;
			}
			return buttons[index].DoublePressedAndHeld(speed);
		}

		public virtual bool GetButtonDoublePressDown(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			return GetButtonDoublePressDown(index, 0f);
		}

		public virtual bool GetButtonDoublePressDown(int index, float speed)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if (index < 0 || index >= _buttonCount)
			{
				return false;
			}
			return buttons[index].JustDoublePressed(speed);
		}

		public virtual double GetButtonTimePressed(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			if (index < 0 || index >= _buttonCount)
			{
				return 0.0;
			}
			return buttons[index].timePressed;
		}

		public virtual double GetButtonTimeUnpressed(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			if (index < 0 || index >= _buttonCount)
			{
				return 0.0;
			}
			return buttons[index].timeUnpressed;
		}

		public virtual double GetButtonLastTimePressed(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			if (index < 0 || index >= _buttonCount)
			{
				return 0.0;
			}
			return buttons[index].lastTimePressed;
		}

		public virtual double GetButtonLastTimeUnpressed(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			if (index < 0 || index >= _buttonCount)
			{
				return 0.0;
			}
			return buttons[index].lastTimeUnpressed;
		}

		public virtual bool GetAnyButton()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			for (int i = 0; i < _buttonCount; i++)
			{
				if (buttons[i].value)
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool GetAnyButtonDown()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			for (int i = 0; i < _buttonCount; i++)
			{
				if (buttons[i].justPressed)
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool GetAnyButtonUp()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			for (int i = 0; i < _buttonCount; i++)
			{
				if (buttons[i].justReleased)
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool GetAnyButtonPrev()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			for (int i = 0; i < _buttonCount; i++)
			{
				if (buttons[i].valuePrev)
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool GetAnyButtonChanged()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			for (int i = 0; i < _buttonCount; i++)
			{
				if (buttons[i].justChangedState)
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool GetButtonById(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			int buttonIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetButtonIndex(elementIdentifierId);
			if (buttonIndex < 0 || buttonIndex >= _buttonCount)
			{
				return false;
			}
			return buttons[buttonIndex].value;
		}

		public virtual bool GetButtonDownById(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			int buttonIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetButtonIndex(elementIdentifierId);
			if (buttonIndex < 0 || buttonIndex >= _buttonCount)
			{
				return false;
			}
			return buttons[buttonIndex].justPressed;
		}

		public virtual bool GetButtonUpById(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			int buttonIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetButtonIndex(elementIdentifierId);
			if (buttonIndex < 0 || buttonIndex >= _buttonCount)
			{
				return false;
			}
			return buttons[buttonIndex].justReleased;
		}

		public virtual bool GetButtonDoublePressHoldById(int elementIdentifierId, float speed)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			int buttonIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetButtonIndex(elementIdentifierId);
			if (buttonIndex < 0 || buttonIndex >= _buttonCount)
			{
				return false;
			}
			return buttons[buttonIndex].DoublePressedAndHeld(speed);
		}

		public virtual bool GetButtonDoublePressDownById(int elementIdentifierId, float speed)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			int buttonIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetButtonIndex(elementIdentifierId);
			if (buttonIndex < 0 || buttonIndex >= _buttonCount)
			{
				return false;
			}
			return buttons[buttonIndex].JustDoublePressed(speed);
		}

		public virtual bool GetButtonDoublePressHoldById(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			int buttonIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetButtonIndex(elementIdentifierId);
			return GetButtonDoublePressHold(buttonIndex, 0f);
		}

		public virtual bool GetButtonDoublePressDownById(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			int buttonIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetButtonIndex(elementIdentifierId);
			return GetButtonDoublePressDown(buttonIndex, 0f);
		}

		public virtual bool GetButtonPrevById(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			int buttonIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetButtonIndex(elementIdentifierId);
			if (buttonIndex < 0 || buttonIndex >= _buttonCount)
			{
				return false;
			}
			return buttons[buttonIndex].valuePrev;
		}

		public virtual double GetButtonTimePressedById(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			int buttonIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetButtonIndex(elementIdentifierId);
			if (buttonIndex < 0 || buttonIndex >= _buttonCount)
			{
				return 0.0;
			}
			return buttons[buttonIndex].timePressed;
		}

		public virtual double GetButtonTimeUnpressedById(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			int buttonIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetButtonIndex(elementIdentifierId);
			if (buttonIndex < 0 || buttonIndex >= _buttonCount)
			{
				return 0.0;
			}
			return buttons[buttonIndex].timeUnpressed;
		}

		public virtual double GetButtonLastTimePressedById(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			int buttonIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetButtonIndex(elementIdentifierId);
			if (buttonIndex < 0 || buttonIndex >= _buttonCount)
			{
				return 0.0;
			}
			return buttons[buttonIndex].lastTimePressed;
		}

		public virtual double GetButtonLastTimeUnpressedById(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			int buttonIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetButtonIndex(elementIdentifierId);
			if (buttonIndex < 0 || buttonIndex >= _buttonCount)
			{
				return 0.0;
			}
			return buttons[buttonIndex].lastTimeUnpressed;
		}

		public virtual ControllerPollingInfo PollForFirstElement()
		{
			return PollForFirstButton();
		}

		public virtual ControllerPollingInfo PollForFirstElementDown()
		{
			return PollForFirstButtonDown();
		}

		public virtual ControllerPollingInfo PollForFirstButton()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
			}
			UpdatePollingFrameTracking();
			for (int i = 0; i < _buttonCount; i++)
			{
				if (sVnBSRZxBIImwjxjwYkaJXrsPSmo(i, out var num))
				{
					return new ControllerPollingInfo(true, -1, id, _name, _type, ControllerElementType.Button, i, Pole.Positive, yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetElementIdentifierName(num), num, KeyCode.None);
				}
			}
			return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
		}

		public virtual ControllerPollingInfo PollForFirstButtonDown()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
			}
			UpdatePollingFrameTracking();
			for (int i = 0; i < _buttonCount; i++)
			{
				if (gCuAnzWoyHDElAQuCxijoMWbIyQsA(i, out var num))
				{
					return new ControllerPollingInfo(true, -1, id, _name, _type, ControllerElementType.Button, i, Pole.Positive, yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetElementIdentifierName(num), num, KeyCode.None);
				}
			}
			return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
		}

		public virtual IEnumerable<ControllerPollingInfo> PollForAllElements()
		{
			return PollForAllButtons();
		}

		public virtual IEnumerable<ControllerPollingInfo> PollForAllElementsDown()
		{
			return PollForAllButtonsDown();
		}

		public virtual IEnumerable<ControllerPollingInfo> PollForAllButtons()
		{
			return new xcmNUnYrzbmBpFBeeBycrfhodYvl(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this
			};
		}

		public virtual IEnumerable<ControllerPollingInfo> PollForAllButtonsDown()
		{
			return new fUiarBgAEIUPlRGfIoVhMItVLAJeA(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this
			};
		}

		private bool sVnBSRZxBIImwjxjwYkaJXrsPSmo(int P_0, out int P_1)
		{
			P_1 = -1;
			if (!buttons[P_0].value || buttons[P_0].uTuDqdqmRErAFbVwkHOWZNOolvqi._excludeFromPolling)
			{
				return false;
			}
			P_1 = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.buttonElementIdentifierIds[P_0];
			if (P_1 < 0)
			{
				return false;
			}
			return true;
		}

		private bool gCuAnzWoyHDElAQuCxijoMWbIyQsA(int P_0, out int P_1)
		{
			P_1 = -1;
			if (!buttons[P_0].justPressed || buttons[P_0].uTuDqdqmRErAFbVwkHOWZNOolvqi._excludeFromPolling)
			{
				return false;
			}
			P_1 = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.buttonElementIdentifierIds[P_0];
			if (P_1 < 0)
			{
				return false;
			}
			return true;
		}

		protected void UpdatePollingFrameTracking()
		{
			if (xszhRiGkPkqOHSiZVbURULfUhzIdA == ReInput.currentFrame)
			{
				return;
			}
			uDpZemkhMQIbgEfUgDitvsITBpOg = xszhRiGkPkqOHSiZVbURULfUhzIdA;
			xszhRiGkPkqOHSiZVbURULfUhzIdA = ReInput.currentFrame;
			if (!bqBVvAjBJdFrKbReiGkrAWyUTDkhb)
			{
				if (uypLrHiCVLoiDTqTLBgqBIQjfPUGb == uint.MaxValue)
				{
					uypLrHiCVLoiDTqTLBgqBIQjfPUGb = 0u;
				}
				else
				{
					uypLrHiCVLoiDTqTLBgqBIQjfPUGb++;
				}
			}
		}

		public virtual double GetLastTimeActive()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			return GetLastTimeActive(useRawValues: false);
		}

		public virtual double GetLastTimeActive(bool useRawValues)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			return GetLastTimeAnyButtonPressed();
		}

		public virtual double GetLastTimeAnyElementChanged()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			return GetLastTimeAnyElementChanged(useRawValues: false);
		}

		public virtual double GetLastTimeAnyElementChanged(bool useRawValues)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			return GetLastTimeAnyButtonChanged();
		}

		public double GetLastTimeAnyButtonPressed()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			if (buttons == null)
			{
				return 0.0;
			}
			double num = 0.0;
			for (int i = 0; i < buttons.Length; i++)
			{
				double lastTimePressed = buttons[i].lastTimePressed;
				if (lastTimePressed > num)
				{
					num = lastTimePressed;
				}
			}
			return num;
		}

		public double GetLastTimeAnyButtonChanged()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			if (buttons == null)
			{
				return 0.0;
			}
			double num = 0.0;
			for (int i = 0; i < buttons.Length; i++)
			{
				double lastTimeStateChanged = buttons[i].lastTimeStateChanged;
				if (lastTimeStateChanged > num)
				{
					num = lastTimeStateChanged;
				}
			}
			return num;
		}

		public T GetExtension<T>() where T : class
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			return ieNssgrFeJiIpjsZwbsJdGwhvyJU as T;
		}

		public IControllerTemplate GetTemplate(Guid typeGuid)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			for (int i = 0; i < VQZKpibEZbbTYjeSLBjyauxAsrVEA.Length; i++)
			{
				if (VQZKpibEZbbTYjeSLBjyauxAsrVEA[i].typeGuid == typeGuid)
				{
					return VQZKpibEZbbTYjeSLBjyauxAsrVEA[i];
				}
			}
			return null;
		}

		public IControllerTemplate GetTemplate(Type type)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			for (int i = 0; i < VQZKpibEZbbTYjeSLBjyauxAsrVEA.Length; i++)
			{
				if (ReflectionTools.DoesTypeImplement(VQZKpibEZbbTYjeSLBjyauxAsrVEA[i].GetType(), type))
				{
					return VQZKpibEZbbTYjeSLBjyauxAsrVEA[i];
				}
			}
			return null;
		}

		public T GetTemplate<T>() where T : class
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			for (int i = 0; i < VQZKpibEZbbTYjeSLBjyauxAsrVEA.Length; i++)
			{
				if (VQZKpibEZbbTYjeSLBjyauxAsrVEA[i] as T != null)
				{
					return VQZKpibEZbbTYjeSLBjyauxAsrVEA[i] as T;
				}
			}
			return null;
		}

		public bool ImplementsTemplate(Guid typeGuid)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			for (int i = 0; i < VQZKpibEZbbTYjeSLBjyauxAsrVEA.Length; i++)
			{
				if (VQZKpibEZbbTYjeSLBjyauxAsrVEA[i].typeGuid == typeGuid)
				{
					return true;
				}
			}
			return false;
		}

		public bool ImplementsTemplate(Type type)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if ((object)type == null)
			{
				throw new ArgumentNullException("type");
			}
			for (int i = 0; i < VQZKpibEZbbTYjeSLBjyauxAsrVEA.Length; i++)
			{
				if (ReflectionTools.DoesTypeImplement(VQZKpibEZbbTYjeSLBjyauxAsrVEA[i].GetType(), type))
				{
					return true;
				}
			}
			return false;
		}

		public bool ImplementsTemplate<T>() where T : class
		{
			return ImplementsTemplate(typeof(T));
		}

		internal void UEbGwSCSBxwfLJkvPMrrMkcOzlpMA(IControllerTemplate[] P_0)
		{
			if (P_0 != null)
			{
				VQZKpibEZbbTYjeSLBjyauxAsrVEA = P_0;
				TkPCsfblXRmCKiRlSYUmJkYsPUwdb = new ReadOnlyCollection<IControllerTemplate>(VQZKpibEZbbTYjeSLBjyauxAsrVEA);
			}
		}

		internal virtual void PwSVyxiNOmkyuuXUHWyuymUyabog(UpdateLoopType P_0)
		{
			bool num = ReInput.IsInputAllowed(_type);
			int num2 = _buttonCount;
			if (num)
			{
				for (int i = 0; i < num2; i++)
				{
					if (buttons[i].HKnKeWNkqTdTiwoOhLnaWaFipaWu <= 0)
					{
						buttons[i].tzfYrQaBdsWHACSbQzIxoLZqqtry(P_0, i, BkAqQtJmzNvLobJflzLPUhNYRphu);
					}
				}
			}
			else
			{
				for (int j = 0; j < num2; j++)
				{
					if (buttons[j].HKnKeWNkqTdTiwoOhLnaWaFipaWu <= 0)
					{
						buttons[j].qFFjteInwJBtWaXvxhnWFfmOLOfx(P_0);
					}
				}
			}
			if (ieNssgrFeJiIpjsZwbsJdGwhvyJU != null)
			{
				ieNssgrFeJiIpjsZwbsJdGwhvyJU.UpdateData(P_0);
			}
		}

		internal virtual ButtonStateFlags TVecWANOQkYLjqMuBBqvkpWXDQpjA(int P_0)
		{
			if (P_0 < 0 || P_0 >= _buttonCount)
			{
				return ButtonStateFlags.Off;
			}
			return buttons[P_0].UFCGiNgJQTBJXbozkINBMenjSxAW;
		}

		internal void VFmrvNCYadPaAhlYPgWJbhhQLtSOA(Extension P_0)
		{
			if (P_0 == null)
			{
				ieNssgrFeJiIpjsZwbsJdGwhvyJU = null;
				return;
			}
			if (ieNssgrFeJiIpjsZwbsJdGwhvyJU != null)
			{
				VzvRQsucdCcVUuEaxTEkFVSKLlQM(P_0);
				return;
			}
			P_0.SetController(this);
			ieNssgrFeJiIpjsZwbsJdGwhvyJU = P_0.Clone();
		}

		internal void VzvRQsucdCcVUuEaxTEkFVSKLlQM(Extension P_0)
		{
			if (ieNssgrFeJiIpjsZwbsJdGwhvyJU != null)
			{
				ieNssgrFeJiIpjsZwbsJdGwhvyJU.SetSource(P_0);
				ieNssgrFeJiIpjsZwbsJdGwhvyJU.SetController(this);
				P_0?.SetController(this);
			}
			else
			{
				VFmrvNCYadPaAhlYPgWJbhhQLtSOA(P_0);
			}
		}

		internal virtual void SPGTRPyvIslcMdbPTItsewSLRPxx()
		{
			for (int i = 0; i < _buttonCount; i++)
			{
				if (buttons[i] != null)
				{
					buttons[i].Reset();
				}
			}
			if (BkAqQtJmzNvLobJflzLPUhNYRphu != null)
			{
				BkAqQtJmzNvLobJflzLPUhNYRphu.ClearData();
			}
			if (ieNssgrFeJiIpjsZwbsJdGwhvyJU != null)
			{
				ieNssgrFeJiIpjsZwbsJdGwhvyJU.Clear();
			}
		}

		internal virtual bool RQRRqZiAdoXIQMUtzTIWsndPGdwX(bool P_0)
		{
			if (kKFZZElqKQSUFMZnWdEudRvTJGpo == P_0)
			{
				return false;
			}
			if (!P_0)
			{
				SPGTRPyvIslcMdbPTItsewSLRPxx();
			}
			kKFZZElqKQSUFMZnWdEudRvTJGpo = P_0;
			if (eneVgSlIatJCOIDcAjGhRLwviGQy != null)
			{
				eneVgSlIatJCOIDcAjGhRLwviGQy(P_0);
			}
			return true;
		}

		internal virtual void dSUizcQaOGnPQOOBZVgCNLRSewAe(ControllerMap P_0)
		{
			if (P_0 == null)
			{
				return;
			}
			P_0.controllerId = id;
			IList<ActionElementMap> buttonMaps = P_0.ButtonMaps;
			for (int i = 0; i < buttonMaps.Count; i++)
			{
				JpxRPMmkCiJxotCLchUqPLcjlZiQ(P_0, buttonMaps[i]);
			}
			for (int num = buttonMaps.Count - 1; num >= 0; num--)
			{
				if (buttonMaps[num].elementIndex < 0)
				{
					P_0.DeleteElementMap(buttonMaps[num].YVGDeWQAhUWKAOSPoxXuizkXaiTI);
				}
			}
		}

		internal virtual void JpxRPMmkCiJxotCLchUqPLcjlZiQ(ControllerMap P_0, ActionElementMap P_1)
		{
			if (P_1 != null && P_1._elementType == ControllerElementType.Button)
			{
				P_1.nCUqazOObDuxSjUNBaQhabwMxrSG(P_0);
			}
		}

		internal bool SWTmVmkfSZUZmRSbuibWPpvOFzQB(ActionElementMap P_0, int P_1, out float P_2, out bool P_3)
		{
			P_3 = false;
			P_2 = 0f;
			if (P_1 != P_0._actionId)
			{
				return false;
			}
			int fBOOknsxpcCbVstgmGENdPFIgrbiA = P_0.FBOOknsxpcCbVstgmGENdPFIgrbiA;
			if (fBOOknsxpcCbVstgmGENdPFIgrbiA < 0 || fBOOknsxpcCbVstgmGENdPFIgrbiA >= _buttonCount)
			{
				return false;
			}
			P_3 = buttons[fBOOknsxpcCbVstgmGENdPFIgrbiA].rWaOJwvBsTPxWUGJsQKQYZrXeCRj;
			float num = ((!P_3) ? (buttons[fBOOknsxpcCbVstgmGENdPFIgrbiA].value ? 1f : 0f) : buttons[fBOOknsxpcCbVstgmGENdPFIgrbiA].pressure);
			if (num > 0f)
			{
				if (P_0._elementType == ControllerElementType.Button)
				{
					if (P_0._axisContribution == Pole.Negative)
					{
						num *= -1f;
					}
				}
				else if (P_0._elementType == ControllerElementType.Axis)
				{
					if (P_0._axisRange == AxisRange.Full)
					{
						if (P_0._invert)
						{
							num *= -1f;
						}
					}
					else if (P_0._axisContribution == Pole.Negative)
					{
						num *= -1f;
					}
				}
			}
			P_2 = num;
			return true;
		}

		internal bool SWTmVmkfSZUZmRSbuibWPpvOFzQB(ActionElementMap P_0, int P_1, bool P_2, out float P_3)
		{
			P_3 = 0f;
			if (P_1 != P_0._actionId)
			{
				return false;
			}
			float num = (P_2 ? 1f : 0f);
			if (num > 0f)
			{
				if (P_0._elementType == ControllerElementType.Button)
				{
					if (P_0._axisContribution == Pole.Negative)
					{
						num *= -1f;
					}
				}
				else if (P_0._elementType == ControllerElementType.Axis)
				{
					if (P_0._axisRange == AxisRange.Full)
					{
						if (P_0._invert)
						{
							num *= -1f;
						}
					}
					else if (P_0._axisContribution == Pole.Negative)
					{
						num *= -1f;
					}
				}
			}
			P_3 = num;
			return true;
		}

		internal void JymOYvJCaUZfrfNvqSliYMwhgCGz(Element P_0)
		{
			if (P_0 != null)
			{
				ListTools.AddIfUnique(hObmyuQkxgNXDiBtSEwFNgLuBsoZ, P_0);
			}
		}

		internal void ilFFizeJvQVyRFyOMjSxGFTfdnmy(CompoundElement P_0)
		{
			if (P_0 != null)
			{
				ListTools.AddIfUnique(WmzLsZTiHtXhWsarHfHNeIilmoMX, P_0);
			}
		}

		internal virtual Guid drNXyIBkoHEcsUvvDHyWjbtAnXop()
		{
			return Guid.Empty;
		}

		internal virtual void xKXEQmiiNXKorUsTsPlIOtwcwnfr(bool P_0)
		{
			if (!P_0 && !ReInput.IsInputAllowed(_type) && ieNssgrFeJiIpjsZwbsJdGwhvyJU != null)
			{
				ieNssgrFeJiIpjsZwbsJdGwhvyJU.Clear();
			}
		}

		protected virtual void Connected()
		{
			_isConnected = true;
		}

		protected virtual void Disconnected()
		{
			_isConnected = false;
			if (BkAqQtJmzNvLobJflzLPUhNYRphu != null)
			{
				BkAqQtJmzNvLobJflzLPUhNYRphu.ClearData();
			}
		}
	}
}
