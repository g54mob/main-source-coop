using System;
using System.Collections.Generic;
using System.ComponentModel;
using Rewired.Utils;
using Rewired.Utils.Interfaces;
using UnityEngine;

namespace Rewired.Data
{
	[Serializable]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class Player_Editor
	{
		[Serializable]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class Mapping
		{
			[SerializeField]
			[CustomObfuscation(rename = false)]
			private bool _enabled;

			[SerializeField]
			[CustomObfuscation(rename = false)]
			private int _categoryId;

			[SerializeField]
			[CustomObfuscation(rename = false)]
			private int _layoutId;

			public int categoryId
			{
				get
				{
					return _categoryId;
				}
				internal set
				{
					_categoryId = num;
				}
			}

			public int layoutId
			{
				get
				{
					return _layoutId;
				}
				internal set
				{
					_layoutId = num;
				}
			}

			public bool enabled
			{
				get
				{
					return _enabled;
				}
				internal set
				{
					_enabled = flag;
				}
			}

			public Mapping()
			{
				Clear();
			}

			public Mapping(bool P_0, int P_1, int P_2)
			{
				_enabled = P_0;
				_categoryId = P_1;
				_layoutId = P_2;
			}

			public void Clear()
			{
				_categoryId = 0;
				_layoutId = 0;
				_enabled = true;
			}

			public Mapping Clone()
			{
				return new Mapping(_enabled, _categoryId, _layoutId);
			}

			internal IRueBbHgtviLAypteudQcHzlDhLM vGSGaSiAOxpnfQEILewnATzUWVpVA()
			{
				return new IRueBbHgtviLAypteudQcHzlDhLM(_categoryId, _layoutId, _enabled);
			}
		}

		[Serializable]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class ControllerMapLayoutManagerSettings : IDeepCloneable
		{
			[SerializeField]
			[CustomObfuscation(rename = false)]
			private bool _enabled = true;

			[SerializeField]
			[CustomObfuscation(rename = false)]
			private bool _loadFromUserDataStore = true;

			[SerializeField]
			[CustomObfuscation(rename = false)]
			private List<RuleSetMapping> _ruleSets;

			public bool enabled
			{
				get
				{
					return _enabled;
				}
				set
				{
					_enabled = value;
				}
			}

			public bool loadFromUserDataStore
			{
				get
				{
					return _loadFromUserDataStore;
				}
				set
				{
					_loadFromUserDataStore = value;
				}
			}

			public List<RuleSetMapping> ruleSets
			{
				get
				{
					return _ruleSets;
				}
				set
				{
					_ruleSets = value ?? (_ruleSets = new List<RuleSetMapping>());
				}
			}

			public ControllerMapLayoutManagerSettings()
			{
				_ruleSets = new List<RuleSetMapping>();
				_enabled = true;
				_loadFromUserDataStore = true;
			}

			public ControllerMapLayoutManagerSettings(ControllerMapLayoutManagerSettings P_0)
			{
				if (P_0 == null)
				{
					throw new ArgumentNullException("source");
				}
				_enabled = P_0._enabled;
				_loadFromUserDataStore = P_0._loadFromUserDataStore;
				_ruleSets = MiscTools.DeepClone(P_0._ruleSets) ?? new List<RuleSetMapping>();
			}

			internal ControllerMapLayoutManager.ICvEqTkajGQZLXkOyPTomcjaeCWs MJHfMyBLinpbxXjnaGNrevuOWSsCA()
			{
				return new ControllerMapLayoutManager.ICvEqTkajGQZLXkOyPTomcjaeCWs(_enabled, _loadFromUserDataStore, wBbWnkNeGLJCeZeZnnBdICADnzwr());
			}

			private VKBFHptpOwZTIxZpGslzThtgRcTO[] wBbWnkNeGLJCeZeZnnBdICADnzwr()
			{
				List<VKBFHptpOwZTIxZpGslzThtgRcTO> list = new List<VKBFHptpOwZTIxZpGslzThtgRcTO>();
				int num = ((_ruleSets != null) ? _ruleSets.Count : 0);
				for (int i = 0; i < num; i++)
				{
					if (_ruleSets[i] != null)
					{
						list.Add(_ruleSets[i].dIopkEJKtPEDEzLIUUUiimcgPTio());
					}
				}
				return list.ToArray();
			}

			object IDeepCloneable.DeepClone()
			{
				return new ControllerMapLayoutManagerSettings(this);
			}
		}

		[Serializable]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class ControllerMapEnablerSettings : IDeepCloneable
		{
			[SerializeField]
			[CustomObfuscation(rename = false)]
			private bool _enabled = true;

			[SerializeField]
			[CustomObfuscation(rename = false)]
			private List<RuleSetMapping> _ruleSets;

			public bool enabled
			{
				get
				{
					return _enabled;
				}
				set
				{
					_enabled = value;
				}
			}

			public List<RuleSetMapping> ruleSets
			{
				get
				{
					return _ruleSets;
				}
				set
				{
					_ruleSets = value ?? (_ruleSets = new List<RuleSetMapping>());
				}
			}

			public ControllerMapEnablerSettings()
			{
				_ruleSets = new List<RuleSetMapping>();
				_enabled = true;
			}

			public ControllerMapEnablerSettings(ControllerMapEnablerSettings P_0)
			{
				if (P_0 == null)
				{
					throw new ArgumentNullException("source");
				}
				_enabled = P_0._enabled;
				_ruleSets = MiscTools.DeepClone(P_0._ruleSets) ?? new List<RuleSetMapping>();
			}

			internal ControllerMapEnabler.CyXCJRXKNwVfGTMPVLIRUynNmCKI YNoLOReakxUorAzAJFCaIyXpqBIS()
			{
				return new ControllerMapEnabler.CyXCJRXKNwVfGTMPVLIRUynNmCKI(_enabled, fxIBuzSxEmXHNitAvHNynBCMAcNe());
			}

			private VKBFHptpOwZTIxZpGslzThtgRcTO[] fxIBuzSxEmXHNitAvHNynBCMAcNe()
			{
				List<VKBFHptpOwZTIxZpGslzThtgRcTO> list = new List<VKBFHptpOwZTIxZpGslzThtgRcTO>();
				int num = ((_ruleSets != null) ? _ruleSets.Count : 0);
				for (int i = 0; i < num; i++)
				{
					if (_ruleSets[i] != null)
					{
						list.Add(_ruleSets[i].dIopkEJKtPEDEzLIUUUiimcgPTio());
					}
				}
				return list.ToArray();
			}

			object IDeepCloneable.DeepClone()
			{
				return new ControllerMapEnablerSettings(this);
			}
		}

		[Serializable]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class RuleSetMapping : IDeepCloneable
		{
			[SerializeField]
			[CustomObfuscation(rename = false)]
			private bool _enabled;

			[SerializeField]
			[CustomObfuscation(rename = false)]
			private int _id;

			public int id
			{
				get
				{
					return _id;
				}
				internal set
				{
					_id = num;
				}
			}

			public bool enabled
			{
				get
				{
					return _enabled;
				}
				internal set
				{
					_enabled = flag;
				}
			}

			public RuleSetMapping()
			{
				Clear();
			}

			public RuleSetMapping(RuleSetMapping P_0)
				: this()
			{
				if (P_0 == null)
				{
					throw new ArgumentNullException("source");
				}
				_enabled = P_0._enabled;
				_id = P_0._id;
			}

			public RuleSetMapping(bool P_0, int P_1)
			{
				_enabled = P_0;
				_id = P_1;
			}

			public void Clear()
			{
				_id = 0;
				_enabled = true;
			}

			public RuleSetMapping Clone()
			{
				return new RuleSetMapping(_enabled, _id);
			}

			internal VKBFHptpOwZTIxZpGslzThtgRcTO dIopkEJKtPEDEzLIUUUiimcgPTio()
			{
				return new VKBFHptpOwZTIxZpGslzThtgRcTO(_id, _enabled);
			}

			object IDeepCloneable.DeepClone()
			{
				return new RuleSetMapping(this);
			}
		}

		[Serializable]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class CreateControllerInfo
		{
			[SerializeField]
			[CustomObfuscation(rename = false)]
			private int _sourceId;

			[SerializeField]
			[CustomObfuscation(rename = false)]
			private string _tag;

			public int sourceId
			{
				get
				{
					return _sourceId;
				}
				internal set
				{
					_sourceId = num;
				}
			}

			public string tag
			{
				get
				{
					return _tag;
				}
				internal set
				{
					_tag = text;
				}
			}

			public CreateControllerInfo()
			{
			}

			public CreateControllerInfo(int P_0, string P_1)
			{
				_sourceId = P_0;
				_tag = P_1;
			}

			public CreateControllerInfo(CreateControllerInfo P_0)
			{
				_sourceId = P_0._sourceId;
				_tag = P_0._tag;
			}
		}

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int _id;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string _name;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string _descriptiveName;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string _key;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _startPlaying;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<Mapping> _defaultJoystickMaps;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<Mapping> _defaultMouseMaps;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<Mapping> _defaultKeyboardMaps;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<Mapping> _defaultCustomControllerMaps;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<CreateControllerInfo> _startingCustomControllers;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _assignMouseOnStart;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _assignKeyboardOnStart = true;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _excludeFromControllerAutoAssignment;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private ControllerMapLayoutManagerSettings _controllerMapLayoutManagerSettings;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private ControllerMapEnablerSettings _controllerMapEnablerSettings;

		public int id
		{
			get
			{
				return _id;
			}
			internal set
			{
				_id = num;
			}
		}

		public string name
		{
			get
			{
				return _name;
			}
			internal set
			{
				_name = text;
			}
		}

		public string descriptiveName
		{
			get
			{
				return _descriptiveName;
			}
			internal set
			{
				_descriptiveName = text;
			}
		}

		public string key
		{
			get
			{
				return _key;
			}
			internal set
			{
				_key = text;
			}
		}

		public bool startPlaying
		{
			get
			{
				return _startPlaying;
			}
			internal set
			{
				_startPlaying = flag;
			}
		}

		public List<Mapping> defaultJoystickMaps
		{
			get
			{
				return _defaultJoystickMaps;
			}
			internal set
			{
				_defaultJoystickMaps = list;
			}
		}

		public List<Mapping> defaultMouseMaps
		{
			get
			{
				return _defaultMouseMaps;
			}
			internal set
			{
				_defaultMouseMaps = list;
			}
		}

		public List<Mapping> defaultKeyboardMaps
		{
			get
			{
				return _defaultKeyboardMaps;
			}
			internal set
			{
				_defaultKeyboardMaps = list;
			}
		}

		public List<Mapping> defaultCustomControllerMaps
		{
			get
			{
				return _defaultCustomControllerMaps;
			}
			internal set
			{
				_defaultCustomControllerMaps = list;
			}
		}

		public List<CreateControllerInfo> startingCustomControllers
		{
			get
			{
				return _startingCustomControllers;
			}
			internal set
			{
				_startingCustomControllers = list;
			}
		}

		public bool assignMouseOnStart
		{
			get
			{
				return _assignMouseOnStart;
			}
			internal set
			{
				_assignMouseOnStart = flag;
			}
		}

		public bool assignKeyboardOnStart
		{
			get
			{
				return _assignKeyboardOnStart;
			}
			internal set
			{
				_assignKeyboardOnStart = flag;
			}
		}

		public bool excludeFromControllerAutoAssignment
		{
			get
			{
				return _excludeFromControllerAutoAssignment;
			}
			internal set
			{
				_excludeFromControllerAutoAssignment = flag;
			}
		}

		public ControllerMapLayoutManagerSettings controllerMapLayoutManagerSettings
		{
			get
			{
				return _controllerMapLayoutManagerSettings;
			}
			set
			{
				_controllerMapLayoutManagerSettings = value;
			}
		}

		public ControllerMapEnablerSettings controllerMapEnablerSettings
		{
			get
			{
				return _controllerMapEnablerSettings;
			}
			set
			{
				_controllerMapEnablerSettings = value;
			}
		}

		public Player_Editor()
		{
			_defaultKeyboardMaps = new List<Mapping>();
			_defaultJoystickMaps = new List<Mapping>();
			_defaultMouseMaps = new List<Mapping>();
			_defaultCustomControllerMaps = new List<Mapping>();
			_startingCustomControllers = new List<CreateControllerInfo>();
			_excludeFromControllerAutoAssignment = false;
			_controllerMapLayoutManagerSettings = new ControllerMapLayoutManagerSettings();
			_controllerMapEnablerSettings = new ControllerMapEnablerSettings();
		}

		public Player_Editor(Player_Editor P_0)
		{
			_id = P_0._id;
			_name = P_0._name;
			_descriptiveName = P_0._descriptiveName;
			_key = P_0._key;
			_startPlaying = P_0._startPlaying;
			_defaultJoystickMaps = new List<Mapping>();
			if (P_0._defaultJoystickMaps != null)
			{
				for (int i = 0; i < P_0._defaultJoystickMaps.Count; i++)
				{
					_defaultJoystickMaps.Add(P_0._defaultJoystickMaps[i].Clone());
				}
			}
			_defaultKeyboardMaps = new List<Mapping>();
			if (P_0._defaultKeyboardMaps != null)
			{
				for (int j = 0; j < P_0._defaultKeyboardMaps.Count; j++)
				{
					_defaultKeyboardMaps.Add(P_0._defaultKeyboardMaps[j].Clone());
				}
			}
			_defaultMouseMaps = new List<Mapping>();
			if (P_0._defaultMouseMaps != null)
			{
				for (int k = 0; k < P_0._defaultMouseMaps.Count; k++)
				{
					_defaultMouseMaps.Add(P_0._defaultMouseMaps[k].Clone());
				}
			}
			_defaultCustomControllerMaps = new List<Mapping>();
			if (P_0._defaultCustomControllerMaps != null)
			{
				for (int l = 0; l < P_0._defaultCustomControllerMaps.Count; l++)
				{
					_defaultCustomControllerMaps.Add(P_0._defaultCustomControllerMaps[l].Clone());
				}
			}
			_startingCustomControllers = new List<CreateControllerInfo>();
			if (P_0._startingCustomControllers != null)
			{
				for (int m = 0; m < P_0._startingCustomControllers.Count; m++)
				{
					_startingCustomControllers.Add(new CreateControllerInfo(P_0._startingCustomControllers[m]));
				}
			}
			_controllerMapLayoutManagerSettings = MiscTools.DeepClone(P_0._controllerMapLayoutManagerSettings) ?? new ControllerMapLayoutManagerSettings();
			_controllerMapEnablerSettings = MiscTools.DeepClone(P_0._controllerMapEnablerSettings) ?? new ControllerMapEnablerSettings();
			_assignMouseOnStart = P_0._assignMouseOnStart;
			_assignKeyboardOnStart = P_0._assignKeyboardOnStart;
			_excludeFromControllerAutoAssignment = P_0._excludeFromControllerAutoAssignment;
		}

		public Player_Editor Clone()
		{
			return new Player_Editor(this);
		}

		internal EhUOIcAWtPzjNpBYLAHyNaaERzaE qjgbGAcyFmSMTzcJABgXEsUgyQTv()
		{
			IRueBbHgtviLAypteudQcHzlDhLM[] array = null;
			if (_defaultJoystickMaps != null)
			{
				array = new IRueBbHgtviLAypteudQcHzlDhLM[_defaultJoystickMaps.Count];
				for (int i = 0; i < _defaultJoystickMaps.Count; i++)
				{
					array[i] = _defaultJoystickMaps[i].vGSGaSiAOxpnfQEILewnATzUWVpVA();
				}
			}
			IRueBbHgtviLAypteudQcHzlDhLM[] array2 = null;
			if (_defaultKeyboardMaps != null)
			{
				array2 = new IRueBbHgtviLAypteudQcHzlDhLM[_defaultKeyboardMaps.Count];
				for (int j = 0; j < _defaultKeyboardMaps.Count; j++)
				{
					array2[j] = _defaultKeyboardMaps[j].vGSGaSiAOxpnfQEILewnATzUWVpVA();
				}
			}
			IRueBbHgtviLAypteudQcHzlDhLM[] array3 = null;
			if (_defaultMouseMaps != null)
			{
				array3 = new IRueBbHgtviLAypteudQcHzlDhLM[_defaultMouseMaps.Count];
				for (int k = 0; k < _defaultMouseMaps.Count; k++)
				{
					array3[k] = _defaultMouseMaps[k].vGSGaSiAOxpnfQEILewnATzUWVpVA();
				}
			}
			IRueBbHgtviLAypteudQcHzlDhLM[] array4 = null;
			if (_defaultCustomControllerMaps != null)
			{
				array4 = new IRueBbHgtviLAypteudQcHzlDhLM[_defaultCustomControllerMaps.Count];
				for (int l = 0; l < _defaultCustomControllerMaps.Count; l++)
				{
					array4[l] = _defaultCustomControllerMaps[l].vGSGaSiAOxpnfQEILewnATzUWVpVA();
				}
			}
			return new EhUOIcAWtPzjNpBYLAHyNaaERzaE(array, array2, array3, array4);
		}
	}
}
