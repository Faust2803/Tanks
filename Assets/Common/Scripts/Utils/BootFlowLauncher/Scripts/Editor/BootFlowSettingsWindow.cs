using UnityEditor;
using UnityEngine;

namespace BootFlowLauncher.Editor
{
	public class BootFlowSettingsWindow : EditorWindow
	{
		private string _projectName;
		private string _targetScene;
		private string _targetScenePath;
		
		private bool _autoSceneLauncherEnabled;

		[MenuItem("Tools/Auto Boot Scene Launcher/Settings")]
		public static void ShowWindow()
		{
			GetWindow<BootFlowSettingsWindow>("Boot Flow Settings");
		}

		private void OnEnable()
		{
			_projectName = EditorPrefs.GetString(BootFlowConstants.PROJECT_NAME_KEY, "Project");
			_targetScene = EditorPrefs.GetString(BootFlowConstants.TARGET_SCENE_KEY, "GameScene");
			_targetScenePath = EditorPrefs.GetString(BootFlowConstants.TARGET_SCENE_PATH, 
				"Assets/"+_projectName+"/"+_targetScene+".unity");
			
			_autoSceneLauncherEnabled = EditorPrefs.GetBool(BootFlowConstants.ENABLED_KEY, false);
		}

		private void OnGUI()
		{
			GUILayout.Label("Boot Flow Settings", EditorStyles.boldLabel);

			_projectName = EditorGUILayout.TextField("Project Name", _projectName);
			_targetScene = EditorGUILayout.TextField("Target Scene path", _targetScene);
			_targetScenePath = "Assets/" + _projectName + "/" + _targetScene + ".unity";
			
			_autoSceneLauncherEnabled = EditorGUILayout.Toggle("Auto Launch Scene", _autoSceneLauncherEnabled);

			if (GUILayout.Button("Save Settings"))
			{
				EditorPrefs.SetString(BootFlowConstants.PROJECT_NAME_KEY, _projectName);
				EditorPrefs.SetString(BootFlowConstants.TARGET_SCENE_KEY, _targetScene);
				EditorPrefs.SetString(BootFlowConstants.TARGET_SCENE_PATH, _targetScenePath);
				EditorPrefs.SetBool(BootFlowConstants.ENABLED_KEY, _autoSceneLauncherEnabled);

				Debug.Log("<color=yellow>[BootFlowSettingsWindow] Settings saved.</color>");
			}
		}
	}
}