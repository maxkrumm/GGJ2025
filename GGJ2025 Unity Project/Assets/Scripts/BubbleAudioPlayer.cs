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

    private void OnEnable()
    {
        bubble = gameObject.GetComponent<Bubble>();

        // Subscribe to the beat event
        AudioManager.Instance.OnGrid.AddListener(TriggerSoundOnBeat);

        if (bubbleType == BubbleType.Rythm)
        AkSoundEngine.SetSwitch("Music_Group", "Rythm", gameObject);

        else if (bubbleType == BubbleType.Amb)
        AkSoundEngine.SetSwitch("Music_Group", "Amb", gameObject);

        else if (bubbleType == BubbleType.Arp)
        AkSoundEngine.SetSwitch("Music_Group", "Arp", gameObject);

        if (bubble.size == 1)
        AkSoundEngine.SetSwitch("Music_Size", "Small", gameObject);

        else if (bubble.size == 2)
        AkSoundEngine.SetSwitch("Music_Size", "Mid", gameObject);

        else if (bubble.size == 3)
        AkSoundEngine.SetSwitch("Music_Size", "Big", gameObject);

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
        Debug.Log("QueueSound called, triggerEventOnNextBeat: " + triggerEventOnNextBeat);
        if (triggerEventOnNextBeat)
        {
            // Post the Wwise event
            bubblePlayEvent.Post(gameObject);
            Debug.Log("SOUND!");
            // Reset the flag
            triggerEventOnNextBeat = false;
        }

    }
}
