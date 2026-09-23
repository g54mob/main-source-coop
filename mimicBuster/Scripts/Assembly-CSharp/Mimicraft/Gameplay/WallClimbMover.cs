using UnityEngine;

namespace Mimicraft.Gameplay
{
	public class WallClimbMover
	{
		private const float MoveSpeed = 4f;

		private const float RunMultiplier = 1.6f;

		private float moveSpeedScale = 1f;

		private const float ClimbSpeed = 3f;

		private const float JumpHeight = 1.2f;

		private const float Gravity = -20f;

		private const float FacingRotationSpeed = 12f;

		private const float GroundedStickVelocity = -1f;

		private const float WallJumpSpeed = 9f;

		private static readonly RaycastHit[] probeHits = new RaycastHit[8];

		private const float MaxWallReach = 1.5f;

		private const float WallReachSlack = 0.3f;

		private const float MaxWallNormalUpDot = 0.26f;

		private const float ReleaseWallNormalUpDot = 0.5f;

		private const float GrabRequestSeconds = 0.25f;

		private float grabRequest;

		private bool grabWasHeld;

		private bool grabHeld;

		private Vector3 leftNormal;

		private Vector3 leftPoint;

		private float verticalVelocity;

		private float clingWallDistance;

		private Vector3 wallNormal;

		private bool clingLeftGround;

		private int maxAirJumps;

		private int airJumpsUsed;

		private bool wasGrounded = true;

		private float coyoteSeconds;

		private float coyoteUntil;

		private const float SameSurfaceMinDot = 0.85f;

		private const float WrapNormalAlongTravel = 0.7f;

		private const float OverheadNormalUpDot = -0.5f;

		private const float WrapClearance = 0.03f;

		private const float HeadOnTransferDot = 0.5f;

		private const float WrapThicknessDepth = 2f;

		private const float ReturnRadius = 1.5f;

		private const float WrapProbeRadius = 0.02f;

		private static readonly Collider[] overlapHits = new Collider[8];

		private const float WallTransferOffset = 0.05f;

		private const float WallTransferMinTurn = 0.7f;

		public bool IsGrounded { get; private set; }

		public bool IsClinging { get; private set; }

		public bool GrabbedThisTick { get; private set; }

		public bool ReleasedThisTick { get; private set; }

		public Vector3 SurfacePoint { get; private set; }

		public Vector3 SurfaceNormal => wallNormal;

		public LayerMask ClimbableLayers { get; set; } = -1;

		public Quaternion FacingOffset { get; set; } = Quaternion.identity;

		public Vector3 FacingDirection { get; set; }

		public Bounds? BodyBoundsLocal { get; set; }

		public bool JumpedThisTick { get; private set; }

		public bool AirJumpedThisTick { get; private set; }

		public bool LandedThisTick { get; private set; }

		private float JumpSpeed => Mathf.Sqrt(48f);

		public void ResetOrientation()
		{
			IsClinging = false;
			grabRequest = 0f;
			grabWasHeld = false;
			ResetFall();
		}

		public void ResetFall()
		{
			verticalVelocity = -1f;
			airJumpsUsed = 0;
			coyoteUntil = 0f;
		}

		private void PerformGroundJump()
		{
			verticalVelocity = JumpSpeed;
			coyoteUntil = 0f;
			JumpedThisTick = true;
		}

		public void Tick(CharacterController controller, Vector3 rawCamForward, Vector3 rawCamRight, Vector2 input2D, bool jumpPressed, bool running, bool climbHeld, float deltaTime, bool allowWallClimb = true, int airJumpAllowance = 0, float coyoteAllowance = 0f, float speedScale = 1f)
		{
			moveSpeedScale = Mathf.Max(0f, speedScale);
			maxAirJumps = airJumpAllowance;
			coyoteSeconds = coyoteAllowance;
			JumpedThisTick = false;
			AirJumpedThisTick = false;
			LandedThisTick = false;
			GrabbedThisTick = false;
			ReleasedThisTick = false;
			if (!allowWallClimb && IsClinging)
			{
				ResetOrientation();
			}
			grabHeld = allowWallClimb && climbHeld;
			grabRequest = ((grabHeld && !grabWasHeld) ? 0.25f : Mathf.Max(0f, grabRequest - deltaTime));
			grabWasHeld = grabHeld;
			if (IsClinging)
			{
				TickClinging(controller, rawCamForward, input2D, jumpPressed, running, deltaTime);
			}
			else
			{
				TickGrounded(controller, rawCamForward, rawCamRight, input2D, jumpPressed, running, deltaTime, allowWallClimb && climbHeld);
			}
		}

		private void TickGrounded(CharacterController controller, Vector3 rawCamForward, Vector3 rawCamRight, Vector2 input2D, bool jumpPressed, bool running, float deltaTime, bool allowWallClimb)
		{
			Transform transform = controller.transform;
			bool flag = verticalVelocity > 0.01f;
			DetectGround(transform, controller);
			Vector3 normalized = Vector3.ProjectOnPlane(rawCamForward, Vector3.up).normalized;
			Vector3 normalized2 = Vector3.ProjectOnPlane(rawCamRight, Vector3.up).normalized;
			Vector3 vector = normalized * input2D.y + normalized2 * input2D.x;
			if (vector.sqrMagnitude > 1f)
			{
				vector.Normalize();
			}
			if (allowWallClimb && !flag && TryDetectClimbableWall(transform, controller, vector, ClimbableLayers, out var hit))
			{
				IsClinging = true;
				GrabbedThisTick = true;
				wallNormal = hit.normal;
				SurfacePoint = hit.point;
				verticalVelocity = 0f;
				clingLeftGround = false;
				leftNormal = Vector3.zero;
				float num = Mathf.Max(transform.lossyScale.y, 0.0001f);
				clingWallDistance = hit.distance + controller.radius * num * 0.9f;
				return;
			}
			float num2 = 4f * (running ? 1.6f : 1f) * moveSpeedScale;
			if (IsGrounded && !flag)
			{
				LandedThisTick = !wasGrounded;
				wasGrounded = true;
				airJumpsUsed = 0;
				coyoteUntil = Time.unscaledTime + coyoteSeconds;
				verticalVelocity = -1f;
				if (jumpPressed)
				{
					PerformGroundJump();
				}
			}
			else if (jumpPressed && Time.unscaledTime <= coyoteUntil)
			{
				wasGrounded = false;
				PerformGroundJump();
			}
			else if (jumpPressed && airJumpsUsed < maxAirJumps)
			{
				wasGrounded = false;
				airJumpsUsed++;
				verticalVelocity = JumpSpeed;
				AirJumpedThisTick = true;
			}
			else
			{
				wasGrounded = false;
				verticalVelocity += -20f * deltaTime;
			}
			Vector3 vector2 = vector * num2 + Vector3.up * verticalVelocity;
			controller.Move(vector2 * deltaTime);
			Vector3 forward = ((FacingDirection.sqrMagnitude > 0.0001f) ? FacingDirection : vector);
			if (forward.sqrMagnitude > 0.0001f)
			{
				Quaternion b = Quaternion.LookRotation(forward, Vector3.up) * FacingOffset;
				transform.rotation = Quaternion.Slerp(transform.rotation, b, 12f * deltaTime);
			}
		}

		private void TickClinging(CharacterController controller, Vector3 rawCamForward, Vector2 input2D, bool jumpPressed, bool running, float deltaTime)
		{
			Transform transform = controller.transform;
			if (jumpPressed)
			{
				IsClinging = false;
				verticalVelocity = 0f;
				ReleasedThisTick = true;
				controller.Move(wallNormal * 9f * deltaTime);
				return;
			}
			float num = Mathf.Max(transform.lossyScale.y, 0.0001f);
			float num2 = controller.radius * num;
			Vector3 vector = transform.TransformPoint(controller.center);
			float num3 = num2 * 0.8f;
			ClingBasis(wallNormal, rawCamForward, transform.forward, out var planeUp, out var planeRight, out var cameraRelative);
			Vector3 vector2 = planeUp * input2D.y + planeRight * input2D.x;
			if (vector2.sqrMagnitude > 1f)
			{
				vector2.Normalize();
			}
			float num4 = BodyExtent(controller, num, transform, vector2) + 0.05f;
			if (vector2.sqrMagnitude > 0.0001f && TrySphereCast(transform, vector, num3, vector2, Mathf.Max(num4 - num3, 0.01f), ClimbableLayers, out var result) && Vector3.Dot(result.normal, Vector3.up) < 0.26f && Vector3.Dot(result.normal, wallNormal) < 0.7f && Vector3.Dot(result.normal, vector2.normalized) < -0.5f && (grabRequest > 0f || (grabHeld && !ReturningTo(result.normal, vector))) && MapBounds.IsInsidePlayArea(result.point + result.normal * CapsuleExtent(controller, num, result.normal)))
			{
				leftNormal = wallNormal;
				leftPoint = vector;
				wallNormal = result.normal;
				clingWallDistance = result.distance + num3;
				SurfacePoint = result.point;
				GrabbedThisTick = true;
				clingLeftGround = false;
				grabRequest = 0f;
				ClingBasis(wallNormal, rawCamForward, transform.forward, out planeUp, out planeRight, out cameraRelative);
				vector2 = planeUp * input2D.y + planeRight * input2D.x;
				if (vector2.sqrMagnitude > 1f)
				{
					vector2.Normalize();
				}
			}
			float num5 = CapsuleExtent(controller, num, -wallNormal);
			float num6 = Mathf.Clamp(clingWallDistance, num5, Mathf.Max(1.5f, num5)) + 0.3f;
			if (!TrySphereCast(transform, vector, num3, -wallNormal, num6, ClimbableLayers, out var result2) || Vector3.Dot(result2.normal, Vector3.up) >= 0.5f || Vector3.Dot(result2.normal, wallNormal) < 0.85f)
			{
				if (!TryWrapAroundEdge(controller, transform, num, vector, vector2))
				{
					IsClinging = false;
					verticalVelocity = 0f;
					ReleasedThisTick = true;
				}
				return;
			}
			wallNormal = result2.normal;
			SurfacePoint = result2.point;
			clingWallDistance = result2.distance + num3;
			Quaternion quaternion = Quaternion.LookRotation(-wallNormal, FacingAlong(transform, planeUp, wallNormal, vector2, cameraRelative));
			transform.rotation = Quaternion.Slerp(transform.rotation, quaternion * FacingOffset, 12f * deltaTime);
			Vector3 vector3 = Vector3.Project(result2.point + wallNormal * num5 - vector, wallNormal);
			vector3 = Vector3.ClampMagnitude(vector3, num6);
			float num7 = 3f * (running ? 1.6f : 1f);
			controller.Move(vector2 * num7 * deltaTime + vector3);
			if (FloorUnder(transform, controller, out var hit) && Vector3.Dot(hit.normal, Vector3.up) >= 0.5f)
			{
				if (clingLeftGround)
				{
					IsClinging = false;
					IsGrounded = true;
					verticalVelocity = -1f;
					ReleasedThisTick = true;
					return;
				}
			}
			else
			{
				clingLeftGround = true;
			}
			IsGrounded = false;
		}

		private bool TryWrapAroundEdge(CharacterController controller, Transform t, float scale, Vector3 worldCenter, Vector3 climbDir)
		{
			if (climbDir.sqrMagnitude < 0.0001f)
			{
				return false;
			}
			Vector3 normalized = climbDir.normalized;
			float num = controller.radius * scale;
			float num2 = 0.02f;
			float num3 = CapsuleExtent(controller, scale, -wallNormal);
			Vector3 origin = worldCenter + normalized * num * 0.5f - wallNormal * (num3 + num2 + 0.03f);
			float maxDistance = num * 2f + 0.3f;
			if (!TrySphereCast(t, origin, num2, -normalized, maxDistance, -1, out var result) || Vector3.Dot(result.normal, normalized) < 0.7f)
			{
				return false;
			}
			float num4 = Vector3.Dot(result.normal, Vector3.up);
			bool flag = num4 >= 0.5f;
			bool flag2 = num4 < 0.26f;
			if (!flag && !flag2)
			{
				return false;
			}
			if (!flag && ((int)ClimbableLayers & (1 << result.collider.gameObject.layer)) == 0)
			{
				return false;
			}
			if (!flag && !grabHeld && grabRequest <= 0f)
			{
				return false;
			}
			float num5 = CapsuleExtent(controller, scale, result.normal);
			float num6 = (flag ? (controller.skinWidth * scale + 0.03f) : 0.03f);
			Vector3 vector = worldCenter - wallNormal * num3;
			float num7 = Vector3.Dot(result.point - vector, wallNormal);
			Vector3 origin2 = result.point - normalized * 0.03f - wallNormal * (2f + num7);
			float num8 = 2f;
			if (TryRaycast(t, origin2, wallNormal, 2f, -1, out var result2) && Vector3.Dot(result2.normal, wallNormal) < -0.7f)
			{
				num8 = 2f - result2.distance;
			}
			float num9 = Mathf.Min(num, num8 * 0.5f);
			Vector3 vector2 = result.point + result.normal * (num5 + num6) - wallNormal * (num9 + num7);
			if (!MapBounds.IsInsidePlayArea(vector2) || !IsCapsuleClearAt(t, controller, scale, vector2))
			{
				return false;
			}
			Vector3 vector3 = vector2 - worldCenter;
			Vector3 vector4 = Vector3.Project(vector3, normalized);
			controller.Move(vector4);
			controller.Move(vector3 - vector4);
			if (flag)
			{
				IsClinging = false;
				IsGrounded = true;
				verticalVelocity = -1f;
				ReleasedThisTick = true;
				return true;
			}
			wallNormal = result.normal;
			clingWallDistance = num5 + num6;
			SurfacePoint = result.point;
			grabRequest = 0f;
			IsGrounded = false;
			return true;
		}

		private static bool TryRaycast(Transform self, Vector3 origin, Vector3 direction, float maxDistance, int layerMask, out RaycastHit result)
		{
			int num = Physics.RaycastNonAlloc(origin, direction, probeHits, maxDistance, layerMask, QueryTriggerInteraction.Ignore);
			float num2 = float.MaxValue;
			result = default(RaycastHit);
			bool result2 = false;
			for (int i = 0; i < num; i++)
			{
				Transform transform = probeHits[i].transform;
				if (!(transform == self) && !transform.IsChildOf(self) && probeHits[i].distance < num2)
				{
					num2 = probeHits[i].distance;
					result = probeHits[i];
					result2 = true;
				}
			}
			return result2;
		}

		private static bool IsOverhead(Vector3 normal)
		{
			return normal.y < -0.5f;
		}

		private bool ReturningTo(Vector3 normal, Vector3 worldCenter)
		{
			if (leftNormal.sqrMagnitude > 0.5f && Vector3.Dot(normal, leftNormal) > 0.7f)
			{
				return (worldCenter - leftPoint).sqrMagnitude < 2.25f;
			}
			return false;
		}

		private static bool IsCapsuleClearAt(Transform self, CharacterController controller, float scale, Vector3 center)
		{
			float num = controller.radius * scale;
			float num2 = Mathf.Max(controller.height * scale * 0.5f - num, 0f);
			float radius = Mathf.Max(num - controller.skinWidth * scale - 0.01f, num * 0.5f);
			Vector3 vector = Vector3.up * num2;
			int num3 = Physics.OverlapCapsuleNonAlloc(center - vector, center + vector, radius, overlapHits, -1, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < num3; i++)
			{
				Transform transform = overlapHits[i].transform;
				if (!(transform == self) && !transform.IsChildOf(self))
				{
					return false;
				}
			}
			return true;
		}

		private Vector3 FacingAlong(Transform t, Vector3 planeUp, Vector3 normal, Vector3 climbDir, bool cameraRelative)
		{
			if (!cameraRelative)
			{
				return planeUp;
			}
			if (climbDir.sqrMagnitude > 0.0001f)
			{
				Vector3 vector = Vector3.ProjectOnPlane(climbDir, normal);
				if (vector.sqrMagnitude > 0.0001f)
				{
					return vector.normalized;
				}
			}
			Vector3 vector2 = Vector3.ProjectOnPlane(t.rotation * Quaternion.Inverse(FacingOffset) * Vector3.up, normal);
			if (!(vector2.sqrMagnitude > 0.0001f))
			{
				return planeUp;
			}
			return vector2.normalized;
		}

		private void DetectGround(Transform t, CharacterController controller)
		{
			IsGrounded = FloorUnder(t, controller, out var _);
		}

		private static bool FloorUnder(Transform t, CharacterController controller, out RaycastHit hit)
		{
			float num = Mathf.Max(t.lossyScale.y, 0.0001f);
			float num2 = controller.radius * num;
			float num3 = controller.height * num;
			Vector3 vector = t.TransformPoint(controller.center);
			float radius = num2 * 0.9f;
			float num4 = Mathf.Max(num3 * 0.5f - num2, 0f);
			Vector3 origin = vector - Vector3.up * num4;
			float maxDistance = num2 * 0.5f + 0.1f;
			return TrySphereCast(t, origin, radius, Vector3.down, maxDistance, -1, out hit);
		}

		private static void ClingBasis(Vector3 normal, Vector3 cameraForward, Vector3 currentForward, out Vector3 planeUp, out Vector3 planeRight, out bool cameraRelative)
		{
			cameraRelative = false;
			planeUp = Vector3.ProjectOnPlane(Vector3.up, normal);
			Vector3 lhs = normal;
			if (planeUp.sqrMagnitude < 0.0001f)
			{
				planeUp = Vector3.ProjectOnPlane(cameraForward, normal);
				lhs = Vector3.up;
				cameraRelative = true;
			}
			if (planeUp.sqrMagnitude < 0.0001f)
			{
				planeUp = Vector3.ProjectOnPlane(currentForward, normal);
			}
			if (planeUp.sqrMagnitude < 0.0001f)
			{
				planeUp = Vector3.ProjectOnPlane(Vector3.forward, normal);
			}
			planeUp = planeUp.normalized;
			planeRight = Vector3.Cross(lhs, planeUp).normalized;
		}

		private float BodyExtent(CharacterController controller, float scale, Transform t, Vector3 worldDirection)
		{
			Bounds? bodyBoundsLocal = BodyBoundsLocal;
			if (bodyBoundsLocal.HasValue)
			{
				Bounds valueOrDefault = bodyBoundsLocal.GetValueOrDefault();
				Vector3 rhs = t.InverseTransformDirection(worldDirection.normalized);
				return Mathf.Max((Vector3.Dot(valueOrDefault.center - controller.center, rhs) + Mathf.Abs(rhs.x) * valueOrDefault.extents.x + Mathf.Abs(rhs.y) * valueOrDefault.extents.y + Mathf.Abs(rhs.z) * valueOrDefault.extents.z) * scale, CapsuleExtent(controller, scale, worldDirection));
			}
			return CapsuleExtent(controller, scale, worldDirection);
		}

		private static float CapsuleExtent(CharacterController controller, float scale, Vector3 direction)
		{
			float num = controller.radius * scale;
			float num2 = Mathf.Max(controller.height * scale * 0.5f - num, 0f);
			return num + Mathf.Abs(direction.normalized.y) * num2;
		}

		public float Headroom(CharacterController controller, float wanted, float alreadyRisen)
		{
			if (controller == null || wanted <= 0f)
			{
				return 0f;
			}
			Transform transform = controller.transform;
			float num = Mathf.Max(transform.lossyScale.y, 0.0001f);
			float num2 = controller.radius * num;
			float num3 = Mathf.Max(controller.height * num * 0.5f - num2, 0f);
			Vector3 origin = transform.TransformPoint(controller.center) + Vector3.up * (num3 - alreadyRisen * num);
			float num4 = num2 * 0.9f;
			float num5 = num2 - num4;
			if (!TrySphereCast(transform, origin, num4, Vector3.up, wanted + num5, -1, out var result))
			{
				return wanted;
			}
			return Mathf.Clamp(result.distance - num5, 0f, wanted);
		}

		private static bool TryDetectClimbableWall(Transform t, CharacterController controller, Vector3 moveDir, int climbableLayers, out RaycastHit hit)
		{
			hit = default(RaycastHit);
			if (moveDir.sqrMagnitude < 0.0001f)
			{
				return false;
			}
			float num = Mathf.Max(t.lossyScale.y, 0.0001f);
			float num2 = controller.radius * num;
			Vector3 origin = t.TransformPoint(controller.center);
			float radius = num2 * 0.9f;
			float maxDistance = num2 + 0.15f;
			if (TrySphereCast(t, origin, radius, moveDir, maxDistance, climbableLayers, out hit))
			{
				return Vector3.Dot(hit.normal, Vector3.up) < 0.26f;
			}
			return false;
		}

		private static bool TrySphereCast(Transform self, Vector3 origin, float radius, Vector3 direction, float maxDistance, int layerMask, out RaycastHit result)
		{
			int num = Physics.SphereCastNonAlloc(origin, radius, direction, probeHits, maxDistance, layerMask, QueryTriggerInteraction.Ignore);
			float num2 = float.MaxValue;
			result = default(RaycastHit);
			bool result2 = false;
			for (int i = 0; i < num; i++)
			{
				Transform transform = probeHits[i].transform;
				if (!(transform == self) && !transform.IsChildOf(self) && IsUsableSweepHit(probeHits[i]) && probeHits[i].distance < num2)
				{
					num2 = probeHits[i].distance;
					result = probeHits[i];
					result2 = true;
				}
			}
			return result2;
		}

		private static bool IsUsableSweepHit(RaycastHit hit)
		{
			if (hit.distance > 0f)
			{
				return hit.normal.sqrMagnitude > 0.5f;
			}
			return false;
		}
	}
}
