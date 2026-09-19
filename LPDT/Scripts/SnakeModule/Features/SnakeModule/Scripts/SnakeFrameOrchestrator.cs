using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

namespace Features.SnakeModule.Scripts
{
	public class SnakeFrameOrchestrator : MonoBehaviour
	{
		[SerializeField]
		private SnakeVisualSlither _slither;

		[SerializeField]
		private SnakeController _controller;

		[SerializeField]
		private SnakeBodyLineRenderer _lineRenderer;

		private readonly List<ISnakeFrameLateTick> _extraLateTicks = new List<ISnakeFrameLateTick>();

		private static readonly ProfilerMarker _slitherMarker = new ProfilerMarker("Snake.Slither.TickLate");

		private static readonly ProfilerMarker _extraTicksMarker = new ProfilerMarker("Snake.ExtraLateTicks");

		public void RegisterLateTick(ISnakeFrameLateTick tick)
		{
			if (tick != null && !_extraLateTicks.Contains(tick))
			{
				_extraLateTicks.Add(tick);
			}
		}

		public void UnregisterLateTick(ISnakeFrameLateTick tick)
		{
			if (tick != null)
			{
				_extraLateTicks.Remove(tick);
			}
		}

		private void Update()
		{
			if (_controller != null)
			{
				_controller.TickUpdate();
			}
		}

		private void LateUpdate()
		{
			if (_slither != null)
			{
				using (_slitherMarker.Auto())
				{
					_slither.TickLate();
				}
			}
			if (_controller != null)
			{
				_controller.TickLate();
			}
			if (_lineRenderer != null)
			{
				_lineRenderer.TickLate();
			}
			using (_extraTicksMarker.Auto())
			{
				for (int i = 0; i < _extraLateTicks.Count; i++)
				{
					_extraLateTicks[i]?.TickLate();
				}
			}
		}
	}
}
