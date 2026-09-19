using System;
using UnityEngine;
using Zenject;

namespace Features.CartUpgradesModule.Scripts.Core
{
	public class CartUpgradeModulesView : MonoBehaviour
	{
		[SerializeField]
		private CartUpgradeModuleBinding[] _bindings = Array.Empty<CartUpgradeModuleBinding>();

		private ICartUpgradeService _cartUpgradeService;

		private bool _isSubscribed;

		[Inject]
		public void InjectDependencies(ICartUpgradeService cartUpgradeService)
		{
			_cartUpgradeService = cartUpgradeService;
			Subscribe();
			ApplyActiveModules();
		}

		private void OnEnable()
		{
			Subscribe();
			ApplyActiveModules();
		}

		private void OnDisable()
		{
			Unsubscribe();
		}

		private void Subscribe()
		{
			if (!_isSubscribed && _cartUpgradeService != null)
			{
				_cartUpgradeService.OnModulesChanged += ApplyModules;
				_isSubscribed = true;
			}
		}

		private void Unsubscribe()
		{
			if (_isSubscribed)
			{
				_cartUpgradeService.OnModulesChanged -= ApplyModules;
				_isSubscribed = false;
			}
		}

		private void ApplyActiveModules()
		{
			if (_cartUpgradeService != null)
			{
				ApplyModules(_cartUpgradeService.ActiveModulesMask);
			}
		}

		private void ApplyModules(int modulesMask)
		{
			CartUpgradeModuleBinding[] bindings = _bindings;
			foreach (CartUpgradeModuleBinding cartUpgradeModuleBinding in bindings)
			{
				bool flag = cartUpgradeModuleBinding.Module.IsInMask(modulesMask);
				GameObject[] objects = cartUpgradeModuleBinding.Objects;
				foreach (GameObject gameObject in objects)
				{
					if (gameObject != null)
					{
						gameObject.SetActive(flag);
					}
				}
				objects = cartUpgradeModuleBinding.ReplacedObjects;
				foreach (GameObject gameObject2 in objects)
				{
					if (gameObject2 != null)
					{
						gameObject2.SetActive(!flag);
					}
				}
				Behaviour[] behaviours = cartUpgradeModuleBinding.Behaviours;
				foreach (Behaviour behaviour in behaviours)
				{
					if (behaviour != null)
					{
						behaviour.enabled = flag;
					}
				}
			}
		}
	}
}
