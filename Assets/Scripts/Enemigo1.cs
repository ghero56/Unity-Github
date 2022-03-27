using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo1 : MonoBehaviour
{
    //Para controlar su cuerpo rigido
    private Rigidbody2D rb;

    public float movHor = 0f;
    public float speed = 3;

    //Para saber si esta tocando el piso
    public bool isGroundFloor = true;
    public bool isGroundFront = false;

    public LayerMask groundLayer;
    public float frontGrndRayDist = 0.25f;
    public float floorCheckY = 0.52f;
    public float frontCheck = 0.51f;
    public float frontDist = 0.001f;

    public int scoreGive = 50;

    private RaycastHit2D hit;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Evitar caer en precipicio
        isGroundFloor = (Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - floorCheckY, transform.position.z),
            new Vector3(movHor, 0, 0), frontGrndRayDist, groundLayer));

        if (!isGroundFloor)
        {
            movHor = movHor * -1;
        }

        //choque con pared
        if (Physics2D.Raycast(transform.position, new Vector3(movHor, 0, 0), frontCheck, groundLayer))
        {
            movHor = movHor * -1;
        }

        //choque con otro enemigo
        hit = Physics2D.Raycast(new Vector3(transform.position.x + movHor * frontCheck, transform.position.y, transform.position.z),
            new Vector3(movHor, 0, 0), frontDist);

        if (hit != null)
        {
            if (hit.transform != null)
            {
                if (hit.transform.CompareTag("Enemigo1"))
                {
                    movHor = movHor * -1;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(movHor * speed, rb.velocity.y);
    }

    //Daña al personaje
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player._objeto.DamageVidas();
            Debug.Log("Jugador perdio una vida");
        }
    }

    //el enemigo se destruye
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EnemigoEmilinado();
            Debug.Log("Enemigo eliminado");
        }
    }

    private void EnemigoEmilinado()
    {
        gameObject.SetActive(false);
    }
}
