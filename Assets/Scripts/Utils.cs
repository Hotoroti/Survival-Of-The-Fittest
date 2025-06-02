using UnityEngine;

public static class Utils
{
    public static bool IsOnGround(Vector3 target)
    {
        return Physics.Raycast(target, Vector3.down, 1f, LayerMask.GetMask("Ground"));
    }

    public static float GetInheritedGene(string gene, AnimalController otherParent, AnimalController thisParent)
    {
        return Random.value < 0.5f ? thisParent.Dna.Chromosomes[gene] : otherParent.Dna.Chromosomes[gene];
    }

    public static void ApplyRandomMutation(ref float life, ref float speed, ref float sense, ref float energy)
    {
        if (Random.Range(0, 100) >= Settings.Instance.MutationChangeMax)
            return;

        float mutationFactor = Random.Range(0, 2) == 0
            ? Settings.Instance.MutationMultiplier.x
            : Settings.Instance.MutationMultiplier.y;

        int geneToMutate = Random.Range(0, 4);
        switch (geneToMutate)
        {
            case 0:
                life *= mutationFactor;
                Debug.Log("Mutation applied to Life");
                break;
            case 1:
                speed *= mutationFactor;
                Debug.Log("Mutation applied to WalkingSpeed");
                break;
            case 2:
                sense *= mutationFactor;
                Debug.Log("Mutation applied to Sense");
                break;
            case 3:
                energy *= mutationFactor;
                Debug.Log("Mutation applied to Energy");
                break;
        }
    }
}
