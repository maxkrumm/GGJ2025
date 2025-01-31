using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private Image discImage;
    [SerializeField] private float speed = 1.0f;

    public GameObject slider;

    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        discImage.rectTransform.Rotate(new Vector3(0, 0, 1) * speed);
    }

    public void OnClicked()
    {
        AkSoundEngine.PostEvent("Play_Play", gameObject);
        SceneManager.LoadSceneAsync("GameScene");
    }
}
