#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
#if UNITY_2021_1_OR_NEWER
using UnityEngine.UIElements;
#endif

namespace LunyScratch.Editor
{
	/// <summary>
	/// Builds/updates the UnityAssetRegistry ScriptableObject before playmode and before builds.
	/// Scans Assets/LunyScratch for supported asset types.
	/// </summary>
	[InitializeOnLoad]
	internal sealed class AssetRegistryBuilder : IPreprocessBuildWithReport
	{
		private const String RootFolder = "Assets/LunyScratch";
		private const String ResourcesFolder = RootFolder + "/Resources";
		private const String RegistryAssetPath = ResourcesFolder + "/AssetRegistry.asset";

		public Int32 callbackOrder => Int32.MinValue;

		private static void OnPlayModeChanged(PlayModeStateChange state)
		{
			if (state == PlayModeStateChange.ExitingEditMode)
				BuildOrUpdate();
		}

		private static void BuildOrUpdate()
		{
			TryCreateFolders();

			var registry = AssetDatabase.LoadAssetAtPath<UnityAssetRegistry>(RegistryAssetPath);
			if (registry == null)
			{
				registry = ScriptableObject.CreateInstance<UnityAssetRegistry>();
				AssetDatabase.CreateAsset(registry, RegistryAssetPath);
			}

			var prefabItems = new List<(String path, UnityPrefabAsset asset)>();
			var uiItems = new List<(String path, UnityUIAsset asset)>();
			var audioItems = new List<(String path, UnityAudioAsset asset)>();

			// Prefabs (GameObjects)
			foreach (var guid in AssetDatabase.FindAssets("t:GameObject", new[] { RootFolder }))
			{
				var path = AssetDatabase.GUIDToAssetPath(guid);
				var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
				if (go != null)
				{
					Debug.Log($"Adding prefab {go} at path: {path}");
					prefabItems.Add((path, new UnityPrefabAsset(go)));
				}
			}

			// UI (UXML VisualTreeAssets)
			foreach (var guid in AssetDatabase.FindAssets("t:VisualTreeAsset", new[] { RootFolder }))
			{
				var path = AssetDatabase.GUIDToAssetPath(guid);
				var ui = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
				if (ui != null)
					uiItems.Add((path, new UnityUIAsset(ui)));
			}

			// Audio (AudioClips)
			foreach (var guid in AssetDatabase.FindAssets("t:AudioClip", new[] { RootFolder }))
			{
				var path = AssetDatabase.GUIDToAssetPath(guid);
				var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
				if (clip != null)
					audioItems.Add((path, new UnityAudioAsset(clip)));
			}

			registry.SetPrefabsForEditor(prefabItems);
			registry.SetUIForEditor(uiItems);
			registry.SetAudioForEditor(audioItems);
			registry.BuildDictionaries();
			EditorUtility.SetDirty(registry);
			AssetDatabase.SaveAssetIfDirty(registry);
		}

		private static void TryCreateFolders()
		{
			if (!AssetDatabase.IsValidFolder(RootFolder))
			{
				var parts = RootFolder.Split('/');
				var current = parts[0]; // "Assets"
				for (var i = 1; i < parts.Length; i++)
				{
					var next = current + "/" + parts[i];
					if (!AssetDatabase.IsValidFolder(next))
						AssetDatabase.CreateFolder(current, parts[i]);
					current = next;
				}
			}
			if (!AssetDatabase.IsValidFolder(ResourcesFolder))
				AssetDatabase.CreateFolder(RootFolder, "Resources");
		}

		static AssetRegistryBuilder() => EditorApplication.playModeStateChanged += OnPlayModeChanged;

		public void OnPreprocessBuild(BuildReport report) => BuildOrUpdate();
	}
}
#endif
