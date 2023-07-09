using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MusicManager: MonoBehaviour
{   
    public FMODUnity.EventReference music;
    FMOD.Studio.EventInstance musicState;






    


    private void Start() {
        musicState = FMODUnity.RuntimeManager.CreateInstance(music);
        musicState.start();

    }

    private void Update() {
       
        // musicState.setParameterByName("music_Layer1", firstLayer);
        // musicState.setParameterByName("music_Layer2", secondLayer);
        // musicState.setParameterByName("music_Layer3", thirdLayer);    
    } 

    public void StopMusic(){
        musicState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    
    private void OnDestroy() {
        musicState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        // musicState.setParameterByName("music_Layer1", 0f);
        // musicState.setParameterByName("music_Layer2", 0f);
        // musicState.setParameterByName("music_Layer3", 0f);

    }

}
