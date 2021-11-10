using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class Trailer : MonoBehaviour
    {
        public Animator mainAnim;
        public Animator dragonAnim;

        [Space(10)]
        public GameObject gamePlayer;
        public GameObject eventCamera;
        public Transform gamePlayerPosAfterTrailer;

        [Space(10)]
        public PcController pcController;
        public GameObject[] gameGUIStuff;
        public Animator guiAnim;
        public AudioSource guiAudio;

        [Space(10)]
        public ParticleSystem dragonParticles;
        public AudioSource dragonAudio;
        public AudioClip dragonFlap;
        public AudioClip dragonRoar;
        public AudioClip dragonMoan;
        private bool dragonFlying = true;

        public void StartTrailer()
        {
            dragonFlying = true;
            foreach (GameObject gameGUI in gameGUIStuff)
                gameGUI.SetActive(false);
            GameController.Instance.musicManager.ChangeMusic(MusicType.TRAILER);
            GameController.Instance.playerEventManager.FreezePlayers(true);
            dragonAnim.gameObject.SetActive(true);
            pcController.ToPcModeInstant();
            GameController.Instance.playerEventManager.FreezePlayers(true);
            gamePlayer.SetActive(false);
            eventCamera.SetActive(true);
            mainAnim.SetTrigger("Start");
        }

        public void PlayDragonFlyAudio()
        {
            StartCoroutine(DragonFlyAudio());
        }

        IEnumerator DragonFlyAudio()
        {
            dragonAudio.clip = dragonMoan;
            dragonAudio.Play();

            yield return new WaitForSeconds(1.5f);
            dragonAudio.clip = dragonFlap;
            dragonAudio.Play();

            while (dragonFlying)
            {
                yield return new WaitForSeconds(1f);
                dragonAudio.Play();
            }
        }

        public void TrailerEndDragon()
        {
            StartCoroutine(TrailerEnd());
        }

        IEnumerator TrailerEnd()
        {
            dragonAnim.SetTrigger("Roar");
            dragonFlying = false;
            yield return new WaitForSeconds(.9f);
            dragonAudio.clip = dragonRoar;
            dragonAudio.Play();
            dragonParticles.gameObject.SetActive(true);
            yield return new WaitForSeconds(1.2f);
            dragonParticles.Stop();

            yield return new WaitForSeconds(2f);
            guiAnim.SetTrigger("Title");
            yield return new WaitForSeconds(4f);

            yield return GameController.Instance.dialogue.StartDialogue(dFirstImpression);
            yield return new WaitForSeconds(1f);
            guiAnim.SetTrigger("StartGame");
            guiAudio.Play();
            yield return new WaitForSeconds(3.5f);
            ResetTrailerStuff();
            GameController.Instance.playerEventManager.TeleportPlayer(true, gamePlayerPosAfterTrailer);
            GameController.Instance.musicManager.ChangeMusic(MusicType.TOWN);
            yield return new WaitForSeconds(3.5f);

            //Game Start
            yield return new WaitForSeconds(1.5f);
            yield return GameController.Instance.dialogue.StartDialogue(dFirstQuest);
            yield return new WaitForSeconds(0.5f);
            GameController.Instance.playerEventManager.FreezePlayers(false);

            yield return new WaitForSeconds(9f);
            GameController.Instance.playerEventManager.FreezePlayers(true);
            yield return GameController.Instance.dialogue.StartDialogue(dCannotRelax1);
            yield return new WaitForSeconds(0.5f);
            GameController.Instance.playerEventManager.FreezePlayers(false);

            yield return new WaitForSeconds(6f);
            GameController.Instance.playerEventManager.FreezePlayers(true);
            StartCoroutine(pcController.ToNonPcMode());
            yield return GameController.Instance.dialogue.StartDialogue(dCannotRelax2);
            yield return new WaitForSeconds(0.5f);
            GameController.Instance.playerEventManager.FreezePlayers(false);
            GameController.Instance.storyManager.StartTask("T4");
        }

        public void ResetTrailerStuff()
        {
            foreach (GameObject gameGUI in gameGUIStuff)
                gameGUI.SetActive(true);
            dragonAnim.gameObject.SetActive(false);
            gamePlayer.SetActive(true);
            GameController.Instance.playerEventManager.FreezePlayers(false);
            eventCamera.SetActive(false);
        }

        private List<string> dFirstImpression = new List<string>() { "This game looks so awesome! I can't wait to play this all night!" };
        private List<string> dFirstQuest = new List<string>() { "So first I have to speak to the king...", "He's probably in the center of the city." };
        private List<string> dCannotRelax1 = new List<string>() { "...", "I still have chores to do." };
        private List<string> dCannotRelax2 = new List<string>() { "...", "I can't relax yet. I'll do the chores real quick and then I can play without worries." };
    }
}
