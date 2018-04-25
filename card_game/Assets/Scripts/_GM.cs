using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System; 
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class _GM : MonoBehaviour {
	//----
	public float mylifePoints = 2000f;
	public float enemylifePoints = 2000f;
	public int myDeck = 0;
	public int enemyDeck = 0;
	public int randomGame;
	//---- turn controllers
	public bool		myTurn = false;
	public float 	turnTime = 30.0f;
	public float 	turnLimitTime = 30.0f;
	public int 		turns = 0 ;
	public bool 	gameOver = false;
	//public List 	myHand;
	//---- cards prefabs
	public GameObject cardSoldierPrefab;
	public GameObject cardSoldierPrefab2;
	public GameObject cardSargentPrefab2;
	public GameObject cardSpecPrefab2;
	public GameObject enemyCardPrefab;
	public GameObject [] enemyCards;
	public GameObject [] myCards;
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
	//owner of card for battle comparission
	public int cOnwer2 = 0;
	//---------
	//ui
	public Text uiTextEnemyLife;
	public Text uiTextMyLife;
	public Text uiTextEnemyDeck;
	public Text uiTextMyDeck;
	public Text uiTextTurn;
	public Text uiTextTurnTime;
	//---------
	void Start () {
		randomGame = UnityEngine.Random.Range (0,2);
		print (randomGame);
		//creating my cards
		if (randomGame == 0) {
			cards.Add (new Card (0, "Sargent", 1000, 0, 0, 0));
			cards.Add (new Card (1, "Sargent", 1000, 0, 0, 0));
			cards.Add (new Card (2, "Soldier", 600, 0, 0, 0));
			cards.Add (new Card (3, "Soldier", 600, 0, 2, 0));	
		} else {
			cards.Add (new Card (0, "Specialist", 800, 0, 0, 0));
			cards.Add (new Card (1, "Specialist", 800, 0, 0, 0));
			cards.Add (new Card (2, "Soldier", 600, 0, 0, 0));
			cards.Add (new Card (3, "Soldier", 600, 0, 2, 0));	
		}

		//creating enemy cards
		cards.Add (new Card (4,"Sargent", 1000,1,0,0));
		cards.Add (new Card (5,"Soldier", 600,1,0,0));
		cards.Add (new Card (6,"Soldier", 600,1,0,0));
		cards.Add (new Card (7,"Sargent", 1000,1,2,0));
		cards.Sort ();
		//load cards 
		//initialdraw
		foreach (Card card in cards) {
			//print (card.name + " " + card.power);
			if(card.owner == 0){//instaciates my own cards
				if(card.status == 0){// cards with status on hand
					if(card.name =="Soldier"){
						//cardSoldierPrefab.SetActive (true);
						myCards[card.idCard] = Instantiate(cardSoldierPrefab, new Vector3(cardStartPosX,-5.0f,0), Quaternion.identity);
						myCards[card.idCard].name = card.idCard.ToString();
						cardStartPosX += 3.0f;
					}	
					if(card.name =="Specialist"){
						//cardSoldierPrefab.SetActive (true);
						myCards[card.idCard] = Instantiate(cardSpecPrefab2, new Vector3(cardStartPosX,-5.0f,0), Quaternion.identity);
						myCards[card.idCard].name = card.idCard.ToString();
						cardStartPosX += 3.0f;
					}
					if (card.name == "Sargent") {//
						myCards[card.idCard] = Instantiate(cardSargentPrefab2, new Vector3(cardStartPosX,-5.0f,0), Quaternion.identity);
						myCards[card.idCard].name = card.idCard.ToString();
						cardStartPosX += 3.0f;
					}
				}
				else if(card.status == 2){//remaing cards on deck
					myDeck++;	
				}

			}else if(card.owner == 1){//instaciates enemy cards
				if (card.status == 0) {// cards with status on hand
					//print(enemyCard1);
					if (card.name == "Sargent") {
						enemyCards [card.idCard] = Instantiate(enemyCardPrefab, new Vector3(enemyCardStartPosX,5.0f,0), Quaternion.identity);
						enemyCards [card.idCard].name = card.idCard.ToString();
						enemyCardStartPosX += 3.0f;
						countEnemyCards++;
					}
					if(card.name =="Soldier"){
						//cardSoldierPrefab.SetActive (true);
						enemyCards [card.idCard] =  Instantiate(cardSoldierPrefab2, new Vector3(enemyCardStartPosX,5.0f,0), Quaternion.identity);
						enemyCards [card.idCard].name = card.idCard.ToString();
						enemyCardStartPosX += 3.0f;
						countEnemyCards++;
					}
				}
				else if(card.status == 2){//remaing cards on deck
					enemyDeck++;
				}
			}

		}
	}
	
	// Update is called once per frame
	void Update () {
		turnTime -= Time.deltaTime;
		//texts update
		uiTextEnemyLife.text	= enemylifePoints.ToString();
		uiTextMyLife.text		= mylifePoints.ToString();
		uiTextEnemyDeck.text 	= enemyDeck.ToString();
		uiTextMyDeck.text 		= myDeck.ToString();
		uiTextTurn.text 		= myTurn ? "Turn "+turns+": My" : "Turn "+turns+":Enemy";
		uiTextTurnTime.text 	= turnTime.ToString();
		//------------------- end turn after 30 seconds
		if(turnTime < 0){
			endTurn ();
		}
		//-------------------
		if(myTurn){
			//jogar no meu turno	
			if(Input.GetMouseButtonDown(0)){
				//enemyCards [0].SetActive (false);
				int idcard = CastRay();

				if (turns > 1 && idcard != 99) {

					print ("status of clicked card check" + checkStatusCard (idcard));
					if (checkStatusCard (idcard) == 1) {
						tryAttack (0,1);//TODO randomize
						endTurn();
					}
				}

				if (idcard != 99) {
					mySelectCard(idcard);
				}
			}
		}else{
			//npc turn
			StartCoroutine(enemySelectCard(0.2f));//TODO randomize
			//StartCoroutine(npcThinkTime(5.0f));
			if (turns > 1) {
				tryDrawCard (1);
				tryAttack (1,0);//TODO randomize
			}
			//StartCoroutine(enemyEndTurn(1.0f));
			endTurn(); print("enemy ended turn");
		}
		gameCheck ();
	}

	public void endTurn(){		
		turnTime = 30;
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

	public void tryDrawCard(int owner){
		
		//draw a card from deck
		if (owner == 0) {
			myDeck--;
		} else if (owner == 1) {
			enemyDeck--;
		}
	}

	IEnumerator enemySelectCard(float time ){
		yield return new WaitForSeconds (time);
		foreach (Card card in cards) {
			if (card.owner == 1) {//carta do enemy 
				if(card.status == 0){//carta na mao
					enemyCards[card.idCard].transform.position = new Vector3 (enemyCards[card.idCard].transform.position.x,2.2f,enemyCards[card.idCard].transform.position.z);  //@TODOrandaminzar depois
					enemyCards[card.idCard].transform.Rotate(0,0,180.0f);
					rend = enemyCards[card.idCard].GetComponent<SpriteRenderer>();
					rend.enabled = false;
					card.status = 1;
					print ("enemy put in the desk :"+card.name + " " + card.power+" owner: "+card.owner+" status: "+card.status+" card id:"+card.idCard);
					break;
				}
			}
		}
	}

	public void mySelectCard(int sendedIdCard){
		foreach (Card card in cards) {
			if (card.owner == 0) {//my cards
				if(card.status == 0){//carta na mao
					if(card.idCard == sendedIdCard){
						myCards[card.idCard].transform.position =  new Vector3 (myCards[card.idCard].transform.position.x,-2.0f,myCards[card.idCard].transform.position.z);
						card.status = 1;
						print ("i put a card: "+card.name + " " + card.power+" owner: "+card.owner+" status: "+card.status+" card id:"+card.idCard);
						break;						
					}
				}
			}
		}
		//endTurn();
	}

	public int checkStatusCard(int idCard){

		print ("i search for card with idcard = "+idCard);
		foreach (Card card in cards) {
			if (card.owner == 0) {//my cards
				if(card.idCard == idCard){
						print ("card found "+card.name + " " + card.power+" owner: "+card.owner+" status: "+card.status+" card id:"+card.idCard);
						return card.status;
				}
			}
		}
		return 99;

	}

	public void tryAttack (int cOwner, int cOwner2, int sendedIdCard=0){
		print ("primeiro "+cOwner+"segund "+cOwner2);
		bool noCardsOnField = false;
		print("owner "+cOwner+ " tryed attack");
		foreach (Card card in cards) {
			if (card.owner == cOwner) {
				if(card.status == 1){
					foreach (Card card2 in cards) {
						if (card2.owner == cOwner2) {
							//print (card2.name + " " + card2.power+" owner: "+card2.owner+" status: "+card2.status+" card id:"+card2.idCard);
							if (card2.status == 1) {
								//print (card.name + " " + card.power);
								print ("cards battle");
								print (card.name + " " + card.power + " owner: " + card.owner + " status: " + card.status + " card id:" + card.idCard);
								print ("vs");
								print (card2.name + " " + card2.power + " owner: " + card2.owner + " status: " + card2.status + " card id:" + card2.idCard);
								print (card.CompareTo (card2));
								if (card.power > card2.power) {//if attacking card win
									if (cOwner == 0) {
										enemylifePoints = enemylifePoints - card.CompareTo (card2);	//inflicts only diference of powers on field
										enemyCards [card2.idCard].SetActive (false);
										card2.status = 4;

									} else {
										mylifePoints = mylifePoints - card.CompareTo (card2);
										myCards [card2.idCard].SetActive (false);
										card2.status = 4;
									}
								} 
								else if (card.power < card2.power) {//attacking card with more points
									if (cOwner == 0) {
											print ("caiu aquii idcard " + card.idCard + " status " + card.status);
											mylifePoints = mylifePoints + card.CompareTo (card2);
											myCards [card.idCard].SetActive (false);
											card.status = 4;

									} else {
										enemylifePoints = enemylifePoints + card.CompareTo (card2);	//inflicts only diference of powers on field
										enemyCards [card.idCard].SetActive (false);
										card.status = 4;										
									}
								} 
								else if(card.power == card2.power){
									//tie
									print("caiu no tie");
									if (cOwner == 0) {
										myCards [card.idCard].SetActive (false);
										enemyCards [card2.idCard].SetActive (false);
									} else {
										myCards [card2.idCard].SetActive (false);
										enemyCards [card.idCard].SetActive (false);
									}
									card2.status = 4;
									card.status = 4;
								}
								noCardsOnField = false;
								break;
							} else {
								noCardsOnField = true;
							}
						}
					}
					//fild without cards for defend
					if(noCardsOnField){
						print("direct field attack");
						if (cOwner == 0) {
							enemylifePoints = enemylifePoints - card.power;
						} else {
							mylifePoints = mylifePoints - card.power;
						}	
					}

					break;
				}
			}
		}
	}

	public void gameCheck(){

		if (mylifePoints <= 0) {
			print ("game over you loose");
			uiTextTurn.text 		= "Game Over!Game By @folex70 made in 48h for ld#41. Thanks for play! mail me jeff7gamedev@gmail.com";
			gameOver = true;
			StartCoroutine(gameOverResetGame (8));
		}
		if (enemylifePoints <= 0){
			print (" you win!");
			uiTextTurn.text 		= "You Win! Game By @folex70 made in 48h for ld#41. Thanks for play! mail me jeff7gamedev@gmail.com";
			StartCoroutine(gameOverResetGame (10));
		}
	}

	IEnumerator gameOverResetGame(float time){
		yield return new WaitForSeconds (time);
		Scene scene = SceneManager.GetActiveScene(); 
		SceneManager.LoadScene(scene.name);	
	}

	public int CastRay() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);
		if (hit) {
			Debug.Log (hit.collider.gameObject.name);
			int cardId = int.Parse(hit.collider.gameObject.name);
			return cardId;
		} else {
			return 99;
		}
	}
}

public class Card: IComparable<Card>{
	public int 		idCard;
	public string 	name;
	public int 		power;
	public int 		owner; //0 - my ; 1 - enemy;
	public int 		status;//0 - on hand; 1 - onfield;  2 on deck; 4 removed
	public int 		type; //0 - normal ; 1 special ; 2 field; 3 trap

	public Card(int newIdcard, string newName, int newPower, int newOwner, int newStatus, int newType){

		idCard  = newIdcard;
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
