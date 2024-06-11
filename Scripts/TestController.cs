using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class TestController : MonoBehaviour
{
    UIDocument menu;
    private VisualElement root;
    private Label questionLabel;
    private Label lessonTitle;
    private Button option1Button;
    private Button option2Button;
    private Button option3Button;
    private Button option4Button;
    private Button salir;

    private List<QuestionData> questions;
    private int currentQuestionIndex = 0;

    public static int LessonId { get; set; }

    private int correctCount = 0;
    private int incorrectCount = 0;
    private float startTime;

    public VisualTreeAsset resultadoLayoutTemplate; // Asignar el UXML de la plantilla en el Inspector

    void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        root = menu.rootVisualElement;

        salir = root.Q<Button>("nextLessonButton");
        lessonTitle = root.Q<Label>("lessonTitle");
        questionLabel = root.Q<Label>("questionLabel");
        option1Button = root.Q<Button>("option1Button");
        option2Button = root.Q<Button>("option2Button");
        option3Button = root.Q<Button>("option3Button");
        option4Button = root.Q<Button>("option4Button");

        if (salir != null)
        {
            salir.RegisterCallback<ClickEvent>(ev => OnbtnsalirClick());
        }
        else
        {
            Debug.LogError("Button 'nextLessonButton' is not found");
        }

        if (option1Button != null) option1Button.clicked += () => OnOptionSelected(1);
        if (option2Button != null) option2Button.clicked += () => OnOptionSelected(2);
        if (option3Button != null) option3Button.clicked += () => OnOptionSelected(3);
        if (option4Button != null) option4Button.clicked += () => OnOptionSelected(4);

        Debug.Log($"Loading questions for LessonId: {LessonId}");
        LoadQuestions(LessonId);

        startTime = Time.time;
    }

    public void LoadQuestions(int lessonId)
    {
        questions = MongoDBManager.Instance.GetQuestionsByLessonId(lessonId);

        lessonTitle.text = $"Test Lección {lessonId}";
        if (questions != null && questions.Count > 0)
        {
            currentQuestionIndex = 0;
            LoadQuestion();
        }
        else
        {
            Debug.LogError("No se encontraron preguntas para la lección.");
            if (questions == null)
            {
                Debug.LogError("La lista de preguntas es nula.");
            }
            else
            {
                Debug.LogError($"Se encontraron {questions.Count} preguntas.");
            }
        }
    }

    void LoadQuestion()
    {
        option1Button.RemoveFromClassList("btn-correcto");
        option2Button.RemoveFromClassList("btn-correcto");
        option3Button.RemoveFromClassList("btn-correcto");
        option4Button.RemoveFromClassList("btn-correcto");

        option1Button.RemoveFromClassList("btn-incorrecto");
        option2Button.RemoveFromClassList("btn-incorrecto");
        option3Button.RemoveFromClassList("btn-incorrecto");
        option4Button.RemoveFromClassList("btn-incorrecto");

        if (questions == null)
        {
            Debug.LogError("Las preguntas no se cargaron correctamente.");
            return;
        }

        if (currentQuestionIndex < questions.Count)
        {
            var question = questions[currentQuestionIndex];
            questionLabel.text = question.Pregunta;
            option1Button.text = question.Respuesta1;
            option2Button.text = question.Respuesta2;
            option3Button.text = question.Respuesta3;
            option4Button.text = question.Respuesta4;
        }
        else
        {
            ShowResults();
        }
    }

    void OnOptionSelected(int option)
    {
        if (questions == null)
        {
            Debug.LogError("questions is null.");
            return;
        }

        var currentQuestion = questions[currentQuestionIndex];

        // Verificar si la respuesta es correcta
        if (currentQuestion.RespuestaCorrecta == option)
        {
            Debug.Log("Respuesta correcta");
            correctCount++;
            // Aplicar clase CSS para respuesta correcta al botón correspondiente
            ApplyCorrectButtonStyle(option);
        }
        else
        {
            Debug.Log("Respuesta incorrecta");
            incorrectCount++;
            // Aplicar clase CSS para respuesta incorrecta al botón correspondiente
            ApplyIncorrectButtonStyle(option);
        }

        currentQuestionIndex++;
        Invoke("LoadQuestion", 2f);
    }

    void ApplyCorrectButtonStyle(int option)
    {
        Button correctButton;
        switch (option)
        {
            case 1:
                correctButton = option1Button;
                break;
            case 2:
                correctButton = option2Button;
                break;
            case 3:
                correctButton = option3Button;
                break;
            case 4:
                correctButton = option4Button;
                break;
            default:
                correctButton = null;
                break;
        }

        if (correctButton != null)
        {
            correctButton.AddToClassList("btn-correcto");
        }
    }

    void ApplyIncorrectButtonStyle(int option)
    {
        Button incorrectButton;
        switch (option)
        {
            case 1:
                incorrectButton = option1Button;
                break;
            case 2:
                incorrectButton = option2Button;
                break;
            case 3:
                incorrectButton = option3Button;
                break;
            case 4:
                incorrectButton = option4Button;
                break;
            default:
                incorrectButton = null;
                break;
        }

        if (incorrectButton != null)
        {
            incorrectButton.AddToClassList("btn-incorrecto");
        }
    }

    void ShowResults()
    {
        float elapsedSeconds = Time.time - startTime;

        // Cargar la plantilla del UXML
        if (resultadoLayoutTemplate == null)
        {
            Debug.LogError("resultadoLayoutTemplate is not assigned in the Inspector.");
            return;
        }

        VisualElement visualTree = resultadoLayoutTemplate.CloneTree();
        root.Clear();
        root.Add(visualTree);

        var correctLabel = visualTree.Q<Label>("lblpuntuaciontest");
        var incorrectLabel = visualTree.Q<Label>("lbltiempotest");
        var imgEstadoTest = visualTree.Q<VisualElement>("imgestadotest");
        var mensajeEstadoTest = visualTree.Q<Label>("mensajeestadotest");
        var btnvolver=visualTree.Q<Button>("nextLessonButton");
        btnvolver.RegisterCallback<ClickEvent>(ev => OnbtnsalirClick());

        if (correctLabel == null || incorrectLabel == null || imgEstadoTest == null || mensajeEstadoTest == null)
        {
            Debug.LogError("One or more UI elements not found in ResultadoLayout.");
            return;
        }

        correctLabel.text = $"{correctCount}/{questions.Count}";
        incorrectLabel.text = $"{(int)(elapsedSeconds / 60)}:{(elapsedSeconds % 60):00}";

        if (correctCount >= 0.9 * questions.Count)
        {
            mensajeEstadoTest.text = "VICTORIA";
            imgEstadoTest.style.backgroundImage = new StyleBackground(Resources.Load<Texture2D>("VictoriaDerrotaSprites/victoria"));
            AssignNextLesson();
        }
        else
        {
            mensajeEstadoTest.text = "DERROTA";
            imgEstadoTest.style.backgroundImage = new StyleBackground(Resources.Load<Texture2D>("VictoriaDerrotaSprites/derrota"));
        }
    }
    void AssignNextLesson()
    {
        int nextLessonId = LessonId + 1;
        UserSession.Instance.ActualizarIdLeccion(LessonId + 1);
        Debug.Log($"Next lesson {nextLessonId} assigned to user.");
    }
    void OnbtnsalirClick()
    {
        SceneManager.LoadScene("Lecciones");
    }
    
}
