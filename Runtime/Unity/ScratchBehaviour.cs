using System;
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
		private ScratchHUD _scratchHUD;
		private ScratchMenu _scratchMenu;

		public Table Variables => _variables;
		public ScratchHUD HUD
		{
			get => _scratchHUD == null ? _scratchHUD = TryFindSingleComponentInScene<ScratchHUD>() : _scratchHUD;
			set => _scratchHUD = value;
		}
		public ScratchMenu Menu
		{
			get => _scratchMenu == null ? _scratchMenu = TryFindSingleComponentInScene<ScratchMenu>() : _scratchMenu;
			set => _scratchMenu = value;
		}

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
			OnBehaviourAwake();
		}

		private void FixedUpdate()
		{
			_runner.ProcessPhysicsUpdate(Time.fixedDeltaTime);
			OnFixedUpdate(Time.fixedDeltaTime);
		}

		protected virtual void OnFixedUpdate(Single fixedDeltaTime)
		{
		}

		private void Update()
		{
			_runner.ProcessUpdate(Time.deltaTime);
			OnUpdate(Time.deltaTime);
		}

		protected virtual void OnUpdate(Single deltaTime)
		{
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

		protected virtual void OnLateUpdate()
		{
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

		protected T TryFindSingleComponentInScene<T>() where T : Component
		{
			var components = FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			Debug.Log($"Try find {typeof(T)}, found: {components.Length}");
			if (components.Length > 0)
			{
				var moreThanOne = components.Length > 1;
				if (moreThanOne)
					Debug.LogWarning($"More than one {nameof(T)} found in scene");

				return components[0];
			}

			Debug.LogError($"No {nameof(T)} found in scene");
			return null;
		}
	}
}
