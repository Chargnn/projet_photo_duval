USE [H18_Proj_Eq07]
GO

CREATE NONCLUSTERED INDEX IX_FK_Facture_Seance_ID
ON Factures.Facture
(
	Seance_ID
);
GO

CREATE NONCLUSTERED INDEX IX_FK_Seance_Photographe_ID
ON Seances.Seance
(
	Photographe_ID
);
GO

CREATE NONCLUSTERED INDEX IX_FK_Seance_Agent_ID
ON Seances.Seance
(
	Agent_ID
);
GO

CREATE NONCLUSTERED INDEX IX_FK_Photo_Seance_ID
ON Photos.Photo
(
	Seance_ID
);
GO

CREATE NONCLUSTERED INDEX IX_Seance_DateSeance
ON [Seances].[Seance]
(
	DateSeance
);
GO

CREATE NONCLUSTERED INDEX IX_Agent_Nom_Prenom_Telephone
ON Agents.Agent
(
	Nom,
	Prenom,
	Telephone
);
GO