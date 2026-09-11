using System;
using UnityEngine;

namespace Zorro.Core
{
	public abstract class ClassTypeConstraintAttribute : PropertyAttribute
	{
		private ClassGrouping grouping = ClassGrouping.ByNamespaceFlat;

		private bool allowAbstract;

		public ClassGrouping Grouping
		{
			get
			{
				return grouping;
			}
			set
			{
				grouping = value;
			}
		}

		public bool AllowAbstract
		{
			get
			{
				return allowAbstract;
			}
			set
			{
				allowAbstract = value;
			}
		}

		public virtual bool IsConstraintSatisfied(Type type)
		{
			if (!AllowAbstract)
			{
				return !type.IsAbstract;
			}
			return true;
		}
	}
}
