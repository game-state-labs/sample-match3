using System.Collections;
using GameStateLabs;
using GameStateLabs.EventProperties;
using UnityEngine;
using UnityEngine.UI;

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
            
            var levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            var levelLostEvent = new CustomEvent("level_lost", BaseEvent.EventTypes.PlayerAction);
            levelLostEvent.SetCustomProperty("level_id", levelName);
            levelLostEvent.Track();
            
            var gold = GSLEvents.PlayerInventory.GetItem("curr_gold");
            gold.UpdateValue(-10, gold.Value - 10);
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

            var levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            var levelWinEvent = new CustomEvent("level_won", BaseEvent.EventTypes.PlayerAction);
            levelWinEvent.SetCustomProperty("level_id", levelName);
            levelWinEvent.SetCustomProperty("score", score.ToString());
            levelWinEvent.SetCustomProperty("stars", starCount.ToString());
            levelWinEvent.Track();
            
            new AchievementEvent(IAchievementProps.AchievementEventActions.Complete, "first_win", "First win!").Track();

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
            var levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            new LevelEvent(ILevelProps.LevelEventActions.End, levelName).Track();
            var gold = GSLEvents.PlayerInventory.GetItem("curr_gold");
            gold.UpdateValue(10, gold.Value + 10);
            UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelect");
        }

    }
}
