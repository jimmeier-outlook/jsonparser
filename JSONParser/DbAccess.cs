using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace JsonParser
{
    public class DbAccess : IDbAccess
    {
        private readonly string _connectionString;

        public DbAccess(string connectionString)
        {
            _connectionString = connectionString;
        }
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IList<StudentResponse> GetAllStudentResponses(ProjectConfig projectConfig)
        {
            var parameters = new List<SqlParameter>
            {
                BuildParameter("@AccommodatedForms", SqlDbType.Bit, projectConfig.IncludeBrailleAndPaperPencil),
                BuildParameter("@FormIDList", SqlDbType.VarChar, projectConfig.FormId),
                BuildParameter("@Admin", SqlDbType.Int, projectConfig.Admin),
                BuildParameter("@SubjectCode", SqlDbType.VarChar, projectConfig.SubjectCode == "" ? null : projectConfig.SubjectCode),
                BuildParameter("@BatchSize", SqlDbType.Int, projectConfig.BatchSize)
            };
            var dtStudentResponse = GetDataTable("prcStudentState_GetNotProcessed", parameters);
            var studentResponses = dtStudentResponse.AsEnumerable().Select(row => new StudentResponse
            {
                ResponseJson = row["CurrentStateObject"].ToString(),
                FormId = row["Form_ID"].ToString(),
                PartId = int.Parse(row["Part_ID"].ToString()),
                Tufp = int.Parse(row["TestUser_Form_Part_ID"].ToString())
            }).ToList();
            return studentResponses;
        }

        public void UpdateTestUserFormPartProcessedFlag(int testUserFormPartId)
            => UpsertData("prcStudentState_UpdateProcessed", new List<SqlParameter> { BuildParameter("@TestUser_Form_Part_ID", SqlDbType.Int, testUserFormPartId) });

        public void UpdateTestUserFormPartFailedFlag(int testUserFormPartId)
            => UpsertData("prcStudentState_UpdateFailed", new List<SqlParameter> { BuildParameter("@TestUser_Form_Part_ID", SqlDbType.Int, testUserFormPartId) });

        public IEnumerable<Interaction> GetAllInteractions()
            => GetDataTable("prcInteractionDefinition_Get")
                .AsEnumerable()
                .Select(row => new Interaction
                {
                    ItemCode = row["ItemCode"].ToString(),
                    InteractionId = row["InteractionID"].ToString(),
                    InteractionType = row["InteractionType"].ToString(),
                    ItemDefinitionID = int.Parse(row["ItemDefinitionID"].ToString())
                });

        public void SaveDragDropInteractionResponse(
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
            bool? interactionAttempted)
        {
            var parameters = new List<SqlParameter>
            {
                BuildParameter("@Target_GUID", SqlDbType.VarChar, target ?? (object) DBNull.Value),
                BuildParameter("@Source_GUID", SqlDbType.VarChar, source ?? (object) DBNull.Value)
            };
            parameters.AddRange(AddCommonParameters(formId, partId, testUserFormPartId, itemId, interactionId, score, manualScore, itemTotalScore, itemAttempted, interactionAttempted));
            parameters.AddRange(AddCommonParametersforResponse(itemScoreStatus, itemResponse, interactionScoreStatus, interactionResponse, totalMachineScore));

            UpsertData("prcInteraction_ResponseDragDrop_Insert", parameters);
        }

        public void SaveLineMatchingInteractionResponse(
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
            bool? interactionAttempted)
        {
            var parameters = new List<SqlParameter>
            {
                BuildParameter("@Target_GUID", SqlDbType.VarChar, target ?? (object) DBNull.Value),
                BuildParameter("@Source_GUID", SqlDbType.VarChar, source ?? (object) DBNull.Value)
            };
            parameters.AddRange(AddCommonParameters(formId, partId, testUserFormPartId, itemId, interactionId, score, manualScore, itemTotalScore, itemAttempted, interactionAttempted));
            parameters.AddRange(AddCommonParametersforResponse(itemScoreStatus, itemResponse, interactionScoreStatus, interactionResponse, totalMachineScore));

            UpsertData("prcInteraction_ResponseLineMatch_Insert", parameters);
        }

        public void SaveChoiceInteractionResponse(
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
            bool? interactionAttempted)
        {
            var parameters = new List<SqlParameter>
            {
                BuildParameter("@ChoiceID", SqlDbType.VarChar, value ?? (object) DBNull.Value)
            };
            parameters.AddRange(AddCommonParameters(formId, partId, testUserFormPartId, itemId, interactionId, score, manualScore, itemTotalScore, itemAttempted, interactionAttempted));
            parameters.AddRange(AddCommonParametersforResponse(itemScoreStatus, itemResponse, interactionScorestatus, interactionResponse, totalMachineScore));

            UpsertData("prcInteraction_ResponseChoice_Insert", parameters);
        }


        public void SaveAudioRecInteractionResponse(
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
            bool? interactionAttempted)
        {
            var parameters = new List<SqlParameter>
            {
                BuildParameter("@RecDuration", SqlDbType.VarChar, recDuration ?? (object) DBNull.Value),
                BuildParameter("@AssetIdentifier", SqlDbType.VarChar, assetIdentifier ?? (object) DBNull.Value)
            };
            parameters.AddRange(AddCommonParameters(formId, partId, testUserFormPartId, itemId, interactionId, score, manualScore, itemTotalScore, itemAttempted, interactionAttempted));
            parameters.AddRange(AddCommonParametersforResponse(itemScoreStatus, itemResponse, interactionScorestatus, interactionResponse, totalMachineScore));

            UpsertData("prcInteraction_ResponseAudioRec_Insert", parameters);
        }

        public void SaveTextBoxInteractionResponse(
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
            bool? interactionAttempted)
        {
            var parameters = new List<SqlParameter>
            {
                BuildParameter("@TextBoxResponse", SqlDbType.VarChar, value ?? (object) DBNull.Value)
            };
            parameters.AddRange(AddCommonParameters(formId, partId, testUserFormPartId, itemId, interactionId, score, manualScore, itemTotalScore, itemAttempted, interactionAttempted));
            parameters.AddRange(AddCommonParametersforFillInBlankandResponseTextBox(itemScoreStatus, itemResponse, interactionScorestatus, totalMachineScore, hasAlert, nonScores));

            UpsertData("prcInteraction_ResponseTextBox_Insert", parameters);
        }

        public void SaveImageMapInteractionResponse(
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
            bool? interactionAttempted)
        {
            var parameters = new List<SqlParameter>
            {
                BuildParameter("@ChoiceID", SqlDbType.VarChar, value ?? (object) DBNull.Value)
            };
            parameters.AddRange(AddCommonParameters(formId, partId, testUserFormPartId, itemId, interactionId, score, manualScore, itemTotalScore, itemAttempted, interactionAttempted));
            parameters.AddRange(AddCommonParametersforResponse(itemScoreStatus, itemResponse, interactionScorestatus, interactionResponse, totalMachineScore));

            UpsertData("prcInteraction_ResponseImageMap_Insert", parameters);
        }

        public void SaveSelectTextInteractionResponse(
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
            bool? interactionAttempted)
        {
            var parameters = new List<SqlParameter>
            {
                BuildParameter("@ChoiceID", SqlDbType.VarChar, value ?? (object) DBNull.Value)
            };
            parameters.AddRange(AddCommonParameters(formId, partId, testUserFormPartId, itemId, interactionId, score, manualScore, itemTotalScore, itemAttempted, interactionAttempted));
            parameters.AddRange(AddCommonParametersforResponse(itemScoreStatus, itemResponse, interactionScorestatus, interactionResponse, totalMachineScore));

            UpsertData("prcInteraction_ResponseSelectText_Insert", parameters);
        }

        public void SaveDropDownInteractionResponse(
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
            bool? interactionAttempted)
        {
            var parameters = new List<SqlParameter>
            {
                BuildParameter("@ChoiceID", SqlDbType.VarChar, value ?? (object) DBNull.Value)
            };
            parameters.AddRange(AddCommonParameters(formId, partId, testUserFormPartId, itemId, interactionId, score, manualScore, itemTotalScore, itemAttempted, interactionAttempted));
            parameters.AddRange(AddCommonParametersforResponse(itemScoreStatus, itemResponse, interactionScoreStatus, interactionResponse, totalMachineScore));

            UpsertData("prcInteraction_ResponseDropDown_Insert", parameters);
        }

        public void SaveBlankInteraction(
            string formId,
            int partId,
            int testUserFormPartId,
            string itemId,
            string interactionId,
            decimal? score,
            decimal? manualScore,
            decimal? itemTotalScore,
            bool? itemAttempted,
            bool? interactionAttempted)
        {
            var parameters = new List<SqlParameter>
            {
                BuildParameter("@ChoiceID", SqlDbType.VarChar, DBNull.Value)
            };
            parameters.AddRange(AddCommonParameters(formId, partId, testUserFormPartId, itemId, interactionId, score, manualScore, itemTotalScore, itemAttempted, interactionAttempted));


            UpsertData("prcInteraction_ResponseBlank_Insert", parameters);
        }

        public void SaveFillInBlankInteractionResponse(
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
            bool? interactionAttempted)
        {
            var parameters = new List<SqlParameter>
            {
                BuildParameter("@FillInBlankResponse", SqlDbType.VarChar, value ?? (object) DBNull.Value)
            };
            parameters.AddRange(AddCommonParameters(formId, partId, testUserFormPartId, itemId, interactionId, score, manualScore, itemTotalScore, itemAttempted, interactionAttempted));
            parameters.AddRange(AddCommonParametersforFillInBlankandResponseTextBox(itemScoreStatus, itemResponse, interactionScoreStatus, totalMachineScore,hasAlert,nonScores));

            UpsertData("prcInteraction_ResponseFillInBlank_Insert", parameters);
        }

        public void SaveRowColumnInteractionResponse(
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
            bool? interactionAttempted)
        {
            var parameters = new List<SqlParameter>
            {
                BuildParameter("@Row_GUID", SqlDbType.VarChar, target ?? (object) DBNull.Value),
                BuildParameter("@Col_GUID", SqlDbType.VarChar, source ?? (object) DBNull.Value)
            };
            parameters.AddRange(AddCommonParameters(formId, partId, testUserFormPartId, itemId, interactionId, score, manualScore, itemTotalScore, itemAttempted, interactionAttempted));
            parameters.AddRange(AddCommonParametersforResponse(itemScoreStatus, itemResponse, interactionScoreStatus, interactionResponse, totalMachineScore));

            UpsertData("prcInteraction_ResponseRowColumn_Insert", parameters);
        }

        public void SaveItemResponse(
            int testUserFormPartId,
            string itemId,
            string formId,
            int partId)
        {
            var parameters = new List<SqlParameter>
            {
                BuildParameter("@Form_Id", SqlDbType.VarChar, formId),
                BuildParameter("@Part_Id", SqlDbType.Int, partId),
                BuildParameter("@TestUser_Form_Part_ID", SqlDbType.Int, testUserFormPartId),
                BuildParameter("@Item_GUID", SqlDbType.UniqueIdentifier, itemId)
            };
            UpsertData("prcMissingItemResponse_Insert", parameters);
        }

        public void UpdateDbErrorLog(
            int testUserFormPartId,
            string currentStateId,
            string interactionId,
            string message,
            int threadId,
            int procId)
        {
            var parameters = new List<SqlParameter>
            {
                BuildParameter("@TUFPId", SqlDbType.Int, testUserFormPartId),
                BuildParameter("@itemid", SqlDbType.VarChar, currentStateId),
                BuildParameter("@interactionID", SqlDbType.VarChar, interactionId),
                BuildParameter("@exMessage", SqlDbType.VarChar, message),
                BuildParameter("@ThreadId", SqlDbType.Int, threadId),
                BuildParameter("@procID", SqlDbType.Int, procId)

            };
            UpsertData("prcUpdateDBErrorLog", parameters);
        }

        public void UpdateDbSuccessLog(
            int testUserFormPartId,
            string message,
            int threadId,
            int procId)
        {
            var parameters = new List<SqlParameter>
            {
                BuildParameter("@TUFPId", SqlDbType.Int, testUserFormPartId),
                BuildParameter("@exMessage", SqlDbType.VarChar, message),
                BuildParameter("@ThreadId", SqlDbType.Int, threadId),
                BuildParameter("@procID", SqlDbType.Int, procId)

            };
            UpsertData("UpdateDbSuccessLog", parameters);
        }

        public void UpdateDbSummaryLog(
            int procId,
            string totalSuccess,
            string totalErrors,
            string ProjectId,
            string runId)
        {
            var parameters = new List<SqlParameter>
            {
                BuildParameter("@procID", SqlDbType.Int, procId),
                BuildParameter("@totalSuccess", SqlDbType.VarChar, totalSuccess),
                BuildParameter("@totalErrors", SqlDbType.VarChar, totalErrors),
                BuildParameter("@projectId", SqlDbType.VarChar, ProjectId),
                BuildParameter("@runId", SqlDbType.VarChar, runId),
            };
            UpsertData("UpdateProcessSummary", parameters);
        }

        private List<SqlParameter> AddCommonParameters(
            string formId,
            int partId,
            int tufpId,
            string itemId,
            string interactionId,
            decimal? score,
            decimal? manualScore,
            decimal? itemTotalScore,
            bool? itemAttempted,
            bool? interactionAttempted)
        {
            return new List<SqlParameter>
            {
                BuildParameter("@Form_Id", SqlDbType.VarChar, formId),
                BuildParameter("@Part_Id", SqlDbType.Int, partId),
                BuildParameter("@TestUser_Form_Part_ID", SqlDbType.Int, tufpId),
                BuildParameter("@ItemCode", SqlDbType.VarChar, itemId),
                BuildParameter("@InteractionID", SqlDbType.VarChar, interactionId),
                BuildParameter("@Score", SqlDbType.Decimal, score),
                BuildParameter("@HandScore", SqlDbType.Decimal, manualScore),
                BuildParameter("@ItemTotalScore", SqlDbType.Decimal, itemTotalScore),
                BuildParameter("@ItemAttempted",SqlDbType.Bit,itemAttempted),
                BuildParameter("@InteractionAttempted",SqlDbType.Bit,interactionAttempted)
            };
        }

        private List<SqlParameter> AddCommonParametersforResponse(
            int? itemScoreStatus,
            string itemResponse,
            int interactionScorestatus,
            string interactionResponse,
            decimal? totalMachineScore)
        {
            return new List<SqlParameter>
            {
                BuildParameter("@MachineScore", SqlDbType.Decimal, totalMachineScore),
                BuildParameter("@ItemScoreStatus", SqlDbType.TinyInt, itemScoreStatus),
                BuildParameter("@ItemResponse", SqlDbType.VarChar, itemResponse),
                BuildParameter("@InteractionScorestatus", SqlDbType.TinyInt, interactionScorestatus),
                BuildParameter("@InteractionResponse", SqlDbType.VarChar, interactionResponse),
            };
        }

        private List<SqlParameter> AddCommonParametersforFillInBlankandResponseTextBox(
            int? itemScoreStatus,
            string itemResponse,
            int interactionScorestatus,
            decimal? totalMachineScore,
            bool? hasAlert,
            string nonScores)
        {
            return new List<SqlParameter>
            {
                BuildParameter("@MachineScore", SqlDbType.Decimal, totalMachineScore),
                BuildParameter("@ItemResponse", SqlDbType.VarChar, itemResponse),
                BuildParameter("@ItemScoreStatus", SqlDbType.TinyInt, itemScoreStatus),
                BuildParameter("@InteractionScorestatus", SqlDbType.TinyInt, interactionScorestatus),
                BuildParameter("@hasAlert", SqlDbType.Bit, hasAlert),
                BuildParameter("@alertCode", SqlDbType.VarChar, nonScores)
            };
        }

        private SqlParameter BuildParameter(string parameter, SqlDbType type, object value)
            => new SqlParameter { ParameterName = parameter, SqlDbType = type, Value = value };

        private DataTable GetDataTable(string spName, List<SqlParameter> parameters = null)
        {
            var sqlCommand = new SqlCommand
            {
                CommandTimeout = 300
            };

            var sqlConnection = _connectionString;
            using (var conn = new SqlConnection(sqlConnection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = spName;
                sqlCommand.Connection = conn;
                if (parameters != null)
                {
                    sqlCommand.Parameters.AddRange(parameters.ToArray());
                }

                conn.Open();
                var reader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                var dt = new DataTable();
                dt.Load(reader);
                return dt;
            }
        }

        private void UpsertData(string spName, List<SqlParameter> parameters = null)
        {
            var sqlCommand = new SqlCommand();
            var sqlConnection = _connectionString;
            using (var conn = new SqlConnection(sqlConnection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = spName;
                sqlCommand.Connection = conn;
                sqlCommand.CommandTimeout = 60;
                if (parameters != null)
                {
                    sqlCommand.Parameters.AddRange(parameters.ToArray());
                }

                conn.Open();
                sqlCommand.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}