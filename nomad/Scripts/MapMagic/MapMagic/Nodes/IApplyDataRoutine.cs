using System.Collections;
using UnityEngine;

namespace MapMagic.Nodes
{
	public interface IApplyDataRoutine : IApplyData
	{
		IEnumerator ApplyRoutine(Terrain terrain);
	}
}
