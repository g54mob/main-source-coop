using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace NomadDrive.Features.Rope.Scripts
{
	[ExecuteInEditMode]
	public class RigCamera : MonoBehaviour
	{
		public GameObject target;

		public float distance = 10f;

		public float xSpeed = 2500f;

		public float ySpeed = 1200f;

		public float zSpeed = 10f;

		public float yMinLimit = -20f;

		public float yMaxLimit = 80f;

		public float xMinLimit = -20f;

		public float xMaxLimit = 20f;

		public Vector3 offset;

		public float trantime = 4f;

		public float nx;

		public float ny;

		public float nz;

		public float delay = 0.24f;

		public float delayz = 0.24f;

		private float x;

		private float y;

		private float vx;

		private float vy;

		private float vz;

		private float t;

		private Vector3 tpos;

		private MeshRenderer render;

		private SkinnedMeshRenderer srender;

		private MeshFilter filter;

		public float shakeAmt = 1f;

		private DepthOfField dof;

		public PostProcessProfile postprocess;

		private bool havedof;

		private GameObject[] targets;

		private int currentIndex;

		private Vector3 newpos = Vector3.zero;

		public AnimationCurve crv = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

		private float cdofdist;

		private float currentDistance;

		private float dz;

		private float dofdistance;

		public Gradient coltest;

		public Material groundMat;

		public Color[] colors;

		public float ctime = 1f;

		public float ct = 10f;

		public Color currentCol;

		public bool slowfade;

		[Range(0f, 1f)]
		public float fogLerp = 0.25f;

		[Range(0f, 1f)]
		public float ambientLerp = 0.75f;

		public float fadetime = 20f;

		private int lastindex;

		public int colIndex;

		public bool doRaycast;

		public float speed = 1f;

		public bool lookat;

		private float fov;

		private float cfov;

		private float fovvel;

		private float startfov;

		private Vector3 mvel;

		private bool firstrun;

		public float dofdamp = 0.25f;

		public GameObject controlsOn;

		public GameObject controlsOff;

		public static int Compare(GameObject o1, GameObject o2)
		{
			if (o1.transform.position.z < o2.transform.position.z)
			{
				return 1;
			}
			if (o1.transform.position.z > o2.transform.position.z)
			{
				return -1;
			}
			return 0;
		}

		private void Start()
		{
			newpos = base.transform.position;
			fov = Camera.main.fieldOfView;
			cfov = fov;
			startfov = fov;
			lastindex = 0;
			currentCol = colors[colIndex];
			targets = GameObject.FindGameObjectsWithTag("LookAt");
			Array.Sort(targets, Compare);
			for (int i = 0; i < targets.Length; i++)
			{
				if (targets[i] == target)
				{
					currentIndex = i;
					break;
				}
			}
			NewTarget(target);
			if ((bool)target)
			{
				tpos = target.transform.position;
			}
			else
			{
				Vector3 eulerAngles = base.transform.eulerAngles;
				x = eulerAngles.y;
				y = eulerAngles.x;
			}
			vx = (vy = (vz = 0f));
			x = nx;
			y = ny;
			distance = nz;
			cdofdist = nz;
			currentDistance = nz;
			dofdistance = nz;
			if ((bool)postprocess)
			{
				havedof = postprocess.TryGetSettings<DepthOfField>(out dof);
			}
			t = 8f;
			Application.targetFrameRate = -1;
		}

		public void NewTarget(GameObject targ)
		{
			if (target != targ)
			{
				target = targ;
				t = 0f;
				tpos = newpos;
				ChangeCol();
			}
		}

		private void DoColor()
		{
			if (Application.isPlaying || slowfade)
			{
				if (slowfade)
				{
					if (Application.isPlaying)
					{
						ct += Time.deltaTime * speed;
						if (ct > fadetime)
						{
							ct = 0f;
						}
					}
					currentCol = coltest.Evaluate(ct / fadetime);
				}
				else if (ct < ctime)
				{
					ct += Time.deltaTime;
					currentCol = Color.Lerp(colors[lastindex], colors[colIndex], ct / ctime);
				}
			}
			RenderSettings.fogColor = Color.Lerp(currentCol, Color.white, fogLerp);
			RenderSettings.ambientLight = Color.Lerp(currentCol, Color.white, ambientLerp);
			if ((bool)groundMat)
			{
				groundMat.color = currentCol;
			}
		}

		private void ChangeCol()
		{
			if (!slowfade)
			{
				lastindex = colIndex;
				colIndex++;
				if (colIndex >= colors.Length)
				{
					colIndex = 0;
				}
				ct = 0f;
			}
		}

		private void Update()
		{
			if (FullDemoInputs.GetInput(FullDemoInputs.DemoInputs.ShowControls))
			{
				if (controlsOn.activeInHierarchy)
				{
					controlsOn.SetActive(value: false);
					controlsOff.SetActive(value: true);
				}
				else
				{
					controlsOn.SetActive(value: true);
					controlsOff.SetActive(value: false);
				}
			}
			if (FullDemoInputs.GetInput(FullDemoInputs.DemoInputs.Quit))
			{
				Application.Quit();
			}
			DoColor();
			if (FullDemoInputs.GetInput(FullDemoInputs.DemoInputs.MouseLookEnabled))
			{
				lookat = !lookat;
			}
			if (FullDemoInputs.GetInput(FullDemoInputs.DemoInputs.MoveRight))
			{
				currentIndex++;
				if (currentIndex >= targets.Length)
				{
					currentIndex = targets.Length - 1;
				}
				if (targets.Length != 0)
				{
					NewTarget(targets[currentIndex]);
				}
			}
			if (FullDemoInputs.GetInput(FullDemoInputs.DemoInputs.MoveLeft))
			{
				currentIndex--;
				if (currentIndex < 0)
				{
					currentIndex = 0;
				}
				if (targets.Length != 0)
				{
					NewTarget(targets[currentIndex]);
				}
			}
			if (!target)
			{
				return;
			}
			RopeTarget component = target.GetComponent<RopeTarget>();
			if ((bool)component)
			{
				if (FullDemoInputs.GetInput(FullDemoInputs.DemoInputs.DecreaseRopeLength))
				{
					component.length -= component.speed * Time.deltaTime;
				}
				if (FullDemoInputs.GetInput(FullDemoInputs.DemoInputs.IncreaseRopeLength))
				{
					component.length += component.speed * Time.deltaTime;
				}
			}
			if (!lookat)
			{
				if (FullDemoInputs.GetInput(FullDemoInputs.DemoInputs.LookAroundEnabled))
				{
					nx = x + FullDemoInputs.GetMouseDelta("X") * xSpeed * 0.02f;
					ny = y - FullDemoInputs.GetMouseDelta("Y") * ySpeed * 0.02f;
				}
				if (FullDemoInputs.GetInput(FullDemoInputs.DemoInputs.ChangeFOVEnabled))
				{
					fov -= FullDemoInputs.GetMouseScrollVertical() * zSpeed * 0.5f;
				}
				else
				{
					nz -= FullDemoInputs.GetMouseScrollVertical() * zSpeed;
				}
				if (Application.isPlaying)
				{
					x = Mathf.SmoothDamp(x, nx, ref vx, delay);
					y = Mathf.SmoothDamp(y, ny, ref vy, delay);
					distance = Mathf.SmoothDamp(distance, nz, ref vz, delayz);
				}
				else
				{
					x = nx;
					y = ny;
					distance = nz;
				}
				y = ClampAngle(y, yMinLimit, yMaxLimit);
				if (distance < 1f)
				{
					distance = 1f;
					nz = 1f;
				}
				Vector3 vector = target.transform.position + offset;
				if (t < trantime)
				{
					t += Time.deltaTime;
				}
				if (!firstrun)
				{
					firstrun = true;
					newpos = vector;
				}
				newpos = Vector3.SmoothDamp(newpos, vector, ref mvel, 0.25f);
				Quaternion quaternion = Quaternion.Euler(y, x, 0f);
				Vector3 vector2 = quaternion * new Vector3(0f, 0f, 0f - distance) + newpos;
				Quaternion identity = Quaternion.identity;
				if (doRaycast && Physics.Raycast(new Ray(newpos, (vector2 - newpos).normalized), out var hitInfo, (vector2 - newpos).magnitude, 1024))
				{
					vector2 = hitInfo.point + Vector3.up * 0.2f;
				}
				base.transform.rotation = quaternion * identity;
				base.transform.position = vector2;
			}
			else
			{
				if (FullDemoInputs.GetInput(FullDemoInputs.DemoInputs.ChangeFOVEnabled))
				{
					fov -= Input.GetAxis("Mouse ScrollWheel") * zSpeed * 0.5f;
				}
				Vector3 vector3 = target.transform.position + offset;
				newpos = Vector3.SmoothDamp(newpos, vector3, ref mvel, 0.25f);
				Quaternion identity2 = Quaternion.identity;
				Quaternion quaternion2 = Quaternion.LookRotation(newpos - base.transform.position);
				base.transform.rotation = quaternion2 * identity2;
			}
			UpdateDOF();
			fov = Mathf.Clamp(fov, 8f, 45f);
			cfov = Mathf.SmoothDamp(cfov, fov, ref fovvel, 0.25f);
			Camera.main.fieldOfView = cfov;
		}

		private static float ClampAngle(float angle, float min, float max)
		{
			if (angle < -360f)
			{
				angle += 360f;
			}
			if (angle > 360f)
			{
				angle -= 360f;
			}
			return Mathf.Clamp(angle, min, max);
		}

		public void SetDOF(float dist)
		{
			if (havedof)
			{
				dof.focusDistance.value = dist;
			}
		}

		private float GetDOFDist(Ray ray)
		{
			if (Physics.Raycast(ray, out var hitInfo))
			{
				dofdistance = hitInfo.distance;
			}
			return dofdistance;
		}

		public void UpdateDOF()
		{
			if (doRaycast)
			{
				dofdistance = GetDOFDist(new Ray
				{
					origin = base.transform.position,
					direction = Quaternion.Euler(base.transform.eulerAngles) * Vector3.forward
				});
				currentDistance = Mathf.SmoothDamp(currentDistance, dofdistance, ref dz, dofdamp);
				SetDOF(currentDistance);
			}
			else
			{
				SetDOF(distance);
			}
		}
	}
}
