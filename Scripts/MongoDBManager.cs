using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;

public class MongoDBManager
{
    private static MongoDBManager instance;
    private MongoClient client;
    public IMongoDatabase database; 

    private MongoDBManager(string connectionString, string databaseName)
    {
        // Crear el cliente MongoDB
        client = new MongoClient(connectionString);

        // Obtener la base de datos
        database = client.GetDatabase(databaseName);
    }

    public static MongoDBManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("Creando una nueva instancia de MongoDBManager");
                // Configuración de la cadena de conexión y el nombre de la base de datos
                string connectionString = "mongodb+srv://oca6a11:oca6a11@nihongoplay.rurt2cn.mongodb.net/?retryWrites=true&w=majority&appName=NihongoPlay&serverSelectionTimeoutMS=5000";
                string databaseName = "NihongoPlay";

                // Crear una nueva instancia de MongoDBManager
                instance = new MongoDBManager(connectionString, databaseName);
            }
            return instance;
        }
    }

    public List<QuestionData> GetQuestionsByLessonId(int lessonId)
    {
        var collection = database.GetCollection<QuestionData>("Test"); // Use the correct collection name here
        var filter = Builders<QuestionData>.Filter.Eq("id_leccion", lessonId);
        return collection.Find(filter).ToList();
    }
}
