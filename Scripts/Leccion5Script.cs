using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Leccion5Scrip : MonoBehaviour
{
    UIDocument menu;
    private Button btnAlfabetos;
    private Button btnKanjis;
    private Button btnJuegos;
    private Button btnLecciones;
    private Button btnClasificacion;
    private Button btnPerfil;
    
    

    private Button btntest;
    private Button btnsalir;

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
            
            btntest=root.Q<Button>("btntest");
            btnsalir=root.Q<Button>("bntsalir");
            
            

            // Asignar eventos a los botones usando RegisterCallback
            btnAlfabetos.RegisterCallback<ClickEvent>(ev => OnAlfabetosClick());
            btnKanjis.RegisterCallback<ClickEvent>(ev => OnKanjisClick());
            btnJuegos.RegisterCallback<ClickEvent>(ev => OnJuegosClick());
            btnLecciones.RegisterCallback<ClickEvent>(ev => OnLeccionesClick());
            btnClasificacion.RegisterCallback<ClickEvent>(ev => OnClasificacionClick());
            btnPerfil.RegisterCallback<ClickEvent>(ev => OnPerfilClick());
            
            btntest.RegisterCallback<ClickEvent>(ev => OnbtntestClick());
            btnsalir.RegisterCallback<ClickEvent>(ev => OnbtnsalirClick());
            
            
        
        
    }

    // Eventos de los botones
    void OnbtntestClick(){
        SceneManager.LoadScene("");
    }
    void OnbtnsalirClick(){
        SceneManager.LoadScene("Lecciones");
    }

    void Onbtncontenidoleccion1Click(){
        SceneManager.LoadScene("Lecciones 1");
    }

    void Onbtncontenidoleccion2Click(){
        SceneManager.LoadScene("Lecciones 2");
    }
    void Onbtncontenidoleccion3Click(){
        SceneManager.LoadScene("Lecciones 3");
    }
    void Onbtncontenidoleccion4Click(){
        SceneManager.LoadScene("Lecciones 4");
    }
    void Onbtncontenidoleccion5Click(){
        SceneManager.LoadScene("Lecciones 5");
    }
    void Onbtncontenidoleccion6Click(){
        SceneManager.LoadScene("Lecciones 6");
    }


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
