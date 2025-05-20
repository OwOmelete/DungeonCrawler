using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

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
    private bool isActive;
    private bool alreadyInCooldown;
    

    void CheckNearestObject(Transform parent, Transform indicatorTransform)
    {
        Transform nearestObject = null;
        float minSqrDistance = float.MaxValue;
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform currentObject = parent.GetChild(i);
            float sqrDistance = (currentObject.position - indicatorTransform.position).sqrMagnitude;

            if (sqrDistance < minSqrDistance)
            {
                minSqrDistance = sqrDistance;
                nearestObject = currentObject;
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
            isActive = true;
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
        yield return new WaitForSeconds(maxRadarTime);
        isActive = false;
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
        alreadyInCooldown = false;
    }
}
