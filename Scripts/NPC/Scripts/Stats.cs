using UnityEngine;
using UnityEngine.UI;
using Wilson.Game;
using Wilson.Item;
using System.Collections.Generic;

namespace Wilson.NPC
{
    public class Stats : MonoBehaviour
    {
        #region Varialbes
        public TimeManager timeManager;

        public StorageBox storageBox;
        private float hunger;   
        private float thirst;
        private int lastCheckedDay = 0;

        [SerializeField] private Image hungerBar;
        [SerializeField] private Image thirstBar;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            hunger = 100f;
            thirst = 100f;
            lastCheckedDay = timeManager.Day;
        }

        private void Update()
        {
            DayCycle();  
        }

        #endregion

        #region Custom Method
        private void UpdateStatus()
        {
            hunger = Mathf.Clamp(hunger, 0f, 100f);
            thirst = Mathf.Clamp(thirst, 0f, 100f);

            hungerBar.fillAmount = hunger / 100f;
            thirstBar.fillAmount = thirst / 100f;
        }

        public void Eat(float amount)
        {
            hunger += amount;
            hunger = Mathf.Clamp(hunger, 0f, 100f);
        }

        public void Use(float amount)
        {
            hunger -= amount;
            hunger = Mathf.Clamp(hunger, 0f, 100f);
        }

        public void Drink(float amount)
        {
            thirst += amount;
            thirst = Mathf.Clamp(thirst, 0f, 100f);
        }

        public void Waste(float amount)
        {
            thirst -= amount;
            thirst = Mathf.Clamp(thirst, 0f, 100f);
        }

        private void DayCycle()
        {
            if (lastCheckedDay != timeManager.Day)
            {
                Use(30);
                Waste(30);
                lastCheckedDay = timeManager.Day;

                UpdateStatus();
            }
        }

        public void EatFoodWater()
        {
            if (storageBox == null || storageBox.storageItemBox == null || storageBox.storageItemBox.Count == 0)
                return;

            List<ConsumableItemData> hungerItems = new();
            List<ConsumableItemData> thirstItems = new();

            foreach (ItemData item in storageBox.storageItemBox)
            {
                if (item is ConsumableItemData consumable)
                {
                    if (consumable.hungerRestore > 0)
                        hungerItems.Add(consumable);

                    if (consumable.thirstRestore > 0)
                        thirstItems.Add(consumable);
                }
            }

            // 배고픔 회복 아이템 사용
            if (hungerItems.Count > 0)
            {
                ConsumableItemData chosen = hungerItems[Random.Range(0, hungerItems.Count)];
                Eat(chosen.hungerRestore);
                storageBox.storageItemBox.Remove(chosen);
            }

            // 목마름 회복 아이템 사용
            if (thirstItems.Count > 0)
            {
                ConsumableItemData chosen = thirstItems[Random.Range(0, thirstItems.Count)];
                Drink(chosen.thirstRestore);
                storageBox.storageItemBox.Remove(chosen);
            }

            UpdateStatus();
        }

        #endregion
    }
}