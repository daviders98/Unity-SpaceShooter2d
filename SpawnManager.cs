using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] Powerups;
    private bool _stopSpawning = false;
    // Start is called before the first frame update

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 6.93f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.SetParent(_enemyContainer.transform);
            yield return new WaitForSeconds(5.0f);
        }
    }
    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            Vector3 posOfPower = new Vector3(Random.Range(-9.0f, 9.0f), 7.77f, 0);
            int randompower = Random.Range(0, 3);
            GameObject newPowerup = Instantiate(Powerups[randompower], posOfPower, Quaternion.identity);
            newPowerup.transform.SetParent(this.transform);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }
    public void onPlayerDead()
    {
        _stopSpawning = true;
    }
}
