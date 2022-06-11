using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class ItemHoldablePickup : OutlineCreator, Useable
    {
        PlayerController player;

        public GameObject item;
        public bool inGame = true;

        protected override void Start()
        {
            base.Start();

            if (inGame)
                player = GameController.Instance.gamePlayer;
            else
                player = GameController.Instance.metaPlayer;
        }

        void Update()
        {
            outlineHelper.UpdateOutline();
        }

        void Useable.Use()
        {
            PickThisUp();
        }

        public void PickThisUp()
        {
            int newLayer = inGame ? LayerMask.NameToLayer("Always On Top") : LayerMask.NameToLayer("MetaLayer_Always On Top");
            MoveToLayer(item.transform, newLayer);

            outlineHelper.DestroyOutlines();

            player.AddItem(item.GetComponent<ItemHoldable>(), true, !inGame);
            gameObject.SetActive(false);
        }

        void MoveToLayer(Transform root, int layer)
        {
            root.gameObject.layer = layer;
            foreach (Transform child in root)
                MoveToLayer(child, layer);
        }

        void Useable.LookingAt()
        {
            outlineHelper.ShowOutline();
        }
    }
}
