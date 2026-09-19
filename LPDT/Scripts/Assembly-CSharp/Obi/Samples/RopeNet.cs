using UnityEngine;

namespace Obi.Samples
{
	public class RopeNet : MonoBehaviour
	{
		public Material material;

		public Vector2Int resolution = new Vector2Int(5, 5);

		public Vector2 size = new Vector2(0.5f, 0.5f);

		public float nodeSize = 0.2f;

		private void Awake()
		{
			ObiSolver component = new GameObject("solver", typeof(ObiSolver)).GetComponent<ObiSolver>();
			component.substeps = 2;
			component.particleCollisionConstraintParameters.enabled = false;
			component.distanceConstraintParameters.iterations = 8;
			component.pinConstraintParameters.iterations = 4;
			component.parameters.sleepThreshold = 0.001f;
			component.PushSolverParameters();
			CreateNet(component);
		}

		private void CreateNet(ObiSolver solver)
		{
			ObiCollider[,] array = new ObiCollider[resolution.x + 1, resolution.y + 1];
			for (int i = 0; i <= resolution.x; i++)
			{
				for (int j = 0; j <= resolution.y; j++)
				{
					GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
					gameObject.transform.position = new Vector3(i, j, 0f) * size;
					gameObject.transform.localScale = new Vector3(nodeSize, nodeSize, nodeSize);
					gameObject.AddComponent<Rigidbody>();
					array[i, j] = gameObject.AddComponent<ObiCollider>();
					array[i, j].Filter = 1;
				}
			}
			array[0, resolution.y].GetComponent<Rigidbody>().isKinematic = true;
			array[resolution.x, resolution.y].GetComponent<Rigidbody>().isKinematic = true;
			for (int k = 0; k <= resolution.x; k++)
			{
				for (int l = 0; l <= resolution.y; l++)
				{
					Vector3 vector = new Vector3(k, l, 0f) * size;
					if (k < resolution.x)
					{
						Vector3 vector2 = new Vector3(nodeSize * 0.5f, 0f, 0f);
						ObiRope obiRope = CreateRope(vector + vector2, vector + new Vector3(size.x, 0f, 0f) - vector2);
						obiRope.transform.parent = solver.transform;
						PinRope(obiRope, array[k, l], array[k + 1, l]);
					}
					if (l < resolution.y)
					{
						Vector3 vector3 = new Vector3(0f, nodeSize * 0.5f, 0f);
						ObiRope obiRope2 = CreateRope(vector + vector3, vector + new Vector3(0f, size.y, 0f) - vector3);
						obiRope2.transform.parent = solver.transform;
						PinRope(obiRope2, array[k, l], array[k, l + 1]);
					}
				}
			}
		}

		private void PinRope(ObiRope rope, ObiCollider bodyA, ObiCollider bodyB)
		{
			ObiParticleAttachment obiParticleAttachment = rope.gameObject.AddComponent<ObiParticleAttachment>();
			ObiParticleAttachment obiParticleAttachment2 = rope.gameObject.AddComponent<ObiParticleAttachment>();
			obiParticleAttachment.attachmentType = ObiParticleAttachment.AttachmentType.Dynamic;
			obiParticleAttachment2.attachmentType = ObiParticleAttachment.AttachmentType.Dynamic;
			obiParticleAttachment.target = bodyA.transform;
			obiParticleAttachment2.target = bodyB.transform;
			obiParticleAttachment.particleGroup = rope.ropeBlueprint.groups[0];
			obiParticleAttachment2.particleGroup = rope.ropeBlueprint.groups[1];
		}

		private ObiRope CreateRope(Vector3 pointA, Vector3 pointB)
		{
			GameObject obj = new GameObject("solver", typeof(ObiRope), typeof(ObiRopeLineRenderer));
			ObiRope component = obj.GetComponent<ObiRope>();
			ObiRopeLineRenderer component2 = obj.GetComponent<ObiRopeLineRenderer>();
			component.GetComponent<ObiRopeLineRenderer>().material = material;
			component.GetComponent<ObiPathSmoother>().decimation = 0.1f;
			component2.uvScale = new Vector2(1f, 5f);
			ObiRopeBlueprint obiRopeBlueprint = ScriptableObject.CreateInstance<ObiRopeBlueprint>();
			obiRopeBlueprint.resolution = 0.15f;
			obiRopeBlueprint.thickness = 0.02f;
			obiRopeBlueprint.pooledParticles = 0;
			pointA = component.transform.InverseTransformPoint(pointA);
			pointB = component.transform.InverseTransformPoint(pointB);
			Vector3 vector = (pointB - pointA) * 0.25f;
			obiRopeBlueprint.path.Clear();
			obiRopeBlueprint.path.AddControlPoint(pointA, -vector, vector, Vector3.up, 0.1f, 0.1f, 1f, 1, Color.white, "A");
			obiRopeBlueprint.path.AddControlPoint(pointB, -vector, vector, Vector3.up, 0.1f, 0.1f, 1f, 1, Color.white, "B");
			obiRopeBlueprint.path.FlushEvents();
			component.ropeBlueprint = obiRopeBlueprint;
			return component;
		}
	}
}
