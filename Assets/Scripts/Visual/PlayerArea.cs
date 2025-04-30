using UnityEngine;
using System.Collections;

public enum AreaPosition{Top, Low}

//Containts everything a player has and belongs to them on the screen

public class PlayerArea : MonoBehaviour 
{
    public AreaPosition owner;
    public bool ControlsON = true;
    public PlayerDeckVisual PDeck;
    public ManaPoolVisual ManaBar;
    public HandVisual handVisual;
    public TableVisual tableVisual;
    public BoneVisual Bones;

    public GraveYardVisual graveYardVisual;
    public VoidVisual voidVisual;

    public PlayerPortraitVisual Portrait;
    public Transform PortraitPosition;
    public Transform InitialPortraitPosition;

    public bool AllowedToControlThisPlayer{get;  set;}      
}
