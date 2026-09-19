using System;
using UnityEngine;

namespace Fusion.Addons.Physics
{
	public class RunnerSimulatePhysics : SimulationBehaviour, IBeforeTick, IPublicFacingInterface, ISpawned, IDespawned
	{
		private static int _enabledRunnersCount;

		private static bool _originalAutoSimulation;

		private static SimulationMode2D _original2DSimulationMode;

		public bool Update2DPhysicsScene;

		public bool Update3DPhysicsScene;

		[SerializeField]
		private float _timeScale = 1f;

		private bool _initialized;

		public bool HasSimulatedThisTick { get; private set; }

		public float TimeScale
		{
			get
			{
				return Time.timeScale;
			}
			set
			{
				if ((bool)base.Runner && base.Runner.IsServer)
				{
					_timeScale = value;
				}
			}
		}

		public event Action<NetworkRunner> OnBeforeSimulate;

		public event Action<NetworkRunner> OnAfterSimulate;

		void ISpawned.Spawned()
		{
			Startup();
		}

		void IDespawned.Despawned(NetworkRunner runner, bool hasState)
		{
			Shutdown();
		}

		void IBeforeTick.BeforeTick()
		{
			HasSimulatedThisTick = false;
		}

		private void Startup()
		{
			_initialized = true;
			_enabledRunnersCount++;
			if (_enabledRunnersCount == 1)
			{
				_originalAutoSimulation = UnityEngine.Physics.autoSimulation;
				_original2DSimulationMode = Physics2D.simulationMode;
				UnityEngine.Physics.autoSimulation = false;
				Physics2D.simulationMode = SimulationMode2D.Script;
			}
		}

		private void Shutdown()
		{
			if (_initialized)
			{
				_initialized = false;
				_enabledRunnersCount--;
				if (_enabledRunnersCount == 0)
				{
					UnityEngine.Physics.autoSimulation = _originalAutoSimulation;
					Physics2D.simulationMode = _original2DSimulationMode;
				}
			}
		}

		public override void FixedUpdateNetwork()
		{
			if (base.Runner.TryGetPhysicsInfo(out var info))
			{
				if (base.Runner.IsServer)
				{
					info.TimeScale = _timeScale;
					base.Runner.TrySetPhysicsInfo(info);
				}
				else
				{
					_timeScale = info.TimeScale;
				}
			}
			float deltaTime = base.Runner.DeltaTime * _timeScale;
			SimulationExecute(deltaTime);
		}

		private void SimulationExecute(float deltaTime)
		{
			if (!(deltaTime <= 0f))
			{
				DoSimulatePhysicsScene(deltaTime);
			}
		}

		private void DoSimulatePhysicsScene(float deltaTime)
		{
			this.OnBeforeSimulate?.Invoke(base.Runner);
			SimulatePhysicsScene(deltaTime);
			HasSimulatedThisTick = true;
			this.OnAfterSimulate?.Invoke(base.Runner);
		}

		private void SimulatePhysicsScene(float deltaTime)
		{
			if (Update2DPhysicsScene && base.Runner.SceneManager.TryGetPhysicsScene2D(out var scene2D))
			{
				if (scene2D.IsValid())
				{
					scene2D.Simulate(deltaTime);
				}
				else
				{
					Physics2D.Simulate(deltaTime);
				}
			}
			if (Update3DPhysicsScene && base.Runner.SceneManager.TryGetPhysicsScene3D(out var scene3D))
			{
				if (scene3D.IsValid())
				{
					scene3D.Simulate(deltaTime);
				}
				else
				{
					UnityEngine.Physics.Simulate(deltaTime);
				}
			}
		}
	}
}
