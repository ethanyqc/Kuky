using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntity {

    Camera viewCamera;

    protected override void Start()
    {
        base.Start();
        viewCamera = Camera.main;
    }
    void Update(){
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if(groundPlane.Raycast(ray,out rayDistance)){
            Vector3 point = ray.GetPoint(rayDistance);
            Debug.DrawLine(ray.origin,point,Color.red);
        }
    }
}
