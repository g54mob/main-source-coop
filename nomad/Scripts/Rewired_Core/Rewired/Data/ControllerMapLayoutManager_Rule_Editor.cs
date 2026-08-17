using System;
using System.Collections.Generic;
using Rewired.Utils;
using Rewired.Utils.Attributes;
using Rewired.Utils.Interfaces;
using Rewired.Utils.Libraries.TinyJson;
using UnityEngine;

namespace Rewired.Data
{
	[Serializable]
	[Preserve]
	[CustomClassObfuscation(renamePrivateMembers = false, renamePubIntMembers = false)]
	public sealed class ControllerMapLayoutManager_Rule_Editor : IDeepCloneable
	{
		[Serialize]
		[SerializeField]
		private string _tag;

		[Serialize]
		[SerializeField]
		private List<int> _categoryIds;

		[Serialize]
		[SerializeField]
		private int _layoutId;

		[Serialize]
		[SerializeField]
		private ControllerSetSelector_Editor _controllerSetSelector;

		public string tag
		{
			get
			{
				return _tag;
			}
			set
			{
				_tag = value;
			}
		}

		public List<int> categoryIds
		{
			get
			{
				return _categoryIds;
			}
			set
			{
				_categoryIds = value;
			}
		}

		public int layoutId
		{
			get
			{
				return _layoutId;
			}
			set
			{
				_layoutId = value;
			}
		}

		public ControllerSetSelector_Editor controllerSetSelector
		{
			get
			{
				return _controllerSetSelector;
			}
			set
			{
				_controllerSetSelector = value;
			}
		}

		public ControllerMapLayoutManager_Rule_Editor()
		{
			_categoryIds = new List<int>();
			_layoutId = -1;
			_controllerSetSelector = new ControllerSetSelector_Editor(ControllerSetSelector.Type.ControllerType);
		}

		public ControllerMapLayoutManager_Rule_Editor(ControllerMapLayoutManager_Rule_Editor P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("source");
			}
			_tag = P_0._tag;
			_categoryIds = ListTools.ShallowCopy(P_0._categoryIds);
			_layoutId = P_0._layoutId;
			_controllerSetSelector = MiscTools.DeepClone(P_0._controllerSetSelector);
		}

		internal ControllerMapLayoutManager.Rule ToRuntime()
		{
			return new ControllerMapLayoutManager.Rule(_tag, (_categoryIds != null) ? _categoryIds.ToArray() : new int[0], _layoutId, _controllerSetSelector.JjjvrcNzdYqOoQcCPTEXsoBqcxlz());
		}

		object IDeepCloneable.DeepClone()
		{
			return new ControllerMapLayoutManager_Rule_Editor(this);
		}
	}
}
