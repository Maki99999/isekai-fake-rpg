using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class H10FoodFlesh : ItemHoldable, Useable
    {
        public Outline outline;
        new public Collider collider;

        [Space(10)]
        public GameObject flesh;
        public GameObject food;
        public AudioSource sound;

        [Space(10)]
        public GameObject[] objectsToHide;
        public Animator plateFallAnim;
        public Transform fakeFood;

        bool pickedUp = false;
        bool fleshOn = false;

        void Update()
        {
            outline.enabled = false;
        }

        public void LookingAt()
        {
            if (!pickedUp)
                outline.enabled = true;
        }

        public void Use()
        {
            if (!pickedUp)
            {
                pickedUp = true;
                StartCoroutine(H10Flesh());
            }
        }

        IEnumerator H10Flesh()
        {
            GameController.Instance.metaPlayer.AddItem(this, true, true);
            collider.enabled = false;
            MoveToLayer(transform, LayerMask.NameToLayer("MetaLayer_Always On Top"));
            GameController.Instance.playerEventManager.FreezePlayer(false, true);
            yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "Let's eat in the dining room." });
            GameController.Instance.playerEventManager.FreezePlayer(false, false);

            yield return new WaitForSeconds(8.5f);
            ShowFlesh(true);

            yield return new WaitForSeconds(1.5f);
            GameController.Instance.playerEventManager.FreezePlayer(false, true);
            yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "Ew!" });

            GameController.Instance.metaPlayer.RemoveItem(this);
            plateFallAnim.transform.position = GameController.Instance.metaPlayer.transform.position +
                    GameController.Instance.metaPlayer.transform.forward * 0.5f;
            fakeFood.gameObject.SetActive(true);
            fakeFood.position = transform.position;
            fakeFood.rotation = transform.rotation;
            foreach (GameObject obj in objectsToHide)
                obj.SetActive(false);

            transform.parent = fakeFood;
            plateFallAnim.enabled = true;
            plateFallAnim.SetTrigger("Fall");

            yield return GameController.Instance.playerEventManager.LookAt(false, plateFallAnim.transform.position, 2f);
            yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "What the hell is that?", "...", "My appetite is gone now..." });

            GameController.Instance.storyManager.TaskFinished();
            GameController.Instance.storyManager.StartTask("T13");
            GameController.Instance.playerEventManager.FreezePlayer(false, false);
        }

        void MoveToLayer(Transform root, int layer)
        {
            root.gameObject.layer = layer;
            foreach (Transform child in root)
                MoveToLayer(child, layer);
        }

        public void ShowFlesh(bool show)
        {
            fleshOn = show;
            flesh.SetActive(show);
            food.SetActive(!show);
            sound.Play();
        }
    }
}
