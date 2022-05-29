using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class H10FoodFlesh : ItemHoldable, Useable, ISaveDataObject
    {
        public Outline outline;
        new public Collider collider;

        [Space(10)]
        public GameObject flesh;
        public GameObject food;
        public AudioSource sound;
        public AudioSource soundFlesh;
        public AudioSource soundBrokenPlate;

        [Space(10)]
        public GameObject[] objectsToHide;
        public Animator plateFallAnim;
        public Transform fakeFood;

        [Space(10)]
        public MonsterGlitchEffectReceiver glitchEffectReceiver;

        bool pickedUp = false;
        bool fleshOn = false;

        public string saveDataId => "H10" + gameObject.name;

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
            GameController.Instance.playerEventManager.FreezePlayer(false, true, true);
            yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "Let's eat in the dining room." });
            GameController.Instance.playerEventManager.FreezePlayer(false, false);

            yield return new WaitForSeconds(8.5f);
            ShowFlesh(true);

            yield return new WaitForSeconds(1.5f);
            GameController.Instance.playerEventManager.FreezePlayer(false, true, true);
            yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "Ew!" });

            GameController.Instance.metaPlayer.RemoveItem(this);
            Vector3 plateFallPos = GameController.Instance.metaPlayer.transform.position +
                    GameController.Instance.metaPlayer.transform.forward * 0.5f;
            plateFallPos.y = -1;
            plateFallAnim.transform.position = plateFallPos;
            fakeFood.gameObject.SetActive(true);
            fakeFood.position = transform.position;
            fakeFood.rotation = transform.rotation;
            foreach (GameObject obj in objectsToHide)
                obj.SetActive(false);

            fakeFood.parent = plateFallAnim.transform;
            transform.parent = fakeFood;
            plateFallAnim.enabled = true;
            plateFallAnim.Rebind();
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
            if (show)
                soundFlesh.Play();
            else
                soundFlesh.Stop();
            StartCoroutine(ShortCameraEffect());
        }

        private IEnumerator ShortCameraEffect()
        {
            if (glitchEffectReceiver == null)
                yield break;
            glitchEffectReceiver.enabled = true;
            glitchEffectReceiver.desiredPercent = 1;
            yield return new WaitForSeconds(0.5f);
            glitchEffectReceiver.desiredPercent = 0;
            glitchEffectReceiver.enabled = false;
        }

        public SaveDataEntry Save()
        {
            SaveDataEntry entry = new SaveDataEntry();
            entry.Add("fleshOn", fleshOn ? "true" : "false");
            entry.Add("transform", SaveManager.TransformToString(transform));
            if (plateFallAnim != null)
                entry.Add("plateFallTransform", SaveManager.TransformToString(plateFallAnim.transform));
            return entry;
        }

        public void Load(SaveDataEntry dictEntry)
        {
            if (dictEntry == null)
                return;
            string transformString = dictEntry.GetString("transform", null);
            if (transformString != null)
                SaveManager.ApplyStringToTransform(transform, transformString);
            if (dictEntry.GetString("fleshOn", "false") == "true")
            {
                gameObject.SetActive(true);
                fleshOn = true;
                flesh.SetActive(true);
                food.SetActive(false);

                if (soundBrokenPlate != null)
                    soundBrokenPlate.gameObject.SetActive(false);

                if (fakeFood != null && plateFallAnim != null)
                {
                    collider.enabled = false;
                    transformString = dictEntry.GetString("plateFallTransform", null);
                    SaveManager.ApplyStringToTransform(plateFallAnim.transform, transformString);
                    fakeFood.gameObject.SetActive(true);
                    fakeFood.position = transform.position;
                    fakeFood.rotation = transform.rotation;
                    foreach (GameObject obj in objectsToHide)
                        obj.SetActive(false);
                    transform.parent = fakeFood;
                    plateFallAnim.enabled = true;
                    plateFallAnim.SetTrigger("Fall");
                }
            }
        }
    }
}
