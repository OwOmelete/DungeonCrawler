using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class SpikeBallBehaviour : AbstractIA
{
    [SerializeField] SpikeData _spikeData;
    public WeakPointData DamageToAttacker;
    
    public override void ManageTurn(FishDataInstance entity)
    {
        if (entity.FirstCycle)
        {
            entity.FirstCycle = false;
            entity.HasAttacked = true;
            entity.entityChild = entity.prefab.transform.GetChild(0);
            entity.spikeIndexSupr.Clear();
            entity.spikeList.Clear();
        }
        
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
                    CombatManager.Instance.grid[entity.spikeList[i].positionY + entity.spikeList[i].dirY, entity.spikeList[i].positionX + entity.spikeList[i].dirX] = entity.spikeList[i];
                    entity.spikeList[i].prefab.transform.DOMove(new Vector3
                        (entity.spikeList[i].positionX + entity.spikeList[i].dirX, entity.spikeList[i].positionY + entity.spikeList[i].dirY, 0), 0.5f).SetEase(Ease.InOutCubic);
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

        if (entity.PreparingAttack)
        {
            switch (entity.direction)
            {
                case EntityInstance.dir.up:
                    if (CombatManager.Instance.player.positionX > entity.positionX)
                    {
                        TurnLeft(entity);
                    }
                    else
                    {
                        TurnRight(entity);
                    }

                    break;
                case EntityInstance.dir.down:
                    if (CombatManager.Instance.player.positionX > entity.positionX)
                    {
                        TurnRight(entity);
                    }
                    else
                    {
                        TurnLeft(entity);
                    }

                    break;
                case EntityInstance.dir.left:
                    if (CombatManager.Instance.player.positionY > entity.positionY)
                    {
                        TurnLeft(entity);
                    }
                    else
                    {
                        TurnRight(entity);
                    }

                    break;
                case EntityInstance.dir.right:
                    if (CombatManager.Instance.player.positionY > entity.positionY)
                    {
                        TurnRight(entity);
                    }
                    else
                    {
                        TurnLeft(entity);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            entity.PreparingAttack = false;
        }
        else
        {
            if (entity.HasAttacked)
            {
                entity.HasAttacked = false;
                switch (entity.direction)
                {
                    case EntityInstance.dir.up:
                        ShootSpike(entity, 0, -1);
                        break;
                    case EntityInstance.dir.down:
                        ShootSpike(entity, 0, 1);
                        break;
                    case EntityInstance.dir.left:
                        ShootSpike(entity, 1, 0);
                        break;
                    case EntityInstance.dir.right:
                        ShootSpike(entity, -1, 0);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                entity.HasAttacked = true;
            }

            switch (entity.direction)
            {
                case EntityInstance.dir.up:
                    if (CombatManager.Instance.player.positionY > entity.positionY &&
                        SecondaryPlayerCoordsY() > entity.positionY)
                    {
                        FullPlayerDetected(entity);
                    }
                    else
                    {
                        if (CombatManager.Instance.player.positionY > entity.positionY ||
                            SecondaryPlayerCoordsY() > entity.positionY)
                        {
                            HalfPlayerDetected(entity);
                        }
                    }

                    break;
                case EntityInstance.dir.down:
                    if (CombatManager.Instance.player.positionY < entity.positionY &&
                        SecondaryPlayerCoordsY() < entity.positionY)
                    {
                        FullPlayerDetected(entity);
                    }
                    else
                    {
                        if (CombatManager.Instance.player.positionY < entity.positionY ||
                            SecondaryPlayerCoordsY() < entity.positionY)
                        {
                            HalfPlayerDetected(entity);
                        }
                    }

                    break;
                case EntityInstance.dir.left:
                    if (CombatManager.Instance.player.positionX < entity.positionX &&
                        SecondaryPlayerCoordsX() < entity.positionX)
                    {
                        FullPlayerDetected(entity);
                    }
                    else
                    {
                        if (CombatManager.Instance.player.positionX < entity.positionX ||
                            SecondaryPlayerCoordsX() < entity.positionX)
                        {
                            HalfPlayerDetected(entity);
                        }
                    }

                    break;
                case EntityInstance.dir.right:
                    if (CombatManager.Instance.player.positionX > entity.positionX &&
                        SecondaryPlayerCoordsX() > entity.positionX)
                    {
                        FullPlayerDetected(entity);
                    }
                    else
                    {
                        if (CombatManager.Instance.player.positionX > entity.positionX ||
                            SecondaryPlayerCoordsX() > entity.positionX)
                        {
                            HalfPlayerDetected(entity);
                        }
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        void FullPlayerDetected(FishDataInstance entity)
        {
            entity.PreparingAttack = true;
        }

        void HalfPlayerDetected(FishDataInstance entity)
        {
            switch (entity.direction)
            {
                case EntityInstance.dir.up:
                    if (CombatManager.Instance.player.positionX > entity.positionX)
                    {
                        ShootSpike(entity, 1, 0);
                    }
                    else
                    {
                        ShootSpike(entity, -1, 0);
                    }

                    break;
                case EntityInstance.dir.down:
                    if (CombatManager.Instance.player.positionX > entity.positionX)
                    {
                        ShootSpike(entity, 1, 0);
                    }
                    else
                    {
                        ShootSpike(entity, -1, 0);
                    }

                    break;
                case EntityInstance.dir.left:
                    if (CombatManager.Instance.player.positionY > entity.positionY)
                    {
                        ShootSpike(entity, 0, 1);
                    }
                    else
                    {
                        ShootSpike(entity, 0, -1);
                    }

                    break;
                case EntityInstance.dir.right:
                    if (CombatManager.Instance.player.positionY > entity.positionY)
                    {
                        ShootSpike(entity, 0, 1);
                    }
                    else
                    {
                        ShootSpike(entity, 0, -1);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void ShootSpike(FishDataInstance entity, int dirX, int dirY)
        {
            if (CombatManager.Instance.grid[entity.positionY + dirY, entity.positionX + dirX] != null)
            {
                CombatManager.Instance.Damage(
                    CombatManager.Instance.grid[entity.positionY + dirY, entity.positionX + dirX], _spikeData.hp);
            }
            else
            {
                SpikeInstance spike = new SpikeInstance(_spikeData);
                entity.spikeList.Add(spike);
                spike.dirX = dirX;
                spike.dirY = dirY;
                spike.entity = entity;
                spike.positionY = entity.positionY + dirY;
                spike.positionX = entity.positionX + dirX;
                CombatManager.Instance.grid[spike.positionY, spike.positionX] = spike;
                int rotation;
                if (dirX == 1)
                {
                    rotation = -90;
                }
                else if (dirX == -1)
                {
                    rotation = 90;
                }
                else if (dirY == 1)
                {
                    rotation = 0;
                }
                else
                {
                    rotation = 180;
                }

                spike.prefab = Instantiate(spike.prefab, new Vector3(entity.positionX + dirX,
                    entity.positionY + dirY, 0), Quaternion.identity);
                spike.entityChild = spike.prefab.transform.GetChild(0);
                spike.entityChild.transform.rotation = Quaternion.Euler(0,0,rotation);
            }
        }

        int SecondaryPlayerCoordsX()
        {
            switch (CombatManager.Instance.player.direction)
            {
                case EntityInstance.dir.up:
                    return CombatManager.Instance.player.positionX;
                case EntityInstance.dir.down:
                    return CombatManager.Instance.player.positionX;
                case EntityInstance.dir.left:
                    return CombatManager.Instance.player.positionX - 1;
                case EntityInstance.dir.right:
                    return CombatManager.Instance.player.positionX + 1;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        int SecondaryPlayerCoordsY()
        {
            switch (CombatManager.Instance.player.direction)
            {
                case EntityInstance.dir.up:
                    return CombatManager.Instance.player.positionY + 1;
                case EntityInstance.dir.down:
                    return CombatManager.Instance.player.positionY - 1;
                case EntityInstance.dir.left:
                    return CombatManager.Instance.player.positionY;
                case EntityInstance.dir.right:
                    return CombatManager.Instance.player.positionY;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    public void TurnRight(FishDataInstance entity)
    {
        switch (entity.direction)
        {
            case EntityInstance.dir.up:
                entity.direction = EntityInstance.dir.right;
                entity.weakPointList.Clear();
                entity.weakPointList.Add(entity.WeakPointsRight[0]);
                break;
            case EntityInstance.dir.down:
                entity.direction = EntityInstance.dir.left;
                entity.weakPointList.Clear();
                entity.weakPointList.Add(entity.WeakPointsLeft[0]);
                break;
            case EntityInstance.dir.left:
                entity.direction = EntityInstance.dir.up;
                entity.weakPointList.Clear();
                entity.weakPointList.Add(entity.WeakPointsUp[0]);
                break;
            case EntityInstance.dir.right:
                entity.direction = EntityInstance.dir.down;
                entity.weakPointList.Clear();
                entity.weakPointList.Add(entity.WeakPointsDown[0]);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        entity.weakPointList.Add(DamageToAttacker);

        entity.entityChild.DOLocalRotate(new Vector3(0, 0, entity.entityChild.localEulerAngles.z - 90), 0.5f)
            .SetEase(Ease.InOutCubic);
    }
    

    public void TurnLeft(FishDataInstance entity)
    {
        Debug.Log("turningleft");
        switch (entity.direction)
        {
            case EntityInstance.dir.up:
                entity.direction = EntityInstance.dir.left;
                entity.weakPointList.Clear();
                entity.weakPointList.Add(entity.WeakPointsLeft[0]);
                break;
            case EntityInstance.dir.down:
                entity.direction = EntityInstance.dir.right;
                entity.weakPointList.Clear();
                entity.weakPointList.Add(entity.WeakPointsRight[0]);
                break;
            case EntityInstance.dir.left:
                entity.direction = EntityInstance.dir.down;
                entity.weakPointList.Clear();
                entity.weakPointList.Add(entity.WeakPointsDown[0]);
                break;
            case EntityInstance.dir.right:
                entity.direction = EntityInstance.dir.up;
                entity.weakPointList.Clear();
                entity.weakPointList.Add(entity.WeakPointsUp[0]);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        entity.weakPointList.Add(DamageToAttacker);

        entity.entityChild.DOLocalRotate(new Vector3(0, 0, entity.entityChild.localEulerAngles.z + 90), 0.5f)
            .SetEase(Ease.InOutCubic);
    }
    
    
}
