using System;

namespace Sirenix.OdinInspector
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class TypeSelectorSettingsAttribute : Attribute
	{
		public const string FILTER_TYPES_FUNCTION_NAMED_VALUE = "type";

		public string FilterTypesFunction;

		private bool? showNoneItem;

		private bool? showCategories;

		private bool? preferNamespaces;

		[ShowInInspector]
		[OdinDesignerBinding(new string[] { "showNoneItem" })]
		public bool ShowNoneItem
		{
			get
			{
				return showNoneItem == true;
			}
			set
			{
				showNoneItem = value;
			}
		}

		[ShowInInspector]
		[OdinDesignerBinding(new string[] { "showCategories" })]
		public bool ShowCategories
		{
			get
			{
				return showCategories == true;
			}
			set
			{
				showCategories = value;
			}
		}

		[ShowInInspector]
		[OdinDesignerBinding(new string[] { "preferNamespaces" })]
		public bool PreferNamespaces
		{
			get
			{
				return preferNamespaces == true;
			}
			set
			{
				preferNamespaces = value;
			}
		}

		public bool ShowNoneItemIsSet => showNoneItem.HasValue;

		public bool ShowCategoriesIsSet => showCategories.HasValue;

		public bool PreferNamespacesIsSet => preferNamespaces.HasValue;
	}
}
