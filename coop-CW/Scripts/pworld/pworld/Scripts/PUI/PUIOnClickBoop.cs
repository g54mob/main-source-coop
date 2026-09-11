using UnityEngine;
using UnityEngine.EventSystems;
using pworld.Scripts.Extensions;
using pworld.Scripts.PPhys;

namespace pworld.Scripts.PUI
{
	public class PUIOnClickBoop : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		public float boopForce = -4f;

		private RectTransform rectT_g;

		private PPhysSpringBase scaler_g;

		private Vector3 startScale;

		private void Awake()
		{
			rectT_g = GetComponent<RectTransform>();
			scaler_g = GetComponent<PPhysSpringBase>();
		}

		private void Start()
		{
			startScale = rectT_g.localScale;
			scaler_g.Target = startScale;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			scaler_g.Velocity += boopForce.ToVec();
		}
	}
}
