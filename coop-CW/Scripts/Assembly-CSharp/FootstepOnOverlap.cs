using UnityEngine;

public class FootstepOnOverlap : MonoBehaviour
{
	public float delay = 0.1f;

	public SFXInstanceCollection[] stepSound;

	private float m_time;

	private float m_threshold = 0.001f;

	private Vector3 m_prevPosition;

	private Vector3 m_prevDirection;

	private Transform m_legRoot;

	private SFX_Instance[] m_sfx;

	[SerializeField]
	private int m_offset;

	private void Start()
	{
		BodypartAnimationTarget component;
		bool flag = base.transform.parent.TryGetComponent<BodypartAnimationTarget>(out component);
		base.enabled = stepSound.Length != 0 && !flag;
		m_sfx = stepSound[0].instances;
		m_offset = Random.Range(0, m_sfx.Length);
		if (!base.enabled)
		{
			return;
		}
		m_legRoot = base.transform;
		while ((object)m_legRoot.parent != null)
		{
			Transform parent = m_legRoot.parent;
			if (!parent.name.StartsWith("Spine") && !parent.name.StartsWith("Hip"))
			{
				m_legRoot = parent;
				continue;
			}
			break;
		}
	}

	private void Update()
	{
		if (Time.time - m_time < delay)
		{
			return;
		}
		Vector3 position = base.transform.position;
		Vector3 vector = position - m_legRoot.position;
		Vector3 prevDirection = vector - m_prevPosition;
		if (prevDirection.y > m_threshold && m_prevDirection.y < 0f - m_threshold)
		{
			SFX_Player.instance.PlaySFX(m_sfx[m_offset], position);
			if (++m_offset >= m_sfx.Length)
			{
				m_offset = 0;
			}
		}
		m_prevPosition = vector;
		m_prevDirection = prevDirection;
	}
}
