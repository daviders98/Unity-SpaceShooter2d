using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

//the last engine provides the functionality of the class inheritance of MonoBehaviour
public class Player : MonoBehaviour
{
    //underscore is a standard .net practice
    //the thing below is an atribute to a variable, which lets control private variables as if they were public
    [SerializeField]
    private float _speed = 5.0f;
    private float _speedMultiplier = 2;
    //if there's a need to some other class to access to the laserPrefab, then u put public, else private
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = .15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    [SerializeField]
    private GameObject _Tripleshot;
    [SerializeField]
    private GameObject ShieldSprite;

    private bool _TripleShootActive = false;
    private bool _SpeedBoostActive = false;
    private bool _ShieldsActive = false;
    [SerializeField]
    private int _score = 0;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _LeftThruster;
    [SerializeField]
    private GameObject _rightThruster;
    [SerializeField]
    private AudioSource _laserSound;
    [SerializeField]
    private AudioSource _explosionSound;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The spawn manager is null");
        }
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("The ui manager is null");
        }
    }
    // Update is called once per frame
    void Update()
    {
        calculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            fireLaser();
        }
    }
    void calculateMovement()
    {
        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");
        //not optimal using the "NEW" keyword
        Vector3 direction = new Vector3(HorizontalInput, VerticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);
        //best practice in 1 line
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.94f, 0), 0);
        if (transform.position.x > 11)
        {
            transform.position = new Vector3(-11f, transform.position.y, 0);
        }
        else if (transform.position.x < -11)
        {
            transform.position = new Vector3(11f, transform.position.y, 0);
        }
    }
    void fireLaser()
    {
        _canFire = Time.time + _fireRate;
        if (_TripleShootActive == false)
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_Tripleshot, transform.position, Quaternion.identity);
        }
        _laserSound.Play();
    }
    //by default every method is private, and cause the enemy needs to access to this function it must be public
    public void Damage()
    {
        if (_ShieldsActive)
        {
            ShieldSprite.SetActive(false);
            _ShieldsActive = false;
            return;
        }
        _lives--;
        if (_lives == 2)
        {
            _rightThruster.SetActive(true);
        }else if(_lives == 1)
        {
            _LeftThruster.SetActive(true);
        }

        _uiManager.updateLives(_lives);
        if(_lives < 1)
        {
            _spawnManager.onPlayerDead();
            Destroy(this.gameObject);
            _uiManager.ShowGameOver();
        }
    }
    public void TripleShotActive()
    {
        _TripleShootActive = true;
        StartCoroutine(TripleshotPowerDownRoutine());
    }
    IEnumerator TripleshotPowerDownRoutine()
    {
            yield return new WaitForSeconds(5f);
            _TripleShootActive = false;
    }
    public void speedBoost()
    {
        _SpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedboostPowerDownRoutine());
    }

    IEnumerator SpeedboostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _SpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }
    public void ShieldsUp()
    {
        _ShieldsActive = true;
        ShieldSprite.SetActive(true);
    }
    public void ScoreUp(int points)
    {
        _score += points;
    }
    public int ReturnScore()
    {
        return _score;
    }
    public int ReturnLives()
    {
        return _lives;
    }

}
