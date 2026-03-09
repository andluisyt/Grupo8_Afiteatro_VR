using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinInteractivo : MonoBehaviour
{
    [TextArea] public string informacion;

    void OnMouseDown()
    {
        // Requisito del profesor: Uso de Tag
        if (CompareTag("Pin")) 
        {
            GestorPines.Instancia.MostrarInfo(informacion, this);
        }
    }
}