using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
    public static Player _objeto;

    public int _vidas=3;
    public bool _enPiso=false; 
    public bool _enMovimiento=false;
    public bool _inmune=false;

    public float _velocidad = 5f;
    public float  _fuerzaSalto= 3f;
    public float  _movHorizontal= 3f;

    public float  _inmuneCont= 0f;
    public float  _inmuneTiempo = 0.5f;

    public LayerMask _capaPiso;
    public float _radio=0.3f;
    public float _distanciaRayo = 0.5f;

    private Rigidbody2D _rb;
    private Animator _animacion;
    private SpriteRenderer _spr;

    void Awake()
    {
        _objeto = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animacion=GetComponent<Animator>();
        _spr=GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Movimiento de las declas de movhor
        _movHorizontal = Input.GetAxisRaw("Horizontal");
        //Verifica si esta en movivmiento
        _enMovimiento = (_movHorizontal != 0f);
        //regresa un f o t si toca piso
        _enPiso = Physics2D.CircleCast(transform.position, _radio, Vector3.down, _distanciaRayo, _capaPiso);
        //input de salto por teclado
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Saltar();
        }

        _animacion.SetBool("_enMovimiento", _enMovimiento);
        _animacion.SetBool("_enPiso", _enPiso);

        AminacionEspejoVuelta(_movHorizontal);
    }

    //animacion de vuelta
    public void AminacionEspejoVuelta(float _xValue)
    {
        Vector3 escala = transform.localScale;
        if (_xValue < 0)
        {
            escala.x = Mathf.Abs(escala.x) * -1;
        }
        else if(_xValue >0)
        {
            escala.x = Mathf.Abs(escala.x);
        }

        transform.localScale = escala;
    }

    public void Saltar()
    {
        //Para saber si esta en el salto
        if (!_enPiso)
        {
            return;
        }
        //Para darle impulso al salto
        _rb.velocity = Vector2.up * _fuerzaSalto;


    }

    //Metodo para fisicas en unity
    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(_movHorizontal * _velocidad, _rb.velocity.y);
    }

    public void DamageVidas()
    {
        _vidas--;

        if (_vidas<=0)
        {
            this.gameObject.gameObject.SetActive(false);
            Debug.Log("Jugador Eliminado");
        }
    }

    void OnDestroy()
    {
        _objeto = null;
    }

}
