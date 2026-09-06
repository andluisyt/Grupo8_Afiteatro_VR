using Oculus.Interaction.Locomotion;
using UnityEngine;

public class CalibracionAlturaUsuario : MonoBehaviour
{
    [Header("Locomoción del usuario")]
    [SerializeField] private FirstPersonLocomotor locomotor;

    [Header("Límites del ajuste en metros")]
    [Tooltip("Debe ser menor o igual a cero.")]
    [SerializeField] private float alturaMinima = -0.30f;

    [Tooltip("Debe ser mayor o igual a cero.")]
    [SerializeField] private float alturaMaxima = 0.50f;

    private const float PasoAltura = 0.05f;

    [Header("Vista elevada temporal")]
    [SerializeField] private float elevacionVistaSuperior = 0.20f;

    private bool vistaElevadaActiva;
    private float alturaBase;

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
            AlternarVistaElevada();
    }

    private void OnDisable()
    {
        SalirDeVistaElevada();
    }

    private void AlternarVistaElevada()
    {
        if (!TieneLocomotor())
            return;

        if (vistaElevadaActiva)
        {
            SalirDeVistaElevada();
            return;
        }

        if (!LimitesValidos())
            return;

        if (!EsFinito(elevacionVistaSuperior) || elevacionVistaSuperior <= 0f ||
            !EsFinito(locomotor.HeightOffset))
        {
            Debug.LogWarning(
                "Calibración de altura: la elevación debe ser finita y positiva, y HeightOffset debe ser válido.",
                this
            );
            return;
        }

        alturaBase = locomotor.HeightOffset;
        float alturaElevada = Mathf.Clamp(
            alturaBase + elevacionVistaSuperior, alturaMinima, alturaMaxima
        );

        if (alturaElevada == alturaBase)
            return;

        locomotor.HeightOffset = alturaElevada;
        vistaElevadaActiva = true;
    }

    private void SalirDeVistaElevada()
    {
        if (!vistaElevadaActiva)
            return;

        if (locomotor != null)
            locomotor.HeightOffset = alturaBase;

        vistaElevadaActiva = false;
    }

    public void SubirAltura()
    {
        AjustarAltura(PasoAltura);
    }

    public void BajarAltura()
    {
        AjustarAltura(-PasoAltura);
    }

    public void RestablecerAltura()
    {
        SalirDeVistaElevada();

        if (!TieneLocomotor())
            return;

        locomotor.HeightOffset = 0f;
    }

    private void AjustarAltura(float incremento)
    {
        SalirDeVistaElevada();

        if (!TieneLocomotor())
            return;

        if (!LimitesValidos())
            return;

        float alturaActual = locomotor.HeightOffset;
        if (!EsFinito(alturaActual))
        {
            Debug.LogWarning(
                "Calibración de altura: HeightOffset no es válido. Utiliza RestablecerAltura.",
                this
            );
            return;
        }

        locomotor.HeightOffset = Mathf.Clamp(
            alturaActual + incremento, alturaMinima, alturaMaxima
        );
    }

    private bool LimitesValidos()
    {
        // Incluir cero permite restablecer sin salir de los límites configurados.
        if (EsFinito(alturaMinima) && EsFinito(alturaMaxima) &&
            alturaMinima <= 0f && alturaMaxima >= 0f)
            return true;

        Debug.LogWarning(
            "Calibración de altura: configura límites finitos con mínimo <= 0 y máximo >= 0.",
            this
        );
        return false;
    }

    private bool TieneLocomotor()
    {
        if (locomotor != null)
            return true;

        Debug.LogWarning(
            "Calibración de altura: asigna FirstPersonLocomotor en el Inspector.",
            this
        );
        return false;
    }

    private static bool EsFinito(float valor)
    {
        return !float.IsNaN(valor) && !float.IsInfinity(valor);
    }
}
