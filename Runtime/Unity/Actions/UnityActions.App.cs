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
			Application.Quit();

#if UNITY_EDITOR
			EditorApplication.ExitPlaymode();
#endif
		}
	}
}
