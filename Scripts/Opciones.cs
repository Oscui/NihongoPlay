using UnityEngine;
using UnityEngine.UI;

public class Opciones : MonoBehaviour
{
    public Slider slider;
    public AudioSource audioSource;

    
    void Start()
    {
        
        float volumenGuardado = PlayerPrefs.GetFloat("volumenAudio", 0.5f);

        
        audioSource.volume = volumenGuardado;

        
        //slider.value = volumenGuardado;
    }

    
    public void CambiarVolumen()
    {
        audioSource.volume = slider.value;
        PlayerPrefs.SetFloat("volumenAudio", slider.value);
    }

    
    public void SubirVolumen()
    {
        if (slider.value < 1.0f)
        {
            slider.value += 0.1f; 
            CambiarVolumen(); 
        }
    }

    // Método para bajar el volumen
    public void BajarVolumen()
    {
        if (slider.value > 0.0f)
        {
            slider.value -= 0.1f; 
            CambiarVolumen(); 
        }
    }
}
