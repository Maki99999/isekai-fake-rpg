using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    [RequireComponent(typeof(AudioSource))]
    public class GameAudio : MonoBehaviour
    {
        AudioSource audioSource;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        void Update()
        {
            //audioSource.panStereo = GameController.Instance.gameAudioPan;
        }

        //Stereo to mono
        void OnAudioFilterRead(float[] data, int channels)
        {
            if (channels != 2 || GameController.Instance == null) return;

            for (int i = 0; i < data.Length; i += 2)
            {
                float dataMono = data[i] * 0.5f + data[i + 1] * 0.5f;
                data[i] = data[i] * (1 - GameController.Instance.gameAudioFxStrength) + dataMono * GameController.Instance.gameAudioFxStrength;
                data[i + 1] = data[i + 1] * (1 - GameController.Instance.gameAudioFxStrength) + dataMono * GameController.Instance.gameAudioFxStrength;

                if (GameController.Instance.gameAudioPan < 0f && data[i + 1] != 0)
                {
                    data[i + 1] -= data[i + 1] * -GameController.Instance.gameAudioPan * GameController.Instance.gameAudioFxStrength;
                }
                else if (GameController.Instance.gameAudioPan > 0f && data[i] != 0)
                {
                    data[i] -= data[i] * GameController.Instance.gameAudioPan * GameController.Instance.gameAudioFxStrength;
                }
            }
        }
    }
}
