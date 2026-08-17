using GameKit.Dependencies.Inspectors;
using UnityEngine;

namespace GameKit.Utilities.Types
{
	public class CanvasGroupFader : MonoBehaviour
	{
		public enum FadeGoalType
		{
			Unset = 0,
			Hidden = 1,
			Visible = 2
		}

		[Tooltip("CanvasGroup to fade in and out.")]
		[SerializeField]
		[Group("Components", false)]
		protected CanvasGroup CanvasGroup;

		[Tooltip("True to update the CanvasGroup blocking settings when showing and hiding.")]
		[SerializeField]
		[Group("Effects", false)]
		protected bool UpdateCanvasBlocking = true;

		[SerializeField]
		[Group("Effects", false)]
		protected float FadeInDuration = 0.1f;

		[SerializeField]
		[Group("Effects", false)]
		protected float FadeOutDuration = 0.3f;

		private bool _completedOnce;

		public FadeGoalType FadeGoal { get; private set; }

		public bool IsHiding => FadeGoal == FadeGoalType.Hidden;

		public bool IsVisible => CanvasGroup.alpha > 0f;

		protected virtual void OnEnable()
		{
			FadeGoal = ((!(CanvasGroup.alpha > 0f)) ? FadeGoalType.Hidden : FadeGoalType.Visible);
		}

		protected virtual void OnDisable()
		{
			if (FadeGoal == FadeGoalType.Visible)
			{
				ShowImmediately();
			}
			else
			{
				HideImmediately();
			}
		}

		protected virtual void Update()
		{
			Fade();
		}

		public virtual void ShowImmediately()
		{
			SetFadeGoal(fadeIn: true);
			CompleteFade(fadingIn: true);
			OnShow();
		}

		public virtual void HideImmediately()
		{
			SetFadeGoal(fadeIn: false);
			CompleteFade(fadingIn: false);
			OnHide();
		}

		public virtual void Show()
		{
			if (FadeInDuration <= 0f)
			{
				ShowImmediately();
				return;
			}
			SetFadeGoal(fadeIn: true);
			OnShow();
		}

		protected virtual void OnShow()
		{
		}

		public virtual void Hide()
		{
			if (FadeOutDuration <= 0f)
			{
				HideImmediately();
				return;
			}
			SetCanvasGroupBlockingType(CanvasGroupBlockingType.Block);
			SetFadeGoal(fadeIn: false);
			OnHide();
		}

		protected virtual void OnHide()
		{
		}

		private void SetFadeGoal(bool fadeIn)
		{
			FadeGoal = ((!fadeIn) ? FadeGoalType.Hidden : FadeGoalType.Visible);
		}

		private void Fade()
		{
			if (FadeGoal == FadeGoalType.Unset)
			{
				Debug.LogError(base.gameObject.name + " has an unset FadeGoal. This should not be possible.");
				return;
			}
			bool flag = FadeGoal == FadeGoalType.Visible;
			float num;
			float num2;
			if (flag)
			{
				num = 1f;
				num2 = FadeInDuration;
			}
			else
			{
				num = 0f;
				num2 = FadeOutDuration;
			}
			if (!_completedOnce || CanvasGroup.alpha != num)
			{
				float num3 = 1f / num2;
				CanvasGroup.alpha = Mathf.MoveTowards(CanvasGroup.alpha, num, num3 * Time.deltaTime);
				if (CanvasGroup.alpha == num)
				{
					CompleteFade(flag);
				}
			}
		}

		protected virtual void CompleteFade(bool fadingIn)
		{
			CanvasGroupBlockingType canvasGroupBlockingType;
			float alpha;
			if (fadingIn)
			{
				canvasGroupBlockingType = CanvasGroupBlockingType.Block;
				alpha = 1f;
			}
			else
			{
				canvasGroupBlockingType = CanvasGroupBlockingType.DoNotBlock;
				alpha = 0f;
			}
			SetCanvasGroupBlockingType(canvasGroupBlockingType);
			CanvasGroup.alpha = alpha;
			_completedOnce = true;
		}

		protected virtual void SetCanvasGroupBlockingType(CanvasGroupBlockingType blockingType)
		{
			if (UpdateCanvasBlocking)
			{
				CanvasGroup.SetBlockingType(blockingType);
			}
		}
	}
}
