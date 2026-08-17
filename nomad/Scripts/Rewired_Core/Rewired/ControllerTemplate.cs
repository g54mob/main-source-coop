using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rewired.Data.Mapping;
using Rewired.Interfaces;
using Rewired.Internal.Localization;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;
using UnityEngine;

namespace Rewired
{
	public abstract class ControllerTemplate : IControllerTemplate, IControllerTemplate_Internal, bguKJVtsagJfXPpJQeurpzlOLIYd
	{
		internal abstract class DPSMyxkASzarrLDVbAOjLPvrhhtq : IControllerTemplateElement, IControllerTemplateElement_Internal
		{
			private readonly IControllerTemplate uuzbNRWZGsgmTabaIytDbJnTxCTC;

			private readonly int jjQDorEiXUbiIWieoYuFAJLZAOodb;

			private readonly ControllerTemplateElementType pmnGoFivZSjwLhZyHxndkAwlEeRX;

			protected readonly int obdZCULGboiNZlqIYjzCfwkMcfUiA;

			protected readonly PNpsspmaeIckaEYciaSqXgOdyvotA pOpZQuofSVGIjHVEILeSpwZijfTe;

			int IControllerTemplateElement.id
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return -1;
					}
					return jjQDorEiXUbiIWieoYuFAJLZAOodb;
				}
			}

			string IControllerTemplateElement.descriptiveName
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return pOpZQuofSVGIjHVEILeSpwZijfTe.zdsEXqGbgFgpRWZpvCSgpdwbtisv;
				}
			}

			internal string hJVzJwgdSuPPEfJDIWDmjqrGWhXf => pOpZQuofSVGIjHVEILeSpwZijfTe.nonLocalizedDescriptiveName;

			ControllerTemplateElementType IControllerTemplateElement.type
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return ControllerTemplateElementType.Axis;
					}
					return pmnGoFivZSjwLhZyHxndkAwlEeRX;
				}
			}

			IControllerTemplate IControllerTemplateElement_Internal.parent => uuzbNRWZGsgmTabaIytDbJnTxCTC;

			public abstract int elementCount { get; }

			public abstract IControllerTemplateElementSource source { get; }

			public abstract bool exists { get; }

			protected DPSMyxkASzarrLDVbAOjLPvrhhtq(IControllerTemplate P_0, int P_1, ControllerTemplateElementType P_2, PNpsspmaeIckaEYciaSqXgOdyvotA P_3)
			{
				if (P_0 == null)
				{
					throw new ArgumentNullException("parent");
				}
				if (P_3 == null)
				{
					throw new ArgumentNullException("localizedElement");
				}
				uuzbNRWZGsgmTabaIytDbJnTxCTC = P_0;
				jjQDorEiXUbiIWieoYuFAJLZAOodb = P_1;
				pmnGoFivZSjwLhZyHxndkAwlEeRX = P_2;
				obdZCULGboiNZlqIYjzCfwkMcfUiA = ReInput.id;
				pOpZQuofSVGIjHVEILeSpwZijfTe = P_3;
			}

			public abstract IControllerTemplateElement GetElement(int index);

			public abstract int GetElementTargets(ControllerElementTarget find, ref IList<ControllerTemplateElementTarget> list);

			protected static PNpsspmaeIckaEYciaSqXgOdyvotA lifdeXJbzWkfqiCcaOMyELWULpyBA(IControllerTemplate_Internal P_0, int P_1, string P_2, string P_3)
			{
				return tgEJXVjqgYQGLEfNpszPQLpUhWeE.RBoAIeJFHlIncqSOQlAFyhiztwBtA(new PNpsspmaeIckaEYciaSqXgOdyvotA(rJaQxrECseLmKNOnbKRvRuXxdZoR.oqrPXVFddmGZZCIUEBtKUvjlPOvCA(cBFxQChnAZFRRQeDStCHagOAAZyI.ControllerTemplate, FDNFDGKMldROgCHjPdSVTnUzAnLgb.sztzDKprOgaEtSRoFjITTczsHDuW.Unknown, FDNFDGKMldROgCHjPdSVTnUzAnLgb.LsWebCorzTdhEUjUrAlgVzPmJJHR.None, P_1, P_0.deviceLocalizationInfo), "controller/template", string.Empty, P_2, P_3));
			}
		}

		internal abstract class DyTbRweylYMEjrmDqRebZlSGAXwIA : DPSMyxkASzarrLDVbAOjLPvrhhtq
		{
			protected readonly int koFxFffZFSSeEpVnUhvQPLkFAAukA;

			protected readonly miYoADVkblEyjEtgJEzTqnbvebgPA[] XThRFlTFtHsVfMrcfSyEchcwTibG;

			bool DPSMyxkASzarrLDVbAOjLPvrhhtq.exists
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return false;
					}
					if (XThRFlTFtHsVfMrcfSyEchcwTibG == null)
					{
						return false;
					}
					for (int i = 0; i < XThRFlTFtHsVfMrcfSyEchcwTibG.Length; i++)
					{
						if (XThRFlTFtHsVfMrcfSyEchcwTibG[i].zHGTQktLKhMnxFlTIuFPsfSsFhHA != null)
						{
							return true;
						}
					}
					return false;
				}
			}

			protected DyTbRweylYMEjrmDqRebZlSGAXwIA(IControllerTemplate P_0, int P_1, ControllerTemplateElementType P_2, IList<miYoADVkblEyjEtgJEzTqnbvebgPA> P_3, PNpsspmaeIckaEYciaSqXgOdyvotA P_4)
				: base(P_0, P_1, P_2, P_4)
			{
				XThRFlTFtHsVfMrcfSyEchcwTibG = ((P_3 != null) ? ListTools.ToArray(P_3) : null);
				koFxFffZFSSeEpVnUhvQPLkFAAukA = ((XThRFlTFtHsVfMrcfSyEchcwTibG != null) ? XThRFlTFtHsVfMrcfSyEchcwTibG.Length : 0);
			}
		}

		internal abstract class RZNDjkbvHOEbvhWYTRUapEJHWiOu : DyTbRweylYMEjrmDqRebZlSGAXwIA, IControllerTemplateAxis, IControllerTemplateElement, IControllerTemplateButton
		{
			private xcslkxDzwrCojABLPbRuUvYdnRhl JpZXBFPbdWGxQcXxDMtqTuaZjrIEA;

			public float JhIemKcyQaKiPCPTNJYzjiaeKgYU
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return 0f;
					}
					if (koFxFffZFSSeEpVnUhvQPLkFAAukA == 1)
					{
						return XThRFlTFtHsVfMrcfSyEchcwTibG[0].ocVeAgdLlOJhUguJXXZWkdByjITmA;
					}
					if (koFxFffZFSSeEpVnUhvQPLkFAAukA == 2)
					{
						float num = XThRFlTFtHsVfMrcfSyEchcwTibG[0].ocVeAgdLlOJhUguJXXZWkdByjITmA;
						float num2 = XThRFlTFtHsVfMrcfSyEchcwTibG[1].ocVeAgdLlOJhUguJXXZWkdByjITmA;
						return MathTools.Clamp(num + num2, -1f, 1f);
					}
					return 0f;
				}
			}

			public float NjKARPHoNcQkOkgVdduIUEZXmnbeA
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return 0f;
					}
					if (koFxFffZFSSeEpVnUhvQPLkFAAukA == 1)
					{
						return XThRFlTFtHsVfMrcfSyEchcwTibG[0].SgLrkJfbtoCAeNlACFDDfhgvgMfZ;
					}
					if (koFxFffZFSSeEpVnUhvQPLkFAAukA == 2)
					{
						float num = XThRFlTFtHsVfMrcfSyEchcwTibG[0].SgLrkJfbtoCAeNlACFDDfhgvgMfZ;
						float num2 = XThRFlTFtHsVfMrcfSyEchcwTibG[1].SgLrkJfbtoCAeNlACFDDfhgvgMfZ;
						return MathTools.Clamp(num + num2, -1f, 1f);
					}
					return 0f;
				}
			}

			public bool tUZOZdxQzjGKRTqCVLhelEmRByYK
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return false;
					}
					if (koFxFffZFSSeEpVnUhvQPLkFAAukA == 1)
					{
						return XThRFlTFtHsVfMrcfSyEchcwTibG[0].wcbtUjICGPboMarNZtqcvWSmcmby;
					}
					if (koFxFffZFSSeEpVnUhvQPLkFAAukA == 2)
					{
						if (!XThRFlTFtHsVfMrcfSyEchcwTibG[0].wcbtUjICGPboMarNZtqcvWSmcmby)
						{
							return XThRFlTFtHsVfMrcfSyEchcwTibG[1].wcbtUjICGPboMarNZtqcvWSmcmby;
						}
						return true;
					}
					return false;
				}
			}

			public bool QHiOivwdVsbVZGYEARAaBUrWfiyVA
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return false;
					}
					if (koFxFffZFSSeEpVnUhvQPLkFAAukA == 1)
					{
						return XThRFlTFtHsVfMrcfSyEchcwTibG[0].gxIxsogcCjxNsRlndyaYlOZIYnfN;
					}
					if (koFxFffZFSSeEpVnUhvQPLkFAAukA == 2)
					{
						if (!XThRFlTFtHsVfMrcfSyEchcwTibG[0].gxIxsogcCjxNsRlndyaYlOZIYnfN)
						{
							return XThRFlTFtHsVfMrcfSyEchcwTibG[1].gxIxsogcCjxNsRlndyaYlOZIYnfN;
						}
						return true;
					}
					return false;
				}
			}

			string IControllerTemplateAxis.positiveDescriptiveName
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return vTMSaLESpgMxDSoTJSsxXFKAuqZj.XGjIAzpsVEfgdjmEEVsMkekgEDul;
				}
			}

			string IControllerTemplateAxis.negativeDescriptiveName
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return vTMSaLESpgMxDSoTJSsxXFKAuqZj.vWZrYDhqXBsBRfJkzXZxwmluNeIb;
				}
			}

			float IControllerTemplateAxis.value
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return 0f;
					}
					return JhIemKcyQaKiPCPTNJYzjiaeKgYU;
				}
			}

			float IControllerTemplateAxis.valuePrev
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return 0f;
					}
					return NjKARPHoNcQkOkgVdduIUEZXmnbeA;
				}
			}

			IControllerTemplateAxisSource IControllerTemplateAxis.source
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return JpZXBFPbdWGxQcXxDMtqTuaZjrIEA;
				}
			}

			bool IControllerTemplateButton.value
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return false;
					}
					return tUZOZdxQzjGKRTqCVLhelEmRByYK;
				}
			}

			bool IControllerTemplateButton.valuePrev
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return false;
					}
					return QHiOivwdVsbVZGYEARAaBUrWfiyVA;
				}
			}

			bool IControllerTemplateButton.justPressed
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return false;
					}
					if (koFxFffZFSSeEpVnUhvQPLkFAAukA == 1)
					{
						return XThRFlTFtHsVfMrcfSyEchcwTibG[0].DDvFvYetWkGPEPbHSgpRYPblCbje;
					}
					if (koFxFffZFSSeEpVnUhvQPLkFAAukA == 2)
					{
						if (!XThRFlTFtHsVfMrcfSyEchcwTibG[0].DDvFvYetWkGPEPbHSgpRYPblCbje || XThRFlTFtHsVfMrcfSyEchcwTibG[1].gxIxsogcCjxNsRlndyaYlOZIYnfN)
						{
							if (XThRFlTFtHsVfMrcfSyEchcwTibG[1].DDvFvYetWkGPEPbHSgpRYPblCbje)
							{
								return !XThRFlTFtHsVfMrcfSyEchcwTibG[0].gxIxsogcCjxNsRlndyaYlOZIYnfN;
							}
							return false;
						}
						return true;
					}
					return false;
				}
			}

			bool IControllerTemplateButton.justReleased
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return false;
					}
					if (koFxFffZFSSeEpVnUhvQPLkFAAukA == 1)
					{
						return XThRFlTFtHsVfMrcfSyEchcwTibG[0].aKuxZnEjETiDBPmNDQDCnfUnUSSs;
					}
					if (koFxFffZFSSeEpVnUhvQPLkFAAukA == 2)
					{
						if (!XThRFlTFtHsVfMrcfSyEchcwTibG[0].aKuxZnEjETiDBPmNDQDCnfUnUSSs || XThRFlTFtHsVfMrcfSyEchcwTibG[1].wcbtUjICGPboMarNZtqcvWSmcmby)
						{
							if (XThRFlTFtHsVfMrcfSyEchcwTibG[1].aKuxZnEjETiDBPmNDQDCnfUnUSSs)
							{
								return !XThRFlTFtHsVfMrcfSyEchcwTibG[0].wcbtUjICGPboMarNZtqcvWSmcmby;
							}
							return false;
						}
						return true;
					}
					return false;
				}
			}

			bool IControllerTemplateButton.justChangedState
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return false;
					}
					return tUZOZdxQzjGKRTqCVLhelEmRByYK != QHiOivwdVsbVZGYEARAaBUrWfiyVA;
				}
			}

			float IControllerTemplateButton.pressure
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return 0f;
					}
					return JhIemKcyQaKiPCPTNJYzjiaeKgYU;
				}
			}

			float IControllerTemplateButton.pressurePrev
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return 0f;
					}
					return NjKARPHoNcQkOkgVdduIUEZXmnbeA;
				}
			}

			IControllerTemplateButtonSource IControllerTemplateButton.source
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return JpZXBFPbdWGxQcXxDMtqTuaZjrIEA;
				}
			}

			IControllerTemplateElementSource DPSMyxkASzarrLDVbAOjLPvrhhtq.source
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return JpZXBFPbdWGxQcXxDMtqTuaZjrIEA;
				}
			}

			int DPSMyxkASzarrLDVbAOjLPvrhhtq.elementCount => 0;

			IControllerTemplateAxis IControllerTemplateButton.AsAxis
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return this;
				}
			}

			IControllerTemplateButton IControllerTemplateAxis.AsButton
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return this;
				}
			}

			protected NaBoqZQhgrnxbkUDrZrVVpJNFDVG vTMSaLESpgMxDSoTJSsxXFKAuqZj => (NaBoqZQhgrnxbkUDrZrVVpJNFDVG)pOpZQuofSVGIjHVEILeSpwZijfTe;

			protected RZNDjkbvHOEbvhWYTRUapEJHWiOu(IControllerTemplate P_0, int P_1, ControllerTemplateElementType P_2, xcslkxDzwrCojABLPbRuUvYdnRhl P_3, IList<miYoADVkblEyjEtgJEzTqnbvebgPA> P_4, NaBoqZQhgrnxbkUDrZrVVpJNFDVG P_5)
				: base(P_0, P_1, P_2, P_4, P_5)
			{
				if (P_4 != null && P_4.Count > 2)
				{
					throw new ArgumentOutOfRangeException("sourceElements.Count must be <= 2.");
				}
				if (P_3 == null)
				{
					throw new ArgumentNullException("target");
				}
				JpZXBFPbdWGxQcXxDMtqTuaZjrIEA = P_3;
			}

			string IControllerTemplateAxis.GetDescriptiveName(AxisRange axisRange)
			{
				if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
				{
					ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
					return null;
				}
				return axisRange switch
				{
					AxisRange.Full => base.Rewired_002EIControllerTemplateElement_002EdescriptiveName, 
					AxisRange.Positive => ((IControllerTemplateAxis)this).positiveDescriptiveName, 
					AxisRange.Negative => ((IControllerTemplateAxis)this).negativeDescriptiveName, 
					_ => throw new NotImplementedException(), 
				};
			}

			public override IControllerTemplateElement GetElement(int index)
			{
				return null;
			}

			public override int GetElementTargets(ControllerElementTarget find, ref IList<ControllerTemplateElementTarget> list)
			{
				if (find.elementIdentifierId < 0)
				{
					return 0;
				}
				int num = 0;
				switch (base.Rewired_002EIControllerTemplateElement_002Etype)
				{
				case ControllerTemplateElementType.Axis:
				{
					IControllerTemplateAxisSource jpZXBFPbdWGxQcXxDMtqTuaZjrIEA = JpZXBFPbdWGxQcXxDMtqTuaZjrIEA;
					if (jpZXBFPbdWGxQcXxDMtqTuaZjrIEA.splitAxis)
					{
						if (WQIbDUwfviJQCXfRJmQwoCBDPYZV(find, jpZXBFPbdWGxQcXxDMtqTuaZjrIEA.positiveTarget))
						{
							ListTools.AddAndCreateList(ref list, new ControllerTemplateElementTarget(this, AxisRange.Positive));
							num++;
						}
						if (WQIbDUwfviJQCXfRJmQwoCBDPYZV(find, jpZXBFPbdWGxQcXxDMtqTuaZjrIEA.negativeTarget))
						{
							ListTools.AddAndCreateList(ref list, new ControllerTemplateElementTarget(this, AxisRange.Negative));
							num++;
						}
					}
					else if (WQIbDUwfviJQCXfRJmQwoCBDPYZV(find, jpZXBFPbdWGxQcXxDMtqTuaZjrIEA.fullTarget))
					{
						ListTools.AddAndCreateList(ref list, new ControllerTemplateElementTarget(this, find.axisRange));
						num++;
					}
					break;
				}
				case ControllerTemplateElementType.Button:
					if (WQIbDUwfviJQCXfRJmQwoCBDPYZV(find, ((IControllerTemplateButtonSource)JpZXBFPbdWGxQcXxDMtqTuaZjrIEA).target))
					{
						ListTools.AddAndCreateList(ref list, new ControllerTemplateElementTarget(this, AxisRange.Full));
						num++;
					}
					break;
				default:
					throw new NotImplementedException();
				}
				return num;
			}

			private static bool WQIbDUwfviJQCXfRJmQwoCBDPYZV(ControllerElementTarget P_0, IControllerElementTarget P_1)
			{
				if (P_1.elementIdentifierId != P_0.elementIdentifierId)
				{
					return false;
				}
				switch (P_1.elementType)
				{
				case ControllerElementType.Axis:
				{
					AxisRange axisRange = P_1.axisRange;
					if (axisRange == AxisRange.Full)
					{
						return true;
					}
					if (axisRange == P_0.axisRange)
					{
						return true;
					}
					return false;
				}
				case ControllerElementType.Button:
					return true;
				default:
					throw new NotImplementedException();
				}
			}
		}

		internal sealed class wEDpgRYkJezclVWPwPJlOnrGQLHE : RZNDjkbvHOEbvhWYTRUapEJHWiOu
		{
			public wEDpgRYkJezclVWPwPJlOnrGQLHE(IControllerTemplate_Internal P_0, int P_1, string P_2, string P_3, string P_4, string P_5, string P_6, string P_7, xcslkxDzwrCojABLPbRuUvYdnRhl P_8, IList<miYoADVkblEyjEtgJEzTqnbvebgPA> P_9)
				: base(P_0, P_1, ControllerTemplateElementType.Axis, P_8, P_9, (NaBoqZQhgrnxbkUDrZrVVpJNFDVG)tgEJXVjqgYQGLEfNpszPQLpUhWeE.RBoAIeJFHlIncqSOQlAFyhiztwBtA(new NaBoqZQhgrnxbkUDrZrVVpJNFDVG(bUlLQaUKfECmSjzpJPefXKFSSdNK.OcjIawXXAcgwClAgPsVkrwpdXTij(cBFxQChnAZFRRQeDStCHagOAAZyI.ControllerTemplate, FDNFDGKMldROgCHjPdSVTnUzAnLgb.sztzDKprOgaEtSRoFjITTczsHDuW.Axis, FDNFDGKMldROgCHjPdSVTnUzAnLgb.LsWebCorzTdhEUjUrAlgVzPmJJHR.None, P_1, P_0.deviceLocalizationInfo), "controller/template", string.Empty, P_2, P_3, P_4, P_5, P_6, P_7)))
			{
				if (P_9 != null && P_9.Count > 2)
				{
					throw new ArgumentOutOfRangeException("sourceElements.Count must be <= 2.");
				}
			}

			internal static wEDpgRYkJezclVWPwPJlOnrGQLHE pNeCOamePGsLRrhAkHfuRGxROIqc(IControllerTemplate_Internal P_0)
			{
				return new wEDpgRYkJezclVWPwPJlOnrGQLHE(P_0, -1, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, xcslkxDzwrCojABLPbRuUvYdnRhl.szNaYxvmrjQKoEMODBrltQFmmKyi(ControllerTemplateElementType.Axis), null);
			}
		}

		internal sealed class BShaaIVTHefYkCCCxsFLxwxLvTMk : RZNDjkbvHOEbvhWYTRUapEJHWiOu
		{
			public BShaaIVTHefYkCCCxsFLxwxLvTMk(IControllerTemplate_Internal P_0, int P_1, string P_2, string P_3, string P_4, string P_5, string P_6, string P_7, xcslkxDzwrCojABLPbRuUvYdnRhl P_8, IList<miYoADVkblEyjEtgJEzTqnbvebgPA> P_9)
				: base(P_0, P_1, ControllerTemplateElementType.Button, P_8, P_9, (NaBoqZQhgrnxbkUDrZrVVpJNFDVG)tgEJXVjqgYQGLEfNpszPQLpUhWeE.RBoAIeJFHlIncqSOQlAFyhiztwBtA(new NaBoqZQhgrnxbkUDrZrVVpJNFDVG(bUlLQaUKfECmSjzpJPefXKFSSdNK.OcjIawXXAcgwClAgPsVkrwpdXTij(cBFxQChnAZFRRQeDStCHagOAAZyI.ControllerTemplate, FDNFDGKMldROgCHjPdSVTnUzAnLgb.sztzDKprOgaEtSRoFjITTczsHDuW.Button, FDNFDGKMldROgCHjPdSVTnUzAnLgb.LsWebCorzTdhEUjUrAlgVzPmJJHR.None, P_1, P_0.deviceLocalizationInfo), "controller/template", string.Empty, P_2, P_3, P_4, P_5, P_6, P_7)))
			{
				if (P_9 != null && P_9.Count > 1)
				{
					throw new ArgumentOutOfRangeException("sourceElements.Count must be <= 1.");
				}
			}

			internal static BShaaIVTHefYkCCCxsFLxwxLvTMk SkAnIcKlidTGodlqxURKePAzlVlp(IControllerTemplate_Internal P_0)
			{
				return new BShaaIVTHefYkCCCxsFLxwxLvTMk(P_0, -1, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, xcslkxDzwrCojABLPbRuUvYdnRhl.szNaYxvmrjQKoEMODBrltQFmmKyi(ControllerTemplateElementType.Button), null);
			}
		}

		internal abstract class vMvJMFtwSXbllSYAnduMJGvPpvIU : DPSMyxkASzarrLDVbAOjLPvrhhtq
		{
			protected readonly int KZVqyPAqMCUpEQIDQOPfHyqjISUw;

			protected readonly DPSMyxkASzarrLDVbAOjLPvrhhtq[] EBYBGrccfgTZWrODyLHzmrHhaNrBb;

			bool DPSMyxkASzarrLDVbAOjLPvrhhtq.exists
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return false;
					}
					for (int i = 0; i < KZVqyPAqMCUpEQIDQOPfHyqjISUw; i++)
					{
						if (EBYBGrccfgTZWrODyLHzmrHhaNrBb[i].exists)
						{
							return true;
						}
					}
					return false;
				}
			}

			IControllerTemplateElementSource DPSMyxkASzarrLDVbAOjLPvrhhtq.source
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return null;
				}
			}

			int DPSMyxkASzarrLDVbAOjLPvrhhtq.elementCount => KZVqyPAqMCUpEQIDQOPfHyqjISUw;

			protected vMvJMFtwSXbllSYAnduMJGvPpvIU(IControllerTemplate P_0, int P_1, ControllerTemplateElementType P_2, DPSMyxkASzarrLDVbAOjLPvrhhtq[] P_3, PNpsspmaeIckaEYciaSqXgOdyvotA P_4)
				: base(P_0, P_1, P_2, P_4)
			{
				if (P_3 == null)
				{
					throw new ArgumentNullException("elements");
				}
				if (P_3.Length == 0)
				{
					throw new ArgumentException("elements.Length is zero.");
				}
				for (int i = 0; i < P_3.Length; i++)
				{
					if (P_3[i] == null)
					{
						throw new ArgumentNullException("elements contains a null entry.");
					}
				}
				EBYBGrccfgTZWrODyLHzmrHhaNrBb = P_3;
				KZVqyPAqMCUpEQIDQOPfHyqjISUw = P_3.Length;
			}

			public virtual IControllerTemplateElement AFzxaEsRaCizHJcxQpZUkdAFCqiO(int P_0)
			{
				return EBYBGrccfgTZWrODyLHzmrHhaNrBb[P_0];
			}

			public virtual int eDihnOffFpKsMOBFCQqhtHtakulSA(ControllerElementTarget P_0, ref IList<ControllerTemplateElementTarget> P_1)
			{
				int num = 0;
				for (int i = 0; i < EBYBGrccfgTZWrODyLHzmrHhaNrBb.Length; i++)
				{
					num += EBYBGrccfgTZWrODyLHzmrHhaNrBb[i].GetElementTargets(P_0, ref P_1);
				}
				return num;
			}
		}

		internal abstract class BQMEipmquyukYckiZIiAgbhcxieHA : vMvJMFtwSXbllSYAnduMJGvPpvIU, IControllerTemplateAxis2D, IControllerTemplateElement
		{
			protected const int LmGkzqQTARBxauVexNylSlJOECqm = 0;

			protected const int cNcfhserwtiffdahspHlbWOxHKYHb = 1;

			protected const int FvhsgqfhbqpTdosDEfLyydYNZEM = 2;

			Vector2 IControllerTemplateAxis2D.value
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return Vector2.zero;
					}
					return new Vector2((KZVqyPAqMCUpEQIDQOPfHyqjISUw > 0) ? ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[0]).JhIemKcyQaKiPCPTNJYzjiaeKgYU : 0f, (KZVqyPAqMCUpEQIDQOPfHyqjISUw > 1) ? ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[1]).JhIemKcyQaKiPCPTNJYzjiaeKgYU : 0f);
				}
			}

			Vector2 IControllerTemplateAxis2D.valuePrev
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return Vector2.zero;
					}
					return new Vector2((KZVqyPAqMCUpEQIDQOPfHyqjISUw > 0) ? ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[0]).NjKARPHoNcQkOkgVdduIUEZXmnbeA : 0f, (KZVqyPAqMCUpEQIDQOPfHyqjISUw > 1) ? ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[1]).NjKARPHoNcQkOkgVdduIUEZXmnbeA : 0f);
				}
			}

			IControllerTemplateAxis IControllerTemplateAxis2D.horizontal
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateAxis)EBYBGrccfgTZWrODyLHzmrHhaNrBb[0];
				}
			}

			IControllerTemplateAxis IControllerTemplateAxis2D.vertical
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateAxis)EBYBGrccfgTZWrODyLHzmrHhaNrBb[1];
				}
			}

			protected BQMEipmquyukYckiZIiAgbhcxieHA(IControllerTemplate P_0, int P_1, ControllerTemplateElementType P_2, DPSMyxkASzarrLDVbAOjLPvrhhtq[] P_3, PNpsspmaeIckaEYciaSqXgOdyvotA P_4)
				: base(P_0, P_1, P_2, P_3, P_4)
			{
			}
		}

		internal abstract class JgClGlCROAlrSHpHDfxgrRatRDFC : vMvJMFtwSXbllSYAnduMJGvPpvIU, IControllerTemplateAxis3D, IControllerTemplateElement
		{
			protected const int CANCSDETAieDWXspryuBCIcIVEiGA = 0;

			protected const int yOhrRfCcRusekDKPXgRRvgNCwIVN = 1;

			protected const int WRkGktGEWfqFboXvEBMExjpZaOyA = 2;

			protected const int RqfXSocElbnfZhhZrtNbOGhZHdtp = 3;

			Vector3 IControllerTemplateAxis3D.value
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return Vector3.zero;
					}
					return new Vector3((KZVqyPAqMCUpEQIDQOPfHyqjISUw > 0) ? ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[0]).JhIemKcyQaKiPCPTNJYzjiaeKgYU : 0f, (KZVqyPAqMCUpEQIDQOPfHyqjISUw > 1) ? ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[1]).JhIemKcyQaKiPCPTNJYzjiaeKgYU : 0f, (KZVqyPAqMCUpEQIDQOPfHyqjISUw > 2) ? ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[2]).JhIemKcyQaKiPCPTNJYzjiaeKgYU : 0f);
				}
			}

			Vector3 IControllerTemplateAxis3D.valuePrev
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return Vector3.zero;
					}
					return new Vector3((KZVqyPAqMCUpEQIDQOPfHyqjISUw > 0) ? ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[0]).NjKARPHoNcQkOkgVdduIUEZXmnbeA : 0f, (KZVqyPAqMCUpEQIDQOPfHyqjISUw > 1) ? ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[1]).NjKARPHoNcQkOkgVdduIUEZXmnbeA : 0f, (KZVqyPAqMCUpEQIDQOPfHyqjISUw > 2) ? ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[2]).NjKARPHoNcQkOkgVdduIUEZXmnbeA : 0f);
				}
			}

			IControllerTemplateAxis IControllerTemplateAxis3D.horizontal
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateAxis)EBYBGrccfgTZWrODyLHzmrHhaNrBb[0];
				}
			}

			IControllerTemplateAxis IControllerTemplateAxis3D.vertical
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateAxis)EBYBGrccfgTZWrODyLHzmrHhaNrBb[1];
				}
			}

			IControllerTemplateAxis IControllerTemplateAxis3D.depth
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateAxis)EBYBGrccfgTZWrODyLHzmrHhaNrBb[2];
				}
			}

			protected JgClGlCROAlrSHpHDfxgrRatRDFC(IControllerTemplate_Internal P_0, int P_1, ControllerTemplateElementType P_2, DPSMyxkASzarrLDVbAOjLPvrhhtq[] P_3, PNpsspmaeIckaEYciaSqXgOdyvotA P_4)
				: base(P_0, P_1, P_2, P_3, P_4)
			{
			}
		}

		internal abstract class hGomgYtnWolfdYhtqCWGrqELGMBn : vMvJMFtwSXbllSYAnduMJGvPpvIU, IControllerTemplateAxis6D, IControllerTemplateElement
		{
			protected const int mXrAdmgzGIformmPqQrkdOhRCPOBb = 0;

			protected const int FsxvXNPoVHiFWPzwfrAnzIFcjCnV = 1;

			protected const int hSeYfkxBhRfVPPSnclkmmfcgKVaD = 2;

			protected const int EUwNpuuyrcktLzOuePxqoTKrzMmm = 3;

			protected const int FjsDgRERnrGVLsDdJULQVaglFlVTA = 4;

			protected const int YHXLuFGuDWAggEIgzcnGWBjteJps = 5;

			protected const int mseIEcZubajJhMjpzjLRCxGVQIYS = 6;

			Vector3 IControllerTemplateAxis6D.position
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return Vector3.zero;
					}
					return new Vector3((KZVqyPAqMCUpEQIDQOPfHyqjISUw > 0) ? ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[0]).JhIemKcyQaKiPCPTNJYzjiaeKgYU : 0f, (KZVqyPAqMCUpEQIDQOPfHyqjISUw > 1) ? ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[1]).JhIemKcyQaKiPCPTNJYzjiaeKgYU : 0f, (KZVqyPAqMCUpEQIDQOPfHyqjISUw > 2) ? ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[2]).JhIemKcyQaKiPCPTNJYzjiaeKgYU : 0f);
				}
			}

			Vector3 IControllerTemplateAxis6D.positionPrev
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return Vector3.zero;
					}
					return new Vector3((KZVqyPAqMCUpEQIDQOPfHyqjISUw > 0) ? ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[0]).NjKARPHoNcQkOkgVdduIUEZXmnbeA : 0f, (KZVqyPAqMCUpEQIDQOPfHyqjISUw > 1) ? ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[1]).NjKARPHoNcQkOkgVdduIUEZXmnbeA : 0f, (KZVqyPAqMCUpEQIDQOPfHyqjISUw > 2) ? ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[2]).NjKARPHoNcQkOkgVdduIUEZXmnbeA : 0f);
				}
			}

			Vector3 IControllerTemplateAxis6D.rotation
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return Vector3.zero;
					}
					return new Vector3((KZVqyPAqMCUpEQIDQOPfHyqjISUw > 3) ? ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[3]).JhIemKcyQaKiPCPTNJYzjiaeKgYU : 0f, (KZVqyPAqMCUpEQIDQOPfHyqjISUw > 4) ? ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[4]).JhIemKcyQaKiPCPTNJYzjiaeKgYU : 0f, (KZVqyPAqMCUpEQIDQOPfHyqjISUw > 5) ? ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[5]).JhIemKcyQaKiPCPTNJYzjiaeKgYU : 0f);
				}
			}

			Vector3 IControllerTemplateAxis6D.rotationPrev
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return Vector3.zero;
					}
					return new Vector3((KZVqyPAqMCUpEQIDQOPfHyqjISUw > 3) ? ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[3]).NjKARPHoNcQkOkgVdduIUEZXmnbeA : 0f, (KZVqyPAqMCUpEQIDQOPfHyqjISUw > 4) ? ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[4]).NjKARPHoNcQkOkgVdduIUEZXmnbeA : 0f, (KZVqyPAqMCUpEQIDQOPfHyqjISUw > 5) ? ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[5]).NjKARPHoNcQkOkgVdduIUEZXmnbeA : 0f);
				}
			}

			IControllerTemplateAxis IControllerTemplateAxis6D.positionX
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateAxis)EBYBGrccfgTZWrODyLHzmrHhaNrBb[0];
				}
			}

			IControllerTemplateAxis IControllerTemplateAxis6D.positionY
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateAxis)EBYBGrccfgTZWrODyLHzmrHhaNrBb[1];
				}
			}

			IControllerTemplateAxis IControllerTemplateAxis6D.positionZ
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateAxis)EBYBGrccfgTZWrODyLHzmrHhaNrBb[2];
				}
			}

			IControllerTemplateAxis IControllerTemplateAxis6D.rotationX
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateAxis)EBYBGrccfgTZWrODyLHzmrHhaNrBb[3];
				}
			}

			IControllerTemplateAxis IControllerTemplateAxis6D.rotationY
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateAxis)EBYBGrccfgTZWrODyLHzmrHhaNrBb[4];
				}
			}

			IControllerTemplateAxis IControllerTemplateAxis6D.rotationZ
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateAxis)EBYBGrccfgTZWrODyLHzmrHhaNrBb[5];
				}
			}

			protected hGomgYtnWolfdYhtqCWGrqELGMBn(IControllerTemplate P_0, int P_1, ControllerTemplateElementType P_2, DPSMyxkASzarrLDVbAOjLPvrhhtq[] P_3, PNpsspmaeIckaEYciaSqXgOdyvotA P_4)
				: base(P_0, P_1, P_2, P_3, P_4)
			{
			}
		}

		internal sealed class bBeHCAvUbWuNvvSIEzTQGAlChFUL : JgClGlCROAlrSHpHDfxgrRatRDFC, IControllerTemplateStick, IControllerTemplateElement
		{
			private const int AZvwQxnzXfQeapVRHpkyOeDJLbHd = 3;

			IControllerTemplateAxis IControllerTemplateStick.rotation
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateAxis)EBYBGrccfgTZWrODyLHzmrHhaNrBb[2];
				}
			}

			private bBeHCAvUbWuNvvSIEzTQGAlChFUL(IControllerTemplate_Internal P_0, int P_1, string P_2, string P_3, DPSMyxkASzarrLDVbAOjLPvrhhtq[] P_4)
				: base(P_0, P_1, ControllerTemplateElementType.Stick, P_4, DPSMyxkASzarrLDVbAOjLPvrhhtq.lifdeXJbzWkfqiCcaOMyELWULpyBA(P_0, P_1, P_2, P_3))
			{
				if (P_4.Length != 3)
				{
					throw new ArgumentException("elements.Length must be " + 3);
				}
			}

			public bBeHCAvUbWuNvvSIEzTQGAlChFUL(IControllerTemplate_Internal P_0, int P_1, string P_2, string P_3, RZNDjkbvHOEbvhWYTRUapEJHWiOu P_4, RZNDjkbvHOEbvhWYTRUapEJHWiOu P_5, RZNDjkbvHOEbvhWYTRUapEJHWiOu P_6)
				: this(P_0, P_1, P_2, P_3, new DPSMyxkASzarrLDVbAOjLPvrhhtq[3] { P_4, P_5, P_6 })
			{
			}
		}

		internal sealed class qcDscyXMOxZMVECthAEVcLYTrmfFA : BQMEipmquyukYckiZIiAgbhcxieHA, IControllerTemplateThumbStick, IControllerTemplateElement
		{
			private const int BokljwUdGXiwyrrGibZbGrXmCdBD = 2;

			private const int ZQCOnzebFVHWMnnazDwGgZuJThvdA = 3;

			IControllerTemplateButton IControllerTemplateThumbStick.press
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateButton)EBYBGrccfgTZWrODyLHzmrHhaNrBb[2];
				}
			}

			private qcDscyXMOxZMVECthAEVcLYTrmfFA(IControllerTemplate_Internal P_0, int P_1, string P_2, string P_3, DPSMyxkASzarrLDVbAOjLPvrhhtq[] P_4)
				: base(P_0, P_1, ControllerTemplateElementType.ThumbStick, P_4, DPSMyxkASzarrLDVbAOjLPvrhhtq.lifdeXJbzWkfqiCcaOMyELWULpyBA(P_0, P_1, P_2, P_3))
			{
				if (P_4.Length != 3)
				{
					throw new ArgumentException("elements.Length must be " + 3);
				}
			}

			internal qcDscyXMOxZMVECthAEVcLYTrmfFA(IControllerTemplate_Internal P_0, int P_1, string P_2, string P_3, RZNDjkbvHOEbvhWYTRUapEJHWiOu P_4, RZNDjkbvHOEbvhWYTRUapEJHWiOu P_5, RZNDjkbvHOEbvhWYTRUapEJHWiOu P_6)
				: this(P_0, P_1, P_2, P_3, new DPSMyxkASzarrLDVbAOjLPvrhhtq[3] { P_4, P_5, P_6 })
			{
			}
		}

		internal sealed class SliKgniFbePJsGyYVBvMEPUoYIzw : vMvJMFtwSXbllSYAnduMJGvPpvIU, IControllerTemplateDPad, IControllerTemplateElement
		{
			private const int fiSQSfqmmiMmNUmcJalGwdIgZJdm = 0;

			private const int nFlvbDguXZHBVmOMLHjsINzhCrWdA = 1;

			private const int FVbtiLlSADkQEdhxoyuSVqhDAmmO = 2;

			private const int ESGEXagBlxnMYgiFBLWuVZcyFyMKA = 3;

			private const int tFDelddNmObwhFmepLLWphTuTcmZ = 4;

			private const int aOZtoumvpuSKPFkhdLgQrYUAjHXc = 5;

			Vector2 IControllerTemplateDPad.value
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return Vector2.zero;
					}
					return new Vector2(MathTools.Clamp(((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[0]).JhIemKcyQaKiPCPTNJYzjiaeKgYU + ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[2]).JhIemKcyQaKiPCPTNJYzjiaeKgYU * -1f, -1f, 1f), MathTools.Clamp(((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[3]).JhIemKcyQaKiPCPTNJYzjiaeKgYU * -1f + ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[1]).JhIemKcyQaKiPCPTNJYzjiaeKgYU, -1f, 1f));
				}
			}

			Vector2 IControllerTemplateDPad.valuePrev
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return Vector2.zero;
					}
					return new Vector2(MathTools.Clamp(((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[0]).NjKARPHoNcQkOkgVdduIUEZXmnbeA + ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[2]).NjKARPHoNcQkOkgVdduIUEZXmnbeA * -1f, -1f, 1f), MathTools.Clamp(((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[3]).NjKARPHoNcQkOkgVdduIUEZXmnbeA * -1f + ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[1]).NjKARPHoNcQkOkgVdduIUEZXmnbeA, -1f, 1f));
				}
			}

			IControllerTemplateButton IControllerTemplateDPad.up
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateButton)EBYBGrccfgTZWrODyLHzmrHhaNrBb[0];
				}
			}

			IControllerTemplateButton IControllerTemplateDPad.right
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateButton)EBYBGrccfgTZWrODyLHzmrHhaNrBb[1];
				}
			}

			IControllerTemplateButton IControllerTemplateDPad.down
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateButton)EBYBGrccfgTZWrODyLHzmrHhaNrBb[2];
				}
			}

			IControllerTemplateButton IControllerTemplateDPad.left
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateButton)EBYBGrccfgTZWrODyLHzmrHhaNrBb[3];
				}
			}

			IControllerTemplateButton IControllerTemplateDPad.press
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateButton)EBYBGrccfgTZWrODyLHzmrHhaNrBb[4];
				}
			}

			private SliKgniFbePJsGyYVBvMEPUoYIzw(IControllerTemplate_Internal P_0, int P_1, string P_2, string P_3, DPSMyxkASzarrLDVbAOjLPvrhhtq[] P_4)
				: base(P_0, P_1, ControllerTemplateElementType.DPad, P_4, DPSMyxkASzarrLDVbAOjLPvrhhtq.lifdeXJbzWkfqiCcaOMyELWULpyBA(P_0, P_1, P_2, P_3))
			{
				if (P_4.Length != 5)
				{
					throw new ArgumentException("elements.Length must be " + 5);
				}
			}

			internal SliKgniFbePJsGyYVBvMEPUoYIzw(IControllerTemplate_Internal P_0, int P_1, string P_2, string P_3, RZNDjkbvHOEbvhWYTRUapEJHWiOu P_4, RZNDjkbvHOEbvhWYTRUapEJHWiOu P_5, RZNDjkbvHOEbvhWYTRUapEJHWiOu P_6, RZNDjkbvHOEbvhWYTRUapEJHWiOu P_7, RZNDjkbvHOEbvhWYTRUapEJHWiOu P_8)
				: this(P_0, P_1, P_2, P_3, new DPSMyxkASzarrLDVbAOjLPvrhhtq[5] { P_4, P_5, P_6, P_7, P_8 })
			{
			}
		}

		internal sealed class ZhUbNCvNLygNgeiSvJTuRTUgdRAB : vMvJMFtwSXbllSYAnduMJGvPpvIU, IControllerTemplateThrottle, IControllerTemplateElement
		{
			private const int tHNndQvlBgdSHlQKgEYyhYjeZcxo = 0;

			private const int xCehtiaOtxTQammKiGzTnZXXbfgxA = 1;

			private const int tMuUSTyLMWFcibduWbFtBaHhkiiCb = 2;

			float IControllerTemplateThrottle.value
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return 0f;
					}
					return ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[0]).JhIemKcyQaKiPCPTNJYzjiaeKgYU;
				}
			}

			float IControllerTemplateThrottle.valuePrev
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return 0f;
					}
					return ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[0]).NjKARPHoNcQkOkgVdduIUEZXmnbeA;
				}
			}

			IControllerTemplateAxis IControllerTemplateThrottle.throttle
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateAxis)EBYBGrccfgTZWrODyLHzmrHhaNrBb[0];
				}
			}

			IControllerTemplateButton IControllerTemplateThrottle.minDetent
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateButton)EBYBGrccfgTZWrODyLHzmrHhaNrBb[1];
				}
			}

			private ZhUbNCvNLygNgeiSvJTuRTUgdRAB(IControllerTemplate_Internal P_0, int P_1, string P_2, string P_3, DPSMyxkASzarrLDVbAOjLPvrhhtq[] P_4)
				: base(P_0, P_1, ControllerTemplateElementType.Throttle, P_4, DPSMyxkASzarrLDVbAOjLPvrhhtq.lifdeXJbzWkfqiCcaOMyELWULpyBA(P_0, P_1, P_2, P_3))
			{
				if (P_4.Length != 2)
				{
					throw new ArgumentException("elements.Length must be " + 2);
				}
			}

			internal ZhUbNCvNLygNgeiSvJTuRTUgdRAB(IControllerTemplate_Internal P_0, int P_1, string P_2, string P_3, RZNDjkbvHOEbvhWYTRUapEJHWiOu P_4, RZNDjkbvHOEbvhWYTRUapEJHWiOu P_5)
				: this(P_0, P_1, P_2, P_3, new DPSMyxkASzarrLDVbAOjLPvrhhtq[2] { P_4, P_5 })
			{
			}
		}

		internal sealed class uKwdxDCUNWFtWJyMkiqclARhnTxm : vMvJMFtwSXbllSYAnduMJGvPpvIU, IControllerTemplateHat, IControllerTemplateElement
		{
			private const int sTRssNnnqcInUqNZzLnMSiwMAFBFA = 0;

			private const int EZkTYKsXGZOdVIQFCFqtXfpetibu = 1;

			private const int OLwTeIwykpQzrOvtDiGxBnemJMuX = 2;

			private const int XASLJkUpUDNsGMcVMwDggxYLQvnR = 3;

			private const int xBVTIFPTdMwVncIDwHbhHNnofZZRA = 4;

			private const int JPQgJbilwbqSdxabYEAwKSHVogpCb = 5;

			private const int NLtYGEUYcdnRmlpjgINOXICawNwt = 6;

			private const int udRtrwgbYzkyJaOWjmbazzlziyIp = 7;

			private const int PwRDvLLDpnhXlafzOnqJrYhuRQlW = 8;

			Vector2 IControllerTemplateHat.value
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return Vector2.zero;
					}
					Vector2 result = default(Vector2);
					result.y += ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[0]).JhIemKcyQaKiPCPTNJYzjiaeKgYU;
					result.x += ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[2]).JhIemKcyQaKiPCPTNJYzjiaeKgYU;
					result.y -= ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[4]).JhIemKcyQaKiPCPTNJYzjiaeKgYU;
					result.x -= ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[6]).JhIemKcyQaKiPCPTNJYzjiaeKgYU;
					float num = ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[1]).JhIemKcyQaKiPCPTNJYzjiaeKgYU;
					float num2 = ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[3]).JhIemKcyQaKiPCPTNJYzjiaeKgYU;
					float num3 = ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[5]).JhIemKcyQaKiPCPTNJYzjiaeKgYU;
					float num4 = ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[7]).JhIemKcyQaKiPCPTNJYzjiaeKgYU;
					result.x += num + num2 - num3 - num4;
					result.y += num + num4 - num2 - num3;
					result.x = MathTools.Clamp(result.x, -1f, 1f);
					result.y = MathTools.Clamp(result.y, -1f, 1f);
					return result;
				}
			}

			Vector2 IControllerTemplateHat.valuePrev
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return Vector2.zero;
					}
					Vector2 result = default(Vector2);
					result.y += ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[0]).NjKARPHoNcQkOkgVdduIUEZXmnbeA;
					result.x += ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[2]).NjKARPHoNcQkOkgVdduIUEZXmnbeA;
					result.y -= ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[4]).NjKARPHoNcQkOkgVdduIUEZXmnbeA;
					result.x -= ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[6]).NjKARPHoNcQkOkgVdduIUEZXmnbeA;
					float num = ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[1]).NjKARPHoNcQkOkgVdduIUEZXmnbeA;
					float num2 = ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[3]).NjKARPHoNcQkOkgVdduIUEZXmnbeA;
					float num3 = ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[5]).NjKARPHoNcQkOkgVdduIUEZXmnbeA;
					float num4 = ((RZNDjkbvHOEbvhWYTRUapEJHWiOu)EBYBGrccfgTZWrODyLHzmrHhaNrBb[7]).NjKARPHoNcQkOkgVdduIUEZXmnbeA;
					result.x += num + num2 - num3 - num4;
					result.y += num + num4 - num2 - num3;
					result.x = MathTools.Clamp(result.x, -1f, 1f);
					result.y = MathTools.Clamp(result.y, -1f, 1f);
					return result;
				}
			}

			IControllerTemplateButton IControllerTemplateHat.up
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateButton)EBYBGrccfgTZWrODyLHzmrHhaNrBb[0];
				}
			}

			IControllerTemplateButton IControllerTemplateHat.upRight
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateButton)EBYBGrccfgTZWrODyLHzmrHhaNrBb[1];
				}
			}

			IControllerTemplateButton IControllerTemplateHat.right
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateButton)EBYBGrccfgTZWrODyLHzmrHhaNrBb[2];
				}
			}

			IControllerTemplateButton IControllerTemplateHat.downRight
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateButton)EBYBGrccfgTZWrODyLHzmrHhaNrBb[3];
				}
			}

			IControllerTemplateButton IControllerTemplateHat.down
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateButton)EBYBGrccfgTZWrODyLHzmrHhaNrBb[4];
				}
			}

			IControllerTemplateButton IControllerTemplateHat.downLeft
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateButton)EBYBGrccfgTZWrODyLHzmrHhaNrBb[5];
				}
			}

			IControllerTemplateButton IControllerTemplateHat.left
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateButton)EBYBGrccfgTZWrODyLHzmrHhaNrBb[6];
				}
			}

			IControllerTemplateButton IControllerTemplateHat.upLeft
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateButton)EBYBGrccfgTZWrODyLHzmrHhaNrBb[7];
				}
			}

			private uKwdxDCUNWFtWJyMkiqclARhnTxm(IControllerTemplate_Internal P_0, int P_1, string P_2, string P_3, DPSMyxkASzarrLDVbAOjLPvrhhtq[] P_4)
				: base(P_0, P_1, ControllerTemplateElementType.Hat, P_4, DPSMyxkASzarrLDVbAOjLPvrhhtq.lifdeXJbzWkfqiCcaOMyELWULpyBA(P_0, P_1, P_2, P_3))
			{
				if (P_4.Length != 8)
				{
					throw new ArgumentException("elements.Length must be " + 8);
				}
			}

			internal uKwdxDCUNWFtWJyMkiqclARhnTxm(IControllerTemplate_Internal P_0, int P_1, string P_2, string P_3, RZNDjkbvHOEbvhWYTRUapEJHWiOu P_4, RZNDjkbvHOEbvhWYTRUapEJHWiOu P_5, RZNDjkbvHOEbvhWYTRUapEJHWiOu P_6, RZNDjkbvHOEbvhWYTRUapEJHWiOu P_7, RZNDjkbvHOEbvhWYTRUapEJHWiOu P_8, RZNDjkbvHOEbvhWYTRUapEJHWiOu P_9, RZNDjkbvHOEbvhWYTRUapEJHWiOu P_10, RZNDjkbvHOEbvhWYTRUapEJHWiOu P_11)
				: this(P_0, P_1, P_2, P_3, new DPSMyxkASzarrLDVbAOjLPvrhhtq[8] { P_4, P_5, P_6, P_7, P_8, P_9, P_10, P_11 })
			{
			}
		}

		internal sealed class inxZkaEHmZUyFfVMDQJuFqMbNRMr : BQMEipmquyukYckiZIiAgbhcxieHA, IControllerTemplateYoke, IControllerTemplateElement
		{
			private const int fashBHEVQnFNgJJsmPzhTKetGAQP = 2;

			IControllerTemplateAxis IControllerTemplateYoke.rotation
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateAxis)EBYBGrccfgTZWrODyLHzmrHhaNrBb[0];
				}
			}

			IControllerTemplateAxis IControllerTemplateYoke.pushPull
			{
				get
				{
					if (ReInput._id != obdZCULGboiNZlqIYjzCfwkMcfUiA)
					{
						ReInput.CheckInitialized(obdZCULGboiNZlqIYjzCfwkMcfUiA);
						return null;
					}
					return (IControllerTemplateAxis)EBYBGrccfgTZWrODyLHzmrHhaNrBb[1];
				}
			}

			private inxZkaEHmZUyFfVMDQJuFqMbNRMr(IControllerTemplate_Internal P_0, int P_1, string P_2, string P_3, DPSMyxkASzarrLDVbAOjLPvrhhtq[] P_4)
				: base(P_0, P_1, ControllerTemplateElementType.Yoke, P_4, DPSMyxkASzarrLDVbAOjLPvrhhtq.lifdeXJbzWkfqiCcaOMyELWULpyBA(P_0, P_1, P_2, P_3))
			{
			}

			internal inxZkaEHmZUyFfVMDQJuFqMbNRMr(IControllerTemplate_Internal P_0, int P_1, string P_2, string P_3, RZNDjkbvHOEbvhWYTRUapEJHWiOu P_4, RZNDjkbvHOEbvhWYTRUapEJHWiOu P_5)
				: this(P_0, P_1, P_2, P_3, new DPSMyxkASzarrLDVbAOjLPvrhhtq[2] { P_4, P_5 })
			{
			}
		}

		internal sealed class SOkOVpcmhaJUHgnjLKLpLaoNynPm : hGomgYtnWolfdYhtqCWGrqELGMBn, IControllerTemplateStick6D, IControllerTemplateElement
		{
			private const int JgKXtrfdXyBLcxrznJMuQkrJNnld = 6;

			private SOkOVpcmhaJUHgnjLKLpLaoNynPm(IControllerTemplate_Internal P_0, int P_1, string P_2, string P_3, DPSMyxkASzarrLDVbAOjLPvrhhtq[] P_4)
				: base(P_0, P_1, ControllerTemplateElementType.Stick6D, P_4, DPSMyxkASzarrLDVbAOjLPvrhhtq.lifdeXJbzWkfqiCcaOMyELWULpyBA(P_0, P_1, P_2, P_3))
			{
			}

			internal SOkOVpcmhaJUHgnjLKLpLaoNynPm(IControllerTemplate_Internal P_0, int P_1, string P_2, string P_3, RZNDjkbvHOEbvhWYTRUapEJHWiOu P_4, RZNDjkbvHOEbvhWYTRUapEJHWiOu P_5, RZNDjkbvHOEbvhWYTRUapEJHWiOu P_6, RZNDjkbvHOEbvhWYTRUapEJHWiOu P_7, RZNDjkbvHOEbvhWYTRUapEJHWiOu P_8, RZNDjkbvHOEbvhWYTRUapEJHWiOu P_9)
				: this(P_0, P_1, P_2, P_3, new DPSMyxkASzarrLDVbAOjLPvrhhtq[6] { P_4, P_5, P_6, P_7, P_8, P_9 })
			{
			}
		}

		internal class miYoADVkblEyjEtgJEzTqnbvebgPA
		{
			public readonly Controller.Element zHGTQktLKhMnxFlTIuFPsfSsFhHA;

			public readonly IControllerElementTarget gzssxMidtoeZTfGWPiKnbrudBysub;

			public bool wcbtUjICGPboMarNZtqcvWSmcmby
			{
				get
				{
					if (zHGTQktLKhMnxFlTIuFPsfSsFhHA == null)
					{
						return false;
					}
					switch (zHGTQktLKhMnxFlTIuFPsfSsFhHA.type)
					{
					case ControllerElementType.Button:
						return (zHGTQktLKhMnxFlTIuFPsfSsFhHA as Controller.Button).value;
					case ControllerElementType.Axis:
					{
						float value = (zHGTQktLKhMnxFlTIuFPsfSsFhHA as Controller.Axis).value;
						switch (gzssxMidtoeZTfGWPiKnbrudBysub.axisRange)
						{
						case AxisRange.Full:
							if (value > 0.01f)
							{
								return true;
							}
							if (value < -0.01f)
							{
								return true;
							}
							break;
						case AxisRange.Positive:
							if (value > 0.01f)
							{
								return true;
							}
							break;
						case AxisRange.Negative:
							if (value < -0.01f)
							{
								return true;
							}
							break;
						}
						break;
					}
					}
					return false;
				}
			}

			public bool gxIxsogcCjxNsRlndyaYlOZIYnfN
			{
				get
				{
					if (zHGTQktLKhMnxFlTIuFPsfSsFhHA == null)
					{
						return false;
					}
					switch (zHGTQktLKhMnxFlTIuFPsfSsFhHA.type)
					{
					case ControllerElementType.Button:
						return (zHGTQktLKhMnxFlTIuFPsfSsFhHA as Controller.Button).valuePrev;
					case ControllerElementType.Axis:
					{
						float valuePrev = (zHGTQktLKhMnxFlTIuFPsfSsFhHA as Controller.Axis).valuePrev;
						switch (gzssxMidtoeZTfGWPiKnbrudBysub.axisRange)
						{
						case AxisRange.Full:
							if (valuePrev > 0.01f)
							{
								return true;
							}
							if (valuePrev < -0.01f)
							{
								return true;
							}
							break;
						case AxisRange.Positive:
							if (valuePrev > 0.01f)
							{
								return true;
							}
							break;
						case AxisRange.Negative:
							if (valuePrev < -0.01f)
							{
								return true;
							}
							break;
						}
						break;
					}
					}
					return false;
				}
			}

			public bool DDvFvYetWkGPEPbHSgpRYPblCbje
			{
				get
				{
					if (zHGTQktLKhMnxFlTIuFPsfSsFhHA == null)
					{
						return false;
					}
					switch (zHGTQktLKhMnxFlTIuFPsfSsFhHA.type)
					{
					case ControllerElementType.Button:
						return (zHGTQktLKhMnxFlTIuFPsfSsFhHA as Controller.Button).justPressed;
					case ControllerElementType.Axis:
						if (MathTools.Abs(ocVeAgdLlOJhUguJXXZWkdByjITmA) > 0.01f && MathTools.Abs(SgLrkJfbtoCAeNlACFDDfhgvgMfZ) <= 0.01f)
						{
							return true;
						}
						break;
					}
					return false;
				}
			}

			public bool aKuxZnEjETiDBPmNDQDCnfUnUSSs
			{
				get
				{
					if (zHGTQktLKhMnxFlTIuFPsfSsFhHA == null)
					{
						return false;
					}
					switch (zHGTQktLKhMnxFlTIuFPsfSsFhHA.type)
					{
					case ControllerElementType.Button:
						return (zHGTQktLKhMnxFlTIuFPsfSsFhHA as Controller.Button).justReleased;
					case ControllerElementType.Axis:
						if (MathTools.Abs(ocVeAgdLlOJhUguJXXZWkdByjITmA) <= 0.01f && MathTools.Abs(SgLrkJfbtoCAeNlACFDDfhgvgMfZ) > 0.01f)
						{
							return true;
						}
						break;
					}
					return false;
				}
			}

			public float ocVeAgdLlOJhUguJXXZWkdByjITmA
			{
				get
				{
					if (zHGTQktLKhMnxFlTIuFPsfSsFhHA == null)
					{
						return 0f;
					}
					switch (zHGTQktLKhMnxFlTIuFPsfSsFhHA.type)
					{
					case ControllerElementType.Button:
						if (!(zHGTQktLKhMnxFlTIuFPsfSsFhHA as Controller.Button).value)
						{
							return 0f;
						}
						return 1f;
					case ControllerElementType.Axis:
					{
						float value = (zHGTQktLKhMnxFlTIuFPsfSsFhHA as Controller.Axis).value;
						switch (gzssxMidtoeZTfGWPiKnbrudBysub.axisRange)
						{
						case AxisRange.Full:
							return value;
						case AxisRange.Positive:
							if (value > 0f)
							{
								return value;
							}
							break;
						case AxisRange.Negative:
							if (value < 0f)
							{
								return value;
							}
							break;
						}
						break;
					}
					}
					return 0f;
				}
			}

			public float SgLrkJfbtoCAeNlACFDDfhgvgMfZ
			{
				get
				{
					if (zHGTQktLKhMnxFlTIuFPsfSsFhHA == null)
					{
						return 0f;
					}
					switch (zHGTQktLKhMnxFlTIuFPsfSsFhHA.type)
					{
					case ControllerElementType.Button:
						if (!(zHGTQktLKhMnxFlTIuFPsfSsFhHA as Controller.Button).valuePrev)
						{
							return 0f;
						}
						return 1f;
					case ControllerElementType.Axis:
					{
						float valuePrev = (zHGTQktLKhMnxFlTIuFPsfSsFhHA as Controller.Axis).valuePrev;
						switch (gzssxMidtoeZTfGWPiKnbrudBysub.axisRange)
						{
						case AxisRange.Full:
							return valuePrev;
						case AxisRange.Positive:
							if (valuePrev > 0f)
							{
								return valuePrev;
							}
							break;
						case AxisRange.Negative:
							if (valuePrev < 0f)
							{
								return valuePrev;
							}
							break;
						}
						break;
					}
					}
					return 0f;
				}
			}

			public miYoADVkblEyjEtgJEzTqnbvebgPA(IControllerElementTarget P_0, Controller.Element P_1)
			{
				zHGTQktLKhMnxFlTIuFPsfSsFhHA = P_1;
				gzssxMidtoeZTfGWPiKnbrudBysub = P_0;
			}

			public static miYoADVkblEyjEtgJEzTqnbvebgPA JAdIxiaMatclzxjuezgRNGXKLgfW()
			{
				return new miYoADVkblEyjEtgJEzTqnbvebgPA(SzcVmbDpoJahYmnXXukLaOXfCanz.MoUHBwpcMangYCquFpcvJDNGaBMD(), null);
			}
		}

		internal class OdAKxDscjuzVRrIHPbBVncTTNlyo
		{
			public readonly Controller MAsTchZmeXruANrVsALgzPFkHFUX;

			public readonly IHardwareControllerTemplateMap_Internal xIHkkvSRgQEHFfVOoOXdaQWHdoWi;

			public OdAKxDscjuzVRrIHPbBVncTTNlyo(Controller P_0, IHardwareControllerTemplateMap_Internal P_1)
			{
				if (P_0 == null)
				{
					throw new ArgumentNullException("controller");
				}
				if (P_1 == null)
				{
					throw new ArgumentNullException("templateMap");
				}
				MAsTchZmeXruANrVsALgzPFkHFUX = P_0;
				xIHkkvSRgQEHFfVOoOXdaQWHdoWi = P_1;
			}
		}

		private sealed class tgEJXVjqgYQGLEfNpszPQLpUhWeE
		{
			[Serializable]
			private sealed class vkkjtiHEIDakYEsxQgOutHsVTWQs
			{
				public static readonly vkkjtiHEIDakYEsxQgOutHsVTWQs _003C_003E9 = new vkkjtiHEIDakYEsxQgOutHsVTWQs();

				public static Func<PNpsspmaeIckaEYciaSqXgOdyvotA, PNpsspmaeIckaEYciaSqXgOdyvotA, bool> _003C_003E9__4_0;

				internal bool BkvGiFnWJtCrMhbOYDNCOygBLObs(PNpsspmaeIckaEYciaSqXgOdyvotA P_0, PNpsspmaeIckaEYciaSqXgOdyvotA P_1)
				{
					if (P_0 == null || P_1 == null)
					{
						return false;
					}
					return P_0.HigUEXRMHDCBxDAIsCfdHIKAlrTJ(P_1, false);
				}
			}

			private static tgEJXVjqgYQGLEfNpszPQLpUhWeE fAtewkbCOhQboNwwtyolFizBMsmgb;

			private readonly global::JZpDMkieCQWHnBUBlnLvccwpXKuI<PNpsspmaeIckaEYciaSqXgOdyvotA> cWGoxXEuhssXeeOtBdtUdIGAyqfJ;

			private static tgEJXVjqgYQGLEfNpszPQLpUhWeE vpvKKqRjfHecrSKXQRWUuuRgdIvs
			{
				get
				{
					if (fAtewkbCOhQboNwwtyolFizBMsmgb != null)
					{
						return fAtewkbCOhQboNwwtyolFizBMsmgb;
					}
					fAtewkbCOhQboNwwtyolFizBMsmgb = new tgEJXVjqgYQGLEfNpszPQLpUhWeE();
					fAtewkbCOhQboNwwtyolFizBMsmgb.UgPEDvcVuuOkhAhlIPqPSeKFkuZj();
					return fAtewkbCOhQboNwwtyolFizBMsmgb;
				}
			}

			private tgEJXVjqgYQGLEfNpszPQLpUhWeE()
			{
				cWGoxXEuhssXeeOtBdtUdIGAyqfJ = new global::JZpDMkieCQWHnBUBlnLvccwpXKuI<PNpsspmaeIckaEYciaSqXgOdyvotA>(vkkjtiHEIDakYEsxQgOutHsVTWQs._003C_003E9.BkvGiFnWJtCrMhbOYDNCOygBLObs);
			}

			private void UgPEDvcVuuOkhAhlIPqPSeKFkuZj()
			{
				ReInput.ShutDownEvent += fAtewkbCOhQboNwwtyolFizBMsmgb.jsGWGBoTZaLHBLeseeRATLZNEUtDA;
			}

			private void jsGWGBoTZaLHBLeseeRATLZNEUtDA()
			{
				if (fAtewkbCOhQboNwwtyolFizBMsmgb == this)
				{
					fAtewkbCOhQboNwwtyolFizBMsmgb = null;
				}
				ReInput.ShutDownEvent -= jsGWGBoTZaLHBLeseeRATLZNEUtDA;
			}

			public static PNpsspmaeIckaEYciaSqXgOdyvotA RBoAIeJFHlIncqSOQlAFyhiztwBtA(PNpsspmaeIckaEYciaSqXgOdyvotA P_0)
			{
				Bytes20 bytes = ((P_0.SEmUEYhFCOinEboyOFclOuvyHfqgb is cBQeMhYqRgOCwlnJCsFnDXPZyIWh cBQeMhYqRgOCwlnJCsFnDXPZyIWh2) ? cBQeMhYqRgOCwlnJCsFnDXPZyIWh2.NoNdGdFDpUCLxyUmgslGWEGfuOYG.hash : default(Bytes20));
				return vpvKKqRjfHecrSKXQRWUuuRgdIvs.cWGoxXEuhssXeeOtBdtUdIGAyqfJ.gOGhAlpaDWHngyOvcwqBdSVIJQot(bytes, P_0);
			}

			public static bool BLeUgsfurTukGDdsaHSOfYnmtywc(PNpsspmaeIckaEYciaSqXgOdyvotA P_0, out PNpsspmaeIckaEYciaSqXgOdyvotA P_1)
			{
				Bytes20 bytes = ((P_0.SEmUEYhFCOinEboyOFclOuvyHfqgb is cBQeMhYqRgOCwlnJCsFnDXPZyIWh cBQeMhYqRgOCwlnJCsFnDXPZyIWh2) ? cBQeMhYqRgOCwlnJCsFnDXPZyIWh2.NoNdGdFDpUCLxyUmgslGWEGfuOYG.hash : default(Bytes20));
				return vpvKKqRjfHecrSKXQRWUuuRgdIvs.cWGoxXEuhssXeeOtBdtUdIGAyqfJ.UCVjRYAbAROoXGysMPODoXVTfPnm(bytes, P_0, out P_1);
			}

			public static void peqtYrWKSkQRPTHcIlqMTwoqWDHo(PNpsspmaeIckaEYciaSqXgOdyvotA P_0)
			{
				Bytes20 bytes = ((P_0.SEmUEYhFCOinEboyOFclOuvyHfqgb is cBQeMhYqRgOCwlnJCsFnDXPZyIWh cBQeMhYqRgOCwlnJCsFnDXPZyIWh2) ? cBQeMhYqRgOCwlnJCsFnDXPZyIWh2.NoNdGdFDpUCLxyUmgslGWEGfuOYG.hash : default(Bytes20));
				vpvKKqRjfHecrSKXQRWUuuRgdIvs.cWGoxXEuhssXeeOtBdtUdIGAyqfJ.jwZfZMJFGsXepvnbcdwNeIsRDvtv(bytes, P_0);
			}
		}

		private const string grSQoJJdofygetLmTlhTwRjnDsuN = "controller/template";

		private string KIScItCTfnCOOFKmjnjbnlwqEWtJ;

		private string WkpNjJoPgxCsNnWWPEzDtsrGvGIg;

		private int bHBDdxImSGYiRbixbmokNJNNjfHN;

		private readonly Guid aCdSDBlKLoLTiXoolGMRdshkPNjqA;

		private readonly DeviceLocalizationInfo fFCOrrdLDuZqUQZFZuBPrceZyrgS;

		private readonly Controller yjjfTzNlhNRpabESdtooKwmvhMK;

		private readonly ADictionary<int, IControllerTemplateElement> GdIKqzolwsMeRARjSVbgVgmEPqSn;

		private readonly ADictionary<string, IControllerTemplateElement> hxrCRzlMeWiDIyxXNElPtadNHJdBA;

		private IControllerTemplateElement[] stnSkDINQXKfFBNfJdSaQJDLSWQe;

		private ReadOnlyCollection<IControllerTemplateElement> ObdsddvdByawJopSdNSlolKXyaFx;

		private readonly uDMHJlZGqPMWeMVCxldavZLGKMck WTPqjfHlyJnysiqirJlbWZoerCnN;

		private readonly int HujKpPcPNqHbRjJVxKVxjSvlVteu;

		internal DeviceLocalizationInfo DNGuhkdrnJEqKpprcBksGezorUwc => fFCOrrdLDuZqUQZFZuBPrceZyrgS;

		DeviceLocalizationInfo IControllerTemplate_Internal.deviceLocalizationInfo => fFCOrrdLDuZqUQZFZuBPrceZyrgS;

		Controller IControllerTemplate.controller
		{
			get
			{
				if (ReInput._id != HujKpPcPNqHbRjJVxKVxjSvlVteu)
				{
					ReInput.CheckInitialized(HujKpPcPNqHbRjJVxKVxjSvlVteu);
					return null;
				}
				return yjjfTzNlhNRpabESdtooKwmvhMK;
			}
		}

		string IControllerTemplate.name
		{
			get
			{
				if (ReInput._id != HujKpPcPNqHbRjJVxKVxjSvlVteu)
				{
					ReInput.CheckInitialized(HujKpPcPNqHbRjJVxKVxjSvlVteu);
					return null;
				}
				if (!LocalizationManager.isEnabled)
				{
					return KIScItCTfnCOOFKmjnjbnlwqEWtJ;
				}
				return WTPqjfHlyJnysiqirJlbWZoerCnN.MpfwJMTclVnnxEuHhBPCmlxJadkBA;
			}
		}

		Guid IControllerTemplate.typeGuid
		{
			get
			{
				if (ReInput._id != HujKpPcPNqHbRjJVxKVxjSvlVteu)
				{
					ReInput.CheckInitialized(HujKpPcPNqHbRjJVxKVxjSvlVteu);
					return Guid.Empty;
				}
				return aCdSDBlKLoLTiXoolGMRdshkPNjqA;
			}
		}

		IList<IControllerTemplateElement> IControllerTemplate.elements
		{
			get
			{
				if (ReInput._id != HujKpPcPNqHbRjJVxKVxjSvlVteu)
				{
					ReInput.CheckInitialized(HujKpPcPNqHbRjJVxKVxjSvlVteu);
					return null;
				}
				return ObdsddvdByawJopSdNSlolKXyaFx;
			}
		}

		int IControllerTemplate.elementCount
		{
			get
			{
				if (ReInput._id != HujKpPcPNqHbRjJVxKVxjSvlVteu)
				{
					ReInput.CheckInitialized(HujKpPcPNqHbRjJVxKVxjSvlVteu);
					return 0;
				}
				return stnSkDINQXKfFBNfJdSaQJDLSWQe.Length;
			}
		}

		string bguKJVtsagJfXPpJQeurpzlOLIYd.keyCategory => "controller/template";

		string bguKJVtsagJfXPpJQeurpzlOLIYd.scriptingName => string.Empty;

		string bguKJVtsagJfXPpJQeurpzlOLIYd.nonLocalizedDescriptiveName
		{
			get
			{
				return KIScItCTfnCOOFKmjnjbnlwqEWtJ;
			}
			set
			{
				KIScItCTfnCOOFKmjnjbnlwqEWtJ = value;
			}
		}

		string bguKJVtsagJfXPpJQeurpzlOLIYd.key => WkpNjJoPgxCsNnWWPEzDtsrGvGIg;

		int bguKJVtsagJfXPpJQeurpzlOLIYd.autoGeneratedValueFlags
		{
			get
			{
				return bHBDdxImSGYiRbixbmokNJNNjfHN;
			}
			set
			{
				bHBDdxImSGYiRbixbmokNJNNjfHN = value;
			}
		}

		protected ControllerTemplate(object P_0)
			: this((OdAKxDscjuzVRrIHPbBVncTTNlyo)P_0)
		{
		}

		private ControllerTemplate(OdAKxDscjuzVRrIHPbBVncTTNlyo P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("initializer");
			}
			if (P_0.MAsTchZmeXruANrVsALgzPFkHFUX == null)
			{
				throw new ArgumentNullException("initializer.controller");
			}
			if (P_0.xIHkkvSRgQEHFfVOoOXdaQWHdoWi == null)
			{
				throw new ArgumentNullException("initializer.templateMap");
			}
			HujKpPcPNqHbRjJVxKVxjSvlVteu = ReInput.id;
			yjjfTzNlhNRpabESdtooKwmvhMK = P_0.MAsTchZmeXruANrVsALgzPFkHFUX;
			IHardwareControllerTemplateMap_Internal xIHkkvSRgQEHFfVOoOXdaQWHdoWi = P_0.xIHkkvSRgQEHFfVOoOXdaQWHdoWi;
			KIScItCTfnCOOFKmjnjbnlwqEWtJ = xIHkkvSRgQEHFfVOoOXdaQWHdoWi.name;
			WkpNjJoPgxCsNnWWPEzDtsrGvGIg = xIHkkvSRgQEHFfVOoOXdaQWHdoWi.typeKey;
			aCdSDBlKLoLTiXoolGMRdshkPNjqA = xIHkkvSRgQEHFfVOoOXdaQWHdoWi.typeGuid;
			fFCOrrdLDuZqUQZFZuBPrceZyrgS = new DeviceLocalizationInfo(yjjfTzNlhNRpabESdtooKwmvhMK.type, true, aCdSDBlKLoLTiXoolGMRdshkPNjqA, new List<string> { xIHkkvSRgQEHFfVOoOXdaQWHdoWi.typeKey }, null);
			fFCOrrdLDuZqUQZFZuBPrceZyrgS.FinishRuntimeSetup();
			WTPqjfHlyJnysiqirJlbWZoerCnN = uDMHJlZGqPMWeMVCxldavZLGKMck.KoYuvhvHwZHOiMEpyUHHifMSYsLO(this);
			int elementIdentifierCount = xIHkkvSRgQEHFfVOoOXdaQWHdoWi.GetElementIdentifierCount();
			ADictionary<int, IControllerTemplateElement> aDictionary = new ADictionary<int, IControllerTemplateElement>();
			List<IControllerTemplateElement> list = new List<IControllerTemplateElement>();
			List<IControllerTemplateAxis> list2 = new List<IControllerTemplateAxis>();
			List<IControllerTemplateButton> list3 = new List<IControllerTemplateButton>();
			List<IControllerTemplateElement> list4 = new List<IControllerTemplateElement>();
			for (int i = 0; i < elementIdentifierCount; i++)
			{
				IControllerTemplateElementIdentifier templateElementIdentifier = xIHkkvSRgQEHFfVOoOXdaQWHdoWi.GetTemplateElementIdentifier(i);
				if (templateElementIdentifier != null && InputTools.IsMappableType(templateElementIdentifier.elementType))
				{
					switch (templateElementIdentifier.elementType)
					{
					case ControllerTemplateElementType.Axis:
					{
						xcslkxDzwrCojABLPbRuUvYdnRhl xcslkxDzwrCojABLPbRuUvYdnRhl3 = xIHkkvSRgQEHFfVOoOXdaQWHdoWi.GetAxisTarget(yjjfTzNlhNRpabESdtooKwmvhMK, templateElementIdentifier.id) ?? xcslkxDzwrCojABLPbRuUvYdnRhl.szNaYxvmrjQKoEMODBrltQFmmKyi(ControllerTemplateElementType.Axis);
						wEDpgRYkJezclVWPwPJlOnrGQLHE item2 = new wEDpgRYkJezclVWPwPJlOnrGQLHE(this, templateElementIdentifier.id, templateElementIdentifier.nonLocalizedName, (!templateElementIdentifier.isNonLocalizedPositiveNameAutoGenerated) ? templateElementIdentifier.nonLocalizedPositiveName : string.Empty, (!templateElementIdentifier.isNonLocalizedNegativeNameAutoGenerated) ? templateElementIdentifier.nonLocalizedNegativeName : string.Empty, templateElementIdentifier.key, (!templateElementIdentifier.isPositiveKeyAutoGenerated) ? templateElementIdentifier.positiveKey : string.Empty, (!templateElementIdentifier.isNegativeKeyAutoGenerated) ? templateElementIdentifier.negativeKey : string.Empty, xcslkxDzwrCojABLPbRuUvYdnRhl3, GDKbPceIBQNrGGySkMVbSoKopZHU(yjjfTzNlhNRpabESdtooKwmvhMK, xcslkxDzwrCojABLPbRuUvYdnRhl3));
						list2.Add(item2);
						break;
					}
					case ControllerTemplateElementType.Button:
					{
						xcslkxDzwrCojABLPbRuUvYdnRhl xcslkxDzwrCojABLPbRuUvYdnRhl2 = xIHkkvSRgQEHFfVOoOXdaQWHdoWi.GetButtonTarget(yjjfTzNlhNRpabESdtooKwmvhMK, templateElementIdentifier.id) ?? xcslkxDzwrCojABLPbRuUvYdnRhl.szNaYxvmrjQKoEMODBrltQFmmKyi(ControllerTemplateElementType.Button);
						BShaaIVTHefYkCCCxsFLxwxLvTMk item = new BShaaIVTHefYkCCCxsFLxwxLvTMk(this, templateElementIdentifier.id, templateElementIdentifier.nonLocalizedName, (!templateElementIdentifier.isNonLocalizedPositiveNameAutoGenerated) ? templateElementIdentifier.nonLocalizedPositiveName : string.Empty, (!templateElementIdentifier.isNonLocalizedNegativeNameAutoGenerated) ? templateElementIdentifier.nonLocalizedNegativeName : string.Empty, templateElementIdentifier.key, (!templateElementIdentifier.isPositiveKeyAutoGenerated) ? templateElementIdentifier.positiveKey : string.Empty, (!templateElementIdentifier.isNegativeKeyAutoGenerated) ? templateElementIdentifier.negativeKey : string.Empty, xcslkxDzwrCojABLPbRuUvYdnRhl2, XSDALdYIRCVHpQaLaoKRPzrXfLVA(yjjfTzNlhNRpabESdtooKwmvhMK, xcslkxDzwrCojABLPbRuUvYdnRhl2));
						list3.Add(item);
						break;
					}
					default:
						throw new NotImplementedException();
					}
				}
			}
			for (int j = 0; j < list2.Count; j++)
			{
				list.Add(list2[j]);
			}
			for (int k = 0; k < list3.Count; k++)
			{
				list.Add(list3[k]);
			}
			for (int l = 0; l < list.Count; l++)
			{
				aDictionary.Add(list[l].id, list[l]);
			}
			for (int m = 0; m < elementIdentifierCount; m++)
			{
				IControllerTemplateElementIdentifier templateElementIdentifier2 = xIHkkvSRgQEHFfVOoOXdaQWHdoWi.GetTemplateElementIdentifier(m);
				if (templateElementIdentifier2 == null || InputTools.IsMappableType(templateElementIdentifier2.elementType))
				{
					continue;
				}
				IControllerTemplateMapSpecialElement_Internal specialTemplateElementByElementIdentifierId = xIHkkvSRgQEHFfVOoOXdaQWHdoWi.GetSpecialTemplateElementByElementIdentifierId(templateElementIdentifier2.id);
				DPSMyxkASzarrLDVbAOjLPvrhhtq dPSMyxkASzarrLDVbAOjLPvrhhtq;
				switch (templateElementIdentifier2.elementType)
				{
				case ControllerTemplateElementType.ThumbStick:
				{
					if (specialTemplateElementByElementIdentifierId == null)
					{
						Logger.LogError(templateElementIdentifier2.elementType.ToString() + " element missing for Element Identifier Id " + templateElementIdentifier2.id);
					}
					ControllerTemplateThumbStickMapping mapping5 = specialTemplateElementByElementIdentifierId.GetMapping<ControllerTemplateThumbStickMapping>();
					dPSMyxkASzarrLDVbAOjLPvrhhtq = new qcDscyXMOxZMVECthAEVcLYTrmfFA(this, templateElementIdentifier2.id, templateElementIdentifier2.nonLocalizedName, templateElementIdentifier2.key, (mapping5 != null) ? fsykLgQOrXBgAfjTJkurpbRKedpgA(this, aDictionary, mapping5.eid_axisX) : wEDpgRYkJezclVWPwPJlOnrGQLHE.pNeCOamePGsLRrhAkHfuRGxROIqc(this), (mapping5 != null) ? fsykLgQOrXBgAfjTJkurpbRKedpgA(this, aDictionary, mapping5.eid_axisY) : wEDpgRYkJezclVWPwPJlOnrGQLHE.pNeCOamePGsLRrhAkHfuRGxROIqc(this), (mapping5 != null) ? LuEhvQwaPnRWGxjJXLJAdcmenECs(this, aDictionary, mapping5.eid_button) : BShaaIVTHefYkCCCxsFLxwxLvTMk.SkAnIcKlidTGodlqxURKePAzlVlp(this));
					break;
				}
				case ControllerTemplateElementType.DPad:
				{
					if (specialTemplateElementByElementIdentifierId == null)
					{
						Logger.LogError(templateElementIdentifier2.elementType.ToString() + " element missing for Element Identifier Id " + templateElementIdentifier2.id);
					}
					ControllerTemplateDPadMapping mapping3 = specialTemplateElementByElementIdentifierId.GetMapping<ControllerTemplateDPadMapping>();
					dPSMyxkASzarrLDVbAOjLPvrhhtq = new SliKgniFbePJsGyYVBvMEPUoYIzw(this, templateElementIdentifier2.id, templateElementIdentifier2.nonLocalizedName, templateElementIdentifier2.key, (mapping3 != null) ? LuEhvQwaPnRWGxjJXLJAdcmenECs(this, aDictionary, mapping3.eid_up) : BShaaIVTHefYkCCCxsFLxwxLvTMk.SkAnIcKlidTGodlqxURKePAzlVlp(this), (mapping3 != null) ? LuEhvQwaPnRWGxjJXLJAdcmenECs(this, aDictionary, mapping3.eid_right) : BShaaIVTHefYkCCCxsFLxwxLvTMk.SkAnIcKlidTGodlqxURKePAzlVlp(this), (mapping3 != null) ? LuEhvQwaPnRWGxjJXLJAdcmenECs(this, aDictionary, mapping3.eid_down) : BShaaIVTHefYkCCCxsFLxwxLvTMk.SkAnIcKlidTGodlqxURKePAzlVlp(this), (mapping3 != null) ? LuEhvQwaPnRWGxjJXLJAdcmenECs(this, aDictionary, mapping3.eid_left) : BShaaIVTHefYkCCCxsFLxwxLvTMk.SkAnIcKlidTGodlqxURKePAzlVlp(this), (mapping3 != null) ? LuEhvQwaPnRWGxjJXLJAdcmenECs(this, aDictionary, mapping3.eid_press) : BShaaIVTHefYkCCCxsFLxwxLvTMk.SkAnIcKlidTGodlqxURKePAzlVlp(this));
					break;
				}
				case ControllerTemplateElementType.Stick:
				{
					if (specialTemplateElementByElementIdentifierId == null)
					{
						Logger.LogError(templateElementIdentifier2.elementType.ToString() + " element missing for Element Identifier Id " + templateElementIdentifier2.id);
					}
					ControllerTemplateStickMapping mapping2 = specialTemplateElementByElementIdentifierId.GetMapping<ControllerTemplateStickMapping>();
					dPSMyxkASzarrLDVbAOjLPvrhhtq = new bBeHCAvUbWuNvvSIEzTQGAlChFUL(this, templateElementIdentifier2.id, templateElementIdentifier2.nonLocalizedName, templateElementIdentifier2.key, (mapping2 != null) ? fsykLgQOrXBgAfjTJkurpbRKedpgA(this, aDictionary, mapping2.eid_axisX) : wEDpgRYkJezclVWPwPJlOnrGQLHE.pNeCOamePGsLRrhAkHfuRGxROIqc(this), (mapping2 != null) ? fsykLgQOrXBgAfjTJkurpbRKedpgA(this, aDictionary, mapping2.eid_axisY) : wEDpgRYkJezclVWPwPJlOnrGQLHE.pNeCOamePGsLRrhAkHfuRGxROIqc(this), (mapping2 != null) ? fsykLgQOrXBgAfjTJkurpbRKedpgA(this, aDictionary, mapping2.eid_axisZ) : wEDpgRYkJezclVWPwPJlOnrGQLHE.pNeCOamePGsLRrhAkHfuRGxROIqc(this));
					break;
				}
				case ControllerTemplateElementType.Throttle:
				{
					if (specialTemplateElementByElementIdentifierId == null)
					{
						Logger.LogError(templateElementIdentifier2.elementType.ToString() + " element missing for Element Identifier Id " + templateElementIdentifier2.id);
					}
					ControllerTemplateThrottleMapping mapping6 = specialTemplateElementByElementIdentifierId.GetMapping<ControllerTemplateThrottleMapping>();
					dPSMyxkASzarrLDVbAOjLPvrhhtq = new ZhUbNCvNLygNgeiSvJTuRTUgdRAB(this, templateElementIdentifier2.id, templateElementIdentifier2.nonLocalizedName, templateElementIdentifier2.key, (mapping6 != null) ? fsykLgQOrXBgAfjTJkurpbRKedpgA(this, aDictionary, mapping6.eid_axis) : wEDpgRYkJezclVWPwPJlOnrGQLHE.pNeCOamePGsLRrhAkHfuRGxROIqc(this), (mapping6 != null) ? LuEhvQwaPnRWGxjJXLJAdcmenECs(this, aDictionary, mapping6.eid_minDetent) : BShaaIVTHefYkCCCxsFLxwxLvTMk.SkAnIcKlidTGodlqxURKePAzlVlp(this));
					break;
				}
				case ControllerTemplateElementType.Hat:
				{
					if (specialTemplateElementByElementIdentifierId == null)
					{
						Logger.LogError(templateElementIdentifier2.elementType.ToString() + " element missing for Element Identifier Id " + templateElementIdentifier2.id);
					}
					ControllerTemplateHatMapping mapping7 = specialTemplateElementByElementIdentifierId.GetMapping<ControllerTemplateHatMapping>();
					dPSMyxkASzarrLDVbAOjLPvrhhtq = new uKwdxDCUNWFtWJyMkiqclARhnTxm(this, templateElementIdentifier2.id, templateElementIdentifier2.nonLocalizedName, templateElementIdentifier2.key, (mapping7 != null) ? LuEhvQwaPnRWGxjJXLJAdcmenECs(this, aDictionary, mapping7.eid_up) : BShaaIVTHefYkCCCxsFLxwxLvTMk.SkAnIcKlidTGodlqxURKePAzlVlp(this), (mapping7 != null) ? LuEhvQwaPnRWGxjJXLJAdcmenECs(this, aDictionary, mapping7.eid_upRight) : BShaaIVTHefYkCCCxsFLxwxLvTMk.SkAnIcKlidTGodlqxURKePAzlVlp(this), (mapping7 != null) ? LuEhvQwaPnRWGxjJXLJAdcmenECs(this, aDictionary, mapping7.eid_right) : BShaaIVTHefYkCCCxsFLxwxLvTMk.SkAnIcKlidTGodlqxURKePAzlVlp(this), (mapping7 != null) ? LuEhvQwaPnRWGxjJXLJAdcmenECs(this, aDictionary, mapping7.eid_downRight) : BShaaIVTHefYkCCCxsFLxwxLvTMk.SkAnIcKlidTGodlqxURKePAzlVlp(this), (mapping7 != null) ? LuEhvQwaPnRWGxjJXLJAdcmenECs(this, aDictionary, mapping7.eid_down) : BShaaIVTHefYkCCCxsFLxwxLvTMk.SkAnIcKlidTGodlqxURKePAzlVlp(this), (mapping7 != null) ? LuEhvQwaPnRWGxjJXLJAdcmenECs(this, aDictionary, mapping7.eid_downLeft) : BShaaIVTHefYkCCCxsFLxwxLvTMk.SkAnIcKlidTGodlqxURKePAzlVlp(this), (mapping7 != null) ? LuEhvQwaPnRWGxjJXLJAdcmenECs(this, aDictionary, mapping7.eid_left) : BShaaIVTHefYkCCCxsFLxwxLvTMk.SkAnIcKlidTGodlqxURKePAzlVlp(this), (mapping7 != null) ? LuEhvQwaPnRWGxjJXLJAdcmenECs(this, aDictionary, mapping7.eid_upLeft) : BShaaIVTHefYkCCCxsFLxwxLvTMk.SkAnIcKlidTGodlqxURKePAzlVlp(this));
					break;
				}
				case ControllerTemplateElementType.Yoke:
				{
					if (specialTemplateElementByElementIdentifierId == null)
					{
						Logger.LogError(templateElementIdentifier2.elementType.ToString() + " element missing for Element Identifier Id " + templateElementIdentifier2.id);
					}
					ControllerTemplateYokeMapping mapping4 = specialTemplateElementByElementIdentifierId.GetMapping<ControllerTemplateYokeMapping>();
					dPSMyxkASzarrLDVbAOjLPvrhhtq = new inxZkaEHmZUyFfVMDQJuFqMbNRMr(this, templateElementIdentifier2.id, templateElementIdentifier2.nonLocalizedName, templateElementIdentifier2.key, (mapping4 != null) ? fsykLgQOrXBgAfjTJkurpbRKedpgA(this, aDictionary, mapping4.eid_axisX) : wEDpgRYkJezclVWPwPJlOnrGQLHE.pNeCOamePGsLRrhAkHfuRGxROIqc(this), (mapping4 != null) ? fsykLgQOrXBgAfjTJkurpbRKedpgA(this, aDictionary, mapping4.eid_axisZ) : wEDpgRYkJezclVWPwPJlOnrGQLHE.pNeCOamePGsLRrhAkHfuRGxROIqc(this));
					break;
				}
				case ControllerTemplateElementType.Stick6D:
				{
					if (specialTemplateElementByElementIdentifierId == null)
					{
						Logger.LogError(templateElementIdentifier2.elementType.ToString() + " element missing for Element Identifier Id " + templateElementIdentifier2.id);
					}
					ControllerTemplateStick6DMapping mapping = specialTemplateElementByElementIdentifierId.GetMapping<ControllerTemplateStick6DMapping>();
					dPSMyxkASzarrLDVbAOjLPvrhhtq = new SOkOVpcmhaJUHgnjLKLpLaoNynPm(this, templateElementIdentifier2.id, templateElementIdentifier2.nonLocalizedName, templateElementIdentifier2.key, (mapping != null) ? fsykLgQOrXBgAfjTJkurpbRKedpgA(this, aDictionary, mapping.eid_positionX) : wEDpgRYkJezclVWPwPJlOnrGQLHE.pNeCOamePGsLRrhAkHfuRGxROIqc(this), (mapping != null) ? fsykLgQOrXBgAfjTJkurpbRKedpgA(this, aDictionary, mapping.eid_positionY) : wEDpgRYkJezclVWPwPJlOnrGQLHE.pNeCOamePGsLRrhAkHfuRGxROIqc(this), (mapping != null) ? fsykLgQOrXBgAfjTJkurpbRKedpgA(this, aDictionary, mapping.eid_positionZ) : wEDpgRYkJezclVWPwPJlOnrGQLHE.pNeCOamePGsLRrhAkHfuRGxROIqc(this), (mapping != null) ? fsykLgQOrXBgAfjTJkurpbRKedpgA(this, aDictionary, mapping.eid_rotationX) : wEDpgRYkJezclVWPwPJlOnrGQLHE.pNeCOamePGsLRrhAkHfuRGxROIqc(this), (mapping != null) ? fsykLgQOrXBgAfjTJkurpbRKedpgA(this, aDictionary, mapping.eid_rotationY) : wEDpgRYkJezclVWPwPJlOnrGQLHE.pNeCOamePGsLRrhAkHfuRGxROIqc(this), (mapping != null) ? fsykLgQOrXBgAfjTJkurpbRKedpgA(this, aDictionary, mapping.eid_rotationZ) : wEDpgRYkJezclVWPwPJlOnrGQLHE.pNeCOamePGsLRrhAkHfuRGxROIqc(this));
					break;
				}
				default:
					throw new NotImplementedException();
				}
				if (dPSMyxkASzarrLDVbAOjLPvrhhtq != null)
				{
					list4.Add(dPSMyxkASzarrLDVbAOjLPvrhhtq);
				}
			}
			for (int n = 0; n < list4.Count; n++)
			{
				list.Add(list4[n]);
				aDictionary.Add(list4[n].id, list4[n]);
			}
			stnSkDINQXKfFBNfJdSaQJDLSWQe = list.ToArray();
			GdIKqzolwsMeRARjSVbgVgmEPqSn = aDictionary;
			hxrCRzlMeWiDIyxXNElPtadNHJdBA = new ADictionary<string, IControllerTemplateElement>();
			for (int num = 0; num < stnSkDINQXKfFBNfJdSaQJDLSWQe.Length; num++)
			{
				if (!(xIHkkvSRgQEHFfVOoOXdaQWHdoWi.GetTemplateElementIdentifierById(stnSkDINQXKfFBNfJdSaQJDLSWQe[num].id) is IControllerTemplateElementIdentifier_Editor controllerTemplateElementIdentifier_Editor))
				{
					continue;
				}
				for (int num2 = 0; num2 < 2; num2++)
				{
					string text = ((num2 != 0) ? controllerTemplateElementIdentifier_Editor.alternateScriptingName : controllerTemplateElementIdentifier_Editor.scriptingName);
					if (!string.IsNullOrEmpty(text))
					{
						try
						{
							hxrCRzlMeWiDIyxXNElPtadNHJdBA.Add(text, stnSkDINQXKfFBNfJdSaQJDLSWQe[num]);
						}
						catch
						{
							Logger.LogError("A duplicate Controller Template element scripting name (" + text + ") was found in template " + KIScItCTfnCOOFKmjnjbnlwqEWtJ + ". This element should be renamed to a unique name.");
						}
					}
				}
			}
			ObdsddvdByawJopSdNSlolKXyaFx = new ReadOnlyCollection<IControllerTemplateElement>(stnSkDINQXKfFBNfJdSaQJDLSWQe);
		}

		protected IControllerTemplateElement GetElement(int id)
		{
			if (!GdIKqzolwsMeRARjSVbgVgmEPqSn.TryGetValue(id, out var value))
			{
				Logger.LogWarning("There is no element with the id \"" + id + "\" in the " + GetType().ToString() + ".");
			}
			return value;
		}

		protected T GetElement<T>(int id) where T : class, IControllerTemplateElement
		{
			return GetElement(id) as T;
		}

		IControllerTemplateElement IControllerTemplate.GetElement(int id)
		{
			if (ReInput._id != HujKpPcPNqHbRjJVxKVxjSvlVteu)
			{
				ReInput.CheckInitialized(HujKpPcPNqHbRjJVxKVxjSvlVteu);
				return null;
			}
			return GetElement(id);
		}

		T IControllerTemplate.GetElement<T>(int id)
		{
			if (ReInput._id != HujKpPcPNqHbRjJVxKVxjSvlVteu)
			{
				ReInput.CheckInitialized(HujKpPcPNqHbRjJVxKVxjSvlVteu);
				return null;
			}
			return GetElement<T>(id);
		}

		int IControllerTemplate.GetElementTargets(ControllerElementTarget find, IList<ControllerTemplateElementTarget> results)
		{
			if (ReInput._id != HujKpPcPNqHbRjJVxKVxjSvlVteu)
			{
				ReInput.CheckInitialized(HujKpPcPNqHbRjJVxKVxjSvlVteu);
				return 0;
			}
			if (results == null)
			{
				throw new ArgumentNullException("results");
			}
			return GetElementTargets(find, ref results);
		}

		private int GetElementTargets(ControllerElementTarget find, ref IList<ControllerTemplateElementTarget> results)
		{
			if (results != null)
			{
				results.Clear();
			}
			int num = 0;
			for (int i = 0; i < stnSkDINQXKfFBNfJdSaQJDLSWQe.Length; i++)
			{
				if (InputTools.IsMappableType(stnSkDINQXKfFBNfJdSaQJDLSWQe[i].type))
				{
					num += (stnSkDINQXKfFBNfJdSaQJDLSWQe[i] as IControllerTemplateElement_Internal).GetElementTargets(find, ref results);
				}
			}
			return num;
		}

		[CustomObfuscation(rename = false)]
		internal static Type GetInterfaceType(ControllerTemplateElementType elementType)
		{
			return elementType switch
			{
				ControllerTemplateElementType.Axis => typeof(IControllerTemplateAxis), 
				ControllerTemplateElementType.Button => typeof(IControllerTemplateButton), 
				ControllerTemplateElementType.ThumbStick => typeof(IControllerTemplateThumbStick), 
				ControllerTemplateElementType.DPad => typeof(IControllerTemplateDPad), 
				ControllerTemplateElementType.Stick => typeof(IControllerTemplateStick), 
				ControllerTemplateElementType.Throttle => typeof(IControllerTemplateThrottle), 
				ControllerTemplateElementType.Hat => typeof(IControllerTemplateHat), 
				ControllerTemplateElementType.Yoke => typeof(IControllerTemplateYoke), 
				ControllerTemplateElementType.Stick6D => typeof(IControllerTemplateStick6D), 
				_ => throw new NotImplementedException(), 
			};
		}

		private static IList<miYoADVkblEyjEtgJEzTqnbvebgPA> GDKbPceIBQNrGGySkMVbSoKopZHU(Controller P_0, IControllerTemplateAxisSource P_1)
		{
			if (P_1 == null)
			{
				return null;
			}
			if (P_1.splitAxis)
			{
				IList<miYoADVkblEyjEtgJEzTqnbvebgPA> list = null;
				bool flag = false;
				if (P_1.positiveTarget != null)
				{
					Controller.Element elementById = P_0.GetElementById(P_1.positiveTarget.elementIdentifierId);
					if (elementById != null)
					{
						ListTools.AddAndCreateList(ref list, new miYoADVkblEyjEtgJEzTqnbvebgPA(P_1.positiveTarget, elementById));
						flag = true;
					}
				}
				if (!flag)
				{
					ListTools.AddAndCreateList(ref list, miYoADVkblEyjEtgJEzTqnbvebgPA.JAdIxiaMatclzxjuezgRNGXKLgfW());
				}
				flag = false;
				if (P_1.negativeTarget != null)
				{
					Controller.Element elementById2 = P_0.GetElementById(P_1.negativeTarget.elementIdentifierId);
					if (elementById2 != null)
					{
						ListTools.AddAndCreateList(ref list, new miYoADVkblEyjEtgJEzTqnbvebgPA(P_1.negativeTarget, elementById2));
						flag = true;
					}
				}
				if (!flag)
				{
					ListTools.AddAndCreateList(ref list, miYoADVkblEyjEtgJEzTqnbvebgPA.JAdIxiaMatclzxjuezgRNGXKLgfW());
				}
				return list;
			}
			return yvtZHsJeRfJoFCBmHnjvgpStkncS(P_0, P_1.fullTarget);
		}

		private static IList<miYoADVkblEyjEtgJEzTqnbvebgPA> XSDALdYIRCVHpQaLaoKRPzrXfLVA(Controller P_0, IControllerTemplateButtonSource P_1)
		{
			if (P_1 == null)
			{
				return null;
			}
			return yvtZHsJeRfJoFCBmHnjvgpStkncS(P_0, P_1.target);
		}

		private static IList<miYoADVkblEyjEtgJEzTqnbvebgPA> yvtZHsJeRfJoFCBmHnjvgpStkncS(Controller P_0, IControllerElementTarget P_1)
		{
			if (P_1 == null)
			{
				return null;
			}
			Controller.Element elementById = P_0.GetElementById(P_1.elementIdentifierId);
			if (elementById == null)
			{
				return null;
			}
			return new List<miYoADVkblEyjEtgJEzTqnbvebgPA>
			{
				new miYoADVkblEyjEtgJEzTqnbvebgPA(P_1, elementById)
			};
		}

		private static IControllerTemplateElement ToMVpRXlknilMfcRZBynlhZgySFBA(List<IControllerTemplateElement> P_0, int P_1)
		{
			int count = P_0.Count;
			for (int i = 0; i < count; i++)
			{
				if (P_0[i].id == P_1)
				{
					return P_0[i];
				}
			}
			return null;
		}

		private static RZNDjkbvHOEbvhWYTRUapEJHWiOu fsykLgQOrXBgAfjTJkurpbRKedpgA(IControllerTemplate_Internal P_0, ADictionary<int, IControllerTemplateElement> P_1, int P_2)
		{
			if (!(P_1.GetValueSafe(P_2) is RZNDjkbvHOEbvhWYTRUapEJHWiOu result))
			{
				return wEDpgRYkJezclVWPwPJlOnrGQLHE.pNeCOamePGsLRrhAkHfuRGxROIqc(P_0);
			}
			return result;
		}

		private static RZNDjkbvHOEbvhWYTRUapEJHWiOu LuEhvQwaPnRWGxjJXLJAdcmenECs(IControllerTemplate_Internal P_0, ADictionary<int, IControllerTemplateElement> P_1, int P_2)
		{
			if (!(P_1.GetValueSafe(P_2) is RZNDjkbvHOEbvhWYTRUapEJHWiOu result))
			{
				return BShaaIVTHefYkCCCxsFLxwxLvTMk.SkAnIcKlidTGodlqxURKePAzlVlp(P_0);
			}
			return result;
		}
	}
}
