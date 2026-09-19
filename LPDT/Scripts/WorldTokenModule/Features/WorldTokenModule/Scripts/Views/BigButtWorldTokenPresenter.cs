using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.WorldTokenModule.Scripts.Views
{
	[PublicAPI]
	public class BigButtWorldTokenPresenter : PresenterBehaviour<BigButtWorldTokenViewBase>
	{
		public Transform TokenTransform => base.View.transform;

		public void SetText(string text)
		{
			base.View.SetText(text);
		}

		public void StartMovement()
		{
			base.View.StartMovement();
		}

		public void StopMovement()
		{
			base.View.StopMovement();
		}

		public void SetCameraToLookAt(Camera camera)
		{
			base.View.ActiveCamera = camera;
		}

		public bool IsActive()
		{
			return base.View.IsActive;
		}

		public void SetPosition(Vector3 position)
		{
			base.View.transform.position = position;
		}

		public void SetColor(Color color)
		{
			base.View.SetColor(color);
		}

		public void SetMovementDirection(Vector2 direction)
		{
			base.View.SetMovementDirection(direction);
		}
	}
}
