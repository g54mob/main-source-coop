using System;
using System.Collections.Generic;
using HeathenEngineering.Serializable;

namespace HeathenEngineering
{
	[Serializable]
	public class RectTransformListReference : VariableReference<List<SerializableRectTransform>>
	{
		public RectTransformListVariable Variable;

		public override IDataVariable<List<SerializableRectTransform>> m_variable => Variable;

		public RectTransformListReference(List<SerializableRectTransform> value)
			: base(value)
		{
		}
	}
}
