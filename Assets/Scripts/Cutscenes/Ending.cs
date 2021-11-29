using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class Ending : MonoBehaviour
    {
        public PcController pcController;
        public PlayerController metaPlayer;
        public Transform metaPlayerWrap;
        public Transform metaPlayerWrap2;
        public PlayerController gamePlayer;
        public Animator animator;
        public Animator animatorTumble;
        public Animator animatorEyes;
        public Animator mosterAnim;
        public TeleporterInstant teleporterInstant;
        public EndingEnemy enemy;
        public EndingEnemy enemy2;

        public AudioSource audioSource;
        public AudioSource loudStartSFX;

        private bool waitForContinue = false;

        public void StartEnding()
        {
            StartCoroutine(EndingAnim());
        }

        public void ContinueEnding()
        {
            waitForContinue = true;
        }

        IEnumerator EndingAnim()
        {
            GameController.Instance.playerEventManager.FreezePlayers(true);

            //no knife allowed
            metaPlayer.UnequipCurrentItem();

            //ImmersionBreak with a loud clang or sth
            pcController.ImmerseBreak(true);
            metaPlayer.transform.SetParent(metaPlayerWrap);
            metaPlayer.transform.localPosition = Vector3.zero;
            metaPlayer.transform.localEulerAngles = Vector3.zero;
            loudStartSFX.Play();

            //player looks around
            animator.Rebind();
            animator.enabled = true;
            animator.SetTrigger("Start");
            yield return new WaitForSeconds(4f);
            yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "...", "I'm getting paranoid." });
            teleporterInstant.TeleportPlayer();
            yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "What is this?!" });

            //glitchy monster grabs player through the screen...
            animator.SetTrigger("Grab");
            yield return new WaitUntil(() => waitForContinue);
            waitForContinue = false;

            //... into the game
            pcController.SetImmersedValueWithoutTransform(1f);
            metaPlayer.cam.cullingMask = gamePlayer.cam.cullingMask;
            yield return new WaitForSeconds(4f);
            yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "I'm... in the game?!" });
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Wait"));

            //spider walks towards player
            animator.SetTrigger("Monster");
            enemy.enabled = false;
            yield return new WaitForSeconds(0.5f);
            yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "I need to run." });
            enemy.enabled = true;

            //...and attacks player TODO:(warped scream sfx)
            //player stumbles, breaks free, then can move again and has to run away TODO: stumble sfx
            GameController.Instance.playerEventManager.FreezePlayer(false, false);
            GameController.Instance.playerEventManager.FreezePlayer(false, false);
            animator.enabled = false;
            yield return new WaitUntil(() => waitForContinue);
            waitForContinue = false;

            GameController.Instance.playerEventManager.FreezePlayer(false, true);
            metaPlayerWrap2.position = metaPlayer.transform.position;
            metaPlayerWrap2.rotation = metaPlayer.transform.rotation;
            metaPlayer.transform.SetParent(metaPlayerWrap2);
            animatorTumble.Rebind();
            animatorTumble.SetTrigger("Tumble");

            yield return new WaitUntil(() => animatorTumble.GetCurrentAnimatorStateInfo(0).IsName("Wait"));
            GameController.Instance.playerEventManager.FreezePlayer(false, false);
            animatorTumble.enabled = false;
            metaPlayer.transform.SetParent(metaPlayerWrap);
            yield return new WaitUntil(() => waitForContinue);
            waitForContinue = false;

            //spider catches up with player again, grabbes player and attacks again (more scream sfx)
            //last scream drawn out with echo, you close your eyes, everything is black, and when the echo stops, the game is over
            GameController.Instance.playerEventManager.FreezePlayer(false, true);
            metaPlayerWrap2.position = metaPlayer.transform.position;
            metaPlayerWrap2.rotation = metaPlayer.transform.rotation;
            metaPlayer.transform.SetParent(metaPlayerWrap2);
            animatorTumble.Rebind();
            animatorTumble.enabled = true;
            animatorTumble.SetTrigger("Tumble2");
            enemy.gameObject.SetActive(false);
            yield return new WaitUntil(() => animatorTumble.GetCurrentAnimatorStateInfo(0).IsName("Wait"));
            StartCoroutine(enemy2.AttackTarget());

            yield return new WaitUntil(() => waitForContinue);
            waitForContinue = false;
            animatorEyes.SetFloat("Speed", 3.5f);
            animatorEyes.SetTrigger("Close");

            //TODO: Start Credits Scene
            Application.Quit();
        }
    }
}
