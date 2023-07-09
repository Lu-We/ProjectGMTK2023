using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    //internal PlayerScript player;

    public FMODUnity.EventReference KnightAtkSound;
    public FMODUnity.EventReference MageIceAtkSound;
    public FMODUnity.EventReference MageFireAtkSound;

    public FMODUnity.EventReference ArcherAtkSound;
    
    public FMODUnity.EventReference ClickSound;
    public FMODUnity.EventReference OverSound;
    public FMODUnity.EventReference HealSound;
    
    public FMODUnity.EventReference WinSound;
    // public FMODUnity.EventReference AttackUp;
    // public FMODUnity.EventReference AttackMid;
    // public FMODUnity.EventReference AttackDown;

    public float stepSoundPeriod = 0.2f;
    public float stepSoundTimer = 0f;

    void Start(){
    }
    // Update is called once per frame
    void Update()
    {
    }


    public void playHealSFX(){
         FMODUnity.RuntimeManager.PlayOneShot(HealSound,transform.position);
    }

    public void PlayKnightAtkSFX(){
        FMODUnity.RuntimeManager.PlayOneShot(KnightAtkSound,transform.position);
    }

    public void PlayMageSFX(){
        FMODUnity.RuntimeManager.PlayOneShot(MageIceAtkSound, transform.position);
    }

    public void PlayArcherAtkSFX(){
        FMODUnity.RuntimeManager.PlayOneShot(ArcherAtkSound, transform.position);
    }

    public void PlayClickSFX(){
        FMODUnity.RuntimeManager.PlayOneShot(ClickSound, transform.position);
    }

    public void PlayOverSFX(){
        FMODUnity.RuntimeManager.PlayOneShot(OverSound, transform.position);
    }

    public void PlayWinSFX(){
        FMODUnity.RuntimeManager.PlayOneShot(WinSound, transform.position);
    }


}
