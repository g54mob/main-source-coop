using System.Collections.Generic;
using UnityEngine;

namespace Features.GrabModule.Scripts.PhysGrab.CartGrabber
{
	public interface ICartItemsContainer
	{
		HashSet<IPointGrabable> Items { get; }

		Transform transform { get; }

		IPointGrabable CartGrabbable { get; }
	}
}
