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

    //dont need prolly
    //public PlayerPortraitVisual Portrait;
    //public HeroPowerButton HeroPower;
    //public EndTurnButton EndTurnButton;
    //public Transform PortraitPosition;

    public bool AllowedToControlThisPlayer
    {
        get;
        set;
    }      
}
