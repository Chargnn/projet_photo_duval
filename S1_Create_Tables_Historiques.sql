USE [H18_Proj_Eq07]
GO

CREATE TABLE Seances.Hist_Seance
(
	Hist_Seance_ID int NOT NULL IDENTITY(1,1),
	Seance_ID int NOT NULL,
	Photographe_ID int NOT NULL,
	Agent_ID int NOT NULL,
	Adresse nvarchar(50) NOT NULL,
	Ville varchar(50) NOT NULL,
	Statut varchar(30) NOT NULL,
	DateSeance datetime2 NOT NULL,
	DateFinSeance datetime2,
	DateHistSeance datetime2 NOT NULL

	CONSTRAINT PK_Hist_Seance_ID PRIMARY KEY (Hist_Seance_ID)
) ON [PRIMARY];
GO

CREATE TABLE Factures.Hist_Facture
(
	Hist_Facture_ID int NOT NULL IDENTITY(1,1),
	Seance_ID int NOT NULL,
	Prix decimal(18,0) NOT NULL,
	EstPayee int NOT NULL,
	DateHistFacture datetime2 NOT NULL

	CONSTRAINT PK_Hist_Facture_ID PRIMARY KEY (Hist_Facture_ID)
) ON [PRIMARY];
GO

CREATE TABLE Photos.Hist_Photo
(
	Hist_Photo_ID int NOT NULL IDENTITY(1,1),
	Photo_ID int NOT NULL,
	Photo image,
	Seance_ID int NOT NULL,
	Nom varchar(50),
	DateHistPhoto datetime2 NOT NULL

	CONSTRAINT PK_Hist_Photo_ID PRIMARY KEY (Hist_Photo_ID)
) ON [PRIMARY];
GO