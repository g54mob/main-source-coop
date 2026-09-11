using UnityEngine;

public static class FRILerp
{
	public static void PositionSpring(ref Vector3 currentPosition, Vector3 target, float spring, float drag, ref Vector3 velo)
	{
		Vector3 vector = target - currentPosition;
		velo = Lerp(velo, vector * spring, drag);
		currentPosition += velo * Time.deltaTime;
	}

	public static void DirectionSpring(ref Vector3 currentForward, Vector3 targetForward, float spring, float drag, ref Vector3 velo)
	{
		Vector3 vector = Vector3.Cross(currentForward, targetForward).normalized * Vector3.Angle(currentForward, targetForward);
		velo = Lerp(velo, vector * spring, drag);
		currentForward = Quaternion.Euler(velo * Time.deltaTime) * currentForward;
	}

	public static void RotationSpring(ref Vector3 currentForward, Vector3 targetForward, ref Vector3 currentUp, Vector3 targetUp, float spring, float drag, ref Vector3 velo)
	{
		Vector3 vector = Vector3.Cross(currentForward, targetForward).normalized * Vector3.Angle(currentForward, targetForward);
		vector += Vector3.Cross(currentUp, targetUp).normalized * (Vector3.Angle(currentUp, targetUp) * 0.5f);
		velo = Lerp(velo, vector * spring, drag);
		currentForward = Quaternion.Euler(velo * Time.deltaTime) * currentForward;
		currentUp = Quaternion.Euler(velo * Time.deltaTime) * currentUp;
	}

	public static Vector3 Lerp(Vector3 from, Vector3 target, float speed, bool useTimeScale = true)
	{
		return Vector3.Lerp(from, target, 1f - Mathf.Exp((0f - speed) * (useTimeScale ? Time.deltaTime : Time.unscaledDeltaTime)));
	}

	public static Vector3 PLerp(Vector3 from, Vector3 target, float speed, float dt)
	{
		return Vector3.Lerp(from, target, 1f - Mathf.Exp((0f - speed) * dt));
	}

	public static Quaternion PLerp(Quaternion from, Quaternion target, float speed, float dt)
	{
		return Quaternion.Lerp(from, target, 1f - Mathf.Exp((0f - speed) * dt));
	}

	public static float PLerp(float from, float target, float speed, float dt)
	{
		return Mathf.Lerp(from, target, 1f - Mathf.Exp((0f - speed) * dt));
	}

	public static Vector3 LerpFixed(Vector3 from, Vector3 target, float speed, bool useTimeScale = true)
	{
		return Vector3.Lerp(from, target, 1f - Mathf.Exp((0f - speed) * (useTimeScale ? Time.fixedDeltaTime : Time.unscaledDeltaTime)));
	}

	public static Vector3 LerpUnclamped(Vector3 from, Vector3 target, float speed)
	{
		return Vector3.LerpUnclamped(from, target, 1f - Mathf.Exp((0f - speed) * Time.deltaTime));
	}

	public static float Lerp(float from, float target, float speed, bool useTimeScale = true)
	{
		return Mathf.Lerp(from, target, 1f - Mathf.Exp((0f - speed) * (useTimeScale ? Time.fixedDeltaTime : Time.unscaledDeltaTime)));
	}

	public static float LerpUnclamped(float from, float target, float speed)
	{
		return Mathf.LerpUnclamped(from, target, 1f - Mathf.Exp((0f - speed) * Time.deltaTime));
	}

	public static Vector3 Slerp(Vector3 from, Vector3 target, float speed)
	{
		return Vector3.Slerp(from, target, 1f - Mathf.Exp((0f - speed) * Time.deltaTime));
	}

	public static Vector3 SlerpUnclamped(Vector3 from, Vector3 target, float speed)
	{
		return Vector3.SlerpUnclamped(from, target, 1f - Mathf.Exp((0f - speed) * Time.deltaTime));
	}

	public static Quaternion Lerp(Quaternion from, Quaternion target, float speed)
	{
		return Quaternion.Lerp(from, target, 1f - Mathf.Exp((0f - speed) * Time.deltaTime));
	}

	public static Quaternion LerpUnclamped(Quaternion from, Quaternion target, float speed)
	{
		return Quaternion.LerpUnclamped(from, target, 1f - Mathf.Exp((0f - speed) * Time.deltaTime));
	}
}
