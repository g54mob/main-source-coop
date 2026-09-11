using System;
using System.Collections.Generic;

namespace EasyTextEffects.Editor.MyBoxCopy.Types
{
	[Serializable]
	public class CollectionWrapperList<T> : CollectionWrapperBase
	{
		public List<T> Value = new List<T>();
	}
}
