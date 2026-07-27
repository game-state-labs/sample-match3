using System.Collections;
using System.Collections.Generic;
using GameStateLabs;
using GameStateLabs.Common;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;
using Event = UnityEngine.Event;

namespace Match3
{
    public class GameOver : MonoBehaviour
    {
        public GameObject screenParent;
        public GameObject scoreParent;
        public Text loseText;
        public Text scoreText;
        public Image[] stars;

        private void Start ()
        {
            screenParent.SetActive(false);

            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].enabled = false;
            }
        }

        public void ShowLose()
        {
            screenParent.SetActive(true);
            scoreParent.SetActive(false);

            Animator animator = GetComponent<Animator>();

            if (animator)
            {
                animator.Play("GameOverShow");
            }
            
            Profiler.BeginSample("GSLSDK.Events");
            var levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            var levelLostDictionary = new Dictionary<string, PrimitiveTypeUnion> { { "level_id", new IntType(2) } };
            Profiler.EndSample();
            
        }

        public void ShowWin(int score, int starCount)
        {
            screenParent.SetActive(true);
            loseText.enabled = false;

            scoreText.text = score.ToString();
            scoreText.enabled = false;

            Animator animator = GetComponent<Animator>();

            if (animator)
            {
                animator.Play("GameOverShow");
            }

            Profiler.BeginSample("GSLSDK.Events");
            var levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            var levelWinEvent = new GslEvent("level_won");
            levelWinEvent.SetCustomProperty("level_id", new StringType( levelName));
            levelWinEvent.SetCustomProperty("score", new FloatType(score*4));
            levelWinEvent.SetCustomProperty("stars", new IntType(starCount));
            levelWinEvent.Track();
            Profiler.EndSample();
            
            Profiler.BeginSample("GSLSDK.Events");
            // new GslEvent("level_complete", new Dictionary<string, PrimitiveTypeUnion>() {{"first_win", new StringType("first_win")}}).Track();
            Profiler.EndSample();
            
            StartCoroutine(ShowWinCoroutine(starCount));
        }

        private IEnumerator ShowWinCoroutine(int starCount)
        {
            yield return new WaitForSeconds(0.5f);

            if (starCount < stars.Length)
            {
                for (int i = 0; i <= starCount; i++)
                {
                    stars[i].enabled = true;

                    if (i > 0)
                    {
                        stars[i - 1].enabled = false;
                    }

                    yield return new WaitForSeconds(0.5f);
                }
            }

            scoreText.enabled = true;
        }

        public void OnReplayClicked()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        public void OnDoneClicked()
        {
            Profiler.BeginSample("GSLSDK.Events");
            var levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            var levelEvent =  new GslEvent("level_done");
            levelEvent.SetCustomProperty("level_done", new IntType(1));
            Profiler.EndSample();
            UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelect");
        }

    }
}
