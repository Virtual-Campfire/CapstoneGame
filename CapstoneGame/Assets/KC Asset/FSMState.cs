using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum StateID
{
    NullStateId,
    Ideal,
    Doubt,
    Chasing

}

public enum Transition
{
    NullTransition,
    IntoIdeal,
    IntoDoubt,
    IntoChasing
}


public abstract class FSMState:MonoBehaviour
{

    public Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>(); 
    protected StateID stateID;                                     //Ë½ÓÐID
    public StateID ID                                           //×´Ì¬ID
    {
        get { return stateID; }
    }


    protected GameManager manager; 
    public GameManager Manager
    {
        set { manager = value; }
    }


   
    public void AddTransition(Transition trans, StateID id)
    {
        if (trans == Transition.NullTransition) // Check if anyone of the args is invalid 
        {
            Debug.LogError("FSMState ERROR: NullTransition is not allowed for a real transition");
            return;
        }

        if (id == StateID.NullStateId)
        {
            Debug.LogError("FSMState ERROR: NullStateID is not allowed for a real ID");
            return;
        }

        if (map.ContainsKey(trans)) 
        {
            Debug.LogError("FSMState ERROR: State " + stateID.ToString() + " already has transition " + trans.ToString() +
                           "Impossible to assign to another state");
            return;
        }

        map.Add(trans, id);
    }



    public void DeleteTransition(Transition trans)
    {
        if (trans == Transition.NullTransition) // Check for NullTransition 
        {
            Debug.LogError("FSMState ERROR: NullTransition is not allowed");
            return;
        }

        if (map.ContainsKey(trans)) 
        {
            map.Remove(trans);
            return;
        }
        Debug.LogError("FSMState ERROR: Transition " + trans.ToString() + " passed to " + stateID.ToString() +
                       " was not on the state's transition list");
    }


    public StateID GetOutputState(Transition trans)
    {
        if (map.ContainsKey(trans)) // Check if the map has this transition 
        {
            return map[trans];
        }
        return StateID.NullStateId;
    }
    //what you want me do? put in here.
    public virtual void Act()
    {
    }
    //the is the trans condition
    public virtual void Reason()
    {
    } 

    //this is doing before acting behievior
    public virtual void DoBeforeEntering()
    {
    }


    //this is doing before leaving acting behievior
    public virtual void DoBeforeLeaving()
    {
    }





}

