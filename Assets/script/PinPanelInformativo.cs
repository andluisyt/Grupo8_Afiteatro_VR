using System.Collections;
using System.Collections;
using UnityEngine;

public class PinPanelInformativo : MonoBehaviour
{
    [Header("Referencias")]
    public GestorPanelesInformativos gestorPaneles;
    public GameObject panelCorrespondiente;
    public Transform camaraVR;

    [Header("Posición del panel")]
    public float alturaSobreElPin = 0.28f;
    public float distanciaHaciaElUsuario = 0.38f;

    [Header("Billboard")]
    public bool orientarHaciaCamara = true;
    public bool girarPanel180 = true;

    [Header("Protección contra activaciones rápidas")]
    public float tiempoMinimoEntreActivaciones = 0.35f;

    private float ultimaActivacion = -100f;

    public void AlternarInformacion()
    {
        // Evita que pequeñas vibraciones del rayo ejecuten
        // varias activaciones casi simultáneamente.
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

        ColocarPanelSobreElPin();

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

        OrientarPanelHaciaCamara(camaraUsada);
    }

    private void ColocarPanelSobreElPin()
    {
        Transform camaraUsada = ObtenerCamara();

        Vector3 nuevaPosicion =
            transform.position +
            Vector3.up * alturaSobreElPin;

        if (camaraUsada != null)
        {
            Vector3 direccionHaciaUsuario =
                (
                    camaraUsada.position -
                    nuevaPosicion
                ).normalized;

            nuevaPosicion +=
                direccionHaciaUsuario *
                distanciaHaciaElUsuario;
        }

        panelCorrespondiente.transform.position =
            nuevaPosicion;

        if (camaraUsada != null && orientarHaciaCamara)
        {
            OrientarPanelHaciaCamara(camaraUsada);
        }
    }

    private void OrientarPanelHaciaCamara(
        Transform camaraUsada
    )
    {
        Vector3 direccion =
            camaraUsada.position -
            panelCorrespondiente.transform.position;

        // Evita errores cuando ambas posiciones coinciden.
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