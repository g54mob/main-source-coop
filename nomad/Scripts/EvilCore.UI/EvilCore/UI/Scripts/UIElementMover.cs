using PrimeTween;
using UnityEngine;

namespace EvilCore.UI.Scripts
{
	public class UIElementMover : MonoBehaviour
	{
		public enum MovementDirection
		{
			Horizontal = 0,
			Vertical = 1,
			Custom = 2
		}

		[Header("Hareket Ayarları")]
		[SerializeField]
		private RectTransform targetElement;

		[SerializeField]
		private MovementDirection direction;

		[SerializeField]
		private float movementDistance = 20f;

		[SerializeField]
		private float duration = 1f;

		[SerializeField]
		private float delayBetweenMovements = 0.2f;

		[SerializeField]
		private int loopCount = -1;

		[SerializeField]
		private Ease easeType = Ease.InOutQuad;

		[SerializeField]
		private bool playOnStart = true;

		private Sequence _movementSequence;

		private Vector2 originalPosition;

		private void Awake()
		{
			if (targetElement == null)
			{
				targetElement = GetComponent<RectTransform>();
			}
		}

		private void Start()
		{
			originalPosition = targetElement.anchoredPosition;
			if (playOnStart)
			{
				PlayAnimation();
			}
		}

		public void PlayAnimation()
		{
			_movementSequence.Stop();
			Vector2 vector = Vector2.zero;
			switch (direction)
			{
			case MovementDirection.Horizontal:
				vector = new Vector2(movementDistance, 0f);
				break;
			case MovementDirection.Vertical:
				vector = new Vector2(0f, movementDistance);
				break;
			case MovementDirection.Custom:
				vector = new Vector2(movementDistance * 0.7f, movementDistance * 0.7f);
				break;
			}
			int cycles = ((loopCount == -1) ? (-1) : loopCount);
			_movementSequence = Sequence.Create(cycles, Sequence.SequenceCycleMode.Restart, Ease.Linear, useUnscaledTime: true).Chain(Tween.UIAnchoredPosition(targetElement, originalPosition + vector, duration / 2f, easeType, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true)).ChainDelay(delayBetweenMovements)
				.Chain(Tween.UIAnchoredPosition(targetElement, originalPosition, duration / 2f, easeType, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true))
				.ChainDelay(delayBetweenMovements);
		}

		public void StopAnimation()
		{
			_movementSequence.Stop();
			targetElement.anchoredPosition = originalPosition;
		}

		public void SetDirection(MovementDirection newDirection)
		{
			direction = newDirection;
			if (_movementSequence.isAlive)
			{
				StopAnimation();
				PlayAnimation();
			}
		}

		public void SetMovementDistance(float distance)
		{
			movementDistance = distance;
			if (_movementSequence.isAlive)
			{
				StopAnimation();
				PlayAnimation();
			}
		}

		private void OnDestroy()
		{
			_movementSequence.Stop();
		}
	}
}
