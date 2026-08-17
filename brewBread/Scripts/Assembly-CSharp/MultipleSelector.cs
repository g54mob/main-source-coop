using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class MultipleSelector : MonoBehaviour
{
	[Header("Options")]
	public TextMeshProUGUI text;

	public List<string> optionsList;

	[Header("Arrow Animation")]
	public GameObject leftArrow;

	public GameObject rightArrow;

	[Tooltip("Force applied to the arrows to animate")]
	public float punchForce = 10f;

	[HideInInspector]
	public Selector slider;

	private Slider slide;

	public UnityEvent valueChanged;

	private int valueAux;

	public int Value
	{
		get
		{
			return GetValue();
		}
		set
		{
			SetValue(value);
		}
	}

	private void Awake()
	{
		slide = GetComponent<Slider>();
	}

	private void Start()
	{
		valueAux = slider.value;
		slide.onValueChanged.AddListener(ValueChanged);
	}

	public void SetBoundaries(int min, int max)
	{
		slider.minValue = min;
		slide.minValue = min;
		slider.maxValue = max;
		slide.maxValue = max;
	}

	private int GetValue()
	{
		return slider.value;
	}

	private void SetValue(float value)
	{
		slide.value = value;
		ValueChanged(value);
	}

	private void ValueChanged(float newValue)
	{
		if ((float)slider.value != newValue)
		{
			slider.value = (int)newValue;
		}
		if (newValue == -1f)
		{
			slider.value = slider.maxValue - 1;
			slide.value = slider.value;
			return;
		}
		if (newValue == (float)slider.maxValue)
		{
			slider.value = 0;
			slide.value = slider.value;
			return;
		}
		slide.value = slider.value;
		AnimateArrows();
		SetSelectorText((int)newValue);
		valueChanged?.Invoke();
	}

	public void SetSelectorText(int newValue)
	{
		text.text = optionsList[newValue];
	}

	private void AnimateArrows()
	{
		if (slider.value == slider.maxValue - 1 && valueAux == 0)
		{
			leftArrow.transform.DOComplete();
			leftArrow.transform.DOPunchPosition(new Vector3(0f - punchForce + leftArrow.transform.position.x, 0f, 0f), 0.5f);
		}
		else if (valueAux == slider.maxValue - 1 && slider.value == 0)
		{
			rightArrow.transform.DOComplete();
			rightArrow.transform.DOPunchPosition(new Vector3(punchForce + rightArrow.transform.position.x, 0f, 0f), 0.5f);
		}
		else if (valueAux > slider.value)
		{
			leftArrow.transform.DOComplete();
			leftArrow.transform.DOPunchPosition(new Vector3(0f - punchForce + leftArrow.transform.position.x, 0f, 0f), 0.5f);
		}
		else if (valueAux < slider.value)
		{
			rightArrow.transform.DOComplete();
			rightArrow.transform.DOPunchPosition(new Vector3(punchForce + rightArrow.transform.position.x, 0f, 0f), 0.5f);
		}
		valueAux = slider.value;
	}

	public void AddValue(int adding)
	{
		if (slider.value + adding < -1)
		{
			slider.value = slider.maxValue;
		}
		else if (slider.value + adding > slider.maxValue)
		{
			slider.value = 0;
		}
		else
		{
			slider.value += adding;
		}
		ValueChanged(slider.value);
	}
}
