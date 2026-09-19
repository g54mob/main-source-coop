using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace Features.AIModule.Scripts.CustomNavMesh
{
	[RequireComponent(typeof(NavMeshAgent))]
	public class AgentLinkMover : MonoBehaviour
	{
		[SerializeField]
		private bool _disableUpdateRotation;

		[SerializeField]
		private bool _rotateDuringLinkMovement;

		[SerializeField]
		private float _bodyRotationSpeed;

		public List<LinkMoverData> LinkMoverData;

		public LinkMoverData DefaultMoverData;

		private readonly Dictionary<int, LinkMoverData> _dataDictionary = new Dictionary<int, LinkMoverData>();

		private float _currentBodyYRotation;

		private void Awake()
		{
			foreach (LinkMoverData linkMoverDatum in LinkMoverData)
			{
				_dataDictionary.Add(NavMesh.GetAreaFromName(linkMoverDatum.NavMeshAreas.ToString()), linkMoverDatum);
			}
		}

		private IEnumerator Start()
		{
			NavMeshAgent agent = GetComponent<NavMeshAgent>();
			agent.autoTraverseOffMeshLink = false;
			agent.updateUpAxis = false;
			if (_disableUpdateRotation)
			{
				agent.updateRotation = false;
			}
			while (true)
			{
				if (agent != null && agent.isActiveAndEnabled && agent.isOnOffMeshLink)
				{
					agent.updatePosition = false;
					try
					{
						yield return MoveEnumerator(ResolveMoverData(agent), agent);
						if (agent != null && agent.isActiveAndEnabled)
						{
							if (agent.isOnNavMesh && agent.isOnOffMeshLink)
							{
								agent.CompleteOffMeshLink();
							}
							else
							{
								agent.Warp(agent.transform.position);
							}
						}
					}
					finally
					{
						if (agent != null)
						{
							agent.updatePosition = true;
						}
					}
				}
				yield return null;
			}
		}

		private LinkMoverData ResolveMoverData(NavMeshAgent agent)
		{
			NavMeshLink navMeshLink = agent.navMeshOwner as NavMeshLink;
			if (navMeshLink != null)
			{
				if (_dataDictionary.TryGetValue(navMeshLink.area, out var value))
				{
					return value;
				}
				return DefaultMoverData;
			}
			if (agent.navMeshOwner is DynamicJumpNavMeshLinker dynamicJumpNavMeshLinker && _dataDictionary.TryGetValue(dynamicJumpNavMeshLinker.AreaId, out var value2))
			{
				return value2;
			}
			return DefaultMoverData;
		}

		private Transform GetRotationTransform(NavMeshAgent agent)
		{
			return agent.transform;
		}

		private void RotateTowardsDirection(Vector3 direction, NavMeshAgent agent, float speedMultiplier = 1f)
		{
			direction.y = 0f;
			if (!(direction.sqrMagnitude < 0.0001f))
			{
				direction.Normalize();
				float b = Mathf.Atan2(direction.x, direction.z) * 57.29578f;
				_currentBodyYRotation = Mathf.LerpAngle(_currentBodyYRotation, b, Time.deltaTime * _bodyRotationSpeed * speedMultiplier);
				GetRotationTransform(agent).rotation = Quaternion.Euler(0f, _currentBodyYRotation, 0f);
			}
		}

		private IEnumerator MoveEnumerator(LinkMoverData moverData, NavMeshAgent agent)
		{
			Transform rotationTransform = GetRotationTransform(agent);
			_currentBodyYRotation = rotationTransform.eulerAngles.y;
			if (moverData.LinkMoveMethod == OffMeshLinkMoveMethod.NormalSpeed)
			{
				yield return StartCoroutine(NormalSpeed(agent));
			}
			else if (moverData.LinkMoveMethod == OffMeshLinkMoveMethod.Parabola)
			{
				yield return StartCoroutine(Parabola(agent, moverData.Height, moverData.Duration));
			}
			else if (moverData.LinkMoveMethod == OffMeshLinkMoveMethod.Curve)
			{
				yield return StartCoroutine(Curve(agent, 0.5f, moverData.Curve));
			}
		}

		private IEnumerator NormalSpeed(NavMeshAgent agent)
		{
			Vector3 endPos = agent.currentOffMeshLinkData.endPos + Vector3.up * agent.baseOffset;
			float elapsed = 0f;
			while (!(Vector3.Distance(agent.transform.position, endPos) <= 0.05f))
			{
				if (elapsed >= 3f)
				{
					Debug.LogWarning("NormalSpeed timeout — телепортируем в endPos");
					agent.transform.position = endPos;
					break;
				}
				agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime);
				if (_rotateDuringLinkMovement)
				{
					Vector3 direction = endPos - agent.transform.position;
					RotateTowardsDirection(direction, agent, 2f);
				}
				elapsed += Time.deltaTime;
				yield return null;
			}
			agent.transform.position = endPos;
		}

		private IEnumerator Parabola(NavMeshAgent agent, float height, float duration)
		{
			OffMeshLinkData currentOffMeshLinkData = agent.currentOffMeshLinkData;
			Vector3 startPos = agent.transform.position;
			Vector3 endPos = currentOffMeshLinkData.endPos + Vector3.up * agent.baseOffset;
			float normalizedTime = 0f;
			while (normalizedTime < 1f)
			{
				float num = height * 4f * (normalizedTime - normalizedTime * normalizedTime);
				agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + num * Vector3.up;
				if (_rotateDuringLinkMovement)
				{
					Vector3 direction = endPos - agent.transform.position;
					RotateTowardsDirection(direction, agent, 2f);
				}
				normalizedTime += Time.deltaTime / duration;
				yield return null;
			}
		}

		private IEnumerator Curve(NavMeshAgent agent, float duration, AnimationCurve curve)
		{
			OffMeshLinkData currentOffMeshLinkData = agent.currentOffMeshLinkData;
			Vector3 startPos = agent.transform.position;
			Vector3 endPos = currentOffMeshLinkData.endPos + Vector3.up * agent.baseOffset;
			float normalizedTime = 0f;
			while (normalizedTime < 1f)
			{
				float num = curve.Evaluate(normalizedTime);
				agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + num * Vector3.up;
				if (_rotateDuringLinkMovement)
				{
					Vector3 direction = endPos - agent.transform.position;
					RotateTowardsDirection(direction, agent, 2f);
				}
				normalizedTime += Time.deltaTime / duration;
				yield return null;
			}
		}
	}
}
