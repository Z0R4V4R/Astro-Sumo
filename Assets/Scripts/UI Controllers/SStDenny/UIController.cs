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
            levelProgress = root.Q<ProgressBar>("levelProgress");
            gameProgress = root.Q<ProgressBar>("gameProgress");
            //Determines the levelTime for this Scene
            int scenesRemaining = SceneManager.sceneCountInBuildSettings - GlobalEvents.SceneIndex;
            levelTime = GlobalEvents.GameTime / scenesRemaining;
            //Sets the start values of the progress bar for this Scene
            levelProgress.highValue = levelTime;
            levelProgress.value = levelTime;

        }

        // Update is called once per frame
        void Update()
        {
            
        }   
        private void ScoreUpdate()
        {
            
        }
        private void ProgressUpdate()
        {
            
        }   
        private void TimeRemaining()
        {
            
        }   
        private void SwitchScenes()
        {
            
        }
    }
}

