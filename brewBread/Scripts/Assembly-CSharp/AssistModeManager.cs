using UnityEngine;

public class AssistModeManager : Singleton<AssistModeManager>
{
	[SerializeField]
	private bool _infiniteJumps;

	[SerializeField]
	private bool _pullingRope;

	[SerializeField]
	private bool _checkPoints;

	public bool InfiniteJumps
	{
		get
		{
			return _infiniteJumps;
		}
		set
		{
			_infiniteJumps = value;
		}
	}

	public bool PullingRope
	{
		get
		{
			return _pullingRope;
		}
		set
		{
			_pullingRope = value;
		}
	}

	public bool CheckPoints
	{
		get
		{
			return _checkPoints;
		}
		set
		{
			_checkPoints = value;
		}
	}
}
