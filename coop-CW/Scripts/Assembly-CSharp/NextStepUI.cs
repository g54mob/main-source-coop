using TMPro;
using UnityEngine;

public class NextStepUI : MonoBehaviour
{
	public CanvasGroup canvasGroup;

	public TextMeshProUGUI objectiveText;

	private Objective m_currentObjective;

	private void Awake()
	{
		LocalizationKeys.OnLanguageChanged += OnLanguageChanged;
	}

	private void OnDestroy()
	{
		LocalizationKeys.OnLanguageChanged -= OnLanguageChanged;
	}

	public void Show()
	{
		canvasGroup.alpha = 1f;
	}

	public void Hide()
	{
		canvasGroup.alpha = 0f;
	}

	public void SetData(Objective objective)
	{
		Show();
		if (m_currentObjective != objective)
		{
			objectiveText.text = objective.GetObjectiveDescription();
			m_currentObjective = objective;
		}
	}

	private void OnLanguageChanged()
	{
		if (m_currentObjective != null)
		{
			objectiveText.text = m_currentObjective.GetObjectiveDescription();
		}
	}
}
