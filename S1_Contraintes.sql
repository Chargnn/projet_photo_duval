use [H18_Proj_Eq07];

GO
ALTER TABLE Agents.Agent
	ADD CONSTRAINT CHK_Agent_Nom
			CHECK(Nom NOT LIKE '%[0-9]%'),
		CONSTRAINT CHK_Agent_Prenom
			CHECK(Prenom NOT LIKE '%[0-9]%'),
		CONSTRAINT CHK_Agent_Telephone
			CHECK(Telephone NOT LIKE '%[A-Za-z]%'),
		CONSTRAINT CHK_Agent_Courriel
			CHECK(Courriel LIKE '%_@__%.__%'),
		CONSTRAINT UQ_Agent_Courriel
			UNIQUE(Courriel);
GO

GO
ALTER TABLE Photographes.Photographe
	ADD CONSTRAINT CHK_Photographe_Nom
			CHECK(Nom NOT LIKE '%[0-9]%'),
		CONSTRAINT CHK_Photographe_Prenom
			CHECK(Prenom NOT LIKE '%[0-9]%'),
		CONSTRAINT CHK_Photographe_Telephone
			CHECK(Telephone NOT LIKE '%[A-Za-z]%'),
		CONSTRAINT CHK_Photographe_Courriel
			CHECK(Courriel LIKE '%_@__%.__%'),
		CONSTRAINT UQ_Photographe_Courriel
			UNIQUE(Courriel);
GO

GO
ALTER TABLE Seances.Seance
	ADD CONSTRAINT CHK_Seance_Date
			CHECK(DateSeance > CURRENT_TIMESTAMP),
		CONSTRAINT CHK_Seance_Ville
			CHECK(Ville NOT LIKE '%[0-9]%'),
		CONSTRAINT CHK_Seance_Statut
			CHECK(Statut IN ('demandée','confirmée','reportée','réalisée','facturée','payée')),
		CONSTRAINT CHK_Seance_DateFinSeance
			CHECK(DateFinSeance > DateSeance)
GO

GO
ALTER TABLE Factures.Facture
	ADD CONSTRAINT CHK_Facture_Prix
			CHECK(Prix > 0),
		CONSTRAINT CHK_Seance_EstPayee
			CHECK(EstPayee IN (0,1))
GO