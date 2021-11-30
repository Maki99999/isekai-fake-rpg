using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class SavePositionManager : MonoBehaviour, ISaveDataObject
    {
        [SerializeField] private PlayerController gamePlayer;
        [SerializeField] private PlayerController metaPlayer;

        [SerializeField] private Transform temp;
        [SerializeField] private Transform[] savePositions;

        [SerializeField] private PcController pcController;

        public string saveDataId => "SavePositionManager";

        private Transform GetSaveLocation(Transform playerTransform)
        {
            Transform nearestTransform = savePositions[0];
            float nearestDistanceSqr = (playerTransform.position - nearestTransform.position).sqrMagnitude;

            foreach (Transform savePosition in savePositions)
            {
                if (nearestDistanceSqr > (playerTransform.position - savePosition.position).sqrMagnitude)
                {
                    nearestTransform = savePosition;
                    nearestDistanceSqr = (playerTransform.position - savePosition.position).sqrMagnitude;
                }
            }
            return nearestTransform;
        }

        public SaveDataEntry Save()
        {
            SaveDataEntry entry = new SaveDataEntry();
            entry.Add("posGame", SaveManager.TransformToString(GetSaveLocation(gamePlayer.transform)));
            entry.Add("posMeta", SaveManager.TransformToString(GetSaveLocation(metaPlayer.transform)));
            entry.Add("inPcMode", GameController.Instance.inPcMode);
            return entry;
        }

        public void Load(SaveDataEntry dictEntry)
        {
            if (dictEntry == null)
                return;

            string posString = dictEntry.GetString("posGame", null);
            if (posString != null)
            {
                SaveManager.ApplyStringToTransform(temp, posString);
                gamePlayer.TeleportPlayer(temp);
            }
            if (dictEntry.GetBool("inPcMode", GameController.Instance.inPcMode))
                pcController.ToPcModeInstant();
            else
            {
                posString = dictEntry.GetString("posMeta", null);
                if (posString != null)
                {
                    SaveManager.ApplyStringToTransform(temp, posString);
                    metaPlayer.TeleportPlayer(temp);
                }
            }
        }
    }
}
