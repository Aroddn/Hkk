using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
using Mirror;

public class CreatureAttackVisual : NetworkBehaviour
{
    private OneCreatureManager manager;
    private WhereIsTheCardOrCreature w;

    void Awake()
    {
        manager = GetComponent<OneCreatureManager>();    
        w = GetComponent<WhereIsTheCardOrCreature>();
    }

    [Command(requiresAuthority = false)]
    public void AttackTarget(int targetUniqueID, int damageTakenByTarget, int damageTakenByAttacker, int attackerHealthAfter, int targetHealthAfter)
    {
        RpcAttackTarget(targetUniqueID, damageTakenByTarget, damageTakenByAttacker, attackerHealthAfter, targetHealthAfter);
    }

    [ClientRpc]
    public void RpcAttackTarget(int targetUniqueID, int damageTakenByTarget, int damageTakenByAttacker, int attackerHealthAfter, int targetHealthAfter)
    {
        manager.CanAttackNow = false;
        GameObject target = IDHolder.GetGameObjectWithID(targetUniqueID);

        w.BringToFront();
        VisualStates tempState = w.VisualState;
        w.VisualState = VisualStates.Transition;

        transform.DOMove(target.transform.position, 0.5f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InCubic).OnComplete(() =>
        {
            if (damageTakenByTarget > 0)
                DamageEffect.CreateDamageEffect(target.transform.position, damageTakenByTarget);
            if (damageTakenByAttacker > 0)
                DamageEffect.CreateDamageEffect(transform.position, damageTakenByAttacker);

            if (targetUniqueID == GlobalSettings.Instance.LowPlayer.PlayerID || targetUniqueID == GlobalSettings.Instance.TopPlayer.PlayerID)
            {
                //target.GetComponent<PlayerPortraitVisual>().HealthText.text = targetHealthAfter.ToString();
                target.GetComponent<PlayerPortraitVisual>().TakeDamage(damageTakenByTarget, targetHealthAfter);
            }
            else
                target.GetComponent<OneCreatureManager>().TakeDamage(damageTakenByTarget, targetHealthAfter);
            //target.GetComponent<OneCreatureManager>().HealthText.text = targetHealthAfter.ToString();

            w.SetTableSortingOrder();
            w.VisualState = tempState;

            manager.HealthText.text = attackerHealthAfter.ToString();
            Sequence s = DOTween.Sequence();
            s.AppendInterval(1f);
            s.OnComplete(Command.CommandExecutionComplete);
            //Command.CommandExecutionComplete();
        });
    }  
}
