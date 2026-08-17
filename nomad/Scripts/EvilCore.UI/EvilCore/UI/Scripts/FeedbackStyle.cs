using System;
using Ami.BroAudio;
using UnityEngine;

namespace EvilCore.UI.Scripts
{
	[Serializable]
	public class FeedbackStyle
	{
		[Header("Audio")]
		[Tooltip("Played via IAudioManager.PlayOneShotUI when this feedback is shown. Leave empty to skip.")]
		[SerializeField]
		private SoundID sound;

		[Header("Visuals")]
		[SerializeField]
		private Color accentColor = Color.white;

		[Tooltip("Optional severity icon. Hidden automatically when unset.")]
		[SerializeField]
		private Sprite icon;

		[Header("Entrance")]
		[SerializeField]
		private FeedbackEntrance entrance;

		[Tooltip("Starting scale for Fade / PopIn entrances, as a fraction of the resting scale.")]
		[SerializeField]
		[Range(0.1f, 1f)]
		private float entranceScaleFrom = 0.9f;

		[Tooltip("Punch scale strength for the Warning feel.")]
		[SerializeField]
		private Vector3 punchStrength = new Vector3(0.2f, 0.2f, 0f);

		[Tooltip("Shake offset strength for the Error feel.")]
		[SerializeField]
		private Vector3 shakeStrength = new Vector3(14f, 14f, 0f);

		[SerializeField]
		private float accentDuration = 0.35f;

		[SerializeField]
		private float accentFrequency = 12f;

		[Header("Lifecycle")]
		[Tooltip("How long the message holds at full opacity before fading out.")]
		[SerializeField]
		private float onScreenDuration = 0.6f;

		[SerializeField]
		private float riseDuration = 0.5f;

		[SerializeField]
		private float fadeDuration = 0.4f;

		[Tooltip("How far the message travels upward as it rises in.")]
		[SerializeField]
		private float riseDistance = 250f;

		public SoundID Sound => sound;

		public Color AccentColor => accentColor;

		public Sprite Icon => icon;

		public FeedbackEntrance Entrance => entrance;

		public float EntranceScaleFrom => entranceScaleFrom;

		public Vector3 PunchStrength => punchStrength;

		public Vector3 ShakeStrength => shakeStrength;

		public float AccentDuration => accentDuration;

		public float AccentFrequency => accentFrequency;

		public float OnScreenDuration => onScreenDuration;

		public float RiseDuration => riseDuration;

		public float FadeDuration => fadeDuration;

		public float RiseDistance => riseDistance;
	}
}
