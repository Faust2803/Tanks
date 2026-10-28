using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Util
{
    public class SceneLoader 
    {
        private string ActiveSceneName => SceneManager.GetActiveScene().name;

        public async UniTask<bool> LoadScene(string scene, IProgress<float> progress = null)
        {
            if (ActiveSceneName == scene)
            {
                progress?.Report(1f);
                return true;
            }
            return await Load(scene, progress);
        }

        public async UniTask<bool> ReloadScene(IProgress<float> progress = null)
        {
            return await Load(ActiveSceneName, progress);
        }

        private async UniTask<bool> Load(string sceneName, IProgress<float> progress = null)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

            if (operation == null)
            {
                Debug.Log($"<color=FF0000>Failed to load scene: {sceneName} loaded.</color>");
                return false;
            }

            operation.allowSceneActivation = false;

            while (operation.progress < 0.9f)
            {
                var value = operation.progress / 0.9f;
                progress?.Report(value);
                await UniTask.Yield();
            }
            progress?.Report(1f);

            Debug.Log($"<color=green> Scene '{sceneName}' loaded. Activating...</color>");
            operation.allowSceneActivation = true;
            await UniTask.WaitUntil(() => operation.isDone);

            Debug.Log($"<color=green> Scene '{sceneName}' activated.</color>");
            return true;
        }
    }
}


//вызов
// var result = await _sceneLoader.LoadScene(SceneName.Lobby.ToString(),
//     new Progress<float>(value =>
//     {
//         Debug.Log($"Loading: {value:P0}");
//     })
// );
//
// if (result)
// {
//     Debug.Log("Сцена полностью загружена!");
//     CloseSelf();
// }