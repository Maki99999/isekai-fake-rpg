using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class DialogueNode
    {
        public string currentText = null;
        public string currentNameText = "";
        public DialogueVoice currentVoice = DialogueVoice.Normal;
        public List<string> currentChoices = new List<string>();
        public List<DialogueNode> nextNodes = new List<DialogueNode>();
        public string end = null;

        public DialogueNode(string endName = null)
        {
            end = endName;
        }

        public DialogueNode(string name, string text, DialogueVoice voice = DialogueVoice.Normal)
        {
            currentText = text;
            currentNameText = name;
            currentVoice = voice;
            nextNodes.Add(new DialogueNode());
        }

        //Adds texts to the current node, returns last element
        public DialogueNode Add(List<string> names, List<string> texts, List<DialogueVoice> voices)
        {
            DialogueNode currentNode = currentText == null ? this : nextNodes[0];

            for (int i = 0; i < texts.Count; i++)
            {
                currentNode.currentNameText = names[i % names.Count];
                currentNode.currentText = texts[i % texts.Count];
                currentNode.currentVoice = voices[i % voices.Count];

                currentNode.nextNodes.Add(new DialogueNode());

                if (i < texts.Count - 1)
                    currentNode = currentNode.nextNodes[0];
            }

            return currentNode;
        }

        public DialogueNode Add(string name, List<string> texts, DialogueVoice voice = DialogueVoice.Normal)
        {
            List<string> names = new List<string>();
            for (int i = 0; i < texts.Count; i++)
                names.Add(name);
            return Add(names, texts, new List<DialogueVoice>() { voice });
        }

        public DialogueNode Add(string name, string text, DialogueVoice voice = DialogueVoice.Normal)
        {
            return Add(new List<string>() { name }, new List<string>() { text }, new List<DialogueVoice>() { voice });
        }

        public DialogueNode Add(DialogueNode node)
        {
            nextNodes[nextNodes.Count - 1] = node;

            return null;
        }
    }

    public class Dialogue
    {
        public static DialogueNode Cutscene01StartDialogue()
        {
            List<string> texts = new List<string>
            {
                "I'm done for today.",
                "You can go too after doing everything on the to-do list.",
                "So see ya."
            };

            DialogueNode node = new DialogueNode();
            node.Add("Manager", texts);
            return node;
        }

        public static DialogueNode AutoDoorDisappearDialogue()
        {
            List<string> texts = new List<string>
            {
                "§i...",
                "...",
                "What...",
                "WHAT THE HELL?!",
                "WHAT HAPPENED?!",
                "The door... just DISAPPEARED!",
                "Okay, I think I'm going crazy.",
                "I think I'll take another exit.",
                "The first room in the back has an exit."
            };

            DialogueNode node = new DialogueNode();
            node.Add("You", texts);
            return node;
        }

        public static DialogueNode InspectingBoxDialogue()
        {
            List<string> texts = new List<string>
            {
                "§iA box just fell from the shelf...",
                "Are these bones?",
                "No, that can't be..."
            };

            DialogueNode node = new DialogueNode();
            node.Add("You", texts);
            return node;
        }

        public static DialogueNode InspectingBoxDialogue2()
        {
            List<string> texts = new List<string>
            {
                "§iHuh?",
                "They're gone!",
                "Did I just imagine?",
                "Hmm, whatever! I should just put the mop away and go home."
            };

            DialogueNode node = new DialogueNode();
            node.Add("You", texts);
            return node;
        }

        public static DialogueNode AutoDoorAntiPlayerDialogue()
        {
            DialogueNode node = new DialogueNode();
            node.Add("You", "§iI still have stuff to do. I can check these on the to-do list.");
            return node;
        }

        public static DialogueNode LastShelfDialogue()
        {
            List<string> texts = new List<string>
            {
                "§iThat was the last one. Now I can finally go home.",
                "I still have the mop, so I have to put it back in a locker."
            };

            DialogueNode node = new DialogueNode();
            node.Add("You", texts);
            return node;
        }

        public static DialogueNode LightsOnDialogue()
        {
            List<string> texts = new List<string>
            {
                "§iOkay, Light is back on.",
                "Now, did I already clean all puddles?",
                "I should check that."
            };

            DialogueNode node = new DialogueNode();
            node.Add("You", texts);
            return node;
        }

        public static DialogueNode LightsOutDialogue()
        {
            List<string> texts = new List<string>
            {
                "§iWhat happened?",
                "Maybe the fuse has blown.",
                "But I need a flashlight first.",
                "There is one below the counter."
            };

            DialogueNode node = new DialogueNode();
            node.Add("You", texts);
            return node;
        }

        public static DialogueNode FinishedPuddlesDialogue()
        {
            List<string> texts = new List<string>
            {
                "§iWhy was this last puddle so red?",
                "And it smelled a bit like iron.",
                "Well, whatever, that was the last one.",
                "Now I have to refill the empty shelves!",
                "The products are in the boxes in the storage room.",
                "I'll just pick a random box from there and put the content on the right shelf."
            };

            DialogueNode node = new DialogueNode();
            node.Add("You", texts);
            return node;
        }

        public static DialogueNode Cutscene02StartDialogue()
        {
            List<string> texts = new List<string>
            {
                "...",
                "...",
                "§iOkay I give up. What the hell is this?",
                "There is only one logical explanation...",
                "I'm dreaming!\nAnd now that I know this, I just need to wake up! Easy peasy.",
                "...",
                "Now how do I wake up?",
                "Well it doesn't matter, I can just walk around here and wait for me to wake up!",
                "I mean, this place looks really cool."
            };

            DialogueNode node = new DialogueNode();
            node.Add("You", texts);
            return node;
        }

        public static DialogueNode Cutscene02FaceDialogue()
        {
            DialogueNode start = new DialogueNode();
            DialogueNode explanation = new DialogueNode();
            DialogueNode end = new DialogueNode();

            List<string> textsStart = new List<string>
            {
                "Oh, that face.",
                "Hello.",
                "Hello there. I heard you wanna wake up. I can help you with that.",
                "Sounds good.",
                "How about a nice little game.",
                "Nah I'm fine, I just wanna wake up.",
                "Let's play one little game and if you win, I'll tell you.",
                "And if I lose?",
                "If you lose, you can just try again.",
                "Alright. What's the game?"
            };

            List<string> names = new List<string>
            {
                "You",
                "You",
                "Face",
                "You",
                "Face",
                "You",
                "Face",
                "You",
                "Face",
                "You"
            };

            List<DialogueVoice> voices = new List<DialogueVoice>
            {
                DialogueVoice.Normal,
                DialogueVoice.Normal,
                DialogueVoice.Creepy,
                DialogueVoice.Normal,
                DialogueVoice.Creepy,
                DialogueVoice.Normal,
                DialogueVoice.Creepy,
                DialogueVoice.Normal,
                DialogueVoice.Creepy,
                DialogueVoice.Normal
            };

            List<string> textsExpl = new List<string>
            {
                "The game plays as follows:",
                "I'll drop random items and ask a simple question about them after that.",
                "For example: I'll drop 5 apples and 3 bananas and will ask if there were more apples or bananas.",
                "Did you understand?"
            };

            explanation = start.Add(names, textsStart, voices);
            end = explanation.Add("Face", textsExpl, DialogueVoice.Creepy);

            end.currentChoices.Add("No.");
            end.Add(explanation.nextNodes[0]);

            end.currentChoices.Add("Yes!");
            end.nextNodes.Add(new DialogueNode("Face", "Okay, then I'll start. We'll play 5 Rounds.", DialogueVoice.Creepy));

            return start;
        }

        public static DialogueNode Cutscene02RoundXReady(int roundNum)
        {
            DialogueNode start = new DialogueNode("Face", "Round " + roundNum + ". Are you ready?", DialogueVoice.Creepy);
            start.currentChoices.Add("Yes.");
            start.Add("Face", "Okay, here it comes.", DialogueVoice.Creepy);

            return start;
        }

        public static DialogueNode Cutscene02Round1(bool apples, int fruitCount)
        {
            string fruit = apples ? "apples" : "bananas";

            DialogueNode start = new DialogueNode("Face", "So, how many " + fruit + " did I throw?", DialogueVoice.Creepy);

            DialogueNode wrong = new DialogueNode("Face", "Wrong. Let's try again.", DialogueVoice.Creepy);
            wrong.Add(new DialogueNode("Wrong"));

            DialogueNode right = new DialogueNode("Face", "That's right!", DialogueVoice.Creepy);

            int wrongValue = (fruitCount + (Random.value < 0.5f ? -2 : 2));
            if (wrongValue < 0)
                wrongValue += 4;

            if (Random.value < 0.5f)
            {
                //First choice is right
                start.currentChoices.Add("" + fruitCount);
                start.Add(right);

                start.currentChoices.Add("" + wrongValue);
                start.nextNodes.Add(wrong);
            }
            else
            {
                //2nd is right
                start.currentChoices.Add("" + wrongValue);
                start.Add(wrong);

                start.currentChoices.Add("" + fruitCount);
                start.nextNodes.Add(right);
            }

            return start;
        }

        public static DialogueNode Cutscene02Round2(string typeA, string typeB, bool moreOfTypeA)
        {
            DialogueNode start = new DialogueNode("Face", "So, did I throw more " + typeA + " or did I throw more " + typeB + "?", DialogueVoice.Creepy);

            DialogueNode wrong = new DialogueNode("Face", "Wrong. Let's try again.", DialogueVoice.Creepy);
            wrong.Add(new DialogueNode("Wrong"));

            DialogueNode right = new DialogueNode("Face", "That's right! You're pretty good at this.", DialogueVoice.Creepy);

            if (moreOfTypeA)
            {
                //First choice is right
                start.currentChoices.Add("More " + typeA);
                start.Add(right);

                start.currentChoices.Add("More " + typeB);
                start.nextNodes.Add(wrong);
            }
            else
            {
                //2nd is right
                start.currentChoices.Add("More " + typeA);
                start.Add(wrong);

                start.currentChoices.Add("More " + typeB);
                start.nextNodes.Add(right);
            }

            return start;
        }

        public static DialogueNode Cutscene02Round3(string type, bool existed)
        {
            DialogueNode start = new DialogueNode("Face", "Alright, did I throw any " + type + "?", DialogueVoice.Creepy);

            DialogueNode wrong = new DialogueNode("Face", "Wrong. Let's try again.", DialogueVoice.Creepy);
            wrong.Add(new DialogueNode("Wrong"));

            DialogueNode right = new DialogueNode("Face", "Yes, that's right!", DialogueVoice.Creepy);

            if (existed)
            {
                //First choice is right
                start.currentChoices.Add("Yes");
                start.Add(right);

                start.currentChoices.Add("No");
                start.nextNodes.Add(wrong);
            }
            else
            {
                //2nd is right
                start.currentChoices.Add("Yes");
                start.Add(wrong);

                start.currentChoices.Add("No");
                start.nextNodes.Add(right);
            }

            return start;
        }

        public static DialogueNode Cutscene02Round4Whoops()
        {
            DialogueNode part1 = new DialogueNode("Face", "Whoops, you didn't see anything did you?", DialogueVoice.Creepy);
            DialogueNode part2 = new DialogueNode("Face", "It's nothing, let's just restart this round.", DialogueVoice.Creepy);

            //First choice is right
            part1.currentChoices.Add("Uhhh...");
            part1.Add(part2);

            part1.currentChoices.Add("What was that?");
            part1.nextNodes.Add(part2);

            return part1;
        }

        public static DialogueNode Cutscene02Round4A(string fruit, int fruitCount)
        {
            DialogueNode start = new DialogueNode("Face", "Okay, how many " + fruit + " did I throw?", DialogueVoice.Creepy);

            DialogueNode wrong = new DialogueNode("Face", "Wrong. Let's try again.", DialogueVoice.Creepy);
            wrong.Add(new DialogueNode("Wrong"));

            DialogueNode right = new DialogueNode("Face", "That's right! One Round left.", DialogueVoice.Creepy);

            int wrongValue = (fruitCount + (Random.value < 0.5f ? -2 : 2));
            if (wrongValue < 0)
                wrongValue += 4;

            if (Random.value < 0.5f)
            {
                //First choice is right
                start.currentChoices.Add("" + fruitCount);
                start.Add(right);

                start.currentChoices.Add("" + wrongValue);
                start.nextNodes.Add(wrong);
            }
            else
            {
                //2nd is right
                start.currentChoices.Add("" + wrongValue);
                start.Add(wrong);

                start.currentChoices.Add("" + fruitCount);
                start.nextNodes.Add(right);
            }

            return start;
        }

        public static DialogueNode Cutscene02Round4B(string typeA, string typeB, bool moreOfTypeA)
        {
            DialogueNode start = new DialogueNode("Face", "Okay, did I throw more " + typeA + " or did I throw more " + typeB + "?", DialogueVoice.Creepy);

            DialogueNode wrong = new DialogueNode("Face", "Wrong. Let's try again.", DialogueVoice.Creepy);
            wrong.Add(new DialogueNode("Wrong"));

            DialogueNode right = new DialogueNode("Face", "That's right! One Round left.", DialogueVoice.Creepy);

            if (moreOfTypeA)
            {
                //First choice is right
                start.currentChoices.Add("More " + typeA);
                start.Add(right);

                start.currentChoices.Add("More " + typeB);
                start.nextNodes.Add(wrong);
            }
            else
            {
                //2nd is right
                start.currentChoices.Add("More " + typeA);
                start.Add(wrong);

                start.currentChoices.Add("More " + typeB);
                start.nextNodes.Add(right);
            }

            return start;
        }

        public static DialogueNode Cutscene02Round4C(string type, bool existed)
        {
            DialogueNode start = new DialogueNode("Face", "Okay, did I throw any " + type + "?", DialogueVoice.Creepy);

            DialogueNode wrong = new DialogueNode("Face", "Wrong. Let's try again.", DialogueVoice.Creepy);
            wrong.Add(new DialogueNode("Wrong"));

            DialogueNode right = new DialogueNode("Face", "That's right! One Round left.", DialogueVoice.Creepy);

            if (existed)
            {
                //First choice is right
                start.currentChoices.Add("Yes");
                start.Add(right);

                start.currentChoices.Add("No");
                start.nextNodes.Add(wrong);
            }
            else
            {
                //2nd is right
                start.currentChoices.Add("Yes");
                start.Add(wrong);

                start.currentChoices.Add("No");
                start.nextNodes.Add(right);
            }

            return start;
        }

        public static DialogueNode Cutscene02Round5()
        {
            DialogueNode start = new DialogueNode("Face", "Final Question: How many people did I kill for this game?", DialogueVoice.Creepy);
            DialogueNode part02 = new DialogueNode();

            start.currentChoices.Add("Uhhh...");
            start.Add(part02);

            start.currentChoices.Add("Are you okay?");
            start.nextNodes.Add(part02);

            List<string> texts = new List<string>
            {
                "The answer is: not enough! I need to kill more! MORE!",
                "Okay, game is over, now I'll tell you how to wake up. It's pretty simple.",
                "If you die in this dream, you'll wake up.",
                "I don't believe you. Nothing happens when I jump down the island!",
                "Yes it's a bit more complicated. You see, this is my personal world.",
                "I decide who can die and who not. That's why you don't die in here when you jump down the islands.",
                "Okay. Then go and kill me.",
                "So there is a small problem with that.",
                "Huh?",
                "The problem is that this is your dream and I only exist in this dream. When you die, this dream will end and I will cease to exist.",
                "...So you don't wanna help me wake up.",
                "Nope. Sorry dude.",
                "And what do I do now?",
                "I don't know. Count sheeps or something.",
                "I'm off. Bye~!"
            };

            List<string> names = new List<string>
            {
                "Face",
                "Face",
                "Face",
                "You",
                "Face",
                "Face",
                "You",
                "Face",
                "You",
                "Face",
                "You",
                "Face",
                "You",
                "Face",
                "Face"
            };

            List<DialogueVoice> voices = new List<DialogueVoice>
            {
                DialogueVoice.Creepy,
                DialogueVoice.Creepy,
                DialogueVoice.Creepy,
                DialogueVoice.Normal,
                DialogueVoice.Creepy,
                DialogueVoice.Creepy,
                DialogueVoice.Normal,
                DialogueVoice.Creepy,
                DialogueVoice.Normal,
                DialogueVoice.Creepy,
                DialogueVoice.Normal,
                DialogueVoice.Creepy,
                DialogueVoice.Normal,
                DialogueVoice.Creepy,
                DialogueVoice.Creepy
            };

            part02.Add(names, texts, voices);

            return start;
        }

        public static DialogueNode Cutscene02End01()
        {
            List<string> texts = new List<string>
            {
                "§iWhat is something like this doing here?",
                "...Looks like a normal book."
            };

            DialogueNode node = new DialogueNode();
            node.Add("You", texts);
            return node;
        }

        public static DialogueNode Cutscene02End02()
        {
            List<string> texts = new List<string>
            {
                "§iYep, just a normal book. But what language is this.",
                "\"§nTeeray lala la rayteela Someelasoso so Solateesoso lala lateelala Sodoh?\"",
                "§iI don't think I've ever heard this language."
            };

            DialogueNode node = new DialogueNode();
            node.Add("You", texts);
            return node;
        }

        public static DialogueNode Cutscene02End03()
        {
            List<string> texts = new List<string>
            {
                "§iWhat's happening!?"
            };

            DialogueNode node = new DialogueNode();
            node.Add("You", texts);
            return node;
        }

        public static DialogueNode OneLineMonologue(string line)
        {
            DialogueNode node = new DialogueNode();
            node.Add("You", "§i" + line);
            return node;
        }
    }

    public enum DialogueVoice
    {
        Normal, Creepy
    }
}