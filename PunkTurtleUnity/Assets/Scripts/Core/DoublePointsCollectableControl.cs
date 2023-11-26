namespace Core
{
    public class DoublePointsCollectableControl : CollectableControl
    {
        protected override void CollectableEffect(PlayerControl player)
        {
            player.AudioControl().PlayCollectable();
            player.ActivateDoublePoints();
        }
    }
}