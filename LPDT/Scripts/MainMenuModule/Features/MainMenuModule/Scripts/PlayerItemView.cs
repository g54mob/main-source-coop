using System.Collections.Generic;
using Features.PlayerItemViewModule.Scripts;
using UnityEngine;

namespace Features.MainMenuModule.Scripts
{
	internal class PlayerItemView : PlayerItemViewBase
	{
		[SerializeField]
		private float _deadAlphaValue;

		[SerializeField]
		private List<CanvasGroup> _deadCanvasGroups;

		public override void SetDeadColor(bool isDead)
		{
			foreach (CanvasGroup deadCanvasGroup in _deadCanvasGroups)
			{
				if (!(deadCanvasGroup == null))
				{
					if (isDead)
					{
						deadCanvasGroup.alpha = _deadAlphaValue;
					}
					else
					{
						deadCanvasGroup.alpha = 1f;
					}
				}
			}
		}
	}
}
