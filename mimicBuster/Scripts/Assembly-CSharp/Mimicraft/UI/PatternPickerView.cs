using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class PatternPickerView : MonoBehaviour
	{
		private const int Capacity = 16;

		private const float SwatchSize = 28f;

		private const float Spacing = 4f;

		private const int Columns = 4;

		private static readonly Color SelectedTint = Color.white;

		private static readonly Color UnselectedTint = new Color(0.55f, 0.55f, 0.55f, 1f);

		[SerializeField]
		private List<RawImage> swatchImages = new List<RawImage>();

		[SerializeField]
		private List<Button> swatchButtons = new List<Button>();

		private readonly List<Texture2D> patterns = new List<Texture2D>();

		public event Action<Texture2D> PatternClicked;

		public static PatternPickerView Create(Transform parent)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "PatternPicker");
			GridLayoutGroup gridLayoutGroup = rectTransform.gameObject.AddComponent<GridLayoutGroup>();
			gridLayoutGroup.cellSize = new Vector2(28f, 28f);
			gridLayoutGroup.spacing = new Vector2(4f, 4f);
			gridLayoutGroup.childAlignment = TextAnchor.UpperLeft;
			gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			gridLayoutGroup.constraintCount = 4;
			ContentSizeFitter contentSizeFitter = rectTransform.gameObject.AddComponent<ContentSizeFitter>();
			contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			List<RawImage> list = new List<RawImage>();
			List<Button> list2 = new List<Button>();
			for (int i = 0; i < 16; i++)
			{
				RectTransform rectTransform2 = UIFactory.CreateRect(rectTransform, $"Swatch{i}");
				RawImage rawImage = rectTransform2.gameObject.AddComponent<RawImage>();
				rawImage.color = UnselectedTint;
				Button item = rectTransform2.gameObject.AddComponent<Button>();
				rectTransform2.gameObject.SetActive(value: false);
				list.Add(rawImage);
				list2.Add(item);
			}
			PatternPickerView patternPickerView = rectTransform.gameObject.AddComponent<PatternPickerView>();
			patternPickerView.swatchImages = list;
			patternPickerView.swatchButtons = list2;
			return patternPickerView;
		}

		private void Awake()
		{
			patterns.Clear();
			foreach (RawImage swatchImage in swatchImages)
			{
				if (swatchImage.gameObject.activeSelf && swatchImage.texture is Texture2D item)
				{
					patterns.Add(item);
				}
			}
			for (int i = 0; i < swatchButtons.Count; i++)
			{
				int index = i;
				swatchButtons[i].onClick.RemoveAllListeners();
				swatchButtons[i].onClick.AddListener(delegate
				{
					if (index < patterns.Count)
					{
						this.PatternClicked?.Invoke(patterns[index]);
					}
				});
			}
		}

		public void SetPatterns(IReadOnlyList<Texture2D> textures)
		{
			patterns.Clear();
			patterns.AddRange(textures);
			for (int i = 0; i < swatchImages.Count; i++)
			{
				bool flag = i < patterns.Count;
				swatchImages[i].gameObject.SetActive(flag);
				if (flag)
				{
					swatchImages[i].texture = patterns[i];
				}
			}
		}

		public void SetSelected(Texture2D selected)
		{
			for (int i = 0; i < patterns.Count; i++)
			{
				swatchImages[i].color = ((patterns[i] == selected) ? SelectedTint : UnselectedTint);
			}
		}
	}
}
