namespace JsonParser
{
    public interface IInteractionParser
    {
        string FormId { set; get; }
        int PartId { set; get; }
        int TufpId { set; get; }
        string ItemId { set; get; }
        string InteractionId { set; get; }
        decimal? Score { set; get; }
        decimal? manualScore { set; get; }
        decimal? ItemTotalScore { set; get; }
        int ItemScoreStatus { set; get; }
        string ItemResponse { set; get; }
        int InteractionScorestatus { set; get; }
        string InteractionResponse { set; get; }
        decimal? TotalMachineScore { set; get; }
        bool? hasAlert { set; get; }
        string nonScores { set; get; }
        bool?  itemAttempted { set; get; }
        bool? interactionAttempted { set; get; }
        void ParseAndSave(IDbAccess dbAccessccess, string choiceId, string value);
    }
}