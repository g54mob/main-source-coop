using System;
using System.Collections;
using UnityEngine;

namespace UnityHFSM
{
	public class CoState<TStateId, TEvent> : ActionState<TStateId, TEvent>
	{
		private readonly MonoBehaviour mono;

		private readonly Func<IEnumerator> coroutineCreator;

		private readonly Action<CoState<TStateId, TEvent>> onEnter;

		private readonly Action<CoState<TStateId, TEvent>> onExit;

		private readonly Func<CoState<TStateId, TEvent>, bool> canExit;

		private readonly bool shouldLoopCoroutine;

		public ITimer timer;

		private Coroutine activeCoroutine;

		public CoState(MonoBehaviour mono, Func<CoState<TStateId, TEvent>, IEnumerator> coroutine, Action<CoState<TStateId, TEvent>> onEnter = null, Action<CoState<TStateId, TEvent>> onExit = null, Func<CoState<TStateId, TEvent>, bool> canExit = null, bool loop = true, bool needsExitTime = false, bool isGhostState = false)
			: base(needsExitTime, isGhostState)
		{
			CoState<TStateId, TEvent> arg = this;
			this.mono = mono;
			coroutineCreator = () => coroutine(arg);
			this.onEnter = onEnter;
			this.onExit = onExit;
			this.canExit = canExit;
			shouldLoopCoroutine = loop;
			timer = new Timer();
		}

		public CoState(MonoBehaviour mono, Func<IEnumerator> coroutine, Action<CoState<TStateId, TEvent>> onEnter = null, Action<CoState<TStateId, TEvent>> onExit = null, Func<CoState<TStateId, TEvent>, bool> canExit = null, bool loop = true, bool needsExitTime = false, bool isGhostState = false)
			: base(needsExitTime, isGhostState)
		{
			this.mono = mono;
			coroutineCreator = coroutine;
			this.onEnter = onEnter;
			this.onExit = onExit;
			this.canExit = canExit;
			shouldLoopCoroutine = loop;
			timer = new Timer();
		}

		public override void OnEnter()
		{
			timer.Reset();
			onEnter?.Invoke(this);
			if (coroutineCreator != null)
			{
				activeCoroutine = mono.StartCoroutine(shouldLoopCoroutine ? LoopCoroutine() : coroutineCreator());
			}
		}

		private IEnumerator LoopCoroutine()
		{
			IEnumerator routine = coroutineCreator();
			while (true)
			{
				if (routine.MoveNext())
				{
					yield return routine.Current;
				}
				else
				{
					yield return null;
				}
				while (routine.MoveNext())
				{
					yield return routine.Current;
				}
				routine = coroutineCreator();
			}
		}

		public override void OnLogic()
		{
			if (needsExitTime && canExit != null && fsm.HasPendingTransition && canExit(this))
			{
				fsm.StateCanExit();
			}
		}

		public override void OnExit()
		{
			if (activeCoroutine != null)
			{
				mono.StopCoroutine(activeCoroutine);
				activeCoroutine = null;
			}
			onExit?.Invoke(this);
		}

		public override void OnExitRequest()
		{
			if (canExit != null && canExit(this))
			{
				fsm.StateCanExit();
			}
		}
	}
	public class CoState<TStateId> : CoState<TStateId, string>
	{
		public CoState(MonoBehaviour mono, Func<CoState<TStateId, string>, IEnumerator> coroutine, Action<CoState<TStateId, string>> onEnter = null, Action<CoState<TStateId, string>> onExit = null, Func<CoState<TStateId, string>, bool> canExit = null, bool loop = true, bool needsExitTime = false, bool isGhostState = false)
			: base(mono, coroutine, onEnter, onExit, canExit, loop, needsExitTime, isGhostState)
		{
		}

		public CoState(MonoBehaviour mono, Func<IEnumerator> coroutine, Action<CoState<TStateId, string>> onEnter = null, Action<CoState<TStateId, string>> onExit = null, Func<CoState<TStateId, string>, bool> canExit = null, bool loop = true, bool needsExitTime = false, bool isGhostState = false)
			: base(mono, coroutine, onEnter, onExit, canExit, loop, needsExitTime, isGhostState)
		{
		}
	}
	public class CoState : CoState<string, string>
	{
		public CoState(MonoBehaviour mono, Func<CoState<string, string>, IEnumerator> coroutine, Action<CoState<string, string>> onEnter = null, Action<CoState<string, string>> onExit = null, Func<CoState<string, string>, bool> canExit = null, bool loop = true, bool needsExitTime = false, bool isGhostState = false)
			: base(mono, coroutine, onEnter, onExit, canExit, loop, needsExitTime, isGhostState)
		{
		}

		public CoState(MonoBehaviour mono, Func<IEnumerator> coroutine, Action<CoState<string, string>> onEnter = null, Action<CoState<string, string>> onExit = null, Func<CoState<string, string>, bool> canExit = null, bool loop = true, bool needsExitTime = false, bool isGhostState = false)
			: base(mono, coroutine, onEnter, onExit, canExit, loop, needsExitTime, isGhostState)
		{
		}
	}
}
