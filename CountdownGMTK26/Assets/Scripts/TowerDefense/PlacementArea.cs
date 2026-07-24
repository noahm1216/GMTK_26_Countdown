using UnityEngine;

public class PlacementArea : MonoBehaviour
{
    void OnMouseUp()
    {
        Debug.Log("Placement area clicked");
        buyTroop();
    }

     public void buyTroop()
    {
        BuildingManager bm = GameObject.FindObjectsByType<BuildingManager>()[0];
        if (bm.selectedTroop != null)
        {
            TD_ScoreManager sm = GameObject.FindObjectsByType<TD_ScoreManager>()[0];
            if (sm.currency < bm.selectedTroop.GetComponent<Troop>().cost)
            {
                Debug.Log("Not enough currency.");
                return;
            }
            // Instantiate the selected troop at the placement area
            Debug.Log("Attempting to place troop.");
            Instantiate(bm.selectedTroop, this.transform.parent.position, this.transform.parent.rotation);
            GameObject.FindObjectsByType<TD_ScoreManager>()[0].currency -= bm.selectedTroop.GetComponent<Troop>().cost;
            Destroy(this.transform.parent.gameObject);
        }
    }


}
