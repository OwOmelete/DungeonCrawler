using System.Linq;
using UnityEngine;
using DG.Tweening;

public class TwinsBehaviour : AbstractIA
{
    [SerializeField] private SpikeData _spikeData;
    public override void ManageTurn(FishDataInstance entity)
    {
        for (int i = 0; i < entity.spikeList.Count; i++)
        {
            CombatManager.Instance.grid[entity.spikeList[i].positionY, entity.spikeList[i].positionX] = null;
            if (entity.spikeList[i].positionY + entity.spikeList[i].dirY < 0 ||
                entity.spikeList[i].positionY + entity.spikeList[i].dirY > CombatManager.Instance.grid.GetLength(0) - 1 ||
                entity.spikeList[i].positionX + entity.spikeList[i].dirX < 0 ||
                entity.spikeList[i].positionX + entity.spikeList[i].dirX > CombatManager.Instance.grid.GetLength(1) - 1)
            {
                entity.spikeIndexSupr.Add(i);
            }
            else
            {
                EntityInstance newPos =
                    CombatManager.Instance.grid[entity.spikeList[i].positionY + entity.spikeList[i].dirY, entity.spikeList[i].positionX + entity.spikeList[i].dirX];
                if (newPos != null && newPos is not SpikeInstance)
                {
                    entity.spikeIndexSupr.Add(i);
                    CombatManager.Instance.Damage(newPos, entity.spikeList[i].hp);
                }
                else
                {
                    CombatManager.Instance.grid[entity.spikeList[i].positionY + entity.spikeList[i].dirY,
                        entity.spikeList[i].positionX + entity.spikeList[i].dirX] = entity.spikeList[i];
                    entity.spikeList[i].prefab.transform.DOMove
                    (new Vector3(entity.spikeList[i].positionX + entity.spikeList[i].dirX,
                        entity.spikeList[i].positionY + entity.spikeList[i].dirY, 0), 0.5f)
                        .SetEase(Ease.InOutCubic);
                    entity.spikeList[i].positionX += entity.spikeList[i].dirX;
                    entity.spikeList[i].positionY += entity.spikeList[i].dirY;
                }
            }
        }
        for (int i = entity.spikeIndexSupr.Count; i > 0; i--)
        {
            if (entity.spikeList.Count >= i)
            {
                Destroy(entity.spikeList[entity.spikeIndexSupr[i-1]].prefab);
                CombatManager.Instance.grid[entity.spikeList[entity.spikeIndexSupr[i - 1]].positionY,
                    entity.spikeList[entity.spikeIndexSupr[i - 1]].positionX] = null;
                entity.spikeList.Remove(entity.spikeList[entity.spikeIndexSupr[i-1]]);
            }
        }
        entity.spikeIndexSupr.Clear();

        if (CombatManager.Instance.combatFinished)
        {
            return;
        }
        if (entity.HasAttacked)
        {
            if (entity.TurnCount >= 2)
            {
                entity.HasAttacked = false;
                entity.TurnCount = 0;
            }
            else
            {
                entity.TurnCount ++;
            }
        }
        if (entity.PreparingAttack)
        {
            if (entity.positionX > CombatManager.Instance.player.positionX)
            {
                Shoot(entity, -1);
            }
            else
            {
                Shoot(entity, 1);
            }
            
        }
        else if (CombatManager.Instance.player.positionY >= entity.positionY &&
            CombatManager.Instance.player.positionY <= entity.positionY+2)
        {
            if (!entity.PreparingAttack && !entity.HasAttacked)
            {
                entity.LastPrevisualisation =
                    Instantiate(entity.PrevisualisationAttack,
                        new Vector3(entity.positionX, entity.positionY + 1, 0), Quaternion.identity);
                entity.weakPointList.Add(entity.WeakPointsRight[0]);
                entity.PreparingAttack = true;
            }
        }
        else
        {
            if (CombatManager.Instance.player.positionY < entity.positionY+1)
            {
                CombatManager.Instance.Move(entity, entity.positionX, entity.positionY - 1);
            }
            else
            {
                CombatManager.Instance.Move(entity, entity.positionX, entity.positionY + 1);
            }
        }
    }

    void Shoot(FishDataInstance entity, int dirX)
    {
        entity.PreparingAttack = false;
        
        entity.weakPointList.Clear();
        
        Destroy(entity.LastPrevisualisation);
        
        if (CombatManager.Instance.grid[entity.positionY + 1, entity.positionX + dirX] != null)
        {
            CombatManager.Instance.Damage(
                CombatManager.Instance.grid[entity.positionY, entity.positionX + dirX], _spikeData.hp);
        }
        else
        {
            SpikeInstance spike = new SpikeInstance(_spikeData);
            entity.spikeList.Add(spike);
            spike.dirX = dirX;
            spike.dirY = 0;
            spike.entity = entity;
            spike.positionY = entity.positionY + 1;
            spike.positionX = entity.positionX + dirX;
            CombatManager.Instance.grid[spike.positionY, spike.positionX] = spike;
            int rotation;
            if (dirX == 1)
            {
                rotation = -90;
            }
            else
            {
                rotation = 90;
            }
            spike.prefab = Instantiate(spike.prefab, new Vector3(entity.positionX + dirX,
                entity.positionY + 1, 0), Quaternion.identity);
            spike.entityChild = spike.prefab.transform.GetChild(0);
            spike.entityChild.transform.rotation = Quaternion.Euler(0,0,rotation);
        }
        
        entity.HasAttacked = true;
    }
    
}
