using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Managers;
using SO.Scripts;
using Tanks.Common.Scripts.Player;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Windows
{
    public class ShopItemPanel : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private ShopItem shopItemPrefab;
        [SerializeField] private Transform  _content;
        [SerializeField] private ShopPanel  _panelType;
        [Space]
        [SerializeField] private RenderTexture  _renderTexture;
        [SerializeField] private Camera _renderCamera;
        [SerializeField] private Transform  _targetForModel;

        [Inject] private readonly TanksConfig _tanksConfig;
        [Inject] private readonly ColorsConfig _colorsConfig;
        [Inject] private  PlayerManager _playerManager;
        [Inject] private UiManager _uiManager;
        
        private List<ShopItem> _shopItems;
        
        private  async UniTask RenderModelToTexture()
        {
            for (var i = 0; i < _tanksConfig.TanksCount; i++)
            {
                if (_tanksConfig.TanksList[i].ShowInShop)
                {
                    var item = Instantiate(shopItemPrefab, _content).GetComponent<ShopItemTank>();
                    var model = Instantiate(_tanksConfig.TanksList[i].Prefab, _targetForModel);
                    
                    model.SetActive(true);

                    // Ждем окончания кадра, чтобы Unity обновила трансформы и свет
                    await UniTask.Delay(1);

                    _renderCamera.Render();

                    Texture2D resultTexture = new Texture2D(_renderTexture.width, _renderTexture.height, TextureFormat.RGBA32, false);
    
                    RenderTexture previousActive = RenderTexture.active;
                    RenderTexture.active = _renderTexture;
    
                    resultTexture.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
                    resultTexture.Apply();
    
                    RenderTexture.active = previousActive;
                    model.SetActive(false);

                    // Сохранение для проверки
                    // byte[] bytes = resultTexture.EncodeToPNG();
                    // string filePath = System.IO.Path.Combine(Application.dataPath, "TestRender" + i + ".png");
                    // System.IO.File.WriteAllBytes(filePath, bytes);
                    // Debug.Log($"[Check] Текстура сохранена по пути: {filePath}");
                    
                    
                    item.CreateObjects(_tanksConfig.TanksList[i], resultTexture, _playerManager, _uiManager);           
                    _shopItems.Add(item);
                }
            }

            ResetScrollToStartAsync().Forget();
        }
        
        public void CreateObjects()
        {
            if (_shopItems == null)
            {
                _shopItems = new List<ShopItem>();
                switch (_panelType)
                {
                    case ShopPanel.Tanks:
                        RenderModelToTexture().Forget();
                        break;
                    case ShopPanel.Colors:
                        for (var j = 0; j < _colorsConfig.GetColorsCount; j++)
                        {
                            if (_colorsConfig.ColorList[j].ShowInShop)
                            {
                                var item = Instantiate(shopItemPrefab, _content).GetComponent<ShopItemColor>();
                                item.CreateObjects(_colorsConfig.ColorList[j], _playerManager, _uiManager);    
                                _shopItems.Add(item);
                            }
                        }
                        break;
                    case ShopPanel.Boosters:
                        break;
                    case ShopPanel.Weapons:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            ResetScrollToStartAsync().Forget();
        }
        
        private async UniTask ResetScrollToStartAsync()
        {
            gameObject.SetActive(true);
            await UniTask.Delay(10);
            if (_scrollRect != null)
            {
                //_scrollRect.verticalNormalizedPosition = 1f;
                _scrollRect.horizontalNormalizedPosition = 0f;
            }
        }
    }
    
    public enum ShopPanel
    {
        Tanks,
        Colors,
        Boosters,
        Weapons
    }
}