using UnityEngine;
using pworld.Scripts.Extensions;

public class DealSelectionUi : MonoBehaviour
{
	public SFX_Instance click;

	public Transform content;

	public GameObject dealPrefab;

	public DIFFICULTY difficulty;

	public void LoadNewDeal(NetworkDealBase deal)
	{
		content.KillAllChildren(destroyImmediate: true);
		Object.Instantiate(dealPrefab, content).GetComponent<DealProposalUI>().LoadDeal(deal);
		click.Play(base.transform.position);
	}
}
