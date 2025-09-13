using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraDamageEffects : MonoBehaviour
{
    [SerializeField] float Intensity = 0;
    [SerializeField] Color32 HealingColor;
    [SerializeField] Color32 DamageColor;
    [SerializeField] Color32 ChargeColor;
    PostProcessVolume volume;
    Vignette vignette;
    void Awake()
    {
        volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings<Vignette>(out vignette);
        if (!vignette)
            Debug.Log("no vignette found");
        else
        {
            // vignette.enabled.Override(false);
            vignette.intensity.Override(0);
            vignette.color.Override(DamageColor);
        }
    }
    public void TakeDamage()
    {
        // print("TakeDmage Called");
        if (vignette)
            StartCoroutine(takedamage());
    }
    IEnumerator takedamage()
    {
        if (Time.timeScale > 0)
        {
            // vignette.enabled.Override(true);
            // vignette.color.Override(DamageColor);
            float intensity = Intensity;
            while (intensity > 0)
            {
                vignette.intensity.Override(intensity);
                yield return new WaitForSecondsRealtime(0.5f);
                intensity -= 0.25f;
            }
            vignette.intensity.Override(0);
            // vignette.enabled.Override(false);
        }
        else
        {
            yield return new WaitForEndOfFrame();
        }
    }
    public void Heal()
    {
        print("heal called");
        if(vignette)
        StartCoroutine(heal());
    }
    IEnumerator heal()
    {
        // vignette.enabled.Override(true);
        vignette.color.Override(HealingColor);
        float intensity = Intensity;
        while(intensity > 0)
        {
            vignette.intensity.Override(intensity);
            yield return new WaitForSecondsRealtime(0.5f);
            intensity -= 0.25f;
        }
        vignette.intensity.Override(0);
        vignette.color.Override(DamageColor);
        // vignette.enabled.Override(false);
    }
    private void OnDisable() {
        if(vignette)
        {
            vignette.intensity.Override(0);
            vignette.color.Override(DamageColor);
            // vignette.enabled.Override(false);
        }
    }
}
