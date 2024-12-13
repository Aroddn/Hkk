using UnityEngine;
using System.Collections;

public interface ICharacter: IIdentifiable
{	
    int Health { get; set;}

    int Attack { get; set;}

    void Die(bool sacrifice);

    void BuffHealth(int amount);
    void BuffAttack(int amount);
    void BuffAttackAndHealth(int attack, int health);    
}

public interface IIdentifiable
{
    int ID { get; }
}
