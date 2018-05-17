USE	[H18_Proj_Eq07]
GO
IF OBJECT_ID('usp_RapportMensuel_Vente_Agent','P') IS NOT NULL DROP PROCEDURE usp_RapportMensuel_Vente_Agent;
GO
CREATE PROCEDURE usp_RapportMensuel_Vente_Agent
@date datetime2
AS
BEGIN
	DECLARE @mois int, @annee int;
	SET @mois = DATEPART(MONTH, @date);
	SET @annee = DATEPART(YEAR, @date);

	WITH QtyPayee (AgentID, QtyPayee)
	AS
	(
		SELECT s.Agent_ID, f.Prix 'QtyPayee'
		FROM Factures.Facture f 
		INNER JOIN Seances.Seance s ON f.Seance_ID = s.Seance_ID
		WHERE EstPayee = 1
	),

	QtyDue (AgentID, QtyDue)
	AS
	(
		SELECT s.Agent_ID, f.Prix 'QtyDue'
		FROM Factures.Facture f
		INNER JOIN Seances.Seance s ON f.Seance_ID = s.Seance_ID
		WHERE EstPayee = 0
	)

	SELECT a.Agent_ID, a.Nom, a.Prenom, SUM(f.Prix) as 'Ventes', SUM(p.QtyPayee) as N'Somme Payée', SUM(d.QtyDue) as N'Somme Due'
	FROM Agents.Agent a
	INNER JOIN Seances.Seance s ON a.Agent_ID = s.Agent_ID
	INNER JOIN Factures.Facture f ON s.Seance_ID = f.Seance_ID
	LEFT JOIN QtyPayee p ON a.Agent_ID = p.AgentID
	LEFT JOIN QtyDue d ON a.Agent_ID = d.AgentID
	WHERE DATEPART(month, f.DateFacturation) = @mois AND DATEPART(year, f.DateFacturation) = @annee
	GROUP BY a.Agent_ID, a.Nom, a.Prenom
	ORDER BY Ventes DESC;
END
GO
