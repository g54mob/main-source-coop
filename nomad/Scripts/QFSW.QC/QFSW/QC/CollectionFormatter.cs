using System;
using UnityEngine.Scripting;
using UnityEngine.Serialization;

namespace QFSW.QC
{
	[Serializable]
	public class CollectionFormatter : TypeFormatter
	{
		[FormerlySerializedAs("seperatorString")]
		public string SeperatorString = ",";

		[FormerlySerializedAs("leftScoper")]
		public string LeftScoper = "[";

		[FormerlySerializedAs("rightScoper")]
		public string RightScoper = "]";

		[Preserve]
		public CollectionFormatter(Type type)
			: base(type)
		{
		}
	}
}
