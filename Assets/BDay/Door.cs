using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Default
{
    public class Door : MonoBehaviour, Useable
    {
        public Animator animator;
        public Transform desiredTransf;
        public GameObject cross;

        void Useable.Use()
        {
            StartCoroutine(UseEnum());
        }

        IEnumerator UseEnum()
        {
            cross.SetActive(false);
            GameController.Instance.player.SetCanMove(false);
            GameController.Instance.player.transform.GetComponent<EntityStats>().SetHideUi(true);
            yield return GameController.Instance.player.MovePlayer(desiredTransf, 40);
            animator.enabled = true;
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("NEXT"));
            SceneManager.LoadScene("BDay");
        }
    }
}
