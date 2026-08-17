using UnityEngine;
using UnityEngine.UI;

namespace CW.Common
{
	[HelpURL("https://carloswilkes.com/Documentation/Common#CwDemoButtonBuilder")]
	[AddComponentMenu("Common/CW Demo Button Builder")]
	public class CwDemoButtonBuilder : MonoBehaviour
	{
		[SerializeField]
		private GameObject buttonPrefab;

		[SerializeField]
		private RectTransform buttonRoot;

		[SerializeField]
		private Sprite icon;

		[SerializeField]
		private Color color = Color.white;

		[SerializeField]
		[Multiline(3)]
		private string overrideName;

		[SerializeField]
		private GameObject clone;

		public GameObject ButtonPrefab
		{
			get
			{
				return buttonPrefab;
			}
			set
			{
				buttonPrefab = value;
			}
		}

		public RectTransform ButtonRoot
		{
			get
			{
				return buttonRoot;
			}
			set
			{
				buttonRoot = value;
			}
		}

		public Sprite Icon
		{
			get
			{
				return icon;
			}
			set
			{
				icon = value;
			}
		}

		public Color Color
		{
			get
			{
				return color;
			}
			set
			{
				color = value;
			}
		}

		public string OverrideName
		{
			get
			{
				return overrideName;
			}
			set
			{
				overrideName = value;
			}
		}

		[ContextMenu("Build")]
		public void Build()
		{
			if (clone != null)
			{
				Object.DestroyImmediate(clone);
			}
			if (buttonPrefab != null)
			{
				clone = DoInstantiate();
				clone.name = base.name;
				Image component = clone.GetComponent<Image>();
				if (component != null)
				{
					component.sprite = icon;
					component.color = color;
				}
				Text componentInChildren = clone.GetComponentInChildren<Text>();
				if (componentInChildren != null)
				{
					componentInChildren.text = ((!string.IsNullOrEmpty(overrideName)) ? overrideName : base.name);
				}
				CwDemoButton component2 = clone.GetComponent<CwDemoButton>();
				if (component2 != null)
				{
					component2.IsolateTarget = base.transform;
				}
			}
		}

		[ContextMenu("Build All")]
		public void BuildAll()
		{
			CwDemoButtonBuilder[] componentsInChildren = base.transform.parent.GetComponentsInChildren<CwDemoButtonBuilder>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Build();
			}
		}

		private GameObject DoInstantiate()
		{
			return Object.Instantiate(buttonPrefab, buttonRoot, worldPositionStays: false);
		}
	}
}
