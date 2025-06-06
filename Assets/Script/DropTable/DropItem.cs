using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField]private DropTable dropTable;
    [SerializeField]private GameObject itemPrefab;
    [SerializeField]private List<ItemData> DropList;
    private bool IsDropping = false;
    public void ItemDrop(Transform transform)
    {
        if (!IsDropping)
        {
            IsDropping=true;
            Table table = dropTable.LootingTable[0];
            DropList = dropTable.RollLoot(table);

            foreach (ItemData item in DropList)
            {
                GameObject obj = Instantiate(itemPrefab, transform.position, Quaternion.identity);
                obj.GetComponent<WorldItem>().SetItem(item, 1);
                Rigidbody2D rb = obj.AddComponent<Rigidbody2D>();
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                if (rb != null)
                    StartCoroutine(ApplyFloatDrop(rb));

            }
        }
    }

    public void DropSpecficItem(ItemData item)
    {
        if(item == null) return;
        GameObject obj = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        obj.GetComponent<WorldItem>().SetItem(item, 1);
        Rigidbody2D rb = obj.AddComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (rb != null)
            StartCoroutine(ApplyFloatDrop(rb));

    }
    private IEnumerator ApplyFloatDrop(Rigidbody2D rb)
    {

        Vector2 force = new Vector2(UnityEngine.Random.Range(-1f, 1f), 1.5f).normalized * 3f;
        if (rb == null)
            yield break;
        rb.AddForce(force, ForceMode2D.Impulse);


        yield return new WaitForSeconds(0.2f);
        if (rb == null)
            yield break;
        rb.gravityScale = 1f;


        yield return new WaitForSeconds(0.7f);
        if (rb == null)
            yield break;
        rb.gravityScale = 0f; rb.velocity = Vector2.zero;
    }




}
