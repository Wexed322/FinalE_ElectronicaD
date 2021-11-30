using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Misiil : MonoBehaviour
{
    public GameObject rango_Explosion;
    StressReceiver shake;
    public GameObject explosion;
    Mira instancia_mira;

    void Start()
    {
        shake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<StressReceiver>();
        instancia_mira = GameObject.FindGameObjectWithTag("Mira").GetComponent<Mira>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "piso")
        {
            shake._trauma = 2;
            Debug.Log("PUMM");
            Instantiate(rango_Explosion, this.transform.position, Quaternion.identity);
            GameObject nueeee = Instantiate(explosion, this.transform.position, Quaternion.identity);
            instancia_mira.particulas.Add(nueeee);
            Destroy(this.gameObject);
        }
    }
}
