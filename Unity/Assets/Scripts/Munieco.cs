using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Munieco : MonoBehaviour
{
    public GameObject mainCamara;
    public int nivel = 1;
    //MANO
    public GameObject Nivel2;
    public bool Nivel2activado = false;
    //UI 
    public GameObject UI;
    //MIRA
    public GameObject Nivel1;
    public bool Nivel1activado = false;

    //Auto
    public GameObject Nivel3;
    public bool Nivel3activado = false;

    public float timeToCalculatePoints;
    public List<Vector3> tarjetPoints;
    public int numberPoints;
    public int indiceTarjet = 0;
    public float velocidad;
    public float distanciaMinimaParaCambioTarjet;
    public float distanciaMinimaEntreTarjets;
    public int vida;
    public Animator cortinas_anim;
    void Start()
    {
        reestablecerVida(4);
        calculatePoints();
    }

    void Update()
    {
        if (vida == 0)
        {
            cortinas_anim.SetTrigger("Corti");
            nivel++;
            reestablecerVida(2);
        }

        Moves();

        if (nivel == 1 && Nivel1activado == false) 
        {
            Nivel1.gameObject.SetActive(true);
            Nivel1activado = true;
        }

        if (nivel == 2 && Nivel2activado == false)
        {
            Nivel2.gameObject.SetActive(true);
            Nivel1.gameObject.SetActive(false);
            velocidad = 150;
            distanciaMinimaEntreTarjets = 50;
            Nivel2activado = true;
        }

        if (nivel == 3 && Nivel3activado == false)
        {
            mainCamara.gameObject.SetActive(false);
            Nivel2.gameObject.SetActive(false);
            Nivel3.gameObject.SetActive(true);
            velocidad = 35;
            distanciaMinimaEntreTarjets = 70;
            Nivel3activado = true;
        }

        if (nivel == 4) 
        {
            UI.gameObject.SetActive(true);
            Destroy(this.gameObject);
        }

    }
    public void Moves() 
    {
        float step = velocidad * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, tarjetPoints[indiceTarjet], step);
        transform.LookAt(tarjetPoints[indiceTarjet]);
        if (Vector3.Distance(transform.position, tarjetPoints[indiceTarjet]) < distanciaMinimaParaCambioTarjet)
        {
            indiceTarjet++;
            if (indiceTarjet == numberPoints)
            {
                calculatePoints();
                indiceTarjet = 0;
            }
        }
    }
    public void calculatePoints() 
    {
        tarjetPoints.Clear();
        for (int i = 0; i < numberPoints; i++)
        {
            tarjetPoints.Add(new Vector3(Random.Range(-distanciaMinimaEntreTarjets, distanciaMinimaEntreTarjets), 0, Random.Range(-distanciaMinimaEntreTarjets, distanciaMinimaEntreTarjets)));
        }        
    }
    public void reestablecerVida(int parametr) 
    {
        this.vida = parametr;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning(other.gameObject.tag);
        if (other.gameObject.tag == "RANGO") 
        {
            vida -= 1;
        }

        if (other.gameObject.tag == "MANO")
        {
            vida -= 1;
        }

        if (other.gameObject.tag == "AUTO")
        {
            this.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 50, 0),ForceMode.Impulse);
            vida -= 1;
        }
    }
}
