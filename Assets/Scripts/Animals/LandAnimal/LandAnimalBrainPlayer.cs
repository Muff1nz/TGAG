﻿using UnityEngine;
using System.Collections;

public class LandAnimalBrainPlayer : AnimalBrainPlayer {
    bool jumping = false;

    override public float slowSpeed { get { return 5f; } }
    override public float fastSpeed { get { return 20f; } }

    /// <summary>
    /// Function for that lets the player control the animal
    /// </summary>
    override public void move() {
        state.desiredSpeed = 0;

        if (!Input.GetKey(KeyCode.LeftAlt)) {
            state.desiredHeading = Camera.main.transform.forward;
            state.desiredHeading.y = 0;
            state.desiredHeading.Normalize();
            oldHeading = state.desiredHeading;
        } else {
            state.desiredHeading = oldHeading;
        }

        Vector3 finalHeading = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) {
            finalHeading += state.desiredHeading;
        }
        if (Input.GetKey(KeyCode.A)) {
            finalHeading += Quaternion.AngleAxis(-90, Vector3.up) * state.desiredHeading;
        }
        if (Input.GetKey(KeyCode.D)) {
            finalHeading += Quaternion.AngleAxis(90, Vector3.up) * state.desiredHeading;
        }
        if (Input.GetKey(KeyCode.S)) {
            finalHeading -= state.desiredHeading;
        }

        if (!jumping && state.grounded && Input.GetKeyDown(KeyCode.Space)) {
            actions["jump"]();
        }

        if (finalHeading != Vector3.zero) {
            state.desiredHeading = finalHeading;
            setSpeed();
        }
    }

    /// <summary>
    /// Sets speed based on input
    /// </summary>
    private void setSpeed() {
        if (!state.grounded && !state.inWater && !state.onWaterSurface) {
            state.desiredSpeed = 0;
        } else {
            if (Input.GetKey(KeyCode.LeftShift)) {
                state.desiredSpeed = fastSpeed;
            } else {
                state.desiredSpeed = slowSpeed;
            }
        }
    }    
}
