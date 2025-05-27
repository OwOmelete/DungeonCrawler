using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RadarScript : MonoBehaviour
{
    [SerializeField] private Transform indicatorHealTransform;
    [SerializeField] private Transform indicatorLightTransform;
    [SerializeField] private Transform indicatorOxygenTransform;
    [SerializeField] private Transform healParent;
    [SerializeField] private Transform lightParent;
    [SerializeField] private Transform oxygenParent;
    [SerializeField] private float maxRadarTime = 5f;
    [SerializeField] private float cooldown = 20f;
    [SerializeField] private Slider sliderRef;
    private bool isActive;
    private bool alreadyInCooldown;
    

    void CheckNearestObject(Transform parent, Transform indicatorTransform)
    {
        Transform nearestObject = null;
        float minSqrDistance = float.MaxValue;
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).gameObject.activeSelf)
            {
                Transform currentObject = parent.GetChild(i);
                float sqrDistance = (currentObject.position - indicatorTransform.position).sqrMagnitude;

                if (sqrDistance < minSqrDistance)
                {
                    minSqrDistance = sqrDistance;
                    nearestObject = currentObject;
                }
            }
        }
        if (nearestObject != null)
        {
            Vector3 direction = nearestObject.position - indicatorTransform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            indicatorTransform.DORotate(new Vector3(0f, 0f, angle), 0.2f);
        }
    }
    

    public void ChangeRadarState()
    {
        if (!alreadyInCooldown)
        {
            indicatorHealTransform.localScale = Vector3.zero;
            indicatorLightTransform.localScale = Vector3.zero;
            indicatorOxygenTransform.localScale = Vector3.zero;
            isActive = true;
            indicatorHealTransform.DOScale(Vector3.one, 0.5f);
            indicatorLightTransform.DOScale(Vector3.one, 0.5f);
            indicatorOxygenTransform.DOScale(Vector3.one, 0.5f);
            StartCoroutine(UpdateRadar());
            StartCoroutine(MaxTime());
            indicatorHealTransform.gameObject.SetActive(true);
            indicatorLightTransform.gameObject.SetActive(true);
            indicatorOxygenTransform.gameObject.SetActive(true);
            
        }
        
    }

    IEnumerator UpdateRadar()
    {
        while (isActive)
        {
            CheckNearestObject(healParent, indicatorHealTransform);
            CheckNearestObject(lightParent, indicatorLightTransform);
            CheckNearestObject(oxygenParent, indicatorOxygenTransform);
            yield return null;
        }
        indicatorHealTransform.gameObject.SetActive(false);
        indicatorLightTransform.gameObject.SetActive(false);
        indicatorOxygenTransform.gameObject.SetActive(false);
    }

    private IEnumerator MaxTime()
    {
        alreadyInCooldown = true;
        sliderRef.DOValue(0, maxRadarTime).SetEase(Ease.Linear);
        yield return new WaitForSeconds(maxRadarTime);
        indicatorHealTransform.DOScale(Vector3.zero, 0.5f);
        indicatorLightTransform.DOScale(Vector3.zero, 0.5f);
        indicatorOxygenTransform.DOScale(Vector3.zero, 0.5f);
        yield return new WaitForSeconds(0.5f);
        isActive = false;
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        sliderRef.DOValue(1, cooldown).SetEase(Ease.Linear);
        yield return new WaitForSeconds(cooldown);
        alreadyInCooldown = false;
    }
}
