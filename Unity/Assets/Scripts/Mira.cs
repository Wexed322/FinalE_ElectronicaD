using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mira : MonoBehaviour
{
    public List<GameObject> particulas;
    void Start()
    {
        particulas = new List<GameObject>();
    }
    void Update()
    {
        if (particulas.Count > 0) 
        {
            for (int i = 0; i < particulas.Count; i++)
            {
                Destroy(particulas[i].gameObject,1.5f);
            }
            particulas.Clear();
            
        }
    }
}
