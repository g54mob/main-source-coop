using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;
using UnityEngine;

namespace Rewired
{
	public class PlayerController : IPlayerController
	{
		public class Definition
		{
			public bool enabled = true;

			public int playerId = -1;

			public ICollection<Element.Definition> elements;
		}

		public static class Factory
		{
			public static PlayerController Create(Definition definition)
			{
				return new PlayerController(definition);
			}
		}

		public enum AbsoluteToRelativeScalingMode
		{
			None = 0,
			ScreenWidth = 1,
			ScreenHeight = 2,
			MaxScreenDimension = 3,
			MinScreenDimension = 4,
			ViewportWidth = 5,
			ViewportHeight = 6,
			MaxViewportDimension = 7,
			MinViewportDimension = 8
		}

		public class Axis : ElementWithSource
		{
			public new class Definition : ElementWithSource.Definition
			{
				public AxisCoordinateMode coordinateMode;

				public float absoluteToRelativeSensitivity;

				public AbsoluteToRelativeScalingMode absoluteToRelativeScalingMode;

				public Definition()
				{
					coordinateMode = AxisCoordinateMode.Absolute;
					absoluteToRelativeSensitivity = 1f;
					absoluteToRelativeScalingMode = AbsoluteToRelativeScalingMode.None;
				}

				internal virtual Element ofxnkmHDDZPZVCNuSfcKaxrkBbRn(PlayerController P_0)
				{
					return new Axis(P_0, this);
				}
			}

			internal const float mHOMGjqrJEvBmUDOWgudnxVWMJpS = 1f;

			internal const AbsoluteToRelativeScalingMode lQwlelTpQjXYnmgsqEJZdfvyCNZG = AbsoluteToRelativeScalingMode.None;

			[CustomObfuscation(rename = false)]
			internal const AxisCoordinateMode defaultAxisCoordinateMode = AxisCoordinateMode.Absolute;

			private float KyAAspbJBaTLxhnoCNPdavaDPSzeA = 1f;

			private AxisCoordinateMode tpkfaQpqeyOALLDnsZLpQEFTzDlu;

			private AbsoluteToRelativeScalingMode KytluhADWHMxzpYPdjsRHEhtyCib;

			public float absoluteToRelativeSensitivity
			{
				get
				{
					return KyAAspbJBaTLxhnoCNPdavaDPSzeA;
				}
				set
				{
					if (value < 0f)
					{
						value = 0f;
					}
					KyAAspbJBaTLxhnoCNPdavaDPSzeA = value;
				}
			}

			public AbsoluteToRelativeScalingMode absoluteToRelativeScalingMode
			{
				get
				{
					return KytluhADWHMxzpYPdjsRHEhtyCib;
				}
				set
				{
					KytluhADWHMxzpYPdjsRHEhtyCib = value;
				}
			}

			public AxisCoordinateMode coordinateMode => tpkfaQpqeyOALLDnsZLpQEFTzDlu;

			public virtual float value
			{
				get
				{
					if (!base.selfAndParentEnabled || base.player == null)
					{
						return 0f;
					}
					float num = base.player.GetAxis(base.actionId);
					switch (base.player.GetAxisCoordinateMode(base.actionId))
					{
					case AxisCoordinateMode.Relative:
						if (tpkfaQpqeyOALLDnsZLpQEFTzDlu == AxisCoordinateMode.Absolute)
						{
							return 0f;
						}
						break;
					case AxisCoordinateMode.Absolute:
						if (tpkfaQpqeyOALLDnsZLpQEFTzDlu == AxisCoordinateMode.Relative)
						{
							switch (KytluhADWHMxzpYPdjsRHEhtyCib)
							{
							case AbsoluteToRelativeScalingMode.ScreenHeight:
								num *= (float)Screen.currentResolution.height / absoluteToRelativeScalingReferenceResolution.y;
								break;
							case AbsoluteToRelativeScalingMode.ScreenWidth:
								num *= (float)Screen.currentResolution.width / absoluteToRelativeScalingReferenceResolution.x;
								break;
							case AbsoluteToRelativeScalingMode.ViewportHeight:
								num *= (float)Screen.height / absoluteToRelativeScalingReferenceResolution.y;
								break;
							case AbsoluteToRelativeScalingMode.ViewportWidth:
								num *= (float)Screen.width / absoluteToRelativeScalingReferenceResolution.x;
								break;
							case AbsoluteToRelativeScalingMode.MaxScreenDimension:
								num = ((Screen.currentResolution.width < Screen.currentResolution.height) ? (num * ((float)Screen.currentResolution.height / absoluteToRelativeScalingReferenceResolution.y)) : (num * ((float)Screen.currentResolution.width / absoluteToRelativeScalingReferenceResolution.x)));
								break;
							case AbsoluteToRelativeScalingMode.MinScreenDimension:
								num = ((Screen.currentResolution.width > Screen.currentResolution.height) ? (num * ((float)Screen.currentResolution.height / absoluteToRelativeScalingReferenceResolution.y)) : (num * ((float)Screen.currentResolution.width / absoluteToRelativeScalingReferenceResolution.x)));
								break;
							case AbsoluteToRelativeScalingMode.MaxViewportDimension:
								num = ((Screen.width < Screen.height) ? (num * ((float)Screen.height / absoluteToRelativeScalingReferenceResolution.y)) : (num * ((float)Screen.width / absoluteToRelativeScalingReferenceResolution.x)));
								break;
							case AbsoluteToRelativeScalingMode.MinViewportDimension:
								num = ((Screen.width > Screen.height) ? (num * ((float)Screen.height / absoluteToRelativeScalingReferenceResolution.y)) : (num * ((float)Screen.width / absoluteToRelativeScalingReferenceResolution.x)));
								break;
							}
							num *= (float)ReInput.unscaledDeltaTime * KyAAspbJBaTLxhnoCNPdavaDPSzeA;
						}
						break;
					}
					return num;
				}
			}

			public virtual float valueRaw
			{
				get
				{
					if (!base.selfAndParentEnabled || base.player == null)
					{
						return 0f;
					}
					return base.player.GetAxisRaw(base.actionId);
				}
			}

			internal Axis(PlayerController P_0, Definition P_1)
				: base(P_0, P_1)
			{
				KyAAspbJBaTLxhnoCNPdavaDPSzeA = P_1.absoluteToRelativeSensitivity;
				tpkfaQpqeyOALLDnsZLpQEFTzDlu = P_1.coordinateMode;
				KytluhADWHMxzpYPdjsRHEhtyCib = P_1.absoluteToRelativeScalingMode;
			}
		}

		public class MouseAxis : Axis
		{
			public new class Definition : Axis.Definition
			{
				public Definition()
				{
					coordinateMode = AxisCoordinateMode.Relative;
					absoluteToRelativeSensitivity = 600f;
					absoluteToRelativeScalingMode = AbsoluteToRelativeScalingMode.ScreenWidth;
				}

				internal virtual Element ofYicVHXqZXZYgqKOLmQzkAvAmSb(PlayerController P_0)
				{
					return new MouseAxis(P_0, this);
				}
			}

			[CustomObfuscation(rename = false)]
			internal new const AxisCoordinateMode defaultAxisCoordinateMode = AxisCoordinateMode.Relative;

			[CustomObfuscation(rename = false)]
			internal const float defaultAbsoluteToRelativeSensitivity = 600f;

			[CustomObfuscation(rename = false)]
			internal const AbsoluteToRelativeScalingMode defaultAbsoluteToRelativeScalingMode = AbsoluteToRelativeScalingMode.ScreenWidth;

			internal MouseAxis(PlayerController P_0, Definition P_1)
				: base(P_0, P_1)
			{
			}
		}

		public class Axis2D : CompoundElement
		{
			public new class Definition : CompoundElement.Definition
			{
				private Axis.Definition EcogQyyQiXXRQMKLufSaHTKXPzZi;

				private Axis.Definition fRvZqEAAjylmLulGhXdDVJWRLUcl;

				public Axis.Definition xAxis
				{
					get
					{
						return EcogQyyQiXXRQMKLufSaHTKXPzZi;
					}
					set
					{
						EcogQyyQiXXRQMKLufSaHTKXPzZi = value;
					}
				}

				public Axis.Definition yAxis
				{
					get
					{
						return fRvZqEAAjylmLulGhXdDVJWRLUcl;
					}
					set
					{
						fRvZqEAAjylmLulGhXdDVJWRLUcl = value;
					}
				}

				internal virtual Element iKuOrOKdNKVWylBNdrPyQsyhkffy(PlayerController P_0)
				{
					return new Axis2D(P_0, this);
				}
			}

			internal const int lFZGzQzmXYpiRqtoVZCDxKUTYTxI = 0;

			internal const int iTFnybujNwLrfWWhqORICoEhBkhw = 1;

			internal const int txqMhvlFHCmoLolAezUJEcyxYUWt = 2;

			public Axis xAxis => AfsieBjIjldjgyYsyYijIuXtmLSc<Axis>(0);

			public Axis yAxis => AfsieBjIjldjgyYsyYijIuXtmLSc<Axis>(1);

			public virtual Vector2 value => new Vector2(AfsieBjIjldjgyYsyYijIuXtmLSc<Axis>(0).value, AfsieBjIjldjgyYsyYijIuXtmLSc<Axis>(1).value);

			public virtual Vector2 valueRaw => new Vector2(AfsieBjIjldjgyYsyYijIuXtmLSc<Axis>(0).valueRaw, AfsieBjIjldjgyYsyYijIuXtmLSc<Axis>(1).valueRaw);

			internal Axis2D(PlayerController P_0, Definition P_1, Element.Definition[] P_2)
				: base(P_0, P_1, P_2)
			{
			}

			internal Axis2D(PlayerController P_0, Definition P_1)
				: base(P_0, P_1, (P_1 == null) ? null : new Element.Definition[2]
				{
					(P_1.xAxis != null) ? P_1.xAxis : new Axis.Definition(),
					(P_1.yAxis != null) ? P_1.yAxis : new Axis.Definition()
				})
			{
			}
		}

		public sealed class MouseAxis2D : Axis2D
		{
			public new class Definition : Axis2D.Definition
			{
				public new MouseAxis.Definition xAxis
				{
					get
					{
						return base.xAxis as MouseAxis.Definition;
					}
					set
					{
						base.xAxis = value;
					}
				}

				public new MouseAxis.Definition yAxis
				{
					get
					{
						return base.yAxis as MouseAxis.Definition;
					}
					set
					{
						base.yAxis = value;
					}
				}

				internal virtual Element agoalJdsFWdAuSwhJfaDUTlyuyyN(PlayerController P_0)
				{
					return new MouseAxis2D(P_0, this);
				}
			}

			public new MouseAxis xAxis => AfsieBjIjldjgyYsyYijIuXtmLSc<MouseAxis>(0);

			public new MouseAxis yAxis => AfsieBjIjldjgyYsyYijIuXtmLSc<MouseAxis>(1);

			internal MouseAxis2D(PlayerController P_0, Definition P_1)
				: base(P_0, P_1, (P_1 == null) ? null : new Element.Definition[2]
				{
					(P_1.xAxis != null) ? P_1.xAxis : new MouseAxis.Definition(),
					(P_1.yAxis != null) ? P_1.yAxis : new MouseAxis.Definition()
				})
			{
			}
		}

		public sealed class Button : ElementWithSource
		{
			public new class Definition : ElementWithSource.Definition
			{
				internal virtual Element NanGEEdnXWynGSjDQHpuaLbQiwFIA(PlayerController P_0)
				{
					return new Button(P_0, this);
				}
			}

			public bool value
			{
				get
				{
					if (!base.selfAndParentEnabled || base.player == null)
					{
						return false;
					}
					return base.player.GetButton(base.actionId);
				}
			}

			public bool valuePrev
			{
				get
				{
					if (!base.selfAndParentEnabled || base.player == null)
					{
						return false;
					}
					return base.player.GetButtonPrev(base.actionId);
				}
			}

			public bool justPressed
			{
				get
				{
					if (!base.selfAndParentEnabled || base.player == null)
					{
						return false;
					}
					return base.player.GetButtonDown(base.actionId);
				}
			}

			public bool justReleased
			{
				get
				{
					if (!base.selfAndParentEnabled || base.player == null)
					{
						return false;
					}
					return base.player.GetButtonUp(base.actionId);
				}
			}

			internal Button(PlayerController P_0, Definition P_1)
				: base(P_0, P_1)
			{
			}
		}

		public abstract class CompoundElement : Element
		{
			public new abstract class Definition : Element.Definition
			{
				public Definition()
				{
				}
			}

			private readonly List<Element> WLsIoPjuOLiWhbUNeSpsBnUHgKhjA;

			internal int SqJvWBsKUUFYvXvvLZUdwsNIBPKd => WLsIoPjuOLiWhbUNeSpsBnUHgKhjA.Count;

			internal CompoundElement(PlayerController P_0, Definition P_1, Element.Definition[] P_2)
				: base(P_0, P_1)
			{
				WLsIoPjuOLiWhbUNeSpsBnUHgKhjA = new List<Element>();
				if (P_2 == null)
				{
					return;
				}
				for (int i = 0; i < P_2.Length; i++)
				{
					if (P_2[i] != null)
					{
						uyXSbfHiMLgJilqFXGhEJklEyGVkA(P_2[i].uMSAgxMOdUpelJFYWHYvEseLjvL(P_0));
					}
				}
			}

			internal _0001 AfsieBjIjldjgyYsyYijIuXtmLSc<_0001>(int P_0) where _0001 : Element
			{
				if ((uint)P_0 >= (uint)WLsIoPjuOLiWhbUNeSpsBnUHgKhjA.Count)
				{
					return null;
				}
				return WLsIoPjuOLiWhbUNeSpsBnUHgKhjA[P_0] as _0001;
			}

			internal void XIjuBKaDxZOESCHIEyDrEAidVhrv(List<Element> P_0)
			{
				for (int i = 0; i < WLsIoPjuOLiWhbUNeSpsBnUHgKhjA.Count; i++)
				{
					if (WLsIoPjuOLiWhbUNeSpsBnUHgKhjA[i] is CompoundElement)
					{
						(WLsIoPjuOLiWhbUNeSpsBnUHgKhjA[i] as CompoundElement).XIjuBKaDxZOESCHIEyDrEAidVhrv(P_0);
					}
					else
					{
						P_0.Add(WLsIoPjuOLiWhbUNeSpsBnUHgKhjA[i]);
					}
				}
			}

			internal void uyXSbfHiMLgJilqFXGhEJklEyGVkA(Element P_0)
			{
				if (P_0 == null)
				{
					throw new ArgumentNullException("element");
				}
				WLsIoPjuOLiWhbUNeSpsBnUHgKhjA.Add(P_0);
				P_0.ETenJLdSoKJHMOZGxsJdytHmFzBf = true;
			}
		}

		public abstract class Element
		{
			[CustomObfuscation(rename = false)]
			internal enum Type
			{
				[CustomObfuscation(rename = false)]
				Button = 0,
				[CustomObfuscation(rename = false)]
				Axis = 1,
				[CustomObfuscation(rename = false)]
				MouseAxis = 2,
				[CustomObfuscation(rename = false)]
				MouseWheelAxis = 3,
				[CustomObfuscation(rename = false)]
				Axis2D = 100,
				[CustomObfuscation(rename = false)]
				MouseAxis2D = 101,
				[CustomObfuscation(rename = false)]
				MouseWheel = 102
			}

			[CustomObfuscation(rename = false)]
			internal enum TypeWithSource
			{
				[CustomObfuscation(rename = false)]
				Button = 0,
				[CustomObfuscation(rename = false)]
				Axis = 1,
				[CustomObfuscation(rename = false)]
				MouseAxis = 2,
				[CustomObfuscation(rename = false)]
				MouseWheelAxis = 3
			}

			[CustomObfuscation(rename = false)]
			internal enum CompoundTypes
			{
				[CustomObfuscation(rename = false)]
				Axis2D = 100,
				[CustomObfuscation(rename = false)]
				MouseAxis2D = 101,
				[CustomObfuscation(rename = false)]
				MouseWheel = 102
			}

			public abstract class Definition
			{
				public bool enabled;

				public string name;

				public Definition()
				{
					enabled = true;
					name = null;
				}

				internal abstract Element uMSAgxMOdUpelJFYWHYvEseLjvL(PlayerController P_0);
			}

			internal struct XERrLtphHQIHSBrJxPdXFFmEKzRGc
			{
				public ControllerElementType RjqfGLAcSXLcXdHYJfiJESkDhBfFd;

				public int ofrCjdIYCiplwXublesLtmDZAedz;

				public float mwNoymLGuoRtuCRYiRcMGERhjyFV;

				public XERrLtphHQIHSBrJxPdXFFmEKzRGc(ControllerElementType P_0, int P_1, float P_2)
				{
					RjqfGLAcSXLcXdHYJfiJESkDhBfFd = P_0;
					ofrCjdIYCiplwXublesLtmDZAedz = P_1;
					mwNoymLGuoRtuCRYiRcMGERhjyFV = P_2;
				}
			}

			[CustomObfuscation(rename = false)]
			internal const bool defaultEnabled = true;

			private readonly PlayerController kTOvhJnGblXwcGPmcdTOTNYAgGTz;

			private bool PpcGWNfWvurZCCRCccNrFmqrXaoN;

			private bool YUEUtOKQwNKWCNVsVjVocsYmOmgA = true;

			private string HzHQEySawRmnknGodGpGIiCGCLHGA;

			private static int[] KYEbQSioAzTFTNdelgfHPukHFWkG;

			private static int[] WFVCLXTjUOdDkmwBmkHJqeavvCwt;

			protected Player player
			{
				get
				{
					if (!ReInput.isReady)
					{
						return null;
					}
					return ReInput.players.GetPlayer(kTOvhJnGblXwcGPmcdTOTNYAgGTz.FifFxABWbxyeBIxwvpHgdMBAWjWU);
				}
			}

			protected bool selfAndParentEnabled
			{
				get
				{
					if (YUEUtOKQwNKWCNVsVjVocsYmOmgA)
					{
						return kTOvhJnGblXwcGPmcdTOTNYAgGTz.zXBSreIhOpZqydEcACSRWUxuZtul;
					}
					return false;
				}
			}

			internal bool ETenJLdSoKJHMOZGxsJdytHmFzBf
			{
				get
				{
					return PpcGWNfWvurZCCRCccNrFmqrXaoN;
				}
				set
				{
					PpcGWNfWvurZCCRCccNrFmqrXaoN = true;
				}
			}

			public bool enabled
			{
				get
				{
					return YUEUtOKQwNKWCNVsVjVocsYmOmgA;
				}
				set
				{
					if (YUEUtOKQwNKWCNVsVjVocsYmOmgA != value)
					{
						YUEUtOKQwNKWCNVsVjVocsYmOmgA = value;
						EnabledStateChanged(value);
					}
				}
			}

			public string name
			{
				get
				{
					return HzHQEySawRmnknGodGpGIiCGCLHGA;
				}
				set
				{
					HzHQEySawRmnknGodGpGIiCGCLHGA = value;
				}
			}

			internal Element(PlayerController P_0, Definition P_1)
			{
				if (P_0 == null)
				{
					throw new ArgumentNullException("parent");
				}
				if (P_1 == null)
				{
					throw new ArgumentNullException("definition");
				}
				kTOvhJnGblXwcGPmcdTOTNYAgGTz = P_0;
				YUEUtOKQwNKWCNVsVjVocsYmOmgA = P_1.enabled;
				HzHQEySawRmnknGodGpGIiCGCLHGA = P_1.name;
			}

			internal virtual void SbMnAGsGmWYEjONsULhJPoDeiPHfA()
			{
			}

			protected virtual void EnabledStateChanged(bool state)
			{
			}

			[CustomObfuscation(rename = false)]
			internal static bool IsTypeWithSource(Type type)
			{
				if (KYEbQSioAzTFTNdelgfHPukHFWkG == null)
				{
					KYEbQSioAzTFTNdelgfHPukHFWkG = (int[])Enum.GetValues(typeof(TypeWithSource));
				}
				return ArrayTools.Contains(KYEbQSioAzTFTNdelgfHPukHFWkG, (int)type);
			}

			[CustomObfuscation(rename = false)]
			internal static bool IsCompoundType(Type type)
			{
				if (WFVCLXTjUOdDkmwBmkHJqeavvCwt == null)
				{
					WFVCLXTjUOdDkmwBmkHJqeavvCwt = (int[])Enum.GetValues(typeof(CompoundTypes));
				}
				return ArrayTools.Contains(WFVCLXTjUOdDkmwBmkHJqeavvCwt, (int)type);
			}

			[CustomObfuscation(rename = false)]
			internal static int GetMaxElementCount(Type type)
			{
				if (IsTypeWithSource(type))
				{
					return 1;
				}
				if (IsCompoundType(type))
				{
					return type switch
					{
						Type.Axis2D => 2, 
						Type.MouseAxis2D => 2, 
						Type.MouseWheel => 2, 
						_ => throw new NotImplementedException(), 
					};
				}
				throw new NotImplementedException();
			}

			[CustomObfuscation(rename = false)]
			internal static string GetElementTitle(Type type, int index)
			{
				if (index < 0 || index > GetMaxElementCount(type))
				{
					return null;
				}
				if (IsTypeWithSource(type))
				{
					return null;
				}
				if (IsCompoundType(type))
				{
					if ((uint)(type - 100) <= 2u)
					{
						if (index != 0)
						{
							return "Y Axis";
						}
						return "X Axis";
					}
					throw new NotImplementedException();
				}
				throw new NotImplementedException();
			}

			[CustomObfuscation(rename = false)]
			internal static Definition CreateDefinition(Type type)
			{
				return type switch
				{
					Type.Axis => new Axis.Definition(), 
					Type.Button => new Button.Definition(), 
					Type.MouseAxis => new MouseAxis.Definition(), 
					Type.MouseWheelAxis => new MouseWheelAxis.Definition(), 
					Type.Axis2D => new Axis2D.Definition(), 
					Type.MouseAxis2D => new MouseAxis2D.Definition(), 
					Type.MouseWheel => new MouseWheel.Definition(), 
					_ => throw new NotImplementedException(), 
				};
			}
		}

		public abstract class ElementWithSource : Element
		{
			public new abstract class Definition : Element.Definition
			{
				private int bUOwTgJmSZVNGTjQRmlodNUFLNQo;

				public int actionId
				{
					get
					{
						return bUOwTgJmSZVNGTjQRmlodNUFLNQo;
					}
					set
					{
						bUOwTgJmSZVNGTjQRmlodNUFLNQo = value;
					}
				}

				public string actionName
				{
					get
					{
						if (!ReInput.isReady || bUOwTgJmSZVNGTjQRmlodNUFLNQo < 0)
						{
							return null;
						}
						return ReInput.mapping.GetAction(bUOwTgJmSZVNGTjQRmlodNUFLNQo)?.name;
					}
					set
					{
						if (!ReInput.isReady)
						{
							Logger.LogError("You cannot set an Action Name because Rewired has not been intialized.");
							return;
						}
						InputAction action = ReInput.mapping.GetAction(value);
						if (action == null)
						{
							bUOwTgJmSZVNGTjQRmlodNUFLNQo = -1;
						}
						else
						{
							bUOwTgJmSZVNGTjQRmlodNUFLNQo = action.id;
						}
					}
				}

				public Definition()
				{
					bUOwTgJmSZVNGTjQRmlodNUFLNQo = -1;
				}
			}

			[CustomObfuscation(rename = false)]
			internal const int defaultActionId = -1;

			private int GDlcPZUyPPIftxDeDxHnSeUnhmzA = -1;

			public int actionId
			{
				get
				{
					return GDlcPZUyPPIftxDeDxHnSeUnhmzA;
				}
				set
				{
					GDlcPZUyPPIftxDeDxHnSeUnhmzA = value;
				}
			}

			public string actionName
			{
				get
				{
					if (!ReInput.isReady || GDlcPZUyPPIftxDeDxHnSeUnhmzA < 0)
					{
						return null;
					}
					return ReInput.mapping.GetAction(GDlcPZUyPPIftxDeDxHnSeUnhmzA)?.name;
				}
				set
				{
					if (ReInput.isReady)
					{
						InputAction action = ReInput.mapping.GetAction(value);
						if (action == null)
						{
							GDlcPZUyPPIftxDeDxHnSeUnhmzA = -1;
						}
						else
						{
							GDlcPZUyPPIftxDeDxHnSeUnhmzA = action.id;
						}
					}
				}
			}

			internal ElementWithSource(PlayerController P_0, Definition P_1)
				: base(P_0, P_1)
			{
				GDlcPZUyPPIftxDeDxHnSeUnhmzA = P_1.actionId;
			}
		}

		public sealed class MouseWheel : Axis2D
		{
			public new class Definition : Axis2D.Definition
			{
				public new MouseWheelAxis.Definition xAxis
				{
					get
					{
						return base.xAxis as MouseWheelAxis.Definition;
					}
					set
					{
						base.xAxis = value;
					}
				}

				public new MouseWheelAxis.Definition yAxis
				{
					get
					{
						return base.yAxis as MouseWheelAxis.Definition;
					}
					set
					{
						base.yAxis = value;
					}
				}

				internal virtual Element sLidTdGsAZaMbfmtmrZHUxKaDIGtA(PlayerController P_0)
				{
					return new MouseWheel(P_0, this);
				}
			}

			public new MouseWheelAxis xAxis => AfsieBjIjldjgyYsyYijIuXtmLSc<MouseWheelAxis>(0);

			public new MouseWheelAxis yAxis => AfsieBjIjldjgyYsyYijIuXtmLSc<MouseWheelAxis>(1);

			internal MouseWheel(PlayerController P_0, Definition P_1)
				: base(P_0, P_1, (P_1 == null) ? null : new Element.Definition[2]
				{
					(P_1.xAxis != null) ? P_1.xAxis : new MouseWheelAxis.Definition(),
					(P_1.yAxis != null) ? P_1.yAxis : new MouseWheelAxis.Definition()
				})
			{
			}
		}

		public sealed class MouseWheelAxis : Axis
		{
			public new class Definition : Axis.Definition
			{
				public float repeatRate;

				public Definition()
				{
					coordinateMode = AxisCoordinateMode.Relative;
					repeatRate = 4f;
				}

				internal virtual Element UGsxZLwGWXHQTnhFPxqZScVvFxZs(PlayerController P_0)
				{
					return new MouseWheelAxis(P_0, this);
				}
			}

			[CustomObfuscation(rename = false)]
			internal const float defaultRepeatRate = 4f;

			[CustomObfuscation(rename = false)]
			internal new const AxisCoordinateMode defaultAxisCoordinateMode = AxisCoordinateMode.Relative;

			private const float nLBVxWJKCFZNyETEKBqAgHzNCzul = 0.01f;

			private float kRmcdwIvaTUgEIoeWhiOfZvQGACMA = 0.25f;

			private double NEoBgqRWhUEdHAVcBcsbwUUGKqaH;

			private float YaQSbeluAVwrhOQKPebgaUtnjuzw;

			public float repeatRate
			{
				get
				{
					if (kRmcdwIvaTUgEIoeWhiOfZvQGACMA == 0f)
					{
						return 0f;
					}
					return 1f / kRmcdwIvaTUgEIoeWhiOfZvQGACMA;
				}
				set
				{
					if (value < 0f)
					{
						value = 0f;
					}
					if (value == 0f)
					{
						kRmcdwIvaTUgEIoeWhiOfZvQGACMA = 0f;
					}
					else
					{
						kRmcdwIvaTUgEIoeWhiOfZvQGACMA = 1f / value;
					}
				}
			}

			float Axis.value
			{
				get
				{
					if (!base.selfAndParentEnabled)
					{
						return 0f;
					}
					return YaQSbeluAVwrhOQKPebgaUtnjuzw;
				}
			}

			internal MouseWheelAxis(PlayerController P_0, Definition P_1)
				: base(P_0, P_1)
			{
				repeatRate = P_1.repeatRate;
			}

			internal void KnufIakUazmmwIeCEMKgSUYNmnssA()
			{
				base.SbMnAGsGmWYEjONsULhJPoDeiPHfA();
				if (base.selfAndParentEnabled)
				{
					YaQSbeluAVwrhOQKPebgaUtnjuzw = IwpmZyWAQMAmDQQWubCTOQRnNrdE();
				}
			}

			protected override void EnabledStateChanged(bool state)
			{
				base.EnabledStateChanged(state);
				if (!state)
				{
					zvNgAhqOtCpnnLCBdQheOKajxABl();
				}
			}

			private float IwpmZyWAQMAmDQQWubCTOQRnNrdE()
			{
				if (base.player == null)
				{
					return 0f;
				}
				float num = base.player.GetAxis(base.actionId);
				switch (base.player.GetAxisCoordinateMode(base.actionId))
				{
				case AxisCoordinateMode.Absolute:
				{
					bool flag = false;
					if (base.player.GetButtonDown(base.actionId))
					{
						flag = true;
						num = 1f;
					}
					if (base.player.GetNegativeButtonDown(base.actionId))
					{
						flag = true;
						num = -1f;
					}
					if (!flag && ReInput.unscaledTime < NEoBgqRWhUEdHAVcBcsbwUUGKqaH + (double)kRmcdwIvaTUgEIoeWhiOfZvQGACMA)
					{
						return 0f;
					}
					if (Mathf.Abs(num) <= 0.01f)
					{
						return 0f;
					}
					num = Mathf.Sign(num);
					num *= base.absoluteToRelativeSensitivity;
					NEoBgqRWhUEdHAVcBcsbwUUGKqaH = ReInput.unscaledTime;
					break;
				}
				}
				return num;
			}

			private void zvNgAhqOtCpnnLCBdQheOKajxABl()
			{
				YaQSbeluAVwrhOQKPebgaUtnjuzw = 0f;
				NEoBgqRWhUEdHAVcBcsbwUUGKqaH = 0.0;
			}
		}

		internal readonly int EXFEcMcVkijIhrVBqTYMJVvSNSSH;

		private bool zXBSreIhOpZqydEcACSRWUxuZtul;

		private int FifFxABWbxyeBIxwvpHgdMBAWjWU;

		private readonly AList<Element> kFMAXZxLfisMKYTzevjoCdXXMxmf;

		private readonly AList<Button> yFMRuJAtAuXaGwvEFyOxDcchgjPE;

		private readonly AList<Axis> PemAltvWhpSrfJArrYngqJLptdRL;

		private readonly ReadOnlyCollection<Element> wbBTUngAjTFsdfMfOfjIGTHrwHXbb;

		private readonly ReadOnlyCollection<Button> qtZANZCQwSSWMMQRPoGLSCLLbgJcb;

		private readonly ReadOnlyCollection<Axis> KrLvMcrxaUYOHkOXmbPcjQvuoOVA;

		private readonly List<Element.XERrLtphHQIHSBrJxPdXFFmEKzRGc> ndgXMQbJoMoXKjBGllnzLejcTtZd;

		private Action<int, bool> QXtjeBaEJPtpWnlsFKwcgJNINjJl;

		private Action<int, float> bTSVVeilONMrrGXtnjRfOUgEMAyH;

		private Action<bool> okHUliTgGQyhchHxBpIWzPfLdKAT;

		private static Vector2 kQoXwJLpJTnnjBdjeokpLPEGadpKA = new Vector2(1920f, 1080f);

		bool IPlayerController.enabled
		{
			get
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
					return false;
				}
				return zXBSreIhOpZqydEcACSRWUxuZtul;
			}
			set
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
				}
				else
				{
					if (zXBSreIhOpZqydEcACSRWUxuZtul == value)
					{
						return;
					}
					if (!value)
					{
						ClearVars();
					}
					zXBSreIhOpZqydEcACSRWUxuZtul = value;
					for (int i = 0; i < kFMAXZxLfisMKYTzevjoCdXXMxmf._count; i++)
					{
						kFMAXZxLfisMKYTzevjoCdXXMxmf[i].enabled = value;
					}
					if (okHUliTgGQyhchHxBpIWzPfLdKAT != null)
					{
						try
						{
							okHUliTgGQyhchHxBpIWzPfLdKAT(value);
						}
						catch (Exception ex)
						{
							Logger.LogError("An exception occurred in a listener of EnabledStateChangedEvent. This means an exception was thrown by your code.\n" + ex);
						}
					}
				}
			}
		}

		int IPlayerController.playerId
		{
			get
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
					return -1;
				}
				return FifFxABWbxyeBIxwvpHgdMBAWjWU;
			}
			set
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
				}
				else if (FifFxABWbxyeBIxwvpHgdMBAWjWU != value)
				{
					FifFxABWbxyeBIxwvpHgdMBAWjWU = value;
					ClearVars();
				}
			}
		}

		IList<Button> IPlayerController.buttons
		{
			get
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
					return null;
				}
				return qtZANZCQwSSWMMQRPoGLSCLLbgJcb;
			}
		}

		IList<Axis> IPlayerController.axes
		{
			get
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
					return null;
				}
				return KrLvMcrxaUYOHkOXmbPcjQvuoOVA;
			}
		}

		IList<Element> IPlayerController.elements
		{
			get
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
					return null;
				}
				return wbBTUngAjTFsdfMfOfjIGTHrwHXbb;
			}
		}

		int IPlayerController.buttonCount
		{
			get
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
					return 0;
				}
				if (yFMRuJAtAuXaGwvEFyOxDcchgjPE == null)
				{
					return 0;
				}
				return yFMRuJAtAuXaGwvEFyOxDcchgjPE._count;
			}
		}

		int IPlayerController.axisCount
		{
			get
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
					return 0;
				}
				if (PemAltvWhpSrfJArrYngqJLptdRL == null)
				{
					return 0;
				}
				return PemAltvWhpSrfJArrYngqJLptdRL._count;
			}
		}

		int IPlayerController.elementCount
		{
			get
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
					return 0;
				}
				if (kFMAXZxLfisMKYTzevjoCdXXMxmf == null)
				{
					return 0;
				}
				return kFMAXZxLfisMKYTzevjoCdXXMxmf._count;
			}
		}

		internal Player QiJEXVIXovhEnewuSvocJGYfBNrfA
		{
			get
			{
				if (!ReInput.isReady)
				{
					return null;
				}
				return ReInput.players.GetPlayer(Rewired_002EIPlayerController_002EplayerId);
			}
		}

		public static Vector2 absoluteToRelativeScalingReferenceResolution
		{
			get
			{
				return kQoXwJLpJTnnjBdjeokpLPEGadpKA;
			}
			set
			{
				if (value.x < 1f)
				{
					value.x = 1f;
				}
				if (value.y < 1f)
				{
					value.y = 1f;
				}
				kQoXwJLpJTnnjBdjeokpLPEGadpKA = value;
			}
		}

		event Action<int, bool> IPlayerController.ButtonStateChangedEvent
		{
			add
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
				}
				else
				{
					QXtjeBaEJPtpWnlsFKwcgJNINjJl = (Action<int, bool>)Delegate.Combine(QXtjeBaEJPtpWnlsFKwcgJNINjJl, value);
				}
			}
			remove
			{
				QXtjeBaEJPtpWnlsFKwcgJNINjJl = (Action<int, bool>)Delegate.Remove(QXtjeBaEJPtpWnlsFKwcgJNINjJl, value);
			}
		}

		event Action<int, float> IPlayerController.AxisValueChangedEvent
		{
			add
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
				}
				else
				{
					bTSVVeilONMrrGXtnjRfOUgEMAyH = (Action<int, float>)Delegate.Combine(bTSVVeilONMrrGXtnjRfOUgEMAyH, value);
				}
			}
			remove
			{
				bTSVVeilONMrrGXtnjRfOUgEMAyH = (Action<int, float>)Delegate.Remove(bTSVVeilONMrrGXtnjRfOUgEMAyH, value);
			}
		}

		event Action<bool> IPlayerController.EnabledStateChangedEvent
		{
			add
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
				}
				else
				{
					okHUliTgGQyhchHxBpIWzPfLdKAT = (Action<bool>)Delegate.Combine(okHUliTgGQyhchHxBpIWzPfLdKAT, value);
				}
			}
			remove
			{
				okHUliTgGQyhchHxBpIWzPfLdKAT = (Action<bool>)Delegate.Remove(okHUliTgGQyhchHxBpIWzPfLdKAT, value);
			}
		}

		internal PlayerController(Definition P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("definition");
			}
			if (P_0.elements == null)
			{
				throw new ArgumentNullException("definition.elements");
			}
			EXFEcMcVkijIhrVBqTYMJVvSNSSH = ReInput._id;
			FifFxABWbxyeBIxwvpHgdMBAWjWU = P_0.playerId;
			zXBSreIhOpZqydEcACSRWUxuZtul = P_0.enabled;
			List<Element> list = new List<Element>();
			List<Element> list2 = new List<Element>();
			List<Button> list3 = new List<Button>();
			List<Axis> list4 = new List<Axis>();
			foreach (Element.Definition element in P_0.elements)
			{
				QodgguHTjBoGYdHnkaHiqPzdkcpPB(element.uMSAgxMOdUpelJFYWHYvEseLjvL(this), list, list2, list3, list4);
			}
			list.AddRange(list2);
			kFMAXZxLfisMKYTzevjoCdXXMxmf = new AList<Element>(list);
			yFMRuJAtAuXaGwvEFyOxDcchgjPE = new AList<Button>(list3);
			PemAltvWhpSrfJArrYngqJLptdRL = new AList<Axis>(list4);
			wbBTUngAjTFsdfMfOfjIGTHrwHXbb = new ReadOnlyCollection<Element>(kFMAXZxLfisMKYTzevjoCdXXMxmf);
			qtZANZCQwSSWMMQRPoGLSCLLbgJcb = new ReadOnlyCollection<Button>(yFMRuJAtAuXaGwvEFyOxDcchgjPE);
			KrLvMcrxaUYOHkOXmbPcjQvuoOVA = new ReadOnlyCollection<Axis>(PemAltvWhpSrfJArrYngqJLptdRL);
			ndgXMQbJoMoXKjBGllnzLejcTtZd = new List<Element.XERrLtphHQIHSBrJxPdXFFmEKzRGc>();
			ReInput.UpdateEndedEvent += sKTcvxEUkmwxpekCXcaPlUfbZpzr;
		}

		~PlayerController()
		{
			ReInput.UpdateEndedEvent -= sKTcvxEUkmwxpekCXcaPlUfbZpzr;
		}

		public bool GetButton(int index)
		{
			if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
			{
				ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
				return false;
			}
			if ((uint)index >= (uint)yFMRuJAtAuXaGwvEFyOxDcchgjPE._count)
			{
				return false;
			}
			return yFMRuJAtAuXaGwvEFyOxDcchgjPE[index].value;
		}

		bool IPlayerController.GetButton(int index)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetButton
			return this.GetButton(index);
		}

		public bool GetButtonDown(int index)
		{
			if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
			{
				ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
				return false;
			}
			if ((uint)index >= (uint)yFMRuJAtAuXaGwvEFyOxDcchgjPE._count)
			{
				return false;
			}
			return yFMRuJAtAuXaGwvEFyOxDcchgjPE[index].justPressed;
		}

		bool IPlayerController.GetButtonDown(int index)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetButtonDown
			return this.GetButtonDown(index);
		}

		public bool GetButtonUp(int index)
		{
			if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
			{
				ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
				return false;
			}
			if ((uint)index >= (uint)yFMRuJAtAuXaGwvEFyOxDcchgjPE._count)
			{
				return false;
			}
			return yFMRuJAtAuXaGwvEFyOxDcchgjPE[index].justReleased;
		}

		bool IPlayerController.GetButtonUp(int index)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetButtonUp
			return this.GetButtonUp(index);
		}

		public float GetAxis(int index)
		{
			if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
			{
				ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
				return 0f;
			}
			if ((uint)index >= (uint)PemAltvWhpSrfJArrYngqJLptdRL._count)
			{
				return 0f;
			}
			return PemAltvWhpSrfJArrYngqJLptdRL[index].value;
		}

		float IPlayerController.GetAxis(int index)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetAxis
			return this.GetAxis(index);
		}

		public float GetAxisRaw(int index)
		{
			if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
			{
				ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
				return 0f;
			}
			if ((uint)index >= (uint)PemAltvWhpSrfJArrYngqJLptdRL._count)
			{
				return 0f;
			}
			return PemAltvWhpSrfJArrYngqJLptdRL[index].valueRaw;
		}

		float IPlayerController.GetAxisRaw(int index)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetAxisRaw
			return this.GetAxisRaw(index);
		}

		public Element GetElement(int index)
		{
			if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
			{
				ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
				return null;
			}
			if ((uint)index >= (uint)kFMAXZxLfisMKYTzevjoCdXXMxmf._count)
			{
				return null;
			}
			return kFMAXZxLfisMKYTzevjoCdXXMxmf[index];
		}

		Element IPlayerController.GetElement(int index)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetElement
			return this.GetElement(index);
		}

		public T GetElement<T>(int index) where T : Element
		{
			return GetElement(index) as T;
		}

		T IPlayerController.GetElement<T>(int index)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetElement
			return this.GetElement<T>(index);
		}

		private void sKTcvxEUkmwxpekCXcaPlUfbZpzr(UpdateLoopType P_0)
		{
			Update(P_0);
			UpdateFinished();
		}

		protected virtual bool Update(UpdateLoopType updateLoop)
		{
			if (!zXBSreIhOpZqydEcACSRWUxuZtul)
			{
				return false;
			}
			bool flag = bTSVVeilONMrrGXtnjRfOUgEMAyH != null;
			bool flag2 = QXtjeBaEJPtpWnlsFKwcgJNINjJl != null;
			for (int i = 0; i < kFMAXZxLfisMKYTzevjoCdXXMxmf._count; i++)
			{
				float num = 0f;
				if (flag && kFMAXZxLfisMKYTzevjoCdXXMxmf[i] is Axis)
				{
					Axis axis = kFMAXZxLfisMKYTzevjoCdXXMxmf[i] as Axis;
					num = ((axis.coordinateMode != AxisCoordinateMode.Absolute) ? 0f : axis.value);
				}
				kFMAXZxLfisMKYTzevjoCdXXMxmf[i].SbMnAGsGmWYEjONsULhJPoDeiPHfA();
				if (flag2 && kFMAXZxLfisMKYTzevjoCdXXMxmf[i] is Button)
				{
					Button button = kFMAXZxLfisMKYTzevjoCdXXMxmf[i] as Button;
					if (button.justPressed && button.value)
					{
						ndgXMQbJoMoXKjBGllnzLejcTtZd.Add(new Element.XERrLtphHQIHSBrJxPdXFFmEKzRGc(ControllerElementType.Button, i, 1f));
					}
					else if (button.justReleased && !button.value)
					{
						ndgXMQbJoMoXKjBGllnzLejcTtZd.Add(new Element.XERrLtphHQIHSBrJxPdXFFmEKzRGc(ControllerElementType.Button, i, 0f));
					}
				}
				else if (flag && kFMAXZxLfisMKYTzevjoCdXXMxmf[i] is Axis)
				{
					ndgXMQbJoMoXKjBGllnzLejcTtZd.Add(new Element.XERrLtphHQIHSBrJxPdXFFmEKzRGc(ControllerElementType.Axis, i, (kFMAXZxLfisMKYTzevjoCdXXMxmf[i] as Axis).value - num));
				}
			}
			return true;
		}

		protected virtual void UpdateFinished()
		{
			int count = ndgXMQbJoMoXKjBGllnzLejcTtZd.Count;
			if (count <= 0)
			{
				return;
			}
			for (int i = 0; i < count; i++)
			{
				Element.XERrLtphHQIHSBrJxPdXFFmEKzRGc xERrLtphHQIHSBrJxPdXFFmEKzRGc = ndgXMQbJoMoXKjBGllnzLejcTtZd[i];
				if (xERrLtphHQIHSBrJxPdXFFmEKzRGc.RjqfGLAcSXLcXdHYJfiJESkDhBfFd == ControllerElementType.Button)
				{
					try
					{
						QXtjeBaEJPtpWnlsFKwcgJNINjJl(xERrLtphHQIHSBrJxPdXFFmEKzRGc.ofrCjdIYCiplwXublesLtmDZAedz, xERrLtphHQIHSBrJxPdXFFmEKzRGc.mwNoymLGuoRtuCRYiRcMGERhjyFV > 0f);
					}
					catch (Exception ex)
					{
						Logger.LogError("An exception occurred in a listener of ButtonStateChangedEvent. This means an exception was thrown by your code.\n" + ex);
					}
				}
				else if (xERrLtphHQIHSBrJxPdXFFmEKzRGc.RjqfGLAcSXLcXdHYJfiJESkDhBfFd == ControllerElementType.Axis)
				{
					try
					{
						bTSVVeilONMrrGXtnjRfOUgEMAyH(xERrLtphHQIHSBrJxPdXFFmEKzRGc.ofrCjdIYCiplwXublesLtmDZAedz, xERrLtphHQIHSBrJxPdXFFmEKzRGc.mwNoymLGuoRtuCRYiRcMGERhjyFV);
					}
					catch (Exception ex2)
					{
						Logger.LogError("An exception occurred in a listener of AxisValueChangedEvent. This means an exception was thrown by your code.\n" + ex2);
					}
				}
			}
			ndgXMQbJoMoXKjBGllnzLejcTtZd.Clear();
		}

		protected virtual void ClearVars()
		{
			ndgXMQbJoMoXKjBGllnzLejcTtZd.Clear();
		}

		internal void iSDfvshuzoGnugywoIegMIxXehOfA(Element P_0)
		{
			if (P_0 != null)
			{
				if (P_0 is Axis)
				{
					PemAltvWhpSrfJArrYngqJLptdRL.Add(P_0 as Axis);
				}
				else if (P_0 is Button)
				{
					yFMRuJAtAuXaGwvEFyOxDcchgjPE.Add(P_0 as Button);
				}
				kFMAXZxLfisMKYTzevjoCdXXMxmf.Add(P_0);
			}
		}

		private void QodgguHTjBoGYdHnkaHiqPzdkcpPB(Element P_0, List<Element> P_1, List<Element> P_2, List<Button> P_3, List<Axis> P_4)
		{
			if (P_0 == null)
			{
				return;
			}
			P_0.GetType();
			if (P_0 is ElementWithSource)
			{
				if (P_0 is Button)
				{
					P_3.Add((Button)P_0);
				}
				else
				{
					if (!(P_0 is Axis))
					{
						Logger.LogWarning("Unknown Element type encountered: " + P_0.GetType());
						return;
					}
					P_4.Add((Axis)P_0);
				}
				P_1.Add(P_0);
			}
			else if (P_0 is CompoundElement)
			{
				using (TempListPool.TList<Element> tList = TempListPool.GetTList<Element>())
				{
					List<Element> list = tList.list;
					(P_0 as CompoundElement).XIjuBKaDxZOESCHIEyDrEAidVhrv(list);
					for (int i = 0; i < list.Count; i++)
					{
						QodgguHTjBoGYdHnkaHiqPzdkcpPB(list[i], P_1, P_2, P_3, P_4);
					}
				}
				P_2.Add(P_0);
			}
			else
			{
				Logger.LogWarning("Unknown Element type encountered: " + P_0.GetType());
			}
		}

		internal static int yWBUvNGgiXQmTtGkmItUCJfuMrUGb<_0001>(IList<_0001> P_0, Predicate<_0001> P_1, int P_2) where _0001 : Element
		{
			int num = 0;
			for (int i = 0; i < P_0.Count; i++)
			{
				if (P_1(P_0[i]))
				{
					num++;
				}
				if (num == P_2)
				{
					return i;
				}
			}
			return -1;
		}
	}
}
