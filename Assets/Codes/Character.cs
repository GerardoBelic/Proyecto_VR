using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterController personajeControl;
    public Vector3 velocidadPersonaje;
    private bool estaEnPiso;
    public float gravedad, aceleracionPersonaje, fuerzaSalto;

    public bool canMove = false;

    void Start()
    {
        personajeControl = GetComponent<CharacterController>();
        canMove = false;
    }

    void Update()
    {

        if (canMove)
            muevePersonaje();

        mueveCamara();            
            
    }

    private void muevePersonaje()
    {//print("moviendo");
        /// Posicionamiento del personaje sobre el suelo
        estaEnPiso = personajeControl.isGrounded;
        if (estaEnPiso && velocidadPersonaje.y < 0)
            velocidadPersonaje.y = -0.01f;

        Vector3 mueve = Vector3.zero;

        /// Movimiento del personaje
        //mueve.x += Input.GetAxis("Horizontal") * Time.deltaTime * 2.0f;
        //mueve.z += Input.GetAxis("Vertical") * Time.deltaTime * 2.0f;

        mueve.x += Input.GetAxis("LS_h") * Time.deltaTime * 2.0f;
        mueve.z -= Input.GetAxis("LS_v") * Time.deltaTime * 2.0f;

        //mueve = Vector3.Normalize(mueve);
        mueve = Vector3.ClampMagnitude(mueve, Time.deltaTime * 2.0f);

        /// Rotamos el movimiento de acuerdo a donde miremos
        //mueve = Quaternion.AngleAxis(transform.rotation.eulerAngles.y/* + transform.Find("Main Camera").transform.rotation.eulerAngles.y*/, Vector3.up) * mueve;
        mueve = Quaternion.AngleAxis(transform.Find("Main Camera").transform.rotation.eulerAngles.y, Vector3.up) * mueve;
        /*print("player: " + transform.rotation.eulerAngles.y);
        print("camera: " + transform.Find("Main Camera").transform.rotation.eulerAngles.y);*/
       

        /// Saltar
        if ((Input.GetButtonDown("A") ) && estaEnPiso)
            velocidadPersonaje.y += fuerzaSalto;

        /// Aplicar movimiento
        personajeControl.Move(mueve * aceleracionPersonaje);

        velocidadPersonaje.y += gravedad * Time.deltaTime * Time.deltaTime;
        personajeControl.Move(velocidadPersonaje);

    }

    private void mueveCamara()
    {
        var rot = transform.rotation.eulerAngles;
        rot.y += Input.GetAxis("RS_h") * Time.deltaTime * 150.0f;
        transform.rotation = Quaternion.Euler(rot);

        rot = transform.rotation.eulerAngles;
        rot.x += Input.GetAxis("RS_v") * Time.deltaTime * 150.0f;
        transform.rotation = Quaternion.Euler(rot);
    }

}