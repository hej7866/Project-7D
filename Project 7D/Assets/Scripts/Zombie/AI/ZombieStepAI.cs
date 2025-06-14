using System;
using UnityEngine;

public enum StepState { None, Step, Transfer}

public class ZombieStepAI : MonoBehaviour
{
    [Header("설정")]
    public float stepSpeed = 0.5f;
    public float transferSpeedFactor = 0.3f;
    public float stepDuration = 1.3f;
    public float transferDuration = 0.4f;

    [Header("디버그용")]
    public StepState currentStep = StepState.None;
    public float stepTimer = 0f;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        currentStep = StepState.None;
    }

    public void ZombieStep(Transform target)
    {
        if (target == null) return;

        Vector3 dir = target.position - transform.position;
        dir.y = 0f;
        dir.Normalize();

        stepTimer += Time.deltaTime;

        switch (currentStep)
        {
            case StepState.Step:
                transform.position += dir * stepSpeed * Time.deltaTime;
                if (stepTimer >= stepDuration)
                {
                    currentStep = StepState.Transfer;
                    stepTimer = 0f;
                }
                break;
            case StepState.Transfer:
                transform.position += dir * stepSpeed * transferSpeedFactor * Time.deltaTime;
                if (stepTimer >= transferDuration)
                {
                    currentStep = StepState.Step;
                    stepTimer = 0f;
                }
                break;
        }
    }
}
