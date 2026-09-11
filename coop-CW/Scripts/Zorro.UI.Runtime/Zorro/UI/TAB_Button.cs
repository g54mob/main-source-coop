using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Zorro.UI
{
	[DefaultExecutionOrder(-100)]
	public class TAB_Button : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		public Graphic background;

		public TextMeshProUGUI text;

		protected bool hoveredOn;

		protected bool selected;

		public bool Selected
		{
			get
			{
				return selected;
			}
			protected set
			{
				selected = value;
			}
		}

		protected virtual void Start()
		{
			GetComponent<Button>().onClick.AddListener(ButtonClicked);
		}

		protected virtual void UpdateSelection()
		{
		}

		public virtual void ButtonClicked()
		{
			base.transform.parent.GetComponent<ITABS>().SelectGeneric(this);
		}

		public void Select()
		{
			Selected = true;
			UpdateSelection();
			ITabAction[] components = GetComponents<ITabAction>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].Select();
			}
		}

		public void Deselect()
		{
			Selected = false;
			UpdateSelection();
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			hoveredOn = true;
			UpdateSelection();
			OnHover();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			hoveredOn = false;
			UpdateSelection();
		}

		public virtual void OnHover()
		{
		}
	}
}
