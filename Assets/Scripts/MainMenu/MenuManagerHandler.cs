using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Audio;
using UnityEngine.Rendering;

enum audioType
{
    SFX,
    BGM
}
public class audioManager
{
    public string AudioName;
    public AudioClip clip;
    [SerializeField] audioType type;
}
public class MenuManagerHandler : MonoBehaviour
{
    [SerializeField] public audioManager[] audioLibrary;
    public AudioMixer audioMixer;
    
    public void SetBGMVolume(float Volume){
        audioMixer.SetFloat("BGMVolume", Volume);
    }

    public void SetVFXVolume(float Volume){
        audioMixer.SetFloat("VFXVolume", Volume);
    }

    void Update()
    {
        
    }

    protected void playSound()
    {

    }
    public void PlayGame(){
        AudioController.Instance.PlayBGM("InGameBattle");

        SceneManager.LoadSceneAsync(2);
    }
    public void ExitGame(){
        Application.Quit();
    }
}

