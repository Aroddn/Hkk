using UnityEngine;
using System.Collections;

public interface ICharacter: IIdentifiable
{	
    int Health { get; set;}

    int Attack { get; set;}

    void Die(bool sacrifice);

    void ChangeHealth(int amount, bool heal);
  
}

public interface IIdentifiable
{
    int ID { get; }
}
