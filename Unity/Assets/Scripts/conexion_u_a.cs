using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO.Ports;
using System.Linq;
using UnityEngine.EventSystems;

public class conexion_u_a : MonoBehaviour
{
    public GameObject UI;
    //Auto
    public GameObject carro;
    public float velocidadRotacion;
    public float velocidadAuto;
    float rotacion_carro = 0;
    public Renderer parabrisa;
    public Texture[] texturas_parabrisas;

    public float timeCronometroAuto;

    int input5viejo = 0;
    int total = 0;

    //Mano plum
    public GameObject mano;
    public Animator anim_mano;
    public bool empezarCronometro;
    public float manoCronometroTime;
    //Mira
    public float misilCooldown;
    public float misilCooldownTime;
    public bool empezarConteoCooldown = false;


    public float xJoystick;
    public float yJoystick;
    public int pressJoystick;
    public GameObject mira;
    public GameObject misil;
    public float velocidad;

    //CONEXION
    bool isConnected = false;
    SerialPort port;
    string[] ports;
    public Dropdown lista;
    string portname;

    //ENTRADAS
    public List<string> input;


    void DropdownItemSelected(Dropdown lista)
    {
        int indice = lista.value;
        portname = lista.options[indice].text;
    }

    public int String_TO_Int(string cadena)
    {
        int resul_final = 0;
        int longitu_palabra = cadena.Length;
        int[] arreglo_de_ints = new int[longitu_palabra];
        for (int i = 0; i < cadena.Length; i++)
        {
            arreglo_de_ints[i] = (int)char.GetNumericValue(cadena[i]);
            longitu_palabra -= 1;
            resul_final = ((int)Mathf.Pow(10, longitu_palabra)) * arreglo_de_ints[i] + resul_final;
        }
        return resul_final;
    }

    #region CONECTAR
    public void conectar()
    {
        if (!isConnected)
        {
            connect_to_Arduino();
        }
        UI.gameObject.SetActive(false);
    }

    void connect_to_Arduino()
    {
        isConnected = true;
        port = new SerialPort(portname, 9600, Parity.None, 8, StopBits.One);

        port.Open();
        port.Write("#STAR\n");
    }
    #endregion

    #region DESCONECTAR
    public void desconectar()
    {
        if (isConnected)
        {
            disconnect_from_Arduino();
        }

    }

    void disconnect_from_Arduino()
    {
        isConnected = false;
        port.Write("#STOP\n");
        port.Close();


    }
    #endregion

    private void Awake()
    {
        input5viejo = 0;
        input = new List<string>(3);
        lista.options.Clear();
        ports = SerialPort.GetPortNames();

        foreach (string port in ports)
        {
            lista.options.Add(new Dropdown.OptionData() { text = port });
        }

        DropdownItemSelected(lista);
        lista.onValueChanged.AddListener(delegate { DropdownItemSelected(lista); });
    }


    void Update()
    {
       
        if (isConnected)
        {
            string entradas = port.ReadLine();
            input = entradas.Split('#').ToList();
            mecanicaMira(input[0], input[1], input[2]);
            mecanicaMano(input[3]);
            mecanicaAuto(float.Parse(input[4]), input[2]);
            parabrisaFunc((int)float.Parse(input[5]));
            Ensuciar2Parabrisa();
        }

    }
    #region Mano
    public void mecanicaMano(string smash) 
    {
        if (smash == "SMASH" && manoCronometroTime<0)
        {
            empezarCronometro = true;
            manoCronometroTime = 2;
            anim_mano.SetTrigger("Mano");
        }
        contadorManoxd();
    }

    public void contadorManoxd()
    {
        if (manoCronometroTime>0)
        {
            manoCronometroTime -= Time.deltaTime;
            Debug.Log(manoCronometroTime);
        }

    }
    #endregion
    #region JUEGO_MIRA
    public void mecanicaMira(string p, string x, string y)
    {

        //JOYSTICK
        pressJoystick = String_TO_Int(p);
        xJoystick = ((float)String_TO_Int(x) / 1023) - 0.5f;
        yJoystick = ((float)String_TO_Int(y) / 1023) - 0.5f;

        if (Math.Abs(xJoystick) < 0.1)
        {
            xJoystick = 0;
        }

        if (Math.Abs(yJoystick) < 0.1)
        {
            yJoystick = 0;
        }

        mira.transform.position += new Vector3(-xJoystick * Time.deltaTime * velocidad, 0, yJoystick * Time.deltaTime * velocidad);
        mano.transform.position += new Vector3(-xJoystick * Time.deltaTime * velocidad, 0, yJoystick * Time.deltaTime * velocidad);

        if (pressJoystick == 0 && misilCooldown < 0)
        {
            misilCooldown = misilCooldownTime;
            empezarConteoCooldown = true;
            Instantiate(misil, mira.transform.position + new Vector3(0, 20, 0), Quaternion.Euler(0, 0, -180));
        }
        contadorCooldown();
    }
    public void contadorCooldown()
    {
        if (empezarConteoCooldown)
        {
            misilCooldown -= Time.deltaTime;
        }

    }
    #endregion
    public void mecanicaAuto(float rotacion,string joyStick) 
    {
        float yJoystick = ((float)String_TO_Int(joyStick) / 1023) - 0.5f;
        float rotacion_ = (rotacion / 180)-0.5f;
        if (Math.Abs(yJoystick) < 0.1)
        {
            yJoystick = 0;
        }
        carro.transform.position += carro.transform.TransformDirection(new Vector3(0, 0, yJoystick * Time.deltaTime * velocidadAuto));

        rotacion_carro -= rotacion_ * velocidadRotacion * Time.deltaTime;
        carro.transform.rotation = Quaternion.AngleAxis(rotacion_carro, Vector3.up);
    }

    public void parabrisaFunc(int rotacion2)
    {
        if (input5viejo != rotacion2)
        {
            if (input5viejo > rotacion2)
            {
                total += input5viejo - rotacion2;
            }
            else
            {
                total += rotacion2 - input5viejo;
            }
            input5viejo = rotacion2;
        }

        Debug.Log(total);

        if (total >= 360 && total < 370)
        {
            parabrisa.material.SetTexture("_MainTex", texturas_parabrisas[2]);
        }

        if (total >= 540 && total < 550)
        {
            parabrisa.material.SetTexture("_MainTex", texturas_parabrisas[1]);
        }

        if (total >= 720 && total < 730)
        {
            parabrisa.material.SetTexture("_MainTex", texturas_parabrisas[0]);
            input5viejo = 0;
            total = 0;
        }


    }

    public void Ensuciar2Parabrisa() 
    {
        timeCronometroAuto -= Time.deltaTime;

        if (timeCronometroAuto < 0)
        {
            parabrisa.material.SetTexture("_MainTex", texturas_parabrisas[3]);
            timeCronometroAuto =20;
        }
    }

}
