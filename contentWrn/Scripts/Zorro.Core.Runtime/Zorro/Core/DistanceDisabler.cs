using System;
using UnityEngine;

namespace Zorro.Core
{
	public class DistanceDisabler : MonoBehaviour
	{
		public enum Target
		{
			GameObject = 0,
			Behaviour = 1
		}

		public float Distance = 100f;

		public Target target;

		public Behaviour behaviourTarget;

		public GameObject gameObjectTarget;

		private bool m_registered;

		public void SetNewState(bool active)
		{
			switch (target)
			{
			case Target.Behaviour:
				behaviourTarget.enabled = active;
				break;
			case Target.GameObject:
				gameObjectTarget.SetActive(active);
				break;
			default:
				throw new ArgumentOutOfRangeException("Mode not supported");
			}
		}

		private bool ShowGameObject()
		{
			return target == Target.GameObject;
		}

		private bool ShowBehaviour()
		{
			return target == Target.Behaviour;
		}

		private void Start()
		{
			Singleton<DistanceDisablerHandler>.Instance.RegisterDistanceDisabler(this);
			m_registered = true;
		}

		private void OnDestroy()
		{
			if (Singleton<DistanceDisablerHandler>.Instance != null && m_registered)
			{
				Singleton<DistanceDisablerHandler>.Instance.UnregisterDistanceDisabler(this);
				m_registered = false;
			}
		}
	}
}
