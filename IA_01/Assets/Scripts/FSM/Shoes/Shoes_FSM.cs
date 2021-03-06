﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using Steerings;

[RequireComponent(typeof(ShoesBlackboard))]
[RequireComponent(typeof(WanderPlusAvoid))]

public class Shoes_FSM : FiniteStateMachine
{
    public enum State
    {
        INITIAL, WANDER
    }

    public State currentState = State.INITIAL;
    public int dropFoodRate = 10;

    private ShoesBlackboard shoesBlackboard;
    private WanderPlusAvoid wanderPlusAvoid;

    private void Awake()
    {
        shoesBlackboard = GetComponent<ShoesBlackboard>();
        wanderPlusAvoid = GetComponent<WanderPlusAvoid>();
        wanderPlusAvoid.enabled = false;
    }

    public override void Exit()
    {
        wanderPlusAvoid.enabled = false;
        base.Exit();
    }

    public override void ReEnter()
    {
        currentState = State.INITIAL;
        base.ReEnter();
    }

    void Update()
    {
        switch (currentState)
        {
            case State.INITIAL:
                ChangeState(State.WANDER);
                break;
            case State.WANDER:
                Wander();
                break;
            default:
                break;
        }
    }

    void ChangeState(State newState)
    {
        switch (currentState)
        {
            case State.WANDER:
                wanderPlusAvoid.enabled = false;
                break;
        }

        switch(newState)
        {
            case State.WANDER:
                wanderPlusAvoid.enabled = true;
                break;
        }

        currentState = newState;
    }

    void Wander()
    {
        ShoeKillingWithRadius();
        //ShoeRKillingWithRadius();
        //ShoeLKillingWithRadius();
        DropFood();
    }

    public void ShoeKillingWithRadius()
    {
        int i;

        for (i = 0; i <= 5; i++)
        {
            GameObject ant = SensingUtils.FindInstanceWithinRadius(shoesBlackboard.shoes[i], "ANT", shoesBlackboard.radius);

            if (ant != null)
            {
                Destroy(ant);
            }
        }
        //i = -1;
    }
    /*
    public void ShoeRKillingWithRadius()
    {
        GameObject ant = SensingUtils.FindInstanceWithinRadius(shoesBlackboard.shoeR, "ANT", shoesBlackboard.radius);

        if (ant != null)
        {
            Destroy(ant);
        }
    }
    public void ShoeLKillingWithRadius()
    {
        GameObject ant = SensingUtils.FindInstanceWithinRadius(shoesBlackboard.shoeL, "ANT", shoesBlackboard.radius);

        if (ant != null)
        {
            Destroy(ant);
        }
    }
    */
    public void DropFood()
    {
        shoesBlackboard.counter += Time.deltaTime;

        if (shoesBlackboard.counter >= dropFoodRate)
        {
            Instantiate(shoesBlackboard.food, shoesBlackboard.foodSpawn.transform.position, shoesBlackboard.foodSpawn.transform.rotation);
            shoesBlackboard.counter = 0f;
        }
    }
}
