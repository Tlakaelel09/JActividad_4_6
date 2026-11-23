using UnityEngine;

public class Objetivo3D : MonoBehaviour
{
    public int puntos = 10;

    void Update()
    {
        transform.Rotate(Vector3.up * 90f * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GestorPuntaje3D.instancia != null)
            {
                GestorPuntaje3D.instancia.SumarPuntos(puntos);
            }
            Destroy(gameObject);
        }
    }
}