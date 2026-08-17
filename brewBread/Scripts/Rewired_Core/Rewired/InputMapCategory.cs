using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rewired.Utils;
using UnityEngine;

namespace Rewired
{
	[Serializable]
	public sealed class InputMapCategory : InputCategory
	{
		[CustomObfuscation(rename = false)]
		[SerializeField]
		private bool _checkConflictsWithAllCategories;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private List<int> _checkConflictsCategoryIds;

		private ReadOnlyCollection<int> _checkConflictsCategoryIds_readOnly;

		public bool checkConflictsWithAllCategories
		{
			get
			{
				return _checkConflictsWithAllCategories;
			}
			internal set
			{
				_checkConflictsWithAllCategories = flag;
			}
		}

		public IList<int> checkConflictsCategoryIds => _checkConflictsCategoryIds_readOnly;

		internal List<int> IjzdeMdTjOgEWlKUvZcNaMwzeZsDA => _checkConflictsCategoryIds;

		public InputMapCategory()
		{
			_checkConflictsCategoryIds = new List<int>();
		}

		public InputMapCategory(InputMapCategory P_0)
			: base(P_0)
		{
			_checkConflictsWithAllCategories = P_0._checkConflictsWithAllCategories;
			_checkConflictsCategoryIds = ListTools.ShallowCopy(P_0._checkConflictsCategoryIds);
		}

		internal void zQQfvDZMmpVqPPLYlLuSJXXpwJcI()
		{
			if (_checkConflictsCategoryIds != null)
			{
				_checkConflictsCategoryIds_readOnly = new ReadOnlyCollection<int>(_checkConflictsCategoryIds);
			}
		}
	}
}
