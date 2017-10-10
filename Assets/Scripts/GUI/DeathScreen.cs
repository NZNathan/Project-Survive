using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour {

	public GameObject deathScreen;
	public CanvasGroup deathGroup;
	public CanvasGroup inheritanceGroup; 

	public SpriteGen spriteGen;

	//Dead parent Screen
	public Text characterName;
	public Text causeOfDeath;
	public Text revengeName;
	public CharacterUI player;
	public CharacterUI revengeTarget;

	//Inheritance Screen
	public CharacterUI[] childrenUI = new CharacterUI[3];
	public Animator[] animators = new Animator[3];
	public Text[] namesText = new Text[3];
	public Text[] classText= new Text[3];
	public Image[] abilities = new Image[9];
	private Player[] children = new Player[3];

	//Animation Variables
	private float fadeStep = 0.1f;

	// Use this for initialization
	public void newDeathScreen()
	{
		deathScreen.SetActive(true);

        setupDeathScreen();

		StartCoroutine("fadeIn");
	}

	public void transition()
	{
		StartCoroutine("transitionFade");
	}

	IEnumerator fadeIn()
	{
		deathGroup.gameObject.SetActive(true);

		while (deathGroup.alpha < 1)
        {
            deathGroup.alpha += fadeStep;
            yield return new WaitForSeconds(0.01f);
        }
	}

	IEnumerator transitionFade()
	{
		while (deathGroup.alpha > 0)
        {
            deathGroup.alpha -= fadeStep;
            yield return new WaitForSeconds(0.01f);
        }

		deathGroup.gameObject.SetActive(false);

		setupInheritance();

		while (inheritanceGroup.alpha < 1)
        {
            inheritanceGroup.alpha += fadeStep;
            yield return new WaitForSeconds(0.01f);
        }
	}
	
    private void setupDeathScreen()
    {
        //Set up text
        characterName.text = Player.instance.firstName + " " + Player.instance.lastName;
        revengeTarget.setSpriteController(Player.instance.getAttacker().getSpriteController());
        revengeName.text = Player.instance.getAttacker().getName();
        player.setSpriteController(Player.instance.getSpriteController());
    }

	private void setupInheritance()
	{
		inheritanceGroup.gameObject.SetActive(true);

		for(int i = 0; i < 3; i++){ 
			children[i] = spriteGen.createNewPlayer();
			//childrenUI[i].setSprites(children[i].getSprites());
			namesText[i].text = children[i].getName();
			classText[i].text = children[i].getClass().name;
			children[i].gameObject.name = "child_" + i;
			animators[i].runtimeAnimatorController = children[i].getSpriteController();

			for(int j = 0; j < 3; j++){

				abilities[j + (3 * i)].sprite = children[i].getClass().abilities[j].getIcon();

			}
				

			children[i].gameObject.SetActive(false);
		}
	}

	public void chosenCharacter(int index)
	{
		for(int i = 0; i < 3; i++){ 

			if(i != index)
				Destroy(children[i].gameObject);
		}

		WorldManager.instance.resetPlayer(children[index]);
		inheritanceGroup.alpha = 0f;
		inheritanceGroup.gameObject.SetActive(false);
		deathScreen.SetActive(false);
	}
}
