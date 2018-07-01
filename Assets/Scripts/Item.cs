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
	public Sprite sprite; //miniature
	public Sprite pictureSprite; //big piture
	private Contract contract;
	private ItemGeneratorContract itemGenerator;

	public string ABI = @"[{""constant"":true,""inputs"":[],""name"":""name"",""outputs"":[{""name"":"""",""type"":""string""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""hash"",""outputs"":[{""name"":"""",""type"":""bytes32""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""getName"",""outputs"":[{""name"":"""",""type"":""string""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""getDescription"",""outputs"":[{""name"":"""",""type"":""string""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""signature"",""outputs"":[{""name"":"""",""type"":""bytes32""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[{""name"":""to"",""type"":""address""}],""name"":""delegate"",""outputs"":[{""name"":""onlyOwner"",""type"":""address""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""description"",""outputs"":[{""name"":"""",""type"":""string""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""owner"",""outputs"":[{""name"":"""",""type"":""address""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""icon"",""outputs"":[{""name"":"""",""type"":""string""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""getIcon"",""outputs"":[{""name"":"""",""type"":""string""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""inputs"":[{""name"":""_hash"",""type"":""bytes32""},{""name"":""_name"",""type"":""string""},{""name"":""_description"",""type"":""string""},{""name"":""_icon"",""type"":""string""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""constructor""}]";	

	public static Item CreateInstance(int id, string _adress, ItemGeneratorContract g)
	{
		var data = ScriptableObject.CreateInstance<Item>();
		data.Init(id, _adress, g);
		return data;
	}

	public void Init(int id, string _adress, ItemGeneratorContract g)
	{
		itemGenerator = g;
		adress = _adress;
		spriteMap = SpriteMapList.Instance;
		GetDataOnBC();
	}

	public void GetDataOnBC()
	{
		this.contract = new Contract (null, ABI, adress);

		itemGenerator.StartCoroutine( GetName() );
		itemGenerator.StartCoroutine( GetIcon() );
		itemGenerator.StartCoroutine( GetDescription() );	
		itemGenerator.StartCoroutine( GetOwner() );	
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
	public Function ContractGetOwner () {
		return contract.GetFunction ("owner");
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

	public CallInput CallGetOwner () 
	{
		var function = ContractGetOwner ();
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

	public IEnumerator GetName () 
	{
		var request = new EthCallUnityRequest (Inventory.URL);

		var callInput = CallGetName ();
		Debug.Log ("Getting item("+adress+") name...");

		yield return request.SendRequest (callInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest ());

		if (request.Exception == null) 
		{
			var function = ContractGetName();
			name = itemGenerator.ResultToString(function, request.Result);
			// itemListLength = System.Int32.Parse( request.Result, NumberStyles.AllowHexSpecifier );
			// Debug.Log ("item name (HEX): " + request.Result);
			// Debug.Log ("item name (string):" + name);
		}
		else 
		{
			// if we had an error in the UnityRequest we just display the Exception error
			Debug.Log ("Error submitting item list length tx: " + request.Exception.Message);
		}
	}
	
	
	public IEnumerator GetIcon () 
	{
		var request = new EthCallUnityRequest (Inventory.URL);

		var callInput = CallGetIcon ();
		Debug.Log ("Getting item("+adress+") icon...");

		yield return request.SendRequest (callInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest ());

		if (request.Exception == null) 
		{
			var function = ContractGetIcon();
			iconName = itemGenerator.ResultToString(function, request.Result);
			sprite = spriteMap.GetPictureSpriteByName(iconName);
			pictureSprite = spriteMap.GetPictureSpriteByName(iconName);
			// itemListLength = System.Int32.Parse( request.Result, NumberStyles.AllowHexSpecifier );
			// Debug.Log ("item icon (HEX): " + request.Result);
			// Debug.Log ("item icon (string):" + iconName);
		}
		else 
		{
			// if we had an error in the UnityRequest we just display the Exception error
			Debug.Log ("Error submitting item icon length tx: " + request.Exception.Message);
		}
	}
	
	public IEnumerator GetDescription () 
	{
		var request = new EthCallUnityRequest (Inventory.URL);

		var callInput = CallGetDescription ();
		Debug.Log ("Getting item("+adress+") description...");

		yield return request.SendRequest (callInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest ());

		if (request.Exception == null) 
		{
			var function = ContractGetDescription();
			description = itemGenerator.ResultToString(function, request.Result);
			// itemListLength = System.Int32.Parse( request.Result, NumberStyles.AllowHexSpecifier );
			Debug.Log ("item description (HEX): " + request.Result);
			Debug.Log ("item description (string):" + description);
		}
		else 
		{
			// if we had an error in the UnityRequest we just display the Exception error
			Debug.Log ("Error submitting description tx: " + request.Exception.Message);
		}
	}
	
	public IEnumerator GetOwner () 
	{
		var request = new EthCallUnityRequest (Inventory.URL);

		var callInput = CallGetOwner ();
		Debug.Log ("Getting item("+adress+") owner...");

		yield return request.SendRequest (callInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest ());

		if (request.Exception == null) 
		{
			var function = ContractGetOwner();
			owner = itemGenerator.ResultToString(function, request.Result);
			// itemListLength = System.Int32.Parse( request.Result, NumberStyles.AllowHexSpecifier );
			// Debug.Log ("item owner (HEX): " + request.Result);
			// Debug.Log ("item owner (string):" + owner);
		}
		else 
		{
			// if we had an error in the UnityRequest we just display the Exception error
			Debug.Log ("Error submitting owner tx: " + request.Exception.Message);
		}
	}

	#endregion
}

public enum ItemMode
{
	ingame,
	inventory
}