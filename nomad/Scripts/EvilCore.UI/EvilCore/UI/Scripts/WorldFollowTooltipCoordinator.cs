using System.Collections.Generic;
using UnityEngine;

namespace EvilCore.UI.Scripts
{
	public class WorldFollowTooltipCoordinator : MonoBehaviour
	{
		[Tooltip("Vertical gap (canvas px) between stacked tooltips that share a target.")]
		[SerializeField]
		private float stackSpacing = 12f;

		private readonly List<WorldFollowTooltip> _active = new List<WorldFollowTooltip>();

		private readonly Dictionary<GameObject, List<WorldFollowTooltip>> _groups = new Dictionary<GameObject, List<WorldFollowTooltip>>();

		private readonly Stack<List<WorldFollowTooltip>> _listPool = new Stack<List<WorldFollowTooltip>>();

		public static WorldFollowTooltipCoordinator Instance { get; private set; }

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Object.Destroy(this);
			}
			else
			{
				Instance = this;
			}
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}

		public void Register(WorldFollowTooltip tooltip)
		{
			if (tooltip != null && !_active.Contains(tooltip))
			{
				_active.Add(tooltip);
			}
		}

		public void Unregister(WorldFollowTooltip tooltip)
		{
			_active.Remove(tooltip);
		}

		private void LateUpdate()
		{
			if (_active.Count == 0)
			{
				return;
			}
			BuildGroups();
			foreach (KeyValuePair<GameObject, List<WorldFollowTooltip>> group in _groups)
			{
				List<WorldFollowTooltip> value = group.Value;
				if (value.Count == 1)
				{
					WorldFollowTooltip worldFollowTooltip = value[0];
					if (worldFollowTooltip.TryComputeBaseAnchor(out var canvasPos))
					{
						worldFollowTooltip.ApplyAnchoredPosition(worldFollowTooltip.ClampToCanvas(canvasPos));
					}
					continue;
				}
				value.Sort(CompareByPriority);
				if (value[0].TryComputeBaseAnchor(out var canvasPos2))
				{
					Vector2 vector = value[0].ClampToCanvas(canvasPos2);
					value[0].ApplyAnchoredPosition(vector);
					Vector2 vector2 = vector;
					for (int i = 1; i < value.Count; i++)
					{
						WorldFollowTooltip worldFollowTooltip2 = value[i];
						Vector2 pos = new Vector2(canvasPos2.x, vector2.y - stackSpacing - worldFollowTooltip2.Size.y);
						pos = worldFollowTooltip2.ClampToCanvas(pos);
						worldFollowTooltip2.ApplyAnchoredPosition(pos);
						vector2 = pos;
					}
				}
			}
			ReleaseGroups();
		}

		private void BuildGroups()
		{
			for (int i = 0; i < _active.Count; i++)
			{
				WorldFollowTooltip worldFollowTooltip = _active[i];
				if (!(worldFollowTooltip == null) && worldFollowTooltip.IsFollowing)
				{
					GameObject target = worldFollowTooltip.Target;
					if (!_groups.TryGetValue(target, out var value))
					{
						value = ((_listPool.Count > 0) ? _listPool.Pop() : new List<WorldFollowTooltip>());
						value.Clear();
						_groups[target] = value;
					}
					value.Add(worldFollowTooltip);
				}
			}
		}

		private void ReleaseGroups()
		{
			foreach (KeyValuePair<GameObject, List<WorldFollowTooltip>> group in _groups)
			{
				_listPool.Push(group.Value);
			}
			_groups.Clear();
		}

		private static int CompareByPriority(WorldFollowTooltip a, WorldFollowTooltip b)
		{
			return a.StackPriority.CompareTo(b.StackPriority);
		}
	}
}
