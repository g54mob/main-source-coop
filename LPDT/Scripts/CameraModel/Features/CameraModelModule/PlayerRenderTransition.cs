using System;
using Features.PlayerRenderModule.Scripts;
using UnityEngine;

namespace Features.CameraModelModule
{
	[Serializable]
	public class PlayerRenderTransition
	{
		[field: SerializeField]
		public bool Enabled { get; private set; }

		[field: SerializeField]
		public float PlayerStartTransitionAlpha { get; private set; }

		[field: SerializeField]
		public float PlayerEndTransitionAlpha { get; private set; }

		[field: SerializeField]
		public PlayerRendererSurfaceType PlayerStartSurface { get; private set; }

		[field: SerializeField]
		public PlayerRendererSurfaceType PlayerEndSurface { get; private set; }

		[field: SerializeField]
		public AnimationCurve RenderUpdateCurve { get; private set; }

		[field: SerializeField]
		public int PlayerStartRenderQueue { get; private set; }

		[field: SerializeField]
		public int PlayerEndRenderQueue { get; private set; }
	}
}
