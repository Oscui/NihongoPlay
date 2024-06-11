using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class AlfabetosScript : MonoBehaviour
{

    public VisualTreeAsset newRangoPanel;
    public VisualTreeAsset rachaPanel;
    public AudioSource audioSource;

    UIDocument menu;
    private Button btnAlfabetos;
    private Button btnKanjis;
    private Button btnJuegos;
    private Button btnLecciones;
    private Button btnClasificacion;
    private Button btnPerfil;
    private Button btnhiragana;
    private Button btnkatakana;

    private Button[] buttons;


    void OnEnable()
    {

        menu = GetComponent<UIDocument>();
        VisualElement root = menu.rootVisualElement;

        // Obtener referencias a los botones principales
        btnAlfabetos = root.Q<Button>("btnalfabetos");
        btnKanjis = root.Q<Button>("btnkanjis");
        btnJuegos = root.Q<Button>("btnjuegos");
        btnLecciones = root.Q<Button>("btnlecciones");
        btnClasificacion = root.Q<Button>("btnclasificacion");
        btnPerfil = root.Q<Button>("btnperfil");
        btnhiragana = root.Q<Button>("btnhiragana");
        btnkatakana = root.Q<Button>("btnkatakana");

        // Asignar eventos a los botones principales
        btnAlfabetos.RegisterCallback<ClickEvent>(ev => OnAlfabetosClick());
        btnKanjis.RegisterCallback<ClickEvent>(ev => OnKanjisClick());
        btnJuegos.RegisterCallback<ClickEvent>(ev => OnJuegosClick());
        btnLecciones.RegisterCallback<ClickEvent>(ev => OnLeccionesClick());
        btnClasificacion.RegisterCallback<ClickEvent>(ev => OnClasificacionClick());
        btnPerfil.RegisterCallback<ClickEvent>(ev => OnPerfilClick());
        btnhiragana.RegisterCallback<ClickEvent>(ev => OnHiraganaClick());
        btnkatakana.RegisterCallback<ClickEvent>(ev => OnKatakanaClick());

        // Obtener todos los botones del alfabeto hiragana
        List<Button> buttons = root.Query<Button>().Where(btn => btn.name.StartsWith("btn") && char.IsLetter(btn.name[3])).ToList();


        // Asignar eventos a los botones del hiragana
        foreach (Button button in buttons)
        {
            button.RegisterCallback<ClickEvent>(ev => OnButtonClick(button.name));
        }
        MostrarPanelInicial();
    }

    // Eventos de los botones principales
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

    // Evento común para todos los botones del hiragana
    void OnButtonClick(string buttonName)
    {
        Debug.Log("Botón " + buttonName + " pulsado");
        ReproducirAudio("AlfabetosSFX/kanasound-" + buttonName.Substring(3).ToLower());
    }

    // Reproducir audio
    void ReproducirAudio(string path)
    {
        AudioClip clip = Resources.Load<AudioClip>(path);

        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No se pudo cargar el archivo de audio desde: " + path);
        }
    }

    void MostrarPanelInicial()
    {
        // Verificar si EstadoNotificaciones.Instance está inicializado
        if (EstadoNotificaciones.Instance == null)
        {
            Debug.LogError("EstadoNotificaciones.Instance no está inicializado.");
            return;
        }

        // Verificar si el panel de información ya está oculto
        if (EstadoNotificaciones.Instance.IsPanelInformacionOculto)
        {
            VisualElement root = menu.rootVisualElement;
            VisualElement panelContainer = root.Q<VisualElement>("panel-container");
            if (panelContainer != null)
            {
                panelContainer.RemoveFromHierarchy();
                Debug.Log("Contenedor del panel eliminado de la jerarquía.");
            }
            else
            {
                Debug.LogWarning("El contenedor del panel no fue encontrado.");
            }
            return;

        }


        UserSession userSession = UserSession.Instance;

        if (userSession != null)
        {
            VisualElement root = menu.rootVisualElement;
            VisualElement panelContainer = root.Q<VisualElement>("panel-container");

            if (userSession.IsNewRango)
            {
                MostrarPanelRango(panelContainer, userSession);
            }
            else
            {
                MostrarPanelRacha(panelContainer, userSession);
            }
        }
    }

    void MostrarPanelRango(VisualElement panelContainer, UserSession userSession)
    {
        VisualElement newRangoPanelInstance = newRangoPanel.CloneTree();
        panelContainer.Add(newRangoPanelInstance);
        newRangoPanelInstance.BringToFront(); // Coloca el nuevo panel en la parte delantera

        VisualElement imgrango = newRangoPanelInstance.Q<VisualElement>("imgrango");
        Label lblRango = newRangoPanelInstance.Q<Label>("rangoactual");
        Button btnsalir = newRangoPanelInstance.Q<Button>("btnsalir");

        lblRango.text = userSession.Rango; 
        imgrango.style.backgroundImage = GetRankSprite(lblRango.text);

        btnsalir.RegisterCallback<ClickEvent>(ev => OcultarPanel(newRangoPanelInstance));

        
        userSession.IsNewRango = false;
    }

    void MostrarPanelRacha(VisualElement panelContainer, UserSession userSession)
    {
        VisualElement rachaPanelInstance = rachaPanel.CloneTree();
        panelContainer.Add(rachaPanelInstance);
        rachaPanelInstance.BringToFront(); // Coloca el nuevo panel en la parte delantera

        Label lblRacha = rachaPanelInstance.Q<Label>("rachaactual");
        Button btnsalir = rachaPanelInstance.Q<Button>("btnsalir");

        lblRacha.text = userSession.Racha.ToString();

        btnsalir.RegisterCallback<ClickEvent>(ev => OcultarPanel(rachaPanelInstance));
    }


    void OcultarPanel(VisualElement panel)
    {
        if (panel != null)
        {
            Debug.Log("Ocultando panel: " + panel.name);

            // Eliminar el panel de la jerarquía
            panel.RemoveFromHierarchy();
            Debug.Log("Panel eliminado de la jerarquía: " + panel.name);

            // Eliminar el contenedor del panel de la jerarquía
            VisualElement root = menu.rootVisualElement;
            VisualElement panelContainer = root.Q<VisualElement>("panel-container");
            if (panelContainer != null)
            {
                panelContainer.RemoveFromHierarchy();
                Debug.Log("Contenedor del panel eliminado de la jerarquía.");
            }
            else
            {
                Debug.LogWarning("El contenedor del panel no fue encontrado.");
            }

            // Marcar el panel como oculto en el estado de notificaciones
            EstadoNotificaciones.Instance.IsPanelInformacionOculto = true;
            Debug.Log("Estado de notificaciones actualizado: " + EstadoNotificaciones.Instance.IsPanelInformacionOculto);

            // Forzar a la UI a recalcular el layout
            menu.rootVisualElement.MarkDirtyRepaint();
            Debug.Log("UI layout recalculado después de ocultar el panel.");

            // Listar elementos en la jerarquía
           
        }
        else
        {
            Debug.LogWarning("El panel es nulo, no se puede ocultar.");
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
