using System.Collections.Generic;
using Features.AudioDevicesModule.Scripts.PushToTalk.Views;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.PlayerItemViewModule.Scripts
{
	public abstract class PlayerItemViewBase : ViewBehaviour
	{
		[field: SerializeField]
		public TMP_Text PlayerName { get; set; }

		[field: SerializeField]
		public TMP_Text Ping { get; set; }

		[field: SerializeField]
		public List<Image> PlayerIcons { get; set; }

		[field: SerializeField]
		public Animator Animator { get; set; }

		[field: SerializeField]
		public float VoiceAnimationThreshold { get; set; }

		[field: SerializeField]
		public GameObject CrossGameObject { get; set; }

		[field: SerializeField]
		public Button KickButton { get; set; }

		[field: SerializeField]
		public Transform PushToTalkContainer { get; set; }

		[field: SerializeField]
		public PushToTalkHintViewBase PushToTalkHintViewBase { get; set; }

		[field: SerializeField]
		public bool SpawnPushToTalkDisabled { get; set; }

		[field: SerializeField]
		public GameObject DeadPartDamagedObject { get; set; }

		public abstract void SetDeadColor(bool isDead);
	}
}
