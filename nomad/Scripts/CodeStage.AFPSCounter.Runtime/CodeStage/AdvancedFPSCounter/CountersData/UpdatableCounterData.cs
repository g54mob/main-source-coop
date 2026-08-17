using System;
using System.Collections;
using UnityEngine;

namespace CodeStage.AdvancedFPSCounter.CountersData
{
	public abstract class UpdatableCounterData : BaseCounterData
	{
		protected Coroutine updateCoroutine;

		[Tooltip("Update interval in seconds.")]
		[Range(0.1f, 10f)]
		[SerializeField]
		protected float updateInterval = 0.5f;

		public float UpdateInterval
		{
			get
			{
				return updateInterval;
			}
			set
			{
				if (!(Math.Abs(updateInterval - value) < 0.001f) && Application.isPlaying)
				{
					updateInterval = value;
				}
			}
		}

		protected override void PerformInitActions()
		{
			base.PerformInitActions();
			StartUpdateCoroutine();
		}

		protected override void PerformDeActivationActions()
		{
			base.PerformDeActivationActions();
			StopUpdateCoroutine();
		}

		protected abstract IEnumerator UpdateCounter();

		private void StartUpdateCoroutine()
		{
			updateCoroutine = main.StartCoroutine(UpdateCounter());
		}

		private void StopUpdateCoroutine()
		{
			main.StopCoroutine(updateCoroutine);
		}
	}
}
