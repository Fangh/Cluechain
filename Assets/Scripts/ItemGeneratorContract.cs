using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.Encoders;
using Nethereum.Signer;
using Nethereum.Hex.HexConvertors.Extensions;
using UnityEngine;
using System.Collections.Generic;
using Nethereum.JsonRpc.UnityClient;
using System.Collections;
using System.Threading.Tasks;

public class ItemGeneratorContract : MonoBehaviour
{
	public static string ABI = @"[{""constant"":true,""inputs"":[{""name"":"""",""type"":""address""}],""name"":""holders"",""outputs"":[{""name"":"""",""type"":""address""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[{""name"":""to"",""type"":""address""},{""name"":""itemFrom"",""type"":""address""},{""name"":""itemTo"",""type"":""address""}],""name"":""proposeExchange"",""outputs"":[{""name"":"""",""type"":""bytes32""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""nbDeal"",""outputs"":[{""name"":"""",""type"":""uint256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""itemListLength"",""outputs"":[{""name"":"""",""type"":""uint256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[{""name"":""from"",""type"":""address""},{""name"":""itemFrom"",""type"":""address""},{""name"":""itemTo"",""type"":""address""}],""name"":""acceptExchange"",""outputs"":[{""name"":"""",""type"":""bytes32""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":false,""inputs"":[{""name"":""item"",""type"":""address""}],""name"":""addItem"",""outputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[{""name"":"""",""type"":""bytes32""}],""name"":""deals"",""outputs"":[{""name"":""exist"",""type"":""bool""},{""name"":""from"",""type"":""address""},{""name"":""to"",""type"":""address""},{""name"":""fromOk"",""type"":""bool""},{""name"":""toOk"",""type"":""bool""},{""name"":""itemFrom"",""type"":""address""},{""name"":""itemTo"",""type"":""address""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[{""name"":""to"",""type"":""address""},{""name"":""itemAddr"",""type"":""address""}],""name"":""delegate"",""outputs"":[{""name"":""onlyOwner"",""type"":""address""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""owner"",""outputs"":[{""name"":"""",""type"":""address""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[{""name"":"""",""type"":""uint256""}],""name"":""items"",""outputs"":[{""name"":"""",""type"":""address""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""inputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""constructor""}]";
	private static string contractAddress = "0xe25f8387b587169d800ee4cf214bcae4ae2561c4";
	private Contract contract;

	public List<string> itemAdresses = new List<string>();
	public int itemListLength = 0;
	public List<Item> itemDatabase = new List<Item>();
	public bool AllItemsGetted = false;

	public static ItemGeneratorContract Instance;

	public ItemGeneratorContract () 
	{
		this.contract = new Contract (null, ABI, contractAddress);
		Instance = this;
	}

	void Update()
	{
		if ( !AllItemsGetted && itemListLength != 0 && itemAdresses.Count == itemListLength)
		{
			AllItemsGetted = true;
			GenerateItems();
		}
	}

	public void ApproveExchange(Item myItem, Item theirItem)
	{
		StartCoroutine( AcceptExchange(Inventory.Instance.playerAdress, Inventory.Instance.playerPrivateKey, theirItem.owner, theirItem.adress, myItem.adress ) );		
	}

	public Item GetItemByName(string name)
	{
		for (int i = 0; i < itemDatabase.Count; i++)
		{
			if ( itemDatabase[i].name == name)
				return itemDatabase[i];
		}
		Debug.LogError("There is no object with this name");
		return null;
	}
	
	public Item GetItemByAdress(string _adress)
	{
		for (int i = 0; i < itemDatabase.Count; i++)
		{
			if ( itemDatabase[i].adress == _adress)
				return itemDatabase[i];
		}
		Debug.LogError("There is no object with this adress");
		return null;
	}

	[ContextMenu ("get item list length")]
	public void GetItemsListLengthFromBC()
	{
		StartCoroutine( GetItemsListLength() );
	}

	[ContextMenu ("Get items from Blockchains")]
	public void GetItemsFromBC()
	{
		for (int i = 0; i < itemListLength; i++)
		{
			StartCoroutine( GetItems(i) );
		}
		// FilterItems();
		// FillItemAdresses();
		// create every items from adresses ?
	}

	public void GenerateItems()
	{
		for (int i = 0; i<itemAdresses.Count; i++)
		{		
			itemDatabase.Add( Item.CreateInstance(i, itemAdresses[i], this) );
		}
	}

	#region list of all functions in the contract
	
	public Function ContractGetItems () {
		return contract.GetFunction ("items");
	}
	
	public Function ContractItemsListLength () {
		return contract.GetFunction ("itemListLength");
	}
	
	public Function ContractProposeExchange () {
		return contract.GetFunction ("proposeExchange");
	}
	
	public Function ContractAcceptExchange () {
		return contract.GetFunction ("acceptExchange");
	}

	#endregion

	#region list of all the callings

	public CallInput CallGetItems (int index) 
	{
		var function = ContractGetItems ();
		return function.CreateCallInput (index);
	}
	
	public CallInput CallItemsListLength () 
	{
		var function = ContractItemsListLength ();
		return function.CreateCallInput ();
	}
	
	public CallInput CallProposeExchange () 
	{
		var function = ContractProposeExchange ();
		return function.CreateCallInput ();
	}
	
	public CallInput CallAcceptExchange () 
	{
		var function = ContractAcceptExchange ();
		return function.CreateCallInput ();
	}

	#endregion


	#region list of all transaction functions

 	public TransactionInput TransactionProposeExchange (
		string addressFrom,
		string privateKey,
		string toOwner,
		string myItem,
		string theirItem,
		HexBigInteger gas = null,
		HexBigInteger gasPrice = null,
		HexBigInteger valueAmount = null) {

		var function = ContractProposeExchange ();
		return function.CreateTransactionInput (addressFrom, gas, gasPrice, valueAmount, toOwner, myItem, theirItem);
	} 

 	public TransactionInput TransactionAcceptExchange (
		string addressFrom,
		string privateKey,
		string fromOwner,
		string theirItem,
		string myItem,
		HexBigInteger gas = null,
		HexBigInteger gasPrice = null,
		HexBigInteger valueAmount = null) {

		var function = ContractAcceptExchange ();
		return function.CreateTransactionInput (addressFrom, gas, gasPrice, valueAmount, fromOwner, myItem, theirItem);
	} 

	#endregion


	#region list of all IENumerator

	public IEnumerator ProposeExchange (string accountAddress, string accountPrivateKey, string toOwner, string myItem, string theirItem) 
	{
		var transactionInput = TransactionProposeExchange (
			accountAddress,
			accountPrivateKey,
			toOwner,
			myItem,
			theirItem,
			new HexBigInteger (50000),
			new HexBigInteger (0),
			new HexBigInteger (0)
		);

		var transactionSignedRequest = new TransactionSignedUnityRequest (Inventory.URL, accountPrivateKey, accountAddress);

		// Then we send it and wait
		Debug.Log("Sending propose Exchange transaction (to = "+ toOwner + " myItem = " + myItem + " theirItem = " + theirItem);
		yield return transactionSignedRequest.SignAndSendTransaction (transactionInput);
		if (transactionSignedRequest.Exception == null) {
			// If we don't have exceptions we just display the result, congrats!
			Debug.Log ("propose Exchange transaction submitted: " + transactionSignedRequest.Result);
		} else {
			// if we had an error in the UnityRequest we just display the Exception error
			Debug.Log ("Error submitting propose Exchange transaction: " + transactionSignedRequest.Exception.Message);
		}
	}

	public IEnumerator AcceptExchange (string accountAddress, string accountPrivateKey, string fromOwner, string theirItem, string myItem) 
	{
		var transactionInput = TransactionAcceptExchange (
			accountAddress,
			accountPrivateKey,
			fromOwner,
			theirItem,
			myItem,
			new HexBigInteger (50000),
			new HexBigInteger (0),
			new HexBigInteger (0)
		);

		var transactionSignedRequest = new TransactionSignedUnityRequest (Inventory.URL, accountPrivateKey, accountAddress);

		// Then we send it and wait
		Debug.Log("Sending AcceptExchange transaction (from = "+ fromOwner + " theirItem = " + theirItem + " myItem = " + myItem);
		yield return transactionSignedRequest.SignAndSendTransaction (transactionInput);
		if (transactionSignedRequest.Exception == null) {
			// If we don't have exceptions we just display the result, congrats!
			Debug.Log ("AcceptExchange transaction submitted: " + transactionSignedRequest.Result);
		} else {
			// if we had an error in the UnityRequest we just display the Exception error
			Debug.Log ("Error submitting AcceptExchange transaction: " + transactionSignedRequest.Exception.Message);
		}
	}

	
	public IEnumerator GetItemsListLength () 
	{
		var request = new EthCallUnityRequest (Inventory.URL);

		var callInput = CallItemsListLength ();
		Debug.Log ("Getting item list length...");

		yield return request.SendRequest (callInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest ());

		if (request.Exception == null) 
		{
			var function = ContractItemsListLength();
			// itemListLength = System.Int32.Parse( request.Result, NumberStyles.AllowHexSpecifier );
			Debug.Log("string of result = " +  ResultToInt(function, request.Result));
			itemListLength = ResultToInt(function, request.Result);
			Debug.Log ("item list length (HEX): " + request.Result);
			Debug.Log ("item list length (int):" + itemListLength);
		}
		else 
		{
			// if we had an error in the UnityRequest we just display the Exception error
			Debug.Log ("Error submitting item list length tx: " + request.Exception.Message);
		}
	}
	
	public IEnumerator GetItems (int index) 
	{
		var request = new EthCallUnityRequest (Inventory.URL);

		var callInput = CallGetItems (index);
		Debug.Log ("Getting all items...");

		yield return request.SendRequest (callInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest ());

		if (request.Exception == null) 
		{
			// If we don't have exceptions we just display the raw result and the
			// result decode it with our function (decodePings) from the service, congrats!
			var function = ContractGetItems();
			itemAdresses.Add(ResultToString(function, request.Result));
			// allItemsInOneString = 
			Debug.Log ("items (HEX): " + request.Result);
			Debug.Log ("items (string):" + itemAdresses[itemAdresses.Count - 1]);
		}
		else 
		{
			// if we had an error in the UnityRequest we just display the Exception error
			Debug.Log ("Error submitting item list length tx: " + request.Exception.Message);
		}
	}
	
	// public IEnumerator GetItems (string accountAddress, string accountPrivateKey, int index) 
	// {
	// 	var function = ContractGetItems ();
	// 	var tx = function.CreateTransactionInput (accountAddress, new HexBigInteger (50000), new HexBigInteger (0), new HexBigInteger (0), index);

	// 	// var item = await function.SendTransactionAndWaitForReceiptAsyncaccountAddress, new HexBigInteger (50000), new HexBigInteger (0), index);

	// 	var transactionSignedRequest = new TransactionSignedUnityRequest (Inventory.URL, accountPrivateKey, accountAddress);

	// 	// Then we send it and wait
	// 	Debug.Log("Sending GetItems ("+index+") transaction (from = "+ accountAddress);
	// 	yield return transactionSignedRequest.SignAndSendTransaction (tx);
	// 	if (transactionSignedRequest.Exception == null) {
	// 		// If we don't have exceptions we just display the result, congrats!
	// 		Debug.Log ("GetItems transaction submitted: " + transactionSignedRequest.Result);
	// 		Debug.Log ("GetItems transaction submitted (string): " + ResultToString(function, transactionSignedRequest.Result));
	// 	} else {
	// 		// if we had an error in the UnityRequest we just display the Exception error
	// 		Debug.Log ("Error submitting GetItems transaction: " + transactionSignedRequest.Exception.Message);
	// 	}
	// }

	#endregion


	public string ResultToString(Function f, string result)
	{
		var function = f;
		return function.DecodeSimpleTypeOutput<string>(result);
	}
	
	public int ResultToInt(Function f, string result)
	{
		var function = f;
		return function.DecodeSimpleTypeOutput<int>(result);
	}

	public long ResultToLong(Function f, string result)
	{
		var function = f;
		return function.DecodeSimpleTypeOutput<long>(result);
	}
}