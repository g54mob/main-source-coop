using System;
using System.Collections.Generic;
using Rewired.Utils;
using Rewired.Utils.Attributes;
using Rewired.Utils.Libraries.TinyJson;
using UnityEngine;

namespace Rewired.Data
{
	[Serializable]
	[Preserve]
	[CustomClassObfuscation(renamePrivateMembers = false, renamePubIntMembers = false)]
	public sealed class ControllerMapEnabler_RuleSet_Editor
	{
		[SerializeField]
		[Serialize]
		private int _id;

		[Serialize]
		[SerializeField]
		private string _name;

		[Serialize]
		[SerializeField]
		private string _tag;

		[SerializeField]
		[Serialize]
		private List<ControllerMapEnabler_Rule_Editor> _rules;

		public int id
		{
			get
			{
				return _id;
			}
			set
			{
				_id = value;
			}
		}

		public string name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

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

		public List<ControllerMapEnabler_Rule_Editor> rules
		{
			get
			{
				return _rules;
			}
			set
			{
				_rules = value;
			}
		}

		public ControllerMapEnabler_RuleSet_Editor()
		{
			_rules = new List<ControllerMapEnabler_Rule_Editor>();
		}

		public ControllerMapEnabler_RuleSet_Editor(ControllerMapEnabler_RuleSet_Editor P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("source");
			}
			_id = P_0._id;
			_name = P_0._name;
			_tag = P_0._tag;
			_rules = MiscTools.DeepClone(P_0._rules);
		}

		internal ControllerMapEnabler_RuleSet_Editor Clone()
		{
			return new ControllerMapEnabler_RuleSet_Editor(this);
		}

		internal ControllerMapEnabler.RuleSet ToRuntime()
		{
			List<ControllerMapEnabler.Rule> list = new List<ControllerMapEnabler.Rule>();
			if (_rules != null)
			{
				for (int i = 0; i < _rules.Count; i++)
				{
					if (_rules[i] != null)
					{
						list.Add(_rules[i].ToRuntime());
					}
				}
			}
			return new ControllerMapEnabler.RuleSet(true, _tag, list);
		}
	}
}
