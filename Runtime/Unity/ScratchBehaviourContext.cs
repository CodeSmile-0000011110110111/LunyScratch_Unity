using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunyScratch
{
	/// <summary>
	/// Unity-specific implementation of IScratchContext.
	/// Provides component caching and child lookup for a GameObject.
	/// </summary>
	internal class ScratchBehaviourContext : IScratchContext
	{
		private readonly ScratchBehaviour _owner;
		private readonly Dictionary<String, IEngineObject> _childrenByName = new();

		private readonly List<GameObject> _collisionEnterQueue = new();
		protected Boolean _isRuntimeContext;
		private IEngineObject _self;
		private IRigidbody _cachedRigidbody;
		private ITransform _cachedTransform;
		private IEngineAudio _cachedAudio;
		private Boolean _rigidbodyCached;
		private Boolean _transformCached;
		private Boolean _audioCached;
		public Boolean IsScheduledForDestruction { get; private set; }

		public IScratchRunner Runner => _owner;
		public IRigidbody Rigidbody
		{
			get
			{
				if (_isRuntimeContext)
					throw new Exception("Runtime context does not have Rigidbody");

				if (!_rigidbodyCached)
				{
					var rb = _owner.GetComponent<Rigidbody>();
					_cachedRigidbody = rb != null ? new UnityRigidbody(rb) : null;
					_rigidbodyCached = true;
				}
				return _cachedRigidbody;
			}
		}

		public ITransform Transform
		{
			get
			{
				if (_isRuntimeContext)
					throw new Exception("Runtime context does not have Transform");

				if (!_transformCached)
				{
					_cachedTransform = new UnityTransform(_owner.transform);
					_transformCached = true;
				}
				return _cachedTransform;
			}
		}

		public IEngineAudio Audio
		{
			get
			{
				if (!_audioCached)
				{
					var src = _owner.GetComponent<AudioSource>();
					_cachedAudio = src != null ? new UnityEngineAudio(src) : null;
					_audioCached = true;
				}
				return _cachedAudio;
			}
		}

		public IEngineObject Self => _self ??= new UnityGameObject(_owner.gameObject);

		public ScratchBehaviourContext(ScratchBehaviour owner) => _owner = owner;

		public Boolean QueryCollisionEnterEvents(String nameFilter, String tagFilter)
		{
			for (var i = 0; i < _collisionEnterQueue.Count; i++)
			{
				var go = _collisionEnterQueue[i];
				var nameOk = nameFilter == null || String.Equals(go.name, nameFilter, StringComparison.InvariantCulture);
				var tagOk = tagFilter == null || go.CompareTag(tagFilter);
				if (nameOk && tagOk)
					return true;
			}
			return false;
		}

		public void ScheduleDestroy() => IsScheduledForDestruction = true;

		public IEngineObject FindChild(String childName)
		{
			if (_isRuntimeContext)
				throw new Exception("Runtime context does not have children");

			// Check cache first
			if (_childrenByName.TryGetValue(childName, out var cached))
				return cached;

			var transform = _owner.transform;

			// Find in direct children (not recursive)
			var childTransform = transform.Find(childName);

			// If not found in direct children, search recursively in all children
			if (childTransform == null)
			{
				var allChildren = _owner.GetComponentsInChildren<Transform>(true);
				foreach (var child in allChildren)
				{
					if (String.Equals(child.name, childName, StringComparison.InvariantCulture))
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
			Debug.LogWarning($"{_owner.gameObject.name} ({_owner.gameObject.GetInstanceID()}): could not find child named '{childName}'");
			return null;
		}

		internal void EnqueueCollisionEnter(GameObject other)
		{
			if (other != null)
				_collisionEnterQueue.Add(other);
		}

		public void ClearCollisionEventQueues() => _collisionEnterQueue.Clear();

		public void Dispose()
		{
			_self = null;

			_cachedRigidbody = null;
			_rigidbodyCached = false;

			_cachedTransform = null;
			_transformCached = false;

			_cachedAudio = null;
			_audioCached = false;

			_childrenByName.Clear();
			_collisionEnterQueue.Clear();
		}
	}
}
