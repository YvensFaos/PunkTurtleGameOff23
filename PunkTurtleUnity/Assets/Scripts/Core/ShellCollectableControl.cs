namespace Core
{
    public class ShellCollectableControl : CollectableControl
    {
        protected override void CollectableEffect(PlayerControl player)
        {
            player.AudioControl().PlayCollectable();
            player.GetShell();
        }
    }
}