﻿using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public abstract class LandAnimal : MonoBehaviour {
    protected AnimalSkeleton skeleton;
    protected float ikSpeed = 10;
    protected const float ikTolerance = 0.1f;

    protected Vector3 heading = Vector3.zero;
    protected float speed = 2f;
    protected const float walkSpeed = 2f;
    protected const float runSpeed = walkSpeed * 3f;
    protected const float levelSpeed = 3f;
    protected const float walkAnimSpeed = 0.2f;
    protected const float runAnimSpeed = walkAnimSpeed * 3f;

    protected float timer = 0;

    // Update is called once per frame
    void Update() {
        if (skeleton != null) {
            move();
            levelSpine();
            walk();
            timer += Time.deltaTime * speed / 2f;
        }
    }

    /// <summary>
    /// sets the skeleton, and applies the new mesh.
    /// </summary>
    public void setSkeleton(AnimalSkeleton skeleton) {
        this.skeleton = skeleton;
        GetComponent<SkinnedMeshRenderer>().sharedMesh = skeleton.getMesh();
        GetComponent<SkinnedMeshRenderer>().rootBone = transform;

        List<Bone> skeletonBones = skeleton.getBones(BodyPart.ALL);
        Transform[] bones = new Transform[skeletonBones.Count];
        for (int i = 0; i < bones.Length; i++) {
            bones[i] = skeletonBones[i].bone;
        }
        GetComponent<SkinnedMeshRenderer>().bones = bones;
    }

    protected abstract void move();

    /// <summary>
    /// Tries to level the spine with the ground
    /// </summary>
    protected void levelSpine() {
        float spineLength = skeleton.getBodyParameter<float>(BodyParameter.SPINE_LENGTH);
        Bone spine = skeleton.getBones(BodyPart.SPINE)[0];

        RaycastHit hit1;
        RaycastHit hit2;
        Physics.Raycast(new Ray(spine.bone.position + spine.bone.forward * spineLength / 2f, Vector3.down), out hit1);
        Physics.Raycast(new Ray(spine.bone.position - spine.bone.forward * spineLength / 2f, Vector3.down), out hit2);
        Vector3 a = hit1.point - hit2.point;
        Vector3 b = spine.bone.forward * spineLength;

        float angle = Mathf.Acos(Vector3.Dot(a, b) / (a.magnitude * b.magnitude));
        Vector3 normal = Vector3.Cross(a, b);
        if (angle > 0.01f) {
            spine.bone.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg * levelSpeed * Time.deltaTime, -normal) * spine.bone.rotation;
            if (!checkConstraints(spine)) {
                spine.bone.rotation = Quaternion.AngleAxis(-angle * Mathf.Rad2Deg * levelSpeed * Time.deltaTime, -normal) * spine.bone.rotation;
            }
        }
    }

   

    /// <summary>
    /// Keeps the legs of the animal grounded.
    /// </summary>
    protected void stayGrounded() { //TODO - Update to new system of getting legs
        //List<Bone> rightLegs = skeleton.getBones(BodyPart.RIGHT_LEGS);
        //List<Bone> leftLegs = skeleton.getBones(BodyPart.LEFT_LEGS);
        
        //var right1 = rightLegs.GetRange(0, 3);
        //var right2 = rightLegs.GetRange(3, 3);
        //var left1 = leftLegs.GetRange(0, 3);
        //var left2 = leftLegs.GetRange(3, 3);

        //groundLeg(right1, -1);
        //groundLeg(right2, -1);
        //groundLeg(left1, 1);
        //groundLeg(left2, 1);
    }

    /// <summary>
    /// Grounds one leg
    /// </summary>
    /// <param name="leg">List<Bone> leg</param>
    /// <param name="sign">int sign, used to get a correct offset for IK target</param>
    protected void groundLeg(List<Bone> leg, int sign) {
        Vector3 target = leg[0].bone.position + sign * transform.right * skeleton.getBodyParameter<float>(BodyParameter.LEG_LENGTH) / 2f;

        RaycastHit hit;
        if (Physics.Raycast(new Ray(target, Vector3.down), out hit)) {
            ccd(leg, hit.point, ikSpeed);
        }
    }

    /// <summary>
    /// Makes the animal do a walking animation
    /// </summary>
    protected void walk() {
        int legPairs = skeleton.getBodyParameter<int>(BodyParameter.LEG_PAIRS);
        for(int i = 0; i < legPairs; i++) {
            walkLeg(skeleton.getLeg(true, i), -1, Mathf.PI * i);
            walkLeg(skeleton.getLeg(false, i), 1, Mathf.PI * (i + 1));
        }
    }

    /// <summary>
    /// Uses IK to make the leg walk.
    /// </summary>
    /// <param name="leg">List<Bone> leg</param>
    /// <param name="sign">int sign, used to get a correct offset for IK target</param>
    /// <param name="radOffset">Walk animation offset in radians</param>
    protected void walkLeg(List<Bone> leg, int sign, float radOffset) {
        float legLength = skeleton.getBodyParameter<float>(BodyParameter.LEG_LENGTH);

        Vector3 target = leg[0].bone.position + sign * transform.right * legLength / 4f; //Offset to the right
        target += heading * Mathf.Cos(timer + radOffset) * legLength / 4f;  //Forward/Backward motion
        float rightOffset = (Mathf.Sin(timer + Mathf.PI + radOffset)) * legLength / 8f; //Right/Left motion
        rightOffset = (rightOffset > 0) ? rightOffset : 0;
        target += sign * transform.right * rightOffset;
        target.y -= skeleton.getBodyParameter<float>(BodyParameter.LEG_JOINT_LENGTH) / 2f;
        ccd(leg.GetRange(0, 2), target, ikSpeed / 4f);

        RaycastHit hit;
        if (Physics.Raycast(new Ray(target, Vector3.down), out hit)) {
            float heightOffset = (Mathf.Sin(timer + Mathf.PI + radOffset)) * legLength / 8f; //Up/Down motion
            heightOffset = (heightOffset > 0) ? heightOffset : 0;

            target = hit.point;
            target.y += heightOffset;
            ccd(leg, target, ikSpeed);
        }

    }

    /// <summary>
    /// Does one iteration of CCD
    /// </summary>
    /// <param name="limb">Limb to bend</param>
    /// <param name="target">Target to reach</param>
    /// <returns>Bool target reached</returns>
    protected bool ccd(List<Bone> limb, Vector3 target, float speed) {
        Debug.DrawLine(target, target + Vector3.up * 10, Color.red);
        Transform effector = limb[limb.Count - 1].bone;
        float dist = Vector3.Distance(effector.position, target);

        if (dist > ikTolerance) {
            for (int i = limb.Count - 1; i >= 0; i--) {
                Transform bone = limb[i].bone;

                Vector3 a = effector.position - bone.position;
                Vector3 b = target - bone.position;


                float angle = Mathf.Acos(Vector3.Dot(a, b) / (a.magnitude * b.magnitude));
                Vector3 normal = Vector3.Cross(a, b);
                if (angle > 0.01f) {
                    bone.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg * speed * Time.deltaTime, normal) * bone.rotation;
                    if (!checkConstraints(limb[i])) {
                        bone.rotation = Quaternion.AngleAxis(-angle * Mathf.Rad2Deg * speed * Time.deltaTime, normal) * bone.rotation;
                    }
                }
            }
        }        
        return dist < ikTolerance;
    }

    /// <summary>
    /// Checks the joint constraints for the bone
    /// </summary>
    /// <param name="bone">Bone bone, the bone to check</param>
    /// <returns>True if bone satisfies bone constraints, false otherwise</returns>
    protected bool checkConstraints(Bone bone) {
        Vector3 rotation = bone.bone.localEulerAngles;
        rotation.x = (rotation.x > 180) ? rotation.x - 360 : rotation.x;
        rotation.y = (rotation.y > 180) ? rotation.y - 360 : rotation.y;
        rotation.z = (rotation.z > 180) ? rotation.z - 360 : rotation.z;

        //Debug.Log("Rot: " + rotation + "__Min: " + bone.minAngles + "__Max: " + bone.maxAngles);
        bool min = rotation.x > bone.minAngles.x && rotation.y > bone.minAngles.y && rotation.z > bone.minAngles.z;
        bool max = rotation.x < bone.maxAngles.x && rotation.y < bone.maxAngles.y && rotation.z < bone.maxAngles.z;
        return min && max;
    }
}
