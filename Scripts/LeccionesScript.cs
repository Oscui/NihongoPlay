using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class LeccionesScript : MonoBehaviour
{
    UIDocument menu;
    private Button btnAlfabetos;
    private Button btnKanjis;
    private Button btnJuegos;
    private Button btnLecciones;
    private Button btnClasificacion;
    private Button btnPerfil;

    private Button btncontenidoleccion1;
    private Button btntestleccion1;

    private Button btncontenidoleccion2;
    private Button btntestleccion2;

    private Button btncontenidoleccion3;
    private Button btntestleccion3;

    private Button btncontenidoleccion4;
    private Button btntestleccion4;

    private Button btncontenidoleccion5;
    private Button btntestleccion5;

    private Button btncontenidoleccion6;
    private Button btntestleccion6;

    private Button btntest;
    private Button btnsalir;


    private VisualElement leccion1;
    public VisualElement leccion2;
    public VisualElement leccion3;
    public VisualElement leccion4;
    public VisualElement leccion5;
    public VisualElement leccion6;
    private int leccionActual;

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        VisualElement root = menu.rootVisualElement;

        btnAlfabetos = root.Q<Button>("btnalfabetos");
        btnKanjis = root.Q<Button>("btnkanjis");
        btnJuegos = root.Q<Button>("btnjuegos");
        btnLecciones = root.Q<Button>("btnlecciones");
        btnClasificacion = root.Q<Button>("btnclasificacion");
        btnPerfil = root.Q<Button>("btnperfil");
        btncontenidoleccion1 = root.Q<Button>("btncontenidoleccion1");
        btncontenidoleccion2 = root.Q<Button>("btncontenidoleccion2");
        btncontenidoleccion3 = root.Q<Button>("btncontenidoleccion3");
        btncontenidoleccion4 = root.Q<Button>("btncontenidoleccion4");
        btncontenidoleccion5 = root.Q<Button>("btncontenidoleccion5");
        btncontenidoleccion6 = root.Q<Button>("btncontenidoleccion6");
        btntestleccion1 = root.Q<Button>("btntestleccion1");
        btntestleccion2 = root.Q<Button>("btntestleccion2");
        btntestleccion3 = root.Q<Button>("btntestleccion3");
        btntestleccion4 = root.Q<Button>("btntestleccion4");
        btntestleccion5 = root.Q<Button>("btntestleccion5");
        btntestleccion6 = root.Q<Button>("btntestleccion6");
        btntest = root.Q<Button>("bnttest");
        btnsalir = root.Q<Button>("btnsalir");

        leccion1=root.Q<VisualElement>("leccion1");
        leccion2=root.Q<VisualElement>("leccion2");
        leccion3=root.Q<VisualElement>("leccion3");
        leccion4=root.Q<VisualElement>("leccion4");
        leccion5=root.Q<VisualElement>("leccion5");
        leccion6=root.Q<VisualElement>("leccion6");

        if (btnAlfabetos != null) btnAlfabetos.RegisterCallback<ClickEvent>(ev => OnAlfabetosClick());
        if (btnKanjis != null) btnKanjis.RegisterCallback<ClickEvent>(ev => OnKanjisClick());
        if (btnJuegos != null) btnJuegos.RegisterCallback<ClickEvent>(ev => OnJuegosClick());
        if (btnLecciones != null) btnLecciones.RegisterCallback<ClickEvent>(ev => OnLeccionesClick());
        if (btnClasificacion != null) btnClasificacion.RegisterCallback<ClickEvent>(ev => OnClasificacionClick());
        if (btnPerfil != null) btnPerfil.RegisterCallback<ClickEvent>(ev => OnPerfilClick());

        if (btncontenidoleccion1 != null) btncontenidoleccion1.RegisterCallback<ClickEvent>(ev => Onbtncontenidoleccion1Click());
        if (btncontenidoleccion2 != null) btncontenidoleccion2.RegisterCallback<ClickEvent>(ev => Onbtncontenidoleccion2Click());
        if (btncontenidoleccion3 != null) btncontenidoleccion3.RegisterCallback<ClickEvent>(ev => Onbtncontenidoleccion3Click());
        if (btncontenidoleccion4 != null) btncontenidoleccion4.RegisterCallback<ClickEvent>(ev => Onbtncontenidoleccion4Click());
        if (btncontenidoleccion5 != null) btncontenidoleccion5.RegisterCallback<ClickEvent>(ev => Onbtncontenidoleccion5Click());
        if (btncontenidoleccion6 != null) btncontenidoleccion6.RegisterCallback<ClickEvent>(ev => Onbtncontenidoleccion6Click());

        if (btntestleccion1 != null) btntestleccion1.RegisterCallback<ClickEvent>(ev => OnbtntestleccionClick(1));
        if (btntestleccion2 != null) btntestleccion2.RegisterCallback<ClickEvent>(ev => OnbtntestleccionClick(2));
        if (btntestleccion3 != null) btntestleccion3.RegisterCallback<ClickEvent>(ev => OnbtntestleccionClick(3));
        if (btntestleccion4 != null) btntestleccion4.RegisterCallback<ClickEvent>(ev => OnbtntestleccionClick(4));
        if (btntestleccion5 != null) btntestleccion5.RegisterCallback<ClickEvent>(ev => OnbtntestleccionClick(5));
        if (btntestleccion6 != null) btntestleccion6.RegisterCallback<ClickEvent>(ev => OnbtntestleccionClick(6));

        if (btntest != null) btntest.RegisterCallback<ClickEvent>(ev => OnbtntestClick());
        if (btnsalir != null) btnsalir.RegisterCallback<ClickEvent>(ev => OnbtnsalirClick());

        leccionActual = UserSession.Instance.IdLeccion; 
        MostrarLecciones();
    }

    void MostrarLecciones()
    {
        leccion1.style.display = leccionActual >= 1 ? DisplayStyle.Flex : DisplayStyle.None;
        

        leccion2.style.display = leccionActual >= 2 ? DisplayStyle.Flex : DisplayStyle.None;
        

        leccion3.style.display = leccionActual >= 3 ? DisplayStyle.Flex : DisplayStyle.None;
        

        leccion4.style.display = leccionActual >= 4 ? DisplayStyle.Flex : DisplayStyle.None;
        

        leccion5.style.display = leccionActual >= 5 ? DisplayStyle.Flex : DisplayStyle.None;
        

        leccion6.style.display = leccionActual >= 6 ? DisplayStyle.Flex : DisplayStyle.None;
        
    }

    void OnbtntestClick()
    {
        SceneManager.LoadScene("Lecciones");
    }

    void OnbtntestleccionClick(int lessonId)
    {
        Debug.Log($"Button for lesson {lessonId} clicked.");
        TestController.LessonId = lessonId; // Establecer el ID de la lección en TestController
        SceneManager.LoadScene("Test 1"); // Cargar la escena de test
    }

    void OnbtnsalirClick()
    {
        SceneManager.LoadScene("Lecciones");
    }

    void Onbtncontenidoleccion1Click()
    {
        SceneManager.LoadScene("Lecciones 1");
    }

    void Onbtncontenidoleccion2Click()
    {
        SceneManager.LoadScene("Lecciones 2");
    }

    void Onbtncontenidoleccion3Click()
    {
        SceneManager.LoadScene("Lecciones 3");
    }

    void Onbtncontenidoleccion4Click()
    {
        SceneManager.LoadScene("Lecciones 4");
    }

    void Onbtncontenidoleccion5Click()
    {
        SceneManager.LoadScene("Lecciones 5");
    }

    void Onbtncontenidoleccion6Click()
    {
        SceneManager.LoadScene("Lecciones 6");
    }

    void OnAlfabetosClick()
    {
        SceneManager.LoadScene("Alfabetos");
    }

    void OnKanjisClick()
    {
        SceneManager.LoadScene("Kanjis");
    }

    void OnJuegosClick()
    {
        SceneManager.LoadScene("Juegos");
    }

    void OnLeccionesClick()
    {
        SceneManager.LoadScene("Lecciones");
    }

    void OnClasificacionClick()
    {
        SceneManager.LoadScene("Clasificacion");
    }

    void OnPerfilClick()
    {
        SceneManager.LoadScene("MiCuenta");
    }
}
