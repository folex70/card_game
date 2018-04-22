using System.Collections;
using System.Collections.Generic;
using System; 
using UnityEngine;
using UnityEngine.UI;

public class _GM : MonoBehaviour {
	//----
	public float mylifePoints = 2000f;
	public float enemylifePoints = 2000f;
	//---- turn controllers
	public bool		myTurn = false;
	public float 	turnTime = 0.0f;
	public float 	turnLimitTime = 30.0f;
	public int 		turns = 0 ;
	//public List 	myHand;
	//---- cards prefabs
	public GameObject cardSoldierPrefab;
	public GameObject enemyCardPrefab;
	public GameObject [] enemyCards;
	public int countEnemyCards = 0;
	//---- positons
	public float cardStartPosX = -4.0f;
	public float enemyCardStartPosX = -4.0f;
	//----
	public Renderer rend ;
	// Use this for initialization
	//----
	public List<Card> cards = new List<Card> ();
	//---------
	//ui
	public Text uiTextEnemyLife;
	public Text uiTextMyLife;
	//---------
	void Start () {
		//creating my cards
		cards.Add (new Card ("Soldier", 600,0,0,0));
		cards.Add (new Card ("Soldier", 600,0,0,0));
		cards.Add (new Card ("Soldier", 600,0,0,0));
		cards.Add (new Card ("Soldier", 600,0,2,0));
		//creating enemy cards
		cards.Add (new Card ("Sargent", 1000,1,0,0));
		cards.Add (new Card ("Sargent", 1000,1,0,0));
		cards.Add (new Card ("Sargent", 1000,1,0,0));
		cards.Add (new Card ("Sargent", 1000,1,2,0));
		cards.Sort ();

		foreach (Card card in cards) {
			//print (card.name + " " + card.power);
			if(card.owner == 0){//instaciates my own cards
				if(card.status == 0){// cards with status on hand
					if(card.name =="Soldier"){
						//cardSoldierPrefab.SetActive (true);
						Instantiate(cardSoldierPrefab, new Vector3(cardStartPosX,-5.0f,0), Quaternion.identity);
						cardStartPosX += 3.0f;
					}	
				}

			}else if(card.owner == 1){//instaciates enemy cards
				if (card.status == 0) {// cards with status on hand
					//print(enemyCard1);
					if (card.name == "Sargent") {
						enemyCards [countEnemyCards] = Instantiate(enemyCardPrefab, new Vector3(enemyCardStartPosX,5.0f,0), Quaternion.identity);
						enemyCardStartPosX += 3.0f;
						countEnemyCards++;
					}
				}
			}

		}

	}
	
	// Update is called once per frame
	void Update () {
		turnTime += Time.deltaTime;
		uiTextEnemyLife.text	= enemylifePoints.ToString();
		uiTextMyLife.text		= mylifePoints.ToString();
		//print (turnTime);
		//troca turno pelo tempo
		if(turnTime > turnLimitTime){ endTurn();}

		if(myTurn){
			//jogar no meu turno	
			if(Input.GetMouseButtonDown(0)){
				//enemyCards [0].SetActive (false);
			}
		}else{
			//esperar jogada do npc
			enemySelectCard(0);//TODO randomize
			if (turns > 1) {
				tryAttack (0);//TODO randomize
			}
			endTurn ();
		}
		gameCheck ();
	}

	public void endTurn(){		
		turnTime = 0;
		//myTurn ? myTurn = false : myTurn = true; 
		if(myTurn){
			myTurn = false;
		}else{
			myTurn = true;
		}
		//TODO: resetar animacao da ampulheta
		//TODO: a cada fim de turno, fazer um serch na lista e alterar o status da ultima carta para 0
		turns++;
	}

	public void enemySelectCard(int c){
		enemyCards[c].transform.position = new Vector3 (enemyCards[c].transform.position.x,2.2f,enemyCards[c].transform.position.z);  //@TODOrandaminzar depois
		enemyCards[c].transform.Rotate(0,0,180.0f);
		rend = enemyCards[c].GetComponent<SpriteRenderer>();
		rend.enabled = false;
		//set status card TODO add index or id for specific cards
		foreach (Card card in cards) {
			if (card.owner == 1) {
				if(card.status == 0){
					card.status = 1;
				}
			}
		}

	}

	public void tryAttack (int c){
		foreach (Card card in cards) {
			if (card.owner == 1) {
				if(card.status == 1){
					print (card.name + " " + card.power);

					foreach (Card card2 in cards) {
						if (card2.owner == 0) {
							if(card2.status == 1){
								print (card.name + " " + card.power);
								//card.CompareTo(card,card2);
								//destruir carta que perdeu
							}else{
								//fild without cards for defend
								print("oooooooooooooo");
								mylifePoints = mylifePoints - card.power;
								break;
							}
						}
					}

				}
			}
		}
	}

	public void gameCheck(){
		if (mylifePoints < 0) {
			print ("game over you loose");
			//gameover
		}
		if (enemylifePoints < 0){
			print (" you win!");
		}
	}

}

public class Card: IComparable<Card>{
	public string 	name;
	public int 		power;
	public int 		owner; //0 - my ; 1 - enemy;
	public int 		status;//0 - on hand; 1 - onfield;  2 on deck
	public int 		type; //0 - normal ; 1 special ; 2 field; 3 trap

	public Card(string newName, int newPower, int newOwner, int newStatus, int newType){

		name 	= newName;
		power 	= newPower;
		owner 	= newOwner;
		status 	= newStatus;
		type 	= newType;
	}

	public Card Battle(Card other){

		//if(other == null){
		//	return 1;
		//}

		if (other.power > power) {
			return other;
		}else{
			return this;
		}
	}

	public int CompareTo(Card other){
		if(other == null){
			return 1;
		}

		return power - other.power;
	}

	public void changeStatus(Card c){
		status = c.status;
	} 

}
