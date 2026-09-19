using System;
using System.Linq;
using Features.AIModule.Scripts;
using UnityEngine;

namespace Features.DebugModule.Scripts
{
	public class EnemySpawnDebugView : EnemySpawnDebugViewBase
	{
		private EnemyType[] _availableEnemyTypes;

		public void SetupEnemyDropdown()
		{
			_availableEnemyTypes = (from EnemyType type in Enum.GetValues(typeof(EnemyType))
				where type != EnemyType.None
				select type).ToArray();
			base.EnemyDropdown.ClearOptions();
			base.EnemyDropdown.AddOptions(_availableEnemyTypes.Select((EnemyType type) => type.ToString()).ToList());
			base.EnemyDropdown.SetValueWithoutNotify(0);
			base.EnemyDropdown.RefreshShownValue();
		}

		public EnemyType GetSelectedEnemyType()
		{
			if (_availableEnemyTypes == null || _availableEnemyTypes.Length == 0)
			{
				return EnemyType.None;
			}
			int num = Mathf.Clamp(base.EnemyDropdown.value, 0, _availableEnemyTypes.Length - 1);
			return _availableEnemyTypes[num];
		}
	}
}
