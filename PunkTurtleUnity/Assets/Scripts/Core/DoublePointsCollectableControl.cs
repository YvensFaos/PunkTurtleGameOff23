namespace Core
{
    public class DoublePointsCollectableControl : CollectableControl
    {
        protected override void CollectableEffect(PlayerControl player)
        {
            player.ActivateDoublePoints();
        }
    }
}