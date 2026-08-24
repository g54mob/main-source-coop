using System;
using UnityEngine.UI;

namespace HeathenEngineering
{
	[Serializable]
	public class ImageFillMethodReference : VariableReference<Image.FillMethod>
	{
		public ImageFillMethodPointerVariable Variable;

		public override IDataVariable<Image.FillMethod> m_variable => Variable;

		public ImageFillMethodReference(Image.FillMethod value)
			: base(value)
		{
		}
	}
}
