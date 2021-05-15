using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Default
{
    public class ShopItem : MonoBehaviour, Useable
    {
        public Text priceText;
        public bool isItem;     //Maybe replace this with an enum
        public BuyableItem[] items;

        int currentItem = 0;

        PlayerController player;
        List<Outline> outlines;

        void Start()
        {
            player = GameController.Instance.gamePlayer;

            outlines = new List<Outline>();

            MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in renderers)
            {
                Outline newOutline = renderer.gameObject.AddComponent<Outline>();
                newOutline.enabled = false;
                outlines.Add(newOutline);
            }

            foreach (BuyableItem item in items)
                item.theObject.SetActive(false);

            priceText.text = items[currentItem].price.ToString();
            items[currentItem].theObject.SetActive(true);
        }

        void Update()
        {
            foreach (Outline outline in outlines)
                outline.enabled = false;
        }

        void Useable.Use()
        {
            if (currentItem < items.Length && player.stats.ChangeCoins(-items[currentItem].price))
            {
                if (isItem)
                {
                    player.AddHoldableItem(items[currentItem].theObject.GetComponent<ItemHoldable>(), true);
                    foreach (Transform child in items[currentItem].theObject.transform)
                        child.gameObject.layer = LayerMask.NameToLayer("Always On Top");
                }
                else
                    player.stats.AddOrReplaceStatItem(items[currentItem].theObject.GetComponent<Armor>());

                List<Outline> outlinesToDelete = new List<Outline>(items[currentItem].theObject.GetComponents<Outline>());
                outlinesToDelete.AddRange(items[currentItem].theObject.GetComponentsInChildren<Outline>());
                foreach (Outline outline in outlinesToDelete)
                {
                    outlines.Remove(outline);
                    Destroy(outline);
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
            foreach (Outline outline in outlines)
                outline.enabled = true;
        }
    }

    [System.Serializable]
    public class BuyableItem
    {
        public GameObject theObject;
        public int price;
    }
}
