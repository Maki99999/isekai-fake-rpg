using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Default
{
    public class King : MonoBehaviour
    {
        public Transform uiFollowPos;
        public Transform uiFollowPosServant;

        private static readonly Dictionary<string, string[]> textsQuestStart = new Dictionary<string, string[]> {
            {"Q2", new string[] {}},
            {"Q3", new string[] {
                "How did you even do that\nwith that weird stick.",
                "I don't like the look\nof it. Go buy any\nnew staff from the main street."
            }},
            {"Q6", new string[] {}},
            {"Q7", new string[] {
                "Something was blocking\nthe bridge outside of\nthe town. Go fix that."
            }},

            {"Q10", new string[] {
                "But he didn't saw\nthem, right?",
                "Go in the cave and\ncheck if there are\nreally goblins in\nthere!"
            }},
            {"Q11", new string[] {
                "You look extremely weak,\nbut maybe with better\nequipment and a little\nbit of training\nyou can eliminate them.",
                "Buy better equipment first."
            }},
            {"Q5", new string[] {
                "And now level up some more."
            }},
            {"Q4", new string[] {
                "Now hurry up and defeat\neverything in the cave!",
                "Especially the leader of the cave!"
            }},
            {"Q12", new string[] {
                "I don't even know\nif you are the hero!",
                "While you were away,\nanother person claimed\nhe's the hero.",
                "And he really looks\nlike the real deal,\nholding a holy sword and all.",
                "If you really are\nthe hero, defeat\nhim in a battle to death!",
                "Speak to him when\nyou are ready.\nHe should be somewhere\naround here."
            }},
            {"Q13", new string[] {
                "Now that we know that\nyou are the hero, we\ncan finally give you\nreal quests.",
                "Your first quest will\nbe to find the ancient\nscroll that was lost\na long time ago.",
                "They say it has the\npower to destroy a whole\nkingdom! I want that!",
                "But the things we\nknow of the current\nlocation of the scroll\nis little.",
                "So go and find it. The thing we\nknow is that its location\nhas something to do with a dragon."
            }},

            {"QEnd", new string[] {"Your next quest will\nbe to level up."}}
        };

        private static readonly Dictionary<string, string[]> textsQuestOngoing = new Dictionary<string, string[]> {
            {"Q2", new string[] {"Come back after you\ndefeated 10 slimes."}},
            {"Q3", new string[] {"Come back after you\nbought a new staff\nfrom the main street."}},
            {"Q6", new string[] {"Go kill 5 rats and spiders\nin the cave outside\nof the town."}},

            {"Q10", new string[] {
                "Go in the cave and\ncheck if there are\ngoblins in there!"
            }},
            {"Q11", new string[] {
                "Buy some better equipment."
            }},
            {"Q5", new string[] {
                "Level up some more."
            }},
            {"Q4", new string[] {
                "Hurry up and defeat\neverything in the cave!",
            }},
            {"Q12", new string[] {
                "Speak to the hero when\nyou are ready to battle\nto death. He should be\nsomewhere around here."
            }},
            {"Q13", new string[] {
                "Go and find the\nscroll of the legend. The thing we\nknow is that its location\nhas something to do with a dragon."
            }},

            {"QEnd", new string[] {"Please level up some more."}}
        };

        private static readonly Dictionary<string, string[]> textsQuestEnd = new Dictionary<string, string[]> {
            {"Q1", new string[] {}},
            {"Q2", new string[] {
                "Already given up?",
                "Oh, uh...\nSeems like you've already\ncompleted the quest.",
                "You were faster than you look."
            }},
            {"Q3", new string[] {
                "Finally.",
                "Give the old staff to Alfred.\nYou won't need that totally\ncompletely useless old staff anymore.",
            }},
            {"Q6", new string[] {
                "Uh. Yeah."
            }},

            {"Q9", new string[] {
                "G- Goblins?!",
            }},
            {"Q10", new string[] {
                "So there are really\ngoblins in there.",
                "That's bad! I don't\nwant them to form a\npeaceful civilization\nthat will overpower the\nhumans by just being\nmore human that us humans!"
            }},
            {"Q11", new string[] { }},
            {"Q5", new string[] {
                "Okay."
            }},
            {"Q4", new string[] {
                "Hurry up and defeat-",
                "O- Oh, you say you\nare already done?",
                "Did you bring the leaders\nhead as evidence? No?",
                "Now how do you think\nI'm supposed to know\nyou're telling the truth?",
            }},
            {"Q12", new string[] {
                "A- As suspected, you won.\nI never doubted you."
            }},
        };

        private static readonly string[] textBusy = new string[] { "I'm busy. Don't you have stuff to do?" };

        private bool inSpeech = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!inSpeech && other.CompareTag("Player") && !other.GetComponent<PlayerController>().IsFrozen())
            {
                TalkToPlayer();
            }
        }

        private void TalkToPlayer()
        {
            string currentQuestId = GameController.Instance.questManager.currentQuestId;
            if (currentQuestId == "Q1" || currentQuestId == "Q9")
                GameController.Instance.questManager.QuestAccomplished();

            if (GameController.Instance.questManager.currentQuestState == QuestManager.QuestState.PRE_ACCEPTED
                    && textsQuestStart.ContainsKey(currentQuestId))
                StartCoroutine(Dialogue());
            else if (GameController.Instance.questManager.currentQuestState == QuestManager.QuestState.PRE_CONFIRMED
                    && textsQuestEnd.ContainsKey(currentQuestId))
                StartCoroutine(Dialogue());
            else if (textsQuestOngoing.ContainsKey(currentQuestId))
                StartCoroutine(Dialogue());
        }

        private IEnumerator Dialogue()
        {
            string currentQuestId = GameController.Instance.questManager.currentQuestId;
            QuestManager.QuestState currentQuestState = GameController.Instance.questManager.currentQuestState;

            string[] texts;
            if (currentQuestState == QuestManager.QuestState.PRE_ACCEPTED && textsQuestStart.ContainsKey(currentQuestId))
                texts = textsQuestStart[currentQuestId];
            else if (currentQuestState == QuestManager.QuestState.PRE_CONFIRMED && textsQuestEnd.ContainsKey(currentQuestId))
                texts = textsQuestEnd[currentQuestId];
            else if (textsQuestOngoing.ContainsKey(currentQuestId))
                texts = textsQuestOngoing[currentQuestId];
            else
                yield break;

            inSpeech = true;
            if (currentQuestId == "Q2" && currentQuestState == QuestManager.QuestState.PRE_ACCEPTED)
            {
                yield return DialogueQ2();
            }
            else if (currentQuestId == "Q6" && currentQuestState == QuestManager.QuestState.PRE_ACCEPTED)
            {
                yield return DialogueQ6();
            }
            else
            {
                GameController.Instance.dialogueBubble.followPosition = uiFollowPos;
                yield return GameController.Instance.dialogueBubble.WriteTexts(texts);
            }

            if (GameController.Instance.questManager.currentQuestState == QuestManager.QuestState.PRE_ACCEPTED)
            {
                GameController.Instance.questManager.QuestAccepted();
            }

            inSpeech = false;

            if (GameController.Instance.questManager.currentQuestState == QuestManager.QuestState.PRE_CONFIRMED)
            {
                GameController.Instance.questManager.QuestConfirmed();
                TalkToPlayer();
            }
        }

        private IEnumerator DialogueQ2()
        {
            GameController.Instance.dialogueBubble.followPosition = uiFollowPos;
            yield return GameController.Instance.dialogueBubble.WriteTexts(new string[] { "Who is this?" });

            GameController.Instance.dialogueBubble.followPosition = uiFollowPosServant;
            yield return GameController.Instance.dialogueBubble.WriteTexts(new string[] { "It's the hero, your majesty!" });

            GameController.Instance.dialogueBubble.followPosition = uiFollowPos;
            yield return GameController.Instance.dialogueBubble.WriteTexts(new string[]
            {
                "Ah, another one of these people...",
                "Ahem!\nWelcome, hero!",
                "I have summoned\nyou here so you\ncan help us in defeating\nthe evil demon lord!",
                "But first i want\nto see what you\nare capable of.",
                "So your first quest\nis to defeat\n10 slimes.",
                "Come back after\nyou did so."
            });

        }

        private IEnumerator DialogueQ6()
        {
            GameController.Instance.dialogueBubble.followPosition = uiFollowPosServant;
            yield return GameController.Instance.dialogueBubble.WriteTexts(new string[]
            {
                "Huh? Ah, yes. Completely useless.",
                "I will just take that.\nThank you."
            });

            GameController.Instance.dialogueBubble.followPosition = uiFollowPos;
            yield return GameController.Instance.dialogueBubble.WriteTexts(new string[]
            {
                "The next thing you can do is...",
                "Is..."
            });

            GameController.Instance.dialogueBubble.followPosition = uiFollowPosServant;
            yield return GameController.Instance.dialogueBubble.WriteTexts(new string[]
            {
                "Taking care of the many\nspiders and rats in the cave?",
            });

            GameController.Instance.dialogueBubble.followPosition = uiFollowPos;
            yield return GameController.Instance.dialogueBubble.WriteTexts(new string[]
            {
                "Ah yes, that's it.",
                "Go to the cave just outside\nthe town and kill\nsome spiders and rats.",
                "I really hate seeing them."
            });
        }
    }
}
