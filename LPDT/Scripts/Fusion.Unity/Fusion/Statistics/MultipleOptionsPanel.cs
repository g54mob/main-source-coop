using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Fusion.Statistics
{
	public class MultipleOptionsPanel : MonoBehaviour
	{
		public const int MAX_BUTTONS_COUNT = 10;

		[SerializeField]
		private Text _label;

		[SerializeField]
		private Transform _content;

		[SerializeField]
		private InputField _searchInput;

		public void Setup<T>(string label, T[] options, Func<T, string> defineButtonText, Action<T> buttonAction)
		{
			Button buttonPrototype = _content.GetChild(0).GetComponent<Button>();
			buttonPrototype.gameObject.SetActive(value: false);
			_label.text = label;
			_searchInput.onValueChanged.RemoveAllListeners();
			_searchInput.onValueChanged.AddListener(delegate
			{
				UpdateDisplay();
			});
			UpdateDisplay();
			void UpdateDisplay()
			{
				for (int i = 1; i < _content.childCount; i++)
				{
					UnityEngine.Object.Destroy(_content.GetChild(i).gameObject);
				}
				string searchText = _searchInput.text.ToLower();
				List<T> list = options.Where((T arg) => string.IsNullOrEmpty(searchText) || defineButtonText(arg).ToLower().Contains(searchText)).ToList();
				for (int num = 0; num < 10 && num < list.Count; num++)
				{
					Button button = UnityEngine.Object.Instantiate(buttonPrototype, _content);
					T option = list[num];
					button.GetComponentInChildren<Text>().text = defineButtonText(option);
					button.onClick.RemoveAllListeners();
					button.onClick.AddListener(delegate
					{
						buttonAction(option);
					});
					button.onClick.AddListener(delegate
					{
						UnityEngine.Object.Destroy(base.gameObject);
					});
					button.gameObject.SetActive(value: true);
				}
			}
		}

		public void Close()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
