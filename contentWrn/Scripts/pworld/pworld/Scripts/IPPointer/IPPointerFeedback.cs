using UnityEngine;
using UnityEngine.EventSystems;
using pworld.Scripts.Extensions;
using pworld.Scripts.PPhys;

namespace pworld.Scripts.IPPointer
{
	public class IPPointerFeedback : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler
	{
		public float clickF = -4f;

		public float enterS = 1.5f;

		public float downS = 1.4f;

		private PPhysSpringBase spring;

		private Vector3 startSTarget;

		private void Awake()
		{
			spring = GetComponent<PPhysSpringBase>();
		}

		private void Start()
		{
			startSTarget = spring.Target;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			spring.Velocity += clickF.ToVec();
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			spring.Target = startSTarget * downS;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			spring.Target = startSTarget * enterS;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			spring.Target = startSTarget;
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			spring.Target = startSTarget;
		}
	}
}
