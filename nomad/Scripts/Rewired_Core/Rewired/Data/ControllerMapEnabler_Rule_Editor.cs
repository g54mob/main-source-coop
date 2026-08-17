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
	public sealed class ControllerMapEnabler_Rule_Editor : IDeepCloneable
	{
		[Serialize]
		[SerializeField]
		private string _tag;

		[Serialize]
		[SerializeField]
		private bool _enable;

		[Serialize]
		[SerializeField]
		private List<int> _categoryIds;

		[Serialize]
		[SerializeField]
		private List<int> _layoutIds;

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

		public bool enable
		{
			get
			{
				return _enable;
			}
			set
			{
				_enable = value;
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

		public List<int> layoutIds
		{
			get
			{
				return _layoutIds;
			}
			set
			{
				_layoutIds = value;
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

		public ControllerMapEnabler_Rule_Editor()
		{
			_enable = true;
			_categoryIds = new List<int>();
			_layoutIds = new List<int>();
			_controllerSetSelector = new ControllerSetSelector_Editor(ControllerSetSelector.Type.ControllerType);
		}

		public ControllerMapEnabler_Rule_Editor(ControllerMapEnabler_Rule_Editor P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("source");
			}
			_tag = P_0._tag;
			_enable = P_0._enable;
			_categoryIds = ListTools.ShallowCopy(P_0._categoryIds);
			_layoutIds = ListTools.ShallowCopy(P_0._layoutIds);
			_controllerSetSelector = MiscTools.DeepClone(P_0._controllerSetSelector);
		}

		internal ControllerMapEnabler.Rule ToRuntime()
		{
			return new ControllerMapEnabler.Rule(_tag, _enable, (_categoryIds != null) ? _categoryIds.ToArray() : new int[0], (_layoutIds != null) ? _layoutIds.ToArray() : new int[0], _controllerSetSelector.JjjvrcNzdYqOoQcCPTEXsoBqcxlz());
		}

		object IDeepCloneable.DeepClone()
		{
			return new ControllerMapEnabler_Rule_Editor(this);
		}
	}
}
