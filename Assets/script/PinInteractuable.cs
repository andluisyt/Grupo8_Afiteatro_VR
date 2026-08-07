using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinInteractuable : MonoBehaviour
{
    [Header("Configuración del Panel")]
    [Tooltip("Escribe exactamente el Tag del panel correspondiente.")]
    [SerializeField] private string tagDelPanelAActivar;

    private GameObject panelAsociado;
    private Transform camaraVR;

    // Compartido por todos los pines.
    private static GameObject panelActualmenteVisible;

    private void Start()
    {
        // Buscar cámara VR.
        if (Camera.main != null)
        {
            camaraVR = Camera.main.transform;
        }
        else
        {
            Debug.LogError(
                "[PinInteractuable] No existe una cámara con el Tag MainCamera."
            );
        }

        // Comprobar Tag.
        if (string.IsNullOrEmpty(tagDelPanelAActivar))
        {
            Debug.LogWarning(
                "[PinInteractuable] El pin " + gameObject.name +
                " no tiene configurado el Tag del panel."
            );

            return;
        }

        // Buscar panel asociado.
        panelAsociado =
            GameObject.FindGameObjectWithTag(tagDelPanelAActivar);

        if (panelAsociado == null)
        {
            Debug.LogError(
                "[PinInteractuable] No se encontró el panel con Tag: " +
                tagDelPanelAActivar +
                " para el pin: " + gameObject.name
            );

            return;
        }

        // Lo ocultamos al iniciar.
        panelAsociado.SetActive(false);
    }

    public void MostrarInformacion()
    {
        Debug.Log(
            "Pin detectado: " + gameObject.name +
            " | Panel: " +
            (panelAsociado != null ? panelAsociado.name : "NULL")
        );

        if (panelAsociado == null)
        {
            return;
        }

        // Si existe otro panel abierto, lo cerramos.
        if (panelActualmenteVisible != null &&
            panelActualmenteVisible != panelAsociado)
        {
            panelActualmenteVisible.SetActive(false);
        }

        // IMPORTANTE:
        // NO cambiamos la posición del panel.
        // Aparecerá exactamente donde fue colocado en el Editor.

        panelAsociado.SetActive(true);

        panelActualmenteVisible = panelAsociado;

        Debug.Log(
            "Panel visible: " + panelAsociado.name +
            " | Posición conservada: " +
            panelAsociado.transform.position
        );
    }

    private void LateUpdate()
    {
        // Solamente hacemos que mire al usuario.
        // NO modificamos su posición.
        if (panelAsociado != null &&
            panelAsociado.activeSelf &&
            camaraVR != null)
        {
            OrientarPanel();
        }
    }

    private void OrientarPanel()
    {
        Vector3 direccionHaciaCamara =
            camaraVR.position -
            panelAsociado.transform.position;

        direccionHaciaCamara.y = 0f;

        if (direccionHaciaCamara.sqrMagnitude > 0.001f)
        {
            panelAsociado.transform.rotation =
                Quaternion.LookRotation(-direccionHaciaCamara);
        }
    }

    public void OcultarInformacion()
    {
        if (panelAsociado == null)
        {
            return;
        }

        panelAsociado.SetActive(false);

        if (panelActualmenteVisible == panelAsociado)
        {
            panelActualmenteVisible = null;
        }
    }
}