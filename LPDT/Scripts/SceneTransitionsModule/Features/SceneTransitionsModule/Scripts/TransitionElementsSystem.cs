using System;
using UnityEngine;

namespace Features.SceneTransitionsModule.Scripts
{
	public class TransitionElementsSystem : IDisposable
	{
		private readonly SceneTransitionCanvas _transitionCanvas;

		private readonly SceneTransitionCamera _transitionCamera;

		public TransitionElementsSystem(SceneTransitionCanvas transitionCanvas, SceneTransitionCamera transitionCamera)
		{
			_transitionCanvas = transitionCanvas;
			_transitionCamera = transitionCamera;
		}

		public void Dispose()
		{
			UnityEngine.Object.Destroy(_transitionCanvas.gameObject);
			UnityEngine.Object.Destroy(_transitionCamera.gameObject);
		}
	}
}
