using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using pworld.Scripts.PPhys;

namespace pworld.Scripts.PUI
{
	public class PUIOnHover : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
	{
		public float hoverScaleMul = 1.1f;

		private RectTransform rectT_g;

		[CanBeNull]
		private PPhysScaleLocal scale_g;

		private bool selected;

		private Vector3 startScale;

		private void Awake()
		{
			scale_g = GetComponent<PPhysScaleLocal>();
			rectT_g = GetComponent<RectTransform>();
			if ((bool)rectT_g)
			{
				startScale = rectT_g.transform.localScale;
			}
			else
			{
				startScale = base.transform.localScale;
			}
		}

		private void Start()
		{
			if ((bool)rectT_g)
			{
				scale_g.Target = rectT_g.localScale;
			}
			else
			{
				scale_g.Target = base.transform.localScale;
			}
		}

		public void OnDeselect(BaseEventData eventData)
		{
			selected = false;
			MakeSmall();
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			MakeBig();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (!selected)
			{
				MakeSmall();
			}
		}

		public void OnSelect(BaseEventData eventData)
		{
			selected = true;
			MakeBig();
		}

		private void MakeBig()
		{
			scale_g.Target = startScale * hoverScaleMul;
		}

		private void MakeSmall()
		{
			scale_g.Target = startScale;
		}
	}
}
