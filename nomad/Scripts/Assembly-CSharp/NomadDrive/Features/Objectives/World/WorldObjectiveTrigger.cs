using System;
using Cysharp.Threading.Tasks;
using EvilCore.EvilPack.EvilLogger;
using NomadDrive.Features.Player;
using UnityEngine;

namespace NomadDrive.Features.Objectives.World
{
	[RequireComponent(typeof(Collider))]
	public class WorldObjectiveTrigger : MonoBehaviour
	{
		[Tooltip("Objective to discover when a player enters this trigger volume. Bypasses the objective's own DiscoveryTriggers (any of those may still fire in parallel, but discovery is idempotent so only the first arrival counts).")]
		[SerializeField]
		private string objectiveId;

		[Tooltip("Optional. StepId in the selected objective to mark complete on first entry. Leave empty to just discover.")]
		[SerializeField]
		private string autoCompleteStepId;

		[Tooltip("Seconds to wait between player entering the volume and the objective firing. Once the countdown starts it cannot be cancelled by leaving the zone (commit-on-enter). 0 = fire immediately on enter.")]
		[SerializeField]
		private float triggerDelaySeconds;

		[Tooltip("Fire only once per session (default). If false, the trigger re-arms after each delayed fire (harmless since the service is idempotent on discovery, but keeps the OnTriggerEnter cost predictable).")]
		[SerializeField]
		private bool oneShot = true;

		[Tooltip("If true, the collider is forced into trigger mode at Awake. Disable only if you have a custom setup.")]
		[SerializeField]
		private bool forceTriggerOnAwake = true;

		[Tooltip("Enable to log every OnTriggerEnter/Stay event and decision point through EvilLogger. Use this when a placed volume does not seem to fire.")]
		[SerializeField]
		private bool debugLogging = true;

		[Tooltip("Wireframe color drawn in the scene view to visualize the trigger volume bounds.")]
		[SerializeField]
		private Color gizmoColor = new Color(0f, 1f, 0.4f, 0.35f);

		private bool _completed;

		private bool _firing;

		private void Awake()
		{
			Collider component = GetComponent<Collider>();
			if (forceTriggerOnAwake && component != null && !component.isTrigger)
			{
				component.isTrigger = true;
			}
			_ = debugLogging;
		}

		private void OnTriggerEnter(Collider other)
		{
			TryFire(other, "OnTriggerEnter");
		}

		private void OnTriggerStay(Collider other)
		{
			TryFire(other, "OnTriggerStay");
		}

		private void TryFire(Collider other, string source)
		{
			if (_firing || (_completed && oneShot))
			{
				return;
			}
			NomadDrive.Features.Player.Player componentInParent = other.GetComponentInParent<NomadDrive.Features.Player.Player>();
			if (componentInParent == null || !componentInParent.isLocalPlayer)
			{
				return;
			}
			if (string.IsNullOrEmpty(objectiveId))
			{
				_ = debugLogging;
				return;
			}
			_ = debugLogging;
			if (triggerDelaySeconds <= 0f)
			{
				FireNow();
			}
			else
			{
				FireAfterDelayAsync().Forget();
			}
		}

		private void OnDrawGizmos()
		{
			Collider component = GetComponent<Collider>();
			if (component == null)
			{
				return;
			}
			Gizmos.color = gizmoColor;
			Matrix4x4 matrix = Gizmos.matrix;
			Gizmos.matrix = base.transform.localToWorldMatrix;
			if (!(component is BoxCollider boxCollider))
			{
				if (component is SphereCollider sphereCollider)
				{
					Gizmos.DrawSphere(sphereCollider.center, sphereCollider.radius);
					Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 1f);
					Gizmos.DrawWireSphere(sphereCollider.center, sphereCollider.radius);
				}
				else
				{
					Gizmos.matrix = matrix;
					Bounds bounds = component.bounds;
					Gizmos.DrawWireCube(bounds.center, bounds.size);
				}
			}
			else
			{
				Gizmos.DrawCube(boxCollider.center, boxCollider.size);
				Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 1f);
				Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
			}
			Gizmos.matrix = matrix;
		}

		private async UniTaskVoid FireAfterDelayAsync()
		{
			_firing = true;
			try
			{
				await UniTask.Delay(TimeSpan.FromSeconds(triggerDelaySeconds), ignoreTimeScale: false, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
			}
			catch (OperationCanceledException)
			{
				_firing = false;
				return;
			}
			FireNow();
		}

		private void FireNow()
		{
			ObjectivesService objectivesService = UnityEngine.Object.FindFirstObjectByType<ObjectivesService>();
			if (objectivesService == null)
			{
				if (debugLogging)
				{
					EvilLogger.LogError("[WorldObjectiveTrigger] '" + base.name + "' FireNow: ObjectivesService not found in scene.", "FireNow", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Objectives\\Scripts\\World\\WorldObjectiveTrigger.cs", 146);
				}
			}
			else
			{
				_ = debugLogging;
				objectivesService.TryDiscoverObjective(objectiveId, autoCompleteStepId);
			}
			_completed = true;
			_firing = false;
		}
	}
}
