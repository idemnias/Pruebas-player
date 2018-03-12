﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling_BackGround : MonoBehaviour {

    public float backgroundSize;
    public float paralaxSpeed;


    private Transform cameraTransform;
    private Transform[] layers;
    private float viewZone = 10;
    private int leftIndex;
    private int rightIndex;
    private float lastCameraX;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraX = cameraTransform.position.x;
        layers = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            layers[i] = transform.GetChild(i);

        leftIndex = 0;

        rightIndex = layers.Length - 1;

    }

    private void Update()
    {
            float deltaX = cameraTransform.position.x - lastCameraX;
            transform.position += Vector3.right * (deltaX * paralaxSpeed);
            lastCameraX = cameraTransform.position.x;

            if (cameraTransform.position.x < (layers[leftIndex].transform.position.x + viewZone))
            ScrollLeft();

            if (cameraTransform.position.x > (layers[rightIndex].transform.position.x + viewZone))
            ScrollRight();


    }

    private void ScrollLeft()
    {
        int lastRight = rightIndex;
        layers[lastRight].position = Vector3.right * (layers[leftIndex].position.x - backgroundSize);
        leftIndex = rightIndex;
        rightIndex--;
        if (rightIndex < 0)
            rightIndex = layers.Length - 1;

    }
    private void ScrollRight()
    {
        int lastleft = leftIndex;
        layers[lastleft].position = Vector3.right * (layers[rightIndex].position.x + backgroundSize);
        rightIndex = leftIndex;
        leftIndex++;
        if (leftIndex < 0)
            leftIndex = 0;

    }
}
