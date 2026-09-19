using System;
using System.Collections.Generic;
using System.Linq;
using Features.GameCycle.Scripts.SessionCleanup;

namespace Features.LineArmModule.Scripts
{
	public class LineArmsModel : ISessionCleanup
	{
		private readonly Dictionary<int, Dictionary<LineArmType, LineArmControllerBase>> _lineArms = new Dictionary<int, Dictionary<LineArmType, LineArmControllerBase>>();

		public Action<int> OnLineArmPlayerRegistered;

		public LineArmControllerBase GetLineArmForPlayer(int playerId)
		{
			if (!TryGetLineArmForPlayer(playerId, out var lineArm))
			{
				throw new KeyNotFoundException($"No line arm registered for player {playerId}.");
			}
			return lineArm;
		}

		public LineArmControllerBase GetLineArmForPlayer(int playerId, LineArmType lineArmType)
		{
			if (!_lineArms.TryGetValue(playerId, out var value))
			{
				return null;
			}
			return value.Values.FirstOrDefault((LineArmControllerBase a) => a != null && a.enabled && a.ArmType == lineArmType);
		}

		public bool TryGetLineArmForPlayer(int playerId, out LineArmControllerBase lineArm)
		{
			lineArm = null;
			if (!_lineArms.TryGetValue(playerId, out var value))
			{
				return false;
			}
			lineArm = value.Values.FirstOrDefault((LineArmControllerBase a) => a != null && a.enabled);
			return lineArm != null;
		}

		public bool TryGetLineArmForPlayer(int playerId, LineArmType lineArmType, out LineArmControllerBase lineArm)
		{
			lineArm = null;
			if (!_lineArms.TryGetValue(playerId, out var value))
			{
				return false;
			}
			return value.TryGetValue(lineArmType, out lineArm);
		}

		public Dictionary<LineArmType, LineArmControllerBase> GetAllLineArmsForPlayer(int playerId)
		{
			if (!_lineArms.TryGetValue(playerId, out var value))
			{
				return null;
			}
			return value;
		}

		public void RegisterLineArm(int playerId, LineArmType lineArmType, LineArmControllerBase lineArm)
		{
			if (!_lineArms.TryGetValue(playerId, out var value))
			{
				value = new Dictionary<LineArmType, LineArmControllerBase>();
				_lineArms[playerId] = value;
			}
			value[lineArmType] = lineArm;
			OnLineArmPlayerRegistered?.Invoke(playerId);
		}

		public void UnregisterLineArm(int playerId, LineArmType lineArmType)
		{
			if (_lineArms.TryGetValue(playerId, out var value))
			{
				value.Remove(lineArmType);
				if (value.Count == 0)
				{
					_lineArms.Remove(playerId);
				}
			}
		}

		public void UnregisterPlayer(int playerId)
		{
			_lineArms.Remove(playerId);
		}

		public void Cleanup()
		{
			_lineArms.Clear();
		}
	}
}
