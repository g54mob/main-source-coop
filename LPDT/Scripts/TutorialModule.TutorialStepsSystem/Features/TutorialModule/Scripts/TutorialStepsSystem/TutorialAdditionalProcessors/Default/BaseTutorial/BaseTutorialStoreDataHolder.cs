using System;
using System.Collections.Generic;
using Features.StoreModule.Scripts;
using Features.TutorialModule.Scripts.GuideModule;
using UnityEngine;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial
{
	public class BaseTutorialStoreDataHolder
	{
		private readonly Dictionary<TipType, Transform> _tipHolders = new Dictionary<TipType, Transform>();

		public IReadOnlyDictionary<TipType, Transform> TipHolders => _tipHolders;

		public StoreCardBehaviour CurrentProcessedCard { get; set; }

		public event Action<TipType, Transform> OnTipHolderAdded;

		public void AddTipHolder(TipType tipType, Transform transform)
		{
			_tipHolders.Add(tipType, transform);
			this.OnTipHolderAdded?.Invoke(tipType, transform);
		}

		public void RemoveTipHolder(TipType tipType)
		{
			_tipHolders.Remove(tipType);
		}
	}
}
