using System;
using UnityEngine;

namespace LunyScratch
{
	/// <summary>
	/// Base class for all Scratch-style behaviors.
	/// Provides local block execution context for the GameObject it's attached to.
	/// </summary>
	[Serializable] // required for Rider to not mark all subclasses' [SerializeField] as "redundant" (RIDER-18729)
	public abstract class ScratchBehaviour : MonoBehaviour, IScratchRunner
	{
		private protected readonly Table _variables = new();
		private BlockRunner _runner;
		private ScratchBehaviourContext _context;
		private ScratchHUD _scratchHUD;
		private ScratchMenu _scratchMenu;

		public Table Variables => _variables;
		public Table GlobalVariables => ScratchRuntime.Singleton.Variables; // redirect to runtime's Variables override
		public ScratchHUD HUD
		{
			get => ScratchRuntime.Singleton.HUD;
			set => ScratchRuntime.Singleton.HUD = value;
		}
		public ScratchMenu Menu
		{
			get => ScratchRuntime.Singleton.Menu;
			set => ScratchRuntime.Singleton.Menu = value;
		}
		public ScratchCamera ActiveCamera => ScratchRuntime.Singleton.ActiveCamera;

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
			_context = this is ScratchRuntime ? new ScratchRuntimeContext(this) : new ScratchBehaviourContext(this);
			_runner = new BlockRunner(_context);
			OnCreateComponent();
		}

		private void Start() {}

		private void FixedUpdate()
		{
			_runner.ProcessPhysicsUpdate(Time.fixedDeltaTime);
			OnFixedStep(Time.fixedDeltaTime);
		}

		private void Update()
		{
			_runner.ProcessUpdate(Time.deltaTime);
			OnUpdate(Time.deltaTime);
		}

		private void LateUpdate()
		{
			_context?.ClearCollisionEventQueues();

			if (_context.IsScheduledForDestruction)
			{
				//Debug.LogWarning($"Destroying: {gameObject.name} ({gameObject.GetInstanceID()})");
				Destroy(gameObject);
			}
			OnLateUpdate();
		}

		private void OnDestroy()
		{
			_runner.Dispose();
			_context.Dispose();
			OnDestroyComponent();
		}

		private void OnCollisionEnter(Collision other) => _context?.EnqueueCollisionEnter(other.gameObject);

		/// <summary>
		/// Override this instead of Awake to handle initialization in derived classes.
		/// </summary>
		protected virtual void OnCreateComponent() {}

		/// <summary>
		/// Override this instead of OnDestroy to handle cleanup in derived classes.
		/// </summary>
		protected virtual void OnDestroyComponent() {}

		protected virtual void OnFixedStep(Single fixedDeltaTime) {}
		protected virtual void OnUpdate(Single deltaTime) {}
		protected virtual void OnLateUpdate() {}
	}
}
