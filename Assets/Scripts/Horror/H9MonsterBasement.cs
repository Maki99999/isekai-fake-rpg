using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class H9MonsterBasement : MonoBehaviour
    {
        public Animator monsterBasementAnim;
        public Animator monsterAnim;

        public AudioSource knifeAbsorpSfx;
        public AudioSource t14BassSfx;
        public AudioSource noiseIncreasingVolumeSfx;

        public OpenableDoor door;
        public Collider doorCollider;
        public Collider doorUseableCollider;

        public void SpawnMonster()
        {
            StartCoroutine(MonsterAnim());
        }

        IEnumerator MonsterAnim()
        {
            door.Close();
            door.lockedMode = OpenableDoor.State.LOCKED;
            GameController.Instance.playerEventManager.FreezePlayer(false, true, true);
            yield return GameController.Instance.playerEventManager.LookAt(false, monsterAnim.transform.position + Vector3.up, 1f);
            monsterAnim.gameObject.SetActive(true);
            monsterAnim.SetBool("Show", true);
            yield return new WaitForSeconds(1.2f);
            yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "What the..." });

            monsterBasementAnim.SetTrigger("Go");
            StartCoroutine(NoiseIncreaseVolume(20f));
            yield return GameController.Instance.playerEventManager.LookAt(false, door.transform.position, 1f);
            GameController.Instance.playerEventManager.FreezePlayer(false, false);

            yield return new WaitForSeconds(19f);
            door.lockedMode = OpenableDoor.State.UNLOCKED;
            doorCollider.enabled = false;
            doorUseableCollider.enabled = false;
            door.Open();
            GameController.Instance.horrorEventManager.StartEvent("H916");
            monsterAnim.gameObject.SetActive(false);
            t14BassSfx.Stop();
            noiseIncreasingVolumeSfx.Stop();

            yield return new WaitForSeconds(1.7f);
            GameController.Instance.storyManager.TaskFinished();
            GameController.Instance.playerEventManager.FreezePlayer(false, true, true);
            yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "What was that?!" });
            GameController.Instance.playerEventManager.FreezePlayer(false, false);
            doorCollider.enabled = true;
            doorUseableCollider.enabled = true;
            gameObject.SetActive(false);
        }

        public IEnumerator NoiseIncreaseVolume(float seconds)
        {
            noiseIncreasingVolumeSfx.volume = 0;
            noiseIncreasingVolumeSfx.Play();

            float fromVol = 0;
            float toVol = 1f;
            float rate = 1f / seconds;
            float fSmooth;
            for (float f = 0f; f <= 1f; f += rate * Time.deltaTime)
            {
                fSmooth = Mathf.SmoothStep(0f, 1f, f);
                noiseIncreasingVolumeSfx.volume = Mathf.Lerp(fromVol, toVol, fSmooth);
                yield return null;
            }
        }

        public void StealKnife()
        {
            KnifeItem knife = (KnifeItem)GameController.Instance.metaPlayer.GetItem("Knife");
            knife.animator.enabled = false;
            knife.enabled = false;
            GameController.Instance.metaPlayer.UnequipCurrentItem();
            knife.transform.parent = monsterAnim.transform;
            MoveToLayer(knife.transform, LayerMask.NameToLayer("MetaLayer"));
            StartCoroutine(TransformOperations.MoveToLocal(knife.transform, Vector3.up, 3f));
            knifeAbsorpSfx.Play();
        }

        void MoveToLayer(Transform root, int layer)
        {
            root.gameObject.layer = layer;
            foreach (Transform child in root)
                MoveToLayer(child, layer);
        }
    }
}
