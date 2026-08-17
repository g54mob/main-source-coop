using System;
using EvilCore.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NomadDrive.Features.LiquidTransferSystem.UI
{
	[Serializable]
	public class LiquidContainerUIObject : MonoBehaviour
	{
		public Image liquidAmountImage;

		public TextMeshProUGUI nameTextMesh;

		public TextMeshProUGUI liquidAmountTextMesh;

		public TextMeshProUGUI liquidTypeTextMesh;

		public ILiquidContainer LiquidContainer;

		private Color _color;

		private ILocalizationService _localizationService;

		public void Setup(ILiquidContainer liquidContainer, Color color, ILocalizationService localizationService)
		{
			_localizationService = localizationService;
			if (liquidContainer == null)
			{
				base.gameObject.SetActive(value: false);
				LiquidContainer = null;
			}
			else
			{
				_color = color;
				LiquidContainer = liquidContainer;
				base.gameObject.SetActive(value: true);
			}
		}

		public void SetColor(Color color)
		{
			_color = color;
		}

		public void UpdateUI()
		{
			if (LiquidContainer != null && !(LiquidContainer as UnityEngine.Object == null))
			{
				UpdateFillAmount();
				UpdateAmountText();
				UpdateLiquidTypeUI();
				UpdateTextColor(_color);
				UpdateNameText();
			}
		}

		private void UpdateFillAmount()
		{
			if (!(liquidAmountImage == null))
			{
				liquidAmountImage.fillAmount = LiquidContainer.FillRatio;
			}
		}

		private void UpdateTextColor(Color color)
		{
			if (!(liquidAmountImage == null))
			{
				liquidAmountImage.color = color;
			}
		}

		private void UpdateNameText()
		{
			if (!(nameTextMesh == null))
			{
				nameTextMesh.text = _localizationService?.LocalizeName(LiquidContainer.Name) ?? LiquidContainer.Name;
			}
		}

		private void UpdateAmountText()
		{
			if (!(liquidAmountTextMesh == null))
			{
				string text = LiquidContainer.CurrentAmount.ToString("0.00");
				string text2 = LiquidContainer.Capacity.ToString("0.00");
				liquidAmountTextMesh.text = text + "/" + text2 + " LT";
			}
		}

		private void UpdateLiquidTypeUI()
		{
			if (!(liquidTypeTextMesh == null))
			{
				string text = LiquidContainer.CurrentLiquidType switch
				{
					LiquidType.Water => "@liquid.type_water", 
					LiquidType.Gasoline => "@liquid.type_gasoline", 
					LiquidType.Oil => "@liquid.type_oil", 
					LiquidType.Coffee => "@liquid.type_coffee", 
					LiquidType.Milk => "@liquid.type_milk", 
					LiquidType.Coolant => "@liquid.type_coolant", 
					_ => "@liquid.type_empty", 
				};
				liquidTypeTextMesh.text = _localizationService?.Localize(text) ?? text;
			}
		}
	}
}
