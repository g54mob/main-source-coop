using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using pworld.Scripts.Extensions;

namespace pworld.Scripts.IPPointer
{
	public class PCameraPointer : MonoBehaviour
	{
		private readonly HashSet<IPPointerHandler> currentPointer = new HashSet<IPPointerHandler>();

		private readonly HashSet<IPPointerHandler> downPointer = new HashSet<IPPointerHandler>();

		private HashSet<IPPointerHandler> lastPointer = new HashSet<IPPointerHandler>();

		private void Awake()
		{
		}

		private void Start()
		{
		}

		private void Update()
		{
			if (PExt.GetMouseRaycastAll(out var hit, -1))
			{
				RaycastHit[] array = hit;
				foreach (RaycastHit hit2 in array)
				{
					HandleHit(hit2);
				}
				CheckUp();
			}
		}

		private void LateUpdate()
		{
			CheckExit();
		}

		private void HandleHit(RaycastHit hit)
		{
			IPPointerHandler[] components = hit.transform.gameObject.GetComponents<IPPointerHandler>();
			foreach (IPPointerHandler iPPointerHandler in components)
			{
				if (iPPointerHandler == null)
				{
					break;
				}
				currentPointer.Add(iPPointerHandler);
				CheckEnter(iPPointerHandler);
				CheckDown(iPPointerHandler);
				CheckClick(iPPointerHandler);
			}
		}

		private void CheckUp()
		{
			if (Input.GetKeyUp(KeyCode.Mouse0))
			{
				if (downPointer.Count > 0)
				{
					downPointer.First().OnPPointerUp();
				}
				downPointer.Clear();
			}
		}

		private void CheckEnter(IPPointerHandler pointerHandler)
		{
			if (!lastPointer.Contains(pointerHandler))
			{
				pointerHandler.OnPPointerEnter();
			}
		}

		private void CheckExit()
		{
			foreach (IPPointerHandler item in lastPointer.Where((IPPointerHandler _item) => !currentPointer.Contains(_item)))
			{
				item.OnPPointerExit();
			}
			lastPointer = new HashSet<IPPointerHandler>(currentPointer);
			currentPointer.Clear();
		}

		private void CheckClick(IPPointerHandler pointH)
		{
			if (Input.GetKeyUp(KeyCode.Mouse0) && downPointer.Contains(pointH))
			{
				pointH.OnPPointerClick();
			}
		}

		private void CheckDown(IPPointerHandler pointerHandler)
		{
			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				downPointer.Add(pointerHandler);
				pointerHandler.OnPPointerDown();
			}
		}
	}
}
