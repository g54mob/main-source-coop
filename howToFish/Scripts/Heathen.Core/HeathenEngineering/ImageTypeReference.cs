using System;
using UnityEngine.UI;

namespace HeathenEngineering
{
	[Serializable]
	public class ImageTypeReference : VariableReference<Image.Type>
	{
		public ImageTypePointerVariable Variable;

		public override IDataVariable<Image.Type> m_variable => Variable;

		public ImageTypeReference(Image.Type value)
			: base(value)
		{
		}
	}
}
