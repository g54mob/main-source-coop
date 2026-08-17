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
			private int vjknechPsSfQkZSCkSRQbZAmqtXX = -1;

			private ControllerMap GcPaoqjLHqNMJBFlheleBgCBvYuRB;

			private ActionElementMap EKGqpxrqZrNMsBlKHQKddMctQjbT;

			private AxisRange QzVJTehNlgNAhEmJduwVhHEJnkFC = AxisRange.Positive;

			private bool XtqkfdQoddtenDnhQjTEdoQQPjjAA;

			public int actionId
			{
				get
				{
					return vjknechPsSfQkZSCkSRQbZAmqtXX;
				}
				set
				{
					if (!XyutGZnwkUqnLjfNdYCpLzjPrWFC())
					{
						vjknechPsSfQkZSCkSRQbZAmqtXX = value;
					}
				}
			}

			public string actionName
			{
				get
				{
					InputAction action = ReInput.mapping.GetAction(vjknechPsSfQkZSCkSRQbZAmqtXX);
					if (action == null)
					{
						return string.Empty;
					}
					return action.name;
				}
				set
				{
					if (!XyutGZnwkUqnLjfNdYCpLzjPrWFC())
					{
						InputAction action = ReInput.mapping.GetAction(value);
						if (action == null)
						{
							vjknechPsSfQkZSCkSRQbZAmqtXX = -1;
							Logger.LogError("The Action \"" + value + "\" is not a valid Action and cannot be used!");
						}
						else
						{
							vjknechPsSfQkZSCkSRQbZAmqtXX = action.id;
						}
					}
				}
			}

			public ControllerMap controllerMap
			{
				get
				{
					return GcPaoqjLHqNMJBFlheleBgCBvYuRB;
				}
				set
				{
					if (!XyutGZnwkUqnLjfNdYCpLzjPrWFC())
					{
						GcPaoqjLHqNMJBFlheleBgCBvYuRB = value;
					}
				}
			}

			public ActionElementMap actionElementMapToReplace
			{
				get
				{
					return EKGqpxrqZrNMsBlKHQKddMctQjbT;
				}
				set
				{
					if (!XyutGZnwkUqnLjfNdYCpLzjPrWFC())
					{
						EKGqpxrqZrNMsBlKHQKddMctQjbT = value;
					}
				}
			}

			public AxisRange actionRange
			{
				get
				{
					return QzVJTehNlgNAhEmJduwVhHEJnkFC;
				}
				set
				{
					if (!XyutGZnwkUqnLjfNdYCpLzjPrWFC())
					{
						QzVJTehNlgNAhEmJduwVhHEJnkFC = value;
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

			internal void hRoGhfsvQZtMcGTBODScYVadgsmL()
			{
				XtqkfdQoddtenDnhQjTEdoQQPjjAA = true;
			}

			private bool XyutGZnwkUqnLjfNdYCpLzjPrWFC()
			{
				if (XtqkfdQoddtenDnhQjTEdoQQPjjAA)
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
				destination.vjknechPsSfQkZSCkSRQbZAmqtXX = source.vjknechPsSfQkZSCkSRQbZAmqtXX;
				destination.GcPaoqjLHqNMJBFlheleBgCBvYuRB = source.GcPaoqjLHqNMJBFlheleBgCBvYuRB;
				destination.EKGqpxrqZrNMsBlKHQKddMctQjbT = source.EKGqpxrqZrNMsBlKHQKddMctQjbT;
				destination.QzVJTehNlgNAhEmJduwVhHEJnkFC = source.QzVJTehNlgNAhEmJduwVhHEJnkFC;
			}
		}

		public enum ConflictResponse
		{
			Cancel = 0,
			Replace = 1,
			Add = 2,
			Ignore = 3,
			Swap = 4
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

			private readonly Func<int, bool> PMbmsntgekiJdfnAmTQmVoxUDsSeA;

			public bool IsSwapAllowed(int maxInputFieldCount)
			{
				if (PMbmsntgekiJdfnAmTQmVoxUDsSeA == null)
				{
					return false;
				}
				return PMbmsntgekiJdfnAmTQmVoxUDsSeA(maxInputFieldCount);
			}

			internal ConflictFoundEventData(InputMapper P_0, Action<ConflictResponse> P_1, ElementAssignmentInfo P_2, IList<ElementAssignmentConflictInfo> P_3, bool P_4, Func<int, bool> P_5)
				: base(P_0)
			{
				responseCallback = P_1;
				assignment = P_2;
				conflicts = P_3;
				isProtected = P_4;
				PMbmsntgekiJdfnAmTQmVoxUDsSeA = P_5;
			}
		}

		private enum RUUjrZvkAxzsIWNwpNzEkhPKtVfn
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

		private class iUSpOGpilluOVBqbohoiADoLvdIpA
		{
			private enum pWyGywFBBTKKLAlpHVtKfeBVlOYIc
			{
				Quit = 0,
				Continue = 1
			}

			private enum PDKWIIOgJETIuGFOekoXNceGHKQy
			{
				None = 0,
				ConflictChecking = 1
			}

			private class YudGZyihgeFRkmdjkdZLWDzwuEPM
			{
				private Player GqBRUkNRsEPlPXxzYZvQWCkgcDvS;

				private int rGHedZrGjMxGwRBohxNZvKFeDyut;

				private Context GkWZigGmwSYEnYficDdJgylvKmSAA;

				private ControllerType mExoOgJLatFUPjQydJSUsUMgWYVV;

				private int crNSndJFrgLNpBMokHhiEMFTnWPCA;

				private ControllerPollingInfo QUhKKorAfinukTvGDJDkxCffkssf;

				private ModifierKeyFlags XOpYtpECoqCzoFiCCBXsJbGrEXkf;

				public Player iVtfZWSuNqSTktUuYIhfVfoaoUko => GqBRUkNRsEPlPXxzYZvQWCkgcDvS;

				public int DZQFvCgovLqnRCPirQldsaYOBvgSA => rGHedZrGjMxGwRBohxNZvKFeDyut;

				public Context EfbAlpjtXjKuMJGgIeMCqsQNXcoD => GkWZigGmwSYEnYficDdJgylvKmSAA;

				public ControllerType faBsnBPuHqXjbwbmDEdxSnMXkRro => mExoOgJLatFUPjQydJSUsUMgWYVV;

				public int bGlyMFacSvOEhNqhqdCRcgVKMDBlA => crNSndJFrgLNpBMokHhiEMFTnWPCA;

				public ControllerPollingInfo RwNZleHDwGkNrRgGSnirsniMlyIs => QUhKKorAfinukTvGDJDkxCffkssf;

				public ModifierKeyFlags EqQRoUNHixRvLoBRUZqLdsjZcLIT => XOpYtpECoqCzoFiCCBXsJbGrEXkf;

				public AxisRange QggidMkjTVxdkBCMkEZIyjaGdLDx
				{
					get
					{
						AxisRange result = AxisRange.Positive;
						if (RwNZleHDwGkNrRgGSnirsniMlyIs.elementType == ControllerElementType.Axis)
						{
							result = ((GkWZigGmwSYEnYficDdJgylvKmSAA.actionRange != AxisRange.Full) ? ((RwNZleHDwGkNrRgGSnirsniMlyIs.axisPole == Pole.Positive) ? AxisRange.Positive : AxisRange.Negative) : AxisRange.Full);
						}
						return result;
					}
				}

				public string XbdAZNfOutrKOumKezLIhIOnpLQab
				{
					get
					{
						if (faBsnBPuHqXjbwbmDEdxSnMXkRro == ControllerType.Keyboard && EqQRoUNHixRvLoBRUZqLdsjZcLIT != ModifierKeyFlags.None)
						{
							return $"{Keyboard.ModifierKeyFlagsToString(EqQRoUNHixRvLoBRUZqLdsjZcLIT)} + {RwNZleHDwGkNrRgGSnirsniMlyIs.elementIdentifierName}";
						}
						string text = RwNZleHDwGkNrRgGSnirsniMlyIs.elementIdentifierName;
						if (RwNZleHDwGkNrRgGSnirsniMlyIs.elementType == ControllerElementType.Axis)
						{
							if (QggidMkjTVxdkBCMkEZIyjaGdLDx == AxisRange.Positive)
							{
								text += " +";
							}
							else if (QggidMkjTVxdkBCMkEZIyjaGdLDx == AxisRange.Negative)
							{
								text += " -";
							}
						}
						return text;
					}
				}

				public void eDzGtaGsqWBoNFJXooszNqHADapX(Player P_0, Context P_1)
				{
					if (P_1.controllerMap == null)
					{
						throw new ArgumentNullException("controllerMap");
					}
					xfEvSSTKDvYeCpagRjfmBhhHVYzD();
					GqBRUkNRsEPlPXxzYZvQWCkgcDvS = P_0;
					rGHedZrGjMxGwRBohxNZvKFeDyut = P_1.actionId;
					mExoOgJLatFUPjQydJSUsUMgWYVV = P_1.controllerMap.controllerType;
					crNSndJFrgLNpBMokHhiEMFTnWPCA = P_1.controllerMap.controllerId;
					GkWZigGmwSYEnYficDdJgylvKmSAA = P_1;
					mExoOgJLatFUPjQydJSUsUMgWYVV = P_1.controllerMap.controllerType;
					crNSndJFrgLNpBMokHhiEMFTnWPCA = P_1.controllerMap.controllerId;
					P_1.hRoGhfsvQZtMcGTBODScYVadgsmL();
				}

				public void xfEvSSTKDvYeCpagRjfmBhhHVYzD()
				{
					GqBRUkNRsEPlPXxzYZvQWCkgcDvS = null;
					rGHedZrGjMxGwRBohxNZvKFeDyut = -1;
					GkWZigGmwSYEnYficDdJgylvKmSAA = null;
					mExoOgJLatFUPjQydJSUsUMgWYVV = ControllerType.Keyboard;
					crNSndJFrgLNpBMokHhiEMFTnWPCA = -1;
					QUhKKorAfinukTvGDJDkxCffkssf = default(ControllerPollingInfo);
					XOpYtpECoqCzoFiCCBXsJbGrEXkf = ModifierKeyFlags.None;
				}

				public ElementAssignment eDXTsRrMLmvyBhAOaoJUErGLtihE(ControllerPollingInfo P_0)
				{
					QUhKKorAfinukTvGDJDkxCffkssf = P_0;
					return zYZxLRNvSbFQpNgjYJGUTCIllhF();
				}

				public ElementAssignment GPIdFnCSDLFUplbhWeSmObUXwUzMA(ControllerPollingInfo P_0, ModifierKeyFlags P_1)
				{
					QUhKKorAfinukTvGDJDkxCffkssf = P_0;
					XOpYtpECoqCzoFiCCBXsJbGrEXkf = P_1;
					return zYZxLRNvSbFQpNgjYJGUTCIllhF();
				}

				public ElementAssignment zYZxLRNvSbFQpNgjYJGUTCIllhF()
				{
					return new ElementAssignment(faBsnBPuHqXjbwbmDEdxSnMXkRro, QUhKKorAfinukTvGDJDkxCffkssf.elementType, QUhKKorAfinukTvGDJDkxCffkssf.elementIdentifierId, QggidMkjTVxdkBCMkEZIyjaGdLDx, QUhKKorAfinukTvGDJDkxCffkssf.keyboardKey, XOpYtpECoqCzoFiCCBXsJbGrEXkf, rGHedZrGjMxGwRBohxNZvKFeDyut, (GkWZigGmwSYEnYficDdJgylvKmSAA.actionRange == AxisRange.Negative) ? Pole.Negative : Pole.Positive, false, (GkWZigGmwSYEnYficDdJgylvKmSAA.actionElementMapToReplace != null) ? GkWZigGmwSYEnYficDdJgylvKmSAA.actionElementMapToReplace.id : (-1));
				}
			}

			private sealed class tSQdXsuMosbQFuccslsytyxCYVPd
			{
				public ActionElementMap LlTnQiYwIgbsjUxXRyVXKGFrfiXS;

				internal bool EotPiTkRXCnVRKDqBxYwSjcSzkZf(ElementAssignmentConflictInfo P_0)
				{
					return P_0.elementMapId == LlTnQiYwIgbsjUxXRyVXKGFrfiXS.id;
				}
			}

			private sealed class jhaWOmGFtfqtFfGXoiZjgHQJBYt
			{
				public iUSpOGpilluOVBqbohoiADoLvdIpA fLOzdtiBnghsjKjuTLGuJIXczmTs;

				public ElementAssignmentInfo nFWXUlWmIVHAUllIzDLbBXUuTxkMA;

				public IList<ElementAssignmentConflictInfo> UQjmyBQgIEsGvpXNHauHIJdkdUeeA;

				public bool ZQnAjdeufiHBwaXVFaTuIZreCVCs;

				internal bool HTGOUVGrSmKFVsUIvqXaPZQoiSAT(int P_0)
				{
					return fLOzdtiBnghsjKjuTLGuJIXczmTs.CjHfqsPRuodrizUAzciCkGPVVlRZ(nFWXUlWmIVHAUllIzDLbBXUuTxkMA, UQjmyBQgIEsGvpXNHauHIJdkdUeeA, ZQnAjdeufiHBwaXVFaTuIZreCVCs, P_0);
				}
			}

			private readonly InputMapper YRXLNHJZfcafMEOvgAYCGtZTwVvy;

			private readonly Options ViEPLUptWijHECrAsMdPdcDIodfM = new Options();

			private readonly YudGZyihgeFRkmdjkdZLWDzwuEPM iPlsoHhZZaKXtwdkeUadBAXXBwvd = new YudGZyihgeFRkmdjkdZLWDzwuEPM();

			private readonly Dictionary<RUUjrZvkAxzsIWNwpNzEkhPKtVfn, SafeDelegate> ZMHGyXdOcBHmhEJIQvHWhakpQEfy;

			private readonly Dictionary<string, SafeDelegate> BECpXavnboGHuUqMHAiEGhPGNdfnA;

			private Status mpnPasITZqPgFbVepuLrhkdpdkliA;

			private PDKWIIOgJETIuGFOekoXNceGHKQy xbdjVaeUuBmCzXMursJywYRXnHwgA;

			private double OykGGhCWicUeICyUwBgCyfaoabtqA;

			private bool opqnzxTjjuCkrnVjrsnwYEUYQItC;

			private List<Player> gDUavrTmBkoEbXZVnFhpncBEGxMW = new List<Player>();

			private readonly List<ControllerPollingInfo> yVRsCAICgutrbYiXUHDMGWKNwGMI = new List<ControllerPollingInfo>();

			private ElementAssignment pyZEWZkoXmKsvmrEUixCDjFpsTLG;

			public Status HYgTVIOcNoBWqoALLiYCisveLmbF => mpnPasITZqPgFbVepuLrhkdpdkliA;

			public float IxsqmqnGHpasWIMGHaYqQpSwUtDf
			{
				get
				{
					if (mpnPasITZqPgFbVepuLrhkdpdkliA == Status.Idle)
					{
						return 0f;
					}
					if (ViEPLUptWijHECrAsMdPdcDIodfM.timeout <= 0f)
					{
						return 0f;
					}
					return (float)MathTools.Max(0.0, OykGGhCWicUeICyUwBgCyfaoabtqA + (double)ViEPLUptWijHECrAsMdPdcDIodfM.timeout - ReInput.unscaledTime);
				}
			}

			public Context xzqtlFewlXbyivRbqfwQRtheTGbP
			{
				get
				{
					if (mpnPasITZqPgFbVepuLrhkdpdkliA == Status.Idle)
					{
						return null;
					}
					return iPlsoHhZZaKXtwdkeUadBAXXBwvd.EfbAlpjtXjKuMJGgIeMCqsQNXcoD;
				}
			}

			private bool MdWDEhLHUUERjNMFcVCBpFyGnjZB
			{
				get
				{
					if (opqnzxTjjuCkrnVjrsnwYEUYQItC)
					{
						return false;
					}
					return ViEPLUptWijHECrAsMdPdcDIodfM.timeout > 0f;
				}
			}

			public iUSpOGpilluOVBqbohoiADoLvdIpA(InputMapper P_0, Dictionary<RUUjrZvkAxzsIWNwpNzEkhPKtVfn, SafeDelegate> P_1)
			{
				if (P_0 == null)
				{
					throw new ArgumentNullException("parent");
				}
				if (P_1 == null)
				{
					throw new ArgumentNullException("events");
				}
				YRXLNHJZfcafMEOvgAYCGtZTwVvy = P_0;
				ZMHGyXdOcBHmhEJIQvHWhakpQEfy = P_1;
				NSamiWrqqmjZSFbGiOrUCeeIyqHH();
			}

			protected virtual void SRSIVHTGRraNMrwNedETzqfYWRSn()
			{
				try
				{
					QvZTYgtaUuZfveCaedxXBLCseXNv();
				}
				finally
				{
					base.Finalize();
				}
			}

			public void nduqsKumnOSsbdwmQvNbJGFikdPB(Context P_0, Options P_1)
			{
				if (mpnPasITZqPgFbVepuLrhkdpdkliA != Status.Idle)
				{
					XrkgkhHGZrSbxbZCtJpKdJqUCLOhb("User started a new listening session.");
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
				Options.Copy(P_1, ViEPLUptWijHECrAsMdPdcDIodfM);
				Player player = ReInput.players.GetPlayer(P_0.controllerMap.playerId);
				if (ReInput.mapping.GetAction(P_0.actionId) == null)
				{
					hSnjROgtWauzbSBQvFvjOnAYIxlS("No Action found for actionId: " + P_0.actionId);
					return;
				}
				iPlsoHhZZaKXtwdkeUadBAXXBwvd.eDzGtaGsqWBoNFJXooszNqHADapX(player, P_0);
				mpnPasITZqPgFbVepuLrhkdpdkliA = Status.Listening;
				SAEzcrPBAPCHICpWheAHVHYvMRww();
				uKWmoxZzHGMwhQDjJBzEgRJXbadXA();
				IuFjSTHIhLjssRZxKFpahdNxqVFm();
				BxgwIOUKHSmMQArXrkTDoDTHwrxj();
			}

			public void SIpWgzfzvCMNBnysysnMRJgfLzZF(string P_0)
			{
				if (mpnPasITZqPgFbVepuLrhkdpdkliA != Status.Idle)
				{
					XrkgkhHGZrSbxbZCtJpKdJqUCLOhb(P_0);
				}
			}

			private void opxUHWDSWLRjgvHhZMFxRYnpCUCR(UpdateLoopType P_0)
			{
				if (P_0 == UpdateLoopType.Update && mpnPasITZqPgFbVepuLrhkdpdkliA == Status.Listening)
				{
					ElementAssignment elementAssignment;
					if (MdWDEhLHUUERjNMFcVCBpFyGnjZB && IxsqmqnGHpasWIMGHaYqQpSwUtDf <= 0f)
					{
						cZxNGmvreXXhsXrBttNFFpqBADMl();
					}
					else if (ReInput.controllers.GetController(iPlsoHhZZaKXtwdkeUadBAXXBwvd.faBsnBPuHqXjbwbmDEdxSnMXkRro, iPlsoHhZZaKXtwdkeUadBAXXBwvd.bGlyMFacSvOEhNqhqdCRcgVKMDBlA) == null)
					{
						hSnjROgtWauzbSBQvFvjOnAYIxlS("Controller not found for type: " + iPlsoHhZZaKXtwdkeUadBAXXBwvd.faBsnBPuHqXjbwbmDEdxSnMXkRro.ToString() + " id: " + iPlsoHhZZaKXtwdkeUadBAXXBwvd.bGlyMFacSvOEhNqhqdCRcgVKMDBlA);
					}
					else if (KcCyaSGmdGwvszGMFukTBAYyadtp(out elementAssignment) != pWyGywFBBTKKLAlpHVtKfeBVlOYIc.Quit && IzxRXjEmtAoydCSWRpmvvAkWZXcv(elementAssignment) != pWyGywFBBTKKLAlpHVtKfeBVlOYIc.Quit)
					{
						bawArQArHvCiSndcdZDGMBAHbiTfA(elementAssignment);
					}
				}
			}

			private void XzuBPdQalzOHKURnLlGOfTHkteLo()
			{
				if (mpnPasITZqPgFbVepuLrhkdpdkliA != Status.Idle)
				{
					NSamiWrqqmjZSFbGiOrUCeeIyqHH();
					QvZTYgtaUuZfveCaedxXBLCseXNv();
					zSNexwcxUticPvBEjSASMFmQGLOn();
				}
			}

			private void NSamiWrqqmjZSFbGiOrUCeeIyqHH()
			{
				mpnPasITZqPgFbVepuLrhkdpdkliA = Status.Idle;
				OykGGhCWicUeICyUwBgCyfaoabtqA = 0.0;
				ViEPLUptWijHECrAsMdPdcDIodfM.nCIsEurlPuGVJRLEodcdEVbpmLgcA();
				iPlsoHhZZaKXtwdkeUadBAXXBwvd.xfEvSSTKDvYeCpagRjfmBhhHVYzD();
				pyZEWZkoXmKsvmrEUixCDjFpsTLG = default(ElementAssignment);
				xbdjVaeUuBmCzXMursJywYRXnHwgA = PDKWIIOgJETIuGFOekoXNceGHKQy.None;
				opqnzxTjjuCkrnVjrsnwYEUYQItC = false;
				gDUavrTmBkoEbXZVnFhpncBEGxMW.Clear();
			}

			private pWyGywFBBTKKLAlpHVtKfeBVlOYIc KcCyaSGmdGwvszGMFukTBAYyadtp(out ElementAssignment P_0)
			{
				if (!LbdAfFMeooiTlHcwihQHaDQyOboiA(out var enumerable, out var modifierKeyFlags))
				{
					P_0 = default(ElementAssignment);
					return pWyGywFBBTKKLAlpHVtKfeBVlOYIc.Quit;
				}
				ControllerPollingInfo controllerPollingInfo = default(ControllerPollingInfo);
				foreach (ControllerPollingInfo item in enumerable)
				{
					if (item.success && !dGAGEACmwuItzTDPRoYzlyCJlrqy(item, ViEPLUptWijHECrAsMdPdcDIodfM))
					{
						controllerPollingInfo = item;
						break;
					}
				}
				if (!controllerPollingInfo.success)
				{
					P_0 = default(ElementAssignment);
					return pWyGywFBBTKKLAlpHVtKfeBVlOYIc.Quit;
				}
				if (!QzyBGBuxJgbIKQziOdBPkQoFElSF(iPlsoHhZZaKXtwdkeUadBAXXBwvd, controllerPollingInfo, ViEPLUptWijHECrAsMdPdcDIodfM))
				{
					P_0 = default(ElementAssignment);
					return pWyGywFBBTKKLAlpHVtKfeBVlOYIc.Quit;
				}
				P_0 = iPlsoHhZZaKXtwdkeUadBAXXBwvd.eDXTsRrMLmvyBhAOaoJUErGLtihE(controllerPollingInfo);
				P_0.modifierKeyFlags = modifierKeyFlags;
				return pWyGywFBBTKKLAlpHVtKfeBVlOYIc.Continue;
			}

			private bool LbdAfFMeooiTlHcwihQHaDQyOboiA(out IEnumerable<ControllerPollingInfo> P_0, out ModifierKeyFlags P_1)
			{
				P_1 = ModifierKeyFlags.None;
				ControllerType controllerType = iPlsoHhZZaKXtwdkeUadBAXXBwvd.faBsnBPuHqXjbwbmDEdxSnMXkRro;
				int controllerId = iPlsoHhZZaKXtwdkeUadBAXXBwvd.bGlyMFacSvOEhNqhqdCRcgVKMDBlA;
				if (controllerType == ControllerType.Keyboard)
				{
					P_0 = vpBMhDrjkuZjwNKZgZiKicBDUDUX(out P_1);
					return true;
				}
				if (ViEPLUptWijHECrAsMdPdcDIodfM.allowAxes)
				{
					if (ViEPLUptWijHECrAsMdPdcDIodfM.allowButtons)
					{
						if (iPlsoHhZZaKXtwdkeUadBAXXBwvd.iVtfZWSuNqSTktUuYIhfVfoaoUko != null)
						{
							P_0 = iPlsoHhZZaKXtwdkeUadBAXXBwvd.iVtfZWSuNqSTktUuYIhfVfoaoUko.controllers.polling.PollControllerForAllElementsDown(controllerType, controllerId);
						}
						else
						{
							P_0 = ReInput.controllers.polling.PollControllerForAllElementsDown(iPlsoHhZZaKXtwdkeUadBAXXBwvd.faBsnBPuHqXjbwbmDEdxSnMXkRro, iPlsoHhZZaKXtwdkeUadBAXXBwvd.bGlyMFacSvOEhNqhqdCRcgVKMDBlA);
						}
					}
					else if (iPlsoHhZZaKXtwdkeUadBAXXBwvd.iVtfZWSuNqSTktUuYIhfVfoaoUko != null)
					{
						P_0 = iPlsoHhZZaKXtwdkeUadBAXXBwvd.iVtfZWSuNqSTktUuYIhfVfoaoUko.controllers.polling.PollControllerForAllAxes(controllerType, controllerId);
					}
					else
					{
						P_0 = ReInput.controllers.polling.PollControllerForAllAxes(controllerType, controllerId);
					}
				}
				else
				{
					if (!ViEPLUptWijHECrAsMdPdcDIodfM.allowButtons)
					{
						hSnjROgtWauzbSBQvFvjOnAYIxlS("You must enable listening for at least one element type.");
						P_0 = null;
						return false;
					}
					if (iPlsoHhZZaKXtwdkeUadBAXXBwvd.iVtfZWSuNqSTktUuYIhfVfoaoUko != null)
					{
						P_0 = iPlsoHhZZaKXtwdkeUadBAXXBwvd.iVtfZWSuNqSTktUuYIhfVfoaoUko.controllers.polling.PollControllerForAllButtonsDown(controllerType, controllerId);
					}
					else
					{
						P_0 = ReInput.controllers.polling.PollControllerForAllButtonsDown(controllerType, controllerId);
					}
				}
				return true;
			}

			private IEnumerable<ControllerPollingInfo> vpBMhDrjkuZjwNKZgZiKicBDUDUX(out ModifierKeyFlags P_0)
			{
				P_0 = ModifierKeyFlags.None;
				yVRsCAICgutrbYiXUHDMGWKNwGMI.Clear();
				if (!ViEPLUptWijHECrAsMdPdcDIodfM.allowButtons)
				{
					return yVRsCAICgutrbYiXUHDMGWKNwGMI;
				}
				yVRsCAICgutrbYiXUHDMGWKNwGMI.Add(vyOmTDICvIgvluzEKdILLhCMjNNk(ViEPLUptWijHECrAsMdPdcDIodfM, out P_0));
				return yVRsCAICgutrbYiXUHDMGWKNwGMI;
			}

			private ControllerPollingInfo vyOmTDICvIgvluzEKdILLhCMjNNk(Options P_0, out ModifierKeyFlags P_1)
			{
				bool flag;
				string text;
				ControllerPollingInfo result = xrvIgOqEmLgsuCluobwcahSBGtSwA(P_0, out flag, out P_1, out text);
				if (flag)
				{
					SAEzcrPBAPCHICpWheAHVHYvMRww();
				}
				return result;
			}

			private static ControllerPollingInfo xrvIgOqEmLgsuCluobwcahSBGtSwA(Options P_0, out bool P_1, out ModifierKeyFlags P_2, out string P_3)
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
						P_3 = Keyboard.ModifierKeyFlagsToString(modifierKeyFlags, getShortName: false);
					}
				}
				return default(ControllerPollingInfo);
			}

			private static bool dGAGEACmwuItzTDPRoYzlyCJlrqy(ControllerPollingInfo P_0, Options P_1)
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
				SafePredicate<ControllerPollingInfo> safePredicate = P_1.uJXJEaNGufdhHihztzbIATgypdhqA<SafePredicate<ControllerPollingInfo>>("isElementAllowed");
				if (safePredicate != null)
				{
					return !safePredicate.Invoke(P_0);
				}
				return false;
			}

			private static bool QzyBGBuxJgbIKQziOdBPkQoFElSF(YudGZyihgeFRkmdjkdZLWDzwuEPM P_0, ControllerPollingInfo P_1, Options P_2)
			{
				if (P_0 == null)
				{
					return false;
				}
				if (P_2 == null)
				{
					return true;
				}
				if (P_0.QggidMkjTVxdkBCMkEZIyjaGdLDx == AxisRange.Full && !P_2.allowButtonsOnFullAxisAssignment && P_1.elementType == ControllerElementType.Button)
				{
					return false;
				}
				return true;
			}

			private void uKWmoxZzHGMwhQDjJBzEgRJXbadXA()
			{
				if (!ViEPLUptWijHECrAsMdPdcDIodfM.checkForConflicts)
				{
					return;
				}
				if (ViEPLUptWijHECrAsMdPdcDIodfM.checkForConflictsWithSelf && iPlsoHhZZaKXtwdkeUadBAXXBwvd.iVtfZWSuNqSTktUuYIhfVfoaoUko != null)
				{
					ListTools.AddIfUnique(gDUavrTmBkoEbXZVnFhpncBEGxMW, iPlsoHhZZaKXtwdkeUadBAXXBwvd.iVtfZWSuNqSTktUuYIhfVfoaoUko);
				}
				if (ViEPLUptWijHECrAsMdPdcDIodfM.checkForConflictsWithSystemPlayer)
				{
					ListTools.AddIfUnique(gDUavrTmBkoEbXZVnFhpncBEGxMW, ReInput.players.SystemPlayer);
				}
				if (ViEPLUptWijHECrAsMdPdcDIodfM.checkForConflictsWithAllPlayers)
				{
					IList<Player> players = ReInput.players.Players;
					for (int i = 0; i < players.Count; i++)
					{
						ListTools.AddIfUnique(gDUavrTmBkoEbXZVnFhpncBEGxMW, players[i]);
					}
				}
				else
				{
					if (ViEPLUptWijHECrAsMdPdcDIodfM.checkForConflictsWithPlayerIds == null)
					{
						return;
					}
					IList<Player> allPlayers = ReInput.players.AllPlayers;
					int count = allPlayers.Count;
					for (int j = 0; j < count; j++)
					{
						if (ArrayTools.Contains(ViEPLUptWijHECrAsMdPdcDIodfM.checkForConflictsWithPlayerIds, allPlayers[j].id))
						{
							ListTools.AddIfUnique(gDUavrTmBkoEbXZVnFhpncBEGxMW, allPlayers[j]);
						}
					}
				}
			}

			private pWyGywFBBTKKLAlpHVtKfeBVlOYIc IzxRXjEmtAoydCSWRpmvvAkWZXcv(ElementAssignment P_0)
			{
				if (ViEPLUptWijHECrAsMdPdcDIodfM.checkForConflicts && iPlsoHhZZaKXtwdkeUadBAXXBwvd.iVtfZWSuNqSTktUuYIhfVfoaoUko != null && biqpGsyfflZqFTnmlbBMDqoOXvDuA(iPlsoHhZZaKXtwdkeUadBAXXBwvd, P_0, gDUavrTmBkoEbXZVnFhpncBEGxMW))
				{
					return EKhbgVdGbotBaxXwgGrbRHXiUjGVA(P_0);
				}
				return pWyGywFBBTKKLAlpHVtKfeBVlOYIc.Continue;
			}

			private static bool biqpGsyfflZqFTnmlbBMDqoOXvDuA(YudGZyihgeFRkmdjkdZLWDzwuEPM P_0, ElementAssignment P_1, List<Player> P_2)
			{
				if (P_0 == null || P_0.iVtfZWSuNqSTktUuYIhfVfoaoUko == null)
				{
					return false;
				}
				if (P_2 == null || P_2.Count == 0)
				{
					return false;
				}
				if (!tGUxMYoiFpvoPwJHexuibWFnkWSK(P_0, P_1, out var conflictCheck))
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

			private static bool SolGlxjkGMXZxEdRMDnmhSaysPfdb(YudGZyihgeFRkmdjkdZLWDzwuEPM P_0, ElementAssignment P_1, List<Player> P_2)
			{
				if (P_0 == null || P_0.iVtfZWSuNqSTktUuYIhfVfoaoUko == null)
				{
					return false;
				}
				if (P_2 == null || P_2.Count == 0)
				{
					return false;
				}
				if (!tGUxMYoiFpvoPwJHexuibWFnkWSK(P_0, P_1, out var conflictCheck))
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

			private static IList<ElementAssignmentConflictInfo> ieyqiXjoKjYuEdjESIqmBdpJpSDM(YudGZyihgeFRkmdjkdZLWDzwuEPM P_0, ElementAssignment P_1, List<Player> P_2)
			{
				if (P_0 == null || P_0.iVtfZWSuNqSTktUuYIhfVfoaoUko == null)
				{
					return null;
				}
				if (P_2 == null || P_2.Count == 0)
				{
					return null;
				}
				if (!tGUxMYoiFpvoPwJHexuibWFnkWSK(P_0, P_1, out var conflictCheck))
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

			private static bool tGUxMYoiFpvoPwJHexuibWFnkWSK(YudGZyihgeFRkmdjkdZLWDzwuEPM P_0, ElementAssignment P_1, out ElementAssignmentConflictCheck P_2)
			{
				Player player;
				if (P_0 == null || (player = P_0.iVtfZWSuNqSTktUuYIhfVfoaoUko) == null)
				{
					P_2 = default(ElementAssignmentConflictCheck);
					return false;
				}
				P_2 = P_1.ToElementAssignmentConflictCheck();
				P_2.playerId = player.id;
				P_2.controllerType = P_0.faBsnBPuHqXjbwbmDEdxSnMXkRro;
				P_2.controllerId = P_0.bGlyMFacSvOEhNqhqdCRcgVKMDBlA;
				P_2.controllerMapId = P_0.EfbAlpjtXjKuMJGgIeMCqsQNXcoD.controllerMap.id;
				P_2.controllerMapCategoryId = P_0.EfbAlpjtXjKuMJGgIeMCqsQNXcoD.controllerMap.categoryId;
				if (P_0.EfbAlpjtXjKuMJGgIeMCqsQNXcoD.actionElementMapToReplace != null)
				{
					P_2.elementMapId = P_0.EfbAlpjtXjKuMJGgIeMCqsQNXcoD.actionElementMapToReplace.id;
				}
				return true;
			}

			private static void nBjtGOQFrMdvGjLndCbVDxwWJJrbA(YudGZyihgeFRkmdjkdZLWDzwuEPM P_0, ElementAssignment P_1, List<Player> P_2)
			{
				if (P_0 == null || P_0.iVtfZWSuNqSTktUuYIhfVfoaoUko == null)
				{
					return;
				}
				if (!tGUxMYoiFpvoPwJHexuibWFnkWSK(P_0, P_1, out var conflictCheck))
				{
					Logger.LogError("Error creating conflict check!");
					return;
				}
				for (int i = 0; i < P_2.Count; i++)
				{
					P_2[i].controllers.conflictChecking.RemoveElementAssignmentConflicts(conflictCheck);
				}
			}

			private void IuFjSTHIhLjssRZxKFpahdNxqVFm()
			{
				ReInput.UpdateEndedEvent -= opxUHWDSWLRjgvHhZMFxRYnpCUCR;
				ReInput.UpdateEndedEvent += opxUHWDSWLRjgvHhZMFxRYnpCUCR;
			}

			private void QvZTYgtaUuZfveCaedxXBLCseXNv()
			{
				ReInput.UpdateEndedEvent -= opxUHWDSWLRjgvHhZMFxRYnpCUCR;
			}

			private bool wIdeAoQnynwsnVOLEhWzBGvYBXnEb(RUUjrZvkAxzsIWNwpNzEkhPKtVfn P_0)
			{
				SafeDelegate safeDelegate = ZMHGyXdOcBHmhEJIQvHWhakpQEfy[P_0];
				if (safeDelegate != null)
				{
					return safeDelegate.Count > 0;
				}
				return false;
			}

			private void BsXhymSCxjKpXdwnJWSsfQVIjuin<_0001>(RUUjrZvkAxzsIWNwpNzEkhPKtVfn P_0, _0001 P_1)
			{
				SafeAction<_0001> safeAction = (SafeAction<_0001>)ZMHGyXdOcBHmhEJIQvHWhakpQEfy[P_0];
				if (safeAction.Count != 0)
				{
					safeAction.Invoke(P_1);
				}
			}

			private void SAEzcrPBAPCHICpWheAHVHYvMRww()
			{
				OykGGhCWicUeICyUwBgCyfaoabtqA = ReInput.unscaledTime;
			}

			private void UMSWxCuoATCYCqdiceTccNMLdhJi()
			{
				opqnzxTjjuCkrnVjrsnwYEUYQItC = true;
			}

			private bool CjHfqsPRuodrizUAzciCkGPVVlRZ(ElementAssignmentInfo P_0, IList<ElementAssignmentConflictInfo> P_1, bool P_2, int P_3)
			{
				if (P_3 < 0)
				{
					P_3 = 0;
				}
				if (P_0 == null || P_1 == null)
				{
					return false;
				}
				if (P_2)
				{
					return false;
				}
				ActionElementMap elementMap = P_0.elementMap;
				if (elementMap == null)
				{
					return false;
				}
				List<ElementAssignmentConflictInfo> list = new List<ElementAssignmentConflictInfo>();
				for (int i = 0; i < P_1.Count; i++)
				{
					if (P_1[i].playerId == P_0.player.id)
					{
						list.Add(P_1[i]);
					}
				}
				if (list.Count > 1)
				{
					return false;
				}
				ElementAssignmentConflictInfo elementAssignmentConflictInfo = list[0];
				if (elementAssignmentConflictInfo.elementMap == null)
				{
					return false;
				}
				if (!elementAssignmentConflictInfo.isConflict)
				{
					return false;
				}
				if (elementAssignmentConflictInfo.playerId != P_0.player.id)
				{
					return false;
				}
				int actionId = elementAssignmentConflictInfo.elementMap.actionId;
				Pole axisContribution = elementAssignmentConflictInfo.elementMap.axisContribution;
				AxisRange axisRange = elementMap.axisRange;
				ControllerElementType elementType = elementMap.elementType;
				if (elementType == elementAssignmentConflictInfo.elementMap.elementType && elementType == ControllerElementType.Axis)
				{
					if (axisRange != elementAssignmentConflictInfo.elementMap.axisRange)
					{
						if (axisRange == AxisRange.Full)
						{
							axisRange = AxisRange.Positive;
						}
						else if (elementAssignmentConflictInfo.elementMap.axisRange != AxisRange.Full)
						{
						}
					}
				}
				else if (elementType == ControllerElementType.Axis && (elementAssignmentConflictInfo.elementMap.elementType == ControllerElementType.Button || (elementAssignmentConflictInfo.elementMap.elementType == ControllerElementType.Axis && elementAssignmentConflictInfo.elementMap.axisRange != AxisRange.Full)) && axisRange == AxisRange.Full)
				{
					axisRange = AxisRange.Positive;
				}
				int num = 0;
				if (P_0.action.id == elementAssignmentConflictInfo.actionId && P_0.controllerMap == elementAssignmentConflictInfo.controllerMap)
				{
					Controller controller = ReInput.controllers.GetController(P_0.controllerType, P_0.controllerId);
					if (QUyCDFFMJsMTeqKYYkmskJjWNMxZ(elementType, axisRange, axisContribution, controller.GetElementById(P_0.elementIdentifier.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid).type, P_0.axisRange, P_0.axisContribution))
					{
						num++;
					}
				}
				using (IEnumerator<ActionElementMap> enumerator = elementAssignmentConflictInfo.controllerMap.ElementMapsWithAction(actionId).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						tSQdXsuMosbQFuccslsytyxCYVPd tSQdXsuMosbQFuccslsytyxCYVPd2 = new tSQdXsuMosbQFuccslsytyxCYVPd();
						tSQdXsuMosbQFuccslsytyxCYVPd2.LlTnQiYwIgbsjUxXRyVXKGFrfiXS = enumerator.Current;
						if (tSQdXsuMosbQFuccslsytyxCYVPd2.LlTnQiYwIgbsjUxXRyVXKGFrfiXS.id != elementMap.id && ListTools.FindIndex(list, tSQdXsuMosbQFuccslsytyxCYVPd2.EotPiTkRXCnVRKDqBxYwSjcSzkZf) < 0 && QUyCDFFMJsMTeqKYYkmskJjWNMxZ(elementType, axisRange, axisContribution, tSQdXsuMosbQFuccslsytyxCYVPd2.LlTnQiYwIgbsjUxXRyVXKGFrfiXS.elementType, tSQdXsuMosbQFuccslsytyxCYVPd2.LlTnQiYwIgbsjUxXRyVXKGFrfiXS.axisRange, tSQdXsuMosbQFuccslsytyxCYVPd2.LlTnQiYwIgbsjUxXRyVXKGFrfiXS.axisContribution))
						{
							num++;
						}
					}
				}
				return num < P_3;
			}

			private bool eEFwGOPdJozQkekuGYjWXrVrafNO(YudGZyihgeFRkmdjkdZLWDzwuEPM P_0, ElementAssignment P_1, bool P_2, out string P_3)
			{
				if (P_0 == null)
				{
					P_3 = "Mapping is null reference.";
					return false;
				}
				List<Player> list = new List<Player> { P_0.iVtfZWSuNqSTktUuYIhfVfoaoUko };
				IList<ElementAssignmentConflictInfo> list2 = ieyqiXjoKjYuEdjESIqmBdpJpSDM(P_0, P_1, list);
				int count = list2.Count;
				if (count == 0)
				{
					P_3 = "Swap was canceled because no conflicts were found.";
					return false;
				}
				if (count > 1)
				{
					P_3 = "Swap was canceled because more than one conflict was found.";
					return false;
				}
				if (P_2)
				{
					P_3 = "Swap was canceled due to a protected conflict that cannot be replaced.";
					return false;
				}
				if (P_0.EfbAlpjtXjKuMJGgIeMCqsQNXcoD.actionElementMapToReplace == null)
				{
					P_3 = "Swap was canceled because this is not a replacement assignment.";
					return false;
				}
				ElementAssignmentConflictInfo elementAssignmentConflictInfo = list2[0];
				if (!elementAssignmentConflictInfo.isConflict)
				{
					P_3 = "Swap was canceled because conflict was invalid.";
					return false;
				}
				ActionElementMap actionElementMap = new ActionElementMap(elementAssignmentConflictInfo.elementMap);
				if (actionElementMap == null)
				{
					P_3 = "Swap was canceled because conflict ActionElementMap was null.";
					return false;
				}
				ActionElementMap actionElementMap2 = new ActionElementMap(P_0.EfbAlpjtXjKuMJGgIeMCqsQNXcoD.actionElementMapToReplace);
				nBjtGOQFrMdvGjLndCbVDxwWJJrbA(P_0, P_1, list);
				int actionId = actionElementMap.actionId;
				Pole axisContribution = actionElementMap.axisContribution;
				bool invert = actionElementMap.invert;
				AxisRange axisRange = actionElementMap2.axisRange;
				ControllerElementType elementType = actionElementMap2.elementType;
				int elementIdentifierId = actionElementMap2.elementIdentifierId;
				KeyCode keyCode = actionElementMap2.keyCode;
				ModifierKeyFlags modifierKeyFlags = actionElementMap2.modifierKeyFlags;
				if (elementType == actionElementMap.elementType && elementType == ControllerElementType.Axis)
				{
					if (axisRange != actionElementMap.axisRange)
					{
						if (axisRange == AxisRange.Full)
						{
							axisRange = AxisRange.Positive;
						}
						else if (actionElementMap.axisRange != AxisRange.Full)
						{
						}
					}
				}
				else if (elementType == ControllerElementType.Axis && (actionElementMap.elementType == ControllerElementType.Button || (actionElementMap.elementType == ControllerElementType.Axis && actionElementMap.axisRange != AxisRange.Full)) && axisRange == AxisRange.Full)
				{
					axisRange = AxisRange.Positive;
				}
				if (elementType != ControllerElementType.Axis || axisRange != AxisRange.Full)
				{
					invert = false;
				}
				elementAssignmentConflictInfo.controllerMap.ReplaceOrCreateElementMap(ElementAssignment.CompleteAssignment(P_0.faBsnBPuHqXjbwbmDEdxSnMXkRro, elementType, elementIdentifierId, axisRange, keyCode, modifierKeyFlags, actionId, axisContribution, invert));
				P_3 = null;
				return true;
			}

			private static bool QUyCDFFMJsMTeqKYYkmskJjWNMxZ(ControllerElementType P_0, AxisRange P_1, Pole P_2, ControllerElementType P_3, AxisRange P_4, Pole P_5)
			{
				if ((P_0 == ControllerElementType.Button || (P_0 == ControllerElementType.Axis && P_1 != AxisRange.Full)) && (P_3 == ControllerElementType.Button || (P_3 == ControllerElementType.Axis && P_4 != AxisRange.Full)) && P_5 == P_2)
				{
					return true;
				}
				if (P_0 == ControllerElementType.Axis && P_1 == AxisRange.Full && P_3 == ControllerElementType.Axis && P_4 == AxisRange.Full)
				{
					return true;
				}
				return false;
			}

			private void SvoTHjSioRNxJPMeLbQhdnXpHZtC(ActionElementMap P_0)
			{
				aZkEgrNSVpPbetmJReBIjqHAmhIZA(P_0);
				XzuBPdQalzOHKURnLlGOfTHkteLo();
			}

			private void XrkgkhHGZrSbxbZCtJpKdJqUCLOhb(string P_0)
			{
				VYPnmTUIwJvSMwMFMSJmkDCqndBu(P_0);
				XzuBPdQalzOHKURnLlGOfTHkteLo();
			}

			private pWyGywFBBTKKLAlpHVtKfeBVlOYIc EKhbgVdGbotBaxXwgGrbRHXiUjGVA(ElementAssignment P_0)
			{
				if (wIdeAoQnynwsnVOLEhWzBGvYBXnEb(RUUjrZvkAxzsIWNwpNzEkhPKtVfn.ConflictsFound))
				{
					bool flag = SolGlxjkGMXZxEdRMDnmhSaysPfdb(iPlsoHhZZaKXtwdkeUadBAXXBwvd, P_0, gDUavrTmBkoEbXZVnFhpncBEGxMW);
					pyZEWZkoXmKsvmrEUixCDjFpsTLG = P_0;
					IList<ElementAssignmentConflictInfo> list = ieyqiXjoKjYuEdjESIqmBdpJpSDM(iPlsoHhZZaKXtwdkeUadBAXXBwvd, P_0, gDUavrTmBkoEbXZVnFhpncBEGxMW);
					xbdjVaeUuBmCzXMursJywYRXnHwgA = PDKWIIOgJETIuGFOekoXNceGHKQy.ConflictChecking;
					ThccWOAPPhMgXFhqFafSeoiFUcieE();
					aorwSedDRIzlSKGYxMPFsZaevYRF(new ElementAssignmentInfo(iPlsoHhZZaKXtwdkeUadBAXXBwvd.EfbAlpjtXjKuMJGgIeMCqsQNXcoD.controllerMap, P_0), list, flag);
					return pWyGywFBBTKKLAlpHVtKfeBVlOYIc.Quit;
				}
				return dycBrYeoHDeycXdeoetAHqdowXbB(ViEPLUptWijHECrAsMdPdcDIodfM.defaultActionWhenConflictFound, P_0);
			}

			private pWyGywFBBTKKLAlpHVtKfeBVlOYIc dycBrYeoHDeycXdeoetAHqdowXbB(ConflictResponse P_0, ElementAssignment P_1)
			{
				return PvHWkAquRLNUqtKiwmPzOQYbDMYh(P_0, P_1, SolGlxjkGMXZxEdRMDnmhSaysPfdb(iPlsoHhZZaKXtwdkeUadBAXXBwvd, P_1, gDUavrTmBkoEbXZVnFhpncBEGxMW));
			}

			private pWyGywFBBTKKLAlpHVtKfeBVlOYIc PvHWkAquRLNUqtKiwmPzOQYbDMYh(ConflictResponse P_0, ElementAssignment P_1, bool P_2)
			{
				switch (P_0)
				{
				case ConflictResponse.Cancel:
					XrkgkhHGZrSbxbZCtJpKdJqUCLOhb("Mapping assignment was canceled due to a conflict.");
					return pWyGywFBBTKKLAlpHVtKfeBVlOYIc.Quit;
				case ConflictResponse.Replace:
					if (P_2)
					{
						XrkgkhHGZrSbxbZCtJpKdJqUCLOhb("Mapping assignment was canceled due to a protected conflict that cannot be replaced.");
						return pWyGywFBBTKKLAlpHVtKfeBVlOYIc.Quit;
					}
					nBjtGOQFrMdvGjLndCbVDxwWJJrbA(iPlsoHhZZaKXtwdkeUadBAXXBwvd, P_1, gDUavrTmBkoEbXZVnFhpncBEGxMW);
					return pWyGywFBBTKKLAlpHVtKfeBVlOYIc.Continue;
				case ConflictResponse.Add:
					return pWyGywFBBTKKLAlpHVtKfeBVlOYIc.Continue;
				case ConflictResponse.Ignore:
					ObTBJhnmdbIEZcBBQuCiBGpmYYlU();
					return pWyGywFBBTKKLAlpHVtKfeBVlOYIc.Quit;
				case ConflictResponse.Swap:
				{
					if (!eEFwGOPdJozQkekuGYjWXrVrafNO(iPlsoHhZZaKXtwdkeUadBAXXBwvd, P_1, P_2, out var text))
					{
						XrkgkhHGZrSbxbZCtJpKdJqUCLOhb(text);
						return pWyGywFBBTKKLAlpHVtKfeBVlOYIc.Quit;
					}
					return pWyGywFBBTKKLAlpHVtKfeBVlOYIc.Continue;
				}
				default:
					throw new NotImplementedException();
				}
			}

			private void cZxNGmvreXXhsXrBttNFFpqBADMl()
			{
				lOFqeswSTOSXCxjhFIXgIrFvLVjf();
				XzuBPdQalzOHKURnLlGOfTHkteLo();
			}

			private void hSnjROgtWauzbSBQvFvjOnAYIxlS(string P_0)
			{
				oDaHMyJKhEeRcwVDNZCctjfoJucIA(P_0);
				XzuBPdQalzOHKURnLlGOfTHkteLo();
			}

			private void ThccWOAPPhMgXFhqFafSeoiFUcieE()
			{
				UMSWxCuoATCYCqdiceTccNMLdhJi();
				QvZTYgtaUuZfveCaedxXBLCseXNv();
				mpnPasITZqPgFbVepuLrhkdpdkliA = Status.AwaitingResponse;
			}

			private void ObTBJhnmdbIEZcBBQuCiBGpmYYlU()
			{
				mpnPasITZqPgFbVepuLrhkdpdkliA = Status.Listening;
				xbdjVaeUuBmCzXMursJywYRXnHwgA = PDKWIIOgJETIuGFOekoXNceGHKQy.None;
				SAEzcrPBAPCHICpWheAHVHYvMRww();
				IuFjSTHIhLjssRZxKFpahdNxqVFm();
			}

			private void bawArQArHvCiSndcdZDGMBAHbiTfA(ElementAssignment P_0)
			{
				if (iPlsoHhZZaKXtwdkeUadBAXXBwvd.EfbAlpjtXjKuMJGgIeMCqsQNXcoD.controllerMap.ReplaceOrCreateElementMap(P_0, out var result))
				{
					SvoTHjSioRNxJPMeLbQhdnXpHZtC(result);
				}
				else
				{
					hSnjROgtWauzbSBQvFvjOnAYIxlS("Failed to create element assignment.");
				}
			}

			private void aZkEgrNSVpPbetmJReBIjqHAmhIZA(ActionElementMap P_0)
			{
				if (wIdeAoQnynwsnVOLEhWzBGvYBXnEb(RUUjrZvkAxzsIWNwpNzEkhPKtVfn.InputMapped))
				{
					BsXhymSCxjKpXdwnJWSsfQVIjuin(RUUjrZvkAxzsIWNwpNzEkhPKtVfn.InputMapped, new InputMappedEventData(YRXLNHJZfcafMEOvgAYCGtZTwVvy, P_0));
				}
			}

			private void lOFqeswSTOSXCxjhFIXgIrFvLVjf()
			{
				if (wIdeAoQnynwsnVOLEhWzBGvYBXnEb(RUUjrZvkAxzsIWNwpNzEkhPKtVfn.TimedOut))
				{
					BsXhymSCxjKpXdwnJWSsfQVIjuin(RUUjrZvkAxzsIWNwpNzEkhPKtVfn.TimedOut, new TimedOutEventData(YRXLNHJZfcafMEOvgAYCGtZTwVvy));
				}
			}

			private void oDaHMyJKhEeRcwVDNZCctjfoJucIA(string P_0)
			{
				if (wIdeAoQnynwsnVOLEhWzBGvYBXnEb(RUUjrZvkAxzsIWNwpNzEkhPKtVfn.Error))
				{
					BsXhymSCxjKpXdwnJWSsfQVIjuin(RUUjrZvkAxzsIWNwpNzEkhPKtVfn.Error, new ErrorEventData(YRXLNHJZfcafMEOvgAYCGtZTwVvy, P_0));
				}
			}

			private void VYPnmTUIwJvSMwMFMSJmkDCqndBu(string P_0)
			{
				if (wIdeAoQnynwsnVOLEhWzBGvYBXnEb(RUUjrZvkAxzsIWNwpNzEkhPKtVfn.Canceled))
				{
					BsXhymSCxjKpXdwnJWSsfQVIjuin(RUUjrZvkAxzsIWNwpNzEkhPKtVfn.Canceled, new CanceledEventData(YRXLNHJZfcafMEOvgAYCGtZTwVvy, P_0));
				}
			}

			private void aorwSedDRIzlSKGYxMPFsZaevYRF(ElementAssignmentInfo P_0, IList<ElementAssignmentConflictInfo> P_1, bool P_2)
			{
				jhaWOmGFtfqtFfGXoiZjgHQJBYt jhaWOmGFtfqtFfGXoiZjgHQJBYt2 = new jhaWOmGFtfqtFfGXoiZjgHQJBYt();
				jhaWOmGFtfqtFfGXoiZjgHQJBYt2.fLOzdtiBnghsjKjuTLGuJIXczmTs = this;
				jhaWOmGFtfqtFfGXoiZjgHQJBYt2.nFWXUlWmIVHAUllIzDLbBXUuTxkMA = P_0;
				jhaWOmGFtfqtFfGXoiZjgHQJBYt2.UQjmyBQgIEsGvpXNHauHIJdkdUeeA = P_1;
				jhaWOmGFtfqtFfGXoiZjgHQJBYt2.ZQnAjdeufiHBwaXVFaTuIZreCVCs = P_2;
				if (wIdeAoQnynwsnVOLEhWzBGvYBXnEb(RUUjrZvkAxzsIWNwpNzEkhPKtVfn.ConflictsFound))
				{
					BsXhymSCxjKpXdwnJWSsfQVIjuin(RUUjrZvkAxzsIWNwpNzEkhPKtVfn.ConflictsFound, new ConflictFoundEventData(YRXLNHJZfcafMEOvgAYCGtZTwVvy, FxVPMVOeDRAHSozhauweSmKJPgoV, jhaWOmGFtfqtFfGXoiZjgHQJBYt2.nFWXUlWmIVHAUllIzDLbBXUuTxkMA, jhaWOmGFtfqtFfGXoiZjgHQJBYt2.UQjmyBQgIEsGvpXNHauHIJdkdUeeA, jhaWOmGFtfqtFfGXoiZjgHQJBYt2.ZQnAjdeufiHBwaXVFaTuIZreCVCs, jhaWOmGFtfqtFfGXoiZjgHQJBYt2.HTGOUVGrSmKFVsUIvqXaPZQoiSAT));
				}
			}

			private void BxgwIOUKHSmMQArXrkTDoDTHwrxj()
			{
				if (wIdeAoQnynwsnVOLEhWzBGvYBXnEb(RUUjrZvkAxzsIWNwpNzEkhPKtVfn.Started))
				{
					BsXhymSCxjKpXdwnJWSsfQVIjuin(RUUjrZvkAxzsIWNwpNzEkhPKtVfn.Started, new StartedEventData(YRXLNHJZfcafMEOvgAYCGtZTwVvy));
				}
			}

			private void zSNexwcxUticPvBEjSASMFmQGLOn()
			{
				if (wIdeAoQnynwsnVOLEhWzBGvYBXnEb(RUUjrZvkAxzsIWNwpNzEkhPKtVfn.Stopped))
				{
					BsXhymSCxjKpXdwnJWSsfQVIjuin(RUUjrZvkAxzsIWNwpNzEkhPKtVfn.Stopped, new StoppedEventData(YRXLNHJZfcafMEOvgAYCGtZTwVvy));
				}
			}

			public void FxVPMVOeDRAHSozhauweSmKJPgoV(ConflictResponse P_0)
			{
				if (mpnPasITZqPgFbVepuLrhkdpdkliA != Status.AwaitingResponse || xbdjVaeUuBmCzXMursJywYRXnHwgA != PDKWIIOgJETIuGFOekoXNceGHKQy.ConflictChecking)
				{
					Logger.LogWarning("The Mapping Listener was not waiting for a conflict checking response. The response will be ignored.");
					return;
				}
				try
				{
					if (dycBrYeoHDeycXdeoetAHqdowXbB(P_0, pyZEWZkoXmKsvmrEUixCDjFpsTLG) == pWyGywFBBTKKLAlpHVtKfeBVlOYIc.Continue)
					{
						bawArQArHvCiSndcdZDGMBAHbiTfA(pyZEWZkoXmKsvmrEUixCDjFpsTLG);
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
			private sealed class eNCbPKDcwoXkQZjKuPWQSNgBJvqp
			{
				public static readonly eNCbPKDcwoXkQZjKuPWQSNgBJvqp _003C_003E9 = new eNCbPKDcwoXkQZjKuPWQSNgBJvqp();

				public static Action<Exception> _003C_003E9__64_0;

				internal void aLrUZhnkOBdldRAHafAwhFaKnnlpA(Exception P_0)
				{
					ReInput.HandleCallbackException("InputMapper.Options.isElementAllowedCallback", P_0);
				}
			}

			private bool ayYMIKmCQGuFEdkkBbVGveXcOedN = true;

			private bool pHBROPbDbbWOqeqqKIidrPuWnUqp = true;

			private bool ZXgCHXooMShIrlMQmdAwDFpwKpaVA = true;

			private float cjFdXiuHwWegTLJXBFkDLuRDyCyA;

			private bool KkkIChbosdanowMjheZlyUsrcBZu = true;

			private bool SuBzkZZQHouWwChHlfSbYmWKCEhu = true;

			private bool HJKgObKcESqyfsyMdQaJdwQBojHbA = true;

			private bool riRftaKBNDaAwcaDdHGBCFxoKoFAA = true;

			private int[] YiKFXwlJtVRrIuUGCGUbQYqcQcME;

			private ConflictResponse qeNbISmEpsVAqiSycGvEqQVDSHN = ConflictResponse.Replace;

			private bool cpFWNeprLPOQwlkvCaNChPbmwszRA;

			private bool yGLWtrhCJTbrovWnpWshaqbzFREJA;

			private bool JEkWTVxqQpEJzZaETYtNMxaiIRys = true;

			private bool YcfGcTpbkHuJTeHKvkhXcJNinrOb = true;

			private float YpEvVvdefdbeoVhHWEMXUaLeYeYv = 1f;

			internal const string JOtvIDLShdjVvVzOFxzYcpBUOpZS = "isElementAllowed";

			private readonly Dictionary<string, SafeDelegate> HfKivbavYEsVuYkOLKYkQSDOriVQ = new Dictionary<string, SafeDelegate> { { "isElementAllowed", null } };

			public bool allowAxes
			{
				get
				{
					return ayYMIKmCQGuFEdkkBbVGveXcOedN;
				}
				set
				{
					ayYMIKmCQGuFEdkkBbVGveXcOedN = value;
				}
			}

			public bool allowButtons
			{
				get
				{
					return pHBROPbDbbWOqeqqKIidrPuWnUqp;
				}
				set
				{
					pHBROPbDbbWOqeqqKIidrPuWnUqp = value;
				}
			}

			public bool allowButtonsOnFullAxisAssignment
			{
				get
				{
					return ZXgCHXooMShIrlMQmdAwDFpwKpaVA;
				}
				set
				{
					ZXgCHXooMShIrlMQmdAwDFpwKpaVA = value;
				}
			}

			public float timeout
			{
				get
				{
					return cjFdXiuHwWegTLJXBFkDLuRDyCyA;
				}
				set
				{
					cjFdXiuHwWegTLJXBFkDLuRDyCyA = MathTools.Max(0f, value);
				}
			}

			public bool checkForConflicts
			{
				get
				{
					return KkkIChbosdanowMjheZlyUsrcBZu;
				}
				set
				{
					KkkIChbosdanowMjheZlyUsrcBZu = value;
				}
			}

			public bool checkForConflictsWithAllPlayers
			{
				get
				{
					return SuBzkZZQHouWwChHlfSbYmWKCEhu;
				}
				set
				{
					SuBzkZZQHouWwChHlfSbYmWKCEhu = value;
				}
			}

			public bool checkForConflictsWithSelf
			{
				get
				{
					return HJKgObKcESqyfsyMdQaJdwQBojHbA;
				}
				set
				{
					HJKgObKcESqyfsyMdQaJdwQBojHbA = value;
				}
			}

			public bool checkForConflictsWithSystemPlayer
			{
				get
				{
					return riRftaKBNDaAwcaDdHGBCFxoKoFAA;
				}
				set
				{
					riRftaKBNDaAwcaDdHGBCFxoKoFAA = value;
				}
			}

			public int[] checkForConflictsWithPlayerIds
			{
				get
				{
					return YiKFXwlJtVRrIuUGCGUbQYqcQcME;
				}
				set
				{
					YiKFXwlJtVRrIuUGCGUbQYqcQcME = value;
				}
			}

			public ConflictResponse defaultActionWhenConflictFound
			{
				get
				{
					return qeNbISmEpsVAqiSycGvEqQVDSHN;
				}
				set
				{
					qeNbISmEpsVAqiSycGvEqQVDSHN = value;
				}
			}

			public bool ignoreMouseXAxis
			{
				get
				{
					return cpFWNeprLPOQwlkvCaNChPbmwszRA;
				}
				set
				{
					cpFWNeprLPOQwlkvCaNChPbmwszRA = value;
				}
			}

			public bool ignoreMouseYAxis
			{
				get
				{
					return yGLWtrhCJTbrovWnpWshaqbzFREJA;
				}
				set
				{
					yGLWtrhCJTbrovWnpWshaqbzFREJA = value;
				}
			}

			public bool allowKeyboardKeysWithModifiers
			{
				get
				{
					return JEkWTVxqQpEJzZaETYtNMxaiIRys;
				}
				set
				{
					JEkWTVxqQpEJzZaETYtNMxaiIRys = value;
				}
			}

			public bool allowKeyboardModifierKeyAsPrimary
			{
				get
				{
					return YcfGcTpbkHuJTeHKvkhXcJNinrOb;
				}
				set
				{
					YcfGcTpbkHuJTeHKvkhXcJNinrOb = value;
				}
			}

			public float holdDurationToMapKeyboardModifierKeyAsPrimary
			{
				get
				{
					return YpEvVvdefdbeoVhHWEMXUaLeYeYv;
				}
				set
				{
					YpEvVvdefdbeoVhHWEMXUaLeYeYv = MathTools.Max(0f, value);
				}
			}

			public Predicate<ControllerPollingInfo> isElementAllowedCallback
			{
				get
				{
					return (SafePredicate<ControllerPollingInfo>)HfKivbavYEsVuYkOLKYkQSDOriVQ["isElementAllowed"];
				}
				set
				{
					SafePredicate<ControllerPollingInfo> safePredicate = value;
					if (safePredicate != null)
					{
						safePredicate.ExceptionHandler = eNCbPKDcwoXkQZjKuPWQSNgBJvqp._003C_003E9.aLrUZhnkOBdldRAHafAwhFaKnnlpA;
					}
					HfKivbavYEsVuYkOLKYkQSDOriVQ["isElementAllowed"] = safePredicate;
				}
			}

			internal _0001 uJXJEaNGufdhHihztzbIATgypdhqA<_0001>(string P_0) where _0001 : SafeDelegate
			{
				if (!HfKivbavYEsVuYkOLKYkQSDOriVQ.TryGetValue(P_0, out var value))
				{
					return null;
				}
				return value as _0001;
			}

			public Options()
			{
				nCIsEurlPuGVJRLEodcdEVbpmLgcA();
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
				stringBuilder.Append("allowAxes = " + ayYMIKmCQGuFEdkkBbVGveXcOedN + "\n");
				stringBuilder.Append("allowButtons = " + pHBROPbDbbWOqeqqKIidrPuWnUqp + "\n");
				stringBuilder.Append("allowButtonsOnFullAxisAssignment = " + ZXgCHXooMShIrlMQmdAwDFpwKpaVA + "\n");
				stringBuilder.Append("timeout = " + cjFdXiuHwWegTLJXBFkDLuRDyCyA + "\n");
				stringBuilder.Append("checkForConflicts = " + KkkIChbosdanowMjheZlyUsrcBZu + "\n");
				stringBuilder.Append("checkForConflictsWithAllPlayers = " + SuBzkZZQHouWwChHlfSbYmWKCEhu + "\n");
				stringBuilder.Append("checkForConflictsWithSelf = " + HJKgObKcESqyfsyMdQaJdwQBojHbA + "\n");
				stringBuilder.Append("checkForConflictsWithSystemPlayer = " + riRftaKBNDaAwcaDdHGBCFxoKoFAA + "\n");
				if (YiKFXwlJtVRrIuUGCGUbQYqcQcME == null)
				{
					stringBuilder.Append("_checkForConflictsWithPlayerIds = null\n");
				}
				else
				{
					stringBuilder.Append("_checkForConflictsWithPlayerIds = " + StringTools.ToString(YiKFXwlJtVRrIuUGCGUbQYqcQcME) + "\n");
				}
				stringBuilder.Append("defaultActionWhenConflictFound = " + qeNbISmEpsVAqiSycGvEqQVDSHN.ToString() + "\n");
				stringBuilder.Append("ignoreMouseXAxis = " + cpFWNeprLPOQwlkvCaNChPbmwszRA);
				stringBuilder.Append("ignoreMouseYAxis = " + yGLWtrhCJTbrovWnpWshaqbzFREJA);
				stringBuilder.Append("allowKeyboardKeysWithModifiers = " + JEkWTVxqQpEJzZaETYtNMxaiIRys + "\n");
				stringBuilder.Append("allowKeyboardModifierAsPrimary = " + YcfGcTpbkHuJTeHKvkhXcJNinrOb + "\n");
				stringBuilder.Append("holdDurationToMapKeyboardModifierKeyAsPrimary = " + YpEvVvdefdbeoVhHWEMXUaLeYeYv + "\n");
				return stringBuilder.ToString();
			}

			internal void nCIsEurlPuGVJRLEodcdEVbpmLgcA()
			{
				ayYMIKmCQGuFEdkkBbVGveXcOedN = true;
				pHBROPbDbbWOqeqqKIidrPuWnUqp = true;
				ZXgCHXooMShIrlMQmdAwDFpwKpaVA = true;
				cjFdXiuHwWegTLJXBFkDLuRDyCyA = 0f;
				KkkIChbosdanowMjheZlyUsrcBZu = true;
				SuBzkZZQHouWwChHlfSbYmWKCEhu = true;
				HJKgObKcESqyfsyMdQaJdwQBojHbA = true;
				riRftaKBNDaAwcaDdHGBCFxoKoFAA = true;
				YiKFXwlJtVRrIuUGCGUbQYqcQcME = null;
				qeNbISmEpsVAqiSycGvEqQVDSHN = ConflictResponse.Replace;
				cpFWNeprLPOQwlkvCaNChPbmwszRA = false;
				yGLWtrhCJTbrovWnpWshaqbzFREJA = false;
				JEkWTVxqQpEJzZaETYtNMxaiIRys = true;
				YcfGcTpbkHuJTeHKvkhXcJNinrOb = true;
				YpEvVvdefdbeoVhHWEMXUaLeYeYv = 1f;
				foreach (string item in new List<string>(HfKivbavYEsVuYkOLKYkQSDOriVQ.Keys))
				{
					HfKivbavYEsVuYkOLKYkQSDOriVQ[item] = null;
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
				destination.ayYMIKmCQGuFEdkkBbVGveXcOedN = source.ayYMIKmCQGuFEdkkBbVGveXcOedN;
				destination.pHBROPbDbbWOqeqqKIidrPuWnUqp = source.pHBROPbDbbWOqeqqKIidrPuWnUqp;
				destination.ZXgCHXooMShIrlMQmdAwDFpwKpaVA = source.ZXgCHXooMShIrlMQmdAwDFpwKpaVA;
				destination.cjFdXiuHwWegTLJXBFkDLuRDyCyA = source.cjFdXiuHwWegTLJXBFkDLuRDyCyA;
				destination.KkkIChbosdanowMjheZlyUsrcBZu = source.KkkIChbosdanowMjheZlyUsrcBZu;
				destination.SuBzkZZQHouWwChHlfSbYmWKCEhu = source.SuBzkZZQHouWwChHlfSbYmWKCEhu;
				destination.HJKgObKcESqyfsyMdQaJdwQBojHbA = source.HJKgObKcESqyfsyMdQaJdwQBojHbA;
				destination.riRftaKBNDaAwcaDdHGBCFxoKoFAA = source.riRftaKBNDaAwcaDdHGBCFxoKoFAA;
				destination.YiKFXwlJtVRrIuUGCGUbQYqcQcME = ArrayTools.ShallowCopy(source.YiKFXwlJtVRrIuUGCGUbQYqcQcME);
				destination.qeNbISmEpsVAqiSycGvEqQVDSHN = source.qeNbISmEpsVAqiSycGvEqQVDSHN;
				destination.cpFWNeprLPOQwlkvCaNChPbmwszRA = source.cpFWNeprLPOQwlkvCaNChPbmwszRA;
				destination.yGLWtrhCJTbrovWnpWshaqbzFREJA = source.yGLWtrhCJTbrovWnpWshaqbzFREJA;
				destination.JEkWTVxqQpEJzZaETYtNMxaiIRys = source.JEkWTVxqQpEJzZaETYtNMxaiIRys;
				destination.YcfGcTpbkHuJTeHKvkhXcJNinrOb = source.YcfGcTpbkHuJTeHKvkhXcJNinrOb;
				destination.YpEvVvdefdbeoVhHWEMXUaLeYeYv = source.YpEvVvdefdbeoVhHWEMXUaLeYeYv;
				foreach (KeyValuePair<string, SafeDelegate> item in source.HfKivbavYEsVuYkOLKYkQSDOriVQ)
				{
					destination.HfKivbavYEsVuYkOLKYkQSDOriVQ[item.Key] = MiscTools.Clone(item.Value);
				}
			}
		}

		[Serializable]
		private sealed class ctadaNIsmGaHsMeBLrWjfjncRjrHB
		{
			public static readonly ctadaNIsmGaHsMeBLrWjfjncRjrHB _003C_003E9 = new ctadaNIsmGaHsMeBLrWjfjncRjrHB();

			public static Action<Exception> _003C_003E9__54_0;

			public static Action<Exception> _003C_003E9__54_1;

			public static Action<Exception> _003C_003E9__54_2;

			public static Action<Exception> _003C_003E9__54_3;

			public static Action<Exception> _003C_003E9__54_4;

			public static Action<Exception> _003C_003E9__54_5;

			public static Action<Exception> _003C_003E9__54_6;

			internal void SDcCiNOztzrZWLCtSShDihbgmGKn(Exception P_0)
			{
				ReInput.HandleCallbackException("InputMapper.AssignedEvent", P_0);
			}

			internal void dkgXwFWUYPjvpbdDOAgEjEsMmgFBb(Exception P_0)
			{
				ReInput.HandleCallbackException("InputMapper.ErrorEvent", P_0);
			}

			internal void XZpPVmewQhnXiqaliVWRltMVjQER(Exception P_0)
			{
				ReInput.HandleCallbackException("InputMapper.CanceledEvent", P_0);
			}

			internal void adbBgQMmTAxRCwmzVrHpxbBafNVGA(Exception P_0)
			{
				ReInput.HandleCallbackException("InputMapper.TimedOutEvent", P_0);
			}

			internal void QWUrZUZZYDBFTrNtSkMOfeTAdJCE(Exception P_0)
			{
				ReInput.HandleCallbackException("InputMapper.StartedEvent", P_0);
			}

			internal void HyRkeyQCUrLOlSysMUDzYeFdtkUq(Exception P_0)
			{
				ReInput.HandleCallbackException("InputMapper.StoppedEvent", P_0);
			}

			internal void uXJvHpodeEhEnkaAToiNnaFVMiyeb(Exception P_0)
			{
				ReInput.HandleCallbackException("InputMapper.ConflictFoundEvent", P_0);
			}
		}

		private static InputMapper zcaOfnopWUbgQHWlNqeUUkpDRZS;

		private static int LDcEpLhszRLEMISPekPgpAVytMGfB;

		private readonly int ujxJAhKrNoLWsRbjNEciDlFBFIoxA;

		private readonly bool ysXVpKULZYRkvDvboQYEPmmsMHoV;

		private readonly iUSpOGpilluOVBqbohoiADoLvdIpA ADZtutNakkyIiDqLUKfidIbdIvMf;

		private Options YYsgWueqrmHHHGREdwdTwGNmwtTFA;

		private readonly Dictionary<RUUjrZvkAxzsIWNwpNzEkhPKtVfn, SafeDelegate> rUSFFvvynTtSbbOYfpBNPiPuRbNx = new Dictionary<RUUjrZvkAxzsIWNwpNzEkhPKtVfn, SafeDelegate>
		{
			{
				RUUjrZvkAxzsIWNwpNzEkhPKtVfn.InputMapped,
				new SafeAction<InputMappedEventData>(ctadaNIsmGaHsMeBLrWjfjncRjrHB._003C_003E9.SDcCiNOztzrZWLCtSShDihbgmGKn)
			},
			{
				RUUjrZvkAxzsIWNwpNzEkhPKtVfn.Error,
				new SafeAction<ErrorEventData>(ctadaNIsmGaHsMeBLrWjfjncRjrHB._003C_003E9.dkgXwFWUYPjvpbdDOAgEjEsMmgFBb)
			},
			{
				RUUjrZvkAxzsIWNwpNzEkhPKtVfn.Canceled,
				new SafeAction<CanceledEventData>(ctadaNIsmGaHsMeBLrWjfjncRjrHB._003C_003E9.XZpPVmewQhnXiqaliVWRltMVjQER)
			},
			{
				RUUjrZvkAxzsIWNwpNzEkhPKtVfn.TimedOut,
				new SafeAction<TimedOutEventData>(ctadaNIsmGaHsMeBLrWjfjncRjrHB._003C_003E9.adbBgQMmTAxRCwmzVrHpxbBafNVGA)
			},
			{
				RUUjrZvkAxzsIWNwpNzEkhPKtVfn.Started,
				new SafeAction<StartedEventData>(ctadaNIsmGaHsMeBLrWjfjncRjrHB._003C_003E9.QWUrZUZZYDBFTrNtSkMOfeTAdJCE)
			},
			{
				RUUjrZvkAxzsIWNwpNzEkhPKtVfn.Stopped,
				new SafeAction<StoppedEventData>(ctadaNIsmGaHsMeBLrWjfjncRjrHB._003C_003E9.HyRkeyQCUrLOlSysMUDzYeFdtkUq)
			},
			{
				RUUjrZvkAxzsIWNwpNzEkhPKtVfn.ConflictsFound,
				new SafeAction<ConflictFoundEventData>(ctadaNIsmGaHsMeBLrWjfjncRjrHB._003C_003E9.uXJvHpodeEhEnkaAToiNnaFVMiyeb)
			}
		};

		public static InputMapper Default => zcaOfnopWUbgQHWlNqeUUkpDRZS ?? (zcaOfnopWUbgQHWlNqeUUkpDRZS = new InputMapper(true));

		public Options options
		{
			get
			{
				Options obj = YYsgWueqrmHHHGREdwdTwGNmwtTFA;
				if (obj == null)
				{
					if (!ysXVpKULZYRkvDvboQYEPmmsMHoV)
					{
						return YYsgWueqrmHHHGREdwdTwGNmwtTFA = Default.options.Clone();
					}
					obj = (YYsgWueqrmHHHGREdwdTwGNmwtTFA = new Options());
				}
				return obj;
			}
			set
			{
				YYsgWueqrmHHHGREdwdTwGNmwtTFA = value;
			}
		}

		public Context mappingContext => ADZtutNakkyIiDqLUKfidIbdIvMf.xzqtlFewlXbyivRbqfwQRtheTGbP;

		public Status status => ADZtutNakkyIiDqLUKfidIbdIvMf.HYgTVIOcNoBWqoALLiYCisveLmbF;

		public float timeRemaining => ADZtutNakkyIiDqLUKfidIbdIvMf.IxsqmqnGHpasWIMGHaYqQpSwUtDf;

		internal int DkTMpQhiElAJSEDOeDtUGGIFJZGkB => ujxJAhKrNoLWsRbjNEciDlFBFIoxA;

		public event Action<InputMappedEventData> InputMappedEvent
		{
			add
			{
				if (value != null)
				{
					RUUjrZvkAxzsIWNwpNzEkhPKtVfn key = RUUjrZvkAxzsIWNwpNzEkhPKtVfn.InputMapped;
					rUSFFvvynTtSbbOYfpBNPiPuRbNx[key] = (SafeAction<InputMappedEventData>)rUSFFvvynTtSbbOYfpBNPiPuRbNx[key] + value;
				}
			}
			remove
			{
				if (value != null)
				{
					RUUjrZvkAxzsIWNwpNzEkhPKtVfn key = RUUjrZvkAxzsIWNwpNzEkhPKtVfn.InputMapped;
					rUSFFvvynTtSbbOYfpBNPiPuRbNx[key] = (SafeAction<InputMappedEventData>)rUSFFvvynTtSbbOYfpBNPiPuRbNx[key] - value;
				}
			}
		}

		public event Action<ErrorEventData> ErrorEvent
		{
			add
			{
				if (value != null)
				{
					RUUjrZvkAxzsIWNwpNzEkhPKtVfn key = RUUjrZvkAxzsIWNwpNzEkhPKtVfn.Error;
					rUSFFvvynTtSbbOYfpBNPiPuRbNx[key] = (SafeAction<ErrorEventData>)rUSFFvvynTtSbbOYfpBNPiPuRbNx[key] + value;
				}
			}
			remove
			{
				if (value != null)
				{
					RUUjrZvkAxzsIWNwpNzEkhPKtVfn key = RUUjrZvkAxzsIWNwpNzEkhPKtVfn.Error;
					rUSFFvvynTtSbbOYfpBNPiPuRbNx[key] = (SafeAction<ErrorEventData>)rUSFFvvynTtSbbOYfpBNPiPuRbNx[key] - value;
				}
			}
		}

		public event Action<CanceledEventData> CanceledEvent
		{
			add
			{
				if (value != null)
				{
					RUUjrZvkAxzsIWNwpNzEkhPKtVfn key = RUUjrZvkAxzsIWNwpNzEkhPKtVfn.Canceled;
					rUSFFvvynTtSbbOYfpBNPiPuRbNx[key] = (SafeAction<CanceledEventData>)rUSFFvvynTtSbbOYfpBNPiPuRbNx[key] + value;
				}
			}
			remove
			{
				if (value != null)
				{
					RUUjrZvkAxzsIWNwpNzEkhPKtVfn key = RUUjrZvkAxzsIWNwpNzEkhPKtVfn.Canceled;
					rUSFFvvynTtSbbOYfpBNPiPuRbNx[key] = (SafeAction<CanceledEventData>)rUSFFvvynTtSbbOYfpBNPiPuRbNx[key] - value;
				}
			}
		}

		public event Action<TimedOutEventData> TimedOutEvent
		{
			add
			{
				if (value != null)
				{
					RUUjrZvkAxzsIWNwpNzEkhPKtVfn key = RUUjrZvkAxzsIWNwpNzEkhPKtVfn.TimedOut;
					rUSFFvvynTtSbbOYfpBNPiPuRbNx[key] = (SafeAction<TimedOutEventData>)rUSFFvvynTtSbbOYfpBNPiPuRbNx[key] + value;
				}
			}
			remove
			{
				if (value != null)
				{
					RUUjrZvkAxzsIWNwpNzEkhPKtVfn key = RUUjrZvkAxzsIWNwpNzEkhPKtVfn.TimedOut;
					rUSFFvvynTtSbbOYfpBNPiPuRbNx[key] = (SafeAction<TimedOutEventData>)rUSFFvvynTtSbbOYfpBNPiPuRbNx[key] - value;
				}
			}
		}

		public event Action<StartedEventData> StartedEvent
		{
			add
			{
				if (value != null)
				{
					RUUjrZvkAxzsIWNwpNzEkhPKtVfn key = RUUjrZvkAxzsIWNwpNzEkhPKtVfn.Started;
					rUSFFvvynTtSbbOYfpBNPiPuRbNx[key] = (SafeAction<StartedEventData>)rUSFFvvynTtSbbOYfpBNPiPuRbNx[key] + value;
				}
			}
			remove
			{
				if (value != null)
				{
					RUUjrZvkAxzsIWNwpNzEkhPKtVfn key = RUUjrZvkAxzsIWNwpNzEkhPKtVfn.Started;
					rUSFFvvynTtSbbOYfpBNPiPuRbNx[key] = (SafeAction<StartedEventData>)rUSFFvvynTtSbbOYfpBNPiPuRbNx[key] - value;
				}
			}
		}

		public event Action<StoppedEventData> StoppedEvent
		{
			add
			{
				if (value != null)
				{
					RUUjrZvkAxzsIWNwpNzEkhPKtVfn key = RUUjrZvkAxzsIWNwpNzEkhPKtVfn.Stopped;
					rUSFFvvynTtSbbOYfpBNPiPuRbNx[key] = (SafeAction<StoppedEventData>)rUSFFvvynTtSbbOYfpBNPiPuRbNx[key] + value;
				}
			}
			remove
			{
				if (value != null)
				{
					RUUjrZvkAxzsIWNwpNzEkhPKtVfn key = RUUjrZvkAxzsIWNwpNzEkhPKtVfn.Stopped;
					rUSFFvvynTtSbbOYfpBNPiPuRbNx[key] = (SafeAction<StoppedEventData>)rUSFFvvynTtSbbOYfpBNPiPuRbNx[key] - value;
				}
			}
		}

		public event Action<ConflictFoundEventData> ConflictFoundEvent
		{
			add
			{
				if (value != null)
				{
					RUUjrZvkAxzsIWNwpNzEkhPKtVfn key = RUUjrZvkAxzsIWNwpNzEkhPKtVfn.ConflictsFound;
					rUSFFvvynTtSbbOYfpBNPiPuRbNx[key] = (SafeAction<ConflictFoundEventData>)rUSFFvvynTtSbbOYfpBNPiPuRbNx[key] + value;
				}
			}
			remove
			{
				if (value != null)
				{
					RUUjrZvkAxzsIWNwpNzEkhPKtVfn key = RUUjrZvkAxzsIWNwpNzEkhPKtVfn.ConflictsFound;
					rUSFFvvynTtSbbOYfpBNPiPuRbNx[key] = (SafeAction<ConflictFoundEventData>)rUSFFvvynTtSbbOYfpBNPiPuRbNx[key] - value;
				}
			}
		}

		private static int yfsnuwUkwjmsSaUtumEcifgucOad()
		{
			int lDcEpLhszRLEMISPekPgpAVytMGfB = LDcEpLhszRLEMISPekPgpAVytMGfB;
			if (LDcEpLhszRLEMISPekPgpAVytMGfB == 2147483647)
			{
				LDcEpLhszRLEMISPekPgpAVytMGfB = 0;
				return lDcEpLhszRLEMISPekPgpAVytMGfB;
			}
			LDcEpLhszRLEMISPekPgpAVytMGfB++;
			return lDcEpLhszRLEMISPekPgpAVytMGfB;
		}

		public InputMapper()
			: this(false)
		{
			ujxJAhKrNoLWsRbjNEciDlFBFIoxA = yfsnuwUkwjmsSaUtumEcifgucOad();
		}

		private InputMapper(bool P_0)
		{
			ysXVpKULZYRkvDvboQYEPmmsMHoV = P_0;
			if (ysXVpKULZYRkvDvboQYEPmmsMHoV)
			{
				YYsgWueqrmHHHGREdwdTwGNmwtTFA = new Options();
			}
			ADZtutNakkyIiDqLUKfidIbdIvMf = new iUSpOGpilluOVBqbohoiADoLvdIpA(this, rUSFFvvynTtSbbOYfpBNPiPuRbNx);
		}

		public void RemoveEventListeners(object listenerOrParent)
		{
			if (listenerOrParent == null)
			{
				return;
			}
			foreach (KeyValuePair<RUUjrZvkAxzsIWNwpNzEkhPKtVfn, SafeDelegate> item in rUSFFvvynTtSbbOYfpBNPiPuRbNx)
			{
				item.Value.RemoveDelegateOrAllDelegatesFromAnObject(listenerOrParent);
			}
		}

		public void RemoveAllEventListeners()
		{
			foreach (KeyValuePair<RUUjrZvkAxzsIWNwpNzEkhPKtVfn, SafeDelegate> item in rUSFFvvynTtSbbOYfpBNPiPuRbNx)
			{
				item.Value.Clear();
			}
		}

		internal void dQYjsgETVhudUobmjWNBbCxxjSKN(object P_0)
		{
		}

		internal void gthWANUBobmcdOIUlQmZHLwerHJd()
		{
		}

		public bool Start(Context mappingContext)
		{
			return CUFoqRPOoGOaPwxbksYAgUzQpzPT(mappingContext, (YYsgWueqrmHHHGREdwdTwGNmwtTFA != null) ? YYsgWueqrmHHHGREdwdTwGNmwtTFA : Default.options);
		}

		public void Stop()
		{
			ADZtutNakkyIiDqLUKfidIbdIvMf.SIpWgzfzvCMNBnysysnMRJgfLzZF("User canceled.");
		}

		public void Clear()
		{
			Stop();
			RemoveAllEventListeners();
			gthWANUBobmcdOIUlQmZHLwerHJd();
			YYsgWueqrmHHHGREdwdTwGNmwtTFA = null;
		}

		private bool CUFoqRPOoGOaPwxbksYAgUzQpzPT(Context P_0, Options P_1)
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
				ADZtutNakkyIiDqLUKfidIbdIvMf.nduqsKumnOSsbdwmQvNbJGFikdPB(P_0, P_1);
				return true;
			}
			catch
			{
				ADZtutNakkyIiDqLUKfidIbdIvMf.SIpWgzfzvCMNBnysysnMRJgfLzZF("Failed to start due to an exception.");
				return false;
			}
		}
	}
}
