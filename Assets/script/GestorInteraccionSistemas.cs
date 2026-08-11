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

    // Último sistema seleccionado
    private GameObject modeloActual;
    private GameObject pinesActuales;

    // Estado general del botón PIN
    private bool pinesHabilitados = false;


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


    private void SeleccionarSistema(
        GameObject modelo,
        GameObject grupoPines
    )
    {
        CerrarTodosLosPaneles();
        DesactivarTodosLosPines();

        // Si el botón del sistema acaba de ENCENDER el modelo,
        // pasa a ser el sistema interactuable actual.
        if (modelo != null && modelo.activeSelf)
        {
            modeloActual = modelo;
            pinesActuales = grupoPines;

            if (pinesHabilitados &&
                pinesActuales != null)
            {
                pinesActuales.SetActive(true);
            }
        }
        else
        {
            // Si se apagó precisamente el sistema actual,
            // también deja de ser interactuable.
            if (modeloActual == modelo)
            {
                modeloActual = null;
                pinesActuales = null;
            }
        }
    }


    // Lo llamará el botón general PIN.
    public void CambiarEstadoPines(bool estado)
    {
        pinesHabilitados = estado;

        CerrarTodosLosPaneles();
        DesactivarTodosLosPines();

        if (!pinesHabilitados)
            return;

        if (modeloActual != null &&
            modeloActual.activeSelf &&
            pinesActuales != null)
        {
            pinesActuales.SetActive(true);
        }
    }


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