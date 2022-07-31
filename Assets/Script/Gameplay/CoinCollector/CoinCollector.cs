using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

public class CoinCollector : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<int> coins = new NetworkVariable<int>();
    [SerializeField] private TextMeshProUGUI coinText;

    [ServerRpc]
    public void currentCoinServerRpc(int value)
    {
        coins.Value += value;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (IsLocalPlayer)
        {
            coinText = GameObject.Find("CoinText").GetComponent<TextMeshProUGUI>();
            coins.OnValueChanged += CoinTextChange;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (IsLocalPlayer && coinText == null)
        {
            coinText = GameObject.Find("CoinText").GetComponent<TextMeshProUGUI>();
            coins.OnValueChanged += CoinTextChange;
        }

    }
    private void CoinTextChange(int previousValue, int newValue)
    {
        coinText.text = "Coins : "+ newValue.ToString();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!IsLocalPlayer) { return; }
        Coin coin = collision.gameObject.GetComponent<Coin>();
        if (coin == null) { return; }
        currentCoinServerRpc(coin.prize);
        Destroy(collision.gameObject);
   
    }
}
