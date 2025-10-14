#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
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

		private static String NormalizePath(String path)
		{
			if (String.IsNullOrEmpty(path))
				return path;

			// Ensure forward slashes (Unity asset paths use '/')
			var normalized = path.Replace('\\', '/');

			// Strip file extension
			var ext = Path.GetExtension(normalized);
			if (!String.IsNullOrEmpty(ext))
				normalized = normalized.Substring(0, normalized.Length - ext.Length);

			// Remove RootFolder prefix
			if (normalized.StartsWith(RootFolder, StringComparison.OrdinalIgnoreCase))
				normalized = normalized.Substring(RootFolder.Length);

			// Trim any leading/trailing separators
			normalized = normalized.Trim('/');
			return normalized;
		}

		private static void BuildOrUpdate()
		{
			TryCreateFolders();

			var registry = AssetDatabase.LoadAssetAtPath<ScratchAssetRegistry>(RegistryAssetPath);
			if (registry == null)
			{
				registry = ScriptableObject.CreateInstance<ScratchAssetRegistry>();
				AssetDatabase.CreateAsset(registry, RegistryAssetPath);
			}

			var prefabItems = new List<(String path, ScratchPrefabAsset asset)>();
			var uiItems = new List<(String path, ScratchUIAsset asset)>();
			var audioItems = new List<(String path, ScratchAudioAsset asset)>();

			// Prefabs (GameObjects)
			foreach (var guid in AssetDatabase.FindAssets("t:GameObject", new[] { RootFolder }))
			{
				var fullPath = AssetDatabase.GUIDToAssetPath(guid);
				var go = AssetDatabase.LoadAssetAtPath<GameObject>(fullPath);
				if (go != null)
				{
					var path = NormalizePath(fullPath);
					prefabItems.Add((path, new ScratchPrefabAsset(go)));
				}
			}

			// UI (UXML VisualTreeAssets)
			foreach (var guid in AssetDatabase.FindAssets("t:VisualTreeAsset", new[] { RootFolder }))
			{
				var fullPath = AssetDatabase.GUIDToAssetPath(guid);
				var ui = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(fullPath);
				if (ui != null)
				{
					var path = NormalizePath(fullPath);
					uiItems.Add((path, new ScratchUIAsset(ui)));
				}
			}

			// Audio (AudioClips)
			foreach (var guid in AssetDatabase.FindAssets("t:AudioClip", new[] { RootFolder }))
			{
				var fullPath = AssetDatabase.GUIDToAssetPath(guid);
				var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(fullPath);
				if (clip != null)
				{
					var path = NormalizePath(fullPath);
					audioItems.Add((path, new ScratchAudioAsset(clip)));
				}
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
