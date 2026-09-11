using System;
using UnityEngine;

[Serializable]
public abstract class DealRewardBase
{
	[HideInInspector]
	public DIFFICULTY difficulty;

	[HideInInspector]
	public bool inited;

	public abstract bool UseInGame { get; }

	public virtual void Init(DIFFICULTY difficulty)
	{
		this.difficulty = difficulty;
		inited = true;
	}

	public abstract DealRewardBase CreateNew();

	public abstract string GetRewardDescription();

	public abstract bool ClaimReward();

	public abstract byte GetIndex();

	public abstract string GetRewardClaimDescription();

	public abstract void MakeLocalized();

	public virtual void DebugReward()
	{
	}

	protected byte GetIndex<T>() where T : DealRewardBase
	{
		string[] array = typeof(T).Name.Split('_');
		try
		{
			return byte.Parse(array[1]);
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			return byte.MaxValue;
		}
	}
}
