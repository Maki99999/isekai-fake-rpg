using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

namespace Default
{
    public class SaveManager : MonoBehaviour
    {
        public Animator saveAnim;
        private static readonly string pathSuffix = "/progress.sav";

        private ICollection<ISaveDataObject> saveObjects;

        #region Debug
        public int debugGame;
        private static readonly string[] debugGameFiles = new string[]
        {
            //Start
            "{\"info\":{\"dictString\":{\"version\":\"0.1\"},\"dictList\":{}},\"saveManager\":{\"dictString\":{\"currentTaskId\":\"\",\"taskOnDelay\":\"\",\"taskOnDelayWaitQuest\":\"\"},\"dictList\":{\"finishedTasks\":[]}}}"
            ,
            //Machines done
            "{\"info\":{\"dictString\":{\"version\":\"0.3\"},\"dictList\":{}},\"horrorEventManager\":{\"dictString\":{\"delayedEvent\":\"\"},\"dictList\":{}},\"musicManager\":{\"dictString\":{\"currentType\":\"1\"},\"dictList\":{}},\"OverallStats\":{\"dictString\":{\"PlayerLevel\":\"1\",\"DistanceMovedSqrGame\":\"51\",\"DistanceRanSqrGame\":\"39\",\"HalfChargedShot\":\"2\",\"GotUp\":\"1\",\"DistanceMovedSqrMeta\":\"51\",\"ObjectsUsed\":\"17\"},\"dictList\":{\"StatIdList\":[\"PlayerLevel\",\"DistanceMovedSqrGame\",\"DistanceRanSqrGame\",\"HalfChargedShot\",\"GotUp\",\"DistanceMovedSqrMeta\",\"ObjectsUsed\"]}},\"questManager\":{\"dictString\":{\"currentQuestId\":\"Q1\",\"currentQuestState\":\"1\"},\"dictList\":{}},\"saveManager\":{\"dictString\":{\"currentTaskId\":\"\",\"taskOnDelay\":\"T2\",\"taskOnDelayWaitQuest\":\"Q1\"},\"dictList\":{\"finishedTasks\":[\"T4\"]}},\"T8Cracker\":{\"dictString\":{\"ColaCollected\":\"False\"},\"dictList\":{}},\"SavePositionManager\":{\"dictString\":{\"posGame\":\"446,73;60,09;611,21;-0,02912109;0,3009813;-0,00919581;-0,9531409;1;1;1\",\"posMeta\":\"-12;-0,87;4,06;0,1599204;0,6214368;-0,1315238;0,7556079;1;1;1\",\"inPcMode\":\"False\"},\"dictList\":{}},\"PlayerStatsGamePlayer\":{\"dictString\":{\"maxHp\":\"200\",\"maxMp\":\"100\",\"level\":\"1\",\"statsRegen.hpRegenMultiplier\":\"1\",\"statsRegen.mpRegenMultiplier\":\"1\",\"statPoints.hp\":\"5\",\"statPoints.hpRegen\":\"5\",\"statPoints.mp\":\"5\",\"statPoints.mpRegen\":\"5\",\"statPoints.freePoints\":\"0\",\"coins\":\"0\",\"levelInt\":\"1\",\"currentXp\":\"0\",\"lastNeededXp\":\"0\",\"nextNeededXp\":\"7\"},\"dictList\":{}},\"H1Call\":{\"dictString\":{\"nextCallClip\":\"1\"},\"dictList\":{}},\"ShopItemStaffs\":{\"dictString\":{\"currentItem\":\"0\"},\"dictList\":{}},\"ShopItemArmor\":{\"dictString\":{\"currentItem\":\"0\"},\"dictList\":{}},\"SpiderCrawl\":{\"dictString\":{\"triggered\":\"False\"},\"dictList\":{}},\"H3Window\":{\"dictString\":{\"triggered\":\"0\"},\"dictList\":{}},\"MetaEnviroment/Bedroom/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"SpiderHideBedroom\":{\"dictString\":{\"active\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Bathroom/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/UpperHallway/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/UpperHallway/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/LowerHallway/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/LowerHallway/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Garage/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/Garage/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"SpiderHideGarage\":{\"dictString\":{\"active\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Basement/Lamp\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Basement/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Basement/Lamp/Lamp_2\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"SpiderHideBasement\":{\"dictString\":{\"active\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Living Room/Lamp\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Living Room/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"SpiderHideLiving Room\":{\"dictString\":{\"active\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Dining Room/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/Dining Room/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"H11Glass\":{\"dictString\":{\"triggered\":\"1\"},\"dictList\":{}},\"H10FleshThing\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-12,56054;0,0639;-6,562059;0;-0,1074732;0;0,9942082;1;1;1\"},\"dictList\":{}},\"T2Oven\":{\"dictString\":{\"isActive\":\"False\",\"currentFood\":\"0\",\"ovenState\":\"0\"},\"dictList\":{}},\"MetaEnviroment/Kitchen/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/Kitchen/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Kitchen/Lamp/Lamp_2\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Kitchen/Lamp/Lamp_3\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Kitchen/Lamp/Lamp_4\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"knifeItem\":{\"dictString\":{\"enabled\":\"true\",\"equipped\":\"false\"},\"dictList\":{}},\"H10PizzaFleshPlateFake\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-9,343;-1;-0,232;0;0;0;1;1;1;1\"},\"dictList\":{}},\"H10PizzaFleshPlate\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-15,215;-0,009;-6,41;0;0;0;1;1;1;1\",\"plateFallTransform\":\"-9,343;-1;-0,232;0;0;0;1;1;1;1\"},\"dictList\":{}},\"SpiderHideKitchen\":{\"dictString\":{\"active\":\"False\"},\"dictList\":{}},\"Ending/Lightswitch Lamp SideTable/Lamp SideTable\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}}}"
            ,
            //Food done
            "{\"info\":{\"dictString\":{\"version\":\"0.3\"},\"dictList\":{}},\"horrorEventManager\":{\"dictString\":{\"delayedEvent\":\"\"},\"dictList\":{}},\"musicManager\":{\"dictString\":{\"currentType\":\"0\"},\"dictList\":{}},\"OverallStats\":{\"dictString\":{\"PlayerLevel\":\"10\",\"DistanceMovedSqrGame\":\"1421\",\"DistanceRanSqrGame\":\"1034\",\"HalfChargedShot\":\"39\",\"GotUp\":\"5\",\"DistanceMovedSqrMeta\":\"175\",\"ObjectsUsed\":\"31\",\"Jumped\":\"85\",\"Kill_Slimy Slime\":\"10\",\"PhoneToggled\":\"6\",\"LookedAtTime\":\"11\",\"ItemsBought\":\"2\",\"Bought_Weapon\":\"1\",\"Bought_Armor\":\"1\",\"Kill_Spider\":\"7\",\"Kill_Rat\":\"5\",\"DistanceSneakedSqrGame\":\"2\"},\"dictList\":{\"StatIdList\":[\"PlayerLevel\",\"DistanceMovedSqrGame\",\"DistanceRanSqrGame\",\"HalfChargedShot\",\"GotUp\",\"DistanceMovedSqrMeta\",\"ObjectsUsed\",\"Jumped\",\"Kill_Slimy Slime\",\"PhoneToggled\",\"LookedAtTime\",\"ItemsBought\",\"Bought_Weapon\",\"Bought_Armor\",\"Kill_Spider\",\"Kill_Rat\",\"DistanceSneakedSqrGame\"]}},\"questManager\":{\"dictString\":{\"currentQuestId\":\"Q6\",\"currentQuestState\":\"2\"},\"dictList\":{}},\"saveManager\":{\"dictString\":{\"currentTaskId\":\"T13\",\"taskOnDelay\":\"T8\",\"taskOnDelayWaitQuest\":\"Q3\"},\"dictList\":{\"finishedTasks\":[\"T4\",\"T2\"]}},\"T8Cracker\":{\"dictString\":{\"ColaCollected\":\"False\"},\"dictList\":{}},\"SavePositionManager\":{\"dictString\":{\"posGame\":\"271,09;62,94;573,08;-0,001689206;-0,2516468;-0,0004392184;0,9678175;1;1;1\",\"posMeta\":\"-4,38;-0,87;2,22;-0,006666499;0,9932814;-0,07642884;-0,08663888;1;1;1\",\"inPcMode\":\"False\"},\"dictList\":{}},\"PlayerStatsGamePlayer\":{\"dictString\":{\"maxHp\":\"300\",\"maxMp\":\"100\",\"level\":\"10,55814\",\"statsRegen.hpRegenMultiplier\":\"2\",\"statsRegen.mpRegenMultiplier\":\"3,1\",\"statPoints.hp\":\"5\",\"statPoints.hpRegen\":\"5\",\"statPoints.mp\":\"5\",\"statPoints.mpRegen\":\"7\",\"statPoints.freePoints\":\"7\",\"coins\":\"291\",\"levelInt\":\"10\",\"currentXp\":\"282\",\"lastNeededXp\":\"258\",\"nextNeededXp\":\"301\"},\"dictList\":{}},\"H1Call\":{\"dictString\":{\"nextCallClip\":\"2\"},\"dictList\":{}},\"ShopItemStaffs\":{\"dictString\":{\"currentItem\":\"1\"},\"dictList\":{}},\"ShopItemArmor\":{\"dictString\":{\"currentItem\":\"1\"},\"dictList\":{}},\"SpiderCrawl\":{\"dictString\":{\"triggered\":\"False\"},\"dictList\":{}},\"H3Window\":{\"dictString\":{\"triggered\":\"0\"},\"dictList\":{}},\"MetaEnviroment/Bedroom/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"SpiderHideBedroom\":{\"dictString\":{\"active\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Bathroom/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/UpperHallway/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/UpperHallway/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/LowerHallway/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/LowerHallway/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Garage/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/Garage/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"SpiderHideGarage\":{\"dictString\":{\"active\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Basement/Lamp\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Basement/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Basement/Lamp/Lamp_2\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"SpiderHideBasement\":{\"dictString\":{\"active\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Living Room/Lamp\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Living Room/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"SpiderHideLiving Room\":{\"dictString\":{\"active\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Dining Room/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/Dining Room/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"H11Glass\":{\"dictString\":{\"triggered\":\"1\"},\"dictList\":{}},\"H10FleshThing\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-12,56054;0,0639;-6,562059;0;-0,1074732;0;0,9942082;1;1;1\"},\"dictList\":{}},\"T2Oven\":{\"dictString\":{\"isActive\":\"False\",\"currentFood\":\"2\",\"ovenState\":\"5\"},\"dictList\":{}},\"MetaEnviroment/Kitchen/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/Kitchen/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Kitchen/Lamp/Lamp_2\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Kitchen/Lamp/Lamp_3\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Kitchen/Lamp/Lamp_4\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"knifeItem\":{\"dictString\":{\"enabled\":\"true\",\"equipped\":\"false\"},\"dictList\":{}},\"H10PizzaFleshPlateFake\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-5,094735;-1;-0,8972096;0;0;0;1;1;1;1\"},\"dictList\":{}},\"H10PizzaFleshPlate\":{\"dictString\":{\"fleshOn\":\"true\",\"transform\":\"-5,094735;-1;-0,8972096;-2,910383E-11;0;0;1;1;1;1\",\"plateFallTransform\":\"-5,094735;-1;-0,8972096;0;0;0;1;1;1;1\"},\"dictList\":{}},\"SpiderHideKitchen\":{\"dictString\":{\"active\":\"False\"},\"dictList\":{}},\"Ending/Lightswitch Lamp SideTable/Lamp SideTable\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}}}"
            ,
            //washed hands
            "{\"info\":{\"dictString\":{\"version\":\"0.3\"},\"dictList\":{}},\"horrorEventManager\":{\"dictString\":{\"delayedEvent\":\"\"},\"dictList\":{}},\"musicManager\":{\"dictString\":{\"currentType\":\"0\"},\"dictList\":{}},\"OverallStats\":{\"dictString\":{\"PlayerLevel\":\"10\",\"DistanceMovedSqrGame\":\"1421\",\"DistanceRanSqrGame\":\"1034\",\"HalfChargedShot\":\"39\",\"GotUp\":\"5\",\"DistanceMovedSqrMeta\":\"188\",\"ObjectsUsed\":\"34\",\"Jumped\":\"85\",\"Kill_Slimy Slime\":\"10\",\"PhoneToggled\":\"6\",\"LookedAtTime\":\"11\",\"ItemsBought\":\"2\",\"Bought_Weapon\":\"1\",\"Bought_Armor\":\"1\",\"Kill_Spider\":\"7\",\"Kill_Rat\":\"5\",\"DistanceSneakedSqrGame\":\"2\"},\"dictList\":{\"StatIdList\":[\"PlayerLevel\",\"DistanceMovedSqrGame\",\"DistanceRanSqrGame\",\"HalfChargedShot\",\"GotUp\",\"DistanceMovedSqrMeta\",\"ObjectsUsed\",\"Jumped\",\"Kill_Slimy Slime\",\"PhoneToggled\",\"LookedAtTime\",\"ItemsBought\",\"Bought_Weapon\",\"Bought_Armor\",\"Kill_Spider\",\"Kill_Rat\",\"DistanceSneakedSqrGame\"]}},\"questManager\":{\"dictString\":{\"currentQuestId\":\"Q6\",\"currentQuestState\":\"2\"},\"dictList\":{}},\"saveManager\":{\"dictString\":{\"currentTaskId\":\"\",\"taskOnDelay\":\"T8\",\"taskOnDelayWaitQuest\":\"Q3\"},\"dictList\":{\"finishedTasks\":[\"T4\",\"T2\",\"T13\"]}},\"T8Cracker\":{\"dictString\":{\"ColaCollected\":\"False\"},\"dictList\":{}},\"SavePositionManager\":{\"dictString\":{\"posGame\":\"271,09;62,94;573,08;-0,001689206;-0,2516468;-0,0004392184;0,9678175;1;1;1\",\"posMeta\":\"-15,94;2,13;-2,2;0,05185784;0,9255027;-0,1374917;0,3490724;1;1;1\",\"inPcMode\":\"False\"},\"dictList\":{}},\"PlayerStatsGamePlayer\":{\"dictString\":{\"maxHp\":\"300\",\"maxMp\":\"100\",\"level\":\"10,55814\",\"statsRegen.hpRegenMultiplier\":\"2\",\"statsRegen.mpRegenMultiplier\":\"3,1\",\"statPoints.hp\":\"5\",\"statPoints.hpRegen\":\"5\",\"statPoints.mp\":\"5\",\"statPoints.mpRegen\":\"7\",\"statPoints.freePoints\":\"7\",\"coins\":\"291\",\"levelInt\":\"10\",\"currentXp\":\"282\",\"lastNeededXp\":\"258\",\"nextNeededXp\":\"301\"},\"dictList\":{}},\"H1Call\":{\"dictString\":{\"nextCallClip\":\"2\"},\"dictList\":{}},\"ShopItemStaffs\":{\"dictString\":{\"currentItem\":\"1\"},\"dictList\":{}},\"ShopItemArmor\":{\"dictString\":{\"currentItem\":\"1\"},\"dictList\":{}},\"SpiderCrawl\":{\"dictString\":{\"triggered\":\"False\"},\"dictList\":{}},\"H3Window\":{\"dictString\":{\"triggered\":\"0\"},\"dictList\":{}},\"MetaEnviroment/Bedroom/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"SpiderHideBedroom\":{\"dictString\":{\"active\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Bathroom/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/UpperHallway/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/UpperHallway/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/LowerHallway/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/LowerHallway/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Garage/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/Garage/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"SpiderHideGarage\":{\"dictString\":{\"active\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Basement/Lamp\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Basement/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Basement/Lamp/Lamp_2\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"SpiderHideBasement\":{\"dictString\":{\"active\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Living Room/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/Living Room/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"SpiderHideLiving Room\":{\"dictString\":{\"active\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Dining Room/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/Dining Room/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"H11Glass\":{\"dictString\":{\"triggered\":\"1\"},\"dictList\":{}},\"H10FleshThing\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-12,56054;0,0639;-6,562059;0;-0,1074732;0;0,9942082;1;1;1\"},\"dictList\":{}},\"T2Oven\":{\"dictString\":{\"isActive\":\"False\",\"currentFood\":\"2\",\"ovenState\":\"5\"},\"dictList\":{}},\"MetaEnviroment/Kitchen/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/Kitchen/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Kitchen/Lamp/Lamp_2\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Kitchen/Lamp/Lamp_3\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Kitchen/Lamp/Lamp_4\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"knifeItem\":{\"dictString\":{\"enabled\":\"true\",\"equipped\":\"false\"},\"dictList\":{}},\"H10PizzaFleshPlateFake\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-5,094735;-1;-0,8972096;0;0;0;1;1;1;1\"},\"dictList\":{}},\"H10PizzaFleshPlate\":{\"dictString\":{\"fleshOn\":\"true\",\"transform\":\"-5,094735;-1;-0,8972096;-2,910383E-11;0;0;1;1;1;1\",\"plateFallTransform\":\"-5,094735;-1;-0,8972096;0;0;0;1;1;1;1\"},\"dictList\":{}},\"SpiderHideKitchen\":{\"dictString\":{\"active\":\"False\"},\"dictList\":{}},\"Ending/Lightswitch Lamp SideTable/Lamp SideTable\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}}}"
            ,
            //got crackers (before blackout)
            "{\"info\":{\"dictString\":{\"version\":\"0.3\"},\"dictList\":{}},\"horrorEventManager\":{\"dictString\":{\"delayedEvent\":\"H4\"},\"dictList\":{}},\"musicManager\":{\"dictString\":{\"currentType\":\"5\"},\"dictList\":{}},\"OverallStats\":{\"dictString\":{\"PlayerLevel\":\"10\",\"DistanceMovedSqrGame\":\"1766\",\"DistanceRanSqrGame\":\"1367\",\"HalfChargedShot\":\"39\",\"GotUp\":\"6\",\"DistanceMovedSqrMeta\":\"227\",\"ObjectsUsed\":\"46\",\"Jumped\":\"87\",\"Kill_Slimy Slime\":\"10\",\"PhoneToggled\":\"6\",\"LookedAtTime\":\"11\",\"ItemsBought\":\"2\",\"Bought_Weapon\":\"1\",\"Bought_Armor\":\"1\",\"Kill_Spider\":\"7\",\"Kill_Rat\":\"5\",\"DistanceSneakedSqrGame\":\"2\"},\"dictList\":{\"StatIdList\":[\"PlayerLevel\",\"DistanceMovedSqrGame\",\"DistanceRanSqrGame\",\"HalfChargedShot\",\"GotUp\",\"DistanceMovedSqrMeta\",\"ObjectsUsed\",\"Jumped\",\"Kill_Slimy Slime\",\"PhoneToggled\",\"LookedAtTime\",\"ItemsBought\",\"Bought_Weapon\",\"Bought_Armor\",\"Kill_Spider\",\"Kill_Rat\",\"DistanceSneakedSqrGame\"]}},\"questManager\":{\"dictString\":{\"currentQuestId\":\"Q7\",\"currentQuestState\":\"1\"},\"dictList\":{}},\"saveManager\":{\"dictString\":{\"currentTaskId\":\"\",\"taskOnDelay\":\"\",\"taskOnDelayWaitQuest\":\"\"},\"dictList\":{\"finishedTasks\":[\"T4\",\"T2\",\"T13\",\"T8\"]}},\"T8Cracker\":{\"dictString\":{\"ColaCollected\":\"True\"},\"dictList\":{}},\"SavePositionManager\":{\"dictString\":{\"posGame\":\"446,73;60,09;611,21;-0,02912109;0,3009813;-0,00919581;-0,9531409;1;1;1\",\"posMeta\":\"-19,52;2,13;2;-0,02956228;0,9460298;-0,09025916;-0,30985;1;1;1\",\"inPcMode\":\"True\"},\"dictList\":{}},\"PlayerStatsGamePlayer\":{\"dictString\":{\"maxHp\":\"300\",\"maxMp\":\"100\",\"level\":\"10,55814\",\"statsRegen.hpRegenMultiplier\":\"2\",\"statsRegen.mpRegenMultiplier\":\"3,1\",\"statPoints.hp\":\"5\",\"statPoints.hpRegen\":\"5\",\"statPoints.mp\":\"5\",\"statPoints.mpRegen\":\"7\",\"statPoints.freePoints\":\"7\",\"coins\":\"291\",\"levelInt\":\"10\",\"currentXp\":\"282\",\"lastNeededXp\":\"258\",\"nextNeededXp\":\"301\"},\"dictList\":{}},\"H1Call\":{\"dictString\":{\"nextCallClip\":\"2\"},\"dictList\":{}},\"ShopItemStaffs\":{\"dictString\":{\"currentItem\":\"1\"},\"dictList\":{}},\"ShopItemArmor\":{\"dictString\":{\"currentItem\":\"1\"},\"dictList\":{}},\"SpiderCrawl\":{\"dictString\":{\"triggered\":\"False\"},\"dictList\":{}},\"H3Window\":{\"dictString\":{\"triggered\":\"1\"},\"dictList\":{}},\"MetaEnviroment/Bedroom/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"SpiderHideBedroom\":{\"dictString\":{\"active\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Bathroom/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/UpperHallway/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/UpperHallway/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/LowerHallway/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/LowerHallway/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Garage/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/Garage/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"SpiderHideGarage\":{\"dictString\":{\"active\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Basement/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/Basement/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Basement/Lamp/Lamp_2\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"SpiderHideBasement\":{\"dictString\":{\"active\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Living Room/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/Living Room/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"SpiderHideLiving Room\":{\"dictString\":{\"active\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Dining Room/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/Dining Room/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"H11Glass\":{\"dictString\":{\"triggered\":\"1\"},\"dictList\":{}},\"H10FleshThing\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-12,56054;0,0639;-6,562059;0;-0,1074732;0;0,9942082;1;1;1\"},\"dictList\":{}},\"T2Oven\":{\"dictString\":{\"isActive\":\"False\",\"currentFood\":\"2\",\"ovenState\":\"5\"},\"dictList\":{}},\"MetaEnviroment/Kitchen/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/Kitchen/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Kitchen/Lamp/Lamp_2\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Kitchen/Lamp/Lamp_3\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Kitchen/Lamp/Lamp_4\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"knifeItem\":{\"dictString\":{\"enabled\":\"true\",\"equipped\":\"false\"},\"dictList\":{}},\"H10PizzaFleshPlateFake\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-5,094735;-1;-0,8972096;0;0;0;1;1;1;1\"},\"dictList\":{}},\"H10PizzaFleshPlate\":{\"dictString\":{\"fleshOn\":\"true\",\"transform\":\"-5,094735;-1;-0,8972096;-2,910383E-11;0;0;1;1;1;1\",\"plateFallTransform\":\"-5,094735;-1;-0,8972096;0;0;0;1;1;1;1\"},\"dictList\":{}},\"SpiderHideKitchen\":{\"dictString\":{\"active\":\"False\"},\"dictList\":{}},\"Ending/Lightswitch Lamp SideTable/Lamp SideTable\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}}}"
            ,
            //after the glitch in the basement, waiting for ending
            "{\"info\":{\"dictString\":{\"version\":\"0.3\"},\"dictList\":{}},\"horrorEventManager\":{\"dictString\":{\"delayedEvent\":\"\"},\"dictList\":{}},\"musicManager\":{\"dictString\":{\"currentType\":\"5\"},\"dictList\":{}},\"OverallStats\":{\"dictString\":{\"PlayerLevel\":\"24\",\"DistanceMovedSqrGame\":\"5713\",\"DistanceRanSqrGame\":\"4950\",\"HalfChargedShot\":\"89\",\"GotUp\":\"8\",\"DistanceMovedSqrMeta\":\"290\",\"ObjectsUsed\":\"55\",\"Jumped\":\"502\",\"Kill_Slimy Slime\":\"10\",\"PhoneToggled\":\"6\",\"LookedAtTime\":\"13\",\"ItemsBought\":\"5\",\"Bought_Weapon\":\"3\",\"Bought_Armor\":\"2\",\"Kill_Spider\":\"24\",\"Kill_Rat\":\"15\",\"DistanceSneakedSqrGame\":\"9\",\"LightToggled\":\"2\",\"Kill_Goblin\":\"11\",\"Kill_Goblin Lord\":\"1\",\"Kill_Real(?) Hero\":\"1\"},\"dictList\":{\"StatIdList\":[\"PlayerLevel\",\"DistanceMovedSqrGame\",\"DistanceRanSqrGame\",\"HalfChargedShot\",\"GotUp\",\"DistanceMovedSqrMeta\",\"ObjectsUsed\",\"Jumped\",\"Kill_Slimy Slime\",\"PhoneToggled\",\"LookedAtTime\",\"ItemsBought\",\"Bought_Weapon\",\"Bought_Armor\",\"Kill_Spider\",\"Kill_Rat\",\"DistanceSneakedSqrGame\",\"LightToggled\",\"Kill_Goblin\",\"Kill_Goblin Lord\",\"Kill_Real(?) Hero\"]}},\"questManager\":{\"dictString\":{\"currentQuestId\":\"Q13\",\"currentQuestState\":\"1\"},\"dictList\":{}},\"saveManager\":{\"dictString\":{\"currentTaskId\":\"\",\"taskOnDelay\":\"Ending\",\"taskOnDelayWaitQuest\":\"\"},\"dictList\":{\"finishedTasks\":[\"T4\",\"T2\",\"T13\",\"T8\",\"T12\",\"T14\"]}},\"T8Cracker\":{\"dictString\":{\"ColaCollected\":\"True\"},\"dictList\":{}},\"SavePositionManager\":{\"dictString\":{\"posGame\":\"475,74;60,09;659,85;0,173958;-0,2458904;-0,04490762;-0,9525019;1;1;1\",\"posMeta\":\"-12;-0,87;4,06;0,1599204;0,6214368;-0,1315238;0,7556079;1;1;1\",\"inPcMode\":\"False\"},\"dictList\":{}},\"PlayerStatsGamePlayer\":{\"dictString\":{\"maxHp\":\"700\",\"maxMp\":\"100\",\"level\":\"24,60504\",\"statsRegen.hpRegenMultiplier\":\"2,2\",\"statsRegen.mpRegenMultiplier\":\"5,2\",\"statPoints.hp\":\"15\",\"statPoints.hpRegen\":\"6\",\"statPoints.mp\":\"5\",\"statPoints.mpRegen\":\"14\",\"statPoints.freePoints\":\"13\",\"coins\":\"677\",\"levelInt\":\"24\",\"currentXp\":\"1337\",\"lastNeededXp\":\"1265\",\"nextNeededXp\":\"1384\"},\"dictList\":{}},\"H1Call\":{\"dictString\":{\"nextCallClip\":\"0\"},\"dictList\":{}},\"ShopItemStaffs\":{\"dictString\":{\"currentItem\":\"3\"},\"dictList\":{}},\"ShopItemArmor\":{\"dictString\":{\"currentItem\":\"2\"},\"dictList\":{}},\"SpiderCrawl\":{\"dictString\":{\"triggered\":\"True\"},\"dictList\":{}},\"H3Window\":{\"dictString\":{\"triggered\":\"1\"},\"dictList\":{}},\"MetaEnviroment/Bedroom/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"SpiderHideBedroom\":{\"dictString\":{\"active\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Bathroom/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/UpperHallway/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/UpperHallway/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/LowerHallway/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/LowerHallway/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Garage/Lamp\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Garage/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"SpiderHideGarage\":{\"dictString\":{\"active\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Basement/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/Basement/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Basement/Lamp/Lamp_2\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"SpiderHideBasement\":{\"dictString\":{\"active\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Living Room/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/Living Room/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"SpiderHideLiving Room\":{\"dictString\":{\"active\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Dining Room/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/Dining Room/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"H11Glass\":{\"dictString\":{\"triggered\":\"1\"},\"dictList\":{}},\"H10FleshThing\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-12,56054;0,0639;-6,562059;0;0,4994982;0;0,8663151;1;1;1\"},\"dictList\":{}},\"T2Oven\":{\"dictString\":{\"isActive\":\"False\",\"currentFood\":\"2\",\"ovenState\":\"5\"},\"dictList\":{}},\"MetaEnviroment/Kitchen/Lamp\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}},\"MetaEnviroment/Kitchen/Lamp/Lamp_1\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Kitchen/Lamp/Lamp_2\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Kitchen/Lamp/Lamp_3\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"MetaEnviroment/Kitchen/Lamp/Lamp_4\":{\"dictString\":{\"on\":\"False\"},\"dictList\":{}},\"knifeItem\":{\"dictString\":{\"enabled\":\"false\",\"equipped\":\"false\"},\"dictList\":{}},\"H10PizzaFleshPlateFake\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-5,094735;-1;-0,8972096;0;0;0;1;1;1;1\"},\"dictList\":{}},\"H10PizzaFleshPlate\":{\"dictString\":{\"fleshOn\":\"true\",\"transform\":\"-5,094735;-1;-0,8972096;-2,910383E-11;0;0;1;1;1;1\",\"plateFallTransform\":\"-5,094735;-1;-0,8972096;0;0;0;1;1;1;1\"},\"dictList\":{}},\"SpiderHideKitchen\":{\"dictString\":{\"active\":\"False\"},\"dictList\":{}},\"Ending/Lightswitch Lamp SideTable/Lamp SideTable\":{\"dictString\":{\"on\":\"True\"},\"dictList\":{}}}"
        };
        #endregion

        public void SaveGameNextFrame()
        {
            StartCoroutine(SaveGame());
        }

        private void Awake()
        {
            saveObjects = GetSaveDataObjects();
        }

        private IEnumerator SaveGame()
        {
            yield return new WaitForEndOfFrame();
            string path = Application.persistentDataPath + pathSuffix;

            SaveDataEntry generalInfo = new SaveDataEntry();
            generalInfo.Add("version", Application.version);

            Dictionary<string, SaveDataEntry> saveDict = new Dictionary<string, SaveDataEntry>();
            saveDict.Add("info", generalInfo);
            foreach (ISaveDataObject saveObject in saveObjects)
                saveDict.Add(saveObject.saveDataId, saveObject.Save());

            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(saveDict);

            StreamWriter writer = new StreamWriter(path);
            writer.WriteLine(jsonString);
            writer.Close();

            Debug.Log("Saved Game to:\n" + path);
            saveAnim.SetTrigger("Saved");
        }

        public void LoadGame()
        {
            string path = Application.persistentDataPath + pathSuffix;
            Dictionary<string, SaveDataEntry> saveDict = null;
            if (debugGame >= 0 && debugGame < debugGameFiles.Length)
                saveDict = JsonConvert.DeserializeObject<Dictionary<string, SaveDataEntry>>(debugGameFiles[debugGame]);
            else if (!File.Exists(path))
            {
                Debug.Log("New Save.");
            }
            else
            {
                StreamReader reader = new StreamReader(path);
                string jsonString = reader.ReadLine();
                reader.Close();

                saveDict = JsonConvert.DeserializeObject<Dictionary<string, SaveDataEntry>>(jsonString);

                if (!saveDict.ContainsKey("info"))
                {
                    Debug.Log("Save file is outdated or corrupted. Creating a new save file.");
                }
                else
                {
                    SaveDataEntry info = saveDict["info"];
                    Debug.Log("Loaded save file for version " + info.GetString("version", "null"));
                }
            }

            foreach (ISaveDataObject saveObject in saveObjects)
                if (saveObject != null && saveDict != null && saveDict.ContainsKey(saveObject.saveDataId))
                    saveObject.Load(saveDict[saveObject.saveDataId]);
                else if (saveObject != null)
                    saveObject.Load(null);
        }

        public ICollection<ISaveDataObject> GetSaveDataObjects()
        {
            var saveables = new List<ISaveDataObject>();
            var rootObjs = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var root in rootObjs)
                saveables.AddRange(root.GetComponentsInChildren<ISaveDataObject>(true));
            return saveables;
        }

        public static void DeleteSaveFile()
        {
            string path = Application.persistentDataPath + pathSuffix;
            if (File.Exists(path))
                File.Delete(path);
        }

        public static string TransformToString(Transform transform)
        {
            return $"{transform.position.x};{transform.position.y};{transform.position.z};{transform.rotation.x};{transform.rotation.y};{transform.rotation.z};{transform.rotation.w};{transform.localScale.x};{transform.localScale.y};{transform.localScale.z}";
        }

        public static void ApplyStringToTransform(Transform transform, string transformString)
        {
            string[] transformPartStrings = transformString.Split(';');
            float[] transformParts = new float[transformPartStrings.Length];
            for (int i = 0; i < transformPartStrings.Length; i++)
                transformParts[i] = float.Parse(transformPartStrings[i]);
            transform.position = new Vector3(transformParts[0], transformParts[1], transformParts[2]);
            transform.rotation = new Quaternion(transformParts[3], transformParts[4], transformParts[5], transformParts[6]);
            transform.localScale = new Vector3(transformParts[7], transformParts[8], transformParts[9]);
        }

        public static string GetGameObjectPath(Transform transform)
        {
            Transform lastTransform = transform;
            string path = lastTransform.name;
            while (lastTransform.parent != null)
            {
                lastTransform = lastTransform.parent;
                path = lastTransform.name + "/" + path;
            }
            return path;
        }
    }

    public interface ISaveDataObject
    {
        string saveDataId { get; }

        SaveDataEntry Save();
        void Load(SaveDataEntry dictEntry);
    }

    [System.Serializable]
    public class SaveDataEntry
    {
        [JsonProperty] private Dictionary<string, string> dictString { get; set; }
        [JsonProperty] private Dictionary<string, List<string>> dictList { get; set; }

        public SaveDataEntry()
        {
            dictString = new Dictionary<string, string>();
            dictList = new Dictionary<string, List<string>>();
        }

        public void Add(string key, string value)
        {
            if (dictList.ContainsKey(key) || dictString.ContainsKey(key))
            {
                Debug.LogError("Key already present");
                return;
            }
            dictString.Add(key, value);
        }

        public void Add(string key, int value)
        {
            if (dictList.ContainsKey(key) || dictString.ContainsKey(key))
            {
                Debug.LogError("Key already present");
                return;
            }
            dictString.Add(key, value.ToString());
        }

        public void Add(string key, float value)
        {
            if (dictList.ContainsKey(key) || dictString.ContainsKey(key))
            {
                Debug.LogError("Key already present");
                return;
            }
            dictString.Add(key, value.ToString());
        }

        public void Add(string key, bool value)
        {
            if (dictList.ContainsKey(key) || dictString.ContainsKey(key))
            {
                Debug.LogError("Key already present");
                return;
            }
            dictString.Add(key, value.ToString());
        }

        public void Add(string key, List<string> values)
        {
            if (dictList.ContainsKey(key) || dictString.ContainsKey(key))
            {
                Debug.LogError("Key already present");
                return;
            }
            dictList.Add(key, values);
        }

        public string GetString(string key, string defaultValue)
        {
            if (dictString.ContainsKey(key))
                return dictString[key];
            else
                return defaultValue;
        }

        public int GetInt(string key, int defaultValue)
        {
            int returnValue = defaultValue;
            bool returnSuccessful = dictString.ContainsKey(key) && int.TryParse(dictString[key], out returnValue);
            if (returnSuccessful)
                return returnValue;
            else
                return defaultValue;
        }

        public float GetFloat(string key, float defaultValue)
        {
            float returnValue = defaultValue;
            bool returnSuccessful = dictString.ContainsKey(key) && float.TryParse(dictString[key], out returnValue);
            if (returnSuccessful)
                return returnValue;
            else
                return defaultValue;
        }

        public bool GetBool(string key, bool defaultValue)
        {
            bool returnValue = defaultValue;
            bool returnSuccessful = dictString.ContainsKey(key) && bool.TryParse(dictString[key], out returnValue);
            if (returnSuccessful)
                return returnValue;
            else
                return defaultValue;
        }

        public List<string> GetList(string key, List<string> defaultValue)
        {
            if (dictList.ContainsKey(key))
                return dictList[key];
            else
                return defaultValue;
        }
    }
}
