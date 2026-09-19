using System;
using Features.KrakenModule.Scripts.Core;
using Features.KrakenModule.Scripts.Data;
using Features.QuotaModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.KrakenModule.Scripts.Systems
{
	public class KrakenSessionAppearSystem : IInitializable, IDisposable
	{
		private readonly QuotaSynchronizedModel _quotaSynchronizedModel;

		private readonly KrakenRuntimeModel _runtimeModel;

		private readonly KrakenAppearTriggerWindowModel _triggerWindowModel;

		private readonly KrakenBehaviourConfiguration _configuration;

		public KrakenSessionAppearSystem(QuotaSynchronizedModel quotaSynchronizedModel, KrakenRuntimeModel runtimeModel, KrakenAppearTriggerWindowModel triggerWindowModel, KrakenBehaviourConfiguration configuration)
		{
			_quotaSynchronizedModel = quotaSynchronizedModel;
			_runtimeModel = runtimeModel;
			_triggerWindowModel = triggerWindowModel;
			_configuration = configuration;
		}

		public void Initialize()
		{
			_quotaSynchronizedModel.OnQuotaChanged += OnQuotaChanged;
		}

		public void Dispose()
		{
			_quotaSynchronizedModel.OnQuotaChanged -= OnQuotaChanged;
		}

		private void OnQuotaChanged(float currentQuota, float maxQuota)
		{
			if (!(maxQuota <= 0f))
			{
				float progress = Mathf.Clamp01(currentQuota / maxQuota);
				TryRequestQuotaAppear(progress, _configuration.HalfQuotaAppearThreshold, KrakenAppearReason.HalfQuota);
				TryRequestQuotaAppear(progress, _configuration.FullQuotaAppearThreshold, KrakenAppearReason.FullQuota);
			}
		}

		private void TryRequestQuotaAppear(float progress, float threshold, KrakenAppearReason reason)
		{
			if (!(progress < threshold) && _triggerWindowModel.TryMarkQuotaThresholdTriggered(threshold))
			{
				RequestAppear(reason);
			}
		}

		private void RequestAppear(KrakenAppearReason reason)
		{
			if (_runtimeModel.TryGetController(out var controller))
			{
				controller.RequestAppear(reason);
			}
		}
	}
}
