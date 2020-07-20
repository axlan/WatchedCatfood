using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    enum GameState
    {
        Intro,
        Running,
        Stopped
    }

    public float timerStart;
    public float badTime;
    float time;
    bool fed;
    GameState state;


    // Start is called before the first frame update
    void Start()
    {
        time = timerStart;
        state = GameState.Intro;
        GameAssets.i.dialogueBox.StartDialogue(GameAssets.i.introDialogue);
        GameAssets.i.foodParticles.Stop();
        GameAssets.i.bowlSprite.sprite = GameAssets.i.bowlEmptyTexture;
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
                if (Input.anyKeyDown)
                {
                    state = GameState.Stopped;
                    GameAssets.i.catAnimation.ResetTrigger("Wake");
                    GameAssets.i.catAnimation.SetTrigger("Eat");
                    Dialogue endDialogue = (time < -badTime) ? GameAssets.i.badDialogueLate :
                        (time > badTime) ? GameAssets.i.badDialogueEarly :
                        GameAssets.i.goodDialogue;
                    GameAssets.i.dialogueBox.StartDialogue(endDialogue);
                }
                time -= Time.deltaTime;
                GameAssets.i.timerText.text = string.Format("{0,6:00.00}", time);
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
    }
}