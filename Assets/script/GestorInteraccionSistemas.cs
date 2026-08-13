using System.Collections.Generic;
using UnityEngine;

public class GestorInteraccionSistemas : MonoBehaviour
{
    [Header("Modelos 3D")]
    public GameObject sistemaCirculatorio;
    public GameObject sistemaEsqueletico;
    public GameObject sistemaMuscular;
    public GameObject sistemaDigestivo;
    public GameObject sistemaNervioso;
    public GameObject sistemaRespiratorio;

    [Header("Grupos de Pines")]
    public GameObject pinesCirculatorio;
    public GameObject pinesEsqueletico;
    public GameObject pinesMuscular;
    public GameObject pinesDigestivo;
    public GameObject pinesNervioso;
    public GameObject pinesRespiratorio;

    [Header("Gestores de Paneles Informativos")]
    public GestorPanelesInformativos infoCirculatorio;
    public GestorPanelesInformativos infoEsqueletico;
    public GestorPanelesInformativos infoMuscular;
    public GestorPanelesInformativos infoDigestivo;
    public GestorPanelesInformativos infoNervioso;
    public GestorPanelesInformativos infoRespiratorio;

    // Sistema que actualmente tiene prioridad
    private GameObject modeloActual;
    private GameObject pinesActuales;

    // Estado del botón general PIN
    private bool pinesHabilitados = false;

    // Guarda el orden en que se activaron los sistemas.
    // El último elemento es el sistema con prioridad.
    private List<GameObject> historialSistemas =
        new List<GameObject>();


    private void Start()
    {
        DesactivarTodosLosPines();

        historialSistemas.Clear();

        modeloActual = null;
        pinesActuales = null;
    }


    // =====================================================
    // BOTONES DE SISTEMAS
    // =====================================================

    public void ActivarCirculatorio()
    {
        SeleccionarSistema(
            sistemaCirculatorio,
            pinesCirculatorio
        );
    }


    public void ActivarEsqueletico()
    {
        SeleccionarSistema(
            sistemaEsqueletico,
            pinesEsqueletico
        );
    }


    public void ActivarMuscular()
    {
        SeleccionarSistema(
            sistemaMuscular,
            pinesMuscular
        );
    }


    public void ActivarDigestivo()
    {
        SeleccionarSistema(
            sistemaDigestivo,
            pinesDigestivo
        );
    }


    public void ActivarNervioso()
    {
        SeleccionarSistema(
            sistemaNervioso,
            pinesNervioso
        );
    }


    public void ActivarRespiratorio()
    {
        SeleccionarSistema(
            sistemaRespiratorio,
            pinesRespiratorio
        );
    }


    // =====================================================
    // SELECCIÓN / DESELECCIÓN DE SISTEMA
    // =====================================================

    private void SeleccionarSistema(
        GameObject modelo,
        GameObject grupoPines
    )
    {
        if (modelo == null)
            return;

        // Siempre cerramos cualquier panel informativo abierto
        // cuando cambia el sistema prioritario.
        CerrarTodosLosPaneles();

        // Siempre evitamos superposición de pines.
        DesactivarTodosLosPines();


        // =================================================
        // EL SISTEMA ACABA DE SER ACTIVADO
        // =================================================

        if (modelo.activeSelf)
        {
            // Si ya estaba en el historial,
            // lo eliminamos primero para poder moverlo
            // a la última posición.
            historialSistemas.Remove(modelo);

            // Ahora este sistema se convierte
            // en el último sistema seleccionado.
            historialSistemas.Add(modelo);
        }

        // =================================================
        // EL SISTEMA ACABA DE SER DESACTIVADO
        // =================================================

        else
        {
            historialSistemas.Remove(modelo);
        }


        // Eliminamos del historial cualquier sistema
        // que por alguna razón ya no esté activo.
        LimpiarHistorial();


        // Buscar cuál es ahora el último sistema activo.
        ActualizarSistemaActual();


        // Si el botón PIN está apagado,
        // no mostramos ningún pin.
        if (!pinesHabilitados)
            return;


        // Si existe un sistema actual,
        // mostramos únicamente SUS pines.
        if (modeloActual != null &&
            modeloActual.activeSelf &&
            pinesActuales != null)
        {
            pinesActuales.SetActive(true);
        }
    }


    // =====================================================
    // BOTÓN GENERAL PIN
    // =====================================================

    public void CambiarEstadoPines(bool estado)
    {
        pinesHabilitados = estado;

        CerrarTodosLosPaneles();
        DesactivarTodosLosPines();

        if (!pinesHabilitados)
            return;


        // Por seguridad actualizamos nuevamente
        // cuál es el sistema prioritario.
        LimpiarHistorial();
        ActualizarSistemaActual();


        if (modeloActual != null &&
            modeloActual.activeSelf &&
            pinesActuales != null)
        {
            pinesActuales.SetActive(true);
        }
    }


    // =====================================================
    // DETERMINAR SISTEMA PRIORITARIO
    // =====================================================

    private void ActualizarSistemaActual()
    {
        modeloActual = null;
        pinesActuales = null;


        // Si no hay ningún sistema activo,
        // terminamos aquí.
        if (historialSistemas.Count == 0)
            return;


        // El último sistema del historial
        // es el sistema con prioridad.
        modeloActual =
            historialSistemas[
                historialSistemas.Count - 1
            ];


        pinesActuales =
            ObtenerPinesDelModelo(modeloActual);
    }


    // =====================================================
    // OBTENER LOS PINES CORRESPONDIENTES
    // =====================================================

    private GameObject ObtenerPinesDelModelo(
        GameObject modelo
    )
    {
        if (modelo == sistemaCirculatorio)
            return pinesCirculatorio;

        if (modelo == sistemaEsqueletico)
            return pinesEsqueletico;

        if (modelo == sistemaMuscular)
            return pinesMuscular;

        if (modelo == sistemaDigestivo)
            return pinesDigestivo;

        if (modelo == sistemaNervioso)
            return pinesNervioso;

        if (modelo == sistemaRespiratorio)
            return pinesRespiratorio;

        return null;
    }


    // =====================================================
    // LIMPIAR SISTEMAS QUE YA NO ESTÁN ACTIVOS
    // =====================================================

    private void LimpiarHistorial()
    {
        for (int i = historialSistemas.Count - 1;
             i >= 0;
             i--)
        {
            GameObject sistema =
                historialSistemas[i];

            if (sistema == null ||
                !sistema.activeSelf)
            {
                historialSistemas.RemoveAt(i);
            }
        }
    }


    // =====================================================
    // APAGAR TODOS LOS PINES
    // =====================================================

    private void DesactivarTodosLosPines()
    {
        if (pinesCirculatorio != null)
            pinesCirculatorio.SetActive(false);

        if (pinesEsqueletico != null)
            pinesEsqueletico.SetActive(false);

        if (pinesMuscular != null)
            pinesMuscular.SetActive(false);

        if (pinesDigestivo != null)
            pinesDigestivo.SetActive(false);

        if (pinesNervioso != null)
            pinesNervioso.SetActive(false);

        if (pinesRespiratorio != null)
            pinesRespiratorio.SetActive(false);
    }


    // =====================================================
    // CERRAR TODOS LOS PANELES INFORMATIVOS
    // =====================================================

    private void CerrarTodosLosPaneles()
    {
        if (infoCirculatorio != null)
            infoCirculatorio.OcultarTodosLosPaneles();

        if (infoEsqueletico != null)
            infoEsqueletico.OcultarTodosLosPaneles();

        if (infoMuscular != null)
            infoMuscular.OcultarTodosLosPaneles();

        if (infoDigestivo != null)
            infoDigestivo.OcultarTodosLosPaneles();

        if (infoNervioso != null)
            infoNervioso.OcultarTodosLosPaneles();

        if (infoRespiratorio != null)
            infoRespiratorio.OcultarTodosLosPaneles();
    }
}