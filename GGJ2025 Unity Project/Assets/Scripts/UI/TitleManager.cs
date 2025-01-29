using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private Image discImage;
    [SerializeField] private float speed = 1.0f;

    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        discImage.rectTransform.Rotate(new Vector3(0, 0, 1) * speed);
        if (Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.Space)){
            OnClicked();
        }
    }

    public void OnClicked()
    {
        SceneManager.LoadSceneAsync("GameScene");
    }
}
