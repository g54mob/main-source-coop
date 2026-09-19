using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Features.TutorialModule.Scripts.GuideModule
{
	public class WorldUITipEntity : TipEntity
	{
		[SerializeField]
		private List<Image> _images;

		protected override void InitializeRenderer()
		{
			base.InitializeRenderer();
			foreach (Image image in _images)
			{
				image.material = new Material(image.material);
			}
		}

		protected override void ApplyDissolve(float amount)
		{
			if (_images == null)
			{
				return;
			}
			foreach (Image image in _images)
			{
				if (!(image == null) && !(image.material == null))
				{
					image.material.SetFloat(TipEntity.DissolveId, amount);
				}
			}
		}
	}
}
