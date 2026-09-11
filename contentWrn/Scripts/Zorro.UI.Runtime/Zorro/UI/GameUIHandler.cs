using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zorro.UI
{
	[DefaultExecutionOrder(100)]
	public class GameUIHandler : MonoBehaviour
	{
		private GameUI m_gameUI;

		private void Start()
		{
			m_gameUI = GetComponent<GameUI>();
			foreach (Type key in m_gameUI.GetUISystems().Keys)
			{
				GameUI.Hide(key);
			}
		}

		private void LateUpdate()
		{
			foreach (KeyValuePair<Type, GameUISystem> uISystem in m_gameUI.GetUISystems())
			{
				Type key = uISystem.Key;
				GameUISystem value = uISystem.Value;
				bool flag = value.ShouldShow();
				bool flag2 = m_gameUI.OpenSystems.Contains(value);
				if (flag && !flag2)
				{
					GameUI.ShowUI(key);
				}
				else if (flag2 && !flag)
				{
					GameUI.Hide(key);
				}
			}
		}
	}
}
