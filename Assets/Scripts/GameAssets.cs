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
}
