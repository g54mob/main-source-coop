using System;
using System.Collections.Generic;
using UnityEngine;
using Zorro.Core;

public class AspectRatioUISetter : MonoBehaviour
{
	[Serializable]
	public class AspectPosition
	{
		public Vector2 anchoredPosition;

		public Vector2 anchoredMin;

		public Vector2 anchoredMax;

		public AspectPosition(RectTransform rectTransform)
		{
			anchoredPosition = rectTransform.anchoredPosition;
			anchoredMin = rectTransform.anchorMin;
			anchoredMax = rectTransform.anchorMax;
		}

		public void Load(RectTransform rectTransform)
		{
			rectTransform.anchoredPosition = anchoredPosition;
			rectTransform.anchorMin = anchoredMin;
			rectTransform.anchorMax = anchoredMax;
		}
	}

	[Serializable]
	public class SavedAspectPosition
	{
		public AspectRatio aspectRatio;

		public AspectPosition aspectPosition;

		public SavedAspectPosition(AspectRatio aspectRatio, AspectPosition aspectPosition)
		{
			this.aspectRatio = aspectRatio;
			this.aspectPosition = aspectPosition;
		}
	}

	public AspectPosition defaultAspectPosition;

	public List<SavedAspectPosition> savedAspectPositions = new List<SavedAspectPosition>();

	public void SetDefault()
	{
		defaultAspectPosition = new AspectPosition(GetComponent<RectTransform>());
	}

	public void SetAspectPosition(AspectRatio aspectRatio)
	{
		foreach (SavedAspectPosition savedAspectPosition in savedAspectPositions)
		{
			if (savedAspectPosition.aspectRatio == aspectRatio)
			{
				savedAspectPosition.aspectPosition = new AspectPosition(GetComponent<RectTransform>());
				return;
			}
		}
		savedAspectPositions.Add(new SavedAspectPosition(aspectRatio, new AspectPosition(GetComponent<RectTransform>())));
	}

	public void LoadAspectPosition(AspectRatio aspectRatio)
	{
		RectTransform component = GetComponent<RectTransform>();
		foreach (SavedAspectPosition savedAspectPosition in savedAspectPositions)
		{
			if (savedAspectPosition.aspectRatio == aspectRatio)
			{
				savedAspectPosition.aspectPosition.Load(component);
				return;
			}
		}
		defaultAspectPosition.Load(component);
	}
}
