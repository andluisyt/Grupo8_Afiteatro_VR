using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinAnatomico : MonoBehaviour
{
    [Header("Panel de Información")]
    [Tooltip("Arrastra aquí el panel de información correspondiente")]
    public GameObject panelInformacion;
    
    void Start()
    {
        // Asegurar collider
        if (GetComponent<Collider>() == null)
        {
            SphereCollider collider = gameObject.AddComponent<SphereCollider>();
            collider.isTrigger = true;
        }
        
        // Si no se asignó panel, intentar buscar automáticamente
        if (panelInformacion == null)
        {
            BuscarPanelAutomaticamente();
        }
    }

    void OnMouseDown()
    {
        ActivarPanelInformacion();
    }

    void BuscarPanelAutomaticamente()
    {
        GameObject gestor = GameObject.Find("InformacionDePines");
        
        if (gestor != null)
        {
            // Buscar por nombre (fallback)
            panelInformacion = BuscarPanelRecursivo(gestor.transform, this.name);
        }
    }

    void ActivarPanelInformacion()
    {
        // Si no hay panel asignado, intentar buscar
        if (panelInformacion == null)
        {
            BuscarPanelAutomaticamente();
            
            if (panelInformacion == null)
            {
                Debug.LogWarning("✗ No se encontró panel para: " + this.name);
                return;
            }
        }

        // Desactivar TODOS los paneles primero
        GameObject gestor = GameObject.Find("InformacionDePines");
        if (gestor != null)
        {
            DesactivarTodosLosPaneles(gestor.transform);
        }

        // Activar SOLO este panel
        panelInformacion.SetActive(true);
        Debug.Log("✓ Panel activado: " + panelInformacion.name);
    }

    void DesactivarTodosLosPaneles(Transform padre)
    {
        foreach (Transform child in padre)
        {
            DesactivarRecursivo(child);
        }
    }

    void DesactivarRecursivo(Transform elemento)
    {
        elemento.gameObject.SetActive(false);
        
        foreach (Transform child in elemento)
        {
            DesactivarRecursivo(child);
        }
    }

    GameObject BuscarPanelRecursivo(Transform padre, string nombre)
    {
        foreach (Transform child in padre)
        {
            if (child.name == nombre)
            {
                return child.gameObject;
            }
            
            GameObject encontrado = BuscarPanelRecursivo(child, nombre);
            if (encontrado != null)
            {
                return encontrado;
            }
        }
        
        return null;
    }
}