using UnityEngine;

namespace LunyScratch
{
	/// <summary>
	/// Base class for all Scratch-style behaviors.
	/// Provides local block execution context for the GameObject it's attached to.
	/// </summary>
	public abstract class ScratchBehaviour : MonoBehaviour, IScratchRunner
	{
		private readonly Table _variables = new();
		private BlockRunner _runner;
		private ScratchBehaviourContext _context;

		public Table Variables => _variables;

		// IScratchRunner implementation
		public void Run(params IScratchBlock[] blocks) => _runner.AddBlock(Blocks.Sequence(blocks));

		public void RunPhysics(params IScratchBlock[] blocks) => _runner.AddPhysicsBlock(Blocks.Sequence(blocks));

		public void RepeatForever(params IScratchBlock[] blocks) => _runner.AddBlock(Blocks.RepeatForever(blocks));

		public void RepeatForeverPhysics(params IScratchBlock[] blocks) => _runner.AddPhysicsBlock(Blocks.RepeatForever(blocks));

		// public void RepeatWhileTrue(Func<Boolean> condition, params IScratchBlock[] blocks) =>
		// 	_runner.AddBlock(new RepeatWhileTrueBlock(condition, blocks));
		//
		// public void RepeatUntilTrue(Func<Boolean> condition, params IScratchBlock[] blocks) =>
		// 	_runner.AddBlock(new RepeatUntilTrueBlock(condition, blocks));

		// public void When(Func<Boolean> condition, params IScratchBlock[] blocks) =>
		// 	_runner.AddBlock(Blocks.When(condition, blocks));

		public void When(EventBlock evt, params IScratchBlock[] blocks) => _runner.AddBlock(Blocks.When(evt, blocks));

		private void Awake()
		{
			_context = new ScratchBehaviourContext(this);
			_runner = new BlockRunner(_context);
			OnBehaviourAwake();
		}

		private void FixedUpdate() => _runner.ProcessPhysicsUpdate(Time.fixedDeltaTime);

		private void Update() => _runner.ProcessUpdate(Time.deltaTime);

		private void LateUpdate()
		{
			_context?.ClearCollisionEventQueues();

			if (_context.IsScheduledForDestruction)
			{
				//Debug.LogWarning($"Destroying: {gameObject.name} ({gameObject.GetInstanceID()})");
				Destroy(gameObject);
			}
		}

		private void OnDestroy()
		{
			_runner.Dispose();
			_context.Dispose();
			OnBehaviourDestroy();
		}

		private void OnCollisionEnter(Collision other) => _context?.EnqueueCollisionEnter(other.gameObject);

		/// <summary>
		/// Override this instead of Awake to handle initialization in derived classes.
		/// </summary>
		protected virtual void OnBehaviourAwake() {}

		/// <summary>
		/// Override this instead of OnDestroy to handle cleanup in derived classes.
		/// </summary>
		protected virtual void OnBehaviourDestroy() {}
	}
}
