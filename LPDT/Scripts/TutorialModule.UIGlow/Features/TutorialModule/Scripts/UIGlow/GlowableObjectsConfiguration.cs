using System.Collections.Generic;
using UnityEngine;

namespace Features.TutorialModule.Scripts.UIGlow
{
	[CreateAssetMenu(fileName = "GlowableObjectsConfiguration_Default", menuName = "Configurations/TutorialModule/GlowableObjectsConfiguration")]
	public class GlowableObjectsConfiguration : ScriptableObject
	{
		[SerializeField]
		private List<GlowableObjectEnum> _alloweMultipleGlowableObjects = new List<GlowableObjectEnum>();

		public bool IsAllowedMultipleGlowableObjects(GlowableObjectEnum glowableObjectEnum)
		{
			return _alloweMultipleGlowableObjects.Contains(glowableObjectEnum);
		}
	}
}
