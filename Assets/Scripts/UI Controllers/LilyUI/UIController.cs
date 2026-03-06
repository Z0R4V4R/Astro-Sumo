using UnityEngine;

namespace LLesser
{

    public class UIController : MonoBehaviour

    private UIDocument uIDocument;
    Label[] teamScoresUI;
   private ProgressBar gameProgress;
   private int[] teanScore = new int[4];
   private interface levelTime;

    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            uiDocument = GetComponent<UIDocument>();
            var root = uiDocument.rootvisualElement;

            teamScoresUI = new Label[]
            {
                root.Q<Label>("(9thScore)"),
                root.Q<Label>("10thScore"),
                root.Q<Label>("11thScore"),
                root.Q<Label>("12thScore"),
            };
            
            levelProgress = root.Q<ProgressBar>("LevelProgress");
            gameProgress = root.Q<ProgressBar>("GameProgress");

            //Determines the levelTime for this Scene
            int scenesRemaining = SceneManager.sceneCountInBuildSettings - GlobalEvents.SceneIndex;
            levelTime = GlobalEvents.GameTime / scenesRemaining;
            //Sets the start values of the progress bar for this Scene
            levelProgress.highValue = levelTime;
            levelProgress.value = levelTime;

            InvokeRepeating(nameof(TimeRemaining), 0f, 1f);





        }

        // Update is called once per frame
        void Update()
        {
            ScoreUpdate();
            ProgressUpdate();
        }

    
        void ProgressUpdate()
        {
            levelProgress.value = currentLevelTime;
            gameProgress.value = GlobalEvents.currentGameTime;

        }

        void ScoreUpdate()
        {
            for (int i = 0; i <teamScores.length; ++)
            {
                
            string teamGrade = (i + 9) + "th Grade";
            teamScores[i] = GlobalEvents.teamScores[i];
            teamScoresUI[i].text = teamGrade + ": " + teamScores[i];
            } 
        }

        void TimeRemaining()
        {
           
           levelTime--;
           GlobalEvents.SendGameTime();
           SwitchScenes();
        }

        void switchScenes()
        {
             if (levelTime == 0)
             {
        
             GlobalEvents.SendSceneIndex();

        
            if (GlobalEvents.SceneIndex < SceneManager.sceneCountInBuildSettings)
         {
            
            SceneManager.LoadScene(GlobalEvents.SceneIndex);
         }
            else if (gameTime <= 0)
        {
            // Stop the game
            Time.timeScale = 0;

            // Stop calling the TimeRemaining method
            CancelInvoke("TimeRemaining");
        }
        }
    }
}
