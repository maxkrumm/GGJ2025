using UnityEngine;
using System.Collections.Generic;

public class InstantiateOnClick : MonoBehaviour
{
    // Prefab to be instantiated
    [SerializeField] private GameObject prefab;

    // List to store all instantiated prefabs
    private List<GameObject> instantiatedPrefabs = new List<GameObject>();

    void Update()
    {
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0)) // 0 stands for the left mouse button
        {
            GameObject newPrefab = InstantiatePrefab();
            if (newPrefab != null)
            {
                BubbleAudioPlayer bubbleAudioPlayer = newPrefab.GetComponent<BubbleAudioPlayer>();
                bubbleAudioPlayer.QueueSound();
            }
        }

        // Check if the right mouse button is clicked
        if (Input.GetMouseButtonDown(1)) // 1 stands for the right mouse button
        {
            DestroyInstance();
        }
    }

    GameObject InstantiatePrefab()
    {
        if (prefab != null)
        {
            // Convert mouse position to world coordinates
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.nearClipPlane; // Set distance from the camera (z-value)
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            // Instantiate the prefab at the calculated world position
            GameObject prefabInstance = Instantiate(prefab, worldPosition, Quaternion.identity);

            // Add the instantiated prefab to the list
            instantiatedPrefabs.Add(prefabInstance);

            return prefabInstance;
        }
        else
        {
            Debug.LogWarning("Prefab is not assigned.");
            return null;
        }
    }

    private void DestroyInstance()
    {
        // Perform a raycast to check if the mouse is over any instantiated object (using Physics2D)
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null)
        {
            // Loop through all instantiated prefabs and check if the raycast hits any of them
            foreach (GameObject prefabInstance in instantiatedPrefabs)
            {
                if (hit.transform.gameObject == prefabInstance)
                {
                    // Destroy the prefab that was clicked on
                    instantiatedPrefabs.Remove(prefabInstance);  // Remove from the list first
                    Destroy(prefabInstance);  // Destroy the prefab
                    Debug.Log("Prefab destroyed as mouse is over it!");
                    break; // Exit the loop once the prefab is destroyed
                }
            }
        }
    }
}
