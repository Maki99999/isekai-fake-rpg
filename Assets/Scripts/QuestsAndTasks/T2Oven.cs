using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class T2Oven : MonoBehaviour, Useable, Task
    {
        public Outline outline;
        public Animator ovenAnim;
        public PcController pcController;

        [Space(10)]
        public GameObject tray;
        public UseableEventTrigger fridgeTrigger;
        public Transform teleportPos;
        public Transform lookAtPos;
        [SerializeField] private FoodObject[] foodObjects;
        public int foodTimeMinutes = 10;
        public float foodTimeMinutesMultiplier = 3f;

        [Space(10)]
        public PhoneHolding phone;

        [Space(10)]
        public AudioSource audioSource;

        [Space(10)]
        public Animator lastFoodHorrorEvent;
        public Transform ovenCamPos;
        public Transform facePos;
        public Transform origPosHelper;
        public Animator eyesAnim;
        public Animator fadeAnim;
        public AudioSource fireAudio;

        private OvenState ovenState = OvenState.IDLE;
        private int currentFood = 2;//0

        private IEnumerator Start()
        {
            GameController.Instance.playerEventManager.FreezePlayers(true, true);
            yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "I'm hungry." });
            if (GameController.Instance.inPcMode)
                yield return pcController.ToNonPcMode();
            fridgeTrigger.enabled = true;
            GameController.Instance.playerEventManager.FreezePlayers(false);
        }

        void Update()
        {
            outline.enabled = false;
        }

        public void LookingAt()
        {
            outline.enabled = true;
        }

        public void Use()
        {
            if (ovenState == OvenState.IDLE || ovenState == OvenState.TRAY)
                GameController.Instance.dialogue.StartDialogueWithFreeze(new List<string>() { "Food is in the fridge." });
            else if (ovenState == OvenState.FOOD_NOT_DONE)
                GameController.Instance.dialogue.StartDialogueWithFreeze(new List<string>() { "Food is not done yet." });
            else if (ovenState == OvenState.FOOD_DONE || ovenState == OvenState.FOOD_BURNED)
                StartCoroutine(TakeOutFood());
        }

        public bool BlockingPcMode()
        {
            if (ovenState == OvenState.IDLE || ovenState == OvenState.DONE)
            {
                GameController.Instance.dialogue.StartDialogueWithFreeze(new List<string>() { "I'm hungry." });
                return true;
            }
            return false;
        }

        public void TeleportAndPutIn()
        {
            StartCoroutine(TeleportAndPutInAnim());
        }

        IEnumerator TeleportAndPutInAnim()
        {
            GameController.Instance.playerEventManager.FreezePlayer(false, true, true);
            GameController.Instance.fadingAnimator.SetBool("Black", true);
            yield return new WaitForSeconds(1.5f);

            GameController.Instance.playerEventManager.TeleportPlayer(false, teleportPos);
            foodObjects[currentFood].foodOnTrayRaw?.SetActive(true);
            fridgeTrigger.enabled = false;
            if (ovenState == OvenState.IDLE)
            {
                ovenState = OvenState.TRAY;
                tray.SetActive(true);
            }

            GameController.Instance.fadingAnimator.SetBool("Black", false);
            yield return new WaitForSeconds(2f);

            GameController.Instance.playerEventManager.FreezePlayer(false, false);
            yield return PutInFood();
        }

        IEnumerator PutInFood()
        {
            GameController.Instance.playerEventManager.FreezePlayer(false, true, true);
            ovenAnim.SetBool("PutIn", true);
            audioSource.Play();
            yield return new WaitForSeconds(5f);
            ovenState = OvenState.FOOD_NOT_DONE;

            GameController.Instance.playerEventManager.FreezePlayer(false, false);

            if (currentFood == 0)
                yield return BakeWithTimer();
            if (currentFood == 1)
                yield return BakeWithClock();
            if (currentFood == 2)
                yield return BakeWithPhoneClock();
        }

        IEnumerator BakeWithTimer()
        {
            GameController.Instance.playerEventManager.FreezePlayers(true, true);
            GameController.Instance.metaPlayer.AddItem(phone, true, false);
            float endTime = Time.time + (foodTimeMinutes / foodTimeMinutesMultiplier) * 60f;
            phone.PrepareTimer(endTime, foodTimeMinutesMultiplier);
            yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "Time to play some more." });
            GameController.Instance.playerEventManager.FreezePlayers(false);
            yield return new WaitForSeconds(2.1f);

            phone.StartTime();
            endTime = Time.time + (foodTimeMinutes / foodTimeMinutesMultiplier) * 60f;
            phone.SetTime(endTime);
            GameController.Instance.controlsHelper.ShowControl(ControlsHelper.Control.TOGGLE_PHONE);
            bool showedControls = false;
            while (Time.time < endTime)
            {
                if (GameController.Instance.inPcMode && !showedControls)
                {
                    GameController.Instance.controlsHelper.ShowControl(ControlsHelper.Control.LOOK_AT_TIME);
                    GameController.Instance.controlsHelper.ShowControl(ControlsHelper.Control.GET_UP);
                    showedControls = true;
                }
                yield return null;
            }

            phone.StopTime();
            foodObjects[currentFood].foodOnTrayRaw?.SetActive(false);
            foodObjects[currentFood].foodOnTrayBurned?.SetActive(true);
            ovenState = OvenState.FOOD_BURNED;

            GameController.Instance.dialogue.StartDialogueWithFreeze(new List<string>() { "What does my timer say?" });
            while (ovenState == OvenState.FOOD_BURNED)
            {
                if (phone.IsLookingAtPhone())
                {
                    GameController.Instance.dialogue.StartDialogueWithFreeze(new List<string>() { "Forgot to unmute my phone... Food should be done already." });
                    break;
                }
                yield return new WaitForSeconds(0.1f);
            }
        }

        IEnumerator BakeWithClock()
        {
            GameController.Instance.playerEventManager.FreezePlayer(false, true, true);
            GameController.Instance.metaHouseController.SetFixedTime(22, 48);
            GameController.Instance.metaHouseController.LetTimeAdvance(true, foodTimeMinutesMultiplier);
            float overallTime = (foodTimeMinutes / foodTimeMinutesMultiplier) * 60f;

            GameController.Instance.dialogue.StartDialogueWithFreeze(new List<string>() { "I'll just look at a clock sporadically. Food should be done at 11:00.", "I'll play some more." });
            yield return GameController.Instance.playerEventManager.LookAt(false, lookAtPos.position, 1f);
            GameController.Instance.playerEventManager.FreezePlayer(false, false);

            yield return new WaitForSeconds(overallTime / 2f);
            GameController.Instance.metaHouseController.StopWallClocks();
            GameController.Instance.horrorEventManager.StartEvent("H1");

            yield return new WaitForSeconds(overallTime / 2f);
            foodObjects[currentFood].foodOnTrayRaw?.SetActive(false);
            foodObjects[currentFood].foodOnTrayBurned?.SetActive(true);
            ovenState = OvenState.FOOD_BURNED;
            GameController.Instance.dialogue.StartDialogueWithFreeze(new List<string>() { "...Shouldn't the food be ready by now?" });
        }

        IEnumerator BakeWithPhoneClock()
        {
            GameController.Instance.metaHouseController.SetFixedTime(23, 20);
            GameController.Instance.metaHouseController.LetTimeAdvance(true, foodTimeMinutesMultiplier);
            GameController.Instance.metaPlayer.AddItem(phone, true, false);
            float overallTime = (foodTimeMinutes / foodTimeMinutesMultiplier) * 60f;

            GameController.Instance.dialogue.StartDialogueWithFreeze(new List<string>() { "The clocks seem to be broken, so I'll use the clock on my phone. Should be done at 11:30.", "I really have to keep track of the time this time." });

            yield return new WaitForSeconds(overallTime);
            foodObjects[currentFood].foodOnTrayRaw?.SetActive(false);
            foodObjects[currentFood].foodOnTrayDone?.SetActive(true);
            ovenState = OvenState.FOOD_DONE;

            yield return new WaitForSeconds(overallTime / 2f);
            GameController.Instance.metaHouseController.LetTimeAdvance(false);
            foodObjects[currentFood].foodOnTrayRaw?.SetActive(false);
            foodObjects[currentFood].foodOnTrayBurned?.SetActive(true);
            ovenState = OvenState.FOOD_BURNED;
        }

        IEnumerator TakeOutFood()
        {
            GameController.Instance.playerEventManager.FreezePlayer(false, true, true);
            if (currentFood == 0 || currentFood == 2)
                GameController.Instance.metaPlayer.RemoveItem(phone);
            else if (currentFood == 1)
            {
                GameController.Instance.metaHouseController.brokenWallClocksAcknowledged = true;
                phone.ActivateClockApp();
            }

            ovenAnim.SetBool("PutIn", false);
            audioSource.Play();
            yield return new WaitForSeconds(5f);

            if (ovenState == OvenState.FOOD_BURNED)
            {
                if (currentFood == 0)
                    yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "Burned..." });
                else if (currentFood == 1)
                    yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "Burned... The clocks stopped working." });
                else
                {
                    yield return LastFoodHorrorEvent();
                    ovenState = OvenState.DONE;
                    fridgeTrigger.enabled = false;
                    GameController.Instance.playerEventManager.FreezePlayer(false, false);
                    gameObject.SetActive(false);
                    yield break;
                }
            }

            GameController.Instance.fadingAnimator.SetBool("Black", true);
            yield return new WaitForSeconds(1.5f);

            foodObjects[currentFood].foodOnTrayDone?.SetActive(false);
            foodObjects[currentFood].foodOnTrayBurned?.SetActive(false);
            foodObjects[currentFood].foodOnTrayRaw?.SetActive(false);
            if (ovenState == OvenState.FOOD_DONE)
                foodObjects?[currentFood].foodWithPlate?.SetActive(true);

            GameController.Instance.fadingAnimator.SetBool("Black", false);
            yield return new WaitForSeconds(1.5f);

            if (ovenState == OvenState.FOOD_DONE)
            {
                ovenState = OvenState.DONE;
                fridgeTrigger.enabled = false;
                GameController.Instance.playerEventManager.FreezePlayer(false, false);
                gameObject.SetActive(false);
            }
            else if (ovenState == OvenState.FOOD_BURNED && currentFood < foodObjects.Length - 1)
            {
                if (currentFood == 0)
                    yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "I have more food in the fridge." });
                else
                    yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "I just have one pizza left in the fridge. Probably have to buy something tomorrow." });
                currentFood++;
                ovenState = OvenState.TRAY;
                fridgeTrigger.enabled = true;
                GameController.Instance.playerEventManager.FreezePlayer(false, false);
            }
            else
            {
                GameController.Instance.playerEventManager.FreezePlayer(false, false);
            }
        }

        private IEnumerator LastFoodHorrorEvent()
        {
            lastFoodHorrorEvent.gameObject.SetActive(true);
            yield return GameController.Instance.playerEventManager.LookAt(false, facePos.position, 0.8f);

            yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "Isn't that too much smoke?" });

            lastFoodHorrorEvent.SetTrigger("Play");
            yield return new WaitForSeconds(1.2f);

            eyesAnim.SetFloat("Speed", 2.5f);
            eyesAnim.SetTrigger("Close");
            yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "It stings my eyes." });

            origPosHelper.position = GameController.Instance.metaPlayer.transform.position;
            origPosHelper.rotation = GameController.Instance.metaPlayer.transform.rotation;
            GameController.Instance.playerEventManager.TeleportPlayer(false, ovenCamPos, true);
            eyesAnim.SetFloat("Speed", 1 / 4f);
            eyesAnim.SetTrigger("Open");
            yield return new WaitForSeconds(6f);

            fadeAnim.SetBool("Red", true);
            fireAudio.Play();
            yield return new WaitForSeconds(7f);

            eyesAnim.SetFloat("Speed", 1f);
            eyesAnim.SetTrigger("Close");
            yield return new WaitForSeconds(1f);

            lastFoodHorrorEvent.gameObject.SetActive(false);
            GameController.Instance.playerEventManager.TeleportPlayer(false, origPosHelper, false);
            foodObjects[currentFood].foodOnTrayDone?.SetActive(false);
            foodObjects[currentFood].foodOnTrayBurned?.SetActive(false);
            foodObjects[currentFood].foodOnTrayRaw?.SetActive(false);
            foodObjects?[currentFood].foodWithPlate?.SetActive(true);
            fadeAnim.SetBool("Red", false);
            eyesAnim.SetFloat("Speed", 10f);
            eyesAnim.SetTrigger("Open");
            yield return new WaitForSeconds(0.2f);
            yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "Weird ... daydream?" });
        }

        public void SkipTask()
        {
            ovenState = OvenState.DONE;
            GameController.Instance.metaHouseController.brokenWallClocksAcknowledged = true;
            GameController.Instance.metaHouseController.StopWallClocks();
            phone.ActivateClockApp();
            gameObject.SetActive(false);
        }

        private enum OvenState
        {
            IDLE,
            TRAY,
            FOOD_NOT_DONE,
            FOOD_DONE,
            FOOD_BURNED,
            DONE
        }

        [System.Serializable]
        private struct FoodObject
        {
            public GameObject foodOnTrayDone;
            public GameObject foodOnTrayBurned;
            public GameObject foodOnTrayRaw;
            public GameObject foodWithPlate;
        }
    }
}
