using Newtonsoft.Json.Linq;

namespace JsonParser.InteractionParsers
{
    public class TextBoxInteractionParser : IInteractionParser
    {
        public string FormId { set; get; }
        public int PartId { set; get; }
        public int TufpId { set; get; }
        public string ItemId { set; get; }
        public string InteractionId { set; get; }
        public decimal? Score { set; get; }
        public decimal? manualScore { set; get; }
        public decimal? ItemTotalScore { set; get; }
        public int ItemScoreStatus { set; get; }
        public string ItemResponse { set; get; }
        public int InteractionScorestatus { set; get; }
        public string InteractionResponse { set; get; }
        public decimal? TotalMachineScore { set; get; }
        public bool? hasAlert { set; get; }
        public string nonScores { set; get; }
        public bool? itemAttempted { set; get; }
        public bool? interactionAttempted { set; get; }
        public void ParseAndSave(IDbAccess dbAccess, string choiceId, string value)
        {
            if (value == null || !value.StartsWith("{") && !value.EndsWith("]}"))
            {
                dbAccess.SaveTextBoxInteractionResponse(
                    FormId,
                    PartId,
                    TufpId,
                    ItemId,
                    InteractionId,
                    value,
                    Score,
                    manualScore,
                    ItemTotalScore,
                    ItemScoreStatus,
                    ItemResponse,
                    InteractionScorestatus,
                    InteractionResponse,
                    TotalMachineScore,
                    hasAlert,
                    nonScores,
                    itemAttempted,
                    interactionAttempted);
            }
            else
            {
                if (value.StartsWith("{") && value.EndsWith("]}"))
                {
                    var firstResult = JToken.Parse(JObject.Parse(value)["results"][0].ToString());
                    dbAccess.SaveTextBoxInteractionResponse(
                        FormId,
                        PartId,
                        TufpId,
                        ItemId,
                        InteractionId,
                        firstResult["value"].ToString(),
                        Score,
                        manualScore,
                        ItemTotalScore,
                        ItemScoreStatus,
                        ItemResponse,
                        InteractionScorestatus,
                        InteractionResponse,
                        TotalMachineScore,
                        hasAlert,
                        nonScores,
                        itemAttempted,
                        interactionAttempted);
                }
                else
                {
                    dbAccess.SaveTextBoxInteractionResponse(
                        FormId,
                        PartId,
                        TufpId,
                        ItemId,
                        InteractionId,
                        value,
                        Score,
                        manualScore,
                        ItemTotalScore,
                        ItemScoreStatus,
                        ItemResponse,
                        InteractionScorestatus,
                        InteractionResponse,
                        TotalMachineScore,
                        hasAlert,
                        nonScores,
                        itemAttempted,
                        interactionAttempted);
                }
            }
        }
    }
}