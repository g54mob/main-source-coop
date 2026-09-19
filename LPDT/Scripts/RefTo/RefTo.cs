using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class RefTo<TRefType, THostType> where TRefType : class where THostType : UnityEngine.Object
{
	[SerializeField]
	private THostType _host;

	[SerializeField]
	private long _referenceId;

	private WeakReference<TRefType> _cache;

	public THostType Host => _host;

	public long ReferenceId => _referenceId;

	public TRefType Get()
	{
		if (_host != null)
		{
			WeakReference<TRefType> cache = _cache;
			if (cache != null && cache.TryGetTarget(out var target))
			{
				return target;
			}
			object managedReference = ManagedReferenceUtility.GetManagedReference(_host, _referenceId);
			TRefType target2 = managedReference as TRefType;
			_cache = new WeakReference<TRefType>(target2);
			return managedReference as TRefType;
		}
		return null;
	}

	internal RefTo(THostType host, long referenceId)
	{
		_host = host;
		_referenceId = referenceId;
	}

	public RefTo<TRefType, THostType> CopyWithNewHost(THostType host)
	{
		return new RefTo<TRefType, THostType>(host, _referenceId);
	}
}
[Serializable]
public sealed class RefTo<TRefType> where TRefType : class
{
	[SerializeField]
	private UnityEngine.Object _host;

	[SerializeField]
	private long _referenceId;

	private WeakReference<TRefType> _cache;

	public UnityEngine.Object Host => _host;

	public long ReferenceId => _referenceId;

	public TRefType Get()
	{
		if (_host != null)
		{
			WeakReference<TRefType> cache = _cache;
			if (cache != null && cache.TryGetTarget(out var target))
			{
				return target;
			}
			object managedReference = ManagedReferenceUtility.GetManagedReference(_host, _referenceId);
			TRefType target2 = managedReference as TRefType;
			_cache = new WeakReference<TRefType>(target2);
			return managedReference as TRefType;
		}
		return null;
	}

	internal RefTo(UnityEngine.Object host, long referenceId)
	{
		_host = host;
		_referenceId = referenceId;
	}

	public RefTo<TRefType> CopyWithNewHost(UnityEngine.Object host)
	{
		return new RefTo<TRefType>(_host, _referenceId);
	}
}
