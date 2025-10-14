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
		private readonly HashSet<GameObject> _collisionEnterQueue = new();

		private IEngineObject _self;
		private IRigidbody _cachedRigidbody;
		private ITransform _cachedTransform;
		private IEngineAudioSource _cachedAudio;
		private Boolean _rigidbodyCached;
		private Boolean _transformCached;
		private Boolean _audioCached;
		protected Boolean _isRuntimeContext;
		public Boolean IsScheduledForDestruction { get; private set; }

		public IScratchRunner Runner => _owner;
		public IEngineCamera ActiveCamera => _owner?.ActiveCamera;
		public IRigidbody Rigidbody
		{
			get
			{
				if (_isRuntimeContext)
					throw new Exception("Runtime context does not have Rigidbody");

				if (!_rigidbodyCached)
				{
					var rb = _owner.GetComponent<Rigidbody>();
					_cachedRigidbody = rb != null ? new ScratchRigidbody(rb) : null;
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
					_cachedTransform = new ScratchTransform(_owner.transform);
					_transformCached = true;
				}
				return _cachedTransform;
			}
		}

		public IEngineAudioSource Audio
		{
			get
			{
				if (!_audioCached)
				{
					var src = _owner.GetComponent<AudioSource>();
					_cachedAudio = src != null ? new ScratchAudioSource(src) : null;
					_audioCached = true;
				}
				return _cachedAudio;
			}
		}

		public IEngineObject Self => _self ??= new ScratchGameObject(_owner.gameObject);

		public ScratchBehaviourContext(ScratchBehaviour owner) => _owner = owner;

		public IEngineHUD GetEngineHUD() => _owner?.HUD;

		public IEngineMenu GetEngineMenu() => _owner?.Menu;

		public Boolean QueryCollisionEnterEvents(String nameFilter, String tagFilter)
		{
			foreach (var other in _collisionEnterQueue)
			{
				var nameOk = nameFilter == null || String.Equals(other.name, nameFilter, StringComparison.InvariantCulture);
				var tagOk = tagFilter == null || other.CompareTag(tagFilter);
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
				var engineObject = new ScratchGameObject(childTransform.gameObject);
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
