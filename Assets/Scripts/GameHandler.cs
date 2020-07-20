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
    float time;
    GameState state;


    // Start is called before the first frame update
    void Start()
    {
        time = timerStart;
        state = GameState.Intro;
        GameAssets.i.dialogueBox.StartDialogue(GameAssets.i.introDialogue);
        GameAssets.i.foodParticles.Pause();
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
                    state = GameState.Running;
                    GameAssets.i.catAnimation.SetTrigger("Wake");
                }
                break;
            case GameState.Running:
                if (Input.anyKeyDown)
                {
                    state = GameState.Stopped;
                    GameAssets.i.catAnimation.ResetTrigger("Wake");
                    GameAssets.i.catAnimation.SetTrigger("Eat");
                }
                time -= Time.deltaTime;
                GameAssets.i.timerText.text = string.Format("{0,6:00.00}", time);
                if (GameAssets.i.foodParticles.isPaused && time < 0)
                {
                    GameAssets.i.foodParticles.Play();
                    GameAssets.i.bowlSprite.sprite = GameAssets.i.bowlFullTexture;
                }
                break;
            case GameState.Stopped:
                break;
        }
    }
}