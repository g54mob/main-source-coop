using System;
using NomadDrive.Features.Locking;
using UnityEngine;

namespace NomadDrive.Features.Objectives
{
	[Serializable]
	public class ChestUnlockedTrigger : ObjectiveTrigger
	{
		[Tooltip("Eger isaretliyse sadece secilen chest tipinin unlock'i sayilir. Bos birakilirsa her tip sayilir.")]
		public bool filterByType;

		[Tooltip("filterByType isaretliyse sadece bu tip chest unlock'lari fire eder.")]
		public ChestType requiredType;

		protected override void OnActivate()
		{
			Lock.OnAnyChestUnlocked += HandleUnlocked;
		}

		protected override void OnDeactivate()
		{
			Lock.OnAnyChestUnlocked -= HandleUnlocked;
		}

		private void HandleUnlocked(Lock lockInstance)
		{
			if (!(lockInstance == null) && (!filterByType || lockInstance.LockType == requiredType))
			{
				FireWithSource(lockInstance.netId.ToString());
			}
		}
	}
}
