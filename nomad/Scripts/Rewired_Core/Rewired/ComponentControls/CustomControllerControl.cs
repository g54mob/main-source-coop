using System;
using Rewired.ComponentControls.Data;
using Rewired.Utils;
using UnityEngine;

namespace Rewired.ComponentControls
{
	[Serializable]
	[DisallowMultipleComponent]
	public abstract class CustomControllerControl : ComponentControl
	{
		internal CustomController KIgQwfyVIoOiIXKpXkLoXgtMAONn => dtArSmnbMejNAcEKpEMEOdLulUFo() as CustomController;

		bool ComponentControl.ghAsJwRhDmClYOgMqzKuSmomibZfA => dtArSmnbMejNAcEKpEMEOdLulUFo() as CustomController != null;

		[CustomObfuscation(rename = false)]
		internal CustomControllerControl()
		{
		}

		internal virtual void KXZAZBKCBLiVYWwSNbnniwYSIrBkA()
		{
			base.UgYfXMHEewSKZnKGFlKMEgiFeKHdA();
			if (ghAsJwRhDmClYOgMqzKuSmomibZfA)
			{
				aIvGAdqbAxMoEfcxxqSVxbEqtEqO();
				KIgQwfyVIoOiIXKpXkLoXgtMAONn.InputSourceUpdateEvent += sQwPPohoqIyiVlJAaVdCZygjEmqm;
			}
		}

		internal virtual void rYnUOltSwBtwnpEBEVDesiwZDlZGA()
		{
			base.aIvGAdqbAxMoEfcxxqSVxbEqtEqO();
			if (ghAsJwRhDmClYOgMqzKuSmomibZfA)
			{
				KIgQwfyVIoOiIXKpXkLoXgtMAONn.InputSourceUpdateEvent -= sQwPPohoqIyiVlJAaVdCZygjEmqm;
			}
		}

		[CustomObfuscation(rename = false)]
		internal override IComponentController FindController()
		{
			return UnityTools.GetComponentInSelfOrParents<CustomController>(base.transform);
		}

		[CustomObfuscation(rename = false)]
		internal override Type GetRequiredControllerType()
		{
			return typeof(CustomController);
		}

		internal void GLmFZCmUYCmgFyInFMQJxMkuRoJw(CustomControllerElementTargetSet P_0, float P_1, float P_2)
		{
			if (!ghAsJwRhDmClYOgMqzKuSmomibZfA || P_0 == null)
			{
				return;
			}
			if (P_0 is CustomControllerElementTargetSetForFloat customControllerElementTargetSetForFloat)
			{
				if (!customControllerElementTargetSetForFloat.splitValue)
				{
					TsYByjFktDabodYNdtOuXvnmfXKLA(customControllerElementTargetSetForFloat.target, P_1, P_2);
					return;
				}
				TsYByjFktDabodYNdtOuXvnmfXKLA(customControllerElementTargetSetForFloat.positiveTarget, P_1, P_2);
				TsYByjFktDabodYNdtOuXvnmfXKLA(customControllerElementTargetSetForFloat.negativeTarget, P_1, P_2);
			}
			else if (P_0 is CustomControllerElementTargetSetForBoolean customControllerElementTargetSetForBoolean)
			{
				TsYByjFktDabodYNdtOuXvnmfXKLA(customControllerElementTargetSetForBoolean.target, P_1, P_2);
			}
		}

		internal void mtSnwsaofzmMqmFsUdCcwVcPzKvK(CustomControllerElementTargetSet P_0, bool P_1)
		{
			if (!ghAsJwRhDmClYOgMqzKuSmomibZfA || P_0 == null)
			{
				return;
			}
			if (P_0 is CustomControllerElementTargetSetForBoolean customControllerElementTargetSetForBoolean)
			{
				XTSnoSDYSKdcnEjZKsjlnYfpDbLM(customControllerElementTargetSetForBoolean.target, P_1);
			}
			else if (P_0 is CustomControllerElementTargetSetForFloat customControllerElementTargetSetForFloat)
			{
				if (!customControllerElementTargetSetForFloat.splitValue)
				{
					XTSnoSDYSKdcnEjZKsjlnYfpDbLM(customControllerElementTargetSetForFloat.target, P_1);
					return;
				}
				XTSnoSDYSKdcnEjZKsjlnYfpDbLM(customControllerElementTargetSetForFloat.positiveTarget, P_1);
				XTSnoSDYSKdcnEjZKsjlnYfpDbLM(customControllerElementTargetSetForFloat.negativeTarget, P_1);
			}
		}

		internal abstract void NGxTyoiTkTNoTijSPLHlAytkbZUb();

		private void TsYByjFktDabodYNdtOuXvnmfXKLA(CustomControllerElementTarget P_0, float P_1, float P_2)
		{
			if (P_0 == null)
			{
				return;
			}
			switch (P_0.element.elementType)
			{
			case CustomControllerElementSelector.ElementType.Axis:
				switch (P_0.valueRange)
				{
				case CustomControllerElementTarget.ValueRange.Full:
					if (P_0.invert)
					{
						P_1 *= -1f;
					}
					break;
				case CustomControllerElementTarget.ValueRange.Positive:
					if (P_1 < 0f)
					{
						P_1 = 0f;
					}
					if (P_0.valueContribution == Pole.Negative)
					{
						P_1 *= -1f;
					}
					break;
				case CustomControllerElementTarget.ValueRange.Negative:
					if (P_1 > 0f)
					{
						P_1 = 0f;
					}
					if (P_0.valueContribution == Pole.Positive)
					{
						P_1 *= -1f;
					}
					break;
				}
				KIgQwfyVIoOiIXKpXkLoXgtMAONn.SetAxisValue(P_0.element, P_1);
				break;
			case CustomControllerElementSelector.ElementType.Button:
				switch (P_0.valueRange)
				{
				case CustomControllerElementTarget.ValueRange.Positive:
					if (P_1 < 0f)
					{
						P_1 = 0f;
					}
					break;
				case CustomControllerElementTarget.ValueRange.Negative:
					if (P_1 > 0f)
					{
						P_1 = 0f;
					}
					break;
				}
				KIgQwfyVIoOiIXKpXkLoXgtMAONn.SetButtonValue(P_0.element, MathTools.Abs(P_1) >= MathTools.Abs(P_2));
				break;
			default:
				throw new NotImplementedException();
			}
		}

		private void XTSnoSDYSKdcnEjZKsjlnYfpDbLM(CustomControllerElementTarget P_0, bool P_1)
		{
			if (P_0 == null)
			{
				return;
			}
			switch (P_0.element.elementType)
			{
			case CustomControllerElementSelector.ElementType.Axis:
			{
				float num = (P_1 ? 1f : 0f);
				if (P_0.valueRange == CustomControllerElementTarget.ValueRange.Full)
				{
					if (P_0.invert)
					{
						num *= -1f;
					}
				}
				else if (P_0.valueContribution == Pole.Negative)
				{
					num *= -1f;
				}
				KIgQwfyVIoOiIXKpXkLoXgtMAONn.SetAxisValue(P_0.element, num);
				break;
			}
			case CustomControllerElementSelector.ElementType.Button:
				KIgQwfyVIoOiIXKpXkLoXgtMAONn.SetButtonValue(P_0.element, P_1);
				break;
			default:
				throw new NotImplementedException();
			}
		}

		private void sQwPPohoqIyiVlJAaVdCZygjEmqm()
		{
			if (!xpGKelZOWtEsrJtduJSHNrwIpnAsA() && IcqbeYEmGpfkqqAVukZKtDJbdtuLA())
			{
				NGxTyoiTkTNoTijSPLHlAytkbZUb();
			}
		}
	}
}
