using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PinButtonSistema : MonoBehaviour
{
    [Header("Imagen del botón PIN")]
    public Image buttonImage;

    [Header("Gestor de interacción de sistemas")]
    public GestorInteraccionSistemas gestorInteraccion;

    [Header("Estado del botón")]
    public bool pinActivo = false;

    private readonly Color32 colorOn =
        new Color32(255, 255, 255, 255);

    private readonly Color32 colorOff =
        new Color32(180, 180, 180, 255);

    private void Start()
    {
        pinActivo = false;

        if (buttonImage != null)
            buttonImage.color = colorOff;

        if (gestorInteraccion != null)
            gestorInteraccion.CambiarEstadoPines(false);
    }

    public void OnPress()
    {
        pinActivo = !pinActivo;

        if (buttonImage != null)
        {
            buttonImage.color =
                pinActivo ? colorOn : colorOff;
        }

        if (gestorInteraccion != null)
        {
            gestorInteraccion.CambiarEstadoPines(
                pinActivo
            );
        }
        else
        {
            Debug.LogWarning(
                "No se asignó GestorInteraccionSistemas en ControlDePines."
            );
        }
    }
}