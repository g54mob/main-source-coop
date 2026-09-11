using DefaultNamespace;
using UnityEngine;

public class BudgetCost : MonoBehaviour, IBudgetCost
{
	public int budgetCost;

	public float rarity = 1f;

	public int Cost => budgetCost;

	public float Rarity => rarity;

	GameObject IBudgetCost.gameObject => base.gameObject;
}
