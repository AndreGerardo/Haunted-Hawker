using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasMenuManager : MonoBehaviour
{
    [Header("MENU UI REFERENCE")]
    [SerializeField]
    private GameObject buttonPlay;



    public void ButtonPlay()
    {
        ObjectBounceAnim buttonAnim;
        if(buttonPlay.TryGetComponent(out buttonAnim))
        {
            buttonAnim.Play(() =>
            {
                LevelManager.instance.LoadLevel(SceneList.MainScene);
            });
        }
        else
        {
            LevelManager.instance.LoadLevel(SceneList.MainScene);
        }
        
    }
}
