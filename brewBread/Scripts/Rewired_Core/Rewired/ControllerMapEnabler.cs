using System;
using System.Collections;
using System.Collections.Generic;
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
		[CustomClassObfuscation(renamePrivateMembers = false, renamePubIntMembers = false)]
		[Preserve]
		public sealed class Rule : IDeepCloneable
		{
			[Serialize(Name = "tag")]
			[SerializeField]
			private string _tag;

			[Serialize(Name = "enable")]
			[SerializeField]
			private bool _enable;

			[SerializeField]
			[Serialize(Name = "categoryIds")]
			private int[] _categoryIds;

			[Serialize(Name = "layoutIds")]
			[SerializeField]
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
				if (_controllerSetSelector != null && !_controllerSetSelector.IRxuBlsWPAUhvrFlMvUAiDQPeqYAA)
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
		[CustomClassObfuscation(renamePrivateMembers = false, renamePubIntMembers = false)]
		[SerializationType(SerializationTypeAttribute.SerializationType.Object)]
		[Preserve]
		public sealed class RuleSet : IList<Rule>, ICollection<Rule>, IEnumerable<Rule>, IEnumerable, IDeepCloneable
		{
			private const string className = "ControllerMapEnabler.RuleSet";

			[Serialize(Name = "enabled")]
			[SerializeField]
			private bool _enabled;

			[Serialize(Name = "tag")]
			[SerializeField]
			private string _tag;

			[Serialize(Name = "rules")]
			[SerializeField]
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

			public Rule this[int index]
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

			public int Count
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

			public void Insert(int index, Rule item)
			{
				CheckList();
				_rules.Insert(index, item);
			}

			public void RemoveAt(int index)
			{
				CheckList();
				_rules.RemoveAt(index);
			}

			public void Add(Rule item)
			{
				CheckList();
				_rules.Add(item);
			}

			public void Clear()
			{
				CheckList();
				_rules.Clear();
			}

			public bool Contains(Rule item)
			{
				CheckList();
				return _rules.Contains(item);
			}

			public void CopyTo(Rule[] array, int arrayIndex)
			{
				CheckList();
				_rules.CopyTo(array, arrayIndex);
			}

			public bool Remove(Rule item)
			{
				CheckList();
				return _rules.Remove(item);
			}

			public IEnumerator<Rule> GetEnumerator()
			{
				CheckList();
				return _rules.GetEnumerator();
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

		internal class oejthPnlmICeXCzkndFXvfozHuLjA
		{
			public bool kzXMFixQhdfqdwRZTXNUgEOsGVFD;

			public vNnAsbNOxQRuVRxCcHhnyysOuoGy[] skDiKcsxfIWRYRUooobRNKLwyssi;

			public oejthPnlmICeXCzkndFXvfozHuLjA(bool P_0, vNnAsbNOxQRuVRxCcHhnyysOuoGy[] P_1)
			{
				kzXMFixQhdfqdwRZTXNUgEOsGVFD = P_0;
				skDiKcsxfIWRYRUooobRNKLwyssi = P_1;
			}
		}

		private bool kKFZZElqKQSUFMZnWdEudRvTJGpo;

		private Player XGUdRXTQSWQSPvuOLhFeAdcEiwfEA;

		private oejthPnlmICeXCzkndFXvfozHuLjA XzXJPYjgnEWOyBozllWRpzkenCmx;

		private readonly int QajDFFaomlkLHzaostfWYpGUioys;

		private List<RuleSet> CVIrxabQeQqQApdMyogYZKmdHqZeA;

		public bool enabled
		{
			get
			{
				return kKFZZElqKQSUFMZnWdEudRvTJGpo;
			}
			set
			{
				kKFZZElqKQSUFMZnWdEudRvTJGpo = value;
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
				return CVIrxabQeQqQApdMyogYZKmdHqZeA;
			}
			set
			{
				if (value == null)
				{
					value = new List<RuleSet>();
				}
				CVIrxabQeQqQApdMyogYZKmdHqZeA = value;
			}
		}

		internal ControllerMapEnabler(Player P_0, oejthPnlmICeXCzkndFXvfozHuLjA P_1)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("player");
			}
			if (P_1 == null)
			{
				throw new ArgumentNullException("startingSettings");
			}
			QajDFFaomlkLHzaostfWYpGUioys = ReInput.id;
			XGUdRXTQSWQSPvuOLhFeAdcEiwfEA = P_0;
			XzXJPYjgnEWOyBozllWRpzkenCmx = P_1;
		}

		public void Apply()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
			}
			else
			{
				if (!kKFZZElqKQSUFMZnWdEudRvTJGpo || CVIrxabQeQqQApdMyogYZKmdHqZeA == null)
				{
					return;
				}
				int count = CVIrxabQeQqQApdMyogYZKmdHqZeA.Count;
				if (count == 0)
				{
					return;
				}
				using TempListPool.TList<ControllerMap> tList = TempListPool.GetTList<ControllerMap>();
				List<ControllerMap> list = tList.list;
				XGUdRXTQSWQSPvuOLhFeAdcEiwfEA.controllers.maps.GetAllMaps(list);
				int count2 = list.Count;
				for (int i = 0; i < count; i++)
				{
					RuleSet ruleSet = CVIrxabQeQqQApdMyogYZKmdHqZeA[i];
					if (ruleSet == null || !ruleSet.enabled)
					{
						continue;
					}
					int count3 = ruleSet.Count;
					for (int j = 0; j < count3; j++)
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
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return;
			}
			List<RuleSet> list = new List<RuleSet>();
			int num = ((XzXJPYjgnEWOyBozllWRpzkenCmx != null && XzXJPYjgnEWOyBozllWRpzkenCmx.skDiKcsxfIWRYRUooobRNKLwyssi != null) ? XzXJPYjgnEWOyBozllWRpzkenCmx.skDiKcsxfIWRYRUooobRNKLwyssi.Length : 0);
			for (int i = 0; i < num; i++)
			{
				RuleSet controllerMapEnablerRuleSetInstance = ReInput.mapping.GetControllerMapEnablerRuleSetInstance(XzXJPYjgnEWOyBozllWRpzkenCmx.skDiKcsxfIWRYRUooobRNKLwyssi[i].QSZlWSsqUhpIVZjOleCIDuxHVUuQ);
				if (controllerMapEnablerRuleSetInstance == null)
				{
					Logger.LogError("Invalid Map Enabler Manager Rule Set is assigned to Player. This should not be possible. If you are seeing this error, this is a sign of serialized data corruption, usually caused by a bad source control merge.");
					continue;
				}
				controllerMapEnablerRuleSetInstance.enabled = XzXJPYjgnEWOyBozllWRpzkenCmx.skDiKcsxfIWRYRUooobRNKLwyssi[i].pwRBzCLpxTIgaUjrQHMniKspsYab;
				list.Add(controllerMapEnablerRuleSetInstance);
			}
			if (XzXJPYjgnEWOyBozllWRpzkenCmx != null)
			{
				kKFZZElqKQSUFMZnWdEudRvTJGpo = XzXJPYjgnEWOyBozllWRpzkenCmx.kzXMFixQhdfqdwRZTXNUgEOsGVFD;
			}
			CVIrxabQeQqQApdMyogYZKmdHqZeA = list;
			Apply();
		}

		public string ToXmlString()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return string.Empty;
			}
			try
			{
				return JxowpgGeLUBIhbyiSIIfFkkGllDuA().ToXmlString(writeDocumentTag: true);
			}
			catch (Exception ex)
			{
				Logger.LogWarning("Error writing " + GetType().Name + " to XML. " + ex.Message);
				return string.Empty;
			}
		}

		public string ToJsonString()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return string.Empty;
			}
			try
			{
				return JxowpgGeLUBIhbyiSIIfFkkGllDuA().ToJsonString();
			}
			catch (Exception ex)
			{
				Logger.LogWarning("Error writing " + GetType().Name + " to JSON. " + ex.Message);
				return string.Empty;
			}
		}

		public bool ImportXml(string xmlString)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			try
			{
				ckPgJNiFQjrLdKfwClUsdsySXFyIb(SerializedObject.FromXml(GetType(), xmlString));
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
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			try
			{
				ckPgJNiFQjrLdKfwClUsdsySXFyIb(SerializedObject.FromJson(GetType(), jsonString));
				Apply();
				return true;
			}
			catch (Exception ex)
			{
				Logger.LogError("Error importing " + GetType().Name + " data from JSON. " + ex.Message);
				return false;
			}
		}

		private SerializedObject JxowpgGeLUBIhbyiSIIfFkkGllDuA()
		{
			SerializedObject serializedObject = new SerializedObject(GetType(), SerializedObject.ObjectType.Object);
			eqxcMNQhZrhfftsCuDbhlTUMXCQB(serializedObject);
			return serializedObject;
		}

		private void eqxcMNQhZrhfftsCuDbhlTUMXCQB(SerializedObject P_0)
		{
			if (P_0.xmlInfo == null)
			{
				P_0.xmlInfo = new SerializedObject.XmlInfo();
			}
			P_0.Add("dataVersion", 1, SerializedObject.FieldOptions.ExculdeFromXml);
			P_0.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.nIytNPQltHNThjHAgjtVciMQQyqsA
			{
				OvVRJOklVDAaDOOGrXwIFuDdpwVJ = "dataVersion",
				yYOUbwIbBcyPAQvjeAEXVsjzLAln = 1.ToString()
			});
			P_0.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.nIytNPQltHNThjHAgjtVciMQQyqsA
			{
				yCwJMdebpVsoAZwRHxyMbDxHYvvT = "xmlns",
				OvVRJOklVDAaDOOGrXwIFuDdpwVJ = "xsi",
				ZVXfpRlSPjKWRYaEYlEnqADXJiAs = null,
				yYOUbwIbBcyPAQvjeAEXVsjzLAln = "http://www.w3.org/2001/XMLSchema-instance"
			});
			P_0.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.nIytNPQltHNThjHAgjtVciMQQyqsA
			{
				yCwJMdebpVsoAZwRHxyMbDxHYvvT = "xsi",
				OvVRJOklVDAaDOOGrXwIFuDdpwVJ = "schemaLocation",
				ZVXfpRlSPjKWRYaEYlEnqADXJiAs = null,
				yYOUbwIbBcyPAQvjeAEXVsjzLAln = string.Format("{0} {1}{2}{3}{4}{5}", "http://guavaman.com/rewired", "http://guavaman.com/schemas/rewired/", "1.0", "/", GetType().Name, ".xsd")
			});
			P_0.Add("enabled", kKFZZElqKQSUFMZnWdEudRvTJGpo);
			P_0.Add("ruleSets", CVIrxabQeQqQApdMyogYZKmdHqZeA);
		}

		private bool ckPgJNiFQjrLdKfwClUsdsySXFyIb(SerializedObject P_0)
		{
			kKFZZElqKQSUFMZnWdEudRvTJGpo = false;
			CVIrxabQeQqQApdMyogYZKmdHqZeA = null;
			P_0.TryGetDeserializedValueByRef("enabled", ref kKFZZElqKQSUFMZnWdEudRvTJGpo);
			List<RuleSet> value = new List<RuleSet>();
			P_0.TryGetDeserializedValueByRef("ruleSets", ref value);
			CVIrxabQeQqQApdMyogYZKmdHqZeA = value;
			return true;
		}
	}
}
