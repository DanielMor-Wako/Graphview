using System.Collections.Generic;
using DS.Enumerations;

public class DataController
{
    public static List<ValidConnectionTypeData> ValidTypes;
    private DSGraphView _targetGraphView;

    public static DataController SetGraphInstance(DSGraphView targetGraphView)
    {
        return new DataController
        {
            _targetGraphView = targetGraphView
        };
    }

    public void InitilizeData()
    {
        ValidTypes = new List<ValidConnectionTypeData>();

        ValidTypes.Add(new ValidConnectionTypeData(DSDialogueType.NodeType1, DSDialogueType.NodeType2));
        ValidTypes.Add(new ValidConnectionTypeData(DSDialogueType.NodeType2, DSDialogueType.NodeType3));
        ValidTypes.Add(new ValidConnectionTypeData(DSDialogueType.NodeType3, DSDialogueType.NodeType1));
    }

    public static bool IsValidConnectionByType(DSDialogueType FirstNodeType, DSDialogueType SecondNodeType)
    {
        bool result = false;
        
        for (var i = 0; i < ValidTypes.Count; i++)
        {
            if (FirstNodeType == ValidTypes[i].BaseNodeType &&
                SecondNodeType == ValidTypes[i].EndNodeType)
            {
                result = true;
            }
        }
        
        return result;
    }
}
