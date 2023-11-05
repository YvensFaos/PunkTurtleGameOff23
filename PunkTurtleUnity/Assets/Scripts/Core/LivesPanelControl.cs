using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Lean.Pool;
using UnityEngine;
using UnityEngine.UI;

public class LivesPanelControl : MonoBehaviour
{
    [SerializeField]
    private Image livePrefab;
    [SerializeField]
    private List<Image> liveImages;

    public void UpdateLives(int lives)
    {
        var currentLives = liveImages.Count;

        if (currentLives < lives)
        {
            //Add live
            var difference = lives - currentLives;
            for (var i = 0; i < difference; i++)
            {
                SpawnLive();
            }
        } 
        else if (currentLives > lives)
        {
            //Remove live
            var difference = currentLives - lives;
            var index = liveImages.Count() - 1;
            for (var i = 0; i < difference; i++)
            {
                DespawnLive(index--);
            }
        }
    }

    private void SpawnLive()
    {
        var newLive = LeanPool.Spawn(livePrefab, transform);
        newLive.rectTransform.localScale = new Vector3(0, 0, 0);
        newLive.fillAmount = 0.0f;
        newLive.rectTransform.DOScale(new Vector3(1, 1, 1), 0.3f);
        newLive.DOFillAmount(1.0f, 0.3f);
        liveImages.Add(newLive);
    }

    // ReSharper disable once IdentifierTypo
    private void DespawnLive(int index)
    {
        var removeLive = liveImages[index];
        liveImages.RemoveAt(index);
        removeLive.rectTransform.DOScale(new Vector3(0, 0, 0), 0.3f);
        removeLive.DOFillAmount(0.0f, 0.3f).OnComplete(() =>
        {
            LeanPool.Despawn(removeLive);
        });
    }
}
