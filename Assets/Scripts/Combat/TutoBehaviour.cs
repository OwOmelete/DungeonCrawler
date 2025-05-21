namespace Combat
{
    public class TutoBehaviour : AbstractIA
    {
        
        public override void ManageTurn(FishDataInstance entity)
        {
            if (CombatManager.Instance.grid[entity.positionY, entity.positionX - 1] == CombatManager.Instance.player ||
                CombatManager.Instance.grid[entity.positionY + 1, entity.positionX - 1] == CombatManager.Instance.player )
            {
                if (CombatManager.Instance.CanMove(CombatManager.Instance.player,
                        CombatManager.Instance.player.positionX - 2
                        , CombatManager.Instance.player.positionY))
                {
                    CombatManager.Instance.Move(CombatManager.Instance.player,
                        CombatManager.Instance.player.positionX - 2
                        , CombatManager.Instance.player.positionY);
                }
                else if (CombatManager.Instance.CanMove(CombatManager.Instance.player,
                             CombatManager.Instance.player.positionX - 1
                             , CombatManager.Instance.player.positionY))
                {
                    CombatManager.Instance.Move(CombatManager.Instance.player,
                        CombatManager.Instance.player.positionX - 1
                        , CombatManager.Instance.player.positionY);
                }
            }
        }
    }
}