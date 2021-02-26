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
            if (channels != 2 || GameController.Instance.inPcMode) return;

            for (int i = 0; i < data.Length; i += 2)
            {
                float dataMono = data[i] * 0.5f + data[i + 1] * 0.5f;
                data[i] = dataMono;
                data[i + 1] = dataMono;

                if (GameController.Instance.gameAudioPan < 0f && data[i + 1] != 0)
                {
                    data[i + 1] -= data[i + 1] * -GameController.Instance.gameAudioPan;
                }
                else if (GameController.Instance.gameAudioPan > 0f && data[i] != 0)
                {
                    data[i] -= data[i] * GameController.Instance.gameAudioPan;
                }
            }
        }
    }
}
