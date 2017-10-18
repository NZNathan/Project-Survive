using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour {

	public GameObject deathScreen;
	public CanvasGroup deathGroup;
	public CanvasGroup inheritanceGroup; 

	public SpriteGen spriteGen;

    [Header("Death Screen")]
    //Parent Variables
    public Text characterName;
    public Text causeOfDeath;
    public CharacterUI player;
    //Revenge Target Variables
    public Text revengeClass;
	public Text revengeName;
	public CharacterUI revengeTarget;
    public Text revengeFaction;
    public Text[] revengeStats = new Text[4];
    public Text[] revengeStatChanges = new Text[4];
    public Image[] revengeAbilities = new Image[2];

    [Header("Inheritance Screen")]
	public CharacterUI[] childrenUI = new CharacterUI[3];
	public Animator[] animators = new Animator[3];
	public Text[] namesText = new Text[3];
	public Text[] classText= new Text[3];
	public Image[] abilities = new Image[6];
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

        //Set up name
        revengeName.text = Player.instance.getAttacker().getName();

        //Setup class
        revengeClass.text = newRevengeTarget.getClass().name;

        //set up faction
        revengeFaction.text = newRevengeTarget.faction.ToString();

        revengeStats[0].text = "Level: " + newRevengeTarget.level;
        revengeStats[1].text = "Strength: " + newRevengeTarget.getStats()[0];
        revengeStats[2].text = "Agility: " + newRevengeTarget.getStats()[1];
        revengeStats[3].text = "Endurance: " + newRevengeTarget.getStats()[2];
        

        for (int i = 0; i < 4; i++)
        {
            revengeStatChanges[i].text = "+" + newRevengeTarget.levelupChanges[i];
        }

            revengeAbilities[0].sprite = newRevengeTarget.getClass().abilities[0].getIcon();
        revengeAbilities[1 ].sprite = newRevengeTarget.getClass().abilities[1].getIcon();
    }

	private void setupInheritance()
	{
		inheritanceGroup.gameObject.SetActive(true);

		for(int i = 0; i < 3; i++){ 
			children[i] = spriteGen.createNewPlayer();
            children[i].levelup(Player.instance.level);
			//childrenUI[i].setSprites(children[i].getSprites());
			namesText[i].text = children[i].getName();
			classText[i].text = children[i].getClass().name;
			children[i].gameObject.name = "child_" + i;
			animators[i].runtimeAnimatorController = children[i].getSpriteController();

			for(int j = 0; j < 3; j++){
                statTexts[j + (3 * i)].text = children[i].getStats()[j].ToString();
			}

            abilities[0 + (2 * i)].sprite = children[i].getClass().abilities[0].getIcon();
            abilities[1 + (2 * i)].sprite = children[i].getClass().abilities[1].getIcon();

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
