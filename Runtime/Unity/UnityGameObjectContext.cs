
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunyScratch
{
	/// <summary>
	/// Unity-specific implementation of IScratchContext.
	/// Provides component caching and child lookup for a GameObject.
	/// </summary>
	internal sealed class UnityGameObjectContext : IScratchContext
	{
		private readonly MonoBehaviour _owner;
		private IRigidbody _cachedRigidbody;
		private ITransform _cachedTransform;
		private Boolean _rigidbodyCached;
		private Boolean _transformCached;
		private readonly Dictionary<String, IEngineObject> _childrenByName = new();

		public UnityGameObjectContext(MonoBehaviour owner) => _owner = owner;

		public void Dispose()
		{
			_childrenByName.Clear();
			_cachedRigidbody = null;
			_cachedTransform = null;
			_rigidbodyCached = false;
			_transformCached = false;
		}

		// IScratchContext implementation
		public T GetComponent<T>() where T : class => _owner.GetComponent<T>() as T;

		public T[] GetComponentsInChildren<T>() where T : class => _owner.GetComponentsInChildren<T>() as T[];

		public IRigidbody GetRigidbody()
		{
			if (!_rigidbodyCached)
			{
				var rb = _owner.GetComponent<Rigidbody>();
				_cachedRigidbody = rb != null ? new UnityRigidbody(rb) : null;
				_rigidbodyCached = true;
			}
			return _cachedRigidbody;
		}

		public ITransform GetTransform()
		{
			if (!_transformCached)
			{
				_cachedTransform = new UnityTransform(_owner.transform);
				_transformCached = true;
			}
			return _cachedTransform;
		}

		public IEngineObject FindChild(String childName)
		{
			// Check cache first
			if (_childrenByName.TryGetValue(childName, out var cached))
				return cached;

			var transform = _owner.transform;

			// Find in hierarchy (recursive)
			var childTransform = transform.Find(childName);
			
			// If not found with simple Find, search recursively in all children
			if (childTransform == null)
			{
				var allChildren = _owner.GetComponentsInChildren<Transform>(true);
				foreach (var child in allChildren)
				{
					if (child.name == childName)
					{
						childTransform = child;
						break;
					}
				}
			}

			// Cache and return
			if (childTransform != null)
			{
				var engineObject = new UnityGameObject(childTransform.gameObject);
				_childrenByName[childName] = engineObject;
				return engineObject;
			}

			// Cache null result to avoid repeated searches
			_childrenByName[childName] = null;
			Debug.LogWarning($"{_owner.gameObject.name}: could not find child named '{childName}'");
			return null;
		}
	}
}
