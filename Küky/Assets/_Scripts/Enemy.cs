﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity {

    public enum State { Idle, Chasing, Attacking };
    State currentState;

    NavMeshAgent pathfinder;
    Transform target;
    LivingEntity targetEntity;
    Material skinMaterial;

    Color originalColor;

    float attackDistanceThreshold =1f;//The position enemy attack from
    float timeBetweenAttack = 1;
    float damage = 1;

    float nextAttackTime;
    float myCollisionRadius;
    float targetCollisionRadius;

    bool hasTarget;

	// Use this for initialization
    protected override void Start () {
        base.Start();
        pathfinder = GetComponent<NavMeshAgent>();
        skinMaterial = GetComponent<Renderer>().material;
        originalColor = skinMaterial.color;

        if(GameObject.FindGameObjectWithTag("Player")!=null){
            currentState = State.Chasing;
            hasTarget = true;
            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetEntity = target.GetComponent<LivingEntity>();
            targetEntity.OnDeath += OnTargetDeath;

            myCollisionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;
            StartCoroutine(UpdatePath());
            
        }

	}
	
    void OnTargetDeath(){
        hasTarget = false;
        currentState = State.Idle;
    }

	// Update is called once per frame
	void Update () {

        if(hasTarget){
            if (Time.time > nextAttackTime)
            {
                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
                //print(sqrDstToTarget);
                if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
                {
                    nextAttackTime = Time.time + timeBetweenAttack;
                    //print("start attack");
                    StartCoroutine(Attack());
                }
            }
        }
       
        //float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;


	}
    IEnumerator Attack(){

        currentState = State.Attacking;
        pathfinder.enabled = false;

        Vector3 originalPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius + 0.8f);//這個量控制敵人推進多少


        float percent = 0;
        float attackSpeed = 3;

        skinMaterial.color = Color.red;
        bool hasAppliedDamage = false;

        while(percent<=1){

            if (percent >= .5f && !hasAppliedDamage){
                hasAppliedDamage = true;
                targetEntity.TakeDmg(damage);
            }

            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent,2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);
            //print("attack");

            yield return null;
        }

        skinMaterial.color = originalColor;
        currentState = State.Chasing;
        pathfinder.enabled = true;
    }

    IEnumerator UpdatePath(){
        float refreshRate = 0.25f;

        while(hasTarget){
            if(currentState==State.Chasing){
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPos = target.position-dirToTarget * (myCollisionRadius + targetCollisionRadius +attackDistanceThreshold/2);
                if (!dead){
                    pathfinder.SetDestination((targetPos));
                } 
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
