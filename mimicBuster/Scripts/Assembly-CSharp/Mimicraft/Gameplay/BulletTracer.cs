using UnityEngine;
using UnityEngine.Rendering;

namespace Mimicraft.Gameplay
{
	public class BulletTracer : MonoBehaviour
	{
		private LineRenderer line;

		private Vector3 start;

		private Vector3 end;

		private float travelled;

		private float speed;

		private float length;

		private static Material fallbackMaterial;

		public static void Spawn(Vector3 from, Vector3 to)
		{
			EffectLibrary instance = EffectLibrary.Instance;
			if ((instance != null && !instance.TracersEnabled) || (to - from).sqrMagnitude < 0.01f)
			{
				return;
			}
			GameObject gameObject = ((instance != null) ? instance.TracerPrefab : null);
			GameObject gameObject2 = ((gameObject != null) ? Object.Instantiate(gameObject) : BuildFallback(instance));
			if (!(gameObject2 == null))
			{
				ImpactEffects.Adopt(gameObject2);
				BulletTracer bulletTracer = gameObject2.GetComponent<BulletTracer>();
				if (bulletTracer == null)
				{
					bulletTracer = gameObject2.AddComponent<BulletTracer>();
				}
				bulletTracer.Launch(from, to, (instance != null) ? instance.TracerSpeed : 250f, (instance != null) ? instance.TracerLength : 1.5f);
			}
		}

		private static GameObject BuildFallback(EffectLibrary library)
		{
			GameObject obj = new GameObject("Tracer");
			LineRenderer lineRenderer = obj.AddComponent<LineRenderer>();
			lineRenderer.useWorldSpace = true;
			lineRenderer.positionCount = 2;
			lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
			lineRenderer.receiveShadows = false;
			lineRenderer.alignment = LineAlignment.View;
			lineRenderer.textureMode = LineTextureMode.Stretch;
			float num = (lineRenderer.startWidth = ((library != null) ? library.TracerWidth : 0.02f));
			lineRenderer.endWidth = num * 0.4f;
			Color color = (lineRenderer.startColor = ((library != null) ? library.TracerColor : new Color(1f, 0.85f, 0.5f, 1f)));
			lineRenderer.endColor = new Color(color.r, color.g, color.b, 0f);
			if (fallbackMaterial == null)
			{
				Shader shader = Shader.Find("Universal Render Pipeline/Particles/Unlit") ?? Shader.Find("Sprites/Default");
				fallbackMaterial = ((shader != null) ? new Material(shader) : null);
			}
			if (fallbackMaterial != null)
			{
				lineRenderer.material = fallbackMaterial;
			}
			return obj;
		}

		private void Launch(Vector3 from, Vector3 to, float travelSpeed, float streakLength)
		{
			line = GetComponent<LineRenderer>();
			start = from;
			end = to;
			speed = Mathf.Max(1f, travelSpeed);
			length = Mathf.Max(0.05f, streakLength);
			travelled = 0f;
			if (line != null)
			{
				line.useWorldSpace = true;
				line.positionCount = 2;
				line.SetPosition(0, from);
				line.SetPosition(1, from);
			}
			float num = (to - from).magnitude + length;
			Object.Destroy(base.gameObject, num / speed + 0.5f);
		}

		private void Update()
		{
			if (line == null)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			travelled += speed * Time.deltaTime;
			Vector3 vector = end - start;
			float magnitude = vector.magnitude;
			if (magnitude < 0.001f)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			Vector3 vector2 = vector / magnitude;
			float num = Mathf.Min(travelled, magnitude);
			float num2 = Mathf.Clamp(travelled - length, 0f, magnitude);
			line.SetPosition(0, start + vector2 * num2);
			line.SetPosition(1, start + vector2 * num);
			if (num2 >= magnitude)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}
}
