using System;
using System.Collections;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using BepInEx.Configuration;
using UnityEngine;
using System.Reflection;
using PolyTechFramework;
namespace MoreSimSpeeds {
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    [BepInDependency(PolyTechMain.PluginGuid, BepInDependency.DependencyFlags.HardDependency)]
    
    
    public class MoreSimSpeeds : PolyTechMod {
        
        
        public const string pluginGuid = "polytech.MoreSimSpeeds";
        public const string pluginName = "More Sim Speeds";
        public const string pluginVersion = "1.0.0";
        public static string lastlist = "";
        
        public static ConfigEntry<bool> mEnabled;
        public static ConfigEntry<string> speedlist;
        public static ConfigEntry<int> defaultIndex;
        
        public ConfigDefinition mEnabledDef = new ConfigDefinition(pluginVersion, "Enable/Disable Mod");
        public ConfigDefinition speedlistDef = new ConfigDefinition(pluginVersion, "Speed List");
        public ConfigDefinition defaultIndexDef = new ConfigDefinition(pluginVersion, "Default Speed Index");
        
        
        
        
        public override void enableMod(){
            mEnabled.Value = true;
            this.isEnabled = true;
            lastlist = null;
        }
        public override void disableMod(){
            mEnabled.Value = false;
            this.isEnabled = false;
            //TODO: Make this work
            //PolyTechUtils.setModdedSimSpeeds();
        }
        public override string getSettings(){
            return "";
        }
        public override void setSettings(string settings){
            
        }
        public MoreSimSpeeds(){
            
            mEnabled = Config.Bind(mEnabledDef, false, new ConfigDescription("Controls if the mod should be enabled or disabled", null, new ConfigurationManagerAttributes {Order = 0}));
            speedlist = Config.Bind(speedlistDef, "20, 50, 100, 200, 300", new ConfigDescription("List of Available Speeds", null, new ConfigurationManagerAttributes {Order = -1}));
            defaultIndex = Config.Bind(defaultIndexDef, 2, new ConfigDescription("index starting at 0 of what the default speed should be", null, new ConfigurationManagerAttributes {Order = -2}));
        }
        void Awake(){
            
            this.repositoryUrl = null;
            this.isCheat = true;
            PolyTechMain.registerMod(this);
            Logger.LogInfo("More Sim Speeds Registered");
            Harmony.CreateAndPatchAll(typeof(MoreSimSpeeds));
            Logger.LogInfo("More Sim Speeds Methods Patched");
        }
        void Update(){
            if(lastlist != speedlist.Value){
                lastlist = speedlist.Value;
                if(mEnabled.Value){
                    string[] stringList = speedlist.Value.Replace(" ", "").Split(',');
                    float[] floatList = new float[stringList.Length];
                    for(int i = 0; i < stringList.Length; i++){
                        floatList[i] = float.Parse(stringList[i]) / 100f;
                    }
                    Bridge.NUM_SIMULATION_SPEEDS = floatList.Length;
                    Bridge.DEFAULT_SIMULATION_SPEED_INDEX = defaultIndex.Value;
                    Bridge.m_SimulationSpeeds = floatList;
                }
            }
        }
    }
}