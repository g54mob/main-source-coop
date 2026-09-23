using System;
using System.Collections.Generic;
using Mimicraft.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAnimationsView : MonoBehaviour
{
	[Serializable]
	public class AnimationDefinitions
	{
		public string stateName;

		public string displayName;

		public Sprite sprite;
	}

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private List<AnimationDefinitions> animationNames = new List<AnimationDefinitions>();

	[SerializeField]
	private GameObject buttonTemplate;

	[SerializeField]
	private string defaultAnimation = "idle";

	private void Awake()
	{
		if (buttonTemplate != null)
		{
			buttonTemplate.SetActive(value: false);
		}
		BuildAnimations();
		PlayAnimation(defaultAnimation);
	}

	private void OnDisable()
	{
		PlayAnimation(defaultAnimation);
	}

	private void BuildAnimations()
	{
		if (buttonTemplate == null)
		{
			return;
		}
		Transform parent = buttonTemplate.transform.parent;
		foreach (AnimationDefinitions definition in animationNames)
		{
			GameObject obj = UnityEngine.Object.Instantiate(buttonTemplate, parent);
			obj.SetActive(value: true);
			obj.GetComponent<Button>().onClick.AddListener(delegate
			{
				PlayAnimation(definition.stateName);
			});
			obj.GetComponent<UITooltipTrigger>().Text = definition.displayName;
			TextMeshProUGUI componentInChildren = obj.GetComponentInChildren<TextMeshProUGUI>();
			Image componentInChildren2 = obj.GetComponentInChildren<Image>();
			if (componentInChildren2 != null && definition.sprite != null)
			{
				componentInChildren2.sprite = definition.sprite;
				if (componentInChildren != null)
				{
					componentInChildren.text = string.Empty;
				}
			}
			if (componentInChildren != null && !componentInChildren2.enabled)
			{
				componentInChildren.text = definition.displayName;
			}
		}
	}

	private void PlayAnimation(string animationName)
	{
		if (!(animator == null) && !string.IsNullOrEmpty(animationName))
		{
			animator.Play(animationName);
		}
	}
}
