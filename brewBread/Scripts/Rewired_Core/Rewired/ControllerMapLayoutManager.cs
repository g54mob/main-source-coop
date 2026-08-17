using System;
using System.Collections;
using System.Collections.Generic;
using Rewired.Interfaces;
using Rewired.Utils;
using Rewired.Utils.Attributes;
using Rewired.Utils.Classes.Data;
using Rewired.Utils.Interfaces;
using Rewired.Utils.Libraries.TinyJson;
using UnityEngine;

namespace Rewired
{
	public sealed class ControllerMapLayoutManager
	{
		internal class iZBHTLMJEscsCxujGlXwPdwYSOZv
		{
			public bool kzXMFixQhdfqdwRZTXNUgEOsGVFD;

			public bool RPjxhQSdjcTHwaKJnCMbHbwTpzYDA;

			public vNnAsbNOxQRuVRxCcHhnyysOuoGy[] skDiKcsxfIWRYRUooobRNKLwyssi;

			public iZBHTLMJEscsCxujGlXwPdwYSOZv(bool P_0, bool P_1, vNnAsbNOxQRuVRxCcHhnyysOuoGy[] P_2)
			{
				kzXMFixQhdfqdwRZTXNUgEOsGVFD = P_0;
				RPjxhQSdjcTHwaKJnCMbHbwTpzYDA = P_1;
				skDiKcsxfIWRYRUooobRNKLwyssi = P_2;
			}
		}

		[Serializable]
		[Preserve]
		[CustomClassObfuscation(renamePrivateMembers = false, renamePubIntMembers = false)]
		public sealed class Rule : IDeepCloneable
		{
			[Serialize(Name = "tag")]
			[SerializeField]
			private string _tag;

			[Serialize(Name = "categoryIds")]
			[SerializeField]
			private int[] _categoryIds;

			[Serialize(Name = "layoutId")]
			[SerializeField]
			private int _layoutId;

			[Serialize(Name = "controllerSetSelector")]
			[SerializeField]
			private ControllerSetSelector _controllerSetSelector;

			[NonSerialized]
			private string[] _preInitCategoryNames;

			[NonSerialized]
			private string _preInitLayoutName;

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
					if (!value.IRxuBlsWPAUhvrFlMvUAiDQPeqYAA)
					{
						Logger.LogError(value.type.ToString() + " is not allowed. Each Controller Type has its own unique Layouts and a single Layout cannot be set for all Controller Types. ControllerSelector.type has been changed to ControllerSelector.Type.ControllerType.", requiredThreadSafety: true);
						value.type = ControllerSetSelector.Type.ControllerType;
					}
					_controllerSetSelector = value;
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

			public int layoutId
			{
				get
				{
					Initialize();
					return _layoutId;
				}
				set
				{
					if (value < 0)
					{
						value = -1;
					}
					_layoutId = value;
					_preInitLayoutName = null;
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

			public string layoutName
			{
				get
				{
					if (!ReInput.isReady)
					{
						return _preInitLayoutName;
					}
					Initialize();
					InputLayout layout = ReInput.mapping.GetLayout(controllerSetSelector.controllerType, _layoutId);
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
						_preInitLayoutName = value;
						_layoutId = -1;
						return;
					}
					if (string.IsNullOrEmpty(value))
					{
						_preInitLayoutName = null;
						_layoutId = -1;
						return;
					}
					layoutId = ReInput.mapping.GetLayoutId(controllerSetSelector.controllerType, value);
					if (_layoutId < 0)
					{
						Logger.LogWarning(controllerSetSelector.controllerType.ToString() + " Layout \"" + value + "\" does not exist.");
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
					Initialize();
					if (_categoryIds == null || _categoryIds.Length == 0)
					{
						return false;
					}
					if (!ReInput.isReady)
					{
						if (_categoryIds[0] >= 0)
						{
							return _layoutId >= 0;
						}
						return false;
					}
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
					return ReInput.mapping.GetLayout(_controllerSetSelector.controllerType, _layoutId) != null;
				}
			}

			public Rule()
			{
				_categoryIds = EmptyObjects<int>.array;
				_layoutId = -1;
				_controllerSetSelector = new ControllerSetSelector(ControllerSetSelector.Type.ControllerType);
			}

			public Rule(Rule P_0)
			{
				if (P_0 == null)
				{
					throw new ArgumentNullException("source");
				}
				_tag = P_0._tag;
				_categoryIds = ArrayTools.ShallowCopy(P_0._categoryIds);
				_layoutId = P_0._layoutId;
				_controllerSetSelector = MiscTools.DeepClone(P_0._controllerSetSelector);
				_preInitCategoryNames = ArrayTools.ShallowCopy(P_0._preInitCategoryNames);
				_preInitLayoutName = P_0._preInitLayoutName;
			}

			internal Rule(string P_0, int[] P_1, int P_2, ControllerSetSelector P_3)
			{
				_tag = P_0;
				_categoryIds = P_1;
				_layoutId = P_2;
				_controllerSetSelector = P_3;
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
				if (!string.IsNullOrEmpty(_preInitLayoutName))
				{
					layoutName = _preInitLayoutName;
					_preInitLayoutName = null;
				}
			}

			object IDeepCloneable.DeepClone()
			{
				return new Rule(this);
			}
		}

		[Serializable]
		[Preserve]
		[SerializationType(SerializationTypeAttribute.SerializationType.Object)]
		[CustomClassObfuscation(renamePrivateMembers = false, renamePubIntMembers = false)]
		public sealed class RuleSet : IList<Rule>, ICollection<Rule>, IEnumerable<Rule>, IEnumerable, IDeepCloneable
		{
			private const string className = "ControllerMapLayoutManager.RuleSet";

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
						ReInput.HandleCallbackException("ControllerMapLayoutManager.RuleSet.Find", exception);
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
						ReInput.HandleCallbackException("ControllerMapLayoutManager.RuleSet.FindLast", exception);
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
						ReInput.HandleCallbackException("ControllerMapLayoutManager.RuleSet.FindIndex", exception);
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
						ReInput.HandleCallbackException("ControllerMapLayoutManager.RuleSet.FindLastIndex", exception);
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

		private bool kKFZZElqKQSUFMZnWdEudRvTJGpo;

		private bool yaJjuxjAwvQOrxZOVvBEDekOmAml = true;

		private Player XGUdRXTQSWQSPvuOLhFeAdcEiwfEA;

		private iZBHTLMJEscsCxujGlXwPdwYSOZv XzXJPYjgnEWOyBozllWRpzkenCmx;

		private readonly int QajDFFaomlkLHzaostfWYpGUioys;

		private List<RuleSet> CVIrxabQeQqQApdMyogYZKmdHqZeA;

		private Action SfnipWeCwWMelczDtJvvLzklmVCp;

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

		public bool loadFromUserDataStore
		{
			get
			{
				return yaJjuxjAwvQOrxZOVvBEDekOmAml;
			}
			set
			{
				yaJjuxjAwvQOrxZOVvBEDekOmAml = value;
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

		internal event Action AQUdKrvukOGrQjObMyNlMmxVWmvsA
		{
			add
			{
				SfnipWeCwWMelczDtJvvLzklmVCp = (Action)Delegate.Combine(SfnipWeCwWMelczDtJvvLzklmVCp, b);
			}
			remove
			{
				SfnipWeCwWMelczDtJvvLzklmVCp = (Action)Delegate.Remove(SfnipWeCwWMelczDtJvvLzklmVCp, value2);
			}
		}

		internal ControllerMapLayoutManager(Player P_0, iZBHTLMJEscsCxujGlXwPdwYSOZv P_1)
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
				return;
			}
			SfnipWeCwWMelczDtJvvLzklmVCp?.Invoke();
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
			using TempListPool.TList<Controller> tList2 = TempListPool.GetTList<Controller>();
			List<Controller> list2 = tList2.list;
			if (!list2.Contains(ReInput.controllers.Keyboard))
			{
				list2.Add(ReInput.controllers.Keyboard);
			}
			if (!list2.Contains(ReInput.controllers.Mouse))
			{
				list2.Add(ReInput.controllers.Mouse);
			}
			XGUdRXTQSWQSPvuOLhFeAdcEiwfEA.controllers.maps.GetAllMaps(list);
			list2.AddRange(XGUdRXTQSWQSPvuOLhFeAdcEiwfEA.controllers.Controllers);
			IControllerMapStore controllerMapStore = ReInput.userDataStore as IControllerMapStore;
			for (int i = 0; i < count; i++)
			{
				RuleSet ruleSet = CVIrxabQeQqQApdMyogYZKmdHqZeA[i];
				if (ruleSet == null || !ruleSet.enabled)
				{
					continue;
				}
				int count2 = ruleSet.Count;
				for (int j = 0; j < count2; j++)
				{
					Rule rule = ruleSet[j];
					if (rule == null || !rule.isValid)
					{
						continue;
					}
					for (int num = list.Count - 1; num >= 0; num--)
					{
						ControllerMap controllerMap = list[num];
						if (rule.controllerSetSelector.Matches(controllerMap.controller) && ArrayTools.Contains(rule.categoryIds, controllerMap.categoryId) && controllerMap.layoutId != rule.layoutId)
						{
							list.RemoveAt(num);
							XGUdRXTQSWQSPvuOLhFeAdcEiwfEA.controllers.maps.RemoveMap(controllerMap.controllerType, controllerMap.controllerId, controllerMap.id);
						}
					}
					foreach (Controller controller in XGUdRXTQSWQSPvuOLhFeAdcEiwfEA.controllers.Controllers)
					{
						if (!rule.controllerSetSelector.Matches(controller))
						{
							continue;
						}
						int[] categoryIds = rule.categoryIds;
						for (int k = 0; k < categoryIds.Length; k++)
						{
							ControllerMap controllerMap2 = XGUdRXTQSWQSPvuOLhFeAdcEiwfEA.controllers.maps.GetMap(controller, categoryIds[k], rule.layoutId);
							if (controllerMap2 != null)
							{
								continue;
							}
							if (yaJjuxjAwvQOrxZOVvBEDekOmAml && controllerMapStore != null)
							{
								try
								{
									controllerMap2 = controllerMapStore.LoadControllerMap(XGUdRXTQSWQSPvuOLhFeAdcEiwfEA.id, controller.identifier, categoryIds[k], rule.layoutId);
								}
								catch (Exception exception)
								{
									ReInput.HandleExternalInterfaceException(typeof(ControllerMapLayoutManager).Name, exception);
								}
								if (controllerMap2 != null)
								{
									XGUdRXTQSWQSPvuOLhFeAdcEiwfEA.controllers.maps.AddMap(controller, controllerMap2);
									continue;
								}
							}
							XGUdRXTQSWQSPvuOLhFeAdcEiwfEA.controllers.maps.LoadMap(controller.type, controller.id, categoryIds[k], rule.layoutId, startEnabled: true);
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
				RuleSet controllerMapLayoutManagerRuleSetInstance = ReInput.mapping.GetControllerMapLayoutManagerRuleSetInstance(XzXJPYjgnEWOyBozllWRpzkenCmx.skDiKcsxfIWRYRUooobRNKLwyssi[i].QSZlWSsqUhpIVZjOleCIDuxHVUuQ);
				if (controllerMapLayoutManagerRuleSetInstance == null)
				{
					Logger.LogError("Invalid Layout Manager Rule Set is assigned to Player. This should not be possible. If you are seeing this error, this is a sign of serialized data corruption, usually caused by a bad source control merge.");
					continue;
				}
				controllerMapLayoutManagerRuleSetInstance.enabled = XzXJPYjgnEWOyBozllWRpzkenCmx.skDiKcsxfIWRYRUooobRNKLwyssi[i].pwRBzCLpxTIgaUjrQHMniKspsYab;
				list.Add(controllerMapLayoutManagerRuleSetInstance);
			}
			if (XzXJPYjgnEWOyBozllWRpzkenCmx != null)
			{
				kKFZZElqKQSUFMZnWdEudRvTJGpo = XzXJPYjgnEWOyBozllWRpzkenCmx.kzXMFixQhdfqdwRZTXNUgEOsGVFD;
				yaJjuxjAwvQOrxZOVvBEDekOmAml = XzXJPYjgnEWOyBozllWRpzkenCmx.RPjxhQSdjcTHwaKJnCMbHbwTpzYDA;
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
			P_0.Add("loadFromUserDataStore", yaJjuxjAwvQOrxZOVvBEDekOmAml);
			P_0.Add("ruleSets", CVIrxabQeQqQApdMyogYZKmdHqZeA);
		}

		private bool ckPgJNiFQjrLdKfwClUsdsySXFyIb(SerializedObject P_0)
		{
			kKFZZElqKQSUFMZnWdEudRvTJGpo = false;
			CVIrxabQeQqQApdMyogYZKmdHqZeA = null;
			P_0.TryGetDeserializedValueByRef("enabled", ref kKFZZElqKQSUFMZnWdEudRvTJGpo);
			P_0.TryGetDeserializedValueByRef("loadFromUserDataStore", ref yaJjuxjAwvQOrxZOVvBEDekOmAml);
			List<RuleSet> value = new List<RuleSet>();
			P_0.TryGetDeserializedValueByRef("ruleSets", ref value);
			CVIrxabQeQqQApdMyogYZKmdHqZeA = value;
			return true;
		}
	}
}
