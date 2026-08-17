using System;
using UnityEngine;

namespace EvilCore.DynamicCasting
{
	public interface ICastingManager : IInitialize
	{
		int RegisteredMeshCount { get; }

		int ActiveCount { get; }

		IRaycastHandle Register(CastRequest request, Action<CastResult> onResult);

		void Unregister(IRaycastHandle handle);

		void UnregisterAll();

		void SetEnabled(IRaycastHandle handle, bool enabled);

		void UpdateRequest(IRaycastHandle handle, CastRequest newRequest);

		CastResult CastImmediate(CastRequest request);

		void ProcessAll();

		void Process(IRaycastHandle handle);

		void RegisterMeshTarget(MeshFilter meshFilter);

		void UnregisterMeshTarget(MeshFilter meshFilter);

		void ClearMeshTargets();
	}
}
