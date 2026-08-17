using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Rewired.Utils;
using Rewired.Utils.Attributes;
using Rewired.Utils.Classes.Data;
using Rewired.Utils.Interfaces;
using Rewired.Utils.Libraries.TinyJson;
using UnityEngine;

namespace Rewired
{
	public sealed class ControllerMapEnabler
	{
		[Serializable]
		[Preserve]
		[CustomClassObfuscation(renamePrivateMembers = false, renamePubIntMembers = false)]
		public sealed class Rule : IDeepCloneable
		{
			[SerializeField]
			[Serialize(Name = "tag")]
			private string _tag;

			[SerializeField]
			[Serialize(Name = "enable")]
			private bool _enable;

			[SerializeField]
			[Serialize(Name = "categoryIds")]
			private int[] _categoryIds;

			[SerializeField]
			[Serialize(Name = "layoutIds")]
			private int[] _layoutIds;

			[SerializeField]
			[Serialize(Name = "controllerSetSelector")]
			private ControllerSetSelector _controllerSetSelector;

			[NonSerialized]
			private string[] _preInitCategoryNames;

			[NonSerialized]
			private string[] _preInitLayoutNames;

			internal bool appliesToAllLayouts
			{
				get
				{
					if (_layoutIds != null)
					{
						return _layoutIds.Length == 0;
					}
					return true;
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

			public ControllerSetSelector controllerSetSelector
			{
				get
				{
					return _controllerSetSelector ?? (_controllerSetSelector = new ControllerSetSelector(ControllerSetSelector.Type.ControllerType));
				}
				set
				{
					if (value == null)
					{
						value = new ControllerSetSelector(ControllerSetSelector.Type.ControllerType);
					}
					_controllerSetSelector = value;
				}
			}

			public int[] categoryIds
			{
				get
				{
					Initialize();
					return _categoryIds ?? (_categoryIds = EmptyObjects<int>.array);
				}
				set
				{
					if (value == null)
					{
						value = EmptyObjects<int>.array;
					}
					_categoryIds = value;
					_preInitCategoryNames = null;
				}
			}

			public int[] layoutIds
			{
				get
				{
					Initialize();
					return _layoutIds ?? (_layoutIds = EmptyObjects<int>.array);
				}
				set
				{
					if (value == null)
					{
						value = EmptyObjects<int>.array;
					}
					_layoutIds = value;
					_preInitLayoutNames = null;
					if (value != null || value.Length != 0)
					{
						CheckNoControllerTypeError();
					}
				}
			}

			public int categoryId
			{
				get
				{
					Initialize();
					if (_categoryIds == null || _categoryIds.Length == 0)
					{
						return -1;
					}
					return categoryIds[0];
				}
				set
				{
					if (value < 0)
					{
						_categoryIds = EmptyObjects<int>.array;
					}
					else
					{
						if (_categoryIds == null || _categoryIds.Length == 0)
						{
							_categoryIds = new int[1];
						}
						_categoryIds[0] = value;
					}
					_preInitCategoryNames = null;
				}
			}

			public int layoutId
			{
				get
				{
					Initialize();
					if (_layoutIds == null || _layoutIds.Length == 0)
					{
						return -1;
					}
					return layoutIds[0];
				}
				set
				{
					if (value < 0)
					{
						_layoutIds = EmptyObjects<int>.array;
					}
					else
					{
						if (_layoutIds == null || _layoutIds.Length == 0)
						{
							_layoutIds = new int[1];
						}
						_layoutIds[0] = value;
					}
					if (value >= 0)
					{
						CheckNoControllerTypeError();
					}
					_preInitLayoutNames = null;
				}
			}

			public string[] categoryNames
			{
				get
				{
					if (!ReInput.isReady)
					{
						if (_preInitCategoryNames == null)
						{
							return EmptyObjects<string>.array;
						}
						return _preInitCategoryNames;
					}
					Initialize();
					if (_categoryIds == null)
					{
						return EmptyObjects<string>.array;
					}
					string[] array = new string[_categoryIds.Length];
					for (int i = 0; i < _categoryIds.Length; i++)
					{
						InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(_categoryIds[i]);
						array[i] = ((mapCategory != null) ? mapCategory.name : "INVALID");
					}
					return array;
				}
				set
				{
					if (!ReInput.isReady)
					{
						_preInitCategoryNames = ((value != null && value.Length != 0) ? value : null);
						_categoryIds = EmptyObjects<int>.array;
						return;
					}
					if (value == null || value.Length == 0)
					{
						_preInitCategoryNames = null;
						_categoryIds = EmptyObjects<int>.array;
						return;
					}
					List<int> list = new List<int>(value.Length);
					for (int i = 0; i < value.Length; i++)
					{
						if (!string.IsNullOrEmpty(value[i]))
						{
							int mapCategoryId = ReInput.mapping.GetMapCategoryId(value[i]);
							if (mapCategoryId >= 0)
							{
								list.Add(mapCategoryId);
							}
							else
							{
								Logger.LogWarning("Map Category \"" + value[i] + "\" does not exist.");
							}
						}
					}
					_categoryIds = list.ToArray();
				}
			}

			public string[] layoutNames
			{
				get
				{
					if (!ReInput.isReady)
					{
						if (_preInitLayoutNames == null)
						{
							return EmptyObjects<string>.array;
						}
						return _preInitLayoutNames;
					}
					Initialize();
					if (_layoutIds == null)
					{
						return EmptyObjects<string>.array;
					}
					string[] array = new string[_layoutIds.Length];
					for (int i = 0; i < _layoutIds.Length; i++)
					{
						InputLayout layout = ReInput.mapping.GetLayout(controllerSetSelector.controllerType, _layoutIds[i]);
						array[i] = ((layout != null) ? layout.name : "INVALID");
					}
					return array;
				}
				set
				{
					if (!ReInput.isReady)
					{
						if (value != null && value.Length != 0)
						{
							CheckNoControllerTypeError();
						}
						_preInitLayoutNames = ((value != null && value.Length != 0) ? value : null);
						_layoutIds = EmptyObjects<int>.array;
						return;
					}
					if (value == null || value.Length == 0)
					{
						_preInitLayoutNames = null;
						_layoutIds = EmptyObjects<int>.array;
						return;
					}
					CheckNoControllerTypeError();
					List<int> list = new List<int>(value.Length);
					for (int i = 0; i < value.Length; i++)
					{
						if (!string.IsNullOrEmpty(value[i]))
						{
							int num = ReInput.mapping.GetLayoutId(controllerSetSelector.controllerType, value[i]);
							if (num >= 0)
							{
								list.Add(num);
							}
							else
							{
								Logger.LogWarning("Layout \"" + value[i] + "\" does not exist.");
							}
						}
					}
					_layoutIds = list.ToArray();
				}
			}

			public string categoryName
			{
				get
				{
					if (!ReInput.isReady)
					{
						if (_preInitCategoryNames == null || _preInitCategoryNames.Length == 0)
						{
							return null;
						}
						return _preInitCategoryNames[0];
					}
					Initialize();
					if (_categoryIds == null || _categoryIds.Length == 0)
					{
						return null;
					}
					InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(_categoryIds[0]);
					if (mapCategory == null)
					{
						return "INVALID";
					}
					return mapCategory.name;
				}
				set
				{
					if (!ReInput.isReady)
					{
						_preInitCategoryNames = (string.IsNullOrEmpty(value) ? null : new string[1] { value });
						_categoryIds = EmptyObjects<int>.array;
						return;
					}
					if (string.IsNullOrEmpty(value))
					{
						_preInitCategoryNames = null;
						_categoryIds = EmptyObjects<int>.array;
						return;
					}
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(value);
					if (mapCategoryId >= 0)
					{
						categoryId = mapCategoryId;
					}
					else
					{
						Logger.LogWarning("Map Category \"" + value + "\" does not exist.");
					}
				}
			}

			public string layoutName
			{
				get
				{
					if (!ReInput.isReady)
					{
						if (_preInitLayoutNames == null || _preInitLayoutNames.Length == 0)
						{
							return null;
						}
						return _preInitLayoutNames[0];
					}
					Initialize();
					if (_layoutIds == null || _layoutIds.Length == 0)
					{
						return null;
					}
					InputLayout layout = ReInput.mapping.GetLayout(controllerSetSelector.controllerType, _layoutIds[0]);
					if (layout == null)
					{
						return "INVALID";
					}
					return layout.name;
				}
				set
				{
					if (!ReInput.isReady)
					{
						if (!string.IsNullOrEmpty(value))
						{
							CheckNoControllerTypeError();
						}
						_preInitLayoutNames = (string.IsNullOrEmpty(value) ? null : new string[1] { value });
						_layoutIds = EmptyObjects<int>.array;
					}
					else if (string.IsNullOrEmpty(value))
					{
						_preInitLayoutNames = null;
						_layoutIds = EmptyObjects<int>.array;
					}
					else
					{
						CheckNoControllerTypeError();
						int num = ReInput.mapping.GetLayoutId(controllerSetSelector.controllerType, value);
						if (num >= 0)
						{
							layoutId = num;
						}
						else
						{
							Logger.LogWarning("Map Layout \"" + value + "\" does not exist.");
						}
					}
				}
			}

			internal bool isValid
			{
				get
				{
					if (_controllerSetSelector == null)
					{
						return false;
					}
					if (!ReInput.isReady)
					{
						return true;
					}
					Initialize();
					if (_categoryIds != null && _categoryIds.Length != 0)
					{
						bool flag = false;
						for (int i = 0; i < _categoryIds.Length; i++)
						{
							if (ReInput.mapping.GetMapCategory(_categoryIds[i]) != null)
							{
								flag = true;
							}
						}
						if (!flag)
						{
							return false;
						}
					}
					if (_layoutIds != null && _layoutIds.Length != 0)
					{
						bool flag2 = false;
						for (int j = 0; j < _layoutIds.Length; j++)
						{
							if (ReInput.mapping.GetLayout(_controllerSetSelector.controllerType, _layoutIds[j]) != null)
							{
								flag2 = true;
							}
						}
						if (!flag2)
						{
							return false;
						}
					}
					return true;
				}
			}

			public Rule()
			{
				_enable = true;
				_categoryIds = EmptyObjects<int>.array;
				_layoutIds = EmptyObjects<int>.array;
				_controllerSetSelector = new ControllerSetSelector(ControllerSetSelector.Type.ControllerType);
			}

			public Rule(Rule P_0)
			{
				if (P_0 == null)
				{
					throw new ArgumentNullException("source");
				}
				_tag = P_0._tag;
				_enable = P_0._enable;
				_categoryIds = ArrayTools.ShallowCopy(P_0._categoryIds);
				_layoutIds = ArrayTools.ShallowCopy(P_0._layoutIds);
				_controllerSetSelector = MiscTools.DeepClone(P_0._controllerSetSelector);
				_preInitCategoryNames = ArrayTools.ShallowCopy(P_0._preInitCategoryNames);
				_preInitLayoutNames = ArrayTools.ShallowCopy(P_0._preInitLayoutNames);
			}

			internal Rule(string P_0, bool P_1, int[] P_2, int[] P_3, ControllerSetSelector P_4)
			{
				_tag = P_0;
				_enable = P_1;
				_categoryIds = P_2;
				_layoutIds = P_3;
				_controllerSetSelector = P_4;
			}

			internal bool Matches(ControllerMap map)
			{
				if (map == null)
				{
					return false;
				}
				if (!isValid)
				{
					return false;
				}
				if (_categoryIds != null && _categoryIds.Length != 0 && !ArrayTools.Contains(_categoryIds, map.categoryId))
				{
					return false;
				}
				if (_layoutIds != null && _layoutIds.Length != 0 && !ArrayTools.Contains(_layoutIds, map.layoutId))
				{
					return false;
				}
				if (!_controllerSetSelector.Matches(map.controller))
				{
					return false;
				}
				return true;
			}

			private void Initialize()
			{
				if (!ReInput.isReady || _controllerSetSelector == null)
				{
					return;
				}
				if (_categoryIds == null)
				{
					_categoryIds = EmptyObjects<int>.array;
				}
				if (_preInitCategoryNames != null && _preInitCategoryNames.Length != 0)
				{
					List<int> list = new List<int>(_preInitCategoryNames.Length);
					for (int i = 0; i < _preInitCategoryNames.Length; i++)
					{
						if (!string.IsNullOrEmpty(_preInitCategoryNames[i]))
						{
							int mapCategoryId = ReInput.mapping.GetMapCategoryId(_preInitCategoryNames[i]);
							if (mapCategoryId >= 0)
							{
								list.Add(mapCategoryId);
							}
							else
							{
								Logger.LogWarning("Map Category \"" + _preInitCategoryNames[i] + "\" does not exist.");
							}
						}
					}
					_categoryIds = list.ToArray();
					_preInitCategoryNames = null;
				}
				if (_preInitLayoutNames == null || _preInitLayoutNames.Length == 0)
				{
					return;
				}
				CheckNoControllerTypeError();
				List<int> list2 = new List<int>(_preInitLayoutNames.Length);
				for (int j = 0; j < _preInitLayoutNames.Length; j++)
				{
					if (!string.IsNullOrEmpty(_preInitLayoutNames[j]))
					{
						int num = ReInput.mapping.GetLayoutId(_controllerSetSelector.controllerType, _preInitLayoutNames[j]);
						if (num >= 0)
						{
							list2.Add(num);
						}
						else
						{
							Logger.LogWarning("Map Layout \"" + _preInitLayoutNames[j] + "\" does not exist.");
						}
					}
				}
				_layoutIds = list2.ToArray();
				_preInitLayoutNames = null;
			}

			private void CheckNoControllerTypeError()
			{
				if (_controllerSetSelector != null && !_controllerSetSelector.QVWXuasRsUAvognxOkisyYrwXInSA)
				{
					Logger.LogWarning("A Layout should not be set when using " + typeof(ControllerSetSelector.Type).FullName + "." + _controllerSetSelector.type.ToString() + " because each Controller type has its own unique Layouts.", requiredThreadSafety: true);
				}
			}

			object IDeepCloneable.DeepClone()
			{
				return new Rule(this);
			}
		}

		[Serializable]
		[DefaultMember("Item")]
		[Preserve]
		[SerializationType(SerializationTypeAttribute.SerializationType.Object)]
		[CustomClassObfuscation(renamePrivateMembers = false, renamePubIntMembers = false)]
		public sealed class RuleSet : IList<Rule>, ICollection<Rule>, IEnumerable<Rule>, IEnumerable, IDeepCloneable
		{
			private const string className = "ControllerMapEnabler.RuleSet";

			[SerializeField]
			[Serialize(Name = "enabled")]
			private bool _enabled;

			[SerializeField]
			[Serialize(Name = "tag")]
			private string _tag;

			[SerializeField]
			[Serialize(Name = "rules")]
			private List<Rule> _rules;

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

			public List<Rule> rules
			{
				get
				{
					return _rules;
				}
				set
				{
					_rules = value;
					CheckList();
				}
			}

			Rule IList<Rule>.this[int index]
			{
				get
				{
					CheckList();
					return _rules[index];
				}
				set
				{
					CheckList();
					_rules[index] = value;
				}
			}

			int ICollection<Rule>.Count
			{
				get
				{
					CheckList();
					return _rules.Count;
				}
			}

			bool ICollection<Rule>.IsReadOnly
			{
				get
				{
					CheckList();
					return ((ICollection<Rule>)_rules).IsReadOnly;
				}
			}

			internal RuleSet(bool P_0, string P_1, List<Rule> P_2)
				: this()
			{
				_enabled = P_0;
				_tag = P_1;
				_rules = P_2;
				CheckList();
			}

			public RuleSet()
			{
				_enabled = true;
				_rules = new List<Rule>();
			}

			public RuleSet(RuleSet P_0)
			{
				if (P_0 == null)
				{
					throw new ArgumentNullException("source");
				}
				_enabled = P_0._enabled;
				_tag = P_0._tag;
				_rules = MiscTools.DeepClone(P_0._rules);
				CheckList();
			}

			public Rule Find(Predicate<Rule> predicate)
			{
				if (predicate == null)
				{
					throw new ArgumentNullException("predicate");
				}
				int num = ((_rules != null) ? _rules.Count : 0);
				for (int i = 0; i < num; i++)
				{
					try
					{
						if (predicate(_rules[i]))
						{
							return _rules[i];
						}
					}
					catch (Exception exception)
					{
						ReInput.HandleCallbackException("ControllerMapEnabler.RuleSet.Find", exception);
					}
				}
				return null;
			}

			public Rule FindLast(Predicate<Rule> predicate)
			{
				if (predicate == null)
				{
					throw new ArgumentNullException("predicate");
				}
				for (int num = ((_rules != null) ? _rules.Count : 0) - 1; num >= 0; num--)
				{
					try
					{
						if (predicate(_rules[num]))
						{
							return _rules[num];
						}
					}
					catch (Exception exception)
					{
						ReInput.HandleCallbackException("ControllerMapEnabler.RuleSet.FindLast", exception);
					}
				}
				return null;
			}

			public int FindIndex(Predicate<Rule> predicate)
			{
				if (predicate == null)
				{
					throw new ArgumentNullException("predicate");
				}
				int num = ((_rules != null) ? _rules.Count : 0);
				for (int i = 0; i < num; i++)
				{
					try
					{
						if (predicate(_rules[i]))
						{
							return i;
						}
					}
					catch (Exception exception)
					{
						ReInput.HandleCallbackException("ControllerMapEnabler.RuleSet.FindIndex", exception);
					}
				}
				return -1;
			}

			public int FindLastIndex(Predicate<Rule> predicate)
			{
				if (predicate == null)
				{
					throw new ArgumentNullException("predicate");
				}
				for (int num = ((_rules != null) ? _rules.Count : 0) - 1; num >= 0; num--)
				{
					try
					{
						if (predicate(_rules[num]))
						{
							return num;
						}
					}
					catch (Exception exception)
					{
						ReInput.HandleCallbackException("ControllerMapEnabler.RuleSet.FindLastIndex", exception);
					}
				}
				return -1;
			}

			public int IndexOf(Rule item)
			{
				CheckList();
				return _rules.Count;
			}

			int IList<Rule>.IndexOf(Rule item)
			{
				//ILSpy generated this explicit interface implementation from .override directive in IndexOf
				return this.IndexOf(item);
			}

			public void Insert(int index, Rule item)
			{
				CheckList();
				_rules.Insert(index, item);
			}

			void IList<Rule>.Insert(int index, Rule item)
			{
				//ILSpy generated this explicit interface implementation from .override directive in Insert
				this.Insert(index, item);
			}

			public void RemoveAt(int index)
			{
				CheckList();
				_rules.RemoveAt(index);
			}

			void IList<Rule>.RemoveAt(int index)
			{
				//ILSpy generated this explicit interface implementation from .override directive in RemoveAt
				this.RemoveAt(index);
			}

			public void Add(Rule item)
			{
				CheckList();
				_rules.Add(item);
			}

			void ICollection<Rule>.Add(Rule item)
			{
				//ILSpy generated this explicit interface implementation from .override directive in Add
				this.Add(item);
			}

			public void Clear()
			{
				CheckList();
				_rules.Clear();
			}

			void ICollection<Rule>.Clear()
			{
				//ILSpy generated this explicit interface implementation from .override directive in Clear
				this.Clear();
			}

			public bool Contains(Rule item)
			{
				CheckList();
				return _rules.Contains(item);
			}

			bool ICollection<Rule>.Contains(Rule item)
			{
				//ILSpy generated this explicit interface implementation from .override directive in Contains
				return this.Contains(item);
			}

			public void CopyTo(Rule[] array, int arrayIndex)
			{
				CheckList();
				_rules.CopyTo(array, arrayIndex);
			}

			void ICollection<Rule>.CopyTo(Rule[] array, int arrayIndex)
			{
				//ILSpy generated this explicit interface implementation from .override directive in CopyTo
				this.CopyTo(array, arrayIndex);
			}

			public bool Remove(Rule item)
			{
				CheckList();
				return _rules.Remove(item);
			}

			bool ICollection<Rule>.Remove(Rule item)
			{
				//ILSpy generated this explicit interface implementation from .override directive in Remove
				return this.Remove(item);
			}

			public IEnumerator<Rule> GetEnumerator()
			{
				CheckList();
				return _rules.GetEnumerator();
			}

			IEnumerator<Rule> IEnumerable<Rule>.GetEnumerator()
			{
				//ILSpy generated this explicit interface implementation from .override directive in GetEnumerator
				return this.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				CheckList();
				return _rules.GetEnumerator();
			}

			object IDeepCloneable.DeepClone()
			{
				return new RuleSet(this);
			}

			private void CheckList()
			{
				if (_rules == null)
				{
					_rules = new List<Rule>();
				}
			}
		}

		internal class CyXCJRXKNwVfGTMPVLIRUynNmCKI
		{
			public bool FTZgdpIKGuZTYqEPdvpRPhrYLEtQ;

			public VKBFHptpOwZTIxZpGslzThtgRcTO[] bYtDpVerNwnPVaboXhZiCscKAMVh;

			public CyXCJRXKNwVfGTMPVLIRUynNmCKI(bool P_0, VKBFHptpOwZTIxZpGslzThtgRcTO[] P_1)
			{
				FTZgdpIKGuZTYqEPdvpRPhrYLEtQ = P_0;
				bYtDpVerNwnPVaboXhZiCscKAMVh = P_1;
			}
		}

		private bool EEZBPtCHKWgKRxskJCaeFmtmTnRj;

		private Player muacVVhHkEYxrEHVQAyhdfwxzaNJA;

		private CyXCJRXKNwVfGTMPVLIRUynNmCKI pidZOdEGannxpltLqLlbPecMiPDc;

		private readonly int yILaHSbYFjcvGajBIuwMunxshbFqA;

		private List<RuleSet> jpLteRUMQjFmWRuAkVSEojJGWMjE;

		public bool enabled
		{
			get
			{
				return EEZBPtCHKWgKRxskJCaeFmtmTnRj;
			}
			set
			{
				EEZBPtCHKWgKRxskJCaeFmtmTnRj = value;
				if (value)
				{
					Apply();
				}
			}
		}

		public List<RuleSet> ruleSets
		{
			get
			{
				return jpLteRUMQjFmWRuAkVSEojJGWMjE;
			}
			set
			{
				if (value == null)
				{
					value = new List<RuleSet>();
				}
				jpLteRUMQjFmWRuAkVSEojJGWMjE = value;
			}
		}

		internal ControllerMapEnabler(Player P_0, CyXCJRXKNwVfGTMPVLIRUynNmCKI P_1)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("player");
			}
			if (P_1 == null)
			{
				throw new ArgumentNullException("startingSettings");
			}
			yILaHSbYFjcvGajBIuwMunxshbFqA = ReInput.id;
			muacVVhHkEYxrEHVQAyhdfwxzaNJA = P_0;
			pidZOdEGannxpltLqLlbPecMiPDc = P_1;
		}

		public void Apply()
		{
			if (ReInput._id != yILaHSbYFjcvGajBIuwMunxshbFqA)
			{
				ReInput.CheckInitialized(yILaHSbYFjcvGajBIuwMunxshbFqA);
			}
			else
			{
				if (!EEZBPtCHKWgKRxskJCaeFmtmTnRj || jpLteRUMQjFmWRuAkVSEojJGWMjE == null)
				{
					return;
				}
				int count = jpLteRUMQjFmWRuAkVSEojJGWMjE.Count;
				if (count == 0)
				{
					return;
				}
				using TempListPool.TList<ControllerMap> tList = TempListPool.GetTList<ControllerMap>();
				List<ControllerMap> list = tList.list;
				muacVVhHkEYxrEHVQAyhdfwxzaNJA.controllers.maps.GetAllMaps(list);
				int count2 = list.Count;
				for (int i = 0; i < count; i++)
				{
					RuleSet ruleSet = jpLteRUMQjFmWRuAkVSEojJGWMjE[i];
					if (ruleSet == null || !ruleSet.enabled)
					{
						continue;
					}
					int num = ruleSet.System_002ECollections_002EGeneric_002EICollection_00601_003CRewired_002EControllerMapEnabler_002ERule_003E_002ECount;
					for (int j = 0; j < num; j++)
					{
						Rule rule = ruleSet[j];
						if (rule == null)
						{
							continue;
						}
						for (int k = 0; k < count2; k++)
						{
							ControllerMap controllerMap = list[k];
							if (controllerMap.enabled != rule.enable && rule.Matches(controllerMap))
							{
								controllerMap.enabled = rule.enable;
							}
						}
					}
				}
			}
		}

		public void LoadDefaults()
		{
			if (ReInput._id != yILaHSbYFjcvGajBIuwMunxshbFqA)
			{
				ReInput.CheckInitialized(yILaHSbYFjcvGajBIuwMunxshbFqA);
				return;
			}
			List<RuleSet> list = new List<RuleSet>();
			int num = ((pidZOdEGannxpltLqLlbPecMiPDc != null && pidZOdEGannxpltLqLlbPecMiPDc.bYtDpVerNwnPVaboXhZiCscKAMVh != null) ? pidZOdEGannxpltLqLlbPecMiPDc.bYtDpVerNwnPVaboXhZiCscKAMVh.Length : 0);
			for (int i = 0; i < num; i++)
			{
				RuleSet controllerMapEnablerRuleSetInstance = ReInput.mapping.GetControllerMapEnablerRuleSetInstance(pidZOdEGannxpltLqLlbPecMiPDc.bYtDpVerNwnPVaboXhZiCscKAMVh[i].ZfJoYctgPpVSnpftahQTlMmpPLQm);
				if (controllerMapEnablerRuleSetInstance == null)
				{
					Logger.LogError("Invalid Map Enabler Manager Rule Set is assigned to Player. This should not be possible. If you are seeing this error, this is a sign of serialized data corruption, usually caused by a bad source control merge.");
					continue;
				}
				controllerMapEnablerRuleSetInstance.enabled = pidZOdEGannxpltLqLlbPecMiPDc.bYtDpVerNwnPVaboXhZiCscKAMVh[i].dcQXOoEkwgQXbaVwOAumDixTNHlg;
				list.Add(controllerMapEnablerRuleSetInstance);
			}
			if (pidZOdEGannxpltLqLlbPecMiPDc != null)
			{
				EEZBPtCHKWgKRxskJCaeFmtmTnRj = pidZOdEGannxpltLqLlbPecMiPDc.FTZgdpIKGuZTYqEPdvpRPhrYLEtQ;
			}
			jpLteRUMQjFmWRuAkVSEojJGWMjE = list;
			Apply();
		}

		public string ToXmlString()
		{
			if (ReInput._id != yILaHSbYFjcvGajBIuwMunxshbFqA)
			{
				ReInput.CheckInitialized(yILaHSbYFjcvGajBIuwMunxshbFqA);
				return string.Empty;
			}
			try
			{
				return tOlEzrCdQCOikcCSFAVHZdnCyIMBA().ToXmlString(writeDocumentTag: true);
			}
			catch (Exception ex)
			{
				Logger.LogWarning("Error writing " + GetType().Name + " to XML. " + ex.Message);
				return string.Empty;
			}
		}

		public string ToJsonString()
		{
			if (ReInput._id != yILaHSbYFjcvGajBIuwMunxshbFqA)
			{
				ReInput.CheckInitialized(yILaHSbYFjcvGajBIuwMunxshbFqA);
				return string.Empty;
			}
			try
			{
				return tOlEzrCdQCOikcCSFAVHZdnCyIMBA().ToJsonString();
			}
			catch (Exception ex)
			{
				Logger.LogWarning("Error writing " + GetType().Name + " to JSON. " + ex.Message);
				return string.Empty;
			}
		}

		public bool ImportXml(string xmlString)
		{
			if (ReInput._id != yILaHSbYFjcvGajBIuwMunxshbFqA)
			{
				ReInput.CheckInitialized(yILaHSbYFjcvGajBIuwMunxshbFqA);
				return false;
			}
			try
			{
				qruUqYrXpVaoqMBDHGSkaRRkgXUHA(SerializedObject.FromXml(GetType(), xmlString));
				Apply();
				return true;
			}
			catch (Exception ex)
			{
				Logger.LogError("Error importing " + GetType().Name + " data from XML. " + ex.Message);
				return false;
			}
		}

		public bool ImportJson(string jsonString)
		{
			if (ReInput._id != yILaHSbYFjcvGajBIuwMunxshbFqA)
			{
				ReInput.CheckInitialized(yILaHSbYFjcvGajBIuwMunxshbFqA);
				return false;
			}
			try
			{
				qruUqYrXpVaoqMBDHGSkaRRkgXUHA(SerializedObject.FromJson(GetType(), jsonString));
				Apply();
				return true;
			}
			catch (Exception ex)
			{
				Logger.LogError("Error importing " + GetType().Name + " data from JSON. " + ex.Message);
				return false;
			}
		}

		private SerializedObject tOlEzrCdQCOikcCSFAVHZdnCyIMBA()
		{
			SerializedObject serializedObject = new SerializedObject(GetType(), SerializedObject.ObjectType.Object);
			wetYzpQJKmNryoQUHvOXEamBneVi(serializedObject);
			return serializedObject;
		}

		private void wetYzpQJKmNryoQUHvOXEamBneVi(SerializedObject P_0)
		{
			if (P_0.xmlInfo == null)
			{
				P_0.xmlInfo = new SerializedObject.XmlInfo();
			}
			P_0.Add("dataVersion", 1, SerializedObject.FieldOptions.ExculdeFromXml);
			P_0.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.JISccXeaCjmCeBJbWJlLPGJisilo
			{
				qItGqcAiFsVuEXTeGzrduYLlUPFM = "dataVersion",
				vLOpmXQkMsPmsBAJNcXkcKfWoznZ = 1.ToString()
			});
			P_0.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.JISccXeaCjmCeBJbWJlLPGJisilo
			{
				XCTmYYtBCCzTuZmlJPszaypSgJdS = "xmlns",
				qItGqcAiFsVuEXTeGzrduYLlUPFM = "xsi",
				GpDRMyFZBJdjlvWjrvVYAzZnhbYW = null,
				vLOpmXQkMsPmsBAJNcXkcKfWoznZ = "http://www.w3.org/2001/XMLSchema-instance"
			});
			P_0.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.JISccXeaCjmCeBJbWJlLPGJisilo
			{
				XCTmYYtBCCzTuZmlJPszaypSgJdS = "xsi",
				qItGqcAiFsVuEXTeGzrduYLlUPFM = "schemaLocation",
				GpDRMyFZBJdjlvWjrvVYAzZnhbYW = null,
				vLOpmXQkMsPmsBAJNcXkcKfWoznZ = string.Format("{0} {1}{2}{3}{4}{5}", "http://guavaman.com/rewired", "http://guavaman.com/schemas/rewired/", "1.0", "/", GetType().Name, ".xsd")
			});
			P_0.Add("enabled", EEZBPtCHKWgKRxskJCaeFmtmTnRj);
			P_0.Add("ruleSets", jpLteRUMQjFmWRuAkVSEojJGWMjE);
		}

		private bool qruUqYrXpVaoqMBDHGSkaRRkgXUHA(SerializedObject P_0)
		{
			EEZBPtCHKWgKRxskJCaeFmtmTnRj = false;
			jpLteRUMQjFmWRuAkVSEojJGWMjE = null;
			P_0.TryGetDeserializedValueByRef("enabled", ref EEZBPtCHKWgKRxskJCaeFmtmTnRj);
			List<RuleSet> value = new List<RuleSet>();
			P_0.TryGetDeserializedValueByRef("ruleSets", ref value);
			jpLteRUMQjFmWRuAkVSEojJGWMjE = value;
			return true;
		}
	}
}
