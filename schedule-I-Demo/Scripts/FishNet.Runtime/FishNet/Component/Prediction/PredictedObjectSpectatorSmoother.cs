using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet.Transporting;
using GameKit.Utilities;
using UnityEngine;

namespace FishNet.Component.Prediction
{
	internal class PredictedObjectSpectatorSmoother
	{
		private class GoalData
		{
			public bool IsActive;

			public uint LocalTick;

			public RateData Rates = new RateData();

			public TransformData Transforms = new TransformData();

			public void Reset()
			{
				LocalTick = 0u;
				Transforms.Reset();
				Rates.Reset();
				IsActive = false;
			}

			public void Update(GoalData gd)
			{
				LocalTick = gd.LocalTick;
				Rates.Update(gd.Rates);
				Transforms.Update(gd.Transforms);
				IsActive = true;
			}

			public void Update(uint localTick, RateData rd, TransformData td)
			{
				LocalTick = localTick;
				Rates = rd;
				Transforms = td;
				IsActive = true;
			}
		}

		private class RateData
		{
			public float Position;

			public float Rotation;

			public uint TickSpan;

			internal float TimeRemaining;

			public void Reset()
			{
				Position = 0f;
				Rotation = 0f;
				TickSpan = 0u;
				TimeRemaining = 0f;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Update(RateData rd)
			{
				Update(rd.Position, rd.Rotation, rd.TickSpan, rd.TimeRemaining);
			}

			public void Update(float position, float rotation, uint tickSpan, float timeRemaining)
			{
				Position = position;
				Rotation = rotation;
				TickSpan = tickSpan;
				TimeRemaining = timeRemaining;
			}
		}

		private class TransformData
		{
			public Vector3 Position;

			public Quaternion Rotation;

			public void Reset()
			{
				Position = Vector3.zero;
				Rotation = Quaternion.identity;
			}

			public void Update(TransformData copy)
			{
				Update(copy.Position, copy.Rotation);
			}

			public void Update(Vector3 position, Quaternion rotation)
			{
				Position = position;
				Rotation = rotation;
			}

			public void Update(Rigidbody rigidbody)
			{
				Position = rigidbody.transform.position;
				Rotation = rigidbody.transform.rotation;
			}

			public void Update(Rigidbody2D rigidbody)
			{
				Position = rigidbody.transform.position;
				Rotation = rigidbody.transform.rotation;
			}
		}

		private GoalData _currentGoalData = new GoalData();

		private Transform _graphicalObject;

		private bool _smoothPosition;

		private bool _smoothRotation;

		private uint _interpolation = 4u;

		private List<GoalData> _goalDatas = new List<GoalData>();

		private Rigidbody _rigidbody;

		private Rigidbody2D _rigidbody2d;

		private TransformData _preTickTransformdata = new TransformData();

		private RigidbodyType _rigidbodyType;

		private long _reconcileLocalTick = -1L;

		private bool _preTickReceived;

		private Vector3 _graphicalStartPosition;

		private Quaternion _graphicalStartRotation;

		private float _teleportThreshold;

		private PredictedObject _predictedObject;

		private static Stack<GoalData> _goalDataCache = new Stack<GoalData>();

		private uint _localTick;

		private uint _ignoredTicks;

		private Vector3 _startWorldPosition;

		private const float OVERFLOW_MULTIPLIER = 0.1f;

		private const float UNDERFLOW_MULTIPLIER = 0.02f;

		public void SetGraphicalObject(Transform value)
		{
			_graphicalObject = value;
		}

		public void SetInterpolation(uint value)
		{
			_interpolation = value;
		}

		public void SetIgnoredTicks(uint value)
		{
			_ignoredTicks = value;
		}

		internal void Initialize(PredictedObject po, RigidbodyType rbType, Rigidbody rb, Rigidbody2D rb2d, Transform graphicalObject, bool smoothPosition, bool smoothRotation, float teleportThreshold)
		{
			_predictedObject = po;
			_rigidbodyType = rbType;
			_rigidbody = rb;
			_rigidbody2d = rb2d;
			_graphicalObject = graphicalObject;
			_startWorldPosition = _graphicalObject.position;
			_smoothPosition = smoothPosition;
			_smoothRotation = smoothRotation;
			_teleportThreshold = teleportThreshold;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ManualUpdate()
		{
			if (CanSmooth())
			{
				MoveToTarget();
			}
		}

		public void OnPreTick()
		{
			if (CanSmooth())
			{
				_localTick = _predictedObject.TimeManager.LocalTick;
				if (!_preTickReceived)
				{
					uint tick = _predictedObject.TimeManager.LocalTick - 1;
					CreateGoalData(tick, postTick: false);
				}
				_preTickReceived = true;
				if (_rigidbodyType == RigidbodyType.Rigidbody)
				{
					_preTickTransformdata.Update(_rigidbody);
				}
				else
				{
					_preTickTransformdata.Update(_rigidbody2d);
				}
				_graphicalStartPosition = _graphicalObject.position;
				_graphicalStartRotation = _graphicalObject.rotation;
			}
		}

		public void OnPostTick()
		{
			if (CanSmooth())
			{
				if (!_preTickReceived)
				{
					_graphicalObject.SetPositionAndRotation(_startWorldPosition, Quaternion.identity);
					return;
				}
				_graphicalObject.SetPositionAndRotation(_graphicalStartPosition, _graphicalStartRotation);
				CreateGoalData(_predictedObject.TimeManager.LocalTick, postTick: true);
			}
		}

		public void OnPreReplay(uint tick)
		{
			if (!_preTickReceived && CanSmooth())
			{
				CreateGoalData(tick, postTick: false);
			}
		}

		public void OnPostReplay(uint tick)
		{
			if (CanSmooth() && _reconcileLocalTick != -1)
			{
				CreateGoalData(tick, postTick: false);
			}
		}

		private bool CanSmooth()
		{
			if (_interpolation == 0)
			{
				return false;
			}
			if (_predictedObject.IsPredictingOwner() || _predictedObject.IsServer)
			{
				return false;
			}
			return true;
		}

		public void SetLocalReconcileTick(long value)
		{
			_reconcileLocalTick = value;
		}

		private void StoreGoalData(GoalData gd)
		{
			gd.Reset();
			_goalDataCache.Push(gd);
		}

		private bool GraphicalObjectMatches(Vector3 localPosition, Quaternion localRotation)
		{
			bool num = !_smoothPosition || _graphicalObject.position == localPosition;
			bool flag = !_smoothRotation || _graphicalObject.rotation == localRotation;
			return num && flag;
		}

		private bool HasChanged(TransformData a, TransformData b)
		{
			if (!(a.Position != b.Position))
			{
				return a.Rotation != b.Rotation;
			}
			return true;
		}

		private bool HasChanged(TransformData td)
		{
			Transform transform = ((_rigidbodyType != RigidbodyType.Rigidbody) ? _rigidbody2d.transform : _rigidbody.transform);
			if (!(td.Position != transform.position))
			{
				return td.Rotation != transform.rotation;
			}
			return true;
		}

		private void SetCurrentGoalData(bool afterMove)
		{
			if (_goalDatas.Count == 0)
			{
				_currentGoalData.IsActive = false;
				return;
			}
			_currentGoalData.Update(_goalDatas[0]);
			StoreGoalData(_goalDatas[0]);
			_goalDatas.RemoveAt(0);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void MoveToTarget(float deltaOverride = -1f)
		{
			if (!_currentGoalData.IsActive)
			{
				SetCurrentGoalData(afterMove: false);
				if (!_currentGoalData.IsActive)
				{
					return;
				}
			}
			float num = ((deltaOverride != -1f) ? deltaOverride : Time.deltaTime);
			TransformData transforms = _currentGoalData.Transforms;
			RateData rates = _currentGoalData.Rates;
			int num2 = _goalDatas.Count - (int)_interpolation;
			float num4;
			if (num2 > 0)
			{
				float num3 = ((!_predictedObject.IsOwner) ? 0.1f : 0.1f);
				num4 = 1f + num3;
			}
			else if (num2 < 0)
			{
				float num5 = 0.02f * (float)Mathf.Abs(num2);
				if (num5 > 0.9f)
				{
					num5 = 0.9f;
				}
				num4 = 1f - num5;
			}
			else
			{
				num4 = 1f;
			}
			Transform graphicalObject = _graphicalObject;
			if (_smoothPosition)
			{
				float position = rates.Position;
				Vector3 position2 = transforms.Position;
				if (position == -1f)
				{
					graphicalObject.position = transforms.Position;
				}
				else if (position > 0f)
				{
					graphicalObject.position = Vector3.MoveTowards(graphicalObject.position, position2, position * num * num4);
				}
			}
			if (_smoothRotation)
			{
				float position = rates.Rotation;
				if (position == -1f)
				{
					graphicalObject.rotation = transforms.Rotation;
				}
				else if (position > 0f)
				{
					graphicalObject.rotation = Quaternion.RotateTowards(graphicalObject.rotation, transforms.Rotation, position * num);
				}
			}
			if (rates.TimeRemaining > 0f)
			{
				float num6 = num * num4;
				float timeRemaining = rates.TimeRemaining - num6;
				rates.TimeRemaining = timeRemaining;
			}
			if (!(rates.TimeRemaining <= 0f))
			{
				return;
			}
			float num7 = Mathf.Abs(rates.TimeRemaining);
			SetCurrentGoalData(afterMove: true);
			if (_currentGoalData.IsActive)
			{
				if (num7 > 0f)
				{
					MoveToTarget(num7);
				}
			}
			else if (!GraphicalObjectMatches(transforms.Position, transforms.Rotation))
			{
				_currentGoalData.IsActive = true;
			}
		}

		private void SetInstantRates(RateData rd)
		{
			rd.Update(-1f, -1f, 1u, -1f);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void SetCalculatedRates(GoalData prevGoalData, GoalData nextGoalData, Channel channel)
		{
			TransformData transforms = nextGoalData.Transforms;
			if (channel == Channel.Reliable && HasChanged(prevGoalData.Transforms, transforms))
			{
				nextGoalData.Rates.Update(prevGoalData.Rates);
				return;
			}
			uint num = prevGoalData.LocalTick;
			if (num == 0)
			{
				num = nextGoalData.LocalTick - 1;
			}
			uint num2 = nextGoalData.LocalTick - num;
			float num3 = (float)_predictedObject.TimeManager.TicksToTime(num2);
			RateData rates = nextGoalData.Rates;
			float num4 = Vector3.Distance(prevGoalData.Transforms.Position, transforms.Position);
			if (_teleportThreshold >= 0f && num4 >= _teleportThreshold)
			{
				SetInstantRates(rates);
				return;
			}
			float num5 = num4 / num3;
			num4 = prevGoalData.Transforms.Rotation.Angle(transforms.Rotation, precise: true);
			float num6 = num4 / num3;
			if (num5 == 0f)
			{
				num5 = -1f;
			}
			if (num6 == 0f)
			{
				num6 = -1f;
			}
			rates.Update(num5, num6, num2, num3);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CreateGoalData(uint tick, bool postTick)
		{
			int num = (int)(_interpolation * 8);
			int num2 = _goalDatas.Count - num;
			if (num2 > 0)
			{
				for (int i = 0; i < num2; i++)
				{
					StoreGoalData(_goalDatas[i]);
				}
				_goalDatas.RemoveRange(0, num2);
			}
			uint localTick = _currentGoalData.LocalTick;
			if (tick <= localTick)
			{
				return;
			}
			int count = _goalDatas.Count;
			int num3 = count + 1;
			GoalData goalData;
			if (postTick)
			{
				bool flag;
				if (count == 0)
				{
					goalData = MakeGoalDataFromPreTickTransform();
					flag = HasChanged(goalData.Transforms);
				}
				else
				{
					goalData = _goalDatas[count - 1];
					if (tick - goalData.LocalTick != 1)
					{
						goalData = MakeGoalDataFromPreTickTransform();
					}
					flag = HasChanged(goalData.Transforms);
				}
				if (!flag)
				{
					if (count > 0 && goalData != _goalDatas[count - 1])
					{
						StoreGoalData(goalData);
					}
					return;
				}
			}
			else
			{
				int index = -1;
				if (tick != localTick + 1)
				{
					goalData = ((tick == 0) ? null : GetGoalData(tick - 1, out index));
				}
				else
				{
					goalData = _currentGoalData;
					num3 = 0;
				}
				if (index != -1)
				{
					num3 = index + 1;
				}
				if (goalData == null)
				{
					GoalData goalData2 = RetrieveGoalData();
					goalData2.Transforms.Update(_preTickTransformdata);
					if (!HasChanged(goalData2.Transforms))
					{
						StoreGoalData(goalData2);
						return;
					}
					goalData = goalData2;
				}
				else if (!goalData.IsActive)
				{
					return;
				}
			}
			GoalData goalData3 = RetrieveGoalData();
			goalData3.LocalTick = tick;
			TransformData transforms = goalData3.Transforms;
			if (_rigidbodyType == RigidbodyType.Rigidbody)
			{
				transforms.Update(_rigidbody);
			}
			else
			{
				transforms.Update(_rigidbody2d);
			}
			if (!_smoothPosition)
			{
				transforms.Position = _graphicalStartPosition;
			}
			if (!_smoothRotation)
			{
				transforms.Rotation = _graphicalStartRotation;
			}
			SetCalculatedRates(goalData, goalData3, Channel.Unreliable);
			if (num3 >= _goalDatas.Count)
			{
				_goalDatas.Add(goalData3);
			}
			else
			{
				_goalDatas[num3].Update(goalData3);
			}
			GoalData MakeGoalDataFromPreTickTransform()
			{
				GoalData goalData4 = RetrieveGoalData();
				goalData4.Transforms.Update(_preTickTransformdata);
				return goalData4;
			}
		}

		private GoalData GetGoalData(uint tick, out int index)
		{
			index = -1;
			if (tick == 0)
			{
				return null;
			}
			for (int i = 0; i < _goalDatas.Count; i++)
			{
				if (_goalDatas[i].LocalTick == tick)
				{
					index = i;
					return _goalDatas[i];
				}
			}
			return null;
		}

		private GoalData RetrieveGoalData()
		{
			GoalData obj = ((_goalDataCache.Count > 0) ? _goalDataCache.Pop() : new GoalData());
			obj.IsActive = true;
			return obj;
		}
	}
}
