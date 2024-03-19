using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void playSound()
    {

    }
}
