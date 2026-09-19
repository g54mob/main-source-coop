using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.AIModule.Scripts.Views
{
	public class EnemiesFearDebugView : EnemiesFearDebugViewBase
	{
		private EnemyType[] _availableEnemyTypes;

		private readonly HashSet<EnemyType> _selectedFearEnemies = new HashSet<EnemyType>();

		public void SetupEnemyDropdown()
		{
			_availableEnemyTypes = (from EnemyType type in Enum.GetValues(typeof(EnemyType))
				where type != EnemyType.None
				select type).ToArray();
			base.FearEnemyDropdown.ClearOptions();
			base.FearEnemyDropdown.AddOptions(_availableEnemyTypes.Select((EnemyType type) => type.ToString()).ToList());
		}

		public void SetupFearEnemiesDropdown()
		{
			_selectedFearEnemies.Clear();
			base.FearEnemiesDropdown.ClearOptions();
			base.FearEnemiesDropdown.AddOptions(BuildFearEnemiesOptions());
			base.FearEnemiesDropdown.SetValueWithoutNotify(0);
			base.FearEnemiesDropdown.RefreshShownValue();
		}

		public void ToggleFearEnemySelectionByDropdownIndex(int index)
		{
			if (_availableEnemyTypes != null && _availableEnemyTypes.Length != 0)
			{
				int num = Mathf.Clamp(index, 0, _availableEnemyTypes.Length - 1);
				EnemyType item = _availableEnemyTypes[num];
				if (!_selectedFearEnemies.Add(item))
				{
					_selectedFearEnemies.Remove(item);
				}
				base.FearEnemiesDropdown.ClearOptions();
				base.FearEnemiesDropdown.AddOptions(BuildFearEnemiesOptions());
				base.FearEnemiesDropdown.SetValueWithoutNotify(num);
				base.FearEnemiesDropdown.RefreshShownValue();
			}
		}

		public EnemyType GetSelectedEnemyType()
		{
			if (_availableEnemyTypes == null || _availableEnemyTypes.Length == 0)
			{
				return EnemyType.None;
			}
			return _availableEnemyTypes[Mathf.Clamp(base.FearEnemyDropdown.value, 0, _availableEnemyTypes.Length - 1)];
		}

		public List<EnemyType> GetSelectedFearEnemiesList()
		{
			return _selectedFearEnemies.ToList();
		}

		private List<string> BuildFearEnemiesOptions()
		{
			return _availableEnemyTypes.Select((EnemyType enemyType) => string.Format("{0} {1}", _selectedFearEnemies.Contains(enemyType) ? "[x]" : "[ ]", enemyType)).ToList();
		}
	}
}
