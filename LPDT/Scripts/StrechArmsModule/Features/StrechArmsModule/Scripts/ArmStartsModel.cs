using System;
using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;
using Fusion;
using UnityEngine;

namespace Features.StrechArmsModule.Scripts
{
	public class ArmStartsModel : ISessionCleanup
	{
		public Action<PlayerRef, Transform, Arm> OnArmStartAdded;

		public Action<PlayerRef, IArmEndEntity, Arm> OnArmEndAdded;

		public Action<PlayerRef, Transform, Arm> OnArmAdded;

		public Action<PlayerRef, Transform, Arm> OnArmIdlePoseAdded;

		public Action<PlayerRef, Transform, Arm> OnArmStoreIdlePoseAdded;

		public Dictionary<PlayerRef, Transform> ArmStartsTransformsLeft { get; private set; } = new Dictionary<PlayerRef, Transform>();

		public Dictionary<PlayerRef, Transform> ArmStartsTransformsRight { get; private set; } = new Dictionary<PlayerRef, Transform>();

		public Dictionary<PlayerRef, IArmEndEntity> ArmEndsLeft { get; private set; } = new Dictionary<PlayerRef, IArmEndEntity>();

		public Dictionary<PlayerRef, IArmEndEntity> ArmEndsRight { get; private set; } = new Dictionary<PlayerRef, IArmEndEntity>();

		public Dictionary<PlayerRef, Transform> ArmIdlePosesLeft { get; private set; } = new Dictionary<PlayerRef, Transform>();

		public Dictionary<PlayerRef, Transform> ArmIdleStorePosesLeft { get; private set; } = new Dictionary<PlayerRef, Transform>();

		public Dictionary<PlayerRef, Transform> ArmIdlePosesRight { get; private set; } = new Dictionary<PlayerRef, Transform>();

		public Dictionary<PlayerRef, Transform> ArmIdleStorePosesRight { get; private set; } = new Dictionary<PlayerRef, Transform>();

		public Dictionary<PlayerRef, Transform> ArmsTransformsLeft { get; private set; } = new Dictionary<PlayerRef, Transform>();

		public Dictionary<PlayerRef, Transform> ArmsTransformsRight { get; private set; } = new Dictionary<PlayerRef, Transform>();

		public bool IsContainsStart(PlayerRef playerRef, Arm armOrientation)
		{
			return armOrientation switch
			{
				Arm.Left => ArmStartsTransformsLeft.ContainsKey(playerRef), 
				Arm.Right => ArmStartsTransformsRight.ContainsKey(playerRef), 
				_ => false, 
			};
		}

		public bool IsContainsEnd(PlayerRef playerRef, Arm armOrientation)
		{
			return armOrientation switch
			{
				Arm.Left => ArmEndsLeft.ContainsKey(playerRef), 
				Arm.Right => ArmEndsRight.ContainsKey(playerRef), 
				_ => false, 
			};
		}

		public bool IsContainsIdlePose(PlayerRef playerRef, Arm armOrientation)
		{
			return armOrientation switch
			{
				Arm.Left => ArmIdlePosesLeft.ContainsKey(playerRef), 
				Arm.Right => ArmIdlePosesRight.ContainsKey(playerRef), 
				_ => false, 
			};
		}

		public bool IsContainsIdleStorePose(PlayerRef playerRef, Arm armOrientation)
		{
			return armOrientation switch
			{
				Arm.Left => ArmIdleStorePosesLeft.ContainsKey(playerRef), 
				Arm.Right => ArmIdleStorePosesRight.ContainsKey(playerRef), 
				_ => false, 
			};
		}

		public Transform GetStart(PlayerRef playerRef, Arm armOrientation)
		{
			return armOrientation switch
			{
				Arm.Left => ArmStartsTransformsLeft.GetValueOrDefault(playerRef), 
				Arm.Right => ArmStartsTransformsRight.GetValueOrDefault(playerRef), 
				_ => null, 
			};
		}

		public IArmEndEntity GetEnd(PlayerRef playerRef, Arm armOrientation)
		{
			return armOrientation switch
			{
				Arm.Left => ArmEndsLeft.GetValueOrDefault(playerRef), 
				Arm.Right => ArmEndsRight.GetValueOrDefault(playerRef), 
				_ => null, 
			};
		}

		public Transform GetArm(PlayerRef playerRef, Arm armOrientation)
		{
			return armOrientation switch
			{
				Arm.Left => ArmsTransformsLeft.GetValueOrDefault(playerRef), 
				Arm.Right => ArmsTransformsRight.GetValueOrDefault(playerRef), 
				_ => null, 
			};
		}

		public Transform GetIdlePose(PlayerRef playerRef, Arm armOrientation)
		{
			return armOrientation switch
			{
				Arm.Left => ArmIdlePosesLeft.GetValueOrDefault(playerRef), 
				Arm.Right => ArmIdlePosesRight.GetValueOrDefault(playerRef), 
				_ => null, 
			};
		}

		public Transform GetIdleStorePose(PlayerRef playerRef, Arm armOrientation)
		{
			return armOrientation switch
			{
				Arm.Left => ArmIdleStorePosesLeft.GetValueOrDefault(playerRef), 
				Arm.Right => ArmIdleStorePosesRight.GetValueOrDefault(playerRef), 
				_ => null, 
			};
		}

		public void AddArmStart(PlayerRef playerRef, Transform networkTransform, Arm armOrientation)
		{
			switch (armOrientation)
			{
			case Arm.Left:
				if (!ArmStartsTransformsLeft.TryAdd(playerRef, networkTransform))
				{
					return;
				}
				break;
			case Arm.Right:
				if (!ArmStartsTransformsRight.TryAdd(playerRef, networkTransform))
				{
					return;
				}
				break;
			default:
				throw new ArgumentOutOfRangeException("armOrientation", armOrientation, null);
			}
			OnArmStartAdded?.Invoke(playerRef, networkTransform, armOrientation);
		}

		public void AddArmEnd(PlayerRef playerRef, IArmEndEntity armEndEntity, Arm armOrientation)
		{
			switch (armOrientation)
			{
			case Arm.Left:
				if (!ArmEndsLeft.TryAdd(playerRef, armEndEntity))
				{
					return;
				}
				break;
			case Arm.Right:
				if (!ArmEndsRight.TryAdd(playerRef, armEndEntity))
				{
					return;
				}
				break;
			default:
				throw new ArgumentOutOfRangeException("armOrientation", armOrientation, null);
			}
			OnArmEndAdded?.Invoke(playerRef, armEndEntity, armOrientation);
		}

		public void AddIdlePose(PlayerRef playerRef, Transform transform, Arm armOrientation)
		{
			switch (armOrientation)
			{
			case Arm.Left:
				if (!ArmIdlePosesLeft.TryAdd(playerRef, transform))
				{
					return;
				}
				break;
			case Arm.Right:
				if (!ArmIdlePosesRight.TryAdd(playerRef, transform))
				{
					return;
				}
				break;
			default:
				throw new ArgumentOutOfRangeException("armOrientation", armOrientation, null);
			}
			OnArmIdlePoseAdded?.Invoke(playerRef, transform, armOrientation);
		}

		public void AddArm(PlayerRef playerRef, Transform transform, Arm armOrientation)
		{
			switch (armOrientation)
			{
			case Arm.Left:
				if (!ArmsTransformsLeft.TryAdd(playerRef, transform))
				{
					return;
				}
				break;
			case Arm.Right:
				if (!ArmsTransformsRight.TryAdd(playerRef, transform))
				{
					return;
				}
				break;
			default:
				throw new ArgumentOutOfRangeException("armOrientation", armOrientation, null);
			}
			OnArmAdded?.Invoke(playerRef, transform, armOrientation);
		}

		public void AddIdleStorePose(PlayerRef playerRef, Transform transform, Arm armOrientation)
		{
			switch (armOrientation)
			{
			case Arm.Left:
				if (!ArmIdleStorePosesLeft.TryAdd(playerRef, transform))
				{
					return;
				}
				break;
			case Arm.Right:
				if (!ArmIdleStorePosesRight.TryAdd(playerRef, transform))
				{
					return;
				}
				break;
			default:
				throw new ArgumentOutOfRangeException("armOrientation", armOrientation, null);
			}
			OnArmStoreIdlePoseAdded?.Invoke(playerRef, transform, armOrientation);
		}

		public void RemoveForPlayer(PlayerRef playerRef, Arm armOrientation)
		{
			switch (armOrientation)
			{
			case Arm.Left:
				ArmStartsTransformsLeft.Remove(playerRef);
				ArmEndsLeft.Remove(playerRef);
				ArmIdlePosesLeft.Remove(playerRef);
				ArmIdleStorePosesLeft.Remove(playerRef);
				ArmsTransformsLeft.Remove(playerRef);
				break;
			case Arm.Right:
				ArmStartsTransformsRight.Remove(playerRef);
				ArmEndsRight.Remove(playerRef);
				ArmIdlePosesRight.Remove(playerRef);
				ArmIdleStorePosesRight.Remove(playerRef);
				ArmsTransformsRight.Remove(playerRef);
				break;
			default:
				throw new ArgumentOutOfRangeException("armOrientation", armOrientation, null);
			}
		}

		public void Cleanup()
		{
			ArmStartsTransformsLeft.Clear();
			ArmStartsTransformsRight.Clear();
			ArmEndsLeft.Clear();
			ArmEndsRight.Clear();
			ArmIdlePosesLeft.Clear();
			ArmIdleStorePosesLeft.Clear();
			ArmIdlePosesRight.Clear();
			ArmIdleStorePosesRight.Clear();
			ArmsTransformsLeft.Clear();
			ArmsTransformsRight.Clear();
		}
	}
}
