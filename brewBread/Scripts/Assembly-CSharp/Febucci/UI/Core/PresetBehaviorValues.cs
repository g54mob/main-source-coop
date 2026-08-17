using System;
using UnityEngine;

namespace Febucci.UI.Core
{
	[Serializable]
	internal class PresetBehaviorValues : PresetBaseValues
	{
		[SerializeField]
		public EmissionControl emission;

		public override void Initialize(bool isAppearance)
		{
			base.Initialize(isAppearance);
			emission.Initialize(GetMaxDuration());
		}
	}
}
