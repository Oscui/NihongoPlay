using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoNotificaciones : MonoBehaviour
{
    public static EstadoNotificaciones Instance { get; private set; }

    public bool IsPanelInformacionOculto { get; set; } = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
