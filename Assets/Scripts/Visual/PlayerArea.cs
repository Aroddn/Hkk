using UnityEngine;
using System.Collections;

public enum AreaPosition{Top, Low}

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

    //portrait
    public PlayerPortraitVisual Portrait;
    public Transform PortraitPosition;
    public Transform InitialPortraitPosition;

    //dont need prolly
    //public HeroPowerButton HeroPower;
    //public EndTurnButton EndTurnButton;

    public bool AllowedToControlThisPlayer{get;  set;}      
}
