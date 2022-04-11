using System.Collections.Generic;
using System.ServiceModel;

namespace Common
{
    [ServiceContract]
    public interface ITextAnalyzerContract
    {
        [OperationContract]
        KeyValuePair<string, int> AnalyzeText(string text);

        [OperationContract]
        Dictionary<string, HashSet<string>> FindTyposForWordsInText(string text);
        
        [OperationContract]
        int LetterCount(string text); //receives a string of text and returns the total number of letters (letters are ‘a’-‘z’, ‘A’-‘Z’)
    }
}
