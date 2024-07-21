using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCube : QuestItem
{
    public override void OnQuestUpdated(Objective objective)
    {
        if (objective.name != QuestOrTaskName || objective.state != state)
            return;

        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<SphereCollider>().enabled = true;
    }
}
