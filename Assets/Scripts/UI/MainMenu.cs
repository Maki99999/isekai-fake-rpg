using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Default
{
    public class MainMenu : MonoBehaviour
    {
        public Animator fadeAnim;
        public Animator newGameWarning;
        public AudioListener listener;
        public GameObject LoadingCircle;

        public IEnumerator LoadGameScene()
        {
            fadeAnim.SetBool("Black", true);
            LoadingCircle.SetActive(true);
            yield return new WaitForSeconds(2f);
            listener.enabled = false;
            SceneManager.LoadScene("Scene01");
        }

        public void OpenNewGameWarning()
        {
            newGameWarning.SetBool("Open", true);
        }

        public void CloseNewGameWarning()
        {
            newGameWarning.SetBool("Open", false);
        }

        public void NewGame()
        {
            SaveManager.DeleteSaveFile();
            StartCoroutine(LoadGameScene());
        }

        public void LoadGame()
        {
            StartCoroutine(LoadGameScene());
        }
    }
}
