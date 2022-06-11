using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Default
{
    public class Lamp : MonoBehaviour, UsesPower, ISaveDataObject
    {
        private Light[] lights;
        public bool isOnAtStart = false;
        [SerializeField] private LightMaterial[] materials;

        [Space(10)]
        public GameObject eyes;
        public AudioSource randomOffSound;

        private bool on;
        private bool powerOn = true;
        public static float randomChance = 0.05f;

        public string saveDataId => SaveManager.GetGameObjectPath(transform);

        void Awake()
        {
            on = isOnAtStart;

            Lamp[] lamps = GetComponentsInChildren<Lamp>(false);
            foreach (Lamp lamp in lamps)
                if (lamp != this)
                {
                    lamp.enabled = false;
                    materials = materials.Concat(lamp.materials).ToArray();
                }

            lights = GetComponentsInChildren<Light>(false);
            foreach (Light light in lights)
                light.enabled = on;

            SetMaterials(on);
        }

        void Start()
        {
            if (eyes != null)
                eyes.SetActive(false);
        }

        public void SetPower(bool powerOn)
        {
            this.powerOn = powerOn;
            if (powerOn)
            {
                foreach (Light light in lights)
                    light.enabled = on;

                SetMaterials(on);
            }
            else
            {
                foreach (Light light in lights)
                    light.enabled = false;
                if (eyes != null)
                    eyes.SetActive(false);

                SetMaterials(false);
            }
        }

        public void TurnOn()
        {
            on = true;

            if (powerOn)
            {
                foreach (Light light in lights)
                    light.enabled = on;

                SetMaterials(on);
            }

            if (eyes != null)
                eyes.SetActive(false);

            if (Random.value < randomChance)
                StartCoroutine(TurnOffAfter());
        }

        public void TurnOff()
        {
            on = false;

            if (powerOn)
            {
                foreach (Light light in lights)
                    light.enabled = on;

                SetMaterials(on);
            }

            if (eyes != null && Random.value < randomChance)
                eyes.SetActive(true);
        }

        public void Toggle()
        {
            StopAllCoroutines();
            if (on)
                TurnOff();
            else
                TurnOn();
        }

        IEnumerator TurnOffAfter()
        {
            yield return new WaitForSeconds(Random.Range(7f, 17f));
            if (on)
            {
                randomOffSound.Play();
                TurnOff();
            }
        }

        private void SetMaterials(bool onMat)
        {
            foreach (LightMaterial material in materials)
            {
                Material[] sharedMaterialsCopy = material.renderer.sharedMaterials;
                sharedMaterialsCopy[material.materialSlot] = onMat ? material.onMaterial : material.offMaterial;
                material.renderer.sharedMaterials = sharedMaterialsCopy;
            }
        }

        public SaveDataEntry Save()
        {
            SaveDataEntry entry = new SaveDataEntry();
            entry.Add("on", on);
            return entry;
        }

        public void Load(SaveDataEntry dictEntry)
        {
            if (dictEntry == null || !isActiveAndEnabled)
                return;
            on = dictEntry.GetBool("on", on);
            if (powerOn)
                foreach (Light light in lights)
                    light.enabled = on;
        }

        [System.Serializable]
        private class LightMaterial
        {
            public MeshRenderer renderer;
            public int materialSlot;
            public Material offMaterial;
            public Material onMaterial;
        }
    }
}
