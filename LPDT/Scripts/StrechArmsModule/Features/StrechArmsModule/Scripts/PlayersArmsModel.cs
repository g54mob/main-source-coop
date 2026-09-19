using System;
using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;
using Features.SynchronizedModelsModule.Scripts.DataStreamingSynchronizer;
using Fusion;
using UnityEngine;

namespace Features.StrechArmsModule.Scripts
{
	public class PlayersArmsModel : DataStreamSynchronizableBaseWithCustomData<PlayersArmsModel, ArmData>, ISessionCleanup
	{
		private readonly Dictionary<int, Transform> _playersObiArms = new Dictionary<int, Transform>();

		[field: SerializeField]
		public SerializableDictionary<int, List<Arm>> AvailableArms { get; private set; } = new SerializableDictionary<int, List<Arm>>();

		public IReadOnlyDictionary<int, Transform> PlayerObiArms => _playersObiArms;

		public override bool IsNeedToSynchronizeOnSpawn => true;

		public event Action<int, List<Arm>> OnPlayerArmsUpdated;

		public event Action<int, Transform> OnPlayerObiArmAdded;

		public void AddPlayerArm(int playerId, Arm arm)
		{
			if (!AvailableArms.TryAdd(playerId, new List<Arm> { arm }))
			{
				AvailableArms[playerId].Add(arm);
			}
			this.OnPlayerArmsUpdated?.Invoke(playerId, AvailableArms[playerId]);
			base.Data1 = new ArmData
			{
				PlayerId = playerId,
				AvailableArms = AvailableArms[playerId]
			};
			CustomSynchronize();
		}

		public void RemovePlayerArm(int playerId, Arm arm)
		{
			if (AvailableArms.ContainsKey(playerId))
			{
				AvailableArms[playerId].Remove(arm);
				this.OnPlayerArmsUpdated?.Invoke(playerId, AvailableArms[playerId]);
				base.Data1 = new ArmData
				{
					PlayerId = playerId,
					AvailableArms = AvailableArms[playerId]
				};
				CustomSynchronize();
			}
		}

		public void AddPlayerObiArm(int playerId, Transform obiArm)
		{
			_playersObiArms.Add(playerId, obiArm);
			this.OnPlayerObiArmAdded?.Invoke(playerId, obiArm);
		}

		public void RemovePlayerObiArm(int playerId)
		{
			_playersObiArms.Remove(playerId);
		}

		protected override void SetNewValues(PlayersArmsModel model)
		{
			AvailableArms.Clear();
			foreach (var (num2, value) in model.AvailableArms)
			{
				AvailableArms[num2] = value;
				this.OnPlayerArmsUpdated?.Invoke(num2, AvailableArms[num2]);
			}
		}

		protected override void OnSetNewCustomValues(ArmData data)
		{
			AvailableArms[data.PlayerId] = data.AvailableArms;
			this.OnPlayerArmsUpdated?.Invoke(data.PlayerId, data.AvailableArms);
		}

		public void Cleanup()
		{
			AvailableArms.Clear();
		}
	}
}
