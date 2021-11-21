using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Yetman.PostProcess;

namespace Default
{
    public class MonsterGlitchEffectReceiver : MonoBehaviour
    {
        public Volume volume;
        public AudioSource noiseSFX;

        [Space(10), Header("GlitchEffect")]
        public float power = 0.2f;
        public float speed = 1;
        public float scale = 100;

        [Space(10), Header("ColorAdjustments")]
        public float contrast = 20;

        private float percent = 0f;
        public float desiredPercent = 0f;

        private void Update()
        {
            GlitchEffect glitchEffect;
            volume.profile.TryGet<GlitchEffect>(out glitchEffect);
            ColorAdjustments colorAdjustments;
            volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);

            percent = Mathf.Clamp01(Mathf.Lerp(percent, desiredPercent, 0.5f));

            glitchEffect.active = percent > 0.05f;
            colorAdjustments.active = percent > 0.05f;

            noiseSFX.volume = percent > 0.05f ? percent : 0f;

            glitchEffect.power.value = percent * power;
            glitchEffect.speed.value = percent * speed;
            glitchEffect.scale.value = percent * scale;
            colorAdjustments.contrast.value = percent * contrast;

            desiredPercent = 0f;
        }
    }
}
