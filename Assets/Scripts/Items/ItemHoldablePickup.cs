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
        }

        void Update()
        {
            foreach (Outline outline in outlines)
                outline.enabled = false;
        }

        void Useable.Use()
        {
            player.AddHoldableItem(item.GetComponent<ItemHoldable>(), true);
            foreach (Transform child in item.transform)
                child.gameObject.layer = LayerMask.NameToLayer("Always On Top");

            List<Outline> outlinesToDelete = new List<Outline>(item.GetComponents<Outline>());
            outlinesToDelete.AddRange(item.GetComponentsInChildren<Outline>());
            foreach (Outline outline in outlinesToDelete)
            {
                outlines.Remove(outline);
                Destroy(outline);
            }
        }

        void Useable.LookingAt()
        {
            foreach (Outline outline in outlines)
                outline.enabled = true;
        }
    }
}
