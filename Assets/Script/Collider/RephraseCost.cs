using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RephraseCost : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("tmpText")]    public TextMeshPro JarCost;
[UnityEngine.Serialization.FormerlySerializedAs("Index")]
    public int Adapt;
    private void Start()
    {
        if (JarCost != null)
        {
            JarCost.color = Color.gray;
        }
        
    }

    public void CodAdapt(int index)
    {
        Adapt = index;
        JarCost = GetComponent<TextMeshPro>();
        JarCost.text = index.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Car"))
        {
            return;
        }
        Debug.Log("=====================������ײ");
        JarCost.color = Color.white;
        RWarmDNA.Instance.WarmAlarmRecoil = Adapt;
    }
}
