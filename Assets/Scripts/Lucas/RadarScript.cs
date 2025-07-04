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
    [SerializeField] private Transform indicatorPathTransform;
    [SerializeField] private GameObject cooldownPlayerGO;
    [SerializeField] private Transform healParent;
    [SerializeField] private Transform lightParent;
    [SerializeField] private Transform oxygenParent;
    [SerializeField] private Transform pathParent;
    [SerializeField] private LightManager lightManagerRef;
    [SerializeField] private float looseLightValue = 1f;
    [SerializeField] private GameObject triggerTextRef;
    private float maxRadarTime = 8f;
    private bool isActive;
    
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
        if (!isActive)
        {
            lightManagerRef.canLooseLight = false;
            lightManagerRef.AddLight(-looseLightValue);
            indicatorHealTransform.localScale = Vector3.zero;
            indicatorLightTransform.localScale = Vector3.zero;
            indicatorOxygenTransform.localScale = Vector3.zero;
            indicatorPathTransform.localScale = Vector3.zero;
            cooldownPlayerGO.transform.localScale = Vector3.zero;
            isActive = true;
            indicatorHealTransform.DOScale(Vector3.one, 0.5f);
            indicatorLightTransform.DOScale(Vector3.one, 0.5f);
            indicatorOxygenTransform.DOScale(Vector3.one, 0.5f);
            indicatorPathTransform.DOScale(Vector3.one, 0.5f);
            cooldownPlayerGO.transform.DOScale(Vector3.one, 0.5f);
            
            StartCoroutine(UpdateRadar());
            StartCoroutine(MaxTime());
            indicatorHealTransform.gameObject.SetActive(true);
            indicatorLightTransform.gameObject.SetActive(true);
            indicatorOxygenTransform.gameObject.SetActive(true);
            indicatorPathTransform.gameObject.SetActive(true);
            cooldownPlayerGO.SetActive(true);
            if (InteractTextManager.INSTANCE.firstRadar)
            {
                InteractTextManager.INSTANCE.firstRadar = false;
                triggerTextRef.SetActive(true);
            }
        }
    }

    IEnumerator UpdateRadar()
    {
        while (isActive)
        {
            CheckNearestObject(healParent, indicatorHealTransform);
            CheckNearestObject(lightParent, indicatorLightTransform);
            CheckNearestObject(oxygenParent, indicatorOxygenTransform);
            CheckNearestObject(pathParent, indicatorPathTransform);
            yield return null;
        }
        indicatorHealTransform.gameObject.SetActive(false);
        indicatorLightTransform.gameObject.SetActive(false);
        indicatorOxygenTransform.gameObject.SetActive(false);
        indicatorPathTransform.gameObject.SetActive(false);
        cooldownPlayerGO.SetActive(false);
    }

    private IEnumerator MaxTime()
    {
        yield return new WaitForSeconds(maxRadarTime);
        indicatorHealTransform.DOScale(Vector3.zero, 0.5f);
        indicatorLightTransform.DOScale(Vector3.zero, 0.5f);
        indicatorOxygenTransform.DOScale(Vector3.zero, 0.5f);
        indicatorPathTransform.DOScale(Vector3.zero, 0.5f);
        cooldownPlayerGO.transform.DOScale(Vector3.zero, 0.5f);
        yield return new WaitForSeconds(0.5f);
        isActive = false;
    }

    
}
