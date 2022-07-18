using DS.Enumerations;

public class ValidConnectionTypeData
{
    public DSDialogueType BaseNodeType;
    public DSDialogueType EndNodeType;

    public ValidConnectionTypeData(DSDialogueType baseNodeType, DSDialogueType endNodeType)
    {
        BaseNodeType = baseNodeType;
        EndNodeType = endNodeType;
    }
}