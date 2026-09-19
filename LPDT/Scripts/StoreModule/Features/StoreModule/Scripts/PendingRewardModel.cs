using System.Collections.Generic;
using Features.NetworkedModelCodegen.Scripts;
using UnityEngine;

namespace Features.StoreModule.Scripts
{
	[NetworkedModel(ModelScope.Run, ModelOwnership.Shared)]
	public sealed class PendingRewardModel : NetworkedModelBase
	{
		public const int SLOT_COUNT = 32;

		private Networked<int>[] _cardNetId;

		private Networked<int>[] _cardDataId;

		private Networked<int>[] _colorPacked;

		private Networked<int>[] _targetPlayerId;

		public Networked<int> TargetLevelNumber { get; } = new Networked<int>();

		public Networked<int> Slot0CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot0CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot0ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot0TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot1CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot1CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot1ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot1TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot2CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot2CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot2ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot2TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot3CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot3CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot3ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot3TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot4CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot4CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot4ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot4TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot5CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot5CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot5ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot5TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot6CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot6CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot6ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot6TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot7CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot7CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot7ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot7TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot8CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot8CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot8ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot8TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot9CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot9CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot9ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot9TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot10CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot10CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot10ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot10TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot11CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot11CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot11ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot11TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot12CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot12CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot12ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot12TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot13CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot13CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot13ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot13TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot14CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot14CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot14ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot14TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot15CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot15CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot15ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot15TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot16CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot16CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot16ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot16TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot17CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot17CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot17ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot17TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot18CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot18CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot18ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot18TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot19CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot19CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot19ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot19TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot20CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot20CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot20ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot20TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot21CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot21CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot21ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot21TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot22CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot22CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot22ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot22TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot23CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot23CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot23ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot23TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot24CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot24CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot24ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot24TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot25CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot25CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot25ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot25TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot26CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot26CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot26ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot26TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot27CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot27CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot27ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot27TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot28CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot28CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot28ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot28TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot29CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot29CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot29ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot29TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot30CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot30CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot30ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot30TargetPlayerId { get; } = new Networked<int>();

		public Networked<int> Slot31CardNetId { get; } = new Networked<int>();

		public Networked<int> Slot31CardDataId { get; } = new Networked<int>();

		public Networked<int> Slot31ColorPacked { get; } = new Networked<int>();

		public Networked<int> Slot31TargetPlayerId { get; } = new Networked<int>();

		public PendingRewardModel()
		{
			_cardNetId = new Networked<int>[32]
			{
				Slot0CardNetId, Slot1CardNetId, Slot2CardNetId, Slot3CardNetId, Slot4CardNetId, Slot5CardNetId, Slot6CardNetId, Slot7CardNetId, Slot8CardNetId, Slot9CardNetId,
				Slot10CardNetId, Slot11CardNetId, Slot12CardNetId, Slot13CardNetId, Slot14CardNetId, Slot15CardNetId, Slot16CardNetId, Slot17CardNetId, Slot18CardNetId, Slot19CardNetId,
				Slot20CardNetId, Slot21CardNetId, Slot22CardNetId, Slot23CardNetId, Slot24CardNetId, Slot25CardNetId, Slot26CardNetId, Slot27CardNetId, Slot28CardNetId, Slot29CardNetId,
				Slot30CardNetId, Slot31CardNetId
			};
			_cardDataId = new Networked<int>[32]
			{
				Slot0CardDataId, Slot1CardDataId, Slot2CardDataId, Slot3CardDataId, Slot4CardDataId, Slot5CardDataId, Slot6CardDataId, Slot7CardDataId, Slot8CardDataId, Slot9CardDataId,
				Slot10CardDataId, Slot11CardDataId, Slot12CardDataId, Slot13CardDataId, Slot14CardDataId, Slot15CardDataId, Slot16CardDataId, Slot17CardDataId, Slot18CardDataId, Slot19CardDataId,
				Slot20CardDataId, Slot21CardDataId, Slot22CardDataId, Slot23CardDataId, Slot24CardDataId, Slot25CardDataId, Slot26CardDataId, Slot27CardDataId, Slot28CardDataId, Slot29CardDataId,
				Slot30CardDataId, Slot31CardDataId
			};
			_colorPacked = new Networked<int>[32]
			{
				Slot0ColorPacked, Slot1ColorPacked, Slot2ColorPacked, Slot3ColorPacked, Slot4ColorPacked, Slot5ColorPacked, Slot6ColorPacked, Slot7ColorPacked, Slot8ColorPacked, Slot9ColorPacked,
				Slot10ColorPacked, Slot11ColorPacked, Slot12ColorPacked, Slot13ColorPacked, Slot14ColorPacked, Slot15ColorPacked, Slot16ColorPacked, Slot17ColorPacked, Slot18ColorPacked, Slot19ColorPacked,
				Slot20ColorPacked, Slot21ColorPacked, Slot22ColorPacked, Slot23ColorPacked, Slot24ColorPacked, Slot25ColorPacked, Slot26ColorPacked, Slot27ColorPacked, Slot28ColorPacked, Slot29ColorPacked,
				Slot30ColorPacked, Slot31ColorPacked
			};
			_targetPlayerId = new Networked<int>[32]
			{
				Slot0TargetPlayerId, Slot1TargetPlayerId, Slot2TargetPlayerId, Slot3TargetPlayerId, Slot4TargetPlayerId, Slot5TargetPlayerId, Slot6TargetPlayerId, Slot7TargetPlayerId, Slot8TargetPlayerId, Slot9TargetPlayerId,
				Slot10TargetPlayerId, Slot11TargetPlayerId, Slot12TargetPlayerId, Slot13TargetPlayerId, Slot14TargetPlayerId, Slot15TargetPlayerId, Slot16TargetPlayerId, Slot17TargetPlayerId, Slot18TargetPlayerId, Slot19TargetPlayerId,
				Slot20TargetPlayerId, Slot21TargetPlayerId, Slot22TargetPlayerId, Slot23TargetPlayerId, Slot24TargetPlayerId, Slot25TargetPlayerId, Slot26TargetPlayerId, Slot27TargetPlayerId, Slot28TargetPlayerId, Slot29TargetPlayerId,
				Slot30TargetPlayerId, Slot31TargetPlayerId
			};
		}

		public void Define(int cardNetId, int cardDataId, int colorPacked, int targetPlayerId, int targetLevelNumber)
		{
			if (base.IsAuthority && cardNetId != 0)
			{
				TargetLevelNumber.Value = targetLevelNumber;
				int num = FindSlot(cardNetId);
				if (num < 0)
				{
					num = FindFreeSlot();
				}
				if (num < 0)
				{
					Debug.LogWarning($"[PendingRewardModel] full (SLOT_COUNT {32}) — reward for player {targetPlayerId} on level {targetLevelNumber} dropped.");
					return;
				}
				_cardNetId[num].Value = cardNetId;
				_cardDataId[num].Value = cardDataId;
				_colorPacked[num].Value = colorPacked;
				_targetPlayerId[num].Value = targetPlayerId;
			}
		}

		public void Remove(int cardNetId)
		{
			if (base.IsAuthority)
			{
				int num = FindSlot(cardNetId);
				if (num >= 0)
				{
					ClearSlot(num);
				}
			}
		}

		public void Clear()
		{
			if (base.IsAuthority)
			{
				for (int i = 0; i < 32; i++)
				{
					ClearSlot(i);
				}
				TargetLevelNumber.Value = 0;
			}
		}

		public int GetTargetLevelNumber()
		{
			return TargetLevelNumber.Value;
		}

		public IEnumerable<PendingRewardEntry> Enumerate()
		{
			for (int i = 0; i < 32; i++)
			{
				if (_cardNetId[i].Value != 0)
				{
					yield return new PendingRewardEntry(_cardNetId[i].Value, _cardDataId[i].Value, _colorPacked[i].Value, _targetPlayerId[i].Value);
				}
			}
		}

		private void ClearSlot(int slot)
		{
			_cardNetId[slot].Value = 0;
			_cardDataId[slot].Value = 0;
			_colorPacked[slot].Value = 0;
			_targetPlayerId[slot].Value = 0;
		}

		private int FindSlot(int cardNetId)
		{
			for (int i = 0; i < 32; i++)
			{
				if (_cardNetId[i].Value == cardNetId)
				{
					return i;
				}
			}
			return -1;
		}

		private int FindFreeSlot()
		{
			for (int i = 0; i < 32; i++)
			{
				if (_cardNetId[i].Value == 0)
				{
					return i;
				}
			}
			return -1;
		}
	}
}
