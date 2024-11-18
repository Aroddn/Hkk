using UnityEngine;
using System.Collections;

public enum CharClass{ Elf, Monk, Warrior}

public class CharacterAsset : ScriptableObject 
{
	public CharClass Class;
	public string ClassName;
	public int MaxHealth = 20;
	public string HeroPowerName;
	public Sprite AvatarImage;
    public Sprite AvatarBGImage;
    public Color32 AvatarBGTint;
    public Color32 ClassCardTint;
    public Color32 ClassRibbonsTint;
}
