using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Default
{
    public class ShopItem : OutlineCreator, Useable, ISaveDataObject
    {
        public Text priceText;
        public bool isItem;     //Maybe replace this with an enum
        public BuyableItem[] items;
        public AudioSource boughtSfx;

        int currentItem = 0;

        public string saveDataId => "ShopItem" + gameObject.name;

        protected override void Start()
        {
            base.Start();

            foreach (BuyableItem item in items)
                item.theObject.SetActive(false);

            priceText.text = items[currentItem].price.ToString();
            items[currentItem].theObject.SetActive(true);
        }

        void Update()
        {
            outlineHelper.UpdateOutline();
        }

        void Useable.Use()
        {
            BuyCurrentItem();
        }

        private void BuyCurrentItem(bool silentAndFree = false)
        {
            PlayerController player = GameController.Instance.gamePlayer;

            if (currentItem < items.Length && (silentAndFree || player.stats.ChangeCoins(-items[currentItem].price)))
            {
                if (!silentAndFree)
                {
                    boughtSfx.Play();
                    GameController.Instance.overallStats.AddToStat(1, "ItemsBought");
                }

                if (isItem)
                {
                    player.AddItem(items[currentItem].theObject.GetComponent<ItemHoldable>(), true);
                    foreach (Transform child in items[currentItem].theObject.transform)
                        child.gameObject.layer = LayerMask.NameToLayer("Always On Top");
                    GameController.Instance.overallStats.AddToStat(1, "Bought_Weapon");
                }
                else
                {
                    player.stats.AddOrReplaceStatItem(items[currentItem].theObject.GetComponent<Armor>());
                    Destroy(items[currentItem].theObject);
                    GameController.Instance.overallStats.AddToStat(1, "Bought_Armor");
                }

                List<Outline> outlinesToDelete = new List<Outline>(items[currentItem].theObject.GetComponents<Outline>());
                outlinesToDelete.AddRange(items[currentItem].theObject.GetComponentsInChildren<Outline>());
                foreach (Outline outline in outlinesToDelete)
                {
                    outlineHelper.DestroyOutline(outline);
                }

                currentItem++;
                if (currentItem < items.Length)
                {
                    priceText.text = items[currentItem].price.ToString();
                    items[currentItem].theObject.SetActive(true);
                }
                else
                    priceText.text = "";
            }
        }

        void Useable.LookingAt()
        {
            outlineHelper.ShowOutline();
        }

        public SaveDataEntry Save()
        {
            SaveDataEntry entry = new SaveDataEntry();
            entry.Add("currentItem", currentItem);
            return entry;
        }

        public void Load(SaveDataEntry dictEntry)
        {
            if (dictEntry == null)
                return;
            int itemsToBuy = dictEntry.GetInt("currentItem", currentItem);
            if (itemsToBuy > 0)
            {
                StartCoroutine(BuyItemsNextFrames(itemsToBuy));
            }
        }

        private IEnumerator BuyItemsNextFrames(int itemsToBuy)
        {
            GameController.Instance.playerEventManager.FreezePlayer(true, true);
            yield return null;
            for (int i = 0; i < itemsToBuy; i++)
                BuyCurrentItem(true);
            GameController.Instance.playerEventManager.FreezePlayer(true, false);
        }
    }

    [System.Serializable]
    public class BuyableItem
    {
        public GameObject theObject;
        public int price;
    }
}
