using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LunyScratch
{
	/// <summary>
	/// Auto-instantiating singleton for executing Scratch blocks without GameObject context.
	/// </summary>
	[DefaultExecutionOrder(Int16.MinValue)]
	[AddComponentMenu("GameObject/")] // Do not list in "Add Component" menu
	[DisallowMultipleComponent]
	public sealed class ScratchRuntime : ScratchBehaviour, IEngineRuntime
	{
		private static ScratchRuntime s_Instance;
		private static Boolean s_Initialized;

		public static ScratchRuntime Singleton => s_Instance;

#if UNITY_EDITOR
		// required reset for 'disabled domain reload'
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void InitializeBeforeSceneLoad()
		{
			s_Instance = null;
			s_Initialized = false;
			GC.Collect();
		}
#endif

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void InitializeBeforeAwake()
		{
			if (s_Initialized)
				return;

			s_Initialized = true;

			var go = new GameObject(nameof(ScratchRuntime));
			s_Instance = go.AddComponent<ScratchRuntime>();
			DontDestroyOnLoad(go);

			// Load AssetRegistry from Resources and pass it to GameEngine
			var registry = Resources.Load<UnityAssetRegistry>("AssetRegistry");
			GameEngine.Initialize(s_Instance, new UnityActions(), registry);

			SceneManager.activeSceneChanged += OnActiveSceneChanged;
			SceneManager.sceneUnloaded += OnSceneUnloaded;
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		private static void OnSceneLoaded(Scene loadedScene, LoadSceneMode loadMode) =>
			Debug.Log($"OnSceneLoaded: {loadedScene} with mode={loadMode}");

		private static void OnSceneUnloaded(Scene unloadedScene) => Debug.Log($"OnSceneUnloaded: {unloadedScene}");

		private static void OnActiveSceneChanged(Scene previousScene, Scene activeScene) =>
			Debug.Log($"OnActiveSceneChanged from {previousScene} to {activeScene}");

		protected override void OnBehaviourDestroy()
		{
			SceneManager.activeSceneChanged -= OnActiveSceneChanged;
			SceneManager.sceneUnloaded -= OnSceneUnloaded;
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}
	}
}
