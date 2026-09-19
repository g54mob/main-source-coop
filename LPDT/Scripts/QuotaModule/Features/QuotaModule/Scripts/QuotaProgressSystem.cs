using System;
using System.Collections.Generic;
using Features.ItemsModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using UnityEngine;
using Zenject;

namespace Features.QuotaModule.Scripts
{
	public class QuotaProgressSystem : IInitializable, IDisposable
	{
		private readonly QuotaContainerModel _quotaContainerModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly QuotaSynchronizedModel _quotaSynchronizedModel;

		private readonly QuotaConfiguration _quotaConfiguration;

		private readonly HashSet<object> _countedClusters = new HashSet<object>();

		public QuotaProgressSystem(QuotaContainerModel quotaContainerModel, MultiplayerModel multiplayerModel, QuotaSynchronizedModel quotaSynchronizedModel, QuotaConfiguration quotaConfiguration)
		{
			_quotaContainerModel = quotaContainerModel;
			_multiplayerModel = multiplayerModel;
			_quotaSynchronizedModel = quotaSynchronizedModel;
			_quotaConfiguration = quotaConfiguration;
		}

		public void Initialize()
		{
			_quotaContainerModel.OnFreeContainerItemsUpdated += CalculateQuotaProgress;
			_quotaSynchronizedModel.AttachmentChanged += OnQuotaAttachmentChanged;
		}

		public void Dispose()
		{
			_quotaContainerModel.OnFreeContainerItemsUpdated -= CalculateQuotaProgress;
			_quotaSynchronizedModel.AttachmentChanged -= OnQuotaAttachmentChanged;
		}

		private void OnQuotaAttachmentChanged(bool isAttached)
		{
			if (isAttached && _multiplayerModel.NetworkRunner.IsSharedModeMasterClient && _quotaSynchronizedModel.IsAttached)
			{
				float num = ComputeQuota();
				if (num > _quotaSynchronizedModel.CurrentQuota.Value && !Mathf.Approximately(num, _quotaSynchronizedModel.CurrentQuota.Value))
				{
					_quotaSynchronizedModel.CurrentQuota.Value = num;
				}
			}
		}

		private void CalculateQuotaProgress()
		{
			if (_multiplayerModel.NetworkRunner.IsSharedModeMasterClient && _quotaSynchronizedModel.IsAttached)
			{
				float num = ComputeQuota();
				if (!Mathf.Approximately(num, _quotaSynchronizedModel.CurrentQuota.Value))
				{
					_quotaSynchronizedModel.CurrentQuota.Value = num;
				}
			}
		}

		private float ComputeQuota()
		{
			float num = 0f;
			_countedClusters.Clear();
			foreach (QuotaContainerItemData freeContainerItem in _quotaContainerModel.FreeContainerItems)
			{
				if ((freeContainerItem.PointGrabable == null || freeContainerItem.PointGrabable.GrabbedByPlayers.Count <= 0) && !(freeContainerItem.Item.NetworkObject == null) && freeContainerItem.Item.NetworkObject.IsValid)
				{
					num += (float)DisplayedCurrencyResolver.Resolve(freeContainerItem.Item, _countedClusters);
				}
			}
			return Mathf.Max(0f, num * _quotaConfiguration.CurrencyQuotaCoefficient);
		}
	}
}
