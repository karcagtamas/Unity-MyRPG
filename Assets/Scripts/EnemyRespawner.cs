using UnityEngine;

public class EnemyRespawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] respawnPoints;
    [SerializeField] private float cooldown = 2f;
    [Space]
    [SerializeField] private float cooldownDecreaseRate = .05f;
    [SerializeField] private float cooldownCap = .7f;
    private float timer;
    private Transform player;

    void Awake()
    {
        player = FindFirstObjectByType<Player>()?.transform;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            timer = cooldown;
            CreateNewEnemy();

            cooldown = Mathf.Max(cooldownCap, cooldown - cooldownDecreaseRate);
        }
    }

    private void CreateNewEnemy()
    {
        var respawnPointIndex = Random.Range(0, respawnPoints.Length);
        var spawnPoint = respawnPoints[respawnPointIndex].position;
        var newEnemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);

        // Flip if the enemy is facing into the wrong direction
        if (newEnemy.transform.position.x > player.transform.position.y)
        {
            newEnemy.GetComponent<Enemy>().Flip();
        }
    }
}
