using UnityEngine;

public class DebugMover : MonoBehaviour
{
	public float speed;

	public Animator anim;

	private void Update()
	{
		anim.SetBool("Move", Input.GetKey(KeyCode.CapsLock));
		if (Input.GetKeyDown(KeyCode.C))
		{
			anim.SetBool("Catch", !anim.GetBool("Catch"));
		}
		if (Input.GetKey(KeyCode.CapsLock) && !anim.GetBool("Catch"))
		{
			base.transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
			base.transform.LookAt(new Vector3(Camera.main.transform.position.x, base.transform.position.y, Camera.main.transform.position.z));
		}
	}
}
