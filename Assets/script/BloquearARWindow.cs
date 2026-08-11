using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloquearARWindow : MonoBehaviour
{
    [Header("Script que mueve el AR_Window")]
    public MonoBehaviour scriptMovimiento;

    [Header("Imagen del botón candado")]
    public Image imagenCandado;

    private bool bloqueado = false;

    private readonly Color32 colorBloqueado =
        new Color32(255, 255, 255, 255);

    private readonly Color32 colorDesbloqueado =
        new Color32(180, 180, 180, 255);

    private void Start()
    {
        bloqueado = false;

        if (scriptMovimiento != null)
            scriptMovimiento.enabled = true;

        if (imagenCandado != null)
            imagenCandado.color = colorDesbloqueado;
    }

    public void AlternarBloqueo()
    {
        bloqueado = !bloqueado;

        if (scriptMovimiento != null)
        {
            // Desbloqueado = sigue la cabeza
            // Bloqueado = permanece en su posición actual
            scriptMovimiento.enabled = !bloqueado;
        }

        if (imagenCandado != null)
        {
            imagenCandado.color =
                bloqueado
                ? colorBloqueado
                : colorDesbloqueado;
        }

        Debug.Log(
            bloqueado
                ? "AR_Window BLOQUEADO"
                : "AR_Window DESBLOQUEADO"
        );
    }
}
