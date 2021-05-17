using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace JsonParser.InteractionParsers
{
    class LineMatchingInteractionParser : IInteractionParser
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
            if (value == null)
            {
                dbAccess.SaveLineMatchingInteractionResponse(
                    FormId,
                    PartId,
                    TufpId,
                    ItemId,
                    InteractionId,
                    null,
                    null,
                    Score,
                    manualScore,
                    ItemTotalScore,
                    ItemScoreStatus,
                    ItemResponse,
                    InteractionScorestatus,
                    InteractionResponse,
                    TotalMachineScore,
                    itemAttempted,
                    interactionAttempted);
            }
            else
            {
                var dragDropValues = JToken.Parse(value);
                foreach (var t in dragDropValues)
                {
                    var sources = t["startElementId"].ToString().Split(',').ToList();
                    foreach (var src in sources)
                    {
                        dbAccess.SaveLineMatchingInteractionResponse(
                            FormId,
                            PartId,
                            TufpId,
                            ItemId,
                            InteractionId,
                            t["endElementId"].ToString(),
                            src,
                            Score,
                            manualScore,
                            ItemTotalScore,
                            ItemScoreStatus,
                            ItemResponse,
                            InteractionScorestatus,
                            InteractionResponse,
                            TotalMachineScore,
                            itemAttempted,
                            interactionAttempted);
                    }
                }
            }
        }
    }
}