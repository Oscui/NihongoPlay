using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class KanjisScript : MonoBehaviour
{
     UIDocument menu;
    private Button btnAlfabetos;
    private Button btnKanjis;
    private Button btnJuegos;
    private Button btnLecciones;
    private Button btnClasificacion;
    private Button btnPerfil;
    

    // Método llamado cuando el script se habilita
    void OnEnable()
    {
        menu=GetComponent<UIDocument>();
        VisualElement root=menu.rootVisualElement;
    

        // Obtener el componente UIDocument
        var uiDocument = GetComponent<UIDocument>();
        
        
            // Obtener referencias a los botones
            btnAlfabetos = root.Q<Button>("btnalfabetos");
            btnKanjis = root.Q<Button>("btnkanjis");
            btnJuegos = root.Q<Button>("btnjuegos");
            btnLecciones = root.Q<Button>("btnlecciones");
            btnClasificacion = root.Q<Button>("btnclasificacion");
            btnPerfil = root.Q<Button>("btnperfil");
            
            

            // Asignar eventos a los botones usando RegisterCallback
            btnAlfabetos.RegisterCallback<ClickEvent>(ev => OnAlfabetosClick());
            btnKanjis.RegisterCallback<ClickEvent>(ev => OnKanjisClick());
            btnJuegos.RegisterCallback<ClickEvent>(ev => OnJuegosClick());
            btnLecciones.RegisterCallback<ClickEvent>(ev => OnLeccionesClick());
            btnClasificacion.RegisterCallback<ClickEvent>(ev => OnClasificacionClick());
            btnPerfil.RegisterCallback<ClickEvent>(ev => OnPerfilClick());
            
        
        
    }

    // Eventos de los botones
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

}
