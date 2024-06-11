using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

public class UserSession : MonoBehaviour
{
    public static UserSession Instance { get; private set; }

    public string UserId { get; private set; }
    public int Id { get; private set; }
    public string Nombre { get; private set; }
    public string Correo { get; private set; }
    public string Contraseña { get; private set; }
    public int Racha { get; private set; }
    public string Rango { get; set; }
    public bool IsNewRango { get; set; }
    public string FechaRegistro { get; private set; }
    public int PuntuacionJuego1 { get; private set; }
    public int PuntuacionJuego2 { get; private set; }
    public int IdLeccion { get; private set; }
    public DateTime UltimaConexion { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetUser(BsonDocument usuario)
    {
        UserId = usuario["_id"].ToString();  // Usar _id como identificador
        Nombre = usuario["nombre"].AsString;
        Correo = usuario["correo"].AsString;
        Contraseña = usuario["contraseña"].AsString;
        Racha = usuario["racha"].ToInt32();
        Rango = usuario["rango"].AsString;
        FechaRegistro = usuario["fecha_registro"].AsString;  // Mantener como string para simplicidad
        PuntuacionJuego1 = usuario["puntuacion_juego1"].ToInt32();
        PuntuacionJuego2 = usuario["puntuacion_juego2"].ToInt32();
        IdLeccion = usuario["id_leccion"].ToInt32();
        UltimaConexion = usuario.Contains("ultima_conexion") ? usuario["ultima_conexion"].ToUniversalTime() : DateTime.MinValue;

    }



    public void ClearUser()
    {
        UserId = null;
        Id = 0;
        Nombre = null;
        Correo = null;
        Contraseña = null;
        Racha = 0;
        Rango = null;
        FechaRegistro = null;
        PuntuacionJuego1 = 0;
        PuntuacionJuego2 = 0;
        IdLeccion = 0;
        UltimaConexion = DateTime.MinValue;
    }
    public async void ActualizarPuntuacionJuego1(int nuevaPuntuacion)
    {
        try
        {
            // Obtener la instancia del MongoDBManager
            MongoDBManager mongoDBManager = MongoDBManager.Instance;

            // Obtener la colección de usuarios
            var collection = mongoDBManager.database.GetCollection<BsonDocument>("Usuarios");

            // Construir el filtro para encontrar al usuario actual
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(UserId));

            // Construir la actualización para establecer la nueva puntuación del juego
            var update = Builders<BsonDocument>.Update.Set("puntuacion_juego1", nuevaPuntuacion);

            // Aplicar la actualización
            await collection.UpdateOneAsync(filter, update);

            // Actualizar la puntuación localmente
            PuntuacionJuego1 = nuevaPuntuacion;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error al actualizar la puntuación del juego 1: " + ex.Message);
        }
    }
    public async Task<List<BsonDocument>> GetCompletedLessons()
    {
        var client = new MongoClient("mongodb://localhost:27017");
        var database = client.GetDatabase("NihongoPlay");
        var collection = database.GetCollection<BsonDocument>("lecciones");

        var filter = Builders<BsonDocument>.Filter.Lte("id", IdLeccion); // Obteniendo lecciones con id menor o igual al IdLeccion actual del usuario

        var lessons = await collection.Find(filter).ToListAsync();
        return lessons;
    }
    public async void ActualizarIdLeccion(int nuevaLeccion)
    {
        try
        {
            MongoDBManager mongoDBManager = MongoDBManager.Instance;
            var collection = mongoDBManager.database.GetCollection<BsonDocument>("Usuarios");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(UserId));
            var update = Builders<BsonDocument>.Update.Set("id_leccion", nuevaLeccion);
            await collection.UpdateOneAsync(filter, update);
            IdLeccion = nuevaLeccion;
            Debug.Log("Progreso de la lección actualizado a: " + IdLeccion);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error al actualizar el progreso de la lección: " + ex.Message);
        }
    }

    public async Task<bool> ActualizarRacha()
    {
        try
        {
            DateTime now = DateTime.UtcNow;
            if (UltimaConexion.Date == now.Date)
            {
                // El usuario ya inició sesión hoy
                Debug.Log("El usuario ya inició sesión hoy");
                return false;
            }

            if (UltimaConexion.Date == now.AddDays(-1).Date)
            {
                // El usuario inició sesión ayer
                Racha++;
            }
            else
            {
                // El usuario no ha iniciado sesión en más de un día
                Racha = 1;
            }

            UltimaConexion = now;

            // Guardar el rango anterior para comparación
            string rangoAnterior = Rango;

            // Calcular el nuevo rango
            string nuevoRango = CalcularRango(Racha);
            Debug.Log("Nuevo rango calculado: " + nuevoRango);

            // Determina si el nuevo rango es diferente y si se ha alcanzado un umbral para un nuevo rango
            bool esNuevoRango = EsNuevoRango(nuevoRango);

            // Actualizar el rango y la bandera de nuevo rango
            Rango = nuevoRango;
            IsNewRango = esNuevoRango;

            // Actualizar la base de datos
            MongoDBManager mongoDBManager = MongoDBManager.Instance;
            var collection = mongoDBManager.database.GetCollection<BsonDocument>("Usuarios");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(UserId));
            var update = Builders<BsonDocument>.Update
                .Set("racha", Racha)
                .Set("ultima_conexion", UltimaConexion)
                .Set("rango", nuevoRango);

            var result = await collection.UpdateOneAsync(filter, update);
            Debug.Log("Resultado de la actualización en la base de datos: " + result.ModifiedCount);

            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError("Error al actualizar la racha: " + ex.Message);
            return false;
        }
    }

    private bool EsNuevoRango(string nuevoRango)
    {
        if (nuevoRango == "Principiante" && Racha == 7)
        {
            return true;
        }
        else if (nuevoRango == "Intermedio" && Racha == 14)
        {
            return true;
        }
        else if (nuevoRango == "Avanzado" && Racha == 21)
        {
            return true;
        }
        else if (nuevoRango == "Experto" && Racha == 28)
        {
            return true;
        }
        else if (nuevoRango == "Maestro" && Racha == 35)
        {
            return true;
        }
        else if (nuevoRango == "Profesional" && Racha == 42)
        {
            return true;
        }
        return false;
    }

    private string CalcularRango(int racha)
    {
        Debug.Log("Calculando rango para racha: " + racha);
        if (racha >= 7 && racha < 14)
        {
            return "Principiante";
        }
        else if (racha >= 14 && racha < 21)
        {
            return "Intermedio";
        }
        else if (racha >= 21 && racha < 28)
        {
            return "Avanzado";
        }
        else if (racha >= 28 && racha < 35)
        {
            return "Experto";
        }
        else if (racha >= 35 && racha < 42)
        {
            return "Maestro";
        }
        else if (racha >= 42)
        {
            return "Profesional";
        }
        else
        {
            return "Novato";
        }
    }


}


