using UnityEngine;

//Interface of Damge
public interface IDamage {

    void TakeHit(float damage, Collision col);
    void TakeDmg(float damage);
}
