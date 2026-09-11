using System.Collections;
using UnityEngine;

public class MonsterAnimationHandler : MonoBehaviour
{
	private Player player;

	public Animator animator;

	private Bot bot;

	public bool strafeMovement;

	private static readonly int MoveInputXProp = Animator.StringToHash("Move Input X");

	private static readonly int MoveInputYProp = Animator.StringToHash("Move Input Y");

	private static readonly int MovementTypeProp = Animator.StringToHash("Movement Type");

	private static readonly int SuspicionValueProp = Animator.StringToHash("Suspicion Value");

	private static readonly int TargetAngleProp = Animator.StringToHash("Target Angle");

	private static readonly int GrabProp = Animator.StringToHash("Grab");

	private static readonly int AttackingProp = Animator.StringToHash("Attacking");

	private static readonly int TurnProp = Animator.StringToHash("Turn");

	private static readonly int AggroProp = Animator.StringToHash("Aggro");

	private static readonly int AttackTypeProp = Animator.StringToHash("Attack Type");

	private static readonly int DamageProp = Animator.StringToHash("Damage");

	private bool HasMoveInputXProp;

	private bool HasMoveInputYProp;

	private bool HasMovementTypeProp;

	private bool HasSuspicionValueProp;

	private bool HasTargetAngleProp;

	private bool HasGrabProp;

	private bool HasAttackingProp;

	private bool HasTurnProp;

	private bool HasAggroProp;

	private bool HasAttackTypeProp;

	private bool HasDamageProp;

	private bool grabbing;

	private void Start()
	{
		player = GetComponent<Player>();
		animator = player.refs.animator;
		bot = GetComponentInChildren<Bot>();
		HasMoveInputXProp = CheckProp(MoveInputXProp, "Move Input X");
		HasMoveInputYProp = CheckProp(MoveInputYProp, "Move Input Y");
		HasMovementTypeProp = CheckProp(MovementTypeProp, "Movement Type");
		HasSuspicionValueProp = CheckProp(SuspicionValueProp, "Suspicion Value");
		HasTargetAngleProp = CheckProp(TargetAngleProp, "Target Angle");
		HasGrabProp = CheckProp(GrabProp, "Grab");
		HasAttackingProp = CheckProp(AttackingProp, "Attacking");
		HasTurnProp = CheckProp(TurnProp, "Turn");
		HasAggroProp = CheckProp(AggroProp, "Aggro");
		HasAttackTypeProp = CheckProp(AttackTypeProp, "Attack Type");
		HasDamageProp = CheckProp(DamageProp, "Damage");
	}

	private void LateUpdate()
	{
		HandleAnimationTarget();
		SetAnimatorValues();
	}

	private bool CheckProp(int prop, string name)
	{
		AnimatorControllerParameter[] parameters = animator.parameters;
		for (int i = 0; i < parameters.Length; i++)
		{
			if (parameters[i].nameHash == prop)
			{
				return true;
			}
		}
		Debug.LogWarning("MonsterAnimationHandler animator on " + this?.ToString() + " missing property! Prop name: " + name, this);
		return false;
	}

	private void SetAnimatorValues()
	{
		if (strafeMovement)
		{
			if (HasMoveInputXProp)
			{
				animator.SetFloat(MoveInputXProp, bot.syncData.movementInput.x);
			}
			if (HasMoveInputYProp)
			{
				animator.SetFloat(MoveInputYProp, bot.syncData.movementInput.y);
			}
		}
		if (HasMovementTypeProp)
		{
			if (bot.syncData.movementInput == Vector2.zero)
			{
				animator.SetInteger(MovementTypeProp, 0);
			}
			else if (bot.syncData.movementInput.y < -0.1f)
			{
				if (bot.syncData.sprint)
				{
					animator.SetInteger(MovementTypeProp, -2);
				}
				else
				{
					animator.SetInteger(MovementTypeProp, -1);
				}
			}
			else if (bot.syncData.sprint)
			{
				animator.SetInteger(MovementTypeProp, 2);
			}
			else
			{
				animator.SetInteger(MovementTypeProp, 1);
			}
		}
		if (HasSuspicionValueProp)
		{
			animator.SetFloat(SuspicionValueProp, bot.suspicionValue);
		}
		if (HasTargetAngleProp)
		{
			animator.SetFloat(TargetAngleProp, bot.targetAngle);
		}
		if (HasGrabProp)
		{
			animator.SetBool(GrabProp, grabbing);
		}
		if (HasAttackingProp)
		{
			animator.SetBool(AttackingProp, bot.attacking);
		}
		if (HasTurnProp)
		{
			animator.SetFloat(TurnProp, bot.turnVel);
		}
		if (HasAggroProp)
		{
			animator.SetBool(AggroProp, bot.aggro);
		}
		if (HasAttackTypeProp)
		{
			animator.SetInteger(AttackTypeProp, bot.attackType);
		}
		if (HasDamageProp)
		{
			animator.SetBool(DamageProp, bot.hurt);
		}
	}

	public void Grab()
	{
		StartCoroutine(IGrab());
		IEnumerator IGrab()
		{
			grabbing = true;
			yield return new WaitForSeconds(1f);
			grabbing = false;
		}
	}

	private void HandleAnimationTarget()
	{
		if (player.data.physicsAreReady)
		{
			player.data.targetHeight = Vector3.Project(player.refs.ragdoll.GetBodypart(BodypartType.Head).animationTarget.transform.position - player.refs.animatorTransform.position, player.refs.animatorTransform.up).magnitude;
			player.refs.animatorTransform.rotation = HelperFunctions.GetRotationWithUp(player.data.lookDirection, -player.data.gravityDirection);
		}
	}

	internal void PlayAnimation(string animName)
	{
		animator.Play(animName);
	}

	internal void SetFloat(string floatName, float floatValue)
	{
		animator.SetFloat(floatName, floatValue);
	}

	internal void SetFloat(int floatHash, float floatValue)
	{
		animator.SetFloat(floatHash, floatValue);
	}
}
