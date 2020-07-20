using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameHandler : MonoBehaviour
{
    enum GameState
    {
        Intro,
        Running,
        Stopped
    }

    [Serializable]
    struct GameData
    {
        public float bestTime;
        public float bestThreeStreak;
        public GameData(float bestTime, float bestThreeStreak)
        {
            this.bestTime = bestTime;
            this.bestThreeStreak = bestThreeStreak;
        }
    }

    public float timerStart;
    public float badTime;
    float time;
    bool fed;
    GameState state;
    GameData data;
    Queue<float> currentStreak;
    string destination;

    
    // Start is called before the first frame update
    void Start()
    {
        destination = Application.persistentDataPath + "/save.dat";
        time = timerStart;
        state = GameState.Intro;
        data = new GameData(-1,-1);
        currentStreak = new Queue<float>();
        GameAssets.i.dialogueBox.StartDialogue(GameAssets.i.introDialogue);
        GameAssets.i.foodParticles.Stop();
        GameAssets.i.bowlSprite.sprite = GameAssets.i.bowlEmptyTexture;
        LoadFile();
        UpdateScoreText();
    }

    void SaveFile()
    {
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    void LoadFile()
    {
        if (!File.Exists(destination))
        {
            return;
        }
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.LogError("File not found");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        data = (GameData)bf.Deserialize(file);
        file.Close();

    }

    void UpdateScore() {
        float absTime = Math.Abs(time);
        if (data.bestTime < 0 || absTime < data.bestTime)
        {
            data.bestTime = absTime;
        }
        currentStreak.Enqueue(absTime);
        if (currentStreak.Count > 3)
        {
            currentStreak.Dequeue();
        }
        if (currentStreak.Count == 3)
        {
            float newStreakCheck = currentStreak.Sum() / 3.0f;
            if (data.bestThreeStreak < 0 || newStreakCheck < data.bestThreeStreak)
            {
                data.bestThreeStreak = newStreakCheck;
            }
        }
        SaveFile();
    }

    void UpdateScoreText()
    {
        string scoreString = "Best  :";
        if (data.bestTime > 0)
        {
            scoreString += string.Format("{0:0.000}", data.bestTime);
        }
        scoreString += "\nBest 3:";
        if (data.bestThreeStreak > 0)
        {
            scoreString += string.Format("{0:0.000}", data.bestThreeStreak);
        }
        GameAssets.i.scoreText.text = scoreString;
    }

    void NewTry()
    {
        state = GameState.Running;
        time = timerStart;
        GameAssets.i.foodParticles.Stop();
        fed = false;
        GameAssets.i.catAnimation.ResetTrigger("Eat");
        GameAssets.i.catAnimation.SetTrigger("Wake");
        GameAssets.i.bowlSprite.sprite = GameAssets.i.bowlEmptyTexture;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case GameState.Intro:
                if (!GameAssets.i.dialogueBox.IsOpen())
                {
                    NewTry();
                }
                break;
            case GameState.Running:
                time -= Time.deltaTime;
                GameAssets.i.timerText.text = string.Format("{0,6:00.00}", time);
                if (Input.anyKeyDown)
                {
                    state = GameState.Stopped;
                    GameAssets.i.catAnimation.ResetTrigger("Wake");
                    GameAssets.i.catAnimation.SetTrigger("Eat");
                    Dialogue endDialogue = (time < -badTime) ? GameAssets.i.badDialogueLate :
                        (time > badTime) ? GameAssets.i.badDialogueEarly :
                        GameAssets.i.goodDialogue;
                    GameAssets.i.dialogueBox.StartDialogue(endDialogue);
                    UpdateScore();
                    UpdateScoreText();
                }
                break;
            case GameState.Stopped:
                time -= Time.deltaTime;
                if (!GameAssets.i.dialogueBox.IsOpen())
                {
                    NewTry();
                }
                break;
        }
        if (!fed && time < 0)
        {
            GameAssets.i.foodParticles.time = 0;
            fed = true;
            GameAssets.i.foodParticles.Play();
            GameAssets.i.bowlSprite.sprite = GameAssets.i.bowlFullTexture;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}