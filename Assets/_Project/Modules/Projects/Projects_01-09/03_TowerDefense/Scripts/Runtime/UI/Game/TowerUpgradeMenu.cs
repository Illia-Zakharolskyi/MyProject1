using Project.TowerDefense.Runtime;
using Project.TowerDefense.Runtime.UI;
using Project.TowerDefense.Testing;
using Project.TowerDefense.Runtime.Definitions;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.TowerDefense.InTesting
{
    public class TowerUpgradeMenu : MonoBehaviour
    {
        private Sprite towerSprite;
        private uint towerLevel;
        private Tower script;
        private double currDamage;
        private double nextDamage;
        private double upgradeCost;
        private readonly Dictionary<string, IFindTargetStrategy> _strategies = new()
        {
            { "First", new FirstTargetStrategy() },
            { "Strongest", new StrongestTargetStrategy() },
            { "Last", new LastTargetStrategy() },
            { "Weakest", new WeakestTargetStrategy() }
        };
        [SerializeField] private TowerDefenseData _data;
        [SerializeField] private Image _towerSpriteElement;
        [SerializeField] private TMP_Text _towerLevelElement;
        [SerializeField] private GameObject _menuContent;
        [SerializeField] private TMP_Text _currDamageElement;
        [SerializeField] private TMP_Text _nextDamageElement;
        [SerializeField] private TMP_Text _upgradeCostElement;
        [SerializeField] private TMP_Text _currStrategyElement;
        [SerializeField] private GameEvents _events;
        [SerializeField] private PlayerCurrencyManager _currencyManager;
        //private bool _contentActive = false;

        void OnEnable()
        {
            _events.OnTowerUpgradeMenuDataGiven += Handle;
        }
        void OnDisable()
        {
            _events.OnTowerUpgradeMenuDataGiven -= Handle;
        }

        void Handle(Sprite sprite, uint level, MonoBehaviour script)
        {
            Debug.Log("Ha?");
            
            UpdateInfo(sprite, level, script as Tower);
            if (this.script != null)
            {
                this.script.ShowRadiusCircle();
            }
            /*if (!_contentActive) UpdateInfo(sprite, level, script as Tower);
            else InfoHide();*/
        }

        public void NextStrategyButtonPressed()
        {
            if (script._cursorStrategyIndex < _strategies.Count - 1)
            {
                script._cursorStrategyIndex++;
            }
            else
            {
                script._cursorStrategyIndex = 0;
            }

            string text = _strategies.Keys.ElementAt(script._cursorStrategyIndex);
            _currStrategyElement.text = text;
            script._findTargetStrategy = _strategies[text];
        }

        void UpdateInfo(Sprite sprite = null, uint level = 0, Tower script = null)
        {
            towerLevel = level;

            if (sprite != null)
            {
                towerSprite = sprite;
            }
            if (script != null)
            {
                this.script = script;
            }
            if (this.script != null)
            {
                currDamage = this.script.BulletDamage;
                nextDamage = currDamage * Random.Range((float)_data.damageUpgradeModifierMin, (float)_data.damageUpgradeModifierMax);
                upgradeCost = this.script.GetUpgradeCost();
                towerLevel = this.script.CurrentLevel;
                _currStrategyElement.text = _strategies.Keys.ElementAt(this.script._cursorStrategyIndex);
            }

            InfoShow();
        }

        void InfoShow()
        {
            _menuContent.SetActive(true);
            //_contentActive = true;
            _towerSpriteElement.sprite = towerSprite;
            _towerLevelElement.text = NumberFormatter.ReturnFormatted(towerLevel, 0);
            _currDamageElement.text = NumberFormatter.ReturnFormatted(currDamage, 0);
            _nextDamageElement.text = NumberFormatter.ReturnFormatted(nextDamage, 0);
            _upgradeCostElement.text = NumberFormatter.ReturnFormatted(upgradeCost, 0);
        }

        /*void InfoHide()
        {
            _menuContent.SetActive(false);
            _contentActive = false;
        }*/

        public void UpgradeButtonPressed()
        {
            if (script == null)
            {
                Debug.Log("[TowerUpgradeMenu.cs]: The script given is not TowerTestingScript, but another script with parent MonoBehaviour.");
                return;
            }

            
            if (!_currencyManager.Instance.HasEnoughCurrency(upgradeCost, CurrencyType.Gold))
            {
                Debug.Log("not enough");
                return;
            }

            double currDamage = script.BulletDamage;

            _currencyManager.SpendCurrency(upgradeCost, CurrencyType.Gold);
            script.UpgradeStats(_data.damageUpgradeModifierMin, _data.damageUpgradeModifierMax);
            script.IncreaseUpgradeCost(_data.damageUpgradeCostModifierMin, _data.damageUpgradeCostModifierMax);
            UpdateInfo();
        }

        public void Close()
        {
            Debug.Log("Something");
            _menuContent.SetActive(false);
            script.HideRadiusCircle();
            script._upgradeActive = false;
            _data.isTowerCircleShowing = false;
        }
    }
}
