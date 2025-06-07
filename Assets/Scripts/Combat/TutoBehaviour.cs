using System.Xml;
using UnityEngine;

namespace Combat
{
    public class TutoBehaviour : AbstractIA
    {
        public override void ManageTurn(FishDataInstance entity)
        {
            //player au dessus
            if (CombatManager.Instance.player.positionY > entity.positionY+1 && 
                CombatManager.Instance.SecondaryPlayerCoordsY() > entity.positionY + 1)
            {
                if (CombatManager.Instance.player.positionX <= entity.positionX &&
                    CombatManager.Instance.SecondaryPlayerCoordsX() <= entity.positionX)
                {
                    Move(entity.positionX-1, entity.positionY, entity);
                }
                else if (CombatManager.Instance.player.positionX >= entity.positionX + 1 &&
                         CombatManager.Instance.SecondaryPlayerCoordsX() >= entity.positionX + 1)
                {
                    Move(entity.positionX+1, entity.positionY, entity);
                }
                else
                {
                    Move(entity.positionX, entity.positionY+1, entity);
                }
            }
            //player en dessous
            else if (CombatManager.Instance.player.positionY < entity.positionY && 
                     CombatManager.Instance.SecondaryPlayerCoordsY() < entity.positionY)
            {
                if (CombatManager.Instance.player.positionX <= entity.positionX &&
                    CombatManager.Instance.SecondaryPlayerCoordsX() <= entity.positionX)
                {
                    Move(entity.positionX+1, entity.positionY, entity);
                }
                else if (CombatManager.Instance.player.positionX >= entity.positionX + 1 &&
                         CombatManager.Instance.SecondaryPlayerCoordsX() >= entity.positionX + 1)
                {
                    Move(entity.positionX-1, entity.positionY, entity);
                }
                else
                {
                    Move(entity.positionX, entity.positionY+1, entity);
                }
            }
            //player sur le coté
            else
            {
                if (CombatManager.Instance.player.positionX <= entity.positionX &&
                    CombatManager.Instance.SecondaryPlayerCoordsX() <= entity.positionX)
                {
                    Move(entity.positionX-1, entity.positionY, entity);
                }
                else if (CombatManager.Instance.player.positionX >= entity.positionX + 1 &&
                         CombatManager.Instance.SecondaryPlayerCoordsX() >= entity.positionX + 1)
                {
                    Move(entity.positionX+1, entity.positionY, entity);
                }
                else
                {
                    //rien
                }
            }
            if (CombatManager.Instance.grid[entity.positionY, entity.positionX - 1] == CombatManager.Instance.player ||
                CombatManager.Instance.grid[entity.positionY + 1, entity.positionX - 1] == CombatManager.Instance.player )
            {
                if (CombatManager.Instance.CanMove(CombatManager.Instance.player,
                        CombatManager.Instance.player.positionX - 2
                        , CombatManager.Instance.player.positionY))
                {
                    entity.Animator.SetTrigger("isAttacking");
                    CombatManager.Instance.Move(CombatManager.Instance.player,
                        CombatManager.Instance.player.positionX - 2
                        , CombatManager.Instance.player.positionY);
                }
                else if (CombatManager.Instance.CanMove(CombatManager.Instance.player,
                             CombatManager.Instance.player.positionX - 1
                             , CombatManager.Instance.player.positionY))
                {
                    entity.Animator.SetTrigger("isAttacking");
                    CombatManager.Instance.Move(CombatManager.Instance.player,
                        CombatManager.Instance.player.positionX - 1
                        , CombatManager.Instance.player.positionY);
                }
            }
        }
        void Move(int newX, int newY, EntityInstance entity)
        {
            if (newX > 0 && newY > 0 && newX < 4 && newY < 3)
            {
                CombatManager.Instance.Move(entity, newX, newY);
            }
            else
            {
                Debug.Log("canot Move");
            }
        }
    }
}