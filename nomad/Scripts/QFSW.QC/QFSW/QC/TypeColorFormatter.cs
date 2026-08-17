using System;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Serialization;

namespace QFSW.QC
{
	[Serializable]
	public class TypeColorFormatter : TypeFormatter
	{
		[FormerlySerializedAs("color")]
		public Color Color = Color.white;

		[Preserve]
		public TypeColorFormatter(Type type)
			: base(type)
		{
		}
	}
}
