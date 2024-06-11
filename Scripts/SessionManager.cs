using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public static SessionManager Instance { get; private set; }

    public string CurrentUser { get; private set; }
    public bool IsLoggedIn => !string.IsNullOrEmpty(CurrentUser);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LogIn(string username)
    {
        CurrentUser = username;
    }

    public void LogOut()
    {
        CurrentUser = null;
    }
}
