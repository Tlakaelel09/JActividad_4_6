using System.Collections.Generic;
using UnityEngine;

public class GeneradorNiveles3D : MonoBehaviour
{
    [Header("Referencia del Jugador")]
    public Transform jugador;

    [Header("Configuración del Nivel (Editable en Inspector)")]
    public float largoModulo = 15f;
    public float anchoModulo = 10f;
    public int modulosVisibles = 5;

    [Header("Dificultad Progresiva")]
    public int deathPartsIniciales = 2;
    public int maxDeathPartsPorModulo = 8;
    public int objetivosPorModulo = 2;

    private float siguienteZ = 0f;
    private int nivelActual = 0;
    private Queue<GameObject> modulosActivos = new Queue<GameObject>();

    void Start()
    {

        for (int i = 0; i < modulosVisibles; i++)
        {
            GenerarModulo(i == 0);
        }
    }

    void Update()
    {
        if (jugador == null) return;

        if (jugador.position.z + (largoModulo * (modulosVisibles - 2)) > siguienteZ)
        {
            GenerarModulo(false);
            EliminarModuloAntiguo();
        }
    }

    void GenerarModulo(bool esZonaSegura)
    {
        GameObject modulo = new GameObject($"Modulo_Nivel_{nivelActual}");
        modulo.transform.position = new Vector3(0f, 0f, siguienteZ);

        GameObject piso = GameObject.CreatePrimitive(PrimitiveType.Cube);
        piso.name = "Piso";
        piso.transform.SetParent(modulo.transform);
        piso.transform.localPosition = Vector3.zero;
        piso.transform.localScale = new Vector3(anchoModulo, 1f, largoModulo);
        piso.GetComponent<Renderer>().material.color = (nivelActual % 2 == 0) ? new Color(0.75f, 0.75f, 0.75f) : new Color(0.6f, 0.6f, 0.65f);

        if (!esZonaSegura)
        {

            int cantidadDeathParts = Mathf.Min(deathPartsIniciales + (nivelActual / 2), maxDeathPartsPorModulo);

            for (int i = 0; i < cantidadDeathParts; i++)
            {
                GameObject trampa = GameObject.CreatePrimitive(PrimitiveType.Cube);
                trampa.name = "DeathPart";
                trampa.transform.SetParent(modulo.transform);
                trampa.transform.localScale = new Vector3(1.4f, 1f, 1.4f);

                float posX = Random.Range(-(anchoModulo / 2f) + 1f, (anchoModulo / 2f) - 1f);
                float posZ = Random.Range(-(largoModulo / 2f) + 1.5f, (largoModulo / 2f) - 1.5f);
                trampa.transform.localPosition = new Vector3(posX, 1f, posZ);

                trampa.GetComponent<Renderer>().material.color = Color.red;
                trampa.AddComponent<DeathPart>();
            }

            for (int i = 0; i < objetivosPorModulo; i++)
            {
                GameObject objetivo = GameObject.CreatePrimitive(PrimitiveType.Cube);
                objetivo.name = "Objetivo";
                objetivo.transform.SetParent(modulo.transform);
                objetivo.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

                float posX = Random.Range(-(anchoModulo / 2f) + 1f, (anchoModulo / 2f) - 1f);
                float posZ = Random.Range(-(largoModulo / 2f) + 1.5f, (largoModulo / 2f) - 1.5f);
                objetivo.transform.localPosition = new Vector3(posX, 1f, posZ);

                objetivo.GetComponent<BoxCollider>().isTrigger = true;
                objetivo.GetComponent<Renderer>().material.color = Color.yellow;
                objetivo.AddComponent<Objetivo3D>();
            }
        }

        modulosActivos.Enqueue(modulo);
        siguienteZ += largoModulo;
        nivelActual++;
    }

    void EliminarModuloAntiguo()
    {
        if (modulosActivos.Count > modulosVisibles)
        {
            GameObject moduloViejo = modulosActivos.Dequeue();
            Destroy(moduloViejo);
        }
    }
}