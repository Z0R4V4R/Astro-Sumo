using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace ZNijjar.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        private Label[] teamScoresUI;
        private ProgressBar levelProgress;
        private ProgressBar gameProgress;
        private int[] teamScores = new int[4];
        private int levelTime;

        private void Start()
        {
            if (uiDocument == null)
                uiDocument = GetComponent<UIDocument>();

            VisualElement root = uiDocument.rootVisualElement;

            // Task 2: Query Score Labels
            teamScoresUI = new Label[]
            {
                root.Q<Label>("9thScore"),
                root.Q<Label>("10thScore"),
                root.Q<Label>("11thScore"),
                root.Q<Label>("12thScore")
            };

            // Task 3: Connect Progress Bars
            levelProgress = root.Q<ProgressBar>("levelProgress");
            gameProgress = root.Q<ProgressBar>("gameProgress");

            // Task 4: Set Level Clock
            int scenesRemaining = SceneManager.sceneCountInBuildSettings - GlobalEvents.SceneIndex;
            levelTime = GlobalEvents.GameTime / scenesRemaining;
            levelProgress.highValue = levelTime;
            levelProgress.value = levelTime;

            // Task 5: Schedule Game Clock
            InvokeRepeating(nameof(TimeRemaining), 0f, 1f);
        }

        private void Update()
        {
            ScoreUpdate();
            ProgressUpdate();
        }

        private void ScoreUpdate()
        {
            for (int i = 0; i < teamScores.Length; i++)
            {
                teamScores[i] = GlobalEvents.TeamScores[i];
                teamScoresUI[i].text = teamScores[i].ToString();
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
                    CancelInvoke(nameof(TimeRemaining));
                }
            }
        }
    }
}