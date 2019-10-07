using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joint : MonoBehaviour {


    public Vector3 Axis;
    public Vector3 StartOffset;
    
    public float MinAngle;
    public float MaxAngle;
    public float SamplingDistance;
    public float DistanceThreshold;
    public float LearningRate;
    public Joint[] Joints;
    public Root root;
    void Awake()
    {
        StartOffset = transform.localPosition;
    }

    public Vector3 ForwardKinematics(float[] angles)
    {
        Vector3 prevPoint = Joints[0].transform.position;
        Quaternion rotation = Quaternion.identity;
        for (int i = 1; i < Joints.Length; i++)
        {
            // Rotates around a new axis
            rotation *= Quaternion.AngleAxis(angles[i - 1], Joints[i - 1].Axis);
            Vector3 nextPoint = prevPoint + rotation * Joints[i].StartOffset;

            prevPoint = nextPoint;
        }
        return prevPoint;
    }

    public float DistanceFromTarget(Vector3 target, float[] angles)
    {
        Vector3 point = ForwardKinematics(angles);
        return Vector3.Distance(point, target);
    }

    public float PartialGradient(Vector3 target, float[] angles, int i)
    {
        // Saves the angle,
        // it will be restored later
        float angle = angles[i];

        // Gradient : [F(x+SamplingDistance) - F(x)] / h
        float f_x = DistanceFromTarget(target, angles);

        angles[i] += SamplingDistance;
        float f_x_plus_d = DistanceFromTarget(target, angles);

        float gradient = (f_x_plus_d - f_x) / SamplingDistance;

        // Restores
        angles[i] = angle;

        return gradient;
    }

    public void InverseKinematics(Vector3 target, float[] angles)
    {
        if (DistanceFromTarget(target, angles) < DistanceThreshold)
            return;

        for (int i = Joints.Length - 1; i >= 0; i--)
        {
            // Gradient descent
            // Update : Solution -= LearningRate * Gradient
            float gradient = PartialGradient(target, angles, i);
            angles[i] -= LearningRate * gradient;
            angles[i] = Mathf.Clamp(angles[i], Joints[i].MinAngle, Joints[i].MaxAngle);
            // Early termination
            if (DistanceFromTarget(target, angles) < DistanceThreshold)
                return;
        }
    }
    // Use this for initialization
    void Start () {
        
        Joints = GetComponentsInParent<Joint>();
	}
	
	// Update is called once per frame
	void Update () {
        float[] angles = new float[Joints.Length];
        Vector3 axis;
        for (int i=0;i <Joints.Length;i++)
        {
            axis = Joints[i].Axis;
            if(axis[0] == 1)
            {
                angles[i] = Joints[i].transform.localEulerAngles.x;
            }
            else if(axis[1] == 1)
            {
                angles[i] = Joints[i].transform.localEulerAngles.y;
            }
            else
            {
                angles[i] = Joints[i].transform.localEulerAngles.z;
            }
        }
        InverseKinematics(root.target.transform.position, angles);
	}
}
