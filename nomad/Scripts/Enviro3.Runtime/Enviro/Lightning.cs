using System.Collections;
using UnityEngine;

namespace Enviro
{
	public class Lightning : MonoBehaviour, ILightningEffect
	{
		public float flashIntensity = 50f;

		public Vector3 target;

		private LineRenderer lineRend;

		public Light myLight;

		public Material planeMat;

		public int arcs = 20;

		public float arcLength = 100f;

		public float arcVariation = 1f;

		public float inaccuracy = 0.5f;

		public int splits = 4;

		public int maxSplits = 24;

		private int splitCount;

		public float splitLength = 100f;

		public float splitVariation = 1f;

		public Vector3 toTarget;

		private bool fadeOut;

		private float fadeTimer;

		public void CastBolt(Vector3 origin, Vector3 target)
		{
			base.transform.position = origin;
			this.target = target;
			CastBolt();
		}

		private void OnEnable()
		{
			lineRend = base.gameObject.GetComponent<LineRenderer>();
		}

		private IEnumerator CreateLightningBolt()
		{
			myLight.enabled = false;
			lineRend.widthMultiplier = 10f;
			planeMat.SetFloat("_Brightness", 1f);
			lineRend.SetPosition(0, base.transform.position);
			lineRend.positionCount = 2;
			lineRend.SetPosition(1, base.transform.position);
			Vector3 vector = base.transform.position;
			float num = Vector3.Distance(base.transform.position, target);
			float arcDist = num / (float)arcs;
			for (int i = 1; i < arcs; i++)
			{
				planeMat.SetFloat("_Brightness", Random.Range(0f, 2f));
				lineRend.positionCount = i + 1;
				Vector3 newVector = target - vector;
				newVector.Normalize();
				Vector3 pos = Randomize(newVector, inaccuracy);
				pos *= Random.Range(arcLength * arcVariation, arcLength) * arcDist;
				pos += vector;
				lineRend.SetPosition(i, pos);
				if (i % 2 == 0)
				{
					for (int s = 0; s <= splits; s++)
					{
						if (splitCount < maxSplits)
						{
							StartCoroutine(CreateSplit(pos, target));
							yield return new WaitForSeconds(Random.Range(0.0001f, 0.0002f));
						}
					}
				}
				vector = pos;
			}
			yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
			lineRend.SetPosition(arcs - 1, target);
			myLight.transform.position = target;
			if (EnviroManager.instance.Camera != null)
			{
				myLight.transform.LookAt(EnviroManager.instance.Camera.transform.position, Vector3.up);
			}
			lineRend.material.SetFloat("_Brightness", flashIntensity);
			planeMat.SetFloat("_Brightness", 20f);
			myLight.enabled = true;
			yield return new WaitForSeconds(Random.Range(0.025f, 0.035f));
			lineRend.material.SetFloat("_Brightness", 1f);
			planeMat.SetFloat("_Brightness", 1f);
			myLight.enabled = false;
			yield return new WaitForSeconds(Random.Range(0.025f, 0.035f));
			lineRend.material.SetFloat("_Brightness", flashIntensity);
			planeMat.SetFloat("_Brightness", 20f);
			myLight.enabled = true;
			yield return new WaitForSeconds(Random.Range(0.025f, 0.035f));
			lineRend.material.SetFloat("_Brightness", 1f);
			planeMat.SetFloat("_Brightness", 1f);
			myLight.enabled = false;
			yield return new WaitForSeconds(Random.Range(0.025f, 0.035f));
			lineRend.material.SetFloat("_Brightness", flashIntensity);
			planeMat.SetFloat("_Brightness", 0f);
			myLight.enabled = true;
			yield return new WaitForSeconds(Random.Range(0.025f, 0.035f));
			myLight.enabled = false;
			fadeTimer = 50f;
			fadeOut = true;
		}

		private IEnumerator CreateSplit(Vector3 pos, Vector3 targetP)
		{
			splitCount++;
			GameObject split = new GameObject();
			split.transform.SetParent(base.transform);
			split.transform.position = pos;
			LineRenderer lineRenderer = split.AddComponent<LineRenderer>();
			lineRenderer.material = lineRend.material;
			lineRenderer.material.SetFloat("_Brightness", flashIntensity * 0.5f);
			lineRenderer.positionCount = 2;
			lineRenderer.SetPosition(0, split.transform.position);
			lineRenderer.SetPosition(1, split.transform.position);
			toTarget = targetP - pos;
			toTarget = Vector3.Normalize(toTarget);
			Vector3 vector = Random.insideUnitSphere * 500f + pos + toTarget * 1000f;
			Vector3 vector2 = split.transform.position;
			float num = Vector3.Distance(split.transform.position, vector) / 32f;
			for (int i = 1; i < 32; i++)
			{
				lineRenderer.positionCount = i + 1;
				Vector3 newVector = vector - vector2;
				newVector.Normalize();
				Vector3 vector3 = Randomize(newVector, inaccuracy);
				vector3 *= Random.Range(splitLength * splitVariation, splitLength) * num;
				vector3 += vector2;
				lineRenderer.SetPosition(i, vector3);
				vector2 = vector3;
			}
			yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));
			Object.DestroyImmediate(split);
		}

		public void CastBolt()
		{
			lineRend.positionCount = 1;
			StartCoroutine(CreateLightningBolt());
		}

		private Vector3 Randomize(Vector3 newVector, float devation)
		{
			newVector += new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * devation;
			newVector.Normalize();
			return newVector;
		}

		private void Update()
		{
			if (fadeOut)
			{
				fadeTimer = Mathf.Lerp(fadeTimer, 0f, 10f * Time.deltaTime);
				lineRend.material.SetFloat("_Brightness", fadeTimer);
				if (fadeTimer <= 1f)
				{
					lineRend.positionCount = 1;
					fadeOut = false;
					Object.DestroyImmediate(base.gameObject);
				}
			}
		}
	}
}
