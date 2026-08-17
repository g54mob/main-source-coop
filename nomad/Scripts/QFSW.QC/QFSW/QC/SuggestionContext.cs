using System;
using System.Collections.Generic;

namespace QFSW.QC
{
	public struct SuggestionContext
	{
		public int Depth;

		public string Prompt;

		public Type TargetType;

		public IQcSuggestorTag[] Tags;

		public bool HasTag<T>() where T : IQcSuggestorTag
		{
			if (Tags == null)
			{
				return false;
			}
			IQcSuggestorTag[] tags = Tags;
			for (int i = 0; i < tags.Length; i++)
			{
				if (tags[i] is T)
				{
					return true;
				}
			}
			return false;
		}

		public T GetTag<T>() where T : IQcSuggestorTag
		{
			if (Tags != null)
			{
				IQcSuggestorTag[] tags = Tags;
				foreach (IQcSuggestorTag qcSuggestorTag in tags)
				{
					if (qcSuggestorTag is T)
					{
						return (T)qcSuggestorTag;
					}
				}
			}
			throw new KeyNotFoundException($"No tags of type {typeof(T)} could be found.");
		}

		public IEnumerable<T> GetTags<T>() where T : IQcSuggestorTag
		{
			if (Tags == null)
			{
				yield break;
			}
			IQcSuggestorTag[] tags = Tags;
			foreach (IQcSuggestorTag qcSuggestorTag in tags)
			{
				if (qcSuggestorTag is T)
				{
					yield return (T)qcSuggestorTag;
				}
			}
		}
	}
}
