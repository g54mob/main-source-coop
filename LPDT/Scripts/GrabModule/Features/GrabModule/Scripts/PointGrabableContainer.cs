using UnityEngine;

namespace Features.GrabModule.Scripts
{
	public class PointGrabableContainer : MonoBehaviour
	{
		[SerializeField]
		private SimplePointGrabable _pointGrabable;

		public IPointGrabable PointGrabable
		{
			get
			{
				if (!(_pointGrabable != null))
				{
					return null;
				}
				return _pointGrabable;
			}
		}

		public void SetPointGrabable(SimplePointGrabable pointGrabable)
		{
			_pointGrabable = pointGrabable;
		}
	}
}
