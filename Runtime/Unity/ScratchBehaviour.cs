
using System;
using UnityEngine;

namespace LunyScratch
{
	/// <summary>
	/// Base class for all Scratch-style behaviors.
	/// Provides local block execution context for the GameObject it's attached to.
	/// </summary>
	public abstract class ScratchBehaviour : MonoBehaviour, IScratchRunner, IScratchContext
	{
		private BlockRunner _runner;
		private IRigidbody _cachedRigidbody;
		private ITransform _cachedTransform;
		private Boolean _rigidbodyCached;
		private Boolean _transformCached;

		private void Awake()
		{
			_runner = new BlockRunner(this);
			OnBehaviourAwake();
		}

		/// <summary>
		/// Override this instead of Awake to handle initialization in derived classes.
		/// </summary>
		protected virtual void OnBehaviourAwake() {}

		private void Update() => _runner.ProcessUpdate(Time.deltaTime);

		private void FixedUpdate() => _runner.ProcessPhysicsUpdate(Time.fixedDeltaTime);

		private void OnDestroy()
		{
			_runner.Dispose();
			OnBehaviourDestroy();
		}

		/// <summary>
		/// Override this instead of OnDestroy to handle cleanup in derived classes.
		/// </summary>
		protected virtual void OnBehaviourDestroy() {}

		// IScratchRunner implementation
		public void Run(params IScratchBlock[] blocks) => _runner.AddBlock(new SequenceBlock(blocks));

		public void RunPhysics(params IScratchBlock[] blocks) => _runner.AddPhysicsBlock(new SequenceBlock(blocks));

		public void RepeatForever(params IScratchBlock[] blocks) => _runner.AddBlock(new RepeatForeverBlock(blocks));

		public void RepeatForeverPhysics(params IScratchBlock[] blocks) => _runner.AddPhysicsBlock(new RepeatForeverBlock(blocks));

		public void RepeatWhileTrue(Func<Boolean> condition, params IScratchBlock[] blocks) =>
			_runner.AddBlock(new RepeatWhileTrueBlock(condition, blocks));

		public void RepeatUntilTrue(Func<Boolean> condition, params IScratchBlock[] blocks) =>
			_runner.AddBlock(new RepeatUntilTrueBlock(condition, blocks));

		// IScratchContext implementation - with proper caching and no null coalescing on Unity objects
		T IScratchContext.GetComponent<T>() => GetComponent<T>() as T;

		T[] IScratchContext.GetComponentsInChildren<T>() => GetComponentsInChildren<T>() as T[];

		IRigidbody IScratchContext.GetRigidbody()
		{
			if (!_rigidbodyCached)
			{
				var rb = GetComponent<Rigidbody>();
				_cachedRigidbody = rb != null ? new UnityRigidbody(rb) : null;
				_rigidbodyCached = true;
			}
			return _cachedRigidbody;
		}

		ITransform IScratchContext.GetTransform()
		{
			if (!_transformCached)
			{
				_cachedTransform = new UnityTransform(transform);
				_transformCached = true;
			}
			return _cachedTransform;
		}
	}
}
