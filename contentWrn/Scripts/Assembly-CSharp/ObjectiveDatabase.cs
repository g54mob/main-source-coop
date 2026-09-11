using System;
using Zorro.Core;

public static class ObjectiveDatabase
{
	private static bool m_isInitialized;

	private static BidirectionalDictionary<byte, Type> types;

	public static void Initialize()
	{
		if (!m_isInitialized)
		{
			m_isInitialized = true;
			types = new BidirectionalDictionary<byte, Type>(15);
			types.Add(0, typeof(InviteFriendsObjective));
			types.Add(1, typeof(LeaveHouseObjective));
			types.Add(2, typeof(PickupTheCameraObjective));
			types.Add(3, typeof(EnterDiveBellObjective));
			types.Add(4, typeof(FilmSomethingScaryObjective));
			types.Add(5, typeof(ReturnToTheDiveBellObjective));
			types.Add(6, typeof(ExtractVideoObjective));
			types.Add(7, typeof(PickupDiscObjective));
			types.Add(8, typeof(UploadVideoObjective));
			types.Add(9, typeof(CelebrateObjective));
			types.Add(10, typeof(GoToBedSuccessObjective));
			types.Add(11, typeof(GoToBedFailedObjective));
			types.Add(12, typeof(WakeUpObjective));
			types.Add(13, typeof(BuyEquipmentObjective));
			types.Add(14, typeof(EnterDiveBellDay2Objective));
		}
	}

	public static byte GetIndex(Type type)
	{
		Initialize();
		return types.Get(type);
	}

	public static byte GetIndex(Objective objective)
	{
		Initialize();
		return types.Get(objective.GetType());
	}

	public static Type GetType(byte key)
	{
		Initialize();
		return types.GetFromKey(key);
	}

	public static Objective GetObjectiveInstance(byte index)
	{
		Initialize();
		return (Objective)Activator.CreateInstance(types.GetFromKey(index));
	}

	public static T GetObjectiveInstance<T>() where T : Objective
	{
		Initialize();
		byte key = types.Get(typeof(T));
		return (T)Activator.CreateInstance(types.GetFromKey(key));
	}

	public static bool HasObjectiveBeenCompleted(Objective objective)
	{
		return false;
	}
}
