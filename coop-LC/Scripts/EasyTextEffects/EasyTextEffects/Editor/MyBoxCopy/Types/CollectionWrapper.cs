using System;

namespace EasyTextEffects.Editor.MyBoxCopy.Types
{
	[Serializable]
	public class CollectionWrapper<T> : CollectionWrapperBase
	{
		public T[] Value;
	}
}
