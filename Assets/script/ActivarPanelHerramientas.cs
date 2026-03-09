using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivarPanelHerramientas : MonoBehaviour
{
    [Header("Modelos 3D")]
    public GameObject sistemaCirculatorio;
    public GameObject sistemaRespiratorio;
    public GameObject sistemaDigestivo;
    public GameObject sistemaMuscular;
    public GameObject sistemaNervioso;
    public GameObject sistemaEsqueletico;

    [Header("Paneles")]
    public GameObject panelHerramientas;
    public GameObject panelBorrador;

    [Header("Botones Sistemas")]
    public Image btnCirculatorio;
    public Image btnRespiratorio;
    public Image btnDigestivo;
    public Image btnMuscular;
    public Image btnNervioso;
    public Image btnEsqueletico;

    private readonly Color32 colorActivo = new Color32(255, 255, 255, 255);
    private readonly Color32 colorGris   = new Color32(180, 180, 180, 255);

    // 🔴 CLAVE: cada vez que se PRENDE el panel sistemas
    void OnEnable()
    {
        ResetearTodo();
    }

    // 🔴 CLAVE: cada vez que se APAGA el panel sistemas
    void OnDisable()
    {
        ResetearTodo();
    }

    void ResetearTodo()
    {
        // Apagar sistemas
        if (sistemaCirculatorio) sistemaCirculatorio.SetActive(false);
        if (sistemaRespiratorio) sistemaRespiratorio.SetActive(false);
        if (sistemaDigestivo) sistemaDigestivo.SetActive(false);
        if (sistemaMuscular) sistemaMuscular.SetActive(false);
        if (sistemaNervioso) sistemaNervioso.SetActive(false);
        if (sistemaEsqueletico) sistemaEsqueletico.SetActive(false);

        // Apagar paneles SIEMPRE
        if (panelHerramientas) panelHerramientas.SetActive(false);
        if (panelBorrador) panelBorrador.SetActive(false);

        // Botones a gris
        if (btnCirculatorio) btnCirculatorio.color = colorGris;
        if (btnRespiratorio) btnRespiratorio.color = colorGris;
        if (btnDigestivo) btnDigestivo.color = colorGris;
        if (btnMuscular) btnMuscular.color = colorGris;
        if (btnNervioso) btnNervioso.color = colorGris;
        if (btnEsqueletico) btnEsqueletico.color = colorGris;
    }

    bool HaySistemasActivos()
    {
        return (sistemaCirculatorio && sistemaCirculatorio.activeSelf) ||
               (sistemaRespiratorio && sistemaRespiratorio.activeSelf) ||
               (sistemaDigestivo && sistemaDigestivo.activeSelf) ||
               (sistemaMuscular && sistemaMuscular.activeSelf) ||
               (sistemaNervioso && sistemaNervioso.activeSelf) ||
               (sistemaEsqueletico && sistemaEsqueletico.activeSelf);
    }

    void ActualizarPaneles()
    {
        bool activos = HaySistemasActivos();

        if (panelHerramientas)
            panelHerramientas.SetActive(activos);

        // si no hay sistemas, el borrador NUNCA queda activo
        if (!activos && panelBorrador)
            panelBorrador.SetActive(false);
    }

    // =========================
    // TOGGLES
    // =========================
    public void ToggleCirculatorio()
    {
        sistemaCirculatorio.SetActive(!sistemaCirculatorio.activeSelf);
        btnCirculatorio.color = sistemaCirculatorio.activeSelf ? colorActivo : colorGris;
        ActualizarPaneles();
    }

    public void ToggleRespiratorio()
    {
        sistemaRespiratorio.SetActive(!sistemaRespiratorio.activeSelf);
        btnRespiratorio.color = sistemaRespiratorio.activeSelf ? colorActivo : colorGris;
        ActualizarPaneles();
    }

    public void ToggleDigestivo()
    {
        sistemaDigestivo.SetActive(!sistemaDigestivo.activeSelf);
        btnDigestivo.color = sistemaDigestivo.activeSelf ? colorActivo : colorGris;
        ActualizarPaneles();
    }

    public void ToggleMuscular()
    {
        sistemaMuscular.SetActive(!sistemaMuscular.activeSelf);
        btnMuscular.color = sistemaMuscular.activeSelf ? colorActivo : colorGris;
        ActualizarPaneles();
    }

    public void ToggleNervioso()
    {
        sistemaNervioso.SetActive(!sistemaNervioso.activeSelf);
        btnNervioso.color = sistemaNervioso.activeSelf ? colorActivo : colorGris;
        ActualizarPaneles();
    }

    public void ToggleEsqueletico()
    {
        sistemaEsqueletico.SetActive(!sistemaEsqueletico.activeSelf);
        btnEsqueletico.color = sistemaEsqueletico.activeSelf ? colorActivo : colorGris;
        ActualizarPaneles();
    }
}