using System;
using UnityEngine;

namespace NomadDrive.Features.EvilRoads.Cable
{
	public class CableConnectionPoints : MonoBehaviour
	{
		[Header("Cable Entry Points")]
		[Tooltip("Points where cables from the PREVIOUS object connect. Index-matched with exit points.")]
		[SerializeField]
		private Transform[] _entryPoints;

		[Header("Cable Exit Points")]
		[Tooltip("Points where cables TO the NEXT object originate. Index-matched with entry points.")]
		[SerializeField]
		private Transform[] _exitPoints;

		public Transform[] EntryPoints => _entryPoints ?? Array.Empty<Transform>();

		public Transform[] ExitPoints => _exitPoints ?? Array.Empty<Transform>();

		public int EntryPointCount
		{
			get
			{
				Transform[] entryPoints = _entryPoints;
				if (entryPoints == null)
				{
					return 0;
				}
				return entryPoints.Length;
			}
		}

		public int ExitPointCount
		{
			get
			{
				Transform[] exitPoints = _exitPoints;
				if (exitPoints == null)
				{
					return 0;
				}
				return exitPoints.Length;
			}
		}

		public Transform GetEntryPoint(int index)
		{
			if (_entryPoints == null || index < 0 || index >= _entryPoints.Length)
			{
				return null;
			}
			return _entryPoints[index];
		}

		public Transform GetExitPoint(int index)
		{
			if (_exitPoints == null || index < 0 || index >= _exitPoints.Length)
			{
				return null;
			}
			return _exitPoints[index];
		}

		public int GetMatchingPointCount()
		{
			return Mathf.Min(EntryPointCount, ExitPointCount);
		}

		public bool ValidatePoints(out string errorMessage)
		{
			errorMessage = string.Empty;
			if (_entryPoints != null)
			{
				for (int i = 0; i < _entryPoints.Length; i++)
				{
					if (_entryPoints[i] == null)
					{
						errorMessage = $"Entry point at index {i} is null";
						return false;
					}
				}
			}
			if (_exitPoints != null)
			{
				for (int j = 0; j < _exitPoints.Length; j++)
				{
					if (_exitPoints[j] == null)
					{
						errorMessage = $"Exit point at index {j} is null";
						return false;
					}
				}
			}
			return true;
		}

		private void OnValidate()
		{
			if (EntryPointCount != ExitPointCount && EntryPointCount > 0 && ExitPointCount > 0)
			{
				Debug.LogWarning($"[CableConnectionPoints] Entry ({EntryPointCount}) and Exit ({ExitPointCount}) point counts don't match on {base.gameObject.name}");
			}
		}
	}
}
