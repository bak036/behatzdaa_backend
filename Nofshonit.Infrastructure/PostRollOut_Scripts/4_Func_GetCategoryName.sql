USE [Histadrut]
GO

/****** Object:  UserDefinedFunction [dbo].[GetCategoryName]    Script Date: 16/04/2019 16:22:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER FUNCTION [dbo].[GetCategoryName](
	@CategoryNumber int
) 
RETURNS nvarchar(200)
AS
BEGIN
	
	declare @categoryName nvarchar(200)

	select @categoryName = DisplayName from DTS_Online..OrganizationCategories where OrganizationID = 102 and CategoryNumber = @CategoryNumber

	if @categoryName is null
	BEGIN
		select @categoryName = CategoryName from DTS_Online..PortalCategories where CategoryNumber = @CategoryNumber 
	END

	return @categoryName

END
GO


