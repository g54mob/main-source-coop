using UnityEngine;

public class RigCreatorJoint : MonoBehaviour
{
	internal RigCreatorBodypart targetPart;

	public RigCreator creator;

	private void Start()
	{
		Object.DestroyImmediate(this);
	}

	public void Save()
	{
		RigCreatorJoint[] componentsInChildren = base.transform.root.GetComponentsInChildren<RigCreatorJoint>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].DoSave();
		}
		RigCreatorCollider[] componentsInChildren2 = base.transform.root.GetComponentsInChildren<RigCreatorCollider>();
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].DoSave();
		}
	}

	public void DoSave()
	{
		creator.SaveJoint(targetPart, GetComponent<ConfigurableJoint>());
	}

	internal void Init(RigCreator rigCreator, RigCreatorBodypart rigCreatorBodypart)
	{
		creator = rigCreator;
		targetPart = rigCreatorBodypart;
	}
}
