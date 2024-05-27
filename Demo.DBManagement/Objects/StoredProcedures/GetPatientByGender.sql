CREATE  PROCEDURE [dbo].[GetPatientByGender] 
@Gender CHAR(1),
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
	WHERE Gender = @Gender
	ORDER BY PatientID
	OFFSET @Offset ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
