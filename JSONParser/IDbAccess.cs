using System.Collections.Generic;

namespace JsonParser
{
    public interface IDbAccess
    {
        IEnumerable<Interaction> GetAllInteractions();

        IList<StudentResponse> GetAllStudentResponses(ProjectConfig projectConfig);

        void SaveAudioRecInteractionResponse(
            string formId,
            int partId,
            int testUserFormPartId,
            string itemId,
            string interactionId,
            string recDuration,
            string assetIdentifier,
            decimal? score,
            decimal? manualScore,
            decimal? itemTotalScore,
            int itemScoreStatus,
            string itemResponse,
            int interactionScorestatus,
            string interactionResponse,
            decimal? totalMachineScore,
            bool? itemAttempted,
            bool? interactionAttempted);

        void SaveBlankInteraction(
            string formId,
            int partId,
            int testUserFormPartId,
            string itemId,
            string interactionId,
            decimal? score,
            decimal? manualScore,
            decimal? itemTotalScore,
            bool? itemAttempted,
            bool? interactionAttempted);


        void SaveChoiceInteractionResponse(
            string formId,
            int partId,
            int testUserFormPartId,
            string itemId,
            string interactionId,
            string value,
            decimal? score,
            decimal? manualScore,
            decimal? itemTotalScore,
            int itemScoreStatus,
            string itemResponse,
            int interactionScorestatus,
            string interactionResponse,
            decimal? totalMachineScore,
            bool? itemAttempted,
            bool? interactionAttempted);

        void SaveDragDropInteractionResponse(
            string formId,
            int partId,
            int testUserFormPartId,
            string itemId,
            string interactionId,
            string target,
            string source,
            decimal? score,
            decimal? manualScore,
            decimal? itemTotalScore,
            int itemScoreStatus,
            string itemResponse,
            int interactionScoreStatus,
            string interactionResponse,
            decimal? totalMachineScore,
            bool? itemAttempted,
            bool? interactionAttempted);

        void SaveDropDownInteractionResponse(
            string formId,
            int partId,
            int testUserFormPartId,
            string itemId,
            string interactionId,
            string value,
            decimal? score,
            decimal? manualScore,
            decimal? itemTotalScore,
            int itemScoreStatus,
            string itemResponse,
            int interactionScoreStatus,
            string interactionResponse,
            decimal? totalMachineScore,
            bool? itemAttempted,
            bool? interactionAttempted);

        void SaveFillInBlankInteractionResponse(
            string formId,
            int partId,
            int testUserFormPartId,
            string itemId,
            string interactionId,
            string value,
            decimal? score,
            decimal? manualScore,
            decimal? itemTotalScore,
            int itemScoreStatus,
            string itemResponse,
            int interactionScoreStatus,
            string interactionResponse,
            decimal? totalMachineScore,
            bool? hasAlert,
            string nonScores,
            bool? itemAttempted,
            bool? interactionAttempted);

        void SaveImageMapInteractionResponse(
            string formId,
            int partId,
            int testUserFormPartId,
            string itemId,
            string interactionId,
            string value,
            decimal? score,
            decimal? manualScore,
            decimal? itemTotalScore,
            int itemScoreStatus,
            string itemResponse,
            int interactionScorestatus,
            string interactionResponse,
            decimal? totalMachineScore,
            bool? itemAttempted,
            bool? interactionAttempted);

        void SaveItemResponse(
            int testUserFormPartId,
            string itemId,
            string formId,
            int partId);

        void SaveLineMatchingInteractionResponse(
            string formId,
            int partId,
            int testUserFormPartId,
            string itemId,
            string interactionId,
            string target,
            string source,
            decimal? score,
            decimal? manualScore,
            decimal? itemTotalScore,
            int itemScoreStatus,
            string itemResponse,
            int interactionScoreStatus,
            string interactionResponse,
            decimal? totalMachineScore,
            bool? itemAttempted,
            bool? interactionAttempted);

        void SaveRowColumnInteractionResponse(
            string formId,
            int partId,
            int testUserFormPartId,
            string itemId,
            string interactionId,
            string target,
            string source,
            decimal? score,
            decimal? manualScore,
            decimal? itemTotalScore,
            int itemScoreStatus,
            string itemResponse,
            int interactionScoreStatus,
            string interactionResponse,
            decimal? totalMachineScore,
            bool? itemAttempted,
            bool? interactionAttempted);

        void SaveSelectTextInteractionResponse(
            string formId,
            int partId,
            int testUserFormPartId,
            string itemId,
            string interactionId,
            string value,
            decimal? score,
            decimal? manualScore,
            decimal? itemTotalScore,
            int itemScoreStatus,
            string itemResponse,
            int interactionScorestatus,
            string interactionResponse,
            decimal? totalMachineScore,
            bool? itemAttempted,
            bool? interactionAttempted);

        void SaveTextBoxInteractionResponse(
            string formId,
            int partId,
            int testUserFormPartId,
            string itemId,
            string interactionId,
            string value,
            decimal? score,
            decimal? manualScore,
            decimal? itemTotalScore,
            int itemScoreStatus,
            string itemResponse,
            int interactionScorestatus,
            string interactionResponse,
            decimal? totalMachineScore,
            bool? hasAlert,
            string nonScores,
            bool? itemAttempted,
            bool? interactionAttempted);

        void UpdateTestUserFormPartFailedFlag(int testUserFormPartId);

        void UpdateTestUserFormPartProcessedFlag(int testUserFormPartId);
        void UpdateDbErrorLog(int testUserFormPartId, string currentStateId, string interactionId, string message, int threadId, int procId);
        void UpdateDbSuccessLog(int testUserFormPartId, string message, int threadId, int procId);
        void UpdateDbSummaryLog(int procId, string totalSuccess, string totalErrors, string ProjectId, string runId);
    }
}