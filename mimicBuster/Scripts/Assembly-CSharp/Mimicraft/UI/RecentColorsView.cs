using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class RecentColorsView : MonoBehaviour
	{
		private const int Capacity = 8;

		private const float SwatchSize = 22f;

		private const float Spacing = 4f;

		private readonly List<Color> colors = new List<Color>();

		[SerializeField]
		private List<Image> swatchImages = new List<Image>();

		[SerializeField]
		private List<Button> swatchButtons = new List<Button>();

		public event Action<Color> SwatchClicked;

		public static RecentColorsView Create(Transform parent)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "RecentColors");
			HorizontalLayoutGroup horizontalLayoutGroup = rectTransform.gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.spacing = 4f;
			horizontalLayoutGroup.childForceExpandWidth = false;
			horizontalLayoutGroup.childForceExpandHeight = false;
			horizontalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
			List<Image> list = new List<Image>();
			List<Button> list2 = new List<Button>();
			for (int i = 0; i < 8; i++)
			{
				Image image = UIFactory.CreatePanel(rectTransform, $"Swatch{i}", Color.white);
				((RectTransform)image.transform).sizeDelta = new Vector2(22f, 22f);
				Button item = image.gameObject.AddComponent<Button>();
				image.gameObject.SetActive(value: false);
				list.Add(image);
				list2.Add(item);
			}
			RecentColorsView recentColorsView = rectTransform.gameObject.AddComponent<RecentColorsView>();
			recentColorsView.swatchImages = list;
			recentColorsView.swatchButtons = list2;
			return recentColorsView;
		}

		private void Awake()
		{
			for (int i = 0; i < swatchButtons.Count; i++)
			{
				int index = i;
				swatchButtons[i].onClick.RemoveAllListeners();
				swatchButtons[i].onClick.AddListener(delegate
				{
					if (index < colors.Count)
					{
						this.SwatchClicked?.Invoke(colors[index]);
					}
				});
			}
		}

		public void Push(Color color)
		{
			colors.RemoveAll((Color c) => c == color);
			colors.Insert(0, color);
			if (colors.Count > 8)
			{
				colors.RemoveRange(8, colors.Count - 8);
			}
			for (int num = 0; num < swatchImages.Count; num++)
			{
				bool flag = num < colors.Count;
				swatchImages[num].gameObject.SetActive(flag);
				if (flag)
				{
					swatchImages[num].color = colors[num];
				}
			}
		}
	}
}
