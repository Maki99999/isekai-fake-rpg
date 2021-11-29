using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Default
{
    public class MonsterGlitchEffectReceiver : MonoBehaviour
    {
        public Volume volume;
        public AudioSource noiseSFX;
        public BlitRendererFeature.Blit rendererFeature;

        [Space(10), Header("GlitchEffect")]
        public float power = 0.2f;
        public float speed = 1;
        public float scale = 100;

        [Space(10), Header("ColorAdjustments")]
        public float contrast = 20;

        private float percent = 0f;
        public float desiredPercent = 0f;

        private float timeElapsed, previousFrameTime;
        private Material m_Material;

        static class ShaderIDs
        {
            internal readonly static int Power = Shader.PropertyToID("_GlitchPower");
            internal readonly static int Time = Shader.PropertyToID("_NoiseX");
            internal readonly static int Scale = Shader.PropertyToID("_GlitchScale");
        }

        private void Awake()
        {
            timeElapsed = 0;
            previousFrameTime = Time.time;
            m_Material = rendererFeature.settings.blitMaterial;
        }

        private void Update()
        {
            ColorAdjustments colorAdjustments;
            volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);

            percent = Mathf.Clamp01(Mathf.Lerp(percent, desiredPercent, 0.5f));

            colorAdjustments.active = percent > 0.05f;
            rendererFeature.SetActive(percent > 0.05f);

            noiseSFX.volume = percent > 0.05f ? percent : 0f;

            timeElapsed += percent * speed * (Time.time - previousFrameTime);
            previousFrameTime = Time.time;

            if (m_Material != null)
            {
                m_Material.SetFloat(ShaderIDs.Power, percent * power);
                m_Material.SetFloat(ShaderIDs.Time, timeElapsed);
                m_Material.SetFloat(ShaderIDs.Scale, percent * scale);
            }

            colorAdjustments.contrast.value = percent * contrast;

            desiredPercent = 0f;
        }
    }
}
