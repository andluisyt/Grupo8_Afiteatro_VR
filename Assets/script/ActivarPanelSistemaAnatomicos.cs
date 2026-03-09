
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivarPanelSistemaAnatomicos : MonoBehaviour
{
    // =========================
    // PANELES
    // =========================
    public GameObject panel_explorador;
    public GameObject panel_sistemas_anatomicos;
    public GameObject panelHerramientas;
    public GameObject panelBorrador;

    // =========================
    // MODELOS 3D
    // =========================
    public GameObject sistemaCirculatorio;
    public GameObject sistemaEsqueletico;
    public GameObject sistemaMuscular;
    public GameObject sistemaDigestivo;
    public GameObject sistemaNervioso;
    public GameObject sistemaRespiratorio;

    // =========================
    // BOTONES
    // =========================
    public Image explorerButtonImage;
    public Image herramientasButtonImage;

    void Start()
    {
        ApagarTodos();

        explorerButtonImage.color = new Color32(180, 180, 180, 255);
        herramientasButtonImage.color = new Color32(180, 180, 180, 255);
    }

    // =========================
    // APAGAR TODO
    // =========================
    void ApagarTodos()
    {
        panel_sistemas_anatomicos.SetActive(false);
        panelHerramientas.SetActive(false);
        panelBorrador.SetActive(false);
    }

    // =========================
    // BOTÓN EXPLORER
    // =========================
    public void ActivarPanelAnatomico()
    {
        if (panel_sistemas_anatomicos.activeSelf)
        {
            panel_sistemas_anatomicos.SetActive(false);
            explorerButtonImage.color = new Color32(180, 180, 180, 255);

            panelHerramientas.SetActive(false);
            panelBorrador.SetActive(false);
        }
        else
        {
            panel_sistemas_anatomicos.SetActive(true);
            explorerButtonImage.color = Color.white;

            // =========================
            // CONDICIÓN 1:
            // Si algún modelo 3D está activo → mostrar panel herramientas
            // =========================
            bool modeloActivo =
                sistemaCirculatorio.activeSelf ||
                sistemaEsqueletico.activeSelf ||
                sistemaMuscular.activeSelf ||
                sistemaDigestivo.activeSelf ||
                sistemaNervioso.activeSelf ||
                sistemaRespiratorio.activeSelf;

            panelHerramientas.SetActive(modeloActivo);

            // =========================
            // CONDICIÓN 2:
            // Si el botón herramientas está blanco → mostrar panel borrador
            // =========================
            panelBorrador.SetActive(
                herramientasButtonImage.color == Color.white
            );
        }
    }
}
