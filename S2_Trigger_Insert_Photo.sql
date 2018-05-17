USE [H18_Proj_Eq07]
GO

CREATE TRIGGER trg_Insert_Photo
ON Photos.Photo
AFTER INSERT
AS
BEGIN
	DECLARE @Seance_ID int, @statut nvarchar(20)
	SELECT @Seance_ID = i.Seance_ID, @statut = s.Statut
	FROM inserted i INNER JOIN Seances.Seance s ON i.Seance_ID = s.Seance_ID
	IF @statut NOT IN(N'livr�e',N'factur�e',N'pay�e')
	BEGIN
		UPDATE Seances.Seance SET Statut=N'livr�e' WHERE Seance_ID = @Seance_ID
	END

END