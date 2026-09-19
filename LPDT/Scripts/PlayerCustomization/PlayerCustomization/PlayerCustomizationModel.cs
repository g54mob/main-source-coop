using System;
using System.Collections.Generic;
using System.Linq;
using Features.GameCycle.Scripts.SessionCleanup;
using UnityEngine;

namespace PlayerCustomization
{
	[Serializable]
	public class PlayerCustomizationModel : ISessionCleanup
	{
		[SerializeField]
		private List<PlayerCustomizationSlotData> _slots = new List<PlayerCustomizationSlotData>();

		public IReadOnlyList<PlayerCustomizationSlotData> Slots => _slots;

		public event Action OnSlotsChanged;

		public event Action OnClear;

		public void UpsertSlot(int playerId, PlayerCustomizationSlotData slotData)
		{
			int num = _slots.FindIndex((PlayerCustomizationSlotData s) => s.PlayerId == playerId);
			if (num >= 0)
			{
				_slots[num] = slotData;
			}
			else
			{
				_slots.Add(slotData);
			}
			this.OnSlotsChanged?.Invoke();
		}

		public void OverrideSlotData(int playerId, PlayerCustomizationSlotData slotData)
		{
			UpsertSlot(playerId, slotData);
		}

		public void RemoveSlot(int playerId)
		{
			int num = _slots.FindIndex((PlayerCustomizationSlotData s) => s.PlayerId == playerId);
			if (num >= 0)
			{
				_slots.RemoveAt(num);
				this.OnSlotsChanged?.Invoke();
			}
		}

		public void SortPlayers()
		{
			_slots = _slots.OrderBy((PlayerCustomizationSlotData s) => s.PlayerId).ToList();
		}

		public void Cleanup()
		{
			_slots.Clear();
			this.OnClear?.Invoke();
			this.OnSlotsChanged?.Invoke();
		}
	}
}
