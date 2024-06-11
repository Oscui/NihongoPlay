using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class AlfabetosKatakanaScript : MonoBehaviour
{
    UIDocument menu;
    private Button btnAlfabetos;
    private Button btnKanjis;
    private Button btnJuegos;
    private Button btnLecciones;
    private Button btnClasificacion;
    private Button btnPerfil;

    private Button btnhiragana;
    private Button btnkatakana;

    public AudioSource audioSource;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        VisualElement root = menu.rootVisualElement;

        audioSource = GetComponent<AudioSource>();

        // Obtener referencias a los botones
        btnAlfabetos = root.Q<Button>("btnalfabetos");
        btnKanjis = root.Q<Button>("btnkanjis");
        btnJuegos = root.Q<Button>("btnjuegos");
        btnLecciones = root.Q<Button>("btnlecciones");
        btnClasificacion = root.Q<Button>("btnclasificacion");
        btnPerfil = root.Q<Button>("btnperfil");
        btnhiragana = root.Q<Button>("btnhiragana");
        btnkatakana = root.Q<Button>("btnkatakana");

        // Asignar eventos a los botones usando RegisterCallback
        btnAlfabetos.RegisterCallback<ClickEvent>(ev => OnAlfabetosClick());
        btnKanjis.RegisterCallback<ClickEvent>(ev => OnKanjisClick());
        btnJuegos.RegisterCallback<ClickEvent>(ev => OnJuegosClick());
        btnLecciones.RegisterCallback<ClickEvent>(ev => OnLeccionesClick());
        btnClasificacion.RegisterCallback<ClickEvent>(ev => OnClasificacionClick());
        btnPerfil.RegisterCallback<ClickEvent>(ev => OnPerfilClick());
        btnhiragana.RegisterCallback<ClickEvent>(ev => OnHiraganaClick());
        btnkatakana.RegisterCallback<ClickEvent>(ev => OnKatakanaClick());

        // Obtener todos los botones del alfabeto
        List<Button> buttons = root.Query<Button>().Where(btn => btn.name.StartsWith("btn") && char.IsLetter(btn.name[3])).ToList();

        // Asignar eventos a los botones del alfabeto
        foreach (Button button in buttons)
        {
            button.RegisterCallback<ClickEvent>(ev => OnAlphabetButtonClick(button.name));
        }
    }

    // Eventos de los botones del menú
    void OnAlfabetosClick()
    {
        Debug.Log("Botón Alfabetos pulsado");
        SceneManager.LoadScene("Alfabetos");
    }

    void OnKanjisClick()
    {
        Debug.Log("Botón Kanjis pulsado");
        SceneManager.LoadScene("Kanjis");
    }

    void OnJuegosClick()
    {
        Debug.Log("Botón Juegos pulsado");
        SceneManager.LoadScene("Juegos");
    }

    void OnLeccionesClick()
    {
        Debug.Log("Botón Lecciones pulsado");
        SceneManager.LoadScene("Lecciones");
    }

    void OnClasificacionClick()
    {
        Debug.Log("Botón Clasificación pulsado");
        SceneManager.LoadScene("Clasificacion");
    }

    void OnPerfilClick()
    {
        Debug.Log("Botón Perfil pulsado");
        SceneManager.LoadScene("MiCuenta");
    }

    // Eventos de los botones del alfabeto
    void OnAlphabetButtonClick(string buttonName)
    {
        Debug.Log("Botón " + buttonName + " pulsado");
        ReproducirAudio("AlfabetosSFX/kanasound-" + buttonName.Substring(3).ToLower());
    }

    void ReproducirAudio(string path)
    {
        // Cargar el archivo de audio desde la ruta especificada
        AudioClip clip = Resources.Load<AudioClip>(path);

        if (clip != null)
        {
            // Asignar el clip al AudioSource
            audioSource.clip = clip;
            // Reproducir el audio
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No se pudo cargar el archivo de audio desde: " + path);
        }
    }
    void OnHiraganaClick()
    {
        Debug.Log("Botón Hiragana pulsado");
        SceneManager.LoadScene("Alfabetos");
    }

    void OnKatakanaClick()
    {
        Debug.Log("Botón Katakana pulsado");
        SceneManager.LoadScene("AlfabetosKatakana");
    }

}
