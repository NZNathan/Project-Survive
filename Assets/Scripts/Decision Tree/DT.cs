using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DT : MonoBehaviour {

    public DTNode root;

    public GameObject getScenario()
    {
        return root.getLeaf();
    }

    public void createTree()
    {
        //Depth = 0
        //Root Node
        root = (DTNode) DTNode.CreateInstance("DTNode");
        root.setUpDTNode("Safe", 1);


        //Depth = 1
        //Yes it is safe
        DTNode people = (DTNode)DTNode.CreateInstance("DTNode");
        people.setUpDTNode("People", 0.4f);

        //No it isn't safe
        DTNode faction = (DTNode)DTNode.CreateInstance("DTNode");
        faction.setUpDTNode("Faction", 0.6f);

        root.add(people, faction);


        //Depth = 2
        //It is safe -> Yes there are People
        DTLeaf camp = (DTLeaf)DTLeaf.CreateInstance("DTLeaf");
        camp.setUpDTNode("Campers", 0.5f);
        camp.setPrefab(LandscapeGen.prefabs.friendlyCamps);

        //It is safe -> No there aren't People
        DTLeaf emptyHouse = (DTLeaf)DTLeaf.CreateInstance("DTLeaf");
        emptyHouse.setUpDTNode("Empty Houses", 0.5f);
        emptyHouse.setPrefab(LandscapeGen.prefabs.emptyHouses);

        people.add(camp, emptyHouse);


        //Depth = 2
        //It isn't safe -> Feral Faction
        DTLeaf feralGroups = (DTLeaf)DTLeaf.CreateInstance("DTLeaf");
        feralGroups.setUpDTNode("Feral Groups", 0.4f);
        feralGroups.setPrefab(LandscapeGen.prefabs.feralCrowds);

        //It isn't safe -> Bandit Faction
        DTLeaf banditGroup = (DTLeaf)DTLeaf.CreateInstance("DTLeaf");
        banditGroup.setUpDTNode("Bandit Faction", 0.4f);
        banditGroup.setPrefab(LandscapeGen.prefabs.hostileCamps);

        //It isn't safe -> Feral & Bandit Fight
        DTLeaf banditFeralFight = (DTLeaf)DTLeaf.CreateInstance("DTLeaf");
        banditFeralFight.setUpDTNode("Bandit & Feral Fight", 0.2f);
        banditFeralFight.setPrefab(LandscapeGen.prefabs.fightCrowds);

        faction.add(feralGroups, banditGroup, banditFeralFight);
    }
	
}
