using System;
using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;

namespace Features.MouseVisibilityModule.Scripts
{
	public class MouseVisibilityModel : ISessionCleanup
	{
		private readonly Dictionary<int, MouseVisibilityRequest> _mouseVisibilityRequests = new Dictionary<int, MouseVisibilityRequest>();

		private MouseVisibilityRequest _priorityMouseVisibility;

		private bool _isMouseVisibleOnBlocked;

		private bool _isMouseVisibleBlocked;

		public Action OnMouseVisibilityChanged;

		public Action OnsMouseVisibleOnBlocked;

		public Action OnMouseVisibleBlockedChanged;

		public MouseVisibilityRequest PriorityMouseVisibility => _priorityMouseVisibility;

		public bool IsMouseVisibleOnBlocked
		{
			get
			{
				return _isMouseVisibleOnBlocked;
			}
			set
			{
				_isMouseVisibleOnBlocked = value;
				OnsMouseVisibleOnBlocked?.Invoke();
			}
		}

		public bool IsMouseVisibleBlocked
		{
			get
			{
				return _isMouseVisibleBlocked;
			}
			set
			{
				_isMouseVisibleBlocked = value;
				OnMouseVisibleBlockedChanged?.Invoke();
			}
		}

		public MouseVisibilityModel()
		{
			_mouseVisibilityRequests.Add(0, new MouseVisibilityRequest(-1, isMouseVisible: true));
			UpdatePriorityMouseVisibility();
		}

		public void AddMouseVisibilityRequest(MouseVisibilityRequest request)
		{
			_mouseVisibilityRequests[request.Priority] = request;
			UpdatePriorityMouseVisibility();
		}

		public void RemoveMouseVisibilityRequest(MouseVisibilityRequest request)
		{
			_mouseVisibilityRequests.Remove(request.Priority);
			UpdatePriorityMouseVisibility();
		}

		public bool IsMouseVisibilityRequestsContains(int priority)
		{
			return _mouseVisibilityRequests.ContainsKey(priority);
		}

		public bool IsMouseVisibilityRequestsContains(MouseVisibilityRequest request)
		{
			if (_mouseVisibilityRequests.TryGetValue(request.Priority, out var value) && value == request)
			{
				return true;
			}
			return false;
		}

		private void UpdatePriorityMouseVisibility()
		{
			MouseVisibilityRequest mouseVisibilityRequest = null;
			foreach (MouseVisibilityRequest value in _mouseVisibilityRequests.Values)
			{
				if (mouseVisibilityRequest == null || value.Priority > mouseVisibilityRequest.Priority)
				{
					mouseVisibilityRequest = value;
				}
			}
			_priorityMouseVisibility = mouseVisibilityRequest;
			OnMouseVisibilityChanged?.Invoke();
		}

		public void Cleanup()
		{
			_mouseVisibilityRequests.Clear();
			_mouseVisibilityRequests.Add(0, new MouseVisibilityRequest(-1, isMouseVisible: true));
			_isMouseVisibleBlocked = false;
			_isMouseVisibleOnBlocked = false;
			UpdatePriorityMouseVisibility();
		}
	}
}
