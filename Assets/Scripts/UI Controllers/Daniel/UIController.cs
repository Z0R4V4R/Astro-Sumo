using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


namespace Daniel
{
    public class UIController : MonoBehaviour
    {
        private UIDocument uiDocument;
        private Label[] teamScoresUI;
        private ProgressBar levelProgress;
        private ProgressBar gameProgress;
        private int[] teamScores = new int[4];

        private int levelTime = 90;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            InvokeRepeating("TimeRemaining", 0f, 1f);
            uiDocument = GetComponent<UIDocument>();
            var root = uiDocument.rootVisualElement;
            teamScoresUI = new Label[] { 
                root.Q<Label>("9thScore"),
                root.Q<Label>("10thScore"),
                root.Q<Label>("11thScore"),
                root.Q<Label>("12thScore"),
                };
            levelProgress = root.Q<ProgressBar>("LevelTimeRemaining");
            gameProgress =  root.Q<ProgressBar>("GameTimeRemaining");
            
            // TODO: Add the 10th, 11th, and 12th score queries here...
            //Determines the levelTime for this Scene
            int scenesRemaining = SceneManager.sceneCountInBuildSettings - GlobalEvents.SceneIndex;
            levelTime = GlobalEvents.GameTime / scenesRemaining;
            //Sets the start values of the progress bar for this Scene
            levelProgress.highValue = levelTime;
            levelProgress.value = levelTime;
        }

        // Update is called once per frame
        private void Update()
        {
        
            ScoreUpdate();
            ProgressUpdate();
        }

        private void ScoreUpdate()
        {
            for (int i = 0; i < teamScores.Length; i++)
            {
            // Create grade level string (9th, 10th, 11th, 12th)
            int gradeLevel = i + 9;
            string teamString = gradeLevel + "th Grade";

            // Assign Global Events score to teamScores
            teamScores[i] = GlobalEvents.TeamScores[i];

            // Update UI text with grade and score
            teamScoresUI[i].text = teamString + " " + teamScores[i];
            }           
        }
        private void ProgressUpdate()
        {
            
            levelProgress.value =  levelTime;
            gameProgress.value = GlobalEvents.GameTime;

        }
        private void TimeRemaining()
        {
            levelTime -= 10;
            GlobalEvents.SendGameTime();
            SwitchScenes();
            
        }
        private void SwitchScenes()
        {
            if (levelTime <= 0)
            {
                GlobalEvents.SendSceneIndex();
                if (GlobalEvents.SceneIndex <= SceneManager.sceneCountInBuildSettings)
                {
                    Debug.Log(GlobalEvents.SceneIndex);
                    SceneManager.LoadScene(GlobalEvents.SceneIndex);
                }
            }
        }
    }
}

