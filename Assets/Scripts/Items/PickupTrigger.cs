using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Default
{
    public class PickupTrigger : ItemHoldable, Useable
    {
        public Outline outline;
        [SerializeField] private bool inGame;
        private PlayerController player;
        [SerializeField] private UnityEvent unityEvent;

        private void Start()
        {
            if (inGame)
                player = GameController.Instance.gamePlayer;
            else
                player = GameController.Instance.metaPlayer;
        }

        private void Update()
        {
            if (enabled)
                outline.enabled = false;
        }

        public void LookingAt()
        {
            if (enabled)
                outline.enabled = true;
        }

        public void Use()
        {
            if (enabled)
            {
                player.AddItem(this, true, !inGame);
                unityEvent.Invoke();

                outline.enabled = false;
                enabled = false;
            }
        }

        public override void OnEquip()
        {
            player.UnequipCurrentItem();
        }

        public override void OnUnequip()
        {
            gameObject.SetActive(false);
        }
    }
}
