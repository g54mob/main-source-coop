using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

namespace Features.ItemsModule.Scripts
{
	public class ItemBreakParts : MonoBehaviour
	{
		[SerializeField]
		private bool _isEnabled;

		[SerializeField]
		private EventReference _breakSound;

		[SerializeField]
		private List<ItemBreakPartData> _parts = new List<ItemBreakPartData>();

		public bool IsEnabled => _isEnabled;

		public EventReference BreakSound => _breakSound;

		public IReadOnlyList<ItemBreakPartData> Parts => _parts;
	}
}
