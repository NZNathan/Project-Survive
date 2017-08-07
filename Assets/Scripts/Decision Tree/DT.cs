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
        people.setUpDTNode("People", 0.5f);

        //No it isn't safe
        DTNode faction = (DTNode)DTNode.CreateInstance("DTNode");
        faction.setUpDTNode("Faction", 0.5f);

        root.add(people, faction);


        //Depth = 2
        //It is safe -> Yes there are People
        DTLeaf camp = (DTLeaf)DTLeaf.CreateInstance("DTLeaf");
        camp.setUpDTNode("Camping", 0.5f);
        camp.setPrefab(LandscapeGen.prefabs.friendlyCamps);

        //It is safe -> No there aren't People
        DTLeaf travellers = (DTLeaf)DTLeaf.CreateInstance("DTLeaf");
        travellers.setUpDTNode("Travellers", 0.5f);
        travellers.setPrefab(LandscapeGen.prefabs.friendlyTravellers);

        people.add(camp, travellers);


        //Depth = 2
        //It isn't safe -> Feral Faction
        DTLeaf feralGroups = (DTLeaf)DTLeaf.CreateInstance("DTLeaf");
        feralGroups.setUpDTNode("Feral Groups", 1f);
        feralGroups.setPrefab(LandscapeGen.prefabs.feralCrowds);

        faction.add(feralGroups);
    }
	
}
