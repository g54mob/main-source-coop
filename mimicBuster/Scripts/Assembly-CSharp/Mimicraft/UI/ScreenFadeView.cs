using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class ScreenFadeView : MonoBehaviour
	{
		[Tooltip("Full-screen black Image. Authored in the scene with Alpha 0 and Raycast Target off.")]
		[SerializeField]
		private Image target;

		private float current;

		private float goal;

		private float speed;

		public static ScreenFadeView Instance { get; private set; }

		public float Alpha => current;

		private void Awake()
		{
			Instance = this;
			if (target == null)
			{
				target = GetComponent<Image>();
			}
			if (target == null)
			{
				Debug.LogWarning("[ScreenFadeView] Image bağlanmamış - karartma çalışmayacak.");
				base.enabled = false;
			}
			else
			{
				current = (goal = 0f);
				Apply();
			}
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}

		public void FadeTo(float alpha, float seconds)
		{
			goal = Mathf.Clamp01(alpha);
			speed = ((seconds > 0.001f) ? (Mathf.Abs(goal - current) / seconds) : float.MaxValue);
		}

		public void SetImmediate(float alpha)
		{
			current = (goal = Mathf.Clamp01(alpha));
			speed = float.MaxValue;
			Apply();
		}

		private void Update()
		{
			if (!Mathf.Approximately(current, goal))
			{
				current = Mathf.MoveTowards(current, goal, speed * Time.unscaledDeltaTime);
				Apply();
			}
		}

		private void Apply()
		{
			Color color = target.color;
			color.a = current;
			target.color = color;
			if (target.enabled != current > 0.001f)
			{
				target.enabled = current > 0.001f;
			}
		}
	}
}
