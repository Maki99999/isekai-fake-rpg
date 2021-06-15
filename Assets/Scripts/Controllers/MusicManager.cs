using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class MusicManager : MonoBehaviour
    {
        public AudioSource audioSource;
        public MusicToEnum[] musicToEnums;

        public float maxVolume = 0.7f;
        public float fadeTime = 2f;

        private Dictionary<MusicType, AudioClip[]> musicToEnumsDict = new Dictionary<MusicType, AudioClip[]>();
        private MusicType currentType = MusicType.NONE;
        private int currentClipNum;
        bool inTransition = false;

        void Start()
        {
            foreach (MusicToEnum musicToEnum in musicToEnums)
            {
                if (musicToEnumsDict.ContainsKey(musicToEnum.musicType))
                    Debug.LogWarning("Array contains a MusicType twice");
                else
                    musicToEnumsDict.Add(musicToEnum.musicType, musicToEnum.audioClips);
            }
            ChangeMusic(MusicType.TRAVEL);
        }

        void Update()
        {
            if (!inTransition && !audioSource.isPlaying)
            {
                ChangeMusic(currentType, true);
            }
        }

        public void ChangeMusic(MusicType type, bool nextClip = false)
        {
            if (currentType == type)
                return;
            StopAllCoroutines();
            currentType = type;
            currentClipNum = nextClip ? (currentClipNum + 1) % musicToEnumsDict[currentType].Length : 0;
            StartCoroutine(MusicFade(fadeTime));
        }

        IEnumerator MusicFade(float duration)
        {
            inTransition = true;
            float currentTime = 0;
            float start = audioSource.volume;

            float rate = 1f / (duration / 2f);
            float fSmooth;
            if (audioSource.isPlaying)
            {
                for (float f = 0f; f <= 1f; f += rate * Time.deltaTime)
                {
                    fSmooth = Mathf.SmoothStep(0f, 1f, f);
                    currentTime += Time.deltaTime;
                    audioSource.volume = Mathf.Lerp(start, 0, fSmooth);
                    yield return null;
                }
            }

            audioSource.clip = musicToEnumsDict[currentType][currentClipNum];
            audioSource.Play();

            for (float f = 0f; f <= 1f; f += rate * Time.deltaTime)
            {
                fSmooth = Mathf.SmoothStep(0f, 1f, f);
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(0, maxVolume, fSmooth);
                yield return null;
            }
            inTransition = false;
        }
    }

    [System.Serializable]
    public struct MusicToEnum
    {
        public MusicType musicType;
        public AudioClip[] audioClips;
    }

    public enum MusicType
    {
        TRAVEL,
        TOWN,
        CAVE,
        CAVE_BOSS,
        NONE
    }
}
