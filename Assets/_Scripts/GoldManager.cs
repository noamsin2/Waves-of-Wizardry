using UnityEngine;

public class GoldManager : MonoBehaviour
{
    const float BASE_GOLD_DROP_RATE = 0.2f;
    public static GoldManager Instance { get; private set; }

    [field: SerializeField] private float goldMultiplier;
    [field: SerializeField] private float goldDropRate;
    [SerializeField] private Transform prefabCoin;
    [SerializeField] private Transform prefabPile1;
    [SerializeField] private Transform prefabPile2;
    [SerializeField] private Transform prefabPile3;
    private void Awake()
    {
        Instance = this;
    }
    public void DropGold(int gold, Vector2 dropPosition)
    {
        if(Random.Range(1,101) <= goldDropRate * 100)
        {
            Transform goldObject = null;
            //calculate after multiplier
            int goldAmount = (int)(gold * (1 + goldMultiplier));
            //make a gold instance based on the amount
            if(goldAmount < 5)
                goldObject = Instantiate(prefabCoin, dropPosition, Quaternion.identity);
            else if(goldAmount >= 5 && goldAmount < 21)
                goldObject = Instantiate(prefabPile1, dropPosition, Quaternion.identity);
            else if(goldAmount >= 21 && goldAmount <= 50)
                goldObject = Instantiate(prefabPile2, dropPosition, Quaternion.identity);
            else if(goldAmount > 50)
                goldObject = Instantiate(prefabPile3, dropPosition, Quaternion.identity);
            goldObject.GetComponent<GoldDrop>().SetAmount(goldAmount);
            Destroy(goldObject.gameObject, 60f);
        }
    }
    public void IncreaseGoldMultiplier(float multiplier)
    {
        goldMultiplier += (multiplier / 100);
        Debug.Log(goldMultiplier);
    }
    public void IncreaseGoldDropRate(int rate)
    {
        goldDropRate += BASE_GOLD_DROP_RATE * (rate / 100);
    }
}
