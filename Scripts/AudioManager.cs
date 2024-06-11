using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSource;

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    
    public void CambiarVolumen(float volumen)
    {
        audioSource.volume = volumen;
        
        PlayerPrefs.SetFloat("volumenAudio", volumen);
    }
}
