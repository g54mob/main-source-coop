using System;
using System.Collections.Generic;
using Rewired.UI;
using Rewired.Utils.Classes.Data;
using UnityEngine;

namespace Rewired
{
	public sealed class PlayerMouse : PlayerController, IPlayerController, IPlayerMouse, IMouseInputSource
	{
		public new sealed class Definition : PlayerController.Definition
		{
			public bool defaultToCenter = true;

			public bool clampToMovementArea = true;

			public ScreenRect movementArea = fIHmOZwdmcjhhxjckEtHRKVcffPV;

			public MovementAreaUnit movementAreaUnit;

			public float pointerSpeed = 1f;

			public bool useHardwarePointerPosition = true;

			internal Definition()
			{
			}
		}

		public new static class Factory
		{
			public static PlayerMouse Create()
			{
				return lOzmglNwrCkddSvkHTjSvLufiURJ(3, 3);
			}

			private static PlayerMouse lOzmglNwrCkddSvkHTjSvLufiURJ(int P_0, int P_1)
			{
				if (P_0 < 0)
				{
					P_0 = 0;
				}
				if (P_1 < 0)
				{
					P_1 = 0;
				}
				List<Element.Definition> list = new List<Element.Definition>(P_0 + P_1);
				if (P_1 >= 1)
				{
					list.Add(new MouseAxis2D.Definition
					{
						name = "Movement",
						xAxis = new MouseAxis.Definition
						{
							name = "Horizontal"
						},
						yAxis = new MouseAxis.Definition
						{
							name = "Vertical"
						}
					});
				}
				if (P_1 >= 3)
				{
					list.Add(new MouseWheel.Definition
					{
						name = "Wheel",
						xAxis = new MouseWheelAxis.Definition
						{
							name = "Wheel Horizontal"
						},
						yAxis = new MouseWheelAxis.Definition
						{
							name = "Wheel Vertical"
						}
					});
				}
				for (int i = 4; i < P_1; i++)
				{
					list.Add(new Axis.Definition
					{
						coordinateMode = AxisCoordinateMode.Relative
					});
				}
				if (P_0 >= 1)
				{
					list.Add(new Button.Definition
					{
						name = "Left Button"
					});
				}
				if (P_0 >= 2)
				{
					list.Add(new Button.Definition
					{
						name = "Right Button"
					});
				}
				if (P_0 >= 3)
				{
					list.Add(new Button.Definition
					{
						name = "Middle Button"
					});
				}
				for (int j = 3; j < P_0; j++)
				{
					list.Add(new Button.Definition());
				}
				return new PlayerMouse(new Definition
				{
					elements = list
				});
			}

			public static PlayerMouse Create(Definition definition)
			{
				return new PlayerMouse(definition);
			}
		}

		public enum MovementAreaUnit
		{
			Screen = 0,
			Pixel = 1
		}

		[Serializable]
		private sealed class OSlalNkQgdyCxwlOJLXPaRyIfeNGb
		{
			public static readonly OSlalNkQgdyCxwlOJLXPaRyIfeNGb _003C_003E9 = new OSlalNkQgdyCxwlOJLXPaRyIfeNGb();

			public static Predicate<Axis> _003C_003E9__18_0;

			public static Predicate<Axis> _003C_003E9__18_1;

			internal bool puHFbMbZKRgRrEHnPuZHpJpqxAWQ(Axis P_0)
			{
				if ((object)P_0.GetType() == typeof(MouseWheelAxis))
				{
					return !P_0.RJWetIMScbNkkayUbJoRDyHMCesM;
				}
				return false;
			}

			internal bool REKreJAUTcsuDTPasfcxGOWTYUPN(Axis P_0)
			{
				if ((object)P_0.GetType() == typeof(MouseWheelAxis))
				{
					return !P_0.RJWetIMScbNkkayUbJoRDyHMCesM;
				}
				return false;
			}
		}

		internal const bool crtgmFtaoxGkFJALkMneypaJfbiyA = true;

		internal const float MMQMoBURzBdZBfOjtcIkfEfkHrEOb = 1f;

		internal const bool LzflEobrLLpTJphRiJMslgKKQqtW = true;

		internal const bool UaubgkjmjCUdjbSqoBxQcCBeFCzaA = true;

		internal const MovementAreaUnit fWjLMXkdMrbVvorEgceTaczTnnaCA = MovementAreaUnit.Screen;

		internal static readonly ScreenRect fIHmOZwdmcjhhxjckEtHRKVcffPV = new ScreenRect(0f, 0f, 1f, 1f);

		private const int SJoqyEGVJCuGrikQRqLOaiOnfzEy = 3;

		private const int GUNBIxEurJSTmnKyGWDLWunVbhacb = 3;

		internal const string UBEwMPhRyWhgjZfJODajRYTkhJxs = "Movement";

		internal const string lVzfbDHjIdsxijlEtVFXCigxqwNcA = "Horizontal";

		internal const string UCSlGWzkwqjaLupizGIniUHJEIdaA = "Vertical";

		internal const string YLFBHELRKskAimMMrrXAfAyFFpmKA = "Wheel";

		internal const string dDrOhokZbajufyoDftYgXqwWJoVn = "Wheel Horizontal";

		internal const string afSeorwduFlbtHKKNjdxQHAhqnTD = "Wheel Vertical";

		internal const string nhUJEDdiQsIbcGpOsANfefKxnMRT = "Left Button";

		internal const string qWQgTKqBQhGnlAHZJvMzvUPiGZsRA = "Right Button";

		internal const string MwHiTwfJjckLcVTHAoDvFCfriYBE = "Middle Button";

		private readonly int hxwwgxFnUtCslUpZOgPZCzFqFiqTA = -1;

		private readonly int DfEbwMtWVLWsSBqreuwzeVrQSwSl = -1;

		private readonly int vNbVHcuTotuEFGaawHzjVdfyCmuy = -1;

		private readonly int fDcRizljsZXtisTJjJvDCnfVRvUD = -1;

		private readonly int TxNNxPSzkkcdkUCsYUxErSslcylE = -1;

		private readonly int CLpkKlcRLHDYaVHsUTnEbULSfghR = -1;

		private bool fyHlDWcNNzDNgJlSyUKoyrsWGWGe;

		private Vector2 oxrDIMticzxeKaxrOYeUhCepuoyF;

		private Vector2 pplLsqMoYgzVgPCIqghiiSHWqzGt;

		private Vector2 JHlQdvIDknwhbpDkONoyRzgqysDj;

		private Vector2 NNeHrvYGutiOIXhlhFaJfwMWTlqw;

		private Vector2 QBcPXjdeXZuzabhNCsEDWKxPBdte;

		private float GDHSElRvsGdWdBmtMLHmEKBiKTpC;

		private bool HdRGQgJBdNkpNiMbgdWNiHTQRrDaB;

		private Action<Vector2> rnnZqqYfwFdqnJnqPRsOnSOuchohb;

		private bool mBgpERXzEyTjbWeGHFvSbaUoJveW;

		private ScreenRect jJwbONnbKvXKWbnpYndOAguJcFwjA;

		private bool plrzNanNFfWnyYQasrBNsxGHRJzg;

		private MovementAreaUnit GmCwmKihHCIrRGKDcEPfvjNWacpx;

		public bool defaultToCenter
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return false;
				}
				return mBgpERXzEyTjbWeGHFvSbaUoJveW;
			}
			set
			{
				mBgpERXzEyTjbWeGHFvSbaUoJveW = value;
			}
		}

		public bool clampToMovementArea
		{
			get
			{
				return plrzNanNFfWnyYQasrBNsxGHRJzg;
			}
			set
			{
				plrzNanNFfWnyYQasrBNsxGHRJzg = value;
			}
		}

		public ScreenRect movementArea
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return default(ScreenRect);
				}
				return jJwbONnbKvXKWbnpYndOAguJcFwjA;
			}
			set
			{
				jJwbONnbKvXKWbnpYndOAguJcFwjA = value;
			}
		}

		public MovementAreaUnit movementAreaUnit
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return MovementAreaUnit.Screen;
				}
				return GmCwmKihHCIrRGKDcEPfvjNWacpx;
			}
			set
			{
				GmCwmKihHCIrRGKDcEPfvjNWacpx = value;
			}
		}

		public Vector2 screenPosition
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return Vector2.zero;
				}
				if (!base.enabled)
				{
					return Vector2.zero;
				}
				return JHlQdvIDknwhbpDkONoyRzgqysDj;
			}
			set
			{
				vtzxdsmrfDcFGyGcdkIRsgZqTqgO(value);
			}
		}

		public Vector2 screenPositionPrev
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return Vector2.zero;
				}
				if (!base.enabled)
				{
					return Vector2.zero;
				}
				return NNeHrvYGutiOIXhlhFaJfwMWTlqw;
			}
		}

		public Vector2 screenPositionDelta
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return Vector2.zero;
				}
				if (!base.enabled)
				{
					return Vector2.zero;
				}
				return QBcPXjdeXZuzabhNCsEDWKxPBdte;
			}
		}

		public MouseAxis xAxis
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return null;
				}
				if (DfEbwMtWVLWsSBqreuwzeVrQSwSl < 0)
				{
					return null;
				}
				return (MouseAxis)base.axes[DfEbwMtWVLWsSBqreuwzeVrQSwSl];
			}
		}

		public MouseAxis yAxis
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return null;
				}
				if (vNbVHcuTotuEFGaawHzjVdfyCmuy < 0)
				{
					return null;
				}
				return (MouseAxis)base.axes[vNbVHcuTotuEFGaawHzjVdfyCmuy];
			}
		}

		public MouseWheel wheel
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return null;
				}
				if (hxwwgxFnUtCslUpZOgPZCzFqFiqTA < 0)
				{
					return null;
				}
				return (MouseWheel)base.elements[hxwwgxFnUtCslUpZOgPZCzFqFiqTA];
			}
		}

		public Button leftButton
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return null;
				}
				if (fDcRizljsZXtisTJjJvDCnfVRvUD < 0)
				{
					return null;
				}
				return base.buttons[fDcRizljsZXtisTJjJvDCnfVRvUD];
			}
		}

		public Button rightButton
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return null;
				}
				if (TxNNxPSzkkcdkUCsYUxErSslcylE < 0)
				{
					return null;
				}
				return base.buttons[TxNNxPSzkkcdkUCsYUxErSslcylE];
			}
		}

		public Button middleButton
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return null;
				}
				if (CLpkKlcRLHDYaVHsUTnEbULSfghR < 0)
				{
					return null;
				}
				return base.buttons[CLpkKlcRLHDYaVHsUTnEbULSfghR];
			}
		}

		public float pointerSpeed
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return 0f;
				}
				return GDHSElRvsGdWdBmtMLHmEKBiKTpC;
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
					value = 0f;
				}
				GDHSElRvsGdWdBmtMLHmEKBiKTpC = value;
			}
		}

		public bool useHardwarePointerPosition
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return false;
				}
				return HdRGQgJBdNkpNiMbgdWNiHTQRrDaB;
			}
			set
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return;
				}
				HdRGQgJBdNkpNiMbgdWNiHTQRrDaB = value;
				if (!value)
				{
					PBiXClCdYpXIpnoPsGXhDsFMbZNGb();
				}
			}
		}

		bool IMouseInputSource.enabled => base.enabled;

		Vector2 IMouseInputSource.screenPosition => JHlQdvIDknwhbpDkONoyRzgqysDj;

		Vector2 IMouseInputSource.screenPositionDelta => QBcPXjdeXZuzabhNCsEDWKxPBdte;

		Vector2 IMouseInputSource.wheelDelta
		{
			get
			{
				if (wheel == null)
				{
					return Vector2.zero;
				}
				return wheel.value;
			}
		}

		bool IMouseInputSource.locked => false;

		public event Action<Vector2> ScreenPositionChangedEvent
		{
			add
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				}
				else
				{
					rnnZqqYfwFdqnJnqPRsOnSOuchohb = (Action<Vector2>)Delegate.Combine(rnnZqqYfwFdqnJnqPRsOnSOuchohb, value);
				}
			}
			remove
			{
				rnnZqqYfwFdqnJnqPRsOnSOuchohb = (Action<Vector2>)Delegate.Remove(rnnZqqYfwFdqnJnqPRsOnSOuchohb, value);
			}
		}

		private PlayerMouse(Definition P_0)
			: base(P_0)
		{
			mBgpERXzEyTjbWeGHFvSbaUoJveW = P_0.defaultToCenter;
			plrzNanNFfWnyYQasrBNsxGHRJzg = P_0.clampToMovementArea;
			jJwbONnbKvXKWbnpYndOAguJcFwjA = P_0.movementArea;
			GmCwmKihHCIrRGKDcEPfvjNWacpx = P_0.movementAreaUnit;
			GDHSElRvsGdWdBmtMLHmEKBiKTpC = P_0.pointerSpeed;
			HdRGQgJBdNkpNiMbgdWNiHTQRrDaB = P_0.useHardwarePointerPosition;
			int num = base.elementCount;
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < num; i++)
			{
				if (num2 < 2 && (object)base.elements[i].GetType() == typeof(MouseAxis))
				{
					if (num2 == 0)
					{
						DfEbwMtWVLWsSBqreuwzeVrQSwSl = base.axes.IndexOf((MouseAxis)base.elements[i]);
					}
					else
					{
						vNbVHcuTotuEFGaawHzjVdfyCmuy = base.axes.IndexOf((MouseAxis)base.elements[i]);
					}
					num2++;
				}
				else if (hxwwgxFnUtCslUpZOgPZCzFqFiqTA < 0 && base.elements[i] is MouseWheel)
				{
					hxwwgxFnUtCslUpZOgPZCzFqFiqTA = i;
				}
				else if (num3 < 3 && (object)base.elements[i].GetType() == typeof(Button))
				{
					switch (num3)
					{
					case 0:
						fDcRizljsZXtisTJjJvDCnfVRvUD = base.buttons.IndexOf((Button)base.elements[i]);
						break;
					case 1:
						TxNNxPSzkkcdkUCsYUxErSslcylE = base.buttons.IndexOf((Button)base.elements[i]);
						break;
					case 2:
						CLpkKlcRLHDYaVHsUTnEbULSfghR = base.buttons.IndexOf((Button)base.elements[i]);
						break;
					}
					num3++;
				}
			}
			if (hxwwgxFnUtCslUpZOgPZCzFqFiqTA < 0)
			{
				int num4 = PlayerController.LmWghArlQXRYdjBGSILmEsYWzCwDA(base.axes, OSlalNkQgdyCxwlOJLXPaRyIfeNGb._003C_003E9.puHFbMbZKRgRrEHnPuZHpJpqxAWQ, 1);
				int num5 = PlayerController.LmWghArlQXRYdjBGSILmEsYWzCwDA(base.axes, OSlalNkQgdyCxwlOJLXPaRyIfeNGb._003C_003E9.REKreJAUTcsuDTPasfcxGOWTYUPN, 2);
				if (num4 >= 0 || num5 >= 0)
				{
					MouseWheel mouseWheel = new MouseWheel(this, new MouseWheel.Definition
					{
						name = "Wheel"
					});
					JymOYvJCaUZfrfNvqSliYMwhgCGz(mouseWheel);
					hxwwgxFnUtCslUpZOgPZCzFqFiqTA = base.elements.Count - 1;
					if (num4 < 0 || num5 < 0)
					{
						Element element = new MouseWheelAxis(this, new MouseWheelAxis.Definition
						{
							name = "Wheel Horizontal",
							coordinateMode = AxisCoordinateMode.Relative
						});
						JymOYvJCaUZfrfNvqSliYMwhgCGz(element);
						mouseWheel.JymOYvJCaUZfrfNvqSliYMwhgCGz(element);
						mouseWheel.JymOYvJCaUZfrfNvqSliYMwhgCGz((num4 < 0) ? base.axes[num5] : base.axes[num4]);
					}
					else
					{
						mouseWheel.JymOYvJCaUZfrfNvqSliYMwhgCGz(base.axes[num4]);
						mouseWheel.JymOYvJCaUZfrfNvqSliYMwhgCGz(base.axes[num5]);
					}
				}
			}
			if (mBgpERXzEyTjbWeGHFvSbaUoJveW)
			{
				ScreenRect screenRect = NPAuUJFLGVUbDGIXwfwBXFKraBmJ();
				JHlQdvIDknwhbpDkONoyRzgqysDj = new Vector2(screenRect.center.x, screenRect.center.y);
			}
			else
			{
				JHlQdvIDknwhbpDkONoyRzgqysDj = Vector2.zero;
			}
		}

		protected override bool Update(UpdateLoopType updateLoop)
		{
			if (!base.Update(updateLoop))
			{
				return false;
			}
			if (updateLoop != UpdateLoopType.Update)
			{
				return false;
			}
			Player player;
			if (HdRGQgJBdNkpNiMbgdWNiHTQRrDaB && (player = base.XAbNzJSvSNjTFwhnJridEdiVWtEM) != null)
			{
				if (!player.controllers.hasMouse)
				{
					PBiXClCdYpXIpnoPsGXhDsFMbZNGb();
				}
				else
				{
					oxrDIMticzxeKaxrOYeUhCepuoyF = ReInput.controllers.Mouse.screenPosition;
					if (oxrDIMticzxeKaxrOYeUhCepuoyF.x != pplLsqMoYgzVgPCIqghiiSHWqzGt.x || oxrDIMticzxeKaxrOYeUhCepuoyF.y != pplLsqMoYgzVgPCIqghiiSHWqzGt.y)
					{
						JHlQdvIDknwhbpDkONoyRzgqysDj.x = oxrDIMticzxeKaxrOYeUhCepuoyF.x;
						JHlQdvIDknwhbpDkONoyRzgqysDj.y = oxrDIMticzxeKaxrOYeUhCepuoyF.y;
					}
					pplLsqMoYgzVgPCIqghiiSHWqzGt.x = oxrDIMticzxeKaxrOYeUhCepuoyF.x;
					pplLsqMoYgzVgPCIqghiiSHWqzGt.y = oxrDIMticzxeKaxrOYeUhCepuoyF.y;
				}
			}
			if (DfEbwMtWVLWsSBqreuwzeVrQSwSl >= 0)
			{
				JHlQdvIDknwhbpDkONoyRzgqysDj.x = kRkyQqefvvPbnHhOJTSOYsKxiTXc(base.axes[DfEbwMtWVLWsSBqreuwzeVrQSwSl], JHlQdvIDknwhbpDkONoyRzgqysDj.x, GDHSElRvsGdWdBmtMLHmEKBiKTpC);
			}
			if (vNbVHcuTotuEFGaawHzjVdfyCmuy >= 0)
			{
				JHlQdvIDknwhbpDkONoyRzgqysDj.y = kRkyQqefvvPbnHhOJTSOYsKxiTXc(base.axes[vNbVHcuTotuEFGaawHzjVdfyCmuy], JHlQdvIDknwhbpDkONoyRzgqysDj.y, GDHSElRvsGdWdBmtMLHmEKBiKTpC);
			}
			vtzxdsmrfDcFGyGcdkIRsgZqTqgO(JHlQdvIDknwhbpDkONoyRzgqysDj);
			QBcPXjdeXZuzabhNCsEDWKxPBdte.x = JHlQdvIDknwhbpDkONoyRzgqysDj.x - NNeHrvYGutiOIXhlhFaJfwMWTlqw.x;
			QBcPXjdeXZuzabhNCsEDWKxPBdte.y = JHlQdvIDknwhbpDkONoyRzgqysDj.y - NNeHrvYGutiOIXhlhFaJfwMWTlqw.y;
			fyHlDWcNNzDNgJlSyUKoyrsWGWGe = JHlQdvIDknwhbpDkONoyRzgqysDj.x != NNeHrvYGutiOIXhlhFaJfwMWTlqw.x || JHlQdvIDknwhbpDkONoyRzgqysDj.y != NNeHrvYGutiOIXhlhFaJfwMWTlqw.y;
			NNeHrvYGutiOIXhlhFaJfwMWTlqw.x = JHlQdvIDknwhbpDkONoyRzgqysDj.x;
			NNeHrvYGutiOIXhlhFaJfwMWTlqw.y = JHlQdvIDknwhbpDkONoyRzgqysDj.y;
			return true;
		}

		protected override void UpdateFinished()
		{
			base.UpdateFinished();
			if (fyHlDWcNNzDNgJlSyUKoyrsWGWGe && rnnZqqYfwFdqnJnqPRsOnSOuchohb != null)
			{
				try
				{
					rnnZqqYfwFdqnJnqPRsOnSOuchohb(JHlQdvIDknwhbpDkONoyRzgqysDj);
				}
				catch (Exception ex)
				{
					Logger.LogError("An exception occurred in a listener of ScreenPositionChangedEvent. This means an exception was thrown by your code.\n" + ex);
				}
				fyHlDWcNNzDNgJlSyUKoyrsWGWGe = false;
			}
		}

		protected override void ClearVars()
		{
			base.ClearVars();
			NNeHrvYGutiOIXhlhFaJfwMWTlqw = JHlQdvIDknwhbpDkONoyRzgqysDj;
			QBcPXjdeXZuzabhNCsEDWKxPBdte = Vector2.zero;
			PBiXClCdYpXIpnoPsGXhDsFMbZNGb();
			fyHlDWcNNzDNgJlSyUKoyrsWGWGe = false;
		}

		private void vtzxdsmrfDcFGyGcdkIRsgZqTqgO(Vector2 P_0)
		{
			if (!plrzNanNFfWnyYQasrBNsxGHRJzg)
			{
				JHlQdvIDknwhbpDkONoyRzgqysDj = P_0;
				return;
			}
			if (GmCwmKihHCIrRGKDcEPfvjNWacpx == MovementAreaUnit.Screen)
			{
				float num = Screen.width;
				float num2 = Screen.height;
				JHlQdvIDknwhbpDkONoyRzgqysDj.x = Mathf.Clamp(P_0.x, jJwbONnbKvXKWbnpYndOAguJcFwjA.xMin * num, jJwbONnbKvXKWbnpYndOAguJcFwjA.xMax * num);
				JHlQdvIDknwhbpDkONoyRzgqysDj.y = Mathf.Clamp(P_0.y, jJwbONnbKvXKWbnpYndOAguJcFwjA.yMin * num2, jJwbONnbKvXKWbnpYndOAguJcFwjA.yMax * num2);
				return;
			}
			if (GmCwmKihHCIrRGKDcEPfvjNWacpx == MovementAreaUnit.Pixel)
			{
				JHlQdvIDknwhbpDkONoyRzgqysDj.x = Mathf.Clamp(P_0.x, jJwbONnbKvXKWbnpYndOAguJcFwjA.xMin, jJwbONnbKvXKWbnpYndOAguJcFwjA.xMax);
				JHlQdvIDknwhbpDkONoyRzgqysDj.y = Mathf.Clamp(P_0.y, jJwbONnbKvXKWbnpYndOAguJcFwjA.yMin, jJwbONnbKvXKWbnpYndOAguJcFwjA.yMax);
				return;
			}
			throw new NotImplementedException();
		}

		private ScreenRect NPAuUJFLGVUbDGIXwfwBXFKraBmJ()
		{
			if (GmCwmKihHCIrRGKDcEPfvjNWacpx == MovementAreaUnit.Screen)
			{
				return new ScreenRect(jJwbONnbKvXKWbnpYndOAguJcFwjA.xMin * (float)Screen.width, jJwbONnbKvXKWbnpYndOAguJcFwjA.yMin * (float)Screen.height, jJwbONnbKvXKWbnpYndOAguJcFwjA.width * (float)Screen.width, jJwbONnbKvXKWbnpYndOAguJcFwjA.height * (float)Screen.height);
			}
			if (GmCwmKihHCIrRGKDcEPfvjNWacpx == MovementAreaUnit.Pixel)
			{
				return jJwbONnbKvXKWbnpYndOAguJcFwjA;
			}
			throw new NotImplementedException();
		}

		private void PBiXClCdYpXIpnoPsGXhDsFMbZNGb()
		{
			oxrDIMticzxeKaxrOYeUhCepuoyF = Vector2.zero;
			pplLsqMoYgzVgPCIqghiiSHWqzGt = Vector2.zero;
		}

		private static float kRkyQqefvvPbnHhOJTSOYsKxiTXc(Axis P_0, float P_1, float P_2)
		{
			if (P_0 == null)
			{
				return P_1;
			}
			return P_0.coordinateMode switch
			{
				AxisCoordinateMode.Absolute => P_0.value, 
				AxisCoordinateMode.Relative => P_1 + P_0.value * P_2, 
				_ => throw new NotImplementedException(), 
			};
		}

		bool IMouseInputSource.GetButtonDown(int button)
		{
			return GetButtonDown(button);
		}

		bool IMouseInputSource.GetButtonUp(int button)
		{
			return GetButtonUp(button);
		}

		bool IMouseInputSource.GetButton(int button)
		{
			return GetButton(button);
		}
	}
}
