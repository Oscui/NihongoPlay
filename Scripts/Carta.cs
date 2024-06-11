using System;
using UnityEngine;
using System.Collections;

public class Carta : MonoBehaviour
{
    public int idCarta = 0;
    public event Action<Carta> OnCartaClick;

    private bool mostrando = false;

    public Sprite spritePorDefecto;
    private Sprite spriteReal;

    private Quaternion rotacionInicial;
    private Quaternion rotacionFinal;

    void Start()
    {
        rotacionInicial = transform.rotation;
        rotacionFinal = Quaternion.Euler(0, 360, 0) * rotacionInicial; // Girar 180 grados sobre el eje Y
    }

    public void MostrarCarta()
    {
        if (!mostrando)
        {
            StartCoroutine(GirarCarta(rotacionInicial, rotacionFinal, spriteReal));
            mostrando = true;
        }
    }

    public void OcultarCarta()
    {
        if (mostrando)
        {
            StartCoroutine(GirarCarta(rotacionFinal, rotacionInicial, spritePorDefecto));
            mostrando = false;
        }
    }

    IEnumerator GirarCarta(Quaternion from, Quaternion to, Sprite spriteFinal)
    {
        float elapsedTime = 0f;
        float duration = 0.5f; // Duración de la animación de giro
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // Primera mitad de la rotación
        while (elapsedTime < duration / 2)
        {
            transform.rotation = Quaternion.Slerp(from, Quaternion.Euler(0, 90, 0) * from, elapsedTime / (duration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que la rotación esté en 90 grados exactos
        transform.rotation = Quaternion.Euler(0, 90, 0) * from;

        // Actualizar el sprite cuando la carta está a 90 grados (de espaldas)
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = spriteFinal;
        }

        elapsedTime = 0f;

        // Segunda mitad de la rotación
        while (elapsedTime < duration / 2)
        {
            transform.rotation = Quaternion.Slerp(Quaternion.Euler(0, 90, 0) * from, to, elapsedTime / (duration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que la rotación esté en el estado final exacto
        transform.rotation = to;
    }


    public void DesactivarCarta()
    {
        GetComponent<Collider2D>().enabled = false;
    }

    void OnMouseDown()
    {
        OnCartaClick?.Invoke(this);
    }

    public void SetSprite(Sprite sprite)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = sprite;
        }
    }

    public void SetSpriteReal(Sprite sprite)
    {
        spriteReal = sprite;
    }

    public void SetSpritePorDefecto(Sprite sprite)
    {
        spritePorDefecto = sprite;
    }

    public bool EstaMostrando()
    {
        return mostrando;
    }
}
