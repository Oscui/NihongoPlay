using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MicuentaScript : MonoBehaviour
{
    private UserSession userSession;
    private SessionManager sessionManager;

    UIDocument menu;
    private Button btnAlfabetos;
    private Button btnKanjis;
    private Button btnJuegos;
    private Button btnLecciones;
    private Button btnClasificacion;
    private Button btnPerfil;
    private Button btncerrarsesion;
    
    private Label lblUsuario;
    private Label lblCorreo;
    private Label lblRacha;
    private Label lblFechaRegistro;
    private Label lblPuntuacion1;
    private Label lblPuntuacion2;
    private Label lblLeccion;
    private VisualElement vefotorango;

    void OnEnable()
    {
        userSession = UserSession.Instance;
        sessionManager = SessionManager.Instance;

        menu = GetComponent<UIDocument>();
        VisualElement root = menu.rootVisualElement;

        // Obtener el componente UIDocument
        var uiDocument = GetComponent<UIDocument>();

        // Obtener referencias a los botones
        btnAlfabetos = root.Q<Button>("btnalfabetos");
        btnKanjis = root.Q<Button>("btnkanjis");
        btnJuegos = root.Q<Button>("btnjuegos");
        btnLecciones = root.Q<Button>("btnlecciones");
        btnClasificacion = root.Q<Button>("btnclasificacion");
        btnPerfil = root.Q<Button>("btnperfil");
        btncerrarsesion = root.Q<Button>("btncerrarsesion");

        // Obtener referencias a los labels
        lblUsuario = root.Q<Label>("lblusuario");
        lblCorreo = root.Q<Label>("lblcorreo");
        lblRacha = root.Q<Label>("lblracha");
        lblFechaRegistro = root.Q<Label>("lblfecharegistro");
        lblPuntuacion1 = root.Q<Label>("lblpuntuacion1");
        lblPuntuacion2 = root.Q<Label>("lblpuntuacion2");
        lblLeccion = root.Q<Label>("lblleccion");
        vefotorango = root.Q<VisualElement>("vefotorango");

        // Asignar eventos a los botones usando RegisterCallback
        btnAlfabetos.RegisterCallback<ClickEvent>(ev => OnAlfabetosClick());
        btnKanjis.RegisterCallback<ClickEvent>(ev => OnKanjisClick());
        btnJuegos.RegisterCallback<ClickEvent>(ev => OnJuegosClick());
        btnLecciones.RegisterCallback<ClickEvent>(ev => OnLeccionesClick());
        btnClasificacion.RegisterCallback<ClickEvent>(ev => OnClasificacionClick());
        btnPerfil.RegisterCallback<ClickEvent>(ev => OnPerfilClick());
        btncerrarsesion.RegisterCallback<ClickEvent>(ev => OnCerrarSesionClick());

        // Cargar los datos del usuario y actualizar la UI
        CargarDatosUsuario();
    }

    void CargarDatosUsuario()
    {
        if (userSession != null)
        {
            // Actualizar los labels con los datos del usuario
            lblUsuario.text = userSession.Nombre;
            lblCorreo.text = userSession.Correo;
            lblRacha.text = userSession.Racha.ToString();
            lblFechaRegistro.text = userSession.FechaRegistro;
            lblPuntuacion1.text = userSession.PuntuacionJuego1.ToString();
            lblPuntuacion2.text = userSession.PuntuacionJuego2.ToString();
            lblLeccion.text = userSession.IdLeccion.ToString();

            // Asignar la imagen del rango
            vefotorango.style.backgroundImage = GetRankSprite(userSession.Rango);
        }
        else
        {
            Debug.LogError("Error: UserSession.Instance es nulo.");
        }
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

    void OnCerrarSesionClick()
    {
        if (sessionManager != null)
        {
            // Limpiar la sesión del usuario y cerrar sesión
            userSession.ClearUser();
            sessionManager.LogOut();

            Debug.Log("Sesión cerrada. Redireccionando a la página de login.");
            // Redirigir al usuario a la escena de login
            SceneManager.LoadScene("Login");
        }
        else
        {
            Debug.LogError("Error: SessionManager.Instance es nulo.");
        }
    }

    StyleBackground GetRankSprite(string rango)
    {
        Texture2D texture;
        switch (rango)
        {
            case "Novato":
                texture = Resources.Load<Texture2D>("Rangos/onigirihierro");
                break;
            case "Principiante":
                texture = Resources.Load<Texture2D>("Rangos/onigiribronce");
                break;
            case "Intermedio":
                texture = Resources.Load<Texture2D>("Rangos/onigiriplata");
                break;
            case "Avanzado":
                texture = Resources.Load<Texture2D>("Rangos/onigirioro");
                break;
            case "Experto":
                texture = Resources.Load<Texture2D>("Rangos/onigiridiamante");
                break;
            case "Maestro":
                texture = Resources.Load<Texture2D>("Rangos/onigiriesmeralda");
                break;
            case "Profesional":
                texture = Resources.Load<Texture2D>("Rangos/onigirinativo");
                break;
            default:
                texture = Resources.Load<Texture2D>("Rangos/onigirihierro");
                break;
        }
        return new StyleBackground(texture);
    }
}
