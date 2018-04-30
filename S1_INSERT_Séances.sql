USE [H18_Proj_Eq07]
GO

INSERT Seances.Seance (Photographe_ID, Agent_ID, Adresse, Ville, DateSeance, DateFinSeance, Statut)
	VALUES
		(1, 1, '420 rue Laflamme', 'Longueuil', '2018-04-20 16:20:00', '2018-04-20 17:10:00', 'facturée'),
		(1, 4, '62 rue Bourgogne', 'Laprairie', '2018-04-20 18:30:00', '2018-04-20 19:30:00', 'facturée'),
		(1, 4, '1350 ch. du Lac', 'Laprairie', '2018-04-20 19:45:00', '2018-04-20 20:15:00', 'facturée'),
		(2, 2, '444 rue Gourde', 'Longueuil', '2018-04-21 10:00:00', '2018-04-21 10:45:00', 'livrée'),
		(2, 1, '802 place Laplace', 'Longueuil', '2018-04-21 11:00:00', '2018-04-21 11:50:00', 'livrée'),
		(3, 2, '92 boulevard Castonguay', 'Laval', '2018-04-21 11:00:00', '2018-04-21 11:45:00', 'réalisée'),
		(2, 6, '2222 avenue Gascoigne', 'Boucherville', '2018-04-21 13:30:00', '2018-04-21 15:00:00', 'réalisée'),
		(2, 5, '305 rue Bastien', 'Boucherville', '2018-04-21 16:00:00', '2018-04-21 16:30:00', 'réalisée'),
		(1, 4, '11 rue Gouache', 'Laval', '2018-04-24 13:00:00', NULL, 'demandée'),
		(2, 5, '88 rue Roberval', 'Anjou', '2018-04-25 14:00:00', NULL, 'confirmée');
GO
