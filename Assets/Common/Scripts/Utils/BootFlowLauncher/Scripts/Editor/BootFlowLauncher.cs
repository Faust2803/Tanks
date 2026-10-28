#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace BootFlowLauncher.Editor
{
    [InitializeOnLoad]
    public static class BootFlowLauncher
	{
        static BootFlowLauncher()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        private static void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (!EditorPrefs.GetBool(BootFlowConstants.ENABLED_KEY, false))
                return;

            switch (state)
            {
                case PlayModeStateChange.ExitingEditMode:
                    if (EditorSceneManager.GetActiveScene().path == 
						EditorPrefs.GetString(BootFlowConstants.TARGET_SCENE_PATH))
                        return;

                    EditorPrefs.SetString(BootFlowConstants.PREVIOUS_SCENE_PATH, 
						EditorSceneManager.GetActiveScene().path);
                    EditorApplication.isPlaying = false;

                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
					{
						var r = EditorPrefs.GetString(BootFlowConstants.TARGET_SCENE_PATH);

						EditorSceneManager.OpenScene(r);
                    }
                    EditorApplication.isPlaying = true;
                    break;

                case PlayModeStateChange.EnteredEditMode:
                    var previousScene = EditorPrefs.GetString(BootFlowConstants.PREVIOUS_SCENE_PATH);
                    if (!string.IsNullOrEmpty(previousScene))
                    {
                        EditorSceneManager.OpenScene(previousScene);
                        EditorPrefs.SetString(BootFlowConstants.PREVIOUS_SCENE_PATH, null);
                    }
                    break;

                case PlayModeStateChange.EnteredPlayMode:
                    if (!string.IsNullOrEmpty(EditorPrefs.GetString(BootFlowConstants.PREVIOUS_SCENE_PATH)))
                    {
                        Debug.Log($"<color=yellow>[{nameof(BootFlowLauncher)}] Scene " +
							$"{EditorPrefs.GetString(BootFlowConstants.TARGET_SCENE_KEY)} loaded.</color>");
                    }
                    break;
            }
        }

        [MenuItem("Tools/Auto Boot Scene Launcher/Toggle %#t")]
        private static void Toggle()
        {
            EditorPrefs.SetBool(BootFlowConstants.ENABLED_KEY, 
				!EditorPrefs.GetBool(BootFlowConstants.ENABLED_KEY, false));
            Debug.Log($"<color=yellow>[{nameof(BootFlowLauncher)}] Autoload " +
				$"{EditorPrefs.GetString(BootFlowConstants.TARGET_SCENE_KEY)} " +
				$"scene {(EditorPrefs.GetBool(BootFlowConstants.ENABLED_KEY, false) ? "ON" : "OFF")}</color>");
        }

        [MenuItem("Tools/Auto Boot Scene Launcher/Toggle %#t", true)]
        private static bool ToggleValidate()
        {
            Menu.SetChecked($"Tools/Auto {EditorPrefs.GetString(BootFlowConstants.TARGET_SCENE_KEY)} " +
				$"Scene Launcher/Toggle %#t", EditorPrefs.GetBool(BootFlowConstants.ENABLED_KEY, false));
            return true;
        }
    }
}
#endif
