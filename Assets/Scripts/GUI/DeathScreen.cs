using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour {

	public GameObject deathScreen;
	public CanvasGroup deathGroup;
	public CanvasGroup inheritanceGroup; 

	public SpriteGen spriteGen;

    [Header("Death Screen")]
    public Text characterName;
	public Text causeOfDeath;
	public Text revengeName;
	public CharacterUI player;
	public CharacterUI revengeTarget;
    public Text revengeFaction;
    public Text[] revengeStats = new Text[3];

	[Header("Inheritance Screen")]
	public CharacterUI[] childrenUI = new CharacterUI[3];
	public Animator[] animators = new Animator[3];
	public Text[] namesText = new Text[3];
	public Text[] classText= new Text[3];
	public Image[] abilities = new Image[9];
    public Text[] statTexts = new Text[9];
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
        player.setSpriteController(Player.instance.getSpriteController());
        causeOfDeath.text = Player.instance.causeOfDeath;

        //Set up revenge Target
        CMoveCombatable newRevengeTarget = Player.instance.getAttacker();
        revengeTarget.setSpriteController(newRevengeTarget.getSpriteController());
        revengeName.text = Player.instance.getAttacker().getName();
        revengeFaction.text = newRevengeTarget.faction.ToString();

        revengeStats[0].text = "Strength: " + newRevengeTarget.getStats()[0];
        revengeStats[1].text = "Agility: " + newRevengeTarget.getStats()[1];
        revengeStats[2].text = "Endurance: " + newRevengeTarget.getStats()[2];
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
                statTexts[j + (3 * i)].text = children[i].getStats()[j].ToString();
			}

            abilities[0].sprite = children[i].getClass().abilities[0].getIcon();
            abilities[1].sprite = children[i].getClass().abilities[1].getIcon();

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
