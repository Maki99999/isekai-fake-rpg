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

        public OpenableDoor door;
        public Collider doorCollider;

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
            yield return GameController.Instance.playerEventManager.LookAt(false, door.transform.position, 1f);
            GameController.Instance.playerEventManager.FreezePlayer(false, false);

            yield return new WaitForSeconds(20f);
            door.Open();
            door.lockedMode = OpenableDoor.State.UNLOCKED;
            doorCollider.enabled = false;
            monsterAnim.gameObject.SetActive(false);

            yield return new WaitForSeconds(2f);
            doorCollider.enabled = true;
            GameController.Instance.horrorEventManager.StartEvent("H916");
            GameController.Instance.storyManager.TaskFinished();
            monsterAnim.SetBool("Show", false);

            yield return new WaitForSeconds(1.75f);
            GameController.Instance.playerEventManager.FreezePlayer(false, true, true);
            yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "What was that?!" });
            GameController.Instance.playerEventManager.FreezePlayer(false, false);
            gameObject.SetActive(false);
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
