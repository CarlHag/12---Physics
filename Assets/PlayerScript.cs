using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float speed = 100;
    public float jumpHeight = 1000;
    public float horizontalDrag = 0.1f;
    public AudioClip jumpSound;
    public AudioClip shootSound;
    public GameObject OrbPrefab;
    public float reloadSpeed = 1;
    public float shootSpeed = 10;
    [HideInInspector] public bool grounded
    {
        get { return _grounded; }
        set {

            if (!value)
                StartCoroutine(lateJumpTimer(0.1f));
            else
                _grounded = value;
            }
    }

    Rigidbody2D body;
    bool moveLeft, moveRight, jump, _grounded, reloading, shoot;
    AudioSource audioSource;
    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    IEnumerator earlyJumpTimer(float time)
    {
        yield return new WaitForSeconds(time);
        jump = false;
    }
    IEnumerator lateJumpTimer(float time)
    {
        yield return new WaitForSeconds(time);
        _grounded = false;
    }
    IEnumerator reload(float time)
    {
        reloading = true;
        yield return new WaitForSeconds(time);
        reloading = false;
    }
    private void Update()
    {
        if (Input.GetMouseButton(0) && !reloading)
            shoot = true;

        if (Input.GetKey(KeyCode.D))
            moveRight = true;
        else
            moveRight = false;

        if (Input.GetKey(KeyCode.A))
            moveLeft = true;
        else
            moveLeft = false;
            
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(grounded);
            jump = true;
            if(!grounded)
                StartCoroutine(earlyJumpTimer(0.1f));
                
        }
            
    }
    void FixedUpdate()
    {
        if(shoot)
        {
            GameObject orb = Instantiate(OrbPrefab, transform.position, Quaternion.identity);
            Destroy(orb, 5);
            Rigidbody2D orbBody = orb.GetComponent<Rigidbody2D>();
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = ((Vector2)(mousePos - transform.position)).normalized;
            orbBody.AddForce(direction * shootSpeed);
            StartCoroutine(reload(1/reloadSpeed));
            shoot = false;
            audioSource.PlayOneShot(shootSound);
            Debug.Log(direction);
            sprite.flipX = direction.x < 0;
            orb.transform.SetParent(GameObject.Find("Orbs").transform);
        }

        if (moveRight)
        {
            body.AddForce(new Vector2(speed, 0));
            if(!Input.GetMouseButton(0))
                sprite.flipX = false;
        }
            
        if (moveLeft)
        {
            body.AddForce(new Vector2(-speed, 0));
            if (!Input.GetMouseButton(0))
                sprite.flipX = true;
        }
            
            

        if (jump && grounded)
        {
            audioSource.PlayOneShot(jumpSound);
            body.AddForce(new Vector2(0, jumpHeight));
            jump = false;
        }
            

        {
            var vel = body.velocity;
            vel.x *= 1 - horizontalDrag;
            body.velocity = vel;
        }
        

        //Debug.Log(body.velocity.x);

    }
    

}
