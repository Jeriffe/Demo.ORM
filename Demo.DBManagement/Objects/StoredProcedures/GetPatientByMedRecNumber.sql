CREATE  PROCEDURE [dbo].[GetPatientByMedRecNumber] 
@MedRecNumber INT,
@Offset INT,
@PageSize INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
	     P.PatientID
		,P.FirstName
		,P.MiddleInitial
		,P.LastName
		,P.BirthDate
		,P.Gender
		,P.MedRecNumber
		,P.PatientType
		,P.SiteID
	FROM dbo.T_PATIENT AS P
	INNER JOIN dbo.T_VISIT AS V 
		ON P.PatientID = V.PatientID
	WHERE V.CareUnitID = @MedRecNumber
	ORDER BY PatientID
	OFFSET @Offset ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
