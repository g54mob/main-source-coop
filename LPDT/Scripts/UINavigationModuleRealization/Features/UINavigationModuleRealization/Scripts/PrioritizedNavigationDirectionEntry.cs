using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.UINavigationModuleRealization.Scripts
{
	[Serializable]
	public class PrioritizedNavigationDirectionEntry
	{
		[SerializeField]
		private MoveDirection _direction;

		[SerializeField]
		private List<Selectable> _targets = new List<Selectable>();

		public MoveDirection Direction => _direction;

		public IReadOnlyList<Selectable> Targets => _targets;
	}
}
