using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestorPines : MonoBehaviour
{
    public static GestorPines Instancia;
    public GameObject panelInfo;
    public Text textoInfo;
    private PinInteractivo pinActivo;

    void Awake() { Instancia = this; }

    public void MostrarInfo(string info, PinInteractivo pinOrigen)
    {
        // Si clickeamos el mismo, lo desactivamos
        if (pinActivo == pinOrigen)
        {
            panelInfo.SetActive(false);
            pinActivo = null;
            return;
        }

        // Activar nuevo y desactivar anterior lógicamente
        pinActivo = pinOrigen;
        textoInfo.text = info;
        panelInfo.SetActive(true);
    }
}