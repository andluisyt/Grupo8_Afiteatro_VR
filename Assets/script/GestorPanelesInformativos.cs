using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestorPanelesInformativos : MonoBehaviour
{
    private GameObject panelActual;
    private PinPanelInformativo pinActual;

    private void Start()
    {
        OcultarTodosLosPaneles();
    }

    public void AlternarPanel(
        GameObject panelNuevo,
        PinPanelInformativo pinNuevo
    )
    {
        if (panelNuevo == null || pinNuevo == null)
        {
            Debug.LogWarning(
                "No se asignó correctamente el panel o el pin."
            );

            return;
        }

        // Si vuelve a detectar el mismo pin,
        // cierra el panel que ya estaba visible.
        if (panelActual == panelNuevo && pinActual == pinNuevo)
        {
            panelActual.SetActive(false);

            panelActual = null;
            pinActual = null;

            return;
        }

        // Si había otro panel abierto, lo cierra.
        if (panelActual != null)
        {
            panelActual.SetActive(false);
        }

        // Abre el panel del nuevo pin.
        panelActual = panelNuevo;
        pinActual = pinNuevo;

        panelActual.SetActive(true);
    }

    public void CerrarPanelActual()
    {
        if (panelActual != null)
        {
            panelActual.SetActive(false);
        }

        panelActual = null;
        pinActual = null;
    }

    public void CerrarSiEsActual(GameObject panel)
    {
        if (panelActual == panel)
        {
            CerrarPanelActual();
        }
    }

    public void OcultarTodosLosPaneles()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        panelActual = null;
        pinActual = null;
    }
}
