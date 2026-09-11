using UnityEngine;

public class RigCreatorCollider : MonoBehaviour
{
	private int frames;

	private void Start()
	{
		if (base.transform.IsChildOf(GetComponentInParent<Player>().refs.animatorTransform))
		{
			Object.Destroy(this);
			return;
		}
		Renderer component = GetComponent<Renderer>();
		if ((bool)component)
		{
			component.enabled = false;
		}
	}

	private void FixedUpdate()
	{
		if (frames > 1)
		{
			GetComponent<Collider>().enabled = true;
			base.enabled = false;
		}
		frames++;
	}

	public void Save()
	{
		GetComponentInParent<RigCreator>().ClearColliderData();
		RigCreatorCollider[] componentsInChildren = base.transform.root.GetComponentsInChildren<RigCreatorCollider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].DoSave();
		}
	}

	public void DoSave()
	{
		GetComponentInParent<RigCreator>().SaveBodypartCollider(base.transform.parent.name, base.transform);
	}
}
