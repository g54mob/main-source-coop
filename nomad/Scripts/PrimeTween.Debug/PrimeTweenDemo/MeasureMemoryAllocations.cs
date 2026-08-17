using System.Collections.Generic;
using UnityEngine;

namespace PrimeTweenDemo
{
	public class MeasureMemoryAllocations : MonoBehaviour
	{
		[SerializeField]
		private bool logAllocations;

		[SerializeField]
		private bool logFiltered;

		[SerializeField]
		private bool logIgnored;

		[SerializeField]
		private List<string> filterAllocations = new List<string>();

		[SerializeField]
		private List<string> ignoreAllocations = new List<string>();

		private void Awake()
		{
			if (Application.isEditor)
			{
				Debug.LogWarning("MeasureMemoryAllocations is only supported in Unity 2019.1 or newer.", this);
			}
			else
			{
				base.gameObject.SetActive(value: false);
			}
		}
	}
}
