using System;
using UnityEngine;
using Util;
using Zenject;

namespace UI.Panels
{
    public class LoadingPanelMediator :BasePanelMediator <LoadingPanelView, UIData>
    {
        private SceneLoader _sceneLoader;
        
        
        [Inject]
        private void Construct(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader; 
        }
		
        protected override void ShowStart()
        {
            base.ShowStart();
            Target.StartButton.onClick.AddListener(OnStartButtonClicked);
        }

        private async void OnStartButtonClicked()
        {
            var result = await _sceneLoader.LoadScene(SceneName.Lobby.ToString(),
                new Progress<float>(value =>
                {
                    Debug.Log($"Loading: {value:P0}");
                })
            );

            if (result)
            {
                CloseSelf();
            }
        }
    }
}