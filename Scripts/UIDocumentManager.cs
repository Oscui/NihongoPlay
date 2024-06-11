using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIDocumentManager : MonoBehaviour
{
    public List<string> scenesToHideUIDocument;
    private UIDocument uiDocument;
    private bool isUIDocumentHidden;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument not found on this GameObject.");
            return;
        }

        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        CheckAndHideUIDocument(SceneManager.GetActiveScene().name);
    }

    void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    private void OnActiveSceneChanged(Scene previous, Scene current)
    {
        CheckAndHideUIDocument(current.name);
    }

    private void CheckAndHideUIDocument(string sceneName)
    {
        if (scenesToHideUIDocument.Contains(sceneName))
        {
            if (!isUIDocumentHidden)
            {
                uiDocument.rootVisualElement.style.display = DisplayStyle.None;
                isUIDocumentHidden = true;
            }
        }
        else
        {
            if (isUIDocumentHidden)
            {
                uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
                isUIDocumentHidden = false;
            }
        }
    }
}
