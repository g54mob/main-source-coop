using System;
using System.Collections.Generic;
using System.Linq;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.UINavigationModuleRealization.Scripts.BackButton
{
	[CreateAssetMenu(fileName = "UIBackButtonPriorityConfiguration_Default", menuName = "Configurations/UINavigation/UIBackButtonPriorityConfiguration")]
	public class UIBackButtonPriorityConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public SerializableDictionary<BackButtonProcessorType, int> TypePriorities { get; private set; } = new SerializableDictionary<BackButtonProcessorType, int>();

		public int GetPriority(BackButtonProcessorType type)
		{
			if (TypePriorities != null && TypePriorities.ContainsKey(type))
			{
				return TypePriorities[type];
			}
			Debug.LogError(string.Format("{0} missing priority for {1}.", "UIBackButtonPriorityConfiguration", type), this);
			return 0;
		}

		public IReadOnlyList<BackButtonProcessorType> GetTypesByDescendingPriority()
		{
			return Enum.GetValues(typeof(BackButtonProcessorType)).Cast<BackButtonProcessorType>().OrderByDescending(GetPriority)
				.ToList();
		}
	}
}
