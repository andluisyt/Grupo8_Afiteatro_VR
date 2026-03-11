using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinAnatomico : MonoBehaviour
{
    [Header("Referencia al Panel")]
    [Tooltip("Nombre exacto del panel a activar (ej: InfoDeArteriaCarotidaComunIzquierda)")]
    public string nombrePanelInfo;
    
    void Start()
    {
        // Asegurar collider
        if (GetComponent<Collider>() == null)
        {
            SphereCollider collider = gameObject.AddComponent<SphereCollider>();
            collider.isTrigger = true;
        }
        
        // Si no se asignó nombre, usar el nombre del objeto
        if (string.IsNullOrEmpty(nombrePanelInfo))
        {
            nombrePanelInfo = this.name;
        }
    }

    void OnMouseDown()
    {
        ActivarPanelInformacion();
    }

    void ActivarPanelInformacion()
    {
        GameObject gestor = GameObject.Find("InformacionDePines");
        
        if (gestor == null)
        {
            Debug.LogError("No se encontró InformacionDePines");
            return;
        }

        // Desactivar TODOS los paneles de TODOS los sistemas
        DesactivarTodosLosPaneles(gestor.transform);

        // Buscar y activar el panel específico
        GameObject panelActivar = BuscarPanelRecursivo(gestor.transform, nombrePanelInfo);

        if (panelActivar != null)
        {
            panelActivar.SetActive(true);
            Debug.Log("✓ Panel activado: " + nombrePanelInfo);
        }
        else
        {
            Debug.LogWarning("✗ No se encontró el panel: " + nombrePanelInfo);
        }
    }

    void DesactivarTodosLosPaneles(Transform padre)
    {
        foreach (Transform child in padre)
        {
            // Desactivar hijos directos (los sistemas)
            child.gameObject.SetActive(false);
            
            // O si quieres desactivar cada panel individual dentro de los sistemas:
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
        // Buscar en hijos directos
        foreach (Transform child in padre)
        {
            if (child.name == nombre)
            {
                return child.gameObject;
            }
            
            // Buscar recursivamente en los hijos
            GameObject encontrado = BuscarPanelRecursivo(child, nombre);
            if (encontrado != null)
            {
                return encontrado;
            }
        }
        
        return null;
    }
}