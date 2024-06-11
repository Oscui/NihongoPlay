using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

public class RegisterScript : MonoBehaviour
{
    private MongoDBManager dbManager;
    UIDocument menu;
    private TextField tfCorreo;
    private TextField tfPassword;
    private TextField tfNombre;
    private Button btnRegistrar;
    private Label lblIniciarSesion;

    void Start()
{
    dbManager = MongoDBManager.Instance;

    // Obtener referencias a los elementos UI
    tfCorreo = GetComponent<UIDocument>().rootVisualElement.Q<TextField>("tfusuarioregister");
    tfPassword = GetComponent<UIDocument>().rootVisualElement.Q<TextField>("tfpasswordregister");
    tfNombre = GetComponent<UIDocument>().rootVisualElement.Q<TextField>("tfnombreregister");
    btnRegistrar = GetComponent<UIDocument>().rootVisualElement.Q<Button>("botonregistro");
    lblIniciarSesion = GetComponent<UIDocument>().rootVisualElement.Q<Label>("lbliniciasesion");

    // Asignar eventos a los botones
    btnRegistrar.RegisterCallback<ClickEvent>(ev => OnRegistrarClick());
    lblIniciarSesion.RegisterCallback<ClickEvent>(ev => OnIniciarSesionClick());
}

void OnRegistrarClick()
{
    // Verificar si dbManager es null
    if (dbManager == null)
    {
        Debug.LogError("dbManager is not initialized.");
        return;
    }

    string nombre = tfNombre.value;
    string correo = tfCorreo.value;
    string contraseña = tfPassword.value;
    string fechaRegistro = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
    DateTime ultimaConexion = DateTime.UtcNow;

    // Crear el documento del usuario con ObjectId como ID
    var usuario = new BsonDocument
    {
        { "nombre", nombre },
        { "correo", correo },
        { "contraseña", contraseña },
        { "fecha_registro", fechaRegistro },
        { "racha", 0 },
        { "rango", "Novato" },
        { "puntuacion_juego1", 0 },
        { "puntuacion_juego2", 0 },
        { "id_leccion", 1 },
        { "ultima_conexion", ultimaConexion }
        
    };

    // Insertar el usuario en la base de datos
    if (InsertUsuario(usuario))
    {
        // Si la inserción es exitosa, cargar la escena de inicio de sesión
        SceneManager.LoadScene("Login");
    }
    else
    {
        Debug.LogError("Error al registrar el usuario.");
        // Aquí puedes manejar el error de inserción de alguna manera, como mostrar un mensaje al usuario
    }
}

bool InsertUsuario(BsonDocument usuario)
{
    try
    {
        // Obtener la colección de usuarios
        var collection = dbManager.database.GetCollection<BsonDocument>("Usuarios");

        // Insertar el documento del usuario en la colección
        collection.InsertOne(usuario);

        // Si llegamos aquí, la inserción fue exitosa
        return true;
    }
    catch (Exception ex)
    {
        // Manejar cualquier excepción que pueda ocurrir durante la inserción
        Debug.LogError("Error al insertar usuario: " + ex.Message);
        return false;
    }
}

    void OnIniciarSesionClick()
    {
        Debug.Log("Redireccionando a la página de inicio de sesión");
        SceneManager.LoadScene("Login");
    }
}
