using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class ItemHoldablePickup : MonoBehaviour, Useable
    {
        PlayerController player;
        List<Outline> outlines;

        public GameObject item;
        public bool inGame = true;

        void Start()
        {
            if (inGame)
                player = GameController.Instance.gamePlayer;
            else
                player = GameController.Instance.metaPlayer;

            outlines = new List<Outline>();

            MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in renderers)
            {
                Outline newOutline = renderer.gameObject.AddComponent<Outline>();
                newOutline.enabled = false;
                outlines.Add(newOutline);
            }
        }

        void Update()
        {
            foreach (Outline outline in outlines)
                outline.enabled = false;
        }

        void Useable.Use()
        {
            PickThisUp();
        }

        public void PickThisUp()
        {
            int newLayer = inGame ? LayerMask.NameToLayer("Always On Top") : LayerMask.NameToLayer("MetaLayer_Always On Top");
            foreach (Transform child in item.transform)
                child.gameObject.layer = newLayer;

            foreach (Outline outline in outlines)
                Destroy(outline);

            player.AddItem(item.GetComponent<ItemHoldable>(), true, !inGame);
            gameObject.SetActive(false);
        }

        void Useable.LookingAt()
        {
            foreach (Outline outline in outlines)
                outline.enabled = true;
        }
    }
}
