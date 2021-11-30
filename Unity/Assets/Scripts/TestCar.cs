using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCar : MonoBehaviour
{
    float rotacion_carro = 0;
    public float velocidadRotacion;
    public float velocidad;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += transform.TransformDirection(new Vector3(0, 0, Input.GetAxis("Vertical") * Time.deltaTime * velocidad));
        if (Input.GetKey(KeyCode.D))
        {
            rotacion_carro = rotacion_carro + Input.GetAxis("Horizontal")*velocidadRotacion*Time.deltaTime;
            this.transform.rotation = Quaternion.AngleAxis(rotacion_carro, Vector3.up);

        }

        if (Input.GetKey(KeyCode.A))
        {
            rotacion_carro = rotacion_carro + Input.GetAxis("Horizontal") * velocidadRotacion * Time.deltaTime;
            this.transform.rotation = Quaternion.AngleAxis(rotacion_carro, Vector3.up);

        }


    }
}
