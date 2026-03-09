
using UnityEngine;
using UnityEngine.UI;

public class ActivarPanelBorrador : MonoBehaviour
{
    public GameObject panel_Terciario;
    public Image imagenBoton; // <- Button Image (HIJO)

    private readonly Color colorActivo = Color.white;
    private readonly Color colorGris = new Color(0.7f, 0.7f, 0.7f, 1f);

    void Start()
    {
        panel_Terciario.SetActive(false);
        imagenBoton.color = colorGris;
    }

    public void Activarpanel_Terciario()
    {
        bool activo = !panel_Terciario.activeSelf;
        panel_Terciario.SetActive(activo);

        imagenBoton.color = activo ? colorActivo : colorGris;
    }
}