USE [H18_Proj_Eq07]
GO
CREATE TRIGGER trg_Update_Facture
ON Factures.Facture
AFTER INSERT, UPDATE
AS
BEGIN
	DECLARE @paye int, @Seance_ID int, @action char
	SELECT @paye = EstPayee, @Seance_ID = Seance_ID FROM inserted
		
		SET @action = 
		CASE
			WHEN EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) THEN 'U'
			WHEN EXISTS (SELECT * FROM inserted) AND NOT EXISTS (SELECT * FROM deleted) THEN 'I'
			WHEN NOT EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) THEN 'D'
			END
	
	IF @action = 'I'
	BEGIN
		UPDATE Seances.Seance SET Statut = 'facturée' WHERE Seance_ID = @Seance_ID
	END

	IF @action = 'U'
	BEGIN	
		IF @paye = 1
		BEGIN
			UPDATE Seances.Seance SET Statut = 'payée' WHERE Seance_ID = @Seance_ID
		END
	END
END
