using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CrearCasilla : MonoBehaviour
{
    public GameObject CartaPrefab;
    public int Ancho;
    public int Alto;
    public float EspacioEntreCartas = 1.5f;
    public Sprite[] sprites;
    public Sprite spritePorDefecto;

    private Carta cartaSeleccionada1;
    private Carta cartaSeleccionada2;
    private bool bloquearInput = false;
    private int parejasEncontradas = 0;
    private int intentosRealizados = 0;
    private int puntuacion = 0;
    private ControBotonesTatamiTiles controBotonesTatamiTiles;
    public float tiempoRestante; // Tiempo límite en segundos

    void Start()
    {
        Crear();
    }

    void Update()
    {
        if (!bloquearInput)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null)
                {
                    Carta carta = hit.collider.gameObject.GetComponent<Carta>();
                    if (carta != null && !carta.EstaMostrando())
                    {
                        SeleccionarCarta(carta);
                    }
                }
            }
        }

        // Actualizar el tiempo restante
        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            if (tiempoRestante <= 0)
            {
                // Guardar puntuación usando PlayerPrefs antes de cambiar de escena
                PlayerPrefs.SetInt("Puntuacion", puntuacion);
                PlayerPrefs.Save();
                UserSession.Instance.ActualizarPuntuacionJuego1(puntuacion);
                // Si el tiempo ha terminado, cargar la escena de derrota
                SceneManager.LoadScene("Tatami TilesEscenaDerrota");
            }
        }
    }

    void SeleccionarCarta(Carta carta)
    {
        if (cartaSeleccionada1 == null)
        {
            cartaSeleccionada1 = carta;
            carta.MostrarCarta();
        }
        else if (cartaSeleccionada2 == null && carta != cartaSeleccionada1)
        {
            cartaSeleccionada2 = carta;
            carta.MostrarCarta();
            StartCoroutine(CheckMatch());
        }
    }

    IEnumerator CheckMatch()
    {
        bloquearInput = true;

        yield return new WaitForSeconds(1f);

        if (cartaSeleccionada1.idCarta == cartaSeleccionada2.idCarta)
        {
            cartaSeleccionada1.DesactivarCarta();
            cartaSeleccionada2.DesactivarCarta();
            parejasEncontradas++;

            // Incrementar la puntuación por encontrar una pareja
            puntuacion += 100;

            // Comprobar si se han encontrado todas las parejas
            if (parejasEncontradas >= (Ancho * Alto) / 2)
            {
                
                // Victoria
                PlayerPrefs.SetInt("Puntuacion", puntuacion);
                PlayerPrefs.Save(); // Asegúrate de guardar los cambios
                UserSession.Instance.ActualizarPuntuacionJuego1(puntuacion);
                SceneManager.LoadScene("Tatami TilesEscenaVictoria");

            }
        }
        else
        {
            cartaSeleccionada1.OcultarCarta();
            cartaSeleccionada2.OcultarCarta();

            // Incrementar el número de intentos realizados
            intentosRealizados++;
        }

        cartaSeleccionada1 = null;
        cartaSeleccionada2 = null;

        bloquearInput = false;
    }


    public void Crear()
    {
        List<int> indices = new List<int>();

        // Crear una lista de índices para las cartas
        for (int i = 0; i < (Ancho * Alto) / 2; i++)
        {
            indices.Add(i);
            indices.Add(i);
        }

        // Barajar la lista de índices
        indices = Shuffle(indices);

        Carta[] cartas = new Carta[Ancho * Alto];
        int index = 0;
        for (int i = 0; i < Ancho; i++)
        {
            for (int j = 0; j < Alto; j++)
            {
                float posX = i * EspacioEntreCartas;
                float posY = j * EspacioEntreCartas;

                GameObject cartaTemp = Instantiate(CartaPrefab, new Vector3(posX, posY, 0), Quaternion.identity);
                Carta cartaComponente = cartaTemp.GetComponent<Carta>();
                cartaComponente.idCarta = indices[index];
                cartaComponente.SetSprite(spritePorDefecto);
                cartaComponente.SetSpriteReal(sprites[indices[index]]);
                cartas[index] = cartaComponente;
                index++;
            }
        }
    }

    // Función para barajar una lista
    List<T> Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }
}
