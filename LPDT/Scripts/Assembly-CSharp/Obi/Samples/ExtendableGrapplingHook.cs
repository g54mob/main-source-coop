using System.Collections;
using UnityEngine;

namespace Obi.Samples
{
	public class ExtendableGrapplingHook : MonoBehaviour
	{
		public ObiSolver solver;

		public ObiCollider character;

		public Material material;

		public ObiRopeSection section;

		[Range(0f, 1f)]
		public float hookResolution = 0.5f;

		public float hookExtendRetractSpeed = 2f;

		public float hookShootSpeed = 30f;

		public int particlePoolSize = 100;

		private ObiRope rope;

		private ObiRopeBlueprint blueprint;

		private ObiRopeExtrudedRenderer ropeRenderer;

		private ObiRopeCursor cursor;

		private RaycastHit hookAttachment;

		private void Awake()
		{
			rope = base.gameObject.AddComponent<ObiRope>();
			ropeRenderer = base.gameObject.AddComponent<ObiRopeExtrudedRenderer>();
			ropeRenderer.section = section;
			ropeRenderer.uvScale = new Vector2(1f, 4f);
			ropeRenderer.normalizeV = false;
			ropeRenderer.uvAnchor = 1f;
			ropeRenderer.material = material;
			blueprint = ScriptableObject.CreateInstance<ObiRopeBlueprint>();
			blueprint.resolution = 0.5f;
			blueprint.pooledParticles = particlePoolSize;
			rope.maxBending = 0.02f;
			cursor = rope.gameObject.AddComponent<ObiRopeCursor>();
			cursor.cursorMu = 0f;
			cursor.direction = true;
		}

		private void OnDestroy()
		{
			Object.DestroyImmediate(blueprint);
		}

		private void LaunchHook()
		{
			Vector3 mousePosition = Input.mousePosition;
			mousePosition.z = base.transform.position.z - Camera.main.transform.position.z;
			Vector3 vector = Camera.main.ScreenToWorldPoint(mousePosition);
			if (Physics.Raycast(new Ray(base.transform.position, vector - base.transform.position), out hookAttachment))
			{
				StartCoroutine(AttachHook());
			}
		}

		private void LayParticlesInStraightLine(Vector3 origin, Vector3 direction)
		{
			float num = 0f;
			for (int i = 0; i < rope.elements.Count; i++)
			{
				int particle = rope.elements[i].particle1;
				int particle2 = rope.elements[i].particle2;
				ObiNativeVector4List prevPositions = solver.prevPositions;
				Vector4 value = (solver.positions[particle] = origin + direction * num);
				prevPositions[particle] = value;
				num += rope.elements[i].restLength;
				ObiNativeVector4List prevPositions2 = solver.prevPositions;
				value = (solver.positions[particle2] = origin + direction * num);
				prevPositions2[particle2] = value;
			}
		}

		private IEnumerator AttachHook()
		{
			yield return null;
			ObiConstraints<ObiPinConstraintsBatch> pinConstraints = rope.GetConstraintsByType(Oni.ConstraintType.Pin) as ObiConstraints<ObiPinConstraintsBatch>;
			pinConstraints.Clear();
			Vector3 vector = rope.transform.InverseTransformPoint(hookAttachment.point);
			int filter = ObiUtils.MakeFilter(65535, 0);
			blueprint.path.Clear();
			blueprint.path.AddControlPoint(Vector3.zero, Vector3.zero, Vector3.zero, Vector3.up, 0.1f, 0.1f, 1f, filter, Color.white, "Hook start");
			blueprint.path.AddControlPoint(vector.normalized * 0.5f, Vector3.zero, Vector3.zero, Vector3.up, 0.1f, 0.1f, 1f, filter, Color.white, "Hook end");
			blueprint.path.FlushEvents();
			yield return blueprint.Generate();
			rope.ropeBlueprint = blueprint;
			rope.GetComponent<ObiRopeExtrudedRenderer>().enabled = true;
			yield return new WaitForFixedUpdate();
			yield return null;
			for (int i = 0; i < rope.activeParticleCount; i++)
			{
				solver.invMasses[rope.solverIndices[i]] = 0f;
			}
			Vector3 origin;
			Vector3 direction;
			float num;
			while (true)
			{
				origin = solver.transform.InverseTransformPoint(rope.transform.position);
				direction = solver.transform.InverseTransformPoint(hookAttachment.point) - origin;
				float magnitude = direction.magnitude;
				direction.Normalize();
				LayParticlesInStraightLine(origin, direction);
				num = magnitude - cursor.ChangeLength(hookShootSpeed * Time.deltaTime);
				if (num < 0f)
				{
					break;
				}
				yield return null;
			}
			cursor.ChangeLength(num);
			yield return new WaitForFixedUpdate();
			yield return null;
			LayParticlesInStraightLine(origin, direction);
			for (int j = 0; j < rope.activeParticleCount; j++)
			{
				solver.invMasses[rope.solverIndices[j]] = 10f;
			}
			ObiPinConstraintsBatch obiPinConstraintsBatch = new ObiPinConstraintsBatch();
			obiPinConstraintsBatch.AddConstraint(rope.elements[0].particle1, character, base.transform.localPosition, Quaternion.identity, 0f, 0f);
			obiPinConstraintsBatch.AddConstraint(rope.elements[rope.elements.Count - 1].particle2, hookAttachment.collider.GetComponent<ObiColliderBase>(), hookAttachment.collider.transform.InverseTransformPoint(hookAttachment.point), Quaternion.identity, 0f, 0f);
			obiPinConstraintsBatch.activeConstraintCount = 2;
			pinConstraints.AddBatch(obiPinConstraintsBatch);
			rope.SetConstraintsDirty(Oni.ConstraintType.Pin);
		}

		private void DetachHook()
		{
			rope.ropeBlueprint = null;
			rope.GetComponent<ObiRopeExtrudedRenderer>().enabled = false;
		}

		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (!rope.isLoaded)
				{
					LaunchHook();
				}
				else
				{
					DetachHook();
				}
			}
			if (rope.isLoaded)
			{
				if (Input.GetKey(KeyCode.W))
				{
					cursor.ChangeLength((0f - hookExtendRetractSpeed) * Time.deltaTime);
				}
				if (Input.GetKey(KeyCode.S))
				{
					cursor.ChangeLength(hookExtendRetractSpeed * Time.deltaTime);
				}
			}
		}
	}
}
