using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ControBotonesTatamiTiles : MonoBehaviour
{
    public float tiempoLimite;
    private float tiempoRestante;
    public TextMeshProUGUI tiempoRestanteText;
    public TextMeshProUGUI puntuacionText;

    private int puntuacion = 0;

    void Start()
    {
        tiempoRestante = tiempoLimite;
        puntuacion = PlayerPrefs.GetInt("Puntuacion", 0);
        puntuacionText.text = "Puntuación: " + puntuacion.ToString();
    }

    void Update()
    {
        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            if (tiempoRestante <= 0)
            {
                SceneManager.LoadScene("Tatami TilesEscenaDerrota");
            }
        }
        tiempoRestanteText.text = "Tiempo Restante: " + Mathf.Round(tiempoRestante).ToString() + " s";
    }

    public void OnBotonMenuApp()
    {
        SceneManager.LoadScene("Alfabetos");
    }

    public void OnBotonNiveles()
    {
        SceneManager.LoadScene("Tatami TilesNiveles");
    }

    public void OnBotonOpciones()
    {
        SceneManager.LoadScene("Tatami TilesOpciones");
    }

    public void OnVolverMenu()
    {
        SceneManager.LoadScene("Tatami TilesMenu");
    }

    public void OnBotonNivel1()
    {
        SceneManager.LoadScene("Tatami TilesNivel1");
    }

    public void OnBotonNivel2()
    {
        SceneManager.LoadScene("Tatami TilesNivel2");
    }

    public void OnBotonNivel3()
    {
        SceneManager.LoadScene("Tatami TilesNivel3");
    }

    public void OnBotonNivel4()
    {
        SceneManager.LoadScene("Tatami TilesNivel4");
    }

    public void OnBotonNivel5()
    {
        SceneManager.LoadScene("Tatami TilesNivel5");
    }

    public void OnBotonNivel6()
    {
        SceneManager.LoadScene("Tatami TilesNivel6");
    }

    // Método para cargar la escena de victoria con la puntuación
    public void CargarEscenaVictoria()
    {
        SceneManager.LoadScene("Tatami TilesEscenaVictoria");
    }

    // Método para establecer la puntuación actual
    public void EstablecerPuntuacion(int puntuacion)
    {
        this.puntuacion = puntuacion;
    }

    // Método para obtener la puntuación actual
    public int ObtenerPuntuacion()
    {
        return puntuacion;
    }
}
