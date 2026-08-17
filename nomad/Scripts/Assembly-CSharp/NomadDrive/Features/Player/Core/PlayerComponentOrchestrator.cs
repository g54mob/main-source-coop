using System;
using System.Collections.Generic;
using EvilCore.EvilPack.EvilLogger;
using UnityEngine;

namespace NomadDrive.Features.Player.Core
{
	public class PlayerComponentOrchestrator : MonoBehaviour
	{
		private List<IPlayerComponent> _components;

		private bool _isSetupComplete;

		public bool IsSetupComplete => _isSetupComplete;

		private void Awake()
		{
			CacheComponents();
		}

		private void CacheComponents()
		{
			IPlayerComponent[] componentsInChildren = GetComponentsInChildren<IPlayerComponent>();
			_components = new List<IPlayerComponent>(componentsInChildren);
			_components.Sort((IPlayerComponent a, IPlayerComponent b) => a.SetupPriority.CompareTo(b.SetupPriority));
		}

		public void Setup(bool isLocalPlayer)
		{
			if (_components == null || _components.Count == 0)
			{
				CacheComponents();
			}
			foreach (IPlayerComponent component in _components)
			{
				try
				{
					component.SetupForPlayer(isLocalPlayer);
					if ((component as MonoBehaviour)?.GetType().Name == null)
					{
						_ = component.GetType().Name;
					}
				}
				catch (Exception ex)
				{
					string text = (component as MonoBehaviour)?.GetType().Name ?? component.GetType().Name;
					EvilLogger.LogError("[PlayerComponentOrchestrator] Failed to setup " + text + ": " + ex.Message + "\n" + ex.StackTrace, "Setup", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\Core\\PlayerComponentOrchestrator.cs", 65);
				}
			}
			_isSetupComplete = true;
		}
	}
}
