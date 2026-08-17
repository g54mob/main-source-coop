using UnityEngine;

namespace VInspector
{
	public class VariantsAttribute : PropertyAttribute
	{
		public object[] variants;

		public VariantsAttribute(params object[] variants)
		{
			this.variants = variants;
		}
	}
}
