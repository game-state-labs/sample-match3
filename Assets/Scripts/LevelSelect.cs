using System;
using GameStateLabs;
using GameStateLabs.EventProperties;
using GameStateLabs.Items;
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
            var playerProps = GSLEvents.PlayerProperties;
            playerProps.CustomPlayerId = Guid.NewGuid().ToString();
            playerProps.InstallDate = "2025-01-10";
            playerProps.PlayerUsername = Guid.NewGuid().ToString();

            Profiler.BeginSample("GSLSDK.Initialization");
            GSLEvents.Initialize(playerProps.CustomPlayerId);
            Profiler.EndSample();

            var currency = new Currency("curr_gold", 100)
            {
                Name = "Gold"
            };
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
            Profiler.BeginSample("GSLSDK.Events");
            new LevelEvent(ILevelProps.LevelEventActions.Start, levelName)
                .Track();
            Profiler.EndSample();
        }
    }
}