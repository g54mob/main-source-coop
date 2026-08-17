using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FIMSpace.AnimationTools
{
	[Serializable]
	public class LegAnimationClipMotion
	{
		public enum EAnalyzeMode
		{
			HeelFeetTight = 0,
			HeelFeetWide = 1
		}

		[Serializable]
		public class MotionSample
		{
			public Vector3 sampledToesLocal;

			public Vector3 sampledAnkleRoot;

			public Vector3 sampledFootLocal;

			public Vector3 sampledAnkleLocal;

			public Vector3 sampledHeelLocal;

			public Vector3 sampledKneeLocal;

			public Vector3 sampledUpperLegLocal;

			public Vector3 sampledRootLocal;

			public Vector3 sampledFootInRMLocal;

			public Vector3 sampledFootInAnimLocal;

			public bool grounded;

			public bool predictState;

			public bool swingForwards;

			public MotionSample GetCopy()
			{
				return MemberwiseClone() as MotionSample;
			}
		}

		public struct TransformsBackup
		{
			public Transform t;

			public Vector3 localPos;

			public Quaternion localRot;

			public Vector3 localScale;

			public void Restore()
			{
				if (!(t == null))
				{
					t.localPosition = localPos;
					t.localRotation = localRot;
					t.localScale = localScale;
				}
			}
		}

		public bool analyzed;

		public AnimationClip targetClip;

		public List<MotionSample> sampledData;

		public Vector3 LowestFootCoords;

		public Vector3 HighestFootCoords;

		public Vector3 FootCoordsDiffs;

		public Vector3 startStepFootLocal;

		public int startStepFootLocalIndex;

		public float startStepFootProgress;

		public Vector3 endStepFootLocal;

		public int endStepFootLocalIndex;

		public float endStepFootProgress;

		public Vector3 approximateFootPushDirection;

		public Vector3 approximateFootPushDirectionDominant;

		public float approximateFootLocalXPosDuringPush;

		public float approximateFootLocalZPosDuringPush;

		public Vector3 _footForward = Vector3.forward;

		public Vector3 _footToToes = Vector3.forward;

		public Vector3 _footToToesForw = Vector3.forward;

		public Vector3 _footLocalToGround = Vector3.zero;

		public float _latestToesTesh = 1f;

		public float _latestHeelTesh = 1f;

		public float _latestHoldOnGround;

		public float _latestFloorOffset;

		public EAnalyzeMode _latestAnalyzeMode;

		public float _groundToFootHeight;

		public Quaternion _initFootRot;

		public Quaternion _initFootLocRot;

		public Quaternion _footRotMapping;

		public AnimationCurve GroundingCurve;

		private int i_gameFrame = -1;

		public int i_lowerIndex;

		public int i_higherIndex;

		public float i_progress;

		public float i_forProgress;

		public static List<TransformsBackup> poseBackup = new List<TransformsBackup>();

		public LegAnimationClipMotion(AnimationClip clip)
		{
			analyzed = false;
			targetClip = clip;
		}

		public List<MotionSample> AnalyzeClip(Transform rootTr, Transform hips, Transform upperLeg, Transform knee, Transform foot, Transform optionalToes, Transform animator, int samples = 20, float heelTreshold = 1f, float toesTreshold = 1f, float holdOnGround = 0f, bool displayEditorProgressBar = false, bool removeRootMotion = false, float floorOffset = 0f, EAnalyzeMode mode = EAnalyzeMode.HeelFeetTight, int cutFirstFrames = 0, int cutLastFrames = 0)
		{
			analyzed = false;
			Matrix4x4 inverse = Matrix4x4.TRS(animator.position, animator.rotation, animator.lossyScale).inverse;
			_latestToesTesh = toesTreshold;
			_latestHeelTesh = heelTreshold;
			_latestHoldOnGround = holdOnGround;
			_latestAnalyzeMode = mode;
			_latestFloorOffset = floorOffset;
			if (cutFirstFrames < 0)
			{
				cutFirstFrames = 0;
			}
			if (cutLastFrames < 0)
			{
				cutLastFrames = 0;
			}
			sampledData = new List<MotionSample>();
			StorePoseBackup(rootTr);
			_initFootRot = foot.rotation;
			_initFootLocRot = foot.localRotation;
			_groundToFootHeight = rootTr.InverseTransformPoint(foot.position).y;
			float num = (float)cutFirstFrames / (float)samples;
			samples -= cutFirstFrames + cutLastFrames;
			float num2 = 1f / (float)samples;
			if (displayEditorProgressBar)
			{
				ProgressBar("Preparing foot analysis...", 0f);
			}
			Transform transform = null;
			SkinnedMeshRenderer[] componentsInChildren = animator.GetComponentsInChildren<SkinnedMeshRenderer>();
			if (componentsInChildren != null && componentsInChildren.Length != 0)
			{
				SkinnedMeshRenderer skinnedMeshRenderer = componentsInChildren.OrderBy((SkinnedMeshRenderer s) => s.bones.Length).First();
				if (skinnedMeshRenderer != null)
				{
					transform = skinnedMeshRenderer.rootBone;
				}
			}
			if (transform == null)
			{
				transform = hips;
			}
			Vector3 position = new Vector3(foot.position.x, rootTr.position.y, foot.position.z);
			position = foot.InverseTransformPoint(position);
			Vector3 normalized = foot.InverseTransformPoint(foot.position + rootTr.forward).normalized;
			Vector3 vector = ((!optionalToes) ? (normalized * (Vector3.Distance(foot.position, knee.position) + Vector3.Distance(upperLeg.position, knee.position)) * 0.325f) : foot.InverseTransformPoint(optionalToes.position));
			Vector3 vector2;
			if ((bool)optionalToes)
			{
				Vector3 position2 = optionalToes.position;
				position2.y = foot.position.y;
				vector2 = foot.InverseTransformPoint(position2);
			}
			else
			{
				vector2 = vector;
			}
			_ = vector.normalized;
			_footForward = normalized;
			_footToToes = vector;
			_footToToesForw = vector2;
			_footLocalToGround = position;
			_footRotMapping = Quaternion.FromToRotation(foot.InverseTransformDirection(rootTr.right), Vector3.right);
			_footRotMapping *= Quaternion.FromToRotation(foot.InverseTransformDirection(rootTr.up), Vector3.up);
			Vector3 vector3 = rootTr.InverseTransformPoint(transform.position);
			GroundingCurve = new AnimationCurve();
			for (int num3 = 0; num3 < samples; num3++)
			{
				if (displayEditorProgressBar)
				{
					ProgressBar("Sampling leg animation data " + num3 + " / " + samples, (float)num3 / (float)samples);
				}
				MotionSample motionSample = new MotionSample();
				targetClip.SampleAnimation(animator.gameObject, num + num2 * (float)num3 * targetClip.length);
				if (removeRootMotion)
				{
					transform.localPosition = Vector3.zero;
					Vector3 position3 = animator.InverseTransformPoint(hips.position);
					position3.z = 0f;
					hips.position = animator.TransformPoint(position3);
				}
				motionSample.sampledAnkleRoot = inverse.MultiplyPoint(foot.position);
				_ = transform.position;
				Vector3 vector4 = rootTr.InverseTransformPoint(transform.position);
				transform.position = rootTr.TransformPoint(vector4.x, vector4.y, vector3.z);
				motionSample.sampledRootLocal = rootTr.InverseTransformPoint(transform.position);
				motionSample.sampledFootInRMLocal = transform.InverseTransformPoint(transform.position);
				motionSample.sampledFootInAnimLocal = animator.InverseTransformPoint(transform.position);
				Vector3 position4 = foot.position;
				motionSample.sampledAnkleLocal = rootTr.InverseTransformPoint(position4);
				position4 += foot.TransformDirection(position);
				motionSample.sampledFootLocal = rootTr.InverseTransformPoint(position4);
				Vector3 samplingToesPoint = GetSamplingToesPoint(foot, toesTreshold);
				Vector3 samplingHeelPoint = GetSamplingHeelPoint(foot, heelTreshold);
				if (mode == EAnalyzeMode.HeelFeetTight)
				{
					motionSample.sampledToesLocal = rootTr.InverseTransformPoint(samplingToesPoint);
					motionSample.sampledHeelLocal = rootTr.InverseTransformPoint(samplingHeelPoint);
				}
				else
				{
					samplingToesPoint = foot.position;
					samplingToesPoint += foot.TransformDirection(vector);
					samplingToesPoint += foot.TransformDirection(vector2) * position.magnitude * 4f;
					motionSample.sampledToesLocal = rootTr.InverseTransformPoint(samplingToesPoint);
					samplingHeelPoint = foot.position;
					samplingHeelPoint += foot.TransformDirection(position);
					samplingHeelPoint -= foot.TransformDirection(normalized) * position.magnitude * 0.8f;
					motionSample.sampledHeelLocal = rootTr.InverseTransformPoint(samplingHeelPoint);
				}
				motionSample.sampledKneeLocal = rootTr.InverseTransformPoint(knee.position);
				motionSample.sampledUpperLegLocal = rootTr.InverseTransformPoint(upperLeg.position);
				sampledData.Add(motionSample);
			}
			if (displayEditorProgressBar)
			{
				ProgressBar("Restoring character pose", 1f);
			}
			AnimationClip animationClip = targetClip;
			string[] array = new string[3] { "idle", "stop", "none" };
			Animator component = animator.GetComponent<Animator>();
			if ((bool)component)
			{
				if ((bool)component.runtimeAnimatorController)
				{
					for (int num4 = 0; num4 < component.runtimeAnimatorController.animationClips.Length; num4++)
					{
						for (int num5 = 0; num5 < array.Length; num5++)
						{
							if (component.runtimeAnimatorController.animationClips[num4].name.ToLower().Contains(array[num5]))
							{
								animationClip = component.runtimeAnimatorController.animationClips[num4];
								break;
							}
						}
					}
				}
				else
				{
					Debug.Log("[Error] No Animator Controller in " + component.name);
				}
			}
			animationClip.SampleAnimation(animator.gameObject, 0f);
			if (displayEditorProgressBar)
			{
				ProgressBar("Checking collected data", 0f);
			}
			LowestFootCoords = new Vector3(3.4028235E+38f, 3.4028235E+38f, 3.4028235E+38f);
			HighestFootCoords = new Vector3(-3.4028235E+38f, -3.4028235E+38f, -3.4028235E+38f);
			for (int num6 = 0; num6 < samples; num6++)
			{
				if (sampledData[num6].sampledFootLocal.x < LowestFootCoords.x)
				{
					LowestFootCoords.x = sampledData[num6].sampledFootLocal.x;
				}
				if (sampledData[num6].sampledFootLocal.y < LowestFootCoords.y)
				{
					LowestFootCoords.y = sampledData[num6].sampledFootLocal.y;
				}
				if (sampledData[num6].sampledFootLocal.z < LowestFootCoords.z)
				{
					LowestFootCoords.z = sampledData[num6].sampledFootLocal.z;
				}
				if (sampledData[num6].sampledFootLocal.x > HighestFootCoords.x)
				{
					HighestFootCoords.x = sampledData[num6].sampledFootLocal.x;
				}
				if (sampledData[num6].sampledFootLocal.y > HighestFootCoords.y)
				{
					HighestFootCoords.y = sampledData[num6].sampledFootLocal.y;
				}
				if (sampledData[num6].sampledFootLocal.z > HighestFootCoords.z)
				{
					HighestFootCoords.z = sampledData[num6].sampledFootLocal.z;
				}
			}
			FootCoordsDiffs = new Vector3(LowestFootCoords.x - HighestFootCoords.x, LowestFootCoords.y - HighestFootCoords.y, LowestFootCoords.z - HighestFootCoords.z);
			float refScale = GetRefScale(knee, foot);
			float heightTresholdScale = GetHeightTresholdScale(knee, foot, refScale);
			bool? flag = null;
			float num7 = 0f;
			float num8 = 0f;
			float num9 = 1f / (float)samples;
			approximateFootLocalXPosDuringPush = sampledData[0].sampledFootLocal.x;
			approximateFootLocalZPosDuringPush = sampledData[0].sampledFootLocal.z;
			bool flag2 = false;
			bool flag3 = false;
			for (int num10 = 0; num10 < samples; num10++)
			{
				if (displayEditorProgressBar)
				{
					ProgressBar("Sampling collected data " + num10 + " / " + samples, (float)num10 / (float)samples);
				}
				float num11 = (float)num10 / (float)samples;
				MotionSample motionSample2 = sampledData[num10];
				motionSample2.grounded = false;
				if (mode == EAnalyzeMode.HeelFeetTight)
				{
					if (motionSample2.sampledHeelLocal.y < heightTresholdScale * heelTreshold + floorOffset)
					{
						motionSample2.grounded = true;
					}
					if (motionSample2.sampledToesLocal.y < heightTresholdScale * toesTreshold + floorOffset)
					{
						motionSample2.grounded = true;
					}
				}
				else if (motionSample2.sampledHeelLocal.y < heightTresholdScale * heelTreshold + floorOffset || motionSample2.sampledFootLocal.y < heightTresholdScale * toesTreshold + floorOffset)
				{
					motionSample2.grounded = true;
				}
				if (motionSample2.grounded && flag == true)
				{
					num7 += num9;
					num8 = 0f;
				}
				else if (num7 > 0f)
				{
					if (holdOnGround > 0f && num7 < holdOnGround)
					{
						num7 += num9;
						motionSample2.grounded = true;
					}
				}
				else
				{
					num8 += num9;
					if (num8 > num9 * 3f && num7 > holdOnGround)
					{
						num7 = 0f;
					}
				}
				if (flag != true && motionSample2.grounded)
				{
					startStepFootLocal = motionSample2.sampledFootLocal;
					startStepFootLocalIndex = num10;
					startStepFootProgress = (float)num10 / (float)samples;
				}
				else if (flag == true && !motionSample2.grounded)
				{
					endStepFootLocal = motionSample2.sampledFootLocal;
					endStepFootLocalIndex = num10;
					endStepFootProgress = (float)num10 / (float)samples;
				}
				flag = motionSample2.grounded;
				if (motionSample2.grounded)
				{
					float num12 = num11 - 2f / (float)samples;
					if (num12 < 0f)
					{
						num12 += 1f;
					}
					Vector3 vector5 = GetFootLocalPosition(num11) - GetFootLocalPosition(num12);
					vector5.y = 0f;
					approximateFootPushDirection += vector5;
					if (!flag2)
					{
						flag2 = true;
						approximateFootLocalXPosDuringPush = motionSample2.sampledFootLocal.x;
					}
					else
					{
						approximateFootLocalXPosDuringPush = Mathf.Lerp(approximateFootLocalXPosDuringPush, motionSample2.sampledFootLocal.x, 0.5f);
					}
					if (!flag3)
					{
						flag3 = true;
						approximateFootLocalZPosDuringPush = motionSample2.sampledFootLocal.z;
					}
					else
					{
						approximateFootLocalZPosDuringPush = Mathf.Lerp(approximateFootLocalZPosDuringPush, motionSample2.sampledFootLocal.z, 0.5f);
					}
				}
				approximateFootPushDirection.Normalize();
				approximateFootPushDirectionDominant = FVectorMethods.ChooseDominantAxis(approximateFootPushDirection);
				Vector3 footLocalPosition = GetFootLocalPosition(num11 - num2);
				if (GetFootLocalPosition(num11).z > footLocalPosition.z)
				{
					motionSample2.swingForwards = true;
				}
				float num13 = refScale;
				if (num13 < 0.4f)
				{
					num13 = 0.4f;
				}
				float value = Mathf.Clamp(motionSample2.sampledFootLocal.y / num13, 0.2f, 1f);
				if (motionSample2.grounded)
				{
					value = 0f;
				}
				GroundingCurve.AddKey(num11, value);
			}
			List<Keyframe> list = new List<Keyframe>();
			list.Add(GroundingCurve.keys[0]);
			for (int num14 = 1; num14 < GroundingCurve.keys.Length - 1; num14++)
			{
				if (GroundingCurve.keys[num14].value == 0f && GroundingCurve.keys[num14 + 1].value > 0f)
				{
					list.Add(GroundingCurve.keys[num14]);
				}
				else if (GroundingCurve.keys[num14 - 1].value > 0f && GroundingCurve.keys[num14].value == 0f)
				{
					list.Add(GroundingCurve.keys[num14]);
				}
				else if (GroundingCurve.keys[num14 - 1].value != GroundingCurve.keys[num14].value)
				{
					list.Add(GroundingCurve.keys[num14]);
				}
			}
			list.Add(GroundingCurve.keys[GroundingCurve.keys.Length - 1]);
			GroundingCurve = new AnimationCurve(list.ToArray());
			GroundingCurve = AnimationGenerateUtils.ReduceKeyframes(GroundingCurve, 0.1f);
			for (int num15 = 0; num15 < samples; num15++)
			{
				if (displayEditorProgressBar)
				{
					ProgressBar("Defining Additional Data " + num15 + " / " + samples, (float)num15 / (float)samples);
				}
				MotionSample motionSample3 = sampledData[num15];
				if (motionSample3.grounded)
				{
					continue;
				}
				if (motionSample3.sampledFootLocal.z < 0f)
				{
					if (motionSample3.swingForwards && motionSample3.sampledFootLocal.z > LowestFootCoords.z / 2f)
					{
						motionSample3.predictState = true;
					}
				}
				else
				{
					motionSample3.predictState = true;
				}
			}
			if (displayEditorProgressBar)
			{
				ProgressBar("Finalizing", 1f);
			}
			RestorePoseBackup();
			if (displayEditorProgressBar)
			{
				ProgressBar("", 1.1f);
			}
			analyzed = true;
			return sampledData;
		}

		public float GetRefScale(Transform knee, Transform foot)
		{
			return Vector3.Distance(foot.position, knee.position) * 0.1f;
		}

		public float GetTresholdLength(Transform knee, Transform foot, float treshold, float amplification = 1f)
		{
			float num = GetHeightTresholdScale(knee, foot) + LowestFootCoords.y;
			return GetAmplified02Range(1f - treshold, amplification) * num;
		}

		public float GetHeightTresholdScale(Transform knee, Transform foot, float? refScale = null)
		{
			float num = (refScale.HasValue ? refScale.Value : GetRefScale(knee, foot));
			return num + LowestFootCoords.y;
		}

		public Vector3 GetToesFootLocalPosition(float progress, float toesToFoot = 0.5f)
		{
			return Vector3.LerpUnclamped(GetToesLocalPosition(progress), GetFootLocalPosition(progress), toesToFoot);
		}

		public Vector3 GetToesLocalPosition(float progress)
		{
			RefreshInterpolationIndexes(progress);
			return Vector3.LerpUnclamped(sampledData[i_lowerIndex].sampledToesLocal, sampledData[i_higherIndex].sampledToesLocal, i_progress);
		}

		public Vector3 GetAnkleLocalPosition(float progress)
		{
			RefreshInterpolationIndexes(progress);
			return Vector3.LerpUnclamped(sampledData[i_lowerIndex].sampledAnkleLocal, sampledData[i_higherIndex].sampledAnkleLocal, i_progress);
		}

		public MotionSample GetSampleInProgress(float progress, bool higher = true)
		{
			RefreshInterpolationIndexes(progress);
			if (higher)
			{
				return sampledData[i_higherIndex];
			}
			return sampledData[i_lowerIndex];
		}

		public Vector3 GetFootLocalPositionInAnimator(float progress)
		{
			progress = RoundCycle(progress);
			RefreshInterpolationIndexes(progress);
			return Vector3.LerpUnclamped(sampledData[i_lowerIndex].sampledFootInAnimLocal, sampledData[i_higherIndex].sampledFootInAnimLocal, i_progress);
		}

		public Vector3 GetFootLocalPositionInRootMotion(float progress)
		{
			progress = RoundCycle(progress);
			RefreshInterpolationIndexes(progress);
			return Vector3.LerpUnclamped(sampledData[i_lowerIndex].sampledFootInRMLocal, sampledData[i_higherIndex].sampledFootInRMLocal, i_progress);
		}

		public Vector3 GetFootLocalPosition(float progress)
		{
			progress = RoundCycle(progress);
			RefreshInterpolationIndexes(progress);
			return Vector3.LerpUnclamped(sampledData[i_lowerIndex].sampledFootLocal, sampledData[i_higherIndex].sampledFootLocal, i_progress);
		}

		public Vector3 GetHeelLocalPosition(float progress)
		{
			RefreshInterpolationIndexes(progress);
			return Vector3.LerpUnclamped(sampledData[i_lowerIndex].sampledHeelLocal, sampledData[i_higherIndex].sampledHeelLocal, i_progress);
		}

		public Vector3 GetUpperLegLocalPosition(float progress)
		{
			RefreshInterpolationIndexes(progress);
			return Vector3.LerpUnclamped(sampledData[i_lowerIndex].sampledUpperLegLocal, sampledData[i_higherIndex].sampledUpperLegLocal, i_progress);
		}

		public Vector3 GetRootLocalPosition(float progress)
		{
			RefreshInterpolationIndexes(progress);
			return Vector3.LerpUnclamped(sampledData[i_lowerIndex].sampledRootLocal, sampledData[i_higherIndex].sampledRootLocal, i_progress);
		}

		public Vector3 GetKneeLocalPosition(float progress)
		{
			RefreshInterpolationIndexes(progress);
			return Vector3.LerpUnclamped(sampledData[i_lowerIndex].sampledKneeLocal, sampledData[i_higherIndex].sampledKneeLocal, i_progress);
		}

		public bool GroundedIn(float progress)
		{
			if (sampledData == null)
			{
				return false;
			}
			if (sampledData.Count == 0)
			{
				return false;
			}
			return GroundingCurve.Evaluate(progress) < 0.05f;
		}

		public bool PredictIn(float progress)
		{
			RefreshInterpolationIndexes(progress);
			if (i_progress > 0.5f)
			{
				return sampledData[i_higherIndex].predictState;
			}
			return sampledData[i_lowerIndex].predictState;
		}

		public bool GetSwingingForward(float progress)
		{
			RefreshInterpolationIndexes(progress);
			if (i_progress > 0.5f)
			{
				return sampledData[i_higherIndex].swingForwards;
			}
			return sampledData[i_lowerIndex].swingForwards;
		}

		public float GetAmplified02Range(float enterValue, float amplifyAfter1UpTo2 = 5f)
		{
			if (enterValue <= 1f)
			{
				return enterValue;
			}
			float num = 0f - (1f - enterValue);
			return enterValue + num * amplifyAfter1UpTo2;
		}

		public void RefreshInterpolationIndexes(float progress)
		{
			if (sampledData != null && sampledData.Count != 0 && (progress != i_forProgress || i_gameFrame != Time.frameCount))
			{
				i_gameFrame = Time.frameCount;
				i_forProgress = progress;
				i_progress = RoundCycle(i_forProgress);
				i_higherIndex = Mathf.CeilToInt(i_progress * (float)sampledData.Count);
				if (i_higherIndex > sampledData.Count - 1)
				{
					i_lowerIndex = sampledData.Count - 1;
					i_higherIndex = 0;
					i_progress = Mathf.InverseLerp(i_lowerIndex, sampledData.Count, i_progress * (float)sampledData.Count);
				}
				else
				{
					i_lowerIndex = Mathf.FloorToInt(i_progress * (float)sampledData.Count);
					i_progress = Mathf.InverseLerp(i_lowerIndex, i_higherIndex, i_progress * (float)sampledData.Count);
				}
			}
		}

		private void ProgressBar(string text, float prog)
		{
		}

		public static float RoundCycle(float cycleProgress)
		{
			return cycleProgress - Mathf.Floor(cycleProgress);
		}

		public LegAnimationClipMotion GetCopy()
		{
			return MemberwiseClone() as LegAnimationClipMotion;
		}

		private static void StorePoseBackup(Transform rootTransform)
		{
			poseBackup.Clear();
			Transform[] componentsInChildren = rootTransform.GetComponentsInChildren<Transform>();
			foreach (Transform transform in componentsInChildren)
			{
				TransformsBackup item = new TransformsBackup
				{
					t = transform,
					localPos = transform.localPosition,
					localRot = transform.localRotation,
					localScale = transform.localScale
				};
				poseBackup.Add(item);
			}
		}

		private static void RestorePoseBackup()
		{
			for (int num = poseBackup.Count - 1; num >= 0; num--)
			{
				poseBackup[num].Restore();
			}
			poseBackup.Clear();
		}

		public Vector3 GetSamplingToesPoint(Transform foot, float tresh)
		{
			return foot.position + foot.TransformDirection(_footLocalToGround) + foot.TransformDirection(_footToToesForw) * _footLocalToGround.magnitude * (4f - Mathf.LerpUnclamped(0f, 0.1f, tresh));
		}

		public Vector3 GetSamplingHeelPoint(Transform foot, float tresh)
		{
			return foot.position + foot.TransformDirection(_footLocalToGround) - foot.TransformDirection(_footForward) * _footLocalToGround.magnitude * (0.6f - Mathf.Lerp(0f, 2f, tresh));
		}
	}
}
