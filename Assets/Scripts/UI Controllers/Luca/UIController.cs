using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    // UI References
    private UIDocument uiDocument;
    private Label[] teamScoresUI;
    private ProgressBar levelProgress;
    private ProgressBar gameProgress;

    // Game Data
    private int[] teamScores = new int[4];
    private int levelTime = 90;
    private float currentTime;

    void Start()
    {
        // Get UIDocument
        uiDocument = GetComponent<UIDocument>();

        // Get root element
        VisualElement root = uiDocument.rootVisualElement;

        // Get UI Elements (Make sure names match your UI Builder)
        teamScoresUI = new Label[4];
        teamScoresUI[0] = root.Q<Label>("9thScore");
        teamScoresUI[1] = root.Q<Label>("10thScore");
        teamScoresUI[2] = root.Q<Label>("11thScore");
        teamScoresUI[3] = root.Q<Label>("12thScore");

        levelProgress = root.Q<ProgressBar>("LevelTimeRemaining");
        gameProgress = root.Q<ProgressBar>("GameTimeRemaining");

        currentTime = levelTime;

        ScoreUpdate();
        ProgressUpdate();
        InvokeRepeating("TimeRemaining", 0f, 1f);
    }

    void Update()
    {
        ScoreUpdate();
        ProgressUpdate();
    }

    void ScoreUpdate()
{
    for (int i = 0; i < teamScores.Length; i++)
    {
        string teamName = (9 + i) + "th Grade";

        teamScores[i] = GlobalEvents.TeamScores[i];

        teamScoresUI[i].text = teamName + ": " + teamScores[i];
    }
}
    void ProgressUpdate()
    {
    
        levelProgress.value = levelTime;

        // Example: total score progress
        int totalScore = 0;
        foreach (int score in teamScores)
        {
            totalScore += score;
        }

        gameProgress.value = totalScore;
    }

    void TimeRemaining()
    {
            levelTime-= 10;
            GlobalEvents.SendGameTime();
            SwitchScenes();
        
    }

void SwitchScenes()
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