using System;
using System.Linq;
using TMPro;

namespace Global.Modules.Localization_Module.Scripts.MonoBehaviours
{
	public class EnumDropDown<TEnum> : TMP_Dropdown where TEnum : struct, Enum
	{
		protected override void Awake()
		{
			base.Awake();
			FillWithEnumValues();
		}

		public TEnum GetEnumValue()
		{
			TEnum[] array = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToArray();
			if (base.value >= 0 && base.value < array.Length)
			{
				return array[base.value];
			}
			return default(TEnum);
		}

		public void SetEnumValue(TEnum enumValue)
		{
			int num = Array.IndexOf(Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToArray(), enumValue);
			if (num >= 0)
			{
				base.value = num;
				RefreshShownValue();
			}
		}

		protected void FillWithEnumValues()
		{
			base.options.Clear();
			foreach (TEnum item in Enum.GetValues(typeof(TEnum)).Cast<TEnum>())
			{
				string displayName = GetDisplayName(item);
				base.options.Add(new OptionData(displayName));
			}
			RefreshShownValue();
		}

		protected virtual string GetDisplayName(TEnum enumValue)
		{
			return enumValue.ToString();
		}
	}
}
