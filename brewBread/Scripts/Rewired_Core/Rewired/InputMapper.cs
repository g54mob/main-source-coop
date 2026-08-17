using System;
using System.Collections.Generic;
using System.Text;
using Rewired.Utils;
using UnityEngine;

namespace Rewired
{
	public sealed class InputMapper
	{
		public class Context
		{
			private int JwOchbdvhsvHVdSyYjtqEJdTZzYG = -1;

			private ControllerMap mtCzlvnEPTtCfVpotNRemSaRaiHy;

			private ActionElementMap ZgUUXfqlaqVFIkvwiPHXzUSJadsv;

			private AxisRange tHhmpPjSUBKFuHQGzuGSIhGJKVlJ = AxisRange.Positive;

			private bool TxSJwJoaEWVxcucVapNSpYPnWYhO;

			public int actionId
			{
				get
				{
					return JwOchbdvhsvHVdSyYjtqEJdTZzYG;
				}
				set
				{
					if (!DcXiEIErDgHhAMPiWzVHzKbFEhbaA())
					{
						JwOchbdvhsvHVdSyYjtqEJdTZzYG = value;
					}
				}
			}

			public string actionName
			{
				get
				{
					InputAction action = ReInput.mapping.GetAction(JwOchbdvhsvHVdSyYjtqEJdTZzYG);
					if (action == null)
					{
						return string.Empty;
					}
					return action.name;
				}
				set
				{
					if (!DcXiEIErDgHhAMPiWzVHzKbFEhbaA())
					{
						InputAction action = ReInput.mapping.GetAction(value);
						if (action == null)
						{
							JwOchbdvhsvHVdSyYjtqEJdTZzYG = -1;
							Logger.LogError("The Action \"" + value + "\" is not a valid Action and cannot be used!");
						}
						else
						{
							JwOchbdvhsvHVdSyYjtqEJdTZzYG = action.id;
						}
					}
				}
			}

			public ControllerMap controllerMap
			{
				get
				{
					return mtCzlvnEPTtCfVpotNRemSaRaiHy;
				}
				set
				{
					if (!DcXiEIErDgHhAMPiWzVHzKbFEhbaA())
					{
						mtCzlvnEPTtCfVpotNRemSaRaiHy = value;
					}
				}
			}

			public ActionElementMap actionElementMapToReplace
			{
				get
				{
					return ZgUUXfqlaqVFIkvwiPHXzUSJadsv;
				}
				set
				{
					if (!DcXiEIErDgHhAMPiWzVHzKbFEhbaA())
					{
						ZgUUXfqlaqVFIkvwiPHXzUSJadsv = value;
					}
				}
			}

			public AxisRange actionRange
			{
				get
				{
					return tHhmpPjSUBKFuHQGzuGSIhGJKVlJ;
				}
				set
				{
					if (!DcXiEIErDgHhAMPiWzVHzKbFEhbaA())
					{
						tHhmpPjSUBKFuHQGzuGSIhGJKVlJ = value;
					}
				}
			}

			public Context()
			{
			}

			private Context(Context P_0)
				: this()
			{
				if (P_0 == null)
				{
					throw new ArgumentNullException("source");
				}
				Copy(P_0, this);
			}

			public Context Clone()
			{
				return new Context(this);
			}

			internal void McsukFueSylUgFGcFPeYxdJEncmf()
			{
				TxSJwJoaEWVxcucVapNSpYPnWYhO = true;
			}

			private bool DcXiEIErDgHhAMPiWzVHzKbFEhbaA()
			{
				if (TxSJwJoaEWVxcucVapNSpYPnWYhO)
				{
					Logger.LogError("Context is read-only and cannot be modified after Input Mapper has been started.");
					return true;
				}
				return false;
			}

			public static void Copy(Context source, Context destination)
			{
				if (source == null)
				{
					throw new ArgumentNullException("source");
				}
				if (destination == null)
				{
					throw new ArgumentNullException("destination");
				}
				destination.JwOchbdvhsvHVdSyYjtqEJdTZzYG = source.JwOchbdvhsvHVdSyYjtqEJdTZzYG;
				destination.mtCzlvnEPTtCfVpotNRemSaRaiHy = source.mtCzlvnEPTtCfVpotNRemSaRaiHy;
				destination.ZgUUXfqlaqVFIkvwiPHXzUSJadsv = source.ZgUUXfqlaqVFIkvwiPHXzUSJadsv;
				destination.tHhmpPjSUBKFuHQGzuGSIhGJKVlJ = source.tHhmpPjSUBKFuHQGzuGSIhGJKVlJ;
			}
		}

		public enum ConflictResponse
		{
			Cancel = 0,
			Replace = 1,
			Add = 2,
			Ignore = 3
		}

		public abstract class EventData
		{
			public readonly InputMapper inputMapper;

			internal EventData(InputMapper P_0)
			{
				inputMapper = P_0;
			}
		}

		public class InputMappedEventData : EventData
		{
			public readonly ActionElementMap actionElementMap;

			internal InputMappedEventData(InputMapper P_0, ActionElementMap P_1)
				: base(P_0)
			{
				actionElementMap = P_1;
			}
		}

		public class CanceledEventData : EventData
		{
			public readonly string message;

			internal CanceledEventData(InputMapper P_0, string P_1)
				: base(P_0)
			{
				message = P_1;
			}
		}

		public class ErrorEventData : EventData
		{
			public readonly string message;

			internal ErrorEventData(InputMapper P_0, string P_1)
				: base(P_0)
			{
				message = P_1;
			}
		}

		public class TimedOutEventData : EventData
		{
			internal TimedOutEventData(InputMapper P_0)
				: base(P_0)
			{
			}
		}

		public class StartedEventData : EventData
		{
			internal StartedEventData(InputMapper P_0)
				: base(P_0)
			{
			}
		}

		public class StoppedEventData : EventData
		{
			internal StoppedEventData(InputMapper P_0)
				: base(P_0)
			{
			}
		}

		public class ConflictFoundEventData : EventData
		{
			public readonly Action<ConflictResponse> responseCallback;

			public readonly ElementAssignmentInfo assignment;

			public readonly IList<ElementAssignmentConflictInfo> conflicts;

			public readonly bool isProtected;

			internal ConflictFoundEventData(InputMapper P_0, Action<ConflictResponse> P_1, ElementAssignmentInfo P_2, IList<ElementAssignmentConflictInfo> P_3, bool P_4)
				: base(P_0)
			{
				responseCallback = P_1;
				assignment = P_2;
				conflicts = P_3;
				isProtected = P_4;
			}
		}

		private enum pqkQWTNgxZgZDuoNDchCJcOmqHeX
		{
			InputMapped = 0,
			Error = 1,
			Canceled = 2,
			TimedOut = 3,
			Started = 4,
			Stopped = 5,
			ConflictsFound = 6
		}

		public enum Status
		{
			Idle = 0,
			Listening = 1,
			AwaitingResponse = 2
		}

		private class CvqneWRBGLDpMpWAQjcseqxhBpXs
		{
			private enum orJKwROrxEMHlZoxkEuFEygqZgJe
			{
				Quit = 0,
				Continue = 1
			}

			private enum ntchdUCcXwcgtXgjQQoXMalaVOZY
			{
				None = 0,
				ConflictChecking = 1
			}

			private class ahBotaQnJGXqbWYAMrHFvDmYrQCw
			{
				private Player XGUdRXTQSWQSPvuOLhFeAdcEiwfEA;

				private int JwOchbdvhsvHVdSyYjtqEJdTZzYG;

				private Context EyTixTudEJpQwYlPZfsgAcQHgTShA;

				private ControllerType KHkewZTAeuvHVDDJaWvnQRRxxDiF;

				private int UceKlChkUHGjQaNwpAzeqaieFMpt;

				private ControllerPollingInfo CKVHjYHWrghTEaPwhAisJZEYMPckA;

				private ModifierKeyFlags njfMvUCuswHuLtHoZcdqAwTyNgamA;

				public Player XAbNzJSvSNjTFwhnJridEdiVWtEM => XGUdRXTQSWQSPvuOLhFeAdcEiwfEA;

				public int tlJxlOEWqQEUYpNGSwfnmndnnqSe => JwOchbdvhsvHVdSyYjtqEJdTZzYG;

				public Context pPfCANsxvNQuUlkJnpcfgXnXxIDA => EyTixTudEJpQwYlPZfsgAcQHgTShA;

				public ControllerType zCReGrIKaGDiebYAdEKHNKYMyFBzA => KHkewZTAeuvHVDDJaWvnQRRxxDiF;

				public int dxNWoKAWRnEjMzagpBqSNtmzbbJFA => UceKlChkUHGjQaNwpAzeqaieFMpt;

				public ControllerPollingInfo sszRiigBPWwssYinvCcBpLjSAKDk => CKVHjYHWrghTEaPwhAisJZEYMPckA;

				public ModifierKeyFlags wwFgCSpoPkJdnJHHZcbOWaUGGwudA => njfMvUCuswHuLtHoZcdqAwTyNgamA;

				public AxisRange fHLnmlqZlxUGHumVxAlzXLaaqYid
				{
					get
					{
						AxisRange result = AxisRange.Positive;
						if (sszRiigBPWwssYinvCcBpLjSAKDk.elementType == ControllerElementType.Axis)
						{
							result = ((EyTixTudEJpQwYlPZfsgAcQHgTShA.actionRange != AxisRange.Full) ? ((sszRiigBPWwssYinvCcBpLjSAKDk.axisPole == Pole.Positive) ? AxisRange.Positive : AxisRange.Negative) : AxisRange.Full);
						}
						return result;
					}
				}

				public string nEoUuvcSycNqjMMxCItgDAaeDsweA
				{
					get
					{
						if (zCReGrIKaGDiebYAdEKHNKYMyFBzA == ControllerType.Keyboard && wwFgCSpoPkJdnJHHZcbOWaUGGwudA != ModifierKeyFlags.None)
						{
							return $"{Keyboard.ModifierKeyFlagsToString(wwFgCSpoPkJdnJHHZcbOWaUGGwudA)} + {sszRiigBPWwssYinvCcBpLjSAKDk.elementIdentifierName}";
						}
						string text = sszRiigBPWwssYinvCcBpLjSAKDk.elementIdentifierName;
						if (sszRiigBPWwssYinvCcBpLjSAKDk.elementType == ControllerElementType.Axis)
						{
							if (fHLnmlqZlxUGHumVxAlzXLaaqYid == AxisRange.Positive)
							{
								text += " +";
							}
							else if (fHLnmlqZlxUGHumVxAlzXLaaqYid == AxisRange.Negative)
							{
								text += " -";
							}
						}
						return text;
					}
				}

				public void zQQfvDZMmpVqPPLYlLuSJXXpwJcI(Player P_0, Context P_1)
				{
					if (P_1.controllerMap == null)
					{
						throw new ArgumentNullException("controllerMap");
					}
					SPGTRPyvIslcMdbPTItsewSLRPxx();
					XGUdRXTQSWQSPvuOLhFeAdcEiwfEA = P_0;
					JwOchbdvhsvHVdSyYjtqEJdTZzYG = P_1.actionId;
					KHkewZTAeuvHVDDJaWvnQRRxxDiF = P_1.controllerMap.controllerType;
					UceKlChkUHGjQaNwpAzeqaieFMpt = P_1.controllerMap.controllerId;
					EyTixTudEJpQwYlPZfsgAcQHgTShA = P_1;
					KHkewZTAeuvHVDDJaWvnQRRxxDiF = P_1.controllerMap.controllerType;
					UceKlChkUHGjQaNwpAzeqaieFMpt = P_1.controllerMap.controllerId;
					P_1.McsukFueSylUgFGcFPeYxdJEncmf();
				}

				public void SPGTRPyvIslcMdbPTItsewSLRPxx()
				{
					XGUdRXTQSWQSPvuOLhFeAdcEiwfEA = null;
					JwOchbdvhsvHVdSyYjtqEJdTZzYG = -1;
					EyTixTudEJpQwYlPZfsgAcQHgTShA = null;
					KHkewZTAeuvHVDDJaWvnQRRxxDiF = ControllerType.Keyboard;
					UceKlChkUHGjQaNwpAzeqaieFMpt = -1;
					CKVHjYHWrghTEaPwhAisJZEYMPckA = default(ControllerPollingInfo);
					njfMvUCuswHuLtHoZcdqAwTyNgamA = ModifierKeyFlags.None;
				}

				public ElementAssignment cSuMzNCcXuHqgLTvaWKYHJKlsSxF(ControllerPollingInfo P_0)
				{
					CKVHjYHWrghTEaPwhAisJZEYMPckA = P_0;
					return cSuMzNCcXuHqgLTvaWKYHJKlsSxF();
				}

				public ElementAssignment cSuMzNCcXuHqgLTvaWKYHJKlsSxF(ControllerPollingInfo P_0, ModifierKeyFlags P_1)
				{
					CKVHjYHWrghTEaPwhAisJZEYMPckA = P_0;
					njfMvUCuswHuLtHoZcdqAwTyNgamA = P_1;
					return cSuMzNCcXuHqgLTvaWKYHJKlsSxF();
				}

				public ElementAssignment cSuMzNCcXuHqgLTvaWKYHJKlsSxF()
				{
					return new ElementAssignment(zCReGrIKaGDiebYAdEKHNKYMyFBzA, CKVHjYHWrghTEaPwhAisJZEYMPckA.elementType, CKVHjYHWrghTEaPwhAisJZEYMPckA.elementIdentifierId, fHLnmlqZlxUGHumVxAlzXLaaqYid, CKVHjYHWrghTEaPwhAisJZEYMPckA.keyboardKey, njfMvUCuswHuLtHoZcdqAwTyNgamA, JwOchbdvhsvHVdSyYjtqEJdTZzYG, (EyTixTudEJpQwYlPZfsgAcQHgTShA.actionRange == AxisRange.Negative) ? Pole.Negative : Pole.Positive, false, (EyTixTudEJpQwYlPZfsgAcQHgTShA.actionElementMapToReplace != null) ? EyTixTudEJpQwYlPZfsgAcQHgTShA.actionElementMapToReplace.id : (-1));
				}
			}

			private readonly InputMapper afWhZEYTbMFjMUeulVkAPjICyxpP;

			private readonly Options bQaqZJNDPpHZPsCPODygOhVQNZSi = new Options();

			private readonly ahBotaQnJGXqbWYAMrHFvDmYrQCw XBlAEiNAPcZviZPoPeMZCDRJgHvhb = new ahBotaQnJGXqbWYAMrHFvDmYrQCw();

			private readonly Dictionary<pqkQWTNgxZgZDuoNDchCJcOmqHeX, SafeDelegate> ItibWhVtFYsKkkzgAslrAMMJBUdm;

			private readonly Dictionary<string, SafeDelegate> EYTwJGPmykStfmwylYPUYmHfLkXc;

			private Status XdgtdolzOKCisEMMipKQoOiLOdpP;

			private ntchdUCcXwcgtXgjQQoXMalaVOZY mCQjtXATsANARKIfzgFjCFzLJbuk;

			private double ExyPsGsshISNmqeXwjckKDdtxeWL;

			private bool krpgyNgQxdwqyoPnegpuDaJKflImb;

			private List<Player> giFwvOCPGoSmUaILJeUWgcGjKatcb = new List<Player>();

			private readonly List<ControllerPollingInfo> BkWFEQMfPJgBeguEEWxANYukEiIpA = new List<ControllerPollingInfo>();

			private ElementAssignment wzLoyrPiQXkkLKFeRGxQilFyGjAW;

			public Status sziyftxgQGatauzlOMGISVXaTbkL => XdgtdolzOKCisEMMipKQoOiLOdpP;

			public float naGntkOZQLOShDdZHaUwnVhYxrwU
			{
				get
				{
					if (XdgtdolzOKCisEMMipKQoOiLOdpP == Status.Idle)
					{
						return 0f;
					}
					if (bQaqZJNDPpHZPsCPODygOhVQNZSi.timeout <= 0f)
					{
						return 0f;
					}
					return (float)MathTools.Max(0.0, ExyPsGsshISNmqeXwjckKDdtxeWL + (double)bQaqZJNDPpHZPsCPODygOhVQNZSi.timeout - ReInput.unscaledTime);
				}
			}

			public Context kMUvbQCqjibbmcznhhMODCzGvqQpA
			{
				get
				{
					if (XdgtdolzOKCisEMMipKQoOiLOdpP == Status.Idle)
					{
						return null;
					}
					return XBlAEiNAPcZviZPoPeMZCDRJgHvhb.pPfCANsxvNQuUlkJnpcfgXnXxIDA;
				}
			}

			private bool byOwfPNomSOHtxIoYQbeOLyoyvtp
			{
				get
				{
					if (krpgyNgQxdwqyoPnegpuDaJKflImb)
					{
						return false;
					}
					if (!(bQaqZJNDPpHZPsCPODygOhVQNZSi.timeout > 0f))
					{
						return false;
					}
					return true;
				}
			}

			public CvqneWRBGLDpMpWAQjcseqxhBpXs(InputMapper P_0, Dictionary<pqkQWTNgxZgZDuoNDchCJcOmqHeX, SafeDelegate> P_1)
			{
				if (P_0 == null)
				{
					throw new ArgumentNullException("parent");
				}
				if (P_1 == null)
				{
					throw new ArgumentNullException("events");
				}
				afWhZEYTbMFjMUeulVkAPjICyxpP = P_0;
				ItibWhVtFYsKkkzgAslrAMMJBUdm = P_1;
				ptzeSUAYMaLUihwOYdMoDqBEoBwfb();
			}

			protected virtual void iMcWPzJbivbQRVFtjrscsWpjuUvv()
			{
				try
				{
					ZOnnIzLBcftHDPCtAfFaHvArCPmu();
				}
				finally
				{
					base.Finalize();
				}
			}

			public void edGQWybkvEgdpdpFVqgmUdPGZTqV(Context P_0, Options P_1)
			{
				if (XdgtdolzOKCisEMMipKQoOiLOdpP != Status.Idle)
				{
					cdvAJZQOaueWVSUWhZVkYhmBZYnr("User started a new listening session.");
				}
				if (P_0 == null)
				{
					throw new ArgumentNullException("context");
				}
				if (P_0.controllerMap == null)
				{
					throw new ArgumentNullException("controllerMap");
				}
				if (P_1 == null)
				{
					throw new ArgumentNullException("options");
				}
				P_0 = P_0.Clone();
				Options.Copy(P_1, bQaqZJNDPpHZPsCPODygOhVQNZSi);
				Player player = ReInput.players.GetPlayer(P_0.controllerMap.playerId);
				if (ReInput.mapping.GetAction(P_0.actionId) == null)
				{
					azHfZagcdORTVurnPQNDbgGCdlzpA("No Action found for actionId: " + P_0.actionId);
					return;
				}
				XBlAEiNAPcZviZPoPeMZCDRJgHvhb.zQQfvDZMmpVqPPLYlLuSJXXpwJcI(player, P_0);
				XdgtdolzOKCisEMMipKQoOiLOdpP = Status.Listening;
				oPFAmVelZJDtWASPerVVQHoNElpEB();
				RGdghEDCtZTDtTJkekaqelsMLCYxA();
				xYtunnRQSBvIGaqlmkiBhHCsbShL();
				ykcpdHpSundMctFmxDBIkMsMwkTvA();
			}

			public void aewbAWLwjOMPaDEVSdRuwWfvOfOy(string P_0)
			{
				if (XdgtdolzOKCisEMMipKQoOiLOdpP != Status.Idle)
				{
					cdvAJZQOaueWVSUWhZVkYhmBZYnr(P_0);
				}
			}

			private void jRaYtHNVcykNMAbqOnSGaKIIGSEaA(UpdateLoopType P_0)
			{
				if (P_0 == UpdateLoopType.Update && XdgtdolzOKCisEMMipKQoOiLOdpP == Status.Listening)
				{
					ElementAssignment elementAssignment;
					if (byOwfPNomSOHtxIoYQbeOLyoyvtp && naGntkOZQLOShDdZHaUwnVhYxrwU <= 0f)
					{
						NoVBwEHJvuoWIcMpToDgoLvFkuZq();
					}
					else if (ReInput.controllers.GetController(XBlAEiNAPcZviZPoPeMZCDRJgHvhb.zCReGrIKaGDiebYAdEKHNKYMyFBzA, XBlAEiNAPcZviZPoPeMZCDRJgHvhb.dxNWoKAWRnEjMzagpBqSNtmzbbJFA) == null)
					{
						azHfZagcdORTVurnPQNDbgGCdlzpA("Controller not found for type: " + XBlAEiNAPcZviZPoPeMZCDRJgHvhb.zCReGrIKaGDiebYAdEKHNKYMyFBzA.ToString() + " id: " + XBlAEiNAPcZviZPoPeMZCDRJgHvhb.dxNWoKAWRnEjMzagpBqSNtmzbbJFA);
					}
					else if (UyRVEmUxbBtltEEdOBBAVeBHFQqI(out elementAssignment) != orJKwROrxEMHlZoxkEuFEygqZgJe.Quit && XQvbhXZZWShRcStLzMWDUWauxTgB(elementAssignment) != orJKwROrxEMHlZoxkEuFEygqZgJe.Quit)
					{
						uKsxONBMitySWjBsbNQCaNoHODzg(elementAssignment);
					}
				}
			}

			private void neftIrgqNxhCwszXDdZtxDjrjvobA()
			{
				if (XdgtdolzOKCisEMMipKQoOiLOdpP != Status.Idle)
				{
					ptzeSUAYMaLUihwOYdMoDqBEoBwfb();
					ZOnnIzLBcftHDPCtAfFaHvArCPmu();
					xLsGPWjIjHQugjDsVbvTUfZqPFCkA();
				}
			}

			private void ptzeSUAYMaLUihwOYdMoDqBEoBwfb()
			{
				XdgtdolzOKCisEMMipKQoOiLOdpP = Status.Idle;
				ExyPsGsshISNmqeXwjckKDdtxeWL = 0.0;
				bQaqZJNDPpHZPsCPODygOhVQNZSi.SPGTRPyvIslcMdbPTItsewSLRPxx();
				XBlAEiNAPcZviZPoPeMZCDRJgHvhb.SPGTRPyvIslcMdbPTItsewSLRPxx();
				wzLoyrPiQXkkLKFeRGxQilFyGjAW = default(ElementAssignment);
				mCQjtXATsANARKIfzgFjCFzLJbuk = ntchdUCcXwcgtXgjQQoXMalaVOZY.None;
				krpgyNgQxdwqyoPnegpuDaJKflImb = false;
				giFwvOCPGoSmUaILJeUWgcGjKatcb.Clear();
			}

			private orJKwROrxEMHlZoxkEuFEygqZgJe UyRVEmUxbBtltEEdOBBAVeBHFQqI(out ElementAssignment P_0)
			{
				if (!STPFoldQiIgbyUwLdbiFQeDTmmXcA(out var enumerable, out var modifierKeyFlags))
				{
					P_0 = default(ElementAssignment);
					return orJKwROrxEMHlZoxkEuFEygqZgJe.Quit;
				}
				ControllerPollingInfo controllerPollingInfo = default(ControllerPollingInfo);
				foreach (ControllerPollingInfo item in enumerable)
				{
					if (item.success && !aWFHDrZoEukmCfujrhhslmlnRYxI(item, bQaqZJNDPpHZPsCPODygOhVQNZSi))
					{
						controllerPollingInfo = item;
						break;
					}
				}
				if (!controllerPollingInfo.success)
				{
					P_0 = default(ElementAssignment);
					return orJKwROrxEMHlZoxkEuFEygqZgJe.Quit;
				}
				if (!dnAIMjUBnUPJVoHyTaRwRwAwQKUw(XBlAEiNAPcZviZPoPeMZCDRJgHvhb, controllerPollingInfo, bQaqZJNDPpHZPsCPODygOhVQNZSi))
				{
					P_0 = default(ElementAssignment);
					return orJKwROrxEMHlZoxkEuFEygqZgJe.Quit;
				}
				P_0 = XBlAEiNAPcZviZPoPeMZCDRJgHvhb.cSuMzNCcXuHqgLTvaWKYHJKlsSxF(controllerPollingInfo);
				P_0.modifierKeyFlags = modifierKeyFlags;
				return orJKwROrxEMHlZoxkEuFEygqZgJe.Continue;
			}

			private bool STPFoldQiIgbyUwLdbiFQeDTmmXcA(out IEnumerable<ControllerPollingInfo> P_0, out ModifierKeyFlags P_1)
			{
				P_1 = ModifierKeyFlags.None;
				ControllerType controllerType = XBlAEiNAPcZviZPoPeMZCDRJgHvhb.zCReGrIKaGDiebYAdEKHNKYMyFBzA;
				int controllerId = XBlAEiNAPcZviZPoPeMZCDRJgHvhb.dxNWoKAWRnEjMzagpBqSNtmzbbJFA;
				if (controllerType == ControllerType.Keyboard)
				{
					P_0 = BiwbXVvcKnIOQbTlOKLCPppmELTTA(out P_1);
					return true;
				}
				if (bQaqZJNDPpHZPsCPODygOhVQNZSi.allowAxes)
				{
					if (bQaqZJNDPpHZPsCPODygOhVQNZSi.allowButtons)
					{
						if (XBlAEiNAPcZviZPoPeMZCDRJgHvhb.XAbNzJSvSNjTFwhnJridEdiVWtEM != null)
						{
							P_0 = XBlAEiNAPcZviZPoPeMZCDRJgHvhb.XAbNzJSvSNjTFwhnJridEdiVWtEM.controllers.polling.PollControllerForAllElementsDown(controllerType, controllerId);
						}
						else
						{
							P_0 = ReInput.controllers.polling.PollControllerForAllElementsDown(XBlAEiNAPcZviZPoPeMZCDRJgHvhb.zCReGrIKaGDiebYAdEKHNKYMyFBzA, XBlAEiNAPcZviZPoPeMZCDRJgHvhb.dxNWoKAWRnEjMzagpBqSNtmzbbJFA);
						}
					}
					else if (XBlAEiNAPcZviZPoPeMZCDRJgHvhb.XAbNzJSvSNjTFwhnJridEdiVWtEM != null)
					{
						P_0 = XBlAEiNAPcZviZPoPeMZCDRJgHvhb.XAbNzJSvSNjTFwhnJridEdiVWtEM.controllers.polling.PollControllerForAllAxes(controllerType, controllerId);
					}
					else
					{
						P_0 = ReInput.controllers.polling.PollControllerForAllAxes(controllerType, controllerId);
					}
				}
				else
				{
					if (!bQaqZJNDPpHZPsCPODygOhVQNZSi.allowButtons)
					{
						azHfZagcdORTVurnPQNDbgGCdlzpA("You must enable listening for at least one element type.");
						P_0 = null;
						return false;
					}
					if (XBlAEiNAPcZviZPoPeMZCDRJgHvhb.XAbNzJSvSNjTFwhnJridEdiVWtEM != null)
					{
						P_0 = XBlAEiNAPcZviZPoPeMZCDRJgHvhb.XAbNzJSvSNjTFwhnJridEdiVWtEM.controllers.polling.PollControllerForAllButtonsDown(controllerType, controllerId);
					}
					else
					{
						P_0 = ReInput.controllers.polling.PollControllerForAllButtonsDown(controllerType, controllerId);
					}
				}
				return true;
			}

			private IEnumerable<ControllerPollingInfo> BiwbXVvcKnIOQbTlOKLCPppmELTTA(out ModifierKeyFlags P_0)
			{
				P_0 = ModifierKeyFlags.None;
				BkWFEQMfPJgBeguEEWxANYukEiIpA.Clear();
				if (!bQaqZJNDPpHZPsCPODygOhVQNZSi.allowButtons)
				{
					return BkWFEQMfPJgBeguEEWxANYukEiIpA;
				}
				BkWFEQMfPJgBeguEEWxANYukEiIpA.Add(jPMuuFpqUaInuRebGecmGOJvhrTc(bQaqZJNDPpHZPsCPODygOhVQNZSi, out P_0));
				return BkWFEQMfPJgBeguEEWxANYukEiIpA;
			}

			private ControllerPollingInfo jPMuuFpqUaInuRebGecmGOJvhrTc(Options P_0, out ModifierKeyFlags P_1)
			{
				bool flag;
				string text;
				ControllerPollingInfo result = jPMuuFpqUaInuRebGecmGOJvhrTc(P_0, out flag, out P_1, out text);
				if (flag)
				{
					oPFAmVelZJDtWASPerVVQHoNElpEB();
				}
				return result;
			}

			private static ControllerPollingInfo jPMuuFpqUaInuRebGecmGOJvhrTc(Options P_0, out bool P_1, out ModifierKeyFlags P_2, out string P_3)
			{
				P_3 = string.Empty;
				P_1 = false;
				P_2 = ModifierKeyFlags.None;
				int num = 0;
				ControllerPollingInfo result = default(ControllerPollingInfo);
				ControllerPollingInfo result2 = default(ControllerPollingInfo);
				ModifierKeyFlags modifierKeyFlags = ModifierKeyFlags.None;
				foreach (ControllerPollingInfo item in ReInput.controllers.Keyboard.PollForAllKeys())
				{
					KeyCode keyboardKey = item.keyboardKey;
					if (keyboardKey == KeyCode.AltGr)
					{
						continue;
					}
					if (Keyboard.IsModifierKey(item.keyboardKey))
					{
						if (num == 0)
						{
							result2 = item;
						}
						modifierKeyFlags |= Keyboard.KeyCodeToModifierKeyFlags(keyboardKey);
						num++;
					}
					else if (result.keyboardKey == KeyCode.None)
					{
						result = item;
					}
				}
				if (result.keyboardKey != KeyCode.None)
				{
					if (!ReInput.controllers.Keyboard.GetKeyDown(result.keyboardKey))
					{
						return default(ControllerPollingInfo);
					}
					if (num == 0 || !P_0.allowKeyboardKeysWithModifiers)
					{
						return result;
					}
					P_2 = modifierKeyFlags;
					return result;
				}
				if (num > 0)
				{
					P_1 = true;
					if (num == 1)
					{
						if (P_0.allowKeyboardModifierKeyAsPrimary)
						{
							if (!P_0.allowKeyboardKeysWithModifiers || P_0.holdDurationToMapKeyboardModifierKeyAsPrimary <= 0f)
							{
								if (!ReInput.controllers.Keyboard.GetKeyDown(result2.keyboardKey))
								{
									return default(ControllerPollingInfo);
								}
								return result2;
							}
							if (ReInput.controllers.Keyboard.GetKeyTimePressed(result2.keyboardKey) >= (double)P_0.holdDurationToMapKeyboardModifierKeyAsPrimary)
							{
								return result2;
							}
						}
						P_3 = Keyboard.GetKeyName(result2.keyboardKey);
					}
					else
					{
						P_3 = Keyboard.ModifierKeyFlagsToString(modifierKeyFlags);
					}
				}
				return default(ControllerPollingInfo);
			}

			private static bool aWFHDrZoEukmCfujrhhslmlnRYxI(ControllerPollingInfo P_0, Options P_1)
			{
				if (!P_1.allowAxes && P_0.elementType == ControllerElementType.Axis)
				{
					return false;
				}
				if (!P_1.allowButtons && P_0.elementType == ControllerElementType.Button)
				{
					return false;
				}
				if (P_0.controllerType == ControllerType.Mouse && P_0.elementType == ControllerElementType.Axis)
				{
					switch (P_0.elementIndex)
					{
					case 0:
						if (P_1.ignoreMouseXAxis)
						{
							return true;
						}
						break;
					case 1:
						if (P_1.ignoreMouseYAxis)
						{
							return true;
						}
						break;
					}
				}
				SafePredicate<ControllerPollingInfo> safePredicate = P_1.ILFjrlXErAaQYsYRoWPNsXQagWVF<SafePredicate<ControllerPollingInfo>>("isElementAllowed");
				if (safePredicate != null)
				{
					return !safePredicate.Invoke(P_0);
				}
				return false;
			}

			private static bool dnAIMjUBnUPJVoHyTaRwRwAwQKUw(ahBotaQnJGXqbWYAMrHFvDmYrQCw P_0, ControllerPollingInfo P_1, Options P_2)
			{
				if (P_0 == null)
				{
					return false;
				}
				if (P_2 == null)
				{
					return true;
				}
				if (P_0.fHLnmlqZlxUGHumVxAlzXLaaqYid == AxisRange.Full && !P_2.allowButtonsOnFullAxisAssignment && P_1.elementType == ControllerElementType.Button)
				{
					return false;
				}
				return true;
			}

			private void RGdghEDCtZTDtTJkekaqelsMLCYxA()
			{
				if (!bQaqZJNDPpHZPsCPODygOhVQNZSi.checkForConflicts)
				{
					return;
				}
				if (bQaqZJNDPpHZPsCPODygOhVQNZSi.checkForConflictsWithSelf && XBlAEiNAPcZviZPoPeMZCDRJgHvhb.XAbNzJSvSNjTFwhnJridEdiVWtEM != null)
				{
					ListTools.AddIfUnique(giFwvOCPGoSmUaILJeUWgcGjKatcb, XBlAEiNAPcZviZPoPeMZCDRJgHvhb.XAbNzJSvSNjTFwhnJridEdiVWtEM);
				}
				if (bQaqZJNDPpHZPsCPODygOhVQNZSi.checkForConflictsWithSystemPlayer)
				{
					ListTools.AddIfUnique(giFwvOCPGoSmUaILJeUWgcGjKatcb, ReInput.players.SystemPlayer);
				}
				if (bQaqZJNDPpHZPsCPODygOhVQNZSi.checkForConflictsWithAllPlayers)
				{
					IList<Player> players = ReInput.players.Players;
					for (int i = 0; i < players.Count; i++)
					{
						ListTools.AddIfUnique(giFwvOCPGoSmUaILJeUWgcGjKatcb, players[i]);
					}
				}
				else
				{
					if (bQaqZJNDPpHZPsCPODygOhVQNZSi.checkForConflictsWithPlayerIds == null)
					{
						return;
					}
					IList<Player> allPlayers = ReInput.players.AllPlayers;
					int count = allPlayers.Count;
					for (int j = 0; j < count; j++)
					{
						if (ArrayTools.Contains(bQaqZJNDPpHZPsCPODygOhVQNZSi.checkForConflictsWithPlayerIds, allPlayers[j].id))
						{
							ListTools.AddIfUnique(giFwvOCPGoSmUaILJeUWgcGjKatcb, allPlayers[j]);
						}
					}
				}
			}

			private orJKwROrxEMHlZoxkEuFEygqZgJe XQvbhXZZWShRcStLzMWDUWauxTgB(ElementAssignment P_0)
			{
				if (bQaqZJNDPpHZPsCPODygOhVQNZSi.checkForConflicts && XBlAEiNAPcZviZPoPeMZCDRJgHvhb.XAbNzJSvSNjTFwhnJridEdiVWtEM != null && zpdYSIHfNkXPRvuXubZssDEJWmfo(XBlAEiNAPcZviZPoPeMZCDRJgHvhb, P_0, giFwvOCPGoSmUaILJeUWgcGjKatcb))
				{
					return JzzzVbXyFKRxXlzBbmvWUgjGLrtn(P_0);
				}
				return orJKwROrxEMHlZoxkEuFEygqZgJe.Continue;
			}

			private static bool zpdYSIHfNkXPRvuXubZssDEJWmfo(ahBotaQnJGXqbWYAMrHFvDmYrQCw P_0, ElementAssignment P_1, List<Player> P_2)
			{
				if (P_0 == null || P_0.XAbNzJSvSNjTFwhnJridEdiVWtEM == null)
				{
					return false;
				}
				if (P_2 == null || P_2.Count == 0)
				{
					return false;
				}
				if (!NwmzUMEJidbTncESwkEkhCWCHBGaB(P_0, P_1, out var conflictCheck))
				{
					return false;
				}
				for (int i = 0; i < P_2.Count; i++)
				{
					if (P_2[i].controllers.conflictChecking.DoesElementAssignmentConflict(conflictCheck))
					{
						return true;
					}
				}
				return false;
			}

			private static bool BUpNasfbyZAwibvyZWWVBAbhdvHvA(ahBotaQnJGXqbWYAMrHFvDmYrQCw P_0, ElementAssignment P_1, List<Player> P_2)
			{
				if (P_0 == null || P_0.XAbNzJSvSNjTFwhnJridEdiVWtEM == null)
				{
					return false;
				}
				if (P_2 == null || P_2.Count == 0)
				{
					return false;
				}
				if (!NwmzUMEJidbTncESwkEkhCWCHBGaB(P_0, P_1, out var conflictCheck))
				{
					return false;
				}
				for (int i = 0; i < P_2.Count; i++)
				{
					foreach (ElementAssignmentConflictInfo item in P_2[i].controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck))
					{
						if (!item.isUserAssignable)
						{
							return true;
						}
					}
				}
				return false;
			}

			private static IList<ElementAssignmentConflictInfo> mghaFDRvWsAVVqqMCmSlbieDlvjf(ahBotaQnJGXqbWYAMrHFvDmYrQCw P_0, ElementAssignment P_1, List<Player> P_2)
			{
				if (P_0 == null || P_0.XAbNzJSvSNjTFwhnJridEdiVWtEM == null)
				{
					return null;
				}
				if (P_2 == null || P_2.Count == 0)
				{
					return null;
				}
				if (!NwmzUMEJidbTncESwkEkhCWCHBGaB(P_0, P_1, out var conflictCheck))
				{
					return null;
				}
				List<ElementAssignmentConflictInfo> list = new List<ElementAssignmentConflictInfo>();
				for (int i = 0; i < P_2.Count; i++)
				{
					foreach (ElementAssignmentConflictInfo item in P_2[i].controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck))
					{
						list.Add(item);
					}
				}
				return list;
			}

			private static bool NwmzUMEJidbTncESwkEkhCWCHBGaB(ahBotaQnJGXqbWYAMrHFvDmYrQCw P_0, ElementAssignment P_1, out ElementAssignmentConflictCheck P_2)
			{
				Player player;
				if (P_0 == null || (player = P_0.XAbNzJSvSNjTFwhnJridEdiVWtEM) == null)
				{
					P_2 = default(ElementAssignmentConflictCheck);
					return false;
				}
				P_2 = P_1.ToElementAssignmentConflictCheck();
				P_2.playerId = player.id;
				P_2.controllerType = P_0.zCReGrIKaGDiebYAdEKHNKYMyFBzA;
				P_2.controllerId = P_0.dxNWoKAWRnEjMzagpBqSNtmzbbJFA;
				P_2.controllerMapId = P_0.pPfCANsxvNQuUlkJnpcfgXnXxIDA.controllerMap.id;
				P_2.controllerMapCategoryId = P_0.pPfCANsxvNQuUlkJnpcfgXnXxIDA.controllerMap.categoryId;
				if (P_0.pPfCANsxvNQuUlkJnpcfgXnXxIDA.actionElementMapToReplace != null)
				{
					P_2.elementMapId = P_0.pPfCANsxvNQuUlkJnpcfgXnXxIDA.actionElementMapToReplace.id;
				}
				return true;
			}

			private static void hXvMxxkKCLvhHBeSrwUyHeBlAfieA(ahBotaQnJGXqbWYAMrHFvDmYrQCw P_0, ElementAssignment P_1, List<Player> P_2)
			{
				if (P_0 == null || P_0.XAbNzJSvSNjTFwhnJridEdiVWtEM == null)
				{
					return;
				}
				if (!NwmzUMEJidbTncESwkEkhCWCHBGaB(P_0, P_1, out var conflictCheck))
				{
					Logger.LogError("Error creating conflict check!");
					return;
				}
				for (int i = 0; i < P_2.Count; i++)
				{
					P_2[i].controllers.conflictChecking.RemoveElementAssignmentConflicts(conflictCheck);
				}
			}

			private void xYtunnRQSBvIGaqlmkiBhHCsbShL()
			{
				ReInput.UpdateEndedEvent -= jRaYtHNVcykNMAbqOnSGaKIIGSEaA;
				ReInput.UpdateEndedEvent += jRaYtHNVcykNMAbqOnSGaKIIGSEaA;
			}

			private void ZOnnIzLBcftHDPCtAfFaHvArCPmu()
			{
				ReInput.UpdateEndedEvent -= jRaYtHNVcykNMAbqOnSGaKIIGSEaA;
			}

			private bool wkpNeFCrHzcEPUxAratvQgFUxojE(pqkQWTNgxZgZDuoNDchCJcOmqHeX P_0)
			{
				SafeDelegate safeDelegate = ItibWhVtFYsKkkzgAslrAMMJBUdm[P_0];
				if (safeDelegate != null)
				{
					return safeDelegate.Count > 0;
				}
				return false;
			}

			private void VsncEXPmWhCpsdBznLpDcPGGNdsDb<_0001>(pqkQWTNgxZgZDuoNDchCJcOmqHeX P_0, _0001 P_1)
			{
				SafeAction<_0001> safeAction = (SafeAction<_0001>)ItibWhVtFYsKkkzgAslrAMMJBUdm[P_0];
				if (safeAction.Count != 0)
				{
					safeAction.Invoke(P_1);
				}
			}

			private void oPFAmVelZJDtWASPerVVQHoNElpEB()
			{
				ExyPsGsshISNmqeXwjckKDdtxeWL = ReInput.unscaledTime;
			}

			private void kHUcNXbpGaVFWfMPDPbJeQQOjAMsA()
			{
				krpgyNgQxdwqyoPnegpuDaJKflImb = true;
			}

			private void pGjfThRdwdYslSUltPbtbdmIslJG(ActionElementMap P_0)
			{
				TrZGBvKAMkLMckUvfUMfwWVjFdgj(P_0);
				neftIrgqNxhCwszXDdZtxDjrjvobA();
			}

			private void cdvAJZQOaueWVSUWhZVkYhmBZYnr(string P_0)
			{
				ELYOwNrqZrDeeRwnkiivRaQzqjNs(P_0);
				neftIrgqNxhCwszXDdZtxDjrjvobA();
			}

			private orJKwROrxEMHlZoxkEuFEygqZgJe JzzzVbXyFKRxXlzBbmvWUgjGLrtn(ElementAssignment P_0)
			{
				if (wkpNeFCrHzcEPUxAratvQgFUxojE(pqkQWTNgxZgZDuoNDchCJcOmqHeX.ConflictsFound))
				{
					bool flag = BUpNasfbyZAwibvyZWWVBAbhdvHvA(XBlAEiNAPcZviZPoPeMZCDRJgHvhb, P_0, giFwvOCPGoSmUaILJeUWgcGjKatcb);
					wzLoyrPiQXkkLKFeRGxQilFyGjAW = P_0;
					IList<ElementAssignmentConflictInfo> list = mghaFDRvWsAVVqqMCmSlbieDlvjf(XBlAEiNAPcZviZPoPeMZCDRJgHvhb, P_0, giFwvOCPGoSmUaILJeUWgcGjKatcb);
					mCQjtXATsANARKIfzgFjCFzLJbuk = ntchdUCcXwcgtXgjQQoXMalaVOZY.ConflictChecking;
					tgUTbEyPyhymqcBMomTjUwYyjwls();
					KPPucdQoFIfBoANWjeNBCXpDcNUQB(new ElementAssignmentInfo(XBlAEiNAPcZviZPoPeMZCDRJgHvhb.pPfCANsxvNQuUlkJnpcfgXnXxIDA.controllerMap, P_0), list, flag);
					return orJKwROrxEMHlZoxkEuFEygqZgJe.Quit;
				}
				return pyvXxNScWSyyWnhaFCmMYsdGfuIN(bQaqZJNDPpHZPsCPODygOhVQNZSi.defaultActionWhenConflictFound, P_0);
			}

			private orJKwROrxEMHlZoxkEuFEygqZgJe pyvXxNScWSyyWnhaFCmMYsdGfuIN(ConflictResponse P_0, ElementAssignment P_1)
			{
				return pyvXxNScWSyyWnhaFCmMYsdGfuIN(P_0, P_1, BUpNasfbyZAwibvyZWWVBAbhdvHvA(XBlAEiNAPcZviZPoPeMZCDRJgHvhb, P_1, giFwvOCPGoSmUaILJeUWgcGjKatcb));
			}

			private orJKwROrxEMHlZoxkEuFEygqZgJe pyvXxNScWSyyWnhaFCmMYsdGfuIN(ConflictResponse P_0, ElementAssignment P_1, bool P_2)
			{
				switch (P_0)
				{
				case ConflictResponse.Cancel:
					cdvAJZQOaueWVSUWhZVkYhmBZYnr("Mapping assignment was canceled due to a conflict.");
					return orJKwROrxEMHlZoxkEuFEygqZgJe.Quit;
				case ConflictResponse.Replace:
					if (P_2)
					{
						cdvAJZQOaueWVSUWhZVkYhmBZYnr("Mapping assignment was canceled due to a protected conflict that cannot be replaced.");
						return orJKwROrxEMHlZoxkEuFEygqZgJe.Quit;
					}
					hXvMxxkKCLvhHBeSrwUyHeBlAfieA(XBlAEiNAPcZviZPoPeMZCDRJgHvhb, P_1, giFwvOCPGoSmUaILJeUWgcGjKatcb);
					return orJKwROrxEMHlZoxkEuFEygqZgJe.Continue;
				case ConflictResponse.Add:
					return orJKwROrxEMHlZoxkEuFEygqZgJe.Continue;
				case ConflictResponse.Ignore:
					eocEHWldcxlYmWpmkDPDdzoPtuby();
					return orJKwROrxEMHlZoxkEuFEygqZgJe.Quit;
				default:
					throw new NotImplementedException();
				}
			}

			private void NoVBwEHJvuoWIcMpToDgoLvFkuZq()
			{
				DswviOkrXXirXpDfcdVjxtbqGOvM();
				neftIrgqNxhCwszXDdZtxDjrjvobA();
			}

			private void azHfZagcdORTVurnPQNDbgGCdlzpA(string P_0)
			{
				giuQAOGqgJGpaeVmdyUvrLopDENzA(P_0);
				neftIrgqNxhCwszXDdZtxDjrjvobA();
			}

			private void tgUTbEyPyhymqcBMomTjUwYyjwls()
			{
				kHUcNXbpGaVFWfMPDPbJeQQOjAMsA();
				ZOnnIzLBcftHDPCtAfFaHvArCPmu();
				XdgtdolzOKCisEMMipKQoOiLOdpP = Status.AwaitingResponse;
			}

			private void eocEHWldcxlYmWpmkDPDdzoPtuby()
			{
				XdgtdolzOKCisEMMipKQoOiLOdpP = Status.Listening;
				mCQjtXATsANARKIfzgFjCFzLJbuk = ntchdUCcXwcgtXgjQQoXMalaVOZY.None;
				oPFAmVelZJDtWASPerVVQHoNElpEB();
				xYtunnRQSBvIGaqlmkiBhHCsbShL();
			}

			private void uKsxONBMitySWjBsbNQCaNoHODzg(ElementAssignment P_0)
			{
				if (XBlAEiNAPcZviZPoPeMZCDRJgHvhb.pPfCANsxvNQuUlkJnpcfgXnXxIDA.controllerMap.ReplaceOrCreateElementMap(P_0, out var result))
				{
					pGjfThRdwdYslSUltPbtbdmIslJG(result);
				}
				else
				{
					azHfZagcdORTVurnPQNDbgGCdlzpA("Failed to create element assignment.");
				}
			}

			private void TrZGBvKAMkLMckUvfUMfwWVjFdgj(ActionElementMap P_0)
			{
				if (wkpNeFCrHzcEPUxAratvQgFUxojE(pqkQWTNgxZgZDuoNDchCJcOmqHeX.InputMapped))
				{
					VsncEXPmWhCpsdBznLpDcPGGNdsDb(pqkQWTNgxZgZDuoNDchCJcOmqHeX.InputMapped, new InputMappedEventData(afWhZEYTbMFjMUeulVkAPjICyxpP, P_0));
				}
			}

			private void DswviOkrXXirXpDfcdVjxtbqGOvM()
			{
				if (wkpNeFCrHzcEPUxAratvQgFUxojE(pqkQWTNgxZgZDuoNDchCJcOmqHeX.TimedOut))
				{
					VsncEXPmWhCpsdBznLpDcPGGNdsDb(pqkQWTNgxZgZDuoNDchCJcOmqHeX.TimedOut, new TimedOutEventData(afWhZEYTbMFjMUeulVkAPjICyxpP));
				}
			}

			private void giuQAOGqgJGpaeVmdyUvrLopDENzA(string P_0)
			{
				if (wkpNeFCrHzcEPUxAratvQgFUxojE(pqkQWTNgxZgZDuoNDchCJcOmqHeX.Error))
				{
					VsncEXPmWhCpsdBznLpDcPGGNdsDb(pqkQWTNgxZgZDuoNDchCJcOmqHeX.Error, new ErrorEventData(afWhZEYTbMFjMUeulVkAPjICyxpP, P_0));
				}
			}

			private void ELYOwNrqZrDeeRwnkiivRaQzqjNs(string P_0)
			{
				if (wkpNeFCrHzcEPUxAratvQgFUxojE(pqkQWTNgxZgZDuoNDchCJcOmqHeX.Canceled))
				{
					VsncEXPmWhCpsdBznLpDcPGGNdsDb(pqkQWTNgxZgZDuoNDchCJcOmqHeX.Canceled, new CanceledEventData(afWhZEYTbMFjMUeulVkAPjICyxpP, P_0));
				}
			}

			private void KPPucdQoFIfBoANWjeNBCXpDcNUQB(ElementAssignmentInfo P_0, IList<ElementAssignmentConflictInfo> P_1, bool P_2)
			{
				if (wkpNeFCrHzcEPUxAratvQgFUxojE(pqkQWTNgxZgZDuoNDchCJcOmqHeX.ConflictsFound))
				{
					VsncEXPmWhCpsdBznLpDcPGGNdsDb(pqkQWTNgxZgZDuoNDchCJcOmqHeX.ConflictsFound, new ConflictFoundEventData(afWhZEYTbMFjMUeulVkAPjICyxpP, UAUojKKmPKyQXeiIwvgxiCJhByZFA, P_0, P_1, P_2));
				}
			}

			private void ykcpdHpSundMctFmxDBIkMsMwkTvA()
			{
				if (wkpNeFCrHzcEPUxAratvQgFUxojE(pqkQWTNgxZgZDuoNDchCJcOmqHeX.Started))
				{
					VsncEXPmWhCpsdBznLpDcPGGNdsDb(pqkQWTNgxZgZDuoNDchCJcOmqHeX.Started, new StartedEventData(afWhZEYTbMFjMUeulVkAPjICyxpP));
				}
			}

			private void xLsGPWjIjHQugjDsVbvTUfZqPFCkA()
			{
				if (wkpNeFCrHzcEPUxAratvQgFUxojE(pqkQWTNgxZgZDuoNDchCJcOmqHeX.Stopped))
				{
					VsncEXPmWhCpsdBznLpDcPGGNdsDb(pqkQWTNgxZgZDuoNDchCJcOmqHeX.Stopped, new StoppedEventData(afWhZEYTbMFjMUeulVkAPjICyxpP));
				}
			}

			public void UAUojKKmPKyQXeiIwvgxiCJhByZFA(ConflictResponse P_0)
			{
				if (XdgtdolzOKCisEMMipKQoOiLOdpP != Status.AwaitingResponse || mCQjtXATsANARKIfzgFjCFzLJbuk != ntchdUCcXwcgtXgjQQoXMalaVOZY.ConflictChecking)
				{
					Logger.LogWarning("The Mapping Listener was not waiting for a conflict checking response. The response will be ignored.");
					return;
				}
				try
				{
					if (pyvXxNScWSyyWnhaFCmMYsdGfuIN(P_0, wzLoyrPiQXkkLKFeRGxQilFyGjAW) == orJKwROrxEMHlZoxkEuFEygqZgJe.Continue)
					{
						uKsxONBMitySWjBsbNQCaNoHODzg(wzLoyrPiQXkkLKFeRGxQilFyGjAW);
					}
				}
				catch (Exception ex)
				{
					Logger.LogError("An exception occurred in the conflict check user response callback.\n" + ex);
				}
			}
		}

		public class Options
		{
			[Serializable]
			private sealed class KlqFwApbPSjPRAtbGRKUfSdtEfxiA
			{
				public static readonly KlqFwApbPSjPRAtbGRKUfSdtEfxiA _003C_003E9 = new KlqFwApbPSjPRAtbGRKUfSdtEfxiA();

				public static Action<Exception> _003C_003E9__64_0;

				internal void HEmcIuwmkXHRtdQYatrKCGtQfrEQA(Exception P_0)
				{
					ReInput.HandleCallbackException("InputMapper.Options.isElementAllowedCallback", P_0);
				}
			}

			private bool IDPlvLmvqniVnBkNLduvMBGhhgoJ = true;

			private bool GYYKsUfQAvHqiGkybLpagildpMBMB = true;

			private bool MDPuypiBRcAoKOWhcnOZosrsRkyP = true;

			private float xRNCaqgKPAyRGPJLoiKsOYWNCGdRA;

			private bool WMmVYLXfNSKDUyXjoBbZHboFGnogA = true;

			private bool kWWfdMvCuNaUyfrFZmQkblqizLTiB = true;

			private bool ilSnwtrglULTnOhqJautuPKbGAKn = true;

			private bool mUKfPJHjhyqwCOWcAewLNAKTELllA = true;

			private int[] kkOpBBidpWKrnMNyxIsrIeLWSTlK;

			private ConflictResponse OWCkeBziEouRJRnoAgCFhBhPFobi = ConflictResponse.Replace;

			private bool PbumTIAPdGbQbaGQKLcBLfEqoNnvA;

			private bool njiBtqGzYTEYcKVKEUjVepkJLgMCA;

			private bool bMCLrBQXlpobcuKOSepkFZFwnOju = true;

			private bool olgzYYtkqXhUvreijXBRFMlhjXLy = true;

			private float HyZFlvYpIwEbpyDWQFrICFwcgdvQA = 1f;

			internal const string xUYjZpzFCvsWRWbASgswBDnvNOOv = "isElementAllowed";

			private readonly Dictionary<string, SafeDelegate> EYTwJGPmykStfmwylYPUYmHfLkXc = new Dictionary<string, SafeDelegate> { { "isElementAllowed", null } };

			public bool allowAxes
			{
				get
				{
					return IDPlvLmvqniVnBkNLduvMBGhhgoJ;
				}
				set
				{
					IDPlvLmvqniVnBkNLduvMBGhhgoJ = value;
				}
			}

			public bool allowButtons
			{
				get
				{
					return GYYKsUfQAvHqiGkybLpagildpMBMB;
				}
				set
				{
					GYYKsUfQAvHqiGkybLpagildpMBMB = value;
				}
			}

			public bool allowButtonsOnFullAxisAssignment
			{
				get
				{
					return MDPuypiBRcAoKOWhcnOZosrsRkyP;
				}
				set
				{
					MDPuypiBRcAoKOWhcnOZosrsRkyP = value;
				}
			}

			public float timeout
			{
				get
				{
					return xRNCaqgKPAyRGPJLoiKsOYWNCGdRA;
				}
				set
				{
					xRNCaqgKPAyRGPJLoiKsOYWNCGdRA = MathTools.Max(0f, value);
				}
			}

			public bool checkForConflicts
			{
				get
				{
					return WMmVYLXfNSKDUyXjoBbZHboFGnogA;
				}
				set
				{
					WMmVYLXfNSKDUyXjoBbZHboFGnogA = value;
				}
			}

			public bool checkForConflictsWithAllPlayers
			{
				get
				{
					return kWWfdMvCuNaUyfrFZmQkblqizLTiB;
				}
				set
				{
					kWWfdMvCuNaUyfrFZmQkblqizLTiB = value;
				}
			}

			public bool checkForConflictsWithSelf
			{
				get
				{
					return ilSnwtrglULTnOhqJautuPKbGAKn;
				}
				set
				{
					ilSnwtrglULTnOhqJautuPKbGAKn = value;
				}
			}

			public bool checkForConflictsWithSystemPlayer
			{
				get
				{
					return mUKfPJHjhyqwCOWcAewLNAKTELllA;
				}
				set
				{
					mUKfPJHjhyqwCOWcAewLNAKTELllA = value;
				}
			}

			public int[] checkForConflictsWithPlayerIds
			{
				get
				{
					return kkOpBBidpWKrnMNyxIsrIeLWSTlK;
				}
				set
				{
					kkOpBBidpWKrnMNyxIsrIeLWSTlK = value;
				}
			}

			public ConflictResponse defaultActionWhenConflictFound
			{
				get
				{
					return OWCkeBziEouRJRnoAgCFhBhPFobi;
				}
				set
				{
					OWCkeBziEouRJRnoAgCFhBhPFobi = value;
				}
			}

			public bool ignoreMouseXAxis
			{
				get
				{
					return PbumTIAPdGbQbaGQKLcBLfEqoNnvA;
				}
				set
				{
					PbumTIAPdGbQbaGQKLcBLfEqoNnvA = value;
				}
			}

			public bool ignoreMouseYAxis
			{
				get
				{
					return njiBtqGzYTEYcKVKEUjVepkJLgMCA;
				}
				set
				{
					njiBtqGzYTEYcKVKEUjVepkJLgMCA = value;
				}
			}

			public bool allowKeyboardKeysWithModifiers
			{
				get
				{
					return bMCLrBQXlpobcuKOSepkFZFwnOju;
				}
				set
				{
					bMCLrBQXlpobcuKOSepkFZFwnOju = value;
				}
			}

			public bool allowKeyboardModifierKeyAsPrimary
			{
				get
				{
					return olgzYYtkqXhUvreijXBRFMlhjXLy;
				}
				set
				{
					olgzYYtkqXhUvreijXBRFMlhjXLy = value;
				}
			}

			public float holdDurationToMapKeyboardModifierKeyAsPrimary
			{
				get
				{
					return HyZFlvYpIwEbpyDWQFrICFwcgdvQA;
				}
				set
				{
					HyZFlvYpIwEbpyDWQFrICFwcgdvQA = MathTools.Max(0f, value);
				}
			}

			public Predicate<ControllerPollingInfo> isElementAllowedCallback
			{
				get
				{
					return (SafePredicate<ControllerPollingInfo>)EYTwJGPmykStfmwylYPUYmHfLkXc["isElementAllowed"];
				}
				set
				{
					SafePredicate<ControllerPollingInfo> safePredicate = value;
					if (safePredicate != null)
					{
						safePredicate.ExceptionHandler = KlqFwApbPSjPRAtbGRKUfSdtEfxiA._003C_003E9.HEmcIuwmkXHRtdQYatrKCGtQfrEQA;
					}
					EYTwJGPmykStfmwylYPUYmHfLkXc["isElementAllowed"] = safePredicate;
				}
			}

			internal _0001 ILFjrlXErAaQYsYRoWPNsXQagWVF<_0001>(string P_0) where _0001 : SafeDelegate
			{
				if (!EYTwJGPmykStfmwylYPUYmHfLkXc.TryGetValue(P_0, out var value))
				{
					return null;
				}
				return value as _0001;
			}

			public Options()
			{
				SPGTRPyvIslcMdbPTItsewSLRPxx();
			}

			private Options(Options P_0)
			{
				if (P_0 == null)
				{
					throw new ArgumentNullException("source");
				}
				Copy(P_0, this);
			}

			public Options Clone()
			{
				return new Options(this);
			}

			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("Options:\n");
				stringBuilder.Append("allowAxes = " + IDPlvLmvqniVnBkNLduvMBGhhgoJ + "\n");
				stringBuilder.Append("allowButtons = " + GYYKsUfQAvHqiGkybLpagildpMBMB + "\n");
				stringBuilder.Append("allowButtonsOnFullAxisAssignment = " + MDPuypiBRcAoKOWhcnOZosrsRkyP + "\n");
				stringBuilder.Append("timeout = " + xRNCaqgKPAyRGPJLoiKsOYWNCGdRA + "\n");
				stringBuilder.Append("checkForConflicts = " + WMmVYLXfNSKDUyXjoBbZHboFGnogA + "\n");
				stringBuilder.Append("checkForConflictsWithAllPlayers = " + kWWfdMvCuNaUyfrFZmQkblqizLTiB + "\n");
				stringBuilder.Append("checkForConflictsWithSelf = " + ilSnwtrglULTnOhqJautuPKbGAKn + "\n");
				stringBuilder.Append("checkForConflictsWithSystemPlayer = " + mUKfPJHjhyqwCOWcAewLNAKTELllA + "\n");
				if (kkOpBBidpWKrnMNyxIsrIeLWSTlK == null)
				{
					stringBuilder.Append("_checkForConflictsWithPlayerIds = null\n");
				}
				else
				{
					stringBuilder.Append("_checkForConflictsWithPlayerIds = " + StringTools.ToString(kkOpBBidpWKrnMNyxIsrIeLWSTlK) + "\n");
				}
				stringBuilder.Append("defaultActionWhenConflictFound = " + OWCkeBziEouRJRnoAgCFhBhPFobi.ToString() + "\n");
				stringBuilder.Append("ignoreMouseXAxis = " + PbumTIAPdGbQbaGQKLcBLfEqoNnvA);
				stringBuilder.Append("ignoreMouseYAxis = " + njiBtqGzYTEYcKVKEUjVepkJLgMCA);
				stringBuilder.Append("allowKeyboardKeysWithModifiers = " + bMCLrBQXlpobcuKOSepkFZFwnOju + "\n");
				stringBuilder.Append("allowKeyboardModifierAsPrimary = " + olgzYYtkqXhUvreijXBRFMlhjXLy + "\n");
				stringBuilder.Append("holdDurationToMapKeyboardModifierKeyAsPrimary = " + HyZFlvYpIwEbpyDWQFrICFwcgdvQA + "\n");
				return stringBuilder.ToString();
			}

			internal void SPGTRPyvIslcMdbPTItsewSLRPxx()
			{
				IDPlvLmvqniVnBkNLduvMBGhhgoJ = true;
				GYYKsUfQAvHqiGkybLpagildpMBMB = true;
				MDPuypiBRcAoKOWhcnOZosrsRkyP = true;
				xRNCaqgKPAyRGPJLoiKsOYWNCGdRA = 0f;
				WMmVYLXfNSKDUyXjoBbZHboFGnogA = true;
				kWWfdMvCuNaUyfrFZmQkblqizLTiB = true;
				ilSnwtrglULTnOhqJautuPKbGAKn = true;
				mUKfPJHjhyqwCOWcAewLNAKTELllA = true;
				kkOpBBidpWKrnMNyxIsrIeLWSTlK = null;
				OWCkeBziEouRJRnoAgCFhBhPFobi = ConflictResponse.Replace;
				PbumTIAPdGbQbaGQKLcBLfEqoNnvA = false;
				njiBtqGzYTEYcKVKEUjVepkJLgMCA = false;
				bMCLrBQXlpobcuKOSepkFZFwnOju = true;
				olgzYYtkqXhUvreijXBRFMlhjXLy = true;
				HyZFlvYpIwEbpyDWQFrICFwcgdvQA = 1f;
				foreach (string item in new List<string>(EYTwJGPmykStfmwylYPUYmHfLkXc.Keys))
				{
					EYTwJGPmykStfmwylYPUYmHfLkXc[item] = null;
				}
			}

			public static void Copy(Options source, Options destination)
			{
				if (source == null)
				{
					throw new ArgumentNullException("source");
				}
				if (destination == null)
				{
					throw new ArgumentNullException("destination");
				}
				destination.IDPlvLmvqniVnBkNLduvMBGhhgoJ = source.IDPlvLmvqniVnBkNLduvMBGhhgoJ;
				destination.GYYKsUfQAvHqiGkybLpagildpMBMB = source.GYYKsUfQAvHqiGkybLpagildpMBMB;
				destination.MDPuypiBRcAoKOWhcnOZosrsRkyP = source.MDPuypiBRcAoKOWhcnOZosrsRkyP;
				destination.xRNCaqgKPAyRGPJLoiKsOYWNCGdRA = source.xRNCaqgKPAyRGPJLoiKsOYWNCGdRA;
				destination.WMmVYLXfNSKDUyXjoBbZHboFGnogA = source.WMmVYLXfNSKDUyXjoBbZHboFGnogA;
				destination.kWWfdMvCuNaUyfrFZmQkblqizLTiB = source.kWWfdMvCuNaUyfrFZmQkblqizLTiB;
				destination.ilSnwtrglULTnOhqJautuPKbGAKn = source.ilSnwtrglULTnOhqJautuPKbGAKn;
				destination.mUKfPJHjhyqwCOWcAewLNAKTELllA = source.mUKfPJHjhyqwCOWcAewLNAKTELllA;
				destination.kkOpBBidpWKrnMNyxIsrIeLWSTlK = ArrayTools.ShallowCopy(source.kkOpBBidpWKrnMNyxIsrIeLWSTlK);
				destination.OWCkeBziEouRJRnoAgCFhBhPFobi = source.OWCkeBziEouRJRnoAgCFhBhPFobi;
				destination.PbumTIAPdGbQbaGQKLcBLfEqoNnvA = source.PbumTIAPdGbQbaGQKLcBLfEqoNnvA;
				destination.njiBtqGzYTEYcKVKEUjVepkJLgMCA = source.njiBtqGzYTEYcKVKEUjVepkJLgMCA;
				destination.bMCLrBQXlpobcuKOSepkFZFwnOju = source.bMCLrBQXlpobcuKOSepkFZFwnOju;
				destination.olgzYYtkqXhUvreijXBRFMlhjXLy = source.olgzYYtkqXhUvreijXBRFMlhjXLy;
				destination.HyZFlvYpIwEbpyDWQFrICFwcgdvQA = source.HyZFlvYpIwEbpyDWQFrICFwcgdvQA;
				foreach (KeyValuePair<string, SafeDelegate> item in source.EYTwJGPmykStfmwylYPUYmHfLkXc)
				{
					destination.EYTwJGPmykStfmwylYPUYmHfLkXc[item.Key] = MiscTools.Clone(item.Value);
				}
			}
		}

		[Serializable]
		private sealed class CsGLCHGTcukphAwqbUFlUowthqtc
		{
			public static readonly CsGLCHGTcukphAwqbUFlUowthqtc _003C_003E9 = new CsGLCHGTcukphAwqbUFlUowthqtc();

			public static Action<Exception> _003C_003E9__54_0;

			public static Action<Exception> _003C_003E9__54_1;

			public static Action<Exception> _003C_003E9__54_2;

			public static Action<Exception> _003C_003E9__54_3;

			public static Action<Exception> _003C_003E9__54_4;

			public static Action<Exception> _003C_003E9__54_5;

			public static Action<Exception> _003C_003E9__54_6;

			internal void LxzSEBmrlSVTqfGKXIVzaayfTTdc(Exception P_0)
			{
				ReInput.HandleCallbackException("InputMapper.AssignedEvent", P_0);
			}

			internal void whQAbIKdlWAuKNfnCCpBCsQATqQq(Exception P_0)
			{
				ReInput.HandleCallbackException("InputMapper.ErrorEvent", P_0);
			}

			internal void GnyWwuvxKiIMVMoznmjfsbsRoSwP(Exception P_0)
			{
				ReInput.HandleCallbackException("InputMapper.CanceledEvent", P_0);
			}

			internal void XXnxMLQjJTcsWUofzcaMfDmkXJIUA(Exception P_0)
			{
				ReInput.HandleCallbackException("InputMapper.TimedOutEvent", P_0);
			}

			internal void UOYunpdinGHuNRmNChOUOPDbJPnU(Exception P_0)
			{
				ReInput.HandleCallbackException("InputMapper.StartedEvent", P_0);
			}

			internal void HhbwmKFqIeHIdcsYQeyCYPMSvbEoA(Exception P_0)
			{
				ReInput.HandleCallbackException("InputMapper.StoppedEvent", P_0);
			}

			internal void OEwqbJSoFMHyBHdRIAzzLZQaFDvE(Exception P_0)
			{
				ReInput.HandleCallbackException("InputMapper.ConflictFoundEvent", P_0);
			}
		}

		private static InputMapper sKlAxiBmvyBxAjrDeOltNvBiEdkP;

		private static int TMzSxjjyapuKOmNiKYPcOmUjpTlD;

		private readonly int wfFGgdoRWtehIGyVjVmwswiztTuV;

		private readonly bool wAyxrYqTkTfseXCamASZxBaTedubA;

		private readonly CvqneWRBGLDpMpWAQjcseqxhBpXs gKQddRRUdFFTaMlSYrVtPlluZoRb;

		private Options bQaqZJNDPpHZPsCPODygOhVQNZSi;

		private readonly Dictionary<pqkQWTNgxZgZDuoNDchCJcOmqHeX, SafeDelegate> ItibWhVtFYsKkkzgAslrAMMJBUdm = new Dictionary<pqkQWTNgxZgZDuoNDchCJcOmqHeX, SafeDelegate>
		{
			{
				pqkQWTNgxZgZDuoNDchCJcOmqHeX.InputMapped,
				new SafeAction<InputMappedEventData>(CsGLCHGTcukphAwqbUFlUowthqtc._003C_003E9.LxzSEBmrlSVTqfGKXIVzaayfTTdc)
			},
			{
				pqkQWTNgxZgZDuoNDchCJcOmqHeX.Error,
				new SafeAction<ErrorEventData>(CsGLCHGTcukphAwqbUFlUowthqtc._003C_003E9.whQAbIKdlWAuKNfnCCpBCsQATqQq)
			},
			{
				pqkQWTNgxZgZDuoNDchCJcOmqHeX.Canceled,
				new SafeAction<CanceledEventData>(CsGLCHGTcukphAwqbUFlUowthqtc._003C_003E9.GnyWwuvxKiIMVMoznmjfsbsRoSwP)
			},
			{
				pqkQWTNgxZgZDuoNDchCJcOmqHeX.TimedOut,
				new SafeAction<TimedOutEventData>(CsGLCHGTcukphAwqbUFlUowthqtc._003C_003E9.XXnxMLQjJTcsWUofzcaMfDmkXJIUA)
			},
			{
				pqkQWTNgxZgZDuoNDchCJcOmqHeX.Started,
				new SafeAction<StartedEventData>(CsGLCHGTcukphAwqbUFlUowthqtc._003C_003E9.UOYunpdinGHuNRmNChOUOPDbJPnU)
			},
			{
				pqkQWTNgxZgZDuoNDchCJcOmqHeX.Stopped,
				new SafeAction<StoppedEventData>(CsGLCHGTcukphAwqbUFlUowthqtc._003C_003E9.HhbwmKFqIeHIdcsYQeyCYPMSvbEoA)
			},
			{
				pqkQWTNgxZgZDuoNDchCJcOmqHeX.ConflictsFound,
				new SafeAction<ConflictFoundEventData>(CsGLCHGTcukphAwqbUFlUowthqtc._003C_003E9.OEwqbJSoFMHyBHdRIAzzLZQaFDvE)
			}
		};

		public static InputMapper Default => sKlAxiBmvyBxAjrDeOltNvBiEdkP ?? (sKlAxiBmvyBxAjrDeOltNvBiEdkP = new InputMapper(true));

		public Options options
		{
			get
			{
				Options obj = bQaqZJNDPpHZPsCPODygOhVQNZSi;
				if (obj == null)
				{
					if (!wAyxrYqTkTfseXCamASZxBaTedubA)
					{
						return bQaqZJNDPpHZPsCPODygOhVQNZSi = Default.options.Clone();
					}
					obj = (bQaqZJNDPpHZPsCPODygOhVQNZSi = new Options());
				}
				return obj;
			}
			set
			{
				bQaqZJNDPpHZPsCPODygOhVQNZSi = value;
			}
		}

		public Context mappingContext => gKQddRRUdFFTaMlSYrVtPlluZoRb.kMUvbQCqjibbmcznhhMODCzGvqQpA;

		public Status status => gKQddRRUdFFTaMlSYrVtPlluZoRb.sziyftxgQGatauzlOMGISVXaTbkL;

		public float timeRemaining => gKQddRRUdFFTaMlSYrVtPlluZoRb.naGntkOZQLOShDdZHaUwnVhYxrwU;

		internal int QSZlWSsqUhpIVZjOleCIDuxHVUuQ => wfFGgdoRWtehIGyVjVmwswiztTuV;

		public event Action<InputMappedEventData> InputMappedEvent
		{
			add
			{
				if (value != null)
				{
					pqkQWTNgxZgZDuoNDchCJcOmqHeX key = pqkQWTNgxZgZDuoNDchCJcOmqHeX.InputMapped;
					ItibWhVtFYsKkkzgAslrAMMJBUdm[key] = (SafeAction<InputMappedEventData>)ItibWhVtFYsKkkzgAslrAMMJBUdm[key] + value;
				}
			}
			remove
			{
				if (value != null)
				{
					pqkQWTNgxZgZDuoNDchCJcOmqHeX key = pqkQWTNgxZgZDuoNDchCJcOmqHeX.InputMapped;
					ItibWhVtFYsKkkzgAslrAMMJBUdm[key] = (SafeAction<InputMappedEventData>)ItibWhVtFYsKkkzgAslrAMMJBUdm[key] - value;
				}
			}
		}

		public event Action<ErrorEventData> ErrorEvent
		{
			add
			{
				if (value != null)
				{
					pqkQWTNgxZgZDuoNDchCJcOmqHeX key = pqkQWTNgxZgZDuoNDchCJcOmqHeX.Error;
					ItibWhVtFYsKkkzgAslrAMMJBUdm[key] = (SafeAction<ErrorEventData>)ItibWhVtFYsKkkzgAslrAMMJBUdm[key] + value;
				}
			}
			remove
			{
				if (value != null)
				{
					pqkQWTNgxZgZDuoNDchCJcOmqHeX key = pqkQWTNgxZgZDuoNDchCJcOmqHeX.Error;
					ItibWhVtFYsKkkzgAslrAMMJBUdm[key] = (SafeAction<ErrorEventData>)ItibWhVtFYsKkkzgAslrAMMJBUdm[key] - value;
				}
			}
		}

		public event Action<CanceledEventData> CanceledEvent
		{
			add
			{
				if (value != null)
				{
					pqkQWTNgxZgZDuoNDchCJcOmqHeX key = pqkQWTNgxZgZDuoNDchCJcOmqHeX.Canceled;
					ItibWhVtFYsKkkzgAslrAMMJBUdm[key] = (SafeAction<CanceledEventData>)ItibWhVtFYsKkkzgAslrAMMJBUdm[key] + value;
				}
			}
			remove
			{
				if (value != null)
				{
					pqkQWTNgxZgZDuoNDchCJcOmqHeX key = pqkQWTNgxZgZDuoNDchCJcOmqHeX.Canceled;
					ItibWhVtFYsKkkzgAslrAMMJBUdm[key] = (SafeAction<CanceledEventData>)ItibWhVtFYsKkkzgAslrAMMJBUdm[key] - value;
				}
			}
		}

		public event Action<TimedOutEventData> TimedOutEvent
		{
			add
			{
				if (value != null)
				{
					pqkQWTNgxZgZDuoNDchCJcOmqHeX key = pqkQWTNgxZgZDuoNDchCJcOmqHeX.TimedOut;
					ItibWhVtFYsKkkzgAslrAMMJBUdm[key] = (SafeAction<TimedOutEventData>)ItibWhVtFYsKkkzgAslrAMMJBUdm[key] + value;
				}
			}
			remove
			{
				if (value != null)
				{
					pqkQWTNgxZgZDuoNDchCJcOmqHeX key = pqkQWTNgxZgZDuoNDchCJcOmqHeX.TimedOut;
					ItibWhVtFYsKkkzgAslrAMMJBUdm[key] = (SafeAction<TimedOutEventData>)ItibWhVtFYsKkkzgAslrAMMJBUdm[key] - value;
				}
			}
		}

		public event Action<StartedEventData> StartedEvent
		{
			add
			{
				if (value != null)
				{
					pqkQWTNgxZgZDuoNDchCJcOmqHeX key = pqkQWTNgxZgZDuoNDchCJcOmqHeX.Started;
					ItibWhVtFYsKkkzgAslrAMMJBUdm[key] = (SafeAction<StartedEventData>)ItibWhVtFYsKkkzgAslrAMMJBUdm[key] + value;
				}
			}
			remove
			{
				if (value != null)
				{
					pqkQWTNgxZgZDuoNDchCJcOmqHeX key = pqkQWTNgxZgZDuoNDchCJcOmqHeX.Started;
					ItibWhVtFYsKkkzgAslrAMMJBUdm[key] = (SafeAction<StartedEventData>)ItibWhVtFYsKkkzgAslrAMMJBUdm[key] - value;
				}
			}
		}

		public event Action<StoppedEventData> StoppedEvent
		{
			add
			{
				if (value != null)
				{
					pqkQWTNgxZgZDuoNDchCJcOmqHeX key = pqkQWTNgxZgZDuoNDchCJcOmqHeX.Stopped;
					ItibWhVtFYsKkkzgAslrAMMJBUdm[key] = (SafeAction<StoppedEventData>)ItibWhVtFYsKkkzgAslrAMMJBUdm[key] + value;
				}
			}
			remove
			{
				if (value != null)
				{
					pqkQWTNgxZgZDuoNDchCJcOmqHeX key = pqkQWTNgxZgZDuoNDchCJcOmqHeX.Stopped;
					ItibWhVtFYsKkkzgAslrAMMJBUdm[key] = (SafeAction<StoppedEventData>)ItibWhVtFYsKkkzgAslrAMMJBUdm[key] - value;
				}
			}
		}

		public event Action<ConflictFoundEventData> ConflictFoundEvent
		{
			add
			{
				if (value != null)
				{
					pqkQWTNgxZgZDuoNDchCJcOmqHeX key = pqkQWTNgxZgZDuoNDchCJcOmqHeX.ConflictsFound;
					ItibWhVtFYsKkkzgAslrAMMJBUdm[key] = (SafeAction<ConflictFoundEventData>)ItibWhVtFYsKkkzgAslrAMMJBUdm[key] + value;
				}
			}
			remove
			{
				if (value != null)
				{
					pqkQWTNgxZgZDuoNDchCJcOmqHeX key = pqkQWTNgxZgZDuoNDchCJcOmqHeX.ConflictsFound;
					ItibWhVtFYsKkkzgAslrAMMJBUdm[key] = (SafeAction<ConflictFoundEventData>)ItibWhVtFYsKkkzgAslrAMMJBUdm[key] - value;
				}
			}
		}

		private static int ZbDIHfcjnfeHcHypbxiSfPJiJyKdA()
		{
			int tMzSxjjyapuKOmNiKYPcOmUjpTlD = TMzSxjjyapuKOmNiKYPcOmUjpTlD;
			if (TMzSxjjyapuKOmNiKYPcOmUjpTlD == int.MaxValue)
			{
				TMzSxjjyapuKOmNiKYPcOmUjpTlD = 0;
				return tMzSxjjyapuKOmNiKYPcOmUjpTlD;
			}
			TMzSxjjyapuKOmNiKYPcOmUjpTlD++;
			return tMzSxjjyapuKOmNiKYPcOmUjpTlD;
		}

		public InputMapper()
			: this(false)
		{
			wfFGgdoRWtehIGyVjVmwswiztTuV = ZbDIHfcjnfeHcHypbxiSfPJiJyKdA();
		}

		private InputMapper(bool P_0)
		{
			wAyxrYqTkTfseXCamASZxBaTedubA = P_0;
			if (wAyxrYqTkTfseXCamASZxBaTedubA)
			{
				bQaqZJNDPpHZPsCPODygOhVQNZSi = new Options();
			}
			gKQddRRUdFFTaMlSYrVtPlluZoRb = new CvqneWRBGLDpMpWAQjcseqxhBpXs(this, ItibWhVtFYsKkkzgAslrAMMJBUdm);
		}

		public void RemoveEventListeners(object listenerOrParent)
		{
			if (listenerOrParent == null)
			{
				return;
			}
			foreach (KeyValuePair<pqkQWTNgxZgZDuoNDchCJcOmqHeX, SafeDelegate> item in ItibWhVtFYsKkkzgAslrAMMJBUdm)
			{
				item.Value.RemoveDelegateOrAllDelegatesFromAnObject(listenerOrParent);
			}
		}

		public void RemoveAllEventListeners()
		{
			foreach (KeyValuePair<pqkQWTNgxZgZDuoNDchCJcOmqHeX, SafeDelegate> item in ItibWhVtFYsKkkzgAslrAMMJBUdm)
			{
				item.Value.Clear();
			}
		}

		internal void QGoWwGTHIgFNtnnAzcjJuXXWdtOR(object P_0)
		{
		}

		internal void evVJrhpZcMcQSGvGODgHKmRArfFv()
		{
		}

		public bool Start(Context mappingContext)
		{
			return edGQWybkvEgdpdpFVqgmUdPGZTqV(mappingContext, (bQaqZJNDPpHZPsCPODygOhVQNZSi != null) ? bQaqZJNDPpHZPsCPODygOhVQNZSi : Default.options);
		}

		public void Stop()
		{
			gKQddRRUdFFTaMlSYrVtPlluZoRb.aewbAWLwjOMPaDEVSdRuwWfvOfOy("User canceled.");
		}

		public void Clear()
		{
			Stop();
			RemoveAllEventListeners();
			evVJrhpZcMcQSGvGODgHKmRArfFv();
			bQaqZJNDPpHZPsCPODygOhVQNZSi = null;
		}

		private bool edGQWybkvEgdpdpFVqgmUdPGZTqV(Context P_0, Options P_1)
		{
			if (!ReInput.isReady)
			{
				return false;
			}
			if (P_0 == null)
			{
				Logger.LogError("The Context cannot be null.");
				return false;
			}
			if (P_0.controllerMap == null)
			{
				Logger.LogError("The Controller Map cannot be null.");
				return false;
			}
			if (P_0.actionElementMapToReplace != null && !P_0.controllerMap.ContainsElementMap(P_0.actionElementMapToReplace))
			{
				Logger.LogError("The Action Element Map must belong to the same Controller Map you are passing in.");
				return false;
			}
			try
			{
				gKQddRRUdFFTaMlSYrVtPlluZoRb.edGQWybkvEgdpdpFVqgmUdPGZTqV(P_0, P_1);
				return true;
			}
			catch
			{
				gKQddRRUdFFTaMlSYrVtPlluZoRb.aewbAWLwjOMPaDEVSdRuwWfvOfOy("Failed to start due to an exception.");
				return false;
			}
		}
	}
}
