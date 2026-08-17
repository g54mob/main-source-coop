using System.Collections;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace PrimeTweenDemo
{
	public class Demo : MonoBehaviour
	{
		private enum AnimateAllType
		{
			Sequence = 0,
			Async = 1,
			Coroutine = 2
		}

		[SerializeField]
		private AnimateAllType animateAllType;

		[SerializeField]
		private Slider sequenceTimelineSlider;

		[SerializeField]
		private Text pausedLabel;

		[SerializeField]
		private Button animateAllPartsButton;

		[SerializeField]
		private TypewriterAnimatorExample typewriterAnimatorExample;

		[SerializeField]
		private Animatable[] animatables;

		[SerializeField]
		private Wheels wheels;

		private bool isAnimatingWithCoroutineOrAsync;

		public Sequence animateAllSequence;

		private bool notifySliderChanged = true;

		private void Awake()
		{
			PrimeTweenConfig.SetTweensCapacity(100);
		}

		private void OnEnable()
		{
			sequenceTimelineSlider.fillRect.gameObject.SetActive(value: false);
			sequenceTimelineSlider.onValueChanged.AddListener(SequenceTimelineSliderChanged);
		}

		private void OnDisable()
		{
			sequenceTimelineSlider.onValueChanged.RemoveListener(SequenceTimelineSliderChanged);
		}

		private void SequenceTimelineSliderChanged(float sliderValue)
		{
			if (notifySliderChanged)
			{
				if (!animateAllSequence.isAlive)
				{
					wheels.OnClick();
				}
				animateAllSequence.isPaused = true;
				animateAllSequence.progressTotal = sliderValue;
			}
		}

		private void UpdateSlider()
		{
			bool flag = animateAllType == AnimateAllType.Sequence && !isAnimatingWithCoroutineOrAsync;
			sequenceTimelineSlider.gameObject.SetActive(flag);
			if (flag)
			{
				pausedLabel.gameObject.SetActive(animateAllSequence.isAlive && animateAllSequence.isPaused);
				bool isAlive = animateAllSequence.isAlive;
				sequenceTimelineSlider.handleRect.gameObject.SetActive(isAlive);
				if (isAlive)
				{
					notifySliderChanged = false;
					sequenceTimelineSlider.value = animateAllSequence.progressTotal;
					notifySliderChanged = true;
				}
			}
		}

		private void Update()
		{
			animateAllPartsButton.GetComponent<Image>().enabled = !isAnimatingWithCoroutineOrAsync;
			animateAllPartsButton.GetComponentInChildren<Text>().enabled = !isAnimatingWithCoroutineOrAsync;
			UpdateSlider();
		}

		public void AnimateAll(bool toEndValue)
		{
			if (!isAnimatingWithCoroutineOrAsync)
			{
				switch (animateAllType)
				{
				case AnimateAllType.Sequence:
					AnimateAllSequence(toEndValue);
					break;
				case AnimateAllType.Async:
					AnimateAllAsync(toEndValue);
					break;
				case AnimateAllType.Coroutine:
					StartCoroutine(AnimateAllCoroutine(toEndValue));
					break;
				}
			}
		}

		private void AnimateAllSequence(bool toEndValue)
		{
			if (animateAllSequence.isAlive)
			{
				animateAllSequence.isPaused = !animateAllSequence.isPaused;
				return;
			}
			animateAllSequence = Sequence.Create();
			animateAllSequence.Group(typewriterAnimatorExample.Animate());
			float num = 0f;
			Animatable[] array = animatables;
			foreach (Animatable animatable in array)
			{
				animateAllSequence.Insert(num, animatable.Animate(toEndValue));
				num += 0.6f;
			}
		}

		private async void AnimateAllAsync(bool toEndValue)
		{
			isAnimatingWithCoroutineOrAsync = true;
			Animatable[] array = animatables;
			for (int i = 0; i < array.Length; i++)
			{
				await array[i].Animate(toEndValue);
			}
			isAnimatingWithCoroutineOrAsync = false;
		}

		private IEnumerator AnimateAllCoroutine(bool toEndValue)
		{
			isAnimatingWithCoroutineOrAsync = true;
			Animatable[] array = animatables;
			foreach (Animatable animatable in array)
			{
				yield return animatable.Animate(toEndValue).ToYieldInstruction();
			}
			isAnimatingWithCoroutineOrAsync = false;
		}
	}
}
