using UnityEngine;
using UnityEngine.SceneManagement;

public class UserInterface : MonoBehaviour
{
    [SerializeField] private GameObject instructionsPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) instructionsPanel.SetActive(!instructionsPanel.activeSelf);

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Scene _currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(_currentScene.name);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
                Application.Quit();
            }
    }

}
