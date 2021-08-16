using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    public Animator anima;
    float xmov;
    public Rigidbody2D rdb;
    bool jump,doublejump;
    float jumptime, jumptimeside;
    public ParticleSystem fire;
    void Start()
    {

    }
    void Update()
    {
        xmov = Input.GetAxis("Horizontal"); // Não sei se entendi aqui
        if (Input.GetButtonDown("Jump"))
        {
            if (jumptime < 0.1f)
            {
                doublejump = true;
            }
        }

        if (Input.GetButton("Jump"))
        {
            jump = true;
        }
        else
        {
            jump = false;
            doublejump = false;
            jumptime = 0;
            jumptimeside = 0;
        }
        anima.SetBool("Fire", false);

        if (Input.GetButtonDown("Fire1"))
        {
            fire.Emit(1);
            anima.SetBool("Fire", true);
        }

    }
   
    void FixedUpdate()
    {
        Reverser();
        anima.SetFloat("Velocity", Mathf.Abs(xmov));
        //rdb.velocity = new Vector2(xmov * 1.3f, rdb.velocity.y);

        rdb.AddForce(new Vector2(xmov * 20/(rdb.velocity.magnitude+1), 0)); // Velocidade da personagem na plataforma

        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, Vector2.down);
        if (hit)
        {
            anima.SetFloat("Height", hit.distance);
            JumpRoutine(hit);
        }

        RaycastHit2D hitright;
        hitright = Physics2D.Raycast(transform.position+
            Vector3.up*0.8f, transform.right,1);

        if (hitright)
        {
            if (hitright.distance < 0.8f)
            {
                JumpRoutineSide(hitright);
            }
            Debug.DrawLine(hitright.point, transform.position 
                + Vector3.up * 0.9f);
        }

        
    }
    /// <summary>
    /// rotina de pulo parte fisica
    /// </summary>
    /// <param name="hit">coloque aqui o raycast hit para altura do chao</param>
    private void JumpRoutine(RaycastHit2D hit)
    {
        if (hit.distance < 0.6f) // Pulo da personagem, resistência ~ por aqui podemos verificar o peso da personagem
        {
            jumptime = 2;
        }
      

            if (jump)
            {
                jumptime = Mathf.Lerp(jumptime, 0, Time.fixedDeltaTime * 20);
                rdb.AddForce(Vector2.up * jumptime, ForceMode2D.Impulse);
            }
        
    }

    private void JumpRoutineSide(RaycastHit2D hitside)
    {
        if (hitside.distance < 0.5f )
        {

            jumptimeside = 1;
           
        }

        if (doublejump)
        {
            PhisicalReverser();
            jumptimeside = Mathf.Lerp(jumptimeside, 0, Time.fixedDeltaTime*10);
            rdb.AddForce((hitside.normal*50 + Vector2.up*80) * jumptimeside);
        }
    }




    /// <summary>
    /// funcao pra inverter o personagem
    /// </summary>
    void Reverser()
    {
        if (xmov > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (xmov < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

    }
    void PhisicalReverser()
    {
        if (rdb.velocity.x > 0.1f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (rdb.velocity.x < 0.1f)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Damage"))
        {
            LevelManager.instance.LowDamage();
        }
    }
}
