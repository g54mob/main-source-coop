using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Profiling;
using UnityEngine;

namespace Obi
{
	[AddComponentMenu("Physics/Obi/Obi Solver", 800)]
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[HelpURL("https://obi.virtualmethodstudio.com/manual/7.0/obisolver.html")]
	public sealed class ObiSolver : MonoBehaviour
	{
		public enum BackendType
		{
			[InspectorName("Compute (GPU)")]
			Compute = 0,
			[InspectorName("Burst (CPU)")]
			Burst = 1
		}

		public enum Synchronization
		{
			Asynchronous = 0,
			Synchronous = 1,
			SynchronousFixed = 2
		}

		[Serializable]
		public class ParticleInActor
		{
			public ObiActor actor;

			public int indexInActor;

			public ParticleInActor()
			{
				actor = null;
				indexInActor = -1;
			}

			public ParticleInActor(ObiActor actor, int indexInActor)
			{
				this.actor = actor;
				this.indexInActor = indexInActor;
			}
		}

		public class SpatialQuery
		{
			public ObiNativeQueryShapeList shapes;

			public ObiNativeAffineTransformList transforms;

			public ObiNativeQueryResultList results;

			public Action callback;

			public bool synchronous;

			public bool isValid
			{
				get
				{
					if (shapes != null && transforms != null && results != null && shapes.count > 0)
					{
						return transforms.count > 0;
					}
					return false;
				}
			}

			public bool done => results.noReadbackInFlight;

			public SpatialQuery(ObiNativeQueryShapeList shapes, ObiNativeAffineTransformList transforms, ObiNativeQueryResultList results, Action callback = null, bool synchronous = false)
			{
				this.shapes = shapes;
				this.transforms = transforms;
				this.results = results;
				this.callback = callback;
				this.synchronous = synchronous;
			}

			public void WaitForCompletion()
			{
				results.WaitForReadback();
			}
		}

		public delegate void SolverCallback(ObiSolver solver);

		public delegate void SolverStepCallback(ObiSolver solver, float timeToSimulate, float substepTime);

		public delegate void CollisionCallback(ObiSolver solver, ObiNativeContactList contacts);

		public delegate void SpatialQueryCallback(ObiSolver solver, ObiNativeQueryResultList results);

		private static ProfilerMarker m_StateInterpolationPerfMarker = new ProfilerMarker("ApplyStateInterpolation");

		private static ProfilerMarker m_UpdateVisibilityPerfMarker = new ProfilerMarker("UpdateVisibility");

		private static ProfilerMarker m_GetSolverBoundsPerfMarker = new ProfilerMarker("GetSolverBounds");

		private static ProfilerMarker m_TestBoundsPerfMarker = new ProfilerMarker("TestBoundsAgainstCameras");

		private static ProfilerMarker m_GetAllCamerasPerfMarker = new ProfilerMarker("GetAllCameras");

		private static ProfilerMarker m_PushActiveParticles = new ProfilerMarker("PushActiveParticles");

		private static ProfilerMarker m_UpdateColliderWorld = new ProfilerMarker("UpdateColliderWorld");

		private static ProfilerMarker m_PushSimplices = new ProfilerMarker("PushSimplices");

		private static ProfilerMarker m_PushDeformableEdges = new ProfilerMarker("PushDeformableEdges");

		private static ProfilerMarker m_PushDeformableTriangles = new ProfilerMarker("PushDeformableTriangles");

		[Tooltip("If enabled, will force the solver to keep simulating even when not visible from any camera.")]
		public bool simulateWhenInvisible = true;

		private IObiBackend m_SimulationBackend = new BurstBackend();

		[SerializeField]
		private BackendType m_Backend = BackendType.Burst;

		private ObiRenderSystemStack m_RenderSystems = new ObiRenderSystemStack(3);

		[Min(1f)]
		public int substeps = 4;

		[Min(0f)]
		public int maxStepsPerFrame = 1;

		public Synchronization synchronization;

		public bool isUpdatedFixed = true;

		public Oni.SolverParameters parameters = new Oni.SolverParameters(Oni.SolverParameters.Interpolation.None, new Vector4(0f, -9.81f, 0f, 0f));

		[Min(32f)]
		[SerializeField]
		private uint m_MaxSurfaceChunks = 32768u;

		public uint maxQueryResults = 8192u;

		public uint maxFoamParticles = 8192u;

		public uint maxParticleNeighbors = 128u;

		public uint maxParticleContacts = 6u;

		public bool useLimits;

		public bool killOffLimitsParticles;

		public Bounds boundaryLimits = new Bounds(Vector3.zero, new Vector3(10f, 10f, 10f));

		public Vector3 gravity = new Vector3(0f, -9.81f, 0f);

		public Space gravitySpace = Space.Self;

		public Vector3 ambientWind = new Vector3(0f, 0f, 0f);

		public Space windSpace = Space.Self;

		[Min(1f)]
		public int foamSubsteps = 1;

		[Tooltip("Minimum amount of fluid particles around a foam particle necessary for the foam to be advected, instead of following a ballistic trajectory.")]
		[Min(0f)]
		public int foamMinNeighbors = 3;

		[Tooltip("When enabled, each individual foam particle will be tested for collisions against ObiColliders.")]
		public bool foamCollisions;

		[Tooltip("Foam particles can stretch along the direction of their velocity. This parameter controls the maximum amount of stretch.")]
		[Range(0f, 3f)]
		public float maxFoamVelocityStretch = 0.3f;

		[Tooltip("Scales the size of foam particles.")]
		[Min(0f)]
		public float foamRadiusScale = 1f;

		[Tooltip("Determines how foam particles fade in/out during its lifetime.")]
		[MinMax(0f, 1f)]
		public Vector2 foamFade = new Vector2(0.05f, 0.8f);

		[Tooltip("Determines the utilization % range in which particles age faster.")]
		[MinMax(0f, 1f)]
		public Vector2 foamAccelAgingRange = new Vector2(0.5f, 0.8f);

		[Tooltip("Determines the utilization % range in which particles age faster.")]
		[Min(1f)]
		public float foamAccelAging = 4f;

		[Tooltip("Color of the light scattered by foam.")]
		[Min(0f)]
		public float foamVolumeDensity = 0.1f;

		[Tooltip("Color of the light scattered by foam.")]
		[Min(0f)]
		public float foamAmbientDensity = 0.02f;

		[Tooltip("Color of the light scattered by foam.")]
		public Color foamScatterColor = new Color(0.8f, 0.75f, 0.7f, 1f);

		[Tooltip("Color used for foam ambient lighting.")]
		public Color foamAmbientColor = new Color(0.4f, 0.5f, 0.6f, 1f);

		[Tooltip("How much does world-space linear inertia affect particles in the solver.")]
		[Range(0f, 1f)]
		public float worldLinearInertiaScale;

		[Tooltip("How much does world-space angular inertia affect particles in the solver.")]
		[Range(0f, 1f)]
		public float worldAngularInertiaScale;

		[NonSerialized]
		[HideInInspector]
		public List<ObiActor> actors = new List<ObiActor>();

		[NonSerialized]
		[HideInInspector]
		private ParticleInActor[] m_ParticleToActor;

		[NonSerialized]
		[HideInInspector]
		private Queue<ObiActor> addBuffer = new Queue<ObiActor>();

		private ObiNativeIntList freeList;

		private Stack<int> freeGroupIDs = new Stack<int>();

		[NonSerialized]
		public ObiNativeIntList deformableTriangles;

		[NonSerialized]
		public ObiNativeIntList deformableEdges;

		[NonSerialized]
		public ObiNativeVector2List deformableUVs;

		[NonSerialized]
		private ObiNativeIntList m_Points;

		[NonSerialized]
		private ObiNativeIntList m_Edges;

		[NonSerialized]
		private ObiNativeIntList m_Triangles;

		[NonSerialized]
		public SimplexCounts m_SimplexCounts;

		[NonSerialized]
		private IObiJobHandle simulationHandle;

		[NonSerialized]
		private Synchronization bufferedSynchronization;

		[NonSerialized]
		private int steps;

		[NonSerialized]
		private float substepTime;

		[NonSerialized]
		private float simulatedTime;

		[NonSerialized]
		private float accumulatedTime;

		[NonSerialized]
		[HideInInspector]
		public bool dirtyDeformableTriangles = true;

		[NonSerialized]
		[HideInInspector]
		public bool dirtyDeformableEdges = true;

		[NonSerialized]
		[HideInInspector]
		public Oni.SimplexType dirtySimplices = Oni.SimplexType.All;

		[NonSerialized]
		[HideInInspector]
		public int dirtyRendering;

		[NonSerialized]
		[HideInInspector]
		public int dirtyConstraints;

		public bool synchronousSpatialQueries;

		private bool m_dirtyActiveParticles = true;

		private Bounds m_Bounds;

		private Bounds m_BoundsWS;

		private Plane[] planes = new Plane[6];

		private Camera[] sceneCameras = new Camera[1];

		[NonSerialized]
		private IObiConstraints[] m_Constraints = new IObiConstraints[18];

		public Oni.ConstraintParameters distanceConstraintParameters = new Oni.ConstraintParameters(enabled: true, Oni.ConstraintParameters.EvaluationOrder.Sequential, 1);

		public Oni.ConstraintParameters bendingConstraintParameters = new Oni.ConstraintParameters(enabled: true, Oni.ConstraintParameters.EvaluationOrder.Parallel, 1);

		public Oni.ConstraintParameters particleCollisionConstraintParameters = new Oni.ConstraintParameters(enabled: true, Oni.ConstraintParameters.EvaluationOrder.Sequential, 1);

		public Oni.ConstraintParameters particleFrictionConstraintParameters = new Oni.ConstraintParameters(enabled: true, Oni.ConstraintParameters.EvaluationOrder.Parallel, 1);

		public Oni.ConstraintParameters collisionConstraintParameters = new Oni.ConstraintParameters(enabled: true, Oni.ConstraintParameters.EvaluationOrder.Sequential, 1);

		public Oni.ConstraintParameters frictionConstraintParameters = new Oni.ConstraintParameters(enabled: true, Oni.ConstraintParameters.EvaluationOrder.Parallel, 1);

		public Oni.ConstraintParameters skinConstraintParameters = new Oni.ConstraintParameters(enabled: true, Oni.ConstraintParameters.EvaluationOrder.Sequential, 1);

		public Oni.ConstraintParameters volumeConstraintParameters = new Oni.ConstraintParameters(enabled: true, Oni.ConstraintParameters.EvaluationOrder.Parallel, 1);

		public Oni.ConstraintParameters shapeMatchingConstraintParameters = new Oni.ConstraintParameters(enabled: true, Oni.ConstraintParameters.EvaluationOrder.Parallel, 1);

		public Oni.ConstraintParameters tetherConstraintParameters = new Oni.ConstraintParameters(enabled: true, Oni.ConstraintParameters.EvaluationOrder.Parallel, 1);

		public Oni.ConstraintParameters pinConstraintParameters = new Oni.ConstraintParameters(enabled: true, Oni.ConstraintParameters.EvaluationOrder.Parallel, 1);

		public Oni.ConstraintParameters pinholeConstraintParameters = new Oni.ConstraintParameters(enabled: true, Oni.ConstraintParameters.EvaluationOrder.Parallel, 1);

		public Oni.ConstraintParameters stitchConstraintParameters = new Oni.ConstraintParameters(enabled: true, Oni.ConstraintParameters.EvaluationOrder.Parallel, 1);

		public Oni.ConstraintParameters densityConstraintParameters = new Oni.ConstraintParameters(enabled: true, Oni.ConstraintParameters.EvaluationOrder.Parallel, 1);

		public Oni.ConstraintParameters stretchShearConstraintParameters = new Oni.ConstraintParameters(enabled: true, Oni.ConstraintParameters.EvaluationOrder.Sequential, 1);

		public Oni.ConstraintParameters bendTwistConstraintParameters = new Oni.ConstraintParameters(enabled: true, Oni.ConstraintParameters.EvaluationOrder.Sequential, 1);

		public Oni.ConstraintParameters chainConstraintParameters = new Oni.ConstraintParameters(enabled: false, Oni.ConstraintParameters.EvaluationOrder.Sequential, 1);

		private ObiNativeVector4List m_RigidbodyLinearVelocities;

		private ObiNativeVector4List m_RigidbodyAngularVelocities;

		[NonSerialized]
		private ObiNativeColorList m_Colors;

		[NonSerialized]
		private ObiNativeInt4List m_CellCoords;

		[NonSerialized]
		private ObiNativeIntList m_ActiveParticles;

		[NonSerialized]
		private ObiNativeIntList m_Simplices;

		[NonSerialized]
		private ObiNativeIntList m_DeadParticles;

		[NonSerialized]
		private ObiNativeVector4List m_Positions;

		[NonSerialized]
		private ObiNativeVector4List m_PrevPositions;

		[NonSerialized]
		private ObiNativeVector4List m_RestPositions;

		[NonSerialized]
		private ObiNativeVector4List m_StartPositions;

		[NonSerialized]
		private ObiNativeVector4List m_EndPositions;

		[NonSerialized]
		private ObiNativeVector4List m_RenderablePositions;

		[NonSerialized]
		private ObiNativeQuaternionList m_Orientations;

		[NonSerialized]
		private ObiNativeQuaternionList m_PrevOrientations;

		[NonSerialized]
		private ObiNativeQuaternionList m_RestOrientations;

		[NonSerialized]
		private ObiNativeQuaternionList m_StartOrientations;

		[NonSerialized]
		private ObiNativeQuaternionList m_EndOrientations;

		[NonSerialized]
		private ObiNativeQuaternionList m_RenderableOrientations;

		[NonSerialized]
		private ObiNativeVector4List m_Velocities;

		[NonSerialized]
		private ObiNativeVector4List m_AngularVelocities;

		[NonSerialized]
		private ObiNativeFloatList m_InvMasses;

		[NonSerialized]
		private ObiNativeFloatList m_InvRotationalMasses;

		[NonSerialized]
		private ObiNativeVector4List m_ExternalForces;

		[NonSerialized]
		private ObiNativeVector4List m_ExternalTorques;

		[NonSerialized]
		private ObiNativeVector4List m_Wind;

		[NonSerialized]
		private ObiNativeVector4List m_PositionDeltas;

		[NonSerialized]
		private ObiNativeQuaternionList m_OrientationDeltas;

		[NonSerialized]
		private ObiNativeIntList m_PositionConstraintCounts;

		[NonSerialized]
		private ObiNativeIntList m_OrientationConstraintCounts;

		[NonSerialized]
		private ObiNativeIntList m_CollisionMaterials;

		[NonSerialized]
		private ObiNativeIntList m_Phases;

		[NonSerialized]
		private ObiNativeIntList m_Filters;

		[NonSerialized]
		private ObiNativeVector4List m_PrincipalRadii;

		[NonSerialized]
		private ObiNativeVector4List m_RenderableRadii;

		[NonSerialized]
		private ObiNativeVector4List m_Normals;

		[NonSerialized]
		private ObiNativeFloatList m_Life;

		[NonSerialized]
		private ObiNativeVector4List m_FluidData;

		[NonSerialized]
		private ObiNativeVector4List m_FluidMaterials;

		[NonSerialized]
		private ObiNativeVector4List m_FluidMaterials2;

		[NonSerialized]
		private ObiNativeVector4List m_FluidInterface;

		[NonSerialized]
		private ObiNativeVector4List m_UserData;

		[NonSerialized]
		private ObiNativeMatrix4x4List m_Anisotropy;

		[NonSerialized]
		private ObiNativeVector4List m_FoamPositions;

		[NonSerialized]
		private ObiNativeVector4List m_FoamVelocities;

		[NonSerialized]
		private ObiNativeVector4List m_FoamColors;

		[NonSerialized]
		private ObiNativeVector4List m_FoamAttributes;

		[NonSerialized]
		private ObiNativeIntList m_FoamCount;

		[NonSerialized]
		private ObiNativeContactList m_ColliderContacts;

		[NonSerialized]
		private ObiNativeContactList m_ParticleContacts;

		[NonSerialized]
		private ObiNativeEffectiveMassesList m_ContactEffectiveMasses;

		[NonSerialized]
		private ObiNativeEffectiveMassesList m_ParticleContactEffectiveMasses;

		[NonSerialized]
		private ObiNativeQueryShapeList m_BufferedQueryShapes;

		[NonSerialized]
		private ObiNativeAffineTransformList m_BufferedQueryTransforms;

		[NonSerialized]
		private ObiNativeQueryShapeList m_QueryShapes;

		[NonSerialized]
		private ObiNativeAffineTransformList m_QueryTransforms;

		[NonSerialized]
		private ObiNativeQueryResultList m_QueryResults;

		public uint maxSurfaceChunks
		{
			get
			{
				return m_MaxSurfaceChunks;
			}
			set
			{
				m_MaxSurfaceChunks = value;
				dirtyRendering |= 512;
			}
		}

		public uint usedSurfaceChunks
		{
			get
			{
				if (!(GetRenderSystem(Oni.RenderingSystemType.Fluid) is ISurfaceChunkUser surfaceChunkUser))
				{
					return 0u;
				}
				return surfaceChunkUser.usedChunkCount;
			}
		}

		public float timeSinceSimulationStart { get; private set; }

		public bool dirtyActiveParticles
		{
			get
			{
				return m_dirtyActiveParticles;
			}
			set
			{
				m_dirtyActiveParticles = value;
			}
		}

		public ISolverImpl implementation { get; private set; }

		public bool initialized => implementation != null;

		public IObiBackend simulationBackend => m_SimulationBackend;

		public BackendType backendType
		{
			get
			{
				return m_Backend;
			}
			set
			{
				if (m_Backend != value)
				{
					m_Backend = value;
					UpdateBackend();
				}
			}
		}

		public SimplexCounts simplexCounts => m_SimplexCounts;

		public Bounds bounds => m_BoundsWS;

		public Bounds localBounds => m_Bounds;

		public bool isVisible { get; private set; } = true;

		public float maxScale { get; private set; } = 1f;

		public bool simulationInFlight { get; private set; }

		public int pendingQueryCount => bufferedQueryShapes.count;

		public int allocParticleCount => particleToActor.Count((ParticleInActor s) => s != null && s.actor != null);

		public int activeParticleCount => activeParticles.count;

		public int contactCount
		{
			get
			{
				if (backendType != BackendType.Burst && this.OnCollision == null)
				{
					return 0;
				}
				return colliderContacts.count;
			}
		}

		public int particleContactCount
		{
			get
			{
				if (backendType != BackendType.Burst && this.OnParticleCollision == null)
				{
					return 0;
				}
				return particleContacts.count;
			}
		}

		public ParticleInActor[] particleToActor
		{
			get
			{
				if (m_ParticleToActor == null)
				{
					m_ParticleToActor = new ParticleInActor[0];
				}
				return m_ParticleToActor;
			}
		}

		public ObiNativeIntList activeParticles
		{
			get
			{
				if (m_ActiveParticles == null)
				{
					m_ActiveParticles = new ObiNativeIntList();
				}
				return m_ActiveParticles;
			}
		}

		public ObiNativeIntList deadParticles
		{
			get
			{
				if (m_DeadParticles == null)
				{
					m_DeadParticles = new ObiNativeIntList();
				}
				return m_DeadParticles;
			}
		}

		public ObiNativeIntList simplices
		{
			get
			{
				if (m_Simplices == null)
				{
					m_Simplices = new ObiNativeIntList();
				}
				return m_Simplices;
			}
		}

		public ObiNativeIntList points
		{
			get
			{
				if (m_Points == null)
				{
					m_Points = new ObiNativeIntList(8, 16);
				}
				return m_Points;
			}
		}

		public ObiNativeIntList edges
		{
			get
			{
				if (m_Edges == null)
				{
					m_Edges = new ObiNativeIntList(8, 16);
				}
				return m_Edges;
			}
		}

		public ObiNativeIntList triangles
		{
			get
			{
				if (m_Triangles == null)
				{
					m_Triangles = new ObiNativeIntList(8, 16);
				}
				return m_Triangles;
			}
		}

		public ObiNativeVector4List rigidbodyLinearDeltas
		{
			get
			{
				if (m_RigidbodyLinearVelocities == null)
				{
					m_RigidbodyLinearVelocities = new ObiNativeVector4List();
				}
				return m_RigidbodyLinearVelocities;
			}
		}

		public ObiNativeVector4List rigidbodyAngularDeltas
		{
			get
			{
				if (m_RigidbodyAngularVelocities == null)
				{
					m_RigidbodyAngularVelocities = new ObiNativeVector4List();
				}
				return m_RigidbodyAngularVelocities;
			}
		}

		public ObiNativeColorList colors
		{
			get
			{
				if (m_Colors == null)
				{
					m_Colors = new ObiNativeColorList();
				}
				return m_Colors;
			}
		}

		public ObiNativeInt4List cellCoords
		{
			get
			{
				if (m_CellCoords == null)
				{
					m_CellCoords = new ObiNativeInt4List(8, 16, new VInt4(int.MaxValue));
				}
				return m_CellCoords;
			}
		}

		public ObiNativeVector4List positions
		{
			get
			{
				if (m_Positions == null)
				{
					m_Positions = new ObiNativeVector4List();
				}
				return m_Positions;
			}
		}

		public ObiNativeVector4List prevPositions
		{
			get
			{
				if (m_PrevPositions == null)
				{
					m_PrevPositions = new ObiNativeVector4List();
				}
				return m_PrevPositions;
			}
		}

		public ObiNativeVector4List restPositions
		{
			get
			{
				if (m_RestPositions == null)
				{
					m_RestPositions = new ObiNativeVector4List();
				}
				return m_RestPositions;
			}
		}

		public ObiNativeVector4List startPositions
		{
			get
			{
				if (m_StartPositions == null)
				{
					m_StartPositions = new ObiNativeVector4List();
				}
				return m_StartPositions;
			}
		}

		public ObiNativeVector4List endPositions
		{
			get
			{
				if (m_EndPositions == null)
				{
					m_EndPositions = new ObiNativeVector4List();
				}
				return m_EndPositions;
			}
		}

		public ObiNativeVector4List renderablePositions
		{
			get
			{
				if (m_RenderablePositions == null)
				{
					m_RenderablePositions = new ObiNativeVector4List();
				}
				return m_RenderablePositions;
			}
		}

		public ObiNativeQuaternionList orientations
		{
			get
			{
				if (m_Orientations == null)
				{
					m_Orientations = new ObiNativeQuaternionList();
				}
				return m_Orientations;
			}
		}

		public ObiNativeQuaternionList prevOrientations
		{
			get
			{
				if (m_PrevOrientations == null)
				{
					m_PrevOrientations = new ObiNativeQuaternionList();
				}
				return m_PrevOrientations;
			}
		}

		public ObiNativeQuaternionList restOrientations
		{
			get
			{
				if (m_RestOrientations == null)
				{
					m_RestOrientations = new ObiNativeQuaternionList();
				}
				return m_RestOrientations;
			}
		}

		public ObiNativeQuaternionList startOrientations
		{
			get
			{
				if (m_StartOrientations == null)
				{
					m_StartOrientations = new ObiNativeQuaternionList();
				}
				return m_StartOrientations;
			}
		}

		public ObiNativeQuaternionList endOrientations
		{
			get
			{
				if (m_EndOrientations == null)
				{
					m_EndOrientations = new ObiNativeQuaternionList();
				}
				return m_EndOrientations;
			}
		}

		public ObiNativeQuaternionList renderableOrientations
		{
			get
			{
				if (m_RenderableOrientations == null)
				{
					m_RenderableOrientations = new ObiNativeQuaternionList();
				}
				return m_RenderableOrientations;
			}
		}

		public ObiNativeVector4List velocities
		{
			get
			{
				if (m_Velocities == null)
				{
					m_Velocities = new ObiNativeVector4List();
				}
				return m_Velocities;
			}
		}

		public ObiNativeVector4List angularVelocities
		{
			get
			{
				if (m_AngularVelocities == null)
				{
					m_AngularVelocities = new ObiNativeVector4List();
				}
				return m_AngularVelocities;
			}
		}

		public ObiNativeFloatList invMasses
		{
			get
			{
				if (m_InvMasses == null)
				{
					m_InvMasses = new ObiNativeFloatList();
				}
				return m_InvMasses;
			}
		}

		public ObiNativeFloatList invRotationalMasses
		{
			get
			{
				if (m_InvRotationalMasses == null)
				{
					m_InvRotationalMasses = new ObiNativeFloatList();
				}
				return m_InvRotationalMasses;
			}
		}

		public ObiNativeVector4List externalForces
		{
			get
			{
				if (m_ExternalForces == null)
				{
					m_ExternalForces = new ObiNativeVector4List();
				}
				return m_ExternalForces;
			}
		}

		public ObiNativeVector4List externalTorques
		{
			get
			{
				if (m_ExternalTorques == null)
				{
					m_ExternalTorques = new ObiNativeVector4List();
				}
				return m_ExternalTorques;
			}
		}

		public ObiNativeVector4List wind
		{
			get
			{
				if (m_Wind == null)
				{
					m_Wind = new ObiNativeVector4List();
				}
				return m_Wind;
			}
		}

		public ObiNativeVector4List positionDeltas
		{
			get
			{
				if (m_PositionDeltas == null)
				{
					m_PositionDeltas = new ObiNativeVector4List();
				}
				return m_PositionDeltas;
			}
		}

		public ObiNativeQuaternionList orientationDeltas
		{
			get
			{
				if (m_OrientationDeltas == null)
				{
					m_OrientationDeltas = new ObiNativeQuaternionList(8, 16, new Quaternion(0f, 0f, 0f, 0f));
				}
				return m_OrientationDeltas;
			}
		}

		public ObiNativeIntList positionConstraintCounts
		{
			get
			{
				if (m_PositionConstraintCounts == null)
				{
					m_PositionConstraintCounts = new ObiNativeIntList();
				}
				return m_PositionConstraintCounts;
			}
		}

		public ObiNativeIntList orientationConstraintCounts
		{
			get
			{
				if (m_OrientationConstraintCounts == null)
				{
					m_OrientationConstraintCounts = new ObiNativeIntList();
				}
				return m_OrientationConstraintCounts;
			}
		}

		public ObiNativeIntList collisionMaterials
		{
			get
			{
				if (m_CollisionMaterials == null)
				{
					m_CollisionMaterials = new ObiNativeIntList();
				}
				return m_CollisionMaterials;
			}
		}

		public ObiNativeIntList phases
		{
			get
			{
				if (m_Phases == null)
				{
					m_Phases = new ObiNativeIntList();
				}
				return m_Phases;
			}
		}

		public ObiNativeIntList filters
		{
			get
			{
				if (m_Filters == null)
				{
					m_Filters = new ObiNativeIntList();
				}
				return m_Filters;
			}
		}

		public ObiNativeVector4List renderableRadii
		{
			get
			{
				if (m_RenderableRadii == null)
				{
					m_RenderableRadii = new ObiNativeVector4List();
				}
				return m_RenderableRadii;
			}
		}

		public ObiNativeVector4List principalRadii
		{
			get
			{
				if (m_PrincipalRadii == null)
				{
					m_PrincipalRadii = new ObiNativeVector4List();
				}
				return m_PrincipalRadii;
			}
		}

		public ObiNativeVector4List normals
		{
			get
			{
				if (m_Normals == null)
				{
					m_Normals = new ObiNativeVector4List();
				}
				return m_Normals;
			}
		}

		public ObiNativeFloatList life
		{
			get
			{
				if (m_Life == null)
				{
					m_Life = new ObiNativeFloatList();
				}
				return m_Life;
			}
		}

		public ObiNativeVector4List fluidData
		{
			get
			{
				if (m_FluidData == null)
				{
					m_FluidData = new ObiNativeVector4List();
				}
				return m_FluidData;
			}
		}

		public ObiNativeVector4List userData
		{
			get
			{
				if (m_UserData == null)
				{
					m_UserData = new ObiNativeVector4List();
				}
				return m_UserData;
			}
		}

		public ObiNativeVector4List fluidInterface
		{
			get
			{
				if (m_FluidInterface == null)
				{
					m_FluidInterface = new ObiNativeVector4List();
				}
				return m_FluidInterface;
			}
		}

		public ObiNativeVector4List fluidMaterials
		{
			get
			{
				if (m_FluidMaterials == null)
				{
					m_FluidMaterials = new ObiNativeVector4List();
				}
				return m_FluidMaterials;
			}
		}

		public ObiNativeVector4List fluidMaterials2
		{
			get
			{
				if (m_FluidMaterials2 == null)
				{
					m_FluidMaterials2 = new ObiNativeVector4List();
				}
				return m_FluidMaterials2;
			}
		}

		public ObiNativeMatrix4x4List anisotropies
		{
			get
			{
				if (m_Anisotropy == null)
				{
					m_Anisotropy = new ObiNativeMatrix4x4List();
				}
				return m_Anisotropy;
			}
		}

		public ObiNativeVector4List foamPositions
		{
			get
			{
				if (m_FoamPositions == null)
				{
					m_FoamPositions = new ObiNativeVector4List();
				}
				return m_FoamPositions;
			}
		}

		public ObiNativeVector4List foamVelocities
		{
			get
			{
				if (m_FoamVelocities == null)
				{
					m_FoamVelocities = new ObiNativeVector4List();
				}
				return m_FoamVelocities;
			}
		}

		public ObiNativeVector4List foamColors
		{
			get
			{
				if (m_FoamColors == null)
				{
					m_FoamColors = new ObiNativeVector4List();
				}
				return m_FoamColors;
			}
		}

		public ObiNativeVector4List foamAttributes
		{
			get
			{
				if (m_FoamAttributes == null)
				{
					m_FoamAttributes = new ObiNativeVector4List();
				}
				return m_FoamAttributes;
			}
		}

		public ObiNativeIntList foamCount
		{
			get
			{
				if (m_FoamCount == null)
				{
					m_FoamCount = new ObiNativeIntList();
					m_FoamCount.ResizeUninitialized(9);
					m_FoamCount.CopyFrom(new int[9] { 0, 1, 1, 0, 0, 1, 1, 0, 0 }, 0, 0, 9);
				}
				return m_FoamCount;
			}
		}

		public ObiNativeContactList colliderContacts
		{
			get
			{
				if (m_ColliderContacts == null)
				{
					m_ColliderContacts = new ObiNativeContactList();
				}
				return m_ColliderContacts;
			}
		}

		public ObiNativeContactList particleContacts
		{
			get
			{
				if (m_ParticleContacts == null)
				{
					m_ParticleContacts = new ObiNativeContactList();
				}
				return m_ParticleContacts;
			}
		}

		public ObiNativeEffectiveMassesList contactEffectiveMasses
		{
			get
			{
				if (m_ContactEffectiveMasses == null)
				{
					m_ContactEffectiveMasses = new ObiNativeEffectiveMassesList();
				}
				return m_ContactEffectiveMasses;
			}
		}

		public ObiNativeEffectiveMassesList particleContactEffectiveMasses
		{
			get
			{
				if (m_ParticleContactEffectiveMasses == null)
				{
					m_ParticleContactEffectiveMasses = new ObiNativeEffectiveMassesList();
				}
				return m_ParticleContactEffectiveMasses;
			}
		}

		private ObiNativeQueryShapeList bufferedQueryShapes
		{
			get
			{
				if (m_BufferedQueryShapes == null)
				{
					m_BufferedQueryShapes = new ObiNativeQueryShapeList();
				}
				return m_BufferedQueryShapes;
			}
		}

		private ObiNativeAffineTransformList bufferedQueryTransforms
		{
			get
			{
				if (m_BufferedQueryTransforms == null)
				{
					m_BufferedQueryTransforms = new ObiNativeAffineTransformList(8, 16);
				}
				return m_BufferedQueryTransforms;
			}
		}

		private ObiNativeQueryShapeList queryShapes
		{
			get
			{
				if (m_QueryShapes == null)
				{
					m_QueryShapes = new ObiNativeQueryShapeList();
				}
				return m_QueryShapes;
			}
		}

		private ObiNativeAffineTransformList queryTransforms
		{
			get
			{
				if (m_QueryTransforms == null)
				{
					m_QueryTransforms = new ObiNativeAffineTransformList(8, 16);
				}
				return m_QueryTransforms;
			}
		}

		public ObiNativeQueryResultList queryResults
		{
			get
			{
				if (m_QueryResults == null)
				{
					m_QueryResults = new ObiNativeQueryResultList();
				}
				return m_QueryResults;
			}
		}

		public event CollisionCallback OnCollision;

		public event CollisionCallback OnParticleCollision;

		public event SpatialQueryCallback OnSpatialQueryResults;

		public event SolverCallback OnAdvection;

		public event SolverCallback OnInitialize;

		public event SolverCallback OnTeardown;

		public event SolverCallback OnUpdateParameters;

		public event SolverCallback OnParticleCountChanged;

		public event SolverStepCallback OnSimulationStart;

		public event SolverStepCallback OnCollisionDetectionStart;

		public event SolverStepCallback OnSubstepsStart;

		public event SolverCallback OnRequestReadback;

		public event SolverStepCallback OnSimulationEnd;

		public event SolverStepCallback OnInterpolate;

		public void OnEnable()
		{
			bufferedSynchronization = synchronization;
			accumulatedTime = 0f;
		}

		private void FixedUpdate()
		{
			if (isUpdatedFixed)
			{
				ProcessUpdate();
			}
		}

		private void Update()
		{
			ObiColliderWorld.GetInstance().SetDirty();
		}

		private void ProcessUpdate()
		{
			if (steps++ == 0 && bufferedSynchronization == Synchronization.Asynchronous)
			{
				CompleteSimulation();
			}
			if (bufferedSynchronization == Synchronization.SynchronousFixed)
			{
				ObiColliderWorld.GetInstance().SetDirty();
				ObiColliderWorld.GetInstance().UpdateWorld(Time.fixedDeltaTime);
				StartSimulation(Time.fixedDeltaTime, 1);
				CompleteSimulation();
			}
		}

		private void LateUpdate()
		{
			if (!isUpdatedFixed)
			{
				ProcessUpdate();
			}
			Vector3 lossyScale = base.transform.lossyScale;
			maxScale = Mathf.Max(Mathf.Max(lossyScale.x, lossyScale.y), lossyScale.z);
			if (Application.isPlaying)
			{
				ObiColliderWorld.GetInstance().UpdateWorld(Time.fixedDeltaTime * (float)steps, steps > 0 && bufferedSynchronization != Synchronization.SynchronousFixed);
				accumulatedTime += Time.deltaTime - Time.fixedDeltaTime * (float)steps;
				accumulatedTime = Mathf.Clamp(accumulatedTime, 0f, Time.fixedDeltaTime);
			}
			else
			{
				accumulatedTime = 0f;
				UpdateBounds();
			}
			if (bufferedSynchronization == Synchronization.Asynchronous || bufferedSynchronization == Synchronization.SynchronousFixed)
			{
				Render(accumulatedTime);
			}
			if (Application.isPlaying && bufferedSynchronization != Synchronization.SynchronousFixed)
			{
				StartSimulation(Time.fixedDeltaTime, steps);
			}
			if (bufferedSynchronization == Synchronization.Synchronous)
			{
				if (steps > 0)
				{
					CompleteSimulation();
				}
				Render(accumulatedTime);
			}
			steps = 0;
		}

		private void OnApplicationQuit()
		{
			OnDestroy();
		}

		private void OnDestroy()
		{
			while (actors.Count > 0)
			{
				RemoveActor(actors[actors.Count - 1]);
			}
		}

		private void CreateBackend()
		{
			switch (m_Backend)
			{
			case BackendType.Burst:
				m_SimulationBackend = new BurstBackend();
				return;
			case BackendType.Compute:
				if (SystemInfo.supportsComputeShaders)
				{
					m_SimulationBackend = new ComputeBackend();
					return;
				}
				break;
			}
			Debug.LogWarning("The Burst backend depends on the following packages: Mathematics, Collections, Jobs and Burst. Please install the required dependencies. Simulation will fall back to the compute backend, if possible.");
			if (SystemInfo.supportsComputeShaders)
			{
				m_SimulationBackend = new ComputeBackend();
				return;
			}
			Debug.LogError("This platform doesn't support compute shaders. Please switch to the Burst backend.");
			m_SimulationBackend = new NullBackend();
		}

		public void Initialize()
		{
			if (!initialized)
			{
				CreateBackend();
				substepTime = Time.fixedDeltaTime / (float)substeps;
				actors = new List<ObiActor>();
				freeList = new ObiNativeIntList();
				m_ParticleToActor = new ParticleInActor[0];
				deformableUVs = new ObiNativeVector2List();
				deformableTriangles = new ObiNativeIntList();
				deformableEdges = new ObiNativeIntList();
				m_Constraints[5] = new ObiDistanceConstraintsData();
				m_Constraints[4] = new ObiBendConstraintsData();
				m_Constraints[14] = new ObiAerodynamicConstraintsData();
				m_Constraints[8] = new ObiStretchShearConstraintsData();
				m_Constraints[7] = new ObiBendTwistConstraintsData();
				m_Constraints[2] = new ObiChainConstraintsData();
				m_Constraints[6] = new ObiShapeMatchingConstraintsData();
				m_Constraints[1] = new ObiVolumeConstraintsData();
				m_Constraints[0] = new ObiTetherConstraintsData();
				m_Constraints[13] = new ObiSkinConstraintsData();
				m_Constraints[9] = new ObiPinConstraintsData();
				m_Constraints[3] = new ObiPinholeConstraintsData();
				implementation = m_SimulationBackend.CreateSolver(this, 0);
				implementation.ParticleCountChanged(this);
				implementation.SetRigidbodyArrays(this);
				this.OnParticleCountChanged?.Invoke(this);
				InitializeTransformFrame();
				ObiColliderWorld.GetInstance().SetDirty();
				ObiColliderWorld.GetInstance().UpdateWorld(0f);
				ObiColliderWorld.GetInstance().SetDirty();
				this.OnInitialize?.Invoke(this);
				PushSolverParameters();
			}
		}

		public void Teardown()
		{
			if (initialized)
			{
				CompleteSimulation();
				PushConstraints();
				m_SimulationBackend.DestroySolver(implementation);
				implementation = null;
				FreeParticleArrays();
				FreeRigidbodyArrays();
				freeList.Dispose();
				m_Bounds = default(Bounds);
				this.OnTeardown?.Invoke(this);
			}
		}

		public void UpdateBackend()
		{
			List<ObiActor> list = new List<ObiActor>(actors);
			foreach (ObiActor item in list)
			{
				item.RemoveFromSolver();
			}
			foreach (ObiActor item2 in list)
			{
				item2.AddToSolver();
			}
		}

		private void FreeRigidbodyArrays()
		{
			rigidbodyLinearDeltas.Dispose();
			rigidbodyAngularDeltas.Dispose();
			m_RigidbodyLinearVelocities = null;
			m_RigidbodyAngularVelocities = null;
		}

		public void EnsureRigidbodyArraysCapacity(int count)
		{
			if (initialized && (count > rigidbodyLinearDeltas.count || !rigidbodyLinearDeltas.isCreated))
			{
				rigidbodyLinearDeltas.ResizeInitialized(count);
				rigidbodyAngularDeltas.ResizeInitialized(count);
				implementation.SetRigidbodyArrays(this);
			}
		}

		private void FreeParticleArrays()
		{
			activeParticles.Dispose();
			deadParticles.Dispose();
			simplices.Dispose();
			points.Dispose();
			edges.Dispose();
			triangles.Dispose();
			colors.Dispose();
			cellCoords.Dispose();
			startPositions.Dispose();
			endPositions.Dispose();
			startOrientations.Dispose();
			endOrientations.Dispose();
			positions.Dispose();
			prevPositions.Dispose();
			restPositions.Dispose();
			velocities.Dispose();
			orientations.Dispose();
			prevOrientations.Dispose();
			restOrientations.Dispose();
			angularVelocities.Dispose();
			invMasses.Dispose();
			invRotationalMasses.Dispose();
			principalRadii.Dispose();
			collisionMaterials.Dispose();
			phases.Dispose();
			filters.Dispose();
			renderablePositions.Dispose();
			renderableOrientations.Dispose();
			renderableRadii.Dispose();
			fluidInterface.Dispose();
			fluidMaterials.Dispose();
			fluidMaterials2.Dispose();
			foamPositions.Dispose();
			foamVelocities.Dispose();
			foamColors.Dispose();
			foamAttributes.Dispose();
			foamCount.Dispose();
			anisotropies.Dispose();
			life.Dispose();
			fluidData.Dispose();
			userData.Dispose();
			externalForces.Dispose();
			externalTorques.Dispose();
			wind.Dispose();
			positionDeltas.Dispose();
			orientationDeltas.Dispose();
			positionConstraintCounts.Dispose();
			orientationConstraintCounts.Dispose();
			normals.Dispose();
			colliderContacts.Dispose();
			particleContacts.Dispose();
			contactEffectiveMasses.Dispose();
			particleContactEffectiveMasses.Dispose();
			bufferedQueryShapes.Dispose();
			bufferedQueryTransforms.Dispose();
			queryShapes.Dispose();
			queryTransforms.Dispose();
			queryResults.Dispose();
			deformableUVs.Dispose();
			deformableTriangles.Dispose();
			deformableEdges.Dispose();
			m_ActiveParticles = null;
			m_DeadParticles = null;
			m_Simplices = null;
			m_Points = null;
			m_Edges = null;
			m_Triangles = null;
			m_Colors = null;
			m_CellCoords = null;
			m_Positions = null;
			m_RestPositions = null;
			m_PrevPositions = null;
			m_StartPositions = null;
			m_EndPositions = null;
			m_RenderablePositions = null;
			m_Orientations = null;
			m_RestOrientations = null;
			m_PrevOrientations = null;
			m_StartOrientations = null;
			m_EndOrientations = null;
			m_RenderableOrientations = null;
			m_Velocities = null;
			m_AngularVelocities = null;
			m_InvMasses = null;
			m_InvRotationalMasses = null;
			m_ExternalForces = null;
			m_ExternalTorques = null;
			m_Wind = null;
			m_PositionDeltas = null;
			m_OrientationDeltas = null;
			m_PositionConstraintCounts = null;
			m_OrientationConstraintCounts = null;
			m_CollisionMaterials = null;
			m_Phases = null;
			m_Filters = null;
			m_RenderableRadii = null;
			m_PrincipalRadii = null;
			m_Normals = null;
			m_Life = null;
			m_FluidData = null;
			m_UserData = null;
			m_FluidInterface = null;
			m_FluidMaterials = null;
			m_FluidMaterials2 = null;
			m_FoamPositions = null;
			m_FoamVelocities = null;
			m_FoamColors = null;
			m_FoamAttributes = null;
			m_FoamCount = null;
			m_Anisotropy = null;
			m_ColliderContacts = null;
			m_ParticleContacts = null;
			m_ContactEffectiveMasses = null;
			m_ParticleContactEffectiveMasses = null;
			m_BufferedQueryShapes = null;
			m_BufferedQueryTransforms = null;
			m_QueryShapes = null;
			m_QueryTransforms = null;
			m_QueryResults = null;
			deformableUVs = null;
			deformableTriangles = null;
			deformableEdges = null;
		}

		private void EnsureParticleArraysCapacity(int count)
		{
			if (count >= positions.count)
			{
				colors.ResizeInitialized(count, Color.white);
				deadParticles.ResizeInitialized(count, 0);
				startPositions.ResizeInitialized(count);
				endPositions.ResizeInitialized(count);
				positions.ResizeInitialized(count);
				prevPositions.ResizeInitialized(count);
				restPositions.ResizeInitialized(count);
				startOrientations.ResizeInitialized(count, Quaternion.identity);
				endOrientations.ResizeInitialized(count, Quaternion.identity);
				orientations.ResizeInitialized(count, Quaternion.identity);
				prevOrientations.ResizeInitialized(count, Quaternion.identity);
				restOrientations.ResizeInitialized(count, Quaternion.identity);
				renderablePositions.ResizeInitialized(count);
				renderableOrientations.ResizeInitialized(count, Quaternion.identity);
				velocities.ResizeInitialized(count);
				angularVelocities.ResizeInitialized(count);
				invMasses.ResizeInitialized(count, 0f);
				invRotationalMasses.ResizeInitialized(count, 0f);
				principalRadii.ResizeInitialized(count);
				collisionMaterials.ResizeInitialized(count, 0);
				phases.ResizeInitialized(count, 0);
				filters.ResizeInitialized(count, 0);
				renderableRadii.ResizeInitialized(count);
				fluidInterface.ResizeInitialized(count);
				fluidMaterials.ResizeInitialized(count);
				fluidMaterials2.ResizeInitialized(count);
				anisotropies.ResizeInitialized(count);
				life.ResizeInitialized(count, float.PositiveInfinity);
				fluidData.ResizeInitialized(count);
				userData.ResizeInitialized(count);
				externalForces.ResizeInitialized(count);
				externalTorques.ResizeInitialized(count);
				wind.ResizeInitialized(count);
				positionDeltas.ResizeInitialized(count);
				orientationDeltas.ResizeInitialized(count, new Quaternion(0f, 0f, 0f, 0f));
				positionConstraintCounts.ResizeInitialized(count, 0);
				orientationConstraintCounts.ResizeInitialized(count, 0);
				normals.ResizeInitialized(count);
				deadParticles.count = 0;
			}
			if (count >= m_ParticleToActor.Length)
			{
				Array.Resize(ref m_ParticleToActor, count * 2);
			}
		}

		private void UpdateFoamParticleCapacity()
		{
			if (maxFoamParticles != foamPositions.count)
			{
				foamPositions.ResizeUninitialized((int)maxFoamParticles);
				foamVelocities.ResizeUninitialized((int)maxFoamParticles);
				foamColors.ResizeUninitialized((int)maxFoamParticles);
				foamAttributes.ResizeUninitialized((int)maxFoamParticles);
				foamCount[3] = Mathf.Min(foamCount[3], (int)maxFoamParticles);
				implementation.MaxFoamParticleCountChanged(this);
			}
		}

		private void AllocateParticles(ObiNativeIntList particleIndices)
		{
			if (particleIndices.count > freeList.count)
			{
				int num = particleIndices.count - freeList.count;
				for (int i = 0; i < num; i++)
				{
					freeList.Add(positions.count + i);
				}
				EnsureParticleArraysCapacity(positions.count + particleIndices.count);
			}
			int num2 = freeList.count - particleIndices.count;
			particleIndices.CopyFrom(freeList, num2, 0, particleIndices.count);
			freeList.ResizeUninitialized(num2);
		}

		private void FreeParticles(ObiNativeIntList particleIndices)
		{
			freeList.AddRange(particleIndices);
		}

		private void CollisionCallbacks()
		{
			if (this.OnCollision != null)
			{
				colliderContacts.WaitForReadback();
				this.OnCollision(this, colliderContacts);
			}
			if (this.OnParticleCollision != null)
			{
				particleContacts.WaitForReadback();
				this.OnParticleCollision(this, particleContacts);
			}
			if (this.OnAdvection != null)
			{
				foamPositions.WaitForReadback();
				foamVelocities.WaitForReadback();
				foamAttributes.WaitForReadback();
				foamColors.WaitForReadback();
				foamCount.WaitForReadback();
				this.OnAdvection(this);
				foamPositions.Upload();
				foamVelocities.Upload();
				foamAttributes.Upload();
				foamColors.Upload();
				foamCount.Upload();
			}
		}

		private void NotifyDeceasedParticles()
		{
			deadParticles.WaitForReadback();
			int count = deadParticles.count;
			for (int i = 0; i < count; i++)
			{
				int num = deadParticles[i];
				ParticleInActor particleInActor = particleToActor[num];
				particleInActor?.actor.DeactivateParticle(particleInActor.indexInActor);
			}
			deadParticles.count = 0;
		}

		public void StartSimulation(float stepDelta, int simulationSteps)
		{
			if (simulationSteps <= 0)
			{
				return;
			}
			CompleteSimulation();
			simulatedTime = stepDelta * (float)simulationSteps;
			substepTime = stepDelta / (float)substeps;
			bufferedSynchronization = synchronization;
			NotifyDeceasedParticles();
			ObiActor result;
			while (addBuffer.TryDequeue(out result))
			{
				InsertBufferedActor(result);
			}
			if (!initialized || maxStepsPerFrame <= 0)
			{
				return;
			}
			simulationInFlight = true;
			int num = Mathf.Min(maxStepsPerFrame, simulationSteps) * substeps;
			float num2 = (float)num * substepTime;
			UpdateFoamParticleCapacity();
			using (m_UpdateColliderWorld.Auto())
			{
				ObiColliderWorld.GetInstance().UpdateCollisionMaterials();
				EnsureRigidbodyArraysCapacity(ObiColliderWorld.GetInstance().rigidbodyHandles.Count);
			}
			this.OnSimulationStart?.Invoke(this, num2, substepTime);
			foreach (ObiActor actor in actors)
			{
				actor.SimulationStart(num2, substepTime);
			}
			PushActiveParticles();
			PushSimplices();
			PushDeformableTriangles();
			PushDeformableEdges();
			PushConstraints();
			parameters.gravity = ((gravitySpace == Space.World) ? base.transform.InverseTransformVector(gravity) : gravity);
			parameters.ambientWind = ((windSpace == Space.World) ? base.transform.InverseTransformVector(ambientWind) : ambientWind);
			implementation.SetParameters(parameters);
			m_RenderSystems.Step();
			implementation.PushData();
			this.OnCollisionDetectionStart?.Invoke(this, num2, substepTime);
			foreach (ObiActor actor2 in actors)
			{
				actor2.CollisionDetectionStart(num2, substepTime);
			}
			simulationHandle = UpdateTransformFrame(simulatedTime);
			simulationHandle = implementation.UpdateBounds(simulationHandle, simulatedTime);
			if (simulateWhenInvisible || isVisible)
			{
				simulationHandle = implementation.CollisionDetection(simulationHandle, simulatedTime);
				simulationHandle?.Complete();
			}
			FlushSpatialQueries();
			this.OnSubstepsStart?.Invoke(this, num2, substepTime);
			foreach (ObiActor actor3 in actors)
			{
				actor3.SubstepsStart(num2, substepTime);
			}
			float num3 = simulatedTime;
			for (int i = 0; i < num; i++)
			{
				if ((simulateWhenInvisible || isVisible) && initialized)
				{
					simulationHandle = implementation.Substep(simulationHandle, stepDelta, substepTime, simulationSteps, num3);
				}
				num3 -= substepTime;
			}
			timeSinceSimulationStart += num2;
			RequestReadback();
		}

		private void FlushSpatialQueries()
		{
			while (bufferedQueryShapes.count > 0)
			{
				queryShapes.ResizeUninitialized(bufferedQueryShapes.count);
				queryTransforms.ResizeUninitialized(bufferedQueryTransforms.count);
				queryShapes.CopyFrom(bufferedQueryShapes);
				queryTransforms.CopyFrom(bufferedQueryTransforms);
				bufferedQueryShapes.Clear();
				bufferedQueryTransforms.Clear();
				implementation.SpatialQuery(queryShapes, queryTransforms, queryResults);
				queryResults.Readback();
				if (synchronousSpatialQueries)
				{
					queryResults.WaitForReadback();
					this.OnSpatialQueryResults?.Invoke(this, queryResults);
				}
			}
		}

		public void CompleteSimulation()
		{
			if (!initialized || !simulationInFlight)
			{
				return;
			}
			simulationHandle?.Complete();
			implementation.FinishSimulation();
			this.OnSimulationEnd?.Invoke(this, simulatedTime, substepTime);
			foreach (ObiActor actor in actors)
			{
				actor.SimulationEnd(simulatedTime, substepTime);
			}
			ObiColliderWorld.GetInstance().UpdateRigidbodyVelocities(this);
			if (!synchronousSpatialQueries)
			{
				queryResults.WaitForReadback();
				this.OnSpatialQueryResults?.Invoke(this, queryResults);
			}
			CollisionCallbacks();
			simulationInFlight = false;
		}

		public void Render(float unsimulatedTime)
		{
			if (!initialized)
			{
				return;
			}
			if (simulateWhenInvisible || isVisible)
			{
				using (m_StateInterpolationPerfMarker.Auto())
				{
					simulationHandle = implementation.ApplyInterpolation(simulationHandle, startPositions, startOrientations, Time.fixedDeltaTime, unsimulatedTime);
				}
			}
			simulationHandle?.Complete();
			UpdateVisibility();
			this.OnInterpolate?.Invoke(this, simulatedTime, substepTime);
			foreach (ObiActor actor in actors)
			{
				actor.Interpolate(simulatedTime, substepTime);
			}
			if (!Application.isPlaying)
			{
				positions.Upload();
				orientations.Upload();
				renderablePositions.Upload();
				renderableOrientations.Upload();
			}
			if (dirtyRendering != 0)
			{
				m_RenderSystems.Setup(dirtyRendering);
				dirtyRendering = 0;
			}
			if (simulateWhenInvisible || isVisible)
			{
				m_RenderSystems.Render();
			}
		}

		private void UpdateBounds()
		{
			if (initialized)
			{
				PushActiveParticles();
				PushSimplices();
				simulationHandle = UpdateTransformFrame(0f);
				simulationHandle = implementation.UpdateBounds(simulationHandle, 0f);
				simulationHandle?.Complete();
			}
		}

		private void RequestReadback()
		{
			if (!initialized)
			{
				return;
			}
			this.OnRequestReadback?.Invoke(this);
			foreach (ObiActor actor in actors)
			{
				actor.RequestReadback();
			}
			implementation.RequestReadback();
			if (this.OnCollision != null)
			{
				colliderContacts.Readback();
			}
			if (this.OnParticleCollision != null)
			{
				particleContacts.Readback();
			}
			if (this.OnAdvection != null)
			{
				foamPositions.Readback();
				foamVelocities.Readback();
				foamAttributes.Readback();
				foamColors.Readback();
				foamCount.Readback();
			}
		}

		public bool AddActor(ObiActor actor)
		{
			if (actor == null || actors == null || actor.sourceBlueprint == null || actor.sourceBlueprint.empty || actors.Contains(actor) || addBuffer.Contains(actor))
			{
				return false;
			}
			if (!Application.isPlaying)
			{
				InsertBufferedActor(actor);
			}
			else
			{
				addBuffer.Enqueue(actor);
			}
			return true;
		}

		public bool RemoveActor(ObiActor actor)
		{
			if (actor == null)
			{
				return false;
			}
			addBuffer = new Queue<ObiActor>(addBuffer.Where((ObiActor s) => s != actor));
			int num = actors.IndexOf(actor);
			if (num >= 0)
			{
				actor.UnloadBlueprint();
				for (int num2 = 0; num2 < actor.solverIndices.count; num2++)
				{
					particleToActor[actor.solverIndices[num2]] = null;
				}
				FreeParticles(actor.solverIndices);
				freeGroupIDs.Push(actor.groupID);
				actors.RemoveAt(num);
				actor.solverIndices.Dispose();
				actor.solverIndices = null;
				for (int num3 = 0; num3 < actor.solverBatchOffsets.Length; num3++)
				{
					actor.solverBatchOffsets[num3].Clear();
				}
				if (actors.Count == 0)
				{
					Teardown();
				}
				return true;
			}
			return false;
		}

		private void InsertBufferedActor(ObiActor actor)
		{
			if (!(actor == null))
			{
				Initialize();
				if (actor.solverIndices == null)
				{
					actor.solverIndices = new ObiNativeIntList();
				}
				actor.solverIndices.ResizeUninitialized(actor.sourceBlueprint.particleCount);
				AllocateParticles(actor.solverIndices);
				for (int i = 0; i < actor.solverIndices.count; i++)
				{
					particleToActor[actor.solverIndices[i]] = new ParticleInActor(actor, i);
				}
				actors.Add(actor);
				if (freeGroupIDs.Count == 0)
				{
					freeGroupIDs.Push(actors.Count);
				}
				actor.groupID = freeGroupIDs.Pop();
				actor.LoadBlueprint();
				implementation.ParticleCountChanged(this);
				this.OnParticleCountChanged?.Invoke(this);
			}
		}

		public void PushSolverParameters()
		{
			if (initialized)
			{
				implementation.SetParameters(parameters);
				implementation.SetConstraintGroupParameters(Oni.ConstraintType.Distance, ref distanceConstraintParameters);
				implementation.SetConstraintGroupParameters(Oni.ConstraintType.Bending, ref bendingConstraintParameters);
				implementation.SetConstraintGroupParameters(Oni.ConstraintType.ParticleCollision, ref particleCollisionConstraintParameters);
				implementation.SetConstraintGroupParameters(Oni.ConstraintType.ParticleFriction, ref particleFrictionConstraintParameters);
				implementation.SetConstraintGroupParameters(Oni.ConstraintType.Collision, ref collisionConstraintParameters);
				implementation.SetConstraintGroupParameters(Oni.ConstraintType.Friction, ref frictionConstraintParameters);
				implementation.SetConstraintGroupParameters(Oni.ConstraintType.Density, ref densityConstraintParameters);
				implementation.SetConstraintGroupParameters(Oni.ConstraintType.Skin, ref skinConstraintParameters);
				implementation.SetConstraintGroupParameters(Oni.ConstraintType.Volume, ref volumeConstraintParameters);
				implementation.SetConstraintGroupParameters(Oni.ConstraintType.ShapeMatching, ref shapeMatchingConstraintParameters);
				implementation.SetConstraintGroupParameters(Oni.ConstraintType.Tether, ref tetherConstraintParameters);
				implementation.SetConstraintGroupParameters(Oni.ConstraintType.Pin, ref pinConstraintParameters);
				implementation.SetConstraintGroupParameters(Oni.ConstraintType.Pinhole, ref pinholeConstraintParameters);
				implementation.SetConstraintGroupParameters(Oni.ConstraintType.Stitch, ref stitchConstraintParameters);
				implementation.SetConstraintGroupParameters(Oni.ConstraintType.StretchShear, ref stretchShearConstraintParameters);
				implementation.SetConstraintGroupParameters(Oni.ConstraintType.BendTwist, ref bendTwistConstraintParameters);
				implementation.SetConstraintGroupParameters(Oni.ConstraintType.Chain, ref chainConstraintParameters);
				if (this.OnUpdateParameters != null)
				{
					this.OnUpdateParameters(this);
				}
			}
		}

		public Oni.ConstraintParameters GetConstraintParameters(Oni.ConstraintType constraintType)
		{
			return constraintType switch
			{
				Oni.ConstraintType.Distance => distanceConstraintParameters, 
				Oni.ConstraintType.Bending => bendingConstraintParameters, 
				Oni.ConstraintType.ParticleCollision => particleCollisionConstraintParameters, 
				Oni.ConstraintType.ParticleFriction => particleFrictionConstraintParameters, 
				Oni.ConstraintType.Collision => collisionConstraintParameters, 
				Oni.ConstraintType.Friction => frictionConstraintParameters, 
				Oni.ConstraintType.Skin => skinConstraintParameters, 
				Oni.ConstraintType.Volume => volumeConstraintParameters, 
				Oni.ConstraintType.ShapeMatching => shapeMatchingConstraintParameters, 
				Oni.ConstraintType.Tether => tetherConstraintParameters, 
				Oni.ConstraintType.Pin => pinConstraintParameters, 
				Oni.ConstraintType.Pinhole => pinholeConstraintParameters, 
				Oni.ConstraintType.Stitch => stitchConstraintParameters, 
				Oni.ConstraintType.Density => densityConstraintParameters, 
				Oni.ConstraintType.StretchShear => stretchShearConstraintParameters, 
				Oni.ConstraintType.BendTwist => bendTwistConstraintParameters, 
				Oni.ConstraintType.Chain => chainConstraintParameters, 
				_ => new Oni.ConstraintParameters(enabled: true, Oni.ConstraintParameters.EvaluationOrder.Sequential, 1), 
			};
		}

		public IObiConstraints GetConstraintsByType(Oni.ConstraintType type)
		{
			if (m_Constraints != null && type >= Oni.ConstraintType.Tether && (int)type < m_Constraints.Length)
			{
				return m_Constraints[(int)type];
			}
			return null;
		}

		private void PushActiveParticles()
		{
			if (!dirtyActiveParticles)
			{
				return;
			}
			using (m_PushActiveParticles.Auto())
			{
				activeParticles.Clear();
				for (int i = 0; i < actors.Count; i++)
				{
					if (actors[i].isActiveAndEnabled)
					{
						activeParticles.AddRange(actors[i].solverIndices, actors[i].activeParticleCount);
					}
				}
				implementation.SetActiveParticles(activeParticles);
				dirtyActiveParticles = false;
			}
		}

		private void PushDeformableTriangles()
		{
			if (!dirtyDeformableTriangles)
			{
				return;
			}
			using (m_PushDeformableTriangles.Auto())
			{
				deformableTriangles.Clear();
				deformableUVs.Clear();
				for (int i = 0; i < actors.Count; i++)
				{
					ObiActor obiActor = actors[i];
					if (obiActor.isActiveAndEnabled)
					{
						obiActor.ProvideDeformableTriangles(deformableTriangles, deformableUVs);
					}
				}
				implementation.SetDeformableTriangles(deformableTriangles, deformableUVs);
				dirtyDeformableTriangles = false;
			}
		}

		private void PushDeformableEdges()
		{
			if (!dirtyDeformableEdges)
			{
				return;
			}
			using (m_PushDeformableEdges.Auto())
			{
				deformableEdges.Clear();
				for (int i = 0; i < actors.Count; i++)
				{
					ObiActor obiActor = actors[i];
					if (obiActor.isActiveAndEnabled)
					{
						obiActor.ProvideDeformableEdges(deformableEdges);
					}
				}
				implementation.SetDeformableEdges(deformableEdges);
				dirtyDeformableEdges = false;
			}
		}

		private void PushSimplices()
		{
			if (dirtySimplices == Oni.SimplexType.None)
			{
				return;
			}
			using (m_PushSimplices.Auto())
			{
				simplices.Clear();
				if ((dirtySimplices & Oni.SimplexType.Point) != Oni.SimplexType.None)
				{
					points.Clear();
				}
				if ((dirtySimplices & Oni.SimplexType.Edge) != Oni.SimplexType.None)
				{
					edges.Clear();
				}
				if ((dirtySimplices & Oni.SimplexType.Triangle) != Oni.SimplexType.None)
				{
					triangles.Clear();
				}
				for (int i = 0; i < actors.Count; i++)
				{
					ObiActor obiActor = actors[i];
					if (!obiActor.isActiveAndEnabled || !obiActor.isLoaded)
					{
						continue;
					}
					if (obiActor.surfaceCollisions)
					{
						if (obiActor.sharedBlueprint.points != null && (dirtySimplices & Oni.SimplexType.Point) != Oni.SimplexType.None)
						{
							for (int j = 0; j < obiActor.sharedBlueprint.points.Length; j++)
							{
								int num = obiActor.sharedBlueprint.points[j];
								if (num < obiActor.activeParticleCount)
								{
									points.Add(obiActor.solverIndices[num]);
								}
							}
						}
						if (obiActor.sharedBlueprint.edges != null && (dirtySimplices & Oni.SimplexType.Edge) != Oni.SimplexType.None)
						{
							for (int k = 0; k < obiActor.sharedBlueprint.edges.Length / 2; k++)
							{
								int num2 = obiActor.sharedBlueprint.edges[k * 2];
								int num3 = obiActor.sharedBlueprint.edges[k * 2 + 1];
								if (num2 < obiActor.activeParticleCount && num3 < obiActor.activeParticleCount)
								{
									edges.Add(obiActor.solverIndices[num2]);
									edges.Add(obiActor.solverIndices[num3]);
								}
							}
						}
						if (obiActor.sharedBlueprint.triangles == null || (dirtySimplices & Oni.SimplexType.Triangle) == 0)
						{
							continue;
						}
						for (int l = 0; l < obiActor.sharedBlueprint.triangles.Length / 3; l++)
						{
							int num4 = obiActor.sharedBlueprint.triangles[l * 3];
							int num5 = obiActor.sharedBlueprint.triangles[l * 3 + 1];
							int num6 = obiActor.sharedBlueprint.triangles[l * 3 + 2];
							if (num4 < obiActor.activeParticleCount && num5 < obiActor.activeParticleCount && num6 < obiActor.activeParticleCount)
							{
								triangles.Add(obiActor.solverIndices[num4]);
								triangles.Add(obiActor.solverIndices[num5]);
								triangles.Add(obiActor.solverIndices[num6]);
							}
						}
					}
					else if ((dirtySimplices & Oni.SimplexType.Point) != Oni.SimplexType.None)
					{
						points.AddRange(obiActor.solverIndices, obiActor.activeParticleCount);
					}
				}
				simplices.EnsureCapacity(points.count + edges.count + triangles.count);
				simplices.AddRange(triangles);
				simplices.AddRange(edges);
				simplices.AddRange(points);
				m_SimplexCounts = new SimplexCounts(points.count, edges.count / 2, triangles.count / 3);
				cellCoords.ResizeInitialized(m_SimplexCounts.simplexCount);
				implementation.SetSimplices(simplices, m_SimplexCounts);
				dirtySimplices = Oni.SimplexType.None;
			}
		}

		private void PushConstraints()
		{
			if (dirtyConstraints == 0)
			{
				return;
			}
			for (int i = 0; i < 18; i++)
			{
				if (m_Constraints[i] != null && ((1 << i) & dirtyConstraints) != 0)
				{
					m_Constraints[i].Clear();
				}
			}
			for (int j = 0; j < actors.Count; j++)
			{
				if (!actors[j].isLoaded)
				{
					continue;
				}
				for (int k = 0; k < 18; k++)
				{
					if (m_Constraints[k] != null && ((1 << k) & dirtyConstraints) != 0)
					{
						IObiConstraints constraintsByType = actors[j].GetConstraintsByType((Oni.ConstraintType)k);
						m_Constraints[k].Merge(actors[j], constraintsByType);
					}
				}
			}
			for (int l = 0; l < 18; l++)
			{
				if (m_Constraints[l] != null && ((1 << l) & dirtyConstraints) != 0)
				{
					m_Constraints[l].AddToSolver(this);
				}
			}
			dirtyConstraints = 0;
		}

		private void UpdateVisibility()
		{
			using (m_UpdateVisibilityPerfMarker.Auto())
			{
				using (m_GetSolverBoundsPerfMarker.Auto())
				{
					Vector3 min = Vector3.zero;
					Vector3 max = Vector3.zero;
					implementation.GetBounds(ref min, ref max);
					m_Bounds.SetMinMax(min, max);
				}
				if (m_Bounds.AreValid())
				{
					using (m_TestBoundsPerfMarker.Auto())
					{
						m_BoundsWS = m_Bounds.Transform(base.transform.localToWorldMatrix);
						using (m_GetAllCamerasPerfMarker.Auto())
						{
							Array.Resize(ref sceneCameras, Camera.allCamerasCount);
							Camera.GetAllCameras(sceneCameras);
						}
						Camera[] array = sceneCameras;
						for (int i = 0; i < array.Length; i++)
						{
							GeometryUtility.CalculateFrustumPlanes(array[i], planes);
							if (!GeometryUtility.TestPlanesAABB(planes, m_BoundsWS))
							{
								continue;
							}
							if (isVisible)
							{
								return;
							}
							isVisible = true;
							{
								foreach (ObiActor actor in actors)
								{
									actor.OnSolverVisibilityChanged(isVisible);
								}
								return;
							}
						}
					}
				}
				if (!isVisible)
				{
					return;
				}
				isVisible = false;
				foreach (ObiActor actor2 in actors)
				{
					actor2.OnSolverVisibilityChanged(isVisible);
				}
			}
		}

		private void InitializeTransformFrame()
		{
			Vector4 translation = base.transform.position;
			Vector4 scale = base.transform.lossyScale;
			Quaternion rotation = base.transform.rotation;
			implementation.InitializeFrame(translation, scale, rotation);
		}

		private IObiJobHandle UpdateTransformFrame(float dt)
		{
			Vector4 translation = base.transform.position;
			Vector4 scale = base.transform.lossyScale;
			Quaternion rotation = base.transform.rotation;
			implementation.UpdateFrame(translation, scale, rotation, dt);
			return implementation.ApplyFrame(worldLinearInertiaScale, worldAngularInertiaScale, dt);
		}

		public void RegisterRenderSystem(IRenderSystem renderSystem)
		{
			m_RenderSystems.RegisterRenderSystem(renderSystem);
		}

		public void UnregisterRenderSystem(IRenderSystem renderSystem)
		{
			m_RenderSystems.UnregisterRenderSystem(renderSystem);
		}

		public RenderSystem<T> GetRenderSystem<T>() where T : ObiRenderer<T>
		{
			return m_RenderSystems.GetRenderSystem<T>();
		}

		public IRenderSystem GetRenderSystem(Oni.RenderingSystemType type)
		{
			return m_RenderSystems.GetRenderSystem(type);
		}

		public int EnqueueSpatialQuery(QueryShape shape, AffineTransform transform)
		{
			if (!initialized)
			{
				return -1;
			}
			int count = bufferedQueryShapes.count;
			bufferedQueryShapes.Add(shape);
			bufferedQueryTransforms.Add(transform);
			return count;
		}

		public int EnqueueSpatialQueries(ObiNativeQueryShapeList shapes, ObiNativeAffineTransformList transforms)
		{
			if (!initialized || shapes == null || transforms == null || shapes.count != transforms.count)
			{
				return -1;
			}
			int count = bufferedQueryShapes.count;
			bufferedQueryShapes.AddRange(shapes);
			bufferedQueryTransforms.AddRange(transforms);
			return count;
		}

		public int EnqueueRaycast(Ray ray, int filter, float maxDistance = 100f, float rayThickness = 0f)
		{
			if (!initialized)
			{
				return -1;
			}
			int count = bufferedQueryShapes.count;
			bufferedQueryShapes.Add(new QueryShape
			{
				type = QueryShape.QueryType.Ray,
				center = ray.origin,
				size = ray.direction * maxDistance,
				contactOffset = rayThickness,
				maxDistance = 0.0001f,
				filter = filter
			});
			bufferedQueryTransforms.Add(new AffineTransform(Vector4.zero, Quaternion.identity, Vector4.one));
			return count;
		}
	}
}
