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

		public class Axis : ElementWithSource
		{
			public new class Definition : ElementWithSource.Definition
			{
				public AxisCoordinateMode coordinateMode;

				public float absoluteToRelativeSensitivity;

				public Definition()
				{
					coordinateMode = AxisCoordinateMode.Absolute;
					absoluteToRelativeSensitivity = 1f;
				}

				internal override Element FRGzoQGxlhWgnLpuxZPVeqiuvlgD(PlayerController P_0)
				{
					return new Axis(P_0, this);
				}
			}

			internal const float tKzWjAAqFxznILFKHvHDMeAnaEtx = 1f;

			[CustomObfuscation(rename = false)]
			internal const AxisCoordinateMode defaultAxisCoordinateMode = AxisCoordinateMode.Absolute;

			private float kGpcICdBcebufqmYNGRRSLHNkiOb = 1f;

			private AxisCoordinateMode IuVzvlfMSigqbjOqXgvDBwRIIUCO;

			public float absoluteToRelativeSensitivity
			{
				get
				{
					return kGpcICdBcebufqmYNGRRSLHNkiOb;
				}
				set
				{
					if (value < 0f)
					{
						value = 0f;
					}
					kGpcICdBcebufqmYNGRRSLHNkiOb = value;
				}
			}

			public AxisCoordinateMode coordinateMode => IuVzvlfMSigqbjOqXgvDBwRIIUCO;

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
						if (IuVzvlfMSigqbjOqXgvDBwRIIUCO == AxisCoordinateMode.Absolute)
						{
							return 0f;
						}
						break;
					case AxisCoordinateMode.Absolute:
						if (IuVzvlfMSigqbjOqXgvDBwRIIUCO == AxisCoordinateMode.Relative)
						{
							num *= (float)ReInput.unscaledDeltaTime * kGpcICdBcebufqmYNGRRSLHNkiOb;
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
				kGpcICdBcebufqmYNGRRSLHNkiOb = P_1.absoluteToRelativeSensitivity;
				IuVzvlfMSigqbjOqXgvDBwRIIUCO = P_1.coordinateMode;
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
				}

				internal override Element FRGzoQGxlhWgnLpuxZPVeqiuvlgD(PlayerController P_0)
				{
					return new MouseAxis(P_0, this);
				}
			}

			[CustomObfuscation(rename = false)]
			internal new const AxisCoordinateMode defaultAxisCoordinateMode = AxisCoordinateMode.Relative;

			[CustomObfuscation(rename = false)]
			internal const float defaultAbsoluteToRelativeSensitivity = 600f;

			public override float value
			{
				get
				{
					float num = base.value;
					if (num == 0f)
					{
						return 0f;
					}
					if (base.coordinateMode == AxisCoordinateMode.Relative && base.player.GetAxisCoordinateMode(base.actionId) == AxisCoordinateMode.Absolute)
					{
						num *= (float)Screen.currentResolution.width / 1920f;
					}
					return num;
				}
			}

			internal MouseAxis(PlayerController P_0, Definition P_1)
				: base(P_0, P_1)
			{
			}
		}

		public class Axis2D : CompoundElement
		{
			public new class Definition : CompoundElement.Definition
			{
				private Axis.Definition mQTUIoqEkWTkreeIZmWyMwdawBgc;

				private Axis.Definition mrWOIvlMbZydDUixWVKOhCmTyEYh;

				public Axis.Definition xAxis
				{
					get
					{
						return mQTUIoqEkWTkreeIZmWyMwdawBgc;
					}
					set
					{
						mQTUIoqEkWTkreeIZmWyMwdawBgc = value;
					}
				}

				public Axis.Definition yAxis
				{
					get
					{
						return mrWOIvlMbZydDUixWVKOhCmTyEYh;
					}
					set
					{
						mrWOIvlMbZydDUixWVKOhCmTyEYh = value;
					}
				}

				internal override Element FRGzoQGxlhWgnLpuxZPVeqiuvlgD(PlayerController P_0)
				{
					return new Axis2D(P_0, this);
				}
			}

			internal const int QsqVdRTBAUHEvDViXQbbzdrUaQRHA = 0;

			internal const int YAbfcWcsFzneCbXcChSDKlaAswPBc = 1;

			internal const int CEQdBhgeYEHICTmuwWJgWayBlcje = 2;

			public Axis xAxis => pAGcyUYRwJwYiiycWKSARieMOZIo<Axis>(0);

			public Axis yAxis => pAGcyUYRwJwYiiycWKSARieMOZIo<Axis>(1);

			public virtual Vector2 value => new Vector2(pAGcyUYRwJwYiiycWKSARieMOZIo<Axis>(0).value, pAGcyUYRwJwYiiycWKSARieMOZIo<Axis>(1).value);

			public virtual Vector2 valueRaw => new Vector2(pAGcyUYRwJwYiiycWKSARieMOZIo<Axis>(0).valueRaw, pAGcyUYRwJwYiiycWKSARieMOZIo<Axis>(1).valueRaw);

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

				internal override Element FRGzoQGxlhWgnLpuxZPVeqiuvlgD(PlayerController P_0)
				{
					return new MouseAxis2D(P_0, this);
				}
			}

			public new MouseAxis xAxis => pAGcyUYRwJwYiiycWKSARieMOZIo<MouseAxis>(0);

			public new MouseAxis yAxis => pAGcyUYRwJwYiiycWKSARieMOZIo<MouseAxis>(1);

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
				internal override Element FRGzoQGxlhWgnLpuxZPVeqiuvlgD(PlayerController P_0)
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

			private readonly List<Element> hObmyuQkxgNXDiBtSEwFNgLuBsoZ;

			internal int VTGRGDGMRWfujiZeioxOnxhTqBWu => hObmyuQkxgNXDiBtSEwFNgLuBsoZ.Count;

			internal CompoundElement(PlayerController P_0, Definition P_1, Element.Definition[] P_2)
				: base(P_0, P_1)
			{
				hObmyuQkxgNXDiBtSEwFNgLuBsoZ = new List<Element>();
				if (P_2 == null)
				{
					return;
				}
				for (int i = 0; i < P_2.Length; i++)
				{
					if (P_2[i] != null)
					{
						JymOYvJCaUZfrfNvqSliYMwhgCGz(P_2[i].FRGzoQGxlhWgnLpuxZPVeqiuvlgD(P_0));
					}
				}
			}

			internal _0001 pAGcyUYRwJwYiiycWKSARieMOZIo<_0001>(int P_0) where _0001 : Element
			{
				if ((uint)P_0 >= (uint)hObmyuQkxgNXDiBtSEwFNgLuBsoZ.Count)
				{
					return null;
				}
				return hObmyuQkxgNXDiBtSEwFNgLuBsoZ[P_0] as _0001;
			}

			internal void yfvUZvCErrubvkdbzulBqRmKnLaT(List<Element> P_0)
			{
				for (int i = 0; i < hObmyuQkxgNXDiBtSEwFNgLuBsoZ.Count; i++)
				{
					if (hObmyuQkxgNXDiBtSEwFNgLuBsoZ[i] is CompoundElement)
					{
						(hObmyuQkxgNXDiBtSEwFNgLuBsoZ[i] as CompoundElement).yfvUZvCErrubvkdbzulBqRmKnLaT(P_0);
					}
					else
					{
						P_0.Add(hObmyuQkxgNXDiBtSEwFNgLuBsoZ[i]);
					}
				}
			}

			internal void JymOYvJCaUZfrfNvqSliYMwhgCGz(Element P_0)
			{
				if (P_0 == null)
				{
					throw new ArgumentNullException("element");
				}
				hObmyuQkxgNXDiBtSEwFNgLuBsoZ.Add(P_0);
				P_0.RJWetIMScbNkkayUbJoRDyHMCesM = true;
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

				internal abstract Element FRGzoQGxlhWgnLpuxZPVeqiuvlgD(PlayerController P_0);
			}

			internal struct znbsAbXzumooNTekBhsPkirgaxEt
			{
				public ControllerElementType MdjBbHAfEpOIyysKWtDJwilBqwCjA;

				public int PqXTkiKXdxRLgXpucsKUqWEJhHUQ;

				public float yYOUbwIbBcyPAQvjeAEXVsjzLAln;

				public znbsAbXzumooNTekBhsPkirgaxEt(ControllerElementType P_0, int P_1, float P_2)
				{
					MdjBbHAfEpOIyysKWtDJwilBqwCjA = P_0;
					PqXTkiKXdxRLgXpucsKUqWEJhHUQ = P_1;
					yYOUbwIbBcyPAQvjeAEXVsjzLAln = P_2;
				}
			}

			[CustomObfuscation(rename = false)]
			internal const bool defaultEnabled = true;

			private readonly PlayerController afWhZEYTbMFjMUeulVkAPjICyxpP;

			private bool ZVriljhasPxEFvNaLvNOVyfTIDdb;

			private bool kKFZZElqKQSUFMZnWdEudRvTJGpo = true;

			private string hEJFspqWqeBOdedbqHNRpPNEbfBG;

			private static int[] qGbdMURwQUZeXFRbaUvBlTnHrsNF;

			private static int[] wmvCHfufqZIquHLzLpGHJPmzGeky;

			protected Player player
			{
				get
				{
					if (!ReInput.isReady)
					{
						return null;
					}
					return ReInput.players.GetPlayer(afWhZEYTbMFjMUeulVkAPjICyxpP.DCGJXzigEdTcwzFynDKUcORiHGSOA);
				}
			}

			protected bool selfAndParentEnabled
			{
				get
				{
					if (kKFZZElqKQSUFMZnWdEudRvTJGpo)
					{
						return afWhZEYTbMFjMUeulVkAPjICyxpP.kKFZZElqKQSUFMZnWdEudRvTJGpo;
					}
					return false;
				}
			}

			internal bool RJWetIMScbNkkayUbJoRDyHMCesM
			{
				get
				{
					return ZVriljhasPxEFvNaLvNOVyfTIDdb;
				}
				set
				{
					ZVriljhasPxEFvNaLvNOVyfTIDdb = true;
				}
			}

			public bool enabled
			{
				get
				{
					return kKFZZElqKQSUFMZnWdEudRvTJGpo;
				}
				set
				{
					if (kKFZZElqKQSUFMZnWdEudRvTJGpo != value)
					{
						kKFZZElqKQSUFMZnWdEudRvTJGpo = value;
						EnabledStateChanged(value);
					}
				}
			}

			public string name
			{
				get
				{
					return hEJFspqWqeBOdedbqHNRpPNEbfBG;
				}
				set
				{
					hEJFspqWqeBOdedbqHNRpPNEbfBG = value;
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
				afWhZEYTbMFjMUeulVkAPjICyxpP = P_0;
				kKFZZElqKQSUFMZnWdEudRvTJGpo = P_1.enabled;
			}

			internal virtual void jRaYtHNVcykNMAbqOnSGaKIIGSEaA()
			{
			}

			protected virtual void EnabledStateChanged(bool state)
			{
			}

			[CustomObfuscation(rename = false)]
			internal static bool IsTypeWithSource(Type type)
			{
				if (qGbdMURwQUZeXFRbaUvBlTnHrsNF == null)
				{
					qGbdMURwQUZeXFRbaUvBlTnHrsNF = (int[])Enum.GetValues(typeof(TypeWithSource));
				}
				return ArrayTools.Contains(qGbdMURwQUZeXFRbaUvBlTnHrsNF, (int)type);
			}

			[CustomObfuscation(rename = false)]
			internal static bool IsCompoundType(Type type)
			{
				if (wmvCHfufqZIquHLzLpGHJPmzGeky == null)
				{
					wmvCHfufqZIquHLzLpGHJPmzGeky = (int[])Enum.GetValues(typeof(CompoundTypes));
				}
				return ArrayTools.Contains(wmvCHfufqZIquHLzLpGHJPmzGeky, (int)type);
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
				private int JwOchbdvhsvHVdSyYjtqEJdTZzYG;

				public int actionId
				{
					get
					{
						return JwOchbdvhsvHVdSyYjtqEJdTZzYG;
					}
					set
					{
						JwOchbdvhsvHVdSyYjtqEJdTZzYG = value;
					}
				}

				public string actionName
				{
					get
					{
						if (!ReInput.isReady || JwOchbdvhsvHVdSyYjtqEJdTZzYG < 0)
						{
							return null;
						}
						return ReInput.mapping.GetAction(JwOchbdvhsvHVdSyYjtqEJdTZzYG)?.name;
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
							JwOchbdvhsvHVdSyYjtqEJdTZzYG = -1;
						}
						else
						{
							JwOchbdvhsvHVdSyYjtqEJdTZzYG = action.id;
						}
					}
				}

				public Definition()
				{
					JwOchbdvhsvHVdSyYjtqEJdTZzYG = -1;
				}
			}

			[CustomObfuscation(rename = false)]
			internal const int defaultActionId = -1;

			private int JwOchbdvhsvHVdSyYjtqEJdTZzYG = -1;

			public int actionId
			{
				get
				{
					return JwOchbdvhsvHVdSyYjtqEJdTZzYG;
				}
				set
				{
					JwOchbdvhsvHVdSyYjtqEJdTZzYG = value;
				}
			}

			public string actionName
			{
				get
				{
					if (!ReInput.isReady || JwOchbdvhsvHVdSyYjtqEJdTZzYG < 0)
					{
						return null;
					}
					return ReInput.mapping.GetAction(JwOchbdvhsvHVdSyYjtqEJdTZzYG)?.name;
				}
				set
				{
					if (ReInput.isReady)
					{
						InputAction action = ReInput.mapping.GetAction(value);
						if (action == null)
						{
							JwOchbdvhsvHVdSyYjtqEJdTZzYG = -1;
						}
						else
						{
							JwOchbdvhsvHVdSyYjtqEJdTZzYG = action.id;
						}
					}
				}
			}

			internal ElementWithSource(PlayerController P_0, Definition P_1)
				: base(P_0, P_1)
			{
				JwOchbdvhsvHVdSyYjtqEJdTZzYG = P_1.actionId;
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

				internal override Element FRGzoQGxlhWgnLpuxZPVeqiuvlgD(PlayerController P_0)
				{
					return new MouseWheel(P_0, this);
				}
			}

			public new MouseWheelAxis xAxis => pAGcyUYRwJwYiiycWKSARieMOZIo<MouseWheelAxis>(0);

			public new MouseWheelAxis yAxis => pAGcyUYRwJwYiiycWKSARieMOZIo<MouseWheelAxis>(1);

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

				internal override Element FRGzoQGxlhWgnLpuxZPVeqiuvlgD(PlayerController P_0)
				{
					return new MouseWheelAxis(P_0, this);
				}
			}

			[CustomObfuscation(rename = false)]
			internal const float defaultRepeatRate = 4f;

			[CustomObfuscation(rename = false)]
			internal new const AxisCoordinateMode defaultAxisCoordinateMode = AxisCoordinateMode.Relative;

			private const float iNzGtcyWdOaKVPKMmzqbfFEUnOWc = 0.01f;

			private float YSkaBBhJiFXPXTRPaOUbCTlETpio = 0.25f;

			private double cGHxdUbOJdeKDTJHxDXvymTAxcnE;

			private float ckUkzuSDJGuJonmPceZrDLTKWbwc;

			public float repeatRate
			{
				get
				{
					if (YSkaBBhJiFXPXTRPaOUbCTlETpio == 0f)
					{
						return 0f;
					}
					return 1f / YSkaBBhJiFXPXTRPaOUbCTlETpio;
				}
				set
				{
					if (value < 0f)
					{
						value = 0f;
					}
					if (value == 0f)
					{
						YSkaBBhJiFXPXTRPaOUbCTlETpio = 0f;
					}
					else
					{
						YSkaBBhJiFXPXTRPaOUbCTlETpio = 1f / value;
					}
				}
			}

			public override float value
			{
				get
				{
					if (!base.selfAndParentEnabled)
					{
						return 0f;
					}
					return ckUkzuSDJGuJonmPceZrDLTKWbwc;
				}
			}

			internal MouseWheelAxis(PlayerController P_0, Definition P_1)
				: base(P_0, P_1)
			{
				repeatRate = P_1.repeatRate;
			}

			internal override void jRaYtHNVcykNMAbqOnSGaKIIGSEaA()
			{
				base.jRaYtHNVcykNMAbqOnSGaKIIGSEaA();
				if (base.selfAndParentEnabled)
				{
					ckUkzuSDJGuJonmPceZrDLTKWbwc = cnPMTOtfmVCjsekZygybCvfqRwyMA();
				}
			}

			protected override void EnabledStateChanged(bool state)
			{
				base.EnabledStateChanged(state);
				if (!state)
				{
					SPGTRPyvIslcMdbPTItsewSLRPxx();
				}
			}

			private float cnPMTOtfmVCjsekZygybCvfqRwyMA()
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
					else if (base.player.GetNegativeButtonDown(base.actionId))
					{
						flag = true;
						num = -1f;
					}
					if (!flag && ReInput.unscaledTime < cGHxdUbOJdeKDTJHxDXvymTAxcnE + (double)YSkaBBhJiFXPXTRPaOUbCTlETpio)
					{
						return 0f;
					}
					if (Mathf.Abs(num) <= 0.01f)
					{
						return 0f;
					}
					num = Mathf.Sign(num);
					num *= base.absoluteToRelativeSensitivity;
					cGHxdUbOJdeKDTJHxDXvymTAxcnE = ReInput.unscaledTime;
					break;
				}
				}
				return num;
			}

			private void SPGTRPyvIslcMdbPTItsewSLRPxx()
			{
				ckUkzuSDJGuJonmPceZrDLTKWbwc = 0f;
				cGHxdUbOJdeKDTJHxDXvymTAxcnE = 0.0;
			}
		}

		internal readonly int QajDFFaomlkLHzaostfWYpGUioys;

		private bool kKFZZElqKQSUFMZnWdEudRvTJGpo;

		private int DCGJXzigEdTcwzFynDKUcORiHGSOA;

		private readonly AList<Element> hObmyuQkxgNXDiBtSEwFNgLuBsoZ;

		private readonly AList<Button> YXuCILbbSuGPNMGgRTZoPeQmcMPsA;

		private readonly AList<Axis> gmjcdmbtDXKatrtMKrVjuasYTyOh;

		private readonly ReadOnlyCollection<Element> VewvmtxDhBKjfOztXaGoFFLyxNDV;

		private readonly ReadOnlyCollection<Button> tnwAAWkpEQQDOblRgOpzbinWPPbqc;

		private readonly ReadOnlyCollection<Axis> cBSHMOCbWzIiiaZpZEDfrfjPCNNab;

		private readonly List<Element.znbsAbXzumooNTekBhsPkirgaxEt> FWgVsHaMpaDmKWXOXIqOajTcbGyhA;

		private Action<int, bool> fJujbQHHjdTObWpjlVIDpdfssDBK;

		private Action<int, float> QNrCqvatavEJcWTIgbPSkLjfyicxA;

		private Action<bool> eneVgSlIatJCOIDcAjGhRLwviGQy;

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
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				}
				else
				{
					if (kKFZZElqKQSUFMZnWdEudRvTJGpo == value)
					{
						return;
					}
					if (!value)
					{
						ClearVars();
					}
					kKFZZElqKQSUFMZnWdEudRvTJGpo = value;
					for (int i = 0; i < hObmyuQkxgNXDiBtSEwFNgLuBsoZ._count; i++)
					{
						hObmyuQkxgNXDiBtSEwFNgLuBsoZ[i].enabled = value;
					}
					if (eneVgSlIatJCOIDcAjGhRLwviGQy != null)
					{
						try
						{
							eneVgSlIatJCOIDcAjGhRLwviGQy(value);
						}
						catch (Exception ex)
						{
							Logger.LogError("An exception occurred in a listener of EnabledStateChangedEvent. This means an exception was thrown by your code.\n" + ex);
						}
					}
				}
			}
		}

		public int playerId
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return -1;
				}
				return DCGJXzigEdTcwzFynDKUcORiHGSOA;
			}
			set
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				}
				else if (DCGJXzigEdTcwzFynDKUcORiHGSOA != value)
				{
					DCGJXzigEdTcwzFynDKUcORiHGSOA = value;
					ClearVars();
				}
			}
		}

		public IList<Button> buttons
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return null;
				}
				return tnwAAWkpEQQDOblRgOpzbinWPPbqc;
			}
		}

		public IList<Axis> axes
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return null;
				}
				return cBSHMOCbWzIiiaZpZEDfrfjPCNNab;
			}
		}

		public IList<Element> elements
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return null;
				}
				return VewvmtxDhBKjfOztXaGoFFLyxNDV;
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
				if (YXuCILbbSuGPNMGgRTZoPeQmcMPsA == null)
				{
					return 0;
				}
				return YXuCILbbSuGPNMGgRTZoPeQmcMPsA._count;
			}
		}

		public int axisCount
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return 0;
				}
				if (gmjcdmbtDXKatrtMKrVjuasYTyOh == null)
				{
					return 0;
				}
				return gmjcdmbtDXKatrtMKrVjuasYTyOh._count;
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
				if (hObmyuQkxgNXDiBtSEwFNgLuBsoZ == null)
				{
					return 0;
				}
				return hObmyuQkxgNXDiBtSEwFNgLuBsoZ._count;
			}
		}

		internal Player XAbNzJSvSNjTFwhnJridEdiVWtEM
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

		public event Action<int, bool> ButtonStateChangedEvent
		{
			add
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				}
				else
				{
					fJujbQHHjdTObWpjlVIDpdfssDBK = (Action<int, bool>)Delegate.Combine(fJujbQHHjdTObWpjlVIDpdfssDBK, value);
				}
			}
			remove
			{
				fJujbQHHjdTObWpjlVIDpdfssDBK = (Action<int, bool>)Delegate.Remove(fJujbQHHjdTObWpjlVIDpdfssDBK, value);
			}
		}

		public event Action<int, float> AxisValueChangedEvent
		{
			add
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				}
				else
				{
					QNrCqvatavEJcWTIgbPSkLjfyicxA = (Action<int, float>)Delegate.Combine(QNrCqvatavEJcWTIgbPSkLjfyicxA, value);
				}
			}
			remove
			{
				QNrCqvatavEJcWTIgbPSkLjfyicxA = (Action<int, float>)Delegate.Remove(QNrCqvatavEJcWTIgbPSkLjfyicxA, value);
			}
		}

		public event Action<bool> EnabledStateChangedEvent
		{
			add
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				}
				else
				{
					eneVgSlIatJCOIDcAjGhRLwviGQy = (Action<bool>)Delegate.Combine(eneVgSlIatJCOIDcAjGhRLwviGQy, value);
				}
			}
			remove
			{
				eneVgSlIatJCOIDcAjGhRLwviGQy = (Action<bool>)Delegate.Remove(eneVgSlIatJCOIDcAjGhRLwviGQy, value);
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
			QajDFFaomlkLHzaostfWYpGUioys = ReInput._id;
			DCGJXzigEdTcwzFynDKUcORiHGSOA = P_0.playerId;
			kKFZZElqKQSUFMZnWdEudRvTJGpo = P_0.enabled;
			List<Element> list = new List<Element>();
			List<Element> list2 = new List<Element>();
			List<Button> list3 = new List<Button>();
			List<Axis> list4 = new List<Axis>();
			foreach (Element.Definition element in P_0.elements)
			{
				JymOYvJCaUZfrfNvqSliYMwhgCGz(element.FRGzoQGxlhWgnLpuxZPVeqiuvlgD(this), list, list2, list3, list4);
			}
			list.AddRange(list2);
			hObmyuQkxgNXDiBtSEwFNgLuBsoZ = new AList<Element>(list);
			YXuCILbbSuGPNMGgRTZoPeQmcMPsA = new AList<Button>(list3);
			gmjcdmbtDXKatrtMKrVjuasYTyOh = new AList<Axis>(list4);
			VewvmtxDhBKjfOztXaGoFFLyxNDV = new ReadOnlyCollection<Element>(hObmyuQkxgNXDiBtSEwFNgLuBsoZ);
			tnwAAWkpEQQDOblRgOpzbinWPPbqc = new ReadOnlyCollection<Button>(YXuCILbbSuGPNMGgRTZoPeQmcMPsA);
			cBSHMOCbWzIiiaZpZEDfrfjPCNNab = new ReadOnlyCollection<Axis>(gmjcdmbtDXKatrtMKrVjuasYTyOh);
			FWgVsHaMpaDmKWXOXIqOajTcbGyhA = new List<Element.znbsAbXzumooNTekBhsPkirgaxEt>();
			ReInput.UpdateEndedEvent += ZCGETbjMQZUkyflRtYAqwQNUBPQIb;
		}

		~PlayerController()
		{
			ReInput.UpdateEndedEvent -= ZCGETbjMQZUkyflRtYAqwQNUBPQIb;
		}

		public bool GetButton(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if ((uint)index >= (uint)YXuCILbbSuGPNMGgRTZoPeQmcMPsA._count)
			{
				return false;
			}
			return YXuCILbbSuGPNMGgRTZoPeQmcMPsA[index].value;
		}

		public bool GetButtonDown(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if ((uint)index >= (uint)YXuCILbbSuGPNMGgRTZoPeQmcMPsA._count)
			{
				return false;
			}
			return YXuCILbbSuGPNMGgRTZoPeQmcMPsA[index].justPressed;
		}

		public bool GetButtonUp(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if ((uint)index >= (uint)YXuCILbbSuGPNMGgRTZoPeQmcMPsA._count)
			{
				return false;
			}
			return YXuCILbbSuGPNMGgRTZoPeQmcMPsA[index].justReleased;
		}

		public float GetAxis(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0f;
			}
			if ((uint)index >= (uint)gmjcdmbtDXKatrtMKrVjuasYTyOh._count)
			{
				return 0f;
			}
			return gmjcdmbtDXKatrtMKrVjuasYTyOh[index].value;
		}

		public float GetAxisRaw(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0f;
			}
			if ((uint)index >= (uint)gmjcdmbtDXKatrtMKrVjuasYTyOh._count)
			{
				return 0f;
			}
			return gmjcdmbtDXKatrtMKrVjuasYTyOh[index].valueRaw;
		}

		public Element GetElement(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			if ((uint)index >= (uint)gmjcdmbtDXKatrtMKrVjuasYTyOh._count)
			{
				return null;
			}
			return hObmyuQkxgNXDiBtSEwFNgLuBsoZ[index];
		}

		public T GetElement<T>(int index) where T : Element
		{
			return GetElement(index) as T;
		}

		private void ZCGETbjMQZUkyflRtYAqwQNUBPQIb(UpdateLoopType P_0)
		{
			Update(P_0);
			UpdateFinished();
		}

		protected virtual bool Update(UpdateLoopType updateLoop)
		{
			if (!kKFZZElqKQSUFMZnWdEudRvTJGpo)
			{
				return false;
			}
			bool flag = QNrCqvatavEJcWTIgbPSkLjfyicxA != null;
			bool flag2 = fJujbQHHjdTObWpjlVIDpdfssDBK != null;
			for (int i = 0; i < hObmyuQkxgNXDiBtSEwFNgLuBsoZ._count; i++)
			{
				float num = 0f;
				if (flag && hObmyuQkxgNXDiBtSEwFNgLuBsoZ[i] is Axis)
				{
					Axis axis = hObmyuQkxgNXDiBtSEwFNgLuBsoZ[i] as Axis;
					num = ((axis.coordinateMode != AxisCoordinateMode.Absolute) ? 0f : axis.value);
				}
				hObmyuQkxgNXDiBtSEwFNgLuBsoZ[i].jRaYtHNVcykNMAbqOnSGaKIIGSEaA();
				if (flag2 && hObmyuQkxgNXDiBtSEwFNgLuBsoZ[i] is Button)
				{
					Button button = hObmyuQkxgNXDiBtSEwFNgLuBsoZ[i] as Button;
					if (button.justPressed && button.value)
					{
						FWgVsHaMpaDmKWXOXIqOajTcbGyhA.Add(new Element.znbsAbXzumooNTekBhsPkirgaxEt(ControllerElementType.Button, i, 1f));
					}
					else if (button.justReleased && !button.value)
					{
						FWgVsHaMpaDmKWXOXIqOajTcbGyhA.Add(new Element.znbsAbXzumooNTekBhsPkirgaxEt(ControllerElementType.Button, i, 0f));
					}
				}
				else if (flag && hObmyuQkxgNXDiBtSEwFNgLuBsoZ[i] is Axis)
				{
					FWgVsHaMpaDmKWXOXIqOajTcbGyhA.Add(new Element.znbsAbXzumooNTekBhsPkirgaxEt(ControllerElementType.Axis, i, (hObmyuQkxgNXDiBtSEwFNgLuBsoZ[i] as Axis).value - num));
				}
			}
			return true;
		}

		protected virtual void UpdateFinished()
		{
			int count = FWgVsHaMpaDmKWXOXIqOajTcbGyhA.Count;
			if (count <= 0)
			{
				return;
			}
			for (int i = 0; i < count; i++)
			{
				Element.znbsAbXzumooNTekBhsPkirgaxEt znbsAbXzumooNTekBhsPkirgaxEt = FWgVsHaMpaDmKWXOXIqOajTcbGyhA[i];
				if (znbsAbXzumooNTekBhsPkirgaxEt.MdjBbHAfEpOIyysKWtDJwilBqwCjA == ControllerElementType.Button)
				{
					try
					{
						fJujbQHHjdTObWpjlVIDpdfssDBK(znbsAbXzumooNTekBhsPkirgaxEt.PqXTkiKXdxRLgXpucsKUqWEJhHUQ, znbsAbXzumooNTekBhsPkirgaxEt.yYOUbwIbBcyPAQvjeAEXVsjzLAln > 0f);
					}
					catch (Exception ex)
					{
						Logger.LogError("An exception occurred in a listener of ButtonStateChangedEvent. This means an exception was thrown by your code.\n" + ex);
					}
				}
				else if (znbsAbXzumooNTekBhsPkirgaxEt.MdjBbHAfEpOIyysKWtDJwilBqwCjA == ControllerElementType.Axis)
				{
					try
					{
						QNrCqvatavEJcWTIgbPSkLjfyicxA(znbsAbXzumooNTekBhsPkirgaxEt.PqXTkiKXdxRLgXpucsKUqWEJhHUQ, znbsAbXzumooNTekBhsPkirgaxEt.yYOUbwIbBcyPAQvjeAEXVsjzLAln);
					}
					catch (Exception ex2)
					{
						Logger.LogError("An exception occurred in a listener of AxisValueChangedEvent. This means an exception was thrown by your code.\n" + ex2);
					}
				}
			}
			FWgVsHaMpaDmKWXOXIqOajTcbGyhA.Clear();
		}

		protected virtual void ClearVars()
		{
			FWgVsHaMpaDmKWXOXIqOajTcbGyhA.Clear();
		}

		internal void JymOYvJCaUZfrfNvqSliYMwhgCGz(Element P_0)
		{
			if (P_0 != null)
			{
				if (P_0 is Axis)
				{
					gmjcdmbtDXKatrtMKrVjuasYTyOh.Add(P_0 as Axis);
				}
				else if (P_0 is Button)
				{
					YXuCILbbSuGPNMGgRTZoPeQmcMPsA.Add(P_0 as Button);
				}
				hObmyuQkxgNXDiBtSEwFNgLuBsoZ.Add(P_0);
			}
		}

		private void JymOYvJCaUZfrfNvqSliYMwhgCGz(Element P_0, List<Element> P_1, List<Element> P_2, List<Button> P_3, List<Axis> P_4)
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
					(P_0 as CompoundElement).yfvUZvCErrubvkdbzulBqRmKnLaT(list);
					for (int i = 0; i < list.Count; i++)
					{
						JymOYvJCaUZfrfNvqSliYMwhgCGz(list[i], P_1, P_2, P_3, P_4);
					}
				}
				P_2.Add(P_0);
			}
			else
			{
				Logger.LogWarning("Unknown Element type encountered: " + P_0.GetType());
			}
		}

		internal static int LmWghArlQXRYdjBGSILmEsYWzCwDA<_0001>(IList<_0001> P_0, Predicate<_0001> P_1, int P_2) where _0001 : Element
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
