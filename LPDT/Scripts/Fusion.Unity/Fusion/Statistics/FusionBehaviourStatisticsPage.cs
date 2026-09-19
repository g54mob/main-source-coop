using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Fusion.Statistics
{
	public class FusionBehaviourStatisticsPage : FusionStatisticsPage
	{
		[Header("References")]
		[SerializeField]
		private RectTransform _content;

		[SerializeField]
		private FusionBehaviourStats _behaviourStatsPrefab;

		[SerializeField]
		private MultipleOptionsPanel _addBehaviourPanelPrefab;

		private MultipleOptionsPanel _addBehaviourPanelInstance;

		private Type[] _allBehaviours;

		private List<FusionBehaviourStats> _stats = new List<FusionBehaviourStats>();

		private bool _showFun;

		public override string PageName => "Behaviour";

		public bool DisplayingFun => _showFun;

		public void DisplayFixedUpdateNetwork(bool value)
		{
			_showFun = value;
		}

		public override void Init()
		{
			List<Type> list = new List<Type>();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			TypeInfo typeInfo = typeof(SimulationBehaviour).GetTypeInfo();
			Assembly[] array = assemblies;
			for (int i = 0; i < array.Length; i++)
			{
				foreach (TypeInfo definedType in array[i].DefinedTypes)
				{
					if (definedType.IsSubclassOf(typeInfo))
					{
						list.Add(definedType);
					}
				}
			}
			_allBehaviours = list.ToArray();
			DisplayFixedUpdateNetwork(value: true);
		}

		public void OpenAddBehaviourPanel()
		{
			if (!_addBehaviourPanelInstance)
			{
				_addBehaviourPanelInstance = UnityEngine.Object.Instantiate(_addBehaviourPanelPrefab, FusionStatistics.GlobalStatisticsCanvas.transform);
				_addBehaviourPanelInstance.Setup("Select Behaviour", _allBehaviours, (Type t) => t.Name, AddBehaviourStat);
			}
		}

		private void AddBehaviourStat(Type type)
		{
			if (!_stats.Select((FusionBehaviourStats b) => b.BehaviourType == type).Any((bool r) => r))
			{
				FusionBehaviourStats fusionBehaviourStats = UnityEngine.Object.Instantiate(_behaviourStatsPrefab, _content);
				_stats.Add(fusionBehaviourStats);
				fusionBehaviourStats.Setup(type, this);
			}
		}

		public override void Render()
		{
			foreach (FusionBehaviourStats stat in _stats)
			{
				stat.RefreshView();
			}
		}

		public override void AfterFusionUpdate()
		{
			foreach (FusionBehaviourStats stat in _stats)
			{
				stat.AccumulateRunAndTime(this);
			}
		}

		public void DeleteStat(FusionBehaviourStats fusionBehaviourStats)
		{
			_stats.Remove(fusionBehaviourStats);
			UnityEngine.Object.Destroy(fusionBehaviourStats.gameObject);
		}
	}
}
