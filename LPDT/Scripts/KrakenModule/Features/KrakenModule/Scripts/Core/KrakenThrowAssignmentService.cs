using Features.GrabModule.Scripts;
using Features.HeadwearModule.Scripts;
using Features.KrakenModule.Scripts.Data;
using UnityEngine;

namespace Features.KrakenModule.Scripts.Core
{
	public class KrakenThrowAssignmentService : IKrakenThrowAssignmentService
	{
		public bool IsGrabbableValid(IPointGrabable pointGrabable)
		{
			if (!IsPointGrabableAlive(pointGrabable))
			{
				return false;
			}
			if (pointGrabable.GameObject == null)
			{
				return false;
			}
			if (IsWornHeadwear(pointGrabable))
			{
				return false;
			}
			if (pointGrabable.GrabbedByPlayersCount > 0 || pointGrabable.InCart)
			{
				return false;
			}
			return !pointGrabable.GrabBlocked;
		}

		public static bool IsWornHeadwear(IPointGrabable pointGrabable)
		{
			if (pointGrabable?.GameObject == null)
			{
				return false;
			}
			Headwear componentInParent = pointGrabable.GameObject.GetComponentInParent<Headwear>(includeInactive: true);
			if (componentInParent != null)
			{
				return componentInParent.IsWorn;
			}
			return false;
		}

		public bool IsItemAlreadyAssigned(TentacleController[] tentacles, IPointGrabable pointGrabable)
		{
			if (tentacles == null || !IsPointGrabableAlive(pointGrabable))
			{
				return false;
			}
			foreach (TentacleController tentacleController in tentacles)
			{
				IPointGrabable pointGrabable2 = ((tentacleController != null) ? tentacleController.AssignedItem : null);
				if (IsPointGrabableAlive(pointGrabable2) && IsSameOrConnectedGrabable(pointGrabable, pointGrabable2))
				{
					return true;
				}
			}
			return false;
		}

		public bool TryFindTentacle(TentacleController[] tentacles, IPointGrabable item, Vector3 fromPosition, KrakenTentacleAssignmentStrategy strategy, out TentacleController tentacle)
		{
			tentacle = ((strategy == KrakenTentacleAssignmentStrategy.RandomFree) ? FindRandomFreeTentacle(tentacles) : FindNearestFreeTentacle(tentacles, fromPosition));
			if (tentacle != null)
			{
				return IsPointGrabableAlive(item);
			}
			return false;
		}

		private TentacleController FindNearestFreeTentacle(TentacleController[] tentacles, Vector3 fromPosition)
		{
			TentacleController result = null;
			float num = float.MaxValue;
			if (tentacles == null)
			{
				return null;
			}
			foreach (TentacleController tentacleController in tentacles)
			{
				if (!(tentacleController == null) && !tentacleController.IsBusy)
				{
					float num2 = Vector3.Distance(tentacleController.transform.position, fromPosition);
					if (!(num2 >= num))
					{
						num = num2;
						result = tentacleController;
					}
				}
			}
			return result;
		}

		private TentacleController FindRandomFreeTentacle(TentacleController[] tentacles)
		{
			if (tentacles == null)
			{
				return null;
			}
			int num = 0;
			TentacleController[] array = tentacles;
			foreach (TentacleController tentacleController in array)
			{
				if (tentacleController != null && !tentacleController.IsBusy)
				{
					num++;
				}
			}
			if (num == 0)
			{
				return null;
			}
			int num2 = Random.Range(0, num);
			int num3 = 0;
			array = tentacles;
			foreach (TentacleController tentacleController2 in array)
			{
				if (!(tentacleController2 == null) && !tentacleController2.IsBusy)
				{
					if (num3 == num2)
					{
						return tentacleController2;
					}
					num3++;
				}
			}
			return null;
		}

		private static bool IsSameOrConnectedGrabable(IPointGrabable pointGrabable, IPointGrabable assignedItem)
		{
			if (!IsPointGrabableAlive(pointGrabable) || !IsPointGrabableAlive(assignedItem))
			{
				return false;
			}
			if (pointGrabable == assignedItem)
			{
				return true;
			}
			if (pointGrabable is SimplePointGrabable simplePointGrabable)
			{
				foreach (SimplePointGrabable connectedGrabable in simplePointGrabable.ConnectedGrabables)
				{
					if (connectedGrabable == assignedItem)
					{
						return true;
					}
				}
			}
			if (assignedItem is SimplePointGrabable simplePointGrabable2)
			{
				foreach (SimplePointGrabable connectedGrabable2 in simplePointGrabable2.ConnectedGrabables)
				{
					if (connectedGrabable2 == pointGrabable)
					{
						return true;
					}
				}
			}
			return false;
		}

		private static bool IsPointGrabableAlive(IPointGrabable pointGrabable)
		{
			if (pointGrabable == null)
			{
				return false;
			}
			if (pointGrabable is Object obj && obj == null)
			{
				return false;
			}
			return true;
		}
	}
}
