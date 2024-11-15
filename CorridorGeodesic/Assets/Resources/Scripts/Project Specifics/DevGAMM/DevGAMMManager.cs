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

    private string directoryPath => $"{Application.persistentDataPath}/saves/";
    private string saveName = "Feedback.json";

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
        if (SceneManager.GetActiveScene().buildIndex == 1)
            return;

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
        SceneManager.LoadScene(1);
        //WorldLoader.Instance.LoadWorld("_Title");
    }

    //=-----------------=
    // External Functions
    //=-----------------=
    public void SaveFeedback(int rating, string feedback)
    {
        saveData.playTimeDatas.Add(new PlayTimeData(rating, feedback, 
            (float)Stopwatch.Instance.timeElapsed));

        string fullPath = Path.Combine(directoryPath, saveName);

        if(!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        using (FileStream stream = new FileStream(fullPath, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                string data = JsonUtility.ToJson(saveData, true);

                writer.Write(data);

                writer.Close();
            }
            stream.Close();
        }
    }
}
[System.Serializable]
public class SaveData
{
    public List<PlayTimeData> playTimeDatas = new List<PlayTimeData>();
}
[System.Serializable]
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