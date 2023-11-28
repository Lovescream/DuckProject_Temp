using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerBallProjectile : Projectile {

    protected override void FixedUpdate() {
        base.FixedUpdate();
        Vector2 velocity = rigid.velocity;

        Vector2 screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);
        if (screenPosition.x <= 0 && velocity.x < 0) velocity.x *=-1;
        else if (screenPosition.x >= Screen.width && velocity.x > 0) velocity.x *=-1;
        if (screenPosition.y <= 0 && velocity.y < 0) velocity.y *=-1;
        else if (screenPosition.y >= Screen.height && velocity.y > 0) velocity.y *=-1;
        
        rigid.velocity = velocity;

    }

}