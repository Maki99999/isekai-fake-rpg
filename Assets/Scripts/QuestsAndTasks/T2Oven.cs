using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class T2Oven : MonoBehaviour, Useable
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
        public float foodSkippedSecondsPcMode = 222f;
        public float foodTimeMinutesMultiplier = 0.5f;

        [Space(10)]
        public PhoneHolding phone;

        [Space(10)]
        public AudioSource audioSource;
        public AudioClip somethingSFX;

        private OvenState ovenState = OvenState.IDLE;
        private int currentFood = 0;

        private IEnumerator Start()
        {
            GameController.Instance.playerEventManager.FreezePlayer(false, true);
            yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "I'm hungry." });
            if (GameController.Instance.inPcMode)
                yield return pcController.ToNonPcMode();
            fridgeTrigger.enabled = true;
            GameController.Instance.playerEventManager.FreezePlayer(false, false);
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
            if (ovenState == OvenState.IDLE)
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
            GameController.Instance.playerEventManager.FreezePlayer(false, true);
            GameController.Instance.fadingAnimator.SetBool("Black", true);
            yield return new WaitForSeconds(1.5f);

            GameController.Instance.playerEventManager.TeleportPlayer(false, teleportPos);
            foodObjects[currentFood].foodOnTrayRaw.SetActive(true);
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
            GameController.Instance.playerEventManager.FreezePlayer(false, true);
            ovenAnim.SetBool("PutIn", true);
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
            GameController.Instance.metaPlayer.AddItem(phone, true, false);
            phone.StartTimer(foodTimeMinutes, foodTimeMinutesMultiplier);
            float endTime = Time.time + (foodTimeMinutes * foodTimeMinutesMultiplier) * 60f;

            bool skippedTime = false;
            while (Time.time < endTime)
            {
                yield return new WaitForSeconds(1f);
                if (!skippedTime && GameController.Instance.inPcMode)
                {
                    skippedTime = true;
                    phone.SkipTime(foodSkippedSecondsPcMode);
                    endTime -= foodSkippedSecondsPcMode;
                }
            }

            foodObjects[currentFood].foodOnTrayRaw.SetActive(false);
            foodObjects[currentFood].foodOnTrayBurned.SetActive(true);
            ovenState = OvenState.FOOD_BURNED;

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
            GameController.Instance.playerEventManager.FreezePlayer(false, true);
            GameController.Instance.metaHouseController.SetFixedTime(22, 48);
            GameController.Instance.metaHouseController.LetTimeAdvance(true, foodTimeMinutesMultiplier);
            float overallTime = (foodTimeMinutes * foodTimeMinutesMultiplier) * 60f;

            GameController.Instance.dialogue.StartDialogueWithFreeze(new List<string>() { "I'll just look at a clock sporadically. Food should be done at 11:00." });
            yield return GameController.Instance.playerEventManager.LookAt(false, lookAtPos.position, 1f);
            GameController.Instance.playerEventManager.FreezePlayer(false, false);

            yield return new WaitForSeconds(overallTime / 2f);
            GameController.Instance.metaHouseController.StopWallClocks();

            //yield return new WaitForSeconds(overallTime / 2f);
            //foodObjects[currentFood].foodOnTrayRaw.SetActive(false);
            //foodObjects[currentFood].foodOnTrayDone.SetActive(true);
            //ovenState = OvenState.FOOD_DONE;

            yield return new WaitForSeconds(overallTime / 2f);
            foodObjects[currentFood].foodOnTrayRaw.SetActive(false);
            foodObjects[currentFood].foodOnTrayBurned.SetActive(true);
            ovenState = OvenState.FOOD_BURNED;
        }

        IEnumerator BakeWithPhoneClock()
        {
            GameController.Instance.metaHouseController.SetFixedTime(23, 18);
            GameController.Instance.metaHouseController.LetTimeAdvance(true, foodTimeMinutesMultiplier);
            GameController.Instance.metaPlayer.AddItem(phone, true, false);
            float overallTime = (foodTimeMinutes * foodTimeMinutesMultiplier) * 60f;

            GameController.Instance.dialogue.StartDialogueWithFreeze(new List<string>() { "The clocks seem to be broken, so I'll use the clock on my phone. Should be done at 11:30." });

            yield return new WaitForSeconds(overallTime);
            foodObjects[currentFood].foodOnTrayRaw.SetActive(false);
            foodObjects[currentFood].foodOnTrayDone.SetActive(true);
            ovenState = OvenState.FOOD_DONE;

            yield return new WaitForSeconds(overallTime / 2f);
            GameController.Instance.metaHouseController.LetTimeAdvance(false);
            foodObjects[currentFood].foodOnTrayRaw.SetActive(false);
            foodObjects[currentFood].foodOnTrayBurned.SetActive(true);
            ovenState = OvenState.FOOD_BURNED;
        }

        IEnumerator TakeOutFood()
        {
            GameController.Instance.playerEventManager.FreezePlayer(false, true);
            if (currentFood == 0 || currentFood == 2)
                GameController.Instance.metaPlayer.RemoveItem(phone);
            else if (currentFood == 1)
            {
                GameController.Instance.metaHouseController.brokenWallClocksAcknowledged = true;
                phone.ActivateClockApp();
            }

            ovenAnim.SetBool("PutIn", false);
            yield return new WaitForSeconds(5f);

            if (ovenState == OvenState.FOOD_BURNED)
            {
                if (currentFood == 1)
                    yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "Burned... The clocks stopped working." });
                else
                    yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "Burned..." });
            }

            GameController.Instance.fadingAnimator.SetBool("Black", true);
            yield return new WaitForSeconds(1.5f);

            foodObjects[currentFood].foodOnTrayDone.SetActive(false);
            foodObjects[currentFood].foodOnTrayBurned.SetActive(false);
            foodObjects[currentFood].foodOnTrayRaw.SetActive(false);
            if (ovenState == OvenState.FOOD_DONE)
                foodObjects[currentFood].foodWithPlate.SetActive(true);

            GameController.Instance.fadingAnimator.SetBool("Black", false);
            yield return new WaitForSeconds(1.5f);

            if (ovenState == OvenState.FOOD_DONE)
            {
                ovenState = OvenState.TRAY;
                GameController.Instance.storyManager.TaskFinished();
                fridgeTrigger.enabled = false;
                GameController.Instance.playerEventManager.FreezePlayer(false, false);
                gameObject.SetActive(false);
            }
            else if (ovenState == OvenState.FOOD_BURNED && currentFood < foodObjects.Length - 1)
            {
                yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "I have more food in the fridge." });
                currentFood++;
                ovenState = OvenState.TRAY;
                fridgeTrigger.enabled = true;
                GameController.Instance.playerEventManager.FreezePlayer(false, false);
            }
            else
            {
                //horror?
                Debug.Log("Boo!");
                GameController.Instance.playerEventManager.FreezePlayer(false, false);
            }
        }

        private enum OvenState
        {
            IDLE,
            TRAY,
            FOOD_NOT_DONE,
            FOOD_DONE,
            FOOD_BURNED
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
