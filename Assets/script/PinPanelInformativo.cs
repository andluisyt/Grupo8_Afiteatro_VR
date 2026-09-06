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

    private static PinPanelInformativo pinResaltado;
    private static readonly int baseColorId = Shader.PropertyToID("_BaseColor");
    private static readonly int baseMapId = Shader.PropertyToID("_BaseMap");

    private MeshRenderer rendererPin;
    private MaterialPropertyBlock propiedadesOriginales;
    private MaterialPropertyBlock propiedadesResaltadas;
    private bool resaltadoActivo;

    public void ResaltarPin()
    {
        if (!isActiveAndEnabled || panelCorrespondiente == null ||
            !panelCorrespondiente.activeInHierarchy)
        {
            return;
        }

        if (pinResaltado != null && pinResaltado != this)
        {
            pinResaltado.RestaurarApariencia();
        }

        if (resaltadoActivo)
            return;

        if (rendererPin == null)
        {
            rendererPin = GetComponent<MeshRenderer>();
            if (rendererPin == null)
                rendererPin = GetComponentInChildren<MeshRenderer>(true);
        }

        if (rendererPin == null)
            return;

        if (propiedadesOriginales == null)
        {
            propiedadesOriginales = new MaterialPropertyBlock();
            propiedadesResaltadas = new MaterialPropertyBlock();
        }

        // Guardar el bloque completo antes de cada nuevo resaltado.
        rendererPin.GetPropertyBlock(propiedadesOriginales);
        rendererPin.GetPropertyBlock(propiedadesResaltadas);
        propiedadesResaltadas.SetColor(baseColorId, new Color(1f, 0.85f, 0f, 1f));
        propiedadesResaltadas.SetTexture(baseMapId, Texture2D.whiteTexture);
        rendererPin.SetPropertyBlock(propiedadesResaltadas);

        resaltadoActivo = true;
        pinResaltado = this;
    }

    public void RestaurarApariencia()
    {
        if (resaltadoActivo && rendererPin != null)
        {
            rendererPin.SetPropertyBlock(
                propiedadesOriginales.isEmpty ? null : propiedadesOriginales
            );
        }

        resaltadoActivo = false;
        if (pinResaltado == this)
            pinResaltado = null;
    }

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
        // También cubrir cierres externos, aunque el billboard esté desactivado.
        if (resaltadoActivo &&
            (panelCorrespondiente == null || !panelCorrespondiente.activeInHierarchy))
        {
            RestaurarApariencia();
        }

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
        RestaurarApariencia();

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
