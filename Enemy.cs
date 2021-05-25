using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    private Player _play;
    private Animator _anim;
    [SerializeField]
    private AudioClip _explosionClip;
    private AudioSource _explosionSound;
    [SerializeField]
    private GameObject _laserPrefab;
    // Start is called before the first frame update
    void Start()
    {
        _play = GameObject.Find("Player").GetComponent<Player>();
        if(_play == null)
        {
            Debug.LogError("The player is null");
        }
        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("The animator is null");
        }
        _explosionSound = GetComponent<AudioSource>();
        if(_explosionSound == null)
        {
            Debug.LogError("The audio explosion for enemy is null");
        }
        else
        {
            _explosionSound.clip = _explosionClip;
        }
        StartCoroutine(FireLaser());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -5.45)
        {
            float randomX = Random.Range(-9.5f, 9.5f);
            transform.position = new Vector3(randomX, 6.93f, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            _anim.SetTrigger("OnEnemyDead");
            _speed = 0;

            _explosionSound.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject,2.5f);
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
        }else if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            
            if (_play != null)
            {
                _play.ScoreUp(10);
            }
            _anim.SetTrigger("OnEnemyDead");
            _speed = 0;
            _explosionSound.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject,2.5f);
        }
        
    }
    IEnumerator FireLaser()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3, 8));
            Instantiate(_laserPrefab, transform.position , Quaternion.identity);
        }
    }

}

//there are 2 types of collisions:
//hard surface collisions
//throwing a ball at a wall
//trigger collisions
//having contact triggers an event, u collide but u don't act ON the object, THERE'S NO BOUNCE OR FORCE BACK