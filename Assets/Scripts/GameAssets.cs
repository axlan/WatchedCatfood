/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public class GameAssets : MonoBehaviour
{

    public static GameAssets i;

    void Start() {
        i = this;
    }

    public Text timerText;
    public Animator catAnimation;
    public ParticleSystem foodParticles;
    public Dialogue introDialogue;
    public Dialogue goodDialogue;
    public Dialogue badDialogueEarly;
    public Dialogue badDialogueLate;
    public DialogueManager dialogueBox;
    public SpriteRenderer bowlSprite;
    public Sprite bowlEmptyTexture;
    public Sprite bowlFullTexture;
}
