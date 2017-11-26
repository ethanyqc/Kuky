using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//living entity that encorporate in both player and enemy
public class LivingEntity : MonoBehaviour, IDamage {

    public float startingHealth;
    protected float health;
    protected bool dead;

    public event System.Action OnDeath;

    protected virtual void Start(){
        health = startingHealth;
    }

    public void TakeHit(float damage, Collision col){
        TakeDmg(damage);
    }

    public void TakeDmg(float damage){
        health -= damage;
        print("-" + damage);

        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    protected void Die(){
        dead = true;
        if(OnDeath!=null){
            OnDeath();
        }
        GameObject.Destroy((gameObject));
    }
	
	
}
