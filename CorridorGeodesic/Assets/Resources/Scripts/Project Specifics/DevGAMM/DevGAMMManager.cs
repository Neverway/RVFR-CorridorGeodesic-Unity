//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DevGAMMManager: MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
	public static DevGAMMManager Instance;

    public SaveData saveData;

    //=-----------------=
    // Private Variables
    //=-----------------=
    private Vector3 mouseLastPos;
    private float mouseDelta;

    private float timer;
    private float playTimeTimer;

    private string directoryPath => $"{Application.dataPath}/saves/";
    private string saveName = "Feedback.json";
    private string fullSavePath => directoryPath + saveName;

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        mouseDelta = (Input.mousePosition - mouseLastPos).magnitude;

        timer += Time.deltaTime;

        if (Input.anyKey || mouseDelta > 0.1f)
            timer = 0;

        if (timer >= 120)
            ResetGame();

        mouseLastPos = Input.mousePosition;

        if (Input.GetKeyDown(KeyCode.G))
        {
            GameInstance.Instance.UI_ShowFeedbackMenu(true);
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    void ResetGame()
    {
        playTimeTimer = 0;
        SceneManager.LoadScene(1);
    }

    //=-----------------=
    // External Functions
    //=-----------------=
    public void SaveFeedback(int rating, string feedback)
    {
        string[] splitLines = feedback.Split('.');

        feedback = "";

        int currentLineLength = 0;

        for (int i = 0; i < splitLines.Length; i++)
        {
            currentLineLength += splitLines[i].Length;

            if(currentLineLength >= 60)
            {
                feedback += splitLines[i] + "\n";
                currentLineLength = 0;
            }  
            else
                feedback += splitLines[i];
        }

        saveData.playTimeDatas.Add(new PlayTimeData(rating, feedback, playTimeTimer));

        FileStream stream;

        if(!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        if (!File.Exists(fullSavePath))
            stream = File.Create(fullSavePath);
        else
            stream = File.OpenWrite(fullSavePath);

        using (StreamWriter writer = new StreamWriter(stream))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(writer, saveData);
        }
    }
}
[System.Serializable]
public class SaveData
{
    public List<PlayTimeData> playTimeDatas = new List<PlayTimeData>();
}
public struct PlayTimeData
{
    public int rating;
    public string feedback;
    public float timePlayed;

    public PlayTimeData(int rating, string feedback, float timePlayed)
    {
        this.rating = rating;
        this.feedback = feedback;
        this.timePlayed = timePlayed;
    }
}