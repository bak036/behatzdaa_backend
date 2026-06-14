USE [Histadrut]
GO

/****** Object:  View [dbo].[AllOrdersHistory]    Script Date: 16/04/2019 12:21:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO







CREATE VIEW [dbo].[AllOrdersHistory]
AS
SELECT * FROM AttractionsOrdersHistory
UNION
SELECT * FROM MoviesOrdersHistory
UNION
SELECT * FROM SpaOrdersHistory
UNION
SELECT * FROM TzimersOrdersHistory
GO


