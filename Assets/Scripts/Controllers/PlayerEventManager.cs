using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class PlayerEventManager : MonoBehaviour
    {
        DialogueManager dialogueManager;

        PlayerController gamePlayer;
        PlayerController metaPlayer;

        void Start()
        {
            gamePlayer = GameController.Instance.gamePlayer;
            metaPlayer = GameController.Instance.metaPlayer;
        }

        public void FreezePlayer(bool inGame, bool freeze)
        {
            if (inGame)
                gamePlayer.SetCanMove(!freeze);
            else
                metaPlayer.SetCanMove(!freeze);
        }

        public IEnumerator MoveRotatePlayer(bool inGame, Transform newPosition, float seconds = 2f, bool cameraPerspective = false, Vector3 offset = default)
        {
            if (inGame)
                yield return gamePlayer.MoveRotatePlayer(newPosition, seconds, cameraPerspective, offset);
            else
                yield return metaPlayer.MoveRotatePlayer(newPosition, seconds, cameraPerspective, offset);
        }

        public IEnumerator MovePlayer(bool inGame, Vector3 newPosition, float seconds = 2f)
        {
            if (inGame)
                yield return gamePlayer.MovePlayer(newPosition, seconds);
            else
                yield return metaPlayer.MovePlayer(newPosition, seconds);
        }

        public IEnumerator RotatePlayer(bool inGame, Quaternion newRotation, float seconds = 2f)
        {
            if (inGame)
                yield return gamePlayer.RotatePlayer(newRotation, seconds);
            else
                yield return metaPlayer.RotatePlayer(newRotation, seconds);
        }

        public void TeleportPlayer(bool inGame, Transform newPosition, bool cameraPerspective = false, Vector3 offset = default)
        {
            if (inGame)
                gamePlayer.TeleportPlayer(newPosition, cameraPerspective, offset);
            else
                metaPlayer.TeleportPlayer(newPosition, cameraPerspective, offset);
        }

        public IEnumerator PlayDialogue(List<string> texts, bool creepy = false)
        {
            yield return dialogueManager.StartDialogue(texts, creepy);
        }
    }
}
