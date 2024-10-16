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

        velocity = Quaternion.AngleAxis(random, new Vector3(0, 0, 1)) * new Vector3(-1, 0, 0);

        GetComponent<Score>().SetScore(400);
    }
}
