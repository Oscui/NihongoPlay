using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

public class LoginScript : MonoBehaviour
{
    private MongoDBManager dbManager;
    private UserSession userSession;
    private SessionManager sessionManager;

    UIDocument menu;
    private TextField tfUsuario;
    private TextField tfPassword;
    private Button btnIniciarSesion;
    private Label lblRegistrate;

    void Start()
    {
        userSession = UserSession.Instance;
        dbManager = MongoDBManager.Instance;
        sessionManager = SessionManager.Instance;

        // Obtener referencias a los elementos UI
        tfUsuario = GetComponent<UIDocument>().rootVisualElement.Q<TextField>("tfusuariologin");
        tfPassword = GetComponent<UIDocument>().rootVisualElement.Q<TextField>("tfpasswordlogin");
        btnIniciarSesion = GetComponent<UIDocument>().rootVisualElement.Q<Button>("btnIniciarSesion");
        lblRegistrate = GetComponent<UIDocument>().rootVisualElement.Q<Label>("lblRegistrate");

        // Asignar eventos a los botones
        btnIniciarSesion.RegisterCallback<ClickEvent>(ev => OnIniciarSesionClick());
        lblRegistrate.RegisterCallback<ClickEvent>(ev => OnRegistrateClick());
    }

    async void OnIniciarSesionClick()
    {
        string usuario = tfUsuario.value;
        string password = tfPassword.value;
        Debug.Log("Iniciando sesión con usuario o correo: " + usuario + " y contraseña: " + password);

        try
        {
            if (userSession != null)
            {
                var filter = Builders<BsonDocument>.Filter.Eq("nombre", usuario) | Builders<BsonDocument>.Filter.Eq("correo", usuario);
                var usuarioEncontrado = dbManager.database.GetCollection<BsonDocument>("Usuarios").Find(filter).FirstOrDefault();

                if (usuarioEncontrado != null)
                {
                    Debug.Log("Usuario encontrado: " + usuarioEncontrado.ToJson());

                    if (usuarioEncontrado.Contains("contraseña"))
                    {
                        string contraseñaGuardada = usuarioEncontrado["contraseña"].AsString;
                        if (password == contraseñaGuardada)
                        {
                            userSession.SetUser(usuarioEncontrado);
                            bool rachaActualizada = await userSession.ActualizarRacha();
                            if (rachaActualizada)
                            {
                                Debug.Log("Racha actualizada: " + userSession.Racha);

                                // No es necesario forzar IsNewRango aquí porque ya se actualiza en ActualizarRacha
                                Debug.Log("Nuevo rango: " + userSession.Rango);
                            }
                            else
                            {
                                Debug.Log("La racha no necesitaba ser actualizada hoy.");
                            }

                            Debug.Log("Inicio de sesión exitoso. Redireccionando a la página de Alfabetos");
                            SceneManager.LoadScene("Alfabetos");
                            return;
                        }
                    }
                    Debug.LogError("Usuario no encontrado o contraseña incorrecta.");
                }
                else
                {
                    Debug.LogError("Usuario no encontrado.");
                }
            }
            else
            {
                Debug.LogError("Error: UserSession.Instance es nulo.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error al iniciar sesión: " + ex.Message);
        }
    }





    void OnRegistrateClick()
    {
        Debug.Log("Redireccionando a la página de registro");
        SceneManager.LoadScene("Register");
    }
}
