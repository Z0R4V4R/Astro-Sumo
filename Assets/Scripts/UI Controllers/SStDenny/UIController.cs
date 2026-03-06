using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace SStDenny
{
    public class UIController : MonoBehaviour
    {
        private UIDocument uiDocument;
        private Label[] teamScoresUI;
        private ProgressBar levelProgress, gameProgress;
        private int[] teamScores  = new int[4];
        private int levelTime;
 
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            uiDocument = GetComponent<UIDocument>();
            var root = uiDocument.rootVisualElement;

            teamScoresUI = new Label[]
            {
                root.Q<Label>("9thScore"),root.Q<Label>("10thScore"),root.Q<Label>("11thScore"),root.Q<Label>("12thScore")
            };
            levelProgress = root.Q<ProgressBar>("LevelTimeRemaining");
            gameProgress = root.Q<ProgressBar>("GameTimeRemaining");
            //Determines the levelTime for this Scene
            int scenesRemaining = SceneManager.sceneCountInBuildSettings - GlobalEvents.SceneIndex;
            levelTime = GlobalEvents.GameTime / scenesRemaining;
            //Sets the start values of the progress bar for this Scene
            levelProgress.highValue = levelTime;
            levelProgress.value = levelTime;
            InvokeRepeating("TimeRemaining", 0, 1f);
        }

        // Update is called once per frame
        void Update()
        {
            ScoreUpdate();
            ProgressUpdate();
        }   
         private void ScoreUpdate()
        {
            for (int i = 0; i < teamScores.Length; i++)
            {
                string teamName = (9 + i) + "th Grade";
                teamScores[i] = GlobalEvents.TeamScores[i];
                teamScoresUI[i].text = teamName + ": " + teamScores[i];
            }
        }

        private void ProgressUpdate()
        {
            levelProgress.value = levelTime;
            gameProgress.value = GlobalEvents.GameTime;
        }

        private void TimeRemaining()
        {
            levelTime--;
            GlobalEvents.SendGameTime();
            SwitchScenes();
        }

        private void SwitchScenes()
        {
            if (levelTime == 0)
            {
                GlobalEvents.SendSceneIndex();

                if (GlobalEvents.SceneIndex < SceneManager.sceneCountInBuildSettings)
                {
                    SceneManager.LoadScene(GlobalEvents.SceneIndex);
                }
                else if (GlobalEvents.GameTime <= 0)
                {
                    Time.timeScale = 0;
                    CancelInvoke("TimeRemaining");
                }
            }
        }
    }
}


