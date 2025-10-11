using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LunyScratch
{
	internal sealed partial class UnityActions
	{
		public void ReloadCurrentScene()
		{
			var active = SceneManager.GetActiveScene();
			SceneManager.LoadScene(active.buildIndex);
		}

		public void QuitApplication()
		{
#if UNITY_EDITOR
			if (Application.isEditor)
			{
				EditorApplication.ExitPlaymode();
				return;
			}
#endif
			Application.Quit();
		}
	}
}
