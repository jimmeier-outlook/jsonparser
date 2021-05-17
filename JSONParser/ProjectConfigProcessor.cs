using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using JsonParser.State;
using Newtonsoft.Json;

namespace JsonParser
{
    public class ProjectConfigProcessor
    {
        public InteractionParserFactory InteractionParserFactory { get; } = new InteractionParserFactory();
        public StringBuilder Builder { get; } = new StringBuilder(20000);
        public string ProjectId { get; }
        public ProjectConfig Config { get; }
        public static int ErrorCount { get; private set; } = 0;

        public static int successCount { get; private set; } = 0;

        public static int NProcessId { get; set; } = System.Diagnostics.Process.GetCurrentProcess().Id;

        // public Log ErrorLog { get; }
        public IDbAccess DbAccess { get; }

        public int thisTUFP { get; set; } = -1;

        public static string runId { get; set; }

        public Dictionary<Tuple<string, string>, InteractionType> InteractionsByItemCodeAndInteractionId { get; set; }

        //private static readonly log4net.ILog l4log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string LogPath => Path.Combine(Program.GetBasePath(), Config.LogPath);

        public ProjectConfigProcessor(string projectId, ProjectConfig config, string prunId)
        {
            ProjectId = projectId;
            Config = config;
            DbAccess = new DbAccess(config.ConnectionString);
            runId = prunId;
            Directory.CreateDirectory(LogPath);

            //ErrorLog = BuildErrorLog();
        }

        public void Process()
        {
            CacheInteractionTypes();
            var allResponses = DbAccess.GetAllStudentResponses(Config);
            while (allResponses.Count > 0)
            {
                allResponses.AsParallel().ForAll(studentResponse =>
                {
                    int success = 0;
                    var testUserFormPartId = -1;
                    try
                    {
                        thisTUFP = studentResponse
                            .Tufp; //JFM 01/24 added this to get the TUFP from query for error handling in case json.serialize fails
                        var response = JsonConvert.DeserializeObject<StudentState>(studentResponse.ResponseJson);
                        testUserFormPartId = studentResponse.Tufp;
                        ProcessCurrentState(response, studentResponse);
                        success = 1;
                        
                    }

                    catch (Exception ex)
                        
                    {
                        if (((System.Data.SqlClient.SqlException)ex).Number == 50000)
                        {
                            DbAccess.UpdateTestUserFormPartFailedFlag(testUserFormPartId);
                            DbAccess.UpdateTestUserFormPartFailedFlag(testUserFormPartId);
                            DbAccess.UpdateTestUserFormPartFailedFlag(testUserFormPartId);
                            success = 0;
                        }
                        else
                        {
                            //LogExceptionAsync(ex, 0, "Error", Thread.CurrentThread.ManagedThreadId, NProcessId, "NotPresent", "NotPresent", "NotPresent");
                            success = 0;
                        }

                    }
                    if (success == 1)
                    {
                        DbAccess.UpdateTestUserFormPartProcessedFlag(testUserFormPartId);
                        //LogSuccess(testUserFormPartId, Thread.CurrentThread.ManagedThreadId, NProcessId);
                    }

                });

                allResponses = DbAccess.GetAllStudentResponses(Config);
                
            }
            //LogSummary(NProcessId, ProjectId, successCount.ToString(), ErrorCount.ToString(), runId);
            SendEmailIfNecessary();
        }

        private void ProcessCurrentState(StudentState studentState, StudentResponse studentResponse )
        {
            string testFormId = studentState.TestState.formId;
            foreach (var currentState in studentState.TestState.currentState)
            {
                foreach (var result in currentState.result)
                {
                    foreach (var response in result.responses)
                    {
                        try
                        {
                            var interactionType = GetInteractionType(currentState, response);
                            var interactionParser = InteractionParserFactory.GetInteractionParser(interactionType);
                            UpdateInteraction(interactionParser, response, currentState, studentResponse);
                            SaveInteraction(interactionParser, response);
                        }
                        catch (Exception ex)
                        {
                            //LogExceptionAsync(ex, studentResponse.Tufp, "Error", Thread.CurrentThread.ManagedThreadId, NProcessId, currentState.id, response.intractionId, testFormId);
                            throw;
                        }
                    }
                }
            }
        }

        private InteractionType GetInteractionType(CurrentState currentState, Response response)
        {
            var itemCodeAndInteractionId = Tuple.Create(currentState.id, response.intractionId);
            return InteractionsByItemCodeAndInteractionId.TryGetValue(itemCodeAndInteractionId, out var retrievedInteractionType)
                ? retrievedInteractionType
                : InteractionType.Null;
        }

        private void SendEmailIfNecessary()
        {
            if (ErrorCount <= 0 || Builder.ToString().Length <= 0) return;
            EmailHelper.SendMail(Builder.ToString(), Config.Environment, Config.ProjectName, ProjectId);
        }
                
        private void SaveInteraction(IInteractionParser interaction, Response response)
        {
            if (response.responseChoices.Count > 0)
            {
                SaveMultipleChoiceInteraction(response, interaction);
            }
            else
            {
                interaction.ParseAndSave(DbAccess, null, null);
            }
        }

        private void SaveMultipleChoiceInteraction(Response response, IInteractionParser interaction)
        {
            foreach (var responseChoice in response.responseChoices)
            {
                if (responseChoice.choiceId == string.Empty && responseChoice.value.ToString() == string.Empty)
                {
                    interaction.ParseAndSave(DbAccess, null, null);
                }
                else
                {
                    interaction.ParseAndSave(DbAccess, responseChoice.choiceId, responseChoice.value?.ToString());
                }
            }
        }

        private static void UpdateInteraction(
            IInteractionParser interaction,
            Response response,
            CurrentState currentState,
            StudentResponse studentResponse)
        {
            string alertCodes;
            if (currentState.externalScores != null)
            {
                alertCodes = currentState.externalScores[0].nonScores;
            }
            else {

                alertCodes = "";
            }
            interaction.InteractionId = response.intractionId;
            interaction.ItemId = currentState.id;
            interaction.TufpId = studentResponse.Tufp;
            interaction.FormId = studentResponse.FormId;
            interaction.PartId = studentResponse.PartId;
            interaction.Score = response.score;
            interaction.manualScore = currentState.manualScore;
            interaction.ItemTotalScore = currentState.totalScore;
            interaction.InteractionScorestatus = ConvertStatusToScoreStatus(response.interactionStatus);
            interaction.ItemScoreStatus = ConvertStatusToScoreStatus(currentState.itemStatus);
            interaction.ItemResponse = currentState.interpretation;
            interaction.InteractionResponse = response.interpretation;
            interaction.TotalMachineScore = currentState.totalMachineScore;
            interaction.hasAlert = currentState.hasAlert;
            interaction.nonScores = alertCodes;
            interaction.itemAttempted = currentState.attempted;
            interaction.interactionAttempted = response.attempted;
        }

        private static int ConvertStatusToScoreStatus(string status)
        
            => status == "1" || status == "3" || status == "MachineScored" || status == "MachineScored " || status == "HandScored"
                ? 1

                : 0;

        private void CacheInteractionTypes()
        {
            InteractionsByItemCodeAndInteractionId = DbAccess
                .GetAllInteractions()
                .GroupBy(i => Tuple.Create(i.ItemCode, i.InteractionId))
                .ToDictionary(g => g.Key, g => ParseInteractionType(g.First().InteractionType));
        }

        private static InteractionType ParseInteractionType(string interactionType)
            => interactionType == null ? InteractionType.Null : (InteractionType)Enum.Parse(typeof(InteractionType), interactionType, true);

        //1/21/19 JM changed this method into asynch and added await to the ErrorLog.WriteLogLine to enable waiting.
        //private void LogExceptionAsync(Exception ex, int testUserFormPartId, string errorLevel, int threadId, int procId, string currentStateId, string interactionId, string formId)
        //{
        //    var message = $"Project: {ProjectId}; TUFP: {testUserFormPartId}; FormId: {formId}; CurrentState.Id: {currentStateId}; InteractionId: {interactionId}; Source: {ex.Source}; Message: {ex.Message}";

        //    switch (errorLevel)
        //    {
        //        case "Fatal":
        //            l4log.Fatal(message);
        //            ErrorCount++;
        //            Builder.AppendLine(message).AppendLine();
        //            DbAccess.UpdateDbErrorLog(testUserFormPartId, currentStateId, interactionId, ex.Message, threadId, procId);
        //            break;
        //        case "Error":
        //            l4log.Error(message);
        //            ErrorCount++;
        //            Builder.AppendLine(message).AppendLine();
        //            DbAccess.UpdateDbErrorLog(testUserFormPartId, currentStateId, interactionId, ex.Message, threadId, procId);
        //            break;
        //        case "Warn":
        //            l4log.Warn(message);
        //            break;
        //        case "Info":
        //            l4log.Info(message);
        //            break;
        //        default:
        //            l4log.Debug(message);
        //            break;
        //    }
        //}

        //private void LogSuccess(int testUserFormPartId, int threadId, int procId)
        //{
        //    //var message = $"TUFP {testUserFormPartId}, Message: Parsed Sucessfully";
        //    //l4log.Info(message);
        //    //DbAccess.UpdateDbSuccessLog(testUserFormPartId, "Success", threadId, procId);
        //    successCount++;
        //}

        private void LogSummary(int procId, string ProjectID, string totalSuccess, string totalErrors, string runId)
        {
            DbAccess.UpdateDbSummaryLog(procId, totalSuccess, totalErrors, ProjectId, runId);
        }

    }
}