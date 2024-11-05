using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] Sprite[] spriteTarget;
    [SerializeField] BoxCollider2D boxCollider2D;
    [SerializeField] GameObject targetPrefabs;
    [SerializeField] float resetTime = 0f;
    Bullet bullet;
    float timer = 5f;
    float targetMilestone = 10; // cot moc
   // float targetEated;
   float targetCreated;
    void Awake()
    {
        bullet = FindObjectOfType<Bullet>();
    }

    void Update()
    {
        RandomSpawnTarget();
    }
    void RandomSpawnTarget()
    {
        timer -= Time.deltaTime;
        if(timer < 0){
            timer = resetTime;
            IncreaseFSpawnSpeed();
            //Random position
            GameObject newTarget = Instantiate(targetPrefabs);
            float targetIndex = Random.Range(boxCollider2D.bounds.min.x, boxCollider2D.bounds.max.x);
            newTarget.transform.position = new Vector3(targetIndex, transform.position.y);
            //Random sprite
            int spriteIndex = Random.Range(0, spriteTarget.Length);
            newTarget.GetComponent<SpriteRenderer>().sprite = spriteTarget[spriteIndex];

        }
    }

    void IncreaseFSpawnSpeed()
    {
        targetCreated++;
        if(targetCreated > 1 && targetCreated % targetMilestone == 1 && resetTime > 0.7f)
        {
            resetTime -= 0.3f;
        }

    //     targetEated = UIDisplay.instance.GetScore();
    //    if(targetEated % targetMilestone == 1 && resetTime > 1f)
    //    {
    //         targetEated = 0;
    //         resetTime -= 0.3f;
    //    }
    }
}
