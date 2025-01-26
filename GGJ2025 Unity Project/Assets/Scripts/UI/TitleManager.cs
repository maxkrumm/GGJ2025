using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.Space)){
            OnClicked();
        }
    }

    public void OnClicked()
    {
        SceneManager.LoadSceneAsync("GameScene");
    }
}
