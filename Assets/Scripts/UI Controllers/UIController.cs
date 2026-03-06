using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace LucaA
{
public class UIController : MonoBehaviour
{
    // Fields
    private UIDocument uiDocument;
    private Label[] teamScoresUI;
    private ProgressBar levelProgress;
    private ProgressBar gameProgress;
    private int[] teamScores = new int[4];
    private int levelTime = 90;

    // Start Method
    void Start()
    {
        // Access UI Document
        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // Query Score Labels
        teamScoresUI = new Label[]
        {
            root.Q<Label>("9thScore"),
            root.Q<Label>("10thScore"),
            root.Q<Label>("11thScore"),
            root.Q<Label>("12thScore")
        };

        // Connect Progress Bars
        levelProgress = root.Q<ProgressBar>("LevelProgress");
        gameProgress = root.Q<ProgressBar>("GameProgress");

        // Determine level time
        int scenesRemaining = SceneManager.sceneCountInBuildSettings - GlobalEvents.SceneIndex;
        levelTime = GlobalEvents.GameTime / scenesRemaining;

        // Set progress bar start values
        levelProgress.highValue = levelTime;
        levelProgress.value = levelTime;

        // Schedule countdown
        InvokeRepeating("TimeRemaining", 0f, 1f);
    }

    // Update Method
    void Update()
    {
        ScoreUpdate();
        ProgressUpdate();
    }

    // ScoreUpdate Method
    void ScoreUpdate()
    {
        Debug.Log($"ScoreUpdate {teamScores.Length}");
        for (int i = 0; i < teamScores.Length; i++)
        {
            string teamName = (9 + i) + "th Grade";

            teamScores[i] = GlobalEvents.TeamScores[i];

            teamScoresUI[i].text = teamName + ": " + teamScores[i];
        }
    }

    // ProgressUpdate Method
    void ProgressUpdate()
    {
        levelProgress.value = levelTime;
        gameProgress.value = GlobalEvents.GameTime;
    }

    // TimeRemaining Method
    void TimeRemaining()
    {
        levelTime--;
        GlobalEvents.SendGameTime();
        SwitchScenes();
    }

    // SwitchScenes Method
    void SwitchScenes()
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