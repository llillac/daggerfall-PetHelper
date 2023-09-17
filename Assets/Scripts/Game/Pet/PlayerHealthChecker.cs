﻿using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Game.Items;
using DaggerfallWorkshop.Utility;
using UnityEngine;

namespace Game.Pet
{
    public class PlayerHealthChecker : MonoBehaviour
    {
        private const int HealPotionRecipeKey = 4975678;
        private const int SparklesIndex = 3;
        private const int BloodArchive = 380;
        private const string PotionMixedTerm = "potionMixed";

        [SerializeField] private LootTimer lootTimer;
        [SerializeField] private int lootDelay;
        [SerializeField, Range(0, 1)] private float minHealthPercent;
        [SerializeField] private PetMotor petMotor;

        private DaggerfallEntityBehaviour _playerEntityBehaviour;
        private DaggerfallLoot _currentLootContainer;
        private bool _readyForNextLoot = true;

        private void Awake()
        {
            _playerEntityBehaviour = GameManager.Instance.PlayerEntityBehaviour;
        }

        private void Update()
        {
            if (GameManager.IsGamePaused || _playerEntityBehaviour.Entity.CurrentHealthPercent == 0)
                return;

            if (_playerEntityBehaviour.Entity.CurrentHealthPercent < minHealthPercent && _readyForNextLoot)
            {
                _readyForNextLoot = false;
                lootTimer.StartTimer(lootDelay, () => _readyForNextLoot = true);
                CreateHealLoot();
            }
        }

        private void CreateHealLoot()
        {
            if (_currentLootContainer)
            {
                Destroy(_currentLootContainer.gameObject);
                _currentLootContainer = null;
            }

            var potion = ItemBuilder.CreatePotion(HealPotionRecipeKey);

            _currentLootContainer =
                GameObjectHelper.CreateDroppedLootContainer(_playerEntityBehaviour.gameObject, DaggerfallUnity.NextUID);
            _currentLootContainer.transform.position = petMotor.FindGroundPosition() + new Vector3(0, 0.3f, 0);
            _currentLootContainer.Items.AddItem(potion);

            DaggerfallUI.Instance.PopupMessage(TextManager.Instance.GetLocalizedText(PotionMixedTerm));
            DaggerfallUI.Instance.PlayOneShot(SoundClips.MakePotion);

            ShowMagicSparkles(_currentLootContainer.transform.position);
            AddHintLight(_currentLootContainer);
        }

        private void AddHintLight(DaggerfallLoot loot)
        {
            var lightComponent = loot.gameObject.AddComponent<Light>();
            lightComponent.type = LightType.Point;
            lightComponent.color = Color.green;
            lightComponent.intensity = 0.8f;
        }

        private void ShowMagicSparkles(Vector3 sparklesPosition)
        {
            var dfUnity = DaggerfallUnity.Instance;
            if (!dfUnity) return;

            var billboardInstance =
                GameObjectHelper.CreateDaggerfallBillboardGameObject(BloodArchive, SparklesIndex, null);
            var billboard = billboardInstance.GetComponent<Billboard>();
            billboardInstance.transform.position = sparklesPosition + transform.forward * 0.02f;
            billboard.OneShot = true;
            billboard.FramesPerSecond = 10;
        }

        [ContextMenu("AddHealth")]
        private void AddHealth()
        {
            _playerEntityBehaviour.Entity.IncreaseHealth(20);
        }
    }
}