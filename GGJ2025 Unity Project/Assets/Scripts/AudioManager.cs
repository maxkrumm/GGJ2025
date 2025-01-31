using UnityEngine;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance{get; private set;}

    public AK.Wwise.Event musicSyncEvent;

    public UnityEvent OnGrid;
    public UnityEvent OnBeat;
    public UnityEvent OnBar;

        private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }
    
    void Start()
    {
        StartMusic();
        AkSoundEngine.PostEvent("Play_Vinyl", gameObject);
    }

    private void StartMusic()
    {
        musicSyncEvent.Post(gameObject, (uint)AkCallbackType.AK_MusicSyncAll, MusicCallbackFunction);
    }

    void MusicCallbackFunction(object in_cookie, AkCallbackType in_type, AkCallbackInfo in_info)
    {
        AkMusicSyncCallbackInfo musicInfo;
        
        if (in_info is AkMusicSyncCallbackInfo)
        {
            musicInfo = (AkMusicSyncCallbackInfo)in_info;

            switch (musicInfo.musicSyncType)
            {
                case AkCallbackType.AK_MusicSyncBeat:
                    OnBeat.Invoke();
                    break;

                case AkCallbackType.AK_MusicSyncBar:
                    OnBar.Invoke();
                    break;

                case AkCallbackType.AK_MusicSyncGrid:
                    OnGrid.Invoke();
                    break;

                default:
                    break;
            }
        }
    }

    void OnDestroy()
    {
       
    }

    public void OnButtonPlay()
    {
        AkSoundEngine.PostEvent("Play_Play", gameObject);
    }

    public void OnButtonHome()
    {
        AkSoundEngine.PostEvent("Play_Home", gameObject);
    }

    
}