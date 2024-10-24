using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ƒ‰ƒ“ƒ_ƒ€‚È•ûŒü‚É‚Î‚ç‚Ü‚­’e </summary>
public class BulletRandom : BulletParent
{
    protected override void Start()
    {
        base.Start();

        speed = 6.0f;

        var random = Random.Range(-30.0f, 40.0f);

        MoveDirection(true);
        velocity = Quaternion.AngleAxis(random, new Vector3(0, 0, 1)) * velocity;

        var nextPos = velocity * speed + this.transform.position;
        dir = nextPos - this.transform.position;

        GetComponent<Score>().SetScore(350);
    }
}
