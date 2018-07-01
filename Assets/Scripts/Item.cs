using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.UnityClient;

[CreateAssetMenu(fileName = "Item", menuName = "Create Item", order = 2)]
public class Item : ScriptableObject 
{
	public string description;
	public string owner;
	public string iconName;
	public string adress; //uniqueID


	public SpriteMapList spriteMap;
	public Sprite sprite;
	private Contract contract;

	public string ABI = @"[{""constant"": true,""inputs"": [],""name"": ""name"",""outputs"": [{""name"": """",""type"": ""string""}],""payable"": false,""stateMutability"": ""view"",""type"": ""function""},{""constant"": true,""inputs"": [],""name"": ""hash"",""outputs"": [{""name"": """",""type"": ""bytes32""}],""payable"": false,""stateMutability"": ""view"",""type"": ""function""},{""constant"": true,""inputs"": [],""name"": ""getName"",""outputs"": [{""name"": """",""type"": ""string""}],""payable"": false,""stateMutability"": ""view"",""type"": ""function""},{""constant"": true,""inputs"": [],""name"": ""getDescription"",""outputs"": [{""name"": """",""type"": ""string""}],""payable"": false,""stateMutability"": ""view"",""type"": ""function""},{""constant"": true,""inputs"": [],""name"": ""signature"",""outputs"": [{""name"": """",""type"": ""bytes32""}],""payable"": false,""stateMutability"": ""view"",""type"": ""function""},{""constant"": false,""inputs"": [{""name"": ""to"",""type"": ""address""}],""name"": ""delegate"",""outputs"": [{""name"": ""onlyOwner"",""type"": ""address""}],""payable"": false,""stateMutability"": ""nonpayable"",""type"": ""function""},{""constant"": true,""inputs"": [],""name"": ""description"",""outputs"": [{""name"": """",""type"": ""string""}],""payable"": false,""stateMutability"": ""view"",""type"": ""function""},{""constant"": true,""inputs"": [],""name"": ""owner"",""outputs"": [{""name"": """",""type"": ""address""}],""payable"": false,""stateMutability"": ""view"",""type"": ""function""},{""constant"": true,""inputs"": [],""name"": ""icon"",""outputs"": [{""name"": """",""type"": ""string""}],""payable"": false,""stateMutability"": ""view"",""type"": ""function""},{""constant"": true,""inputs"": [],""name"": ""getIcon"",""outputs"": [{""name"": """",""type"": ""string""}],""payable"": false,""stateMutability"": ""view"",""type"": ""function""},{""inputs"": [{""name"": ""_hash"",""type"": ""bytes32""},{""name"": ""_name"",""type"": ""string""},{""name"": ""_description"",""type"": ""string""},{""name"": ""_icon"",""type"": ""string""}],""payable"": false,""stateMutability"": ""nonpayable"",""type"": ""constructor""}]";

	public static Item CreateInstance( string _adress, string _owner, string _iconName, string _description )
	{
		var data = ScriptableObject.CreateInstance<Item>();
		data.Init(_adress, _owner, _iconName, _description);
		return data;
	}

	public void Init(string _adress, string _owner, string _iconName, string _description)
	{
		name = _iconName;
		adress = _adress;
		description = _description;
		owner = _owner;
		iconName = _iconName;
		spriteMap = SpriteMapList.Instance;
		sprite = spriteMap.GetSpriteByName(iconName);

		this.contract = new Contract (null, ABI, adress);
	}


	#region list of all functions in the contract
	
	public Function ContractDelegate () {
		return contract.GetFunction ("delegate");
	}
	
	public Function ContractGetName () {
		return contract.GetFunction ("getName");
	}

	public Function ContractGetIcon () {
		return contract.GetFunction ("getIcon");
	}
	
	public Function ContractGetDescription () {
		return contract.GetFunction ("getDescription");
	}

	#endregion

	
	#region list of all the callings

	public CallInput CallDelegate () 
	{
		var function = ContractDelegate ();
		return function.CreateCallInput ();
	}

	public CallInput CallGetName () 
	{
		var function = ContractGetName ();
		return function.CreateCallInput ();
	}

	public CallInput CallGetIcon () 
	{
		var function = ContractGetIcon ();
		return function.CreateCallInput ();
	}

	public CallInput CallGetDescription () 
	{
		var function = ContractGetDescription ();
		return function.CreateCallInput ();
	}

	#endregion

	#region all transactions

	public TransactionInput TransactionDelegate (
	string addressFrom,
	string privateKey,
	string toOwner,
	HexBigInteger gas = null,
	HexBigInteger gasPrice = null,
	HexBigInteger valueAmount = null) 
	{
		var function = ContractDelegate ();
		return function.CreateTransactionInput (addressFrom, gas, gasPrice, valueAmount, toOwner);
	}

	#endregion

	#region all IEnumerator

	public IEnumerator Delegate (string accountAddress, string accountPrivateKey, string toOwner) 
	{
		var transactionInput = TransactionDelegate (
			accountAddress,
			accountPrivateKey,
			toOwner,
			new HexBigInteger (50000),
			new HexBigInteger (0),
			new HexBigInteger (0)
		);

		var transactionSignedRequest = new TransactionSignedUnityRequest (Inventory.URL, accountPrivateKey, accountAddress);

		// Then we send it and wait
		Debug.Log("Sending Delegate transaction...");
		yield return transactionSignedRequest.SignAndSendTransaction (transactionInput);
		if (transactionSignedRequest.Exception == null) {
			// If we don't have exceptions we just display the result, congrats!
			Debug.Log ("Delegate transaction submitted: " + transactionSignedRequest.Result);
		} else {
			// if we had an error in the UnityRequest we just display the Exception error
			Debug.Log ("Error submitting Delegate transaction: " + transactionSignedRequest.Exception.Message);
		}
	}

	#endregion
}

public enum ItemMode
{
	ingame,
	inventory
}