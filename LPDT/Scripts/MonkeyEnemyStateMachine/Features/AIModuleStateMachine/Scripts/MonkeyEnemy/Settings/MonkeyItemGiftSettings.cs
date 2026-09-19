using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings
{
	[CreateAssetMenu(fileName = "MonkeyItemGiftSettings_Default", menuName = "Configurations/AIModuleStateMachine/Monkey/MonkeyItemGiftSettings")]
	public class MonkeyItemGiftSettings : ScriptableObject
	{
		[field: SerializeField]
		[field: Range(0f, 1f)]
		public float ItemGiftChance { get; private set; } = 0.27f;

		[field: SerializeField]
		public float GivingItemDuration { get; private set; } = 1f;

		[field: SerializeField]
		public float ItemScaleDuration { get; private set; } = 2f;

		[field: SerializeField]
		public Ease ItemScaleEase { get; private set; } = Ease.OutBack;

		[field: SerializeField]
		public List<MonkeyItemGiftEntry> GiftItems { get; private set; } = new List<MonkeyItemGiftEntry>();
	}
}
