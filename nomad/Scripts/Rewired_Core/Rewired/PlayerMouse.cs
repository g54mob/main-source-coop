using System;
using System.Collections.Generic;
using Rewired.UI;
using Rewired.Utils.Classes.Data;
using UnityEngine;

namespace Rewired
{
	public sealed class PlayerMouse : PlayerController, IPlayerMouse, IPlayerController, IMouseInputSource
	{
		public new sealed class Definition : PlayerController.Definition
		{
			public bool defaultToCenter = true;

			public bool clampToMovementArea = true;

			public ScreenRect movementArea = MxAQcROZNsWNprUJWAJBemmiPUbM;

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
				return BtxgrQizNtrpWfRAcfVZjvdiBOsDb(3, 3);
			}

			private static PlayerMouse BtxgrQizNtrpWfRAcfVZjvdiBOsDb(int P_0, int P_1)
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
		private sealed class uJESReeJaBteVJvstZPHovVuyMmb
		{
			public static readonly uJESReeJaBteVJvstZPHovVuyMmb _003C_003E9 = new uJESReeJaBteVJvstZPHovVuyMmb();

			public static Predicate<Axis> _003C_003E9__18_0;

			public static Predicate<Axis> _003C_003E9__18_1;

			internal bool yEfeKMWvpNXsTMijZVTkdQPocTOAA(Axis P_0)
			{
				if ((object)P_0.GetType() == typeof(MouseWheelAxis))
				{
					return !P_0.ETenJLdSoKJHMOZGxsJdytHmFzBf;
				}
				return false;
			}

			internal bool TfMuuABiHWObHiRCiCcBotbohJru(Axis P_0)
			{
				if ((object)P_0.GetType() == typeof(MouseWheelAxis))
				{
					return !P_0.ETenJLdSoKJHMOZGxsJdytHmFzBf;
				}
				return false;
			}
		}

		internal const bool xGTnoAUbzpdDaAhXkIDZJOiOlLpnA = true;

		internal const float UkUBNEDvcQQMYCvEmXnwAgcilyFlA = 1f;

		internal const bool IYEWiXFsqlnfMQWDhLFeSBZUMuxI = true;

		internal const bool mFTfqRJhdDFGEjjCvpSFfbAaGwcfA = true;

		internal const MovementAreaUnit IdCNksaJkweepdpxBxfztodFULLWA = MovementAreaUnit.Screen;

		internal static readonly ScreenRect MxAQcROZNsWNprUJWAJBemmiPUbM = new ScreenRect(0f, 0f, 1f, 1f);

		private const int TcSbaXGGAVqdsZaxgBwWGbDxwNdT = 3;

		private const int ocGSZpkwktBwsOqaPQsEuBLYktTm = 3;

		internal const string SpCpLnkrVtsNTByvQjtyInYhFyFN = "Movement";

		internal const string lKdfLMdwQyhQSSVGaOLweFYuXBmSA = "Horizontal";

		internal const string LVWXrLsTdUkXjSMOclKPiwmInHSW = "Vertical";

		internal const string fLHSJIZDviIWanczPOqdTJVOBVTC = "Wheel";

		internal const string GHZTPdgTECOqcJMZNkCaExGPIIzL = "Wheel Horizontal";

		internal const string MJdlzzgaIKErZgZWFocbuuTLpPKr = "Wheel Vertical";

		internal const string GvBsbQRENLfZsmGfsBwJZGlSzDbW = "Left Button";

		internal const string jtoJABySuIQhoRBpWdZcTmzbSVbY = "Right Button";

		internal const string oKhtrcKmioGocbfxbIcaeCgbCncr = "Middle Button";

		private readonly int dmJwUXEPEXdiDHGgZFaxoSbIBsxz = -1;

		private readonly int OoUyLvYkdGKsBuZWpTSclJmMRAuB = -1;

		private readonly int HBtjPwDHkfeXrjRpChpLjeLDyveB = -1;

		private readonly int dCwnvMfCiaXWScQZBBpBamxyTKrH = -1;

		private readonly int lTQimkrquCmFYRsnQPCUpldcPFqD = -1;

		private readonly int DJxIjUrdRztKokhGLgMJZgZlvlAp = -1;

		private bool kJOiPdfosyhDqeHoqWEhtXgbLlbX;

		private Vector2 TYdYdITwdAqTgwuMJQIHaONFfaek;

		private Vector2 GkGfSqVQYHeSucTlgegxadmsxyNdb;

		private Vector2 vdccOITqmnFqYOMLmxBoFxsNPWUu;

		private Vector2 VrKvjyZBxmMHwIjErsJxqbWnVrOK;

		private Vector2 HygApUuTRjCmeiltQHvBPdPtSJVK;

		private float nVvJVUJDpKFJEeKtFCqYURptlijCA;

		private bool DcZhjrgpBCRAkimTIcRJWaziETLuA;

		private Action<Vector2> PLuebwDisDKbcQxRZEmFmlSZJNPYA;

		private bool bGtdVFESOnDfMTZBXiNEHpavmeRCb;

		private ScreenRect UzCWywvBSYGGbAiTJpqtDKgbnfMXb;

		private bool zOYRmfGxdsJtZHUyvQqHLyfLUFiu;

		private MovementAreaUnit pzskApWCRmADopNaqHLaSAABreBI;

		bool IPlayerMouse.defaultToCenter
		{
			get
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
					return false;
				}
				return bGtdVFESOnDfMTZBXiNEHpavmeRCb;
			}
			set
			{
				bGtdVFESOnDfMTZBXiNEHpavmeRCb = value;
			}
		}

		public bool clampToMovementArea
		{
			get
			{
				return zOYRmfGxdsJtZHUyvQqHLyfLUFiu;
			}
			set
			{
				zOYRmfGxdsJtZHUyvQqHLyfLUFiu = value;
			}
		}

		ScreenRect IPlayerMouse.movementArea
		{
			get
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
					return default(ScreenRect);
				}
				return UzCWywvBSYGGbAiTJpqtDKgbnfMXb;
			}
			set
			{
				UzCWywvBSYGGbAiTJpqtDKgbnfMXb = value;
			}
		}

		MovementAreaUnit IPlayerMouse.movementAreaUnit
		{
			get
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
					return MovementAreaUnit.Screen;
				}
				return pzskApWCRmADopNaqHLaSAABreBI;
			}
			set
			{
				pzskApWCRmADopNaqHLaSAABreBI = value;
			}
		}

		Vector2 IPlayerMouse.screenPosition
		{
			get
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
					return Vector2.zero;
				}
				if (!base.Rewired_002EIPlayerController_002Eenabled)
				{
					return Vector2.zero;
				}
				return vdccOITqmnFqYOMLmxBoFxsNPWUu;
			}
			set
			{
				ffdWUCxRlAxhLfvaBOPchOEKYykO(value);
			}
		}

		Vector2 IPlayerMouse.screenPositionPrev
		{
			get
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
					return Vector2.zero;
				}
				if (!base.Rewired_002EIPlayerController_002Eenabled)
				{
					return Vector2.zero;
				}
				return VrKvjyZBxmMHwIjErsJxqbWnVrOK;
			}
		}

		Vector2 IPlayerMouse.screenPositionDelta
		{
			get
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
					return Vector2.zero;
				}
				if (!base.Rewired_002EIPlayerController_002Eenabled)
				{
					return Vector2.zero;
				}
				return HygApUuTRjCmeiltQHvBPdPtSJVK;
			}
		}

		MouseAxis IPlayerMouse.xAxis
		{
			get
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
					return null;
				}
				if (OoUyLvYkdGKsBuZWpTSclJmMRAuB < 0)
				{
					return null;
				}
				return (MouseAxis)base.Rewired_002EIPlayerController_002Eaxes[OoUyLvYkdGKsBuZWpTSclJmMRAuB];
			}
		}

		MouseAxis IPlayerMouse.yAxis
		{
			get
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
					return null;
				}
				if (HBtjPwDHkfeXrjRpChpLjeLDyveB < 0)
				{
					return null;
				}
				return (MouseAxis)base.Rewired_002EIPlayerController_002Eaxes[HBtjPwDHkfeXrjRpChpLjeLDyveB];
			}
		}

		MouseWheel IPlayerMouse.wheel
		{
			get
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
					return null;
				}
				if (dmJwUXEPEXdiDHGgZFaxoSbIBsxz < 0)
				{
					return null;
				}
				return (MouseWheel)base.Rewired_002EIPlayerController_002Eelements[dmJwUXEPEXdiDHGgZFaxoSbIBsxz];
			}
		}

		Button IPlayerMouse.leftButton
		{
			get
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
					return null;
				}
				if (dCwnvMfCiaXWScQZBBpBamxyTKrH < 0)
				{
					return null;
				}
				return base.Rewired_002EIPlayerController_002Ebuttons[dCwnvMfCiaXWScQZBBpBamxyTKrH];
			}
		}

		Button IPlayerMouse.rightButton
		{
			get
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
					return null;
				}
				if (lTQimkrquCmFYRsnQPCUpldcPFqD < 0)
				{
					return null;
				}
				return base.Rewired_002EIPlayerController_002Ebuttons[lTQimkrquCmFYRsnQPCUpldcPFqD];
			}
		}

		Button IPlayerMouse.middleButton
		{
			get
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
					return null;
				}
				if (DJxIjUrdRztKokhGLgMJZgZlvlAp < 0)
				{
					return null;
				}
				return base.Rewired_002EIPlayerController_002Ebuttons[DJxIjUrdRztKokhGLgMJZgZlvlAp];
			}
		}

		float IPlayerMouse.pointerSpeed
		{
			get
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
					return 0f;
				}
				return nVvJVUJDpKFJEeKtFCqYURptlijCA;
			}
			set
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
					return;
				}
				if (value < 0f)
				{
					value = 0f;
				}
				nVvJVUJDpKFJEeKtFCqYURptlijCA = value;
			}
		}

		bool IPlayerMouse.useHardwarePointerPosition
		{
			get
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
					return false;
				}
				return DcZhjrgpBCRAkimTIcRJWaziETLuA;
			}
			set
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
					return;
				}
				DcZhjrgpBCRAkimTIcRJWaziETLuA = value;
				if (!value)
				{
					rXBuYuBKAKSvTehoUkRyttAJGxwJ();
				}
			}
		}

		bool IMouseInputSource.enabled => base.Rewired_002EIPlayerController_002Eenabled;

		Vector2 IMouseInputSource.screenPosition => vdccOITqmnFqYOMLmxBoFxsNPWUu;

		Vector2 IMouseInputSource.screenPositionDelta => HygApUuTRjCmeiltQHvBPdPtSJVK;

		Vector2 IMouseInputSource.wheelDelta
		{
			get
			{
				if (((IPlayerMouse)this).wheel == null)
				{
					return Vector2.zero;
				}
				return ((IPlayerMouse)this).wheel.value;
			}
		}

		bool IMouseInputSource.locked => false;

		event Action<Vector2> IPlayerMouse.ScreenPositionChangedEvent
		{
			add
			{
				if (ReInput._id != EXFEcMcVkijIhrVBqTYMJVvSNSSH)
				{
					ReInput.CheckInitialized(EXFEcMcVkijIhrVBqTYMJVvSNSSH);
				}
				else
				{
					PLuebwDisDKbcQxRZEmFmlSZJNPYA = (Action<Vector2>)Delegate.Combine(PLuebwDisDKbcQxRZEmFmlSZJNPYA, value);
				}
			}
			remove
			{
				PLuebwDisDKbcQxRZEmFmlSZJNPYA = (Action<Vector2>)Delegate.Remove(PLuebwDisDKbcQxRZEmFmlSZJNPYA, value);
			}
		}

		private PlayerMouse(Definition P_0)
			: base(P_0)
		{
			bGtdVFESOnDfMTZBXiNEHpavmeRCb = P_0.defaultToCenter;
			zOYRmfGxdsJtZHUyvQqHLyfLUFiu = P_0.clampToMovementArea;
			UzCWywvBSYGGbAiTJpqtDKgbnfMXb = P_0.movementArea;
			pzskApWCRmADopNaqHLaSAABreBI = P_0.movementAreaUnit;
			nVvJVUJDpKFJEeKtFCqYURptlijCA = P_0.pointerSpeed;
			DcZhjrgpBCRAkimTIcRJWaziETLuA = P_0.useHardwarePointerPosition;
			int num = base.Rewired_002EIPlayerController_002EelementCount;
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < num; i++)
			{
				if (num2 < 2 && (object)base.Rewired_002EIPlayerController_002Eelements[i].GetType() == typeof(MouseAxis))
				{
					if (num2 == 0)
					{
						OoUyLvYkdGKsBuZWpTSclJmMRAuB = base.Rewired_002EIPlayerController_002Eaxes.IndexOf((MouseAxis)base.Rewired_002EIPlayerController_002Eelements[i]);
					}
					else
					{
						HBtjPwDHkfeXrjRpChpLjeLDyveB = base.Rewired_002EIPlayerController_002Eaxes.IndexOf((MouseAxis)base.Rewired_002EIPlayerController_002Eelements[i]);
					}
					num2++;
				}
				else if (dmJwUXEPEXdiDHGgZFaxoSbIBsxz < 0 && base.Rewired_002EIPlayerController_002Eelements[i] is MouseWheel)
				{
					dmJwUXEPEXdiDHGgZFaxoSbIBsxz = i;
				}
				else if (num3 < 3 && (object)base.Rewired_002EIPlayerController_002Eelements[i].GetType() == typeof(Button))
				{
					switch (num3)
					{
					case 0:
						dCwnvMfCiaXWScQZBBpBamxyTKrH = base.Rewired_002EIPlayerController_002Ebuttons.IndexOf((Button)base.Rewired_002EIPlayerController_002Eelements[i]);
						break;
					case 1:
						lTQimkrquCmFYRsnQPCUpldcPFqD = base.Rewired_002EIPlayerController_002Ebuttons.IndexOf((Button)base.Rewired_002EIPlayerController_002Eelements[i]);
						break;
					case 2:
						DJxIjUrdRztKokhGLgMJZgZlvlAp = base.Rewired_002EIPlayerController_002Ebuttons.IndexOf((Button)base.Rewired_002EIPlayerController_002Eelements[i]);
						break;
					}
					num3++;
				}
			}
			if (dmJwUXEPEXdiDHGgZFaxoSbIBsxz < 0)
			{
				int num4 = PlayerController.yWBUvNGgiXQmTtGkmItUCJfuMrUGb(base.Rewired_002EIPlayerController_002Eaxes, uJESReeJaBteVJvstZPHovVuyMmb._003C_003E9.yEfeKMWvpNXsTMijZVTkdQPocTOAA, 1);
				int num5 = PlayerController.yWBUvNGgiXQmTtGkmItUCJfuMrUGb(base.Rewired_002EIPlayerController_002Eaxes, uJESReeJaBteVJvstZPHovVuyMmb._003C_003E9.TfMuuABiHWObHiRCiCcBotbohJru, 2);
				if (num4 >= 0 || num5 >= 0)
				{
					MouseWheel mouseWheel = new MouseWheel(this, new MouseWheel.Definition
					{
						name = "Wheel"
					});
					iSDfvshuzoGnugywoIegMIxXehOfA(mouseWheel);
					dmJwUXEPEXdiDHGgZFaxoSbIBsxz = base.Rewired_002EIPlayerController_002Eelements.Count - 1;
					if (num4 < 0 || num5 < 0)
					{
						Element element = new MouseWheelAxis(this, new MouseWheelAxis.Definition
						{
							name = "Wheel Horizontal",
							coordinateMode = AxisCoordinateMode.Relative
						});
						iSDfvshuzoGnugywoIegMIxXehOfA(element);
						mouseWheel.uyXSbfHiMLgJilqFXGhEJklEyGVkA(element);
						mouseWheel.uyXSbfHiMLgJilqFXGhEJklEyGVkA((num4 < 0) ? base.Rewired_002EIPlayerController_002Eaxes[num5] : base.Rewired_002EIPlayerController_002Eaxes[num4]);
					}
					else
					{
						mouseWheel.uyXSbfHiMLgJilqFXGhEJklEyGVkA(base.Rewired_002EIPlayerController_002Eaxes[num4]);
						mouseWheel.uyXSbfHiMLgJilqFXGhEJklEyGVkA(base.Rewired_002EIPlayerController_002Eaxes[num5]);
					}
				}
			}
			if (bGtdVFESOnDfMTZBXiNEHpavmeRCb)
			{
				ScreenRect screenRect = OMoNCpadTYwaeRNobFNMjdZWnLObA();
				vdccOITqmnFqYOMLmxBoFxsNPWUu = new Vector2(screenRect.center.x, screenRect.center.y);
			}
			else
			{
				vdccOITqmnFqYOMLmxBoFxsNPWUu = Vector2.zero;
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
			if (OoUyLvYkdGKsBuZWpTSclJmMRAuB >= 0)
			{
				vdccOITqmnFqYOMLmxBoFxsNPWUu.x = oaPEuYbnizQaYamkFebskJMVUBDA(base.Rewired_002EIPlayerController_002Eaxes[OoUyLvYkdGKsBuZWpTSclJmMRAuB], vdccOITqmnFqYOMLmxBoFxsNPWUu.x, nVvJVUJDpKFJEeKtFCqYURptlijCA);
			}
			if (HBtjPwDHkfeXrjRpChpLjeLDyveB >= 0)
			{
				vdccOITqmnFqYOMLmxBoFxsNPWUu.y = oaPEuYbnizQaYamkFebskJMVUBDA(base.Rewired_002EIPlayerController_002Eaxes[HBtjPwDHkfeXrjRpChpLjeLDyveB], vdccOITqmnFqYOMLmxBoFxsNPWUu.y, nVvJVUJDpKFJEeKtFCqYURptlijCA);
			}
			Player player;
			if (DcZhjrgpBCRAkimTIcRJWaziETLuA && (player = base.QiJEXVIXovhEnewuSvocJGYfBNrfA) != null)
			{
				if (!player.controllers.hasMouse)
				{
					rXBuYuBKAKSvTehoUkRyttAJGxwJ();
				}
				else
				{
					TYdYdITwdAqTgwuMJQIHaONFfaek = ReInput.controllers.Mouse.screenPosition;
					if (TYdYdITwdAqTgwuMJQIHaONFfaek.x != GkGfSqVQYHeSucTlgegxadmsxyNdb.x || TYdYdITwdAqTgwuMJQIHaONFfaek.y != GkGfSqVQYHeSucTlgegxadmsxyNdb.y)
					{
						vdccOITqmnFqYOMLmxBoFxsNPWUu.x = TYdYdITwdAqTgwuMJQIHaONFfaek.x;
						vdccOITqmnFqYOMLmxBoFxsNPWUu.y = TYdYdITwdAqTgwuMJQIHaONFfaek.y;
					}
					GkGfSqVQYHeSucTlgegxadmsxyNdb.x = TYdYdITwdAqTgwuMJQIHaONFfaek.x;
					GkGfSqVQYHeSucTlgegxadmsxyNdb.y = TYdYdITwdAqTgwuMJQIHaONFfaek.y;
				}
			}
			ffdWUCxRlAxhLfvaBOPchOEKYykO(vdccOITqmnFqYOMLmxBoFxsNPWUu);
			HygApUuTRjCmeiltQHvBPdPtSJVK.x = vdccOITqmnFqYOMLmxBoFxsNPWUu.x - VrKvjyZBxmMHwIjErsJxqbWnVrOK.x;
			HygApUuTRjCmeiltQHvBPdPtSJVK.y = vdccOITqmnFqYOMLmxBoFxsNPWUu.y - VrKvjyZBxmMHwIjErsJxqbWnVrOK.y;
			kJOiPdfosyhDqeHoqWEhtXgbLlbX = vdccOITqmnFqYOMLmxBoFxsNPWUu.x != VrKvjyZBxmMHwIjErsJxqbWnVrOK.x || vdccOITqmnFqYOMLmxBoFxsNPWUu.y != VrKvjyZBxmMHwIjErsJxqbWnVrOK.y;
			VrKvjyZBxmMHwIjErsJxqbWnVrOK.x = vdccOITqmnFqYOMLmxBoFxsNPWUu.x;
			VrKvjyZBxmMHwIjErsJxqbWnVrOK.y = vdccOITqmnFqYOMLmxBoFxsNPWUu.y;
			return true;
		}

		protected override void UpdateFinished()
		{
			base.UpdateFinished();
			if (kJOiPdfosyhDqeHoqWEhtXgbLlbX && PLuebwDisDKbcQxRZEmFmlSZJNPYA != null)
			{
				try
				{
					PLuebwDisDKbcQxRZEmFmlSZJNPYA(vdccOITqmnFqYOMLmxBoFxsNPWUu);
				}
				catch (Exception ex)
				{
					Logger.LogError("An exception occurred in a listener of ScreenPositionChangedEvent. This means an exception was thrown by your code.\n" + ex);
				}
				kJOiPdfosyhDqeHoqWEhtXgbLlbX = false;
			}
		}

		protected override void ClearVars()
		{
			base.ClearVars();
			VrKvjyZBxmMHwIjErsJxqbWnVrOK = vdccOITqmnFqYOMLmxBoFxsNPWUu;
			HygApUuTRjCmeiltQHvBPdPtSJVK = Vector2.zero;
			rXBuYuBKAKSvTehoUkRyttAJGxwJ();
			kJOiPdfosyhDqeHoqWEhtXgbLlbX = false;
		}

		private void ffdWUCxRlAxhLfvaBOPchOEKYykO(Vector2 P_0)
		{
			if (!zOYRmfGxdsJtZHUyvQqHLyfLUFiu)
			{
				vdccOITqmnFqYOMLmxBoFxsNPWUu = P_0;
				return;
			}
			if (pzskApWCRmADopNaqHLaSAABreBI == MovementAreaUnit.Screen)
			{
				float num = Screen.width;
				float num2 = Screen.height;
				vdccOITqmnFqYOMLmxBoFxsNPWUu.x = Mathf.Clamp(P_0.x, UzCWywvBSYGGbAiTJpqtDKgbnfMXb.xMin * num, UzCWywvBSYGGbAiTJpqtDKgbnfMXb.xMax * num);
				vdccOITqmnFqYOMLmxBoFxsNPWUu.y = Mathf.Clamp(P_0.y, UzCWywvBSYGGbAiTJpqtDKgbnfMXb.yMin * num2, UzCWywvBSYGGbAiTJpqtDKgbnfMXb.yMax * num2);
				return;
			}
			if (pzskApWCRmADopNaqHLaSAABreBI == MovementAreaUnit.Pixel)
			{
				vdccOITqmnFqYOMLmxBoFxsNPWUu.x = Mathf.Clamp(P_0.x, UzCWywvBSYGGbAiTJpqtDKgbnfMXb.xMin, UzCWywvBSYGGbAiTJpqtDKgbnfMXb.xMax);
				vdccOITqmnFqYOMLmxBoFxsNPWUu.y = Mathf.Clamp(P_0.y, UzCWywvBSYGGbAiTJpqtDKgbnfMXb.yMin, UzCWywvBSYGGbAiTJpqtDKgbnfMXb.yMax);
				return;
			}
			throw new NotImplementedException();
		}

		private ScreenRect OMoNCpadTYwaeRNobFNMjdZWnLObA()
		{
			if (pzskApWCRmADopNaqHLaSAABreBI == MovementAreaUnit.Screen)
			{
				return new ScreenRect(UzCWywvBSYGGbAiTJpqtDKgbnfMXb.xMin * (float)Screen.width, UzCWywvBSYGGbAiTJpqtDKgbnfMXb.yMin * (float)Screen.height, UzCWywvBSYGGbAiTJpqtDKgbnfMXb.width * (float)Screen.width, UzCWywvBSYGGbAiTJpqtDKgbnfMXb.height * (float)Screen.height);
			}
			if (pzskApWCRmADopNaqHLaSAABreBI == MovementAreaUnit.Pixel)
			{
				return UzCWywvBSYGGbAiTJpqtDKgbnfMXb;
			}
			throw new NotImplementedException();
		}

		private void rXBuYuBKAKSvTehoUkRyttAJGxwJ()
		{
			TYdYdITwdAqTgwuMJQIHaONFfaek = Vector2.zero;
			GkGfSqVQYHeSucTlgegxadmsxyNdb = Vector2.zero;
		}

		private static float oaPEuYbnizQaYamkFebskJMVUBDA(Axis P_0, float P_1, float P_2)
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
