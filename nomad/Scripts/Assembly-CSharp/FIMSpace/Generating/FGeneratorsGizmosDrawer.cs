using System;
using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace.Generating
{
	public class FGeneratorsGizmosDrawer : MonoBehaviour
	{
		public static FGeneratorsGizmosDrawer Instance;

		private List<Action> onGUI;

		public static void CheckExistence()
		{
			if (!(Instance != null))
			{
				Instance = UnityEngine.Object.FindObjectOfType<FGeneratorsGizmosDrawer>();
				if (!(Instance != null))
				{
					Instance = new GameObject("FGenerators-GizmosDrawer").AddComponent<FGeneratorsGizmosDrawer>();
					Instance.Refresh();
				}
			}
		}

		public static void TemporaryRemove()
		{
			if (!(Instance == null))
			{
				FGenerators.DestroyObject(Instance.gameObject);
				Instance = null;
			}
		}

		public static void AddEvent(Action ev)
		{
			CheckExistence();
			if (Instance.onGUI == null)
			{
				Instance.onGUI = new List<Action>();
			}
			if (!Instance.onGUI.Contains(ev))
			{
				Instance.onGUI.Add(ev);
			}
		}

		private void Refresh()
		{
			if ((bool)Instance && Instance != this)
			{
				FGenerators.DestroyObject(Instance.gameObject);
			}
			Instance = this;
		}

		private void OnEnable()
		{
			Refresh();
		}

		private void Awake()
		{
			Refresh();
		}

		private void OnDrawGizmos()
		{
			Refresh();
			if (onGUI == null)
			{
				onGUI = new List<Action>();
			}
			for (int i = 0; i < onGUI.Count; i++)
			{
				if (onGUI[i] != null)
				{
					onGUI[i]();
				}
			}
		}
	}
}
