using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PinButtonSistema : MonoBehaviour
{
    [Header("Imagen del botón PIN")]
    public Image buttonImage;

    [Header("Referencia al gestor de sistemas")]
    public ActivarPanelSistemaAnatomicos sistemaManager;

    [Header("Pines por sistema")]
    public GameObject pinesCirculatorio;
    public GameObject pinesEsqueletico;
    public GameObject pinesMuscular;
    public GameObject pinesDigestivo;
    public GameObject pinesNervioso;
    public GameObject pinesRespiratorio;

    [Header("Estado del botón")]
    public bool pinActivo = false; // ← ESTE es el booleano real

    private readonly Color32 colorOn  = new Color32(255, 255, 255, 255);
    private readonly Color32 colorOff = new Color32(180, 180, 180, 255);

    void Start()
    {
        pinActivo = false;
        buttonImage.color = colorOff;
        DesactivarTodosLosPines();
    }

    public void OnPress()
    {
        pinActivo = !pinActivo;  // ← Toggle real

        if (pinActivo)
        {
            buttonImage.color = colorOn;
            ActivarPinesSegunSistema();
        }
        else
        {
            buttonImage.color = colorOff;
            DesactivarTodosLosPines();
        }
    }

    void ActivarPinesSegunSistema()
    {
        if (!pinActivo) return;

        DesactivarTodosLosPines();

        if (sistemaManager == null) return;

        if (sistemaManager.sistemaCirculatorio.activeSelf && pinesCirculatorio)
            pinesCirculatorio.SetActive(true);

        if (sistemaManager.sistemaEsqueletico.activeSelf && pinesEsqueletico)
            pinesEsqueletico.SetActive(true);

        if (sistemaManager.sistemaMuscular.activeSelf && pinesMuscular)
            pinesMuscular.SetActive(true);

        if (sistemaManager.sistemaDigestivo.activeSelf && pinesDigestivo)
            pinesDigestivo.SetActive(true);

        if (sistemaManager.sistemaNervioso.activeSelf && pinesNervioso)
            pinesNervioso.SetActive(true);

        if (sistemaManager.sistemaRespiratorio.activeSelf && pinesRespiratorio)
            pinesRespiratorio.SetActive(true);
    }

    void DesactivarTodosLosPines()
    {
        if (pinesCirculatorio) pinesCirculatorio.SetActive(false);
        if (pinesEsqueletico) pinesEsqueletico.SetActive(false);
        if (pinesMuscular) pinesMuscular.SetActive(false);
        if (pinesDigestivo) pinesDigestivo.SetActive(false);
        if (pinesNervioso) pinesNervioso.SetActive(false);
        if (pinesRespiratorio) pinesRespiratorio.SetActive(false);
    }
}