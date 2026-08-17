using HSVPicker;
using UnityEngine;

namespace HSVPickerExamples
{
	public class ColorPickerTester : MonoBehaviour
	{
		public Renderer renderer;

		public ColorPicker picker;

		public Color Color = Color.red;

		public bool SetColorOnStart;

		private void Start()
		{
			picker.onValueChanged.AddListener(delegate(Color color)
			{
				renderer.material.color = color;
				Color = color;
			});
			renderer.material.color = picker.CurrentColor;
			if (SetColorOnStart)
			{
				picker.CurrentColor = Color;
			}
		}

		private void Update()
		{
		}
	}
}
