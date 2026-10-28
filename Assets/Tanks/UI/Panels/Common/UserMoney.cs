using Tanks.Common.Scripts.Player;
using TMPro;
using UnityEngine;

namespace Tanks.UI.Panels.Common
{
    public class UserMoney: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI  _currencyText;

        private void OnEnable()
        {
            PlayerManager.OnCurrencyChanged +=  OnCurrencyChanged;
        }

        private void OnDisable()
        {
            PlayerManager.OnCurrencyChanged -=  OnCurrencyChanged;
        }

        private void OnCurrencyChanged(int currency)
        {
            _currencyText.text = currency.ToString();
        }
    }
}