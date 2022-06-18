using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Default
{
    public class EscapeMenuManager : MonoBehaviour
    {
        public AudioMixer audioMixer;
        public Animator animator;

        [Space(10)]
        public bool isMainMenu = false;
        public MeshRenderer screenMesh;
        public Camera gameCamera;
        public Camera eventCamera;

        [Space(10)]
        public Slider mainVolSlider;
        public Slider fxVolSlider;
        public Slider musicVolSlider;
        public Dropdown presetDropdown;
        public Dropdown resDropdown;
        public Text resText;
        public Toggle fullscreenToggle;
        public Toggle vSyncToggle;
        public Dropdown pcResDropdown;
        public Slider brightnessSlider;

        bool inMenu = false;
        bool inSubMenu = false;

        bool pressedLastFrame = false;

        Resolution[] resolutions;

        void Start()
        {
            InitDropdowns();
            SetPrefValues();
            if (isMainMenu)
                OpenMenu();
        }

        void Update()
        {
            if (InputSettings.PressingEscape() && !pressedLastFrame)
            {
                if (inMenu)
                {
                    if (inSubMenu)
                        CloseSubMenu();
                    else
                        CloseMenu();
                }
                else
                {
                    OpenMenu();
                }
            }
            pressedLastFrame = InputSettings.PressingEscape();
        }

        public void CloseMenu()
        {
            if (isMainMenu)
                return;
            animator.SetBool("EscIn", false);
            animator.SetBool("SettingsIn", false);
            PauseManager.Unpause();
            GameController.Instance.LockMouse();
            inMenu = false;
        }

        public void Resume()
        {
            CloseMenu();
        }

        public void OpenMenu()
        {
            animator.SetBool("SettingsIn", false);
            animator.SetBool("EscIn", true);
            if (!isMainMenu)
            {
                PauseManager.Pause();
                GameController.Instance.UnlockMouse();
            }
            inMenu = true;
        }

        public void OpenSettings()
        {
            inSubMenu = true;
            animator.SetBool("SettingsIn", true);
        }

        public void CloseSubMenu()
        {
            inSubMenu = false;
            animator.SetBool("SettingsIn", false);
            animator.SetBool("CreditsIn", false);
        }

        public void OpenCredits()
        {
            inSubMenu = true;
            animator.SetBool("CreditsIn", true);
        }

        public void CloseGame()
        {
            StartCoroutine(QuitNextFrame());
        }

        private IEnumerator QuitNextFrame()
        {
            yield return null;
            Application.Quit();
        }

        void InitDropdowns()
        {
            presetDropdown.AddOptions(new List<string>(QualitySettings.names));

            resolutions = Screen.resolutions;
            foreach (Resolution resolution in resolutions)
            {
                resDropdown.AddOptions(new List<string>() { resolution.width + " x " + resolution.height });
            }
        }

        void SetPrefValues()
        {
            float mainVol = PlayerPrefs.GetFloat("mainVol", 0f);
            audioMixer.SetFloat("mainVol", mainVol);
            mainVolSlider.value = DecibelToLinear(mainVol);

            float fxVol = PlayerPrefs.GetFloat("fxVol", 0f);
            audioMixer.SetFloat("metaFxVol", fxVol);
            audioMixer.SetFloat("gameFxVol", fxVol);
            audioMixer.SetFloat("uiVol", fxVol);
            fxVolSlider.value = DecibelToLinear(fxVol);

            float musicVol = PlayerPrefs.GetFloat("musicVol", 0f);
            audioMixer.SetFloat("metaMusicVol", musicVol);
            audioMixer.SetFloat("gameMusicVol", musicVol);
            musicVolSlider.value = DecibelToLinear(musicVol);


            int prefQualityLevel = PlayerPrefs.GetInt("QualityLevel", 0);
            QualitySettings.SetQualityLevel(prefQualityLevel);
            presetDropdown.value = prefQualityLevel;

            bool fullscreen = PlayerPrefs.GetInt("fullscreenOn", Screen.fullScreenMode == FullScreenMode.FullScreenWindow ? 1 : 0) != 0;
            fullscreenToggle.isOn = fullscreen;

            int resW = PlayerPrefs.GetInt("resW", -1);
            int resH = PlayerPrefs.GetInt("resH", -1);
            int selectedRes = -1;
            for (int i = 0; i < resolutions.Length; i++)
            {
                if (resolutions[i].width == resW && resolutions[i].height == resH)
                {
                    selectedRes = i;
                    break;
                }
            }
            if (selectedRes == -1)
            {
                Screen.fullScreenMode = fullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
                resText.text = "???";
            }
            else
            {
                Screen.SetResolution(resolutions[selectedRes].width, resolutions[selectedRes].height, fullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
                resDropdown.value = selectedRes;
            }

            int vSyncCount = PlayerPrefs.GetInt("vSyncCount", QualitySettings.vSyncCount);
            QualitySettings.vSyncCount = vSyncCount;
            vSyncToggle.isOn = vSyncCount >= 0;

            int pcRes = PlayerPrefs.GetInt("pcRes", 1);
            pcResDropdown.value = pcRes;
            SetPcRes(pcRes, false);

            float brightness = PlayerPrefs.GetFloat("brightness", 0.033f);
            RenderSettings.ambientLight = new Color(brightness, brightness, brightness);
            brightnessSlider.value = brightness;
        }

        public void SetMainVol(float vol0to1)
        {
            float vol = LinearToDecibel(vol0to1);
            audioMixer.SetFloat("mainVol", vol);
            PlayerPrefs.SetFloat("mainVol", vol);
        }

        public void SetFxVol(float vol0to1)
        {
            float vol = LinearToDecibel(vol0to1);
            audioMixer.SetFloat("metaFxVol", vol);
            audioMixer.SetFloat("gameFxVol", vol);
            audioMixer.SetFloat("uiVol", vol);
            PlayerPrefs.SetFloat("fxVol", vol);
        }

        public void SetMusicVol(float vol0to1)
        {
            float vol = LinearToDecibel(vol0to1);
            audioMixer.SetFloat("metaMusicVol", vol);
            audioMixer.SetFloat("gameMusicVol", vol);
            PlayerPrefs.SetFloat("musicVol", vol);
        }

        public void SetGraphicsPreset(int val)
        {
            QualitySettings.SetQualityLevel(val);
            PlayerPrefs.SetInt("QualityLevel", val);
        }

        private float LinearToDecibel(float linear)
        {
            float dB;

            if (linear != 0)
                dB = 20.0f * Mathf.Log10(linear);
            else
                dB = -144.0f;

            return dB;
        }

        private float DecibelToLinear(float dB)
        {
            return Mathf.Pow(10.0f, dB / 20.0f);
        }

        public void SetResolution(int val)
        {
            Screen.SetResolution(resolutions[val].width, resolutions[val].height, Screen.fullScreenMode);

            PlayerPrefs.SetInt("resW", resolutions[val].width);
            PlayerPrefs.SetInt("resH", resolutions[val].height);
        }

        public void SetFullScreen(bool val)
        {
            Screen.fullScreenMode = val ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
            PlayerPrefs.SetInt("fullscreenOn", val ? 1 : 0);
        }

        public void SetVSync(bool val)
        {
            QualitySettings.vSyncCount = val ? 1 : 0;
            PlayerPrefs.SetInt("vSyncCount", val ? 1 : 0);
        }

        public void SetPcRes(int step)
        {
            SetPcRes(step, true);
        }

        private void SetPcRes(int step, bool withSave = true)
        {
            int w;
            int h;
            float scale;

            switch (step)
            {
                case 0:
                    w = 854;
                    h = 480;
                    scale = 1.5f;
                    break;
                case 1:
                    goto default;
                case 2:
                    w = 1920;
                    h = 1080;
                    scale = 0.667f;
                    break;
                case 3:
                    w = 2560;
                    h = 1440;
                    scale = 0.5f;
                    break;
                case 4:
                    w = 3840;
                    h = 2160;
                    scale = 0.333f;
                    break;
                default:
                    w = 1280;
                    h = 720;
                    scale = 1f;
                    break;
            }

            if (gameCamera != null && gameCamera.targetTexture != null)
                gameCamera.targetTexture.Release();

            if (eventCamera != null && eventCamera.targetTexture != null)
                eventCamera.targetTexture.Release();

            RenderTexture newText = new RenderTexture(w, h, 24);

            if (gameCamera != null)
                gameCamera.targetTexture = newText;
            if (eventCamera != null)
                eventCamera.targetTexture = newText;

            if (screenMesh != null)
                screenMesh.material.SetTexture("_MainTex", newText);
            GameController.uiScaleFactor = scale;

            if (withSave)
                PlayerPrefs.SetInt("pcRes", step);
        }

        public void SetBrightness(float vol)
        {
            RenderSettings.ambientLight = new Color(vol, vol, vol);
            PlayerPrefs.SetFloat("brightness", vol);
        }
    }
}
