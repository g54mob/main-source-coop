using UnityEngine;

namespace QFSW.QC.Actions
{
	public class WaitRealtime : ICommandAction
	{
		private float _startTime;

		private readonly float _duration;

		public bool IsFinished => Time.realtimeSinceStartup >= _startTime + _duration;

		public bool StartsIdle => true;

		public WaitRealtime(float seconds)
		{
			_duration = seconds;
		}

		public void Start(ActionContext ctx)
		{
			_startTime = Time.realtimeSinceStartup;
		}

		public void Finalize(ActionContext ctx)
		{
		}
	}
}
