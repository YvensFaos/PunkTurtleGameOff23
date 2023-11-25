namespace Core
{
    public class InvincibleCollectableControl : CollectableControl
    {
        protected override void CollectableEffect(PlayerControl player)
        {
            player.ActivateInvincibility();
        }
    }
}