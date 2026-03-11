using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace LLesser
{
    public class UIController : MonoBehaviour
    {
        private UIDocument uiDocument;
         Label[] teamScoresUI;
        private ProgressBar levelProgress;
        private ProgressBar gameProgress;
        private int[] teamScores = new int[4];
        private int levelTime;

    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            uiDocument = GetComponent<UIDocument>();
            var root = uiDocument.rootVisualElement;

            teamScoresUI = new Label[]
            {
                root.Q<Label>("(9thScore)"),
                root.Q<Label>("10thScore"),
                root.Q<Label>("11thScore"),
                root.Q<Label>("12thScore"),
            };
            
            levelProgress = root.Q<ProgressBar>("LevelTimeRemaining");
            gameProgress = root.Q<ProgressBar>("GameTimeRemaining");

            //Determines the levelTime for this Scene
            int scenesRemaining = SceneManager.sceneCountInBuildSettings - GlobalEvents.SceneIndex;
            levelTime = GlobalEvents.GameTime / scenesRemaining;
            //Sets the start values of the progress bar for this Scene
            levelProgress.highValue = levelTime;
            levelProgress.value = levelTime;

            InvokeRepeating("TimeRemaining", 0f, 1f);

        }

        // Update is called once per frame
        void Update()
        {
            ScoreUpdate();
            ProgressUpdate();
        }

    
        void ProgressUpdate()
        {
            levelProgress.value = levelTime;
            gameProgress.value = GlobalEvents.GameTime;

        }

        void ScoreUpdate()
        {
            for (int i = 0; i <GlobalEvents.TeamScores.Length; i++)
            {
                
                string teamGrade = (i + 9) + "th Grade";
                teamScores[i] = GlobalEvents.TeamScores[i];
                teamScoresUI[i].text = teamGrade + ": " + GlobalEvents.TeamScores[i];
            } 
        }

        void TimeRemaining()
        {
        
            levelTime--;
            GlobalEvents.SendGameTime();
            SwitchScenes();
        }

        void SwitchScenes()
        {
            if (levelTime == 0)
            {
        
                GlobalEvents.SendSceneIndex();

            }

            if (GlobalEvents.SceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                
                SceneManager.LoadScene(GlobalEvents.SceneIndex);
            }
            else if (GlobalEvents.GameTime <= 0)
            
            {
                // Stop the game
                Time.timeScale = 0;

                // Stop calling the TimeRemaining method
                CancelInvoke("TimeRemaining");
            }
        }
    }
}





    


    

    

    