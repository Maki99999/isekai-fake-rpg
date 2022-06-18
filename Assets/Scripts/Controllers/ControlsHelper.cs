using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Default
{
    public class ControlsHelper : MonoBehaviour, OverallStatsObserver
    {
        public TextMeshProUGUI metaText;
        public TextMeshProUGUI gameText;

        private static readonly Dictionary<Control, ControlInfo> controlInfos = new Dictionary<Control, ControlInfo>()
        {
            {Control.MOVE_META,     new ControlInfo("[WASD] Move", "DistanceMovedSqrMeta", 3)},             //Trailer
            {Control.INTERACT_META, new ControlInfo("[E]    Interact with objects", "ObjectsUsed")},        //Trailer
            {Control.TOGGLE_LIGHT,  new ControlInfo("[LMB]  Toggle light", "LightToggled", 2)},             //H4
            {Control.TOGGLE_PHONE,  new ControlInfo("[LMB]  Show/hide phone", "PhoneToggled", 2)},          //T2

            {Control.GET_UP,        new ControlInfo("[F]    Get up", "GotUp")},                             //T2
            {Control.LOOK_AT_TIME,  new ControlInfo("[Q]    Look at time", "LookedAtTime")},                //T2

            {Control.MOVE_GAME,     new ControlInfo("[WASD]  Move", "DistanceMovedSqrGame", 10, true)},     //Trailer
            {Control.RUN_GAME,      new ControlInfo("[Shift] Run", "DistanceRanSqrGame", 3, true)},         //Trailer
            {Control.SNEAK_GAME,    new ControlInfo("[Ctrl]  Sneak", "DistanceSneakedSqrGame", 1, true)},   //CaveSpawnTrigger
            {Control.JUMP_GAME,     new ControlInfo("[Space] Jump", "Jumped", 1, true)},                    //CaveSpawnTrigger
            {Control.BUY_ITEM,      new ControlInfo("[E]     Buy item", "ItemsBought", 1, true)},           //Q3
            {Control.CHARGE_ATTACK, new ControlInfo("[LMB]   Charge Attack", "HalfChargedShot", 2, true)},  //Q2
        };
        private static readonly Dictionary<string, Control> statToControl = new Dictionary<string, Control>();

        private OverallStats stats;

        private void Awake()
        {
            if (statToControl.Count == 0)
                foreach (Control control in controlInfos.Keys)
                    statToControl.Add(controlInfos[control].stat, control);
        }

        void Start()
        {
            stats = GameController.Instance.overallStats;
            foreach (ControlInfo controlInfo in controlInfos.Values)
                stats.AddObserver(this, controlInfo.stat);
            UpdateText();
        }

        public void ShowControl(Control control)
        {
            controlInfos[control].active = true;
            UpdateText();
        }

        public void HideControl(Control control)
        {
            controlInfos[control].active = false;
            UpdateText();
        }

        //unsave, use ShowControl if possible!
        public void ShowControlInt(int controlInt)
        {
            ShowControl((Control)controlInt);
        }

        public void OnStatsUpdated(string category)
        {
            ControlInfo controlInfo = controlInfos[statToControl[category]];
            if (controlInfo.active && stats.GetStat(category) >= controlInfo.statAmount - controlInfo.statAmountPre)
                controlInfo.active = false;

            UpdateText();
        }

        private void UpdateText()
        {
            List<string> newMetaTexts = new List<string>();
            int longestMetaText = 0;
            List<string> newGameTexts = new List<string>();
            int longestGameText = 0;

            foreach (ControlInfo controlInfo in controlInfos.Values)
                if (controlInfo.active)
                {
                    if (controlInfo.inGame)
                    {
                        newGameTexts.Add(controlInfo.controlText);
                        if (controlInfo.controlText.Length > longestGameText)
                            longestGameText = controlInfo.controlText.Length;
                    }
                    else
                    {
                        newMetaTexts.Add(controlInfo.controlText);
                        if (controlInfo.controlText.Length > longestMetaText)
                            longestMetaText = controlInfo.controlText.Length;
                    }
                }
            metaText.text = "<mspace=0.4em>";
            gameText.text = "<mspace=0.4em>";
            foreach (string text in newMetaTexts)
                metaText.text += text.PadRight(longestMetaText) + "<color=#00000000>.</color>\n";
            foreach (string text in newGameTexts)
                gameText.text += text.PadRight(longestGameText) + "<color=#00000000>.</color>\n";
        }

        private class ControlInfo
        {
            public readonly string controlText;
            public readonly string stat;
            public readonly int statAmount = 1;
            public readonly bool inGame = false;
            public int statAmountPre = 0;
            public bool active = false;

            public ControlInfo(string controlText, string stat, int statAmount = 1, bool inGame = false)
            {
                this.controlText = controlText;
                this.stat = stat;
                this.statAmount = statAmount;
                this.inGame = inGame;
            }
        }

        public enum Control
        {
            MOVE_META = 0,
            INTERACT_META = 1,
            TOGGLE_LIGHT = 2,
            TOGGLE_PHONE = 3,

            GET_UP = 4,
            LOOK_AT_TIME = 5,

            MOVE_GAME = 6,
            RUN_GAME = 7,
            SNEAK_GAME = 8,
            JUMP_GAME = 9,
            BUY_ITEM = 10,
            CHARGE_ATTACK = 11,
        }
    }
}
