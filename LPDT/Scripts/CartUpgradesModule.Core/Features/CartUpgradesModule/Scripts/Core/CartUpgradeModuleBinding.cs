using System;
using UnityEngine;

namespace Features.CartUpgradesModule.Scripts.Core
{
	[Serializable]
	public class CartUpgradeModuleBinding
	{
		[SerializeField]
		private CartUpgradeModule _module;

		[SerializeField]
		private GameObject[] _objects = Array.Empty<GameObject>();

		[SerializeField]
		private GameObject[] _replacedObjects = Array.Empty<GameObject>();

		[SerializeField]
		private Behaviour[] _behaviours = Array.Empty<Behaviour>();

		public CartUpgradeModule Module => _module;

		public GameObject[] Objects => _objects;

		public GameObject[] ReplacedObjects => _replacedObjects;

		public Behaviour[] Behaviours => _behaviours;
	}
}
