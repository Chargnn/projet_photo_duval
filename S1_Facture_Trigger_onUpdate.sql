USE [H18_Proj_Eq07]
GO

CREATE TRIGGER trg_Update_Facture
ON Factures.Facture
AFTER UPDATE
AS
BEGIN
	DECLARE @paye int, @Seance_ID int
	SELECT @paye = EstPayee, @Seance_ID = Seance_ID FROM inserted
	IF @paye = 1
		BEGIN
			UPDATE Seances.Seance SET Statut = 'payée' WHERE Seance_ID = @Seance_ID
		END
END
