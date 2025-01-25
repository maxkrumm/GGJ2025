using UnityEngine;
using UnityEngine.Events;

public class BubbleAudioPlayer : MonoBehaviour
{
    public AK.Wwise.Event bubblePlayEvent;
    public AK.Wwise.Event bubbleStopEvent;
    private AudioManager audioManager = AudioManager.Instance;

    BubbleType bubbleType;
    private Bubble bubble;

    private bool triggerEventOnNextBeat = false;

    void Start()
    {
        bubble = GetComponent<Bubble>();
        
        if (bubble.typeInt == 0)
        AkSoundEngine.SetSwitch("Music_Group", "Rythm", gameObject);

        else if (bubble.typeInt == 1)
        AkSoundEngine.SetSwitch("Music_Group", "Amb", gameObject);

        else if (bubble.typeInt == 2)
        AkSoundEngine.SetSwitch("Music_Group", "Arp", gameObject);

        if (bubble.size == 1)
        AkSoundEngine.SetSwitch("Music_Size", "Small", gameObject);

        else if (bubble.size == 2)
        AkSoundEngine.SetSwitch("Music_Size", "Mid", gameObject);

        else if (bubble.size == 3)
        AkSoundEngine.SetSwitch("Music_Size", "Big", gameObject);

        if (bubble.level == 1)
        AkSoundEngine.SetSwitch("Music_Level", "L1", gameObject);

        else if (bubble.level == 2)
        AkSoundEngine.SetSwitch("Music_Level", "L2", gameObject);

        else if (bubble.level == 3)
        AkSoundEngine.SetSwitch("Music_Level", "L3", gameObject);

        // Subscribe to the beat event
        AudioManager.Instance.OnGrid.AddListener(TriggerSoundOnBeat);

    }

    private void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        AudioManager.Instance.OnGrid.RemoveListener(TriggerSoundOnBeat);
        bubbleStopEvent.Post(gameObject);
    }

    public void QueueSound()
    {
        // Set flag to trigger the sound on the next beat
        triggerEventOnNextBeat = true;
        Debug.Log(triggerEventOnNextBeat);
    }

    private void TriggerSoundOnBeat()
    {
        //Debug.Log("QueueSound called, triggerEventOnNextBeat: " + triggerEventOnNextBeat);
        if (triggerEventOnNextBeat)
        {
            // Post the Wwise event
            bubblePlayEvent.Post(gameObject);
            Debug.Log("SOUND: " + bubble.Type + "  Size " + bubble.size.ToString() + "  Level " + bubble.level.ToString());
            // Reset the flag
            triggerEventOnNextBeat = false;
        }

    }
}
