using UnityEngine;
using UnityEngine.UIElements;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

public class KanjiManager : MonoBehaviour
{
    private MongoDBManager dbManager;
    public VisualTreeAsset kanjiEntryTemplate; // Asignar el UXML de la plantilla en el Inspector
    private VisualElement kanjiContainer;

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

        // Cargar el contenedor de los kanjis
        var root = uiDocument.rootVisualElement;
        kanjiContainer = root.Q<VisualElement>("KanjiContainer");

        if (kanjiContainer == null)
        {
            Debug.LogError("kanjiContainer es nulo. Asegúrate de que el nombre del contenedor es correcto y está presente en el UXML.");
            return;
        }

        FetchKanjis();
    }

    async void FetchKanjis()
    {
        var collection = dbManager.database.GetCollection<BsonDocument>("Kanji");
        var filter = new BsonDocument();
        var kanjiList = await collection.Find(filter).ToListAsync();

        List<Kanji> kanjis = new List<Kanji>();
        foreach (var kanjiDocument in kanjiList)
        {
            Kanji kanji = new Kanji
            {
                Id = kanjiDocument["_id"].ToString(),
                KanjiId = kanjiDocument["id"].ToInt32(),
                Character = kanjiDocument["kanji"].AsString,
                Romaji = kanjiDocument["romaji"].AsString,
                Significado = kanjiDocument["significado"].AsString,
                LeccionId = kanjiDocument["leccion_id"].ToInt32()
            };
            kanjis.Add(kanji);
        }

        DisplayKanjis(kanjis);
    }

    void DisplayKanjis(List<Kanji> kanjis)
    {
        // Limpiar el contenedor antes de agregar nuevos elementos
        kanjiContainer.Clear();

        foreach (var kanji in kanjis)
        {
            // Lógica para determinar si el kanji está desbloqueado
            if (IsKanjiUnlocked(kanji))
            {
                VisualElement kanjiEntry = kanjiEntryTemplate.CloneTree();

                var kanjiLabel = kanjiEntry.Q<Label>("Kanji");
                var romajiLabel = kanjiEntry.Q<Label>("Romaji");
                var significadoLabel = kanjiEntry.Q<Label>("Significado");

                if (kanjiLabel == null || romajiLabel == null || significadoLabel == null)
                {
                    Debug.LogError("Uno o más elementos UI dentro de kanjiEntry son nulos. Verifica la plantilla.");
                    continue;
                }

                kanjiLabel.text = kanji.Character;
                romajiLabel.text = kanji.Romaji;
                significadoLabel.text = kanji.Significado;


                var containerElement = kanjiEntry.Q<VisualElement>("kanjientry");
                if (containerElement != null)
                {
                    containerElement.AddToClassList($"leccion-{kanji.LeccionId}");
                }
                else
                {
                    Debug.LogError("ContainerElement no encontrado. Verifica el ID en la plantilla UXML.");
                }


                kanjiContainer.Add(kanjiEntry);
            }
        }
    }


    bool IsKanjiUnlocked(Kanji kanji)
    {
        // Obtener el IdLeccion actual del usuario
        int currentLessonId = UserSession.Instance.IdLeccion;
        return kanji.LeccionId <= currentLessonId;
    }

    public class Kanji
    {
        public string Id { get; set; }
        public int KanjiId { get; set; }
        public string Character { get; set; }
        public string Romaji { get; set; }
        public string Significado { get; set; }
        public int LeccionId { get; set; }
    }
}
