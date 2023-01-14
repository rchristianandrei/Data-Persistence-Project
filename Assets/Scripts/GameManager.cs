using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private TMP_InputField inputField;

    [HideInInspector] public string PlayerName = "Anon";
    [HideInInspector] public string BestScorer;
    [HideInInspector] public int BestScore = 0;

    [SerializeField] private string SaveFileName = "/SaveFile.json";
    private string SaveFilePath;


    private void Awake()
    {
        if(Instance != null) { return; }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SaveFilePath = Application.persistentDataPath + SaveFileName;
        LoadGame();
    }

    private void Start()
    {
        FindStartButton();
        FindExitButton();
    }

    public void LoadMainScene()
    {
        string temp = inputField.text;

        if (!string.IsNullOrEmpty(temp))
            PlayerName = temp;
        else
            PlayerName = "Anon";

        SceneLoader.Instance.LoadScene("main");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void FindStartButton()
    {
        GameObject startButton = GameObject.FindGameObjectWithTag("StartButton");
        if (startButton == null) { return; }

        startButton.GetComponent<Button>().onClick.AddListener(LoadMainScene);
    }

    public void FindExitButton()
    {
        GameObject exitBUtton = GameObject.FindGameObjectWithTag("ExitButton");
        if (exitBUtton == null) { return; }

        exitBUtton.GetComponent<Button>().onClick.AddListener(ExitGame);
    }

    class SaveData
    {
        public string Name;
        public int Score;

        public SaveData(string name, int score)
        {
            Name = name;
            Score = score;
        }
    }

    public void SaveGame(int score)
    {
        // If didn't pass the best score
        if (BestScore >= score) { return; }

        SaveData data = new SaveData(PlayerName, score);

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(SaveFilePath, json);
    }

    public void LoadGame()
    {
        if (!File.Exists(SaveFilePath)) { return; }

        string json = File.ReadAllText(SaveFilePath);

        SaveData data = JsonUtility.FromJson<SaveData>(json);

        BestScorer = data.Name;
        BestScore = data.Score;
    }
}
