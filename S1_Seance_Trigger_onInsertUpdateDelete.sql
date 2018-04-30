USE [H18_Proj_Eq07]
GO

CREATE TRIGGER trg_IUD_Seance
ON Seances.Seance
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
	IF TRIGGER_NESTLEVEL() <= 1
	BEGIN
		DECLARE @statutSeance nvarchar(20), @Seance_ID int, @Agent_ID int
	
		DECLARE  @action CHAR(1);
		SET @action = 
		CASE
			WHEN EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) THEN 'U'
			WHEN EXISTS (SELECT * FROM inserted) AND NOT EXISTS (SELECT * FROM deleted) THEN 'I'
			WHEN NOT EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) THEN 'D'
			END
	
		IF @action = 'I'
		BEGIN
			SELECT @Seance_ID = Seance_ID FROM inserted
			UPDATE [Seances].[Seance] SET Statut = N'demandée' WHERE Seance_ID = @Seance_ID
		END

		IF @action = 'U'
		BEGIN
			SELECT @statutSeance = Statut, @Seance_ID = Seance_ID, @Agent_ID = Agent_ID FROM inserted
			IF @statutSeance = N'facturée'
			BEGIN
				INSERT INTO Factures.Facture (Seance_ID, EstPayee, Prix)
					VALUES (@Seance_ID, 0, 0)
			END
		
			IF @statutSeance = N'payée'
			BEGIN
				INSERT INTO [Seances].[Hist_Seance] (Seance_ID, Photographe_ID, Agent_ID, Adresse, Ville, Statut, DateSeance, DateFinSeance, DateHistSeance)
				SELECT Seance_ID, Photographe_ID, Agent_ID, Adresse, Ville, Statut, DateSeance, DateFinSeance, CURRENT_TIMESTAMP
				FROM inserted
				WHERE Seance_ID = @Seance_ID

				DELETE FROM [Seances].[Seance]
				WHERE Seance_ID = @Seance_ID
			END
		END

		IF @action = 'D'
		BEGIN
			SELECT @Seance_ID = Seance_ID FROM deleted
			INSERT INTO [Seances].[Hist_Seance] (Seance_ID, Photographe_ID, Agent_ID, Adresse, Ville, Statut, DateSeance, DateFinSeance, DateHistSeance)
				SELECT Seance_ID, Photographe_ID, Agent_ID, Adresse, Ville, Statut, DateSeance, DateFinSeance, CURRENT_TIMESTAMP
				FROM deleted
				WHERE Seance_ID = @Seance_ID
		END
	END
END
GO