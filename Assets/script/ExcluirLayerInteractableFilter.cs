using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class ExcluirLayerInteractableFilter : MonoBehaviour, IGameObjectFilter
{
    [Header("Layers que este Interactor NO puede usar")]
    [SerializeField]
    private LayerMask layersBloqueadas;

    public bool Filter(GameObject gameObject)
    {
        if (gameObject == null)
            return false;

        bool estaBloqueada =
            (layersBloqueadas.value & (1 << gameObject.layer)) != 0;

        // true  = permitir
        // false = bloquear
        return !estaBloqueada;
    }
}