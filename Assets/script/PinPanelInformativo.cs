using System.Collections;
using UnityEngine;

public class PinPanelInformativo : MonoBehaviour
{
    [Header("Referencias")]
    public GestorPanelesInformativos gestorPaneles;
    public GameObject panelCorrespondiente;
    public Transform camaraVR;

    [Header("Billboard")]
    public bool orientarHaciaCamara = true;
    public bool girarPanel180 = true;

    [Header("Protección contra activaciones rápidas")]
    public float tiempoMinimoEntreActivaciones = 0.35f;

    private float ultimaActivacion = -100f;

    public void AlternarInformacion()
    {
        // Evita activaciones múltiples por pequeñas
        // vibraciones del Ray del controlador.
        if (
            Time.unscaledTime - ultimaActivacion
            < tiempoMinimoEntreActivaciones
        )
        {
            return;
        }

        ultimaActivacion = Time.unscaledTime;

        if (gestorPaneles == null)
        {
            Debug.LogError(
                "No se asignó el gestor en " + gameObject.name
            );

            return;
        }

        if (panelCorrespondiente == null)
        {
            Debug.LogError(
                "No se asignó el panel en " + gameObject.name
            );

            return;
        }

        // IMPORTANTE:
        // Ya NO modificamos la posición del panel.
        // Mantiene exactamente la posición configurada
        // manualmente en Unity.

        gestorPaneles.AlternarPanel(
            panelCorrespondiente,
            this
        );
    }

    private void LateUpdate()
    {
        if (
            panelCorrespondiente == null ||
            !panelCorrespondiente.activeInHierarchy ||
            !orientarHaciaCamara
        )
        {
            return;
        }

        Transform camaraUsada = ObtenerCamara();

        if (camaraUsada == null)
        {
            return;
        }

        // Solo modificamos la ROTACIÓN.
        // La posición permanece intacta.
        OrientarPanelHaciaCamara(camaraUsada);
    }

    private void OrientarPanelHaciaCamara(
        Transform camaraUsada
    )
    {
        Vector3 direccion =
            camaraUsada.position -
            panelCorrespondiente.transform.position;

        if (direccion.sqrMagnitude < 0.0001f)
        {
            return;
        }

        Quaternion rotacion =
            Quaternion.LookRotation(
                direccion.normalized,
                Vector3.up
            );

        if (girarPanel180)
        {
            rotacion *= Quaternion.Euler(
                0f,
                180f,
                0f
            );
        }

        panelCorrespondiente.transform.rotation =
            rotacion;
    }

    private Transform ObtenerCamara()
    {
        if (camaraVR != null)
        {
            return camaraVR;
        }

        if (Camera.main != null)
        {
            return Camera.main.transform;
        }

        return null;
    }

    private void OnDisable()
    {
        // Si se apaga el sistema anatómico,
        // también cierra su panel.
        if (
            gestorPaneles != null &&
            panelCorrespondiente != null
        )
        {
            gestorPaneles.CerrarSiEsActual(
                panelCorrespondiente
            );
        }
    }
}