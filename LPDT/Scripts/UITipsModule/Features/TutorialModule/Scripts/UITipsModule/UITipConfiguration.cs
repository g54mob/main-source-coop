using Global.SerializableDictionary;
using UnityEngine;

namespace Features.TutorialModule.Scripts.UITipsModule
{
	[CreateAssetMenu(fileName = "UITipConfiguration_Default", menuName = "Configurations/UITipsModule/UITipConfiguration")]
	public class UITipConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public SerializableDictionary<UITipType, UITipViewBase> Prefabs { get; private set; }
	}
}
