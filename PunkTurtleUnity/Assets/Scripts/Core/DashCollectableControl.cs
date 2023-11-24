namespace Core
{
    public class DashCollectableControl : CollectableControl
    {
        protected override void CollectableEffect(PlayerControl player)
        {
            player.ActivateDash();
        }
    }
}