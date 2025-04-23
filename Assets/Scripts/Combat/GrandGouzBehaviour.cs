using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GrandGouzBehaviour : AbstractIA
{

    public override void ManageTurn(FishDataInstance entity)
    {
        if (entity.FirstCycle)
        {
            entity.sr = entity.prefab.GetComponentInChildren<SpriteRenderer>();
            entity.PreparingAttack = false;
            entity.Flipped = false;
            entity.FirstCycle = false;
        }
        entity.HasAttacked = false;
        if (entity.PreparingAttack)
        {
            Attack(entity);
            Debug.Log("attacking");
        }
        else if (!entity.HasAttacked)
        {
            PlayerDetection(entity);
        }
        if(!entity.HasAttacked && !entity.PreparingAttack)
        {
            if (entity.positionY == CombatManager.Instance.player.positionY ||
                entity.positionY == SecondaryPlayerCoordsY())
            {
                if (CombatManager.Instance.player.positionX > entity.positionX)
                {
                    if (entity.Flipped)
                    {
                        Flipping(entity);
                    }
                    else
                    {
                        CombatManager.Instance.Move(entity, entity.positionX + 1, entity.positionY);
                    }
                }
                else
                {
                    if (!entity.Flipped)
                    {
                        Flipping(entity);
                    }
                    else
                    {
                        CombatManager.Instance.Move(entity, entity.positionX - 1, entity.positionY);
                    }
                }
            }
            else if (entity.positionX == CombatManager.Instance.player.positionX ||
                     entity.positionX == SecondaryPlayerCoordsX() ||
                     entity.positionX + 1 == CombatManager.Instance.player.positionX ||
                     entity.positionX + 1 == SecondaryPlayerCoordsX())
            {
                if (entity.positionX < 2)
                {
                    if (CombatManager.Instance.player.positionX > 1 && SecondaryPlayerCoordsX() > 1 && entity.Flipped)
                    {
                        CombatManager.Instance.Move(entity, entity.positionX - 1, entity.positionY);
                    }
                    else
                    {
                        CombatManager.Instance.Move(entity, entity.positionX + 1, entity.positionY);
                    }
                }
                else if (entity.positionX + 1 >= CombatManager.Instance.grid.GetLength(1) - 2)
                {
                    if (CombatManager.Instance.player.positionX < CombatManager.Instance.grid.GetLength(1) - 2 &&
                        SecondaryPlayerCoordsX() < CombatManager.Instance.grid.GetLength(1) - 2 && !entity.Flipped)
                    {
                        CombatManager.Instance.Move(entity, entity.positionX + 1, entity.positionY);
                    }
                    else
                    {
                        CombatManager.Instance.Move(entity, entity.positionX - 1, entity.positionY);
                    }
                }
                else
                {
                    if (entity.Flipped)
                    {
                        CombatManager.Instance.Move(entity, entity.positionX - 1, entity.positionY);
                    }
                    else
                    {
                        CombatManager.Instance.Move(entity, entity.positionX + 1, entity.positionY);

                    }
                }
            }
            else if (CombatManager.Instance.player.positionY > entity.positionY)
            {
                if (CombatManager.Instance.grid[entity.positionY + 1, entity.positionX] == null &&
                    CombatManager.Instance.grid[entity.positionY + 1, entity.positionX + 1] == null)
                {
                    CombatManager.Instance.Move(entity, entity.positionX, entity.positionY + 1);
                }
            }
            else if (CombatManager.Instance.player.positionY < entity.positionY)
            {
                if (CombatManager.Instance.grid[entity.positionY - 1, entity.positionX] == null &&
                    CombatManager.Instance.grid[entity.positionY - 1, entity.positionX + 1] == null)
                {
                    CombatManager.Instance.Move(entity, entity.positionX, entity.positionY - 1);
                }
            }
            else
            {
                Debug.Log("pas d'action");
            }

            PlayerDetection(entity);
            entity.HasAttacked = false;
        }

        void PlayerDetection(FishDataInstance entity)
        {
            if (!entity.HasAttacked)
            {
                if (!entity.Flipped)
                {
                    if (CombatManager.Instance.player.positionX == entity.positionX + 2 ||
                        SecondaryPlayerCoordsX() == entity.positionX + 2)
                    {
                        if ((SecondaryPlayerCoordsY() <= entity.positionY + 1 &&
                             SecondaryPlayerCoordsY() >= entity.positionY - 1) ||
                            (CombatManager.Instance.player.positionY <= entity.positionY + 1 &&
                             CombatManager.Instance.player.positionY >= entity.positionY - 1))
                        {
                            entity.PreparingAttack = true;
                            UpdateWeakPoints(entity);

                            Debug.Log("detection");

                        }
                    }
                }
                else
                {
                    if (CombatManager.Instance.player.positionX == entity.positionX - 1 ||
                        SecondaryPlayerCoordsX() == entity.positionX - 1)
                    {
                        if ((SecondaryPlayerCoordsY() <= entity.positionY + 1 &&
                             SecondaryPlayerCoordsY() >= entity.positionY - 1) ||
                            (CombatManager.Instance.player.positionY <= entity.positionY + 1 &&
                             CombatManager.Instance.player.positionY >= entity.positionY - 1))
                        {
                            entity.PreparingAttack = true;
                            UpdateWeakPoints(entity);

                            Debug.Log("detection");
                        }
                    }
                }
            }
        }

        void UpdateWeakPoints(FishDataInstance entity)
        {
            entity.weakPointList.Clear();
            if (!entity.Flipped)
            {
                if (entity.PreparingAttack)
                {
                    entity.weakPointList.Add(entity.WeakPointsRight[0]);
                }
                else
                {
                    entity.weakPointList.Add(entity.WeakPointsRight[1]);
                    entity.weakPointList.Add(entity.WeakPointsRight[2]);
                }
            }
            else
            {
                if (entity.PreparingAttack)
                {
                    entity.weakPointList.Add(entity.WeakPointsLeft[0]);
                }
                else
                {
                    entity.weakPointList.Add(entity.WeakPointsLeft[1]);
                    entity.weakPointList.Add(entity.WeakPointsLeft[2]);

                }
            }
        }

        void Flipping(FishDataInstance entity)
        {
            entity.sr.flipX = !entity.sr.flipX;
            entity.Flipped = !entity.Flipped;
            UpdateWeakPoints(entity);
        }

        void Attack(FishDataInstance entity)
        {
            if (entity.Flipped)
            {
                foreach (AttackData attack in entity.attacks)
                {
                    CombatManager.Instance.Attack(attack, entity, entity.positionX - 1, entity.positionY);
                }
            }
            else
            {
                foreach (AttackData attack in entity.attacks)
                {
                    CombatManager.Instance.Attack(attack, entity, entity.positionX + 1, entity.positionY);
                }
            }

            entity.PreparingAttack = false;
            entity.HasAttacked = true;
            UpdateWeakPoints(entity);
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
}
