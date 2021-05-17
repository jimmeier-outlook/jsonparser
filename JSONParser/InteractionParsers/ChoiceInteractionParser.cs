﻿using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace JsonParser.InteractionParsers
{
    public class ChoiceInteractionParser : IInteractionParser
    {
        public string FormId { set; get; }
        public int PartId { set; get; }
        public int TufpId { set; get; }
        public string ItemId { set; get; }
        public string InteractionId { set; get; }
        public List<string> Values { set; get; }
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
            if (choiceId == null && value == null)
            {
                dbAccess.SaveChoiceInteractionResponse(
                    FormId,
                    PartId,
                    TufpId,
                    ItemId,
                    InteractionId,
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
            else if (value == null || value == "null")
            {
                dbAccess.SaveChoiceInteractionResponse(
                    FormId,
                    PartId,
                    TufpId,
                    ItemId,
                    InteractionId,
                    choiceId,
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
                var choiceValues = JToken.Parse(value);
                foreach (var choiceValue in choiceValues)
                {
                    dbAccess.SaveChoiceInteractionResponse(
                        FormId,
                        PartId,
                        TufpId,
                        ItemId,
                        InteractionId,
                        choiceValue.ToString(),
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