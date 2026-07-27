using System;
using GameStateLabs;
using GameStateLabs.Common;
using UnityEngine;
using UnityEngine.Profiling;

namespace Match3
{
    public class LevelSelect : MonoBehaviour
    {
        [System.Serializable]
        public struct ButtonPlayerPrefs
        {
            public GameObject gameObject;
            public string playerPrefKey;
        };

        public ButtonPlayerPrefs[] buttons;

        private void InitGSL()
        {
            GslEvents.Initialize();
            Profiler.EndSample();
        }

        private void Start()
        {
            InitGSL();
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
            for (int i = 0; i < buttons.Length; i++)
            {
                int score = PlayerPrefs.GetInt(buttons[i].playerPrefKey, 0);

                for (int starIndex = 1; starIndex <= 3; starIndex++)
                {
                    Transform star = buttons[i].gameObject.transform.Find($"star{starIndex}");
                    star.gameObject.SetActive(starIndex <= score);
                }
            }
        }

        public void OnButtonPress(string levelName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
            
            var levelselected = new GslEvent("level_selected");
            levelselected.Track();

            var gslEvent = new GslEvent("level_start");
            gslEvent.SetCustomProperty("level_name",       new StringType(levelName));
            gslEvent.SetCustomProperty("level_number",     new IntType(10));
            gslEvent.SetCustomProperty("test",             new StringType("test"));
            gslEvent.SetCustomProperty("player_id",        new StringType("abc"));
            gslEvent.SetCustomProperty("difficulty",       new StringType("hard"));
            gslEvent.SetCustomProperty("score",            new IntType(1500));
            gslEvent.SetCustomProperty("coins",            new IntType(320));
            gslEvent.SetCustomProperty("lives_left",       new IntType(4));
            gslEvent.SetCustomProperty("powerup_active",   new BoolType(true));
            gslEvent.SetCustomProperty("time_elapsed",     new FloatType(125.7f)); // seconds
            gslEvent.SetCustomProperty("device_model",     new StringType("Pixel 7"));
            gslEvent.SetCustomProperty("os_version",       new StringType("Android 14"));
            gslEvent.SetCustomProperty("network_type",     new StringType("WiFi"));
            gslEvent.SetCustomProperty("country",          new StringType("IN"));
            gslEvent.Track();

            var gslEventBoosted = new GslEvent("score_boosted");
            gslEventBoosted.Track();


        }
    }
}