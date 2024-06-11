using UnityEngine;
using UnityEngine.UIElements;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

public class LeaderboardManager : MonoBehaviour
{
    private MongoDBManager dbManager;
    public VisualTreeAsset leaderboardEntryTemplate; // Asignar el UXML de la plantilla en el Inspector
    private VisualElement leaderboardContainer;

    void Start()
    {
        dbManager = MongoDBManager.Instance;

        // Obtener el UIDocument del objeto UI
        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("No se encontró el UIDocument en el objeto.");
            return;
        }

        // Cargar el contenedor de la tabla de clasificación
        var root = uiDocument.rootVisualElement;
        leaderboardContainer = root.Q<VisualElement>("LeaderboardContainer");

        if (leaderboardContainer == null)
        {
            Debug.LogError("leaderboardContainer es nulo. Asegúrate de que el nombre del contenedor es correcto y está presente en el UXML.");
            return;
        }

        FetchLeaderboardData();
    }

    async void FetchLeaderboardData()
    {
        var collection = dbManager.database.GetCollection<BsonDocument>("Usuarios");
        var filter = new BsonDocument();
        var usersList = await collection.Find(filter).ToListAsync();

        List<User> users = new List<User>();
        foreach (var userDocument in usersList)
        {
            DateTime fechaRegistro;
            if (DateTime.TryParse(userDocument["fecha_registro"].AsString, out fechaRegistro))
            {
                User user = new User
                {
                    Id = userDocument["_id"].ToString(),
                    Nombre = userDocument["nombre"].AsString,
                    Racha = userDocument["racha"].ToInt32(),
                    Rango = userDocument["rango"].AsString,
                    PuntuacionJuego1 = userDocument["puntuacion_juego1"].ToInt32(),
                    PuntuacionJuego2 = userDocument["puntuacion_juego2"].ToInt32(),
                    FechaRegistro = fechaRegistro.ToString("yyyy-MM-dd")
                };
                users.Add(user);
            }
            else
            {
                Debug.LogError($"Error parsing fecha_registro for user {userDocument["nombre"]}");
            }
        }

        DisplayLeaderboard(users);
    }

    void DisplayLeaderboard(List<User> users)
    {
        // Ordenar por racha
        users.Sort((x, y) => y.Racha.CompareTo(x.Racha));

        foreach (var user in users)
        {
            VisualElement userEntry = leaderboardEntryTemplate.CloneTree();

            var userNameLabel = userEntry.Q<Label>("UserName");
            var userStreakLabel = userEntry.Q<Label>("UserStreak");
            var userRankLabel = userEntry.Q<Label>("UserRank");
            var userScore1Label = userEntry.Q<Label>("UserScore1");
            var userScore2Label = userEntry.Q<Label>("UserScore2");
            var userRegisterDateLabel = userEntry.Q<Label>("UserRegisterDate");
            var rankImage = userEntry.Q<VisualElement>("RankImage");

            if (userNameLabel == null || userStreakLabel == null || userRankLabel == null || 
                userScore1Label == null || userScore2Label == null || userRegisterDateLabel == null || 
                rankImage == null)
            {
                Debug.LogError("Uno o más elementos UI dentro de userEntry son nulos. Verifica la plantilla.");
                continue;
            }

            userNameLabel.text = user.Nombre;
            userStreakLabel.text = user.Racha.ToString();
            userRankLabel.text = user.Rango;
            userScore1Label.text = user.PuntuacionJuego1.ToString();
            userScore2Label.text = user.PuntuacionJuego2.ToString();
            userRegisterDateLabel.text = user.FechaRegistro;

            // Asignar la imagen basada en el rango
            rankImage.style.backgroundImage = GetRankSprite(user.Rango);

            // Asignar clases CSS basadas en el rango
            userEntry.AddToClassList(GetClassForRank(user.Rango));

            leaderboardContainer.Add(userEntry);
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

    string GetClassForRank(string rango)
    {
        switch (rango)
        {
            case "Novato":
                return "rank-novato";
            case "Principiante":
                return "rank-principiante";
            case "Intermedio":
                return "rank-intermedio";
            case "Avanzado":
                return "rank-avanzado";
            case "Experto":
                return "rank-experto";
            case "Maestro":
                return "rank-maestro";
            case "Profesional":
                return "rank-profesional";
            default:
                return "rank-default";
        }
    }

    class User
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public int Racha { get; set; }
        public string Rango { get; set; }
        public int PuntuacionJuego1 { get; set; }
        public int PuntuacionJuego2 { get; set; }
        public string FechaRegistro { get; set; }
    }
}
