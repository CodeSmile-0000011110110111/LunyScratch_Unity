using System;
using Unity.Cinemachine;
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

		private ScratchHUD _scratchHUD;
		private ScratchMenu _scratchMenu;

		public static ScratchRuntime Singleton => s_Instance;
		// hide ScratchBehaviour's GlobalVariables or else stackoverflow
		public new Table GlobalVariables => _variables;

		public new ScratchHUD HUD
		{
			get => _scratchHUD == null ? _scratchHUD = TryFindSingletonComponentInScene<ScratchHUD>() : _scratchHUD;
			set => _scratchHUD = value;
		}
		public new ScratchMenu Menu
		{
			get => _scratchMenu == null ? _scratchMenu = TryFindSingletonComponentInScene<ScratchMenu>() : _scratchMenu;
			set => _scratchMenu = value;
		}
		public new ScratchCamera ActiveCamera
		{
			get
			{
				var brain = Camera.main != null ? Camera.main.GetComponent<CinemachineBrain>() : null;
				var vcam = brain != null ? brain.ActiveVirtualCamera as CinemachineCamera : null;
				return vcam != null ? new ScratchCamera(vcam) : null;
			}
		}

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
			var registry = Resources.Load<ScratchAssetRegistry>("AssetRegistry");
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

		private T TryFindSingletonComponentInScene<T>() where T : Component
		{
			var components = FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None);
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

		protected override void OnDestroyComponent()
		{
			SceneManager.activeSceneChanged -= OnActiveSceneChanged;
			SceneManager.sceneUnloaded -= OnSceneUnloaded;
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}
	}
}
