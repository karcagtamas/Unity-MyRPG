using UnityEngine;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private float redColorDuration = 1;
    public float currentTimeInGame;
    public float lastTimeWasDamaged;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        ChangeColorIfNeeded();
    }

    private void ChangeColorIfNeeded()
    {
        currentTimeInGame = Time.time;

        if (currentTimeInGame > lastTimeWasDamaged + redColorDuration)
        {
            if (sr.color != Color.white)
            {
                sr.color = Color.white;
            }
        }
    }

    public void TakeDamage()
    {
        sr.color = Color.red;
        lastTimeWasDamaged = Time.time;
    }
}
