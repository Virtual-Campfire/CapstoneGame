using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FSMSystem
{

    private List<FSMState> states; //状态集

    private StateID currentStateID;
    public StateID CurrentStateID
    {
        get { return currentStateID; }
    }
    private FSMState currentState;
    public FSMState CurrentState
    {
        get { return currentState; }
    }

    public void UpdateFSM()
    {
        currentState.Reason();
        currentState.Act();
    }

    public FSMSystem()
    {
        states = new List<FSMState>();
    }

    public void SetCurrentState(FSMState state)
    {
        currentState = state;
        currentStateID = state.ID;
        state.DoBeforeEntering(); //开始前状态切换
    }


    public void AddState(FSMState fsmState, GameManager manager)
    {
        // Check for Null reference before deleting 
        if (fsmState == null)
        {
            Debug.LogError("FSM ERROR: Null reference is not allowed");
        }
        else // First State inserted is also the Initial state,
        {
            fsmState.Manager = manager; 

            if (states.Count == 0)
            {
                states.Add(fsmState);
                return;
            }


            foreach (FSMState state in states) // Add the state to the List if it's not inside it 
            {
                if (state.ID == fsmState.ID)
                {
                    //Debug.LogError("FSM ERROR: Impossible to add state " + fsmState.ID.ToString() +
                    //               " because state has already been added");
                    //return;
                }
            }

            states.Add(fsmState);
        }
    }


    public void DeleteState(StateID id)
    {
        if (id == StateID.NullStateId) // Check for NullState before deleting 
        {
            Debug.LogError("FSM ERROR: NullStateID is not allowed for a real state");
            return;
        }


        foreach (FSMState state in states) 
        {
            if (state.ID == id)
            {
                states.Remove(state);
                return;
            }
        }
        Debug.LogError("FSM ERROR: Impossible to delete state " + id.ToString() +
                       ". It was not on the list of states");
    }

    public void PerformTransition(Transition trans)
    {
        if (trans == Transition.NullTransition) 
        {
            Debug.LogError("FSM ERROR: NullTransition is not allowed for a real transition");
            return;
        }


        StateID id = currentState.GetOutputState(trans); 
        if (id == StateID.NullStateId)
        {
            Debug.LogError("FSM ERROR: State " + currentStateID.ToString() + " does not have a target state " +
                           " for transition " + trans.ToString());
            return;
        }


        currentStateID = id;
        foreach (FSMState state in states)
        {
            if (state.ID == currentStateID)
            {
                currentState.DoBeforeLeaving();
                currentState = state;
                currentState.DoBeforeEntering(); 
                break;
            }
        }
    }

}
