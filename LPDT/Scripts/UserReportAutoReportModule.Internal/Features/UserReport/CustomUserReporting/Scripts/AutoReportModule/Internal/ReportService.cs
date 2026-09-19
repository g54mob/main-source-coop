using System.Collections.Generic;
using Features.UserReport.CustomUserReporting.Scripts.AutoReportModule.API;
using Features.UserReport.CustomUserReporting.Scripts.AutoReportModule.API.Configurations;
using Features.UserReport.CustomUserReporting.Scripts.Client;
using Features.UserReport.CustomUserReporting.Scripts.Plugin;
using UnityEngine;

namespace Features.UserReport.CustomUserReporting.Scripts.AutoReportModule.Internal
{
	public class ReportService : IReportService
	{
		public void SendReport(ReportConfiguration reportConfiguration)
		{
			UnityUserReporting.Configure(Application.cloudProjectId, reportConfiguration.UserReportingClientConfiguration);
			UnityUserReporting.CurrentClient.Update();
			UnityUserReporting.CurrentClient.CreateUserReport(delegate(Features.UserReport.CustomUserReporting.Scripts.Client.UserReport userReport)
			{
				UserReportCallback(userReport, reportConfiguration);
			});
		}

		private void UserReportCallback(Features.UserReport.CustomUserReporting.Scripts.Client.UserReport userReport, ReportConfiguration reportConfiguration)
		{
			userReport.Summary = reportConfiguration.ReportName;
			AppendAttachmentData(userReport, reportConfiguration.Attachments);
			AppendDimensionData(userReport, reportConfiguration.Dimensions);
			AppendFields(userReport, reportConfiguration.Fields);
			UnityUserReporting.CurrentClient.SendUserReport(userReport, ProgressCallback, Callback);
		}

		private void AppendDimensionData(Features.UserReport.CustomUserReporting.Scripts.Client.UserReport userReport, IReadOnlyList<ReportDimension> reportConfigurationDimensions)
		{
			foreach (ReportDimension reportConfigurationDimension in reportConfigurationDimensions)
			{
				reportConfigurationDimension.Context = userReport;
				userReport.Dimensions.Add(new UserReportNamedValue(reportConfigurationDimension.Name, reportConfigurationDimension.Value));
			}
		}

		private void AppendAttachmentData(Features.UserReport.CustomUserReporting.Scripts.Client.UserReport userReport, IReadOnlyList<ReportAttachment> reportConfigurationAttachments)
		{
			foreach (ReportAttachment reportConfigurationAttachment in reportConfigurationAttachments)
			{
				reportConfigurationAttachment.Context = userReport;
				userReport.Attachments.Add(new UserReportAttachment(reportConfigurationAttachment.Name, reportConfigurationAttachment.FileName, reportConfigurationAttachment.ContentType, reportConfigurationAttachment.GetData()));
			}
		}

		private void AppendFields(Features.UserReport.CustomUserReporting.Scripts.Client.UserReport userReport, List<ReportField> reportConfigurationFields)
		{
			foreach (ReportField reportConfigurationField in reportConfigurationFields)
			{
				userReport.Fields.Add(new UserReportNamedValue(reportConfigurationField.Name, reportConfigurationField.Value));
			}
		}

		private void ProgressCallback(float uploadProgress, float downloadProgress)
		{
			Debug.Log(uploadProgress);
		}

		private void Callback(bool success, Features.UserReport.CustomUserReporting.Scripts.Client.UserReport userReport)
		{
			Debug.LogError("Report sent: " + success);
		}
	}
}
