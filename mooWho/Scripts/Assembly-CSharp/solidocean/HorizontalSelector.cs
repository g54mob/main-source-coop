using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace solidocean
{
	public class HorizontalSelector : MonoBehaviour
	{
		private TextMeshProUGUI text;

		private int m_Index;

		public int defalutValueIndex;

		public List<string> data = new List<string>();

		public int index
		{
			get
			{
				return m_Index;
			}
			set
			{
				m_Index = value;
				text.text = data[m_Index];
			}
		}

		public string value => data[m_Index];

		private void Start()
		{
			text = base.transform.Find("txt_text").GetComponent<TextMeshProUGUI>();
			base.transform.Find("btn_left").GetComponent<Button>().onClick.AddListener(OnLeftClicked);
			base.transform.Find("btn_right").GetComponent<Button>().onClick.AddListener(OnRightClicked);
			index = defalutValueIndex;
		}

		private void OnLeftClicked()
		{
			if (index == 0)
			{
				index = data.Count - 1;
			}
			else
			{
				index--;
			}
		}

		private void OnRightClicked()
		{
			if (index + 1 >= data.Count)
			{
				index = 0;
			}
			else
			{
				index++;
			}
		}
	}
}
